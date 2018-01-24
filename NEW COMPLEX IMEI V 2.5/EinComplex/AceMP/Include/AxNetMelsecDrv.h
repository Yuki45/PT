#ifndef __AX_NETMELSECDRV_H__
#define __AX_NETMELSECDRV_H__

#pragma once

#include "AxNetMgr.h"
#include "AxNetDriver.h"

class __declspec(dllexport) CAxNetMelsecDrv : public CAxNetDriver 
{
public:
	CAxNetMelsecDrv(UINT nID, CAxNetMgr* pNetMgr);
	virtual ~CAxNetMelsecDrv();

	BOOL Init(LPCTSTR pszFile); // virtual

private:
	enum {
		STATION_NO = 0xFF,
		RW_BYTES = 2
	};

	UINT m_nChannel;
	long m_hHandle;

	BOOL SetVariable();															// virtual 
	BOOL CheckVariable();														// virtual
	BOOL InitDriver();															// virtual
	BOOL ResetDriver();															// virtual
	BOOL CloseDriver();															// virtual
	BOOL CheckDriverError(int nVal, BOOL bReInit);
	void OnTimer();																// virtual
	BOOL ReadData();
	BOOL WriteData();
	BOOL ReadBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL ReadWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL WriteBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
	BOOL WriteWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData);
};

#endif