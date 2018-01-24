#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgPpTeach.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"
#include "DlgDigitPad.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgPpTeach::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgPpTeach, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgPpTeach, CDialog)
	ON_EVENT(CDlgPpTeach, IDC_BTN_MOVE_LEFT, DISPID_CLICK, OnClickBtnMoveLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_MOVE_RIGHT, DISPID_CLICK, OnClickBtnMoveRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_STOP_LEFT, DISPID_CLICK, OnClickBtnStopLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_STOP_RIGHT, DISPID_CLICK, OnClickBtnStopRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_SERVO_ON_LEFT, DISPID_CLICK, OnClickBtnServoOnLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_SERVO_ON_RIGHT, DISPID_CLICK, OnClickBtnServoOnRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_SERVO_OFF_LEFT, DISPID_CLICK, OnClickBtnServoOffLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_SERVO_OFF_RIGHT, DISPID_CLICK, OnClickBtnServoOffRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_ALARM_RESET_LEFT, DISPID_CLICK, OnClickBtnAlarmResetLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_ALARM_RESET_RIGHT, DISPID_CLICK, OnClickBtnAlarmResetRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_ORIGIN_LEFT, DISPID_CLICK, OnClickBtnOriginLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_ORIGIN_RIGHT, DISPID_CLICK, OnClickBtnOriginRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_LD_UP, DISPID_CLICK, OnClickBtnLeftLoadUp, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_LD_UP, DISPID_CLICK, OnClickBtnRightLoadUp, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_LD_DOWN, DISPID_CLICK, OnClickBtnLeftLoadDown, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_LD_DOWN, DISPID_CLICK, OnClickBtnRightLoadDown, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_UD_UP, DISPID_CLICK, OnClickBtnLeftUnloadUp, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_UD_UP, DISPID_CLICK, OnClickBtnRightUnloadUp, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_UD_DOWN, DISPID_CLICK, OnClickBtnLeftUnloadDown, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_UD_DOWN, DISPID_CLICK, OnClickBtnRightUnloadDown, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_LD_GRIP, DISPID_CLICK, OnClickBtnLeftLoadGripOnOff, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_LD_GRIP, DISPID_CLICK, OnClickBtnRightLoadGripOnOff, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_LEFT_UD_GRIP, DISPID_CLICK, OnClickBtnLeftUnloadGripOnOff, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_RIGHT_UD_GRIP, DISPID_CLICK, OnClickBtnRightUnloadGripOnOff, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_CURR_POS_LEFT, DISPID_CLICK, OnClickBtnInputCurrPosLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_CURR_POS_RIGHT, DISPID_CLICK, OnClickBtnInputCurrPosRight, VTS_NONE)

	ON_EVENT(CDlgPpTeach, IDC_BTN_JOG_FAST, DISPID_CLICK, OnClickBtnJogFast, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_JOG_KEY_LEFT, DISPID_CLICK, OnClickBtnJogKeyLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_JOG_KEY_RIGHT, DISPID_CLICK, OnClickBtnJogKeyRight, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_SAVE, DISPID_CLICK, OnClickBtnSave, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)

	ON_EVENT(CDlgPpTeach, IDC_STT_TEACH_POS_LEFT, DISPID_CLICK, OnClickSttTeachPosLeft, VTS_NONE)
	ON_EVENT(CDlgPpTeach, IDC_STT_TEACH_POS_RIGHT, DISPID_CLICK, OnClickSttTeachPosRight, VTS_NONE)

	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_WAIT, IDC_BTN_LEFT_WAIT, DISPID_CLICK, OnClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_WAIT, IDC_BTN_RIGHT_WAIT, DISPID_CLICK, OnClickBtnSelectRight, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_CONVEYOR, IDC_BTN_LEFT_CONVEYOR, DISPID_CLICK, OnClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_CONVEYOR, IDC_BTN_RIGHT_CONVEYOR, DISPID_CLICK, OnClickBtnSelectRight, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_BUFFER, IDC_BTN_LEFT_BUFFER, DISPID_CLICK, OnClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_BUFFER, IDC_BTN_RIGHT_BUFFER, DISPID_CLICK, OnClickBtnSelectRight, VTS_I4)

	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_WAIT, IDC_BTN_LEFT_WAIT, DISPID_DBLCLICK, OnDblClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_WAIT, IDC_BTN_RIGHT_WAIT, DISPID_DBLCLICK, OnDblClickBtnSelectRight, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_CONVEYOR, IDC_BTN_LEFT_CONVEYOR, DISPID_DBLCLICK, OnDblClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_CONVEYOR, IDC_BTN_RIGHT_CONVEYOR, DISPID_DBLCLICK, OnDblClickBtnSelectRight, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_LEFT_BUFFER, IDC_BTN_LEFT_BUFFER, DISPID_DBLCLICK, OnDblClickBtnSelectLeft, VTS_I4)
	ON_EVENT_RANGE(CDlgPpTeach, IDC_BTN_RIGHT_BUFFER, IDC_BTN_RIGHT_BUFFER, DISPID_DBLCLICK, OnDblClickBtnSelectRight, VTS_I4)
END_EVENTSINK_MAP()

CDlgPpTeach::CDlgPpTeach(CWnd* pParent) : CDialog(CDlgPpTeach::IDD, pParent)
{
}

BOOL CDlgPpTeach::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgPpTeach::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgPpTeach::PreTranslateMessage(MSG* pMsg)
{
	if( (pMsg->message == WM_KEYDOWN) ||
		(pMsg->message == WM_KEYUP) )
	{
		if( (pMsg->wParam == VK_ESCAPE) ||
			(pMsg->wParam == VK_RETURN) ||
			(pMsg->wParam == VK_SPACE) ) return 0;
	}

	long lJogSpeed = 0;

	switch (pMsg->message)
	{
	case WM_LBUTTONDOWN:
		if( m_pMainFrm->m_pMaster->GetState() != MS_AUTO )
		{
			if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_LEFT)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftPpStn->m_dSlowSpdX * m_pLeftPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_PP, lJogSpeed, DIR_INC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_RIGHT)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pRightPpStn->m_dSlowSpdX * m_pRightPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_RIGHT, DEF_AXIS_PP, lJogSpeed, DIR_INC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_LEFT)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftPpStn->m_dSlowSpdX * m_pLeftPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_PP, lJogSpeed, DIR_DEC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_RIGHT)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pRightPpStn->m_dSlowSpdX * m_pRightPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_RIGHT, DEF_AXIS_PP, lJogSpeed, DIR_DEC);
			}
		}
		break;

	case WM_LBUTTONUP:
		if( m_pMainFrm->m_pMaster->GetState() != MS_AUTO )
		{
			if( (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_LEFT)->GetSafeHwnd()) ||
				(pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_LEFT)->GetSafeHwnd()) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_PP);
			}
			else if( (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_RIGHT)->GetSafeHwnd()) ||
					 (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_RIGHT)->GetSafeHwnd()) )
			{
				FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_PP);
			}
		}
		break;

	case WM_KEYDOWN:
		if( m_pMainFrm->m_pMaster->GetState() != MS_AUTO )
		{
			if( pMsg->wParam == VK_LEFT )
			{
				if( m_bJogKeyLeft && !m_bJogKeyRight )
				{
					lJogSpeed = (long)(m_pLeftPpStn->m_dSlowSpdX * m_pLeftPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
					FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_PP, lJogSpeed, DIR_DEC);
				}

				if( !m_bJogKeyLeft && m_bJogKeyRight )
				{
					lJogSpeed = (long)(m_pRightPpStn->m_dSlowSpdX * m_pRightPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
					FAS_MoveVelocity(DEF_FAS_RIGHT, DEF_AXIS_PP, lJogSpeed, DIR_INC);
				}
			}
			else if( pMsg->wParam == VK_RIGHT )
			{
				if( m_bJogKeyLeft && !m_bJogKeyRight )
				{
					lJogSpeed = (long)(m_pLeftPpStn->m_dSlowSpdX * m_pLeftPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
					FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_PP, lJogSpeed, DIR_INC);
				}

				if( !m_bJogKeyLeft && m_bJogKeyRight )
				{
					lJogSpeed = (long)(m_pRightPpStn->m_dSlowSpdX * m_pRightPpStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
					FAS_MoveVelocity(DEF_FAS_RIGHT, DEF_AXIS_PP, lJogSpeed, DIR_DEC);
				}
			}
		}
		break;

	case WM_KEYUP:
		if( m_pMainFrm->m_pMaster->GetState() != MS_AUTO )
		{
			if( (pMsg->wParam == VK_LEFT) || (pMsg->wParam == VK_RIGHT) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_PP);
				FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_PP);
			}
		}
		break;
	}

	return CDialog::PreTranslateMessage(pMsg);
}

BOOL CDlgPpTeach::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	m_pLeftPpStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pRightPpStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));

	m_nResIDLeft[0] = IDC_BTN_LEFT_WAIT;
	m_nResIDLeft[1] = IDC_BTN_LEFT_CONVEYOR;
	m_nResIDLeft[2] = IDC_BTN_LEFT_BUFFER;

	m_nResIDRight[0] = IDC_BTN_RIGHT_WAIT;
	m_nResIDRight[1] = IDC_BTN_RIGHT_CONVEYOR;
	m_nResIDRight[2] = IDC_BTN_RIGHT_BUFFER;

	m_bJogFast = FALSE;
	m_bJogKeyLeft = FALSE;
	m_bJogKeyRight = FALSE;

	m_nSelectedLeft = 0;
	m_nLastSelectedLeft = 0;

	m_nSelectedRight = 0;
	m_nLastSelectedRight = 0;

	m_dPosLeft = 0.0;
	m_dPosRight = 0.0;

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgPpTeach::OnClose() 
{
	KillTimer(0);

	DestroyWindow();
}

void CDlgPpTeach::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		KillTimer(0);

		UpdateCurrPos();
		UpdateAxisStatus();
		UpdateIOStatus();

		SetTimer(0, 100, NULL);
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgPpTeach::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgPpTeach::OnClickBtnMoveLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;
	if( chkDoorSensorLeft() ) return;

	if( !m_pLeftPpStn->moveX(m_dPosLeft) )
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Left X Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgPpTeach::OnClickBtnMoveRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;
	if( chkDoorSensorRight() ) return;

	if( !m_pRightPpStn->moveX(m_dPosRight) )
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Right X Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgPpTeach::OnClickBtnStopLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnStopRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnServoOnLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_PP, TRUE);
}

void CDlgPpTeach::OnClickBtnServoOnRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, TRUE);
}

void CDlgPpTeach::OnClickBtnServoOffLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_PP, FALSE);
}

void CDlgPpTeach::OnClickBtnServoOffRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, FALSE);
}

void CDlgPpTeach::OnClickBtnAlarmResetLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnAlarmResetRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnOriginLeft()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	EZISERVO_AXISSTATUS stAxisStatus;
	while( FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_Z, &stAxisStatus.dwValue) != FMM_OK ) Sleep(10);
	if( !stAxisStatus.FFLAG_ORIGINRETOK )
	{
		KillTimer(0);
		AfxMessageBox(_T("Can Not Excute Origin! Origin Z Axis First."));
		SetTimer(0, 100, NULL);
		return;
	}

	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	long lCurX = 0;
	long lCurY = 0;
	long lCurZ = 0;
	double dCurX = 0.0;
	double dCurY = 0.0;
	double dCurZ = 0.0;

	while( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_X, &lCurX) != FMM_OK ) Sleep(10);
	while( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Y, &lCurY) != FMM_OK ) Sleep(10);
	while( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Z, &lCurZ) != FMM_OK ) Sleep(10);
	dCurX = (double)lCurX / pLeftTRStn->m_dScaleX;
	dCurY = (double)lCurY / pLeftTRStn->m_dScaleY;
	dCurZ = (double)lCurZ / pLeftTRStn->m_dScaleZ;

	if( (dCurX >= (pLeftTRStn->m_locLoad.x - m_pMainFrm->m_pMainStn->m_dInposition)) &&
		(dCurX <= (pLeftTRStn->m_locLoad.x + m_pMainFrm->m_pMainStn->m_dInposition)) )
	{
		if( (dCurY >= (pLeftTRStn->m_locLoad.y - m_pMainFrm->m_pMainStn->m_dInposition)) &&
			(dCurY <= (pLeftTRStn->m_locLoad.y + m_pMainFrm->m_pMainStn->m_dInposition)) )
		{
			if( dCurZ > (m_pMainFrm->m_pMainStn->m_dInterlockPosZ + m_pMainFrm->m_pMainStn->m_dInposition) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Can Not Excute Origin! Move Z Up First."));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	if( (dCurX >= (pLeftTRStn->m_locUnload.x - m_pMainFrm->m_pMainStn->m_dInposition)) &&
		(dCurX <= (pLeftTRStn->m_locUnload.x + m_pMainFrm->m_pMainStn->m_dInposition)) )
	{
		if( (dCurY >= (pLeftTRStn->m_locUnload.y - m_pMainFrm->m_pMainStn->m_dInposition)) &&
			(dCurY <= (pLeftTRStn->m_locUnload.y + m_pMainFrm->m_pMainStn->m_dInposition)) )
		{
			if( dCurZ > (m_pMainFrm->m_pMainStn->m_dInterlockPosZ + m_pMainFrm->m_pMainStn->m_dInposition) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Can Not Excute Origin! Move Z Up First."));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	FAS_MoveOriginSingleAxis(DEF_FAS_LEFT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnOriginRight()
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;

	EZISERVO_AXISSTATUS stAxisStatus;
	while( FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &stAxisStatus.dwValue) != FMM_OK ) Sleep(10);
	if( !stAxisStatus.FFLAG_ORIGINRETOK )
	{
		KillTimer(0);
		AfxMessageBox(_T("Can Not Excute Origin! Origin Z Axis First."));
		SetTimer(0, 100, NULL);
		return;
	}

	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));
	long lCurX = 0;
	long lCurY = 0;
	long lCurZ = 0;
	double dCurX = 0.0;
	double dCurY = 0.0;
	double dCurZ = 0.0;

	while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurX) != FMM_OK ) Sleep(10);
	while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurY) != FMM_OK ) Sleep(10);
	while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurZ) != FMM_OK ) Sleep(10);
	dCurX = (double)lCurX / pRightTRStn->m_dScaleX;
	dCurY = (double)lCurY / pRightTRStn->m_dScaleY;
	dCurZ = (double)lCurZ / pRightTRStn->m_dScaleZ;

	if( (dCurX >= (pRightTRStn->m_locLoad.x - m_pMainFrm->m_pMainStn->m_dInposition)) &&
		(dCurX <= (pRightTRStn->m_locLoad.x + m_pMainFrm->m_pMainStn->m_dInposition)) )
	{
		if( (dCurY >= (pRightTRStn->m_locLoad.y - m_pMainFrm->m_pMainStn->m_dInposition)) &&
			(dCurY <= (pRightTRStn->m_locLoad.y + m_pMainFrm->m_pMainStn->m_dInposition)) )
		{
			if( dCurZ > (m_pMainFrm->m_pMainStn->m_dInterlockPosZ + m_pMainFrm->m_pMainStn->m_dInposition) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Can Not Excute Origin! Move Z Up First."));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	if( (dCurX >= (pRightTRStn->m_locUnload.x - m_pMainFrm->m_pMainStn->m_dInposition)) &&
		(dCurX <= (pRightTRStn->m_locUnload.x + m_pMainFrm->m_pMainStn->m_dInposition)) )
	{
		if( (dCurY >= (pRightTRStn->m_locUnload.y - m_pMainFrm->m_pMainStn->m_dInposition)) &&
			(dCurY <= (pRightTRStn->m_locUnload.y + m_pMainFrm->m_pMainStn->m_dInposition)) )
		{
			if( dCurZ > (m_pMainFrm->m_pMainStn->m_dInterlockPosZ + m_pMainFrm->m_pMainStn->m_dInposition) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Can Not Excute Origin! Move Z Up First."));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_PP);
}

void CDlgPpTeach::OnClickBtnLeftLoadUp()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;
}

void CDlgPpTeach::OnClickBtnRightLoadUp()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;
}

void CDlgPpTeach::OnClickBtnLeftLoadDown()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;
}

void CDlgPpTeach::OnClickBtnRightLoadDown()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;
}

void CDlgPpTeach::OnClickBtnLeftUnloadUp()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;
}

void CDlgPpTeach::OnClickBtnRightUnloadUp()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;
}

void CDlgPpTeach::OnClickBtnLeftUnloadDown()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;
}

void CDlgPpTeach::OnClickBtnRightUnloadDown()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;
}

void CDlgPpTeach::OnClickBtnLeftLoadGripOnOff()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = !pWorld->m_bOutput[DEF_IO_LD_GRIP];
}

void CDlgPpTeach::OnClickBtnRightLoadGripOnOff()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LD_GRIP] = !pWorld->m_bOutput[DEF_IO_LD_GRIP];
}

void CDlgPpTeach::OnClickBtnLeftUnloadGripOnOff()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = !pWorld->m_bOutput[DEF_IO_UD_GRIP];
}

void CDlgPpTeach::OnClickBtnRightUnloadGripOnOff()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UD_GRIP] = !pWorld->m_bOutput[DEF_IO_UD_GRIP];
}

void CDlgPpTeach::OnClickBtnInputCurrPosLeft()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get left X position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPos;
	double dPos;
	
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_PP, &lPos) != FMM_OK ) return;

	dPos = (double)lPos / m_pLeftPpStn->m_dScaleX;

	if( m_nSelectedLeft == SELECT_WAIT ) m_pLeftPpStn->m_locWait.x = dPos;
	else if( m_nSelectedLeft == SELECT_CV ) m_pLeftPpStn->m_locCV.x = dPos;
	else if( m_nSelectedLeft == SELECT_BUFF ) m_pLeftPpStn->m_locBuff.x = dPos;
	else return;

	m_dPosLeft = dPos;
	UpdateTeachPos();
}

void CDlgPpTeach::OnClickBtnInputCurrPosRight()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get right X position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPos;
	double dPos;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_PP, &lPos) != FMM_OK ) return;

	dPos = (double)lPos / m_pRightPpStn->m_dScaleX;

	if( m_nSelectedRight == SELECT_WAIT ) m_pRightPpStn->m_locWait.x = dPos;
	else if( m_nSelectedRight == SELECT_CV ) m_pRightPpStn->m_locCV.x = dPos;
	else if( m_nSelectedRight == SELECT_BUFF ) m_pRightPpStn->m_locBuff.x = dPos;
	else return;

	m_dPosRight = dPos;
	UpdateTeachPos();
}

void CDlgPpTeach::OnClickBtnJogFast()
{
	m_bJogFast = !m_bJogFast;

	UpdateSelect();
}

void CDlgPpTeach::OnClickBtnJogKeyLeft()
{
	m_bJogKeyLeft = !m_bJogKeyLeft;
	if( m_bJogKeyLeft ) m_bJogKeyRight = FALSE;

	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_KEY_LEFT))->SetBackColorInterior(m_bJogKeyLeft ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_KEY_RIGHT))->SetBackColorInterior(m_bJogKeyRight ? COLOR_RED : COLOR_WHITE);
}

void CDlgPpTeach::OnClickBtnJogKeyRight()
{
	m_bJogKeyRight = !m_bJogKeyRight;
	if( m_bJogKeyRight ) m_bJogKeyLeft = FALSE;

	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_KEY_LEFT))->SetBackColorInterior(m_bJogKeyLeft ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_KEY_RIGHT))->SetBackColorInterior(m_bJogKeyRight ? COLOR_RED : COLOR_WHITE);
}

void CDlgPpTeach::OnClickBtnSave()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to save all position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	m_pLeftPpStn->SaveProfile();
	m_pLeftPpStn->SaveRecipe();
	m_pRightPpStn->SaveProfile();
	m_pRightPpStn->SaveRecipe();
}

void CDlgPpTeach::OnClickBtnExit()
{
	m_pLeftPpStn->LoadProfile();
	m_pRightPpStn->LoadProfile();

	OnCancel();
}

void CDlgPpTeach::OnClickSttTeachPosLeft()
{
	if( m_nSelectedLeft == 0 ) return;

	KillTimer(0);

	CDlgDigitPad dlg(m_dPosLeft);

	if( dlg.DoModal() == IDOK )
	{
		m_dPosLeft = dlg.GetData();

		if( m_nSelectedLeft == SELECT_WAIT ) m_pLeftPpStn->m_locWait.x = m_dPosLeft;
		else if( m_nSelectedLeft == SELECT_CV ) m_pLeftPpStn->m_locCV.x = m_dPosLeft;
		else if( m_nSelectedLeft == SELECT_BUFF ) m_pLeftPpStn->m_locBuff.x = m_dPosLeft;

		UpdateTeachPos();
	}

	SetTimer(0, 100, NULL);
}

void CDlgPpTeach::OnClickSttTeachPosRight()
{
	if( m_nSelectedRight == 0 ) return;

	KillTimer(0);

	CDlgDigitPad dlg(m_dPosRight);

	if( dlg.DoModal() == IDOK )
	{
		m_dPosRight = dlg.GetData();

		if( m_nSelectedRight == SELECT_WAIT ) m_pRightPpStn->m_locWait.x = m_dPosRight;
		else if( m_nSelectedRight == SELECT_CV ) m_pRightPpStn->m_locCV.x = m_dPosRight;
		else if( m_nSelectedRight == SELECT_BUFF ) m_pRightPpStn->m_locBuff.x = m_dPosRight;

		UpdateTeachPos();
	}

	SetTimer(0, 100, NULL);
}

void CDlgPpTeach::OnClickBtnSelectLeft(UINT nID)
{
	for( int i = 0; i < 3; i++ )
	{
		if( nID == m_nResIDLeft[i] )
		{
			m_nSelectedLeft = i + 1;
			break;
		}
	}

	if( m_nSelectedLeft == SELECT_WAIT ) m_dPosLeft = m_pLeftPpStn->m_locWait.x;
	else if( m_nSelectedLeft == SELECT_CV ) m_dPosLeft = m_pLeftPpStn->m_locCV.x;
	else if( m_nSelectedLeft == SELECT_BUFF ) m_dPosLeft = m_pLeftPpStn->m_locBuff.x;

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelectedLeft = m_nSelectedLeft;
}

void CDlgPpTeach::OnClickBtnSelectRight(UINT nID)
{
	for( int i = 0; i < 3; i++ )
	{
		if( nID == m_nResIDRight[i] )
		{
			m_nSelectedRight = i + 1;
			break;
		}
	}

	if( m_nSelectedRight == SELECT_WAIT ) m_dPosRight = m_pRightPpStn->m_locWait.x;
	else if( m_nSelectedRight == SELECT_CV ) m_dPosRight = m_pRightPpStn->m_locCV.x;
	else if( m_nSelectedRight == SELECT_BUFF ) m_dPosRight = m_pRightPpStn->m_locBuff.x;

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelectedRight = m_nSelectedRight;
}

void CDlgPpTeach::OnDblClickBtnSelectLeft(UINT nID)
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;
	if( chkDoorSensorLeft() ) return;

	for( int i = 0; i < 3; i++ )
	{
		if( nID == m_nResIDLeft[i] )
		{
			m_nSelectedLeft = i + 1;
			break;
		}
	}

	if( m_nSelectedLeft == SELECT_WAIT ) m_dPosLeft = m_pLeftPpStn->m_locWait.x;
	else if( m_nSelectedLeft == SELECT_CV ) m_dPosLeft = m_pLeftPpStn->m_locCV.x;
	else if( m_nSelectedLeft == SELECT_BUFF ) m_dPosLeft = m_pLeftPpStn->m_locBuff.x;

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelectedLeft = m_nSelectedLeft;

	if( m_pLeftPpStn->ioLoadUp() )
	{
		if( m_pLeftPpStn->ioUnloadUp() )
		{
			if( !m_pLeftPpStn->moveX(m_dPosLeft) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Move Left X Axis Failed!"));
				SetTimer(0, 100, NULL);
			}
		}
	}
}

void CDlgPpTeach::OnDblClickBtnSelectRight(UINT nID)
{
	if( m_pMainFrm->m_pMaster->GetState() == MS_AUTO ) return;
	if( chkDoorSensorRight() ) return;

	for( int i = 0; i < 3; i++ )
	{
		if( nID == m_nResIDRight[i] )
		{
			m_nSelectedRight = i + 1;
			break;
		}
	}

	if( m_nSelectedRight == SELECT_WAIT ) m_dPosRight = m_pRightPpStn->m_locWait.x;
	else if( m_nSelectedRight == SELECT_CV ) m_dPosRight = m_pRightPpStn->m_locCV.x;
	else if( m_nSelectedRight == SELECT_BUFF ) m_dPosRight = m_pRightPpStn->m_locBuff.x;

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelectedRight = m_nSelectedRight;

	if( m_pRightPpStn->ioLoadUp() )
	{
		if( m_pRightPpStn->ioUnloadUp() )
		{
			if( !m_pRightPpStn->moveX(m_dPosRight) )
			{
				KillTimer(0);
				AfxMessageBox(_T("Move Right X Axis Failed!"));
				SetTimer(0, 100, NULL);
			}
		}
	}
}

void CDlgPpTeach::UpdateSelect()
{
	if( (m_nLastSelectedLeft > 0) && (m_nLastSelectedLeft <= 3) )
		((CBtnEnh*)GetDlgItem(m_nResIDLeft[m_nLastSelectedLeft - 1]))->SetBackColorInterior(COLOR_WHITE);

	if( (m_nSelectedLeft > 0) && (m_nSelectedLeft <= 3) )
		((CBtnEnh*)GetDlgItem(m_nResIDLeft[m_nSelectedLeft - 1]))->SetBackColorInterior(COLOR_YELLOW);

	if( (m_nLastSelectedRight > 0) && (m_nLastSelectedRight <= 3) )
		((CBtnEnh*)GetDlgItem(m_nResIDRight[m_nLastSelectedRight - 1]))->SetBackColorInterior(COLOR_WHITE);

	if( (m_nSelectedRight > 0) && (m_nSelectedRight <= 3) )
		((CBtnEnh*)GetDlgItem(m_nResIDRight[m_nSelectedRight - 1]))->SetBackColorInterior(COLOR_YELLOW);

	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_FAST))->SetBackColorInterior(m_bJogFast ? COLOR_YELLOW : COLOR_WHITE);
}

void CDlgPpTeach::UpdateTeachPos()
{
	CString sTemp = _T("");

	sTemp.Format(_T("%.03lf"), m_dPosLeft);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEACH_POS_LEFT))->SetCaption(sTemp);

	sTemp.Format(_T("%.03lf"), m_dPosRight);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEACH_POS_RIGHT))->SetCaption(sTemp);
}

void CDlgPpTeach::UpdateCurrPos()
{
	long lPos;
	double dPos;
	CString sTemp = _T("");

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_PP, &lPos) != FMM_OK ) return;
	dPos = (double)lPos / m_pLeftPpStn->m_dScaleX;
	sTemp.Format(_T("%.03lf"), dPos);
	((CBtnEnh*)GetDlgItem(IDC_STT_CURR_POS_LEFT))->SetCaption(sTemp);

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_PP, &lPos) != FMM_OK ) return;
	dPos = (double)lPos / m_pRightPpStn->m_dScaleX;
	sTemp.Format(_T("%.03lf"), dPos);
	((CBtnEnh*)GetDlgItem(IDC_STT_CURR_POS_RIGHT))->SetCaption(sTemp);
}

void CDlgPpTeach::UpdateAxisStatus()
{
	DWORD dwAxisStatus;
	EZISERVO_AXISSTATUS stAxisStatus;

	//////////////////////////////////////////////////////////////////////////
	// Axis Left
	if( FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_PP, &dwAxisStatus) != FMM_OK ) return;
	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_SERVOON )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_LEFT))->SetBackColorInterior(COLOR_YELLOW);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_LEFT))->SetBackColorInterior(COLOR_WHITE);
	}
	else
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_LEFT))->SetBackColorInterior(COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_LEFT))->SetBackColorInterior(COLOR_YELLOW);
	}

	if( stAxisStatus.FFLAG_ERRORALL ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_LEFT))->SetBackColorInterior(COLOR_RED);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_LEFT))->SetBackColorInterior(COLOR_WHITE);

	if( stAxisStatus.FFLAG_ORIGINRETOK ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_LEFT))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_LEFT))->SetBackColorInterior(COLOR_WHITE);

	//////////////////////////////////////////////////////////////////////////
	// Axis Right
	if( FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwAxisStatus) != FMM_OK ) return;
	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_SERVOON )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_RIGHT))->SetBackColorInterior(COLOR_YELLOW);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_RIGHT))->SetBackColorInterior(COLOR_WHITE);
	}
	else
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_RIGHT))->SetBackColorInterior(COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_RIGHT))->SetBackColorInterior(COLOR_YELLOW);
	}

	if( stAxisStatus.FFLAG_ERRORALL ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_RIGHT))->SetBackColorInterior(COLOR_RED);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_RIGHT))->SetBackColorInterior(COLOR_WHITE);

	if( stAxisStatus.FFLAG_ORIGINRETOK ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_RIGHT))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_RIGHT))->SetBackColorInterior(COLOR_WHITE);
}

void CDlgPpTeach::UpdateIOStatus()
{
	CCommWorld* pWorldLeft = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
	CCommWorld* pWorldRight = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
	ULONGLONG dwInput;

	//////////////////////////////////////////////////////////////////////////
	// Left
	if( FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwInput) != FMM_OK ) return;

	if( dwInput & SERVO_IN_BITMASK_USERIN2 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_UP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_UP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN3 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_DOWN))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_DOWN))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN4 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_UP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_UP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN5 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_DOWN))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_DOWN))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN7 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_GRIP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_GRIP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN8 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_GRIP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_GRIP))->SetBackColorInterior(COLOR_WHITE);

	if( pWorldLeft->m_bOutput[DEF_IO_LD_GRIP] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_GRIP))->SetCaption(_T("Load Grip\r\nLeft ON"));
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_GRIP))->SetCaption(_T("Load Grip\r\nLeft OFF"));

	if( pWorldLeft->m_bOutput[DEF_IO_UD_GRIP] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_GRIP))->SetCaption(_T("Unload Grip\r\nLeft ON"));
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_GRIP))->SetCaption(_T("Unload Grip\r\nLeft OFF"));

	//////////////////////////////////////////////////////////////////////////
	// Right
	if( FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput) != FMM_OK ) return;

	if( dwInput & SERVO_IN_BITMASK_USERIN2 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_UP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_UP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN3 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_DOWN))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_DOWN))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN4 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_UP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_UP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN5 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_DOWN))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_DOWN))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN7 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_GRIP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_GRIP))->SetBackColorInterior(COLOR_WHITE);

	if( dwInput & SERVO_IN_BITMASK_USERIN8 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_GRIP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_GRIP))->SetBackColorInterior(COLOR_WHITE);

	if( pWorldRight->m_bOutput[DEF_IO_LD_GRIP] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_GRIP))->SetCaption(_T("Load Grip\r\nRight ON"));
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_GRIP))->SetCaption(_T("Load Grip\r\nRight OFF"));

	if( pWorldRight->m_bOutput[DEF_IO_UD_GRIP] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_GRIP))->SetCaption(_T("Unload Grip\r\nRight ON"));
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_GRIP))->SetCaption(_T("Unload Grip\r\nRight OFF"));
}

BOOL CDlgPpTeach::chkDoorSensorLeft()
{
	if( m_pMainFrm->m_pMainStn->IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}

BOOL CDlgPpTeach::chkDoorSensorRight()
{
	if( m_pMainFrm->m_pMainStn->IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}
