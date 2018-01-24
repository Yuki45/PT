#include "stdafx.h"
#include "AxErrStr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxErrStr::CAxErrStr()
{

}

CAxErrStr::~CAxErrStr()
{

}

CString& CAxErrStr::operator [] (UINT nNum)
{
	return m_strMap[nNum];
}

CString CAxErrStr::Lookup(UINT nNum)
{
	CString str;
	m_strMap.Lookup(nNum, str);
	return str;
}

