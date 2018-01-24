#ifndef __COMMBCR_H__
#define __COMMBCR_H__

#include "Serial.h"

class CCommBCR : public CSerial
{
public:
	enum CmdCode
	{
		CC_LIGHT_ON,
		CC_LIGHT_OFF
	};

	BOOL m_bRecvComp;
	CString m_sRecv;

public:
	CCommBCR();
	virtual	~CCommBCR();

	void SendPacket(CmdCode emCode);
	BOOL RecvPacket();

	virtual void OnReceiveData();
};

#endif
