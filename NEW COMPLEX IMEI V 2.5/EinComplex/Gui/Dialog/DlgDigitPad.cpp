#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgDigitPad.h"

#include <BtnEnh.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgDigitPad::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgDigitPad, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgDigitPad, CDialog)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM0, IDC_BTN_NUM0, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM1, IDC_BTN_NUM1, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM2, IDC_BTN_NUM2, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM3, IDC_BTN_NUM3, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM4, IDC_BTN_NUM4, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM5, IDC_BTN_NUM5, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM6, IDC_BTN_NUM6, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM7, IDC_BTN_NUM7, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM8, IDC_BTN_NUM8, DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgDigitPad, IDC_BTN_NUM9, IDC_BTN_NUM9, DISPID_CLICK, OnClickKey, VTS_I4)

	ON_EVENT(CDlgDigitPad, IDC_BTN_BACKSPACE,	DISPID_CLICK, OnClickBackspace,	VTS_NONE)
	ON_EVENT(CDlgDigitPad, IDC_BTN_CLEAR,		DISPID_CLICK, OnClickClear,		VTS_NONE)
	ON_EVENT(CDlgDigitPad, IDC_BTN_DOT,			DISPID_CLICK, OnClickDot,		VTS_NONE)
	ON_EVENT(CDlgDigitPad, IDC_BTN_PLUS_MINUS,	DISPID_CLICK, OnClickPlusMinus,	VTS_NONE)
	ON_EVENT(CDlgDigitPad, IDC_BTN_OK,			DISPID_CLICK, OnClickOK,		VTS_NONE)
	ON_EVENT(CDlgDigitPad, IDC_BTN_CANCEL,		DISPID_CLICK, OnClickCancel,	VTS_NONE)
END_EVENTSINK_MAP()

CDlgDigitPad::CDlgDigitPad(double dDefault, CWnd* pParent) : CDialog(CDlgDigitPad::IDD, pParent)
{
	int nCompare = (int)(dDefault * 1000.0);

	if( (nCompare % 1000) == 0 ) m_sInput.Format(_T("%.0lf"), dDefault);
	else m_sInput.Format(_T("%.3lf"), dDefault);

	m_bFirstValue = TRUE;
}

BOOL CDlgDigitPad::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgDigitPad::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgDigitPad::OnInitDialog() 
{
	CDialog::OnInitDialog();

	UINT nCtrlID[DEF_MAX_DIGIT_KEY] =
	{
		IDC_BTN_NUM0, IDC_BTN_NUM1, IDC_BTN_NUM2, IDC_BTN_NUM3, IDC_BTN_NUM4,
		IDC_BTN_NUM5, IDC_BTN_NUM6, IDC_BTN_NUM7, IDC_BTN_NUM8, IDC_BTN_NUM9
	};

	for( int i = 0; i < DEF_MAX_DIGIT_KEY; i++ ) m_nCtrlID[i] = nCtrlID[i];

	UpdateDisplay();

	return TRUE;
}

void CDlgDigitPad::OnClose() 
{
	DestroyWindow();
}

void CDlgDigitPad::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgDigitPad::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

BOOL CDlgDigitPad::PreTranslateMessage(MSG* pMsg) 
{
	if( pMsg->message == WM_KEYDOWN )
	{
		if( pMsg->wParam >= _T('0') && pMsg->wParam <= _T('9') )
		{
			OnClickKey(GetCtrlID((char)pMsg->wParam));
			return TRUE;
		}
		else
		{
			switch( pMsg->wParam )
			{
				case VK_BACK:			OnClickBackspace();	return TRUE;
				case DEF_DIGIT_DELETE:	OnClickClear();		return TRUE;
				case DEF_DIGIT_DOT:		OnClickDot();		return TRUE;
				case VK_RETURN:			OnClickOK();		return TRUE;
				case VK_ESCAPE:			OnClickCancel();	return TRUE;
			}
		}
	}

	return CDialog::PreTranslateMessage(pMsg);
}

void CDlgDigitPad::OnClickKey(UINT nID)
{
	if( m_sInput.GetLength() > DEF_MAX_DIGIT_INTPUT ) return;

	if( GetKeyValue(nID) == -1 ) return;

	if( m_bFirstValue )
	{
		OnClickClear();
		m_bFirstValue = FALSE;
	}

	CString sAdd = _T("");
	sAdd.Format(_T("%d"), GetKeyValue(nID));

	m_sInput = m_sInput + sAdd;

	UpdateDisplay();
}

void CDlgDigitPad::OnClickBackspace()
{
	if( m_sInput.GetLength() > 0 )
	{
		m_sInput = m_sInput.Left(m_sInput.GetLength() - 1);
		m_bFirstValue = FALSE;

		UpdateDisplay();
	}
}

void CDlgDigitPad::OnClickClear()
{
	m_sInput = _T("");

	UpdateDisplay();
}

void CDlgDigitPad::OnClickDot()
{
	if( m_sInput.GetLength() > DEF_MAX_DIGIT_INTPUT ) return;
	
	if( (m_sInput.GetLength() > 0) &&
		(m_sInput.Find(_T(".")) == -1) )
	{
		m_sInput = m_sInput + _T(".");
		m_bFirstValue = FALSE;

		UpdateDisplay();
	}
}

void CDlgDigitPad::OnClickPlusMinus()
{
	if( m_sInput.GetLength() > DEF_MAX_DIGIT_INTPUT ) return;
	if( _tcstod(m_sInput, NULL) == 0.0 ) return;

	if( m_sInput.Left(1) == _T("-") ) m_sInput = m_sInput.Right(m_sInput.GetLength() - 1);
	else m_sInput = _T("-") + m_sInput;

	UpdateDisplay();
}

void CDlgDigitPad::OnClickOK()
{
	CDialog::OnOK();
}

void CDlgDigitPad::OnClickCancel()
{
	CDialog::OnCancel();
}

void CDlgDigitPad::UpdateDisplay()
{
	((CBtnEnh*)GetDlgItem(IDC_STT_DISPLAY))->SetCaption(m_sInput);
}

int CDlgDigitPad::GetKeyValue(UINT nCtrlID)
{
	for( int i = 0; i < DEF_MAX_DIGIT_KEY; i++ )
	{
		if( nCtrlID == m_nCtrlID[i] ) return i; 
	}

	return -1;
}

int CDlgDigitPad::GetCtrlID(char cKeyboard)
{
	CString sKey = _T("");
	sKey.Format(_T("%c"), cKeyboard);

	int nNumber = _ttoi(sKey.GetBuffer());
	sKey.ReleaseBuffer();

	if( (nNumber >= 0) &&
		(nNumber < DEF_MAX_DIGIT_KEY) ) return m_nCtrlID[nNumber];

	return -1;
}

double CDlgDigitPad::GetData()
{
	double dRet = 0.0;

	dRet = _tcstod(m_sInput.GetBuffer(), NULL);
	m_sInput.ReleaseBuffer();

	return dRet;
}
