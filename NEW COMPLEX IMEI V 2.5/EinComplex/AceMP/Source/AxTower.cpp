// AxTower.cpp: implementation of the CAxTower class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxTower.h"
#include "AxControlPanel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxTower::CAxTower()
{
	m_sName = _T("Tower");
	m_sErrPath = _T("\\System\\Tower.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\System\\Tower.ini");
	m_nScanTime = 500;

	m_nForceBuzzFlag = eBuzz_DEFAULT;		// 강제 Buzz 모드는 기본값
}

CAxTower::~CAxTower()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);
	DeleteAxThread(m_hPriThread);
}

void CAxTower::Startup()
{
	CAxSystem::Startup();
}

void CAxTower::InitProfile()
{
	CAxSystem::InitProfile();

	m_profile.AddOutput(_T("Buzzer"),		m_opBuzzer);
	m_profile.AddOutput(_T("RedLight"),		m_opRedLight);
	m_profile.AddOutput(_T("GreenLight"),	m_opGreenLight);
	m_profile.AddOutput(_T("AmberLight"),	m_opAmberLight);
	m_profile.AddOutput(_T("RedLight2"),	m_opRedLight2);
	m_profile.AddOutput(_T("GreenLight2"),	m_opGreenLight2);
	m_profile.AddOutput(_T("AmberLight2"),	m_opAmberLight2);

	m_profile.AddInt(_T("Amber_On_Init"),	m_nAmber_On_Init);
	m_profile.AddInt(_T("Amber_On_Run"),	m_nAmber_On_Run);
	m_profile.AddInt(_T("Amber_On_Stop"),	m_nAmber_On_Stop);
	m_profile.AddInt(_T("Amber_On_Error"),	m_nAmber_On_Error);
	m_profile.AddInt(_T("Amber_On_Idle"),	m_nAmber_On_Idle);
	m_profile.AddInt(_T("Red_On_Init"),		m_nRed_On_Init);
	m_profile.AddInt(_T("Red_On_Run"),		m_nRed_On_Run);
	m_profile.AddInt(_T("Red_On_Stop"),		m_nRed_On_Stop);
	m_profile.AddInt(_T("Red_On_Error"),	m_nRed_On_Error);
	m_profile.AddInt(_T("Red_On_Idle"),		m_nRed_On_Idle);
	m_profile.AddInt(_T("Buzzer_On_Init"),	m_nBuzzer_On_Init);
	m_profile.AddInt(_T("Buzzer_On_Run"),	m_nBuzzer_On_Run);
	m_profile.AddInt(_T("Buzzer_On_Stop"),	m_nBuzzer_On_Stop);
	m_profile.AddInt(_T("Buzzer_On_Error"),	m_nBuzzer_On_Error);
	m_profile.AddInt(_T("Buzzer_On_Idle"),	m_nBuzzer_On_Idle);
	m_profile.AddInt(_T("Green_On_Init"),	m_nGreen_On_Init);
	m_profile.AddInt(_T("Green_On_Run"),	m_nGreen_On_Run);
	m_profile.AddInt(_T("Green_On_Stop"),	m_nGreen_On_Stop);
	m_profile.AddInt(_T("Green_On_Error"),	m_nGreen_On_Error);
	m_profile.AddInt(_T("Green_On_Idle"),	m_nGreen_On_Idle);
	m_profile.AddBool(_T("Blink"),			m_bBlink);
}

UINT CAxTower::AutoRun()
{
	int pState;
	CAxMaster* pMaster = CAxMaster::GetMaster();

	while(TRUE) {
		WaitNS(m_nScanTime);
		if(m_bTerminate) throw -1;
		pState = pMaster->GetState();
		
		switch(pState) {
		case MS_IDLE:
			SetRedTower(m_nRed_On_Idle, m_bBlink);
			SetAmberTower(m_nAmber_On_Idle, m_bBlink);
			SetGreenTower(m_nGreen_On_Idle, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Idle, m_bBlink);

			ToggleBlinkFlag(MS_IDLE, m_bBlink);

			SetLed(MS_IDLE);

			break;
		case MS_SETUP:
			SetRedTower(m_nRed_On_Init, m_bBlink);
			SetAmberTower(m_nAmber_On_Init, m_bBlink);
			SetGreenTower(m_nGreen_On_Init, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Init, m_bBlink);

			ToggleBlinkFlag(MS_SETUP, m_bBlink);

			break;
		case MS_READY:
			SetRedTower(m_nRed_On_Stop, m_bBlink);
 			SetAmberTower(m_nAmber_On_Stop, m_bBlink);
			SetGreenTower(m_nGreen_On_Stop, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Stop, m_bBlink);

			ToggleBlinkFlag(MS_READY, m_bBlink);

			break;
		case MS_AUTO:
			SetRedTower(m_nRed_On_Run, m_bBlink);
 			SetAmberTower(m_nAmber_On_Run, m_bBlink);
			SetGreenTower(m_nGreen_On_Run, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Run, m_bBlink);

			ToggleBlinkFlag(MS_AUTO, m_bBlink);

			SetLed(MS_AUTO);

			break;
		case MS_AUTO_STOP:
			SetRedTower(m_nRed_On_Stop, m_bBlink);
 			SetAmberTower(m_nAmber_On_Stop, m_bBlink);
			SetGreenTower(m_nGreen_On_Stop, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Stop, m_bBlink);

			ToggleBlinkFlag(MS_AUTO_STOP, m_bBlink);

			SetLed(MS_AUTO_STOP);
			break;
		case MS_ERROR:
			SetRedTower(m_nRed_On_Error, m_bBlink);
 			SetAmberTower(m_nAmber_On_Error, m_bBlink);
			SetGreenTower(m_nGreen_On_Error, m_bBlink);
			if (m_nForceBuzzFlag == eBuzz_DEFAULT)
				SetBuzzer(m_nBuzzer_On_Error, m_bBlink);

			ToggleBlinkFlag(MS_ERROR, m_bBlink);

			SetLed(MS_ERROR);

			break;
		default:
			break;
		}

		// Buzz가 강제 지정모드라면,
		if (m_nForceBuzzFlag != eBuzz_DEFAULT) {
			// 지정한 동작만 수행한다.
			if (m_nForceBuzzFlag == eBuzz_ON)
				m_opBuzzer.On();
			else 
				m_opBuzzer.Off();
		}
	}// of while
	return 0;
}

void CAxTower::SetAmberTower(int nOperation, BOOL m_bBlink)
{
	if(nOperation == TW_ON) {
		m_opAmberLight.On();
		m_opAmberLight2.On();
	}
	else if(nOperation == TW_OFF) {
		m_opAmberLight.Off();
		m_opAmberLight2.Off();
	}
	else if(nOperation == TW_BLINK) {
		if(m_bBlink == ON) {
			m_opAmberLight.On();
			m_opAmberLight2.On();
		}
		else {
			m_opAmberLight.Off();
			m_opAmberLight2.Off();
		}
	}
}

void CAxTower::SetGreenTower(int nOperation, BOOL m_bBlink)
{
	if(nOperation == TW_ON) {
		m_opGreenLight.On();
		m_opGreenLight2.On();
	}
	else if (nOperation == TW_OFF) {
		m_opGreenLight.Off();
		m_opGreenLight2.Off();
	}
	else if (nOperation == TW_BLINK) {
        if(m_bBlink == ON) {
			m_opGreenLight.On();
			m_opGreenLight2.On();
		}
		else {
			m_opGreenLight.Off();
			m_opGreenLight2.Off();
		}
	}
}


void CAxTower::SetRedTower(int nOperation, BOOL m_bBlink)
{
	if(nOperation == TW_ON) {
		m_opRedLight.On();
		m_opRedLight2.On();
	}
	else if (nOperation == TW_OFF) {
		m_opRedLight.Off();
		m_opRedLight2.Off();
	}
	else if (nOperation == TW_BLINK) {
        if(m_bBlink == ON) {
			m_opRedLight.On();
			m_opRedLight2.On();
		}
		else {
			m_opRedLight.Off();
			m_opRedLight2.Off();
		}
	}
}


void CAxTower::SetBuzzer(int nOperation, BOOL m_bBlink)
{
	CAxControlPanel* pControlPanel = (CAxControlPanel*)CAxSystemHub::GetSystemHub()->GetSystem(_T("ControlPanel"));

	if(!pControlPanel->GetResetState()) {
		m_opBuzzer.Off();
		return;
	}

//	if(pControlPanel->GetResetState()) {
//		m_opBuzzer.Off();
//		return;
//	}

	if(nOperation == TW_ON) {
		m_opBuzzer.On();
	}
	else if(nOperation == TW_OFF) {
		m_opBuzzer.Off();
	}
	else if (nOperation == TW_BLINK) {
        if(m_bBlink == ON) {
			m_opBuzzer.On();
		}
		else {
			m_opBuzzer.Off();
		}
	}
}


//
// Buzzer를 강제로 On/Off 시킬 수 있게 한다.
//
// [in] int nMode :
//		eBuzz_NONE	= 0,		// 강제 지정이 아니다.
//		eBuzz_ON,				// 강제로 ON/
//		eBuzz_OFF,				// 강제로 OFF
//
void CAxTower::SetForceBuzzer(int nMode)
{
	m_nForceBuzzFlag = nMode;
}
// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --






void CAxTower::ToggleBlinkFlag(int nMachineState, BOOL& m_bBlink)
{
 
	switch(nMachineState)
	{
	case MS_IDLE :
			if(m_nRed_On_Idle == TW_BLINK
			|| m_nAmber_On_Idle == TW_BLINK
			|| m_nGreen_On_Idle == TW_BLINK
			|| m_nBuzzer_On_Idle == TW_BLINK)
			{
		       m_bBlink ^= 1;
			}
		    break;
	case MS_SETUP :
		    if(m_nRed_On_Init == TW_BLINK
			|| m_nAmber_On_Init == TW_BLINK
			|| m_nGreen_On_Init == TW_BLINK
			|| m_nBuzzer_On_Init == TW_BLINK)
			{
		      m_bBlink ^= 1;
			}
		    break;

    case MS_READY :   
	case MS_AUTO_STOP:
			if(m_nRed_On_Stop == TW_BLINK
			|| m_nAmber_On_Stop == TW_BLINK
			|| m_nGreen_On_Stop == TW_BLINK
			|| m_nBuzzer_On_Stop == TW_BLINK)
			{
		      m_bBlink ^= 1;
			}
			break;
	case  MS_AUTO:
		 	if(m_nRed_On_Run == TW_BLINK
			|| m_nAmber_On_Run == TW_BLINK
			|| m_nGreen_On_Run == TW_BLINK
			|| m_nBuzzer_On_Run == TW_BLINK)
			{
		      m_bBlink ^= 1;
			}
		    break;
	case MS_ERROR:
    		if(m_nRed_On_Error == TW_BLINK
			|| m_nAmber_On_Error == TW_BLINK
			|| m_nGreen_On_Error == TW_BLINK
			|| m_nBuzzer_On_Error == TW_BLINK)
			{
		      m_bBlink ^= 1;
			}
            break;
	default:
		break;
	}
}

void CAxTower::SetLed(UINT nState)
{
	CAxControlPanel* pControlPanel = (CAxControlPanel*)CAxSystemHub::GetSystemHub()->GetSystem(_T("ControlPanel"));

	switch(nState) {
	case MS_IDLE:
		pControlPanel->SetAbortLed();
		break;
	case MS_AUTO:
		pControlPanel->SetStartLed();
		break;
	case MS_AUTO_STOP:
		pControlPanel->SetStopLed();
		break;
	case MS_ERROR:
		pControlPanel->SetErrorLed();
		break;
	default:
		break;
	}
}