#include "StdAfx.h"
#include "InsertCVSystem.h"
#include "MainControlStation.h"

#include "Resource.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CInsertCVSystem::CInsertCVSystem()
{
	m_sName = _T("InsertCVSystem");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\System\\%s.ini"), m_sName);

	m_nAutoState = AS_INIT;
}

CInsertCVSystem::~CInsertCVSystem()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);

	DeleteAxThread(m_hPriThread);
}

void CInsertCVSystem::InitProfile()
{
	CAxSystem::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CInsertCVSystem::Startup()
{
	CAxSystem::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();
}

UINT CInsertCVSystem::AutoRun()
{
	while( TRUE )
	{
		WaitNS(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			default:
				if( m_bTerminate ) throw -1;
				break;
		}
	}

	return 0;
}

void CInsertCVSystem::AsInit()
{
	m_tmMachineStop.Start();

	m_nAutoState = AS_RUN;
}

void CInsertCVSystem::AsRun()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	if( pMainStn->IsNormalMode() )
	{
		if( chkLoadCVExitSensor() ) m_tmMachineStop.Start();
		else
		{
			if( m_tmMachineStop.IsTimeUp(pMainStn->m_nMaxMachineRestTime) )
			{
				m_tmMachineStop.Start();
				if( pMainStn->m_nStateLeft == MS_AUTO ) pMainStn->m_nStateLeft = MS_AUTO_STOP;
				if( pMainStn->m_nStateRight == MS_AUTO ) pMainStn->m_nStateRight = MS_AUTO_STOP;
				if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) CAxMaster::GetMaster()->Stop();
			}
		}
	}
	else m_tmMachineStop.Start();

	if( pMainStn->m_bInsertCVSyncMode )
	{
		if( !chkLoadCVRun() ) _oCVStop();
		else _oCVRun();
	}
	else
	{
		if( chkExitSensor() ) _oCVStop();
		else _oCVRun();
	}
}

void CInsertCVSystem::_oCVRun()
{
	if( (m_pMainFrm != NULL) && (m_pMainFrm->m_hWnd != NULL) )
	{
		if( m_pMainFrm->m_bSerialHubCreated == TRUE )
		{
			CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
			pWorld->m_bOutput[DEF_IO_INCV_RUN] = TRUE;
		}
	}
}

void CInsertCVSystem::_oCVStop()
{
	if( (m_pMainFrm != NULL) && (m_pMainFrm->m_hWnd != NULL) )
	{
		if( m_pMainFrm->m_bSerialHubCreated == TRUE )
		{
			CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
			pWorld->m_bOutput[DEF_IO_INCV_RUN] = FALSE;
		}
	}
}

BOOL CInsertCVSystem::chkExitSensor()
{
	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN7 ) return TRUE;

	return FALSE;
}

BOOL CInsertCVSystem::chkLoadCVExitSensor()
{
	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}

BOOL CInsertCVSystem::chkLoadCVRun()
{
	if( (m_pMainFrm != NULL) && (m_pMainFrm->m_hWnd != NULL) )
	{
		if( m_pMainFrm->m_bSerialHubCreated == TRUE )
		{
			CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
			return pWorld->m_bOutput[DEF_IO_LDCV_RUN];
		}
	}

	return FALSE;
}
