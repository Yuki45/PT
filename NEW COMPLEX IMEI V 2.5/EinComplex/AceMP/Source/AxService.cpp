// AxService.cpp: implementation of the CAxService class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxService.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxService::CAxService()
{
	m_nTaskGroup = TG_SERVICE;
	m_profile.m_sSect = _T("Settings");
}

CAxService::~CAxService()
{

}

void CAxService::OnLoadProfile()
{
	LoadProfile();
}

void CAxService::OnSaveProfile()
{
	SaveProfile();
}

void CAxService::Setup() //(M) 2009.11.18 - Added
{
}

void CAxService::Startup()
{
}

void CAxService::InitProfile()
{
	m_profile.AddBool(_T("Simulate"), m_bSimulate);
}

void CAxService::LoadProfile()
{
	m_profile.Load();
}


void CAxService::SaveProfile()
{
	m_profile.Save();
}


void CAxService::SetRunMode(UINT nMode)
{
	m_control.SetRunMode(PRI_RUN);
}