#ifndef _CSerial_h_
#define _CSerial_h_

//#define MAXBLOCK		80
//#define MAXPORTS		4

//#define FC_DTRDSR		0x01
//#define FC_RTSCTS		0x02
//#define FC_XONXOFF	0x04

#ifndef _ASCII_TABLE
#define _ASCII_TABLE
	#define ASCII_NUL	0x00	// null
	#define ASCII_SOH	0x01	// start of heading
	#define ASCII_STX	0x02	// start of text
	#define ASCII_ETX	0x03	// end of text
	#define ASCII_EOT	0x04	// end of transmission
	#define ASCII_ENQ	0x05	// enquiry
	#define ASCII_ACK	0x06	// acknowledge
	#define ASCII_BEL	0x07	// bell
	#define ASCII_BS	0x08	// backspace
	#define ASCII_TAB	0x09	// horizontal tab
	#define ASCII_LF	0x0A	// NL line feed, new line
	#define ASCII_VT	0x0B	// vertical tab
	#define ASCII_FF	0x0C	// NP form feed, new page
	#define ASCII_CR	0x0D	// carriage return
	#define ASCII_SO	0x0E	// shift out
	#define ASCII_SI	0x0F	// shift in
	#define ASCII_DLE	0x10	// data link escape
	#define ASCII_XON	0x11	// device control 1
	#define ASCII_DC2	0x12	// device control 2
	#define ASCII_XOFF	0x13	// device control 3
	#define ASCII_DC4	0x14	// device control 4
	#define ASCII_NAK	0x15	// negative acknowledge
	#define ASCII_SYN	0x16	// synchronous idle
	#define ASCII_ETB	0x17	// end of trans block
	#define ASCII_CAN	0x18	// cancel
	#define ASCII_EM	0x19	// end of medium
	#define ASCII_SUB	0x1A	// substitute
	#define ASCII_ESC	0x1B	// escape
	#define ASCII_FS	0x1C	// file separator
	#define ASCII_GS	0x1D	// group separator
	#define ASCII_RS	0x1E	// record separator
	#define ASCII_US	0x1F	// unit separator
#endif

#define	BUFFER_SIZE		2048
#define QUEUE_SIZE		4096

#define	UM_SERIAL_MSG		WM_USER+100
#define UM_SERIAL_MSG_SEND	WM_USER+101
#define UM_SERIAL_MSG_RECV	WM_USER+102

BYTE CRC8_Packet(BYTE* packet, int size);
BYTE BCC_Packet(BYTE* packet, int size);

class CSerial
{
public:
	UINT			m_nPortNo;
	DWORD			m_dwBaudRate;
	UINT			m_nType;
	UINT			m_nIndex;
	UINT			m_nAddr;
	int				m_nErrorCode;

protected:
	CString			m_sName;
	CWnd*			m_pWnd;
	DWORD			m_nMsg;
	DWORD			m_nMsgSend;
	DWORD			m_nMsgRecv;

	BYTE			m_byDataBits;
	BYTE			m_byParity;
	BYTE			m_byStopBits;
	BOOL			m_bXonXoff;

	HANDLE			m_hCommPort;
	HANDLE			m_hCommThread;
	DWORD			m_dwCommThreadID;
	OVERLAPPED		m_osRead;
	OVERLAPPED		m_osWrite;
	DWORD			m_dwTotalReadLen;
	BYTE			m_bySend[BUFFER_SIZE];
	BYTE			m_byRecv[BUFFER_SIZE];

	BOOL			m_bPortOpened;
	BOOL			m_bDeleteThread;

public:
					CSerial();
	virtual		   ~CSerial();

public:
	void			SetPort(
							CString sName = _T("Serial"),
							CWnd* pWnd = NULL,
							UINT  nPortNo = 1, 
							DWORD dwBaudRate = 9600, 
							BYTE  byDataBits = 8, 
							BYTE  byParity = NOPARITY, 
							BYTE  byStopBits = ONESTOPBIT, 
							BOOL  bXonXoff = FALSE);
	BOOL			OpenPort();
	BOOL			OpenPort(
							CString sName,
							CWnd* pWnd = NULL,
							UINT  nPortNo = 1, 
							DWORD dwBaudRate = 9600, 
							BYTE  byDataBits = 8, 
							BYTE  byParity = NOPARITY, 
							BYTE  byStopBits = ONESTOPBIT, 
							BOOL  bXonXoff = FALSE);
	BOOL			ClosePort();
	BOOL			IsActive();
	BYTE*			GetRecvData();
	DWORD			GetRecvSize();
	BYTE*			GetSendData();

	DWORD			WriteData(BYTE* pbyData, DWORD dwSize);

	HWND			GetHWnd() { return m_pWnd->m_hWnd; }
	CString&		GetName() { return m_sName; }
//	UINT			GetPort() { return m_nPortNo; }
//	DWORD			GetBaudRate() { return m_dwBaudRate; }

protected:
	BOOL			CreateComm();
	BOOL			ResumeComm();
	BOOL			SuspendComm();
	BOOL			DeleteComm();

	static DWORD	CommThread(LPVOID lpData);
	DWORD			ReceiveData(BYTE* pbyReceive);
	DWORD			SendData(BYTE* pbyData, DWORD dwSize);
	virtual void	OnReceiveData();
};

typedef CTypedPtrArray<CPtrArray, CSerial*> CSerialPtrArray;

#endif // _CSerial_h_
