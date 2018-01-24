#include "StdAfx.h"
#include "LoadCV2Station.h"

#include "MainControlStation.h"
#include "LoadCVStation.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CLoadCV2Station::CLoadCV2Station()
{
	m_sName = _T("LoadCV2Station");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CLoadCV2Station::~CLoadCV2Station()
{
	DeleteThreads();
}

void CLoadCV2Station::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtLoadComp"), m_evtLoadComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CLoadCV2Station::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CLoadCV2Station::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CLoadCV2Station::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CLoadCV2Station::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CLoadCV2Station::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CLoadCV2Station::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLoadCVStn = (CLoadCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("LoadCVStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	InitVariable();
}

void CLoadCV2Station::Setup()
{
	Wait(100);
}

void CLoadCV2Station::PostAbort()
{
	InitVariable();
}

void CLoadCV2Station::PostStop(UINT nMode)
{
	_oCVStop();
}

void CLoadCV2Station::PreStart()
{
}

UINT CLoadCV2Station::AutoRun()
{
	while( TRUE ) 
	{
		Wait(10);

		switch( m_nAutoState )
		{
		case AS_INIT:				AsInit();			break;
		case AS_WAIT_SET_INPUT:		AsWaitSetInput();	break;
		case AS_WAIT_SET_READY:		AsWaitSetReady();	break;
		case AS_WAIT_LOAD_CV:		AsWaitLoadCV();		break;
		case AS_WAIT_MIDDLE_ON:		AsWaitMiddleOn();	break;
		case AS_WAIT_MIDDLE_OFF:	AsWaitMiddleOff();	break;
		default:										break;
		}
	}

	return 0;
}

UINT CLoadCV2Station::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
	case CMD_NONE:	break;
	default:		break;
	}

	return 0;
}

void CLoadCV2Station::AsInit()
{
	_oCVStop();

	while( !ioStopperUp() ) Error(ERR_STOPPER_UP, emRetry, m_sName, m_sErrPath);
	Wait(10);

	m_tmCVRun.Start();

	m_nAutoState = AS_WAIT_SET_INPUT;
}

void CLoadCV2Station::AsWaitSetInput()
{
	_oCVRun();

	if( m_pMainStn->IsDryRunMode() && m_tmCVRun.IsTimeUp(1000) )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_SET_READY;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && chkEnterSensor() )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_SET_READY;
		return;
	}
}

void CLoadCV2Station::AsWaitSetReady()
{
	_oCVRun();

	if( m_tmCVRun.IsTimeUp(500) )
	{
		_oCVStop();
		m_nAutoState = AS_WAIT_LOAD_CV;
		return;
	}
}

void CLoadCV2Station::AsWaitLoadCV()
{
	WaitEvent(m_pLoadCVStn->m_evtLoadReq);
	m_pLoadCVStn->m_evtLoadReq.Reset();

	while( !ioStopperDown() ) Error(ERR_STOPPER_DOWN, emRetry, m_sName, m_sErrPath);
	Wait(10);

	m_evtLoadComp.Set();
	m_tmCVRun.Start();

	m_nAutoState = AS_WAIT_MIDDLE_ON;
}

void CLoadCV2Station::AsWaitMiddleOn()
{
	_oCVRun();

	if( m_pMainStn->IsDryRunMode() && m_tmCVRun.IsTimeUp(500) )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_MIDDLE_OFF;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && chkExitSensor() )
	{
		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_MIDDLE_OFF;
		return;
	}
}

void CLoadCV2Station::AsWaitMiddleOff()
{
	_oCVRun();

	if( m_pMainStn->IsDryRunMode() && m_tmCVRun.IsTimeUp(1000) )
	{
		_oCVStop();

		while( !ioStopperUp() ) Error(ERR_STOPPER_UP, emRetry, m_sName, m_sErrPath);
		Wait(10);

		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_SET_INPUT;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && !chkExitSensor() )
	{
		_oCVStop();

		while( !ioStopperUp() ) Error(ERR_STOPPER_UP, emRetry, m_sName, m_sErrPath);
		Wait(10);

		m_tmCVRun.Start();
		m_nAutoState = AS_WAIT_SET_INPUT;
		return;
	}
}

void CLoadCV2Station::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtLoadComp.Reset();

	m_nAutoState = AS_INIT;
}

BOOL CLoadCV2Station::ioStopperUp()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_STOPPER_DOWN] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN2 );

	Sleep(500);

	return TRUE;
}

BOOL CLoadCV2Station::ioStopperDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_STOPPER_DOWN] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN2) );

	return TRUE;
}

void CLoadCV2Station::_oStopperUp()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_STOPPER_DOWN] = FALSE;
}

void CLoadCV2Station::_oStopperDown()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_STOPPER_DOWN] = TRUE;
}

void CLoadCV2Station::_oCVRun()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDCV2_RUN] = TRUE;
}

void CLoadCV2Station::_oCVStop()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDCV2_RUN] = FALSE;
}

BOOL CLoadCV2Station::chkStopperDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN2 ) return TRUE;

	return FALSE;
}

BOOL CLoadCV2Station::chkEnterSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN2) ) return TRUE;

	return FALSE;
}

BOOL CLoadCV2Station::chkExitSensor()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN0) ) return TRUE;

	return FALSE;
}
