#ifndef __AX_TIMER_H__
#define __AX_TIMER_H__

#pragma once

class __declspec(dllexport) CAxTimer  
{
public:
/*
	LONG			m_time;
	LARGE_INTEGER	m_freq;

	CAxTimer();
	virtual ~CAxTimer();

	void Start();
	LONG GetTime();
	BOOL IsTimeUp(LONG nTm);
	void WaitTimeUp(LONG nTm);
	LONG IsTimeNow();
	BOOL IsSecTmup(LONG nTm);
*/
	DWORD m_dwStart;

	CAxTimer();
	virtual ~CAxTimer();

	void Start();
	BOOL IsTimeUp(LONG nTm);
	LONG IsTimeNow();
};

#endif
