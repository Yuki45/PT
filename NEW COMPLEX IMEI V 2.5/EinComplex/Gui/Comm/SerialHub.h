#ifndef _CSerialHub_h_
#define _CSerialHub_h_

#pragma once

#include "Serial.h"
#include "CommWorld.h"
#include "CommBCR.h"
#include "CommTester.h"
#include "CommLabelPrinter.h"

class CSerialHub  
{
public:
	enum
	{
		TP_NORMAL,
		TP_WORLD,
		TP_BCR,
		TP_TESTER,
		TP_LABELPRINTER
	};

public:
					CSerialHub();
	virtual		   ~CSerialHub();
	static CSerialHub* GetSerialHub();

	void			CreateSerial(CWnd* pWnd);
	void			CloseSerial();
	int				GetSerialNum() { return m_nNumSerial; }
	CSerial*		GetSerial(int nIndex);
	CSerial*		GetSerial(LPCTSTR pszName);

private:
	void			OpenSerial();
	void			ReadString(LPCTSTR pszSect, LPCTSTR pszKey, CString & sValue);

private:
	static CSerialHub* theSerialHub;

	CWnd*			m_pWnd;
	UINT			m_nMsg;
	UINT			m_nMsgRecv;
	int				m_nNumSerial;
	CString			m_sFile;
	CSerialPtrArray m_serial;
};

#endif // _CSerialHub_h_ 
