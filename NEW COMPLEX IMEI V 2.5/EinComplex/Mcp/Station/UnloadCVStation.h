#ifndef __UNLOADCVSTATION_H__
#define __UNLOADCVSTATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CLeftPPStation;
class CRightPPStation;
class CMainFrame;
class CUnloadCVStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_WAIT_SET
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

	CUnloadCVStation();
	virtual ~CUnloadCVStation();

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
	void AsWaitSet();

	void InitVariable();

	void _oCVRun();
	void _oCVStop();

	BOOL chkEnterSensor();
	BOOL chkFullSensor();
	BOOL chkExitSensor();

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CLeftPPStation* m_pLeftPPStn;
	CRightPPStation* m_pRightPPStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtEnableUseCV;
	CAxEvent m_evtCVStopComp;

	BOOL m_bFirstSetUnloadComp;

	int m_nCVRunTime;
	int m_nCVStopDelay;
	int m_nStandardTACT;

	long m_lTACT;

	CAxTimer m_tmRun;
	CAxTimer m_tmTACT;
};

#endif
