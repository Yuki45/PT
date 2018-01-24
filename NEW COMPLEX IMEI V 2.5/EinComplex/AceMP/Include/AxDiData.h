#ifndef __AX_DIDATA_H__
#define __AX_DIDATA_H__

#pragma once

#include "AxObject.h"
#include "AxEvent.h"

#define ON	(BOOL)1
#define OFF	(BOOL)0

class __declspec(dllexport) CAxDiData : public CAxObject 
{
	friend class CAxInput;
	friend class CAxOutput;
	friend class CAxIOMgr;

public:
	CAxDiData();
	virtual ~CAxDiData();

	void SetValue(BOOL bValue);
	BOOL GetValue();
	void FireEvents();

	CString m_sName;
	CString m_sAddr;

private:
	BOOL		m_bValue;
	CAxEvent	m_evtOn;
	CAxEvent	m_evtOff;
};

typedef CArray<CAxDiData, CAxDiData&> CAxDiDataArray;

#endif