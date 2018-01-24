// AxStation.cpp: implementation of the CAxStation class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxStation::CAxStation() : CAxIOCtrl(&m_errCtrl), CAxRobotCtrl(&m_errCtrl)
{
	m_nTaskGroup = TG_STATION;
	m_profile.m_sSect = _T("Settings");
	m_recipe.m_sSect = _T("Station");

	m_bPreStart = FALSE; //(M) 2014.07.17

	SetControl();
}

CAxStation::~CAxStation()
{
}

void CAxStation::Startup()
{
	CreateThreads();
}

//void CAxStation::OnLoadProfile()
//{
//	LoadProfile();
//}
//
//void CAxStation::OnSaveProfile()
//{
//	SaveProfile();
//}
//
//void CAxStation::OnLoadRecipe()
//{
//	LoadRecipe(m_sRecipeFile);
//}
//
//void CAxStation::OnSaveRecipe()
//{
//	SaveRecipe(m_sRecipeFile);
//}

void CAxStation::InitProfile()
{
	m_profile.AddBool(_T("DryRun"), m_bDryRun);
	m_profile.AddBool(_T("Bypass"), m_bBypass);
	m_profile.AddBool(_T("Simulate"), m_bSimulate);
}

void CAxStation::LoadProfile()
{
	m_profile.Load();
}

void CAxStation::SaveProfile()
{
	m_profile.Save();
}

void CAxStation::InitRecipe()
{
}

void CAxStation::LoadRecipe()
{
	m_recipe.Load();
}

void CAxStation::LoadRecipe(LPCTSTR pszFile)
{
	m_recipe.Load(pszFile);
}

void CAxStation::SaveRecipe()
{
	m_recipe.Save();
}

void CAxStation::SaveRecipe(LPCTSTR pszFile)
{
	m_recipe.Save(pszFile);
}

void CAxStation::SetControl()
{
	m_control.m_priEvt[CT_ERROR]->SetForceMode(FM_NO_FORCE);
	m_control.m_priEvt[CT_CMD]->SetForceMode(FM_NO_FORCE);
	m_control.m_priEvt[CT_IDLE]->SetForceMode(FM_NO_FORCE);
	m_control.m_priEvt[CT_STOPPED]->SetForceMode(FM_NO_FORCE);
	m_control.m_priEvt[CT_DONE]->SetForceMode(FM_NO_FORCE);

	m_control.m_secEvt[CT_CMD]->SetForceMode(FM_NO_FORCE);
	m_control.m_secEvt[CT_ERROR]->SetForceMode(FM_NO_FORCE);
	m_control.m_secEvt[CT_IDLE]->SetForceMode(FM_NO_FORCE);
	m_control.m_secEvt[CT_STOPPED]->SetForceMode(FM_NO_FORCE);
	m_control.m_secEvt[CT_DONE]->SetForceMode(FM_NO_FORCE);
}


void CAxStation::SetRunMode(UINT nMode)
{
	m_control.SetRunMode(nMode);
}

UINT CAxStation::PriRun()
{
	SuspendAxThread(m_hPriThread);
	ResetEvents(m_control.m_priEvt);
	EnterRun();
	while(TRUE) {
		try {
			SetState(TS_IDLE);
			SetIdleControl(m_control.m_priEvt);
			WaitStart(m_control.m_priEvt);
			SetState(TS_AUTO);
			SetRunControl(m_control.m_priEvt);
			AutoRun();
		}
		catch(int nExp) {
			Sleep(10);
			PostAbort();
			if(nExp == -1) {
				if(m_bTerminate) {
					ExitRun();
					AfxEndThread(0);
				}
			}
			ResetEvents(m_control.m_priEvt);
		}
	}
	return 0;
}

UINT CAxStation::SecRun()
{
	SuspendAxThread(m_hSecThread);
	ResetEvents(m_control.m_secEvt);
	EnterRun();
	while (TRUE) {
		try {
			SetIdleControl(m_control.m_secEvt);
			WaitStart(m_control.m_secEvt);
			int nPrevState = GetState();
			if (m_nCmdCode > NO_CMD) {
				SetState(TS_MANUAL);
				SetRunControl(m_control.m_secEvt);
				ManualRun();
				m_nCmdCode = NO_CMD;
			}
			SetState(nPrevState);
		}
		catch (int nExp) {
			Sleep(10);
			if (nExp == -1) {
				if (m_bTerminate) {
					ExitRun();
					AfxEndThread(0);
				}
			}
			ResetEvents(m_control.m_secEvt);
		}
	}
	return 0;
}

void CAxStation::EnterRun()
{
}

void CAxStation::ExitRun()
{
}

void CAxStation::PostAbort()
{
}

void CAxStation::PostStop(UINT nMode)
{
}

void CAxStation::PreResume(UINT nMode)
{
}

void CAxStation::SetDryRun(BOOL bValue)
{
	m_bDryRun = bValue;
}

BOOL CAxStation::GetDryRun()
{
	return m_bDryRun;
}

void CAxStation::SetBypass(BOOL bValue)
{
	m_bBypass = bValue;
}

BOOL CAxStation::GetBypass()
{
	return m_bBypass;
}

void CAxStation::SetSimulate(BOOL bValue)
{
	m_bSimulate = bValue;
}

BOOL CAxStation::GetSimulate()
{
	return m_bSimulate;
}

//(M) 2014.07.17
void CAxStation::SetPreStart(BOOL bValue)
{
	m_bPreStart = bValue;
}

//(M) 2014.07.17
BOOL CAxStation::GetPreStart()
{
	return m_bPreStart;
}
