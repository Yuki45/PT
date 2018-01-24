// AxTask.cpp: implementation of the CAxTask class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxTask.h"
#include "AxErrorMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxTask::CAxTask()
{
	m_nID = 0;
	m_nCmdCode = NO_CMD;
	m_errCtrl.Load(&m_errMsg, &m_control, &m_bSimulate);
}

CAxTask::~CAxTask()
{

}

void CAxTask::DeleteThreads()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);
	m_control.Abort(m_control.m_secEvt);
	CAxThread::DeleteThreads();
}

UINT CAxTask::GetState()
{
	return m_control.m_nState;
}

void CAxTask::SetState(UINT nValue)
{
	m_control.m_nState = nValue;
}

void CAxTask::SetResponse(UINT nResp)
{
	m_control.m_nResponse = nResp;
}

UINT CAxTask::GetResponse()
{
	return m_control.m_nResponse;
}

void CAxTask::SetCmdCode(int nCode)
{
	m_nCmdCode = nCode;
}

CAxEvent* CAxTask::GetCtrlEvent(UINT nType)
{
	return m_control.m_evt[nType];
}

void CAxTask::SetIdleControl(CAxEventPtrArray& event)
{
	SetEvent(event[CT_IDLE]);
	SetEvent(event[CT_DONE]);
	SetEvent(event[CT_STOPPED]);
}

void CAxTask::SetRunControl(CAxEventPtrArray& event)
{
	ResetEvent(event[CT_IDLE]);
	ResetEvent(event[CT_DONE]);
	ResetEvent(event[CT_STOPPED]);
}

void CAxTask::Start()
{
	m_control.Start();
}

void CAxTask::Stop()
{
	m_control.Stop();
}

void CAxTask::Abort()
{
	m_control.Abort();
}

void CAxTask::Reset()
{
	m_control.Reset();
}
	
BOOL CAxTask::SetEvent(CAxEvent* pEvent)
{
	ASSERT(pEvent != NULL);
	return pEvent->Set();
}

BOOL CAxTask::SetEvent(CAxEvent& event)
{
	return event.Set();
}

BOOL CAxTask::ResetEvent(CAxEvent* pEvent)
{
	ASSERT(pEvent != NULL);
	return pEvent->Reset();
}

BOOL CAxTask::ResetEvent(CAxEvent& event)
{
	return event.Reset();
}

void CAxTask::SetEvents(CAxEventPtrArray& evt)
{
	int nSize = evt.GetSize();
	for(int i=0; i<nSize; i++) evt[i]->Set();
}

void CAxTask::ResetEvents(CAxEventPtrArray& evt)
{
	int nSize = evt.GetSize();
	for(int i=0; i<nSize; i++) evt[i]->Reset();
}

void CAxTask::WaitStart(CAxEventPtrArray& evt)
{
	m_control.WaitStart(evt);
}

void CAxTask::Wait(DWORD dwTime)
{
	m_control.Wait(dwTime);
}

int CAxTask::WaitEvent(CAxEvent* pEvent)
{
	CAxEventPtrArray event;

	ASSERT(pEvent != NULL);
	event.Add(pEvent);

	return m_control.WaitEvents(INFINITE, event);
}

int CAxTask::WaitEvent(CAxEvent& evt)
{
	CAxEventPtrArray event;
	event.Add(&evt);

	return m_control.WaitEvents(INFINITE, event);
}

int CAxTask::WaitEvent(DWORD dwTimeout, CAxEvent* pEvent)
{
	CAxEventPtrArray event;

	ASSERT(pEvent != NULL);
	event.Add(pEvent);

	return m_control.WaitEvents(dwTimeout, event);
}

int CAxTask::WaitEvent(DWORD dwTimeout, CAxEvent& evt)
{
	CAxEventPtrArray event;
	
	event.Add(&evt);

	return m_control.WaitEvents(dwTimeout, event);
}


void CAxTask::WaitEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, 
						int nCode, CAxEvent* pEvent)
{
	CAxEventPtrArray event;

	ASSERT(pEvent != NULL);
	event.Add(pEvent);

	WaitTimeout(TRUE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

void CAxTask::WaitEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						int nCode, CAxEvent& evt)
{
	CAxEventPtrArray event;

	event.Add(&evt);

	WaitTimeout(TRUE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitEvents(CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while(pEvent != NULL);
	va_end(eventList);

	return m_control.WaitEvents(INFINITE, event);
}

int CAxTask::WaitEvents(CAxEventPtrArray& event)
{
	return m_control.WaitEvents(INFINITE, event);
}

int CAxTask::WaitEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while(pEvent != NULL);
	va_end(eventList);

	return m_control.WaitEvents(dwTimeout, event);
}

void CAxTask::WaitEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						 int nCode, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while(pEvent != NULL);
	va_end(eventList);

	WaitTimeout(TRUE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitEvents(TRUE, bReset, dwTimeout, event);
}

void CAxTask::WaitEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						 LPCTSTR pszSource, int nCode, 
						 CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitTimeout(TRUE, bReset, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitAllEvents(CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while(pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(INFINITE, event);
}

int CAxTask::WaitAllEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(dwTimeout, event);
}

void CAxTask::WaitAllEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, 
						    LPCTSTR pszSource, int nCode, 
						    CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitAllTimeout(TRUE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitAllEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(TRUE, bReset, dwTimeout, event);
}

void CAxTask::WaitAllEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, 
						    LPCTSTR pszSource, int nCode, 
						    CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitAllTimeout(TRUE, bReset, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

void CAxTask::WaitNS(DWORD dwTime)
{
	m_control.WaitNS(dwTime);
}

int CAxTask::WaitNSEvent(DWORD dwTimeout, CAxEvent* pEvent)
{
	CAxEventPtrArray event;

	ASSERT(pEvent != NULL);
	event.Add(pEvent);

	return m_control.WaitEvents(FALSE, dwTimeout, event);
}

int CAxTask::WaitNSEvent(DWORD dwTimeout, CAxEvent& evt)
{
	CAxEventPtrArray event;

	event.Add(&evt);

	return m_control.WaitEvents(FALSE, dwTimeout, event);
}

void CAxTask::WaitNSEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						  int nCode, CAxEvent* pEvent)
{
	CAxEventPtrArray event;

	ASSERT(pEvent != NULL);
	event.Add(pEvent);

	WaitTimeout(FALSE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

void CAxTask::WaitNSEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						  int nCode, CAxEvent& evt)
{
	CAxEventPtrArray event;

	event.Add(&evt);

	WaitTimeout(FALSE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitNSEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitEvents(FALSE, dwTimeout, event);
}

void CAxTask::WaitNSEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						   LPCTSTR pszSource, int nCode, 
						   CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitTimeout(FALSE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}


int CAxTask::WaitNSEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitEvents(FALSE, bReset, dwTimeout, event);
}


void CAxTask::WaitNSEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						   LPCTSTR pszSource, int nCode, 
						   CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitTimeout(FALSE, bReset, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}


int CAxTask::WaitNSAllEvents(CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(FALSE, INFINITE, event);
}


int CAxTask::WaitNSAllEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(FALSE, dwTimeout, event);
}


void CAxTask::WaitNSAllEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode, 
							  CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitAllTimeout(FALSE, FALSE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}


int CAxTask::WaitNSAllEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	return m_control.WaitAllEvents(FALSE, bReset, dwTimeout, event);
}


void CAxTask::WaitNSAllEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode,
							  CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstEvent != NULL);
	CAxEvent* pEvent = pFirstEvent;

	va_start(eventList, pFirstEvent);
	do {
		event.Add(pEvent);
		pEvent = va_arg(eventList, CAxEvent*);
	} while (pEvent != NULL);
	va_end(eventList);

	WaitAllTimeout(FALSE, bReset, dwTimeout, nResp, pszPath, pszSource, nCode, event);
}

int CAxTask::WaitTimeout(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, UINT nResp, 
						 LPCTSTR pszPath, LPCTSTR pszSource, int nCode, 
						 CAxEventPtrArray& event)
{
	CString sFile;

	if(m_control.WaitEvents(bAddStop, bReset, dwTimeout, event) == WAIT_TIMEOUT) {
		sFile.Format(_T("%s%s%d.htm"), pszPath, pszSource, nCode);
		return Error(nCode, nResp, pszSource, sFile);

	}
	return 0;
}

int CAxTask::WaitAllTimeout(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, UINT nResp,
							LPCTSTR pszPath, LPCTSTR pszSource, int nCode,
							CAxEventPtrArray& event)
{
	CString sFile;

	if (m_control.WaitAllEvents(bAddStop, bReset, dwTimeout, event) == WAIT_TIMEOUT) {
		sFile.Format(_T("%s%s%d.htm"), pszPath, pszSource, nCode);
		return Error(nCode, nResp, pszSource, sFile);

	}
	return 0;
}

int CAxTask::Error(int nNumber, UINT nType, LPCTSTR pszSource,
				   LPCTSTR pszPath, LPCTSTR pszParam1, LPCTSTR pszParam2,
				   LPCTSTR pszParam3, LPCTSTR pszParam4)
{
	return m_errCtrl.Error(nNumber, nType, pszSource, pszPath, pszParam1,
						   pszParam2, pszParam3, pszParam4);
}

int CAxTask::IsEvents(DWORD dwTimeout, CAxEventPtrArray& evt)
{
	DWORD dwRetVal;
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	int nNumEvents = evt.GetSize();

	for( int i=0; i<nNumEvents; i++ ) 
	{
		pSyncObject[i] = evt[i]->GetEvent();
	}

	CMultiLock eventLock(pSyncObject, nNumEvents);
	dwRetVal = eventLock.Lock(dwTimeout, FALSE);

	return dwRetVal;
}

int CAxTask::IsAllEvents(DWORD dwTimeout, CAxEventPtrArray& evt)
{
	DWORD dwRetVal;
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	int nNumEvents = evt.GetSize();

	for( int i=0; i<nNumEvents; i++ ) 
	{
		pSyncObject[i] = evt[i]->GetEvent();
	}

	CMultiLock eventLock(pSyncObject, nNumEvents);
	dwRetVal = eventLock.Lock(dwTimeout, TRUE);

	return dwRetVal;
}

