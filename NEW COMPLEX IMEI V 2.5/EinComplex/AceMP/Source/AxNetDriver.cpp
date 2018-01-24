#include "stdafx.h"
#include "AxNetDriver.h"

#include "AxIOMgr.h"
#include "AxIni.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxNetDriver::CAxNetDriver()
{
	m_nID        = 0;
	m_nType		 = 0;
	m_nScanTime  = 0;
	m_bInit      = FALSE;
	m_bSimulate  = FALSE;
	m_bTerminate = FALSE;
	m_pNetMgr    = NULL;

	m_hMutex	 = m_mutex.m_hObject;
}

CAxNetDriver::~CAxNetDriver()
{

}

BOOL CAxNetDriver::Init(LPCTSTR pszFile)
{
	m_bInit	= FALSE;
	
	CAxIni	profile;
	profile.m_sIniFile = pszFile;
	profile.m_sSect.Format(_T("Driver%d"), m_nID);

	m_nType		   = profile.ReadUint(_T("Type"));
	m_nScanTime	   = profile.ReadUint(_T("ScanTime"));
	m_bSimulate	   = profile.ReadUint(_T("Simulate"));
	
	return TRUE;
}

UINT CAxNetDriver::AutoScan()
{
	m_timer.SetTimer(0, m_nScanTime);
	while( TRUE ) 
	{
		m_timer.WaitTimer();
		WaitForSingleObject(m_hMutex, INFINITE);
		OnTimer();
		ReleaseMutex(m_hMutex);
	}
}

UINT NetThreadFn(LPVOID pParam)
{
	CAxNetDriver* pThread = (CAxNetDriver*)pParam;
	return pThread->AutoScan();
}

void CAxNetDriver::Startup()
{
	CWinThread* pThread = 
	AfxBeginThread(
		NetThreadFn, 
		this,
		THREAD_PRIORITY_ABOVE_NORMAL, 
		200000,
		CREATE_SUSPENDED, 
		0);

	m_hScanThread = pThread->m_hThread;
	ResumeThread(m_hScanThread);
}

void CAxNetDriver::ImmediateTimer()
{
	m_timer.ImmediateTimer();
}

BOOL CAxNetDriver::CheckVariable()
{
	if( m_nScanTime < MIN_SCAN_TIME || 
		m_nScanTime > MAX_SCAN_TIME ) 
	{
		ASSERT(FALSE);
		return FALSE;
	}

	return TRUE;
}

void CAxNetDriver::AddNetData(CAxNetData* pNetData)
{
	m_parrNetData.Add(pNetData);
}