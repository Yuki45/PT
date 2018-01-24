#include "StdAfx.h"
#include "OPSystem.h"
#include "MainFrm.h"
#include "MainControlStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

COPSystem::COPSystem()
{
	m_sName = _T("OPSystem");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\System\\%s.ini"), m_sName);
}

COPSystem::~COPSystem()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);

	DeleteAxThread(m_hPriThread);
}

void COPSystem::InitProfile()
{
	CAxSystem::InitProfile();

	m_profile.AddInt(_T("nBlinkInterval"), m_nBlinkInterval);
	m_profile.AddInt(_T("nAutoBuzzerStopTime"), m_nAutoBuzzerStopTime);
}

void COPSystem::Startup()
{
	CAxSystem::Startup();

	m_bForceBuzzer = FALSE;
	m_bBuzzerStop = FALSE;
	m_bBuzzerBlink = TRUE;
	m_tmBuzzerBlink.Start();

	m_bLeftBlink = TRUE;
	m_tmLeftBlink.Start();

	m_bRightBlink = TRUE;
	m_tmRightBlink.Start();

	m_bEStopError = FALSE;
}

UINT COPSystem::AutoRun()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	while( TRUE )
	{
		//////////////////////////////////////////////////////////////////////////
		// Left Switch
		if( GetLeftStartSwitch() )
		{
			if( pMainStn->m_nStateLeft != MS_AUTO )
			{
				pMainStn->m_nResponseLeft = emRetry;
				pMainStn->m_nStateLeft = MS_AUTO;
				CAxErrorMgr::GetErrorMgr()->SetResponse(emRetry);
				pMaster->Start();
			}

			if( (pMainStn->m_nStateLeft == MS_AUTO) ||
				(pMainStn->m_nStateRight == MS_AUTO) )
				pMainStn->m_pMainFrm->MMMSLog(_T("9000"));
		}
		else if( GetLeftStopSwitch() )
		{
			pMainStn->m_nStateLeft = MS_AUTO_STOP;
			if( pMainStn->m_nStateRight != MS_AUTO ) pMaster->Stop();

			if( (pMainStn->m_nStateLeft == MS_AUTO_STOP) &&
				(pMainStn->m_nStateRight == MS_AUTO_STOP) )
				pMainStn->m_pMainFrm->MMMSLog(_T("9001"));
		}
		else if( GetLeftResetSwitch() )
		{
			if( pMainStn->m_nStateLeft == MS_ERROR ) m_bBuzzerStop = TRUE;
		}

		//////////////////////////////////////////////////////////////////////////
		// Right Switch
		if( GetRightStartSwitch() )
		{
			if( pMainStn->m_nStateRight != MS_AUTO )
			{
				pMainStn->m_nResponseRight = emRetry;
				pMainStn->m_nStateRight = MS_AUTO;
				CAxErrorMgr::GetErrorMgr()->SetResponse(emRetry);
				pMaster->Start();
			}

			if( (pMainStn->m_nStateLeft == MS_AUTO) ||
				(pMainStn->m_nStateRight == MS_AUTO) )
				pMainStn->m_pMainFrm->MMMSLog(_T("9000"));
		}
		else if( GetRightStopSwitch() )
		{
			pMainStn->m_nStateRight = MS_AUTO_STOP;
			if( pMainStn->m_nStateLeft != MS_AUTO ) pMaster->Stop();

			if( (pMainStn->m_nStateLeft == MS_AUTO_STOP) &&
				(pMainStn->m_nStateRight == MS_AUTO_STOP) )
				pMainStn->m_pMainFrm->MMMSLog(_T("9001"));
		}
		else if( GetRightResetSwitch() )
		{
			if( pMainStn->m_nStateRight == MS_ERROR ) m_bBuzzerStop = TRUE;
		}

		//////////////////////////////////////////////////////////////////////////
		// Left Lamp
		if( pMainStn->m_nStateLeft == MS_ERROR )
		{
			SetLeftGreenLamp(FALSE);
			SetLeftRedLamp(m_bLeftBlink);
			SetLeftYellowLamp(!m_bBuzzerStop);

			SetLeftGreenTower(FALSE);
			SetLeftRedTower(m_bLeftBlink);
			SetLeftYellowTower(FALSE);

			if( m_tmLeftBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bLeftBlink = !m_bLeftBlink;
				m_tmLeftBlink.Start();
			}
		}
		else if( (pMainStn->m_nStateLeft == MS_AUTO) &&
				 (pMainStn->m_pMainFrm->m_bInitializing_LPP || pMainStn->m_pMainFrm->m_bInitializing_LTR) )
		{
			SetLeftGreenLamp(m_bLeftBlink);
			SetLeftRedLamp(FALSE);
			SetLeftYellowLamp(FALSE);

			SetLeftGreenTower(m_bLeftBlink);
			SetLeftRedTower(FALSE);
			SetLeftYellowTower(FALSE);

			if( m_tmLeftBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bLeftBlink = !m_bLeftBlink;
				m_tmLeftBlink.Start();
			}
		}
		else if( (pMainStn->m_nStateLeft == MS_AUTO) &&
				 (pMainStn->m_pMainFrm->m_bInStop[0] ||
				 pMainStn->m_pMainFrm->m_bInStop[1] ||
				 pMainStn->m_pMainFrm->m_bInStop[2] ||
				 pMainStn->m_pMainFrm->m_bOutStop[0] ||
				 pMainStn->m_pMainFrm->m_bOutStop[1] ||
				 pMainStn->m_pMainFrm->m_bOutStop[2]) )
		{
			SetLeftGreenLamp(TRUE);
			SetLeftRedLamp(FALSE);
			SetLeftYellowLamp(m_bLeftBlink);

			SetLeftGreenTower(TRUE);
			SetLeftRedTower(FALSE);
			SetLeftYellowTower(m_bLeftBlink);

			if( m_tmLeftBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bLeftBlink = !m_bLeftBlink;
				m_tmLeftBlink.Start();
			}
		}
		else
		{
			m_bLeftBlink = TRUE;
			m_tmLeftBlink.Start();

			if( pMainStn->m_nStateLeft == MS_IDLE )
			{
				SetLeftGreenLamp(TRUE);
				SetLeftRedLamp(TRUE);
				SetLeftYellowLamp(TRUE);

				SetLeftGreenTower(TRUE);
				SetLeftRedTower(TRUE);
				SetLeftYellowTower(TRUE);
			}
			else if( pMainStn->m_nStateLeft == MS_AUTO )
			{
				SetLeftGreenLamp(TRUE);
				SetLeftRedLamp(FALSE);
				SetLeftYellowLamp(FALSE);

				SetLeftGreenTower(TRUE);
				SetLeftRedTower(FALSE);
				SetLeftYellowTower(FALSE);
			}
			else if( pMainStn->m_nStateLeft == MS_AUTO_STOP )
			{
				SetLeftGreenLamp(FALSE);
				SetLeftRedLamp(TRUE);
				SetLeftYellowLamp(FALSE);

				SetLeftGreenTower(FALSE);
				SetLeftRedTower(TRUE);
				SetLeftYellowTower(FALSE);
			}
		}

		//////////////////////////////////////////////////////////////////////////
		// Right Lamp
		if( pMainStn->m_nStateRight == MS_ERROR )
		{
			SetRightGreenLamp(FALSE);
			SetRightRedLamp(m_bRightBlink);
			SetRightYellowLamp(!m_bBuzzerStop);

			SetRightGreenTower(FALSE);
			SetRightRedTower(m_bRightBlink);
			SetRightYellowTower(FALSE);

			if( m_tmRightBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bRightBlink = !m_bRightBlink;
				m_tmRightBlink.Start();
			}
		}
		else if( (pMainStn->m_nStateRight == MS_AUTO) &&
				 (pMainStn->m_pMainFrm->m_bInitializing_RPP || pMainStn->m_pMainFrm->m_bInitializing_RTR) )
		{
			SetRightGreenLamp(m_bRightBlink);
			SetRightRedLamp(FALSE);
			SetRightYellowLamp(FALSE);

			SetRightGreenTower(m_bRightBlink);
			SetRightRedTower(FALSE);
			SetRightYellowTower(FALSE);

			if( m_tmRightBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bRightBlink = !m_bRightBlink;
				m_tmRightBlink.Start();
			}
		}
		else if( (pMainStn->m_nStateRight == MS_AUTO) &&
				 (pMainStn->m_pMainFrm->m_bInStop[3] ||
				 pMainStn->m_pMainFrm->m_bInStop[4] ||
				 pMainStn->m_pMainFrm->m_bInStop[5] ||
				 pMainStn->m_pMainFrm->m_bOutStop[3] ||
				 pMainStn->m_pMainFrm->m_bOutStop[4] ||
				 pMainStn->m_pMainFrm->m_bOutStop[5]) )
		{
			SetRightGreenLamp(TRUE);
			SetRightRedLamp(FALSE);
			SetRightYellowLamp(m_bRightBlink);

			SetRightGreenTower(TRUE);
			SetRightRedTower(FALSE);
			SetRightYellowTower(m_bRightBlink);

			if( m_tmRightBlink.IsTimeUp(m_nBlinkInterval) )
			{
				m_bRightBlink = !m_bRightBlink;
				m_tmRightBlink.Start();
			}
		}
		else
		{
			m_bRightBlink = TRUE;
			m_tmRightBlink.Start();

			if( pMainStn->m_nStateRight == MS_IDLE )
			{
				SetRightGreenLamp(TRUE);
				SetRightRedLamp(TRUE);
				SetRightYellowLamp(TRUE);

				SetRightGreenTower(TRUE);
				SetRightRedTower(TRUE);
				SetRightYellowTower(TRUE);
			}
			else if( pMainStn->m_nStateRight == MS_AUTO )
			{
				SetRightGreenLamp(TRUE);
				SetRightRedLamp(FALSE);
				SetRightYellowLamp(FALSE);

				SetRightGreenTower(TRUE);
				SetRightRedTower(FALSE);
				SetRightYellowTower(FALSE);
			}
			else if( pMainStn->m_nStateRight == MS_AUTO_STOP )
			{
				SetRightGreenLamp(FALSE);
				SetRightRedLamp(TRUE);
				SetRightYellowLamp(FALSE);

				SetRightGreenTower(FALSE);
				SetRightRedTower(TRUE);
				SetRightYellowTower(FALSE);
			}
		}

		//////////////////////////////////////////////////////////////////////////
		// Buzzer
		if( !m_bForceBuzzer )
		{
			if( (pMainStn->m_nStateLeft == MS_ERROR) || (pMainStn->m_nStateRight == MS_ERROR) )
			{
				if( m_tmBuzzerTimeout.IsTimeUp(m_nAutoBuzzerStopTime) )
				{
					m_bBuzzerStop = TRUE;
					SetBuzzer(FALSE);
				}
				else
				{
					SetBuzzer(m_bBuzzerBlink && !m_bBuzzerStop);

					if( m_tmBuzzerBlink.IsTimeUp(m_nBlinkInterval) )
					{
						m_bBuzzerBlink = !m_bBuzzerBlink;
						m_tmBuzzerBlink.Start();
					}
				}
			}
			else
			{
				SetBuzzer(FALSE);

				m_bBuzzerBlink = TRUE;
				m_bBuzzerStop = FALSE;
				m_tmBuzzerBlink.Start();
				m_tmBuzzerTimeout.Start();
			}
		}

		//////////////////////////////////////////////////////////////////////////
		// Check Error (2015.01.29)
		if( pMaster->GetState() == MS_ERROR )
		{
			pMainStn->m_nStateLeft = MS_ERROR;
			pMainStn->m_nStateRight = MS_ERROR;
		}

		//////////////////////////////////////////////////////////////////////////
		// E-Stop
		if( (pMaster->GetState() == MS_ERROR) && m_bEStopError )
		{
			m_bEStopError = FALSE;
			pMaster->Abort();
		}

		WaitNS(10);
	}

	return 0;
}

void COPSystem::SetBuzzer(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_BUZZER] = bOn;
	}
}

void COPSystem::SetLeftGreenLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_START_LAMP] = bOn;
	}
}

void COPSystem::SetLeftRedLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_STOP_LAMP] = bOn;
	}
}

void COPSystem::SetLeftYellowLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_RESET_LAMP] = bOn;
	}
}

void COPSystem::SetLeftGreenTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_TOWER_GREEN] = bOn;
	}
}

void COPSystem::SetLeftRedTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_TOWER_RED] = bOn;
	}
}

void COPSystem::SetLeftYellowTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		pWorldLeft->m_bOutput[DEF_IO_TOWER_YELLOW] = bOn;
	}
}

void COPSystem::SetLeftStartSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN0, 0);
	else FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN0);
}

void COPSystem::SetLeftStopSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN1, 0);
	else FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN1);
}

void COPSystem::SetLeftResetSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN2, 0);
	else FAS_SetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN2);
}

BOOL COPSystem::GetLeftStartSwitch()
{
	ULONGLONG dwInputLeft;
	int nRtnLeft;

	nRtnLeft = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInputLeft);
	if( nRtnLeft != FMM_OK ) return FALSE;

	if( dwInputLeft & SERVO_IN_BITMASK_USERIN0 ) return TRUE;

	return FALSE;
}

BOOL COPSystem::GetLeftStopSwitch()
{
	ULONGLONG dwInputLeft;
	int nRtnLeft;

	nRtnLeft = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInputLeft);
	if( nRtnLeft != FMM_OK ) return FALSE;

	if( dwInputLeft & SERVO_IN_BITMASK_USERIN1 ) return TRUE;

	return FALSE;
}

BOOL COPSystem::GetLeftResetSwitch()
{
	ULONGLONG dwInputLeft;
	int nRtnLeft;

	nRtnLeft = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInputLeft);
	if( nRtnLeft != FMM_OK ) return FALSE;

	if( dwInputLeft & SERVO_IN_BITMASK_USERIN2 ) return TRUE;

	return FALSE;
}

void COPSystem::SetRightGreenLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_START_LAMP] = bOn;
	}
}

void COPSystem::SetRightRedLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_STOP_LAMP] = bOn;
	}
}

void COPSystem::SetRightYellowLamp(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_RESET_LAMP] = bOn;
	}
}

void COPSystem::SetRightGreenTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_TOWER_GREEN] = bOn;
	}
}

void COPSystem::SetRightRedTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_TOWER_RED] = bOn;
	}
}

void COPSystem::SetRightYellowTower(BOOL bOn)
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	if( (pMainFrm == NULL) || (pMainFrm->m_hWnd == NULL) ) return;

	if( pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		pWorldRight->m_bOutput[DEF_IO_TOWER_YELLOW] = bOn;
	}
}

void COPSystem::SetRightStartSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN0, 0);
	else FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN0);
}

void COPSystem::SetRightStopSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN1, 0);
	else FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN1);
}

void COPSystem::SetRightResetSwitch(BOOL bOn)
{
	if( bOn ) FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_IN_BITMASK_USERIN2, 0);
	else FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_BITMASK_USERIN2);
}

BOOL COPSystem::GetRightStartSwitch()
{
	ULONGLONG dwInputRight;
	int nRtnRight;

	nRtnRight = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInputRight);
	if( nRtnRight != FMM_OK ) return FALSE;

	if( dwInputRight & SERVO_IN_BITMASK_USERIN0 ) return TRUE;

	return FALSE;
}

BOOL COPSystem::GetRightStopSwitch()
{
	ULONGLONG dwInputRight;
	int nRtnRight;

	nRtnRight = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInputRight);
	if( nRtnRight != FMM_OK ) return FALSE;

	if( dwInputRight & SERVO_IN_BITMASK_USERIN1 ) return TRUE;

	return FALSE;
}

BOOL COPSystem::GetRightResetSwitch()
{
	ULONGLONG dwInputRight;
	int nRtnRight;

	nRtnRight = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInputRight);
	if( nRtnRight != FMM_OK ) return FALSE;

	if( dwInputRight & SERVO_IN_BITMASK_USERIN2 ) return TRUE;

	return FALSE;
}
