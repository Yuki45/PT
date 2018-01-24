#ifndef __COMMWORLD_H__
#define __COMMWORLD_H__

#include "Serial.h"

class CCommWorld : public CSerial
{
public:
	enum CmdCode
	{
		CC_GET_INPUT,
		CC_SET_OUTPUT
	};

	CCommWorld();
	virtual	~CCommWorld();

	BOOL m_bInput[DEF_MAX_IO_IN_WORLD];
	BOOL m_bOutput[DEF_MAX_IO_IN_WORLD];

	void SendPacket(CmdCode emCode);
	virtual void OnReceiveData();
	void ParseInput(CString sValue, int nStartIndex);
};

#endif
