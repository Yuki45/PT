#ifndef __AX_SERVICEHUB_H__
#define __AX_SERVICEHUB_H__

#pragma once

#include "AxIOMgr.h"
#include "AxEventMgr.h"
#include "AxErrorMgr.h"

class __declspec(dllexport) CAxServiceHub : public CAxObject  
{
public:
	virtual ~CAxServiceHub();

	void		Startup();
	void		Run();
	void		InitProfile();
	void		LoadProfile();
	void		SaveProfile();
	int			GetNumServices();
	void		AddService(CAxService* pService);
	CAxService*	GetService(int nService);
	CAxService*	GetService(LPCTSTR pszName);

	static CAxServiceHub* GetServiceHub();

protected:
	CAxServiceHub();

private:
	int					m_nNumServices;
	CAxServicePtrArray	m_service;

	static CAxServiceHub* theServiceHub;
};

#endif