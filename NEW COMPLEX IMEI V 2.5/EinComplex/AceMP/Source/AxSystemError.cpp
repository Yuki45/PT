// AxSystemError.cpp: implementation of the CAxSystemError class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxSystemError.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxSystemError::CAxSystemError()
{
	m_sName = _T("SystemError");
	m_sErrPath = _T("\\System\\SystemError.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\System\\SystemError.ini");

	m_nErrCode = -1;

	m_bEmgRobotStop = FALSE;
}

CAxSystemError::~CAxSystemError()
{
	m_bTerminate = TRUE;
	m_control.Abort(m_control.m_priEvt);
	DeleteAxThread(m_hPriThread);
}

void CAxSystemError::Startup()
{
	CAxSystem::Startup();
}

void CAxSystemError::InitProfile()
{
	CAxSystem::InitProfile();

}

UINT CAxSystemError::AutoRun()
{
	Sleep(1000);

	while(TRUE) {
		Sleep(10); 

		if(m_bTerminate) throw -1;
		if(m_bSimulate)	continue;

		if(m_nErrCode != -1){
			SetError();
			m_nErrCode = -1;
		}

	}

	return 0;
}

void CAxSystemError::SetError()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();

	if(pMaster->GetState() != MS_ERROR){
		Error(m_nErrCode, emRetry, m_sName, m_sErrPath);
	}	
}

void CAxSystemError::SetErrorCode(int nErrCode)
{
	m_nErrCode = nErrCode;
}

void CAxSystemError::SetEmgRobotStop(BOOL bEmgRobotStop)
{
	m_bEmgRobotStop = bEmgRobotStop;
}

BOOL CAxSystemError::GetEmgRobotStop()
{
	return m_bEmgRobotStop;
}
