// AxTimer.cpp: implementation of the CAxTimer class.
//
// 2014.08.11 : LMW Renewal
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "AxTimer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
/*
CAxTimer::CAxTimer()
{
 	QueryPerformanceFrequency(&m_freq);
	Start();
}

CAxTimer::~CAxTimer()
{
}

void CAxTimer::Start()
{
	m_time = GetTime();
}

LONG CAxTimer::GetTime()
{
	LARGE_INTEGER count;
	
	QueryPerformanceCounter(&count); // (count / sec)
	return (LONG)((double)count.QuadPart / (double)m_freq.QuadPart * 1000);
}

BOOL CAxTimer::IsTimeUp(LONG nTm)
{
	LONG tmSpan = GetTime() - m_time;
	return tmSpan >= nTm;
}

void CAxTimer::WaitTimeUp(LONG nTm)
{
	LONG tmSpan = GetTime() - m_time;
	while(tmSpan < nTm) {
		Sleep(50);
		tmSpan = GetTime() - m_time;
	}
}

// use Stop Watch Function
BOOL CAxTimer::IsSecTmup(LONG nTm)
{
	LONG tmSpan = GetTime() - m_time;
	if( tmSpan >= nTm )
	{ 
		Start();
		return TRUE;
	}
	else return FALSE;
}

LONG CAxTimer::IsTimeNow()
{
	return GetTime() - m_time;
}
*/

CAxTimer::CAxTimer()
{
	Start();
}

CAxTimer::~CAxTimer()
{
}

void CAxTimer::Start()
{
	m_dwStart = GetTickCount();
}

BOOL CAxTimer::IsTimeUp(LONG nTm)
{
	return (IsTimeNow() >= nTm);
}

LONG CAxTimer::IsTimeNow()
{
	DWORD dwElasp = 0;
	DWORD dwStop = GetTickCount();

	if( m_dwStart <= dwStop ) dwElasp = dwStop - m_dwStart;
	else
	{
		dwElasp = 0xFFFFFFFFL - m_dwStart;
		dwElasp += 1;
		dwElasp += dwStop;
	}

	return (LONG)dwElasp;
}
