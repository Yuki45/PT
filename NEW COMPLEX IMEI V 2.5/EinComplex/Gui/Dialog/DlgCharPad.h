#pragma once

#define DEF_MAX_CHAR_KEY	37
#define DEF_MAX_CHAR_INTPUT	50
#define DEF_CHAR_DELETE		0x2e
#define DEF_CHAR_HYPHEN		0xbd

class CDlgCharPad : public CDialog
{
private:
	CString m_sInput;
	UINT m_nCtrlID[DEF_MAX_CHAR_KEY];
	int m_nNormalValue[DEF_MAX_CHAR_KEY];
	int m_nShiftValue[DEF_MAX_CHAR_KEY];
	BOOL m_bShift;
	BOOL m_bFirstValue;

	void UpdateDisplay();
	void UpdateKeyboard();
	int GetKeyIndex(UINT nCtrlID);
	int GetCtrlID(char cKeyboard);

public:
	CDlgCharPad(CString sDefault = _T(""), CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_CHAR_PAD };

	virtual BOOL Create(CWnd* pParentWnd);
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	CString GetData();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnClickKey(UINT nID);
	afx_msg void OnClickBackspace();
	afx_msg void OnClickClear();
	afx_msg void OnClickShift();
	afx_msg void OnClickOK();
	afx_msg void OnClickCancel();
	DECLARE_MESSAGE_MAP()
};
