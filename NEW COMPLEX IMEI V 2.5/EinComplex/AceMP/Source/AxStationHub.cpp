// AxStationHub.cpp: implementation of the CAxStationHub class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxStationHub.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxStationHub* CAxStationHub::theStationHub = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxStationHub::CAxStationHub()
{
	m_sName = _T("Stations");
	m_nNumStations = 0;
}

CAxStationHub::~CAxStationHub()
{
	int nNumStations = m_station.GetSize();

	for(int i=nNumStations-1; i>=0; i--) {
		delete m_station[i];
	}
}

CAxStationHub* CAxStationHub::GetStationHub()
{
	if(theStationHub == NULL) theStationHub = new CAxStationHub();
	return theStationHub;
}

void CAxStationHub::Startup()
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++) {
		m_station[i]->Startup();
	}
}

void CAxStationHub::Run()
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++) {
		m_station[i]->ResumeAxThread(m_station[i]->m_hPriThread);
		m_station[i]->ResumeAxThread(m_station[i]->m_hSecThread);
	}
}

void CAxStationHub::InitProfile()
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++)
		m_station[i]->InitProfile();
}

void CAxStationHub::LoadProfile()
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->LoadProfile();
}

void CAxStationHub::SaveProfile()
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->SaveProfile();
}

void CAxStationHub::InitRecipe()
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->InitRecipe();
}

void CAxStationHub::LoadRecipe()
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->LoadRecipe();
}

void CAxStationHub::LoadRecipe(LPCTSTR pszFile)
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->LoadRecipe(pszFile);
}

void CAxStationHub::SaveRecipe()
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->SaveRecipe();
}

void CAxStationHub::SaveRecipe(LPCTSTR pszFile)
{
	int nNumStations = m_station.GetSize();

	for (int i=0; i<nNumStations; i++)
		m_station[i]->SaveRecipe(pszFile);
}

int CAxStationHub::GetNumStations()
{
	int nNumStations = m_station.GetSize();

	return nNumStations;
}

void CAxStationHub::AddStation(CAxStation* pStation)
{
	m_station.Add(pStation);
	m_nNumStations = m_station.GetSize();

	int nIdx = m_nNumStations-1;
	m_station[nIdx]->m_nID = nIdx+1;
	m_station[nIdx]->m_control.SetStationNum(nIdx);
}

CAxStation* CAxStationHub::GetStation(int nStation)
{
	int nNumStations = m_station.GetSize();

	if( nStation >= nNumStations)
		return NULL;

	return m_station[nStation];
}

CAxStation* CAxStationHub::GetStation(LPCTSTR pszName)
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++) {
		if(m_station[i]->m_sName == pszName) {
			return m_station[i];
		}
	}
	return NULL;
}

CAxStationPtrArray& CAxStationHub::GetStation()
{
	return m_station;
}

void CAxStationHub::PostStop(UINT nType)
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++)
		m_station[i]->PostStop(nType);
}

void CAxStationHub::PreResume(UINT nType)
{
	int nNumStations = m_station.GetSize();

	for(int i=0; i<nNumStations; i++)
		m_station[i]->PreResume(nType);
}
