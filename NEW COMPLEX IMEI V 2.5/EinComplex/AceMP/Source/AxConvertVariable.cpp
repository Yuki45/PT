#include "stdafx.h"
#include "AxConvertVariable.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//-----------------------------------------------------------------------------
// Convert BYTE
//-----------------------------------------------------------------------------

CString GetStrFromByte(int nLen, BYTE* pbyData)
{
	ASSERT( pbyData != NULL );
	
	CString sData((LPCSTR)pbyData, nLen);
	sData.TrimRight();

	return sData;
}

void SetStrToByte(BYTE* pbyDst, CString sSrc, int nLen)
{
	ASSERT( pbyDst != NULL );
	ASSERT( nLen >= lstrlen(sSrc) );
	
	memset(pbyDst, 0x20, nLen); // space 로 초기화 
	nLen = min(nLen, lstrlen(sSrc));
#ifdef _UNICODE
	WideCharToMultiByte(CP_ACP, 0, sSrc, -1, (LPSTR)pbyDst, nLen, NULL, NULL);
#else
	memcpy(pbyDst, sSrc, nLen);
#endif
}

short GetShortFromByte(int nLen, BYTE* pbyData)
{
	ASSERT( nLen == sizeof(short) );
	ASSERT( pbyData != NULL );

	short nData = 0;
	memcpy(&nData, pbyData, nLen);

	return nData;
}

void SetShortToByte(BYTE* pbyDst, short nData, int nLen)
{
	ASSERT( nLen == sizeof(short) );
	ASSERT( pbyDst != NULL );

	memset(pbyDst, 0x00, nLen);
	memcpy(pbyDst, &nData, nLen);
}

int GetIntFromByte(int nLen, BYTE* pbyData)
{
	ASSERT( nLen == sizeof(int) );
	ASSERT( pbyData != NULL );

	int nData = 0;
	memcpy(&nData, pbyData, nLen);

	return nData;
}

void SetIntToByte(BYTE* pbyDst, int nData, int nLen)
{
	ASSERT( nLen == sizeof(int) );
	ASSERT( pbyDst != NULL );

	memset(pbyDst, 0x00, nLen);
	memcpy(pbyDst, &nData, nLen);
}