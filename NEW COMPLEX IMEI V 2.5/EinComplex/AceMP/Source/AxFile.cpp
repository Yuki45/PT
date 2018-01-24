#include "stdafx.h"
#include "AxFile.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BOOL __declspec(dllexport) AxCreateDirectoryAll(CString sPath)
{
	// make directory type
	sPath.TrimRight(_T("\\"));
	if( sPath.GetLength() <= 0 )
	{
		return FALSE;
	}

	// check directory exist -----
	WIN32_FIND_DATA fd;
	HANDLE hFind = FindFirstFile(sPath, &fd);
	if ( hFind != INVALID_HANDLE_VALUE )
	{
		FindClose( hFind );
		return TRUE;
	}

	// create all directory
	int nPos=1;
	while( (nPos = sPath.Find(_T('\\'), nPos)) != -1 )
	{
		CreateDirectory(sPath.Left(nPos+1), NULL);
		nPos++;
	}

	return CreateDirectory(sPath, NULL);
}

BOOL __declspec(dllexport) AxDeleteDirectoryAll(CString sPath)
{
	SHFILEOPSTRUCT FileOp = {0};

	sPath.TrimRight(_T('\\')); // 마지막 문자는 디렉토리문자(\)가 없어야 함
	sPath += _T('\0');

	FileOp.hwnd = NULL;
	FileOp.wFunc = FO_DELETE;
	FileOp.pFrom = sPath;
	FileOp.pTo = NULL;
	FileOp.fFlags = FOF_NOCONFIRMATION | FOF_NOERRORUI; // 확인메시지가 안뜨도록 설정
	FileOp.fAnyOperationsAborted = FALSE;
	FileOp.hNameMappings = NULL;
	FileOp.lpszProgressTitle = NULL;

	if( SHFileOperation(&FileOp) )
	{
		return FALSE;
	}

	return TRUE;
}

HANDLE __declspec(dllexport) AxCreateFile(CString sPathFile)
{
	int nPos = sPathFile.ReverseFind(_T('\\'));
	CString sPath = sPathFile.Left(nPos+1);
	CString sFile = sPathFile.Mid(nPos+1);

	if( sFile.IsEmpty() || sFile == _T("") )
	{
		return INVALID_HANDLE_VALUE;
	}

	if( !AxCreateDirectoryAll(sPath) )
	{
		return INVALID_HANDLE_VALUE;
	}

	HANDLE hFile = 
	CreateFile(
		sPathFile, 
		GENERIC_READ | GENERIC_WRITE, 
		FILE_SHARE_READ | FILE_SHARE_WRITE, 
		NULL, 
		CREATE_NEW, 
		FILE_FLAG_OVERLAPPED, 
		NULL
	); 

	return hFile;
}

HANDLE __declspec(dllexport) AxCreateFile(CString sPath, CString sFile)
{
	sFile.TrimLeft(_T('\\'));
	if( sFile.IsEmpty() || sFile == _T("") )
	{
		return INVALID_HANDLE_VALUE;
	}

	if( !(sPath.IsEmpty() || sPath == _T("")) )
	{
		sPath.TrimRight(_T('\\'));
		if( !AxCreateDirectoryAll(sPath) )
		{
			return INVALID_HANDLE_VALUE;
		}
		sPath += _T('\\');
	}

	CString sPathFile = sPath + sFile;

	HANDLE hFile = 
	CreateFile(
		sPathFile, 
		GENERIC_READ | GENERIC_WRITE, 
		FILE_SHARE_READ | FILE_SHARE_WRITE, 
		NULL, 
		CREATE_NEW, 
		FILE_FLAG_OVERLAPPED, 
		NULL
	); 

	return hFile;
}

CString __declspec(dllexport) AxGetPPString(LPCTSTR szAppName, LPCTSTR szKeyName, LPCTSTR szDefault, LPCTSTR szFileName)
{
	CString sValue;
	GetPrivateProfileString(szAppName, szKeyName, szDefault, sValue.GetBuffer(200), 200, szFileName);
	sValue.ReleaseBuffer();
	return sValue;
}

int __declspec(dllexport) AxGetPPInt(LPCTSTR szAppName, LPCTSTR szKeyName, int nDefault, LPCTSTR szFileName)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), nDefault);
	GetPrivateProfileString(szAppName, szKeyName, sDefault, sValue.GetBuffer(200), 200, szFileName);
	sValue.ReleaseBuffer();
	return _ttoi(sValue);
}

double __declspec(dllexport) AxGetPPDouble(LPCTSTR szAppName, LPCTSTR szKeyName, double dDefault, LPCTSTR szFileName)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%f"), dDefault);
	GetPrivateProfileString(szAppName, szKeyName, sDefault, sValue.GetBuffer(200), 200, szFileName);
	sValue.ReleaseBuffer();
	return _tcstod(sValue, NULL);
}

void __declspec(dllexport) AxWritePPString(LPCTSTR szAppName, LPCTSTR szKeyName, const CString & sValue, LPCTSTR szFileName)
{
	WritePrivateProfileString(szAppName, szKeyName, sValue, szFileName);
}

void __declspec(dllexport) AxWritePPInt(LPCTSTR szAppName, LPCTSTR szKeyName, const int & nValue, LPCTSTR szFileName)
{
	CString sValue;
	sValue.Format(_T("%d"), nValue);
	WritePrivateProfileString(szAppName, szKeyName, sValue, szFileName);
}

void __declspec(dllexport) AxWritePPDouble(LPCTSTR szAppName, LPCTSTR szKeyName, const double & dValue, LPCTSTR szFormat, LPCTSTR szFileName)
{
	CString sValue;
	sValue.Format(szFormat, dValue);
	WritePrivateProfileString(szAppName, szKeyName, sValue, szFileName);
}

CAxFile::CAxFile()
{

}

CAxFile::~CAxFile()
{

}
