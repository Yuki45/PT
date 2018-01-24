// AxProfile.cpp: implementation of the CAxProfile class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxProfile.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxProfile::CAxProfile()
{
	m_pIOMgr = CAxIOMgr::GetIOMgr();
	m_pEventMgr = CAxEventMgr::GetEventMgr();
}

CAxProfile::~CAxProfile()
{

}

void CAxProfile::Load()
{
	CAxIni::Load();
}

void CAxProfile::Load(LPCTSTR pszFile)
{
	m_sIniFile = pszFile;
	CAxIni::Load();
}

void CAxProfile::Save()
{
	CAxIni::Save();
}

void CAxProfile::Save(LPCTSTR pszFile)
{
	m_sIniFile = pszFile;
	CAxIni::Save();
}

void CAxProfile::AddEvent(LPCTSTR pszKey, CAxEvent& event)
{
	SetupEvent(pszKey, event);
	m_eventMap.SetAt(pszKey, &event);
}

void CAxProfile::AddEvent(LPCTSTR pszSect, LPCTSTR pszKey, CAxEvent& event)
{
	SetupEvent(pszSect, pszKey, event);
	m_eventMap.SetAt(pszKey, &event);
}

void CAxProfile::AddInput(LPCTSTR pszKey, CAxInput& input)
{
	SetupInput(pszKey, input);
	m_inputMap.SetAt(pszKey, &input);
}

void CAxProfile::AddInput(LPCTSTR pszSect, LPCTSTR pszKey, CAxInput& input)
{
	SetupInput(pszSect, pszKey, input);
	m_inputMap.SetAt(pszKey, &input);
}

void CAxProfile::AddOutput(LPCTSTR pszKey, CAxOutput& output)
{
	SetupOutput(pszKey, output);
	m_outputMap.SetAt(pszKey, &output);
}

void CAxProfile::AddOutput(LPCTSTR pszSect, LPCTSTR pszKey, CAxOutput& output)
{
	SetupOutput(pszSect, pszKey, output);
	m_outputMap.SetAt(pszKey, &output);
}

void CAxProfile::SetupEvent(LPCTSTR pszKey, CAxEvent& event)
{
	CString sAddr;
	ReadString(_T("Events"), pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pEventMgr->SetupEvent(sAddr, event);
	event.m_sName = pszKey;
}

void CAxProfile::SetupEvent(LPCTSTR pszSect, LPCTSTR pszKey, CAxEvent& event)
{
	CString sAddr;
	ReadString(pszSect, pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pEventMgr->SetupEvent(sAddr, event);
	event.m_sName = pszKey;
}

void CAxProfile::SetupInput(LPCTSTR pszKey, CAxInput& input)
{
	CString sAddr;
	ReadString(_T("Inputs"), pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pIOMgr->SetInput(sAddr, input, pszKey);
	input.m_sName = pszKey;
}

void CAxProfile::SetupInput(LPCTSTR pszSect, LPCTSTR pszKey, CAxInput& input)
{
	CString sAddr;
	ReadString(pszSect, pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pIOMgr->SetInput(sAddr, input, pszKey);
	input.m_sName = pszKey;
}

void CAxProfile::SetupOutput(LPCTSTR pszKey, CAxOutput& output)
{
	CString sAddr;
	ReadString(_T("Outputs"), pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pIOMgr->SetOutput(sAddr, output, pszKey);
	output.m_sName = pszKey;
}

void CAxProfile::SetupOutput(LPCTSTR pszSect, LPCTSTR pszKey, CAxOutput& output)
{
	CString sAddr;
	ReadString(pszSect, pszKey, sAddr.GetBuffer(7), 7);
	sAddr.ReleaseBuffer();
	m_pIOMgr->SetOutput(sAddr, output, pszKey);
	output.m_sName = pszKey;
}
