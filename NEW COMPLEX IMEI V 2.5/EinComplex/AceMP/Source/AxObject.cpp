// AxObject.cpp: implementation of the CAxObject class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxObject.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CString CAxObject::m_sRootPath = _T("");

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxObject::CAxObject()
{

}

CAxObject::~CAxObject()
{

}

CString CAxObject::GetRootPath()
{
	return m_sRootPath;
}
