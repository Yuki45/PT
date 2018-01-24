#ifndef __AX_FILE_H__
#define __AX_FILE_H__

#pragma once

BOOL	__declspec(dllexport) AxCreateDirectoryAll(CString sPath);
BOOL	__declspec(dllexport) AxDeleteDirectoryAll(CString sPath);
HANDLE	__declspec(dllexport) AxCreateFile(CString sPathFile);
HANDLE	__declspec(dllexport) AxCreateFile(CString sPath, CString sFile);
CString	__declspec(dllexport) AxGetPPString(LPCTSTR szAppName, LPCTSTR szKeyName, LPCTSTR szDefault, LPCTSTR szFileName);
int		__declspec(dllexport) AxGetPPInt(LPCTSTR szAppName, LPCTSTR szKeyName, int nDefault, LPCTSTR szFileName);
double	__declspec(dllexport) AxGetPPDouble(LPCTSTR szAppName, LPCTSTR szKeyName, double dDefault, LPCTSTR szFileName);
void	__declspec(dllexport) AxWritePPString(LPCTSTR szAppName, LPCTSTR szKeyName, const CString& sValue, LPCTSTR szFileName);
void	__declspec(dllexport) AxWritePPInt(LPCTSTR szAppName, LPCTSTR szKeyName, const int& nValue, LPCTSTR szFileName);
void	__declspec(dllexport) AxWritePPDouble(LPCTSTR szAppName, LPCTSTR szKeyName, const double& dValue, LPCTSTR szFormat, LPCTSTR szFileName);

class __declspec(dllexport) CAxFile
{
public:
	CAxFile();
	virtual ~CAxFile();
};

#endif