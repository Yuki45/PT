#ifndef __AX_OBJECT_H__
#define __AX_OBJECT_H__

#pragma once

class __declspec(dllexport) CAxObject  
{
public:
	UINT	m_nID;
	CString	m_sName;

	static CString m_sRootPath;

	CAxObject();
	virtual ~CAxObject();

	static CString GetRootPath();
};

#endif