#ifndef __AX_STATIONHUB_H__
#define __AX_STATIONHUB_H__

#pragma once

#include "AxStation.h"

class __declspec(dllexport) CAxStationHub : public CAxObject  
{
public:
	virtual ~CAxStationHub();

	void				Startup();
	void				Run();
	void				InitProfile();
	void				LoadProfile();
	void				SaveProfile();
	void				InitRecipe();
	void				LoadRecipe();
	void				LoadRecipe(LPCTSTR pszFile);
	void				SaveRecipe();
	void				SaveRecipe(LPCTSTR pszFile);
	int					GetNumStations();
	void				AddStation(CAxStation* pStation);
	CAxStation*			GetStation(int nStation);
	CAxStation*			GetStation(LPCTSTR pszName);
	CAxStationPtrArray&	GetStation();
	void				PostStop(UINT nType);
	void				PreResume(UINT nType);

	static CAxStationHub* GetStationHub();

protected:
	CAxStationHub();

private:
	int					m_nNumStations;
	CAxStationPtrArray	m_station;

	static CAxStationHub* theStationHub;
};

#endif