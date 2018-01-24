#ifndef __AX_ERRSTR_H__
#define __AX_ERRSTR_H__

#pragma once

#include "AxObject.h"

typedef CMap<UINT, UINT, CString, CString> CMapUintToString;

class __declspec(dllexport) CAxErrStr : public CAxObject
{
public:
	CAxErrStr();
	virtual ~CAxErrStr();

	CString Lookup(UINT nNum);

	CString& operator [] (UINT nNum);

private:
	CMapUintToString m_strMap;
};

#endif