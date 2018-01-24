// AxIOCtrl.cpp: implementation of the CAxIOCtrl class.
//
//////////////////////////////////////////////////////////////////////
#include "stdafx.h"
#include "AxIOCtrl.h"
#include "AxErrorMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxIOCtrl::CAxIOCtrl(CAxErrorCtrl* pCtrl)
{
	m_pErrCtrl = pCtrl;
	m_pControl = pCtrl->m_pControl;
}


CAxIOCtrl::~CAxIOCtrl()
{

}


void CAxIOCtrl::TurnOn(CAxOutput* pOp)
{
	ASSERT(pOp != NULL);
	pOp->On();
}


void CAxIOCtrl::TurnOn(CAxOutput& op)
{
	op.On();
}


void CAxIOCtrl::TurnOn(CAxOutput* pOp, CAxInput* pIp, BOOL bValue,
					   DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
					   LPCTSTR pszSource, int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pOp != NULL && pIp != NULL);
	event.Add(bValue ? *pIp^ON : *pIp^OFF);

	do {
		TurnOn(pOp);
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOn(CAxOutput& op, CAxInput& ip, BOOL bValue,
					   DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
					   LPCTSTR pszSource, int nCode) 
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(bValue ? ip^ON : ip^OFF);

	do {
		TurnOn(op);
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode, event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOn(CAxOutput* pOp, CAxInput* pOnIp, CAxInput* pOffIp,
					   DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
					   int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pOp != NULL && pOnIp != NULL && pOffIp != NULL);
	event.Add(*pOnIp^ON);
	event.Add(*pOffIp^OFF);

	do {
		TurnOn(pOp);
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOn(CAxOutput& op, CAxInput& onIp, CAxInput& offIp,
					   DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
					   int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(onIp^ON);
	event.Add(offIp^OFF);

	do {
		TurnOn(op);
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnAllOn(CAxOutput* pFirstOp, ...)
{
	va_list eventList;

	ASSERT(pFirstOp != NULL);
	CAxOutput* pOutput = pFirstOp;

	va_start(eventList, pFirstOp);
	do {
		pOutput->On();
		pOutput = va_arg(eventList, CAxOutput*);
	} while (pOutput != NULL);
	va_end(eventList);
}


void CAxIOCtrl::TurnOff(CAxOutput* pOp)
{
	ASSERT(pOp != NULL);
	pOp->Off();
}


void CAxIOCtrl::TurnOff(CAxOutput& op)
{
	op.Off();
}


void CAxIOCtrl::TurnOff(CAxOutput* pOp, CAxInput* pIp, BOOL bValue,
						DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, 
						LPCTSTR pszSource, int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pOp != NULL && pIp != NULL);
	event.Add(bValue ? *pIp^ON : *pIp^OFF);

	do {
		TurnOff(pOp);
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOff(CAxOutput& op, CAxInput& ip, BOOL bValue,
						DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, 
						LPCTSTR pszSource, int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(bValue ? ip^ON : ip^OFF);

	do {
		TurnOff(op);
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOff(CAxOutput* pOp, CAxInput* pOnIp, CAxInput* pOffIp,
					    DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pOp != NULL && pOnIp != NULL && pOffIp != NULL);
	event.Add(*pOnIp^ON);
	event.Add(*pOffIp^OFF);

	do {
		TurnOff(pOp);
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnOff(CAxOutput& op, CAxInput& onIp, CAxInput& offIp,
					    DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
						int nCode)
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(onIp^ON);
	event.Add(offIp^OFF);

	do {
		TurnOff(op);
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::TurnAllOff(CAxOutput* pFirstOp, ...)
{
	va_list eventList;

	ASSERT(pFirstOp != NULL);
	CAxOutput* pOutput = pFirstOp;

	va_start(eventList, pFirstOp);
	do {
		pOutput->Off();
		pOutput = va_arg(eventList, CAxOutput*);
	} while (pOutput != NULL);
	va_end(eventList);
}


void CAxIOCtrl::Turn(CAxOutput* pOp, BOOL bValue)
{
	ASSERT(pOp != NULL);
	pOp->SetValue(bValue);
}


void CAxIOCtrl::Turn(CAxOutput& op, BOOL bValue)
{
	op.SetValue(bValue);
}


void CAxIOCtrl::Turns(CAxOutput* pFirstOp, BOOL bFirstValue, ...)
{
	va_list eventList;

	ASSERT(pFirstOp != NULL);
	CAxOutput* pOutput = pFirstOp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		pOutput->SetValue(bValue);
		pOutput = va_arg(eventList, CAxOutput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pOutput != NULL);
	va_end(eventList);
}


int CAxIOCtrl::WaitInputOn(CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^ON);
	return m_pControl->WaitEvents(INFINITE, event);
}


int CAxIOCtrl::WaitInputOn(CAxInput& ip)
{
	CAxEventPtrArray event;

	event.Add(ip^ON);
	return m_pControl->WaitEvents(INFINITE, event);
}


int CAxIOCtrl::WaitInputOn(DWORD dwTimeout, CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^ON);
	return m_pControl->WaitEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitInputOn(DWORD dwTimeout, CAxInput& ip)
{
	CAxEventPtrArray event;

	event.Add(ip^ON);
	return m_pControl->WaitEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitInputOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, 
							int nCode, CAxInput* pIp)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^ON);
	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitInputOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, 
							int nCode, CAxInput& ip)
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(ip^ON);
	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitInputOnNS(DWORD dwTimeout, CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^ON);
	return m_pControl->WaitEvents(FALSE, dwTimeout, event);
}


int CAxIOCtrl::WaitInputsOn(DWORD dwTimeout, CAxInput* pFirstIp, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;

	va_start(eventList, pFirstIp);
	do {
		event.Add(*pInput^ON);
		pInput = va_arg(eventList, CAxInput*);	
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitInputsOn(DWORD dwTimeout, CAxEventPtrArray& event)
{
	return m_pControl->WaitEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitInputsOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							 LPCTSTR pszSource, int nCode,
							 CAxInput* pFirstIp, ...)
{
	int retCode;
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;

	va_start(eventList, pFirstIp);
	do {
		event.Add(*pInput^ON);
		pInput = va_arg(eventList, CAxInput*);	
	} while (pInput != NULL);
	va_end(eventList);

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitInputsOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							 LPCTSTR pszSource, int nCode,
							 CAxEventPtrArray& event)
{
	int retCode;

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitInputOff(CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^OFF);
	return m_pControl->WaitEvents(INFINITE, event);
}


int CAxIOCtrl::WaitInputOff(CAxInput& ip)
{
	CAxEventPtrArray event;

	event.Add(ip^OFF);
	return m_pControl->WaitEvents(INFINITE, event);
}


int CAxIOCtrl::WaitInputOff(DWORD dwTimeout, CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^OFF);
	return m_pControl->WaitEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitInputOff(DWORD dwTimeout, CAxInput& ip)
{
	CAxEventPtrArray event;

	event.Add(ip^OFF);
	return m_pControl->WaitEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitInputOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
							 int nCode, CAxInput* pIp)
{
	int retCode;
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^OFF);
	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitInputOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource,
							 int nCode, CAxInput& ip)
{
	int retCode;
	CAxEventPtrArray event;

	event.Add(ip^OFF);
	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitInputOffNS(DWORD dwTimeout, CAxInput* pIp)
{
	CAxEventPtrArray event;

	ASSERT(pIp != NULL);
	event.Add(*pIp^OFF);
	return m_pControl->WaitEvents(FALSE, dwTimeout, event);
}


int CAxIOCtrl::WaitInputsOff(DWORD dwTimeout, CAxInput* pFirstIp, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;

	va_start(eventList, pFirstIp);
	do {
		event.Add(*pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);	
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitInputsOff(DWORD dwTimeout, CAxEventPtrArray& event)
{
	return m_pControl->WaitEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitInputsOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode,
							  CAxInput* pFirstIp, ...)
{
	int retCode;
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;

	va_start(eventList, pFirstIp);
	do {
		event.Add(*pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);	
	} while (pInput != NULL);
	va_end(eventList);

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitInputsOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode,
							  CAxEventPtrArray& event)
{
	int retCode;

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitInputs(CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitEvents(INFINITE, event);
}


int CAxIOCtrl::WaitInputs(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitInputs(DWORD dwTimeout, CAxEventPtrArray& event)
{
	return m_pControl->WaitEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						   LPCTSTR pszSource, int nCode,
						   CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	int retCode;
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						   LPCTSTR pszSource, int nCode, 
						   CAxEventPtrArray& event)
{
	int retCode;

	do {
		retCode = WaitTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
							  event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitAllInputs(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitAllEvents(dwTimeout, event);
}


int CAxIOCtrl::WaitAllInputs(DWORD dwTimeout, CAxEventPtrArray& event)
{
	return m_pControl->WaitAllEvents(dwTimeout, event);
}


void CAxIOCtrl::WaitAllInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode,
							  CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	int retCode;
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	do {
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


void CAxIOCtrl::WaitAllInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode,
							  CAxEventPtrArray& event)
{
	int retCode;

	do {
		retCode = WaitAllTimeout(TRUE, dwTimeout, nResp, pszPath, pszSource, nCode,
								 event);
	} while (retCode == emRetry);
}


int CAxIOCtrl::WaitInputsNS(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...)
{
	va_list eventList;
	CAxEventPtrArray event;

	ASSERT(pFirstIp != NULL);
	CAxInput* pInput = pFirstIp;
	BOOL bValue = bFirstValue;

	va_start(eventList, bFirstValue);
	do {
		event.Add(bValue ? *pInput^ON : *pInput^OFF);
		pInput = va_arg(eventList, CAxInput*);
		bValue = va_arg(eventList, BOOL);		
	} while (pInput != NULL);
	va_end(eventList);

	return m_pControl->WaitEvents(FALSE, dwTimeout, event);
}


int CAxIOCtrl::WaitTimeout(BOOL bAddStop, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
						   LPCTSTR pszSource, int nCode,
						   CAxEventPtrArray& event)
{
	if (m_pControl->WaitEvents(bAddStop, dwTimeout, event) == WAIT_TIMEOUT) {
		return m_pErrCtrl->Error(nCode, nResp, pszSource, pszPath);
	}
	return 0;
}


int CAxIOCtrl::WaitAllTimeout(BOOL bAddStop, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath,
							  LPCTSTR pszSource, int nCode, CAxEventPtrArray& event)
{
	if (m_pControl->WaitAllEvents(bAddStop, dwTimeout, event) == WAIT_TIMEOUT) {
		return m_pErrCtrl->Error(nCode, nResp, pszSource, pszPath);
	}
	return 0;
}