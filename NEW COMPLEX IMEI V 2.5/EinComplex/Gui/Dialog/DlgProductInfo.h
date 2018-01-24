#pragma once

class CMainControlStation;
class CDlgProductInfo : public CDialog
{
public:
	CDlgProductInfo(CWnd* pParent = NULL);

#ifdef DEF_EIN_48_LCA
	enum { IDD = IDD_DLG_PRODUCT_INFO48 };
#else
	enum { IDD = IDD_DLG_PRODUCT_INFO56 };
#endif

	virtual BOOL Create(CWnd* pParentWnd);

	void UpdateSelect();
	void UpdateInfo();

	CMainControlStation* m_pMainStn;
	int m_nSelected;
	int m_nResID[DEF_MAX_JIG];
	BOOL m_bIncludeRetest;

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnResetJig();
	afx_msg void OnClickBtnResetAll();
	afx_msg void OnClickBtnIncludeRetest();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnClickBtnSelect(UINT nID);
	DECLARE_MESSAGE_MAP()
};
