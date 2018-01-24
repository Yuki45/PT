// AxSystem.cpp: implementation of the CAxSystem class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxSystem.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxSystem::CAxSystem() : CAxIOCtrl(&m_errCtrl)
{
	m_nTaskGroup = TG_SYSTEM;
	m_profile.m_sSect = _T("Settings");

	SetControl();
}

CAxSystem::~CAxSystem()
{

}

void CAxSystem::Startup()
{
	CreateAxThread();
}

void CAxSystem::OnLoadProfile()
{
	LoadProfile();
}

void CAxSystem::OnSaveProfile()
{
	SaveProfile();
}

void CAxSystem::InitProfile()
{
	m_profile.AddBool(_T("Simulate"), m_bSimulate);
}

void CAxSystem::LoadProfile()
{
	m_profile.Load();
}

void CAxSystem::SaveProfile()
{
	m_profile.Save();
}

void CAxSystem::SetRunMode(UINT nMode)
{
	m_control.SetRunMode(PRI_RUN);
}

void CAxSystem::SetControl()
{
	m_control.m_priEvt[CT_ERROR]->SetForceMode(FM_NO_FORCE);
}

UINT CAxSystem::PriRun()
{
	SuspendAxThread(m_hPriThread);
   	while (TRUE) {
		try {
			SetState(TS_IDLE);
			//WaitStart(m_control.m_priEvt);
			//SetState(TS_AUTO);
			AutoRun();
		}
		catch (int nExp) {
			Sleep(10);
			if (nExp == -1) {
				if (m_bTerminate) AfxEndThread(0);
			}
			ResetEvents(m_control.m_priEvt);
		}
	}
	return 0;
}

void CAxSystem::SetSimulate(BOOL bValue)
{
	m_bSimulate = bValue;
}

BOOL CAxSystem::GetSimulate()
{
	return m_bSimulate;
}
