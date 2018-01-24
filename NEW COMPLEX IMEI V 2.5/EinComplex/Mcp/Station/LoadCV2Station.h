#ifndef __LOADCV2STATION_H__
#define __LOADCV2STATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CLoadCVStation;
class CMainFrame;
class CLoadCV2Station : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_WAIT_SET_INPUT,
		AS_WAIT_SET_READY,
		AS_WAIT_LOAD_CV,
		AS_WAIT_MIDDLE_ON,
		AS_WAIT_MIDDLE_OFF
	};

	enum ManualCmd
	{
		CMD_NONE
	};

	enum ErrorCode 
	{
		ERR_NONE,
		ERR_STOPPER_UP,
		ERR_STOPPER_DOWN
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CLoadCV2Station();
	virtual ~CLoadCV2Station();

	void InitProfile();
	void LoadProfile();
	void SaveProfile();

	void InitRecipe();
	void LoadRecipe();
	void SaveRecipe();
	
	void Startup();
	void Setup();

	void PostAbort();
	void PostStop(UINT nMode);
	void PreStart();

	UINT AutoRun();
	UINT ManualRun();

	void AsInit();
	void AsWaitSetInput();
	void AsWaitSetReady();
	void AsWaitLoadCV();
	void AsWaitMiddleOn();
	void AsWaitMiddleOff();

	void InitVariable();

	BOOL ioStopperUp();
	BOOL ioStopperDown();

	void _oStopperUp();
	void _oStopperDown();
	void _oCVRun();
	void _oCVStop();

	BOOL chkStopperDown();
	BOOL chkEnterSensor();
	BOOL chkExitSensor();

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CLoadCVStation* m_pLoadCVStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtLoadComp;

	CAxTimer m_tmCVRun;
};

#endif
