/**
 * @file	EipFunc.h
 * @brief	Main Header file of the Eipfunc.dll
 * @author	Samsung Mechatronics Center
*/
#ifndef	__EIPFUNC_H__
#define	__EIPFUNC_H__

#if defined(__cplusplus)
extern	"C" {
#endif

// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the EIPLINKERDLL_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// EIPLINKERDLL_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.
#ifdef EIPFUNC_EXPORTS
#define EIPFUNC_API __declspec(dllexport)
#else
#define EIPFUNC_API __declspec(dllimport)
#endif

typedef	enum _ERequestID{
	EIPFUNC_SERVICE_START,
	EIPFUNC_SERVICE_STOP,
	EIPFUNC_SERVICE_CLOSE,
	EIPFUNC_TAG_USE,
	EIPFUNC_TAG_NOTUSE,
	EIPFUNC_GetConnectionState,
	EIPFUNC_TAG_ADD,
	EIPFUNC_TAG_REMOVE,
	EIPFUNC_TAG_MODIFY
}ERequestID;

EIPFUNC_API		short	__stdcall	EipInit();
EIPFUNC_API		short	__stdcall	EipClose();

EIPFUNC_API		short	__stdcall	EipAlive();	
EIPFUNC_API		short	__stdcall	EipReset();	

EIPFUNC_API 	short	__stdcall	ReadTagData( LPSTR TagName, short StartAddr, short DataSize, LPSTR Data );
EIPFUNC_API 	short	__stdcall	WriteTagData( LPSTR TagName, short StartAddr, short DataSize, LPSTR Data );
EIPFUNC_API 	short	__stdcall	WriteTagDataCOS( LPSTR TagName, short StartAddr, short DataSize, LPSTR Data, BOOL bCosTrigger ); 

EIPFUNC_API 	short	__stdcall	ReadIOData( LPSTR TagName, short StartAddr, short DataSize, LPSTR Data );
EIPFUNC_API 	short	__stdcall	WriteIOData( LPSTR TagName, short StartAddr, short DataSize, LPSTR Data );

EIPFUNC_API 	short	__stdcall	ReadTagBit( LPSTR TagName, short StartAddr, short BitNumber, LPSTR Data );
EIPFUNC_API 	short	__stdcall	WriteTagBit( LPSTR TagName, short StartAddr, short BitNumber, LPSTR Data );

EIPFUNC_API		short	__stdcall	RequestCommand( short nCmdId, void *pReq, void *pResult );

EIPFUNC_API		short	__stdcall	ChangeRemoteIp( short ChangeType, LPSTR ControllerName, LPSTR TargetIp );

#if defined(__cplusplus)
}
#endif

#endif
