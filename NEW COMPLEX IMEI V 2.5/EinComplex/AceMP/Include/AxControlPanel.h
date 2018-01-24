#ifndef __AX_CONTROLPANEL_H__
#define __AX_CONTROLPANEL_H__

#pragma once

#include "AxSystem.h"

enum ControlPanel {
	START_BTN = 0,
	START_BTN2,
	STOP_BTN,
	STOP_BTN2,
	RESET_BTN,
	RESET_BTN2,
};

class __declspec(dllexport) CAxControlPanel : public CAxSystem
{
public:
	CAxInput  m_ipStart;
	CAxInput  m_ipStop;
	CAxInput  m_ipReset;
	CAxInput  m_ipStart2;
	CAxInput  m_ipStop2;
	CAxInput  m_ipReset2;
	CAxOutput m_opStart;
	CAxOutput m_opStop;
	CAxOutput m_opReset;
	CAxOutput m_opStart2;
	CAxOutput m_opStop2;
	CAxOutput m_opReset2;
	BOOL      m_bReset;

	CAxControlPanel();
	virtual ~CAxControlPanel();

	void Startup();
	void InitProfile();
	void SetStartLed();
	void SetStopLed();
	void SetAbortLed();
	void SetErrorLed();
	void SetLed();
	void ResetAlarm();
	BOOL GetResetState();
	void StartLockInc();
	void StartLockDec();
	UINT AutoRun();

private:
	int m_nStartLock;
};

#endif