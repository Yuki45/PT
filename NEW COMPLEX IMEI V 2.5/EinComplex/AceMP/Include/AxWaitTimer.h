#ifndef __AX_WAITTIMER_H__
#define __AX_WAITTIMER_H__

#pragma once

class _declspec(dllexport) CAxWaitTimer  
{
public:
	CAxWaitTimer();
	virtual ~CAxWaitTimer();

	void SetTimer(int nDueTm, int nPeriod);
	void WaitTimer();
	void ImmediateTimer();

private:
	HANDLE	m_hTimer;
	int		m_nDueTm;
	int		m_nPeriod;
};

#endif