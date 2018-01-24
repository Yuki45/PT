#ifndef __COMMTESTER_H__
#define __COMMTESTER_H__

#include "Serial.h"

class CCommTester : public CSerial
{
public:
	enum CmdCode
	{
		CC_START,
		CC_ACK,
		CC_NAK
	};

public:
	BOOL m_bRecvReady[DEF_MAX_TESTER_IN_PC];
	BOOL m_bRecvAck[DEF_MAX_TESTER_IN_PC];
	BOOL m_bRecvPass[DEF_MAX_TESTER_IN_PC];
	BOOL m_bRecvFail[DEF_MAX_TESTER_IN_PC];
	BOOL m_bRecvLabel[DEF_MAX_TESTER_IN_PC];

	CString m_sFailName[DEF_MAX_TESTER_IN_PC];

public:
	CCommTester();
	virtual	~CCommTester();

	void SendPacket(CmdCode emCode, int nIndex, CString sBarcode = _T(""));
	BOOL RecvPacket();

	virtual void OnReceiveData();
};

#endif
