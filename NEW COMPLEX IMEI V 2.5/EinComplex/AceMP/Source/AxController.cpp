#include "stdafx.h"
#include "AxController.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

BOOL CAxController::m_bTerminate = FALSE;

CAxController::CAxController()
{
	m_profile.m_sSect = _T("Settings");
	m_recipe.m_sSect = _T("Controller");

	m_bTerminate = FALSE;
}

CAxController::~CAxController()
{
	m_bTerminate = TRUE;

	delete m_pMaster;
	delete m_pStationHub;
	delete m_pSystemHub;
	delete m_pServiceHub;
}

void CAxController::Load(LPCTSTR pszAppRoot)
{
	CAxObject::m_sRootPath = pszAppRoot;
	LoadServices();
	LoadSystems();
	LoadStations();
	LoadMaster();
}

void CAxController::Run()
{
	m_pMaster->Run();
	m_pServiceHub->Run();
	m_pSystemHub->Run();
	m_pStationHub->Run();
}

void CAxController::Startup()
{
	InitProfile();
	LoadProfile();
	InitRecipe();
	LoadRecipe();

	m_pMaster->Startup();

	m_pServiceHub->InitProfile();
	m_pServiceHub->LoadProfile();
	m_pServiceHub->Startup();

	m_pSystemHub->InitProfile();
	m_pSystemHub->LoadProfile();
	m_pSystemHub->Startup();

	m_pStationHub->InitProfile();
	m_pStationHub->LoadProfile();
	m_pStationHub->InitRecipe();
	m_pStationHub->LoadRecipe(GetRecipeFile());
	m_pStationHub->Startup();

	SaveProfile();
	SaveRecipe();
	m_pStationHub->SaveRecipe();
}

void CAxController::InitProfile()
{
	m_profile.AddStr(_T("Recipe"), m_sRecipe);
}

void CAxController::LoadProfile()
{
	m_profile.Load();

	CheckProfile();
}

void CAxController::SaveProfile()
{
	m_profile.Save();
}

void CAxController::CheckProfile()
{
	if( m_sRecipe == _T("") ) m_sRecipe = _T("NONE");
}

void CAxController::InitRecipe()
{
}

void CAxController::LoadRecipe()
{
	m_recipe.Load(GetRecipeFile());
}

void CAxController::SaveRecipe()
{
	m_recipe.Save(GetRecipeFile());
}

void CAxController::ChangeRecipe(LPCTSTR pszRecipe)
{
	m_sRecipe = pszRecipe;
	if( m_sRecipe == _T("") ) m_sRecipe = _T("NONE");

	SaveProfile();

	LoadRecipe();
	m_pStationHub->LoadRecipe(GetRecipeFile());

	SaveRecipe();
	m_pStationHub->SaveRecipe();
}

CString CAxController::GetRecipeFile()
{
	CString sRecipeFile;
	sRecipeFile.Format(_T("%s%s.dat"), (LPCTSTR)m_sRecipePath, (LPCTSTR)m_sRecipe);

	return sRecipeFile;
}

void CAxController::LoadServices()
{
	m_pServiceHub = CAxServiceHub::GetServiceHub();

	AddService(CAxEventMgr::GetEventMgr());
	AddService(CAxErrorMgr::GetErrorMgr());
	AddService(CAxIOMgr::GetIOMgr());
}

void CAxController::LoadSystems()
{
	m_pSystemHub = CAxSystemHub::GetSystemHub();

// 	AddSystem(new CAxControlPanel());
// 	AddSystem(new CAxSafety());
// 	AddSystem(new CAxTower());
	AddSystem(new CAxSystemError());
}

void CAxController::LoadStations()
{
	m_pStationHub = CAxStationHub::GetStationHub();
}

void CAxController::LoadMaster()
{
	m_pMaster = CAxMaster::GetMaster();
}

void CAxController::AddService(CAxService* pService)
{
	m_pServiceHub->AddService(pService);
}

void CAxController::AddSystem(CAxSystem* pSystem)
{
	m_pSystemHub->AddSystem(pSystem);
}

void CAxController::AddStation(CAxStation* pStation)
{
	m_pStationHub->AddStation(pStation);
}

BOOL CAxController::IsTerminate()
{
	return m_bTerminate;
}