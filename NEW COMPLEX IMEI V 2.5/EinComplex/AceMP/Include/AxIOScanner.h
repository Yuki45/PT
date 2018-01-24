#ifndef __AX_IOSCANNER_H__
#define __AX_IOSCANNER_H__

#pragma once

#include "AxIni.h"
#include "AxDiData.h"
#include "AxWaitTimer.h"

class __declspec(dllexport) CAxIOScanner : public CAxObject 
{
	friend class CAxIOMgr;

public:
	int				m_nErr;
	CStringArray	m_sErrStr;
	CAxDiDataArray	m_diData;
	CAxDiDataArray	m_doData;

	CAxIOScanner();
	virtual ~CAxIOScanner();

	UINT AutoScan();

	virtual void OnTimer() = 0;
	virtual BOOL Init(int nScanner, LPCTSTR pszFile) = 0;
	virtual BOOL ReadInput() = 0;
	virtual BOOL WriteOutput() = 0;
	virtual BOOL Reset() = 0;
	virtual BOOL Shutdown() = 0;
	virtual void SetError(int nErr) = 0;
	virtual void InitErrStr() = 0;

protected:
	enum {
		MIN_SCAN_TIME	= 10,
		NUM_SCAN_ERR	= 50,
		MAX_NUM_IO		= 1000
	};

	int				m_nScanner;
	int				m_nScannerType;
	int				m_nScanTime;
	BOOL			m_bInit;
	BOOL			m_bSimulate;
	BOOL			m_bTerminate;
	HANDLE			m_hScanThread;
	CAxWaitTimer	m_timer;
	CAxIOMgr*		m_pIOMgr;
	CAxIni			m_profile;
	int				m_nNumDiIp;
	int				m_nNumDiOp;

	void Startup();
};

typedef CTypedPtrArray<CPtrArray, CAxIOScanner*> CAxIOScannerPtrArray;

#endif