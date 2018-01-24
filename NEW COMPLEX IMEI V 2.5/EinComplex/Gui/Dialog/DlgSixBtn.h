#pragma once

class CDlgSixBtn : public CDialog
{
	DECLARE_DYNAMIC(CDlgSixBtn)

public:
	void SetItemText(CString strTitle, CString strOne, CString strTwo, CString strThree, CString strFour, CString strFive, CString strSix);
	CBrush m_brBckColor;
	CDlgSixBtn(CString strTitle, CString strOne, CString strTwo, CString strThree, CString strFour, CString strFive, CString strSix, CWnd* pParent = NULL);
	CDlgSixBtn(CWnd* pParent = NULL);
	CString m_strTitle;
	CString m_strOne;
	CString m_strTwo;
	CString m_strThree;
	CString m_strFour;
	CString m_strFive;
	CString m_strSix;
	BOOL	m_bClickBtn;
	virtual ~CDlgSixBtn();
	enum { IDD = IDD_DATA_SIXBTN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	DECLARE_MESSAGE_MAP()

public:
	DECLARE_EVENTSINK_MAP()
	void ClickButton1();
	void ClickButton2();
	void ClickButton3();
	void ClickButton4();
	void ClickButton5();
	void ClickButton6();
	virtual BOOL OnInitDialog();
};
