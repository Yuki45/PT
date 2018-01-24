#pragma once

class CLeftPPStation;
class CRightPPStation;
class CMainFrame;
class CDlgPpTeach : public CDialog
{
public:
	CDlgPpTeach(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_PP_TEACH };

	virtual BOOL Create(CWnd* pParentWnd);

	enum
	{
		SELECT_WAIT = 1,
		SELECT_CV,
		SELECT_BUFF
	};

	CMainFrame* m_pMainFrm;
	CLeftPPStation* m_pLeftPpStn;
	CRightPPStation* m_pRightPpStn;

	BOOL m_bJogFast;
	BOOL m_bJogKeyLeft;
	BOOL m_bJogKeyRight;

	int m_nSelectedLeft;
	int m_nSelectedRight;
	int m_nLastSelectedLeft;
	int m_nLastSelectedRight;
	int m_nResIDLeft[3];
	int m_nResIDRight[3];

	double m_dPosLeft;
	double m_dPosRight;

	void UpdateSelect();
	void UpdateTeachPos();
	void UpdateCurrPos();
	void UpdateAxisStatus();
	void UpdateIOStatus();

	BOOL chkDoorSensorLeft();
	BOOL chkDoorSensorRight();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnMoveLeft();
	afx_msg void OnClickBtnMoveRight();
	afx_msg void OnClickBtnStopLeft();
	afx_msg void OnClickBtnStopRight();
	afx_msg void OnClickBtnServoOnLeft();
	afx_msg void OnClickBtnServoOnRight();
	afx_msg void OnClickBtnServoOffLeft();
	afx_msg void OnClickBtnServoOffRight();
	afx_msg void OnClickBtnAlarmResetLeft();
	afx_msg void OnClickBtnAlarmResetRight();
	afx_msg void OnClickBtnOriginLeft();
	afx_msg void OnClickBtnOriginRight();
	afx_msg void OnClickBtnLeftLoadUp();
	afx_msg void OnClickBtnRightLoadUp();
	afx_msg void OnClickBtnLeftLoadDown();
	afx_msg void OnClickBtnRightLoadDown();
	afx_msg void OnClickBtnLeftUnloadUp();
	afx_msg void OnClickBtnRightUnloadUp();
	afx_msg void OnClickBtnLeftUnloadDown();
	afx_msg void OnClickBtnRightUnloadDown();
	afx_msg void OnClickBtnLeftLoadGripOnOff();
	afx_msg void OnClickBtnRightLoadGripOnOff();
	afx_msg void OnClickBtnLeftUnloadGripOnOff();
	afx_msg void OnClickBtnRightUnloadGripOnOff();
	afx_msg void OnClickBtnInputCurrPosLeft();
	afx_msg void OnClickBtnInputCurrPosRight();
	afx_msg void OnClickBtnJogFast();
	afx_msg void OnClickBtnJogKeyLeft();
	afx_msg void OnClickBtnJogKeyRight();
	afx_msg void OnClickBtnSave();
	afx_msg void OnClickBtnExit();
	afx_msg void OnClickSttTeachPosLeft();
	afx_msg void OnClickSttTeachPosRight();
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnClickBtnSelectLeft(UINT nID);
	afx_msg void OnClickBtnSelectRight(UINT nID);
	afx_msg void OnDblClickBtnSelectLeft(UINT nID);
	afx_msg void OnDblClickBtnSelectRight(UINT nID);
	DECLARE_MESSAGE_MAP()
};
