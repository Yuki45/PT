#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgInit.h"
#include "LeftTransferStation.h"
#include "LeftPPStation.h"
#include "LeftNGCVStation.h"
#include "RightTransferStation.h"
#include "RightPPStation.h"
#include "RightNGCVStation.h"
#include "LoadCVStation.h"
#include "UnloadCVStation.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgInit::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgInit, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgInit, CDialog)
	ON_EVENT(CDlgInit, IDC_BTN_LEFT_TR, DISPID_CLICK, OnClickBtnLeftTR, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_LEFT_PP, DISPID_CLICK, OnClickBtnLeftPP, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_LEFT_NG_CV, DISPID_CLICK, OnClickBtnLeftNGCV, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_RIGHT_TR, DISPID_CLICK, OnClickBtnRightTR, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_RIGHT_PP, DISPID_CLICK, OnClickBtnRightPP, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_RIGHT_NG_CV, DISPID_CLICK, OnClickBtnRightNGCV, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_LOAD_CV, DISPID_CLICK, OnClickBtnLoadCV, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_UNLOAD_CV, DISPID_CLICK, OnClickBtnUnloadCV, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_SELECT_ALL, DISPID_CLICK, OnClickBtnSelectAll, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_INIT, DISPID_CLICK, OnClickBtnInit, VTS_NONE)
	ON_EVENT(CDlgInit, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgInit::CDlgInit(CWnd* pParent) : CDialog(CDlgInit::IDD, pParent)
{
}

BOOL CDlgInit::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgInit::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgInit::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgInit::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_bSelLeftTR    = FALSE;
	m_bSelLeftPP    = FALSE;
	m_bSelLeftNGCV  = FALSE;
	m_bSelRightTR   = FALSE;
	m_bSelRightPP   = FALSE;
	m_bSelRightNGCV = FALSE;
	m_bSelLoadCV    = FALSE;
	m_bSelUnloadCV  = FALSE;

	UpdateSelect();

	return TRUE;
}

void CDlgInit::OnClose() 
{
	DestroyWindow();
}

void CDlgInit::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgInit::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgInit::OnClickBtnLeftTR()
{
	m_bSelLeftTR = !m_bSelLeftTR;

	UpdateSelect();
}

void CDlgInit::OnClickBtnLeftPP()
{
	m_bSelLeftPP = !m_bSelLeftPP;

	UpdateSelect();
}

void CDlgInit::OnClickBtnLeftNGCV()
{
	m_bSelLeftNGCV = !m_bSelLeftNGCV;

	UpdateSelect();
}

void CDlgInit::OnClickBtnRightTR()
{
	m_bSelRightTR = !m_bSelRightTR;

	UpdateSelect();
}

void CDlgInit::OnClickBtnRightPP()
{
	m_bSelRightPP = !m_bSelRightPP;

	UpdateSelect();
}

void CDlgInit::OnClickBtnRightNGCV()
{
	m_bSelRightNGCV = !m_bSelRightNGCV;

	UpdateSelect();
}

void CDlgInit::OnClickBtnLoadCV()
{
	m_bSelLoadCV = !m_bSelLoadCV;

	UpdateSelect();
}

void CDlgInit::OnClickBtnUnloadCV()
{
	m_bSelUnloadCV = !m_bSelUnloadCV;

	UpdateSelect();
}

void CDlgInit::OnClickBtnSelectAll()
{
	if( (m_bSelLeftTR    == TRUE) &&
		(m_bSelLeftPP    == TRUE) &&
		(m_bSelLeftNGCV  == TRUE) &&
		(m_bSelRightTR   == TRUE) &&
		(m_bSelRightPP   == TRUE) &&
		(m_bSelRightNGCV == TRUE) &&
		(m_bSelLoadCV    == TRUE) &&
		(m_bSelUnloadCV  == TRUE) )
	{
		m_bSelLeftTR    = FALSE;
		m_bSelLeftPP    = FALSE;
		m_bSelLeftNGCV  = FALSE;
		m_bSelRightTR   = FALSE;
		m_bSelRightPP   = FALSE;
		m_bSelRightNGCV = FALSE;
		m_bSelLoadCV    = FALSE;
		m_bSelUnloadCV  = FALSE;
	}
	else
	{
		m_bSelLeftTR    = TRUE;
		m_bSelLeftPP    = TRUE;
		m_bSelLeftNGCV  = TRUE;
		m_bSelRightTR   = TRUE;
		m_bSelRightPP   = TRUE;
		m_bSelRightNGCV = TRUE;
		m_bSelLoadCV    = TRUE;
		m_bSelUnloadCV  = TRUE;
	}

	UpdateSelect();
}

void CDlgInit::OnClickBtnInit()
{
	CMainControlStation* pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	CLeftTransferStation* pLeftTRStn = (CLeftTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftTransferStation"));
	CLeftPPStation* pLeftPPStn = (CLeftPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftPPStation"));
	CLeftNGCVStation* pLeftNGCVStn = (CLeftNGCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("LeftNGCVStation"));
	CRightTransferStation* pRightTRStn = (CRightTransferStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightTransferStation"));
	CRightPPStation* pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	CRightNGCVStation* pRightNGCVStn = (CRightNGCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightNGCVStation"));
	CLoadCVStation* pLoadCVStn = (CLoadCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("LoadCVStation"));
	CUnloadCVStation* pUnloadCVStn = (CUnloadCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("UnloadCVStation"));

	if( m_bSelLeftTR && m_bSelLeftPP && m_bSelLeftNGCV &&
		m_bSelRightTR && m_bSelRightPP && m_bSelRightNGCV &&
		m_bSelLoadCV && m_bSelUnloadCV )
	{
		pMainStn->m_nStateLeft = MS_IDLE;
		pMainStn->m_nStateRight = MS_IDLE;
		CAxMaster::GetMaster()->Abort();
		OnOK();
	}

	if( m_bSelLeftTR ) pLeftTRStn->InitVariable();
	if( m_bSelLeftPP ) pLeftPPStn->InitVariable();
	if( m_bSelLeftNGCV ) pLeftNGCVStn->InitVariable();
	if( m_bSelRightTR ) pRightTRStn->InitVariable();
	if( m_bSelRightPP ) pRightPPStn->InitVariable();
	if( m_bSelRightNGCV ) pRightNGCVStn->InitVariable();
	if( m_bSelLoadCV ) pLoadCVStn->InitVariable();
	if( m_bSelUnloadCV ) pUnloadCVStn->InitVariable();

	OnOK();
}

void CDlgInit::OnClickBtnExit()
{
	OnCancel();
}

void CDlgInit::UpdateSelect()
{
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_TR))->SetBackColorInterior(m_bSelLeftTR ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_PP))->SetBackColorInterior(m_bSelLeftPP ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LEFT_NG_CV))->SetBackColorInterior(m_bSelLeftNGCV ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_TR))->SetBackColorInterior(m_bSelRightTR ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_PP))->SetBackColorInterior(m_bSelRightPP ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_RIGHT_NG_CV))->SetBackColorInterior(m_bSelRightNGCV ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_LOAD_CV))->SetBackColorInterior(m_bSelLoadCV ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_UNLOAD_CV))->SetBackColorInterior(m_bSelUnloadCV ? COLOR_YELLOW : COLOR_WHITE);
}
