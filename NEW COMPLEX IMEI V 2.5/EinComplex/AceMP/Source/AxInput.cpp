// AxInput.cpp: implementation of the CAxInput class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxInput.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxInput::CAxInput()
{
	m_pValue = NULL;
	m_pevtOn = NULL;
	m_pevtOff = NULL;
	m_bReverse = FALSE;
}

CAxInput::CAxInput(CAxInput& input)
{
	m_sAddr = input.m_sAddr;
	m_pValue = input.m_pValue;
	m_pevtOn = input.m_pevtOn;
	m_pevtOff = input.m_pevtOff;
	m_bReverse = FALSE;
}

CAxInput::CAxInput(CAxDiData& diData, LPCTSTR pszAddr)
{
	m_sAddr = pszAddr;
	m_pValue = &diData.m_bValue;
	m_pevtOn = &diData.m_evtOn;
	m_pevtOff = &diData.m_evtOff;
	m_bReverse = FALSE;
}

CAxInput::~CAxInput()
{

}

void CAxInput::Init(CAxInput* pInput, BOOL bReverse)
{
	m_bReverse = bReverse;

	m_pValue  = pInput->m_pValue;
	m_pevtOn  = bReverse ? pInput->m_pevtOff : pInput->m_pevtOn;
	m_pevtOff = bReverse ? pInput->m_pevtOn  : pInput->m_pevtOff;
}

void CAxInput::On()
{
	*m_pValue = m_bReverse ? OFF : ON;
}

void CAxInput::Off()
{
	*m_pValue = m_bReverse ? ON : OFF;
}

void CAxInput::SetValue(BOOL bValue)
{
	m_bReverse ? *m_pValue = !bValue : *m_pValue = bValue;
}

BOOL CAxInput::GetValue()
{
	return m_bReverse ? !*m_pValue : *m_pValue;
}

LPCTSTR CAxInput::GetAddr()
{
	return m_sAddr;
}

BOOL CAxInput::operator ==(BOOL bValue)
{
	return m_bReverse ? (bValue == !*m_pValue) : (bValue == *m_pValue);
}

CAxEvent* CAxInput::operator ^(BOOL bValue)
{
	// by pjs 2005.05.09
	// Init에서 Reverse 되고 여기에서도 Reverse 되서 원래대로 됨.
//	return m_bReverse ? (bValue ? m_pevtOff:m_pevtOn) : (bValue ? m_pevtOn:m_pevtOff);
	return bValue ? m_pevtOn:m_pevtOff;
}