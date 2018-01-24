// AxIOMgr.cpp: implementation of the CAxIOMgr class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxIOMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxIOMgr* CAxIOMgr::theIOMgr = NULL;
//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxIOMgr::CAxIOMgr() : m_lock(&m_mutex)
{
	m_sName = _T("IOMgr");
	m_sErrPath = _T("\\Service\\IOMgr.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\IOMgr.ini");
	m_nNumScanner = 0;
	m_nNumDiIp = 0;
	m_nNumDiOp = 0;
}

CAxIOMgr::~CAxIOMgr()
{
	POSITION pos;

	for(int i=0; i<m_nNumScanner; i++)
		delete m_scanner[i];

	pos = m_inputMap.GetStartPosition();
	while(pos != NULL) {
		CString sKey;
		CAxInput* pInput;
		m_inputMap.GetNextAssoc(pos, sKey, pInput);
		delete pInput;
	}
	pos = m_outputMap.GetStartPosition();
	while(pos != NULL) {
		CString sKey;
		CAxOutput* pOutput;
		m_outputMap.GetNextAssoc(pos, sKey, pOutput);
		delete pOutput;
	}
}

CAxIOMgr* CAxIOMgr::GetIOMgr()
{
	if(theIOMgr == NULL) theIOMgr = new CAxIOMgr();
	return theIOMgr;
}

void CAxIOMgr::InitProfile()
{
	CAxService::InitProfile();

	//m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxIOMgr"));
}

void CAxIOMgr::LoadProfile()
{
	CAxService::LoadProfile();

	m_nNumScanner = m_profile.ReadInt(_T("General"), _T("NumScanner"));
	//m_Trace.Log(_T("LoadProfile() -> m_nNumScanner : %d"), m_nNumScanner);
	ASSERT(m_nNumScanner);

	int nScannerType;

	for(int i=0; i<m_nNumScanner; i++) {
		m_profile.m_sSect.Format(_T("Scanner%d"), i);
		nScannerType = m_profile.ReadInt(_T("ScannerType"));

		switch(nScannerType) {
		case 0:
			m_scanner.Add(new CAxHilscherScanner(this));
			break;
		case 1:
		case 2:
		default:
			//m_Trace.Log(_T("LoadProfile() -> nScannerType : %d"), nScannerType);
			ASSERT(FALSE);
		}
	}
	
	CreateIO();
}

void CAxIOMgr::SaveProfile()
{
	CAxService::SaveProfile();
}

void CAxIOMgr::Startup()
{
	CAxService::Startup();
}

void CAxIOMgr::CreateIO()
{
	CString sAddr;
	
	//for(int i=0; i<m_scanner.GetSize(); i++) { // pjs 
	for(int i=0; i<m_nNumScanner; i++) {
		if(!m_scanner[i]->Init(i, m_profile.m_sIniFile)) 
		{
			//m_Trace.Log(_T("CreateIO() -> IOScanner(%d) Initialize Failed"), i);
			AfxMessageBox(_T("IOScanner Initialize Failed.\n\nCheck IO Board.\n\nRestart Machine Program."));
			ASSERT(FALSE);
		}
		int j = 0;
		int nNumDiIp = m_scanner[i]->m_nNumDiIp;
		while(j < nNumDiIp) {
			sAddr = m_scanner[i]->m_diData[j].m_sAddr;
			m_inputMap.SetAt(sAddr, new CAxInput(m_scanner[i]->m_diData[j], sAddr));
			j++;
		};
		m_nNumDiIp += nNumDiIp;
		j = 0;
		int nNumDiOp = m_scanner[i]->m_nNumDiOp;
		while(j < nNumDiOp) {
			sAddr = m_scanner[i]->m_doData[j].m_sAddr;
			m_outputMap.SetAt(sAddr, new CAxOutput(m_scanner[i]->m_doData[j], sAddr));
			j++;
		};
		m_nNumDiOp += nNumDiOp;
	}
}

CAxInput* CAxIOMgr::GetInput(LPCTSTR pszAddr)
{
	CAxInput* pInput;
	if(!m_inputMap.Lookup(pszAddr, pInput)) ASSERT(FALSE);
	return pInput;
}

CAxOutput* CAxIOMgr::GetOutput(LPCTSTR pszAddr)
{
	CAxOutput* pOutput;
	if(!m_outputMap.Lookup(pszAddr, pOutput)) ASSERT(FALSE);
	return pOutput;
}

void CAxIOMgr::SetInput(LPCTSTR pszAddr, CAxInput& ip)
{
	CAxInput* pInput;
	CString sAddr(pszAddr);

	BOOL bReverse = FALSE;
	if(sAddr.GetAt(0) == _T('-')) {
		bReverse = TRUE;
		sAddr.TrimLeft(_T("-"));
	}
	if(!m_inputMap.Lookup(sAddr, pInput)) ASSERT(FALSE);

	ip.Init(pInput, bReverse);
	ip.m_sAddr = pszAddr;
}

void CAxIOMgr::SetInput(LPCTSTR pszAddr, CAxInput& ip, LPCTSTR pszKey)
{
	CAxInput* pInput;
	CString sAddr(pszAddr);

	BOOL bReverse = FALSE;
	if(sAddr.GetAt(0) == _T('-')) {
		bReverse = TRUE;
		sAddr.TrimLeft(_T("-"));
	}
	if(!m_inputMap.Lookup(sAddr, pInput)) ASSERT(FALSE);

	ip.Init(pInput, bReverse);
	ip.m_sAddr = pszAddr;

	pInput->m_sName = pszKey;
}


void CAxIOMgr::SetOutput(LPCTSTR pszAddr, CAxOutput& op)
{
	CAxOutput* pOutput;
	CString sAddr(pszAddr);

	BOOL bReverse = FALSE;
	if(sAddr.GetAt(0) == _T('-')) {
		bReverse = TRUE;
		sAddr.TrimLeft(_T("-"));
	}
	if(!m_outputMap.Lookup(sAddr, pOutput)) ASSERT(FALSE);

	op.Init(pOutput, bReverse);
	op.m_sAddr = pszAddr;
	op.m_bReverse = bReverse;
}

void CAxIOMgr::SetOutput(LPCTSTR pszAddr, CAxOutput& op, LPCTSTR pszKey)
{
	CAxOutput* pOutput;
	CString sAddr(pszAddr);

	BOOL bReverse = FALSE;
	if(sAddr.GetAt(0) == _T('-')) {
		bReverse = TRUE;
		sAddr.TrimLeft(_T("-"));
	}
	if(!m_outputMap.Lookup(sAddr, pOutput)) ASSERT(FALSE);

	op.Init(pOutput, bReverse);
	op.m_sAddr = pszAddr;
	op.m_bReverse = bReverse;

	pOutput->m_sName = pszKey;
}

