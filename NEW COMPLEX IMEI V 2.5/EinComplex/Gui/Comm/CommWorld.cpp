#include "stdafx.h"
#include "CommWorld.h"

CCommWorld::CCommWorld()
{
	for( int i = 0; i < DEF_MAX_IO_IN_WORLD; i++ )
	{
		m_bInput[i] = FALSE;
		m_bOutput[i] = FALSE;
	}
}

CCommWorld::~CCommWorld()
{
}

void CCommWorld::SendPacket(CmdCode emCode)
{
	BYTE byData[BUFFER_SIZE] = {0};
	int nFirst = 0;
	int nSecond = 0;
	int nCRC = 0;
	CString sCRC = _T("");
	CString sPacket = _T("");

	if( emCode == CC_GET_INPUT )
	{
		sPacket.Format(_T("%cF1%c"), ASCII_STX, ASCII_ETX);
		sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
	}
	else if( emCode == CC_SET_OUTPUT )
	{
		nFirst += m_bOutput[8];
		nFirst += (m_bOutput[9] * 2);
		nFirst += (m_bOutput[10] * 4);
		nFirst += (m_bOutput[11] * 8);
		nFirst += (m_bOutput[12] * 16);
		nFirst += (m_bOutput[13] * 32);
		nFirst += (m_bOutput[14] * 64);
		nFirst += (m_bOutput[15] * 128);

		nSecond += m_bOutput[0];
		nSecond += (m_bOutput[1] * 2);
		nSecond += (m_bOutput[2] * 4);
		nSecond += (m_bOutput[3] * 8);
		nSecond += (m_bOutput[4] * 16);
		nSecond += (m_bOutput[5] * 32);
		nSecond += (m_bOutput[6] * 64);
		nSecond += (m_bOutput[7] * 128);

		nCRC = nFirst + nSecond;
		sCRC.Format(_T("%02X"), nCRC);

		sPacket.Format(_T("%cF0%02X%02X%2s%c"), ASCII_STX, nFirst, nSecond, (LPCTSTR)sCRC.Right(2), ASCII_ETX);
		sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
	}

	WriteData(byData, strlen((const char*)byData));
}

void CCommWorld::OnReceiveData()
{
	CSerial::OnReceiveData();

 	BYTE* pbyRecv = m_byRecv;
 	CString sPacket = _T("");
 	CString sFirst = _T("");
 	CString sSecond = _T("");
 	CString sCRC= _T("");
 
 	if( m_byRecv[max((m_dwTotalReadLen - 1), 0)] == ASCII_ETX )
 	{
 		BYTE byTemp[100] = {0};
 		memcpy(byTemp, pbyRecv, m_dwTotalReadLen);
 		byTemp[min(m_dwTotalReadLen, 99)] = '\0';
 		sPacket.Format(_T("%s"), byTemp);
 
 		sFirst = sPacket.Mid(1, 2);
 		sSecond = sPacket.Mid(3, 2);
 		sCRC = sPacket.Mid(5, 2);

		ParseInput(sSecond.Right(1), 0);
		ParseInput(sSecond.Left(1), 4);
		ParseInput(sFirst.Right(1), 8);
		ParseInput(sFirst.Left(1), 12);
	}
}

void CCommWorld::ParseInput(CString sValue, int nStartIndex)
{
	int nVal = 0;

	if( sValue == _T("0") ) nVal = 0;
	else if( sValue == _T("1") ) nVal = 1;
	else if( sValue == _T("2") ) nVal = 2;
	else if( sValue == _T("3") ) nVal = 3;
	else if( sValue == _T("4") ) nVal = 4;
	else if( sValue == _T("5") ) nVal = 5;
	else if( sValue == _T("6") ) nVal = 6;
	else if( sValue == _T("7") ) nVal = 7;
	else if( sValue == _T("8") ) nVal = 8;
	else if( sValue == _T("9") ) nVal = 9;
	else if( sValue == _T("A") ) nVal = 10;
	else if( sValue == _T("B") ) nVal = 11;
	else if( sValue == _T("C") ) nVal = 12;
	else if( sValue == _T("D") ) nVal = 13;
	else if( sValue == _T("E") ) nVal = 14;
	else if( sValue == _T("F") ) nVal = 15;
	else return;

	if( nVal >= 8 )
	{
		m_bInput[nStartIndex + 3] = TRUE;
		nVal = nVal - 8;
	}
	else m_bInput[nStartIndex + 3] = FALSE;

	if( nVal >= 4 )
	{
		m_bInput[nStartIndex + 2] = TRUE;
		nVal = nVal - 4;
	}
	else m_bInput[nStartIndex + 2] = FALSE;

	if( nVal >= 2 )
	{
		m_bInput[nStartIndex + 1] = TRUE;
		nVal = nVal - 2;
	}
	else m_bInput[nStartIndex + 1] = FALSE;

	if( nVal >= 1 )
	{
		m_bInput[nStartIndex] = TRUE;
		nVal = nVal - 1;
	}
	else m_bInput[nStartIndex] = FALSE;
}
