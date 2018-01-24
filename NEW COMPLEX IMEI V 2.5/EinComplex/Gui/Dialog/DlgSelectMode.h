#pragma once

class CMainControlStation;
class CDlgSelectMode : public CDialog
{
public:
	CDlgSelectMode(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_SELECT_MODE };

	virtual BOOL Create(CWnd* pParentWnd);

	CMainControlStation* m_pMainStn;

	void UpdateMode();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnNormal();
	afx_msg void OnClickBtnPassrun();
	afx_msg void OnClickBtnDryrun();
	afx_msg void OnClickBtnSimulation();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
