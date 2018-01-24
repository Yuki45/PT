#ifndef __AX_THREAD_H__
#define __AX_THREAD_H__

#pragma once

#include "AxWaitTimer.h"

enum ThreadStatus {
	ACTIVE		= 1,
	SUSPENDED	= 2,
	DELETED		= 3
};

class __declspec(dllexport) CAxThread  
{
public:
	BOOL	m_bTerminate;
	HANDLE	m_hPriThread;
	HANDLE	m_hSecThread;

	CAxThread();
	virtual ~CAxThread();

	BOOL	InitInstance();
	BOOL	CreateAxThread();
	BOOL	ResumeAxThread(HANDLE hThread);
	BOOL	SuspendAxThread(HANDLE hThread);
	void	DeleteAxThread(HANDLE hThread);
	BOOL	CreateThreads();
	void	DeleteThreads();
	int		GetStatus(HANDLE hThread);

	virtual UINT PriRun() = 0;
	virtual UINT SecRun() = 0;

protected:
	CAxWaitTimer	m_timer;
	UINT			m_nScanTime;

private:
	int m_nPriStatus;

	void SetStatus(HANDLE hThread, int nStatus);
};

#endif