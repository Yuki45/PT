#ifndef __AX_ERRORMGR_H__
#define __AX_ERRORMGR_H__

#pragma once

#include "AxService.h"
#include "AxErrMsg.h"

class __declspec(dllexport) CAxErrorMgr : public CAxService 
{
public:
	CMutex mutex;

	~CAxErrorMgr();

	void		RaiseError(const CAxErrData& Error);
	void		Startup();
	void		InitProfile();
	void		LoadProfile();
	void		SaveProfile();
	void		OnAfterSetResponse(int nResponse);
	int			GetResponse();
	void		SetResponse(int nResponse);
	void		ClearError();
	CAxErrData	GetErrData();

	static CAxErrorMgr* GetErrorMgr();

protected:
	CAxErrorMgr();

private:
	CAxErrData	m_err;
	UINT		m_nResponse;

	static CAxErrorMgr* theErrorMgr;
};

#endif