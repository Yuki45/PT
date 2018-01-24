#ifndef __RIGHTSAFETYSTATION_H__
#define __RIGHTSAFETYSTATION_H__

#pragma once

#include <AxStation.h>

class CMainFrame;
class CMainControlStation;
class CRightPPStation;
class CRightTransferStation;
class CRightSafetyStation : public CAxStation
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

	CRightSafetyStation();
	virtual ~CRightSafetyStation();

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

	void WaitRight(DWORD dwTime);
	int ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				   LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	BOOL chkRightDoorOpen();

	void RightAxisStop();
	void RightAxisEStop();
	
	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;
	CMainControlStation* m_pMainStn;
	CRightPPStation* m_pRightPPStn;
	CRightTransferStation* m_pRightTRStn;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxTrace m_trcError;
	CAxIni m_iniLot;
};

#endif
