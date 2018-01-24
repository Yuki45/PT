#ifndef __AX_TASK_H__
#define __AX_TASK_H__

#pragma once

#include "AxObject.h"
#include "AxThread.h"
#include "AxControl.h"
#include "AxErrStr.h"
#include "AxErrorCtrl.h"

enum TaskGroup {
	TG_SERVICE,
	TG_SYSTEM,
	TG_STATION
};

class __declspec(dllexport) CAxTask : public CAxObject, public CAxThread
{
	friend class CAxMaster;

public:
	CAxTask();
	virtual ~CAxTask();

	int			m_nTaskGroup;
	CAxErrMsg	m_errMsg;
	CString		m_sErrPath;
	CAxErrStr	m_errStr;
	CAxControl	m_control;

	void		Start();
	void		Stop();
	void		Abort();
	void		Reset();
	UINT		GetState();
	void		SetState(UINT nValue);
	void		SetResponse(UINT nResp);
	void		SetCmdCode(int nCode);
	CAxEvent*	GetCtrlEvent(UINT nType);
	UINT		GetResponse();
	int			Error(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
					  LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	virtual UINT PriRun() = 0;
	virtual void Startup() = 0;
	virtual void InitProfile() = 0;
	virtual void LoadProfile() = 0;
	virtual void SaveProfile() = 0;
	virtual void SetRunMode(UINT nMode) = 0;

protected:
	int				m_nErrCode;
	int				m_nCmdCode;
	BOOL			m_bSimulate;
	CAxErrorCtrl	m_errCtrl;

	void	DeleteThreads();
	void	SetIdleControl(CAxEventPtrArray& event);
	void	SetRunControl(CAxEventPtrArray& event);
	BOOL	SetEvent(CAxEvent* pEvent);
	BOOL	SetEvent(CAxEvent& event);
	void	SetEvents(CAxEventPtrArray& evt);
	BOOL	ResetEvent(CAxEvent* pEvent);
	BOOL	ResetEvent(CAxEvent& event);
	void	ResetEvents(CAxEventPtrArray& evt);
	void	WaitStart(CAxEventPtrArray& evt);
	void	Wait(DWORD dwTime);
	int		WaitEvent(CAxEvent* pEvent);
	int		WaitEvent(CAxEvent& evt);
	int		WaitEvent(DWORD dwTimeout, CAxEvent* pEvent);
	int		WaitEvent(DWORD dwTimeout, CAxEvent& evt);
	void	WaitEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pEvent);
	void	WaitEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent& evt);
	int		WaitEvents(CAxEvent* pFirstEvent, ...);
	int		WaitEvents(CAxEventPtrArray& event);
	int		WaitEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitAllEvents(CAxEvent* pFirstEvent, ...);
	int		WaitAllEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitAllEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitAllEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitAllEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	void	WaitNS(DWORD dwTime);
	int		WaitNSEvent(DWORD dwTimeout, CAxEvent* pEvent);
	int		WaitNSEvent(DWORD dwTimeout, CAxEvent& evt);
	void	WaitNSEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pEvent);
	void	WaitNSEvent(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent& evt);
	int		WaitNSEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitNSEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitNSEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitNSEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitNSAllEvents(CAxEvent* pFirstEvent, ...);
	int		WaitNSAllEvents(DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitNSAllEvents(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		WaitNSAllEvents(BOOL bReset, DWORD dwTimeout, CAxEvent* pFirstEvent, ...);
	void	WaitNSAllEvents(BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEvent* pFirstEvent, ...);
	int		IsEvents(DWORD dwTimeout, CAxEventPtrArray& evt);
	int		IsAllEvents(DWORD dwTimeout, CAxEventPtrArray& evt);
	
private:
	int WaitTimeout(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
	int WaitAllTimeout(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
};

typedef CTypedPtrArray<CPtrArray, CAxTask*> CAxTaskPtrArray;

#endif