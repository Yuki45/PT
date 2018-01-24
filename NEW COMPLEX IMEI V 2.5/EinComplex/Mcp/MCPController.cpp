#include "StdAfx.h"
#include "MCPController.h"

#include "MainControlStation.h"
#include "LeftTransferStation.h"
#include "RightTransferStation.h"
#include "LeftPPStation.h"
#include "RightPPStation.h"
#include "LeftNGCVStation.h"
#include "RightNGCVStation.h"
#include "LoadCVStation.h"
#include "LoadCV2Station.h"
#include "UnloadCVStation.h"
#include "LeftSafetyStation.h"
#include "RightSafetyStation.h"
#include "OPSystem.h"
#include "CommSystem.h"
#include "InsertCVSystem.h"
#include "JigSystem.h"

#include "Resource.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CMCPController* CMCPController::theController = NULL;

CMCPController::CMCPController()
{
	m_sName = _T("MCPController");
	m_profile.m_sIniFile = _T("\\Data\\Profile\\MainController.ini");

	m_sRecipePath = _T("\\Data\\Recipe\\");
	m_sLotPath = _T("\\Data\\LotList");

	m_sErrorPath = _T("\\Data\\Error");
	m_sMessagePath = _T("\\Data\\Message");

	m_sIOListPath = _T("\\Data\\IO");
	m_sMsgListPath = _T("\\Data\\MsgList");
	m_sErrMsgListPath = _T("\\Data\\ErrMsgList");
	m_sErrListPath = _T("\\Data\\ErrList");
	m_sLogPath = _T("\\Data\\Log"); 
}

CMCPController::~CMCPController()
{
}

CMCPController* CMCPController::GetController()
{
	if( theController == NULL ) theController = new CMCPController();

	return theController;
}

BOOL CMCPController::LoadController(const CString sAppPath)
{
	Load(sAppPath);
	Run();

	return TRUE;
}

void CMCPController::Load(const CString sAppPath)
{
	CAxController::Load(sAppPath);

	AddStation(new CMainControlStation());
	AddStation(new CLeftTransferStation());
	AddStation(new CRightTransferStation());
	AddStation(new CLeftPPStation());
	AddStation(new CRightPPStation());
	AddStation(new CLeftNGCVStation());
	AddStation(new CRightNGCVStation());
	AddStation(new CLoadCVStation());
	AddStation(new CLoadCV2Station());
	AddStation(new CUnloadCVStation());
	AddStation(new CLeftSafetyStation());
	AddStation(new CRightSafetyStation());

	AddSystem(new COPSystem());
 	AddSystem(new CCommSystem());
	AddSystem(new CInsertCVSystem());

	for( int i = 0; i < DEF_MAX_TEST_PC; i++ ) AddSystem(new CJigSystem(i + 1));

	Startup();
}

void CMCPController::InitProfile()
{
	CAxController::InitProfile();

	m_profile.AddStr(_T("MachineName"), m_sMachineName);
	m_profile.AddStr(_T("User"), m_sUser);
	m_profile.AddStr(_T("Lot"), m_sLot);
	m_profile.AddStr(_T("LotFile"), m_sLotFile);
	
	m_profile.AddStr(_T("OperatorPwd"), m_sOperatorPwd);
	m_profile.AddStr(_T("MainterPwd"), m_sMainterPwd);
	m_profile.AddStr(_T("MasterPwd"), m_sMasterPwd);
	m_profile.AddStr(_T("SafetyPwd"), m_sSafetyPwd);
}

void CMCPController::LoadProfile()
{
	CAxController::LoadProfile();
}

void CMCPController::SaveProfile()
{
	CAxController::SaveProfile();
}

void CMCPController::CheckProfile()
{
	if( m_sRecipe == _T("") )
	{
		m_sRecipe = _T("NONE");
		m_sLot = _T("NONE");
		CreateLotFile();
	}
	else if( m_sLot == _T("") )
	{
		m_sLot = _T("NONE");
		CreateLotFile();
	}
	else if( m_sLotFile == _T("") )
	{
		CreateLotFile();
	}
}

void CMCPController::InitRecipe()
{
	CAxController::InitRecipe();
}

void CMCPController::LoadRecipe()
{
	CAxController::LoadRecipe();
}

void CMCPController::SaveRecipe()
{
	CAxController::SaveRecipe();
}

void CMCPController::ChangeLot(LPCTSTR pszRecipe, LPCTSTR pszLot)
{
	m_sRecipe = pszRecipe;
	if( m_sRecipe == _T("") ) m_sRecipe = _T("NONE");

	m_sLot = pszLot;
	if( m_sLot == _T("") ) m_sLot = _T("NONE");

	CreateLotFile();
	SaveProfile();
	
	LoadRecipe();
	CAxStationHub::GetStationHub()->LoadRecipe(GetRecipeFile());

	SaveRecipe();
	CAxStationHub::GetStationHub()->SaveRecipe();
}

void CMCPController::CreateLotFile()
{
	COleDateTime time = COleDateTime::GetCurrentTime();

	CString sLotFullPath = GetRootPath() + m_sLotPath + time.Format(_T("\\%Y%m"));
	AxCreateDirectoryAll(sLotFullPath);

	m_sLotFile.Format(_T("%s\\%s\\%s_%s_%s_%s.ini"),
					  m_sLotPath,
					  time.Format(_T("%Y%m")),
					  time.Format(_T("%Y%m%d")),
					  time.Format(_T("%H%M%S")),
					  m_sRecipe,
					  m_sLot);

	HANDLE hFile = AxCreateFile(CAxObject::GetRootPath() + m_sLotFile);
	CloseHandle(hFile);
}

CString CMCPController::GetLotFile()
{
	return m_sLotFile;
}
