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
	UINT		m_nLocalNo;		// ����ּ��϶� Local ��ȣ, �����ּ��϶��� �ʿ����
	BOOL		m_bAssignAddr;	// ����ּ�(�ڵ�) or �����ּ�(����)
	UINT		m_nBitOffset;	// ����ּ��϶��� Offset, �����ּ��϶��� Start �ּ� (Byte ����)
	UINT		m_nWordOffset;	// ����ּ��϶��� Offset, �����ּ��϶��� Start �ּ� (Byte ����)
	UINT		m_nBitSize;		// (Byte ����)
	UINT		m_nWordSize;	// (Byte ����)
	CByteArray	m_arrBitData;
	CByteArray	m_arrWordData;
	UINT		m_nBitAddr;		// Bit  ���� �ּ� (Byte ����)
	UINT		m_nWordAddr;	// Word ���� �ּ� (Byte ����)
};

typedef CArray<CAxNetData, CAxNetData&>						CAxNetDataArray;
typedef CTypedPtrArray<CPtrArray, CAxNetData*>				CAxNetDataPtrArray;
typedef CTypedPtrMap<CMapStringToPtr, CString, CAxNetData*>	CMapStringToNetData;

#endif
