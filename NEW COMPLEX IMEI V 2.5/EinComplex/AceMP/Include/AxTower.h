#ifndef __AX_TOWER_H__
#define __AX_TOWER_H__

#pragma once

#include "AxSystem.h"
#include "AxMaster.h"

class __declspec(dllexport) CAxTower : public CAxSystem 
{
	enum {
		TW_OFF,
		TW_ON,
		TW_BLINK
	};

	enum {
		BLINK_TIMER = 500
	};

public:
	enum {
		eBuzz_DEFAULT = 0,	// NoForce
		eBuzz_ON,			// Force ON
		eBuzz_OFF			// Force OFF
	};

	int			m_nForceBuzzFlag;
	CAxOutput	m_opBuzzer;
	CAxOutput	m_opRedLight;
	CAxOutput	m_opGreenLight;
	CAxOutput	m_opAmberLight;
	CAxOutput	m_opRedLight2;
	CAxOutput	m_opGreenLight2;
	CAxOutput	m_opAmberLight2;
	int			m_nRed_On_Run;
	int			m_nGreen_On_Run;
	int			m_nAmber_On_Run;
	int			m_nBuzzer_On_Run;
	int			m_nRed_On_Stop;
	int			m_nGreen_On_Stop;
	int			m_nAmber_On_Stop;
	int			m_nBuzzer_On_Stop;
	int			m_nRed_On_Idle;
	int			m_nGreen_On_Idle;
	int			m_nAmber_On_Idle;
	int			m_nBuzzer_On_Idle;
	int			m_nRed_On_Init;
	int			m_nGreen_On_Init;
	int			m_nAmber_On_Init;
	int			m_nBuzzer_On_Init;
	int			m_nRed_On_Error;
	int			m_nGreen_On_Error;
	int			m_nAmber_On_Error;
	int			m_nBuzzer_On_Error;
	BOOL		m_bBlink;

	CAxTower();
	virtual ~CAxTower();

	void Startup();
	void InitProfile();
	UINT AutoRun();
	void ToggleBlinkFlag();
	void SetGreenTower(int nOperation, BOOL bBlink);
	void SetRedTower(int nOperation, BOOL bBlink);
	void SetAmberTower(int nOperation, BOOL bBlink);
	void SetBuzzer(int nOperation, BOOL bBlink);
	void ToggleBlinkFlag(int nState, BOOL& bBlink);
	void SetLed(UINT nState);
	void SetForceBuzzer(int nMode);
};

#endif