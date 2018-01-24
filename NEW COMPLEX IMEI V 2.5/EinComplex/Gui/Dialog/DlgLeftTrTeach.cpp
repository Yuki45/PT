#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgLeftTrTeach.h"
#include "LeftTransferStation.h"
#include "DlgDigitPad.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgLeftTrTeach::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgLeftTrTeach, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgLeftTrTeach, CDialog)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_MOVE_X, DISPID_CLICK, OnClickBtnMoveX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_MOVE_Y, DISPID_CLICK, OnClickBtnMoveY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_MOVE_Z, DISPID_CLICK, OnClickBtnMoveZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_STOP_X, DISPID_CLICK, OnClickBtnStopX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_STOP_Y, DISPID_CLICK, OnClickBtnStopY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_STOP_Z, DISPID_CLICK, OnClickBtnStopZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_ON_X, DISPID_CLICK, OnClickBtnServoOnX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_ON_Y, DISPID_CLICK, OnClickBtnServoOnY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_ON_Z, DISPID_CLICK, OnClickBtnServoOnZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_OFF_X, DISPID_CLICK, OnClickBtnServoOffX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_OFF_Y, DISPID_CLICK, OnClickBtnServoOffY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SERVO_OFF_Z, DISPID_CLICK, OnClickBtnServoOffZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ALARM_RESET_X, DISPID_CLICK, OnClickBtnAlarmResetX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ALARM_RESET_Y, DISPID_CLICK, OnClickBtnAlarmResetY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ALARM_RESET_Z, DISPID_CLICK, OnClickBtnAlarmResetZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ORIGIN_X, DISPID_CLICK, OnClickBtnOriginX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ORIGIN_Y, DISPID_CLICK, OnClickBtnOriginY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ORIGIN_Z, DISPID_CLICK, OnClickBtnOriginZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_GRIP, DISPID_CLICK, OnClickBtnGripOnOff, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_PACK, DISPID_CLICK, OnClickBtnPackInOut, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_CURR_POS_X, DISPID_CLICK, OnClickBtnInputCurrPosX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_CURR_POS_Y, DISPID_CLICK, OnClickBtnInputCurrPosY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_CURR_POS_Z, DISPID_CLICK, OnClickBtnInputCurrPosZ, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_CURR_POS_XY, DISPID_CLICK, OnClickBtnInputCurrPosXY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_CURR_POS, DISPID_CLICK, OnClickBtnInputCurrPos, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_ALL_DATA, DISPID_CLICK, OnClickBtnAllData, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_XY_MOVE, DISPID_CLICK, OnClickBtnXYMove, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_Z_UP, DISPID_CLICK, OnClickBtnZUp, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_JOG_FAST, DISPID_CLICK, OnClickBtnJogFast, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_JOG_KEY, DISPID_CLICK, OnClickBtnJogKey, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_SAVE, DISPID_CLICK, OnClickBtnSave, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)

	ON_EVENT(CDlgLeftTrTeach, IDC_STT_TEACH_POS_X, DISPID_CLICK, OnClickSttTeachPosX, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_STT_TEACH_POS_Y, DISPID_CLICK, OnClickSttTeachPosY, VTS_NONE)
	ON_EVENT(CDlgLeftTrTeach, IDC_STT_TEACH_POS_Z, DISPID_CLICK, OnClickSttTeachPosZ, VTS_NONE)

	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_1, IDC_BTN_JIG_1, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_2, IDC_BTN_JIG_2, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_3, IDC_BTN_JIG_3, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_4, IDC_BTN_JIG_4, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_5, IDC_BTN_JIG_5, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_6, IDC_BTN_JIG_6, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_7, IDC_BTN_JIG_7, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_8, IDC_BTN_JIG_8, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_9, IDC_BTN_JIG_9, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_10, IDC_BTN_JIG_10, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_11, IDC_BTN_JIG_11, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_12, IDC_BTN_JIG_12, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_13, IDC_BTN_JIG_13, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_14, IDC_BTN_JIG_14, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_15, IDC_BTN_JIG_15, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_16, IDC_BTN_JIG_16, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_17, IDC_BTN_JIG_17, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_18, IDC_BTN_JIG_18, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_19, IDC_BTN_JIG_19, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_20, IDC_BTN_JIG_20, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_21, IDC_BTN_JIG_21, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_22, IDC_BTN_JIG_22, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_23, IDC_BTN_JIG_23, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_24, IDC_BTN_JIG_24, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
#ifndef DEF_EIN_48_LCA
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_25, IDC_BTN_JIG_25, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_26, IDC_BTN_JIG_26, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_27, IDC_BTN_JIG_27, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_28, IDC_BTN_JIG_28, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
#endif
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_LD_BUFF, IDC_BTN_LD_BUFF, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_UD_BUFF, IDC_BTN_UD_BUFF, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_BCR, IDC_BTN_BCR, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_NG, IDC_BTN_NG, DISPID_CLICK, OnClickBtnSelect, VTS_I4)

	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_1, IDC_BTN_JIG_1, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_2, IDC_BTN_JIG_2, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_3, IDC_BTN_JIG_3, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_4, IDC_BTN_JIG_4, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_5, IDC_BTN_JIG_5, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_6, IDC_BTN_JIG_6, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_7, IDC_BTN_JIG_7, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_8, IDC_BTN_JIG_8, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_9, IDC_BTN_JIG_9, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_10, IDC_BTN_JIG_10, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_11, IDC_BTN_JIG_11, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_12, IDC_BTN_JIG_12, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_13, IDC_BTN_JIG_13, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_14, IDC_BTN_JIG_14, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_15, IDC_BTN_JIG_15, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_16, IDC_BTN_JIG_16, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_17, IDC_BTN_JIG_17, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_18, IDC_BTN_JIG_18, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_19, IDC_BTN_JIG_19, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_20, IDC_BTN_JIG_20, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_21, IDC_BTN_JIG_21, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_22, IDC_BTN_JIG_22, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_23, IDC_BTN_JIG_23, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_24, IDC_BTN_JIG_24, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
#ifndef DEF_EIN_48_LCA
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_25, IDC_BTN_JIG_25, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_26, IDC_BTN_JIG_26, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_27, IDC_BTN_JIG_27, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_JIG_28, IDC_BTN_JIG_28, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
#endif
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_LD_BUFF, IDC_BTN_LD_BUFF, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_UD_BUFF, IDC_BTN_UD_BUFF, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_BCR, IDC_BTN_BCR, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgLeftTrTeach, IDC_BTN_NG, IDC_BTN_NG, DISPID_DBLCLICK, OnDblClickBtnSelect, VTS_I4)
END_EVENTSINK_MAP()

CDlgLeftTrTeach::CDlgLeftTrTeach(CWnd* pParent) : CDialog(CDlgLeftTrTeach::IDD, pParent)
{
}

BOOL CDlgLeftTrTeach::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgLeftTrTeach::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgLeftTrTeach::PreTranslateMessage(MSG* pMsg)
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
		if( m_pMainFrm->m_pMainStn->m_nStateLeft != MS_AUTO )
		{
			if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_X)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdX * m_pLeftTrStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_X, lJogSpeed, DIR_INC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_X)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdX * m_pLeftTrStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_X, lJogSpeed, DIR_DEC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_Y)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdY * m_pLeftTrStn->m_dScaleY / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Y, lJogSpeed, DIR_INC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_Y)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdY * m_pLeftTrStn->m_dScaleY / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Y, lJogSpeed, DIR_DEC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_Z)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdZ * m_pLeftTrStn->m_dScaleZ / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Z, lJogSpeed, DIR_INC);
			}
			else if( pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_Z)->GetSafeHwnd() )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdZ * m_pLeftTrStn->m_dScaleZ / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Z, lJogSpeed, DIR_DEC);
			}
		}
		break;

	case WM_LBUTTONUP:
		if( m_pMainFrm->m_pMainStn->m_nStateLeft != MS_AUTO )
		{
			if( (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_X)->GetSafeHwnd()) ||
				(pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_X)->GetSafeHwnd()) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_X);
			}
			else if( (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_Y)->GetSafeHwnd()) ||
					 (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_Y)->GetSafeHwnd()) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Y);
			}
			else if( (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_PLUS_Z)->GetSafeHwnd()) ||
					 (pMsg->hwnd == GetDlgItem(IDC_BTN_JOG_MINUS_Z)->GetSafeHwnd()) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Z);
			}
		}
		break;

	case WM_KEYDOWN:
		if( (m_pMainFrm->m_pMainStn->m_nStateLeft != MS_AUTO) && m_bJogKey )
		{
			if( pMsg->wParam == VK_LEFT )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdX * m_pLeftTrStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_X, lJogSpeed, DIR_INC);
			}
			else if( pMsg->wParam == VK_RIGHT )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdX * m_pLeftTrStn->m_dScaleX / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_X, lJogSpeed, DIR_DEC);
			}
			else if( pMsg->wParam == VK_UP )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdY * m_pLeftTrStn->m_dScaleY / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Y, lJogSpeed, DIR_INC);
			}
			else if( pMsg->wParam == VK_DOWN )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdY * m_pLeftTrStn->m_dScaleY / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Y, lJogSpeed, DIR_DEC);
			}
			else if( pMsg->wParam == VK_NEXT )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdZ * m_pLeftTrStn->m_dScaleZ / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Z, lJogSpeed, DIR_INC);
			}
			else if( pMsg->wParam == VK_PRIOR )
			{
				lJogSpeed = (long)(m_pLeftTrStn->m_dSlowSpdZ * m_pLeftTrStn->m_dScaleZ / (m_bJogFast ? 1.0 : 10.0));
				FAS_MoveVelocity(DEF_FAS_LEFT, DEF_AXIS_Z, lJogSpeed, DIR_DEC);
			}
		}
		break;

	case WM_KEYUP:
		if( (m_pMainFrm->m_pMainStn->m_nStateLeft != MS_AUTO) && m_bJogKey )
		{
			if( (pMsg->wParam == VK_LEFT) || (pMsg->wParam == VK_RIGHT) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_X);
			}
			else if( (pMsg->wParam == VK_UP) || (pMsg->wParam == VK_DOWN) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Y);
			}
			else if( (pMsg->wParam == VK_PRIOR) || (pMsg->wParam == VK_NEXT) )
			{
				FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Z);
			}
		}
		break;
	}

	return CDialog::PreTranslateMessage(pMsg);
}

BOOL CDlgLeftTrTeach::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	m_pLeftTrStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));

	m_nResID[0] = IDC_BTN_JIG_1;
	m_nResID[1] = IDC_BTN_JIG_2;
	m_nResID[2] = IDC_BTN_JIG_3;
	m_nResID[3] = IDC_BTN_JIG_4;
	m_nResID[4] = IDC_BTN_JIG_5;
	m_nResID[5] = IDC_BTN_JIG_6;
	m_nResID[6] = IDC_BTN_JIG_7;
	m_nResID[7] = IDC_BTN_JIG_8;
	m_nResID[8] = IDC_BTN_JIG_9;
	m_nResID[9] = IDC_BTN_JIG_10;
	m_nResID[10] = IDC_BTN_JIG_11;
	m_nResID[11] = IDC_BTN_JIG_12;
	m_nResID[12] = IDC_BTN_JIG_13;
	m_nResID[13] = IDC_BTN_JIG_14;
	m_nResID[14] = IDC_BTN_JIG_15;
	m_nResID[15] = IDC_BTN_JIG_16;
	m_nResID[16] = IDC_BTN_JIG_17;
	m_nResID[17] = IDC_BTN_JIG_18;
	m_nResID[18] = IDC_BTN_JIG_19;
	m_nResID[19] = IDC_BTN_JIG_20;
	m_nResID[20] = IDC_BTN_JIG_21;
	m_nResID[21] = IDC_BTN_JIG_22;
	m_nResID[22] = IDC_BTN_JIG_23;
	m_nResID[23] = IDC_BTN_JIG_24;
#ifndef DEF_EIN_48_LCA
	m_nResID[24] = IDC_BTN_JIG_25;
	m_nResID[25] = IDC_BTN_JIG_26;
	m_nResID[26] = IDC_BTN_JIG_27;
	m_nResID[27] = IDC_BTN_JIG_28;
#endif
	m_nResID[DEF_MAX_JIG_ONE_SIDE] = IDC_BTN_LD_BUFF;
	m_nResID[DEF_MAX_JIG_ONE_SIDE + 1] = IDC_BTN_UD_BUFF;
	m_nResID[DEF_MAX_JIG_ONE_SIDE + 2] = IDC_BTN_BCR;
	m_nResID[DEF_MAX_JIG_ONE_SIDE + 3] = IDC_BTN_NG;

	m_bJogFast = FALSE;
	m_bJogKey = FALSE;

	m_nSelected = 0;
	m_nLastSelected = 0;

	m_dPosX = 0.0;
	m_dPosY = 0.0;
	m_dPosZ = 0.0;

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgLeftTrTeach::OnClose() 
{
	KillTimer(0);

	DestroyWindow();
}

void CDlgLeftTrTeach::OnTimer(UINT nIDEvent) 
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

BOOL CDlgLeftTrTeach::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgLeftTrTeach::OnClickBtnMoveX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	if( m_pLeftTrStn->moveZ(0.0) )
	{
		if( !m_pLeftTrStn->moveX(m_dPosX) )
		{
			KillTimer(0);
			AfxMessageBox(_T("Move X Axis Failed!"));
			SetTimer(0, 100, NULL);
		}
	}
	else
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Z Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgLeftTrTeach::OnClickBtnMoveY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	if( m_pLeftTrStn->moveZ(0.0) )
	{
		if( !m_pLeftTrStn->moveY(m_dPosY) )
		{
			KillTimer(0);
			AfxMessageBox(_T("Move Y Axis Failed!"));
			SetTimer(0, 100, NULL);
		}
	}
	else
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Z Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgLeftTrTeach::OnClickBtnMoveZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	if( !m_pLeftTrStn->moveZ(m_dPosZ) )
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Z Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgLeftTrTeach::OnClickBtnStopX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_X);
}

void CDlgLeftTrTeach::OnClickBtnStopY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Y);
}

void CDlgLeftTrTeach::OnClickBtnStopZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveStop(DEF_FAS_LEFT, DEF_AXIS_Z);
}

void CDlgLeftTrTeach::OnClickBtnServoOnX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_X, TRUE);
}

void CDlgLeftTrTeach::OnClickBtnServoOnY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Y, TRUE);
}

void CDlgLeftTrTeach::OnClickBtnServoOnZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Z, TRUE);
}

void CDlgLeftTrTeach::OnClickBtnServoOffX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_X, FALSE);
}

void CDlgLeftTrTeach::OnClickBtnServoOffY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Y, FALSE);
}

void CDlgLeftTrTeach::OnClickBtnServoOffZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoEnable(DEF_FAS_LEFT, DEF_AXIS_Z, FALSE);
}

void CDlgLeftTrTeach::OnClickBtnAlarmResetX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_X);
}

void CDlgLeftTrTeach::OnClickBtnAlarmResetY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_Y);
}

void CDlgLeftTrTeach::OnClickBtnAlarmResetZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_ServoAlarmReset(DEF_FAS_LEFT, DEF_AXIS_Z);
}

void CDlgLeftTrTeach::OnClickBtnOriginX()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveOriginSingleAxis(DEF_FAS_LEFT, DEF_AXIS_X);
}

void CDlgLeftTrTeach::OnClickBtnOriginY()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveOriginSingleAxis(DEF_FAS_LEFT, DEF_AXIS_Y);
}

void CDlgLeftTrTeach::OnClickBtnOriginZ()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;

	FAS_MoveOriginSingleAxis(DEF_FAS_LEFT, DEF_AXIS_Z);
}

void CDlgLeftTrTeach::OnClickBtnGripOnOff()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_TR_GRIP] = !pWorld->m_bOutput[DEF_IO_TR_GRIP];
}

void CDlgLeftTrTeach::OnClickBtnPackInOut()
{
	DWORD dwOutputX, dwOutputY, dwOutputZ, dwOutputPP;

	DWORD dwMask[9] = {SERVO_OUT_BITMASK_USEROUT0, SERVO_OUT_BITMASK_USEROUT1, SERVO_OUT_BITMASK_USEROUT2,
		SERVO_OUT_BITMASK_USEROUT3, SERVO_OUT_BITMASK_USEROUT4, SERVO_OUT_BITMASK_USEROUT5,
		SERVO_OUT_BITMASK_USEROUT6, SERVO_OUT_BITMASK_USEROUT7, SERVO_OUT_BITMASK_USEROUT8};

	if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, &dwOutputX) != FMM_OK ) return;
	if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwOutputY) != FMM_OK ) return;
	if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwOutputZ) != FMM_OK ) return;
	if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwOutputPP) != FMM_OK ) return;

#ifdef DEF_EIN_48_LCA
	if( (m_nSelected >= 1) && (m_nSelected <= 9) )
	{
		if( dwOutputX & dwMask[m_nSelected - 1] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, dwMask[m_nSelected - 1]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, dwMask[m_nSelected - 1], 0);
	}
	else if( (m_nSelected >= 10) && (m_nSelected <= 18) )
	{
		if( dwOutputY & dwMask[m_nSelected - 10] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, dwMask[m_nSelected - 10]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, dwMask[m_nSelected - 10], 0);
	}
	else if( (m_nSelected >= 19) && (m_nSelected <= 24) )
	{
		if( dwOutputZ & dwMask[m_nSelected - 19] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, dwMask[m_nSelected - 19]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, dwMask[m_nSelected - 19], 0);
	}
#else
	if( (m_nSelected >= 1) && (m_nSelected <= 9) )
	{
		if( dwOutputX & dwMask[m_nSelected - 1] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, 0, dwMask[m_nSelected - 1]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, dwMask[m_nSelected - 1], 0);
	}
	else if( (m_nSelected >= 10) && (m_nSelected <= 18) )
	{
		if( dwOutputY & dwMask[m_nSelected - 10] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, 0, dwMask[m_nSelected - 10]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, dwMask[m_nSelected - 10], 0);
	}
	else if( (m_nSelected >= 19) && (m_nSelected <= 27) )
	{
		if( dwOutputZ & dwMask[m_nSelected - 19] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, 0, dwMask[m_nSelected - 19]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, dwMask[m_nSelected - 19], 0);
	}
	else if( m_nSelected == 28 )
	{
		if( dwOutputPP & dwMask[m_nSelected - 28] ) FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, 0, dwMask[m_nSelected - 28]);
		else FAS_SetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, dwMask[m_nSelected - 28], 0);
	}
#endif
}

void CDlgLeftTrTeach::OnClickBtnInputCurrPosX()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get X position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPosX;
	double dPosX;
	
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_X, &lPosX) != FMM_OK ) return;

	dPosX = (double)lPosX / m_pLeftTrStn->m_dScaleX;

	if( (m_nSelected >= 1)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_pLeftTrStn->m_locJig[m_nSelected - 1].x = dPosX;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_pLeftTrStn->m_locLoad.x = dPosX;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_pLeftTrStn->m_locUnload.x = dPosX;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_pLeftTrStn->m_locBCR.x = dPosX;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_pLeftTrStn->m_locNG.x = dPosX;
	}
	else return;

	m_dPosX = dPosX;
	UpdateTeachPos();
}

void CDlgLeftTrTeach::OnClickBtnInputCurrPosY()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get Y position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPosY;
	double dPosY;

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Y, &lPosY) != FMM_OK ) return;

	dPosY = (double)lPosY / m_pLeftTrStn->m_dScaleY;

	if( (m_nSelected >= 1)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_pLeftTrStn->m_locJig[m_nSelected - 1].y = dPosY;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_pLeftTrStn->m_locLoad.y = dPosY;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_pLeftTrStn->m_locUnload.y = dPosY;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_pLeftTrStn->m_locBCR.y = dPosY;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_pLeftTrStn->m_locNG.y = dPosY;
	}
	else return;

	m_dPosY = dPosY;
	UpdateTeachPos();
}

void CDlgLeftTrTeach::OnClickBtnInputCurrPosZ()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get Z position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPosZ;
	double dPosZ;

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Z, &lPosZ) != FMM_OK ) return;

	dPosZ = (double)lPosZ / m_pLeftTrStn->m_dScaleZ;

	if( (m_nSelected >= 1)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_pLeftTrStn->m_locJig[m_nSelected - 1].z = dPosZ;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_pLeftTrStn->m_locLoad.z = dPosZ;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_pLeftTrStn->m_locUnload.z = dPosZ;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_pLeftTrStn->m_locBCR.z = dPosZ;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_pLeftTrStn->m_locNG.z = dPosZ;
	}
	else return;

	m_dPosZ = dPosZ;
	UpdateTeachPos();
}

void CDlgLeftTrTeach::OnClickBtnInputCurrPosXY()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get XY position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPosX, lPosY;
	double dPosX, dPosY;

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_X, &lPosX) != FMM_OK ) return;
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Y, &lPosY) != FMM_OK ) return;

	dPosX = (double)lPosX / m_pLeftTrStn->m_dScaleX;
	dPosY = (double)lPosY / m_pLeftTrStn->m_dScaleY;

	if( (m_nSelected >= 1)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_pLeftTrStn->m_locJig[m_nSelected - 1].x = dPosX;
		m_pLeftTrStn->m_locJig[m_nSelected - 1].y = dPosY;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_pLeftTrStn->m_locLoad.x = dPosX;
		m_pLeftTrStn->m_locLoad.y = dPosY;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_pLeftTrStn->m_locUnload.x = dPosX;
		m_pLeftTrStn->m_locUnload.y = dPosY;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_pLeftTrStn->m_locBCR.x = dPosX;
		m_pLeftTrStn->m_locBCR.y = dPosY;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_pLeftTrStn->m_locNG.x = dPosX;
		m_pLeftTrStn->m_locNG.y = dPosY;
	}
	else return;

	m_dPosX = dPosX;
	m_dPosY = dPosY;
	UpdateTeachPos();
}

void CDlgLeftTrTeach::OnClickBtnInputCurrPos()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get XYZ position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	long lPosX, lPosY, lPosZ;
	double dPosX, dPosY, dPosZ;

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_X, &lPosX) != FMM_OK ) return;
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Y, &lPosY) != FMM_OK ) return;
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Z, &lPosZ) != FMM_OK ) return;

	dPosX = (double)lPosX / m_pLeftTrStn->m_dScaleX;
	dPosY = (double)lPosY / m_pLeftTrStn->m_dScaleY;
	dPosZ = (double)lPosZ / m_pLeftTrStn->m_dScaleZ;

	if( (m_nSelected >= 1)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_pLeftTrStn->m_locJig[m_nSelected - 1].x = dPosX;
		m_pLeftTrStn->m_locJig[m_nSelected - 1].y = dPosY;
		m_pLeftTrStn->m_locJig[m_nSelected - 1].z = dPosZ;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_pLeftTrStn->m_locLoad.x = dPosX;
		m_pLeftTrStn->m_locLoad.y = dPosY;
		m_pLeftTrStn->m_locLoad.z = dPosZ;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_pLeftTrStn->m_locUnload.x = dPosX;
		m_pLeftTrStn->m_locUnload.y = dPosY;
		m_pLeftTrStn->m_locUnload.z = dPosZ;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_pLeftTrStn->m_locBCR.x = dPosX;
		m_pLeftTrStn->m_locBCR.y = dPosY;
		m_pLeftTrStn->m_locBCR.z = dPosZ;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_pLeftTrStn->m_locNG.x = dPosX;
		m_pLeftTrStn->m_locNG.y = dPosY;
		m_pLeftTrStn->m_locNG.z = dPosZ;
	}
	else return;

	m_dPosX = dPosX;
	m_dPosY = dPosY;
	m_dPosZ = dPosZ;
	UpdateTeachPos();
}

void CDlgLeftTrTeach::OnClickBtnXYMove()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	if( m_pLeftTrStn->moveZ(0.0) )
	{
		if( !m_pLeftTrStn->moveXY(m_dPosX, m_dPosY) )
		{
			KillTimer(0);
			AfxMessageBox(_T("Move XY Axis Failed!"));
			SetTimer(0, 100, NULL);
		}
	}
	else
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Z Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgLeftTrTeach::OnClickBtnZUp()
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	if( !m_pLeftTrStn->moveZ(0.0) )
	{
		KillTimer(0);
		AfxMessageBox(_T("Move Z Axis Failed!"));
		SetTimer(0, 100, NULL);
	}
}

void CDlgLeftTrTeach::OnClickBtnJogFast()
{
	m_bJogFast = !m_bJogFast;

	UpdateSelect();
}

void CDlgLeftTrTeach::OnClickBtnJogKey()
{
	m_bJogKey = !m_bJogKey;

	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_KEY))->SetBackColorInterior(m_bJogKey ? COLOR_RED : COLOR_WHITE);
}

void CDlgLeftTrTeach::OnClickBtnAllData()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to get block teaching data?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

#ifdef DEF_EIN_48_LCA
	//////////////////////////////////////////////////////////////////////////
	// Calculation
	double dUnitX, dUnitY;
	double dPitchXY, dPitchYX, dPitchZX, dPitchZY;

	dUnitX = (m_pLeftTrStn->m_locJig[8].x - m_pLeftTrStn->m_locJig[0].x) / 830.0;
	dUnitY = (m_pLeftTrStn->m_locJig[17].y - m_pLeftTrStn->m_locJig[0].y) / 544.0;
	dPitchXY = (m_pLeftTrStn->m_locJig[17].x - m_pLeftTrStn->m_locJig[0].x) / 544.0;
	dPitchYX = (m_pLeftTrStn->m_locJig[8].y - m_pLeftTrStn->m_locJig[0].y) / 830.0;
	dPitchZX = (m_pLeftTrStn->m_locJig[8].z - m_pLeftTrStn->m_locJig[0].z) / 830.0;
	dPitchZY = (m_pLeftTrStn->m_locJig[17].z - m_pLeftTrStn->m_locJig[0].z) / 544.0;

	//////////////////////////////////////////////////////////////////////////
	// Line #1
	m_pLeftTrStn->m_locJig[1].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[1].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[1].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[2].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[2].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[2].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[3].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 315.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[3].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 315.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[3].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 315.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[4].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 415.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[4].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 415.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[4].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 415.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[5].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 515.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[5].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 515.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[5].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 515.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[6].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 630.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[6].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 630.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[6].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 630.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[7].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 730.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[7].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 730.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[7].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 730.0) + (dPitchZY * 0.0);

	//////////////////////////////////////////////////////////////////////////
	// Line #2
	m_pLeftTrStn->m_locJig[9].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 0.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[9].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 0.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[9].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 0.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[10].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[10].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[10].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[11].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[11].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[11].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[12].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 315.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[12].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 315.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[12].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 315.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[13].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 415.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[13].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 415.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[13].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 415.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[14].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 515.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[14].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 515.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[14].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 515.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[15].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 630.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[15].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 630.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[15].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 630.0) + (dPitchZY * 272.0);

	m_pLeftTrStn->m_locJig[16].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 730.0) + (dPitchXY * 272.0);
	m_pLeftTrStn->m_locJig[16].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 730.0) + (dUnitY * 272.0);
	m_pLeftTrStn->m_locJig[16].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 730.0) + (dPitchZY * 272.0);

	//////////////////////////////////////////////////////////////////////////
	// Line #3
	m_pLeftTrStn->m_locJig[18].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[18].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[18].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 544.0);

	m_pLeftTrStn->m_locJig[19].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[19].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[19].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 544.0);

	m_pLeftTrStn->m_locJig[20].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 300.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[20].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 300.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[20].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 300.0) + (dPitchZY * 544.0);

	m_pLeftTrStn->m_locJig[21].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 400.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[21].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 400.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[21].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 400.0) + (dPitchZY * 544.0);

	m_pLeftTrStn->m_locJig[22].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 500.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[22].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 500.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[22].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 500.0) + (dPitchZY * 544.0);

	m_pLeftTrStn->m_locJig[23].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 600.0) + (dPitchXY * 544.0);
	m_pLeftTrStn->m_locJig[23].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 600.0) + (dUnitY * 544.0);
	m_pLeftTrStn->m_locJig[23].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 600.0) + (dPitchZY * 544.0);
#else
	//////////////////////////////////////////////////////////////////////////
	// Calculation
	double dUnitX, dUnitY;
	double dPitchXY, dPitchYX, dPitchZX, dPitchZY;

	dUnitX = (m_pLeftTrStn->m_locJig[8].x - m_pLeftTrStn->m_locJig[0].x) / 830.0;
	dUnitY = (m_pLeftTrStn->m_locJig[23].y - m_pLeftTrStn->m_locJig[0].y) / 784.0;
	dPitchXY = (m_pLeftTrStn->m_locJig[23].x - m_pLeftTrStn->m_locJig[0].x) / 784.0;
	dPitchYX = (m_pLeftTrStn->m_locJig[8].y - m_pLeftTrStn->m_locJig[0].y) / 830.0;
	dPitchZX = (m_pLeftTrStn->m_locJig[8].z - m_pLeftTrStn->m_locJig[0].z) / 830.0;
	dPitchZY = (m_pLeftTrStn->m_locJig[23].z - m_pLeftTrStn->m_locJig[0].z) / 784.0;

	//////////////////////////////////////////////////////////////////////////
	// Line #1
	m_pLeftTrStn->m_locJig[1].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[1].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[1].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[2].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[2].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[2].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[3].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 315.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[3].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 315.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[3].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 315.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[4].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 415.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[4].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 415.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[4].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 415.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[5].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 515.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[5].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 515.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[5].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 515.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[6].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 630.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[6].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 630.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[6].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 630.0) + (dPitchZY * 0.0);

	m_pLeftTrStn->m_locJig[7].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 730.0) + (dPitchXY * 0.0);
	m_pLeftTrStn->m_locJig[7].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 730.0) + (dUnitY * 0.0);
	m_pLeftTrStn->m_locJig[7].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 730.0) + (dPitchZY * 0.0);

	//////////////////////////////////////////////////////////////////////////
	// Line #2
	m_pLeftTrStn->m_locJig[9].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 0.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[9].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 0.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[9].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 0.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[10].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[10].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[10].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[11].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[11].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[11].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[12].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 315.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[12].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 315.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[12].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 315.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[13].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 415.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[13].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 415.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[13].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 415.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[14].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 515.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[14].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 515.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[14].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 515.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[15].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 630.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[15].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 630.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[15].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 630.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[16].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 730.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[16].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 730.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[16].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 730.0) + (dPitchZY * 260.0);

	m_pLeftTrStn->m_locJig[17].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 830.0) + (dPitchXY * 260.0);
	m_pLeftTrStn->m_locJig[17].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 830.0) + (dUnitY * 260.0);
	m_pLeftTrStn->m_locJig[17].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 830.0) + (dPitchZY * 260.0);

	//////////////////////////////////////////////////////////////////////////
	// Line #3
	m_pLeftTrStn->m_locJig[18].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 0.0) + (dPitchXY * 522.0);
	m_pLeftTrStn->m_locJig[18].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 0.0) + (dUnitY * 522.0);
	m_pLeftTrStn->m_locJig[18].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 0.0) + (dPitchZY * 522.0);

	m_pLeftTrStn->m_locJig[19].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 522.0);
	m_pLeftTrStn->m_locJig[19].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 522.0);
	m_pLeftTrStn->m_locJig[19].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 522.0);

	m_pLeftTrStn->m_locJig[20].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 522.0);
	m_pLeftTrStn->m_locJig[20].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 522.0);
	m_pLeftTrStn->m_locJig[20].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 522.0);

	m_pLeftTrStn->m_locJig[21].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 300.0) + (dPitchXY * 522.0);
	m_pLeftTrStn->m_locJig[21].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 300.0) + (dUnitY * 522.0);
	m_pLeftTrStn->m_locJig[21].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 300.0) + (dPitchZY * 522.0);

	m_pLeftTrStn->m_locJig[22].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 400.0) + (dPitchXY * 522.0);
	m_pLeftTrStn->m_locJig[22].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 400.0) + (dUnitY * 522.0);
	m_pLeftTrStn->m_locJig[22].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 400.0) + (dPitchZY * 522.0);

	//////////////////////////////////////////////////////////////////////////
	// Line #4
	m_pLeftTrStn->m_locJig[24].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 100.0) + (dPitchXY * 784.0);
	m_pLeftTrStn->m_locJig[24].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 100.0) + (dUnitY * 784.0);
	m_pLeftTrStn->m_locJig[24].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 100.0) + (dPitchZY * 784.0);

	m_pLeftTrStn->m_locJig[25].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 200.0) + (dPitchXY * 784.0);
	m_pLeftTrStn->m_locJig[25].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 200.0) + (dUnitY * 784.0);
	m_pLeftTrStn->m_locJig[25].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 200.0) + (dPitchZY * 784.0);

	m_pLeftTrStn->m_locJig[26].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 300.0) + (dPitchXY * 784.0);
	m_pLeftTrStn->m_locJig[26].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 300.0) + (dUnitY * 784.0);
	m_pLeftTrStn->m_locJig[26].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 300.0) + (dPitchZY * 784.0);

	m_pLeftTrStn->m_locJig[27].x = m_pLeftTrStn->m_locJig[0].x + (dUnitX * 400.0) + (dPitchXY * 784.0);
	m_pLeftTrStn->m_locJig[27].y = m_pLeftTrStn->m_locJig[0].y + (dPitchYX * 400.0) + (dUnitY * 784.0);
	m_pLeftTrStn->m_locJig[27].z = m_pLeftTrStn->m_locJig[0].z + (dPitchZX * 400.0) + (dPitchZY * 784.0);
#endif

	//////////////////////////////////////////////////////////////////////////
	// Data Save
	m_nSelected = 0;
	m_dPosX = 0.0;
	m_dPosY = 0.0;
	m_dPosZ = 0.0;
	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelected = m_nSelected;
}

void CDlgLeftTrTeach::OnClickBtnSave()
{
	KillTimer(0);

	if( AfxMessageBox(_T("Are you sure to save all position?"), MB_OKCANCEL) != IDOK )
	{
		SetTimer(0, 100, NULL);
		return;
	}

	SetTimer(0, 100, NULL);

	m_pLeftTrStn->SaveProfile();
	m_pLeftTrStn->SaveRecipe();
}

void CDlgLeftTrTeach::OnClickBtnExit()
{
	m_pLeftTrStn->LoadProfile();

	OnCancel();
}

void CDlgLeftTrTeach::OnClickSttTeachPosX()
{
	if( m_nSelected == 0 ) return;

	KillTimer(0);

	CDlgDigitPad dlg(m_dPosX);

	if( dlg.DoModal() == IDOK )
	{
		m_dPosX = dlg.GetData();

		if( m_nSelected == SELECT_LD_BUFF ) m_pLeftTrStn->m_locLoad.x = m_dPosX;
		else if( m_nSelected == SELECT_UD_BUFF ) m_pLeftTrStn->m_locUnload.x = m_dPosX;
		else if( m_nSelected == SELECT_BCR ) m_pLeftTrStn->m_locBCR.x = m_dPosX;
		else if( m_nSelected == SELECT_NG ) m_pLeftTrStn->m_locNG.x = m_dPosX;
		else
		{
			if( (m_nSelected > 0) && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
			{
				m_pLeftTrStn->m_locJig[m_nSelected - 1].x = m_dPosX;
			}
		}

		UpdateTeachPos();
	}

	SetTimer(0, 100, NULL);
}

void CDlgLeftTrTeach::OnClickSttTeachPosY()
{
	if( m_nSelected == 0 ) return;

	KillTimer(0);

	CDlgDigitPad dlg(m_dPosY);

	if( dlg.DoModal() == IDOK )
	{
		m_dPosY = dlg.GetData();

		if( m_nSelected == SELECT_LD_BUFF ) m_pLeftTrStn->m_locLoad.y = m_dPosY;
		else if( m_nSelected == SELECT_UD_BUFF ) m_pLeftTrStn->m_locUnload.y = m_dPosY;
		else if( m_nSelected == SELECT_BCR ) m_pLeftTrStn->m_locBCR.y = m_dPosY;
		else if( m_nSelected == SELECT_NG ) m_pLeftTrStn->m_locNG.y = m_dPosY;
		else
		{
			if( (m_nSelected > 0) && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
			{
				m_pLeftTrStn->m_locJig[m_nSelected - 1].y = m_dPosY;
			}
		}

		UpdateTeachPos();
	}

	SetTimer(0, 100, NULL);
}

void CDlgLeftTrTeach::OnClickSttTeachPosZ()
{
	if( m_nSelected == 0 ) return;

	KillTimer(0);

	CDlgDigitPad dlg(m_dPosZ);

	if( dlg.DoModal() == IDOK )
	{
		m_dPosZ = dlg.GetData();

		if( m_nSelected == SELECT_LD_BUFF ) m_pLeftTrStn->m_locLoad.z = m_dPosZ;
		else if( m_nSelected == SELECT_UD_BUFF ) m_pLeftTrStn->m_locUnload.z = m_dPosZ;
		else if( m_nSelected == SELECT_BCR ) m_pLeftTrStn->m_locBCR.z = m_dPosZ;
		else if( m_nSelected == SELECT_NG ) m_pLeftTrStn->m_locNG.z = m_dPosZ;
		else
		{
			if( (m_nSelected > 0) && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
			{
				m_pLeftTrStn->m_locJig[m_nSelected - 1].z = m_dPosZ;
			}
		}

		UpdateTeachPos();
	}

	SetTimer(0, 100, NULL);
}

void CDlgLeftTrTeach::OnClickBtnSelect(UINT nID)
{
	for( int i = 0; i < (DEF_MAX_JIG_ONE_SIDE + 4); i++ )
	{
		if( nID == m_nResID[i] )
		{
			m_nSelected = i + 1;
			break;
		}
	}

	if( (m_nSelected > 0)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_dPosX = m_pLeftTrStn->m_locJig[m_nSelected - 1].x;
		m_dPosY = m_pLeftTrStn->m_locJig[m_nSelected - 1].y;
		m_dPosZ = m_pLeftTrStn->m_locJig[m_nSelected - 1].z;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_dPosX = m_pLeftTrStn->m_locLoad.x;
		m_dPosY = m_pLeftTrStn->m_locLoad.y;
		m_dPosZ = m_pLeftTrStn->m_locLoad.z;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_dPosX = m_pLeftTrStn->m_locUnload.x;
		m_dPosY = m_pLeftTrStn->m_locUnload.y;
		m_dPosZ = m_pLeftTrStn->m_locUnload.z;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_dPosX = m_pLeftTrStn->m_locBCR.x;
		m_dPosY = m_pLeftTrStn->m_locBCR.y;
		m_dPosZ = m_pLeftTrStn->m_locBCR.z;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_dPosX = m_pLeftTrStn->m_locNG.x;
		m_dPosY = m_pLeftTrStn->m_locNG.y;
		m_dPosZ = m_pLeftTrStn->m_locNG.z;
	}

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelected = m_nSelected;
}

void CDlgLeftTrTeach::OnDblClickBtnSelect(UINT nID)
{
	if( m_pMainFrm->m_pMainStn->m_nStateLeft == MS_AUTO ) return;
	if( chkDoorSensor() ) return;

	for( int i = 0; i < (DEF_MAX_JIG_ONE_SIDE + 4); i++ )
	{
		if( nID == m_nResID[i] )
		{
			m_nSelected = i + 1;
			break;
		}
	}

	if( (m_nSelected > 0)  && (m_nSelected <= DEF_MAX_JIG_ONE_SIDE) )
	{
		m_dPosX = m_pLeftTrStn->m_locJig[m_nSelected - 1].x;
		m_dPosY = m_pLeftTrStn->m_locJig[m_nSelected - 1].y;
		m_dPosZ = m_pLeftTrStn->m_locJig[m_nSelected - 1].z;
	}
	else if( m_nSelected == SELECT_LD_BUFF )
	{
		m_dPosX = m_pLeftTrStn->m_locLoad.x;
		m_dPosY = m_pLeftTrStn->m_locLoad.y;
		m_dPosZ = m_pLeftTrStn->m_locLoad.z;
	}
	else if( m_nSelected == SELECT_UD_BUFF )
	{
		m_dPosX = m_pLeftTrStn->m_locUnload.x;
		m_dPosY = m_pLeftTrStn->m_locUnload.y;
		m_dPosZ = m_pLeftTrStn->m_locUnload.z;
	}
	else if( m_nSelected == SELECT_BCR )
	{
		m_dPosX = m_pLeftTrStn->m_locBCR.x;
		m_dPosY = m_pLeftTrStn->m_locBCR.y;
		m_dPosZ = m_pLeftTrStn->m_locBCR.z;
	}
	else if( m_nSelected == SELECT_NG )
	{
		m_dPosX = m_pLeftTrStn->m_locNG.x;
		m_dPosY = m_pLeftTrStn->m_locNG.y;
		m_dPosZ = m_pLeftTrStn->m_locNG.z;
	}

	UpdateTeachPos();
	UpdateSelect();
	m_nLastSelected = m_nSelected;

	if( (m_nSelected != SELECT_BCR) &&
		(m_dPosZ > m_pMainFrm->m_pMainStn->m_dSlowDownDist) )
	{
		if( m_pLeftTrStn->moveZ(0.0) )
		{
			if( m_pLeftTrStn->moveXY(m_dPosX, m_dPosY) )
			{
				if( m_pLeftTrStn->moveZ(m_dPosZ - m_pMainFrm->m_pMainStn->m_dSlowDownDist) )
				{
					if( !m_pLeftTrStn->moveZ(m_dPosZ, TRUE) )
					{
						KillTimer(0);
						AfxMessageBox(_T("Move Z Axis Failed!"));
						SetTimer(0, 100, NULL);
					}
				}
				else
				{
					KillTimer(0);
					AfxMessageBox(_T("Move Z Axis Failed!"));
					SetTimer(0, 100, NULL);
				}
			}
			else
			{
				KillTimer(0);
				AfxMessageBox(_T("Move XY Axis Failed!"));
				SetTimer(0, 100, NULL);
			}
		}
		else
		{
			KillTimer(0);
			AfxMessageBox(_T("Move Z Axis Failed!"));
			SetTimer(0, 100, NULL);
		}
	}
	else
	{
		if( m_pLeftTrStn->moveZ(0.0) )
		{
			if( m_pLeftTrStn->moveXY(m_dPosX, m_dPosY) )
			{
				if( !m_pLeftTrStn->moveZ(m_dPosZ) )
				{
					KillTimer(0);
					AfxMessageBox(_T("Move Z Axis Failed!"));
					SetTimer(0, 100, NULL);
				}
			}
			else
			{
				KillTimer(0);
				AfxMessageBox(_T("Move XY Axis Failed!"));
				SetTimer(0, 100, NULL);
			}
		}
		else
		{
			KillTimer(0);
			AfxMessageBox(_T("Move Z Axis Failed!"));
			SetTimer(0, 100, NULL);
		}
	}
}

void CDlgLeftTrTeach::UpdateSelect()
{
	if( (m_nLastSelected > 0) && (m_nLastSelected <= (DEF_MAX_JIG_ONE_SIDE + 4)) )
	{
		if( (m_nLastSelected == 1) ||
			(m_nLastSelected == 9) ||
#ifdef DEF_EIN_48_LCA
			(m_nLastSelected == 18) ||
#else
			(m_nLastSelected == 24) ||
#endif
			(m_nLastSelected == SELECT_LD_BUFF) ||
			(m_nLastSelected == SELECT_UD_BUFF) ||
			(m_nLastSelected == SELECT_BCR) ||
			(m_nLastSelected == SELECT_NG) )
		{
			((CBtnEnh*)GetDlgItem(m_nResID[m_nLastSelected - 1]))->SetBackColorInterior(COLOR_CYAN);
		}
		else
		{
			((CBtnEnh*)GetDlgItem(m_nResID[m_nLastSelected - 1]))->SetBackColorInterior(COLOR_WHITE);
		}
	}

	if( (m_nSelected > 0) && (m_nSelected <= (DEF_MAX_JIG_ONE_SIDE + 4)) )
	{
		((CBtnEnh*)GetDlgItem(m_nResID[m_nSelected - 1]))->SetBackColorInterior(COLOR_YELLOW);
	}

	((CBtnEnh*)GetDlgItem(IDC_BTN_JOG_FAST))->SetBackColorInterior(m_bJogFast ? COLOR_YELLOW : COLOR_WHITE);
}

void CDlgLeftTrTeach::UpdateTeachPos()
{
	CString sTemp = _T("");

	sTemp.Format(_T("%.03lf"), m_dPosX);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEACH_POS_X))->SetCaption(sTemp);

	sTemp.Format(_T("%.03lf"), m_dPosY);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEACH_POS_Y))->SetCaption(sTemp);

	sTemp.Format(_T("%.03lf"), m_dPosZ);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEACH_POS_Z))->SetCaption(sTemp);
}

void CDlgLeftTrTeach::UpdateCurrPos()
{
	long lPosX, lPosY, lPosZ;
	double dPosX, dPosY, dPosZ;
	CString sTemp = _T("");

	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_X, &lPosX) != FMM_OK ) return;
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Y, &lPosY) != FMM_OK ) return;
	if( FAS_GetActualPos(DEF_FAS_LEFT, DEF_AXIS_Z, &lPosZ) != FMM_OK ) return;

	dPosX = (double)lPosX / m_pLeftTrStn->m_dScaleX;
	dPosY = (double)lPosY / m_pLeftTrStn->m_dScaleY;
	dPosZ = (double)lPosZ / m_pLeftTrStn->m_dScaleZ;

	sTemp.Format(_T("%.03lf"), dPosX);
	((CBtnEnh*)GetDlgItem(IDC_STT_CURR_POS_X))->SetCaption(sTemp);

	sTemp.Format(_T("%.03lf"), dPosY);
	((CBtnEnh*)GetDlgItem(IDC_STT_CURR_POS_Y))->SetCaption(sTemp);

	sTemp.Format(_T("%.03lf"), dPosZ);
	((CBtnEnh*)GetDlgItem(IDC_STT_CURR_POS_Z))->SetCaption(sTemp);
}

void CDlgLeftTrTeach::UpdateAxisStatus()
{
	DWORD dwAxisStatus;
	EZISERVO_AXISSTATUS stAxisStatus;

	//////////////////////////////////////////////////////////////////////////
	// Axis X
	if( FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_X, &dwAxisStatus) != FMM_OK ) return;
	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_SERVOON )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_X))->SetBackColorInterior(COLOR_YELLOW);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_X))->SetBackColorInterior(COLOR_WHITE);
	}
	else
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_X))->SetBackColorInterior(COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_X))->SetBackColorInterior(COLOR_YELLOW);
	}

	if( stAxisStatus.FFLAG_ERRORALL ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_X))->SetBackColorInterior(COLOR_RED);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_X))->SetBackColorInterior(COLOR_WHITE);

	if( stAxisStatus.FFLAG_ORIGINRETOK ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_X))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_X))->SetBackColorInterior(COLOR_WHITE);

	//////////////////////////////////////////////////////////////////////////
	// Axis Y
	if( FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_Y, &dwAxisStatus) != FMM_OK ) dwAxisStatus = 0;
	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_SERVOON )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_Y))->SetBackColorInterior(COLOR_YELLOW);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_Y))->SetBackColorInterior(COLOR_WHITE);
	}
	else
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_Y))->SetBackColorInterior(COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_Y))->SetBackColorInterior(COLOR_YELLOW);
	}

	if( stAxisStatus.FFLAG_ERRORALL ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_Y))->SetBackColorInterior(COLOR_RED);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_Y))->SetBackColorInterior(COLOR_WHITE);

	if( stAxisStatus.FFLAG_ORIGINRETOK ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_Y))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_Y))->SetBackColorInterior(COLOR_WHITE);

	//////////////////////////////////////////////////////////////////////////
	// Axis Z
	if( FAS_GetAxisStatus(DEF_FAS_LEFT, DEF_AXIS_Z, &dwAxisStatus) != FMM_OK ) dwAxisStatus = 0;
	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_SERVOON )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_Z))->SetBackColorInterior(COLOR_YELLOW);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_Z))->SetBackColorInterior(COLOR_WHITE);
	}
	else
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_ON_Z))->SetBackColorInterior(COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_SERVO_OFF_Z))->SetBackColorInterior(COLOR_YELLOW);
	}

	if( stAxisStatus.FFLAG_ERRORALL ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_Z))->SetBackColorInterior(COLOR_RED);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ALARM_RESET_Z))->SetBackColorInterior(COLOR_WHITE);

	if( stAxisStatus.FFLAG_ORIGINRETOK ) ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_Z))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_ORIGIN_Z))->SetBackColorInterior(COLOR_WHITE);
}

void CDlgLeftTrTeach::UpdateIOStatus()
{
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
	ULONGLONG dwInput;
	DWORD dwOutput;

	DWORD dwMask[9] = {SERVO_OUT_BITMASK_USEROUT0, SERVO_OUT_BITMASK_USEROUT1, SERVO_OUT_BITMASK_USEROUT2,
					   SERVO_OUT_BITMASK_USEROUT3, SERVO_OUT_BITMASK_USEROUT4, SERVO_OUT_BITMASK_USEROUT5,
					   SERVO_OUT_BITMASK_USEROUT6, SERVO_OUT_BITMASK_USEROUT7, SERVO_OUT_BITMASK_USEROUT8};

	//////////////////////////////////////////////////////////////////////////
	// Grip
	if( FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwInput) != FMM_OK ) return;

	if( dwInput & SERVO_IN_BITMASK_USERIN6 ) ((CBtnEnh*)GetDlgItem(IDC_BTN_GRIP))->SetBackColorInterior(COLOR_YELLOW);
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_GRIP))->SetBackColorInterior(COLOR_WHITE);

	if( pWorld->m_bOutput[DEF_IO_TR_GRIP] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_GRIP))->SetCaption(_T("Grip\r\nON"));
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_GRIP))->SetCaption(_T("Grip\r\nOFF"));

	//////////////////////////////////////////////////////////////////////////
	// Pack Insert
	if( (m_nSelected >= 1) && (m_nSelected <= 9) )
	{
		if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, &dwOutput) != FMM_OK ) return;

		if( dwOutput & dwMask[m_nSelected - 1] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
	}
	else if( (m_nSelected >= 10) && (m_nSelected <= 18) )
	{
		if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwOutput) != FMM_OK ) return;

		if( dwOutput & dwMask[m_nSelected - 10] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
	}
#ifdef DEF_EIN_48_LCA
	else if( (m_nSelected >= 19) && (m_nSelected <= 24) )
	{
		if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwOutput) != FMM_OK ) return;

		if( dwOutput & dwMask[m_nSelected - 19] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
	}
#else
	else if( (m_nSelected >= 19) && (m_nSelected <= 27) )
	{
		if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwOutput) != FMM_OK ) return;

		if( dwOutput & dwMask[m_nSelected - 19] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
	}
	else if( m_nSelected == 28 )
	{
		if( FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwOutput) != FMM_OK ) return;

		if( dwOutput & dwMask[m_nSelected - 28] ) ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
	}
	else ((CBtnEnh*)GetDlgItem(IDC_BTN_PACK))->SetBackColorInterior(COLOR_WHITE);
#endif

	//////////////////////////////////////////////////////////////////////////
	// Barcode
	CCommBCR* pBCR = (CCommBCR*)m_pMainFrm->m_pSerialHub->GetSerial(_T("BCR_Left"));
	((CBtnEnh*)GetDlgItem(IDC_STT_BARCODE))->SetCaption(pBCR->m_sRecv);
}

BOOL CDlgLeftTrTeach::chkDoorSensor()
{
	if( m_pMainFrm->m_pMainStn->IsSimulateMode() ) return FALSE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN3) ) return TRUE;

	return FALSE;
}
