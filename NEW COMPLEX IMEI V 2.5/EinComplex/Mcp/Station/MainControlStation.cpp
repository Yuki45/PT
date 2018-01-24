#include "StdAfx.h"
#include "MainControlStation.h"

#include "OPSystem.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CMainControlStation::CMainControlStation()
{
	m_sName = _T("MainControlStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);
	m_trcError.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("Error"));

	m_nAutoState = AS_INIT;

	m_nStateLeft = MS_IDLE;
	m_nStateRight = MS_IDLE;
	m_nResponseLeft = emNone;
	m_nResponseRight = emNone;
}

CMainControlStation::~CMainControlStation()
{
	DeleteThreads();
}

void CMainControlStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
	m_profile.AddBool(_T("bZUpAfterPackIn"), m_bZUpAfterPackIn);
	m_profile.AddBool(_T("bInsertCVSyncMode"), m_bInsertCVSyncMode);
	m_profile.AddBool(_T("bUseBCR_OnPassRun"), m_bUseBCR_OnPassRun);

	m_profile.AddInt(_T("nMotionTimeout"), m_nMotionTimeout);
	m_profile.AddInt(_T("nCylinderTimeout"), m_nCylinderTimeout);
	m_profile.AddInt(_T("nMaxRetestCnt"), m_nMaxRetestCnt);
	m_profile.AddInt(_T("nPackOutBlockCnt"), m_nPackOutBlockCnt);
	m_profile.AddInt(_T("nSameFailBlockCnt"), m_nSameFailBlockCnt);
	m_profile.AddInt(_T("nNGCVRunTime"), m_nNGCVRunTime);
	m_profile.AddInt(_T("nNGCVFullAlarmTime"), m_nNGCVFullAlarmTime);
	m_profile.AddInt(_T("nNGCVFullStopDelay"), m_nNGCVFullStopDelay);
	m_profile.AddInt(_T("nBCRTimeout"), m_nBCRTimeout);
	m_profile.AddInt(_T("nPackInsertDelay"), m_nPackInsertDelay);
	m_profile.AddInt(_T("nMaxMachineRestTime"), m_nMaxMachineRestTime);
	m_profile.AddInt(_T("nJigReadyTimeout"), m_nJigReadyTimeout);
	m_profile.AddInt(_T("nJigSimTestTimeout"), m_nJigSimTestTimeout);
	m_profile.AddInt(_T("nJigTestTimeout"), m_nJigTestTimeout);
	m_profile.AddInt(_T("nJigFailnameTimeout"), m_nJigFailnameTimeout);
	m_profile.AddInt(_T("nBarcodeLength"), m_nBarcodeLength);
	m_profile.AddInt(_T("nRateBlockLeastCnt"), m_nRateBlockLeastCnt);
	m_profile.AddInt(_T("nAckWaitTime"), m_nAckWaitTime);

	m_profile.AddDouble(_T("dInposition"), m_dInposition);
	m_profile.AddDouble(_T("dSlowDownDist"), m_dSlowDownDist);
	m_profile.AddDouble(_T("dInterlockPosZ"), m_dInterlockPosZ);
	m_profile.AddDouble(_T("dBCRRetryDist"), m_dBCRRetryDist);
	m_profile.AddDouble(_T("dRateBlockPercent"), m_dRateBlockPercent);

	CString sTemp = _T("");
	for( int i = 0; i < DEF_MAX_RETEST_NAME; i++ )
	{
		sTemp.Format(_T("sRetestName_%02d"), i);
		m_profile.AddStr(sTemp, m_sRetestName[i]);
	}

	int a, b, c;
	for( a = 0; a < WT_MAX_SHIFT; a++ )
	{
		for( b = 0; b < WT_MAX_LIST; b++ )
		{
			for( c = 0; c < WT_MAX_TYPE; c++ )
			{
				sTemp.Format(_T("nWorkTime_%d%d%d"), a, b, c);
				m_profile.AddInt(sTemp, m_nWorkTime[a][b][c]);
			}
		}
	}
}

void CMainControlStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CMainControlStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CMainControlStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CMainControlStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CMainControlStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CMainControlStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	InitVariable();
}

void CMainControlStation::Setup()
{
	Wait(100);
}

void CMainControlStation::PostAbort()
{
	InitVariable();
}

void CMainControlStation::PostStop(UINT nMode)
{
}

void CMainControlStation::PreStart()
{
}

UINT CMainControlStation::AutoRun()
{
	while( TRUE ) 
	{
		Wait(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			default:					break;
		}
	}

	return 0;
}

UINT CMainControlStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CMainControlStation::AsInit()
{
	m_nAutoState = AS_RUN;
}

void CMainControlStation::AsRun()
{
	if( chkEStopSwitch() )
	{
		COPSystem* pOPSys = (COPSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("OPSystem"));
		pOPSys->m_bEStopError = TRUE;
		m_nStateLeft = MS_ERROR;
		m_nStateRight = MS_ERROR;

		AllAxisStop();
		Error(ERR_ESTOP, emRetry, m_sName, m_sErrPath);
		return;
	}
}

void CMainControlStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();

	m_nAutoState = AS_INIT; 
}

void CMainControlStation::SetNormalMode()
{
	SetBypass(FALSE);
	SetDryRun(FALSE);
	SetSimulate(FALSE);

	SaveProfile();
}

void CMainControlStation::SetBypassMode()
{
	SetBypass(TRUE);
	SetDryRun(FALSE);
	SetSimulate(FALSE);

	SaveProfile();
}

void CMainControlStation::SetDryRunMode()
{
	SetBypass(FALSE);
	SetDryRun(TRUE);
	SetSimulate(FALSE);

	SaveProfile();
}

void CMainControlStation::SetSimulateMode()
{
	SetBypass(FALSE);
	SetDryRun(FALSE);
	SetSimulate(TRUE);

	SaveProfile();
}

BOOL CMainControlStation::IsNormalMode()
{
	return (!GetBypass() && !GetDryRun() && !GetSimulate());
}

BOOL CMainControlStation::IsBypassMode()
{
	return (GetBypass() && !GetDryRun() && !GetSimulate());
}

BOOL CMainControlStation::IsDryRunMode()
{
	return (GetDryRun() && !GetSimulate());
}

BOOL CMainControlStation::IsSimulateMode()
{
	return GetSimulate();
}

BOOL CMainControlStation::chkEStopSwitch()
{
	if( IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return TRUE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN8) ) return TRUE;

	return FALSE;
}

void CMainControlStation::AllAxisStop()
{
	FAS_AllMoveStop(DEF_FAS_LEFT);
	FAS_AllMoveStop(DEF_FAS_RIGHT);
}

void CMainControlStation::AllAxisEStop()
{
	FAS_AllEmergencyStop(DEF_FAS_LEFT);
	FAS_AllEmergencyStop(DEF_FAS_RIGHT);
}
