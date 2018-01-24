#include "stdafx.h"
#include "AxNetMelsecDrv.h"

#include "Mdfunc.h"
#include "AxIni.h"
#include "AxController.h"
#include "AxConvertVariable.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxNetMelsecDrv::CAxNetMelsecDrv(UINT nID, CAxNetMgr* pNetMgr)
{
	m_nID	  = nID;
	m_pNetMgr = pNetMgr;

	m_nChannel = 0;
	m_hHandle  = 0;
}

CAxNetMelsecDrv::~CAxNetMelsecDrv()
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

BOOL CAxNetMelsecDrv::Init(LPCTSTR pszFile)
{
	if( !CAxNetDriver::Init(pszFile) ) return FALSE;

	CAxIni	profile;
	profile.m_sIniFile = pszFile;
	profile.m_sSect.Format(_T("Driver%d"), m_nID);

	m_nChannel = profile.ReadInt(_T("Channel"));

	if( !SetVariable() ) return FALSE;
	if( !CheckVariable() ) return FALSE;

	if( !InitDriver() ) return FALSE;

	Startup(); // create thread, run OnTimer
	m_bInit = TRUE;
	return TRUE;
}

BOOL CAxNetMelsecDrv::SetVariable()
{
	return TRUE;
}

BOOL CAxNetMelsecDrv::CheckVariable()
{
	if( !CAxNetDriver::CheckVariable() ) return FALSE;

	return TRUE;
}

int CAxNetMelsecDrv::InitDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = mdOpen(m_nChannel, -1, &m_hHandle);
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::ResetDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = mdBdRst(m_hHandle);
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::CloseDriver()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;

	nRet = mdClose(m_hHandle);
	if( !CheckDriverError(nRet, FALSE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::CheckDriverError(int nVal, BOOL bReInit)
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

void CAxNetMelsecDrv::OnTimer()
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

BOOL CAxNetMelsecDrv::ReadData()
{
	if( m_bSimulate ) return TRUE;

	int nRet = 0;
	int nSize = m_parrNetData.GetSize();
	CAxNetData* pNetData = NULL;
	CByteArray arrBitData, arrWordData;

	for( int i=0; i<nSize; i++ )
	{
		pNetData = m_parrNetData[i];

		if( pNetData->GetMode() != CAxNetData::MD_READ )
			continue;

		int  nSizebit = pNetData->GetBitSize();
		arrBitData.SetSize(nSizebit);

		nRet = 
		ReadBitData(
			pNetData->GetName(), 
			pNetData->GetBitAddr(), 
			pNetData->GetBitSize(),
			arrBitData.GetData());

		if( !CheckDriverError(nRet, TRUE) ) return FALSE;
		memcpy(pNetData->GetBitData(), arrBitData.GetData(), pNetData->GetBitSize());

		//////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////

		int nSizeWord = pNetData->GetWordSize();
		arrWordData.SetSize(nSizeWord);

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

BOOL CAxNetMelsecDrv::WriteData()
{
	if( m_bSimulate ) return TRUE;
	
	int nRet = 0;
	int nSize = m_parrNetData.GetSize();
	CAxNetData* pNetData = NULL;

	for( int i=0; i<nSize; i++ )
	{
		pNetData = m_parrNetData[i];

		if( pNetData->GetMode() != CAxNetData::MD_WRITE ) //(M) 2010.03.02 - Debug
			continue;

		nRet = 
		WriteBitData(
			pNetData->GetName(), 
			pNetData->GetBitAddr(), 
			pNetData->GetBitSize(),
			pNetData->GetBitData());

		if( !CheckDriverError(nRet, TRUE) ) return FALSE;
		
		//////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////

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

BOOL CAxNetMelsecDrv::ReadBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;
	
	int nRet = 
	mdReceive(
		m_hHandle, 
		STATION_NO,
		DevB,
		nAddr, //0
		(short*)&nSize, 
	//	&ReadData,
		pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::ReadWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;

	int nRet = 
	mdReceive(
		m_hHandle, 
		STATION_NO,
		DevW,
		nAddr, //0
		(short*)&nSize, 
	//	&ReadData,
		pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::WriteBitData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;
	
	int nRet = 
		mdSend(
		m_hHandle, 
		STATION_NO,
		DevB,
		nAddr, //0
		(short*)&nSize, 
	//	&ReadData,
		pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}

BOOL CAxNetMelsecDrv::WriteWordData(LPCSTR pszName, UINT nAddr, UINT nSize, void* pData)
{
	if( m_bSimulate ) return TRUE;
	
	int nRet = 
	mdSend(
		m_hHandle, 
		STATION_NO,
		DevW,
		nAddr, //0
		(short*)&nSize, 
	//	&ReadData,
		pData);

	if( !CheckDriverError(nRet, TRUE) ) return FALSE;

	return TRUE;
}