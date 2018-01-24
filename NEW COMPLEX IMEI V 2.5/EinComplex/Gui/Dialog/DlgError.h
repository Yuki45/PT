#pragma once

class CMainFrame;
class CDlgError : public CDialog
{
public:
	CDlgError(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_ERROR };

	virtual BOOL Create(CWnd* pParentWnd);

	CMainFrame* m_pMainFrm;
	int m_nResponse;

	void UpdateError();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnRetry();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
