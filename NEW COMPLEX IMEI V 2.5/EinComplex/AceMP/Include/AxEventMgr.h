#ifndef __AX_EVENTMGR_H__
#define __AX_EVENTMGR_H__

#pragma once

#include "AxEvent.h"
#include "AxService.h"
#include "AxControl.h"

class __declspec(dllexport) CAxEventMgr : public CAxService 
{
public:
	virtual ~CAxEventMgr();

	void		Startup();
	void		InitProfile();
	void		LoadProfile();
	void		SaveProfile();
	int			GetNumEvent();
	CAxEvent*	GetEvent(LPCTSTR pszAddr);
	void		SetupEvent(LPCTSTR pszAddr, CAxEvent& event);
	BOOL		SetEvent(LPCTSTR pszAddr);
	BOOL		ResetEvent(LPCTSTR pszAddr);

	static CAxEventMgr* GetEventMgr();

protected:
	CAxEventMgr();

private:
	UINT m_nNumEvents;

	static CAxEventMgr* theEventMgr;

	CMap<CString,LPCTSTR,CAxEvent*,CAxEvent*> m_eventMap;
};

#endif