#ifndef __COMMSYSTEM_H__
#define __COMMSYSTEM_H__

#pragma once

#include <AxSystem.h>

class CMainFrame;
class CCommSystem : public CAxSystem
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_RUN
	};

	CCommSystem();
	virtual ~CCommSystem();

	void InitProfile();
	void Startup();
	UINT AutoRun();

	void AsInit();
	void AsRun();

	UINT m_nAutoState;

	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;
};

#endif
