#pragma once

#define DEF_MAX_DIGIT_KEY		10
#define DEF_MAX_DIGIT_INTPUT	20
#define DEF_DIGIT_DELETE		0x2e
#define DEF_DIGIT_DOT			0xbe

class CDlgDigitPad : public CDialog
{
private:
	CString m_sInput;
	UINT m_nCtrlID[DEF_MAX_DIGIT_KEY];
	BOOL m_bFirstValue;

	void UpdateDisplay();
	int GetKeyValue(UINT nCtrlID);
	int GetCtrlID(char cKeyboard);

public:
	CDlgDigitPad(double dDefault = 0.0, CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_DIGIT_PAD };

	virtual BOOL Create(CWnd* pParentWnd);
	virtual BOOL PreTranslateMessage(MSG* pMsg);

	double GetData();

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
	afx_msg void OnClickDot();
	afx_msg void OnClickPlusMinus();
	afx_msg void OnClickOK();
	afx_msg void OnClickCancel();
	DECLARE_MESSAGE_MAP()
};
