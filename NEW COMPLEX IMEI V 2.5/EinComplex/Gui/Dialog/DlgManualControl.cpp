#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgManualControl.h"

#include <BtnEnh.h>
#include "MainFrm.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgManualControl::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgManualControl, CDialog)
	ON_WM_CLOSE()
	ON_WM_SYSCOMMAND()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgManualControl, CDialog)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE1, DISPID_CLICK, OnClickBtnLine1, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE2, DISPID_CLICK, OnClickBtnLine2, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE3, DISPID_CLICK, OnClickBtnLine3, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE4, DISPID_CLICK, OnClickBtnLine4, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE5, DISPID_CLICK, OnClickBtnLine5, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE6, DISPID_CLICK, OnClickBtnLine6, VTS_NONE)
#ifndef DEF_EIN_48_LCA
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE7, DISPID_CLICK, OnClickBtnLine7, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LINE8, DISPID_CLICK, OnClickBtnLine8, VTS_NONE)
#endif
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_ALL, DISPID_CLICK, OnClickBtnLeftAll, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_ALL, DISPID_CLICK, OnClickBtnRightAll, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_LD_UP, DISPID_CLICK, OnClickBtnLeftLoadUp, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_LD_DOWN, DISPID_CLICK, OnClickBtnLeftLoadDown, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_UD_UP, DISPID_CLICK, OnClickBtnLeftUnloadUp, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_UD_DOWN, DISPID_CLICK, OnClickBtnLeftUnloadDown, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LEFT_NG_CV, DISPID_CLICK, OnClickBtnLeftNGCV, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_LD_UP, DISPID_CLICK, OnClickBtnRightLoadUp, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_LD_DOWN, DISPID_CLICK, OnClickBtnRightLoadDown, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_UD_UP, DISPID_CLICK, OnClickBtnRightUnloadUp, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_UD_DOWN, DISPID_CLICK, OnClickBtnRightUnloadDown, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_RIGHT_NG_CV, DISPID_CLICK, OnClickBtnRightNGCV, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LOAD_CV, DISPID_CLICK, OnClickBtnLoadCV, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_LOAD_CV2, DISPID_CLICK, OnClickBtnLoadCV2, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_UNLOAD_CV, DISPID_CLICK, OnClickBtnUnloadCV, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_PACK_IN, DISPID_CLICK, OnClickBtnPackIn, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_PACK_OUT, DISPID_CLICK, OnClickBtnPackOut, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_REPEAT_START, DISPID_CLICK, OnClickBtnRepeatStart, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_REPEAT_STOP, DISPID_CLICK, OnClickBtnRepeatStop, VTS_NONE)
	ON_EVENT(CDlgManualControl, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)

	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG1, IDC_BTN_JIG1, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG2, IDC_BTN_JIG2, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG3, IDC_BTN_JIG3, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG4, IDC_BTN_JIG4, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG5, IDC_BTN_JIG5, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG6, IDC_BTN_JIG6, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG7, IDC_BTN_JIG7, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG8, IDC_BTN_JIG8, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG9, IDC_BTN_JIG9, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG10, IDC_BTN_JIG10, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG11, IDC_BTN_JIG11, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG12, IDC_BTN_JIG12, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG13, IDC_BTN_JIG13, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG14, IDC_BTN_JIG14, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG15, IDC_BTN_JIG15, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG16, IDC_BTN_JIG16, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG17, IDC_BTN_JIG17, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG18, IDC_BTN_JIG18, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG19, IDC_BTN_JIG19, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG20, IDC_BTN_JIG20, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG21, IDC_BTN_JIG21, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG22, IDC_BTN_JIG22, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG23, IDC_BTN_JIG23, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG24, IDC_BTN_JIG24, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG25, IDC_BTN_JIG25, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG26, IDC_BTN_JIG26, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG27, IDC_BTN_JIG27, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG28, IDC_BTN_JIG28, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG29, IDC_BTN_JIG29, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG30, IDC_BTN_JIG30, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG31, IDC_BTN_JIG31, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG32, IDC_BTN_JIG32, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG33, IDC_BTN_JIG33, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG34, IDC_BTN_JIG34, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG35, IDC_BTN_JIG35, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG36, IDC_BTN_JIG36, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG37, IDC_BTN_JIG37, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG38, IDC_BTN_JIG38, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG39, IDC_BTN_JIG39, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG40, IDC_BTN_JIG40, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG41, IDC_BTN_JIG41, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG42, IDC_BTN_JIG42, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG43, IDC_BTN_JIG43, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG44, IDC_BTN_JIG44, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG45, IDC_BTN_JIG45, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG46, IDC_BTN_JIG46, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG47, IDC_BTN_JIG47, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG48, IDC_BTN_JIG48, DISPID_CLICK, OnClickBtnJig, VTS_I4)
#ifndef DEF_EIN_48_LCA
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG49, IDC_BTN_JIG49, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG50, IDC_BTN_JIG50, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG51, IDC_BTN_JIG51, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG52, IDC_BTN_JIG52, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG53, IDC_BTN_JIG53, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG54, IDC_BTN_JIG54, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG55, IDC_BTN_JIG55, DISPID_CLICK, OnClickBtnJig, VTS_I4)
	ON_EVENT_RANGE(CDlgManualControl, IDC_BTN_JIG56, IDC_BTN_JIG56, DISPID_CLICK, OnClickBtnJig, VTS_I4)
#endif
END_EVENTSINK_MAP()

CDlgManualControl::CDlgManualControl(CWnd* pParent) : CDialog(CDlgManualControl::IDD, pParent)
{
}

BOOL CDlgManualControl::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgManualControl::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

void CDlgManualControl::OnSysCommand(UINT nID, LPARAM lParam) 
{
	if( nID == SC_CLOSE ) return;

	CDialog::OnSysCommand(nID, lParam);
}

BOOL CDlgManualControl::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgManualControl::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pLeftTrStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	m_pRightTrStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));

	m_nJigResID[0] = IDC_BTN_JIG1;
	m_nJigResID[1] = IDC_BTN_JIG2;
	m_nJigResID[2] = IDC_BTN_JIG3;
	m_nJigResID[3] = IDC_BTN_JIG4;
	m_nJigResID[4] = IDC_BTN_JIG5;
	m_nJigResID[5] = IDC_BTN_JIG6;
	m_nJigResID[6] = IDC_BTN_JIG7;
	m_nJigResID[7] = IDC_BTN_JIG8;
	m_nJigResID[8] = IDC_BTN_JIG9;
	m_nJigResID[9] = IDC_BTN_JIG10;
	m_nJigResID[10] = IDC_BTN_JIG11;
	m_nJigResID[11] = IDC_BTN_JIG12;
	m_nJigResID[12] = IDC_BTN_JIG13;
	m_nJigResID[13] = IDC_BTN_JIG14;
	m_nJigResID[14] = IDC_BTN_JIG15;
	m_nJigResID[15] = IDC_BTN_JIG16;
	m_nJigResID[16] = IDC_BTN_JIG17;
	m_nJigResID[17] = IDC_BTN_JIG18;
	m_nJigResID[18] = IDC_BTN_JIG19;
	m_nJigResID[19] = IDC_BTN_JIG20;
	m_nJigResID[20] = IDC_BTN_JIG21;
	m_nJigResID[21] = IDC_BTN_JIG22;
	m_nJigResID[22] = IDC_BTN_JIG23;
	m_nJigResID[23] = IDC_BTN_JIG24;
	m_nJigResID[24] = IDC_BTN_JIG25;
	m_nJigResID[25] = IDC_BTN_JIG26;
	m_nJigResID[26] = IDC_BTN_JIG27;
	m_nJigResID[27] = IDC_BTN_JIG28;
	m_nJigResID[28] = IDC_BTN_JIG29;
	m_nJigResID[29] = IDC_BTN_JIG30;
	m_nJigResID[30] = IDC_BTN_JIG31;
	m_nJigResID[31] = IDC_BTN_JIG32;
	m_nJigResID[32] = IDC_BTN_JIG33;
	m_nJigResID[33] = IDC_BTN_JIG34;
	m_nJigResID[34] = IDC_BTN_JIG35;
	m_nJigResID[35] = IDC_BTN_JIG36;
	m_nJigResID[36] = IDC_BTN_JIG37;
	m_nJigResID[37] = IDC_BTN_JIG38;
	m_nJigResID[38] = IDC_BTN_JIG39;
	m_nJigResID[39] = IDC_BTN_JIG40;
	m_nJigResID[40] = IDC_BTN_JIG41;
	m_nJigResID[41] = IDC_BTN_JIG42;
	m_nJigResID[42] = IDC_BTN_JIG43;
	m_nJigResID[43] = IDC_BTN_JIG44;
	m_nJigResID[44] = IDC_BTN_JIG45;
	m_nJigResID[45] = IDC_BTN_JIG46;
	m_nJigResID[46] = IDC_BTN_JIG47;
	m_nJigResID[47] = IDC_BTN_JIG48;
#ifndef DEF_EIN_48_LCA
	m_nJigResID[48] = IDC_BTN_JIG49;
	m_nJigResID[49] = IDC_BTN_JIG50;
	m_nJigResID[50] = IDC_BTN_JIG51;
	m_nJigResID[51] = IDC_BTN_JIG52;
	m_nJigResID[52] = IDC_BTN_JIG53;
	m_nJigResID[53] = IDC_BTN_JIG54;
	m_nJigResID[54] = IDC_BTN_JIG55;
	m_nJigResID[55] = IDC_BTN_JIG56;
#endif

	m_bPackToggle = FALSE;
	for( int i = 0; i < DEF_MAX_JIG; i++ ) m_bSelected[i] = FALSE;
	UpdateSelect();

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgManualControl::OnClose() 
{
	KillTimer(0);
	KillTimer(1);

	DestroyWindow();
}

void CDlgManualControl::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		KillTimer(0);

		UpdateStatus();

		SetTimer(0, 100, NULL);
	}
	else if( nIDEvent == 1 )
	{
		m_bPackToggle = !m_bPackToggle;

		if( m_bPackToggle )
		{
			for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
			{
				if( m_bSelected[i] ) m_pLeftTrStn->_oPackIn(i);
				if( m_bSelected[i + DEF_MAX_JIG_ONE_SIDE] ) m_pRightTrStn->_oPackIn(i);
			}
		}
		else
		{
			for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
			{
				if( m_bSelected[i] ) m_pLeftTrStn->_oPackOut(i);
				if( m_bSelected[i + DEF_MAX_JIG_ONE_SIDE] ) m_pRightTrStn->_oPackOut(i);
			}
		}
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgManualControl::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgManualControl::OnClickBtnLine1()
{
	m_bSelected[0] = !m_bSelected[0];
	m_bSelected[1] = m_bSelected[0];
	m_bSelected[2] = m_bSelected[0];
	m_bSelected[3] = m_bSelected[0];
	m_bSelected[4] = m_bSelected[0];
	m_bSelected[5] = m_bSelected[0];
	m_bSelected[6] = m_bSelected[0];
	m_bSelected[7] = m_bSelected[0];
	m_bSelected[8] = m_bSelected[0];

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine2()
{
#ifdef DEF_EIN_48_LCA
	m_bSelected[9] = !m_bSelected[9];
	m_bSelected[10] = m_bSelected[9];
	m_bSelected[11] = m_bSelected[9];
	m_bSelected[12] = m_bSelected[9];
	m_bSelected[13] = m_bSelected[9];
	m_bSelected[14] = m_bSelected[9];
	m_bSelected[15] = m_bSelected[9];
	m_bSelected[16] = m_bSelected[9];
#else
	m_bSelected[9] = !m_bSelected[9];
	m_bSelected[10] = m_bSelected[9];
	m_bSelected[11] = m_bSelected[9];
	m_bSelected[12] = m_bSelected[9];
	m_bSelected[13] = m_bSelected[9];
	m_bSelected[14] = m_bSelected[9];
	m_bSelected[15] = m_bSelected[9];
	m_bSelected[16] = m_bSelected[9];
	m_bSelected[17] = m_bSelected[9];
#endif

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine3()
{
#ifdef DEF_EIN_48_LCA
	m_bSelected[17] = !m_bSelected[17];
	m_bSelected[18] = m_bSelected[17];
	m_bSelected[19] = m_bSelected[17];
	m_bSelected[20] = m_bSelected[17];
	m_bSelected[21] = m_bSelected[17];
	m_bSelected[22] = m_bSelected[17];
	m_bSelected[23] = m_bSelected[17];
#else
	m_bSelected[18] = !m_bSelected[18];
	m_bSelected[19] = m_bSelected[18];
	m_bSelected[20] = m_bSelected[18];
	m_bSelected[21] = m_bSelected[18];
	m_bSelected[22] = m_bSelected[18];
#endif

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine4()
{
#ifdef DEF_EIN_48_LCA
	m_bSelected[24] = !m_bSelected[24];
	m_bSelected[25] = m_bSelected[24];
	m_bSelected[26] = m_bSelected[24];
	m_bSelected[27] = m_bSelected[24];
	m_bSelected[28] = m_bSelected[24];
	m_bSelected[29] = m_bSelected[24];
	m_bSelected[30] = m_bSelected[24];
	m_bSelected[31] = m_bSelected[24];
	m_bSelected[32] = m_bSelected[24];
#else
	m_bSelected[23] = !m_bSelected[23];
	m_bSelected[24] = m_bSelected[23];
	m_bSelected[25] = m_bSelected[23];
	m_bSelected[26] = m_bSelected[23];
	m_bSelected[27] = m_bSelected[23];
#endif

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine5()
{
#ifdef DEF_EIN_48_LCA
	m_bSelected[33] = !m_bSelected[33];
	m_bSelected[34] = m_bSelected[33];
	m_bSelected[35] = m_bSelected[33];
	m_bSelected[36] = m_bSelected[33];
	m_bSelected[37] = m_bSelected[33];
	m_bSelected[38] = m_bSelected[33];
	m_bSelected[39] = m_bSelected[33];
	m_bSelected[40] = m_bSelected[33];
#else
	m_bSelected[28] = !m_bSelected[28];
	m_bSelected[29] = m_bSelected[28];
	m_bSelected[30] = m_bSelected[28];
	m_bSelected[31] = m_bSelected[28];
	m_bSelected[32] = m_bSelected[28];
	m_bSelected[33] = m_bSelected[28];
	m_bSelected[34] = m_bSelected[28];
	m_bSelected[35] = m_bSelected[28];
	m_bSelected[36] = m_bSelected[28];
#endif

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine6()
{
#ifdef DEF_EIN_48_LCA
	m_bSelected[41] = !m_bSelected[41];
	m_bSelected[42] = m_bSelected[41];
	m_bSelected[43] = m_bSelected[41];
	m_bSelected[44] = m_bSelected[41];
	m_bSelected[45] = m_bSelected[41];
	m_bSelected[46] = m_bSelected[41];
	m_bSelected[47] = m_bSelected[41];
#else
	m_bSelected[37] = !m_bSelected[37];
	m_bSelected[38] = m_bSelected[37];
	m_bSelected[39] = m_bSelected[37];
	m_bSelected[40] = m_bSelected[37];
	m_bSelected[41] = m_bSelected[37];
	m_bSelected[42] = m_bSelected[37];
	m_bSelected[43] = m_bSelected[37];
	m_bSelected[44] = m_bSelected[37];
	m_bSelected[45] = m_bSelected[37];
#endif

	UpdateSelect();
}

#ifndef DEF_EIN_48_LCA
void CDlgManualControl::OnClickBtnLine7()
{
	m_bSelected[46] = !m_bSelected[46];
	m_bSelected[47] = m_bSelected[46];
	m_bSelected[48] = m_bSelected[46];
	m_bSelected[49] = m_bSelected[46];
	m_bSelected[50] = m_bSelected[46];

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLine8()
{
	m_bSelected[51] = !m_bSelected[51];
	m_bSelected[52] = m_bSelected[51];
	m_bSelected[53] = m_bSelected[51];
	m_bSelected[54] = m_bSelected[51];
	m_bSelected[55] = m_bSelected[51];

	UpdateSelect();
}
#endif

void CDlgManualControl::OnClickBtnLeftAll()
{
	m_bSelected[0] = !m_bSelected[0];

	for( int i = 1; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		m_bSelected[i] = m_bSelected[0];
	}

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnRightAll()
{
	m_bSelected[DEF_MAX_JIG_ONE_SIDE] = !m_bSelected[DEF_MAX_JIG_ONE_SIDE];

	for( int i = 1; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		m_bSelected[i + DEF_MAX_JIG_ONE_SIDE] = m_bSelected[DEF_MAX_JIG_ONE_SIDE];
	}

	UpdateSelect();
}

void CDlgManualControl::OnClickBtnLeftLoadUp()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;
}

void CDlgManualControl::OnClickBtnLeftLoadDown()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;
}

void CDlgManualControl::OnClickBtnLeftUnloadUp()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;
}

void CDlgManualControl::OnClickBtnLeftUnloadDown()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;
}

void CDlgManualControl::OnClickBtnLeftNGCV()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_NGCV_RUN] = !pWorld->m_bOutput[DEF_IO_NGCV_RUN];
}

void CDlgManualControl::OnClickBtnRightLoadUp()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = FALSE;
}

void CDlgManualControl::OnClickBtnRightLoadDown()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDPP_DOWN] = TRUE;
}

void CDlgManualControl::OnClickBtnRightUnloadUp()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = FALSE;
}

void CDlgManualControl::OnClickBtnRightUnloadDown()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDPP_DOWN] = TRUE;
}

void CDlgManualControl::OnClickBtnRightNGCV()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_NGCV_RUN] = !pWorld->m_bOutput[DEF_IO_NGCV_RUN];
}

void CDlgManualControl::OnClickBtnLoadCV()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_LDCV_RUN] = !pWorld->m_bOutput[DEF_IO_LDCV_RUN];
}

void CDlgManualControl::OnClickBtnLoadCV2()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

	pWorld->m_bOutput[DEF_IO_LDCV2_RUN] = !pWorld->m_bOutput[DEF_IO_LDCV2_RUN];
}

void CDlgManualControl::OnClickBtnUnloadCV()
{
	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_UDCV_RUN] = !pWorld->m_bOutput[DEF_IO_UDCV_RUN];
}

void CDlgManualControl::OnClickBtnPackIn()
{
	for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		if( m_bSelected[i] ) m_pLeftTrStn->_oPackIn(i);
		if( m_bSelected[i + DEF_MAX_JIG_ONE_SIDE] ) m_pRightTrStn->_oPackIn(i);
	}
}

void CDlgManualControl::OnClickBtnPackOut()
{
	for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		if( m_bSelected[i] ) m_pLeftTrStn->_oPackOut(i);
		if( m_bSelected[i + DEF_MAX_JIG_ONE_SIDE] ) m_pRightTrStn->_oPackOut(i);
	}
}

void CDlgManualControl::OnClickBtnRepeatStart()
{
	SetTimer(1, 3000, NULL);
}

void CDlgManualControl::OnClickBtnRepeatStop()
{
	KillTimer(1);
}

void CDlgManualControl::OnClickBtnExit()
{
	OnCancel();
}

void CDlgManualControl::OnClickBtnJig(UINT nID)
{
	for( int i = 0; i < DEF_MAX_JIG; i++ )
	{
		if( m_nJigResID[i] == nID ) m_bSelected[i] = TRUE;
		else m_bSelected[i] = FALSE;
	}

	UpdateSelect();
}

void CDlgManualControl::LoadInsertStatus()
{
	DWORD dwOutput[8] = {0,};

	FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_X, &dwOutput[0]);
	FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwOutput[1]);
	FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_Z, &dwOutput[2]);
	FAS_GetIOOutput(DEF_FAS_LEFT, DEF_AXIS_PP, &dwOutput[3]);
	FAS_GetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwOutput[4]);
	FAS_GetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwOutput[5]);
	FAS_GetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwOutput[6]);
	FAS_GetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_PP, &dwOutput[7]);

#ifdef DEF_EIN_48_LCA
	m_bInserted[0] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[1] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[2] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[3] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[4] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[5] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[6] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[7] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[8] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[9] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[10] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[11] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[12] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[13] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[14] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[15] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[16] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[17] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[18] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[19] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[20] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[21] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[22] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[23] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[24] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[25] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[26] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[27] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[28] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[29] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[30] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[31] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[32] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[33] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[34] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[35] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[36] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[37] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[38] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[39] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[40] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[41] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[42] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[43] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[44] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[45] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[46] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[47] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT5);
#else
	m_bInserted[0] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[1] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[2] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[3] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[4] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[5] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[6] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[7] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[8] = (dwOutput[0] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[9] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[10] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[11] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[12] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[13] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[14] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[15] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[16] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[17] = (dwOutput[1] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[18] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[19] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[20] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[21] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[22] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[23] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[24] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[25] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[26] = (dwOutput[2] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[27] = (dwOutput[3] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[28] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[29] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[30] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[31] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[32] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[33] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[34] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[35] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[36] = (dwOutput[4] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[37] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[38] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[39] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[40] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[41] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[42] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[43] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[44] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[45] = (dwOutput[5] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[46] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT0);
	m_bInserted[47] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT1);
	m_bInserted[48] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT2);
	m_bInserted[49] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT3);
	m_bInserted[50] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT4);
	m_bInserted[51] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT5);
	m_bInserted[52] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT6);
	m_bInserted[53] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT7);
	m_bInserted[54] = (dwOutput[6] & SERVO_OUT_BITMASK_USEROUT8);
	m_bInserted[55] = (dwOutput[7] & SERVO_OUT_BITMASK_USEROUT0);
#endif
}

void CDlgManualControl::UpdateSelect()
{
	for( int i = 0; i < DEF_MAX_JIG; i++ )
	{
		((CBtnEnh*)GetDlgItem(m_nJigResID[i]))->SetBackColorInterior(m_bSelected[i] ? COLOR_YELLOW : COLOR_WHITE);
	}
}

void CDlgManualControl::UpdateStatus()
{
	ULONGLONG dwInputLeft, dwInputRight;

	if( FAS_GetIOInput(DEF_FAS_LEFT, DEF_AXIS_Y, &dwInputLeft) == FMM_OK )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_UP))->SetBackColorInterior((dwInputLeft & SERVO_IN_BITMASK_USERIN2) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_LD_DOWN))->SetBackColorInterior((dwInputLeft & SERVO_IN_BITMASK_USERIN3) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_UP))->SetBackColorInterior((dwInputLeft & SERVO_IN_BITMASK_USERIN4) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_UD_DOWN))->SetBackColorInterior((dwInputLeft & SERVO_IN_BITMASK_USERIN5) ? COLOR_YELLOW : COLOR_WHITE);
	}

	if( FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInputRight) == FMM_OK )
	{
		((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_UP))->SetBackColorInterior((dwInputRight & SERVO_IN_BITMASK_USERIN2) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_LD_DOWN))->SetBackColorInterior((dwInputRight & SERVO_IN_BITMASK_USERIN3) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_UP))->SetBackColorInterior((dwInputRight & SERVO_IN_BITMASK_USERIN4) ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_UD_DOWN))->SetBackColorInterior((dwInputRight & SERVO_IN_BITMASK_USERIN5) ? COLOR_YELLOW : COLOR_WHITE);
	}

	CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	CCommWorld* pWorldLeft = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
	CCommWorld* pWorldRight = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_NG_CV))->SetBackColorInterior(pWorldLeft->m_bOutput[DEF_IO_NGCV_RUN] ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_NG_CV))->SetBackColorInterior(pWorldRight->m_bOutput[DEF_IO_NGCV_RUN] ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LOAD_CV))->SetBackColorInterior(pWorldRight->m_bOutput[DEF_IO_LDCV_RUN] ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_UNLOAD_CV))->SetBackColorInterior(pWorldRight->m_bOutput[DEF_IO_UDCV_RUN] ? COLOR_YELLOW : COLOR_WHITE);

	LoadInsertStatus();
	
	CString sCaption = _T("");
	for( int i = 0; i < DEF_MAX_JIG; i++ )
	{
		sCaption.Format(_T("J#%02d\r\n%s"), i + 1, m_bInserted[i] ? _T("ON") : _T("OFF"));
		((CBtnEnh*)GetDlgItem(m_nJigResID[i]))->SetCaption(sCaption);
	}
}
