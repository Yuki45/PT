#include "stdafx.h"
#include "CommBCR.h"

#include <math.h>

CCommBCR::CCommBCR()
{
	m_bRecvComp = FALSE;
	m_sRecv = _T("");
}

CCommBCR::~CCommBCR()
{
}

void CCommBCR::SendPacket(CmdCode emCode)
{
	return;

	BYTE byData[BUFFER_SIZE] = {0};
	int nFirst = 0;
	int nSecond = 0;
	int nCRC = 0;
	CString sCRC = _T("");

	if( emCode == CC_LIGHT_ON ) sprintf_s((char*)byData, 2, "L");
	else if( emCode == CC_LIGHT_OFF ) sprintf_s((char*)byData, 2, "F");
	else return;

	byData[3] = byData[1];
	byData[2] = 0x03;
	byData[1] = byData[0];
	byData[0] = 0x02;

	WriteData(byData, strlen((const char*)byData));
}

BOOL CCommBCR::RecvPacket()
{
	BYTE* pbyRecv = m_byRecv;
	CString sPacket = _T("");

	if( m_byRecv[max((m_dwTotalReadLen - 1), 0)] == ASCII_LF )
	{
		BYTE byTemp[100] = {0};
		memcpy(byTemp, pbyRecv, m_dwTotalReadLen);
		byTemp[min(m_dwTotalReadLen, 99)] = '\0';
		sPacket.Format(_T("%s"), byTemp);

		m_sRecv = sPacket.Mid(0, sPacket.GetLength() - 2);
		m_bRecvComp = TRUE;

		return TRUE;
	}

	return FALSE;
}

void CCommBCR::OnReceiveData()
{
	CSerial::OnReceiveData();
}
