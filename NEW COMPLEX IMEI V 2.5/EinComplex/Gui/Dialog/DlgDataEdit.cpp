#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgDataEdit.h"

#include <BtnEnh.h>
#include "MainFrm.h"
#include "MainControlStation.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "DlgDigitPad.h"
#include "DlgCharPad.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgDataEdit::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgDataEdit, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgDataEdit, CDialog)
	ON_EVENT(CDlgDataEdit, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_SHIFT_A, DISPID_CLICK, OnClickBtnSelShiftA, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_SHIFT_B, DISPID_CLICK, OnClickBtnSelShiftB, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_SHIFT_C, DISPID_CLICK, OnClickBtnSelShiftC, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_WORK_START, DISPID_CLICK, OnClickSttWorkStart, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_REST1_START, DISPID_CLICK, OnClickSttRest1Start, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_REST1_END, DISPID_CLICK, OnClickSttRest1End, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_LUNCH_START, DISPID_CLICK, OnClickSttLunchStart, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_LUNCH_END, DISPID_CLICK, OnClickSttLunchEnd, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_REST2_START, DISPID_CLICK, OnClickSttRest2Start, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_REST2_END, DISPID_CLICK, OnClickSttRest2End, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_WORK_END, DISPID_CLICK, OnClickSttWorkEnd, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_LEFT_X, DISPID_CLICK, OnClickBtnSelLeftX, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_LEFT_Y, DISPID_CLICK, OnClickBtnSelLeftY, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_LEFT_Z, DISPID_CLICK, OnClickBtnSelLeftZ, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_LEFT_PP, DISPID_CLICK, OnClickBtnSelLeftPP, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_RIGHT_X, DISPID_CLICK, OnClickBtnSelRightX, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_RIGHT_Y, DISPID_CLICK, OnClickBtnSelRightY, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_RIGHT_Z, DISPID_CLICK, OnClickBtnSelRightZ, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_BTN_SEL_RIGHT_PP, DISPID_CLICK, OnClickBtnSelRightPP, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_VELOCITY, DISPID_CLICK, OnClickSttVelocity, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SLOW_SPD, DISPID_CLICK, OnClickSttSlowSpd, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SW_LIMIT_POS, DISPID_CLICK, OnClickSttSWLimitPos, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SW_LIMIT_NEG, DISPID_CLICK, OnClickSttSWLimitNeg, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SAFE_POS_Z, DISPID_CLICK, OnClickSttSafePosZ, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SLOW_DOWN_DIST, DISPID_CLICK, OnClickSttSlowDownDist, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_Z_UP_AFTER_PACK_IN, DISPID_CLICK, OnClickSttZUpAfterPackIn, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_INSERT_CV_SYNC_MODE, DISPID_CLICK, OnClickSttInsertCVSyncMode, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_MAX_RETEST_CNT, DISPID_CLICK, OnClickSttMaxRetestCnt, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_PACK_BLOCK_CNT, DISPID_CLICK, OnClickSttPackOutBlockCnt, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_SAME_FAIL_BLOCK_CNT, DISPID_CLICK, OnClickSttSameFailBlockCnt, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_MOTION_TIMEOUT, DISPID_CLICK, OnClickSttMotionTimeout, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_CYLINDER_TIMEOUT, DISPID_CLICK, OnClickSttCylinderTimeout, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_BCR_TIMEOUT, DISPID_CLICK, OnClickSttBCRTimeout, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_PACK_INSERT_DELAY, DISPID_CLICK, OnClickSttPackInsertDelay, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_TEST_TIMEOUT, DISPID_CLICK, OnClickSttTestTimeout, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_BARCODE_LENGTH, DISPID_CLICK, OnClickSttBarcodeLength, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME1, DISPID_CLICK, OnClickSttRetestName1, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME2, DISPID_CLICK, OnClickSttRetestName2, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME3, DISPID_CLICK, OnClickSttRetestName3, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME4, DISPID_CLICK, OnClickSttRetestName4, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME5, DISPID_CLICK, OnClickSttRetestName5, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME6, DISPID_CLICK, OnClickSttRetestName6, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME7, DISPID_CLICK, OnClickSttRetestName7, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME8, DISPID_CLICK, OnClickSttRetestName8, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME9, DISPID_CLICK, OnClickSttRetestName9, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RETEST_NAME10, DISPID_CLICK, OnClickSttRetestName10, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RATE_BLOCK_LEAST_CNT, DISPID_CLICK, OnClickSttRateBlockLeastCnt, VTS_NONE)
	ON_EVENT(CDlgDataEdit, IDC_STT_RATE_BLOCK_PERCENT, DISPID_CLICK, OnClickSttRateBlockPercent, VTS_NONE)
END_EVENTSINK_MAP()

CDlgDataEdit::CDlgDataEdit(CWnd* pParent) : CDialog(CDlgDataEdit::IDD, pParent)
{
}

BOOL CDlgDataEdit::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgDataEdit::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgDataEdit::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgDataEdit::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pLeftTrStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	m_pRightTrStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));
	m_pLeftPpStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	m_pRightPpStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));

	m_nSelectAxis = 0;
	m_nSelectShift = 0;

	UpdateParam();

	return TRUE;
}

void CDlgDataEdit::OnClose() 
{
	DestroyWindow();
}

void CDlgDataEdit::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgDataEdit::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgDataEdit::OnClickBtnExit()
{
	OnCancel();
}

void CDlgDataEdit::OnClickBtnSelShiftA()
{
	m_nSelectShift = 0;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelShiftB()
{
	m_nSelectShift = 1;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelShiftC()
{
	m_nSelectShift = 2;
	UpdateParam();
}

void CDlgDataEdit::OnClickSttWorkStart()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRest1Start()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRest1End()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttLunchStart()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttLunchEnd()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRest2Start()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRest2End()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttWorkEnd()
{
	double dTemp = (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_HOUR] * 10000.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_MINUTE] * 100.0;
	dTemp += (double)m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_SECOND];

	int nHour, nMinute, nSecond;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		nHour = (int)(dTemp / 10000.0);
		nMinute = (int)(dTemp / 100.0) % 100;
		nSecond = (int)(dTemp) % 100;

		if( (nHour > 23) || (nHour < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 23)"));
			return;
		}

		if( (nMinute > 59) || (nMinute < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		if( (nSecond > 59) || (nSecond < 0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 59)"));
			return;
		}

		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_HOUR] = nHour;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_MINUTE] = nMinute;
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_SECOND] = nSecond;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickBtnSelLeftX()
{
	m_nSelectAxis = 0;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelLeftY()
{
	m_nSelectAxis = 1;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelLeftZ()
{
	m_nSelectAxis = 2;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelLeftPP()
{
	m_nSelectAxis = 3;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelRightX()
{
	m_nSelectAxis = 4;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelRightY()
{
	m_nSelectAxis = 5;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelRightZ()
{
	m_nSelectAxis = 6;
	UpdateParam();
}

void CDlgDataEdit::OnClickBtnSelRightPP()
{
	m_nSelectAxis = 7;
	UpdateParam();
}

void CDlgDataEdit::OnClickSttVelocity()
{
	double dTemp = 0.0;

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dVelocityX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dVelocityY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dVelocityZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dVelocityX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dVelocityX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dVelocityY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dVelocityZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dVelocityX;
	else return;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 1.0) || (dTemp > 500.0) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 500)"));
			return;
		}

		if( m_nSelectAxis == 0 )
		{
			m_pLeftTrStn->m_dVelocityX = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 1 )
		{
			m_pLeftTrStn->m_dVelocityY = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 2 )
		{
			m_pLeftTrStn->m_dVelocityZ = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 3 )
		{
			m_pLeftPpStn->m_dVelocityX = dTemp;
			m_pLeftPpStn->SaveProfile();
		}
		else if( m_nSelectAxis == 4 )
		{
			m_pRightTrStn->m_dVelocityX = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 5 )
		{
			m_pRightTrStn->m_dVelocityY = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 6 )
		{
			m_pRightTrStn->m_dVelocityZ = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 7 )
		{
			m_pRightPpStn->m_dVelocityX = dTemp;
			m_pRightPpStn->SaveProfile();
		}
		else return;

		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSlowSpd()
{
	double dTemp = 0.0;

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSlowSpdY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSlowSpdZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSlowSpdY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSlowSpdZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSlowSpdX;
	else return;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 1.0) || (dTemp > 500.0) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 500)"));
			return;
		}

		if( m_nSelectAxis == 0 )
		{
			m_pLeftTrStn->m_dSlowSpdX = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 1 )
		{
			m_pLeftTrStn->m_dSlowSpdY = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 2 )
		{
			m_pLeftTrStn->m_dSlowSpdZ = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 3 )
		{
			m_pLeftPpStn->m_dSlowSpdX = dTemp;
			m_pLeftPpStn->SaveProfile();
		}
		else if( m_nSelectAxis == 4 )
		{
			m_pRightTrStn->m_dSlowSpdX = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 5 )
		{
			m_pRightTrStn->m_dSlowSpdY = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 6 )
		{
			m_pRightTrStn->m_dSlowSpdZ = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 7 )
		{
			m_pRightPpStn->m_dSlowSpdX = dTemp;
			m_pRightPpStn->SaveProfile();
		}
		else return;

		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSWLimitPos()
{
	double dTemp = 0.0;

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSWLimitPosY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSWLimitPosZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSWLimitPosY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSWLimitPosZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSWLimitPosX;
	else return;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 1.0) || (dTemp > 1000.0) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 1000)"));
			return;
		}

		if( m_nSelectAxis == 0 )
		{
			m_pLeftTrStn->m_dSWLimitPosX = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 1 )
		{
			m_pLeftTrStn->m_dSWLimitPosY = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 2 )
		{
			m_pLeftTrStn->m_dSWLimitPosZ = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 3 )
		{
			m_pLeftPpStn->m_dSWLimitPosX = dTemp;
			m_pLeftPpStn->SaveProfile();
		}
		else if( m_nSelectAxis == 4 )
		{
			m_pRightTrStn->m_dSWLimitPosX = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 5 )
		{
			m_pRightTrStn->m_dSWLimitPosY = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 6 )
		{
			m_pRightTrStn->m_dSWLimitPosZ = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 7 )
		{
			m_pRightPpStn->m_dSWLimitPosX = dTemp;
			m_pRightPpStn->SaveProfile();
		}
		else return;

		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSWLimitNeg()
{
	double dTemp = 0.0;

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSWLimitNegY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSWLimitNegZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSWLimitNegY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSWLimitNegZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSWLimitNegX;
	else return;

	CDlgDigitPad dlg(dTemp);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < -100.0) || (dTemp > 100.0) )
		{
			AfxMessageBox(_T("Invalid Data! (-100 to 100)"));
			return;
		}

		if( m_nSelectAxis == 0 )
		{
			m_pLeftTrStn->m_dSWLimitNegX = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 1 )
		{
			m_pLeftTrStn->m_dSWLimitNegY = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 2 )
		{
			m_pLeftTrStn->m_dSWLimitNegZ = dTemp;
			m_pLeftTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 3 )
		{
			m_pLeftPpStn->m_dSWLimitNegX = dTemp;
			m_pLeftPpStn->SaveProfile();
		}
		else if( m_nSelectAxis == 4 )
		{
			m_pRightTrStn->m_dSWLimitNegX = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 5 )
		{
			m_pRightTrStn->m_dSWLimitNegY = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 6 )
		{
			m_pRightTrStn->m_dSWLimitNegZ = dTemp;
			m_pRightTrStn->SaveProfile();
		}
		else if( m_nSelectAxis == 7 )
		{
			m_pRightPpStn->m_dSWLimitNegX = dTemp;
			m_pRightPpStn->SaveProfile();
		}
		else return;

		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSafePosZ()
{
	double dTemp = 0.0;
	CDlgDigitPad dlg(m_pMainStn->m_dInterlockPosZ);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 1.0) || (dTemp > 1000.0) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 1000)"));
			return;
		}

		m_pMainStn->m_dInterlockPosZ = dTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSlowDownDist()
{
	double dTemp = 0.0;
	CDlgDigitPad dlg(m_pMainStn->m_dSlowDownDist);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 1.0) || (dTemp > 1000.0) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 1000)"));
			return;
		}

		m_pMainStn->m_dSlowDownDist = dTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttZUpAfterPackIn()
{
	m_pMainStn->m_bZUpAfterPackIn = !m_pMainStn->m_bZUpAfterPackIn;
	m_pMainStn->SaveProfile();
	UpdateParam();
}

void CDlgDataEdit::OnClickSttInsertCVSyncMode()
{
	m_pMainStn->m_bInsertCVSyncMode = !m_pMainStn->m_bInsertCVSyncMode;
	m_pMainStn->SaveProfile();
	UpdateParam();
}

void CDlgDataEdit::OnClickSttMaxRetestCnt()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nMaxRetestCnt);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 0) || (nTemp > 100) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 100)"));
			return;
		}

		m_pMainStn->m_nMaxRetestCnt = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttPackOutBlockCnt()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nPackOutBlockCnt);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 0) || (nTemp > 100) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 100)"));
			return;
		}

		m_pMainStn->m_nPackOutBlockCnt = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttSameFailBlockCnt()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nSameFailBlockCnt);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 0) || (nTemp > 100) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 100)"));
			return;
		}

		m_pMainStn->m_nSameFailBlockCnt = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttMotionTimeout()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nMotionTimeout);
	
	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 1) || (nTemp > 600000) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 600000)"));
			return;
		}

		m_pMainStn->m_nMotionTimeout = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttCylinderTimeout()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nCylinderTimeout);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 1) || (nTemp > 600000) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 600000)"));
			return;
		}

		m_pMainStn->m_nCylinderTimeout = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttBCRTimeout()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nBCRTimeout);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 1) || (nTemp > 600000) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 600000)"));
			return;
		}

		m_pMainStn->m_nBCRTimeout = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttPackInsertDelay()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nPackInsertDelay);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 0) || (nTemp > 60000) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 60000)"));
			return;
		}

		m_pMainStn->m_nPackInsertDelay = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttTestTimeout()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nJigTestTimeout);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 1) || (nTemp > 1000000) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 1000000)"));
			return;
		}

		m_pMainStn->m_nJigTestTimeout = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttBarcodeLength()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nBarcodeLength);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 1) || (nTemp > 100) )
		{
			AfxMessageBox(_T("Invalid Data! (1 to 100)"));
			return;
		}

		m_pMainStn->m_nBarcodeLength = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName1()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[0]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[0] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName2()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[1]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[1] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName3()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[2]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[2] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName4()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[3]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[3] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName5()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[4]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[4] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName6()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[5]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[5] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName7()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[6]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[6] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName8()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[7]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[7] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName9()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[8]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[8] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRetestName10()
{
	CDlgCharPad dlg(m_pMainStn->m_sRetestName[9]);

	if( dlg.DoModal() == IDOK )
	{
		m_pMainStn->m_sRetestName[9] = dlg.GetData();
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRateBlockLeastCnt()
{
	int nTemp = 0;
	CDlgDigitPad dlg(m_pMainStn->m_nRateBlockLeastCnt);

	if( dlg.DoModal() == IDOK )
	{
		nTemp = (int)dlg.GetData();

		if( (nTemp < 0) || (nTemp > 10000) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 10000)"));
			return;
		}

		m_pMainStn->m_nRateBlockLeastCnt = nTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::OnClickSttRateBlockPercent()
{
	double dTemp = 0.0;
	CDlgDigitPad dlg(m_pMainStn->m_dRateBlockPercent);

	if( dlg.DoModal() == IDOK )
	{
		dTemp = dlg.GetData();

		if( (dTemp < 0.0) || (dTemp > 100.0) )
		{
			AfxMessageBox(_T("Invalid Data! (0 to 100)"));
			return;
		}

		m_pMainStn->m_dRateBlockPercent = dTemp;
		m_pMainStn->SaveProfile();
		UpdateParam();
	}
}

void CDlgDataEdit::UpdateParam()
{
	CString sCaption = _T("");
	double dTemp = 0.0;

	//////////////////////////////////////////////////////////////////////////
	// Work Time
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_SHIFT_A))->SetBackColorInterior((m_nSelectShift == 0) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_SHIFT_B))->SetBackColorInterior((m_nSelectShift == 1) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_SHIFT_C))->SetBackColorInterior((m_nSelectShift == 2) ? COLOR_YELLOW : COLOR_WHITE);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_START][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_WORK_START))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_START][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_REST1_START))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST1_END][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_REST1_END))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_START][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_LUNCH_START))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_LUNCH_END][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_LUNCH_END))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_START][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_REST2_START))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_REST2_END][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_REST2_END))->SetCaption(sCaption);

	sCaption.Format(_T("%02d : %02d : %02d"),
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_HOUR],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_MINUTE],
		m_pMainStn->m_nWorkTime[m_nSelectShift][WT_WORK_END][WT_SECOND]);
	((CBtnEnh*)GetDlgItem(IDC_STT_WORK_END))->SetCaption(sCaption);

	//////////////////////////////////////////////////////////////////////////
	// Axis Parameters
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_LEFT_X))->SetBackColorInterior((m_nSelectAxis == 0) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_LEFT_Y))->SetBackColorInterior((m_nSelectAxis == 1) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_LEFT_Z))->SetBackColorInterior((m_nSelectAxis == 2) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_LEFT_PP))->SetBackColorInterior((m_nSelectAxis == 3) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_RIGHT_X))->SetBackColorInterior((m_nSelectAxis == 4) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_RIGHT_Y))->SetBackColorInterior((m_nSelectAxis == 5) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_RIGHT_Z))->SetBackColorInterior((m_nSelectAxis == 6) ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SEL_RIGHT_PP))->SetBackColorInterior((m_nSelectAxis == 7) ? COLOR_YELLOW : COLOR_WHITE);

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dVelocityX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dVelocityY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dVelocityZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dVelocityX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dVelocityX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dVelocityY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dVelocityZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dVelocityX;

	sCaption.Format(_T("%.03lf mm/s"), dTemp);
	((CBtnEnh*)GetDlgItem(IDC_STT_VELOCITY))->SetCaption(sCaption);

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSlowSpdY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSlowSpdZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSlowSpdX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSlowSpdY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSlowSpdZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSlowSpdX;

	sCaption.Format(_T("%.03lf mm/s"), dTemp);
	((CBtnEnh*)GetDlgItem(IDC_STT_SLOW_SPD))->SetCaption(sCaption);

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSWLimitPosY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSWLimitPosZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSWLimitPosX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSWLimitPosY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSWLimitPosZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSWLimitPosX;

	sCaption.Format(_T("%.03lf mm"), dTemp);
	((CBtnEnh*)GetDlgItem(IDC_STT_SW_LIMIT_POS))->SetCaption(sCaption);

	if( m_nSelectAxis == 0 ) dTemp = m_pLeftTrStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 1 ) dTemp = m_pLeftTrStn->m_dSWLimitNegY;
	else if( m_nSelectAxis == 2 ) dTemp = m_pLeftTrStn->m_dSWLimitNegZ;
	else if( m_nSelectAxis == 3 ) dTemp = m_pLeftPpStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 4 ) dTemp = m_pRightTrStn->m_dSWLimitNegX;
	else if( m_nSelectAxis == 5 ) dTemp = m_pRightTrStn->m_dSWLimitNegY;
	else if( m_nSelectAxis == 6 ) dTemp = m_pRightTrStn->m_dSWLimitNegZ;
	else if( m_nSelectAxis == 7 ) dTemp = m_pRightPpStn->m_dSWLimitNegX;

	sCaption.Format(_T("%.03lf mm"), dTemp);
	((CBtnEnh*)GetDlgItem(IDC_STT_SW_LIMIT_NEG))->SetCaption(sCaption);

	sCaption.Format(_T("%.03lf mm"), m_pMainStn->m_dInterlockPosZ);
	((CBtnEnh*)GetDlgItem(IDC_STT_SAFE_POS_Z))->SetCaption(sCaption);

	sCaption.Format(_T("%.03lf mm"), m_pMainStn->m_dSlowDownDist);
	((CBtnEnh*)GetDlgItem(IDC_STT_SLOW_DOWN_DIST))->SetCaption(sCaption);

	sCaption.Format(_T("%d ms"), m_pMainStn->m_nMotionTimeout);
	((CBtnEnh*)GetDlgItem(IDC_STT_MOTION_TIMEOUT))->SetCaption(sCaption);

	//////////////////////////////////////////////////////////////////////////
	// System Options
	sCaption = m_pMainStn->m_bZUpAfterPackIn ? _T("ON") : _T("OFF");
	((CBtnEnh*)GetDlgItem(IDC_STT_Z_UP_AFTER_PACK_IN))->SetCaption(sCaption);

	sCaption = m_pMainStn->m_bInsertCVSyncMode ? _T("ON") : _T("OFF");
	((CBtnEnh*)GetDlgItem(IDC_STT_INSERT_CV_SYNC_MODE))->SetCaption(sCaption);

	sCaption.Format(_T("%d times"), m_pMainStn->m_nMaxRetestCnt);
	((CBtnEnh*)GetDlgItem(IDC_STT_MAX_RETEST_CNT))->SetCaption(sCaption);

	sCaption.Format(_T("%d (Totally)"), m_pMainStn->m_nPackOutBlockCnt);
	((CBtnEnh*)GetDlgItem(IDC_STT_PACK_BLOCK_CNT))->SetCaption(sCaption);

	sCaption.Format(_T("%d (Continuosly)"), m_pMainStn->m_nSameFailBlockCnt);
	((CBtnEnh*)GetDlgItem(IDC_STT_SAME_FAIL_BLOCK_CNT))->SetCaption(sCaption);

	sCaption.Format(_T("%d ms"), m_pMainStn->m_nCylinderTimeout);
	((CBtnEnh*)GetDlgItem(IDC_STT_CYLINDER_TIMEOUT))->SetCaption(sCaption);

	sCaption.Format(_T("%d ms"), m_pMainStn->m_nBCRTimeout);
	((CBtnEnh*)GetDlgItem(IDC_STT_BCR_TIMEOUT))->SetCaption(sCaption);

	sCaption.Format(_T("%d ms"), m_pMainStn->m_nPackInsertDelay);
	((CBtnEnh*)GetDlgItem(IDC_STT_PACK_INSERT_DELAY))->SetCaption(sCaption);

	sCaption.Format(_T("%d ms"), m_pMainStn->m_nJigTestTimeout);
	((CBtnEnh*)GetDlgItem(IDC_STT_TEST_TIMEOUT))->SetCaption(sCaption);

	sCaption.Format(_T("%d"), m_pMainStn->m_nBarcodeLength);
	((CBtnEnh*)GetDlgItem(IDC_STT_BARCODE_LENGTH))->SetCaption(sCaption);

	sCaption.Format(_T("%d ea"), m_pMainStn->m_nRateBlockLeastCnt);
	((CBtnEnh*)GetDlgItem(IDC_STT_RATE_BLOCK_LEAST_CNT))->SetCaption(sCaption);

	sCaption.Format(_T("%.01lf %%"), m_pMainStn->m_dRateBlockPercent);
	((CBtnEnh*)GetDlgItem(IDC_STT_RATE_BLOCK_PERCENT))->SetCaption(sCaption);

	//////////////////////////////////////////////////////////////////////////
	// Retest Fail Text
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME1))->SetCaption(m_pMainStn->m_sRetestName[0]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME2))->SetCaption(m_pMainStn->m_sRetestName[1]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME3))->SetCaption(m_pMainStn->m_sRetestName[2]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME4))->SetCaption(m_pMainStn->m_sRetestName[3]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME5))->SetCaption(m_pMainStn->m_sRetestName[4]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME6))->SetCaption(m_pMainStn->m_sRetestName[5]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME7))->SetCaption(m_pMainStn->m_sRetestName[6]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME8))->SetCaption(m_pMainStn->m_sRetestName[7]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME9))->SetCaption(m_pMainStn->m_sRetestName[8]);
	((CBtnEnh*)GetDlgItem(IDC_STT_RETEST_NAME10))->SetCaption(m_pMainStn->m_sRetestName[9]);
}
