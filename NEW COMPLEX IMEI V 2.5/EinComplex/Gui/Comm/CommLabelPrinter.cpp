#include "stdafx.h"
#include "CommLabelPrinter.h"

#include <math.h>

CCommLabelPrinter::CCommLabelPrinter()
{
	m_nJigNo = 0;
	m_sBarcode = _T("");
	m_sFailName = _T("");
}

CCommLabelPrinter::~CCommLabelPrinter()
{
}

void CCommLabelPrinter::SendPacket(CmdCode emCode)
{
	BYTE byData[BUFFER_SIZE] = {0};

	CString sPacket = _T("");
	CString sFail = _T("");
	CString sFailData = _T("");
	CString sTemp = _T("");

	CTime tmCurrent = CTime::GetCurrentTime();
	CString sTime = tmCurrent.Format("%Y/%m/%d <%H:%M>");

	if( m_sFailName.Find(_T("\"")) != -1 )
	{
		m_sFailName = m_sFailName.Mid(1, m_sFailName.GetLength() - 2);
	}

	int nLength = m_sFailName.GetLength();
	int nCount = nLength / 30;

	if( nCount == 0 )
	{
		sTemp.Format(_T("PP 50,%d\r\n")
					 _T("FT \"Swiss 721 BT\",8,0,65\r\n")
					 _T("PT \"%s\"\r\n"),
					 100,
					 m_sFailName);

		sFail += sTemp;
	}
	else
	{
		for( int i = 0; i <= nCount; i++ )
		{
			nLength = m_sFailName.GetLength();

			if( nLength < 30 )
			{
				sFailData = m_sFailName.Left(nLength);
			}
			else
			{
				sFailData = m_sFailName.Left(30);
			}

			m_sFailName = m_sFailName.Right(nLength - 30);
			sTemp.Format(_T("PP 50,%d\r\n")
						 _T("FT \"Swiss 721 BT\",8,0,65\r\n")
						 _T("PT \"%s\"\r\n"),
						 100 - (i * 25),
						 sFailData);

			sFail += sTemp;
		}
	}
	
	sPacket.Format(_T("CLL\r\n")
				   _T("NEW\r\n")
				   _T("SETUP \"SER-COM,UART1,FLOWCONTROL,XON/XOFF,DATA FROM HOST,ENABLE\"\r\n")
				   _T("SETUP \"SER-COM,UART1,FLOWCONTROL,XON/XOFF,DATA TO HOST,ENABLE\"\r\n")
				   _T("SETUP \"MEDIA,PAPER TYPE,TRANSFER,RIBBON CONSTANT,100\"\r\n")
				   _T("SETUP \"FEEDADJ,STARTADJ,-115\"\r\n")
				   _T("SETUP \"FEEDADJ,STOPADJ,10\"\r\n")
				   _T("SETUP \"MEDIA,CONTRAST,+10%%\"\r\n")
				   _T("PP 50,160 \r\n")
				   _T("FT \"Swiss 721 BT\",8,0,65 \r\n")
				   _T("PT \"Process : IMEI LCA \'%s\'\"\r\n")
				   _T("PP 50,125\r\n")
				   _T("FT \"Swiss 721 BT\",8,0,65\r\n")
				   _T("PT \"Failure Jig No : %d\"\r\n")
				   _T("%s\r\n")
				   _T("PP 50,45\r\n")
				   _T("FT \"Swiss 721 BT\",8,0,65\r\n")
				   _T("PT \"IMEI NO :%s\"\r\n")
				   _T("PP 50,10\r\n")
				   _T("FT \"Swiss 721 BT\",8,0,65\r\n")
				   _T("PT \"Line:Auto Block Cell #1\"\r\n")
				   _T("PF1\r\n"),
				   sTime,
				   m_nJigNo,
				   sFail,
				   m_sBarcode);

	sprintf_s((char*)byData, sPacket.GetLength() + 1, _T("%s"), (LPCTSTR)sPacket);
	sPacket.ReleaseBuffer();

	WriteData(byData, strlen((const char*)byData));
}

BOOL CCommLabelPrinter::RecvPacket()
{
	BYTE* pbyRecv = m_byRecv;
	BYTE byTemp[BUFFER_SIZE] = {0};

	CString sPacket = _T("");

	memcpy(byTemp, pbyRecv, max(min(m_dwTotalReadLen, BUFFER_SIZE - 1), 0));
	byTemp[max(min(m_dwTotalReadLen, BUFFER_SIZE - 1), 0)] = '\0';
	sPacket.Format(_T("%s"), byTemp);

	return TRUE;
}

void CCommLabelPrinter::OnReceiveData()
{
	CSerial::OnReceiveData();
}
