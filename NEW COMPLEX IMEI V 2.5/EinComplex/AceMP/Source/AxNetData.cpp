#include "stdafx.h"
#include "AxNetData.h"

#include "AxNetMgr.h"
#include "AxIni.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxNetData::CAxNetData(UINT nID)
{
	m_nID = nID;
}

CAxNetData::~CAxNetData()
{

}

// nBits  = 로컬당 Bit 수 (Byte 단위)
// nWords = 로컬당 Word 수 (Byte 단위)
BOOL CAxNetData::Init(LPCTSTR pszFile, UINT nBits, UINT nWords)
{
	CAxIni	profile;
	profile.m_sIniFile = pszFile;
	profile.m_sSect.Format(_T("Data%d"), m_nID);

	m_sName		  = profile.ReadStr(_T("Name"));
	m_nMode		  = profile.ReadUint(_T("Mode"));
	m_nDriverID   = profile.ReadUint(_T("DriverID"));
	m_nLocalNo	  = profile.ReadUint(_T("LocalNo"));
	m_bAssignAddr = profile.ReadBool(_T("AssignAddr"));
	m_nBitOffset  = profile.ReadUint(_T("BitOffset"));
	m_nWordOffset = profile.ReadUint(_T("WordOffset"));
	m_nBitSize	  = profile.ReadUint(_T("BitSize"));
	m_nWordSize	  = profile.ReadUint(_T("WordSize"));

	// Auto(상대주소) : Local 번호로 시작번지 할당
	if( !m_bAssignAddr )
	{
		m_nBitAddr  = m_nLocalNo * nBits * 8 ;
		m_nWordAddr = m_nLocalNo * nWords;
	}
	// Manual(절대주소) : Offset 값이 시작번지 
	else
	{
		m_nBitAddr  = m_nBitOffset;
		m_nWordAddr = m_nWordOffset;

		m_nBitOffset  = 0;
		m_nWordOffset = 0;
	}

	m_arrBitData.SetSize(m_nBitSize);
	m_arrWordData.SetSize(m_nWordSize);

	return TRUE;
}

// return = Byte 단위
BYTE* CAxNetData::GetBitData()
{
	return m_arrBitData.GetData();
}

// return = Word 단위
BYTE* CAxNetData::GetWordData()
{
	return m_arrWordData.GetData();
}

// nSize = Byte 단위
void CAxNetData::SetBitData(UINT nSize, BYTE* pbyData)
{
	ASSERT( nSize == (UINT)m_arrBitData.GetSize() );

//	for( UINT i=0; i<nSize; i++ )
//		m_arrBitData[i] = pbyData[i];
	memcpy(m_arrBitData.GetData(), pbyData, sizeof(BYTE)*nSize);
}

// nSize = Word 단위
void CAxNetData::SetWordData(UINT nSize, BYTE* pbyData)
{
	ASSERT( nSize == (UINT)m_arrWordData.GetSize() );

//	for( UINT i=0; i<nSize; i++ )
//		m_arrWordData[i] = pwData[i];
	memcpy(m_arrWordData.GetData(), pbyData, sizeof(BYTE)*nSize);
}

