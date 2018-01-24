// AxIOScanner.cpp: implementation of the CAxIOScanner class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxIOScanner.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxIOScanner::CAxIOScanner()
{
	m_nErr = 0;
	m_nScanner = 0;
	m_nScannerType = 0;
	m_nScanTime = 0;
	m_bInit = FALSE;
	m_bSimulate = FALSE;
	m_bTerminate = FALSE;
	m_nNumDiIp = 0;
	m_nNumDiOp = 0;
}

CAxIOScanner::~CAxIOScanner()
{

}

UINT ScanThreadFn(LPVOID pParam)
{
	CAxIOScanner* pThread = (CAxIOScanner*)pParam;
	return pThread->AutoScan();
}

UINT CAxIOScanner::AutoScan()
{
	m_timer.SetTimer(0, m_nScanTime);
	while(TRUE) {
		m_timer.WaitTimer();
		OnTimer();
	}
}


void CAxIOScanner::Startup()
{
	CWinThread* pThread = AfxBeginThread(ScanThreadFn, this, 
										 THREAD_PRIORITY_ABOVE_NORMAL, 200000,
										 CREATE_SUSPENDED, 0);
	m_hScanThread = pThread->m_hThread;
	ResumeThread(m_hScanThread);
}
