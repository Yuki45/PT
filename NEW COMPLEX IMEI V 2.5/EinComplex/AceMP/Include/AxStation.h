#ifndef __AX_STATION_H__
#define __AX_STATION_H__

#pragma once

#include "AxTask.h"
#include "AxIOCtrl.h"
#include "AxProfile.h"
#include "AxRecipe.h"
#include "AxServiceHub.h"
#include "AxRobotCtrl.h"

class __declspec(dllexport) CAxStation : public CAxTask, public CAxIOCtrl, public CAxRobotCtrl
{
public:
	CAxStation();
	virtual ~CAxStation();

	void SetRunMode(UINT nMode);
	void SetDryRun(BOOL bValue);
	BOOL GetDryRun();
	void SetBypass(BOOL bValue);
	BOOL GetBypass();
	void SetSimulate(BOOL bValue);
	BOOL GetSimulate();

	virtual void Startup();
	virtual void InitProfile();
	virtual void LoadProfile();
	virtual void SaveProfile();
	virtual void InitRecipe();
	virtual void LoadRecipe();
	virtual void LoadRecipe(LPCTSTR pszFile);
	virtual void SaveRecipe();
	virtual void SaveRecipe(LPCTSTR pszFile);
	virtual UINT AutoRun() = 0;
	virtual UINT ManualRun() = 0;
	virtual void Setup() = 0;
	virtual void EnterRun();
	virtual void ExitRun();
	virtual void PostAbort();
	virtual void PostStop(UINT nMode);
	virtual void PreStart() {}; //(M) 2014.07.17
	virtual void PreResume(UINT nMode);
	virtual void CheckStationState() {};

	void SetPreStart(BOOL bValue); //(M) 2014.07.17
	BOOL GetPreStart(); //(M) 2014.07.17
	
protected:
	UINT		m_nAutoState;
	BOOL		m_bBypass;
	BOOL		m_bDryRun;
	BOOL		m_bPreStart; //(M) 2014.07.17

	CAxProfile	m_profile;
	CAxRecipe	m_recipe;

private:
	UINT PriRun();
	UINT SecRun();
	void SetControl();
};

typedef CTypedPtrArray<CPtrArray, CAxStation*> CAxStationPtrArray;

#endif