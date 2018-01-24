#ifndef __AX_INI_H__
#define __AX_INI_H__

#pragma once

#include "AxObject.h"
#include "AxRobotLoc.h"

typedef CTypedPtrMap<CMapStringToPtr, CString, BOOL*>			CMapStringToBool;
typedef CTypedPtrMap<CMapStringToPtr, CString, SHORT*>			CMapStringToShort;
typedef CTypedPtrMap<CMapStringToPtr, CString, USHORT*>			CMapStringToUshort;
typedef CTypedPtrMap<CMapStringToPtr, CString, int*>			CMapStringToInt;
typedef CTypedPtrMap<CMapStringToPtr, CString, UINT*>			CMapStringToUint;
typedef CTypedPtrMap<CMapStringToPtr, CString, float*>			CMapStringToFloat;
typedef CTypedPtrMap<CMapStringToPtr, CString, double*>			CMapStringToDouble;
typedef CTypedPtrMap<CMapStringToPtr, CString, CString*>		CMapStringToStr;
typedef CTypedPtrMap<CMapStringToPtr, CString, CAxRobotLoc*>	CMapStringToRobotLoc;
 
class __declspec(dllexport) CAxIni : public CAxObject 
{
public:
	CAxIni();
	virtual ~CAxIni();

	BOOL	m_bAutoRootPath;
	CString m_sSect;
	CString m_sIniFile;

	void		AddBool(LPCTSTR pszKey, BOOL& num);
	void		AddShort(LPCTSTR pszKey, SHORT& num);
	void		AddUshort(LPCTSTR pszKey, USHORT& num);
	void		AddInt(LPCTSTR pszKey, int& num);
	void		AddUint(LPCTSTR pszKey, UINT& num);
	void		AddFloat(LPCTSTR pszKey, float& num);
	void		AddDouble(LPCTSTR pszKey, double& num);
	void		AddStr(LPCTSTR pszKey, CString& str);
	void		AddRobotLoc(LPCTSTR pszKey, CAxRobotLoc& loc);
	BOOL		GetBool(LPCTSTR pszKey);
	SHORT		GetShort(LPCTSTR pszKey);
	USHORT		GetUshort(LPCTSTR pszKey);
	int			GetInt(LPCTSTR pszKey);
	UINT		GetUint(LPCTSTR pszKey);
	float		GetFloat(LPCTSTR pszKey);
	double		GetDouble(LPCTSTR pszKey);
	CString		GetStr(LPCTSTR pszKey);
	CAxRobotLoc	GetRobotLoc(LPCTSTR pszKey);
	BOOL		ReadBool(LPCTSTR pszKey, BOOL bDefault = FALSE);
	BOOL		ReadBool(LPCTSTR pszSect, LPCTSTR pszKey, BOOL bDefault = FALSE);
	SHORT		ReadShort(LPCTSTR pszKey, SHORT nDefault = 0);
	SHORT		ReadShort(LPCTSTR pszSect, LPCTSTR pszKey, SHORT nDefault = 0);
	USHORT		ReadUshort(LPCTSTR pszKey, USHORT nDefault = 0);
	USHORT		ReadUshort(LPCTSTR pszSect, LPCTSTR pszKey, USHORT nDefault = 0);
	int			ReadInt(LPCTSTR pszKey, int nDefault = 0);
	int			ReadInt(LPCTSTR pszSect, LPCTSTR pszKey, int nDefault = 0);
	UINT		ReadUint(LPCTSTR pszKey, UINT nDefault = 0);
	UINT		ReadUint(LPCTSTR pszSect, LPCTSTR pszKey, UINT nDefault = 0);
	float		ReadFloat(LPCTSTR pszKey, float fDefault = 0);
	float		ReadFloat(LPCTSTR pszSect, LPCTSTR pszKey, float fDefault = 0);
	double		ReadDouble(LPCTSTR pszKey, double dDefault = 0);
	double		ReadDouble(LPCTSTR pszSect, LPCTSTR pszKey, double dDefault = 0);
	CString		ReadStr(LPCTSTR pszKey, CString sDefault = _T(""));
	CString		ReadStr(LPCTSTR pszSect, LPCTSTR pszKey, CString sDefault = _T(""));
	CAxRobotLoc	ReadRobotLoc(LPCTSTR pszKey);
	CAxRobotLoc	ReadRobotLoc(LPCTSTR pszSect, LPCTSTR pszKey);
	void		WriteBool(LPCTSTR pszKey, BOOL bValue);
	void		WriteBool(LPCTSTR pszSect, LPCTSTR pszKey, BOOL bValue);
	void		WriteShort(LPCTSTR pszKey, SHORT nValue);
	void		WriteShort(LPCTSTR pszSect, LPCTSTR pszKey, SHORT nValue);
	void		WriteUshort(LPCTSTR pszKey, USHORT nValue);
	void		WriteUshort(LPCTSTR pszSect, LPCTSTR pszKey, USHORT nValue);
	void		WriteInt(LPCTSTR pszKey, int nValue);
	void		WriteInt(LPCTSTR pszSect, LPCTSTR pszKey, int nValue);
	void		WriteUint(LPCTSTR pszKey, UINT nValue);
	void		WriteUint(LPCTSTR pszSect, LPCTSTR pszKey, UINT nValue);
	void		WriteFloat(LPCTSTR pszKey, float fValue);
	void		WriteFloat(LPCTSTR pszSect, LPCTSTR pszKey, float fValue);
	void		WriteDouble(LPCTSTR pszKey, double dValue);
	void		WriteDouble(LPCTSTR pszSect, LPCTSTR pszKey, double dValue);
	void		WriteStr(LPCTSTR pszKey, CString sValue);
	void		WriteStr(LPCTSTR pszSect, LPCTSTR pszKey, CString sValue);
	void		WriteRobotLoc(LPCTSTR pszKey, CAxRobotLoc* pLoc);
	void		WriteRobotLoc(LPCTSTR pszSect, LPCTSTR pszKey, CAxRobotLoc* pLoc);
	int			ReadString(LPCTSTR pszSect, LPCTSTR pszKey, LPTSTR pszStr, DWORD nSize, CString sDefault = _T(""));
	int			WriteString(LPCTSTR pszSect, LPCTSTR pszKey, CString sValue);

	virtual void Load();
	virtual void Save();

protected:
	CMapStringToBool		m_boolMap;
	CMapStringToShort		m_shortMap;
	CMapStringToUshort		m_ushortMap;
	CMapStringToInt			m_intMap;
	CMapStringToUint		m_uintMap;
	CMapStringToFloat		m_floatMap;
	CMapStringToDouble		m_doubleMap;
	CMapStringToStr			m_stringMap;
	CMapStringToRobotLoc	m_robotLocMap;
};

typedef CArray<CAxIni, CAxIni&>				CAxIniArray;
typedef CTypedPtrArray<CPtrArray, CAxIni*>	CAxIniPtrArray;

#endif