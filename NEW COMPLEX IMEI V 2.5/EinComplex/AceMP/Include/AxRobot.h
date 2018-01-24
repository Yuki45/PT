#ifndef __AX_ROBOT_H__
#define __AX_ROBOT_H__

#pragma once

#include "AxMmcCmd.h"
#include "AxAxis.h"
#include "AxRobotLoc.h"
#include "AxTrace.h"

typedef struct Rmt {
	double x, y, z, t;
	double speed, accel;
} RobotMt;

enum ROBOT_ERR {
	robotNoError	= 0,

	robotMmcError	= 100,
	robotFuncError	= 200,

	robotLastError
};

class __declspec(dllexport) CAxRobot : public CAxObject
{
public:
	UINT			m_nRobot;
	UINT			m_nRobotError;
	CAxAxisPtrArray	m_Axis;

	CAxRobot();
	virtual ~CAxRobot();

	BOOL Init(int nRobot, CAxAxis* pAxis, LPCTSTR pszFile);
	UINT GetErrAxis();
	UINT GetNumAxis();
	BOOL IsHomeCheck();
	BOOL IsHomeCheck(UINT nAxis);
	BOOL IsAmpEnabled();
	BOOL IsAmpEnabled(UINT nAxis);
	BOOL IsAmpDisabled();
	BOOL IsAmpDisabled(UINT nAxis);
	BOOL IsRobotReady();
	BOOL IsRobotReady(UINT nAxis);
	BOOL IsMotionDone();
	BOOL IsAxisDone();
	BOOL GetCurLoc(CAxRobotLoc* locCur);
	BOOL GetCurLocXZAx(CAxRobotLoc* locCur);
	BOOL EnableAmp();
	BOOL EnableAmp(UINT nAxis);
	BOOL DisableAmp();
	BOOL DisableAmp(UINT nAxis);
	BOOL AlarmClear();
	BOOL AlarmClear(UINT nAxis);
	BOOL ClearStatus();
	BOOL ClearStatus(UINT nAxis);
	BOOL Home();
	BOOL Home(UINT nAxis);
	BOOL Move(const CAxRobotLoc& loc);
	BOOL Move(const CAxRobotLoc& loc, double speed);
	BOOL MoveXZAx(const CAxRobotLoc& loc);
	BOOL MoveXZAx(const CAxRobotLoc& loc, double speed);
	BOOL Approach(const CAxRobotLoc& loc);
	BOOL Approach(const CAxRobotLoc& loc, double speed);
	BOOL ApproachXZAx(const CAxRobotLoc& loc);
	BOOL ApproachXZAx(const CAxRobotLoc& loc, double speed);
	BOOL Depart();
	BOOL Depart(double speed);
	BOOL Depart(const CAxRobotLoc& loc);
	BOOL Depart(const CAxRobotLoc& loc, double speed);
	BOOL DepartXZAx();
	BOOL DepartXZAx(double speed);
	BOOL DepartXZAx(const CAxRobotLoc& loc);
	BOOL DepartXZAx(const CAxRobotLoc& loc, double speed);
	BOOL JumpXZAx(const CAxRobotLoc& loc);
	BOOL JumpXZAx(const CAxRobotLoc& loc, double speed);
	BOOL JumpXYZAx(const CAxRobotLoc& loc);
	BOOL JumpXYZAx(const CAxRobotLoc& loc, double speed);
	BOOL Stop();
	BOOL Abort();
	BOOL Resume();

private:
	CAxIni		m_profile;
	CAxMmcCmd*	m_pCmd;
	AxisLoc*	m_pAxisLoc;
	UINT*		m_pnAxisMap;
	UINT		m_nNumAxis;
	BOOL		m_bAxisMapToggle;
	CString		m_sAxisMap;
	double		m_dAppro;
	double		m_dDepart;
	UINT		m_nErrAxis;
	BOOL		m_bSimulate;
	CAxTrace	m_Trace;

	void GetProfile();
	void MapAxis(CAxAxis* pAxis);
	void HandleException(UINT nExp);
	void GetAxisError(UINT nAxis);
	void SetRobotError(UINT nErr);
	BOOL MoveMulti(AxisLoc* pAxisLoc, BOOL bSlow = FALSE);
};

#endif