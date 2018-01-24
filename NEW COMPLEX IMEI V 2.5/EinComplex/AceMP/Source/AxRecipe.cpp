// AxRecipe.cpp: implementation of the CAxRecipe class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxRecipe.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxRecipe::CAxRecipe()
{

}

//CAxRecipe::CAxRecipe(CAxRecipe& recipe)
//{
//
//}

CAxRecipe::~CAxRecipe()
{

}

void CAxRecipe::Load()
{
	CAxIni::Load();
}

void CAxRecipe::Load(LPCTSTR pszFile)
{
	m_sIniFile = pszFile;
	CAxIni::Load();
}

void CAxRecipe::Save()
{
	CAxIni::Save();
}

void CAxRecipe::Save(LPCTSTR pszFile)
{
	m_sIniFile = pszFile;
	CAxIni::Save();
}
