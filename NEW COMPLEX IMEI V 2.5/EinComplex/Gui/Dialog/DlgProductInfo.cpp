#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgProductInfo.h"

#include <BtnEnh.h>
#include "MainFrm.h"
#include "MainControlStation.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgProductInfo::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgProductInfo, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgProductInfo, CDialog)
	ON_EVENT(CDlgProductInfo, IDC_BTN_RESET_JIG, DISPID_CLICK, OnClickBtnResetJig, VTS_NONE)
	ON_EVENT(CDlgProductInfo, IDC_BTN_RESET_ALL, DISPID_CLICK, OnClickBtnResetAll, VTS_NONE)
	ON_EVENT(CDlgProductInfo, IDC_BTN_INCLUDE_RETEST, DISPID_CLICK, OnClickBtnIncludeRetest, VTS_NONE)
	ON_EVENT(CDlgProductInfo, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)

	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_01, IDC_STT_JIG_INFO_01, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_02, IDC_STT_JIG_INFO_02, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_03, IDC_STT_JIG_INFO_03, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_04, IDC_STT_JIG_INFO_04, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_05, IDC_STT_JIG_INFO_05, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_06, IDC_STT_JIG_INFO_06, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_07, IDC_STT_JIG_INFO_07, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_08, IDC_STT_JIG_INFO_08, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_09, IDC_STT_JIG_INFO_09, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_10, IDC_STT_JIG_INFO_10, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_11, IDC_STT_JIG_INFO_11, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_12, IDC_STT_JIG_INFO_12, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_13, IDC_STT_JIG_INFO_13, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_14, IDC_STT_JIG_INFO_14, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_15, IDC_STT_JIG_INFO_15, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_16, IDC_STT_JIG_INFO_16, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_17, IDC_STT_JIG_INFO_17, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_18, IDC_STT_JIG_INFO_18, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_19, IDC_STT_JIG_INFO_19, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_20, IDC_STT_JIG_INFO_20, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_21, IDC_STT_JIG_INFO_21, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_22, IDC_STT_JIG_INFO_22, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_23, IDC_STT_JIG_INFO_23, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_24, IDC_STT_JIG_INFO_24, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_25, IDC_STT_JIG_INFO_25, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_26, IDC_STT_JIG_INFO_26, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_27, IDC_STT_JIG_INFO_27, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_28, IDC_STT_JIG_INFO_28, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_29, IDC_STT_JIG_INFO_29, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_30, IDC_STT_JIG_INFO_30, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_31, IDC_STT_JIG_INFO_31, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_32, IDC_STT_JIG_INFO_32, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_33, IDC_STT_JIG_INFO_33, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_34, IDC_STT_JIG_INFO_34, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_35, IDC_STT_JIG_INFO_35, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_36, IDC_STT_JIG_INFO_36, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_37, IDC_STT_JIG_INFO_37, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_38, IDC_STT_JIG_INFO_38, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_39, IDC_STT_JIG_INFO_39, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_40, IDC_STT_JIG_INFO_40, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_41, IDC_STT_JIG_INFO_41, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_42, IDC_STT_JIG_INFO_42, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_43, IDC_STT_JIG_INFO_43, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_44, IDC_STT_JIG_INFO_44, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_45, IDC_STT_JIG_INFO_45, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_46, IDC_STT_JIG_INFO_46, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_47, IDC_STT_JIG_INFO_47, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_48, IDC_STT_JIG_INFO_48, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
#ifndef DEF_EIN_48_LCA
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_49, IDC_STT_JIG_INFO_49, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_50, IDC_STT_JIG_INFO_50, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_51, IDC_STT_JIG_INFO_51, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_52, IDC_STT_JIG_INFO_52, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_53, IDC_STT_JIG_INFO_53, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_54, IDC_STT_JIG_INFO_54, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_55, IDC_STT_JIG_INFO_55, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
	ON_EVENT_RANGE(CDlgProductInfo, IDC_STT_JIG_INFO_56, IDC_STT_JIG_INFO_56, DISPID_CLICK, OnClickBtnSelect, VTS_I4)
#endif
END_EVENTSINK_MAP()

CDlgProductInfo::CDlgProductInfo(CWnd* pParent) : CDialog(CDlgProductInfo::IDD, pParent)
{
}

BOOL CDlgProductInfo::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgProductInfo::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgProductInfo::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgProductInfo::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	m_nResID[0] = IDC_STT_JIG_INFO_01;
	m_nResID[1] = IDC_STT_JIG_INFO_02;
	m_nResID[2] = IDC_STT_JIG_INFO_03;
	m_nResID[3] = IDC_STT_JIG_INFO_04;
	m_nResID[4] = IDC_STT_JIG_INFO_05;
	m_nResID[5] = IDC_STT_JIG_INFO_06;
	m_nResID[6] = IDC_STT_JIG_INFO_07;
	m_nResID[7] = IDC_STT_JIG_INFO_08;
	m_nResID[8] = IDC_STT_JIG_INFO_09;
	m_nResID[9] = IDC_STT_JIG_INFO_10;
	m_nResID[10] = IDC_STT_JIG_INFO_11;
	m_nResID[11] = IDC_STT_JIG_INFO_12;
	m_nResID[12] = IDC_STT_JIG_INFO_13;
	m_nResID[13] = IDC_STT_JIG_INFO_14;
	m_nResID[14] = IDC_STT_JIG_INFO_15;
	m_nResID[15] = IDC_STT_JIG_INFO_16;
	m_nResID[16] = IDC_STT_JIG_INFO_17;
	m_nResID[17] = IDC_STT_JIG_INFO_18;
	m_nResID[18] = IDC_STT_JIG_INFO_19;
	m_nResID[19] = IDC_STT_JIG_INFO_20;
	m_nResID[20] = IDC_STT_JIG_INFO_21;
	m_nResID[21] = IDC_STT_JIG_INFO_22;
	m_nResID[22] = IDC_STT_JIG_INFO_23;
	m_nResID[23] = IDC_STT_JIG_INFO_24;
	m_nResID[24] = IDC_STT_JIG_INFO_25;
	m_nResID[25] = IDC_STT_JIG_INFO_26;
	m_nResID[26] = IDC_STT_JIG_INFO_27;
	m_nResID[27] = IDC_STT_JIG_INFO_28;
	m_nResID[28] = IDC_STT_JIG_INFO_29;
	m_nResID[29] = IDC_STT_JIG_INFO_30;
	m_nResID[30] = IDC_STT_JIG_INFO_31;
	m_nResID[31] = IDC_STT_JIG_INFO_32;
	m_nResID[32] = IDC_STT_JIG_INFO_33;
	m_nResID[33] = IDC_STT_JIG_INFO_34;
	m_nResID[34] = IDC_STT_JIG_INFO_35;
	m_nResID[35] = IDC_STT_JIG_INFO_36;
	m_nResID[36] = IDC_STT_JIG_INFO_37;
	m_nResID[37] = IDC_STT_JIG_INFO_38;
	m_nResID[38] = IDC_STT_JIG_INFO_39;
	m_nResID[39] = IDC_STT_JIG_INFO_40;
	m_nResID[40] = IDC_STT_JIG_INFO_41;
	m_nResID[41] = IDC_STT_JIG_INFO_42;
	m_nResID[42] = IDC_STT_JIG_INFO_43;
	m_nResID[43] = IDC_STT_JIG_INFO_44;
	m_nResID[44] = IDC_STT_JIG_INFO_45;
	m_nResID[45] = IDC_STT_JIG_INFO_46;
	m_nResID[46] = IDC_STT_JIG_INFO_47;
	m_nResID[47] = IDC_STT_JIG_INFO_48;
#ifndef DEF_EIN_48_LCA
	m_nResID[48] = IDC_STT_JIG_INFO_49;
	m_nResID[49] = IDC_STT_JIG_INFO_50;
	m_nResID[50] = IDC_STT_JIG_INFO_51;
	m_nResID[51] = IDC_STT_JIG_INFO_52;
	m_nResID[52] = IDC_STT_JIG_INFO_53;
	m_nResID[53] = IDC_STT_JIG_INFO_54;
	m_nResID[54] = IDC_STT_JIG_INFO_55;
	m_nResID[55] = IDC_STT_JIG_INFO_56;
#endif

	m_nSelected = 0;
	m_bIncludeRetest = TRUE;
	UpdateSelect();

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgProductInfo::OnClose() 
{
	DestroyWindow();
}

void CDlgProductInfo::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		KillTimer(0);

		UpdateInfo();

		SetTimer(0, 100, NULL);
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgProductInfo::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgProductInfo::OnClickBtnResetJig()
{
	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));

	if( m_nSelected < DEF_MAX_JIG_ONE_SIDE )
	{
		pLeftTRStn->SetJigPassCnt(m_nSelected + 1, 0);
		pLeftTRStn->SetJigFailCnt(m_nSelected + 1, 0);
		pLeftTRStn->SetJigRetestPassCnt(m_nSelected + 1, 0);
		pLeftTRStn->SetJigRetestFailCnt(m_nSelected + 1, 0);
		pLeftTRStn->SaveProfileJigSystem();
	}
	else
	{
		pRightTRStn->SetJigPassCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE, 0);
		pRightTRStn->SetJigFailCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE, 0);
		pRightTRStn->SetJigRetestPassCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE, 0);
		pRightTRStn->SetJigRetestFailCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE, 0);
		pRightTRStn->SaveProfileJigSystem();
	}
}

void CDlgProductInfo::OnClickBtnResetAll()
{
	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));

	for( int i = 1; i <= DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		pLeftTRStn->SetJigPassCnt(i, 0);
		pLeftTRStn->SetJigFailCnt(i, 0);
		pLeftTRStn->SetJigRetestPassCnt(i, 0);
		pLeftTRStn->SetJigRetestFailCnt(i, 0);

		pRightTRStn->SetJigPassCnt(i, 0);
		pRightTRStn->SetJigFailCnt(i, 0);
		pRightTRStn->SetJigRetestPassCnt(i, 0);
		pRightTRStn->SetJigRetestFailCnt(i, 0);
	}

	pLeftTRStn->SaveProfileJigSystem();
	pRightTRStn->SaveProfileJigSystem();
}

void CDlgProductInfo::OnClickBtnIncludeRetest()
{
	m_bIncludeRetest = !m_bIncludeRetest;
	UpdateSelect();
}

void CDlgProductInfo::OnClickBtnExit()
{
	OnCancel();
}

void CDlgProductInfo::OnClickBtnSelect(UINT nID)
{
	for( int i = 0; i < DEF_MAX_JIG; i++ )
	{
		if( nID == m_nResID[i] )
		{
			m_nSelected = i;
			UpdateSelect();
			break;
		}
	}
}

void CDlgProductInfo::UpdateSelect()
{
	for( int i = 0; i < DEF_MAX_JIG; i++ )
	{
		if( m_nSelected == i ) ((CBtnEnh*)GetDlgItem(m_nResID[i]))->SetBackColorInterior(COLOR_YELLOW);
		else ((CBtnEnh*)GetDlgItem(m_nResID[i]))->SetBackColorInterior(COLOR_WHITE);
	}

	((CBtnEnh*)GetDlgItem(IDC_BTN_INCLUDE_RETEST))->SetBackColorInterior(m_bIncludeRetest ? COLOR_YELLOW : COLOR_WHITE);
}

void CDlgProductInfo::UpdateInfo()
{
	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));
	CString sCaption = _T("");
	double dRate = 0.0;
	int nTotal = 0;
	int nPass = 0;
	int nFail = 0;

	if( m_nSelected < DEF_MAX_JIG_ONE_SIDE )
	{
		nPass += pLeftTRStn->GetJigPassCnt(m_nSelected + 1);
		if( m_bIncludeRetest ) nPass += pLeftTRStn->GetJigRetestPassCnt(m_nSelected + 1);
	}
	else
	{
		nPass += pRightTRStn->GetJigPassCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE);
		if( m_bIncludeRetest ) nPass += pRightTRStn->GetJigRetestPassCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE);
	}

	if( m_nSelected < DEF_MAX_JIG_ONE_SIDE )
	{
		nFail += pLeftTRStn->GetJigFailCnt(m_nSelected + 1);
		if( m_bIncludeRetest ) nFail += pLeftTRStn->GetJigRetestFailCnt(m_nSelected + 1);
	}
	else
	{
		nFail += pRightTRStn->GetJigFailCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE);
		if( m_bIncludeRetest ) nFail += pRightTRStn->GetJigRetestFailCnt(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE);
	}

	nTotal = nPass + nFail;
	if( nTotal > 0 ) dRate = (double)nFail / (double)nTotal * 100.0;

	sCaption.Format(_T("%d ea"), nTotal);
	((CBtnEnh*)GetDlgItem(IDC_STT_JIG_TOTAL))->SetCaption(sCaption);

	sCaption.Format(_T("%d ea"), nPass);
	((CBtnEnh*)GetDlgItem(IDC_STT_JIG_PASS))->SetCaption(sCaption);

	sCaption.Format(_T("%d ea"), nFail);
	((CBtnEnh*)GetDlgItem(IDC_STT_JIG_FAIL))->SetCaption(sCaption);

	sCaption.Format(_T("%.01lf %%"), dRate);
	((CBtnEnh*)GetDlgItem(IDC_STT_JIG_FAILRATE))->SetCaption(sCaption);

	double dTestTime = 0.0;

	if( m_nSelected < DEF_MAX_JIG_ONE_SIDE ) dTestTime = pLeftTRStn->GetJigAvgTestTime(m_nSelected + 1);
	else dTestTime = pRightTRStn->GetJigAvgTestTime(m_nSelected + 1 - DEF_MAX_JIG_ONE_SIDE);

	sCaption.Format(_T("%.01lf s"), dTestTime);
	((CBtnEnh*)GetDlgItem(IDC_STT_JIG_TESTTIME))->SetCaption(sCaption);

	nPass = 0;
	nFail = 0;
	dRate = 0.0;

	for( int i = 1; i <= DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		nPass += pLeftTRStn->GetJigPassCnt(i);
		nPass += pRightTRStn->GetJigPassCnt(i);
		if( m_bIncludeRetest ) nPass += pLeftTRStn->GetJigRetestPassCnt(i);
		if( m_bIncludeRetest ) nPass += pRightTRStn->GetJigRetestPassCnt(i);

		nFail += pLeftTRStn->GetJigFailCnt(i);
		nFail += pRightTRStn->GetJigFailCnt(i);
		if( m_bIncludeRetest ) nFail += pLeftTRStn->GetJigRetestFailCnt(i);
		if( m_bIncludeRetest ) nFail += pRightTRStn->GetJigRetestFailCnt(i);
	}

	nTotal = nPass + nFail;
	if( nTotal > 0 ) dRate = (double)nFail / (double)nTotal * 100.0;

	sCaption.Format(_T("%d ea"), nTotal);
	((CBtnEnh*)GetDlgItem(IDC_STT_TOTAL_TOTAL))->SetCaption(sCaption);

	sCaption.Format(_T("%d ea"), nPass);
	((CBtnEnh*)GetDlgItem(IDC_STT_TOTAL_PASS))->SetCaption(sCaption);

	sCaption.Format(_T("%d ea"), nFail);
	((CBtnEnh*)GetDlgItem(IDC_STT_TOTAL_FAIL))->SetCaption(sCaption);

	sCaption.Format(_T("%.01lf %%"), dRate);
	((CBtnEnh*)GetDlgItem(IDC_STT_TOTAL_FAILRATE))->SetCaption(sCaption);

	double dTotalAvgTestTime = 0.0;

	for( int i = 1; i <= DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		dTotalAvgTestTime += pLeftTRStn->GetJigAvgTestTime(i);
		dTotalAvgTestTime += pRightTRStn->GetJigAvgTestTime(i);
	}

	dTotalAvgTestTime = dTotalAvgTestTime / (double)DEF_MAX_JIG;

	sCaption.Format(_T("%.01lf s"), dTotalAvgTestTime);
	((CBtnEnh*)GetDlgItem(IDC_STT_TOTAL_TESTTIME))->SetCaption(sCaption);
}
