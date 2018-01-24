#pragma once

class CLeftTransferStation;
class CMainFrame;
class CDlgLeftTrTeach : public CDialog
{
public:
	CDlgLeftTrTeach(CWnd* pParent = NULL);

#ifdef DEF_EIN_48_LCA
	enum { IDD = IDD_DLG_LEFT_TR_TEACH48 };
#else
	enum { IDD = IDD_DLG_LEFT_TR_TEACH56 };
#endif

	virtual BOOL Create(CWnd* pParentWnd);

	enum
	{
		SELECT_LD_BUFF = DEF_MAX_JIG_ONE_SIDE + 1,
		SELECT_UD_BUFF,
		SELECT_BCR,
		SELECT_NG
	};

	CMainFrame* m_pMainFrm;
	CLeftTransferStation* m_pLeftTrStn;

	BOOL m_bJogFast;
	BOOL m_bJogKey;

	int m_nSelected;
	int m_nLastSelected;
	int m_nResID[DEF_MAX_JIG_ONE_SIDE + 4];

	double m_dPosX;
	double m_dPosY;
	double m_dPosZ;

	void UpdateSelect();
	void UpdateTeachPos();
	void UpdateCurrPos();
	void UpdateAxisStatus();
	void UpdateIOStatus();

	BOOL chkDoorSensor();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnMoveX();
	afx_msg void OnClickBtnMoveY();
	afx_msg void OnClickBtnMoveZ();
	afx_msg void OnClickBtnStopX();
	afx_msg void OnClickBtnStopY();
	afx_msg void OnClickBtnStopZ();
	afx_msg void OnClickBtnServoOnX();
	afx_msg void OnClickBtnServoOnY();
	afx_msg void OnClickBtnServoOnZ();
	afx_msg void OnClickBtnServoOffX();
	afx_msg void OnClickBtnServoOffY();
	afx_msg void OnClickBtnServoOffZ();
	afx_msg void OnClickBtnAlarmResetX();
	afx_msg void OnClickBtnAlarmResetY();
	afx_msg void OnClickBtnAlarmResetZ();
	afx_msg void OnClickBtnOriginX();
	afx_msg void OnClickBtnOriginY();
	afx_msg void OnClickBtnOriginZ();
	afx_msg void OnClickBtnGripOnOff();
	afx_msg void OnClickBtnPackInOut();
	afx_msg void OnClickBtnInputCurrPosX();
	afx_msg void OnClickBtnInputCurrPosY();
	afx_msg void OnClickBtnInputCurrPosZ();
	afx_msg void OnClickBtnInputCurrPosXY();
	afx_msg void OnClickBtnInputCurrPos();
	afx_msg void OnClickBtnAllData();
	afx_msg void OnClickBtnXYMove();
	afx_msg void OnClickBtnZUp();
	afx_msg void OnClickBtnJogFast();
	afx_msg void OnClickBtnJogKey();
	afx_msg void OnClickBtnSave();
	afx_msg void OnClickBtnExit();
	afx_msg void OnClickSttTeachPosX();
	afx_msg void OnClickSttTeachPosY();
	afx_msg void OnClickSttTeachPosZ();
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnClickBtnSelect(UINT nID);
	afx_msg void OnDblClickBtnSelect(UINT nID);
	DECLARE_MESSAGE_MAP()
};
