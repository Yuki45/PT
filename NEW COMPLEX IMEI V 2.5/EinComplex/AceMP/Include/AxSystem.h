#ifndef __AX_SYSTEM_H__
#define __AX_SYSTEM_H__

#pragma once

#include "AxTask.h"
#include "AxIOCtrl.h"
#include "AxProfile.h"
#include "AxServiceHub.h"

class __declspec(dllexport) CAxSystem : public CAxTask, public CAxIOCtrl 
{
public:
	CAxSystem();
	virtual ~CAxSystem();

	void SetControl();
	void SetRunMode(UINT nMode);
	void OnLoadProfile();
	void OnSaveProfile();
	void SetSimulate(BOOL bValue);
	BOOL GetSimulate();

	virtual void Startup();
	virtual void InitProfile();
	virtual void LoadProfile();
	virtual void SaveProfile();
	virtual UINT AutoRun() = 0;

protected:
	CAxProfile m_profile;

private:
	UINT PriRun();
	UINT SecRun() { return 0; }
};

typedef CTypedPtrArray<CPtrArray, CAxSystem*> CAxSystemPtrArray;

#endif