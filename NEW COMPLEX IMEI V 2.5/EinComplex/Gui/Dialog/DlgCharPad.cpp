#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgCharPad.h"

#include <BtnEnh.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgCharPad::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgCharPad, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgCharPad, CDialog)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM1,	IDC_BTN_NUM1,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM2,	IDC_BTN_NUM2,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM3,	IDC_BTN_NUM3,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM4,	IDC_BTN_NUM4,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM5,	IDC_BTN_NUM5,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM6,	IDC_BTN_NUM6,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM7,	IDC_BTN_NUM7,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM8,	IDC_BTN_NUM8,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM9,	IDC_BTN_NUM9,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_NUM0,	IDC_BTN_NUM0,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_HYPHEN,	IDC_BTN_HYPHEN,	DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_Q,		IDC_BTN_Q,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_W,		IDC_BTN_W,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_E,		IDC_BTN_E,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_R,		IDC_BTN_R,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_T,		IDC_BTN_T,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_Y,		IDC_BTN_Y,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_U,		IDC_BTN_U,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_I,		IDC_BTN_I,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_O,		IDC_BTN_O,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_P,		IDC_BTN_P,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_A,		IDC_BTN_A,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_S,		IDC_BTN_S,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_D,		IDC_BTN_D,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_F,		IDC_BTN_F,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_G,		IDC_BTN_G,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_H,		IDC_BTN_H,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_J,		IDC_BTN_J,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_K,		IDC_BTN_K,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_L,		IDC_BTN_L,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_Z,		IDC_BTN_Z,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_X,		IDC_BTN_X,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_C,		IDC_BTN_C,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_V,		IDC_BTN_V,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_B,		IDC_BTN_B,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_N,		IDC_BTN_N,		DISPID_CLICK, OnClickKey, VTS_I4)
	ON_EVENT_RANGE(CDlgCharPad, IDC_BTN_M,		IDC_BTN_M,		DISPID_CLICK, OnClickKey, VTS_I4)

	ON_EVENT(CDlgCharPad, IDC_BTN_BACKSPACE,	DISPID_CLICK, OnClickBackspace,	VTS_NONE)
	ON_EVENT(CDlgCharPad, IDC_BTN_CLEAR,		DISPID_CLICK, OnClickClear,		VTS_NONE)
	ON_EVENT(CDlgCharPad, IDC_BTN_SHIFT,		DISPID_CLICK, OnClickShift,		VTS_NONE)
	ON_EVENT(CDlgCharPad, IDC_BTN_OK,			DISPID_CLICK, OnClickOK,		VTS_NONE)
	ON_EVENT(CDlgCharPad, IDC_BTN_CANCEL,		DISPID_CLICK, OnClickCancel,	VTS_NONE)
END_EVENTSINK_MAP()

CDlgCharPad::CDlgCharPad(CString sDefault, CWnd* pParent) : CDialog(CDlgCharPad::IDD, pParent)
{
	m_sInput = sDefault;
	m_bShift = FALSE;
	m_bFirstValue = TRUE;
}

BOOL CDlgCharPad::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgCharPad::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgCharPad::OnInitDialog() 
{
	CDialog::OnInitDialog();

	UINT nCtrlID[DEF_MAX_CHAR_KEY] =
	{
		IDC_BTN_NUM1,	IDC_BTN_NUM2,	IDC_BTN_NUM3,	IDC_BTN_NUM4,	IDC_BTN_NUM5,
		IDC_BTN_NUM6,	IDC_BTN_NUM7,	IDC_BTN_NUM8,	IDC_BTN_NUM9,	IDC_BTN_NUM0,
		IDC_BTN_HYPHEN,	IDC_BTN_Q,		IDC_BTN_W,		IDC_BTN_E,		IDC_BTN_R,
		IDC_BTN_T,		IDC_BTN_Y,		IDC_BTN_U,		IDC_BTN_I,		IDC_BTN_O,
		IDC_BTN_P,		IDC_BTN_A,		IDC_BTN_S,		IDC_BTN_D,		IDC_BTN_F,
		IDC_BTN_G,		IDC_BTN_H,		IDC_BTN_J,		IDC_BTN_K,		IDC_BTN_L,
		IDC_BTN_Z,		IDC_BTN_X,		IDC_BTN_C,		IDC_BTN_V,		IDC_BTN_B,
		IDC_BTN_N,		IDC_BTN_M
	};

	int nNormalValue[DEF_MAX_CHAR_KEY] =
	{
		_T('1'), _T('2'), _T('3'), _T('4'), _T('5'),
		_T('6'), _T('7'), _T('8'), _T('9'), _T('0'),
		_T('-'), _T('q'), _T('w'), _T('e'), _T('r'),
		_T('t'), _T('y'), _T('u'), _T('i'), _T('o'),
		_T('p'), _T('a'), _T('s'), _T('d'), _T('f'),
		_T('g'), _T('h'), _T('j'), _T('k'), _T('l'),
		_T('z'), _T('x'), _T('c'), _T('v'), _T('b'),
		_T('n'), _T('m')
	};

	int nShiftValue[DEF_MAX_CHAR_KEY] =
	{
		_T('1'), _T('2'), _T('3'), _T('4'), _T('5'),
		_T('6'), _T('7'), _T('8'), _T('9'), _T('0'),
		_T('_'), _T('Q'), _T('W'), _T('E'), _T('R'),
		_T('T'), _T('Y'), _T('U'), _T('I'), _T('O'),
		_T('P'), _T('A'), _T('S'), _T('D'), _T('F'),
		_T('G'), _T('H'), _T('J'), _T('K'), _T('L'),
		_T('Z'), _T('X'), _T('C'), _T('V'), _T('B'),
		_T('N'), _T('M')
	};

	for( int i = 0; i < DEF_MAX_CHAR_KEY; i++ )
	{
		m_nCtrlID[i] = nCtrlID[i];
		m_nNormalValue[i] = nNormalValue[i];
		m_nShiftValue[i] = nShiftValue[i];
	}

	UpdateDisplay();
	UpdateKeyboard();

	return TRUE;
}

void CDlgCharPad::OnClose() 
{
	DestroyWindow();
}

void CDlgCharPad::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgCharPad::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

BOOL CDlgCharPad::PreTranslateMessage(MSG* pMsg) 
{
	if( pMsg->message == WM_KEYDOWN )
	{
		if( (pMsg->wParam >= _T('0') && pMsg->wParam <= _T('9')) ||
			(pMsg->wParam >= _T('a') && pMsg->wParam <= _T('z')) ||
			(pMsg->wParam >= _T('A') && pMsg->wParam <= _T('Z')) )
		{
			OnClickKey(GetCtrlID((char)pMsg->wParam));
			return TRUE;
		}
		else
		{
			switch( pMsg->wParam )
			{
				case DEF_CHAR_HYPHEN:	OnClickKey(IDC_BTN_HYPHEN);	return TRUE;
				case VK_BACK:			OnClickBackspace();			return TRUE;
				case DEF_CHAR_DELETE:	OnClickClear();				return TRUE;
				case VK_SHIFT:			OnClickShift();				return TRUE;
				case VK_RETURN:			OnClickOK();				return TRUE;
				case VK_ESCAPE:			OnClickCancel();			return TRUE;
			}
		}
	}

	return CDialog::PreTranslateMessage(pMsg);
}

void CDlgCharPad::OnClickKey(UINT nID)
{
	if( m_sInput.GetLength() > DEF_MAX_CHAR_INTPUT ) return;

	if( GetKeyIndex(nID) == -1 ) return;

	if( m_bFirstValue )
	{
		OnClickClear();
		m_bFirstValue = FALSE;
	}

	CString sAdd = _T("");
	sAdd.Format(_T("%c"), m_bShift ? m_nShiftValue[GetKeyIndex(nID)] : m_nNormalValue[GetKeyIndex(nID)]);
	
	m_sInput = m_sInput + sAdd;

	UpdateDisplay();
}

void CDlgCharPad::OnClickBackspace()
{
	if( m_sInput.GetLength() > 0 )
	{
		m_sInput = m_sInput.Left(m_sInput.GetLength() - 1);
		m_bFirstValue = FALSE;

		UpdateDisplay();
	}
}

void CDlgCharPad::OnClickClear()
{
	m_sInput = _T("");

	UpdateDisplay();
}

void CDlgCharPad::OnClickShift()
{
	m_bShift = !m_bShift;

	UpdateKeyboard();
}

void CDlgCharPad::OnClickOK()
{
	CDialog::OnOK();
}

void CDlgCharPad::OnClickCancel()
{
	CDialog::OnCancel();
}

void CDlgCharPad::UpdateDisplay()
{
	((CBtnEnh*)GetDlgItem(IDC_STT_DISPLAY))->SetCaption(m_sInput);
}

void CDlgCharPad::UpdateKeyboard()
{
	CString sCaption = _T("");

	for( int i = 0; i < DEF_MAX_CHAR_KEY; i++ )
	{
		sCaption.Format(_T("%c"), m_bShift ? m_nShiftValue[i] : m_nNormalValue[i]);
		((CBtnEnh*)GetDlgItem(m_nCtrlID[i]))->SetCaption(sCaption);
	}

	((CBtnEnh*)GetDlgItem(IDC_BTN_SHIFT))->SetBackColorInterior(m_bShift ? RGB(255, 255, 0) : RGB(255, 255, 191));
}

int CDlgCharPad::GetKeyIndex(UINT nCtrlID)
{
	for( int i = 0; i < DEF_MAX_CHAR_KEY; i++ )
	{
		if( nCtrlID == m_nCtrlID[i] ) return i; 
	}

	return -1;
}

int CDlgCharPad::GetCtrlID(char cKeyboard)
{
	int nKeyIndex = -1;

	for( int i = 0; i < DEF_MAX_CHAR_KEY; i++ )
	{
		if( (int)cKeyboard == m_nNormalValue[i] )
		{
			nKeyIndex = i;
			break;
		}

		if( (int)cKeyboard == m_nShiftValue[i] )
		{
			nKeyIndex = i;
			break;
		}
	}

	if( nKeyIndex == -1 ) return -1;

	return m_nCtrlID[nKeyIndex];
}

CString CDlgCharPad::GetData()
{
	return m_sInput;
}
