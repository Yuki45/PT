#pragma once

class CDlgSelectTeach : public CDialog
{
public:
	int m_nSelected;

	CDlgSelectTeach(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_SELECT_TEACH };

	virtual BOOL Create(CWnd* pParentWnd);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnLeftTr();
	afx_msg void OnClickBtnRightTr();
	afx_msg void OnClickBtnPp();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
