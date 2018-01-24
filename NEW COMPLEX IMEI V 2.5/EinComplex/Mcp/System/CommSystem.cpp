#include "StdAfx.h"
#include "CommSystem.h"

#include "Resource.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CCommSystem::CCommSystem()
{
	m_sName = _T("CommSystem");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\System\\%s.ini"), m_sName);

	m_nAutoState = AS_INIT;
}

CCommSystem::~CCommSystem()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);

	DeleteAxThread(m_hPriThread);
}

void CCommSystem::InitProfile()
{
	CAxSystem::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
}

void CCommSystem::Startup()
{
	CAxSystem::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();
}

UINT CCommSystem::AutoRun()
{
	while( TRUE )
	{
		WaitNS(10);

		switch( m_nAutoState )
		{
			case AS_INIT:	AsInit();	break;
			case AS_RUN:	AsRun();	break;
			default:
				if( m_bTerminate ) throw -1;
				break;
		}
	}

	return 0;
}

void CCommSystem::AsInit()
{
	m_nAutoState = AS_RUN;
}

void CCommSystem::AsRun()
{
	if( m_pMainFrm->m_bSerialHubCreated == TRUE )
	{
		CCommWorld* pWorldRight = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		CCommWorld* pWorldLeft = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));

		pWorldRight->SendPacket(CCommWorld::CC_SET_OUTPUT);
		Sleep(30);
		pWorldLeft->SendPacket(CCommWorld::CC_SET_OUTPUT);
	}
}
