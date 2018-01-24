#ifndef __MAINCONTROLSTATION_H__
#define __MAINCONTROLSTATION_H__

#pragma once

#include <AxStation.h>

class CMainFrame;
class CMainControlStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_RUN
	};

	enum ManualCmd 
	{
		CMD_NONE
	};

	enum ErrorCode 
	{
		ERR_NONE,
		ERR_ESTOP,
		ERR_LEFT_DOOR_OPEN,
		ERR_RIGHT_DOOR_OPEN
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CMainControlStation();
	virtual ~CMainControlStation();

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
	void AsRun();

	void InitVariable();

	void SetNormalMode();
	void SetBypassMode();
	void SetDryRunMode();
	void SetSimulateMode();

	BOOL IsNormalMode();
	BOOL IsBypassMode();
	BOOL IsDryRunMode();
	BOOL IsSimulateMode();

	BOOL chkEStopSwitch();

	void AllAxisStop();
	void AllAxisEStop();
	
	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxTrace m_trcError;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;

	int m_nStateLeft;
	int m_nStateRight;
	int m_nResponseLeft;
	int m_nResponseRight;

	BOOL m_bZUpAfterPackIn;
	BOOL m_bInsertCVSyncMode;
	BOOL m_bUseBCR_OnPassRun;

	int m_nMotionTimeout;
	int m_nCylinderTimeout;
	int m_nMaxRetestCnt;
	int m_nPackOutBlockCnt;
	int m_nSameFailBlockCnt;
	int m_nNGCVRunTime;
	int m_nNGCVFullAlarmTime;
	int m_nNGCVFullStopDelay;
	int m_nBCRTimeout;
	int m_nPackInsertDelay;
	int m_nMaxMachineRestTime;
	int m_nJigReadyTimeout;
	int m_nJigSimTestTimeout;
	int m_nJigTestTimeout;
	int m_nJigFailnameTimeout;
	int m_nBarcodeLength;
	int m_nWorkTime[WT_MAX_SHIFT][WT_MAX_LIST][WT_MAX_TYPE];
	int m_nRateBlockLeastCnt;
	int m_nAckWaitTime;

	double m_dInposition;
	double m_dSlowDownDist;
	double m_dInterlockPosZ;
	double m_dBCRRetryDist;
	double m_dRateBlockPercent;

	CString m_sRetestName[DEF_MAX_RETEST_NAME];
};

#endif
