#ifndef __AX_IOMGR_H__
#define __AX_IOMGR_H__

#pragma once

#include "AxInput.h"
#include "AxOutput.h"
#include "AxService.h"
#include "AxIOScanner.h"
#include "AxHilscherScanner.h"
#include "AxTrace.h"

class __declspec(dllexport) CAxIOMgr : public CAxService 
{
public:
	int m_nNumDiIp;
	int m_nNumDiOp;

	virtual ~CAxIOMgr();

	void		Startup();
	void		InitProfile();
	void		LoadProfile();
	void		SaveProfile();
	void		CreateIO();
	CAxInput*	GetInput(LPCTSTR pszAddr);
	CAxOutput*	GetOutput(LPCTSTR pszAddr);
	void		SetInput(LPCTSTR pszAddr, CAxInput& ip);
	void		SetInput(LPCTSTR pszAddr, CAxInput& ip, LPCTSTR pszKey);
	void		SetOutput(LPCTSTR pszAddr, CAxOutput& op);
	void		SetOutput(LPCTSTR pszAddr, CAxOutput& op, LPCTSTR pszKey);

	static CAxIOMgr* GetIOMgr();

protected:
	CAxIOMgr();

private:
	int			m_nNumScanner;
	CMutex		m_mutex;
	CSingleLock	m_lock;
	CAxTrace	m_Trace;

	static CAxIOMgr* theIOMgr;

public:
	CAxIOScannerPtrArray m_scanner;

	CMap<CString,LPCTSTR,CAxInput*,CAxInput*>	m_inputMap;
	CMap<CString,LPCTSTR,CAxOutput*,CAxOutput*>	m_outputMap;
};

#endif