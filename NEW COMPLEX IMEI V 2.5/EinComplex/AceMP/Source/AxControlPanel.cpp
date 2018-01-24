#include "stdafx.h"
#include "AxControlPanel.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxControlPanel::CAxControlPanel()
{
	m_sName = _T("ControlPanel");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\System\\ControlPanel.ini");
	m_nStartLock = 0;
}

CAxControlPanel::~CAxControlPanel()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);
	DeleteAxThread(m_hPriThread);
}

void CAxControlPanel::Startup()
{
	CAxSystem::Startup();
}

void CAxControlPanel::InitProfile()
{
	CAxSystem::InitProfile();

	// Inputs -----------------------------------------------------------
	m_profile.AddInput(_T("Start"),   m_ipStart);
	m_profile.AddInput(_T("Stop"),    m_ipStop);
	m_profile.AddInput(_T("Reset"),   m_ipReset);
	m_profile.AddInput(_T("Start2"),  m_ipStart2);
	m_profile.AddInput(_T("Stop2"),   m_ipStop2);
	m_profile.AddInput(_T("Reset2"),  m_ipReset2);

	// Outputs -----------------------------------------------------------
	m_profile.AddOutput(_T("Start"),  m_opStart);
	m_profile.AddOutput(_T("Stop"),   m_opStop);
	m_profile.AddOutput(_T("Reset"),  m_opReset);
	m_profile.AddOutput(_T("Start2"), m_opStart2);
	m_profile.AddOutput(_T("Stop2"),  m_opStop2);
	m_profile.AddOutput(_T("Reset2"), m_opReset2);
}

UINT CAxControlPanel::AutoRun()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();
	int nPrevBtn = -1;

	while( TRUE ) {
		int nRetCode = WaitInputsNS(1000, &m_ipStart,  ON,
										  &m_ipStart2, ON,
										  &m_ipStop,   ON,
										  &m_ipStop2,  ON,
										  &m_ipReset,  ON, 
										  &m_ipReset2, ON, 
										  NULL);
		
		int nState = pMaster->GetState();

		switch( nRetCode ) {
		case START_BTN: 
		case START_BTN2: 
			WaitInputsNS(INFINITE, &m_ipStart, OFF, NULL);
			if(m_nStartLock != 0) break;
			if(nState == MS_AUTO) break;
			
			CAxErrorMgr::GetErrorMgr()->SetResponse(emRetry);
			pMaster->Start();
			break;

		case STOP_BTN:
		case STOP_BTN2:
			WaitInputsNS(INFINITE, &m_ipStop, OFF, NULL);
			pMaster->Stop();
			break;

		case RESET_BTN:
		case RESET_BTN2:
			WaitInputsNS(INFINITE, &m_ipReset, OFF, NULL);
			ResetAlarm();
			break;

		default:
			nPrevBtn = -1;
			if( m_bTerminate ) throw -1;
		}

		Sleep(10);
	}

	return 0;
}

void CAxControlPanel::SetStartLed()
{
	m_opStart.On();
	m_opStart2.On();
	m_opStop.Off();
	m_opStop2.Off();
	m_opReset.Off();
	m_opReset2.Off();
	m_bReset = FALSE;
}

void CAxControlPanel::SetStopLed()
{
	m_opStart.Off();
	m_opStart2.Off();
	m_opStop.On();
	m_opStop2.On();
	m_opReset.Off();
	m_opReset2.Off();
}

void CAxControlPanel::SetAbortLed()
{
	m_opStart.Off();
	m_opStart2.Off();
	m_opStop.On();
	m_opStop2.On();
	m_opReset.Off();
	m_opReset2.Off();
}

void CAxControlPanel::SetErrorLed()
{
	m_opStart.Off();
	m_opStart2.Off();
	m_opStop.On();
	m_opStop2.On();

	if( !m_bReset )
	{
		m_opReset.On();
		m_opReset2.On();
	}
}

void CAxControlPanel::ResetAlarm()
{
	m_bReset = TRUE;
	m_opReset.Off();
	m_opReset2.Off();
}

BOOL CAxControlPanel::GetResetState()
{
	return m_opReset.GetValue();
}

void CAxControlPanel::StartLockInc()
{
	m_nStartLock++;
}

void CAxControlPanel::StartLockDec()
{
	m_nStartLock--;
}
