#ifndef __AX_NETDATA_H__
#define __AX_NETDATA_H__

#pragma once

class CAxNetMgr;
class __declspec(dllexport) CAxNetData
{
	friend class CAxNetMgr;

public:
	enum {
		MD_READ,
		MD_WRITE
	};

	CAxNetData(UINT nID);
	virtual ~CAxNetData();

	UINT	GetID() { return m_nID; }
	CString GetName() { return m_sName; }
	UINT	GetBitAddr() { return m_nBitAddr; }
	UINT	GetWordAddr() { return m_nWordAddr; }
	UINT	GetBitSize() { return m_nBitSize; }
	UINT	GetWordSize() { return m_nWordSize; }
	UINT	GetDriverID() { return m_nDriverID; }
	UINT	GetMode() { return m_nMode; }
	BYTE*	GetBitData();
	BYTE*	GetWordData();
	void	SetBitData(UINT nSize, BYTE* pbyData);
	void	SetWordData(UINT nSize, BYTE* pbyData);

	virtual BOOL Init(LPCTSTR pszFile, UINT nBits, UINT nWords);

protected:
	CString		m_sName;		// Tag Name
	UINT		m_nID;			// Data ID
	UINT		m_nMode;		// Read or Write
	UINT		m_nDriverID;	// Driver ID
	UINT		m_nLocalNo;		// 상대주소일때 Local 번호, 절대주소일때는 필요없음
	BOOL		m_bAssignAddr;	// 상대주소(자동) or 절대주소(수동)
	UINT		m_nBitOffset;	// 상대주소일때는 Offset, 절대주소일때는 Start 주소 (Byte 단위)
	UINT		m_nWordOffset;	// 상대주소일때는 Offset, 절대주소일때는 Start 주소 (Byte 단위)
	UINT		m_nBitSize;		// (Byte 단위)
	UINT		m_nWordSize;	// (Byte 단위)
	CByteArray	m_arrBitData;
	CByteArray	m_arrWordData;
	UINT		m_nBitAddr;		// Bit  시작 주소 (Byte 단위)
	UINT		m_nWordAddr;	// Word 시작 주소 (Byte 단위)
};

typedef CArray<CAxNetData, CAxNetData&>						CAxNetDataArray;
typedef CTypedPtrArray<CPtrArray, CAxNetData*>				CAxNetDataPtrArray;
typedef CTypedPtrMap<CMapStringToPtr, CString, CAxNetData*>	CMapStringToNetData;

#endif
