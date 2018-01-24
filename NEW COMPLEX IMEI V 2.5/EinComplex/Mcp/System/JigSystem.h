#ifndef __JIGSYSTEM_H__
#define __JIGSYSTEM_H__

#pragma once

#include <AxSystem.h>
#include <AxMaster.h>

class CCommTester;
class CMainFrame;
class CJigSystem : public CAxSystem
{
public:
	enum AutoState 
	{
		AS_INIT,
		AS_CHECK_READY,
		AS_WAIT_START,
		AS_WAIT_ACK,
		AS_WAIT_RESULT,
		AS_WAIT_FAIL_NAME
	};

	CJigSystem(int nSysIdx);
	virtual ~CJigSystem();

	void InitProfile();
	void Startup();
	UINT AutoRun();

	void AsInit();
	void AsCheckReady();
	void AsWaitStart();
	void AsWaitAck();
	void AsWaitResult();
	void AsWaitFailName();

	void _oPackIn(int nIndex);
	void _oPackOut(int nIndex);

	void CalculateAvgTestTime();

	int m_nSystemIndex;

	BOOL m_bWaitReady[DEF_MAX_TESTER_IN_PC];
	BOOL m_bEnableStart[DEF_MAX_TESTER_IN_PC];
	BOOL m_bEnableUnload[DEF_MAX_TESTER_IN_PC];
	int m_nCurrTester;
	UINT m_nAutoState[DEF_MAX_TESTER_IN_PC];
	long m_lTestTime[DEF_MAX_TESTER_IN_PC];
	double m_dAvgTestTime[DEF_MAX_TESTER_IN_PC];

	CAxMaster* m_pMaster;
	CMainFrame* m_pMainFrm;
	CCommTester* m_pTester;

	CAxTimer m_tmReady[DEF_MAX_TESTER_IN_PC];
	CAxTimer m_tmAck[DEF_MAX_TESTER_IN_PC];
	CAxTimer m_tmResult[DEF_MAX_TESTER_IN_PC];
	CAxTimer m_tmLabel[DEF_MAX_TESTER_IN_PC];

	BOOL m_bJigExist[DEF_MAX_TESTER_IN_PC];
	BOOL m_bJigRateBlocked[DEF_MAX_TESTER_IN_PC];
	int m_nJigUse[DEF_MAX_TESTER_IN_PC];
	int m_nJigStatus[DEF_MAX_TESTER_IN_PC];
	int m_nJigResult[DEF_MAX_TESTER_IN_PC];
	int m_nJigRetestCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigPassCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigFailCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigRetestPassCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigRetestFailCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigPackOutCnt[DEF_MAX_TESTER_IN_PC];
	int m_nJigSameFailCnt[DEF_MAX_TESTER_IN_PC];
	CString m_sJigBarcode[DEF_MAX_TESTER_IN_PC];
	CString m_sJigFailName[DEF_MAX_TESTER_IN_PC];
	CString m_sPrevFailName[DEF_MAX_TESTER_IN_PC];
};

#endif
