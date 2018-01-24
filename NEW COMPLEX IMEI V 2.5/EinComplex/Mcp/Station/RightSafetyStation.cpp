#include "StdAfx.h"
#include "RightSafetyStation.h"

#include "Resource.h"
#include "MainFrm.h"
#include "MainControlStation.h"
#include "RightPPStation.h"
#include "RightTransferStation.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CRightSafetyStation::CRightSafetyStation()
{
	m_sName = _T("RightSafetyStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), _T("MainControlStation"));
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), _T("MainControlStation"));
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);
	m_trcError.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("Error"));

	m_nAutoState = AS_INIT;
}

CRightSafetyStation::~CRightSafetyStation()
{
	DeleteThreads();
}

void CRightSafetyStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CRightSafetyStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CRightSafetyStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CRightSafetyStation::InitRecipe()
{
	CAxStation::InitRecipe();
}	

void CRightSafetyStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CRightSafetyStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CRightSafetyStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	m_pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));

	InitVariable();
}

void CRightSafetyStation::Setup()
{
	Wait(100);
}

void CRightSafetyStation::PostAbort()
{
	InitVariable();
}

void CRightSafetyStation::PostStop(UINT nMode)
{
}

void CRightSafetyStation::PreStart()
{
}

UINT CRightSafetyStation::AutoRun()
{
	while( TRUE ) 
	{
		WaitRight(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			default:					break;
		}
	}

	return 0;
}

UINT CRightSafetyStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CRightSafetyStation::AsInit()
{
	m_nAutoState = AS_RUN;
}

void CRightSafetyStation::AsRun()
{
	if( (m_pMainStn->m_nStateRight == MS_AUTO) && chkRightDoorOpen() )
	{
		RightAxisStop();
		ErrorRight(CMainControlStation::ERR_RIGHT_DOOR_OPEN, emRetry, _T("MainControlStation"), m_sErrPath);
		return;
	}
}

void CRightSafetyStation::InitVariable()
{
	m_nAutoState = AS_INIT; 
}

void CRightSafetyStation::WaitRight(DWORD dwTime)
{
	CAxTimer timer;
	timer.Start();

	while( TRUE )
	{
		if( (m_pMainStn->m_nStateRight == MS_AUTO) &&
			(timer.IsTimeUp((LONG)dwTime)) ) break;

		if( m_pMainStn->m_nStateLeft == MS_AUTO ) Sleep(1);
		else Wait(1);
	}
}

int CRightSafetyStation::ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
									LPCTSTR pszParam1, LPCTSTR pszParam2, LPCTSTR pszParam3, LPCTSTR pszParam4)
{
	if( m_pMainStn->m_nStateRight == MS_ERROR )
	{
		WaitRight(10);
		return emRetry;
	}

	m_pMainStn->m_nStateRight = MS_ERROR;

	m_errCtrl.m_pErrMsg->m_nNumber = nNumber;
	m_errCtrl.m_pErrMsg->m_nType = nType;
	m_errCtrl.m_pErrMsg->m_sSource = pszSource;
	m_errCtrl.m_pErrMsg->m_sHelpFile = pszPath;
	m_errCtrl.m_pErrMsg->m_sParams[0] = pszParam1;
	m_errCtrl.m_pErrMsg->m_sParams[1] = pszParam2;
	m_errCtrl.m_pErrMsg->m_sParams[2] = pszParam3;
	m_errCtrl.m_pErrMsg->m_sParams[3] = pszParam4;
	CAxErrorMgr::GetErrorMgr()->RaiseError((CAxErrData)(*m_errCtrl.m_pErrMsg));

	m_pMainStn->m_nResponseRight = emNone;
	SendMessage(m_pMainFrm->GetSafeHwnd(), UM_MCP_ERROR, 0, 0);

	while( TRUE )
	{
		if( m_pMainStn->m_nResponseRight != emNone ) break;

		if( m_pMainStn->m_nStateRight != MS_ERROR )
		{
			WaitRight(10);
			return emRetry;
		}

		if( m_pMainStn->m_nStateLeft == MS_AUTO ) Sleep(1);
		else Wait(1);
	}

	int nRet = m_pMainStn->m_nResponseRight;
	m_pMainStn->m_nResponseRight = emNone;
	m_pMainStn->m_nStateRight = MS_AUTO;

	return nRet;
}

BOOL CRightSafetyStation::chkRightDoorOpen()
{
	if( m_pMainStn->IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}

void CRightSafetyStation::RightAxisStop()
{
	FAS_AllMoveStop(DEF_FAS_RIGHT);
}

void CRightSafetyStation::RightAxisEStop()
{
	FAS_AllEmergencyStop(DEF_FAS_RIGHT);
}
