#include "stdafx.h"
#include "AxErrorMgr.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxErrorMgr* CAxErrorMgr::theErrorMgr = NULL;

CAxErrorMgr::CAxErrorMgr()
{
	m_sName = _T("ErrorMgr");
	m_sErrPath = _T("\\Service\\ErrorMgr.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\ErrorMgr.ini");
}

CAxErrorMgr::~CAxErrorMgr()
{
}

CAxErrorMgr* CAxErrorMgr::GetErrorMgr()
{
	if( theErrorMgr == NULL ) theErrorMgr = new CAxErrorMgr();
	return theErrorMgr;
}

void CAxErrorMgr::Startup()
{
	CAxService::Startup();
}

void CAxErrorMgr::InitProfile()
{
	CAxService::InitProfile();
}

void CAxErrorMgr::LoadProfile()
{
	CAxService::LoadProfile();
}

void CAxErrorMgr::SaveProfile()
{
	CAxService::SaveProfile();
}

void CAxErrorMgr::OnAfterSetResponse(int nResponse)
{
	CAxMaster* pMaster = CAxMaster::GetMaster();
	m_nResponse = nResponse;

	if( m_nResponse == emAbort ) pMaster->Abort();
	else pMaster->Start();
}

void CAxErrorMgr::RaiseError(const CAxErrData& Error)
{
	m_err = Error;
}

int CAxErrorMgr::GetResponse()
{
	return m_nResponse;
}

void CAxErrorMgr::SetResponse(int nResponse)
{
	m_nResponse = nResponse;
}

void CAxErrorMgr::ClearError()
{
	m_err.Reset();
}

CAxErrData CAxErrorMgr::GetErrData()
{
	return m_err;
}