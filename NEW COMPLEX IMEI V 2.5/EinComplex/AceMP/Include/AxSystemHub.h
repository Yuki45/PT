#ifndef __AX_SYSTEMHUB_H__
#define __AX_SYSTEMHUB_H__

#pragma once

// #include "AxSafety.h"
#include "AxSystemError.h"
// #include "AxTower.h"
// #include "AxControlPanel.h"

class __declspec(dllexport) CAxSystemHub : public CAxObject  
{
public:
	virtual ~CAxSystemHub();

	void				Startup();
	void				Run();
	void				InitProfile();
	void				LoadProfile();
	void				SaveProfile();
	int					GetNumSystem();
	void				AddSystem(CAxSystem* pSystem);
	CAxSystem*			GetSystem(int nSystem);
	CAxSystem*			GetSystem(LPCTSTR pszName);
	CAxSystemPtrArray&	GetSystem();

	static CAxSystemHub* GetSystemHub();

protected:
	CAxSystemHub();

private:
	int					m_nNumSystems;
	CAxSystemPtrArray	m_system;

	static CAxSystemHub* theSystemHub;
};

#endif