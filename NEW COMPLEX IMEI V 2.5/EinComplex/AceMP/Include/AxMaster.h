#ifndef __AX_MASTER_H__
#define __AX_MASTER_H__

#pragma once

#include "AxTimer.h"
#include "AxErrorMgr.h"
#include "AxSystemHub.h"
#include "AxStationHub.h"
#include "AxTrace.h"

enum MachineState {
	MS_INIT,
	MS_IDLE,
	MS_SETUP,
	MS_READY,
	MS_AUTO,
	MS_MANUAL,
	MS_AUTO_STOP= 7,
	MS_MANUAL_STOP,
	MS_ERROR
};

enum StopType {
	ST_AIR_STOP,
	ST_POWER_STOP,
	ST_DOOR_STOP,
	ST_VACUUM_STOP,
	ST_AREA_STOP,
	ST_EMG_STOP,
	ST_CMD_STOP,
	ST_ERR_STOP
};

class __declspec(dllexport) CAxMaster : public CAxObject, public CAxThread
{
	enum {
		NO_ERR_TASK = -1
	};

public:
	UINT m_msgErr;
	UINT m_msgStart;
	UINT m_msgStop;
	UINT m_msgAbort;
	UINT m_msgMessage;
	UINT m_msgErrMessage;

	~CAxMaster();

	void	OnPublishNameSpace();
	UINT	GetState();
	UINT	GetPrevState();
	UINT	GetPreManualState();
	void	Startup();
	void	Run();
	void	Start();
	void	Stop();
	void	Abort();
	void	Cmd();
	void	SetResponse(int nResponse);
	void	SetMainWnd(CWnd* pWnd, UINT msgErr, UINT msgStart, UINT msgStop, UINT msgAbort, UINT msgMessage, UINT msgErrMessage);
	CWnd*	GetMainWnd();
	BOOL	GetTeminate();

	static CAxMaster* GetMaster();

protected:
	CAxMaster();

private:
	int					m_nErrTask;
	int					m_nNumTasks;
	UINT				m_nState;
	UINT				m_nRunMode;
	UINT				m_nPrevState;
	UINT				m_nPreManual;
	UINT				m_nPreError;
	UINT				m_nMinErrSpan;
	CAxTaskPtrArray		m_task;
	CAxEventPtrArray	m_event;
	CAxEventPtrArray	m_start;
	CAxEventPtrArray	m_stop;
	CAxEventPtrArray	m_abort;
	CAxEventPtrArray	m_idle;
	CAxEventPtrArray	m_stopped;
	CAxEventPtrArray	m_cmd;
	CAxEventPtrArray	m_done;
	CAxEventPtrArray	m_error;
	CAxTimer			m_timer;
	CAxIni				m_profile;
	CAxTrace			m_Trace;
	CAxErrorMgr*		m_pErrorMgr;
	CAxStationHub*		m_pStationHub;
	CWnd*				m_pMainWnd;

	static CAxMaster* theMaster;

	UINT		PriRun();
	UINT		SecRun();
	void		InitState();
	void		IdleState();
	void		SetupState();
	void		ReadyState();
	void		AutoState();
	void		ManualState();
	void		AutoStopState();
	void		ManualStopState();
	void		ErrorState();	
	void		SetEvents(CAxEventPtrArray& event);
	void		ResetEvents(CAxEventPtrArray& event);
	void		PackAllTasks();
	void		InitTaskEvents();
	void		SetTaskEvents(BOOL bSet, CAxEventPtrArray& event);
	void		StartTasks(UINT nMode);
	void		StopTasks();
	void		AbortTasks();
	void		ResetTasks();
	void		ResumeTasks(UINT nMode);
	void		SetState(UINT nMode);
	void		SetSetupCmd();
	void		SetRunMode(UINT nMode);
	void		RunPostStop();
	void		RaiseError();
	void		RunPreResume();
	CAxEvent*	WaitEvents(CAxEvent* pFirstEvent, ...);
	int			MonitorEvents(BOOL bWaitAll, CAxEventPtrArray& event);
};

#endif