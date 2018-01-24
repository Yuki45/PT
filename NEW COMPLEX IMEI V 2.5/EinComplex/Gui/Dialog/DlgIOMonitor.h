#pragma once

#include <AxIOMgr.h>
#include <AxIni.h>
#include "MCPController.h"

#define DEF_MAX_IO_LINE 16

class CDlgIOMonitor : public CDialog
{
public:
	CAxIOMgr* m_pAxIOMgr;
	CMCPController* m_pController;
	CAxIni m_iniIOList;

	int m_nResInputID[DEF_MAX_IO_LINE];
	int m_nResInputNameID[DEF_MAX_IO_LINE];
	int m_nResOutputID[DEF_MAX_IO_LINE];
	int m_nResOutputNameID[DEF_MAX_IO_LINE];

	int m_nInputPage;
	int m_nOutputPage;
	int m_nMaxInputPage;
	int m_nMaxOutputPage;

	void UpdateStatus();
	void UpdateInput();
	void UpdateOutput();
	void InitResInfo();

	CDlgIOMonitor(CWnd* pParent = NULL);

	enum { IDD = IDD_DLG_IO_MONITOR };

	virtual BOOL Create(CWnd* pParentWnd);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);
	virtual void PostNcDestroy();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual BOOL OnInitDialog();

	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnClickBtnInputPrev();
	afx_msg void OnClickBtnInputNext();
	afx_msg void OnClickBtnOutputPrev();
	afx_msg void OnClickBtnOutputNext();
	afx_msg void OnClickBtnExit();
	DECLARE_EVENTSINK_MAP()

	afx_msg void OnClickBtnOutput(UINT nID);
	DECLARE_MESSAGE_MAP()
};
