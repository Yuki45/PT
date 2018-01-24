#pragma once

class CDlgInit : public CDialog
{
public:
	CDlgInit(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_INIT };

	virtual BOOL Create(CWnd* pParentWnd);

	void UpdateSelect();

	BOOL m_bSelLeftTR;
	BOOL m_bSelLeftPP;
	BOOL m_bSelLeftNGCV;
	BOOL m_bSelRightTR;
	BOOL m_bSelRightPP;
	BOOL m_bSelRightNGCV;
	BOOL m_bSelLoadCV;
	BOOL m_bSelUnloadCV;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnLeftTR();
	afx_msg void OnClickBtnLeftPP();
	afx_msg void OnClickBtnLeftNGCV();
	afx_msg void OnClickBtnRightTR();
	afx_msg void OnClickBtnRightPP();
	afx_msg void OnClickBtnRightNGCV();
	afx_msg void OnClickBtnLoadCV();
	afx_msg void OnClickBtnUnloadCV();
	afx_msg void OnClickBtnSelectAll();
	afx_msg void OnClickBtnInit();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
