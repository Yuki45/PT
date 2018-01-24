#ifndef __AX_TRACE_H__
#define __AX_TRACE_H__

#pragma once

#include "AxFile.h"

class __declspec(dllexport) CAxTrace
{

public:
	CString	m_sPath;
	CString	m_sFileName;
	BOOL	m_bEnableWriteLog;

	static UINT m_nLimitDay;

	CAxTrace();
	CAxTrace(CString sRootPath, CString sFileName);
	virtual ~CAxTrace();

	void SetPathFile(CString sRootPath, CString sFileName);
	void Log(LPCTSTR pszTrace, ...);
	void LogCSV(LPCTSTR pszTrace, ...);

	static void Delete(CString sRootPath);

private:
	CString	GetTracePathFile();
	CString	GetTracePathFileCSV();
	CString	GetTime();
	BOOL	IsDirectoryDelete(CString dirname);
};

#endif