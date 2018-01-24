#ifndef __AX_SERVICE_H__
#define __AX_SERVICE_H__

#pragma once

#include "AxIni.h"
#include "AxTask.h"

class __declspec(dllexport) CAxService : public CAxTask 
{
public:
	CAxService();
	virtual ~CAxService();

	void OnLoadProfile();
	void OnSaveProfile();
	void SetRunMode(UINT nMode);

	virtual void Setup();
	virtual void Startup();
	virtual void InitProfile();
	virtual void LoadProfile();
	virtual void SaveProfile();

protected:
	CAxIni m_profile;

private:
	UINT PriRun() { return 0; }
	UINT SecRun() { return 0; }
};

typedef CTypedPtrArray<CPtrArray, CAxService*> CAxServicePtrArray;

#endif