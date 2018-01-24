#include "stdafx.h"
#include "EinComplex.h"
#include "SerialHub.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

CSerialHub* CSerialHub::theSerialHub = NULL;

CSerialHub::CSerialHub()
{
	m_pWnd = NULL;
	m_nNumSerial = 0;
	m_sFile = _T("\\EinComplex\\Data\\Profile\\Service\\Serial.ini");
}

CSerialHub::~CSerialHub()
{
	for( int i=m_nNumSerial-1; i>=0; i-- ) 
	{
		delete m_serial[i];
		m_serial[i] = NULL;
	}
}

CSerialHub* CSerialHub::GetSerialHub()
{
	if( theSerialHub == NULL ) { theSerialHub = new CSerialHub; }
	return theSerialHub;
}

void CSerialHub::CreateSerial(CWnd* pWnd)
{
	m_pWnd = pWnd;

	// Init Profile
	CString sValue = _T("");
	CString sSect = _T("");

	ReadString(_T("General"), _T("NumSerial"), sValue);
	m_nNumSerial = _ttoi(sValue);

	// Create Serial
	UINT nType = TP_NORMAL;
	for( int i=0; i<m_nNumSerial; i++ )
	{
		sSect.Format(_T("Serial_%d"), i);
		ReadString(sSect, _T("Type"), sValue);
		nType = _ttoi(sValue);

		if( nType == TP_WORLD )
		{
			CCommWorld* pSerial = new CCommWorld;
			pSerial->m_nType = nType;

			m_serial.Add(pSerial);
		}
		else if( nType == TP_BCR )
		{
			CCommBCR* pSerial = new CCommBCR;
			pSerial->m_nType = nType;

			m_serial.Add(pSerial);
		}
		else if( nType == TP_TESTER )
		{
			CCommTester* pSerial = new CCommTester;
			pSerial->m_nType = nType;

			m_serial.Add(pSerial);
		}
		else if( nType == TP_LABELPRINTER )
		{
			CCommLabelPrinter* pSerial = new CCommLabelPrinter;
			pSerial->m_nType = nType;

			m_serial.Add(pSerial);
		}
		else
		{
			CSerial* pSerial = new CSerial;
			pSerial->m_nType = nType;

			m_serial.Add(pSerial);
		}
	}

	OpenSerial();
}

void CSerialHub::CloseSerial()
{
	for( int i=0; i<m_nNumSerial; i++ )
	{
		m_serial[i]->ClosePort();
	}
}

void CSerialHub::OpenSerial()
{
	CString sValue		= _T("");
	CString sSect		= _T("");

	CString sName		= _T("Serial");
	UINT	nPort		= 1;
	DWORD	dwBaudRate	= 9600;
	BYTE	byDataBits	= 8;
	BYTE	byParity	= NOPARITY;
	BYTE	byStopBits	= ONESTOPBIT;
	BOOL	bXonXoff	= FALSE;
	BOOL	bAutoOpen	= TRUE;
	UINT	nIndex		= 0;
	UINT	nAddr		= 0;

	for( int i=0; i<m_nNumSerial; i++ )
	{
		sSect.Format(_T("Serial_%d"), i);

		ReadString(sSect, _T("Name"), sValue);
		sName = sValue;
		
		ReadString(sSect, _T("Port"), sValue);
		nPort = _ttoi(sValue);

		ReadString(sSect, _T("BaudRate"), sValue);
		dwBaudRate = _ttoi(sValue);

		ReadString(sSect, _T("DataBits"), sValue);
		byDataBits = _ttoi(sValue);

		ReadString(sSect, _T("Parity"),	sValue);
		byParity = _ttoi(sValue);

		ReadString(sSect, _T("StopBits"), sValue);
		byStopBits = _ttoi(sValue);

		ReadString(sSect, _T("XonXoff"), sValue);
		bXonXoff = _ttoi(sValue);

		ReadString(sSect, _T("AutoOpen"), sValue);
		bAutoOpen = _ttoi(sValue);

		ReadString(sSect, _T("Index"), sValue);
		nIndex = _ttoi(sValue);
		m_serial[i]->m_nIndex = nIndex;

		ReadString(sSect, _T("Addr"), sValue);
		nAddr = _ttoi(sValue);
		m_serial[i]->m_nAddr = nAddr;

		if( bAutoOpen )
		{
			m_serial[i]->OpenPort(
					sName,
					m_pWnd,
					nPort,
					dwBaudRate,
					byDataBits,
					byParity,
					byStopBits,
					bXonXoff);
		}
		else
		{
			m_serial[i]->SetPort(
					sName,
					m_pWnd,
					nPort,
					dwBaudRate,
					byDataBits,
					byParity,
					byStopBits,
					bXonXoff);
		}
	}
}

CSerial* CSerialHub::GetSerial(int nIndex)
{
	ASSERT( nIndex < m_nNumSerial );
	return m_serial[nIndex];
}

CSerial* CSerialHub::GetSerial(LPCTSTR pszName)
{
	for( int i=0; i<m_nNumSerial; i++ ) 
	{
		if( m_serial[i]->GetName() == pszName ) 
		{
			return m_serial[i];
		}
	}

	ASSERT( FALSE );
	return NULL;
}

void CSerialHub::ReadString(LPCTSTR pszSect, LPCTSTR pszKey, CString & sValue)
{
	GetPrivateProfileString(pszSect, pszKey, _T(""), sValue.GetBuffer(100), 100, m_sFile);
	sValue.ReleaseBuffer();
}
