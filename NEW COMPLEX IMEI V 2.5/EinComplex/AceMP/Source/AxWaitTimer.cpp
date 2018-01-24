#include "stdafx.h"
#include "AxWaitTimer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxWaitTimer::CAxWaitTimer()
{
	m_nDueTm  = 0;
	m_nPeriod = 0;
	m_hTimer = CreateWaitableTimer(NULL, FALSE, NULL);
}

CAxWaitTimer::~CAxWaitTimer()
{
	CloseHandle(m_hTimer);
}

void CAxWaitTimer::SetTimer(int nDueTm, int nPeriod)
{
	LARGE_INTEGER nDueTime;

	m_nDueTm  = nDueTm;
	m_nPeriod = nPeriod;

	nDueTime.QuadPart = nDueTm * -10000000;
	SetWaitableTimer(m_hTimer, &nDueTime, nPeriod, NULL, NULL, FALSE);
}

void CAxWaitTimer::WaitTimer()
{
	WaitForSingleObject(m_hTimer, INFINITE);
}

void CAxWaitTimer::ImmediateTimer()
{
	CancelWaitableTimer(m_hTimer);
	SetTimer(m_nDueTm, m_nPeriod);
}
