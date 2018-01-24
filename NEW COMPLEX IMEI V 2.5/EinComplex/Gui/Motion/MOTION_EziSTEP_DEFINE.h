#include "MOTION_DEFINE.h"

#pragma once

//------------------------------------------------------------------
//                 Device Type Defines.
//------------------------------------------------------------------
// Driver Type C group : Ezi-STEP Plus-R Family
#ifndef DEVTYPE_EZI_STEP_PLUS_R_ST
#define	DEVTYPE_EZI_STEP_PLUS_R_ST			20
#define DEVNAME_EZI_STEP_PLUS_R_ST			"Ezi-STEP Plus-R"
#endif

//------------------------------------------------------------------
//                 Axis Status Flag Defines.
//------------------------------------------------------------------

typedef union
{
	DWORD	dwValue;
	struct
	{
		unsigned	FFLAG_ERRORALL			: 1; // = 0x00000001;
		unsigned	FFLAG_HWPOSILMT			: 1; // = 0x00000002;
		unsigned	FFLAG_HWNEGALMT			: 1; // = 0x00000004;
		unsigned	FFLAG_SWPOGILMT			: 1; // = 0x00000008;
		unsigned	FFLAG_SWNEGALMT			: 1; // = 0x00000010;
		unsigned	FFLAG_RESERVED0			: 1; // = 0x00000020;
		unsigned	FFLAG_RESERVED1			: 1; // = 0x00000040;
		unsigned	FFLAG_ERRSTEPALARM		: 1; // = 0x00000080;
		unsigned	FFLAG_ERROVERCURRENT	: 1; // = 0x00000100;
		unsigned	FFLAG_ERROVERSPEED		: 1; // = 0x00000200;
		unsigned	FFLAG_ERRSTEPOUT		: 1; // = 0x00000400;
		unsigned	FFLAG_RESERVED2			: 1; // = 0x00000800;
		unsigned	FFLAG_ERROVERHEAT		: 1; // = 0x00001000;
		unsigned	FFLAG_ERRBACKEMF		: 1; // = 0x00002000;
		unsigned	FFLAG_ERRMOTORPOWER		: 1; // = 0x00004000;
		unsigned	FFLAG_ERRLOWPOWER		: 1; // = 0x00008000;
		unsigned	FFLAG_EMGSTOP			: 1; // = 0x00010000;
		unsigned	FFLAG_SLOWSTOP			: 1; // = 0x00020000;
		unsigned	FFLAG_ORIGINRETURNING	: 1; // = 0x00040000;
		unsigned	FFLAG_RESERVED3			: 1; // = 0x00080000;
		unsigned	FFLAG_RESERVED4			: 1; // = 0x00100000;
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
} EZISTEP_AXISSTATUS;

//------------------------------------------------------------------
//                 Input/Output Defines.
//------------------------------------------------------------------

// Input Bit-mask list.
#define	STEP_IN_BITMASK_LIMITP				(0x00000001)
#define	STEP_IN_BITMASK_LIMITN				(0x00000002)
#define	STEP_IN_BITMASK_ORIGIN				(0x00000004)
#define	STEP_IN_BITMASK_CLEARPOSITION		(0x00000008)
#define	STEP_IN_BITMASK_PTA0				(0x00000010)
#define	STEP_IN_BITMASK_PTA1				(0x00000020)
#define	STEP_IN_BITMASK_PTA2				(0x00000040)
#define	STEP_IN_BITMASK_PTA3				(0x00000080)
#define	STEP_IN_BITMASK_PTA4				(0x00000100)
#define	STEP_IN_BITMASK_PTA5				(0x00000200)
#define	STEP_IN_BITMASK_PTA6				(0x00000400)
#define	STEP_IN_BITMASK_PTA7				(0x00000800)
#define	STEP_IN_BITMASK_PTSTART				(0x00001000)
#define	STEP_IN_BITMASK_STOP				(0x00002000)
#define	STEP_IN_BITMASK_PJOG				(0x00004000)
#define	STEP_IN_BITMASK_NJOG				(0x00008000)
#define	STEP_IN_BITMASK_ALARMRESET			(0x00010000)
//#define	STEP_IN_BITMASK_RESERVED		(0x00020000)
#define	STEP_IN_BITMASK_PAUSE				(0x00040000)
#define	STEP_IN_BITMASK_ORIGINSEARCH		(0x00080000)
#define	STEP_IN_BITMASK_TEACHING			(0x00100000)
#define	STEP_IN_BITMASK_ESTOP				(0x00200000)
#define	STEP_IN_BITMASK_JPTIN0				(0x00400000)
#define	STEP_IN_BITMASK_JPTIN1				(0x00800000)
#define	STEP_IN_BITMASK_JPTIN2				(0x01000000)
#define	STEP_IN_BITMASK_JPTSTART			(0x02000000)
#define	STEP_IN_BITMASK_USERIN0				(0x04000000)
#define	STEP_IN_BITMASK_USERIN1				(0x08000000)
#define	STEP_IN_BITMASK_USERIN2				(0x10000000)
#define	STEP_IN_BITMASK_USERIN3				(0x20000000)
#define	STEP_IN_BITMASK_USERIN4				(0x40000000)
#define	STEP_IN_BITMASK_USERIN5				(0x80000000)
#define	STEP_IN_BITMASK_USERIN6				(0x100000000)
#define	STEP_IN_BITMASK_USERIN7				(0x200000000)
#define	STEP_IN_BITMASK_USERIN8				(0x400000000)

// Output Bit-mask list.
#define	STEP_OUT_BITMASK_COMPAREOUT			(0x00000001)
//#define	STEP_OUT_BITMASK_RESERVED		(0x00000002)
#define	STEP_OUT_BITMASK_ALARM				(0x00000004)
#define	STEP_OUT_BITMASK_RUNSTOP			(0x00000008)
#define	STEP_OUT_BITMASK_ACCDEC				(0x00000010)
#define	STEP_OUT_BITMASK_PTACK				(0x00000020)
#define	STEP_OUT_BITMASK_PTEND				(0x00000040)
#define	STEP_OUT_BITMASK_ALARMBLINK			(0x00000080)
#define	STEP_OUT_BITMASK_ORGSEARCHOK		(0x00000100)
//#define	STEP_OUT_BITMASK_RESERVED		(0x00000200)
//#define	STEP_OUT_BITMASK_RESERVED		(0x00000400)
#define	STEP_OUT_BITMASK_BRAKE				(0x00000800)
#define	STEP_OUT_BITMASK_PTOUT0				(0x00001000)
#define	STEP_OUT_BITMASK_PTOUT1				(0x00002000)
#define	STEP_OUT_BITMASK_PTOUT2				(0x00004000)
#define	STEP_OUT_BITMASK_USEROUT0			(0x00008000)
#define	STEP_OUT_BITMASK_USEROUT1			(0x00010000)
#define	STEP_OUT_BITMASK_USEROUT2			(0x00020000)
#define	STEP_OUT_BITMASK_USEROUT3			(0x00040000)
#define	STEP_OUT_BITMASK_USEROUT4			(0x00080000)
#define	STEP_OUT_BITMASK_USEROUT5			(0x00100000)
#define	STEP_OUT_BITMASK_USEROUT6			(0x00200000)
#define	STEP_OUT_BITMASK_USEROUT7			(0x00400000)
#define	STEP_OUT_BITMASK_USEROUT8			(0x00800000)

typedef union
{
	ULONGLONG uValue;
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
		unsigned BIT_RESERVED0		: 1;
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
		unsigned BIT_USERIN6		: 1;
		unsigned BIT_USERIN7		: 1;
		unsigned BIT_USERIN8		: 1;
	};
} EZISTEP_INPUT;

typedef union
{
	DWORD dwValue;
	struct
	{
		unsigned BIT_COMPAREOUT		: 1;
		unsigned BIT_RESERVED0		: 1;
		unsigned BIT_ALARM			: 1;
		unsigned BIT_RUNSTOP		: 1;
		unsigned BIT_ACCDEC			: 1;
		unsigned BIT_PTACK			: 1;
		unsigned BIT_PTEND			: 1;
		unsigned BIT_ALARMBLINK		: 1;
		unsigned BIT_ORGSEARCHOK	: 1;
		unsigned BIT_RESERVED1		: 1;
		unsigned BIT_RESERVED2		: 1;
		unsigned BIT_BRAKE			: 1;
		unsigned BIT_PTOUT0			: 1;
		unsigned BIT_PTOUT1			: 1;
		unsigned BIT_PTOUT2			: 1;
		unsigned BIT_USEROUT0		: 1;
		unsigned BIT_USEROUT1		: 1;
		unsigned BIT_USEROUT2		: 1;
		unsigned BIT_USEROUT3		: 1;
		unsigned BIT_USEROUT4		: 1;
		unsigned BIT_USEROUT5		: 1;
		unsigned BIT_USEROUT6		: 1;
		unsigned BIT_USEROUT7		: 1;
		unsigned BIT_USEROUT8		: 1;
	};
} EZISTEP_OUTPUT;

//------------------------------------------------------------------
//                 Input/Output Assigning Defines.
//------------------------------------------------------------------
typedef enum
{
	STEP_IN_PREASSIGN_LIMITP = 1,
	STEP_IN_PREASSIGN_LIMITN,
	STEP_IN_PREASSIGN_ORIGIN,

	STEP_IN_LOGIC_CLEARPOSITION,
	STEP_IN_LOGIC_PTA0,
	STEP_IN_LOGIC_PTA1,
	STEP_IN_LOGIC_PTA2,
	STEP_IN_LOGIC_PTA3,
	STEP_IN_LOGIC_PTA4,
	STEP_IN_LOGIC_PTA5,
	STEP_IN_LOGIC_PTA6,
	STEP_IN_LOGIC_PTA7,
	STEP_IN_LOGIC_PTSTART,
	STEP_IN_LOGIC_STOP,
	STEP_IN_LOGIC_PJOG,
	STEP_IN_LOGIC_NJOG,
	STEP_IN_LOGIC_ALARMRESET,
	STEP_IN_LOGIC_RESERVED0,
	STEP_IN_LOGIC_PAUSE,
	STEP_IN_LOGIC_ORIGINSEARCH,
	STEP_IN_LOGIC_TEACHING,
	STEP_IN_LOGIC_ESTOP,
	STEP_IN_LOGIC_JPTIN0,
	STEP_IN_LOGIC_JPTIN1,
	STEP_IN_LOGIC_JPTIN2,
	STEP_IN_LOGIC_JPTSTART,
	STEP_IN_LOGIC_USERIN0,
	STEP_IN_LOGIC_USERIN1,
	STEP_IN_LOGIC_USERIN2,
	STEP_IN_LOGIC_USERIN3,
	STEP_IN_LOGIC_USERIN4,
	STEP_IN_LOGIC_USERIN5,
	STEP_IN_LOGIC_USERIN6,
	STEP_IN_LOGIC_USERIN7,
	STEP_IN_LOGIC_USERIN8,

	MAX_STEP_IN_LOGIC
} EZISTEP_INLOGIC;

typedef enum
{
	STEP_OUT_PREASSIGN_COMPAREOUT = 1,

	STEP_OUT_LOGIC_RESERVED0,
	STEP_OUT_LOGIC_ALARM,
	STEP_OUT_LOGIC_RUNSTOP,
	STEP_OUT_LOGIC_ACCDEC,
	STEP_OUT_LOGIC_PTACK,
	STEP_OUT_LOGIC_PTEND,
	STEP_OUT_LOGIC_ALARMBLINK,
	STEP_OUT_LOGIC_ORGSEARCHOK,
	STEP_OUT_LOGIC_RESERVED1,
	STEP_OUT_LOGIC_RESERVED2,
	STEP_OUT_LOGIC_BRAKE,
	STEP_OUT_LOGIC_PTOUT0,
	STEP_OUT_LOGIC_PTOUT1,
	STEP_OUT_LOGIC_PTOUT2,
	STEP_OUT_LOGIC_USEROUT0,
	STEP_OUT_LOGIC_USEROUT1,
	STEP_OUT_LOGIC_USEROUT2,
	STEP_OUT_LOGIC_USEROUT3,
	STEP_OUT_LOGIC_USEROUT4,
	STEP_OUT_LOGIC_USEROUT5,
	STEP_OUT_LOGIC_USEROUT6,
	STEP_OUT_LOGIC_USEROUT7,
	STEP_OUT_LOGIC_USEROUT8,

	MAX_STEP_OUT_LOGIC
} EZISTEP_OUTLOGIC;

#define	IN_LOGIC_CNT		(MAX_STEP_IN_LOGIC - 1)
#define	MAX_PREASSIGNED_IN	(STEP_IN_PREASSIGN_ORIGIN)

#define	OUT_LOGIC_CNT		(MAX_STEP_OUT_LOGIC - 1)
#define	MAX_PREASSIGNED_OUT (STEP_OUT_PREASSIGN_COMPAREOUT)

static const LPCSTR LOGICLIST_EZISTEP_INPUT[IN_LOGIC_CNT] = 
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
	"PT A6",
	"PT A7",
	"PT Start",
	"Stop",
	"Jog +",
	"Jog -",
	"Alarm Reset",
	"Reserved",
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
	"User IN 5",
	"User IN 6",
	"User IN 7",
	"User IN 8"
};

static const LPCSTR LOGICLIST_EZISTEP_OUTPUT[OUT_LOGIC_CNT] = 
{
	"Comp. Out",
	"Reserved",
	"Alarm",
	"Run/Stop",
	"Acc/Dec",
	"PT ACK",
	"PT End",
	"AlarmBlink",
	"Org Search Ok",
	"Reserved",
	"Reserved",
	"Brake",
	"PT OUT 0",
	"PT OUT 1",
	"PT OUT 2",
	"User OUT 0",
	"User OUT 1",
	"User OUT 2",
	"User OUT 3",
	"User OUT 4",
	"User OUT 5",
	"User OUT 6",
	"User OUT 7",
	"User OUT 8"
};

//------------------------------------------------------------------
//                 Parameters Defines.
//------------------------------------------------------------------

typedef enum
{
	STEP_PULSEPERREVOLUTION = 0,
	STEP_AXISMAXSPEED,
	STEP_AXISSTARTSPEED,
	STEP_AXISACCTIME,
	STEP_AXISDECTIME,
	
	STEP_SPEEDOVERRIDE,
	STEP_JOGHIGHSPEED,
	STEP_JOGLOWSPEED,
	STEP_JOGACCDECTIME,
	
	STEP_ALARMLOGIC,
	STEP_RUNSTOPSIGNALLOGIC,		//SERVO_SERVOONLOGIC,
	STEP_RESETLOGIC,
	
	STEP_SWLMTPLUSVALUE,
	STEP_SWLMTMINUSVALUE,
	STEP_SOFTLMTSTOPMETHOD,
	STEP_HARDLMTSTOPMETHOD,
	STEP_LIMITSENSORLOGIC,

	STEP_ORGSPEED,
	STEP_ORGSEARCHSPEED,
	STEP_ORGACCDECTIME,
	STEP_ORGMETHOD,
	STEP_ORGDIR,
	STEP_ORGOFFSET,
	STEP_ORGPOSITIONSET,
	STEP_ORGSENSORLOGIC,

	STEP_STOPCURRENT,
	STEP_MOTIONDIR,

	STEP_LIMITSENSORDIR,
	STEP_ENCODERMULTIVALUE,

	STEP_MOTORLEAD,
	STEP_GEARRATIO,

	MAX_STEP_PARAM

} FM_EZISTEP_PARAM;
