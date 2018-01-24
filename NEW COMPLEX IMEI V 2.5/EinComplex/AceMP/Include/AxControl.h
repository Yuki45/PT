#ifndef __AX_CONTROL_H__
#define __AX_CONTROL_H__

#pragma once

#include "AxEvent.h"

enum ControlType {
	CT_START,
	CT_STOP,
	CT_ABORT,
	CT_IDLE,
	CT_STOPPED,
	CT_CMD,
	CT_DONE,
	CT_ERROR
};

enum TaskState {
	TS_INIT,
	TS_IDLE,
	TS_SETUP,
	TS_READY,
	TS_AUTO,
	TS_MANUAL,
	TS_ERROR
};

enum {
	NO_CMD			= -1,
	NUM_CTS			= 8,
	PRI_RUN			= 1,
	SEC_RUN			= 2,
	MAX_WAIT_EVTS	= 50,
	WAIT_INTERVAL	= 50
};

class __declspec(dllexport) CAxControl
{
public:
	UINT				m_nState;
	UINT				m_nRunMode;
	UINT				m_nStationNum;
	int					m_nResponse;
	CAxEventPtrArray	m_evt;
	CAxEventPtrArray	m_priEvt;
	CAxEventPtrArray	m_secEvt;

	CAxControl();
	virtual ~CAxControl();

	void	SetRunMode(UINT nMode);
	void	SetStationNum(UINT nStation);
	void	CheckStationState();
	void	WaitStart();
	void	WaitStart(CAxEventPtrArray& evt);
	int		WaitStop(DWORD dwTime);
	int		WaitResponse();
	void	Wait(DWORD dwTime);
	void	WaitNS(DWORD dwTime);
	int		WaitEvents(DWORD dwTimeout, CAxEventPtrArray& evt);
	int		WaitEvents(BOOL bAddStop, DWORD dwTimeout, CAxEventPtrArray& evt);
	int		WaitEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt);
	int		WaitAllEvents(DWORD dwTimeout, CAxEventPtrArray& evt);
	int		WaitAllEvents(BOOL bAddStop, DWORD dwTimeout, CAxEventPtrArray& evt);
	int		WaitAllEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt);
	int		MultiWaitEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt);
	int		MultiWaitAllEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt);
	void	Start();
	void	Stop();
	void	Abort();
	void	Abort(CAxEventPtrArray& event);
	void	Reset();
	void	Reset(CAxEventPtrArray& event);

	void PreStart(); //(M) 2014.07.17
};

#endif