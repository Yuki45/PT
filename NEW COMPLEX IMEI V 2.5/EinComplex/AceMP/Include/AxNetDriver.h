#ifndef __AX_NETDRIVER_H__
#define __AX_NETDRIVER_H__

#pragma once

#include "AxWaitTimer.h"
#include "AxNetData.h"

class CAxNetMgr;
class __declspec(dllexport) CAxNetDriver 
{
	friend class CAxNetMgr;

public:
	CAxNetDriver();
	virtual ~CAxNetDriver();

	UINT AutoScan();
	void ImmediateTimer();
	UINT GetID() { return m_nID; }
	void AddNetData(CAxNetData* pNetData);

	virtual BOOL Init(LPCTSTR pszFile);

protected:
	enum {
		MIN_SCAN_TIME = 10,
		MAX_SCAN_TIME = 100
	};

	CString				m_sName;
	UINT				m_nID;
	UINT				m_nType;		// 0 : MelsecNet, 1 : EtherNet
	UINT				m_nScanTime;	// 폴링시간(업데이트시간) [ms]
	BOOL				m_bInit;
	BOOL				m_bSimulate;
	BOOL				m_bTerminate;
	HANDLE				m_hScanThread;
	CAxWaitTimer		m_timer;
	CAxNetMgr*			m_pNetMgr;
	CMutex				m_mutex;
	HANDLE				m_hMutex;
	CAxNetDataPtrArray	m_parrNetData;

	void Startup();

	virtual BOOL SetVariable() = 0;
	virtual BOOL CheckVariable();
	virtual BOOL InitDriver() = 0;
	virtual BOOL ResetDriver() = 0;
	virtual BOOL CloseDriver() = 0;
	virtual void OnTimer() = 0;
};

typedef CTypedPtrArray<CPtrArray, CAxNetDriver*> CAxNetDriverPtrArray;

#endif