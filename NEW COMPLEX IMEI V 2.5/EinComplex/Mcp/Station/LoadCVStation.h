#ifndef __LOADCVSTATION_H__
#define __LOADCVSTATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CLoadCV2Station;
class CLeftPPStation;
class CRightPPStation;
class CMainFrame;
class CLoadCVStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_CHECK_SET,
		AS_WAIT_EVENT,
		AS_WAIT_SET,
		AS_WAIT_LOAD_DELAY,
		AS_WAIT_PICK_UP
	};

	enum ManualCmd
	{
		CMD_NONE
	};

	enum ErrorCode 
	{
		ERR_NONE
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CLoadCVStation();
	virtual ~CLoadCVStation();

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
	void AsCheckSet();
	void AsWaitEvent();
	void AsWaitSet();
	void AsWaitLoadDelay();
	void AsWaitPickUp();

	void InitVariable();

	void _oCVRun();
	void _oCVStop();

	BOOL chkExitSensor();

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CLoadCV2Station* m_pLoadCV2Stn;
	CLeftPPStation* m_pLeftPPStn;
	CRightPPStation* m_pRightPPStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtLoadReq;
	CAxEvent m_evtLoadComp;

	CAxTimer m_tmCVRun;
};

#endif
