#include "stdafx.h"
#include "AxNetEipDrv.h"

#include "EipFunc.h"
#include "AxIni.h"
#include "AxController.h"
#include "AxConvertVariable.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxNetEipDrv::CAxNetEipDrv(UINT nID, CAxNetMgr* pNetMgr)
{
	m_nID	  = nID;
	m_pNetMgr = pNetMgr;
}

CAxNetEipDrv::~CAxNetEipDrv()
{
	DWORD dwExitCode = 0;
	BOOL  bDone = FALSE;

	m_bTerminate = TRUE;
	while( !bDone ) 
	{
		bDone = TRUE;
		if( !::GetExitCodeThread(m_hScanThread, &dwExitCode) )
			break;

		if( dwExitCode == STILL_ACTIVE ) 
		{
			bDone = FALSE;
			Sleep(100);
		}
	}
}

BOOL CAxNetEipDrv::Init(LPCTSTR pszFile)
{
	if( !CAxNetDriver::Init(pszFile) ) return FALSE;

//	CAxIni	profile;
//	profile.m_sIniFile = pszFile;
//	profile.m_sSect.Format(_T("Driver%d"), m_nID);
//
//	m_nChannel = profile.ReadInt(_T("Channel"));

	if( !SetVariable() ) return FALSE;
	if( !CheckVariable() ) return FALSE;

	if( !InitDriver() ) return FALSE;

	Startup(); // create thread, run OnTimer
	m_bInit = TRUE;
	return TRUE;
}

BOOL CAxNetEipDrv::SetVariable()
{
	return TRUE;
}

BOOL CAxNetEipDrv::CheckVariable()
{
	if( !CAxNetDriver::CheckVariable() ) return FALSE;

	return TRUE;
}

int CAxNetEipDrv::InitDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = EipInit();
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetEipDrv::ResetDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = EipReset();
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetEipDrv::CloseDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = EipClose();
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetEipDrv::CheckDriverError(int nVal, BOOL bReInit)
{
//	static BOOL bAbort = FALSE;
//	if( nRetVal != DRV_NO_ERROR ) 
//	{
//		if( !m_bInit || m_bTerminate ) return FALSE;
//
////		SetError(-sRetVal);
//		if( -sRetVal == HSCH_DEV_NO_COM_FLAG ) 
//		{
//			Shutdown();
//			InitScanner();
//		}
//		else 
//		{
//			Reset();
//		}
//
//		return FALSE;
//	}

	return TRUE;
}

void CAxNetEipDrv::OnTimer()
{
	if( CAxController::IsTerminate() || m_bTerminate ) 
	{
		CloseDriver();
		AfxEndThread(0);
		return;
	}

	ReadData();
	WriteData();
}

BOOL CAxNetEipDrv::ReadData()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;
	int nSize = m_parrNetData.GetSize();
	CAxNetData* pNetData = NULL;
	CByteArray arrWordData;

	for( int i=0; i<nSize; i++ )
	{
		pNetData = m_parrNetData[i];

		if( pNetData->GetMode() != CAxNetData::MD_READ )
			continue;

		int  nSize = pNetData->GetWordSize();
		arrWordData.SetSize(nSize);

		nRet = 
		ReadWordData(
			pNetData->GetName(), 
			pNetData->GetWordAddr(), 
			pNetData->GetWordSize(),
			arrWordData.GetData());

		if( !CheckDriverError(nRet, TRUE) ) return FALSE;
		memcpy(pNetData->GetWordData(), arrWordData.GetData(), pNetData->GetWordSize());

	}

	return TRUE;
}

BOOL CAxNetEipDrv::WriteData()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;
	int nSize = m_parrNetData.GetSize();
	CAxNetData* pNetData = NULL;

	for( int i=0; i<nSize; i++ )
	{
		pNetData = m_parrNetData[i];

		if( pNetData->GetMode() != CAxNetData::MD_WRITE )
			continue;

		nRet = 
		WriteWordData(
			pNetData->GetName(), 
			pNetData->GetWordAddr(), 
			pNetData->GetWordSize(),
			pNetData->GetWordData());

		if( !CheckDriverError(nRet, TRUE) ) return FALSE;
	}

	return TRUE;
}

// nSize : Byte 단위 
// nAddr : Byte 단위
BOOL CAxNetEipDrv::ReadBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;

	return TRUE;
}

// nSize : Byte 단위 
// nAddr : Byte 단위
BOOL CAxNetEipDrv::ReadWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;

	int nRet = 
	ReadTagData((LPSTR)pszName, nAddr/RW_BYTES, nSize/RW_BYTES, (LPSTR)pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}

// nSize : Byte 단위 
// nAddr : Byte 단위
BOOL CAxNetEipDrv::WriteBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;

	return TRUE;
}

// nSize : Byte 단위 
// nAddr : Byte 단위
BOOL CAxNetEipDrv::WriteWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;

	int nRet = 
	WriteTagData((LPSTR)pszName, nAddr/RW_BYTES, nSize/RW_BYTES, (LPSTR)pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}

