#include "StdAfx.h"
#include "LeftSafetyStation.h"

#include "Resource.h"
#include "MainFrm.h"
#include "MainControlStation.h"
#include "LeftPPStation.h"
#include "LeftTransferStation.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CLeftSafetyStation::CLeftSafetyStation()
{
	m_sName = _T("LeftSafetyStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), _T("MainControlStation"));
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), _T("MainControlStation"));
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);
	m_trcError.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("Error"));

	m_nAutoState = AS_INIT;
}

CLeftSafetyStation::~CLeftSafetyStation()
{
	DeleteThreads();
}

void CLeftSafetyStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CLeftSafetyStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CLeftSafetyStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CLeftSafetyStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CLeftSafetyStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CLeftSafetyStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CLeftSafetyStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));

	InitVariable();
}

void CLeftSafetyStation::Setup()
{
	Wait(100);
}

void CLeftSafetyStation::PostAbort()
{
	InitVariable();
}

void CLeftSafetyStation::PostStop(UINT nMode)
{
}

void CLeftSafetyStation::PreStart()
{
}

UINT CLeftSafetyStation::AutoRun()
{
	while( TRUE ) 
	{
		WaitLeft(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			default:					break;
		}
	}

	return 0;
}

UINT CLeftSafetyStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CLeftSafetyStation::AsInit()
{
	m_nAutoState = AS_RUN;
}

void CLeftSafetyStation::AsRun()
{
	if( (m_pMainStn->m_nStateLeft == MS_AUTO) && chkLeftDoorOpen() )
	{
		LeftAxisStop();
		ErrorLeft(CMainControlStation::ERR_LEFT_DOOR_OPEN, emRetry, _T("MainControlStation"), m_sErrPath);
		return;
	}
}

void CLeftSafetyStation::InitVariable()
{
	m_nAutoState = AS_INIT; 
}

void CLeftSafetyStation::WaitLeft(DWORD dwTime)
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

int CLeftSafetyStation::ErrorLeft(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
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

BOOL CLeftSafetyStation::chkLeftDoorOpen()
{
	if( m_pMainStn->IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}

void CLeftSafetyStation::LeftAxisStop()
{
	FAS_AllMoveStop(DEF_FAS_LEFT);
}

void CLeftSafetyStation::LeftAxisEStop()
{
	FAS_AllEmergencyStop(DEF_FAS_LEFT);
}
