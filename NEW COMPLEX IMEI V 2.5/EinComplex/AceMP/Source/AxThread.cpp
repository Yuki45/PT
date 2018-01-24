// AxThread.cpp: implementation of the CAxThread class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxThread.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxThread::CAxThread()
{
	m_nScanTime = 0;
	m_bTerminate = FALSE;
	m_hPriThread = NULL;
	m_nPriStatus = ACTIVE;
}

CAxThread::~CAxThread()
{
	
}

BOOL CAxThread::InitInstance ()
{
	return TRUE;
}

UINT PriThreadFn (LPVOID pParam)
{
	CAxThread* pThread = (CAxThread*)pParam;
	return (pThread->PriRun ());
}

UINT SecThreadFn(LPVOID pParam)
{
	CAxThread *pThread = (CAxThread*)pParam;
	return (pThread->SecRun());
}

BOOL CAxThread::CreateAxThread ()
{
	CWinThread* pThread;
	pThread = AfxBeginThread(PriThreadFn, this, THREAD_PRIORITY_NORMAL, 
							200000, CREATE_SUSPENDED, 0);

	if(!pThread) return FALSE;

	m_hPriThread = pThread->m_hThread;
	ResumeAxThread (m_hPriThread);
	m_timer.SetTimer(0, m_nScanTime);

	return TRUE;
	
}

BOOL CAxThread::ResumeAxThread (HANDLE hThread)
{
	if (hThread != m_hPriThread &&
		hThread != m_hSecThread) ASSERT(FALSE);

	ResumeThread(hThread);
	Sleep(10);
	SetStatus(hThread, ACTIVE);
	return TRUE;
}

BOOL CAxThread::SuspendAxThread (HANDLE hThread)
{
	if (hThread != m_hPriThread &&
		hThread != m_hSecThread) ASSERT(FALSE);

	SuspendThread(hThread);
	Sleep(10);
	SetStatus(hThread, SUSPENDED);
	return TRUE;
}

void CAxThread::DeleteAxThread (HANDLE hThread)
{
	BOOL bDone;
	DWORD dwStatus;
	int nTimeout = 5000;

	if (hThread != m_hPriThread &&
		hThread != m_hSecThread) ASSERT(FALSE);

	bDone = FALSE;
	while (!bDone) {
		if (!::GetExitCodeThread(hThread, &dwStatus)) {
			bDone = TRUE;
		}
		else if (dwStatus == STILL_ACTIVE)
		{
			Sleep(50);
			nTimeout -= 50;
			if (nTimeout <= 0) {
//				ASSERT(FALSE);
				TerminateThread(hThread, -1);
				SetStatus(hThread, DELETED);
				bDone = TRUE;
			}
		}
		else {
			SetStatus(hThread, DELETED);
			bDone = TRUE;
		}
	}
}

BOOL CAxThread::CreateThreads ()
{
	CWinThread* pThread;

	pThread = AfxBeginThread(PriThreadFn, this, THREAD_PRIORITY_NORMAL,
 							 200000, CREATE_SUSPENDED, 0);

	if (!pThread) return FALSE;
	m_hPriThread = pThread->m_hThread;

	pThread = AfxBeginThread(SecThreadFn, this, THREAD_PRIORITY_NORMAL,
 							 200000, CREATE_SUSPENDED, 0);

	if (!pThread) return FALSE;
	m_hSecThread = pThread->m_hThread;

	ResumeAxThread(m_hPriThread);
	ResumeAxThread(m_hSecThread);

	m_timer.SetTimer(0, m_nScanTime);

	return TRUE;
}

void CAxThread::DeleteThreads ()
{
	DeleteAxThread(m_hPriThread);
	DeleteAxThread(m_hSecThread);
}

int CAxThread::GetStatus (HANDLE hThread)
{
	if(hThread != m_hPriThread) 
		ASSERT(FALSE); 

	return m_nPriStatus;
}

void CAxThread::SetStatus (HANDLE hThread, int nStatus)
{
	if(hThread == m_hPriThread) m_nPriStatus = nStatus;
}



