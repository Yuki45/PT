#ifndef __AX_OUTPUT_H__
#define __AX_OUTPUT_H__

#pragma once

#include "AxDiData.h"

#define ON	(BOOL)1
#define OFF	(BOOL)0

class __declspec(dllexport) CAxOutput  
{
	friend class CAxIOMgr;

public:
	CString m_sName;

	CAxOutput();
	CAxOutput(CAxOutput& output);
	CAxOutput(CAxDiData& diData, LPCTSTR pszAddr);
	virtual ~CAxOutput();

	void	On();
	void	Off();
	BOOL	GetValue();
	void	SetValue(BOOL bValue);
	LPCTSTR	GetAddr();

	BOOL		operator ==	(BOOL bValue);
	CAxEvent*	operator ^	(BOOL bValue);

private:
	CString		m_sAddr;
	CAxEvent*	m_pevtOn;
	CAxEvent*	m_pevtOff;
	BOOL*		m_pValue;
	BOOL		m_bReverse;

	void Init(CAxOutput* pOutput, BOOL bReverse);
};

typedef CArray<CAxOutput, CAxOutput&>			CAxOutputArray;
typedef CTypedPtrArray<CPtrArray, CAxOutput*>	CAxOutputPtrArray;

#endif
