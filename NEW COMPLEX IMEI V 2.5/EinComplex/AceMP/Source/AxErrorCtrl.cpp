#include "stdafx.h"
#include "AxErrorCtrl.h"
#include "AxErrorMgr.h"
#include "AxMaster.h"
#include "AxStation.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BOOL CAxErrorCtrl::m_bError = FALSE;
int CAxErrorCtrl::m_nErrCnt = 0;

CAxErrorCtrl::CAxErrorCtrl()
{
	m_pErrMsg = NULL;
	m_pControl = NULL;
	m_pSimulate = NULL;
}

CAxErrorCtrl::~CAxErrorCtrl()
{
}

void CAxErrorCtrl::Load(CAxErrMsg* pErrMsg, CAxControl* pControl, BOOL* pSimulate)
{
	m_pErrMsg = pErrMsg;
	m_pControl = pControl;
	m_pSimulate = pSimulate;
}

int CAxErrorCtrl::Error(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath, 
						LPCTSTR pszParam1, LPCTSTR pszParam2, LPCTSTR pszParam3, LPCTSTR pszParam4)
{
	CString sFile;

//	if( *m_pSimulate ) return 0;
//	CAxMaster* pMaster = CAxMaster::GetMaster();
//	if( (pMaster->GetState() == MS_MANUAL) && (pMaster->GetPreManualState() == MS_ERROR) ) return 0;
//	sFile.Format(_T("%s%s%d.htm"), pszPath, pszSource, nNumber);
//	nType = nType | emIgnore;

	m_pErrMsg->m_nNumber = nNumber;
	m_pErrMsg->m_nType = nType;
	m_pErrMsg->m_sSource = pszSource;
	m_pErrMsg->m_sHelpFile = pszPath;
	m_pErrMsg->m_sParams[0] = pszParam1;
	m_pErrMsg->m_sParams[1] = pszParam2;
	m_pErrMsg->m_sParams[2] = pszParam3;
	m_pErrMsg->m_sParams[3] = pszParam4;

//	if( m_nErrCnt > 5 ) m_nErrCnt = 0;
//	while( m_bError ) m_pControl->Wait(300 * (m_nErrCnt + 1));
//	m_nErrCnt++;
//	m_bError = TRUE;

	m_pControl->m_evt[CT_STOPPED]->Set();
	m_pControl->m_evt[CT_ERROR]->Set();

	int nRespCode;
	nRespCode = m_pControl->WaitResponse();

//	m_bError = FALSE;
//	nRespCode = CAxErrorMgr::GetErrorMgr()->GetResponse();
//	if( nRespCode == 0 ) nRespCode = emRetry;

	return nRespCode;
}
