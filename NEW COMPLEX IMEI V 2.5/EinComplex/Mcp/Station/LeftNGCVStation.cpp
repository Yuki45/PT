#include "StdAfx.h"
#include "LeftNGCVStation.h"

#include "MainControlStation.h"
#include "OPSystem.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CLeftNGCVStation::CLeftNGCVStation()
{
	m_sName = _T("LeftNGCVStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CLeftNGCVStation::~CLeftNGCVStation()
{
	DeleteThreads();
}

void CLeftNGCVStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtEnableUse"), m_evtEnableUse);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CLeftNGCVStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CLeftNGCVStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CLeftNGCVStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CLeftNGCVStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CLeftNGCVStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CLeftNGCVStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	InitVariable();
}

void CLeftNGCVStation::Setup()
{
	Wait(100);
}

void CLeftNGCVStation::PostAbort()
{
	InitVariable();
}

void CLeftNGCVStation::PostStop(UINT nMode)
{
	_oCVStop();
}

void CLeftNGCVStation::PreStart()
{
}

UINT CLeftNGCVStation::AutoRun()
{
	while( TRUE ) 
	{
		WaitLeft(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			case AS_FULL:	AsFull();	break;
			default:					break;
		}
	}

	return 0;
}

UINT CLeftNGCVStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CLeftNGCVStation::AsInit()
{
	m_evtEnableUse.Set();

	m_nAutoState = AS_RUN;
}

void CLeftNGCVStation::AsRun()
{
	if( !chkEnterSensor() && !chkExitSensor() ) _oCVStop();

	if( !chkEnterSensor() && chkExitSensor() ) _oCVStop();

	if( chkEnterSensor() && !chkExitSensor() )
	{
		m_evtEnableUse.Reset();
		m_tmRun.Start();
		while( !chkExitSensor() )
		{
			_oCVRun();
			if( m_tmRun.IsTimeUp(m_pMainStn->m_nNGCVRunTime) ) break;
		}
		_oCVStop();
		m_evtEnableUse.Set();
	}


	if( chkEnterSensor() && chkExitSensor() )
	{
		m_evtEnableUse.Reset();
		m_tmFull.Start();
		m_nBuzzerCnt = 0;
		m_nAutoState = AS_FULL;
	}
}

void CLeftNGCVStation::AsFull()
{
	CCommWorld* pWorldRight = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
	COPSystem* pOPSys = (COPSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("OPSystem"));

	if( !chkEnterSensor() || !chkExitSensor() )
	{
		pWorldRight->m_bOutput[DEF_IO_BUZZER] = FALSE;
		pOPSys->m_bForceBuzzer = FALSE;

		m_evtEnableUse.Set();
		m_nAutoState = AS_RUN;
		return;
	}

	if( m_tmFull.IsTimeUp(m_pMainStn->m_nNGCVFullStopDelay) ) _oCVStop();

	if( m_tmFull.IsTimeUp(m_pMainStn->m_nNGCVFullAlarmTime) && (m_nBuzzerCnt < 10) )
	{
		if( m_nBuzzerCnt == 0 )
		{
			pOPSys->m_bForceBuzzer = TRUE;
			pWorldRight->m_bOutput[DEF_IO_BUZZER] = TRUE;
		}
		else if( m_nBuzzerCnt == 9 )
		{
			pWorldRight->m_bOutput[DEF_IO_BUZZER] = FALSE;
			pOPSys->m_bForceBuzzer = FALSE;
		}
		else
		{
			pWorldRight->m_bOutput[DEF_IO_BUZZER] = !pWorldRight->m_bOutput[DEF_IO_BUZZER];
		}

		Sleep(1000);
		m_nBuzzerCnt++;
	}
}

void CLeftNGCVStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtEnableUse.Reset();

	m_nBuzzerCnt = 0;

	m_nAutoState = AS_INIT;
}

void CLeftNGCVStation::WaitLeft(DWORD dwTime)
{
	CAxTimer timer;
	timer.Start();

	while( TRUE )
	{
		if( (m_pMainStn->m_nStateLeft == MS_AUTO) &&
			(timer.IsTimeUp((LONG)dwTime)) ) break;

		if( m_pMainStn->m_nStateRight == MS_AUTO ) Sleep(1);
		else Wait(1);
	}
}

int CLeftNGCVStation::ErrorLeft(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
								LPCTSTR pszParam1, LPCTSTR pszParam2, LPCTSTR pszParam3, LPCTSTR pszParam4)
{
	if( m_pMainStn->m_nStateLeft == MS_ERROR )
	{
		WaitLeft(10);
		return emRetry;
	}

	m_pMainStn->m_nStateLeft = MS_ERROR;

	m_errCtrl.m_pErrMsg->m_nNumber = nNumber;
	m_errCtrl.m_pErrMsg->m_nType = nType;
	m_errCtrl.m_pErrMsg->m_sSource = pszSource;
	m_errCtrl.m_pErrMsg->m_sHelpFile = pszPath;
	m_errCtrl.m_pErrMsg->m_sParams[0] = pszParam1;
	m_errCtrl.m_pErrMsg->m_sParams[1] = pszParam2;
	m_errCtrl.m_pErrMsg->m_sParams[2] = pszParam3;
	m_errCtrl.m_pErrMsg->m_sParams[3] = pszParam4;
	CAxErrorMgr::GetErrorMgr()->RaiseError((CAxErrData)(*m_errCtrl.m_pErrMsg));

	m_pMainStn->m_nResponseLeft = emNone;
	SendMessage(m_pMainFrm->GetSafeHwnd(), UM_MCP_ERROR, 0, 0);

	while( TRUE )
	{
		if( m_pMainStn->m_nResponseLeft != emNone ) break;

		if( m_pMainStn->m_nStateLeft != MS_ERROR )
		{
			WaitLeft(10);
			return emRetry;
		}

		if( m_pMainStn->m_nStateRight == MS_AUTO ) Sleep(1);
		else Wait(1);
	}

	int nRet = m_pMainStn->m_nResponseLeft;
	m_pMainStn->m_nResponseLeft = emNone;
	m_pMainStn->m_nStateLeft = MS_AUTO;

	return nRet;
}

void CLeftNGCVStation::_oCVRun()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_NGCV_RUN] = TRUE;
}

void CLeftNGCVStation::_oCVStop()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_NGCV_RUN] = FALSE;
}

BOOL CLeftNGCVStation::chkEnterSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN0) ) return TRUE;

	return FALSE;
}

BOOL CLeftNGCVStation::chkExitSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN1) ) return TRUE;

	return FALSE;
}