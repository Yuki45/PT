#include "stdafx.h"
#include "AxSafety.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxSafety::CAxSafety()
{
	m_sName = _T("Safety");
	m_sErrPath = _T("\\System\\SystemError.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\System\\Safety.ini");

	m_nErrCodeAuto = -1;
	m_bEmgRobotStopAuto = FALSE;

	m_nErrCodeManual = -1;
	m_bEmgRobotStopManual = FALSE;

	m_lEnable = EM_SE_ALL;
	m_lEnableMaskAuto = EM_SE_NONE;
	m_lEnableMaskManual = EM_SE_NONE;
}

CAxSafety::~CAxSafety()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);
	DeleteAxThread(m_hPriThread);
}

void CAxSafety::Startup()
{
	CAxSystem::Startup();
}

void CAxSafety::InitProfile()
{
	CAxSystem::InitProfile();

	// input ------------------------------------------------------------------
	m_profile.AddInput(_T("ipMainAir"),		m_ipMainAir);
	m_profile.AddInput(_T("ipSubAir"),		m_ipSubAir);
	m_profile.AddInput(_T("ipEStop"),		m_ipEStop);
	m_profile.AddInput(_T("ipPower"),		m_ipPower);

	m_profile.AddInput(_T("ipArea1"),		m_ipArea1);
	m_profile.AddInput(_T("ipArea2"),		m_ipArea2);
	m_profile.AddInput(_T("ipArea3"),		m_ipArea3);
	m_profile.AddInput(_T("ipArea4"),		m_ipArea4);
	m_profile.AddInput(_T("ipArea5"),		m_ipArea5);
	m_profile.AddInput(_T("ipArea6"),		m_ipArea6);
	m_profile.AddInput(_T("ipArea7"),		m_ipArea7);
	m_profile.AddInput(_T("ipArea8"),		m_ipArea8);
	m_profile.AddInput(_T("ipArea9"),		m_ipArea9);
	m_profile.AddInput(_T("ipArea10"),		m_ipArea10);

	m_profile.AddInput(_T("ipDoor1"),		m_ipDoor1);
	m_profile.AddInput(_T("ipDoor2"),		m_ipDoor2);
	m_profile.AddInput(_T("ipDoor3"),		m_ipDoor3);
	m_profile.AddInput(_T("ipDoor4"),		m_ipDoor4);
	m_profile.AddInput(_T("ipDoor5"),		m_ipDoor5);
	m_profile.AddInput(_T("ipDoor6"),		m_ipDoor6);
	m_profile.AddInput(_T("ipDoor7"),		m_ipDoor7);
	m_profile.AddInput(_T("ipDoor8"),		m_ipDoor8);
	m_profile.AddInput(_T("ipDoor9"),		m_ipDoor9);
	m_profile.AddInput(_T("ipDoor10"),		m_ipDoor10);
	m_profile.AddInput(_T("ipDoor11"),		m_ipDoor11);
	m_profile.AddInput(_T("ipDoor12"),		m_ipDoor12);
	m_profile.AddInput(_T("ipDoor13"),		m_ipDoor13);
	m_profile.AddInput(_T("ipDoor14"),		m_ipDoor14);
	m_profile.AddInput(_T("ipDoor15"),		m_ipDoor15);
	m_profile.AddInput(_T("ipDoor16"),		m_ipDoor16);
	m_profile.AddInput(_T("ipDoor17"),		m_ipDoor17);
	m_profile.AddInput(_T("ipDoor18"),		m_ipDoor18);
	m_profile.AddInput(_T("ipDoor19"),		m_ipDoor19);
	m_profile.AddInput(_T("ipDoor20"),		m_ipDoor20);

	// output -----------------------------------------------------------------
//	m_profile.AddOutput(_T("opMainAir"),	m_opMainAir);

	// Emergency --------------------------------------------------------------
	m_profile.AddBool(_T("IsEmgPower"),		m_bIsEmgPower);
	m_profile.AddBool(_T("IsEmgEStop"),		m_bIsEmgEStop);
	m_profile.AddBool(_T("IsEmgAir"),		m_bIsEmgAir);
	m_profile.AddBool(_T("IsEmgArea"),		m_bIsEmgArea);
	m_profile.AddBool(_T("IsEmgDoor"),		m_bIsEmgDoor);
}

UINT CAxSafety::AutoRun()
{
//	TurnOn(m_opMainAir);
	
	Sleep(1000);

	while(TRUE) {
		Sleep(10); // 이거 빼면 CPU 점유율 90% 이상

		if(m_bTerminate) throw -1;
		if(m_bSimulate) continue;
//		if(m_nErrCode == -1) continue;

		GetError();

		SetError();
	}

	return 0;
}

void CAxSafety::GetError()
{
	// ------------------------------------------------------------------------
	m_nErrCodeAutoTemp = -1;
	m_bEmgRobotStopAutoTemp = FALSE;

	m_nErrCodeManualTemp = -1;
	m_bEmgRobotStopManualTemp = FALSE;

	// ------------------------------------------------------------------------
	CheckError(m_ipPower,	OFF,	EM_SE_POWER,	m_bIsEmgPower,	ERR_SE_POWER_OFF);
	CheckError(m_ipEStop,	ON,		EM_SE_ESTOP,	m_bIsEmgEStop,	ERR_SE_ESTOP_ON);
	CheckError(m_ipMainAir,	OFF,	EM_SE_AIR_MAIN,	m_bIsEmgAir,	ERR_SE_AIR_MAIN_OFF);
	CheckError(m_ipSubAir,	OFF,	EM_SE_AIR_SUB,	m_bIsEmgAir,	ERR_SE_AIR_SUB_OFF);

	CheckError(m_ipArea1,	OFF,	EM_SE_AREA_1,	m_bIsEmgArea,	ERR_SE_AREA_1_OFF);
	CheckError(m_ipArea2,	OFF,	EM_SE_AREA_2,	m_bIsEmgArea,	ERR_SE_AREA_2_OFF);
	CheckError(m_ipArea3,	OFF,	EM_SE_AREA_3,	m_bIsEmgArea,	ERR_SE_AREA_3_OFF);
	CheckError(m_ipArea4,	OFF,	EM_SE_AREA_4,	m_bIsEmgArea,	ERR_SE_AREA_4_OFF);
	CheckError(m_ipArea5,	OFF,	EM_SE_AREA_5,	m_bIsEmgArea,	ERR_SE_AREA_5_OFF);
	CheckError(m_ipArea6,	OFF,	EM_SE_AREA_6,	m_bIsEmgArea,	ERR_SE_AREA_6_OFF);
	CheckError(m_ipArea7,	OFF,	EM_SE_AREA_7,	m_bIsEmgArea,	ERR_SE_AREA_7_OFF);
	CheckError(m_ipArea8,	OFF,	EM_SE_AREA_8,	m_bIsEmgArea,	ERR_SE_AREA_8_OFF);
	CheckError(m_ipArea9,	OFF,	EM_SE_AREA_9,	m_bIsEmgArea,	ERR_SE_AREA_9_OFF);
	CheckError(m_ipArea10,	OFF,	EM_SE_AREA_10,	m_bIsEmgArea,	ERR_SE_AREA_10_OFF);

	CheckError(m_ipDoor1,	OFF,	EM_SE_DOOR_1,	m_bIsEmgDoor,	ERR_SE_DOOR_1_OFF);
	CheckError(m_ipDoor2,	OFF,	EM_SE_DOOR_2,	m_bIsEmgDoor,	ERR_SE_DOOR_2_OFF);
	CheckError(m_ipDoor3,	OFF,	EM_SE_DOOR_3,	m_bIsEmgDoor,	ERR_SE_DOOR_3_OFF);
	CheckError(m_ipDoor4,	OFF,	EM_SE_DOOR_4,	m_bIsEmgDoor,	ERR_SE_DOOR_4_OFF);
	CheckError(m_ipDoor5,	OFF,	EM_SE_DOOR_5,	m_bIsEmgDoor,	ERR_SE_DOOR_5_OFF);
	CheckError(m_ipDoor6,	OFF,	EM_SE_DOOR_6,	m_bIsEmgDoor,	ERR_SE_DOOR_6_OFF);
	CheckError(m_ipDoor7,	OFF,	EM_SE_DOOR_7,	m_bIsEmgDoor,	ERR_SE_DOOR_7_OFF);
	CheckError(m_ipDoor8,	OFF,	EM_SE_DOOR_8,	m_bIsEmgDoor,	ERR_SE_DOOR_8_OFF);
	CheckError(m_ipDoor9,	OFF,	EM_SE_DOOR_9,	m_bIsEmgDoor,	ERR_SE_DOOR_9_OFF);
	CheckError(m_ipDoor10,	OFF,	EM_SE_DOOR_10,	m_bIsEmgDoor,	ERR_SE_DOOR_10_OFF);
	CheckError(m_ipDoor11,	OFF,	EM_SE_DOOR_11,	m_bIsEmgDoor,	ERR_SE_DOOR_11_OFF);
	CheckError(m_ipDoor12,	OFF,	EM_SE_DOOR_12,	m_bIsEmgDoor,	ERR_SE_DOOR_12_OFF);
	CheckError(m_ipDoor13,	OFF,	EM_SE_DOOR_13,	m_bIsEmgDoor,	ERR_SE_DOOR_13_OFF);
	CheckError(m_ipDoor14,	OFF,	EM_SE_DOOR_14,	m_bIsEmgDoor,	ERR_SE_DOOR_14_OFF);
	CheckError(m_ipDoor15,	OFF,	EM_SE_DOOR_15,	m_bIsEmgDoor,	ERR_SE_DOOR_15_OFF);
	CheckError(m_ipDoor16,	OFF,	EM_SE_DOOR_16,	m_bIsEmgDoor,	ERR_SE_DOOR_16_OFF);
	CheckError(m_ipDoor17,	OFF,	EM_SE_DOOR_17,	m_bIsEmgDoor,	ERR_SE_DOOR_17_OFF);
	CheckError(m_ipDoor18,	OFF,	EM_SE_DOOR_18,	m_bIsEmgDoor,	ERR_SE_DOOR_18_OFF);
	CheckError(m_ipDoor19,	OFF,	EM_SE_DOOR_19,	m_bIsEmgDoor,	ERR_SE_DOOR_19_OFF);
	CheckError(m_ipDoor20,	OFF,	EM_SE_DOOR_20,	m_bIsEmgDoor,	ERR_SE_DOOR_20_OFF);

	// ------------------------------------------------------------------------
	if(m_nErrCodeAutoTemp == -1){
		m_nErrCodeAuto = -1;
		m_bEmgRobotStopAuto = FALSE;
	}
	else{
		m_nErrCodeAuto = m_nErrCodeAutoTemp;
		m_bEmgRobotStopAuto = m_bEmgRobotStopAutoTemp;
	}
	
	if(m_nErrCodeManualTemp == -1){
		m_nErrCodeManual = -1;
		m_bEmgRobotStopManual = FALSE;
	}
	else{
		m_nErrCodeManual = m_nErrCodeManualTemp;
		m_bEmgRobotStopManual = m_bEmgRobotStopManualTemp;
	}
}

void CAxSafety::CheckError(CAxInput & ipInput, BOOL bipValue, LONGLONG lEnableMask, BOOL bIsEmgency, int nErrCode)
{
	BOOL bInputValue = ipInput.GetValue();

	// check enable mask
	if( !(m_lEnable & lEnableMask) ){
		return;
	}
	
	// check auto enable mask
	if( (m_nErrCodeAutoTemp == -1) || (m_bEmgRobotStopAutoTemp == FALSE) ){
		if( (bInputValue == bipValue) && (m_lEnableMaskAuto & lEnableMask) ){
			m_nErrCodeAutoTemp = nErrCode;
			m_bEmgRobotStopAutoTemp = bIsEmgency; // TRUE or FALSE
		}
	}

	// check manual enable mask
	if( (m_nErrCodeManualTemp == -1) || (m_bEmgRobotStopManualTemp == FALSE) ){
		if( (bInputValue == bipValue) && (m_lEnableMaskManual & lEnableMask) && (bIsEmgency == TRUE) ){
			m_nErrCodeManualTemp = nErrCode;
			m_bEmgRobotStopManualTemp = bIsEmgency; // TRUE
		}
	}
}

void CAxSafety::SetError()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();
	CAxSystemError* pSystemError = (CAxSystemError*)CAxSystemHub::GetSystemHub()->GetSystem(_T("SystemError"));

	if(pSystemError == NULL)
		return;

	int nMasterStatus = pMaster->GetState();

	if( (nMasterStatus == MS_AUTO) && (m_nErrCodeAuto != -1) ){
		pSystemError->SetErrorCode(m_nErrCodeAuto);
	}
	else if( (nMasterStatus == MS_MANUAL) && (m_nErrCodeManual != -1) && (m_bEmgRobotStopManual) ){
		CAxErrData *pErrMsg = new CAxErrData;
		pErrMsg->m_nNumber = m_nErrCodeManual;
		//pErrMsg->m_nType = nBtnType;
		pErrMsg->m_sHelpFile = m_sErrPath;

		SendMessage(pMaster->GetMainWnd()->GetSafeHwnd(), pMaster->m_msgErrMessage, (WPARAM)pErrMsg, (LPARAM)0);
	}
}

void CAxSafety::SetEnable(LONGLONG lEnable, LONGLONG lDisable)
{
	m_lEnable |= lEnable;
	m_lEnable &= (~lDisable);
}

LONGLONG CAxSafety::GetEnable()
{
	return m_lEnable;
}

void CAxSafety::SetEnableMaskAuto(LONGLONG lEnable, LONGLONG lDisable)
{
	m_lEnableMaskAuto |= lEnable;
	m_lEnableMaskAuto &= (~lDisable);
}

void CAxSafety::SetEnableMaskManual(LONGLONG lEnable, LONGLONG lDisable)
{
	m_lEnableMaskManual |= lEnable;
	m_lEnableMaskManual &= (~lDisable);
}

BOOL CAxSafety::GetEmgRobotStop()
{
	if(m_bSimulate) return FALSE;

	int nMasterStatus = CAxMaster::GetMaster()->GetState();
	BOOL bEmgRobotStop = FALSE;

	if( (nMasterStatus == MS_AUTO) && (m_nErrCodeAuto != -1) ){
		bEmgRobotStop = m_bEmgRobotStopAuto;
	}
	else if( (nMasterStatus == MS_MANUAL) && (m_nErrCodeManual != -1) ){
		bEmgRobotStop = m_bEmgRobotStopManual;
	}

	return bEmgRobotStop;
}

BOOL CAxSafety::GetIsEmgPower()
{
	return m_bIsEmgPower;
}

void CAxSafety::SetIsEmgPower(BOOL bVal)
{
	m_bIsEmgPower = bVal;
}

BOOL CAxSafety::GetIsEmgEStop()
{
	return m_bIsEmgEStop;
}

void CAxSafety::SetIsEmgEStop(BOOL bVal)
{
	m_bIsEmgEStop = bVal;
}

BOOL CAxSafety::GetIsEmgAir()
{
	return m_bIsEmgAir;
}

void CAxSafety::SetIsEmgAir(BOOL bVal)
{
	m_bIsEmgAir = bVal;
}

BOOL CAxSafety::GetIsEmgArea()
{
	return m_bIsEmgArea;
}

void CAxSafety::SetIsEmgArea(BOOL bVal)
{
	m_bIsEmgArea = bVal;
}

BOOL CAxSafety::GetIsEmgDoor()
{
	return m_bIsEmgDoor;
}

void CAxSafety::SetIsEmgDoor(BOOL bVal)
{
	m_bIsEmgDoor = bVal;
}
