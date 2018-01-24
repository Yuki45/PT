#ifndef __AX_ERRORCTRL_H__
#define __AX_ERRORCTRL_H__

#pragma once

#include "AxErrMsg.h"
#include "AxControl.h"

class __declspec(dllexport) CAxErrorCtrl : CAxObject  
{
public:
	CAxErrMsg*	m_pErrMsg;
	CAxControl*	m_pControl;
	BOOL*		m_pSimulate;

	static BOOL	m_bError;
	static int	m_nErrCnt;

	CAxErrorCtrl();
	virtual ~CAxErrorCtrl();

	void	Load(CAxErrMsg* pErrMsg, CAxControl* pControl, BOOL* pSimulate);
	int		Error(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				  LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);
};

#endif