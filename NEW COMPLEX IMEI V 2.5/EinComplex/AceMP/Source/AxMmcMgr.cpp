// AxMmcMgr.cpp: implementation of the CAxMmcMgr class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxMmcMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxMmcMgr* CAxMmcMgr::theMmcMgr = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxMmcMgr::CAxMmcMgr()
{
	m_pAxis = NULL;
	m_pRobot = NULL;
	m_pMmcCmd = NULL;

	m_nNumCtr = 0;
	m_nNumRobot = 0;
	m_nNumAxis = 0;
	
	m_sName = _T("MmcMgr");
	m_sErrPath = _T("\\Service\\MmcMgr.err");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\MmcMgr.ini");
}

CAxMmcMgr::~CAxMmcMgr()
{
	if( m_pAxis  != NULL ) delete []m_pAxis;
	if( m_pRobot != NULL ) delete []m_pRobot;
	if( m_pMmcCmd != NULL ) delete m_pMmcCmd;

	//m_bTerminate = TRUE;
	//m_control.Abort(m_control.m_priEvt);
	//DeleteAxThread(m_hPriThread);
}

CAxMmcMgr* CAxMmcMgr::GetMmcMgr()
{
	if( theMmcMgr == NULL ) theMmcMgr = new CAxMmcMgr();
	return theMmcMgr;
}

void CAxMmcMgr::InitProfile()
{
	CAxService::InitProfile();

	//m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxMmcMgr"));
}

void CAxMmcMgr::LoadProfile()
{
	CAxService::LoadProfile();

	m_nNumCtr = m_profile.ReadInt(_T("General"), _T("NumCtr"));
	m_nNumAxis = m_profile.ReadInt(_T("General"), _T("NumAxis"));
	m_nNumRobot = m_profile.ReadInt(_T("General"), _T("NumRobot"));
	
	// by pjs 2006.03.23
	// 모터를 사용하지 않을 경우에는 
	if( m_nNumCtr==0 || m_nNumAxis==0 || m_nNumRobot==0 )
	{
		//m_Trace.Log(_T("LoadProfile() -> m_nNumCtr : %d, m_nNumAxis : %d, m_nNumRobot : %d"), m_nNumCtr, m_nNumAxis, m_nNumRobot);
		return;
	}

	LoadCtr();
	LoadAxis();
	LoadRobot();
}

void CAxMmcMgr::SaveProfile()
{
	CAxService::SaveProfile();
}

void CAxMmcMgr::Startup()
{
	CAxService::Startup();
	

//	CreateAxThread();
}

void CAxMmcMgr::LoadCtr()
{
 	if( m_nNumCtr == 0 ) return;

	m_pMmcCmd = CAxMmcCmd::GetMmcCmd();

	UINT* pnBaseAddr = new UINT[m_nNumCtr];

	for( UINT i=0; i<m_nNumCtr; i++ ) 
	{
		m_profile.m_sSect.Format(_T("Ctr%d"), i);
		pnBaseAddr[i] = m_profile.ReadInt(_T("BaseAddr"));
	}

	try
	{
		m_pMmcCmd->MmcInitx(m_nNumCtr, (long*)pnBaseAddr/*, m_bSimulate, m_nNumAxis*/);
	}
	catch(UINT nExp) 
	{
		nExp= 0;
		//m_Trace.Log(_T("LoadCtr() -> MMC Initialize Failed : %d"), nExp);
		AfxMessageBox(_T("MMC Initialize Failed.\n\nCheck MMC Board.\n\nRestart Machine Program."));
		ASSERT(FALSE);
	}
	
	delete [] pnBaseAddr;
}

void CAxMmcMgr::LoadAxis()
{
	if( m_nNumAxis == 0 ) return;

	m_pAxis = new CAxAxis[m_nNumAxis];

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		m_pAxis[i].Init(i, m_profile.m_sIniFile);
	}
}

void CAxMmcMgr::LoadRobot()
{
	if( m_nNumRobot == 0 ) return;

	m_pRobot = new CAxRobot[m_nNumRobot];

	for(UINT i=0; i<m_nNumRobot; i++) 
	{
		m_pRobot[i].Init(i, m_pAxis, m_profile.m_sIniFile);
	}
}

UINT CAxMmcMgr::PriRun()
{
	SuspendAxThread(m_hPriThread);

   	while( TRUE ) 
	{
		try 
		{
			SetState(TS_IDLE);
			//WaitStart(m_control.m_priEvt);
			//SetState(TS_AUTO);
			AutoRun();
		}
		catch(int nExp) 
		{
			Sleep(10);
			if( nExp == -1 ) 
			{
				if( m_bTerminate ) AfxEndThread(0);
			}
			ResetEvents(m_control.m_priEvt);
		}
	}
	
	return 0;
}

UINT CAxMmcMgr::AutoRun()
{
	return 0;
}

UINT CAxMmcMgr::GetNumRobot()
{
	return m_nNumRobot;
}

int CAxMmcMgr::GetRobotNum(LPCTSTR szRobotName)
{
	for( UINT i=0; i<m_nNumRobot; i++ ) 
	{
		if( m_pRobot[i].m_sName == szRobotName )
		{
			return i;
		}
	}

	//m_Trace.Log(_T("GetRobotNum(LPCTSTR szRobotName) Failed : %s"), szRobotName);
	return -1;
}

CAxRobot* CAxMmcMgr::GetRobot(UINT nRobotNum)
{
	if( nRobotNum > m_nNumRobot ) 
	{
		//m_Trace.Log(_T("GetRobot(UINT nRobotNum) Failed : %d"), nRobotNum);
		return NULL;
	}
	
	return &m_pRobot[nRobotNum];
}

CAxRobot* CAxMmcMgr::GetRobot(LPCTSTR szRobotName)
{
	for( UINT i=0; i<m_nNumRobot; i++ ) 
	{
		if( m_pRobot[i].m_sName == szRobotName )
		{
			return &m_pRobot[i];
		}
	}

	//m_Trace.Log(_T("GetRobot(LPCTSTR szRobotName) Failed : %s"), szRobotName);
	return NULL;
}

UINT CAxMmcMgr::GetNumAxis()
{
	return m_nNumAxis;
}

int CAxMmcMgr::GetAxisNum(LPCTSTR szAxisName)
{
	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( m_pAxis[i].m_sName == szAxisName )
		{
			return i;
		}
	}

	//m_Trace.Log(_T("GetAxisNum(LPCTSTR szAxisName) Failed : %s"), szAxisName);
	return -1;
}

CAxAxis* CAxMmcMgr::GetAxis(UINT nAxisNum)
{
	if( nAxisNum > m_nNumAxis ) 
	{
		//m_Trace.Log(_T("GetAxis(UINT nAxisNum) Failed : %d"), nAxisNum);
		return NULL;
	}

	return &m_pAxis[nAxisNum];
}

CAxAxis* CAxMmcMgr::GetAxis(LPCTSTR szAxisName)
{
	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if(m_pAxis[i].m_sName == szAxisName)
		{
			return &m_pAxis[i];
		}
	}

	//m_Trace.Log(_T("GetAxis(LPCTSTR szAxisName) Failed : %s"), szAxisName);
	return NULL;
}

BOOL CAxMmcMgr::GetSimulate()
{
	return m_bSimulate;
}
