#ifndef __AX_EVENT_H__
#define __AX_EVENT_H__

#pragma once

#include "AxObject.h"

enum ForceMode {
	FM_NO_FORCE,
	FM_FORCE_ON,
	FM_FORCE_OFF
};

class __declspec(dllexport) CAxEvent : public CAxObject
{
	friend class CAxEventMgr;

public:
	CString m_sName;

	CAxEvent(BOOL bInitiallyOwn = FALSE, BOOL bManualReset = TRUE, 
			 LPCTSTR pszName = NULL, LPSECURITY_ATTRIBUTES lpsaAttribute = NULL);
	CAxEvent(LPCTSTR pszAddr, BOOL bInitiallyOwn = FALSE, BOOL bManualReset = TRUE, 
			 LPCTSTR pszName = NULL, LPSECURITY_ATTRIBUTES lpsaAttribute = NULL);
    ~CAxEvent();   

	CEvent*	GetEvent();
	UINT	GetForceMode();
	void	SetForceMode(UINT nMode);
	BOOL	Set();
	BOOL	Reset();
	BOOL	IsSet();	

private:
	CString	m_sAddr;
	CEvent	m_event;
	CEvent*	m_pEvent;
	UINT	m_nForceMode;

	void Init(CAxEvent* pEvent);
};

typedef CArray<CAxEvent, CAxEvent&>				CAxEventArray;
typedef CTypedPtrArray<CPtrArray, CAxEvent*>	CAxEventPtrArray;

#endif