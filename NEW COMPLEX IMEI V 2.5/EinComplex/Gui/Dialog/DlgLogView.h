#pragma once

class CDlgLogView : public CDialog
{
public:
	CDlgLogView(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_LOG_VIEW };

	virtual BOOL Create(CWnd* pParentWnd);

	COleDateTime m_Date;
	CListBox m_listLog;
	CFont m_font;

	void UpdateLog();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	DECLARE_MESSAGE_MAP()
};
