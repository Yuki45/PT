#ifndef __LEFTSAFETYSTATION_H__
#define __LEFTSAFETYSTATION_H__

#pragma once

#include <AxStation.h>

class CMainFrame;
class CMainControlStation;
class CLeftPPStation;
class CLeftTransferStation;
class CLeftSafetyStation : public CAxStation
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
		ERR_NONE
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CLeftSafetyStation();
	virtual ~CLeftSafetyStation();

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

	void WaitLeft(DWORD dwTime);
	int ErrorLeft(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				  LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	BOOL chkLeftDoorOpen();

	void LeftAxisStop();
	void LeftAxisEStop();
	
	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;
	CMainControlStation* m_pMainStn;
	CLeftPPStation* m_pLeftPPStn;
	CLeftTransferStation* m_pLeftTRStn;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxTrace m_trcError;
	CAxIni m_iniLot;
};

#endif
