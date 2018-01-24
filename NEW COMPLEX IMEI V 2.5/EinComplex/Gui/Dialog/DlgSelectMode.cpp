#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgSelectMode.h"

#include <BtnEnh.h>
#include "MainFrm.h"
#include "MainControlStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgSelectMode::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgSelectMode, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgSelectMode, CDialog)
	ON_EVENT(CDlgSelectMode, IDC_BTN_NORMAL, DISPID_CLICK, OnClickBtnNormal, VTS_NONE)
	ON_EVENT(CDlgSelectMode, IDC_BTN_PASSRUN, DISPID_CLICK, OnClickBtnPassrun, VTS_NONE)
	ON_EVENT(CDlgSelectMode, IDC_BTN_DRYRUN, DISPID_CLICK, OnClickBtnDryrun, VTS_NONE)
	ON_EVENT(CDlgSelectMode, IDC_BTN_SIMULATION, DISPID_CLICK, OnClickBtnSimulation, VTS_NONE)
	ON_EVENT(CDlgSelectMode, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgSelectMode::CDlgSelectMode(CWnd* pParent) : CDialog(CDlgSelectMode::IDD, pParent)
{
}

BOOL CDlgSelectMode::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgSelectMode::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgSelectMode::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgSelectMode::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));

	UpdateMode();

	return TRUE;
}

void CDlgSelectMode::OnClose() 
{
	DestroyWindow();
}

void CDlgSelectMode::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgSelectMode::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgSelectMode::OnClickBtnNormal()
{
	m_pMainStn->SetNormalMode();
	m_pMainStn->SaveProfile();
	OnOK();
}

void CDlgSelectMode::OnClickBtnPassrun()
{
	m_pMainStn->SetBypassMode();
	m_pMainStn->SaveProfile();
	OnOK();
}

void CDlgSelectMode::OnClickBtnDryrun()
{
	m_pMainStn->SetDryRunMode();
	m_pMainStn->SaveProfile();
	OnOK();
}

void CDlgSelectMode::OnClickBtnSimulation()
{
	m_pMainStn->SetSimulateMode();
	m_pMainStn->SaveProfile();
	OnOK();
}

void CDlgSelectMode::OnClickBtnExit()
{
	OnCancel();
}

void CDlgSelectMode::UpdateMode()
{
	((CBtnEnh*)GetDlgItem(IDC_BTN_NORMAL))->SetBackColorInterior(m_pMainStn->IsNormalMode() ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_PASSRUN))->SetBackColorInterior(m_pMainStn->IsBypassMode() ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_DRYRUN))->SetBackColorInterior(m_pMainStn->IsDryRunMode() ? COLOR_YELLOW : COLOR_WHITE);
	((CBtnEnh*)GetDlgItem(IDC_BTN_SIMULATION))->SetBackColorInterior(m_pMainStn->IsSimulateMode() ? COLOR_YELLOW : COLOR_WHITE);
}
