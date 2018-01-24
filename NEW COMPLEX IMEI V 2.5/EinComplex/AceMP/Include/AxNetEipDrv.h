#ifndef __AX_NETEIPDRV_H__
#define __AX_NETEIPDRV_H__

#pragma once

#include "AxNetMgr.h"
#include "AxNetDriver.h"

class __declspec(dllexport) CAxNetEipDrv : public CAxNetDriver 
{
public:
	CAxNetEipDrv(UINT nID, CAxNetMgr* pNetMgr);
	virtual ~CAxNetEipDrv();

	BOOL Init(LPCTSTR pszFile); // virtual

private:
	enum {
		RW_BYTES = 4
	};

	BOOL SetVariable();														// virtual 
	BOOL CheckVariable();													// virtual
	BOOL InitDriver();														// virtual
	BOOL ResetDriver();														// virtual
	BOOL CloseDriver();														// virtual
	BOOL CheckDriverError(int nVal, BOOL bReInit);
	void OnTimer();															// virtual
	BOOL ReadData();
	BOOL WriteData();
	BOOL ReadBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL ReadWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL WriteBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL WriteWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
};

#endif