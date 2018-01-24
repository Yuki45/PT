#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgSelectTeach.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgSelectTeach::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgSelectTeach, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgSelectTeach, CDialog)
	ON_EVENT(CDlgSelectTeach, IDC_BTN_LEFT_TR, DISPID_CLICK, OnClickBtnLeftTr, VTS_NONE)
	ON_EVENT(CDlgSelectTeach, IDC_BTN_RIGHT_TR, DISPID_CLICK, OnClickBtnRightTr, VTS_NONE)
	ON_EVENT(CDlgSelectTeach, IDC_BTN_PP, DISPID_CLICK, OnClickBtnPp, VTS_NONE)
	ON_EVENT(CDlgSelectTeach, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgSelectTeach::CDlgSelectTeach(CWnd* pParent) : CDialog(CDlgSelectTeach::IDD, pParent)
{
}

BOOL CDlgSelectTeach::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgSelectTeach::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgSelectTeach::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgSelectTeach::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_nSelected = 0;

	return TRUE;
}

void CDlgSelectTeach::OnClose() 
{
	DestroyWindow();
}

void CDlgSelectTeach::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgSelectTeach::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgSelectTeach::OnClickBtnLeftTr()
{
	m_nSelected = 1;
	OnOK();
}

void CDlgSelectTeach::OnClickBtnRightTr()
{
	m_nSelected = 2;
	OnOK();
}

void CDlgSelectTeach::OnClickBtnPp()
{
	m_nSelected = 3;
	OnOK();
}

void CDlgSelectTeach::OnClickBtnExit()
{
	OnCancel();
}
