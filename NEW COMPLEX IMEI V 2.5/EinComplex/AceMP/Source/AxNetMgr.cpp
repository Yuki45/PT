#include "stdafx.h"
#include "AxNetMgr.h"

#include "AxDefine.h"
#include "AxNetMelsecDrv.h"
#include "AxNetEipDrv.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxNetMgr* CAxNetMgr::theNetMgr = NULL;

CAxNetMgr::CAxNetMgr()
{
	m_sName = _T("NetMgr");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\NetMgr.ini");

	m_nNumDriver = 0;
	m_nNumData   = 0;
	m_nBitSize   = 0;
	m_nWordSize  = 0;
}

CAxNetMgr::~CAxNetMgr()
{
	// delete net driver
	for(UINT i=0; i<m_nNumDriver; i++)
		delete m_arrDriver[i];

	// delete net data
	CString sKey = _T("");
	CAxNetData* pData = NULL;

	POSITION pos = m_mapData.GetStartPosition();
	while( pos != NULL ) 
	{
		m_mapData.GetNextAssoc(pos, sKey, pData);
		delete pData;
	}
}

CAxNetMgr* CAxNetMgr::GetNetMgr()
{
	if( theNetMgr == NULL ) theNetMgr = new CAxNetMgr();
	return theNetMgr;
}

void CAxNetMgr::Setup()
{
	CAxService::Setup();
}

void CAxNetMgr::InitProfile()
{
	CAxService::InitProfile();

	m_profile.AddUint(_T("NumDriver"), m_nNumDriver);
	m_profile.AddUint(_T("NumData"),   m_nNumData);
	m_profile.AddUint(_T("BitSize"),   m_nBitSize);
	m_profile.AddUint(_T("WordSize"),  m_nWordSize);
}

void CAxNetMgr::LoadProfile()
{
	CAxService::LoadProfile();

	CreateData();
	CreateDriver();

	InitData();
	InitDriver();
}

void CAxNetMgr::SaveProfile()
{
	CAxService::SaveProfile();
}

void CAxNetMgr::Startup()
{
	CAxService::Startup();
}

BOOL CAxNetMgr::CreateData()
{
	CString sName = _T("");
	for( UINT i=0; i<m_nNumData; i++ ) 
	{
		m_profile.m_sSect.Format(_T("Data%d"), i);
		sName = m_profile.ReadStr(_T("Name"));

		m_mapData.SetAt(sName, new CAxNetData(i));
	}

	return TRUE;
}

BOOL CAxNetMgr::CreateDriver()
{
	int nType = 0;
	for( UINT i=0; i<m_nNumDriver; i++ ) 
	{
		m_profile.m_sSect.Format(_T("Driver%d"), i);
		nType = m_profile.ReadInt(_T("Type"));

		switch( nType ) 
		{
		case AX_NET_MELSECNET:
			m_arrDriver.Add(new CAxNetMelsecDrv(i, this));
			break;
		case AX_NET_ETHERNET:
			m_arrDriver.Add(new CAxNetEipDrv(i, this));
			break;
		default:
			ASSERT(FALSE);
		}
	}

	return TRUE;
}

BOOL CAxNetMgr::InitData()
{
	CString sKey = _T("");
	CAxNetData* pData = NULL;
	CAxNetDriver* pDriver = NULL;
	CString sMsg = _T("");

	POSITION pos = m_mapData.GetStartPosition();
	while( pos != NULL ) 
	{
		m_mapData.GetNextAssoc(pos, sKey, pData);
		if( !pData->Init(m_profile.m_sIniFile, m_nBitSize, m_nWordSize) )
		{
			sMsg.Format(_T("NetData(%d) Initialize Failed."), pData->GetID());
			AfxMessageBox(sMsg);
			ASSERT( FALSE );
			return FALSE;
		}
		
		pDriver = GetNetDriver(pData->GetDriverID());
		ASSERT( pDriver != NULL );

		pDriver->AddNetData(pData);
	}

	return TRUE;
}

BOOL CAxNetMgr::InitDriver()
{
	CString sMsg = _T("");
	CAxNetDriver* pDriver = NULL;
	
	for( UINT i=0; i<m_nNumDriver; i++ ) 
	{
		pDriver = m_arrDriver[i];
		if( !pDriver->Init(m_profile.m_sIniFile) ) 
		{
			sMsg.Format(_T("NetDriver(%d) Initialize Failed."), pDriver->GetID());
			AfxMessageBox(sMsg);
			ASSERT( FALSE );
			return FALSE;
		}
	}

	return TRUE;
}

void CAxNetMgr::ImmediateTimer()
{
	for( UINT i=0; i<m_nNumDriver; i++ )
		m_arrDriver[i]->ImmediateTimer();
}

void CAxNetMgr::ImmediateTimer(UINT nID)
{
	ASSERT( nID >= 0 && nID < m_nNumDriver );
	m_arrDriver[nID]->ImmediateTimer();
}

CAxNetData* CAxNetMgr::GetNetData(LPCTSTR pszName)
{
	CAxNetData* pData = NULL;
	if( !m_mapData.Lookup(pszName, pData) )
	{
		ASSERT( FALSE );
		return NULL;
	}

	return pData;
}

CAxNetDriver* CAxNetMgr::GetNetDriver(UINT nID)
{
	int nSize = m_arrDriver.GetSize();

	for( int i=0; i<nSize; i++ )
	{
		if( m_arrDriver[i]->GetID() == nID )
			return m_arrDriver[i];
	}

	return NULL;
}

