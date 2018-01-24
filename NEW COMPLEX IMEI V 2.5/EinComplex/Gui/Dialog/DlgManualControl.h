#pragma once

class CLeftTransferStation;
class CRightTransferStation;
class CDlgManualControl : public CDialog
{
public:
	CDlgManualControl(CWnd* pParent = NULL);

#ifdef DEF_EIN_48_LCA
	enum { IDD = IDD_DLG_MANUAL_CONTROL48 };
#else
	enum { IDD = IDD_DLG_MANUAL_CONTROL56 };
#endif

	virtual BOOL Create(CWnd* pParentWnd);

	void LoadInsertStatus();
	void UpdateSelect();
	void UpdateStatus();

	CLeftTransferStation* m_pLeftTrStn;
	CRightTransferStation* m_pRightTrStn;
	BOOL m_bPackToggle;
	BOOL m_bSelected[DEF_MAX_JIG];
	BOOL m_bInserted[DEF_MAX_JIG];
	int m_nJigResID[DEF_MAX_JIG];

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnLine1();
	afx_msg void OnClickBtnLine2();
	afx_msg void OnClickBtnLine3();
	afx_msg void OnClickBtnLine4();
	afx_msg void OnClickBtnLine5();
	afx_msg void OnClickBtnLine6();
#ifndef DEF_EIN_48_LCA
	afx_msg void OnClickBtnLine7();
	afx_msg void OnClickBtnLine8();
#endif
	afx_msg void OnClickBtnLeftAll();
	afx_msg void OnClickBtnRightAll();
	afx_msg void OnClickBtnLeftLoadUp();
	afx_msg void OnClickBtnLeftLoadDown();
	afx_msg void OnClickBtnLeftUnloadUp();
	afx_msg void OnClickBtnLeftUnloadDown();
	afx_msg void OnClickBtnLeftNGCV();
	afx_msg void OnClickBtnRightLoadUp();
	afx_msg void OnClickBtnRightLoadDown();
	afx_msg void OnClickBtnRightUnloadUp();
	afx_msg void OnClickBtnRightUnloadDown();
	afx_msg void OnClickBtnRightNGCV();
	afx_msg void OnClickBtnLoadCV();
	afx_msg void OnClickBtnLoadCV2();
	afx_msg void OnClickBtnUnloadCV();
	afx_msg void OnClickBtnPackIn();
	afx_msg void OnClickBtnPackOut();
	afx_msg void OnClickBtnRepeatStart();
	afx_msg void OnClickBtnRepeatStop();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnClickBtnJig(UINT nID);
	DECLARE_MESSAGE_MAP()
};
