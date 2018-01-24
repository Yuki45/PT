#include "stdafx.h"
#include "Serial.h"

const BYTE CRC8_Table[256] = {
	0x00, 0x5e, 0xbc, 0xe2, 0x61, 0x3f, 0xdd, 0x83,
	0xc2, 0x9c, 0x7e, 0x20, 0xa3, 0xfd, 0x1f, 0x41,
	0x9d, 0xc3, 0x21, 0x7f, 0xfc, 0xa2, 0x40, 0x1e,
	0x5f, 0x01, 0xe3, 0xbd, 0x3e, 0x60, 0x82, 0xdc,
	0x23, 0x7d, 0x9f, 0xc1, 0x42, 0x1c, 0xfe, 0xa0,
	0xe1, 0xbf, 0x5d, 0x03, 0x80, 0xde, 0x3c, 0x62,
	0xbe, 0xe0, 0x02, 0x5c, 0xdf, 0x81, 0x63, 0x3d,
	0x7c, 0x22, 0xc0, 0x9e, 0x1d, 0x43, 0xa1, 0xff,
	0x46, 0x18, 0xfa, 0xa4, 0x27, 0x79, 0x9b, 0xc5,
	0x84, 0xda, 0x38, 0x66, 0xe5, 0xbb, 0x59, 0x07,
	0xdb, 0x85, 0x67, 0x39, 0xba, 0xe4, 0x06, 0x58,
	0x19, 0x47, 0xa5, 0xfb, 0x78, 0x26, 0xc4, 0x9a,
	0x65, 0x3b, 0xd9, 0x87, 0x04, 0x5a, 0xb8, 0xe6,
	0xa7, 0xf9, 0x1b, 0x45, 0xc6, 0x98, 0x7a, 0x24,
	0xf8, 0xa6, 0x44, 0x1a, 0x99, 0xc7, 0x25, 0x7b,
	0x3a, 0x64, 0x86, 0xd8, 0x5b, 0x05, 0xe7, 0xb9,
	0x8c, 0xd2, 0x30, 0x6e, 0xed, 0xb3, 0x51, 0x0f,
	0x4e, 0x10, 0xf2, 0xac, 0x2f, 0x71, 0x93, 0xcd,
	0x11, 0x4f, 0xad, 0xf3, 0x70, 0x2e, 0xcc, 0x92,
	0xd3, 0x8d, 0x6f, 0x31, 0xb2, 0xec, 0x0e, 0x50,
	0xaf, 0xf1, 0x13, 0x4d, 0xce, 0x90, 0x72, 0x2c,
	0x6d, 0x33, 0xd1, 0x8f, 0x0c, 0x52, 0xb0, 0xee,
	0x32, 0x6c, 0x8e, 0xd0, 0x53, 0x0d, 0xef, 0xb1,
	0xf0, 0xae, 0x4c, 0x12, 0x91, 0xcf, 0x2d, 0x73,
	0xca, 0x94, 0x76, 0x28, 0xab, 0xf5, 0x17, 0x49,
	0x08, 0x56, 0xb4, 0xea, 0x69, 0x37, 0xd5, 0x8b,
	0x57, 0x09, 0xeb, 0xb5, 0x36, 0x68, 0x8a, 0xd4,
	0x95, 0xcb, 0x29, 0x77, 0xf4, 0xaa, 0x48, 0x16,
	0xe9, 0xb7, 0x55, 0x0b, 0x88, 0xd6, 0x34, 0x6a,
	0x2b, 0x75, 0x97, 0xc9, 0x4a, 0x14, 0xf6, 0xa8,
	0x74, 0x2a, 0xc8, 0x96, 0x15, 0x4b, 0xa9, 0xf7,
	0xb6, 0xe8, 0x0a, 0x54, 0xd7, 0x89, 0x6b, 0x35
};

BYTE CRC8_Packet(BYTE* packet, int size)
{
	BYTE CRC = 0x00;

	if( !packet || size <= 0 ) return CRC;

	while( size-- )
	{
		CRC = CRC8_Table[CRC ^ *packet++];
	}

	return CRC;
}

BYTE BCC_Packet(BYTE* packet, int size)
{
	BYTE BCC = 0x00;

	if( !packet || size <= 0 ) return BCC;

	while( size-- )
	{
		BCC ^= *packet++;
	}

	return BCC;
}

CSerial::CSerial()
{
	m_sName			 = _T("Serial");
	m_pWnd			 = NULL;
	m_nMsg			 = UM_SERIAL_MSG;
	m_nMsgSend		 = UM_SERIAL_MSG_SEND;
	m_nMsgRecv		 = UM_SERIAL_MSG_RECV;
	m_nErrorCode	 = -1;
	m_nType			 = 0;
	m_nIndex		 = 0;
	m_nAddr			 = 0;
	
	m_nPortNo		 = 1;
	m_dwBaudRate	 = 9600;
	m_byDataBits	 = 8;
	m_byParity		 = NOPARITY;
	m_byStopBits	 = ONESTOPBIT;
	m_bXonXoff		 = FALSE;

	m_hCommPort		 = NULL;
	m_hCommThread	 = NULL;
	m_dwCommThreadID = 0;
	m_osRead.hEvent  = NULL;
	m_osWrite.hEvent = NULL;
	m_dwTotalReadLen = 0;
	memset(m_byRecv, 0, sizeof(m_byRecv));

	m_bDeleteThread  = TRUE;
	m_bPortOpened	 = FALSE;
}

CSerial::~CSerial()
{
	ClosePort();
}

void CSerial::SetPort(
		CString sName/* = _T("Serial")*/,
		CWnd* pWnd/* = NULL*/,
		UINT  nPortNo/* = 1*/, 
		DWORD dwBaudRate/* = 9600*/, 
		BYTE  byDataBits/* = 8*/, 
		BYTE  byParity/* = NOPARITY*/, 
		BYTE  byStopBits/* = ONESTOPBIT*/, 
		BOOL  bXonXoff/* = FALSE*/)
{
	//-------------------------------------------------------------------------
	// Set Data
	//-------------------------------------------------------------------------
	m_sName		 = sName;
	m_pWnd		 = pWnd;
//	m_nMsg		 = UM_SERIAL_MSG;
//	m_nMsgRecv	 = UM_SERIAL_MSG_RECV;
	m_nPortNo	 = nPortNo;
	m_dwBaudRate = dwBaudRate;
	m_byDataBits = byDataBits;
	m_byParity	 = byParity;
	m_byStopBits = byStopBits;
	m_bXonXoff	 = bXonXoff;
}
					  
BOOL CSerial::OpenPort()
{
	return 
	OpenPort(
		m_sName,
		m_pWnd,
//		m_nMsg,
//		m_nMsgRecv,
		m_nPortNo,
		m_dwBaudRate,
		m_byDataBits,
		m_byParity,
		m_byStopBits,
		m_bXonXoff);
}

BOOL CSerial::OpenPort(
		CString sName/* = _T("Serial")*/,
		CWnd* pWnd/* = NULL*/,
		UINT  nPortNo/* = 1*/, 
		DWORD dwBaudRate/* = 9600*/, 
		BYTE  byDataBits/* = 8*/, 
		BYTE  byParity/* = NOPARITY*/, 
		BYTE  byStopBits/* = ONESTOPBIT*/, 
		BOOL  bXonXoff/* = FALSE*/)
{
	//-------------------------------------------------------------------------
	// Set Data
	//-------------------------------------------------------------------------
	m_sName		 = sName;
	m_pWnd		 = pWnd;
//	m_nMsg		 = UM_SERIAL_MSG;
//	m_nMsgRecv	 = UM_SERIAL_MSG_RECV;
	m_nPortNo	 = nPortNo;
	m_dwBaudRate = dwBaudRate;
	m_byDataBits = byDataBits;
	m_byParity	 = byParity;
	m_byStopBits = byStopBits;
	m_bXonXoff	 = bXonXoff;
					  
	//-------------------------------------------------------------------------
	// Check Already Active
	//-------------------------------------------------------------------------
	if( IsActive() == TRUE ) { return TRUE; }

	ClosePort();

	//-------------------------------------------------------------------------
	// Create Overlapped Events
	//-------------------------------------------------------------------------
	m_osRead.Offset=0;
	m_osRead.OffsetHigh=0;
	m_osRead.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
	if( m_osRead.hEvent == NULL )
	{
		ClosePort();
		return FALSE;
	}

	m_osWrite.Offset = 0;
	m_osWrite.OffsetHigh=0;
	m_osWrite.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);

	if( NULL == m_osWrite.hEvent )
	{
		ClosePort();
		return FALSE;
	}
	
	//-------------------------------------------------------------------------
	// Open Port
	//-------------------------------------------------------------------------
	CString strPortNo = _T("");
	
	if( m_nPortNo < 10 )
		{ strPortNo.Format(_T("COM%d"), m_nPortNo);	}
	else 
		{ strPortNo.Format(_T("\\\\.\\COM%d"), m_nPortNo); }

	m_nPortNo = m_nPortNo;

	m_hCommPort = 
	CreateFile( 
		(LPCTSTR) strPortNo, 
		GENERIC_READ | GENERIC_WRITE, 
		0, 
		NULL,
		OPEN_EXISTING, 
		FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED, 
		NULL );

	if( m_hCommPort == INVALID_HANDLE_VALUE )
	{
		ClosePort();
		return FALSE;
	}
	
	//-------------------------------------------------------------------------
	// Set Parameter
	//-------------------------------------------------------------------------
	if( !SetCommMask(m_hCommPort, EV_RXCHAR) )
	{
		ClosePort();
		return FALSE;
	}
	
	if( !SetupComm(m_hCommPort, QUEUE_SIZE, QUEUE_SIZE) )
	{
		ClosePort();
		return FALSE;
	}

	if( !PurgeComm(m_hCommPort, PURGE_TXABORT|PURGE_RXABORT|PURGE_TXCLEAR|PURGE_RXCLEAR) )
	{
		ClosePort();
		return FALSE;
	}
	
	COMMTIMEOUTS CommTimeOuts;
	if( !GetCommTimeouts(m_hCommPort, &CommTimeOuts) )
	{
		ClosePort();
		return FALSE;
	}

	CommTimeOuts.ReadIntervalTimeout			= 40;	// 0xFFFFFFFF;	// 0xFFFFFFFF		// OxFFFFFFFF
	CommTimeOuts.ReadTotalTimeoutMultiplier		= 0;	// 0;			// 0				// 10
	CommTimeOuts.ReadTotalTimeoutConstant		= 1000;	// 1000;		// 1000				// 1000
	CommTimeOuts.WriteTotalTimeoutMultiplier	= 0;	// 0;			// 2*CBR_9600/baud	// 10
	CommTimeOuts.WriteTotalTimeoutConstant		= 1000;	// 1000;		// 0				// 1000

	if( !SetCommTimeouts(m_hCommPort, &CommTimeOuts) )
	{
		ClosePort();
		return FALSE;
	}
	
	//-------------------------------------------------------------------------
	// DCB ���� 
	//-------------------------------------------------------------------------
	// DCBLength 
	//		: SetCommState() �Լ� ���� ȣ��Ǿ�� ��
	// Parity
	//		: NOPARITY(0), ODDPARITY(1), EVENPARITY(2), MARKPARITY(3), SPACEPARITY(4)
	// StopBits
	//		: ONESTOPBIT(0), ONE5STOPBITS(1), TWOSTOPBITS(2)
	// fOutX
	//		: �۽��߿� XON/XOFF �帧��� �� �������� �����Ѵ�.
	//		: TRUE �̸� �۽��߿� XOFF ���ڰ� ���ŵǾ��� �� �۽��� �ߴ��ϰ� 
	//		: XON ���ڰ� ���ŵǸ� �۽��� �簳�Ѵ�.
	// fInX 
	//		: �����߿� XON/XOFF �帧��� �� �������� �����Ѵ�.
	//		: TRUE �̸� �����߿� XOffLim ����Ʈ�� ���۰� ���� á�� �� XOFF ���ڸ� �۽��ϰ�
	//		: XOnLim ����Ʈ ���Ϸ� ����� �� XON ���ڸ� �۽��Ѵ�. 
	// fNull 
	//		: NULL ����Ʈ�� ������ �������� �����Ѵ�.
	//		: TRUE �̸� NULL �� ���ŵǾ��� �� �����Ѵ�.
	// fBinary
	//		: ������� ���� ����
	//		: Win32 ������ ������常 �����ϹǷ� �ݵ�� TRUE �̾�� �Ѵ�. 
	// fErrorChar  
	// 		: fParity �� TRUE �̰� fErrorChar �� TRUE �϶� 
	//		: ���� ���߿� �и�Ƽ ������ �߻��ϸ� ErrorChar ���ڷ� ��ġ�� 
	// ErrorChar
	//		: �и�Ƽ ������ �߻��� �� ��ġ�Ͽ� ����� ����
	// EofChar 
	//		: �������� �� ��ȣ�� ����� ����
	// EvtChar
	//		: EV_RXFLAG �̺�Ʈ�� �߻����� �˸��� ����
	// fOutxCtsFlow
	//		: �۽� �帧��� ���Ͽ� CTS ��ȣ�� �ֽ��� ���� ����
	//		: TRUE �̰� CTS �� �����̸� ������ �� ������ �۽��� ������
	// fOutxDsrFlow
	//		: �۽� �帧��� ���Ͽ� DSR ��ȣ�� �ֽ��� ���� ����
	//		: TRUE �̰� DSR �� �����̸� ������ �� ������ �۽��� ������ 
	// fDtrControl
	//		: ���� �帧 ��� ���Ͽ� DTR �� ����Ѵ�.
	//		: DTR_CONTROL_DISABLE   - ��ġ�� ������ �� DTR ������ ������ �Ѵ�.
	//		: DTR_CONTROL_ENABLE    - ��ġ�� ������ �� DTR ������ ������ �Ѵ�.
	//		: DTR_CONTROL_HANDSHAKE - DTR �帧���� ��ȣ ���� �����ϵ��� �Ѵ�.  
	// fRtsControl
	//		: �Է� �帧 ����
	//		: 0 �̸� �⺻ ���� RTS_CONTROL_HANDSHAKE �� �ȴ�
	//		: RTS_CONTROL_DISABLE   - ��ġ�� ������ RTS ������ ������ �ȴ�.
	//		: RTS_CONTROL_ENABLE    - ��ġ�� ������ RTS ������ ������ �ȴ�.
	//		: RTS_CONTROL_HANDSHAKE - RTS �帧��� �ڵ����� �ϰ� �Ѵ�.
	//								- ������ �Է� ���۰� �����͸� �޾Ƶ��̱ⵥ ����� ������ ������ 
	//								  ���� �� RTS �� ������ �ϰ� DCE�� Enabling �Ѵ�.
	//								- �Է� ���۰� ����� ������ ������ ���� ���� �� RTS ������ ���Ƿ� �ϰ�
	//								  �۽��� ���� DCE�� PRevening �Ѵ�.
	//		: RTS_CONTROL_TOGGLE    - �۽��� ���� ������ RTS ������ ������ �ǰ� 
	//								- �۽��� ��� ������ RTS ������ ������ �ȴ�.
	// fDsrSensitivity 
	//		: ��� ����Ⱑ DSR ��ȣ�� �ΰ��ϰ� �Ѵ�
	//		: TRUE �̸� ������ ���ŵ� ��� ����Ʈ�� �����ϰ� 
	//		: �׷��� �ʴٸ� DSR �� �Է� ������ ������ �ȴ�.
	// fTXContinueOnXoff
	//		: ���� ���۰� ���� á�� �� �۽��� �ߴ��ϰ� ������ XOFF ���ڸ� �۽��� ���� ����
	//		: TRUE �̸� XOFF ���ڰ� �۽ŵǰ� �۽��� ��ӵȴ�.
	//		: FALSE �̸� ���� ���۰� XonLim ����Ʈ ���ϰ� �� ������ �۽��� ���߰� 
	//		: XonLim ����Ʈ ���ϰ� �Ǹ� ������ XON ���ڸ� �����Ѵ�. 
	
	DCB dcb;
	if( !GetCommState(m_hCommPort, &dcb) )
	{
		ClosePort();
		return FALSE;
	}
	
	dcb.DCBlength		  = sizeof(dcb);
	dcb.BaudRate		  = m_dwBaudRate;
	dcb.ByteSize		  = m_byDataBits;
	dcb.Parity			  = m_byParity;	
	dcb.StopBits		  = m_byStopBits;
	dcb.fOutxCtsFlow	  = FALSE;
	dcb.fRtsControl		  = DTR_CONTROL_ENABLE;
	dcb.fOutxDsrFlow	  = FALSE;
	dcb.fDtrControl		  = DTR_CONTROL_ENABLE;
	dcb.fNull			  = FALSE;
	dcb.fBinary			  = TRUE;
	if( m_byParity != NOPARITY )
		{ dcb.fParity	  = TRUE; }
	else 
		{ dcb.fParity	  = FALSE; }
	if( m_bXonXoff == TRUE )
	{
		dcb.fOutX		  = TRUE;
		dcb.fInX		  = TRUE;
		dcb.XonLim		  = 100;
		dcb.XoffLim		  = 100;
		dcb.XonChar		  = ASCII_XON;
		dcb.XoffChar	  = ASCII_XOFF;
	}
	else
	{
		dcb.fOutX		  = FALSE;
		dcb.fInX		  = FALSE;
	}

	if( !SetCommState(m_hCommPort, &dcb) )
	{
		ClosePort();
		return FALSE;
	}

	m_bPortOpened = TRUE;
	
	//-------------------------------------------------------------------------
	// Create Comm Thread
	//-------------------------------------------------------------------------
	if( !CreateComm() )
	{
		ClosePort();
		return FALSE;
	}
	
	return TRUE;
}

BOOL CSerial::ClosePort()
{
	m_bPortOpened = FALSE;
	m_bDeleteThread = TRUE;

	//-------------------------------------------------------------------------
	// Close Port
	//-------------------------------------------------------------------------
	if( m_hCommPort )
	{
		SetCommMask(m_hCommPort, 0);
		EscapeCommFunction(m_hCommPort, CLRDTR);
		EscapeCommFunction(m_hCommPort, CLRRTS);
		PurgeComm(m_hCommPort, PURGE_TXABORT|PURGE_RXABORT|PURGE_TXCLEAR|PURGE_RXCLEAR);

		CloseHandle(m_hCommPort);
		m_hCommPort = NULL;
	}

	//-------------------------------------------------------------------------
	// Delete Comm Thread
	//-------------------------------------------------------------------------
	DeleteComm();

	//-------------------------------------------------------------------------
	// Close Etc Handles
	//-------------------------------------------------------------------------
//	m_pWnd = NULL;

	if( m_osRead.hEvent )
	{
		CloseHandle(m_osRead.hEvent);
		m_osRead.hEvent = NULL;
	}
	
	if( m_osWrite.hEvent )
	{
		CloseHandle(m_osWrite.hEvent);
		m_osWrite.hEvent = NULL;
	}

	return TRUE;
}

BOOL CSerial::IsActive()
{
	BOOL bReturn = TRUE;

	if( m_bPortOpened && m_hCommPort != NULL && m_hCommThread != NULL && !m_bDeleteThread )
		{ bReturn = TRUE; }
	else
		{ bReturn = FALSE; }

	return bReturn;
}

BOOL CSerial::CreateComm()
{
	if( m_hCommThread != NULL ) { return TRUE; }

	m_bDeleteThread = FALSE;

	m_hCommThread = ::CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE) CommThread, (LPVOID)this, 0, &m_dwCommThreadID);

	if( m_hCommThread == NULL ) { return FALSE; }

	return TRUE;
}

BOOL CSerial::ResumeComm()
{
	if( m_hCommThread == NULL ) { return FALSE; }

	DWORD dwReturn = ::ResumeThread(m_hCommThread);

	if( dwReturn == 0xFFFFFFFF ) { return FALSE; }

	return TRUE;
}

BOOL CSerial::SuspendComm()
{
	if( m_hCommThread == NULL ) { return FALSE; }

	DWORD dwReturn = ::SuspendThread(m_hCommThread);

	if( dwReturn == 0xFFFFFFFF ) { return FALSE; }

	return TRUE;
}

BOOL CSerial::DeleteComm()
{
	if( m_hCommThread == NULL ) { return TRUE; }
/*
	DWORD dwExitCode = 0;
	BOOL bRet = ::GetExitCodeThread(m_hCommThread, &dwExitCode);

	int nTimeout = 1000;

	if( dwExitCode == STILL_ACTIVE ) 
	{
		//for( int i=0; i<10; i++ )
		{
			dwExitCode = WaitForSingleObject(m_hCommThread, nTimeout);

			if( dwExitCode == WAIT_OBJECT_0 )
			{ 
				bRet = ::TerminateThread(m_hCommThread, dwExitCode); 
				//break;
			}
			else 
			{ 
				//bRet = ::TerminateThread(m_hCommThread, dwExitCode); 
				bRet = ::PostThreadMessage(m_dwCommThreadID, WM_QUIT, 0, 0); 
			}
		}
	}
	else
	{
		::TerminateThread(m_hCommThread, dwExitCode);
	}
*/
	DWORD dwExitCode = 0;
	int   nTimeout = 1000;

	while( TRUE )
	{
		if( !::GetExitCodeThread(m_hCommThread, &dwExitCode) ) 
		{
			break;
		} 
		else if( dwExitCode == STILL_ACTIVE ) 
		{
			Sleep(50);
			nTimeout -= 50;

			if( nTimeout <= 0 ) 
			{
				TerminateThread(m_hCommThread, dwExitCode);
// 				::PostThreadMessage(m_dwCommThreadID, WM_QUIT, 0, 0);
				break;
			}
		} 
		else 
		{
// 			TerminateThread(m_hCommThread, dwExitCode);
			break;
		}
	}
	
	m_hCommThread = NULL;

	return TRUE;
}

DWORD CSerial::CommThread(LPVOID lpData)
{
	DWORD dwEvtMask = 0, dwReadLen = 0, dwError = 0;
	BOOL bResult = TRUE;
	COMSTAT comstat;
	BYTE byReceive[BUFFER_SIZE];

	CSerial* pComm=(CSerial *) lpData;
	if( pComm == NULL ) { return (DWORD)(-1); }

	pComm->m_dwTotalReadLen = 0;
	while( pComm->IsActive() )
	{
		dwEvtMask = 0;
		bResult = WaitCommEvent(pComm->m_hCommPort, &dwEvtMask, NULL);

		// ���α׷��� ����ɶ� Close Port �� ȣ��Ǿ� �̰����� ����
		if( !bResult )
		{
			if( pComm->m_bPortOpened ) 
				{ pComm->ClosePort(); } // �̹� Close Port �ؼ� ���ͼ� ���ص� �ɰ� ������ 
			break;
		}
		// �������� �̺�Ʈ�� �޾��� ��
		else
		{
			bResult = ClearCommError(pComm->m_hCommPort, &dwError, &comstat);
			if( comstat.cbInQue == 0 ) { continue; }
		}

		// ���� ���ڰ� ������ 
		if( dwEvtMask & EV_RXCHAR )
		{
//			Sleep(10);
			
			pComm->m_dwTotalReadLen = 0;
			memset(pComm->m_byRecv, 0, sizeof(pComm->m_byRecv));

			do
			{
				memset(byReceive, 0, sizeof(byReceive));

				dwReadLen = pComm->ReceiveData(byReceive);

				if( dwReadLen > 0 )
				{
					memcpy(&pComm->m_byRecv[pComm->m_dwTotalReadLen], byReceive, dwReadLen);
					pComm->m_dwTotalReadLen += dwReadLen;
				}
			}
			while( dwReadLen && pComm->IsActive() );

			pComm->m_byRecv[pComm->m_dwTotalReadLen] = '\0';

			if( pComm->m_dwTotalReadLen > 0 )
			{
				pComm->OnReceiveData();
			}

//			pComm->m_dwTotalReadLen = 0;
		}
		// ���̺��� ������ �ȵǸ� �̰����� ���� 
		else if( dwEvtMask == 0 )
		{
			if( pComm->m_bPortOpened ) 
				{ pComm->ClosePort(); }
			break;
		}
		// �ٸ� �̺�Ʈ �ɾ������ �̰����� ���ð� ������ ����� ������ �ȵɰ� ������
		else
		{
			ASSERT( FALSE );
		}

		Sleep(0); // ���������� �����Ͱ� �ö� CPU �������� ���߱� ����
	}

	return TRUE;
}

DWORD CSerial::ReceiveData(BYTE* pbyReceive)
{
	if( IsActive() == FALSE ) { return 0; }

	DWORD dwError = 0;
	COMSTAT csState;
	ClearCommError(m_hCommPort, &dwError, &csState);
	if( csState.cbInQue <= 0 ) { return 0; }
	
	m_osRead.Offset = 0;
	m_osRead.OffsetHigh = 0;

	DWORD dwReadSize = 0;
	if( !ReadFile(m_hCommPort, pbyReceive, BUFFER_SIZE, &dwReadSize, &m_osRead) )
	{
		dwError = GetLastError();
		while( dwError == ERROR_IO_PENDING || dwError == ERROR_IO_INCOMPLETE )
		{
			if( !IsActive() ) { return 0; }

			if( GetOverlappedResult(m_hCommPort, &m_osRead, &dwReadSize, TRUE) )
				{ return dwReadSize; }
			
			dwError = GetLastError();
		}
		
		ClearCommError(m_hCommPort, &dwError, &csState);
	}

	return dwReadSize;
}

void CSerial::OnReceiveData()
{
	if( m_pWnd->m_hWnd == NULL ) { return; }
	m_pWnd->SendMessage(m_nMsgRecv, WPARAM(&m_sName), 0);
}

BYTE* CSerial::GetRecvData()
{
	return m_byRecv;
}

DWORD CSerial::GetRecvSize()
{
	return m_dwTotalReadLen;
}

BYTE* CSerial::GetSendData()
{
	return m_bySend;
}

DWORD CSerial::WriteData(BYTE* pbyData, DWORD dwSize)
{
	return SendData(pbyData, dwSize);
}

DWORD CSerial::SendData(BYTE* pbyData, DWORD dwSize)
{
	if( IsActive() == FALSE ) { return 0; }

	if( pbyData == NULL || dwSize == 0 ) { return 0; }

//	BYTE bySend[BUFFER_SIZE];
	memset(m_bySend, 0, sizeof(m_bySend));
	memcpy(m_bySend, pbyData, dwSize);

	m_osWrite.Offset = 0;
	m_osWrite.OffsetHigh = 0;

	DWORD dwWriteSize = 0;
	DWORD dwError = 0;
	COMSTAT csState;

	if( m_pWnd->m_hWnd != NULL )
		m_pWnd->SendMessage(m_nMsgSend, WPARAM(&m_sName), 0);

	if( !WriteFile(m_hCommPort, m_bySend, dwSize, &dwWriteSize, &m_osWrite) )
	{
		dwError = GetLastError();
		while( dwError == ERROR_IO_PENDING || dwError == ERROR_IO_INCOMPLETE )
		{
			if( !IsActive() ) return 0; 

			if( GetOverlappedResult(m_hCommPort, &m_osWrite, &dwWriteSize, TRUE) )
				return dwWriteSize; 

			dwError = GetLastError();
		}
		
		ClearCommError(m_hCommPort, &dwError, &csState);
	}
	
	return dwWriteSize;
}
