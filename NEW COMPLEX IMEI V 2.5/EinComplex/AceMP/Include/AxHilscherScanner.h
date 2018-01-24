#ifndef __AX_HILSCHERSCANNER_H__
#define __AX_HILSCHERSCANNER_H__

#pragma once

#include "AxIOScanner.h"
#include "AxIOMgr.h"

enum HSCH_ERR {
	HSCH_NO_ERROR					= 0,
	HSCH_BOARD_NOT_INITIALIZED,
	HSCH_INIT_STATE_ERROR,		
	HSCH_READF_STATE_ERROR,		
	HSCH_CMD_ACTIVE,			
	HSCH_PARAMETER_UNKNOWN,		
	HSCH_WRONG_DRIVER_VERSION,					
	HSCH_PCI_SET_CONFIG_MODE,
	HSCH_DRV_PCI_READ_DPM_LENGTH,
	HSCH_PCI_SET_RUN_MODE,
	HSCH_DEV_DPM_ACCESS_ERROR		= 10,
	HSCH_DEV_NOT_READY,
	HSCH_DEV_NOT_RUNNING,
	HSCH_DEV_WATCHDOG_FAILED,
	HSCH_DEV_OS_VERSION_ERROR,
	HSCH_DEV_SYSERR,
	HSCH_DEV_MAILBOX_FULL,
	HSCH_DEV_PUT_TIMEOUT,
	HSCH_DEV_GET_TIMEOUT,
	HSCH_DEV_GET_NO_MESSAGE,
	HSCH_DEV_RESET_TIMEOUT			= 20,
	HSCH_DEV_NO_COM_FLAG,
	HSCH_DEV_EXCHANGE_FAILED,
	HSCH_DEV_EXCHANGE_TIMEOUT,
	HSCH_DEV_COM_MODE_UNKNOWN,
	HSCH_DEV_FUNCTION_FAILED,
	HSCH_DEV_DPMSIZE_MISMATCH,
	HSCH_DEV_STATE_MODE_UNKNOWN,
	HSCH_USR_OPEN_ERROR				= 30,
	HSCH_USR_INIT_DRV_ERROR,
	HSCH_USR_NOT_INITIALIZED,
	HSCH_USR_COMM_ERR,
	HSCH_USR_DEV_NUMBER_INVALID,
	HSCH_USR_INFO_AREA_INVALID,
	HSCH_USR_NUMBER_INVALID,
	HSCH_USR_MODE_INVALID,
	HSCH_USR_MSG_BUF_NULL_PTR,
	HSCH_USR_MSG_BUF_TOO_SHORT,
	HSCH_USR_SIZE_INVALID,
	HSCH_USR_SIZE_ZERO				= 42,
	HSCH_USR_SIZE_TOO_LONG,
	HSCH_USR_DEV_PTR_NULL,
	HSCH_USR_BUF_PTR_NULL,
	HSCH_USR_SENDSIZE_TOO_LONG,
	HSCH_USR_RECVSIZE_TOO_LONG,
	HSCH_USR_SENDBUF_PTR_NULL,
	HSCH_USR_RECVBUF_PTR_NULL,
	HSCH_ERR_LAST
};

class __declspec(dllexport) CAxHilscherScanner : public CAxIOScanner 
{
	enum {
		MAX_DEV_NUM		= 4,
		MAX_NUM_NODE	= 64
	};

public:
	CAxHilscherScanner(CAxIOMgr* pIOMgr);
	virtual ~CAxHilscherScanner();

	BOOL Init(int nScanner, LPCTSTR pszFile);

private:
	int			m_nDeviceNum;
	int			m_nBufferSize;
	int			m_nNumNode;
	int			m_nNumIpByte;
	int			m_nNumOpByte;
	CUIntArray	m_NumDiIp;
	CUIntArray	m_NumDiOp;
	CUIntArray	m_DiIpNum;
	CUIntArray	m_DiOpNum;
	CUIntArray	m_DiIpAddr;
	CUIntArray	m_DiOpAddr;
	CByteArray	m_inData;
	CByteArray	m_outData;

	void OnTimer();
	BOOL InitHilscher();
	BOOL CheckDevInfo();
	BOOL CheckParam();
	BOOL ReadInput();
	BOOL WriteOutput();
	BOOL ReadData();
	BOOL WriteData();
	BOOL CheckDriveErr(short);
	BOOL Reset();
	BOOL Shutdown();
	void CreateDiIp();
	void CreateDiOp();
	void InitDataByte();
	void SetDiIpVal();
	void GetDiOpVal();
	void ReadNodeParam();
	void AssignValue();
	void SetError(int nErr);
	void InitErrStr();
};

#endif