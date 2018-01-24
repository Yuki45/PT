#ifndef __LEFTNGCVSTATION_H__
#define __LEFTNGCVSTATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CMainFrame;
class CLeftNGCVStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_RUN,
		AS_FULL
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

	CLeftNGCVStation();
	virtual ~CLeftNGCVStation();

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
	void AsFull();

	void InitVariable();

	void WaitLeft(DWORD dwTime);
	int ErrorLeft(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				  LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	void _oCVRun();
	void _oCVStop();

	BOOL chkEnterSensor();
	BOOL chkExitSensor();

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtEnableUse;

	int m_nBuzzerCnt;

	CAxTimer m_tmFull;
	CAxTimer m_tmRun;
};

#endif
