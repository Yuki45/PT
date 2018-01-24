#ifndef __AX_SAFETY_H__
#define __AX_SAFETY_H__

#pragma once

#include "AxSystem.h"

#define	EM_SE_NONE		0x0000000000000000

#define	EM_SE_POWER		0x0001000000000000
#define	EM_SE_ESTOP		0x0002000000000000
#define	EM_SE_AIR_MAIN	0x0004000000000000
#define	EM_SE_AIR_SUB	0x0008000000000000

#define	EM_SE_AREA_1	0x0000000100000000
#define	EM_SE_AREA_2	0x0000000200000000
#define	EM_SE_AREA_3	0x0000000400000000
#define	EM_SE_AREA_4	0x0000000800000000
#define	EM_SE_AREA_5	0x0000001000000000
#define	EM_SE_AREA_6	0x0000002000000000
#define	EM_SE_AREA_7	0x0000004000000000
#define	EM_SE_AREA_8	0x0000008000000000
#define	EM_SE_AREA_9	0x0000010000000000
#define	EM_SE_AREA_10	0x0000020000000000
#define	EM_SE_AREA_ALL	0x0000FFFF00000000

#define	EM_SE_DOOR_1	0x0000000000000001
#define	EM_SE_DOOR_2	0x0000000000000002
#define	EM_SE_DOOR_3	0x0000000000000004
#define	EM_SE_DOOR_4	0x0000000000000008
#define	EM_SE_DOOR_5	0x0000000000000010
#define	EM_SE_DOOR_6	0x0000000000000020
#define	EM_SE_DOOR_7	0x0000000000000040
#define	EM_SE_DOOR_8	0x0000000000000080
#define	EM_SE_DOOR_9	0x0000000000000100
#define	EM_SE_DOOR_10	0x0000000000000200
#define	EM_SE_DOOR_11	0x0000000000000400
#define	EM_SE_DOOR_12	0x0000000000000800
#define	EM_SE_DOOR_13	0x0000000000001000
#define	EM_SE_DOOR_14	0x0000000000002000
#define	EM_SE_DOOR_15	0x0000000000004000
#define	EM_SE_DOOR_16	0x0000000000008000
#define	EM_SE_DOOR_17	0x0000000000100000
#define	EM_SE_DOOR_18	0x0000000000200000
#define	EM_SE_DOOR_19	0x0000000000400000
#define	EM_SE_DOOR_20	0x0000000000800000
#define	EM_SE_DOOR_ALL	0x00000000FFFFFFFF

#define	EM_SE_ALL		0xFFFFFFFFFFFFFFFF

class __declspec(dllexport) CAxSafety : public CAxSystem 
{
public:
	CAxSafety();
	virtual ~CAxSafety();

	// System Function --------------------------------------------------------
	void Startup();
	void InitProfile();
	UINT AutoRun();
	
	// User Function ----------------------------------------------------------
	void		SetEnable(LONGLONG lEnable, LONGLONG lDisable);
	LONGLONG	GetEnable();
	void		SetEnableMaskAuto(LONGLONG lEnable, LONGLONG lDisable);
	void		SetEnableMaskManual(LONGLONG lEnable, LONGLONG lDisable);
	void		GetError();
	void		CheckError(CAxInput & ipSafety, BOOL bipValue, LONGLONG lEnableMask, BOOL bIsEmgency, int nErrCode);
	void		SetError();
	BOOL		GetEmgRobotStop();
	BOOL		GetIsEmgPower();
	void		SetIsEmgPower(BOOL bVal);
	BOOL		GetIsEmgEStop();
	void		SetIsEmgEStop(BOOL bVal);
	BOOL		GetIsEmgAir();
	void		SetIsEmgAir(BOOL bVal);
	BOOL		GetIsEmgArea();
	void		SetIsEmgArea(BOOL bVal);
	BOOL		GetIsEmgDoor();
	void		SetIsEmgDoor(BOOL bVal);

	// Input ------------------------------------------------------------------
	CAxInput m_ipPower;
	CAxInput m_ipEStop;
	CAxInput m_ipMainAir;
	CAxInput m_ipSubAir;

	CAxInput m_ipArea1;
	CAxInput m_ipArea2;
	CAxInput m_ipArea3;
	CAxInput m_ipArea4;
	CAxInput m_ipArea5;
	CAxInput m_ipArea6;
	CAxInput m_ipArea7;
	CAxInput m_ipArea8;
	CAxInput m_ipArea9;
	CAxInput m_ipArea10;

	CAxInput m_ipDoor1;
	CAxInput m_ipDoor2;
	CAxInput m_ipDoor3;
	CAxInput m_ipDoor4;
	CAxInput m_ipDoor5;
	CAxInput m_ipDoor6;
	CAxInput m_ipDoor7;
	CAxInput m_ipDoor8;
	CAxInput m_ipDoor9;
	CAxInput m_ipDoor10;
	CAxInput m_ipDoor11;
	CAxInput m_ipDoor12;
	CAxInput m_ipDoor13;
	CAxInput m_ipDoor14;
	CAxInput m_ipDoor15;
	CAxInput m_ipDoor16;
	CAxInput m_ipDoor17;
	CAxInput m_ipDoor18;
	CAxInput m_ipDoor19;
	CAxInput m_ipDoor20;

	// Output -----------------------------------------------------------------

	// Emergency Stop Setting -------------------------------------------------
	// TRUE  : 긴급정지
	// FALSE : 진행동작 완료 후 정지
	// ------------------------------------------------------------------------
	BOOL m_bIsEmgPower;
	BOOL m_bIsEmgEStop;
	BOOL m_bIsEmgAir;
	BOOL m_bIsEmgArea;
	BOOL m_bIsEmgDoor;

	// ------------------------------------------------------------------------
	LONGLONG m_lEnable;
	LONGLONG m_lEnableMaskAuto;
	LONGLONG m_lEnableMaskManual;

	int m_nErrCodeAuto;
	int m_nErrCodeManual;
	int m_nErrCodeAutoTemp;
	int m_nErrCodeManualTemp;

	BOOL m_bEmgRobotStopAuto;
	BOOL m_bEmgRobotStopManual;
	BOOL m_bEmgRobotStopAutoTemp;
	BOOL m_bEmgRobotStopManualTemp;
};

#endif