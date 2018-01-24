#pragma once

//------------------------------------------------------------------
//                 FRAME TYPE Defines.
//------------------------------------------------------------------
#define	FRAME_GETSLAVEINFO				(0x01)

#define	FRAME_GETENCODER				(0x06)

#define	FRAME_SETIDVALUE				(0x08)

// Step Commands
#define	FRAME_STEPGETCURRENT			(0x0B)		// Factory, Distributor only
#define	FRAME_STEPSETCURRENT			(0x0C)		// Factory, Distributor only
#define	FRAME_STEPVERSION				(0x0D)		// Factory, Distributor only
#define	FRAME_STEPGETMOTORDB			(0x0E)		// Factory, Distributor only
#define	FRAME_STEPSETMOTORDB			(0x0F)		// Factory, Distributor only

// Parameters Commands
#define	FRAME_FAS_SAVEALLPARAM			(0x10)
#define	FRAME_FAS_GETROMPARAM			(0x11)
#define	FRAME_FAS_SETPARAMETER			(0x12)
#define	FRAME_FAS_GETPARAMETER			(0x13)

// ROM Writing Commands
#define	FRAME_ISROMERASED				(0x1A)
#define	FRAME_ISRUNROMEXIST				(0x1B)
#define	FRAME_ERASEFLASH				(0x1C)
#define	FRAME_WRITEFLASH				(0x1D)
#define	FRAME_REBOOTBOOTROM				(0x1E)
#define	FRAME_REBOOTRUNROM				(0x1F)

// I/O Commands
#define	FRAME_FAS_SETIO_OUTPUT			(0x20)
#define	FRAME_FAS_SETIO_INPUT			(0x21)
#define	FRAME_FAS_GETIO_INPUT			(0x22)
#define	FRAME_FAS_GETIO_OUTPUT			(0x23)

#define	FRAME_FAS_SET_INPUT_ASSGN_MAP	(0x24)
#define	FRAME_FAS_SET_OUTPUT_ASSGN_MAP	(0x25)
#define	FRAME_FAS_IO_ASSGN_MAP_READROM	(0x26)
#define	FRAME_FAS_GET_INPUT_ASSGN_MAP	(0x27)
#define	FRAME_FAS_GET_OUTPUT_ASSGN_MAP	(0x28)

#define	FRAME_FAS_SERVOENABLE			(0x2A)
#define	FRAME_FAS_ALARMRESET			(0x2B)
#define	FRAME_FAS_STEPALARMRESET		(0x2C)	// Step Alarm Reset function.

#define FRAME_FAS_GETALARMTYPE			(0x2E)	// Alarm Type

// Motion Commands
#define	FRAME_FAS_MOVESTOP				(0x31)
#define	FRAME_FAS_EMERGENCYSTOP			(0x32)

#define	FRAME_FAS_MOVEORIGIN			(0x33)
#define	FRAME_FAS_MOVESINGLEABS			(0x34)
#define	FRAME_FAS_MOVESINGLEINC			(0x35)
#define	FRAME_FAS_MOVETOLIMIT			(0x36)
#define	FRAME_FAS_MOVEVELOCITY			(0x37)

#define	FRAME_FAS_POSABSOVERRIDE		(0x38)
#define	FRAME_FAS_POSINCOVERRIDE		(0x39)
#define	FRAME_FAS_VELOVERRIDE			(0x3A)

#define	FRAME_FAS_ALLMOVESTOP			(0x3B)
#define	FRAME_FAS_ALLEMERGENCYSTOP		(0x3C)
#define	FRAME_FAS_ALLMOVEORIGIN			(0x3D)
#define	FRAME_FAS_ALLMOVESINGLEABS		(0x3E)
#define	FRAME_FAS_ALLMOVESINGLEINC		(0x3F)

// Motion Status Commands
#define	FRAME_FAS_GETAXISSTATUS			(0x40)
#define	FRAME_FAS_GETIOSTATUS			(0x41)
#define	FRAME_FAS_GETMOTIONSTATUS		(0x42)
#define	FRAME_FAS_GETALLSTATUS			(0x43)

#define	FRAME_FAS_GETRUNPTSTATUS		(0x44)

#define	FRAME_FAS_SETCMDPOS				(0x50)
#define	FRAME_FAS_GETCMDPOS				(0x51)
#define	FRAME_FAS_SETACTPOS				(0x52)
#define	FRAME_FAS_GETACTPOS				(0x53)
#define	FRAME_FAS_GETPOSERR				(0x54)
#define	FRAME_FAS_GETACTVEL				(0x55)
#define	FRAME_FAS_CLEARPOS				(0x56)

#define	FRAME_FAS_MOVEPAUSE				(0x58)

// Position Table specific commands.
#define	FRAME_FAS_POSTAB_READ_ITEM		(0x60)
#define	FRAME_FAS_POSTAB_WRITE_ITEM		(0x61)
#define	FRAME_FAS_POSTAB_READ_ROM		(0x62)
#define	FRAME_FAS_POSTAB_WRITE_ROM		(0x63)
#define	FRAME_FAS_POSTAB_RUN_ITEM		(0x64)
#define	FRAME_FAS_POSTAB_IS_DATA		(0x65)

#define	FRAME_FAS_POSTAB_RUN_ONEITEM	(0x68)
#define	FRAME_FAS_POSTAB_CHECK_STOPMODE	(0x69)

// Hidden Parameter commands.
#define	FRAME_FAS_GET_HIDDEN_PARAM		(0x66)
#define	FRAME_FAS_SET_HIDDEN_PARAM		(0x67)

// Position Table specific commands. (2nd)
#define	FRAME_FAS_POSTAB_READ_ONEITEM	(0x6A)
#define	FRAME_FAS_POSTAB_WRITE_ONEITEM	(0x6B)

// Linear Motion commands.
#define	FRAME_FAS_SETLINEARINFO			(0x70)
#define	FRAME_FAS_MOVELINEARINC			(0x71)

// Ex-Motion Commands
#define	FRAME_FAS_MOVESINGLEABS_EX		(0x80)
#define	FRAME_FAS_MOVESINGLEINC_EX		(0x81)
#define	FRAME_FAS_MOVEVELOCITY_EX		(0x82)

// Calibration Commands.
#define	FRAME_FAS_STARTCALIBRATION		(0x90)
#define	FRAME_FAS_GETCALIBRATIONRESULT	(0x91)
#define	FRAME_FAS_SETCALIBRATIONDATA	(0x92)

// BLDC Drive Commands.
#define	FRAME_FAS_BLDC_READ_PARAMETER	(0x84)
#define	FRAME_FAS_BLDC_WRITE_PARAMETER	(0x85)
#define	FRAME_FAS_BLDC_READ_ROM			(0x86)
#define	FRAME_FAS_BLDC_WRITE_ROM		(0x87)

// BLDC Tuning Commands.
#define	FRAME_MONITORING_CHECKFINISH	(0x8A)
#define	FRAME_MONITORING_CANCEL			(0x8B)
#define	FRAME_MONITORING_READDATA		(0x8C)
#define	FRAME_MONITORING_SETCONFIG		(0x8D)
#define	FRAME_MONITORING_GETCONFIG		(0x8E)
#define	FRAME_MONITORING_START			(0x8F)

// Driver Functions
#define	FRAME_DRIVER_GETVERSION			(0xB0)
#define	FRAME_DRIVER_GETPARAM			(0xB1)
#define	FRAME_DRIVER_SETPARAM			(0xB2)
#define	FRAME_DRIVER_FUNCTION			(0xB3)
