#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgError.h"

#include <BtnEnh.h>
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgError::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgError, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgError, CDialog)
	ON_EVENT(CDlgError, IDC_BTN_RETRY, DISPID_CLICK, OnClickBtnRetry, VTS_NONE)
	ON_EVENT(CDlgError, IDC_BTN_EXIT, DISPID_CLICK, OnClickBtnExit, VTS_NONE)
END_EVENTSINK_MAP()

CDlgError::CDlgError(CWnd* pParent) : CDialog(CDlgError::IDD, pParent)
{
}

BOOL CDlgError::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgError::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgError::PreTranslateMessage(MSG* pMsg)
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

BOOL CDlgError::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
	m_nResponse = emNone;

	UpdateError();

	SetTimer(0, 100, NULL);

	return TRUE;
}

void CDlgError::OnClose() 
{
	KillTimer(0);

	DestroyWindow();
}

void CDlgError::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		if( (m_pMainFrm->m_pMainStn->m_nStateLeft != MS_ERROR) &&
			(m_pMainFrm->m_pMainStn->m_nStateRight != MS_ERROR) )
		{
			OnCancel();
		}
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgError::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgError::OnClickBtnRetry()
{
	m_nResponse = emRetry;
	OnOK();
}

void CDlgError::OnClickBtnExit()
{
	OnCancel();
}

void CDlgError::UpdateError()
{
	CString sErrorCode = _T("");
	CString sImageFileName = _T("");

	if( m_pMainFrm->m_sErrorAssy == _T("MainControlStation") )
	{
		if( m_pMainFrm->m_nErrorNum == 1 )
		{
			sErrorCode = _T("0101");
		}
		else if( m_pMainFrm->m_nErrorNum == 2 )
		{
			sErrorCode = _T("0102");
		}
		else if( m_pMainFrm->m_nErrorNum == 3 )
		{
			sErrorCode = _T("0103");
		}
	}
	else if( (m_pMainFrm->m_sErrorAssy == _T("LeftPPStation")) || (m_pMainFrm->m_sErrorAssy == _T("RightPPStation")) )
	{
		if( m_pMainFrm->m_nErrorNum == 1 )
		{
			sErrorCode = _T("0201");
		}
		else if( m_pMainFrm->m_nErrorNum == 2 )
		{
			sErrorCode = _T("0202");
		}
		else if( m_pMainFrm->m_nErrorNum == 3 )
		{
			sErrorCode = _T("0203");
		}
		else if( m_pMainFrm->m_nErrorNum == 4 )
		{
			sErrorCode = _T("0204");
		}
		else if( m_pMainFrm->m_nErrorNum == 5 )
		{
			sErrorCode = _T("0205");
		}
		else if( m_pMainFrm->m_nErrorNum == 6 )
		{
			sErrorCode = _T("0206");
		}
		else if( m_pMainFrm->m_nErrorNum == 7 )
		{
			sErrorCode = _T("0207");
		}
		else if( m_pMainFrm->m_nErrorNum == 8 )
		{
			sErrorCode = _T("0208");
		}
		else if( m_pMainFrm->m_nErrorNum == 9 )
		{
			sErrorCode = _T("0209");
		}
	}
	else if( (m_pMainFrm->m_sErrorAssy == _T("LeftTransferStation")) || (m_pMainFrm->m_sErrorAssy == _T("RightTransferStation")) )
	{
		if( m_pMainFrm->m_nErrorNum == 1 )
		{
			sErrorCode = _T("0301");
		}
		else if( m_pMainFrm->m_nErrorNum == 2 )
		{
			sErrorCode = _T("0302");
		}
		else if( m_pMainFrm->m_nErrorNum == 3 )
		{
			sErrorCode = _T("0303");
		}
		else if( m_pMainFrm->m_nErrorNum == 4 )
		{
			sErrorCode = _T("0304");
		}
		else if( m_pMainFrm->m_nErrorNum == 5 )
		{
			sErrorCode = _T("0305");
		}
		else if( m_pMainFrm->m_nErrorNum == 6 )
		{
			sErrorCode = _T("0306");
		}
	}

	CString sDriveName = _T("");
	char szBuffer[100];
	::GetCurrentDirectory(100, szBuffer);
	sDriveName.Format(_T("%s"), szBuffer);

	sImageFileName.Format(_T("%s\\EinComplex\\Data\\Image\\%s.jpg"), sDriveName.Left(2), m_pMainFrm->m_sErrorSol1);

	((CBtnEnh*)GetDlgItem(IDC_STT_ASSY))->SetCaption(m_pMainFrm->m_sErrorAssy);
	((CBtnEnh*)GetDlgItem(IDC_STT_PART))->SetCaption(m_pMainFrm->m_sErrorPart);
	((CBtnEnh*)GetDlgItem(IDC_STT_CONT))->SetCaption(m_pMainFrm->m_sErrorCont);
	((CBtnEnh*)GetDlgItem(IDC_STT_IMAGE))->SetPicture(sImageFileName);

	m_pMainFrm->m_pMainStn->m_trcError.Log(_T("[%s] %s"), m_pMainFrm->m_sErrorAssy, m_pMainFrm->m_sErrorPart);
	if( sErrorCode != _T("") ) m_pMainFrm->MMMSLog(sErrorCode);
}
