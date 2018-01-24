#ifndef __AX_INPUT_H__
#define __AX_INPUT_H__

#pragma once

#include "AxDiData.h"

#define ON	(BOOL)1
#define OFF	(BOOL)0

class __declspec(dllexport) CAxInput  
{
	friend class CAxIOMgr;

public:
	CString m_sName;

	CAxInput();
	CAxInput(CAxInput& input);
	CAxInput(CAxDiData& diData, LPCTSTR pszAddr);
	virtual ~CAxInput();

	void	On();
	void	Off();
	void	SetValue(BOOL bValue);
	BOOL	GetValue();
	LPCTSTR	GetAddr();

	BOOL		operator ==	(BOOL bValue);
	CAxEvent*	operator ^	(BOOL bValue);


private:
	CString		m_sAddr;
	CAxEvent*	m_pevtOn;
	CAxEvent*	m_pevtOff;
	BOOL*		m_pValue;
	BOOL		m_bReverse;

	void Init(CAxInput* pInput, BOOL bReverse);
};

typedef CArray<CAxInput, CAxInput&>				CAxInputArray;
typedef CTypedPtrArray<CPtrArray, CAxInput*>	CAxInputPtrArray;

#endif