#ifndef __RIGHTPPSTATION_H__
#define __RIGHTPPSTATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CLoadCVStation;
class CUnloadCVStation;
class CLeftPPStation;
class CRightTransferStation;
class CMainFrame;
class CRightPPStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_JOB_CHECK,
		AS_WAIT,
		AS_LOAD_PICK_UP,
		AS_LOAD_PLACE_DOWN,
		AS_UNLOAD_PICK_UP,
		AS_UNLOAD_PLACE_DOWN
	};

	enum ManualCmd
	{
		CMD_NONE
	};

	enum ErrorCode 
	{
		ERR_NONE,
		ERR_MOVE_X,
		ERR_LOAD_UP,
		ERR_LOAD_DOWN,
		ERR_LOAD_GRIP,
		ERR_LOAD_UNGRIP,
		ERR_UNLOAD_UP,
		ERR_UNLOAD_DOWN,
		ERR_UNLOAD_GRIP,
		ERR_UNLOAD_UNGRIP,
		ERR_LOAD_EXIST,
		ERR_UNLOAD_NOT_EXIST
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CRightPPStation();
	virtual ~CRightPPStation();

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
	void AsJobCheck();
	void AsWait();
	void AsLoadPickUp();
	void AsLoadPlaceDown();
	void AsUnloadPickUp();
	void AsUnloadPlaceDown();

	void InitVariable();

	void WaitRight(DWORD dwTime);
	int ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				   LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	BOOL moveX(double dPos, BOOL bSlow = FALSE);
	BOOL moveOriginX();

	BOOL ioLoadUp();
	BOOL ioLoadDown();
	BOOL ioUnloadUp();
	BOOL ioUnloadDown();
	BOOL ioLoadGrip();
	BOOL ioLoadUngrip();
	BOOL ioUnloadGrip();
	BOOL ioUnloadUngrip();

	void _oLoadUp();
	void _oLoadDown();
	void _oUnloadUp();
	void _oUnloadDown();
	void _oLoadGrip();
	void _oLoadUngrip();
	void _oUnloadGrip();
	void _oUnloadUngrip();

	BOOL chkLoadUp();
	BOOL chkLoadDown();
	BOOL chkUnloadUp();
	BOOL chkUnloadDown();
	BOOL chkLoadGrip();
	BOOL chkUnloadGrip();
	BOOL chkLoadBuffExist();
	BOOL chkUnloadBuffExist();

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CLoadCVStation* m_pLoadCVStn;
	CUnloadCVStation* m_pUnloadCVStn;
	CLeftPPStation* m_pLeftPPStn;
	CRightTransferStation* m_pRightTRStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtEnablePosCV;
	CAxEvent m_evtEnablePosBuff;
	CAxEvent m_evtLoadCVPickComp;
	CAxEvent m_evtLoadBuffPlaceComp;
	CAxEvent m_evtUnloadCVStopReq;
	CAxEvent m_evtUnloadCVPlaceComp;

	double m_dScaleX;
	double m_dVelocityX;
	double m_dSlowSpdX;
	double m_dSWLimitPosX;
	double m_dSWLimitNegX;

	CAxRobotLoc m_locWait;
	CAxRobotLoc m_locCV;
	CAxRobotLoc m_locBuff;
};

#endif
