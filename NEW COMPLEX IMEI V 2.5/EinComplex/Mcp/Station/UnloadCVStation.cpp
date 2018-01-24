#include "StdAfx.h"
#include "UnloadCVStation.h"

#include "MainControlStation.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CUnloadCVStation::CUnloadCVStation()
{
	m_sName = _T("UnloadCVStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CUnloadCVStation::~CUnloadCVStation()
{
	DeleteThreads();
}

void CUnloadCVStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtEnableUseCV"), m_evtEnableUseCV);
	m_profile.AddEvent(_T("evtCVStopComp"), m_evtCVStopComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
	m_profile.AddInt(_T("nCVStopDelay"), m_nCVStopDelay);
	m_profile.AddInt(_T("nCVRunTime"), m_nCVRunTime);
	m_profile.AddInt(_T("nStandardTACT"), m_nStandardTACT);
}

void CUnloadCVStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CUnloadCVStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CUnloadCVStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CUnloadCVStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CUnloadCVStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CUnloadCVStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	m_bFirstSetUnloadComp = FALSE;
	m_lTACT = 0;

	InitVariable();
}

void CUnloadCVStation::Setup()
{
	Wait(100);
}

void CUnloadCVStation::PostAbort()
{
	InitVariable();
}

void CUnloadCVStation::PostStop(UINT nMode)
{
	_oCVStop();
}

void CUnloadCVStation::PreStart()
{
}

UINT CUnloadCVStation::AutoRun()
{
	while( TRUE ) 
	{
		Wait(10);

		switch( m_nAutoState )
		{
			case AS_INIT:		AsInit();		break;
			case AS_WAIT_SET:	AsWaitSet();	break;
			default:							break;
		}
	}

	return 0;
}

UINT CUnloadCVStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CUnloadCVStation::AsInit()
{
	_oCVStop();

	m_nAutoState = AS_WAIT_SET;
}

void CUnloadCVStation::AsWaitSet()
{
	if( !m_pMainStn->IsDryRunMode() &&
		chkFullSensor() &&
		chkExitSensor() )
	{
		_oCVStop();
		if( chkEnterSensor() ) m_evtEnableUseCV.Reset();
	}
	else
	{
		if( chkExitSensor() )
		{
			if( m_tmRun.IsTimeUp(m_nCVRunTime) ) _oCVStop();
			else _oCVRun();
		}
		else
		{
			_oCVRun();
			m_tmRun.Start();
		}

		m_evtEnableUseCV.Set();
	}

	int nRet = WaitEvents(10, &m_pLeftPPStn->m_evtUnloadCVStopReq, &m_pRightPPStn->m_evtUnloadCVStopReq, NULL);

	if( nRet == WAIT_OBJECT_0 )
	{
		m_pLeftPPStn->m_evtUnloadCVStopReq.Reset();
		_oCVStop();
		Sleep(m_nCVStopDelay);
		m_evtCVStopComp.Set();
		WaitEvent(m_pLeftPPStn->m_evtUnloadCVPlaceComp);
		m_pLeftPPStn->m_evtUnloadCVPlaceComp.Reset();
		m_tmRun.Start();

		if( !m_bFirstSetUnloadComp )
		{
			m_bFirstSetUnloadComp = TRUE;
			m_tmTACT.Start();
		}
		else
		{
			m_lTACT = m_tmTACT.IsTimeNow();
			m_tmTACT.Start();
		}

		return;
	}
	else if( nRet == (WAIT_OBJECT_0 + 1) )
	{
		m_pRightPPStn->m_evtUnloadCVStopReq.Reset();
		_oCVStop();
		Sleep(m_nCVStopDelay);
		m_evtCVStopComp.Set();
		WaitEvent(m_pRightPPStn->m_evtUnloadCVPlaceComp);
		m_pRightPPStn->m_evtUnloadCVPlaceComp.Reset();
		m_tmRun.Start();

		if( !m_bFirstSetUnloadComp )
		{
			m_bFirstSetUnloadComp = TRUE;
			m_tmTACT.Start();
		}
		else
		{
			m_lTACT = m_tmTACT.IsTimeNow();
			m_tmTACT.Start();
		}

		return;
	}
}

void CUnloadCVStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtEnableUseCV.Reset();
	m_evtCVStopComp.Reset();

	m_nAutoState = AS_INIT;
}

void CUnloadCVStation::_oCVRun()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDCV_RUN] = TRUE;
}

void CUnloadCVStation::_oCVStop()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDCV_RUN] = FALSE;
}

BOOL CUnloadCVStation::chkEnterSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN4) ) return TRUE;

	return FALSE;
}

BOOL CUnloadCVStation::chkFullSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN5) ) return TRUE;

	return FALSE;
}

BOOL CUnloadCVStation::chkExitSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN6) ) return TRUE;

	return FALSE;
}
