#include "stdafx.h"
#include "afxwinappex.h"
#include "afxdialogex.h"
#include "EinComplex.h"
#include "MainFrm.h"

#include "EinComplexDoc.h"
#include "EinComplexView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BEGIN_MESSAGE_MAP(CEinComplexApp, CWinApp)
	ON_COMMAND(ID_FILE_NEW, &CWinApp::OnFileNew)
	ON_COMMAND(ID_FILE_OPEN, &CWinApp::OnFileOpen)
END_MESSAGE_MAP()

CEinComplexApp::CEinComplexApp()
{
	SetAppID(_T("EinComplex.AppID.NoVersion"));
}

CEinComplexApp theApp;

BOOL CEinComplexApp::InitInstance()
{
	HANDLE hMutex = NULL;
	hMutex = CreateMutex(NULL, TRUE, _T("Mutex_EinComplex"));

	if( GetLastError() == ERROR_ALREADY_EXISTS )
	{
		AfxMessageBox(_T("Program is already running!"));
		return FALSE;
	}

	CWinApp::InitInstance();

	if( !AfxOleInit() )
	{
		AfxMessageBox(IDP_OLE_INIT_FAILED);
		return FALSE;
	}

	AfxEnableControlContainer();
	EnableTaskbarInteraction(FALSE);
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));
	LoadStdProfileSettings(0);

	CSingleDocTemplate* pDocTemplate;
	pDocTemplate = new CSingleDocTemplate(IDR_MAINFRAME,
										  RUNTIME_CLASS(CEinComplexDoc),
										  RUNTIME_CLASS(CMainFrame),
										  RUNTIME_CLASS(CEinComplexView));
	if( !pDocTemplate ) return FALSE;
	AddDocTemplate(pDocTemplate);

	CCommandLineInfo cmdInfo;
	ParseCommandLine(cmdInfo);

	if( !ProcessShellCommand(cmdInfo) ) return FALSE;

	m_pMainWnd->ShowWindow(SW_SHOW);
	m_pMainWnd->UpdateWindow();

	return TRUE;
}

int CEinComplexApp::ExitInstance()
{
	AfxOleTerm(FALSE);

	return CWinApp::ExitInstance();
}
