#include "MOTION_DEFINE.h"

#pragma once

//------------------------------------------------------------------
//                 Device Type Defines.
//------------------------------------------------------------------
// Driver Type B group : only Motion Controller (without Drive) family
#ifndef DEVTYPE_EZI_MOTIONLINK
#define	DEVTYPE_EZI_MOTIONLINK				10
#define DEVNAME_EZI_MOTIONLINK				"Ezi-MotionLink"
#endif

//------------------------------------------------------------------
//                 Axis Status Flag Defines.
//------------------------------------------------------------------

typedef union
{
	DWORD	dwValue;
	struct
	{
		unsigned	FFLAG_ERRSERVOALARM		: 1; // = 0x00000001;
		unsigned	FFLAG_HWPOSILMT			: 1; // = 0x00000002;
		unsigned	FFLAG_HWNEGALMT			: 1; // = 0x00000004;
		unsigned	FFLAG_SWPOGILMT			: 1; // = 0x00000008;
		unsigned	FFLAG_SWNEGALMT			: 1; // = 0x00000010;
		unsigned	FFLAG_RESERVED0			: 1; // = 0x00000020;
		unsigned	FFLAG_RESERVED1			: 1; // = 0x00000040;
		unsigned	FFLAG_RESERVED2			: 1; // = 0x00000080;
		unsigned	FFLAG_RESERVED3			: 1; // = 0x00000100;
		unsigned	FFLAG_RESERVED4			: 1; // = 0x00000200;
		unsigned	FFLAG_RESERVED5			: 1; // = 0x00000400;
		unsigned	FFLAG_RESERVED6			: 1; // = 0x00000800;
		unsigned	FFLAG_RESERVED7			: 1; // = 0x00001000;
		unsigned	FFLAG_RESERVED8			: 1; // = 0x00002000;
		unsigned	FFLAG_RESERVED9			: 1; // = 0x00004000;
		unsigned	FFLAG_RESERVED10		: 1; // = 0x00008000;
		unsigned	FFLAG_EMGSTOP			: 1; // = 0x00010000;
		unsigned	FFLAG_SLOWSTOP			: 1; // = 0x00020000;
		unsigned	FFLAG_ORIGINRETURNING	: 1; // = 0x00040000;
		unsigned	FFLAG_INPOSITION		: 1; // = 0x00080000;
		unsigned	FFLAG_SERVOON			: 1; // = 0x00100000;
		unsigned	FFLAG_ALARMRESET		: 1; // = 0x00200000;
		unsigned	FFLAG_PTSTOPPED			: 1; // = 0x00400000;
		unsigned	FFLAG_ORIGINSENSOR		: 1; // = 0x00800000;
		unsigned	FFLAG_ZPULSE			: 1; // = 0x01000000;
		unsigned	FFLAG_ORIGINRETOK		: 1; // = 0x02000000;
		unsigned	FFLAG_MOTIONDIR			: 1; // = 0x04000000;
		unsigned	FFLAG_MOTIONING			: 1; // = 0x08000000;
		unsigned	FFLAG_MOTIONPAUSE		: 1; // = 0x10000000;
		unsigned	FFLAG_MOTIONACCEL		: 1; // = 0x20000000;
		unsigned	FFLAG_MOTIONDECEL		: 1; // = 0x40000000;
		unsigned	FFLAG_MOTIONCONST		: 1; // = 0x80000000;
	};
} EZIMOTIONLINK_AXISSTATUS;

//------------------------------------------------------------------
//                 Input/Output Defines.
//------------------------------------------------------------------

// Input Bit-mask list.
#define	MOTIONLINK_IN_BITMASK_LIMITP		(0x00000001)
#define	MOTIONLINK_IN_BITMASK_LIMITN		(0x00000002)
#define	MOTIONLINK_IN_BITMASK_ORIGIN		(0x00000004)
#define	MOTIONLINK_IN_BITMASK_CLEARPOSITION	(0x00000008)
#define	MOTIONLINK_IN_BITMASK_PTA0			(0x00000010)
#define	MOTIONLINK_IN_BITMASK_PTA1			(0x00000020)
#define	MOTIONLINK_IN_BITMASK_PTA2			(0x00000040)
#define	MOTIONLINK_IN_BITMASK_PTA3			(0x00000080)
#define	MOTIONLINK_IN_BITMASK_PTA4			(0x00000100)
#define	MOTIONLINK_IN_BITMASK_PTA5			(0x00000200)
//#define	MOTIONLINK_IN_BITMASK_RESERVED	(0x00000400)
//#define	MOTIONLINK_IN_BITMASK_RESERVED	(0x00000800)
#define	MOTIONLINK_IN_BITMASK_PTSTART		(0x00001000)
#define	MOTIONLINK_IN_BITMASK_STOP			(0x00002000)
#define	MOTIONLINK_IN_BITMASK_PJOG			(0x00004000)
#define	MOTIONLINK_IN_BITMASK_NJOG			(0x00008000)
#define	MOTIONLINK_IN_BITMASK_ALARMRESET	(0x00010000)
#define	MOTIONLINK_IN_BITMASK_SERVOON		(0x00020000)
#define	MOTIONLINK_IN_BITMASK_PAUSE			(0x00040000)
#define	MOTIONLINK_IN_BITMASK_ORIGINSEARCH	(0x00080000)
#define	MOTIONLINK_IN_BITMASK_TEACHING		(0x00100000)
#define	MOTIONLINK_IN_BITMASK_ESTOP			(0x00200000)
#define	MOTIONLINK_IN_BITMASK_JPTIN0		(0x00400000)
#define	MOTIONLINK_IN_BITMASK_JPTIN1		(0x00800000)
#define	MOTIONLINK_IN_BITMASK_JPTIN2		(0x01000000)
#define	MOTIONLINK_IN_BITMASK_JPTSTART		(0x02000000)
#define	MOTIONLINK_IN_BITMASK_USERIN0		(0x04000000)
#define	MOTIONLINK_IN_BITMASK_USERIN1		(0x08000000)
#define	MOTIONLINK_IN_BITMASK_USERIN2		(0x10000000)
#define	MOTIONLINK_IN_BITMASK_USERIN3		(0x20000000)
#define	MOTIONLINK_IN_BITMASK_USERIN4		(0x40000000)
#define	MOTIONLINK_IN_BITMASK_USERIN5		(0x80000000)

// Output Bit-mask list.
#define	MOTIONLINK_OUT_BITMASK_COMPAREOUT	(0x00000001)
#define	MOTIONLINK_OUT_BITMASK_INPOSITION	(0x00000002)
#define	MOTIONLINK_OUT_BITMASK_ALARM		(0x00000004)
#define	MOTIONLINK_OUT_BITMASK_MOVING		(0x00000008)
#define	MOTIONLINK_OUT_BITMASK_ACCDEC		(0x00000010)
#define	MOTIONLINK_OUT_BITMASK_ACK			(0x00000020)
#define	MOTIONLINK_OUT_BITMASK_END			(0x00000040)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00000080)
#define	MOTIONLINK_OUT_BITMASK_ORGSEARCHOK	(0x00000100)
#define	MOTIONLINK_OUT_BITMASK_SERVOREADY	(0x00000200)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00000400)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00000800)
#define	MOTIONLINK_OUT_BITMASK_PTOUT0		(0x00001000)
#define	MOTIONLINK_OUT_BITMASK_PTOUT1		(0x00002000)
#define	MOTIONLINK_OUT_BITMASK_PTOUT2		(0x00004000)
#define	MOTIONLINK_OUT_BITMASK_USEROUT0		(0x00008000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00010000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00020000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00040000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00080000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00100000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00200000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00400000)
//#define	MOTIONLINK_OUT_BITMASK_RESERVED	(0x00800000)

#pragma pack(1)

typedef union
{
	DWORD	dwValue;
	struct
	{
		unsigned BIT_LIMITP		: 1;
		unsigned BIT_LIMITN		: 1;
		unsigned BIT_ORIGIN		: 1;
		
		unsigned BIT_CLEARPOSITION	: 1;
		unsigned BIT_PTA0			: 1;
		unsigned BIT_PTA1			: 1;
		unsigned BIT_PTA2			: 1;
		unsigned BIT_PTA3			: 1;
		unsigned BIT_PTA4			: 1;
		unsigned BIT_PTA5			: 1;
		unsigned BIT_PTA6			: 1;
		unsigned BIT_PTA7			: 1;
		unsigned BIT_PTSTART		: 1;
		unsigned BIT_STOP			: 1;
		unsigned BIT_PJOG			: 1;
		unsigned BIT_NJOG			: 1;
		unsigned BIT_ALARMRESET		: 1;
		unsigned BIT_SERVOON		: 1;
		unsigned BIT_PAUSE			: 1;
		unsigned BIT_ORIGINSEARCH	: 1;
		unsigned BIT_TEACHING		: 1;
		unsigned BIT_ESTOP			: 1;
		unsigned BIT_JPTIN0			: 1;
		unsigned BIT_JPTIN1			: 1;
		unsigned BIT_JPTIN2			: 1;
		unsigned BIT_JPTSTART		: 1;
		unsigned BIT_USERIN0		: 1;
		unsigned BIT_USERIN1		: 1;
		unsigned BIT_USERIN2		: 1;
		unsigned BIT_USERIN3		: 1;
		unsigned BIT_USERIN4		: 1;
		unsigned BIT_USERIN5		: 1;
	};
} EZIMOTIONLINK_INLOGIC;

typedef union
{
	DWORD dwValue;
	struct
	{
		unsigned BIT_COMPAREOUT		: 1;
		unsigned BIT_INPOSITION		: 1;
		unsigned BIT_ALARM			: 1;
		unsigned BIT_MOVING			: 1;
		unsigned BIT_ACCDEC			: 1;
		unsigned BIT_ACK			: 1;
		unsigned BIT_END			: 1;
		unsigned BIT_RESERVED0		: 1;
		unsigned BIT_ORGSEARCHOK	: 1;
		unsigned BIT_SERVOREADY		: 1;
		unsigned BIT_RESERVED1		: 1;
		unsigned BIT_RESERVED2		: 1;
		unsigned BIT_PTOUT0			: 1;
		unsigned BIT_PTOUT1			: 1;
		unsigned BIT_PTOUT2			: 1;
		unsigned BIT_USEROUT0		: 1;
		unsigned BIT_RESERVED3		: 1;
		unsigned BIT_RESERVED4		: 1;
		unsigned BIT_RESERVED5		: 1;
		unsigned BIT_RESERVED6		: 1;
		unsigned BIT_RESERVED7		: 1;
		unsigned BIT_RESERVED8		: 1;
		unsigned BIT_RESERVED9		: 1;
		unsigned BIT_RESERVED10		: 1;
	};
} EZIMOTIONLINK_OUTLOGIC;

#pragma pack()

//------------------------------------------------------------------
//                 Input/Output Assigning Defines.
//------------------------------------------------------------------
typedef enum
{
	MOTIONLINK_IN_PREASSIGN_LIMITP = 1,
	MOTIONLINK_IN_PREASSIGN_LIMITN,
	MOTIONLINK_IN_PREASSIGN_ORIGIN,

	MOTIONLINK_IN_LOGIC_CLEARPOSITION,
	MOTIONLINK_IN_LOGIC_PTA0,
	MOTIONLINK_IN_LOGIC_PTA1,
	MOTIONLINK_IN_LOGIC_PTA2,
	MOTIONLINK_IN_LOGIC_PTA3,
	MOTIONLINK_IN_LOGIC_PTA4,
	MOTIONLINK_IN_LOGIC_PTA5,
	MOTIONLINK_IN_LOGIC_RESERVED0,
	MOTIONLINK_IN_LOGIC_RESERVED1,
	MOTIONLINK_IN_LOGIC_PTSTART,
	MOTIONLINK_IN_LOGIC_STOP,
	MOTIONLINK_IN_LOGIC_PJOG,
	MOTIONLINK_IN_LOGIC_NJOG,
	MOTIONLINK_IN_LOGIC_ALARMRESET,
	MOTIONLINK_IN_LOGIC_SERVOON,
	MOTIONLINK_IN_LOGIC_PAUSE,
	MOTIONLINK_IN_LOGIC_ORIGINSEARCH,
	MOTIONLINK_IN_LOGIC_TEACHING,
	MOTIONLINK_IN_LOGIC_ESTOP,
	MOTIONLINK_IN_LOGIC_JPTIN0,
	MOTIONLINK_IN_LOGIC_JPTIN1,
	MOTIONLINK_IN_LOGIC_JPTIN2,
	MOTIONLINK_IN_LOGIC_JPTSTART,
	MOTIONLINK_IN_LOGIC_USERIN0,
	MOTIONLINK_IN_LOGIC_USERIN1,
	MOTIONLINK_IN_LOGIC_USERIN2,
	MOTIONLINK_IN_LOGIC_USERIN3,
	MOTIONLINK_IN_LOGIC_USERIN4,
	MOTIONLINK_IN_LOGIC_USERIN5,

	MAX_MOTIONLINK_IN_LOGIC
} EZIMOTIONLINK_INLOGIC_LIST;

typedef enum
{
	MOTIONLINK_OUT_PREASSIGN_COMP = 1,

	MOTIONLINK_OUT_LOGIC_INPOSITION,
	MOTIONLINK_OUT_LOGIC_ALARM,
	MOTIONLINK_OUT_LOGIC_MOVING,
	MOTIONLINK_OUT_LOGIC_ACCDEC,
	MOTIONLINK_OUT_LOGIC_ACK,
	MOTIONLINK_OUT_LOGIC_END,
	MOTIONLINK_OUT_LOGIC_RESERVED0,
	MOTIONLINK_OUT_LOGIC_ORGSEARCHOK,
	MOTIONLINK_OUT_LOGIC_SERVOREADY,
	MOTIONLINK_OUT_LOGIC_RESERVED1,
	MOTIONLINK_OUT_LOGIC_RESERVED2,
	MOTIONLINK_OUT_LOGIC_PTOUT0,
	MOTIONLINK_OUT_LOGIC_PTOUT1,
	MOTIONLINK_OUT_LOGIC_PTOUT2,
	MOTIONLINK_OUT_LOGIC_USEROUT0,
	MOTIONLINK_OUT_LOGIC_RESERVED3,
	MOTIONLINK_OUT_LOGIC_RESERVED4,
	MOTIONLINK_OUT_LOGIC_RESERVED5,
	MOTIONLINK_OUT_LOGIC_RESERVED6,
	MOTIONLINK_OUT_LOGIC_RESERVED7,
	MOTIONLINK_OUT_LOGIC_RESERVED8,
	MOTIONLINK_OUT_LOGIC_RESERVED9,
	MOTIONLINK_OUT_LOGIC_RESERVED10,

	MAX_MOTIONLINK_OUT_LOGIC
} EZIMOTIONLINK_OUTLOGIC_LIST;

#define	IN_LOGIC_CNT		(MAX_MOTIONLINK_IN_LOGIC - 1)
#define	MAX_PREASSIGNED_IN	(MOTIONLINK_IN_PREASSIGN_ORIGIN)

#define	OUT_LOGIC_CNT		(MAX_MOTIONLINK_OUT_LOGIC - 1)
#define	MAX_PREASSIGNED_OUT	(MOTIONLINK_OUT_PREASSIGN_COMP)

static const LPCSTR LOGICLIST_EZIMOTIONLINK_INPUT[IN_LOGIC_CNT] = 
{
	"Limit +",
	"Limit -",
	"Origin",
	"Clear Pos",
	"PT A0",
	"PT A1",
	"PT A2",
	"PT A3",
	"PT A4",
	"PT A5",
	"Reserved",
	"Reserved",
	"PT Start",
	"Stop",
	"Jog +",
	"Jog -",
	"Alarm Reset",
	"Servo On",
	"Pause",
	"Origin Search",
	"Teaching",
	"E-Stop",
	"JPT IN 0",
	"JPT IN 1",
	"JPT IN 2",
	"JPT Start",
	"User IN 0",
	"User IN 1",
	"User IN 2",
	"User IN 3",
	"User IN 4",
	"User IN 5"
};

static const LPCSTR LOGICLIST_EZIMOTIONLINK_OUTPUT[OUT_LOGIC_CNT] = 
{
	"Comp. Out",
	"Inposition",
	"Alarm",
	"Moving",
	"Acc/Dec",
	"ACK",
	"END",
	"Reserved",
	"Org Search Ok",
	"Servo Ready",
	"Reserved",
	"Reserved",
	"PT OUT 0",
	"PT OUT 1",
	"PT OUT 2",
	"User OUT 0",
	"Reserved",
	"Reserved",
	"Reserved",
	"Reserved",
	"Reserved",
	"Reserved",
	"Reserved",
	"Reserved"
};

//------------------------------------------------------------------
//                 Parameters Defines.
//------------------------------------------------------------------

typedef enum
{
	MOTIONLINK_ENCODERMULTIPLY = 0,
	MOTIONLINK_AXISMAXSPEED,
	MOTIONLINK_AXISSTARTSPEED,
	MOTIONLINK_AXISACCTIME,
	MOTIONLINK_AXISDECTIME,
	
	MOTIONLINK_SPEEDOVERRIDE,
	MOTIONLINK_JOGHIGHSPEED,
	MOTIONLINK_JOGLOWSPEED,
	MOTIONLINK_JOGACCDECTIME,
	
	MOTIONLINK_EXTSERVOALARMLOGIC,
	MOTIONLINK_EXTSERVOONLOGIC,
	MOTIONLINK_EXTSERVORESETLOGIC,
	
	MOTIONLINK_SWLMTPLUSVALUE,
	MOTIONLINK_SWLMTMINUSVALUE,
	MOTIONLINK_SOFTLMTSTOPMETHOD,
	MOTIONLINK_HARDLMTSTOPMETHOD,
	MOTIONLINK_LIMITSENSORLOGIC,

	MOTIONLINK_ORGSPEED,
	MOTIONLINK_ORGSEARCHSPEED,
	MOTIONLINK_ORGACCDECTIME,
	MOTIONLINK_ORGMETHOD,
	MOTIONLINK_ORGDIR,
	MOTIONLINK_ORGOFFSET,
	MOTIONLINK_ORGPOSITIONSET,
	MOTIONLINK_ORGSENSORLOGIC,

	MOTIONLINK_LIMITSENSORDIR,
	MOTIONLINK_PULSETYPE,
	MOTIONLINK_ENCODERDIR,
	MOTIONLINK_MOTIONDIR,

	MOTIONLINK_SERVOALARMRESETLOGIC,
	MOTIONLINK_SERVOONOUTPUTLOGIC,
	MOTIONLINK_SERVOALARMLOGIC,

	MOTIONLINK_SERVOINPOSLOGIC,

	MOTIONLINK_SPARE2,
	MOTIONLINK_SPARE3,
	MOTIONLINK_SPARE4,
	MOTIONLINK_SPARE5,

	MOTIONLINK_MOTORLEAD,
	MOTIONLINK_GEARRATIO,

	MAX_MOTIONLINK_PARAM

} FM_EZIMOTIONLINK_PARAM;