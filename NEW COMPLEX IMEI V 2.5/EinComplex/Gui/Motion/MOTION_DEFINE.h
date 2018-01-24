#pragma once

//------------------------------------------------------------------
//                 Device Type Defines.
//------------------------------------------------------------------
// Driver Type A group : Ezi-SERVO Plus-R Inc. Family
#ifndef DEVTYPE_EZI_SERVO_PLUS_R_ST
#define	DEVTYPE_EZI_SERVO_PLUS_R_ST			1
#define DEVNAME_EZI_SERVO_PLUS_R_ST			"Ezi-SERVO Plus-R Inc."
#endif

// Driver Type B group : only Motion Controller (without Drive) family
#ifndef DEVTYPE_EZI_MOTIONLINK
#define	DEVTYPE_EZI_MOTIONLINK				10
#define DEVNAME_EZI_MOTIONLINK				"Ezi-MotionLink"
#endif

// Driver Type C group : Ezi-STEP Plus-R Family
#ifndef DEVTYPE_EZI_STEP_PLUS_R_ST
#define	DEVTYPE_EZI_STEP_PLUS_R_ST			20
#define DEVNAME_EZI_STEP_PLUS_R_ST			"Ezi-STEP Plus-R"
#endif

// Driver Type D group : Ezi-SERVO Plus-R Abs. Family
#ifndef DEVTYPE_EZI_SERVO_PLUS_R_ABS_ST
#define	DEVTYPE_EZI_SERVO_PLUS_R_ABS_ST		30
#define DEVNAME_EZI_SERVO_PLUS_R_ABS_ST		"Ezi-SERVO Plus-R Abs."
#endif

// Driver Type E group : Ezi-SERVO Plus-R BLDC Family
#ifndef DEVTYPE_EZI_SERVO_PLUS_R_BLDC_ST
#define	DEVTYPE_EZI_SERVO_PLUS_R_BLDC_ST	40
#define DEVNAME_EZI_SERVO_PLUS_R_BLDC_ST	"Ezi-SERVO Plus-R BLDC F60"
#endif

//------------------------------------------------------------------
//                 Drive Information Defines.
//------------------------------------------------------------------
#pragma pack(1)

typedef struct _DRIVE_INFO
{
	unsigned short nVersionNo[4];	// Drive Version Number (Major Ver/Minor Ver/Bug fix/Build No.) (?)
	char sVersion[30];				// Drive Version string
	
	unsigned short nDriveType;		// Drive Model
	unsigned short nMotorType;		// Motor Model
	char sMotorInfo[20];			// Motor Info.(?)
	
	unsigned short nInPinNo;		// Input Pin Number
	unsigned short nOutPinNo;		// Output Pin Number
	
	unsigned short nPTNum;			// Position Table Item Number

	unsigned short nFirmwareType;	// Firmware Type Information
} DRIVE_INFO;

#pragma pack()

//------------------------------------------------------------------
//                 Axis Status Flag Defines.
//------------------------------------------------------------------

#define	MAX_AXIS_STATUS		(32)

//------------------------------------------------------------------
//                 Input/Output Assigning Defines.
//------------------------------------------------------------------
#define	LEVEL_LOW_ACTIVE		(0)
#define	LEVEL_HIGH_ACTIVE		(1)

#define	IN_LOGIC_NONE			(0)
#define	OUT_LOGIC_NONE			(0)

//------------------------------------------------------------------
//                 POSITION TABLE Defines.
//------------------------------------------------------------------

static const WORD	MAX_LOOP_COUNT		= 100;
static const WORD	MAX_WAIT_TIME		= 60000;

// Command list.
typedef enum
{
	CMD_ABS_LOWSPEED = 0,
	CMD_ABS_HIGHSPEED,
	CMD_ABS_HIGHSPEEDDECEL,
	CMD_ABS_NORMALMOTION,
	CMD_INC_LOWSPEED,
	CMD_INC_HIGHSPEED,
	CMD_INC_HIGHSPEEDDECEL,
	CMD_INC_NORMALMOTION,
	CMD_MOVE_ORIGIN,
	CMD_COUNTERCLEAR,
	CMD_MAX_COUNT
} COMMAND_LIST;

#pragma pack(2)

// Pos. Table Item structure.
typedef union
{
	WORD	wBuffer[32];		// 64 bytes

	struct
	{
		LONG	lPosition;
		
		DWORD	dwStartSpd;
		DWORD	dwMoveSpd;
		
		WORD	wAccelRate;
		WORD	wDecelRate;
		
		WORD	wCommand;
		WORD	wWaitTime;
		WORD	wContinuous;
		WORD	wBranch;
		
		WORD	wCond_branch0;
		WORD	wCond_branch1;
		WORD	wCond_branch2;
		WORD	wLoopCount;
		WORD	wBranchAfterLoop;
		WORD	wPTSet;
		WORD	wLoopCountCLR;

		WORD	bCheckInpos;		// 0 : Check Inpos, 1 : Don't Check.
	};
} ITEM_NODE, *LPITEM_NODE;

#pragma pack()

//------------------------------------------------------------------
//                 Motion Direction Defines.
//------------------------------------------------------------------

#define DIR_INC		(1)
#define DIR_DEC		(0)

//------------------------------------------------------------------
//                 EX Commands Option Defines.
//------------------------------------------------------------------
#pragma pack(1)

typedef union
{
	BYTE	byBuffer[32];

	struct
	{
		union
		{
			DWORD dwOptionFlag;
			struct
			{
				unsigned BIT_IGNOREEXSTOP	: 1;
				
				unsigned BIT_USE_CUSTOMACCEL	: 1;
				unsigned BIT_USE_CUSTOMDECEL	: 1;
			};
		} flagOption;

		WORD	wCustomAccelTime;
		WORD	wCustomDecelTime;
	};
} MOTION_OPTION_EX;

typedef union
{
	BYTE	byBuffer[32];

	struct
	{
		union
		{
			DWORD dwOptionFlag;
			struct
			{
				unsigned BIT_IGNOREEXSTOP	: 1;
				unsigned BIT_USE_CUSTOMACCDEC	: 1;
			};
		} flagOption;

		WORD	wCustomAccDecTime;
	};
} VELOCITY_OPTION_EX;

#pragma pack()

//------------------------------------------------------------------
//                 Alarm Type Defines.
//------------------------------------------------------------------
typedef enum
{
	ALARM_NONE = 0,

	/*  1 */ ALARM_OVERCURRENT,
	/*  2 */ ALARM_OVERSPEED,
	/*  3 */ ALARM_STEPOUT,
	/*  4 */ ALARM_OVERLOAD,
	/*  5 */ ALARM_OVERTEMPERATURE,
	/*  6 */ ALARM_OVERBACKEMF,
	/*  7 */ ALARM_MOTORCONNECT,
	/*  8 */ ALARM_ENCODERCONNECT,
	/*  9 */ ALARM_LOWMOTORPOWER,
	/* 10 */ ALARM_INPOSITION,
	/* 11 */ ALARM_SYSTEMHALT,
	/* 12 */ ALARM_ROMDEVICE,
	/* 13 */ ALARM_RESERVED0,
	/* 14 */ ALARM_HIGHINPUTVOLTAGE,
	/* 15 */ ALARM_POSITIONOVERFLOW,
	/* 16 */ ALARM_POSITIONCHANGED,

	MAX_ALARM_NUM
} ALARM_TYPE;

static const char* ALARM_DESCRIPTION[MAX_ALARM_NUM] = 
{
	NULL,
	/*  1 */ "Over Current",
	/*  2 */ "Over Speed",
	/*  3 */ "Position Tracking",
	/*  4 */ "Over Load",
	/*  5 */ "Over Temperature",
	/*  6 */ "Over Back EMF",
	/*  7 */ "No Motor Connect",
	/*  8 */ "No Encoder Connect",
	/*  9 */ "Low Motor Power",
	/* 10 */ "Inposition Error",
	/* 11 */ "System Halt",
	/* 12 */ "ROM Device Error",
	/* 13 */ NULL,
	/* 14 */ "High Input Voltage",
	/* 15 */ "Position Overflow",
	/* 16 */ "Position Changed"
};
