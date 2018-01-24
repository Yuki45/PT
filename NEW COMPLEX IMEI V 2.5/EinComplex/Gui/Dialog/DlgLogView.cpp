#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgLogView.h"

#include <BtnEnh.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgLogView::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);

	DDX_Control(pDX, IDC_LIST_LOG, m_listLog);
}

BEGIN_MESSAGE_MAP(CDlgLogView, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgLogView, CDialog)
	ON_EVENT(CDlgLogView, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgLogView::CDlgLogView(CWnd* pParent) : CDialog(CDlgLogView::IDD, pParent)
{
}

BOOL CDlgLogView::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgLogView::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgLogView::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgLogView::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_font.CreateFont(18, 0, 0, 0, FW_NORMAL, FALSE, FALSE, 0, DEFAULT_CHARSET,
					  OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY,
					  DEFAULT_PITCH | FF_SWISS, _T("Malgun Gothic"));

	m_listLog.SetFont(&m_font);

	UpdateLog();

	return TRUE;
}

void CDlgLogView::OnClose() 
{
	DestroyWindow();
}

void CDlgLogView::OnTimer(UINT nIDEvent) 
{
	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgLogView::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgLogView::OnClickBtnExit()
{
	OnCancel();
}

void CDlgLogView::UpdateLog()
{
	CString sFileName;
	CFileFind ff;
	CFile cFile;
	CString sText;
	int nLastEnterIdx;

	CString sDriveName = _T("");
	char szBuffer[100];
	::GetCurrentDirectory(100, szBuffer);
	sDriveName.Format(_T("%s"), szBuffer);

	m_Date = COleDateTime::GetCurrentTime();

	sFileName.Format(_T("%s\\EinComplex\\Data\\Trace\\%s\\Error.log"), sDriveName.Left(2), m_Date.Format(_T("%Y%m%d")));
	if( !ff.FindFile(sFileName) ) return;
	if( !cFile.Open(sFileName, CFile::modeRead) ) return;
	cFile.Read(sText.GetBuffer((int)cFile.GetLength()), (UINT)cFile.GetLength());
	sText.ReleaseBuffer();

	m_listLog.ResetContent();
	nLastEnterIdx = 0;
	for( int i = 0; i < (int)cFile.GetLength(); i++ )
	{
		if( sText.Mid(i, 1) == _T("\n") )
		{
			m_listLog.AddString(sText.Mid(nLastEnterIdx, i - nLastEnterIdx));
			m_listLog.SetCurSel(m_listLog.GetCount() - 1);
			nLastEnterIdx = i;
		}
	}
}
