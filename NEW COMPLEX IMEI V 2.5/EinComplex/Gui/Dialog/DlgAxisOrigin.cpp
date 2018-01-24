#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgAxisOrigin.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgAxisOrigin::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgAxisOrigin, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgAxisOrigin, CDialog)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_LEFT_TR_X, DISPID_CLICK, OnClickBtnLeftTrX, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_LEFT_TR_Y, DISPID_CLICK, OnClickBtnLeftTrY, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_LEFT_TR_Z, DISPID_CLICK, OnClickBtnLeftTrZ, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_LEFT_PP, DISPID_CLICK, OnClickBtnLeftPp, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_RIGHT_TR_X, DISPID_CLICK, OnClickBtnRightTrX, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_RIGHT_TR_Y, DISPID_CLICK, OnClickBtnRightTrY, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_RIGHT_TR_Z, DISPID_CLICK, OnClickBtnRightTrZ, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_RIGHT_PP, DISPID_CLICK, OnClickBtnRightPp, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_SELECT_ALL, DISPID_CLICK, OnClickBtnSelectAll, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_SERVO_ON, DISPID_CLICK, OnClickBtnServoOn, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_SERVO_OFF, DISPID_CLICK, OnClickBtnServoOff, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_ALARM_RESET, DISPID_CLICK, OnClickBtnAlarmReset, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_ORIGIN, DISPID_CLICK, OnClickBtnOrigin, VTS_NONE)
	ON_EVENT(CDlgAxisOrigin, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgAxisOrigin::CDlgAxisOrigin(CWnd* pParent) : CDialog(CDlgAxisOrigin::IDD, pParent)
{
}

BOOL CDlgAxisOrigin::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgAxisOrigin::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgAxisOrigin::PreTranslateMessage(MSG* pMsg)
{
	if( (pMsg->message == WM_KEYDOWN) ||
		(pMsg->message == WM_KEYUP) )
	{
		if( (pMsg->wParam == VK_ESCAPE) ||
			(pMsg->wParam == VK_RETURN) ||
			(pMsg->wParam == VK_SPACE) ) return 0;
	}

	return CDialog::PreTranslateMessage(pMsg);
}

BOOL CDlgAxisOrigin::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_bSelLeftX = FALSE;
	m_bSelLeftY = FALSE;
	m_bSelLeftZ = FALSE;
	m_bSelLeftPP = FALSE;
	m_bSelRightX = FALSE;
	m_bSelRightY = FALSE;
	m_bSelRightZ = FALSE;
	m_bSelRightPP = FALSE;

	UpdateSelect();

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgAxisOrigin::OnClose() 
{
	KillTimer(0);

	DestroyWindow();
}

void CDlgAxisOrigin::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		KillTimer(0);

		UpdateStatus();

		SetTimer(0, 100, NULL);
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgAxisOrigin::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgAxisOrigin::OnClickBtnLeftTrX()
{
	m_bSelLeftX = !m_bSelLeftX;
	m_bSelLeftZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnLeftTrY()
{
	m_bSelLeftY = !m_bSelLeftY;
	m_bSelLeftZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnLeftTrZ()
{
	m_bSelLeftZ = !m_bSelLeftZ;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnLeftPp()
{
	m_bSelLeftPP = !m_bSelLeftPP;
	m_bSelLeftZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnRightTrX()
{
	m_bSelRightX = !m_bSelRightX;
	m_bSelRightZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnRightTrY()
{
	m_bSelRightY = !m_bSelRightY;
	m_bSelRightZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnRightTrZ()
{
	m_bSelRightZ = !m_bSelRightZ;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnRightPp()
{
	m_bSelRightPP = !m_bSelRightPP;
	m_bSelRightZ = TRUE;
	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnSelectAll()
{
	if( (m_bSelLeftX == TRUE) &&
		(m_bSelLeftY == TRUE) &&
		(m_bSelLeftZ == TRUE) &&
		(m_bSelLeftPP == TRUE) &&
		(m_bSelRightX == TRUE) &&
		(m_bSelRightY == TRUE) &&
		(m_bSelRightZ == TRUE) &&
		(m_bSelRightPP == TRUE) )
	{
		m_bSelLeftX = FALSE;
		m_bSelLeftY = FALSE;
		m_bSelLeftZ = FALSE;
		m_bSelLeftPP = FALSE;
		m_bSelRightX = FALSE;
		m_bSelRightY = FALSE;
		m_bSelRightZ = FALSE;
		m_bSelRightPP = FALSE;
	}
	else
	{
		m_bSelLeftX = TRUE;
		m_bSelLeftY = TRUE;
		m_bSelLeftZ = TRUE;
		m_bSelLeftPP = TRUE;
		m_bSelRightX = TRUE;
		m_bSelRightY = TRUE;
		m_bSelRightZ = TRUE;
		m_bSelRightPP = TRUE;
	}

	UpdateSelect();
}

void CDlgAxisOrigin::OnClickBtnServoOn()
{
	if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) return;

	if( m_bSelLeftX ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_X, TRUE);
	if( m_bSelLeftY ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Y, TRUE);
	if( m_bSelLeftZ ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Z, TRUE);
	if( m_bSelLeftPP ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_PP, TRUE);
	if( m_bSelRightX ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, TRUE);
	if( m_bSelRightY ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, TRUE);
	if( m_bSelRightZ ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Z, TRUE);
	if( m_bSelRightPP ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, TRUE);
}

void CDlgAxisOrigin::OnClickBtnServoOff()
{
	if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) return;

	if( m_bSelLeftX ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_X, FALSE);
	if( m_bSelLeftY ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Y, FALSE);
	if( m_bSelLeftZ ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Z, FALSE);
	if( m_bSelLeftPP ) FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_PP, FALSE);
	if( m_bSelRightX ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, FALSE);
	if( m_bSelRightY ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, FALSE);
	if( m_bSelRightZ ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Z, FALSE);
	if( m_bSelRightPP ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_PP, FALSE);
}

void CDlgAxisOrigin::OnClickBtnAlarmReset()
{
	if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) return;

	if( m_bSelLeftX ) FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_X);
	if( m_bSelLeftY ) FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_Y);
	if( m_bSelLeftZ ) FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_Z);
	if( m_bSelLeftPP ) FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_PP);
	if( m_bSelRightX ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_X);
	if( m_bSelRightY ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Y);
	if( m_bSelRightZ ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Z);
	if( m_bSelRightPP ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_PP);
}

void CDlgAxisOrigin::OnClickBtnOrigin()
{
	if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) return;

	CLeftPPStation* pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	CRightPPStation* pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));

	if( m_bSelLeftZ )
	{
		if( !pLeftTRStn->moveOriginZ() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Left Z Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}

	if( m_bSelRightZ )
	{
		if( !pRightTRStn->moveOriginZ() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Right Z Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}

	if( m_bSelLeftX && m_bSelLeftY )
	{
		if( !pLeftTRStn->moveOriginXY() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Left XY Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}
	else
	{
		if( m_bSelLeftX )
		{
			if( !pLeftTRStn->moveOriginX() )
			{
				KillTimer(0);
				AfxMessageBox(_T("Origin Left X Axis Failed!"));
				SetTimer(0, 100, NULL);
				return;
			}
		}

		if( m_bSelLeftY )
		{
			if( !pLeftTRStn->moveOriginY() )
			{
				KillTimer(0);
				AfxMessageBox(_T("Origin Left Y Axis Failed!"));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	if( m_bSelRightX && m_bSelRightY )
	{
		if( !pRightTRStn->moveOriginXY() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Right XY Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}
	else
	{
		if( m_bSelRightX )
		{
			if( !pRightTRStn->moveOriginX() )
			{
				KillTimer(0);
				AfxMessageBox(_T("Origin Right X Axis Failed!"));
				SetTimer(0, 100, NULL);
				return;
			}
		}

		if( m_bSelRightY )
		{
			if( !pRightTRStn->moveOriginY() )
			{
				KillTimer(0);
				AfxMessageBox(_T("Origin Right Y Axis Failed!"));
				SetTimer(0, 100, NULL);
				return;
			}
		}
	}

	if( m_bSelLeftPP )
	{
		if( !pLeftPPStn->moveOriginX() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Left PP Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}

	if( m_bSelRightPP )
	{
		if( !pRightPPStn->moveOriginX() )
		{
			KillTimer(0);
			AfxMessageBox(_T("Origin Right PP Axis Failed!"));
			SetTimer(0, 100, NULL);
			return;
		}
	}

	KillTimer(0);
	AfxMessageBox(_T("Origin Completed."));
	SetTimer(0, 100, NULL);
}

void CDlgAxisOrigin::OnClickBtnExit()
{
	OnCancel();
}

void CDlgAxisOrigin::UpdateStatus()
{
	EZISERVO_AXISSTATUS stAxisStatusLX, stAxisStatusLY, stAxisStatusLZ, stAxisStatusLP;
	EZISERVO_AXISSTATUS stAxisStatusRX, stAxisStatusRY, stAxisStatusRZ, stAxisStatusRP;

	FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_X, &stAxisStatusLX.dwValue);
	FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_Y, &stAxisStatusLY.dwValue);
	FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_Z, &stAxisStatusLZ.dwValue);
	FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_PP, &stAxisStatusLP.dwValue);
	FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &stAxisStatusRX.dwValue);
	FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &stAxisStatusRY.dwValue);
	FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &stAxisStatusRZ.dwValue);
	FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_PP, &stAxisStatusRP.dwValue);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_LEFT_X))->SetBackColorInterior(stAxisStatusLX.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_LEFT_X))->SetBackColorInterior(stAxisStatusLX.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_LEFT_X))->SetBackColorInterior(stAxisStatusLX.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_LEFT_Y))->SetBackColorInterior(stAxisStatusLY.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_LEFT_Y))->SetBackColorInterior(stAxisStatusLY.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_LEFT_Y))->SetBackColorInterior(stAxisStatusLY.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_LEFT_Z))->SetBackColorInterior(stAxisStatusLZ.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_LEFT_Z))->SetBackColorInterior(stAxisStatusLZ.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_LEFT_Z))->SetBackColorInterior(stAxisStatusLZ.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_LEFT_PP))->SetBackColorInterior(stAxisStatusLP.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_LEFT_PP))->SetBackColorInterior(stAxisStatusLP.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_LEFT_PP))->SetBackColorInterior(stAxisStatusLP.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_RIGHT_X))->SetBackColorInterior(stAxisStatusRX.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_RIGHT_X))->SetBackColorInterior(stAxisStatusRX.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_RIGHT_X))->SetBackColorInterior(stAxisStatusRX.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_RIGHT_Y))->SetBackColorInterior(stAxisStatusRY.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_RIGHT_Y))->SetBackColorInterior(stAxisStatusRY.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_RIGHT_Y))->SetBackColorInterior(stAxisStatusRY.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_RIGHT_Z))->SetBackColorInterior(stAxisStatusRZ.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_RIGHT_Z))->SetBackColorInterior(stAxisStatusRZ.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_RIGHT_Z))->SetBackColorInterior(stAxisStatusRZ.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);

	((CBtnEnh*)GetDlgItem(IDC_STT_SERVO_RIGHT_PP))->SetBackColorInterior(stAxisStatusRP.FFLAG_SERVOON ? COLOR_GREEN : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ALARM_RIGHT_PP))->SetBackColorInterior(stAxisStatusRP.FFLAG_ERRORALL ? COLOR_RED : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_STT_ORG_RIGHT_PP))->SetBackColorInterior(stAxisStatusRP.FFLAG_ORIGINRETOK ? COLOR_GREEN : COLOR_WHITE);
}

void CDlgAxisOrigin::UpdateSelect()
{
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_TR_X))->SetBackColorInterior(m_bSelLeftX ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_TR_Y))->SetBackColorInterior(m_bSelLeftY ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_TR_Z))->SetBackColorInterior(m_bSelLeftZ ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_PP))->SetBackColorInterior(m_bSelLeftPP ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_TR_X))->SetBackColorInterior(m_bSelRightX ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_TR_Y))->SetBackColorInterior(m_bSelRightY ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_TR_Z))->SetBackColorInterior(m_bSelRightZ ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_PP))->SetBackColorInterior(m_bSelRightPP ? COLOR_YELLOW : COLOR_WHITE);
}
