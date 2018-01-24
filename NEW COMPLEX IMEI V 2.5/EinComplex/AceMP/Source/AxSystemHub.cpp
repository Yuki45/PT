// AxSystemHub.cpp: implementation of the CAxSystemHub class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxSystemHub.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxSystemHub* CAxSystemHub::theSystemHub = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxSystemHub::CAxSystemHub()
{
	m_sName = _T("System");
	m_nNumSystems = 0;
}

CAxSystemHub::~CAxSystemHub()
{
	for(int i=m_nNumSystems-1; i>=0; i--) {
		delete m_system[i];
	}
}

CAxSystemHub* CAxSystemHub::GetSystemHub()
{
	if(theSystemHub == NULL) theSystemHub = new CAxSystemHub();
	return theSystemHub;
}

void CAxSystemHub::Startup()
{
	for(int i=0; i<m_nNumSystems; i++) {
		m_system[i]->Startup();
	}
}

void CAxSystemHub::Run()
{
	for(int i=0; i<m_nNumSystems; i++) {
		m_system[i]->ResumeAxThread(m_system[i]->m_hPriThread);
	}
}

void CAxSystemHub::InitProfile()
{
	for(int i=0; i<m_nNumSystems; i++) {
		m_system[i]->InitProfile();
	}
}

void CAxSystemHub::LoadProfile()
{
	for(int i=0; i<m_nNumSystems; i++) {
		m_system[i]->LoadProfile();
	}
}

void CAxSystemHub::SaveProfile()
{
	for(int i=0; i<m_nNumSystems; i++) {
		m_system[i]->SaveProfile();
	}
}

void CAxSystemHub::AddSystem(CAxSystem* pSystem)
{
	m_system.Add(pSystem);
	m_nNumSystems = m_system.GetSize();

	int nIdx = m_nNumSystems-1;
	m_system[nIdx]->m_nID = nIdx+1;
}

int CAxSystemHub::GetNumSystem()
{
	return m_nNumSystems;
}

CAxSystem* CAxSystemHub::GetSystem(int nSystem)
{
	if(nSystem >= m_nNumSystems) 
		return NULL;

	if(m_system[nSystem]->m_bTerminate)
		return NULL;

	return m_system[nSystem];
}

CAxSystem* CAxSystemHub::GetSystem(LPCTSTR pszName)
{
	for(int i=0; i<m_nNumSystems; i++) {
		if(m_system[i]->m_bTerminate)
			return NULL;
		
		if(m_system[i]->m_sName == pszName) {
			return m_system[i];
		}
	}

	return NULL;
}

CAxSystemPtrArray& CAxSystemHub::GetSystem()
{
	return m_system;
}
