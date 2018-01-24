// AxEvent.cpp: implementation of the CAxEvent class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxEvent.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxEvent::CAxEvent(BOOL bInitiallyOwn, BOOL bManualReset,
				   LPCTSTR pszName, LPSECURITY_ATTRIBUTES lpsaAttribute) :
				   m_event(bInitiallyOwn, bManualReset, pszName, lpsaAttribute)
{
	m_nForceMode = FM_NO_FORCE;
	m_pEvent = &m_event;
}

CAxEvent::CAxEvent(LPCTSTR pszAddr, BOOL bInitiallyOwn, BOOL bManualReset,
				   LPCTSTR pszName, LPSECURITY_ATTRIBUTES lpsaAttribute) : 
				   m_event(bInitiallyOwn, bManualReset, pszName, lpsaAttribute)
{
	m_sAddr = pszAddr;
	m_nForceMode = FM_NO_FORCE;
	m_pEvent = &m_event;
}
				
CAxEvent::~CAxEvent()
{

}

void CAxEvent::Init(CAxEvent *pEvent)
{
	m_pEvent = pEvent->m_pEvent;
}

CEvent* CAxEvent::GetEvent()
{
	return m_pEvent;
}

UINT CAxEvent::GetForceMode()
{
	return m_nForceMode;
}

void CAxEvent::SetForceMode(UINT nMode)
{
	if(nMode == FM_FORCE_ON) Set();
	else Reset();
	m_nForceMode = nMode;
}

BOOL CAxEvent::Set()
{
	if(m_nForceMode == FM_FORCE_OFF) return FALSE;
	else return m_pEvent->SetEvent();
}

BOOL CAxEvent::Reset()
{
	if(m_nForceMode == FM_FORCE_ON) return FALSE;
	else return m_pEvent->ResetEvent();
}

BOOL CAxEvent::IsSet()
{
	CSingleLock eventLock(m_pEvent);
	return eventLock.Lock(0);
}
