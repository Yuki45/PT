#ifndef __AX_ROBOTCTRL_H__
#define __AX_ROBOTCTRL_H__

#pragma once

#include "AxTimer.h"
#include "AxMmcCmd.h"
#include "AxErrorCtrl.h"
#include "AxRobot.h"
#include "AxTrace.h"

enum {
	motionScanTime = 5
};

class __declspec(dllexport) CAxRobotCtrl  
{
public:
	CAxRobot* m_pRobot;

	CAxRobotCtrl(CAxErrorCtrl* pCtrl);
	virtual ~CAxRobotCtrl();

	BOOL IsHomeCheck();
	BOOL IsHomeCheck(UINT nAxis);
	BOOL IsAmpEnabled();
	BOOL IsAmpEnabled(UINT nAxis);
	BOOL IsAmpDisabled();
	BOOL IsAmpDisabled(UINT nAxis);
	BOOL IsRobotReady();
	BOOL IsRobotReady(UINT nAxis);
	BOOL GetCurLoc(CAxRobotLoc* locCur);
	void EnableAmp();
	void EnableAmp(UINT nAxis);
	BOOL EnableAmpNE();
	BOOL EnableAmpNE(UINT nAxis);
	void DisableAmp();
	void DisableAmp(UINT nAxis);
	BOOL DisableAmpNE();
	BOOL DisableAmpNE(UINT nAxis);
	void AlarmClear();
	void AlarmClear(UINT nAxis);
	BOOL AlarmClearNE();
	BOOL AlarmClearNE(UINT nAxis);
	void ClearStatus();
	void ClearStatus(UINT nAxis);
	BOOL ClearStatusNE();
	BOOL ClearStatusNE(UINT nAxis);
	void Home();
	void Home(UINT nAxis);
	BOOL HomeNE();
	BOOL HomeNE(UINT nAxis);
	void Move(const CAxRobotLoc& loc);
	void Move(const CAxRobotLoc& loc, double speed);
	BOOL MoveNE(const CAxRobotLoc& loc);
	BOOL MoveNE(const CAxRobotLoc& loc, double speed);
	void MoveXZAx(const CAxRobotLoc& loc);
	void MoveXZAx(const CAxRobotLoc& loc, double speed);
	BOOL MoveXZAxNE(const CAxRobotLoc& loc);
	BOOL MoveXZAxNE(const CAxRobotLoc& loc, double speed);
	void MoveWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void MoveWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL MoveWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL MoveWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void MoveXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void MoveXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL MoveXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL MoveXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void Approach(const CAxRobotLoc& loc);
	void Approach(const CAxRobotLoc& loc, double speed);
	BOOL ApproachNE(const CAxRobotLoc& loc);
	BOOL ApproachNE(const CAxRobotLoc& loc, double speed);
	void ApproachXZAx(const CAxRobotLoc& loc);
	void ApproachXZAx(const CAxRobotLoc& loc, double speed);
	BOOL ApproachXZAxNE(const CAxRobotLoc& loc);
	BOOL ApproachXZAxNE(const CAxRobotLoc& loc, double speed);
	void ApproachWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void ApproachWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL ApproachWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL ApproachWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void ApproachXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void ApproachXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL ApproachXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL ApproachXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void Depart();
	void Depart(double speed);
	void Depart(const CAxRobotLoc& loc);
	void Depart(const CAxRobotLoc& loc, double speed);
	BOOL DepartNE();
	BOOL DepartNE(double speed);
	BOOL DepartNE(const CAxRobotLoc& loc);
	BOOL DepartNE(const CAxRobotLoc& loc, double speed);
	void DepartXZAx();
	void DepartXZAx(double speed);
	void DepartXZAx(const CAxRobotLoc& loc);
	void DepartXZAx(const CAxRobotLoc& loc, double speed);
	BOOL DepartXZAxNE();
	BOOL DepartXZAxNE(double speed);
	BOOL DepartXZAxNE(const CAxRobotLoc& loc);
	BOOL DepartXZAxNE(const CAxRobotLoc& loc, double speed);
	void DepartWaitDone(UINT nTmout);
	void DepartWaitDone(UINT nTmout, double speed);
	void DepartWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void DepartWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL DepartWaitDoneNE(UINT nTmout);
	BOOL DepartWaitDoneNE(UINT nTmout, double speed);
	BOOL DepartWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL DepartWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void DepartXZAxWaitDone(UINT nTmout);
	void DepartXZAxWaitDone(UINT nTmout, double speed);
	void DepartXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void DepartXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL DepartXZAxWaitDoneNE(UINT nTmout);
	BOOL DepartXZAxWaitDoneNE(UINT nTmout, double speed);
	BOOL DepartXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL DepartXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void JumpXZAx(const CAxRobotLoc& loc);
	void JumpXZAx(const CAxRobotLoc& loc, double speed);
	BOOL JumpXZAxNE(const CAxRobotLoc& loc);
	BOOL JumpXZAxNE(const CAxRobotLoc& loc, double speed);
	void JumpXYZAx(const CAxRobotLoc& loc);
	void JumpXYZAx(const CAxRobotLoc& loc, double speed);
	BOOL JumpXYZAxNE(const CAxRobotLoc& loc);
	BOOL JumpXYZAxNE(const CAxRobotLoc& loc, double speed);
	void JumpXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void JumpXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL JumpXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL JumpXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void JumpXYZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout);
	void JumpXYZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed);
	BOOL JumpXYZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout);
	BOOL JumpXYZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed);
	void WaitAxisDone(UINT nTime);
	BOOL WaitAxisDoneNE(UINT nTime);
	void WaitMotion(UINT nTime);
	UINT RobotError(UINT nCode, int nType = emNone);
	BOOL IsRobotStopState();
	void CheckMasterTerminate();

private:
	CString			m_sRobotErrPath;
	CAxErrorCtrl*	m_pErrCtrl;
	CAxTimer		m_timer;
	CAxTrace		m_Trace;
};

#endif