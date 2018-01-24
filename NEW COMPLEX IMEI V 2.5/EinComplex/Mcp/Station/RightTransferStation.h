#ifndef __RIGHTTRANSFERSTATION_H__
#define __RIGHTTRANSFERSTATION_H__

#pragma once

#include <AxStation.h>

class CMainControlStation;
class CRightPPStation;
class CRightNGCVStation;
class CMainFrame;
class CRightTransferStation : public CAxStation
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_JOB_CHECK,
		AS_WAIT,
		AS_LOAD_PICK_UP,
		AS_READ_BARCODE,
		AS_JIG_PLACE_DOWN,
		AS_JIG_PICK_UP,
		AS_UNLOAD_PLACE_DOWN,
		AS_NG_PLACE_DOWN
	};

	enum ManualCmd
	{
		CMD_NONE
	};

	enum ErrorCode 
	{
		ERR_NONE,
		ERR_MOVE_XY,
		ERR_MOVE_Z,
		ERR_GRIP,
		ERR_UNGRIP,
		ERR_LOAD_NOT_EXIST,
		ERR_UNLOAD_EXIST,
		ERR_DOUBLE_PLACE
	};

	enum MessgeCode 
	{
		MSG_NONE
	};

	CRightTransferStation();
	virtual ~CRightTransferStation();

	void InitProfile();
	void LoadProfile();
	void SaveProfile();
	void SaveProfileJigSystem();

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
	void AsReadBarcode();
	void AsJigPlaceDown();
	void AsJigPickUp();
	void AsUnloadPlaceDown();
	void AsNGPlaceDown();

	void InitVariable();

	void WaitRight(DWORD dwTime);
	int ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
				   LPCTSTR pszParam1 = NULL, LPCTSTR pszParam2 = NULL, LPCTSTR pszParam3 = NULL, LPCTSTR pszParam4 = NULL);

	BOOL moveX(double dPos, BOOL bSlow = FALSE);
	BOOL moveY(double dPos, BOOL bSlow = FALSE);
	BOOL moveZ(double dPos, BOOL bSlow = FALSE);
	BOOL moveXY(double dPosX, double dPosY, BOOL bSlow = FALSE);
	BOOL moveOriginX();
	BOOL moveOriginY();
	BOOL moveOriginZ();
	BOOL moveOriginXY();

	BOOL ioGrip();
	BOOL ioUngrip();

	void _oGrip();
	void _oUngrip();
	void _oPackIn(int nIndex);
	void _oPackOut(int nIndex);

	BOOL chkGrip();
	BOOL chkDoublePlace();
	BOOL chkLoadableJig();
	BOOL chkUnloadableJig();
	BOOL chkOnlyLoadable();

	int GetPCNumber(int nIndex);
	int GetSlotNumber(int nIndex);
	BOOL GetJigExist(int nIndex);
	int GetJigUse(int nIndex);
	int GetJigStatus(int nIndex);
	int GetJigResult(int nIndex);
	int GetJigRetestCnt(int nIndex);
	CString GetJigBarcode(int nIndex);
	CString GetJigFailName(int nIndex);
	long GetJigTestTime(int nIndex);
	double GetJigAvgTestTime(int nIndex);
	int GetJigPassCnt(int nIndex);
	int GetJigFailCnt(int nIndex);
	int GetJigRetestPassCnt(int nIndex);
	int GetJigRetestFailCnt(int nIndex);
	int GetJigPackOutCnt(int nIndex);
	int GetJigSameFailCnt(int nIndex);
	BOOL GetJigRateBlocked(int nIndex);

	void SetJigExist(int nIndex, BOOL bExist);
	void SetJigUse(int nIndex, int nUse);
	void SetJigStatus(int nIndex, int nStatus);
	void SetJigResult(int nIndex, int nResult);
	void SetJigRetestCnt(int nIndex, int nRetestCnt);
	void SetJigBarcode(int nIndex, CString sBarcode);
	void SetJigFailName(int nIndex, CString sFailName);
	void SetJigInit(int nIndex);
	void SetJigPassCnt(int nIndex, int nCount);
	void SetJigFailCnt(int nIndex, int nCount);
	void SetJigRetestPassCnt(int nIndex, int nCount);
	void SetJigRetestFailCnt(int nIndex, int nCount);
	void SetJigPackOutCnt(int nIndex, int nCount);
	void SetJigSameFailCnt(int nIndex, int nCount);
	void SetJigRateBlocked(int nIndex, BOOL bBlocked);

	BOOL IsRetestSet(int nIndex);

	CAxMaster* m_pMaster;
	CMainControlStation* m_pMainStn;
	CRightPPStation* m_pRightPPStn;
	CRightNGCVStation* m_pRightNGCVStn;
	CMainFrame* m_pMainFrm;
	CString m_sMsgPath;
	CAxTrace m_Trace;
	CAxIni m_iniLot;

	CAxEvent m_evtInitStart;
	CAxEvent m_evtInitComp;
	CAxEvent m_evtEnablePosBuff;
	CAxEvent m_evtEnableUseLoadBuff;
	CAxEvent m_evtEnableUseUnloadBuff;
	CAxEvent m_evtUnloadComp;

	BOOL m_bDoubleOut;

	int m_nJigNo;
	int m_nTransferResult;
	int m_nTransferRetestCnt;

	CString m_sTransferBarcode;
	CString m_sTransferFailName;

	double m_dScaleX;
	double m_dScaleY;
	double m_dScaleZ;
	double m_dVelocityX;
	double m_dVelocityY;
	double m_dVelocityZ;
	double m_dSlowSpdX;
	double m_dSlowSpdY;
	double m_dSlowSpdZ;
	double m_dSWLimitPosX;
	double m_dSWLimitPosY;
	double m_dSWLimitPosZ;
	double m_dSWLimitNegX;
	double m_dSWLimitNegY;
	double m_dSWLimitNegZ;

	CAxRobotLoc m_locLoad;
	CAxRobotLoc m_locUnload;
	CAxRobotLoc m_locBCR;
	CAxRobotLoc m_locNG;
	CAxRobotLoc m_locJig[DEF_MAX_JIG_ONE_SIDE];
};

#endif
