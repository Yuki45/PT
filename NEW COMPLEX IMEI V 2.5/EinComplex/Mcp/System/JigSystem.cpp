#include "StdAfx.h"
#include "JigSystem.h"

#include "MainControlStation.h"
#include "Resource.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CJigSystem::CJigSystem(int nSysIdx)
{
	if( (nSysIdx > 0) && (nSysIdx <= DEF_MAX_TEST_PC) ) m_nSystemIndex = nSysIdx;
	else ASSERT(FALSE);

	m_sName.Format(_T("JigSystem%d"), m_nSystemIndex);
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\System\\%s.ini"), m_sName);

	m_nCurrTester = 0;
	for( int i = 0; i < DEF_MAX_TESTER_IN_PC; i++ )
	{
		m_bWaitReady[i] = FALSE;
		m_bEnableStart[i] = FALSE;
		m_bEnableUnload[i] = FALSE;
		m_lTestTime[i] = 0;
		m_dAvgTestTime[i] = 0.0;

		m_bJigExist[i] = FALSE;
		m_nJigStatus[i] = JS_NONE;
		m_nJigResult[i] = JR_UNKNOWN;
		m_nJigRetestCnt[i] = 0;
		m_sJigBarcode[i] = _T("NoBCR");
		m_sJigFailName[i] = _T("");
		m_sPrevFailName[i] = _T("");

		m_nAutoState[i] = AS_INIT;
	}
}

CJigSystem::~CJigSystem()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);

	DeleteAxThread(m_hPriThread);
}

void CJigSystem::InitProfile()
{
	CAxSystem::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
	CString sKey = _T("");
	for( int i = 0; i < DEF_MAX_TESTER_IN_PC; i++ )
	{
		sKey.Format(_T("bJigRateBlocked_%02d"), i + 1);
		m_profile.AddBool(sKey, m_bJigRateBlocked[i]);

		sKey.Format(_T("nJigUse_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigUse[i]);

		sKey.Format(_T("nJigPassCnt_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigPassCnt[i]);

		sKey.Format(_T("nJigFailCnt_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigFailCnt[i]);

		sKey.Format(_T("nJigRetestPassCnt_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigRetestPassCnt[i]);

		sKey.Format(_T("nJigRetestFailCnt_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigRetestFailCnt[i]);

		sKey.Format(_T("nJigPackOutCnt_%02d"), i + 1);
		m_profile.AddInt(sKey, m_nJigPackOutCnt[i]);
	}
}

void CJigSystem::Startup()
{
	CAxSystem::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();
}

UINT CJigSystem::AutoRun()
{
	while( TRUE )
	{
		WaitNS(10);

		switch( m_nAutoState[m_nCurrTester] )
		{
			case AS_INIT:			AsInit();			break;
			case AS_CHECK_READY:	AsCheckReady();		break;
			case AS_WAIT_START:		AsWaitStart();		break;
			case AS_WAIT_ACK:		AsWaitAck();		break;
			case AS_WAIT_RESULT:	AsWaitResult();		break;
			case AS_WAIT_FAIL_NAME:	AsWaitFailName();	break;
			default:
				if( m_bTerminate ) throw -1;
				break;
		}

		if( (m_nCurrTester < 0) || (m_nCurrTester >= (DEF_MAX_TESTER_IN_PC - 1)) ) m_nCurrTester = 0;
		else m_nCurrTester++;
	}

	return 0;
}

void CJigSystem::AsInit()
{
	if( m_pMainFrm->m_bSerialHubCreated != TRUE ) return;

	CString sTemp = _T("");
	sTemp.Format(_T("Tester_%02d"), m_nSystemIndex);

	m_pTester = (CCommTester*)m_pMainFrm->m_pSerialHub->GetSerial(sTemp);

	m_bWaitReady[m_nCurrTester] = FALSE;
	m_bEnableStart[m_nCurrTester] = FALSE;
	m_bEnableUnload[m_nCurrTester] = FALSE;
	m_lTestTime[m_nCurrTester] = 0;

	m_bJigExist[m_nCurrTester] = FALSE;
	m_nJigStatus[m_nCurrTester] = JS_NONE;
	m_nJigResult[m_nCurrTester] = JR_UNKNOWN;
	m_nJigSameFailCnt[m_nCurrTester] = 0;
	m_sJigBarcode[m_nCurrTester] = _T("NoBCR");
	m_sJigFailName[m_nCurrTester] = _T("");
	m_sPrevFailName[m_nCurrTester] = _T("");

	_oPackOut(m_nCurrTester);

	m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
}

void CJigSystem::AsCheckReady()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	if( !pMainStn->IsNormalMode() )
	{
		m_nJigStatus[m_nCurrTester] = JS_READY;
		m_nAutoState[m_nCurrTester] = AS_WAIT_START;
		return;
	}

	if( !m_bWaitReady[m_nCurrTester] )
	{
		m_pTester->m_bRecvReady[m_nCurrTester] = FALSE;
		m_bWaitReady[m_nCurrTester] = TRUE;
		m_tmReady[m_nCurrTester].Start();
	}
	else
	{
		if( m_pTester->m_bRecvReady[m_nCurrTester] )
		{
			m_bWaitReady[m_nCurrTester] = FALSE;
			m_nJigStatus[m_nCurrTester] = JS_READY;
		}
		else
		{
			if( m_tmReady[m_nCurrTester].IsTimeUp(pMainStn->m_nJigReadyTimeout) )
			{
				m_bWaitReady[m_nCurrTester] = FALSE;
				m_nJigStatus[m_nCurrTester] = JS_NONE;
			}
		}
	}

	if( m_nJigStatus[m_nCurrTester] == JS_READY )
	{
		m_nAutoState[m_nCurrTester] = AS_WAIT_START;
	}
}

void CJigSystem::AsWaitStart()
{
	if( m_bEnableStart[m_nCurrTester] )
	{
		_oPackIn(m_nCurrTester);
		m_bEnableStart[m_nCurrTester] = FALSE;
		m_pTester->m_bRecvAck[m_nCurrTester] = FALSE;
		m_pTester->SendPacket(CCommTester::CC_START, m_nCurrTester + 1, m_sJigBarcode[m_nCurrTester]);
		m_tmAck[m_nCurrTester].Start();
		m_nAutoState[m_nCurrTester] = AS_WAIT_ACK;
	}
	else m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
}

void CJigSystem::AsWaitAck()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	if( !pMainStn->IsNormalMode() || m_pTester->m_bRecvAck[m_nCurrTester] )
	{
		m_pTester->m_bRecvPass[m_nCurrTester] = FALSE;
		m_pTester->m_bRecvFail[m_nCurrTester] = FALSE;
		m_nJigStatus[m_nCurrTester] = JS_WRITING;
		m_tmResult[m_nCurrTester].Start();
		m_nAutoState[m_nCurrTester] = AS_WAIT_RESULT;
		return;
	}

	if( m_tmAck[m_nCurrTester].IsTimeUp(pMainStn->m_nAckWaitTime) )
	{
		m_pTester->m_bRecvPass[m_nCurrTester] = FALSE;
		m_pTester->m_bRecvFail[m_nCurrTester] = FALSE;
		m_pTester->SendPacket(CCommTester::CC_START, m_nCurrTester + 1, m_sJigBarcode[m_nCurrTester]);
		m_nJigStatus[m_nCurrTester] = JS_WRITING;
		m_tmResult[m_nCurrTester].Start();
		m_nAutoState[m_nCurrTester] = AS_WAIT_RESULT;
		return;
	}
}

void CJigSystem::AsWaitResult()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	if( !pMainStn->IsNormalMode() )
	{
		if( m_tmResult[m_nCurrTester].IsTimeUp(pMainStn->m_nJigSimTestTimeout) )
		{
			m_nJigStatus[m_nCurrTester] = JS_WRITING_DONE;
			m_nJigResult[m_nCurrTester] = JR_PASS;
			m_nJigPackOutCnt[m_nCurrTester] = 0;
			m_lTestTime[m_nCurrTester] = m_tmResult[m_nCurrTester].IsTimeNow();
			m_bEnableUnload[m_nCurrTester] = TRUE;
			_oPackOut(m_nCurrTester);

			if( m_nJigRetestCnt[m_nCurrTester] > 0 )
			{
				m_nJigRetestPassCnt[m_nCurrTester]++;

				m_nJigPassCnt[m_nCurrTester]++;
				if( m_nJigFailCnt[m_nCurrTester] > 0 ) m_nJigFailCnt[m_nCurrTester]--;
			}
			else m_nJigPassCnt[m_nCurrTester]++;

			CalculateAvgTestTime();
			SaveProfile();
			m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
		}
		return;
	}

	if( m_pTester->m_bRecvPass[m_nCurrTester] )
	{
		m_nJigStatus[m_nCurrTester] = JS_WRITING_DONE;
		m_nJigResult[m_nCurrTester] = JR_PASS;
		m_nJigPackOutCnt[m_nCurrTester] = 0;
		m_lTestTime[m_nCurrTester] = m_tmResult[m_nCurrTester].IsTimeNow();
		m_bEnableUnload[m_nCurrTester] = TRUE;
		_oPackOut(m_nCurrTester);

		if( m_nJigRetestCnt[m_nCurrTester] > 0 )
		{
			m_nJigRetestPassCnt[m_nCurrTester]++;

			m_nJigPassCnt[m_nCurrTester]++;
			if( m_nJigFailCnt[m_nCurrTester] > 0 ) m_nJigFailCnt[m_nCurrTester]--;
		}
		else m_nJigPassCnt[m_nCurrTester]++;

		CalculateAvgTestTime();
		SaveProfile();
		m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
	}
	else if( m_pTester->m_bRecvFail[m_nCurrTester] )
	{
		m_nJigResult[m_nCurrTester] = JR_WRITE_FAIL;
		m_pTester->m_bRecvLabel[m_nCurrTester] = FALSE;
		m_tmLabel[m_nCurrTester].Start();
		m_nAutoState[m_nCurrTester] = AS_WAIT_FAIL_NAME;
	}
	else
	{
		if( m_tmResult[m_nCurrTester].IsTimeUp(pMainStn->m_nJigTestTimeout) )
		{
			m_nJigStatus[m_nCurrTester] = JS_WRITING_DONE;
			m_nJigResult[m_nCurrTester] = JR_WRITE_TIME_OUT;
			m_sJigFailName[m_nCurrTester] = _T("Write Time Out");
			m_lTestTime[m_nCurrTester] = m_tmResult[m_nCurrTester].IsTimeNow();
			m_bEnableUnload[m_nCurrTester] = TRUE;
			_oPackOut(m_nCurrTester);

			if( m_nJigRetestCnt[m_nCurrTester] > 0 ) m_nJigRetestFailCnt[m_nCurrTester]++;
			else m_nJigFailCnt[m_nCurrTester]++;

			int nPassCnt = m_nJigPassCnt[m_nCurrTester] + m_nJigRetestPassCnt[m_nCurrTester];
			int nFailCnt = m_nJigFailCnt[m_nCurrTester] + m_nJigRetestFailCnt[m_nCurrTester];
			double dFailRate = (nFailCnt > 0) ? ((double)nFailCnt / (double)(nPassCnt + nFailCnt) * 100.0) : 0.0;

			if( (m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt > 0) &&
				((nPassCnt + nFailCnt) >= m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt) &&
				(dFailRate >= m_pMainFrm->m_pMainStn->m_dRateBlockPercent) )
			{
				m_nJigUse[m_nCurrTester] = JU_NOT_USE;
				m_bJigRateBlocked[m_nCurrTester] = TRUE;
			}

			CalculateAvgTestTime();
			SaveProfile();
			m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
		}
	}
}

void CJigSystem::AsWaitFailName()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	if( m_pTester->m_bRecvLabel[m_nCurrTester] )
	{
		m_nJigStatus[m_nCurrTester] = JS_WRITING_DONE;
		m_sJigFailName[m_nCurrTester] = m_pTester->m_sFailName[m_nCurrTester];

		if( (m_sJigFailName[m_nCurrTester].Find(_T("PACK")) == 0) ||
			(m_sJigFailName[m_nCurrTester].Find(_T("Booting")) == 0) )
		{
			m_nJigPackOutCnt[m_nCurrTester]++;

			if( (pMainStn->m_nPackOutBlockCnt > 0) &&
				(m_nJigPackOutCnt[m_nCurrTester] >= pMainStn->m_nPackOutBlockCnt) )
			{
				m_nJigUse[m_nCurrTester] = JU_NOT_USE;
			}

			SaveProfile();
		}
		else m_nJigPackOutCnt[m_nCurrTester] = 0;

		if( (m_sPrevFailName[m_nCurrTester] != _T("")) &&
			(m_sPrevFailName[m_nCurrTester] == m_sJigFailName[m_nCurrTester]) )
		{
			if( (m_sJigFailName[m_nCurrTester].Find(_T("PACK")) != 0) &&
				(m_sJigFailName[m_nCurrTester].Find(_T("Booting")) != 0) )
			{
				m_nJigSameFailCnt[m_nCurrTester]++;
			}

			if( (pMainStn->m_nSameFailBlockCnt > 0) &&
				(m_nJigSameFailCnt[m_nCurrTester] >= pMainStn->m_nSameFailBlockCnt) )
			{
				m_nJigUse[m_nCurrTester] = JU_NOT_USE;
				SaveProfile();
			}
		}
		else
		{
			m_nJigSameFailCnt[m_nCurrTester] = 0;
			m_sPrevFailName[m_nCurrTester] = m_sJigFailName[m_nCurrTester];
		}

		m_lTestTime[m_nCurrTester] = m_tmResult[m_nCurrTester].IsTimeNow();
		m_bEnableUnload[m_nCurrTester] = TRUE;
		_oPackOut(m_nCurrTester);

		if( m_nJigRetestCnt[m_nCurrTester] > 0 ) m_nJigRetestFailCnt[m_nCurrTester]++;
		else m_nJigFailCnt[m_nCurrTester]++;

		int nPassCnt = m_nJigPassCnt[m_nCurrTester] + m_nJigRetestPassCnt[m_nCurrTester];
		int nFailCnt = m_nJigFailCnt[m_nCurrTester] + m_nJigRetestFailCnt[m_nCurrTester];
		double dFailRate = (nFailCnt > 0) ? ((double)nFailCnt / (double)(nPassCnt + nFailCnt) * 100.0) : 0.0;

		if( (m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt > 0) &&
			((nPassCnt + nFailCnt) >= m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt) &&
			(dFailRate >= m_pMainFrm->m_pMainStn->m_dRateBlockPercent) )
		{
			m_nJigUse[m_nCurrTester] = JU_NOT_USE;
			m_bJigRateBlocked[m_nCurrTester] = TRUE;
		}

		CalculateAvgTestTime();
		SaveProfile();
		m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
	}
	else
	{
		if( m_tmLabel[m_nCurrTester].IsTimeUp(pMainStn->m_nJigFailnameTimeout) )
		{
			m_nJigStatus[m_nCurrTester] = JS_WRITING_DONE;
			m_sJigFailName[m_nCurrTester] = _T("Unknown Fail Name");
			m_lTestTime[m_nCurrTester] = m_tmResult[m_nCurrTester].IsTimeNow();
			m_bEnableUnload[m_nCurrTester] = TRUE;
			_oPackOut(m_nCurrTester);

			if( m_nJigRetestCnt[m_nCurrTester] > 0 ) m_nJigRetestFailCnt[m_nCurrTester]++;
			else m_nJigFailCnt[m_nCurrTester]++;

			int nPassCnt = m_nJigPassCnt[m_nCurrTester] + m_nJigRetestPassCnt[m_nCurrTester];
			int nFailCnt = m_nJigFailCnt[m_nCurrTester] + m_nJigRetestFailCnt[m_nCurrTester];
			double dFailRate = (nFailCnt > 0) ? ((double)nFailCnt / (double)(nPassCnt + nFailCnt) * 100.0) : 0.0;

			if( (m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt > 0) &&
				((nPassCnt + nFailCnt) >= m_pMainFrm->m_pMainStn->m_nRateBlockLeastCnt) &&
				(dFailRate >= m_pMainFrm->m_pMainStn->m_dRateBlockPercent) )
			{
				m_nJigUse[m_nCurrTester] = JU_NOT_USE;
				m_bJigRateBlocked[m_nCurrTester] = TRUE;
			}

			CalculateAvgTestTime();
			SaveProfile();
			m_nAutoState[m_nCurrTester] = AS_CHECK_READY;
		}
	}
}

void CJigSystem::_oPackIn(int nIndex)
{
#ifdef DEF_EIN_48_LCA
	if( m_nSystemIndex == 1 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 2 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT7, 0);
	}
	else if( m_nSystemIndex == 3 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT8, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT5, 0);
	}
	else if( m_nSystemIndex == 4 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 5 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT7, 0);
	}
	else if( m_nSystemIndex == 6 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT8, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT5, 0);
	}
#else
	if( m_nSystemIndex == 1 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 2 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 3 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT8, 0);
		else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, SERVO_OUT_BITMASK_USEROUT0, 0);
	}
	else if( m_nSystemIndex == 4 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 5 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT8, 0);
	}
	else if( m_nSystemIndex == 6 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT0, 0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT1, 0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT2, 0);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT3, 0);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT4, 0);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT5, 0);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT6, 0);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT7, 0);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT8, 0);
		else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_PP, SERVO_OUT_BITMASK_USEROUT0, 0);
	}
#endif
}

void CJigSystem::_oPackOut(int nIndex)
{
#ifdef DEF_EIN_48_LCA
	if( m_nSystemIndex == 1 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 2 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT7);
	}
	else if( m_nSystemIndex == 3 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT8);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT5);
	}
	else if( m_nSystemIndex == 4 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 5 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT7);
	}
	else if( m_nSystemIndex == 6 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT8);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT5);
	}
#else
	if( m_nSystemIndex == 1 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 2 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 3 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT8);
		else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, 0, SERVO_OUT_BITMASK_USEROUT0);
	}
	else if( m_nSystemIndex == 4 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 5 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT8);
	}
	else if( m_nSystemIndex == 6 )
	{
		if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT0);
		else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT1);
		else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT2);
		else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT3);
		else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT4);
		else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT5);
		else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT6);
		else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT7);
		else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT8);
		else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_PP, 0, SERVO_OUT_BITMASK_USEROUT0);
	}
#endif
}

void CJigSystem::CalculateAvgTestTime()
{
	unsigned long lTotalNormalSet = 0;
	unsigned long lTotalRetestSet = 0;
	unsigned long lTotalSet = 0;
	double dTotalTestTime = 0.0;

	lTotalNormalSet = m_nJigPassCnt[m_nCurrTester] + m_nJigFailCnt[m_nCurrTester];
	lTotalRetestSet = m_nJigRetestPassCnt[m_nCurrTester] + m_nJigRetestFailCnt[m_nCurrTester];
	lTotalSet = lTotalNormalSet + lTotalRetestSet;

	if( lTotalSet > 0 )
	{
		dTotalTestTime = m_dAvgTestTime[m_nCurrTester] * (double)(lTotalSet - 1);
		dTotalTestTime += (double)m_lTestTime[m_nCurrTester] / 1000.0;
		m_dAvgTestTime[m_nCurrTester] = dTotalTestTime / (double)lTotalSet;
	}
}
