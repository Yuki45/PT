// AxServiceHub.cpp: implementation of the CAxServiceHub class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxServiceHub.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxServiceHub* CAxServiceHub::theServiceHub = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxServiceHub::CAxServiceHub()
{
	m_sName = _T("Service");
	m_nNumServices = 0;
}

CAxServiceHub::~CAxServiceHub()
{
	for(int i=m_nNumServices-1; i>=0; i--) {
		delete m_service[i];
	}		
}

CAxService* CAxServiceHub::GetService(LPCTSTR pszName)
{
	for(int i=0; i<m_nNumServices; i++) {
		if(m_service[i]->m_sName == pszName) {
			return m_service[i];
		}
	}
	return NULL;
}

void CAxServiceHub::Startup()
{
	for(int i=0; i<m_nNumServices; i++) {
		m_service[i]->Startup();
	}
}

void CAxServiceHub::Run()
{
	for(int i=0; i<m_nNumServices; i++) {
		m_service[i]->ResumeAxThread(m_service[i]->m_hPriThread);
	}
}

CAxServiceHub* CAxServiceHub::GetServiceHub()
{
	if(theServiceHub == NULL) theServiceHub = new CAxServiceHub();
	return theServiceHub;
}

void CAxServiceHub::InitProfile()
{
	for(int i=0; i<m_nNumServices; i++)
		m_service[i]->InitProfile();
}

void CAxServiceHub::LoadProfile()
{
	for(int i=0; i<m_nNumServices; i++)
		m_service[i]->LoadProfile();
}

void CAxServiceHub::SaveProfile()
{
	for(int i=0; i<m_nNumServices; i++)
		m_service[i]->SaveProfile();
}

int CAxServiceHub::GetNumServices()
{
	return m_nNumServices;
}

void CAxServiceHub::AddService(CAxService* pService)
{
	m_service.Add(pService);
	m_nNumServices = m_service.GetSize();

	int nIdx = m_nNumServices-1;
	m_service[nIdx]->m_nID = nIdx+1;
}

CAxService* CAxServiceHub::GetService(int nService)
{
	if(nService >= m_nNumServices) return NULL;
	return m_service[nService];
}
