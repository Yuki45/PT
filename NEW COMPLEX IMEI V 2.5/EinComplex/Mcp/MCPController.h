#ifndef __MCPCONTROLLER_H__
#define __MCPCONTROLLER_H__

#pragma once

#include <AxController.h>

class CMCPController : public CAxController  
{
public:
	virtual ~CMCPController();

	static CMCPController* GetController();

	BOOL LoadController(const CString sAppPath);
	void Load(const CString sAppPath);

	void InitProfile();
	void LoadProfile();
	void SaveProfile();
	void CheckProfile();

	void InitRecipe();
	void LoadRecipe();
	void SaveRecipe();

	void ChangeLot(LPCTSTR pszRecipe, LPCTSTR pszLot);
	void CreateLotFile();
	CString GetLotFile();

	CString m_sMachineName;
	CString m_sUser;
	CString m_sLot;
	CString m_sLotPath;
	CString m_sLotFile;

	CString m_sMessagePath;
	CString m_sErrorPath;

	CString m_sIOListPath;
	CString m_sMsgListPath;
	CString m_sErrMsgListPath;
	CString m_sErrListPath;
	CString m_sLogPath;

	CString m_sOperatorPwd;
	CString m_sMainterPwd;
	CString m_sMasterPwd;
	CString m_sSafetyPwd;

private:
	CMCPController();
	static CMCPController* theController;
};

#endif
