// AxOutput.cpp: implementation of the CAxOutput class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxOutput.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxOutput::CAxOutput()
{
	m_pValue = NULL;
	m_pevtOn = NULL;
	m_pevtOff = NULL;
	m_bReverse = FALSE;
}

CAxOutput::CAxOutput(CAxOutput& output)
{
	m_pValue = output.m_pValue;
	m_pevtOn = output.m_pevtOn;
	m_pevtOff = output.m_pevtOff;
	m_bReverse = FALSE;
}

CAxOutput::CAxOutput(CAxDiData& diData, LPCTSTR pszAddr)
{
	m_sAddr = pszAddr;
	m_pValue = &diData.m_bValue;
	m_pevtOn = &diData.m_evtOn;
	m_pevtOff = &diData.m_evtOff;
	m_bReverse = FALSE;
}

CAxOutput::~CAxOutput()
{

}

void CAxOutput::Init(CAxOutput* pOutput, BOOL bReverse)
{
	m_bReverse = bReverse;
	m_pValue = pOutput->m_pValue;
	m_pevtOn  = bReverse ? pOutput->m_pevtOff : pOutput->m_pevtOn;
	m_pevtOff = bReverse ? pOutput->m_pevtOn  : pOutput->m_pevtOff;
}

void CAxOutput::On()
{
	*m_pValue = m_bReverse ? OFF : ON;
}

void CAxOutput::Off()
{
	*m_pValue = m_bReverse ? ON : OFF;
}

void CAxOutput::SetValue(BOOL bValue)
{
	m_bReverse ? *m_pValue = !bValue : *m_pValue = bValue;
}

BOOL CAxOutput::GetValue()
{
	return m_bReverse ? !*m_pValue : *m_pValue;
}

LPCTSTR CAxOutput::GetAddr()
{
	return m_sAddr;
}

BOOL CAxOutput::operator ==(BOOL bValue)
{
	return m_bReverse ? (bValue == !*m_pValue) : (bValue == *m_pValue);
}

CAxEvent* CAxOutput::operator ^(BOOL bValue)
{
//	return m_bReverse ? (bValue ? m_pevtOff:m_pevtOn) : (bValue ? m_pevtOn:m_pevtOff);
	return bValue ? m_pevtOn:m_pevtOff;
}
