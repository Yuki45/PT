#ifndef __AX_PROFILE_H__
#define __AX_PROFILE_H__

#pragma once

#include "AxIni.h"
#include "AxIOMgr.h"
#include "AxEventMgr.h"

typedef CTypedPtrMap<CMapStringToPtr, CString, CAxEvent*>	CMapStringToEvent;
typedef CTypedPtrMap<CMapStringToPtr, CString, CAxInput*>	CMapStringToInput;
typedef CTypedPtrMap<CMapStringToPtr, CString, CAxOutput*>	CMapStringToOutput;

class __declspec(dllexport) CAxProfile : public CAxIni  
{
public:
	CAxProfile();
	virtual ~CAxProfile();

	void Load();
	void Load(LPCTSTR pszFile);
	void Save();
	void Save(LPCTSTR pszFile);
	void AddEvent(LPCTSTR pszKey, CAxEvent& event);
	void AddEvent(LPCTSTR pszSect, LPCTSTR pszKey, CAxEvent& event);
	void AddInput(LPCTSTR pszKey, CAxInput& input);
	void AddInput(LPCTSTR pszSect, LPCTSTR pszKey, CAxInput& input);
	void AddOutput(LPCTSTR pszKey, CAxOutput& output);
	void AddOutput(LPCTSTR pszSect, LPCTSTR pszKey, CAxOutput& output);
	void SetupEvent(LPCTSTR pszKey, CAxEvent& event);
	void SetupEvent(LPCTSTR pszSect, LPCTSTR pszKey, CAxEvent& event);
	void SetupInput(LPCTSTR pszKey, CAxInput& input);
	void SetupInput(LPCTSTR pszSect, LPCTSTR pszKey, CAxInput& input);
	void SetupOutput(LPCTSTR pszKey, CAxOutput& output);
	void SetupOutput(LPCTSTR pszSect, LPCTSTR pszKey, CAxOutput& output);

protected:
	CMapStringToEvent	m_eventMap;
	CMapStringToInput	m_inputMap;
	CMapStringToOutput	m_outputMap;

private:
	CAxIOMgr*		m_pIOMgr;
	CAxEventMgr*	m_pEventMgr;
};

typedef CArray<CAxProfile, CAxProfile&>			CAxProfileArray;
typedef CTypedPtrArray<CPtrArray, CAxProfile*>	CAxProfilePtrArray;

#endif