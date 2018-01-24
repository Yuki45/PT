#pragma once

class CMainControlStation;
class CLeftTransferStation;
class CRightTransferStation;
class CLeftPPStation;
class CRightPPStation;
class CDlgDataEdit : public CDialog
{
public:
	CDlgDataEdit(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_DATA_EDIT };

	virtual BOOL Create(CWnd* pParentWnd);

	void UpdateParam();

	CMainControlStation* m_pMainStn;
	CLeftTransferStation* m_pLeftTrStn;
	CRightTransferStation* m_pRightTrStn;
	CLeftPPStation* m_pLeftPpStn;
	CRightPPStation* m_pRightPpStn;

	int m_nSelectAxis;
	int m_nSelectShift;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnExit();
	afx_msg void OnClickBtnSelShiftA();
	afx_msg void OnClickBtnSelShiftB();
	afx_msg void OnClickBtnSelShiftC();
	afx_msg void OnClickSttWorkStart();
	afx_msg void OnClickSttRest1Start();
	afx_msg void OnClickSttRest1End();
	afx_msg void OnClickSttLunchStart();
	afx_msg void OnClickSttLunchEnd();
	afx_msg void OnClickSttRest2Start();
	afx_msg void OnClickSttRest2End();
	afx_msg void OnClickSttWorkEnd();
	afx_msg void OnClickBtnSelLeftX();
	afx_msg void OnClickBtnSelLeftY();
	afx_msg void OnClickBtnSelLeftZ();
	afx_msg void OnClickBtnSelLeftPP();
	afx_msg void OnClickBtnSelRightX();
	afx_msg void OnClickBtnSelRightY();
	afx_msg void OnClickBtnSelRightZ();
	afx_msg void OnClickBtnSelRightPP();
	afx_msg void OnClickSttVelocity();
	afx_msg void OnClickSttSlowSpd();
	afx_msg void OnClickSttSWLimitPos();
	afx_msg void OnClickSttSWLimitNeg();
	afx_msg void OnClickSttSafePosZ();
	afx_msg void OnClickSttSlowDownDist();
	afx_msg void OnClickSttZUpAfterPackIn();
	afx_msg void OnClickSttInsertCVSyncMode();
	afx_msg void OnClickSttMaxRetestCnt();
	afx_msg void OnClickSttPackOutBlockCnt();
	afx_msg void OnClickSttSameFailBlockCnt();
	afx_msg void OnClickSttMotionTimeout();
	afx_msg void OnClickSttCylinderTimeout();
	afx_msg void OnClickSttBCRTimeout();
	afx_msg void OnClickSttPackInsertDelay();
	afx_msg void OnClickSttTestTimeout();
	afx_msg void OnClickSttBarcodeLength();
	afx_msg void OnClickSttRetestName1();
	afx_msg void OnClickSttRetestName2();
	afx_msg void OnClickSttRetestName3();
	afx_msg void OnClickSttRetestName4();
	afx_msg void OnClickSttRetestName5();
	afx_msg void OnClickSttRetestName6();
	afx_msg void OnClickSttRetestName7();
	afx_msg void OnClickSttRetestName8();
	afx_msg void OnClickSttRetestName9();
	afx_msg void OnClickSttRetestName10();
	afx_msg void OnClickSttRateBlockLeastCnt();
	afx_msg void OnClickSttRateBlockPercent();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
