// AxTrace.cpp: implementation of the CAxTrace class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxTrace.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

UINT CAxTrace::m_nLimitDay = 30;

CAxTrace::CAxTrace()
{
//	m_nLimitDay = 100;
//	m_sPath = GetCurrentPath();
	m_sFileName = _T("TraceDefault");
	m_bEnableWriteLog = TRUE;
}

CAxTrace::CAxTrace(CString sRootPath, CString sFileName)
{
//	m_nLimitDay = 100;
	m_sPath = sRootPath;
	m_sFileName = sFileName;
	m_bEnableWriteLog = TRUE;

	AxCreateDirectoryAll(m_sPath);
}

CAxTrace::~CAxTrace()
{
}

void CAxTrace::SetPathFile(CString sRootPath, CString sFileName)
{
	m_sPath = sRootPath;
	m_sFileName = sFileName;

	AxCreateDirectoryAll(m_sPath);
}

//void CAxTrace::SetTraceFileLimit(UINT nLimit)
//{
//	m_nLimitDay = nLimit;
//}
//
//UINT CAxTrace::GetTraceFileLimit()
//{
//	return m_nLimitDay;
//}

void CAxTrace::Log(LPCTSTR pszTrace, ...)
{
	va_list args;
	CString Trace = _T("");
	CString sLog;
	WORD wBOM = 0xFEFF;
//	WORD wEND[] = {0x000d, 0x000a};
    DWORD NumberOfBytesWritten;

	if( !m_bEnableWriteLog ) { return; }
	
    HANDLE hFile = 
	CreateFile(
		GetTracePathFile(), 
		GENERIC_WRITE, 
		FILE_SHARE_READ|FILE_SHARE_WRITE, // FILE_SHARE_WRITE, 
		NULL, 
		OPEN_ALWAYS, 
		FILE_ATTRIBUTE_NORMAL, 
		NULL
	);

	if( hFile == INVALID_HANDLE_VALUE ) { return; }

	DWORD dwError = GetLastError();

#ifdef _UNICODE
	if( dwError == ERROR_SUCCESS )
	{
		WriteFile(hFile, &wBOM, sizeof(wBOM), &NumberOfBytesWritten, NULL);
	}
#endif

	va_start(args, pszTrace);
	Trace.FormatV(pszTrace, args);
	va_end(args);
 	sLog.Format(_T("%s %s\r\n"), (LPCTSTR)GetTime(), (LPCTSTR)Trace);

	SetFilePointer(hFile, 0, NULL, FILE_END);
	WriteFile(hFile, sLog, (_tcslen(sLog))*(sizeof(TCHAR)), &NumberOfBytesWritten, NULL);
//	WriteFile(hFile, wEND, sizeof(wEND), &NumberOfBytesWritten, NULL);
	CloseHandle(hFile);
}

void CAxTrace::LogCSV(LPCTSTR pszTrace, ...)
{
	va_list args;
	CString Trace = _T("");
	CString sLog;
	WORD wBOM = 0xFEFF;
	//	WORD wEND[] = {0x000d, 0x000a};
	DWORD NumberOfBytesWritten;

	if( !m_bEnableWriteLog ) { return; }

	HANDLE hFile = 
		CreateFile(
		GetTracePathFileCSV(), 
		GENERIC_WRITE, 
		FILE_SHARE_READ|FILE_SHARE_WRITE, // FILE_SHARE_WRITE, 
		NULL, 
		OPEN_ALWAYS, 
		FILE_ATTRIBUTE_NORMAL, 
		NULL
		);

	if( hFile == INVALID_HANDLE_VALUE ) { return; }

	DWORD dwError = GetLastError();

#ifdef _UNICODE
	if( dwError == ERROR_SUCCESS )
	{
		WriteFile(hFile, &wBOM, sizeof(wBOM), &NumberOfBytesWritten, NULL);
	}
#endif

	va_start(args, pszTrace);
	Trace.FormatV(pszTrace, args);
	va_end(args);
	sLog.Format(_T("%s %s\r\n"), (LPCTSTR)GetTime(), (LPCTSTR)Trace);

	SetFilePointer(hFile, 0, NULL, FILE_END);
	WriteFile(hFile, sLog, (_tcslen(sLog))*(sizeof(TCHAR)), &NumberOfBytesWritten, NULL);
	//	WriteFile(hFile, wEND, sizeof(wEND), &NumberOfBytesWritten, NULL);
	CloseHandle(hFile);
}

void CAxTrace::Delete(CString sRootPath)
{
	CFileFind   clsFileFind;
	BOOL		bContinue = TRUE;
	CString		sPath;
	CString		sFile;
	CString		sFindFile;

	int  nYear, nMon, nDay;
	COleDateTime  dirtime;
	COleDateTimeSpan difftm;

	sFindFile = sRootPath + _T("\\*.*");

	if(clsFileFind.FindFile(sFindFile)){
		while(bContinue){
			bContinue = clsFileFind.FindNextFile();
			
			sPath = clsFileFind.GetFilePath();
			sFile = clsFileFind.GetFileName();

			if(clsFileFind.IsDots())
				continue;

			if(clsFileFind.IsDirectory()){
				if( sFile.GetLength() != 8 )
					continue;

				nYear = _ttoi(sFile.Mid(0, 4));
				nMon  = _ttoi(sFile.Mid(4, 2));
				nDay  = _ttoi(sFile.Mid(6, 2));

				if( nYear == 0 || nMon == 0 || nDay == 0 )
					continue;

				dirtime = COleDateTime(nYear, nMon, nDay, 0, 0, 0);
				difftm  = COleDateTime::GetCurrentTime()-dirtime;

				if(m_nLimitDay <= (UINT)difftm.GetTotalDays()){
					AxDeleteDirectoryAll(sPath);
				}

//				if(IsDirectoryDelete(sFile)){
//					DeleteDirectoryAll(sPath);
//				}
			}
			else{
				DeleteFile(sPath);
			}
		}
	}
}

CString CAxTrace::GetTracePathFile()
{
	CFileFind clsFileFind;
	CString sTracePath;
	CString sTraceFile;
	COleDateTime time = COleDateTime::GetCurrentTime();

	sTracePath.Format(_T("%s\\%s"), (LPCTSTR)m_sPath, (LPCTSTR)time.Format(_T("%Y%m%d")));

	if(!clsFileFind.FindFile(sTracePath)){
		AxCreateDirectoryAll(sTracePath);
	}		

	sTraceFile.Format(_T("%s\\%s.log"), (LPCTSTR)sTracePath, (LPCTSTR)m_sFileName);
	
	return sTraceFile;
}

CString CAxTrace::GetTracePathFileCSV()
{
	CFileFind clsFileFind;
	CString sTracePath;
	CString sTraceFile;
	COleDateTime time = COleDateTime::GetCurrentTime();

	sTracePath.Format(_T("%s\\%s"), (LPCTSTR)m_sPath, (LPCTSTR)time.Format(_T("%Y%m%d")));

	if(!clsFileFind.FindFile(sTracePath)){
		AxCreateDirectoryAll(sTracePath);
	}		

	sTraceFile.Format(_T("%s\\%s.csv"), (LPCTSTR)sTracePath, (LPCTSTR)m_sFileName);

	return sTraceFile;
}

CString CAxTrace::GetTime()
{	
	SYSTEMTIME time;
	CString sTime;

	GetLocalTime(&time);

	sTime.Format(_T("[%02d:%02d:%02d.%03d]"), time.wHour, 
		time.wMinute, time.wSecond, time.wMilliseconds);

	return sTime;
}

BOOL CAxTrace::IsDirectoryDelete(CString sDirName)
{
	int  nYear, nMon, nDay;
	COleDateTime  dirtime;
	COleDateTimeSpan difftm;

	nYear = _ttoi(sDirName.Mid(0, 4));
	nMon  = _ttoi(sDirName.Mid(4, 2));
	nDay  = _ttoi(sDirName.Mid(6, 2));

	dirtime = COleDateTime(nYear, nMon, nDay, 0, 0, 0);
	difftm  = COleDateTime::GetCurrentTime()-dirtime;

	if(m_nLimitDay <= (UINT)difftm.GetTotalDays())
		return TRUE;

	return FALSE;
}

//void CAxTrace::SetEnableWriteLog(BOOL bValue)
//{
//	m_bEnableWriteLog = bValue;
//}
//
//BOOL CAxTrace::GetEnableWriteLog()
//{
//	return m_bEnableWriteLog;
//}