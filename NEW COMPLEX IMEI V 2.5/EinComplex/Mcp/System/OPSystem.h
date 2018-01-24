#ifndef __OPSYSTEM_H__
#define __OPSYSTEM_H__

#pragma once

#include <AxSystem.h>
#include <AxMaster.h>

class COPSystem : public CAxSystem
{
public:
	COPSystem();
	virtual ~COPSystem();

	void InitProfile();
	void Startup();
	UINT AutoRun();

	void SetBuzzer(BOOL bOn);

	BOOL m_bForceBuzzer;
	BOOL m_bBuzzerStop;
	BOOL m_bBuzzerBlink;
	CAxTimer m_tmBuzzerBlink;
	CAxTimer m_tmBuzzerTimeout;

	void SetLeftGreenLamp(BOOL bOn);
	void SetLeftRedLamp(BOOL bOn);
	void SetLeftYellowLamp(BOOL bOn);
	void SetLeftGreenTower(BOOL bOn);
	void SetLeftRedTower(BOOL bOn);
	void SetLeftYellowTower(BOOL bOn);
	void SetLeftStartSwitch(BOOL bOn);
	void SetLeftStopSwitch(BOOL bOn);
	void SetLeftResetSwitch(BOOL bOn);
	BOOL GetLeftStartSwitch();
	BOOL GetLeftStopSwitch();
	BOOL GetLeftResetSwitch();

	BOOL m_bLeftBlink;
	CAxTimer m_tmLeftBlink;

	void SetRightGreenLamp(BOOL bOn);
	void SetRightRedLamp(BOOL bOn);
	void SetRightYellowLamp(BOOL bOn);
	void SetRightGreenTower(BOOL bOn);
	void SetRightRedTower(BOOL bOn);
	void SetRightYellowTower(BOOL bOn);
	void SetRightStartSwitch(BOOL bOn);
	void SetRightStopSwitch(BOOL bOn);
	void SetRightResetSwitch(BOOL bOn);
	BOOL GetRightStartSwitch();
	BOOL GetRightStopSwitch();
	BOOL GetRightResetSwitch();

	BOOL m_bRightBlink;
	CAxTimer m_tmRightBlink;

	BOOL m_bEStopError;
	int m_nBlinkInterval;
	int m_nAutoBuzzerStopTime;
};

#endif
