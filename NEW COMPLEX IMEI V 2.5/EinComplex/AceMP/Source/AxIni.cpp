// AxIni.cpp: implementation of the CAxIni class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxIni.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxIni::CAxIni()
{
	m_bAutoRootPath = TRUE;
}

CAxIni::~CAxIni()
{

}

void CAxIni::Load()
{
	POSITION pos;

	pos = m_boolMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		BOOL* pNum;
		m_boolMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadBool(key);
	}
	pos = m_shortMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		SHORT* pNum;
		m_shortMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadShort(key);
	}
	pos = m_ushortMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		USHORT* pNum;
		m_ushortMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadUshort(key);
	}
	pos = m_intMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		INT* pNum;
		m_intMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadInt(key);
	}
	pos = m_uintMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		UINT* pNum;
		m_uintMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadUint(key);
	}
	pos = m_floatMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		float* pNum;
		m_floatMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadFloat(key);
	}
	pos = m_doubleMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		double* pNum;
		m_doubleMap.GetNextAssoc(pos, key, pNum);
		*pNum = ReadDouble(key);
	}
	pos = m_stringMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		CString* pStr;
		m_stringMap.GetNextAssoc(pos, key, pStr);
		*pStr = ReadStr(key);
	}
	pos = m_robotLocMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		CAxRobotLoc* pLoc;
		m_robotLocMap.GetNextAssoc(pos, key, pLoc);
		*pLoc = ReadRobotLoc(key);
	}
}

void CAxIni::Save()
{
	POSITION pos;

	pos = m_boolMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		BOOL *pNum;
		m_boolMap.GetNextAssoc(pos, key, pNum);
		WriteBool(key, *pNum);
	}
	pos = m_shortMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		SHORT* pNum;
		m_shortMap.GetNextAssoc(pos, key, pNum);
		WriteShort(key, *pNum);
	}
	pos = m_ushortMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		USHORT* pNum;
		m_ushortMap.GetNextAssoc(pos, key, pNum);
		WriteUshort(key, *pNum);
	}
	pos = m_intMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		INT* pNum;
		m_intMap.GetNextAssoc(pos, key, pNum);
		WriteInt(key, *pNum);
	}
	pos = m_uintMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		UINT* pNum;
		m_uintMap.GetNextAssoc(pos, key, pNum);
		WriteUint(key, *pNum);
	}
	pos = m_floatMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		float* pNum;
		m_floatMap.GetNextAssoc(pos, key, pNum);
		WriteFloat(key, *pNum);
	}
	pos = m_doubleMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		double* pNum;
		m_doubleMap.GetNextAssoc(pos, key, pNum);
		WriteDouble(key, *pNum);
	}
	pos = m_stringMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		CString* pStr;
		m_stringMap.GetNextAssoc(pos, key, pStr);
		WriteStr(key, *pStr);
	}
	pos = m_robotLocMap.GetStartPosition();
	while(pos != NULL) {
		CString key;
		CAxRobotLoc* pLoc;
		m_robotLocMap.GetNextAssoc(pos, key, pLoc);
		WriteRobotLoc(key, pLoc);
	}

}

void CAxIni::AddBool(LPCTSTR pszKey, BOOL& num)
{
	m_boolMap.SetAt(pszKey, &num);
}

void CAxIni::AddShort(LPCTSTR pszKey, SHORT& num)
{
	m_shortMap.SetAt(pszKey, &num);
}

void CAxIni::AddUshort(LPCTSTR pszKey, USHORT& num)
{
	m_ushortMap.SetAt(pszKey, &num);
}

void CAxIni::AddInt(LPCTSTR pszKey, int& num)
{
	m_intMap.SetAt(pszKey, &num);
}

void CAxIni::AddUint(LPCTSTR pszKey, UINT& num)
{
	m_uintMap.SetAt(pszKey, &num);
}

void CAxIni::AddFloat(LPCTSTR pszKey, float& num)
{
	m_floatMap.SetAt(pszKey, &num);
}

void CAxIni::AddDouble(LPCTSTR pszKey, double& num)
{
	m_doubleMap.SetAt(pszKey, &num);
}

void CAxIni::AddStr(LPCTSTR pszKey, CString& str)
{
	m_stringMap.SetAt(pszKey, &str);
}

void CAxIni::AddRobotLoc(LPCTSTR pszKey, CAxRobotLoc& loc)
{
	m_robotLocMap.SetAt(pszKey, &loc);
}

BOOL CAxIni::GetBool(LPCTSTR pszKey)
{
	BOOL* pNum;
	m_boolMap.Lookup(pszKey, pNum);
	return *pNum;
}

SHORT CAxIni::GetShort(LPCTSTR pszKey)
{
	SHORT* pNum;
	m_shortMap.Lookup(pszKey, pNum);
	return *pNum;
}

USHORT CAxIni::GetUshort(LPCTSTR pszKey)
{
	USHORT* pNum;
	m_ushortMap.Lookup(pszKey, pNum);
	return *pNum;
}

int CAxIni::GetInt(LPCTSTR pszKey)
{
	INT* pNum;
	m_intMap.Lookup(pszKey, pNum);
	return *pNum;
}

UINT CAxIni::GetUint(LPCTSTR pszKey)
{
	UINT* pNum;
	m_uintMap.Lookup(pszKey, pNum);
	return *pNum;
}

float CAxIni::GetFloat(LPCTSTR pszKey)
{
	float* pNum;
	m_floatMap.Lookup(pszKey, pNum);
	return *pNum;
}

double CAxIni::GetDouble(LPCTSTR pszKey)
{
	double* pNum;
	m_doubleMap.Lookup(pszKey, pNum);
	return *pNum;
}

CString CAxIni::GetStr(LPCTSTR pszKey)
{	
	CString* pStr;
	m_stringMap.Lookup(pszKey, pStr);
	return *pStr;
}

CAxRobotLoc CAxIni::GetRobotLoc(LPCTSTR pszKey)
{
	CAxRobotLoc* pLoc;
	m_robotLocMap.Lookup(pszKey, pLoc);
	return *pLoc;
}

BOOL CAxIni::ReadBool(LPCTSTR pszKey, BOOL bDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), bDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(10), 10, sDefault); 
	sValue.ReleaseBuffer();
	return (BOOL)_ttoi(sValue);
}

BOOL CAxIni::ReadBool(LPCTSTR pszSect, LPCTSTR pszKey, BOOL bDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), bDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (BOOL)_ttoi(sValue);
}

SHORT CAxIni::ReadShort(LPCTSTR pszKey, SHORT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), nDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (SHORT)_ttoi(sValue);
}

SHORT CAxIni::ReadShort(LPCTSTR pszSect, LPCTSTR pszKey, SHORT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), nDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (SHORT)_ttoi(sValue);
}

USHORT CAxIni::ReadUshort(LPCTSTR pszKey, USHORT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%ud"), nDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (USHORT)_ttoi(sValue);
}

USHORT CAxIni::ReadUshort(LPCTSTR pszSect, LPCTSTR pszKey, USHORT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%ud"), nDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (USHORT)_ttoi(sValue);
}

int CAxIni::ReadInt(LPCTSTR pszKey, int nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), nDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (int)_ttoi(sValue);
}

int CAxIni::ReadInt(LPCTSTR pszSect, LPCTSTR pszKey, int nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%d"), nDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (int)_ttoi(sValue);
}

UINT CAxIni::ReadUint(LPCTSTR pszKey, UINT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%ud"), nDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (UINT)_ttoi(sValue);
}

UINT CAxIni::ReadUint(LPCTSTR pszSect, LPCTSTR pszKey, UINT nDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%ud"), nDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(10), 10, sDefault);
	sValue.ReleaseBuffer();
	return (UINT)_ttoi(sValue);
}

float CAxIni::ReadFloat(LPCTSTR pszKey, float fDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%f"), fDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(20), 20, sDefault);
	sValue.ReleaseBuffer();
	return (float)_tcstod(sValue, NULL);
}

float CAxIni::ReadFloat(LPCTSTR pszSect, LPCTSTR pszKey, float fDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%f"), fDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(20), 20, sDefault);
	sValue.ReleaseBuffer();
	return (float)_tcstod(sValue, NULL);
}

double CAxIni::ReadDouble(LPCTSTR pszKey, double dDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%f"), dDefault);
	ReadString(m_sSect, pszKey, sValue.GetBuffer(20), 20, sDefault);
	sValue.ReleaseBuffer();
	return _tcstod(sValue, NULL);
}

double CAxIni::ReadDouble(LPCTSTR pszSect, LPCTSTR pszKey, double dDefault)
{
	CString sValue;
	CString sDefault;
	sDefault.Format(_T("%f"), dDefault);
	ReadString(pszSect, pszKey, sValue.GetBuffer(20), 20, sDefault);
	sValue.ReleaseBuffer();
	return _tcstod(sValue, NULL);
}

CString CAxIni::ReadStr(LPCTSTR pszKey, CString sDefault)
{
	CString sValue;
	ReadString(m_sSect, pszKey, sValue.GetBuffer(200), 200, sDefault);
	sValue.ReleaseBuffer();
	return sValue;
}

CString CAxIni::ReadStr(LPCTSTR pszSect, LPCTSTR pszKey, CString sDefault)
{	
	CString sValue;
	ReadString(pszSect, pszKey, sValue.GetBuffer(200), 200, sDefault);
	sValue.ReleaseBuffer();
	return sValue;
}

CAxRobotLoc CAxIni::ReadRobotLoc(LPCTSTR pszKey)
{
	CString sValue;
	CAxRobotLoc pLoc;
	ReadString(m_sSect, pszKey, sValue.GetBuffer(200), 200);

	/*sscanf(sAddr, "%lf %lf %lf %lf %lf %lf %lf %lf %d %d",
			&pLoc.x, &pLoc.y, &pLoc.z, &pLoc.rz, &pLoc.m_dSpeed, &pLoc.m_dAccel, 
			&pLoc.m_dAppro, &pLoc.m_dDepart, &pLoc.m_nDwell, &pLoc.m_nSettle);*/
	sValue.ReleaseBuffer();
	sscanf_s(sValue, _T("%lf %lf %lf %lf "), &pLoc.x, &pLoc.y, &pLoc.z, &pLoc.rz);
	return pLoc;
}

CAxRobotLoc CAxIni::ReadRobotLoc(LPCTSTR pszSect, LPCTSTR pszKey)
{
	CString sValue;
	CAxRobotLoc pLoc;
	ReadString(pszSect, pszKey, sValue.GetBuffer(200), 200);

	/*sscanf(sAddr, "%lf %lf %lf %lf %lf %lf %lf %lf %d %d",
			&pLoc.x, &pLoc.y, &pLoc.z, &pLoc.rz, &pLoc.m_dSpeed, &pLoc.m_dAccel,
			&pLoc.m_dAppro, &pLoc.m_dDepart, &pLoc.m_nDwell, &pLoc.m_nSettle);*/
	sValue.ReleaseBuffer();
	sscanf_s(sValue, _T("%lf %lf %lf %lf "), &pLoc.x, &pLoc.y, &pLoc.z, &pLoc.rz);
	return pLoc;
}

void CAxIni::WriteBool(LPCTSTR pszKey, BOOL bValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), bValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteBool(LPCTSTR pszSect, LPCTSTR pszKey, BOOL bValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), bValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteShort(LPCTSTR pszKey, SHORT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteShort(LPCTSTR pszSect, LPCTSTR pszKey, SHORT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteUshort(LPCTSTR pszKey, USHORT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteUshort(LPCTSTR pszSect, LPCTSTR pszKey, USHORT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteInt(LPCTSTR pszKey, int nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteInt(LPCTSTR pszSect, LPCTSTR pszKey, int nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteUint(LPCTSTR pszKey, UINT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteUint(LPCTSTR pszSect, LPCTSTR pszKey, UINT nValue)
{
	CString sValue;
	sValue.Format(_T(" %d"), nValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteFloat(LPCTSTR pszKey, float fValue)
{
	CString sValue;
	sValue.Format(_T(" %f"), fValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteFloat(LPCTSTR pszSect, LPCTSTR pszKey, float fValue)
{
	CString sValue;
	sValue.Format(_T(" %f"), fValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteDouble(LPCTSTR pszKey, double dValue)
{
	CString sValue;
	sValue.Format(_T(" %f"), dValue);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteDouble(LPCTSTR pszSect, LPCTSTR pszKey, double dValue)
{
	CString sValue;
	sValue.Format(_T(" %f"), dValue);
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteStr(LPCTSTR pszKey, CString sValue)
{
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteStr(LPCTSTR pszSect, LPCTSTR pszKey, CString sValue)
{
	WriteString(pszSect, pszKey, sValue);
}

void CAxIni::WriteRobotLoc(LPCTSTR pszKey, CAxRobotLoc* pLoc)
{
	CString sValue;
	/*sAddr.Format(_T(" %lf %lf %lf %lf %lf %lf %lf %lf %d %d"),
				 pLoc->x, pLoc->y, pLoc->z, pLoc->rz, pLoc->m_dSpeed, pLoc->m_dAccel,
				 pLoc->m_dAppro, pLoc->m_dDepart, pLoc->m_nDwell, pLoc->m_nSettle);*/
	sValue.Format(_T(" %.3lf %.3lf %.3lf %.3lf"), pLoc->x, pLoc->y, pLoc->z, pLoc->rz);
	WriteString(m_sSect, pszKey, sValue);
}

void CAxIni::WriteRobotLoc(LPCTSTR pszSect, LPCTSTR pszKey, CAxRobotLoc* pLoc)
{
	CString sValue;
	/*sAddr.Format(_T(" %lf %lf %lf %lf %lf %lf %lf %lf %d %d"),
				 pLoc->x, pLoc->y, pLoc->z, pLoc->rz, pLoc->m_dSpeed, pLoc->m_dAccel,
				 pLoc->m_dAppro, pLoc->m_dDepart, pLoc->m_nDwell, pLoc->m_nSettle);*/
	sValue.Format(_T(" %.3lf %.3lf %.3lf %.3lf"), pLoc->x, pLoc->y, pLoc->z, pLoc->rz);
	WriteString(pszSect, pszKey, sValue);
}

int CAxIni::ReadString(LPCTSTR pszSect, LPCTSTR pszKey, LPTSTR pszStr, DWORD nSize, CString sDefault)
{
	return GetPrivateProfileString(pszSect, pszKey, sDefault, pszStr, nSize, 
				m_bAutoRootPath ? m_sRootPath+m_sIniFile : m_sIniFile); 
}

int CAxIni::WriteString(LPCTSTR pszSect, LPCTSTR pszKey, CString sValue)
{
	return WritePrivateProfileString(pszSect, pszKey, sValue, 
				m_bAutoRootPath ? m_sRootPath+m_sIniFile : m_sIniFile); 
}
