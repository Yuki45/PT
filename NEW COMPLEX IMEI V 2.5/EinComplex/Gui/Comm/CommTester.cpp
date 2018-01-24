#include "stdafx.h"
#include "CommTester.h"

#include <math.h>

CCommTester::CCommTester()
{
	for( int i = 0; i < DEF_MAX_TESTER_IN_PC; i++ )
	{
		m_bRecvReady[i] = FALSE;
		m_bRecvAck[i] = FALSE;
		m_bRecvPass[i] = FALSE;
		m_bRecvFail[i] = FALSE;
		m_bRecvLabel[i] = FALSE;

		m_sFailName[i] = _T("");
	}
}

CCommTester::~CCommTester()
{
}

void CCommTester::SendPacket(CmdCode emCode, int nIndex, CString sBarcode)
{
	BYTE byData[BUFFER_SIZE] = {0};
	CString sPacket = _T("");

	if( emCode == CC_START )
	{
		sPacket.Format(_T("<ST%02d[%s]4%X>\r\n"), nIndex, sBarcode, sBarcode.GetLength());
		sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
		sPacket.ReleaseBuffer();

		WriteData(byData, strlen((const char*)byData));
	}
	else if( emCode == CC_ACK )
	{
		sPacket.Format(_T("<OK%02d4>\r\n"), nIndex);
		sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
		sPacket.ReleaseBuffer();

		WriteData(byData, strlen((const char*)byData));
	}
	else if( emCode == CC_NAK )
	{
		sPacket.Format(_T("<FL%02d4>\r\n"), nIndex);
		sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
		sPacket.ReleaseBuffer();

		WriteData(byData, strlen((const char*)byData));
	}
}

BOOL CCommTester::RecvPacket()
{
	BYTE* pbyRecv = m_byRecv;
	BYTE byTemp[BUFFER_SIZE] = {0};
	TCHAR* pszNextToken = NULL;

	int nCurrentPoint = 0;
	int nIndex = 0;

	CString sPacket = _T("");
	CString sSplit = _T("");
	CString sCommand = _T("");
	CString sIndex = _T("");
	CString sFailName = _T("");

	memcpy(byTemp, pbyRecv, max(min(m_dwTotalReadLen, BUFFER_SIZE - 1), 0));
	byTemp[max(min(m_dwTotalReadLen, BUFFER_SIZE - 1), 0)] = '\0';
	sPacket.Format(_T("%s"), byTemp);

	for( int i = 0; i < sPacket.GetLength(); i++ )
	{
		if( sPacket.Mid(i, 1) == _T("\n") )
		{
			sSplit = sPacket.Mid(nCurrentPoint, i - nCurrentPoint + 1);
			nCurrentPoint = i + 1;

			if( sSplit.GetLength() < 9 ) continue;

			sCommand = sSplit.Mid(1, 2);
			sIndex = sSplit.Mid(3, 2);

			nIndex = _ttoi(sIndex);
			if( (nIndex < 1) || (nIndex > DEF_MAX_TESTER_IN_PC) ) continue;

			if( sCommand == _T("RD") ) m_bRecvReady[nIndex - 1] = TRUE;
			else if( sCommand == _T("OK") ) m_bRecvAck[nIndex - 1] = TRUE;
			else if( sCommand == _T("PS") ) m_bRecvPass[nIndex - 1] = TRUE;
			else if( sCommand == _T("FL") ) m_bRecvFail[nIndex - 1] = TRUE;
			else if( sCommand == _T("LP") )
			{
				pszNextToken = NULL;
				sFailName = _tcstok_s(sSplit.GetBuffer(sSplit.GetLength()), _T(","), &pszNextToken);
				sFailName = _tcstok_s(NULL, _T(","), &pszNextToken);
				sFailName = _tcstok_s(NULL, _T(","), &pszNextToken);
				sSplit.ReleaseBuffer();

				m_sFailName[nIndex - 1] = sFailName.Mid(1, sFailName.GetLength() - 1);
				m_bRecvLabel[nIndex - 1] = TRUE;
			}
		}
	}

	return TRUE;
}

void CCommTester::OnReceiveData()
{
	CSerial::OnReceiveData();
}
