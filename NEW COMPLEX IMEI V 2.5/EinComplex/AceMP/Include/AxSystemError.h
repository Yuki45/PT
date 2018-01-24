#ifndef __AX_SYSTEMERROR_H__
#define __AX_SYSTEMERROR_H__

#pragma once

#include "AxSystem.h"

enum SystemError {
	ERR_SE_POWER_OFF		= 1,
	ERR_SE_ESTOP_ON,
	ERR_SE_AIR_MAIN_OFF,
	ERR_SE_AIR_SUB_OFF,

	ERR_SE_AREA_1_OFF		= 21,
	ERR_SE_AREA_2_OFF,
	ERR_SE_AREA_3_OFF,
	ERR_SE_AREA_4_OFF,
	ERR_SE_AREA_5_OFF,
	ERR_SE_AREA_6_OFF,
	ERR_SE_AREA_7_OFF,
	ERR_SE_AREA_8_OFF,
	ERR_SE_AREA_9_OFF,
	ERR_SE_AREA_10_OFF,

	ERR_SE_DOOR_1_OFF		= 41,
	ERR_SE_DOOR_2_OFF,
	ERR_SE_DOOR_3_OFF,
	ERR_SE_DOOR_4_OFF,
	ERR_SE_DOOR_5_OFF,
	ERR_SE_DOOR_6_OFF,
	ERR_SE_DOOR_7_OFF,
	ERR_SE_DOOR_8_OFF,
	ERR_SE_DOOR_9_OFF,
	ERR_SE_DOOR_10_OFF,
	ERR_SE_DOOR_11_OFF,
	ERR_SE_DOOR_12_OFF,
	ERR_SE_DOOR_13_OFF,
	ERR_SE_DOOR_14_OFF,
	ERR_SE_DOOR_15_OFF,
	ERR_SE_DOOR_16_OFF,
	ERR_SE_DOOR_17_OFF,
	ERR_SE_DOOR_18_OFF,
	ERR_SE_DOOR_19_OFF,
	ERR_SE_DOOR_20_OFF
};

class __declspec(dllexport) CAxSystemError : public CAxSystem 
{
public:
	CAxSystemError();
	virtual ~CAxSystemError();

	void Startup();
	void InitProfile();
	UINT AutoRun();
	void SetError();
	void SetErrorCode(int nErrCode);

	void SetEmgRobotStop(BOOL bEmgRobotStop);
	BOOL GetEmgRobotStop();

	BOOL m_bEmgRobotStop;
};

#endif