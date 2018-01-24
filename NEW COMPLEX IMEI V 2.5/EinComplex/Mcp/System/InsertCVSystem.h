#ifndef __INSERTCVSYSTEM_H__
#define __INSERTCVSYSTEM_H__

#pragma once

#include <AxSystem.h>
#include "AxTimer.h"

class CMainFrame;
class CInsertCVSystem : public CAxSystem
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_RUN
	};

	CInsertCVSystem();
	virtual ~CInsertCVSystem();

	void InitProfile();
	void Startup();
	UINT AutoRun();

	void AsInit();
	void AsRun();

	void _oCVRun();
	void _oCVStop();

	BOOL chkExitSensor();
	BOOL chkLoadCVExitSensor();
	BOOL chkLoadCVRun();

	UINT m_nAutoState;

	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;

	CAxTimer m_tmMachineStop;
};

#endif
