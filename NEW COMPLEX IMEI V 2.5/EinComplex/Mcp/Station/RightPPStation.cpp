#include "StdAfx.h"
#include "RightPPStation.h"

#include "MainControlStation.h"
#include "LoadCVStation.h"
#include "UnloadCVStation.h"
#include "LeftPPStation.h"
#include "RightTransferStation.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CRightPPStation::CRightPPStation()
{
	m_sName = _T("RightPPStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CRightPPStation::~CRightPPStation()
{
	DeleteThreads();
}

void CRightPPStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtEnablePosCV"), m_evtEnablePosCV);
	m_profile.AddEvent(_T("evtEnablePosBuff"), m_evtEnablePosBuff);
	m_profile.AddEvent(_T("evtLoadCVPickComp"), m_evtLoadCVPickComp);
	m_profile.AddEvent(_T("evtLoadBuffPlaceComp"), m_evtLoadBuffPlaceComp);
	m_profile.AddEvent(_T("evtUnloadCVStopReq"), m_evtUnloadCVStopReq);
	m_profile.AddEvent(_T("evtUnloadCVPlaceComp"), m_evtUnloadCVPlaceComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
	m_profile.AddDouble(_T("dScaleX"), m_dScaleX);
	m_profile.AddDouble(_T("dVelocityX"), m_dVelocityX);
	m_profile.AddDouble(_T("dSlowSpdX"), m_dSlowSpdX);
	m_profile.AddDouble(_T("dSWLimitPosX"), m_dSWLimitPosX);
	m_profile.AddDouble(_T("dSWLimitNegX"), m_dSWLimitNegX);
}

void CRightPPStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CRightPPStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CRightPPStation::InitRecipe()
{
	CAxStation::InitRecipe();

	m_recipe.AddRobotLoc(_T("locWait"), m_locWait);
	m_recipe.AddRobotLoc(_T("locCV"), m_locCV);
	m_recipe.AddRobotLoc(_T("locBuff"), m_locBuff);
}	

void CRightPPStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CRightPPStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CRightPPStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLoadCVStn = (CLoadCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("LoadCVStation"));
	m_pUnloadCVStn = (CUnloadCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("UnloadCVStation"));
	m_pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	InitVariable();
}

void CRightPPStation::Setup()
{
	Wait(100);
}

void CRightPPStation::PostAbort()
{
	InitVariable();
}

void CRightPPStation::PostStop(UINT nMode)
{
}

void CRightPPStation::PreStart()
{
}

UINT CRightPPStation::AutoRun()
{
	while( TRUE ) 
	{
		WaitRight(10);

		switch( m_nAutoState )
		{
			case AS_INIT:				AsInit();				break;
			case AS_JOB_CHECK:			AsJobCheck();			break;
			case AS_WAIT:				AsWait();				break;
			case AS_LOAD_PICK_UP:		AsLoadPickUp();			break;
			case AS_LOAD_PLACE_DOWN:	AsLoadPlaceDown();		break;
			case AS_UNLOAD_PICK_UP:		AsUnloadPickUp();		break;
			case AS_UNLOAD_PLACE_DOWN:	AsUnloadPlaceDown();	break;
			default:											break;
		}
	}

	return 0;
}

UINT CRightPPStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CRightPPStation::AsInit()
{
	m_pMainFrm->m_bInitializing_RPP = TRUE;

	WaitEvent(m_pRightTRStn->m_evtInitComp);

	while( !ioLoadUngrip() ) ErrorRight(ERR_LOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);
	while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);
	while( !ioUnloadUngrip() ) ErrorRight(ERR_UNLOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);
	while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);
	while( !moveOriginX() ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	WaitRight(10);
	while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);

	m_evtEnablePosCV.Set();
	m_evtEnablePosBuff.Set();

	m_nAutoState = AS_JOB_CHECK;

	m_pMainFrm->m_bInitializing_RPP = FALSE;
}

void CRightPPStation::AsJobCheck()
{
	if( m_pLoadCVStn->m_evtLoadComp.IsSet() &&
		m_pLeftPPStn->m_evtEnablePosCV.IsSet() &&
		m_pRightTRStn->m_evtEnableUseLoadBuff.IsSet() &&
		m_pRightTRStn->chkOnlyLoadable() )
	{
		m_nAutoState = AS_LOAD_PICK_UP;
		return;
	}

	if( m_pRightTRStn->m_evtUnloadComp.IsSet() &&
		m_pRightTRStn->m_evtEnablePosBuff.IsSet() &&
		m_pUnloadCVStn->m_evtEnableUseCV.IsSet() &&
		(m_pMainStn->IsDryRunMode() || !m_pUnloadCVStn->chkEnterSensor()) )
	{
		m_nAutoState = AS_UNLOAD_PICK_UP;
		return;
	}

	m_nAutoState = AS_WAIT;
}

void CRightPPStation::AsWait()
{
	while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosCV.Set();
	m_evtEnablePosBuff.Set();
	WaitRight(10);

	m_nAutoState = AS_JOB_CHECK;
}

void CRightPPStation::AsLoadPickUp()
{
	if( !m_pMainStn->IsDryRunMode() && !chkLoadGrip() )
	{
		m_nAutoState = AS_LOAD_PLACE_DOWN;
		return;
	}

	if( !chkLoadUp() )
	{
		while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !chkUnloadUp() )
	{
		while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !m_pLeftPPStn->m_evtEnablePosCV.IsSet() )
	{
		m_nAutoState = AS_JOB_CHECK;
		return;
	}
	
	BOOL bPrevEvtState = m_evtEnablePosCV.IsSet();
	m_evtEnablePosCV.Reset();
	Sleep(10);
	if( !m_pLeftPPStn->m_evtEnablePosCV.IsSet() )
	{
		if( bPrevEvtState ) m_evtEnablePosCV.Set();
		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	while( !moveX(m_locCV.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosBuff.Set();
	WaitRight(10);

	while( !ioLoadDown() )
	{
		while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);

		ErrorRight(ERR_LOAD_DOWN, emRetry, m_sName, m_sErrPath);
	}

	if( !m_pMainStn->IsDryRunMode() )
	{
		while( !ioLoadGrip() )
		{
			while( !ioLoadUngrip() ) ErrorRight(ERR_LOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);
			while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			ErrorRight(ERR_LOAD_GRIP, emRetry, m_sName, m_sErrPath);

			if( m_pLoadCVStn->chkExitSensor() )
			{
				while( !ioLoadDown() ) ErrorRight(ERR_LOAD_DOWN, emRetry, m_sName, m_sErrPath);
				WaitRight(10);
			}
			else
			{
				while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
				m_evtEnablePosCV.Set();
				m_pLoadCVStn->m_evtLoadComp.Reset();
				m_evtLoadCVPickComp.Set();
				WaitRight(10);

				m_nAutoState = AS_JOB_CHECK;
				return;
			}
		}
		WaitRight(10);
	}
	else WaitRight(1500);

	while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() && chkLoadGrip() )
	{
		ErrorRight(ERR_LOAD_GRIP, emRetry, m_sName, m_sErrPath);

		if( chkLoadGrip() )
		{
			while( !ioLoadUngrip() ) ErrorRight(ERR_LOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);
			while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
			m_evtEnablePosCV.Set();
			m_pLoadCVStn->m_evtLoadComp.Reset();
			m_evtLoadCVPickComp.Set();
			WaitRight(10);

			m_nAutoState = AS_JOB_CHECK;
			return;
		}
	}

	m_pLoadCVStn->m_evtLoadComp.Reset();
	m_evtLoadCVPickComp.Set();

	m_nAutoState = AS_LOAD_PLACE_DOWN;
}

void CRightPPStation::AsLoadPlaceDown()
{
	if( !m_pRightTRStn->m_evtEnablePosBuff.IsSet() ||
		!m_pRightTRStn->m_evtEnableUseLoadBuff.IsSet() )
	{
		while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
		m_evtEnablePosCV.Set();
		m_evtEnablePosBuff.Set();
		WaitRight(10);
		return;
	}

	if( !chkLoadUp() )
	{
		while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !chkUnloadUp() )
	{
		while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bPrevEvtState = m_evtEnablePosBuff.IsSet();
	m_evtEnablePosBuff.Reset();
	Sleep(10);
	if( !m_pRightTRStn->m_evtEnablePosBuff.IsSet() )
	{
		if( bPrevEvtState ) m_evtEnablePosBuff.Set();
		return;
	}

	while( !moveX(m_locBuff.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosCV.Set();
	WaitRight(10);

	while( !m_pMainStn->IsDryRunMode() && chkLoadBuffExist() )
	{
		ErrorRight(ERR_LOAD_EXIST, emRetry, m_sName, m_sErrPath);
	}

	while( !ioLoadDown() )
	{
		_oLoadUp();
		ErrorRight(ERR_LOAD_DOWN, emRetry, m_sName, m_sErrPath);
	}
	WaitRight(500);

	if( !m_pMainStn->IsDryRunMode() )
	{
		while( !ioLoadUngrip() ) ErrorRight(ERR_LOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}
	else WaitRight(500);

	while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	m_pRightTRStn->m_evtEnableUseLoadBuff.Reset();
	m_evtLoadBuffPlaceComp.Set();

	m_nAutoState = AS_JOB_CHECK;
}

void CRightPPStation::AsUnloadPickUp()
{
	if( !m_pMainStn->IsDryRunMode() && !chkUnloadGrip() )
	{
		m_nAutoState = AS_UNLOAD_PLACE_DOWN;
		return;
	}

	if( !chkLoadUp() )
	{
		while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !chkUnloadUp() )
	{
		while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !m_pRightTRStn->m_evtEnablePosBuff.IsSet() )
	{
		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && !chkUnloadBuffExist() )
	{
		ErrorRight(ERR_UNLOAD_NOT_EXIST, emRetry, m_sName, m_sErrPath);

		if( !chkUnloadBuffExist() )
		{
			m_pRightTRStn->m_evtUnloadComp.Reset();
			m_pRightTRStn->m_evtEnableUseUnloadBuff.Set();
		}

		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	BOOL bPrevEvtState = m_evtEnablePosBuff.IsSet();
	m_evtEnablePosBuff.Reset();
	Sleep(10);
	if( !m_pRightTRStn->m_evtEnablePosBuff.IsSet() )
	{
		if( bPrevEvtState ) m_evtEnablePosBuff.Set();
		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	while( !moveX(m_locBuff.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosCV.Set();
	WaitRight(10);

	while( !ioUnloadDown() )
	{
		while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);

		ErrorRight(ERR_LOAD_DOWN, emRetry, m_sName, m_sErrPath);
	}

	if( !m_pMainStn->IsDryRunMode() )
	{
		while( !ioUnloadGrip() )
		{
			while( !ioUnloadUngrip() ) ErrorRight(ERR_UNLOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);
			while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			ErrorRight(ERR_UNLOAD_GRIP, emRetry, m_sName, m_sErrPath);

			if( chkUnloadBuffExist() )
			{
				while( !ioLoadDown() ) ErrorRight(ERR_LOAD_DOWN, emRetry, m_sName, m_sErrPath);
				WaitRight(10);
			}
			else
			{
				while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
				m_evtEnablePosBuff.Set();
				if( !chkUnloadBuffExist() )
				{
					m_pRightTRStn->m_evtUnloadComp.Reset();
					m_pRightTRStn->m_evtEnableUseUnloadBuff.Set();
				}
				WaitRight(10);

				m_nAutoState = AS_JOB_CHECK;
				return;
			}
		}
		WaitRight(10);
	}
	else WaitRight(1500);

	while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() && chkUnloadGrip() )
	{
		ErrorRight(ERR_UNLOAD_GRIP, emRetry, m_sName, m_sErrPath);

		if( !chkUnloadBuffExist() )
		{
			while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
			m_evtEnablePosBuff.Set();
			m_pRightTRStn->m_evtUnloadComp.Reset();
			m_pRightTRStn->m_evtEnableUseUnloadBuff.Set();
			WaitRight(10);
		}

		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	m_pRightTRStn->m_evtUnloadComp.Reset();
	m_pRightTRStn->m_evtEnableUseUnloadBuff.Set();

	m_nAutoState = AS_UNLOAD_PLACE_DOWN;
}

void CRightPPStation::AsUnloadPlaceDown()
{
	if( !m_pLeftPPStn->m_evtEnablePosCV.IsSet() ||
		!m_pUnloadCVStn->m_evtEnableUseCV.IsSet() ||
		(!m_pMainStn->IsDryRunMode() && m_pUnloadCVStn->chkEnterSensor()) )
	{
		while( !moveX(m_locWait.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
		m_evtEnablePosCV.Set();
		WaitRight(10);

		if( !m_pMainStn->IsDryRunMode() )
		{
			if( chkUnloadGrip() )
			{
				ErrorRight(ERR_UNLOAD_GRIP, emRetry, m_sName, m_sErrPath);

				if( chkUnloadGrip() )
				{
					while( !ioUnloadUngrip() ) ErrorRight(ERR_UNLOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
					WaitRight(10);

					m_evtEnablePosBuff.Set();

					if( chkUnloadBuffExist() )
					{
						m_pRightTRStn->m_evtUnloadComp.Set();
						m_pRightTRStn->m_evtEnableUseUnloadBuff.Reset();
					}

					m_nAutoState = AS_JOB_CHECK;
					return;
				}
			}
		}

		m_evtEnablePosBuff.Set();
		return;
	}

	if( !chkLoadUp() )
	{
		while( !ioLoadUp() ) ErrorRight(ERR_LOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	if( !chkUnloadUp() )
	{
		while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bPrevEvtState = m_evtEnablePosCV.IsSet();
	m_evtEnablePosCV.Reset();
	Sleep(10);
	if( !m_pLeftPPStn->m_evtEnablePosCV.IsSet() )
	{
		if( bPrevEvtState ) m_evtEnablePosCV.Set();
		return;
	}

	while( !moveX(m_locCV.x) ) ErrorRight(ERR_MOVE_X, emRetry, m_sName, m_sErrPath);
	m_evtUnloadCVStopReq.Set();
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() )
	{
		if( chkUnloadGrip() )
		{
			ErrorRight(ERR_UNLOAD_GRIP, emRetry, m_sName, m_sErrPath);

			if( chkUnloadGrip() )
			{
				while( !ioUnloadUngrip() ) ErrorRight(ERR_UNLOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				m_evtEnablePosBuff.Set();

				WaitEvent(m_pUnloadCVStn->m_evtCVStopComp);
				m_pUnloadCVStn->m_evtCVStopComp.Reset();
				m_evtUnloadCVPlaceComp.Set();

				if( chkUnloadBuffExist() )
				{
					m_pRightTRStn->m_evtUnloadComp.Set();
					m_pRightTRStn->m_evtEnableUseUnloadBuff.Reset();
				}

				m_nAutoState = AS_JOB_CHECK;
				return;
			}
		}
	}

	m_evtEnablePosBuff.Set();

	WaitEvent(m_pUnloadCVStn->m_evtCVStopComp);
	m_pUnloadCVStn->m_evtCVStopComp.Reset();

	while( !ioUnloadDown() )
	{
		_oUnloadUp();
		ErrorRight(ERR_UNLOAD_DOWN, emRetry, m_sName, m_sErrPath);
	}
	WaitRight(500);

	if( !m_pMainStn->IsDryRunMode() )
	{
		while( !ioUnloadUngrip() ) ErrorRight(ERR_UNLOAD_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}
	else WaitRight(500);

	while( !ioUnloadUp() ) ErrorRight(ERR_UNLOAD_UP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	m_evtUnloadCVPlaceComp.Set();

	if( m_pLoadCVStn->m_evtLoadComp.IsSet() ) WaitRight(500);

	m_nAutoState = AS_JOB_CHECK;
}

void CRightPPStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtEnablePosCV.Reset();
	m_evtEnablePosBuff.Reset();
	m_evtLoadCVPickComp.Reset();
	m_evtLoadBuffPlaceComp.Reset();
	m_evtUnloadCVStopReq.Reset();
	m_evtUnloadCVPlaceComp.Reset();

	m_nAutoState = AS_INIT;
}

void CRightPPStation::WaitRight(DWORD dwTime)
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

int CRightPPStation::ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
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

BOOL CRightPPStation::moveX(double dPos, BOOL bSlow)
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	BOOL bRetLD = ioLoadUp();
	BOOL bRetUD = ioUnloadUp();
	if( !bRetLD || !bRetUD ) return FALSE;

	if( (dPos > m_dSWLimitPosX) ||
		(dPos < m_dSWLimitNegX) ) return FALSE;

	long lCurPosLeft = 0;
	double dCurPosLeft = 0.0;

	if( dPos > (m_locWait.x + m_pMainStn->m_dInposition) )
	{
		while( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_PP, &lCurPosLeft) != FMM_OK ) Sleep(10);
		dCurPosLeft = (double)lCurPosLeft / m_pLeftPPStn->m_dScaleX;

		if( dCurPosLeft > (m_pLeftPPStn->m_locWait.x + m_pMainStn->m_dInposition) )
		{
			return FALSE;
		}
	}

	long lCurX = 0;
	long lCurY = 0;
	long lCurZ = 0;
	double dCurX = 0.0;
	double dCurY = 0.0;
	double dCurZ = 0.0;

	if( dPos < (m_locWait.x - m_pMainStn->m_dInposition) )
	{
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurX) != FMM_OK ) Sleep(10);
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurY) != FMM_OK ) Sleep(10);
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurZ) != FMM_OK ) Sleep(10);
		dCurX = (double)lCurX / m_pRightTRStn->m_dScaleX;
		dCurY = (double)lCurY / m_pRightTRStn->m_dScaleY;
		dCurZ = (double)lCurZ / m_pRightTRStn->m_dScaleZ;

		if( (dCurX >= (m_pRightTRStn->m_locLoad.x - m_pMainStn->m_dInposition)) &&
			(dCurX <= (m_pRightTRStn->m_locLoad.x + m_pMainStn->m_dInposition)) )
		{
			if( (dCurY >= (m_pRightTRStn->m_locLoad.y - m_pMainStn->m_dInposition)) &&
				(dCurY <= (m_pRightTRStn->m_locLoad.y + m_pMainStn->m_dInposition)) )
			{
				if( dCurZ > (m_pMainStn->m_dInterlockPosZ + m_pMainStn->m_dInposition) )
				{
					return FALSE;
				}
			}
		}

		if( (dCurX >= (m_pRightTRStn->m_locUnload.x - m_pMainStn->m_dInposition)) &&
			(dCurX <= (m_pRightTRStn->m_locUnload.x + m_pMainStn->m_dInposition)) )
		{
			if( (dCurY >= (m_pRightTRStn->m_locUnload.y - m_pMainStn->m_dInposition)) &&
				(dCurY <= (m_pRightTRStn->m_locUnload.y + m_pMainStn->m_dInposition)) )
			{
				if( dCurZ > (m_pMainStn->m_dInterlockPosZ + m_pMainStn->m_dInposition) )
				{
					return FALSE;
				}
			}
		}
	}

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	long lAbsPos, lVelocity;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_PP);

	if( stAxisStatus.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, TRUE);

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_PP, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	lAbsPos = (long)(dPos * m_dScaleX);
	lVelocity = bSlow ? (long)(m_dSlowSpdX * m_dScaleX) : (long)(m_dVelocityX * m_dScaleX);
	nRtn = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_PP, lAbsPos, lVelocity);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp((long)m_pMainStn->m_nMotionTimeout)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_PP);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_MOTIONING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_PP, &lCurPos) != FMM_OK ) return FALSE;

	dCurPos = (double)lCurPos / m_dScaleX;

	if( fabs(dPos - dCurPos) < m_pMainStn->m_dInposition ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::moveOriginX()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	BOOL bRetLD = ioLoadUp();
	BOOL bRetUD = ioUnloadUp();
	if( !bRetLD || !bRetUD ) return FALSE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_PP);

	if( stAxisStatus.FFLAG_SERVOON == 0 )
	{
		FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, TRUE);
		Sleep(3000);
	}

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_PP, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	nRtn = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_PP);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp(30000)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_PP);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_ORIGINRETURNING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_PP, &lCurPos) != FMM_OK ) return FALSE;

	dCurPos = (double)lCurPos / m_dScaleX;

	if( fabs(dCurPos) < m_pMainStn->m_dInposition) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::ioLoadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN2) );

	return TRUE;
}

BOOL CRightPPStation::ioLoadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN2 );

	return TRUE;
}

BOOL CRightPPStation::ioUnloadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN4) );

	return TRUE;
}

BOOL CRightPPStation::ioUnloadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN4 );

	return TRUE;
}

BOOL CRightPPStation::ioLoadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN7 );

	Sleep(500);

	return TRUE;
}

BOOL CRightPPStation::ioLoadUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN7) );

	return TRUE;
}

BOOL CRightPPStation::ioUnloadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN8 );

	Sleep(500);

	return TRUE;
}

BOOL CRightPPStation::ioUnloadUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN8) );

	return TRUE;
}

void CRightPPStation::_oLoadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;
}

void CRightPPStation::_oLoadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;
}

void CRightPPStation::_oUnloadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;
}

void CRightPPStation::_oUnloadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;
}

void CRightPPStation::_oLoadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = TRUE;
}

void CRightPPStation::_oLoadUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = FALSE;
}

void CRightPPStation::_oUnloadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = TRUE;
}

void CRightPPStation::_oUnloadUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = FALSE;
}

BOOL CRightPPStation::chkLoadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	BOOL bUp = FALSE;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN2 ) bUp = TRUE;

	if( bUp ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkLoadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	BOOL bUp = FALSE;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN2 ) bUp = TRUE;

	if( !bUp ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkUnloadUp()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	BOOL bUp = FALSE;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN4 ) bUp = TRUE;

	if( bUp ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkUnloadDown()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	BOOL bUp = FALSE;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN4 ) bUp = TRUE;

	if( !bUp ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkLoadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN7 ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkUnloadGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN8 ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkLoadBuffExist()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN0) ) return TRUE;

	return FALSE;
}

BOOL CRightPPStation::chkUnloadBuffExist()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN1) ) return TRUE;

	return FALSE;
}
