// AxEventMgr.cpp: implementation of the CAxEventMgr class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxEventMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxEventMgr* CAxEventMgr::theEventMgr = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxEventMgr::CAxEventMgr()
{
	m_sName = _T("EventMgr");
	m_sErrPath = _T("\\Service\\EventMgr.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\EventMgr.ini");
	m_nNumEvents = 0;
}

CAxEventMgr::~CAxEventMgr()
{
	POSITION pos;

	pos = m_eventMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		CAxEvent* pEvent;
		m_eventMap.GetNextAssoc(pos, key, pEvent);
		delete pEvent;
	}
}

CAxEventMgr* CAxEventMgr::GetEventMgr()
{
	if(theEventMgr == NULL) theEventMgr = new CAxEventMgr();
	return theEventMgr;
}

void CAxEventMgr::Startup()
{
	CAxService::Startup();

	CString sAddr;
	for(UINT i=0; i<m_nNumEvents; i++) {
		sAddr.Format(_T("e%03d"), i);
		m_eventMap.SetAt(sAddr, new CAxEvent(sAddr));
	}
}

void CAxEventMgr::InitProfile()
{
	CAxService::InitProfile();

	m_profile.AddUint(_T("NumEvents"), m_nNumEvents);
}

void CAxEventMgr::LoadProfile()
{
	CAxService::LoadProfile();
}

void CAxEventMgr::SaveProfile()
{
	CAxService::SaveProfile();
}

CAxEvent* CAxEventMgr::GetEvent(LPCTSTR pszAddr)
{
	CAxEvent* pEvent;
	if(!m_eventMap.Lookup(pszAddr, pEvent)) return NULL;
	return pEvent;
}

void CAxEventMgr::SetupEvent(LPCTSTR pszAddr, CAxEvent& event)
{
	CAxEvent* pEvent;
	CString sAddr(pszAddr);

	if(!m_eventMap.Lookup(sAddr, pEvent)) ASSERT(FALSE);

	event.Init(pEvent);
	event.m_sAddr = pszAddr;
}

BOOL CAxEventMgr::SetEvent(LPCTSTR pszAddr)
{
	CAxEvent* pEvent;
	if(!m_eventMap.Lookup(pszAddr, pEvent)) return FALSE;
	return pEvent->Set();
}

BOOL CAxEventMgr::ResetEvent(LPCTSTR pszAddr)
{
	CAxEvent* pEvent;
	if(!m_eventMap.Lookup(pszAddr, pEvent)) return FALSE;
	return pEvent->Reset();
}
