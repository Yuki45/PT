#ifndef __AX_NETMGR_H__
#define __AX_NETMGR_H__

#pragma once

#include "AxService.h"
#include "AxNetDriver.h"
#include "AxNetData.h"

class __declspec(dllexport) CAxNetMgr : public CAxService 
{
public:
	virtual ~CAxNetMgr();

	void			Setup();
	void			InitProfile();
	void			LoadProfile();
	void			SaveProfile();
	void			Startup();
	CAxNetData*		GetNetData(LPCTSTR pszName);
	CAxNetDriver*	GetNetDriver(UINT nID);
	void			ImmediateTimer();
	void			ImmediateTimer(UINT nID);

	static CAxNetMgr* GetNetMgr();

private:
	UINT					m_nNumDriver;
	UINT					m_nNumData;
	UINT					m_nBitSize;		// ���ô� Bit �� (Byte ����)
	UINT					m_nWordSize;	// ���ô� Word �� (Byte ����)
	CAxNetDriverPtrArray	m_arrDriver;
	CMapStringToNetData		m_mapData;

	static CAxNetMgr* theNetMgr;

	CAxNetMgr();

	BOOL CreateData();
	BOOL CreateDriver();
	BOOL InitData();
	BOOL InitDriver();
};

#endif