 // AxHilscherScanner.cpp: implementation of the CAxHilscherScanner class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "cifuser.h"
#include "AxHilscherScanner.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxHilscherScanner::CAxHilscherScanner(CAxIOMgr* pIOMgr)
{
	m_pIOMgr = pIOMgr;
	m_nDeviceNum	= 0;
	m_nNumDiIp		= 0;
	m_nNumDiOp		= 0;
	m_nNumIpByte	= 0;
	m_nNumOpByte	= 0;
}

CAxHilscherScanner::~CAxHilscherScanner()
{
	DWORD dwExitCode;
	BOOL bDone = FALSE;

	m_bTerminate = TRUE;
	while(!bDone) {
		bDone = TRUE;
		if(!::GetExitCodeThread(m_hScanThread, &dwExitCode))
			break;
		if(dwExitCode == STILL_ACTIVE) {
			bDone = FALSE;
			Sleep(100);
		}
	}
}

BOOL CAxHilscherScanner::Init(int nScanner, LPCTSTR pszFile)
{
	m_bInit = FALSE;
	m_nScanner = nScanner;
	m_profile.m_sIniFile = pszFile;
	m_profile.m_sSect.Format(_T("Scanner%d"), m_nScanner);
	m_nScanTime		= m_profile.ReadInt(_T("ScanTime"));
	m_nDeviceNum	= m_profile.ReadInt(_T("DeviceNum"));
	m_nNumNode		= m_profile.ReadInt(_T("NumNode"));
	m_nBufferSize	= m_profile.ReadInt(_T("BufferSize"));
	m_bSimulate		= m_profile.ReadInt(_T("Simulate"));

	if(!InitHilscher()) return FALSE;
	if(!CheckDevInfo()) return FALSE;

	ReadNodeParam();
	AssignValue();

	if(!CheckParam()) return FALSE;

	CreateDiIp();
	CreateDiOp();

	InitDataByte();
	InitErrStr();

	Startup();
	m_bInit = TRUE;
	return TRUE;
}

void CAxHilscherScanner::OnTimer()
{
	if(m_bTerminate) {
		for(int i=0; i<m_nNumDiOp;i++)
			m_doData[i].SetValue(OFF);
		WriteOutput();
		Shutdown();
		AfxEndThread(0);
	}
	if(!ReadInput()) return;
	for(int i=0; i<m_nNumDiIp; i++)
		m_diData[i].FireEvents();
	WriteOutput();
}

BOOL CAxHilscherScanner::InitHilscher()
{
	if(m_bSimulate) return TRUE;

	// Open driver
	if(!CheckDriveErr(DevOpenDriver(m_nDeviceNum))) {
		return FALSE;
	}
	// Initialize board
	if(!CheckDriveErr(DevInitBoard(m_nDeviceNum, NULL))) {
		return FALSE;
	}
	
	// Set to HOST_READY state
	if(!CheckDriveErr(DevSetHostState((USHORT)m_nDeviceNum, HOST_READY, 200L))) {
		return FALSE;
	}
	return TRUE;
}

BOOL CAxHilscherScanner::CheckDevInfo()
{
	DEVINFO tDevInfo;

	if(m_bSimulate) return TRUE;

	if(!CheckDriveErr(DevGetInfo((USHORT)m_nDeviceNum, GET_DEV_INFO,
									sizeof(tDevInfo), &tDevInfo))) {
		return FALSE;
	}
	if(tDevInfo.bDpmSize <= m_nBufferSize) {
		ASSERT(FALSE);
		return FALSE;
	}
	return TRUE;
}

void CAxHilscherScanner::ReadNodeParam()
{
	for(int i=0; i<m_nNumNode; i++) {
		m_profile.m_sSect.Format(_T("Node%d"), i+1);
		m_NumDiIp.Add(m_profile.ReadInt(_T("NumDiIp")));
		m_NumDiOp.Add(m_profile.ReadInt(_T("NumDiOp")));
		m_DiIpNum.Add(m_profile.ReadInt(_T("DiIpNum")));
		m_DiOpNum.Add(m_profile.ReadInt(_T("DiOpNum")));
		m_DiIpAddr.Add(m_profile.ReadInt(_T("DiIpAddr")));
		m_DiOpAddr.Add(m_profile.ReadInt(_T("DiOpAddr")));
	}
}

void CAxHilscherScanner::AssignValue()
{
	for(int i=0; i<m_nNumNode; i++) {
		m_nNumDiIp += m_NumDiIp[i];
		m_nNumDiOp += m_NumDiOp[i];
	}

	m_nNumIpByte = m_DiIpAddr[m_nNumNode-1] +
				   m_NumDiIp[m_nNumNode-1] / 8 + 1;
	
	m_nNumOpByte = m_DiOpAddr[m_nNumNode-1] +
				   m_NumDiOp[m_nNumNode-1] / 8 + 1;				   
}

BOOL CAxHilscherScanner::CheckParam()
{
	if(m_nScanTime < MIN_SCAN_TIME) {
		ASSERT(FALSE);
		return FALSE;
	}
	if(m_nNumDiIp > MAX_NUM_IO || m_nNumDiOp > MAX_NUM_IO) {
		ASSERT(FALSE);
		return FALSE;
	}
	if(m_nDeviceNum > MAX_DEV_NUM || m_nNumNode > MAX_NUM_NODE) {
		ASSERT(FALSE);
		return FALSE;
	}
	return TRUE;
}

void CAxHilscherScanner::CreateDiIp()
{
	m_diData.SetSize(m_nNumDiIp);
	for(int i=0; i<m_nNumDiIp; i++) {
		m_diData[i].m_sAddr.Format(_T("x%03x"), i);
	}
}

void CAxHilscherScanner::CreateDiOp()
{
	m_doData.SetSize(m_nNumDiOp);
	for(int i=0; i<m_nNumDiOp; i++) {
		m_doData[i].m_sAddr.Format(_T("y%03x"), i);
	}
}

void CAxHilscherScanner::InitDataByte()
{
	m_inData.SetSize(m_nNumIpByte);
	m_outData.SetSize(m_nNumOpByte);
}

BOOL CAxHilscherScanner::ReadInput()
{
	if(m_bSimulate) return TRUE;
	
	if(!ReadData()) return FALSE;

	SetDiIpVal();
	return TRUE;
}

void CAxHilscherScanner::SetDiIpVal()
{
	int nOpr, nByte;

	for(int i=0; i<m_nNumNode; i++) {
		for(UINT j=0; j<m_NumDiIp[i]; j++) {
			nByte = m_DiIpAddr[i] + (int)(j/8);
			nOpr = 1 << (j%8);
			m_diData[m_DiIpNum[i]+j].SetValue((nOpr & m_inData[nByte]) > 0 ? ON:OFF);
		}
	}
}

BOOL CAxHilscherScanner::WriteOutput()
{
	if(m_bSimulate) return TRUE;

	GetDiOpVal();

	if(!WriteData()) return FALSE;
	return TRUE;
}

void CAxHilscherScanner::GetDiOpVal()
{
	int nOpr, nByte;
	BYTE nOut;

	for(int i=0; i<m_nNumNode; i++) {
		for(UINT j=0; j<m_NumDiOp[i]; j++) {
			nByte = m_DiOpAddr[i] + (int)(j/8);
			nOut = m_outData[nByte];
			nOpr = 1 << (j%8);
			m_outData[nByte] = m_doData[m_DiOpNum[i]+j].GetValue() ?
							   nOut |= nOpr : nOut &= ~nOpr;
		}
	}
}

BOOL CAxHilscherScanner::ReadData()
{
	if(m_bSimulate) return TRUE;

	// Get dual port memory into output data bytes
	if(!CheckDriveErr(DevExchangeIO((USHORT)m_nDeviceNum,
									0, 0, NULL, 0,
									(USHORT)m_nNumIpByte,
									m_inData.GetData(), 100L))) {

		return FALSE;
	}
	return TRUE;
}

BOOL CAxHilscherScanner::WriteData()
{
	if(m_bSimulate) return TRUE;

	// Set dual port memory to values of output data bytes
	if(!CheckDriveErr(DevExchangeIO((USHORT)m_nDeviceNum, 0,
									(USHORT)m_nNumOpByte,
									m_outData.GetData(),
									0, 0, NULL, 100L))) {

		return FALSE;
	}
	return TRUE;
}

BOOL CAxHilscherScanner::Reset()
{
	if(m_bSimulate) return TRUE;

	DevReset((USHORT)m_nDeviceNum, COLDSTART, 15000L);
	return TRUE;
}

BOOL CAxHilscherScanner::Shutdown()
{
	if(m_bSimulate) return TRUE;

	DevSetHostState((USHORT)m_nDeviceNum, HOST_NOT_READY, 0L);
	DevExitBoard((USHORT)m_nDeviceNum);
	DevCloseDriver((USHORT)m_nDeviceNum);
	return TRUE;
}

BOOL CAxHilscherScanner::CheckDriveErr(short sRetVal)
{
	static BOOL bAbort = FALSE;
	if(sRetVal != DRV_NO_ERROR) {
		if(!m_bInit || m_bTerminate) return FALSE;
		SetError(-sRetVal);
		if(-sRetVal == HSCH_DEV_NO_COM_FLAG) {
			Shutdown();
			InitHilscher();
		}
		else {
			Reset();
		}
		return FALSE;
	}
	return TRUE;
}

void CAxHilscherScanner::SetError(int nErr)
{
	m_nErr = nErr;
	m_sErrStr[m_nErr] += m_sName;
	m_nErr = 0;
}


void CAxHilscherScanner::InitErrStr()
{
	m_sErrStr.SetSize(HSCH_ERR_LAST);

	m_sErrStr[HSCH_NO_ERROR]				= _T("No error-");
	m_sErrStr[HSCH_BOARD_NOT_INITIALIZED]	= _T("Board not initialized-");
	m_sErrStr[HSCH_INIT_STATE_ERROR]		= _T("Error in internal init state-");
	m_sErrStr[HSCH_READF_STATE_ERROR]		= _T("Error in internal read state-");
	m_sErrStr[HSCH_CMD_ACTIVE]				= _T("Command on this channel is active-");	
	m_sErrStr[HSCH_PARAMETER_UNKNOWN]		= _T("Unknown parameter in function occured-");
	m_sErrStr[HSCH_WRONG_DRIVER_VERSION]	= _T("Version is incompatible with DLL-");				
	m_sErrStr[HSCH_PCI_SET_CONFIG_MODE]		= _T("Error during PCI set run mode-");
	m_sErrStr[HSCH_DRV_PCI_READ_DPM_LENGTH]	= _T("Could not read PCI dual port memory length-");
	m_sErrStr[HSCH_PCI_SET_RUN_MODE]		= _T("Error during PCI set run mode-");
	m_sErrStr[HSCH_DEV_DPM_ACCESS_ERROR]	= _T("Dual port ram not accessable-");
	m_sErrStr[HSCH_DEV_NOT_READY]			= _T("Not ready-");
	m_sErrStr[HSCH_DEV_NOT_RUNNING]			= _T("Not running-");
	m_sErrStr[HSCH_DEV_WATCHDOG_FAILED]		= _T("Watchdog test failed-");
	m_sErrStr[HSCH_DEV_OS_VERSION_ERROR]	= _T("Wrong OS version-");
	m_sErrStr[HSCH_DEV_SYSERR]				= _T("Error in dual port flags-");
	m_sErrStr[HSCH_DEV_MAILBOX_FULL]		= _T("Send mailbox is full-");
	m_sErrStr[HSCH_DEV_PUT_TIMEOUT]			= _T("PutMessage timeout-");
	m_sErrStr[HSCH_DEV_GET_TIMEOUT]			= _T("GetMessage timeout-");
	m_sErrStr[HSCH_DEV_GET_NO_MESSAGE]		= _T("No message available-");
	m_sErrStr[HSCH_DEV_RESET_TIMEOUT]		= _T("RESET command timeout-");
	m_sErrStr[HSCH_DEV_NO_COM_FLAG]			= _T("COM-flag not set-");
	m_sErrStr[HSCH_DEV_EXCHANGE_FAILED]		= _T("IO data exchange failed-");
	m_sErrStr[HSCH_DEV_EXCHANGE_TIMEOUT]	= _T("IO data exchange timeout-");
	m_sErrStr[HSCH_DEV_COM_MODE_UNKNOWN]	= _T("IO data mode unknown-");
	m_sErrStr[HSCH_DEV_FUNCTION_FAILED]		= _T("Function call failed-");
	m_sErrStr[HSCH_DEV_DPMSIZE_MISMATCH]	= _T("DPM size differs from configuration-");
	m_sErrStr[HSCH_DEV_STATE_MODE_UNKNOWN]	= _T("State mode unknown-");
	m_sErrStr[HSCH_USR_OPEN_ERROR]			= _T("Driver cannot be opened-");
	m_sErrStr[HSCH_USR_INIT_DRV_ERROR]		= _T("Can't connect with device-");
	m_sErrStr[HSCH_USR_NOT_INITIALIZED]		= _T("Board not initialized-");
	m_sErrStr[HSCH_USR_COMM_ERR]			= _T("IOCTRL function failed-");
	m_sErrStr[HSCH_USR_DEV_NUMBER_INVALID]	= _T("Parameter DeviceNumber invalid-");
	m_sErrStr[HSCH_USR_INFO_AREA_INVALID]	= _T("Parameter InfoArea unknown-");
	m_sErrStr[HSCH_USR_NUMBER_INVALID]		= _T("Parameter Number invalid-");
	m_sErrStr[HSCH_USR_MODE_INVALID]		= _T("Parameter Mode invalid-");
	m_sErrStr[HSCH_USR_MSG_BUF_NULL_PTR]	= _T("NULL pointer assignment-");
	m_sErrStr[HSCH_USR_MSG_BUF_TOO_SHORT]	= _T("Message buffer too short-");
	m_sErrStr[HSCH_USR_SIZE_INVALID]		= _T("Parameter Size invalid-");
	m_sErrStr[HSCH_USR_SIZE_ZERO]			= _T("Parameter Size with zero length-");
	m_sErrStr[HSCH_USR_SIZE_TOO_LONG]		= _T("Parameter Size too long-");
	m_sErrStr[HSCH_USR_DEV_PTR_NULL]		= _T("Device address null pointer-");
	m_sErrStr[HSCH_USR_BUF_PTR_NULL]		= _T("Pointer to buffer is a null pointer-");
	m_sErrStr[HSCH_USR_SENDSIZE_TOO_LONG]	= _T("Parameter SendSize too long-");
	m_sErrStr[HSCH_USR_RECVSIZE_TOO_LONG]	= _T("Parameter ReceiveSize too long-");
	m_sErrStr[HSCH_USR_SENDBUF_PTR_NULL]	= _T("Pointer to send buffer is a null pointer-");
	m_sErrStr[HSCH_USR_RECVBUF_PTR_NULL]	= _T("Pointer to receive buffer is a null pointer-");
}
