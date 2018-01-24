#ifndef __AX_CONTROLLER_H__
#define __AX_CONTROLLER_H__

#pragma once

#include "AxMaster.h"
#include "AxProfile.h"
#include "AxServiceHub.h"
#include "AxSystemHub.h"
#include "AxStationHub.h"

class __declspec(dllexport) CAxController : public CAxObject
{
public:
	CString		m_sRecipe;
	CString		m_sRecipePath;
	CAxRecipe	m_recipe;
	CAxProfile	m_profile;

	CAxController();
	virtual ~CAxController();

	void	Load(LPCTSTR pszAppRoot);
	void	Startup();
	void	Run();
	void	ChangeRecipe(LPCTSTR pszRecipe);
	CString	GetRecipeFile();
	void	AddService(CAxService* pService);
	void	AddSystem(CAxSystem* pSystem);
	void	AddStation(CAxStation* pStation);

	virtual void InitProfile();
	virtual void LoadProfile();
	virtual void SaveProfile();
	virtual void CheckProfile();
	virtual void InitRecipe();
	virtual void LoadRecipe();
	virtual void SaveRecipe();

	static BOOL IsTerminate();

protected:
	static BOOL m_bTerminate;

private:
	int					m_nNumServices;
	int					m_nNumSystems;
	int					m_nNumStations;
	CAxServiceHub*		m_pServiceHub;
	CAxSystemHub*		m_pSystemHub;
	CAxStationHub*		m_pStationHub;
	CAxErrorMgr*		m_pErrorMgr;
	CAxEventMgr*		m_pEventMgr;
	CAxMaster*			m_pMaster;
	CAxServicePtrArray	m_service;
	CAxSystemPtrArray	m_system;
	CAxStationPtrArray	m_station;

	void LoadServices();
	void LoadSystems();
	void LoadStations();
	void LoadMaster();
};

#endif