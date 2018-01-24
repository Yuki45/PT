#ifndef __COMMLABELPRINTER_H__
#define __COMMLABELPRINTER_H__

#include "Serial.h"

class CCommLabelPrinter : public CSerial
{
public:
	enum CmdCode
	{
		CC_PRINT
	};

public:
	int m_nJigNo;
	CString m_sBarcode;
	CString m_sFailName;

public:
	CCommLabelPrinter();
	virtual	~CCommLabelPrinter();

	void SendPacket(CmdCode emCode);
	BOOL RecvPacket();

	virtual void OnReceiveData();
};

#endif
