#include "StdAfx.h"
#include "LoadCVStation.h"

#include "MainControlStation.h"
#include "LoadCV2Station.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CLoadCVStation::CLoadCVStation()
{
	m_sName = _T("LoadCVStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CLoadCVStation::~CLoadCVStation()
{
	DeleteThreads();
}

void CLoadCVStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtLoadReq"), m_evtLoadReq);
	m_profile.AddEvent(_T("evtLoadComp"), m_evtLoadComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CLoadCVStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CLoadCVStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CLoadCVStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CLoadCVStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CLoadCVStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CLoadCVStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLoadCV2Stn = (CLoadCV2Station*)CAxStationHub::GetStationHub()->GetStation(_T("LoadCV2Station"));
	m_pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	InitVariable();
}

void CLoadCVStation::Setup()
{
	Wait(100);
}

void CLoadCVStation::PostAbort()
{
	InitVariable();
}

void CLoadCVStation::PostStop(UINT nMode)
{
	_oCVStop();
}

void CLoadCVStation::PreStart()
{
}

UINT CLoadCVStation::AutoRun()
{
	while( TRUE ) 
	{
		Wait(10);

		switch( m_nAutoState )
		{
			case AS_INIT:				AsInit();			break;
			case AS_CHECK_SET:			AsCheckSet();		break;
			case AS_WAIT_EVENT:			AsWaitEvent();		break;
			case AS_WAIT_SET:			AsWaitSet();		break;
			case AS_WAIT_LOAD_DELAY:	AsWaitLoadDelay();	break;
			case AS_WAIT_PICK_UP:		AsWaitPickUp();		break;
			default:										break;
		}
	}

	return 0;
}

UINT CLoadCVStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CLoadCVStation::AsInit()
{
	_oCVStop();
	m_tmCVRun.Start();

	m_nAutoState = AS_CHECK_SET;
}

void CLoadCVStation::AsCheckSet()
{
	_oCVRun();

	if( chkExitSensor() )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_LOAD_DELAY;
		return;
	}

	if( m_tmCVRun.IsTimeUp(3000) )
	{
		_oCVStop();
		m_evtLoadReq.Set();
		m_nAutoState = AS_WAIT_EVENT;
		return;
	}
}

void CLoadCVStation::AsWaitEvent()
{
	WaitEvent(m_pLoadCV2Stn->m_evtLoadComp);
	m_pLoadCV2Stn->m_evtLoadComp.Reset();

	m_tmCVRun.Start();
	m_nAutoState = AS_WAIT_SET;
}

void CLoadCVStation::AsWaitSet()
{
	_oCVRun();

	if( m_pMainStn->IsDryRunMode() && m_tmCVRun.IsTimeUp(3000) )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_LOAD_DELAY;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && chkExitSensor() )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_LOAD_DELAY;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && m_tmCVRun.IsTimeUp(10000) )
	{
		_oCVStop();
		m_evtLoadReq.Set();
		m_nAutoState = AS_WAIT_EVENT;
		return;
	}
}

void CLoadCVStation::AsWaitLoadDelay()
{
	_oCVRun();

	if( !m_tmCVRun.IsTimeUp(500) ) return;

	_oCVStop();
	m_evtLoadComp.Set();

	m_nAutoState = AS_WAIT_PICK_UP;
}

void CLoadCVStation::AsWaitPickUp()
{
	int nRet = WaitEvents(10, &m_pLeftPPStn->m_evtLoadCVPickComp, &m_pRightPPStn->m_evtLoadCVPickComp, NULL);

	if( nRet == WAIT_OBJECT_0 ) m_pLeftPPStn->m_evtLoadCVPickComp.Reset();
	else if( nRet == (WAIT_OBJECT_0 + 1) ) m_pRightPPStn->m_evtLoadCVPickComp.Reset();
	else if( nRet == WAIT_TIMEOUT ) return;

	m_evtLoadReq.Set();
	m_nAutoState = AS_WAIT_EVENT;
}

void CLoadCVStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtLoadReq.Reset();
	m_evtLoadComp.Reset();

	m_nAutoState = AS_INIT;
}

void CLoadCVStation::_oCVRun()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDCV_RUN] = TRUE;
}

void CLoadCVStation::_oCVStop()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDCV_RUN] = FALSE;
}

BOOL CLoadCVStation::chkExitSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}
