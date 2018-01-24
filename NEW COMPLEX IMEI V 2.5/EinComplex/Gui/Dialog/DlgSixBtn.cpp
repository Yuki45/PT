#include "stdafx.h"
#include "EinComplex.h"
#include "DlgSixBtn.h"
#include "afxdialogex.h"

IMPLEMENT_DYNAMIC(CDlgSixBtn, CDialog)

CDlgSixBtn::CDlgSixBtn(CString strTitle, CString strOne, CString strTwo, CString strThree, CString strFour, CString strFive, CString strSix, CWnd* pParent)
	: CDialog(CDlgSixBtn::IDD, pParent)
{
	m_brBckColor.CreateSolidBrush(COLOR_GRAY);
	m_strTitle = strTitle;
	m_strOne = strOne;
	m_strTwo = strTwo;
	m_strThree = strThree;
	m_strFour = strFour;
	m_strFive = strFive;
	m_strSix = strSix;

	m_bClickBtn = FALSE;
}

CDlgSixBtn::CDlgSixBtn(CWnd* pParent)
	: CDialog(CDlgSixBtn::IDD, pParent)
{
	m_brBckColor.CreateSolidBrush(COLOR_GRAY);
	m_strTitle = "";
	m_strOne = "";
	m_strTwo = "";
	m_strThree = "";
	m_strFour = "";
	m_strFive = "";
	m_strSix = "";

	m_bClickBtn = FALSE;
}

CDlgSixBtn::~CDlgSixBtn()
{
}

void CDlgSixBtn::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgSixBtn, CDialog)
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgSixBtn, CDialog)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON1, DISPID_CLICK, CDlgSixBtn::ClickButton1, VTS_NONE)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON2, DISPID_CLICK, CDlgSixBtn::ClickButton2, VTS_NONE)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON3, DISPID_CLICK, CDlgSixBtn::ClickButton3, VTS_NONE)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON4, DISPID_CLICK, CDlgSixBtn::ClickButton4, VTS_NONE)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON5, DISPID_CLICK, CDlgSixBtn::ClickButton5, VTS_NONE)
	ON_EVENT(CDlgSixBtn, IDC_BUTTON6, DISPID_CLICK, CDlgSixBtn::ClickButton6, VTS_NONE)
END_EVENTSINK_MAP()

void CDlgSixBtn::ClickButton1()
{
	m_bClickBtn = TRUE;
	EndDialog(0);
}

void CDlgSixBtn::ClickButton2()
{
	m_bClickBtn = TRUE;
	EndDialog(1);
}

void CDlgSixBtn::ClickButton3()
{
	if( AfxMessageBox(_T("Are you sure to reset jig?"), MB_OKCANCEL) != IDOK ) return;

	m_bClickBtn = TRUE;
	EndDialog(2);
}

void CDlgSixBtn::ClickButton4()
{
	m_bClickBtn = TRUE;
	EndDialog(3);
}

void CDlgSixBtn::ClickButton5()
{
	m_bClickBtn = TRUE;
	EndDialog(4);
}

void CDlgSixBtn::ClickButton6()
{
	if( AfxMessageBox(_T("Are you sure to reset jig?"), MB_OKCANCEL) != IDOK ) return;

	m_bClickBtn = TRUE;
	EndDialog(5);
}

void CDlgSixBtn::SetItemText(CString strTitle, CString strOne, CString strTwo, CString strThree, CString strFour, CString strFive, CString strSix)
{
	m_strTitle = strTitle;
	m_strOne = strOne;
	m_strTwo = strTwo;
	m_strThree = strThree;
	m_strFour = strFour;
	m_strFive = strFive;
	m_strSix = strSix;
}

BOOL CDlgSixBtn::OnInitDialog()
{
	CDialog::OnInitDialog();

	GetDlgItem(IDC_BUTTON1)->SetWindowText(m_strOne);
	GetDlgItem(IDC_BUTTON2)->SetWindowText(m_strTwo);
	GetDlgItem(IDC_BUTTON3)->SetWindowText(m_strThree);
	GetDlgItem(IDC_BUTTON4)->SetWindowText(m_strFour);
	GetDlgItem(IDC_BUTTON5)->SetWindowText(m_strFive);
	GetDlgItem(IDC_BUTTON6)->SetWindowText(m_strSix);

	SetWindowText(m_strTitle);

	return TRUE;
}
