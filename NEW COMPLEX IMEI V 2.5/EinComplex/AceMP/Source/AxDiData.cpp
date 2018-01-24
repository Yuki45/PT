#include "stdafx.h"
#include "AxDiData.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxDiData::CAxDiData()
{
	m_bValue = OFF;
}

CAxDiData::~CAxDiData()
{
}

void CAxDiData::SetValue(BOOL bValue)
{
	m_bValue = bValue;
}

BOOL CAxDiData::GetValue()
{
	return m_bValue;
}

void CAxDiData::FireEvents()
{
	if( m_bValue ) {
		m_evtOn.Set();
		m_evtOff.Reset();
	}
	else {
		m_evtOn.Reset();
		m_evtOff.Set();
	}
}