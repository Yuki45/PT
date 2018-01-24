#ifndef __AX_MMCMGR_H__
#define __AX_MMCMGR_H__

#pragma once

#include "AxService.h"
#include "AxMmcCmd.h"
#include "AxAxis.h"
#include "AxRobot.h"
#include "AxTrace.h"

class __declspec(dllexport) CAxMmcMgr : public CAxService  
{
public:
	virtual ~CAxMmcMgr();

	void		Startup();
	void		InitProfile();
	void		LoadProfile();
	void		SaveProfile();
	UINT		AutoRun();
	UINT		GetNumRobot();
	int			GetRobotNum(LPCTSTR szRobotName);
	CAxRobot*	GetRobot(UINT nRobotNum);
	CAxRobot*	GetRobot(LPCTSTR szRobotName);
	UINT		GetNumAxis();
	int			GetAxisNum(LPCTSTR szAxisName);
	CAxAxis*	GetAxis(UINT nAxisNum);
	CAxAxis*	GetAxis(LPCTSTR szAxisName);
	BOOL		GetSimulate();

	static CAxMmcMgr* GetMmcMgr();

protected:
	CAxMmcMgr();

private:
	UINT		m_nNumCtr;
	UINT		m_nNumAxis;
	UINT		m_nNumRobot;
	CAxMmcCmd*	m_pMmcCmd;
	CAxRobot*	m_pRobot;
	CAxAxis*	m_pAxis;
	CAxTrace	m_Trace;

	static CAxMmcMgr* theMmcMgr;

	UINT PriRun();
	void LoadCtr();
	void LoadAxis();
	void LoadRobot();
};

#endif