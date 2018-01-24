#pragma once

class CDlgAxisOrigin : public CDialog
{
public:
	CDlgAxisOrigin(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_AXIS_ORIGIN };

	virtual BOOL Create(CWnd* pParentWnd);

	void UpdateStatus();
	void UpdateSelect();

	BOOL m_bSelLeftX;
	BOOL m_bSelLeftY;
	BOOL m_bSelLeftZ;
	BOOL m_bSelLeftPP;
	BOOL m_bSelRightX;
	BOOL m_bSelRightY;
	BOOL m_bSelRightZ;
	BOOL m_bSelRightPP;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnLeftTrX();
	afx_msg void OnClickBtnLeftTrY();
	afx_msg void OnClickBtnLeftTrZ();
	afx_msg void OnClickBtnLeftPp();
	afx_msg void OnClickBtnRightTrX();
	afx_msg void OnClickBtnRightTrY();
	afx_msg void OnClickBtnRightTrZ();
	afx_msg void OnClickBtnRightPp();
	afx_msg void OnClickBtnSelectAll();
	afx_msg void OnClickBtnServoOn();
	afx_msg void OnClickBtnServoOff();
	afx_msg void OnClickBtnAlarmReset();
	afx_msg void OnClickBtnOrigin();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
