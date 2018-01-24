#include "stdafx.h"
#include "AxControl.h"
#include "AxErrMsg.h"
#include "AxStationHub.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxControl::CAxControl()
{
	m_nState = TS_INIT;
	m_nRunMode = 0;
	m_nResponse = emRetry;

	for( int i = 0; i < NUM_CTS; i++ ) {
		m_priEvt.Add(new CAxEvent());
		m_secEvt.Add(new CAxEvent());
	}

	m_priEvt[CT_CMD]->SetForceMode(FM_FORCE_OFF);
	m_priEvt[CT_ERROR]->SetForceMode(FM_FORCE_OFF);
	m_priEvt[CT_IDLE]->SetForceMode(FM_FORCE_ON);
	m_priEvt[CT_STOPPED]->SetForceMode(FM_FORCE_ON);
	m_priEvt[CT_DONE]->SetForceMode(FM_FORCE_ON);

	m_secEvt[CT_CMD]->SetForceMode(FM_FORCE_OFF);
	m_secEvt[CT_ERROR]->SetForceMode(FM_FORCE_OFF);
	m_secEvt[CT_IDLE]->SetForceMode(FM_FORCE_ON);
	m_secEvt[CT_STOPPED]->SetForceMode(FM_FORCE_ON);
	m_secEvt[CT_DONE]->SetForceMode(FM_FORCE_ON);

	m_evt.Copy(m_priEvt);
}

CAxControl::~CAxControl()
{
	for( int i = 0; i < NUM_CTS; i++ ) {
		delete m_priEvt[i];
		delete m_secEvt[i];
	}
}

void CAxControl::SetRunMode(UINT nMode)
{
	if( nMode == m_nRunMode ) return;

	m_evt.RemoveAll();
	if( nMode == SEC_RUN ) m_evt.Copy(m_secEvt);
	else m_evt.Copy(m_priEvt);
	m_nRunMode = nMode;
}

void CAxControl::Start()
{
	m_evt[CT_START]->Set();
}

void CAxControl::Stop()
{
	m_evt[CT_STOP]->Set();
}

void CAxControl::Abort()
{
	m_priEvt[CT_ABORT]->Set();
	m_secEvt[CT_ABORT]->Set();
}

void CAxControl::Reset()
{
	Reset(m_priEvt);
	Reset(m_secEvt);
}

void CAxControl::Reset(CAxEventPtrArray& event)
{
	int nSize = event.GetSize();
	for( int i = 0; i < nSize; i++ ) event[i]->Reset();
}

void CAxControl::Abort(CAxEventPtrArray& event)
{
	event[CT_ABORT]->Set();
}

int CAxControl::WaitResponse()
{
	CSyncObject* pSyncObject[2];

	int nPrevState = m_nState;
	m_nState = TS_ERROR;

	m_evt[CT_START]->Reset();

	pSyncObject[0] = m_evt[CT_ABORT]->GetEvent();
	pSyncObject[1] = m_evt[CT_START]->GetEvent();

	CMultiLock eventLock(pSyncObject, 2);

	DWORD dwRetVal = eventLock.Lock(INFINITE, FALSE);
	
	if( dwRetVal ) {
		m_nState = nPrevState;

		CheckStationState();
		PreStart(); //(M) 2014.07.17
		return m_nResponse;
	}
	else {
		CAxErrorCtrl::m_bError = FALSE;
		throw -1;
	}
}

void CAxControl::WaitStart()
{
	CSyncObject* pSyncObject[2];

	pSyncObject[0] = m_evt[CT_ABORT]->GetEvent();
	pSyncObject[1] = m_evt[CT_START]->GetEvent();

	CMultiLock eventLock(pSyncObject, 2);
	DWORD dwRetVal = eventLock.Lock(INFINITE, FALSE);
	
	if (dwRetVal) m_evt[CT_START]->Reset();
	else throw -1;

}

void CAxControl::WaitStart(CAxEventPtrArray& evt)
{
	CSyncObject* pSyncObject[2];

	pSyncObject[0] = evt[CT_ABORT]->GetEvent();
	pSyncObject[1] = evt[CT_START]->GetEvent();

	CMultiLock eventLock(pSyncObject, 2);
	DWORD dwRetVal = eventLock.Lock(INFINITE, FALSE);

	if (dwRetVal) evt[CT_START]->Reset();
	else throw -1;
}

int CAxControl::WaitStop(DWORD dwTime)
{
	CSyncObject* pSyncObject[2];

	pSyncObject[0] = m_evt[CT_ABORT]->GetEvent();
	pSyncObject[1] = m_evt[CT_STOP]->GetEvent();

	CMultiLock eventLock(pSyncObject, 2);
	DWORD dwRetVal = eventLock.Lock(dwTime, FALSE);
	
	if (dwRetVal) return dwRetVal;
	else throw -1;
}

int CAxControl::WaitEvents(DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitEvents(TRUE, FALSE, dwTimeout, evt);
}

int CAxControl::WaitEvents(BOOL bAddStop, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitEvents(bAddStop, FALSE, dwTimeout, evt);
}

int CAxControl::WaitEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitEvents(bAddStop, bReset, dwTimeout, evt);
}

int CAxControl::WaitAllEvents(DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitAllEvents(TRUE, FALSE, dwTimeout, evt);
}

int CAxControl::WaitAllEvents(BOOL bAddStop, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitAllEvents(bAddStop, FALSE, dwTimeout, evt);
}

int CAxControl::WaitAllEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	return MultiWaitAllEvents(bAddStop, bReset, dwTimeout, evt);
}

void CAxControl::Wait(DWORD nTime)
{
	CAxEventPtrArray event;
	MultiWaitEvents(TRUE, FALSE, nTime, event);
}

void CAxControl::WaitNS(DWORD nTime)
{
	CAxEventPtrArray event;
	MultiWaitEvents(FALSE, FALSE, nTime, event);
}

int CAxControl::MultiWaitEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	DWORD dwRetVal;
	CAxEvent* pEvent;
	CAxEventPtrArray event;
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	event.Add(m_evt[CT_ABORT]);
	if( bAddStop ) event.Add(m_evt[CT_STOP]);
	if( bReset ) Reset(evt);

	event.Append(evt);
	int nNumEvents = event.GetSize();

	if( nNumEvents == 1 ) {
		Sleep(dwTimeout);
		return WAIT_TIMEOUT;
	}

	for( int i = 0; i < nNumEvents; i++ ) pSyncObject[i] = event[i]->GetEvent();

	CMultiLock eventLock(pSyncObject, nNumEvents);

	while( TRUE ) {
		dwRetVal = eventLock.Lock(dwTimeout, FALSE);
		if( (dwRetVal >= WAIT_OBJECT_0) && (dwRetVal < (WAIT_OBJECT_0 + nNumEvents)) ) {
			pEvent = event[dwRetVal - WAIT_OBJECT_0];
			if( pEvent == m_evt[CT_STOP] ) {
				m_evt[CT_STOPPED]->Set();
				m_evt[CT_STOP]->Reset();

				WaitStart();
				CheckStationState();
				PreStart(); //(M) 2014.07.17
				eventLock.Unlock();
				continue;
			}
			else if( pEvent == m_evt[CT_ABORT] ) throw -1;
			else break;
		}
		else return dwRetVal;
	}
	return bAddStop ? (dwRetVal - 2) : (dwRetVal - 1);
}

int CAxControl::MultiWaitAllEvents(BOOL bAddStop, BOOL bReset, DWORD dwTimeout, CAxEventPtrArray& evt)
{
	DWORD dwRetVal;
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	if( bReset ) Reset(evt);
	int nNumEvents = evt.GetSize();

	if( nNumEvents == 0 ) {
		Sleep(dwTimeout);
		return WAIT_TIMEOUT;
	}

	for( int i = 0; i < nNumEvents; i++ ) pSyncObject[i] = evt[i]->GetEvent();

	CMultiLock eventLock(pSyncObject, nNumEvents);

	DWORD dwWaitTime = bAddStop ? WAIT_INTERVAL : dwTimeout;

	DWORD dwTmout = 0;
	while( TRUE ) {
		dwRetVal = eventLock.Lock(dwWaitTime, TRUE);
		if( dwRetVal == WAIT_OBJECT_0 ) break;
		dwTmout += dwWaitTime;
		if( dwTmout >= dwTimeout ) {
			dwRetVal = WAIT_TIMEOUT;
			break; 
		}
		else {
			if( WaitStop(WAIT_INTERVAL) == WAIT_TIMEOUT ) {
				eventLock.Unlock();
				dwTmout += WAIT_INTERVAL;
				continue;
			}
			else {
				m_evt[CT_STOPPED]->Set();
				m_evt[CT_STOP]->Reset();

				WaitStart();
				PreStart(); //(M) 2014.07.17
				CheckStationState();
				eventLock.Unlock();
				dwTmout = 0;
				continue;
			}
		}
	}
	return dwRetVal;
}

void CAxControl::CheckStationState()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();
	CAxStationHub* pStationHub = CAxStationHub::GetStationHub();
	
	UINT nMasterState = pMaster->GetState();
	if( (nMasterState == MS_MANUAL) || (nMasterState == MS_MANUAL_STOP) ) return;

	UINT nNumStation = pStationHub->GetNumStations();
	if( m_nStationNum < nNumStation ) {
		CAxStation* pStation = (CAxStation*)pStationHub->GetStation(m_nStationNum);
		pStation->CheckStationState();
	}
}

//(M) 2014.07.17
void CAxControl::PreStart()
{
	if( m_nStationNum == -1 ) return;

	CAxStation* pStation;
	UINT nNumStation = CAxStationHub::GetStationHub()->GetNumStations();

	if( m_nStationNum < nNumStation ) 
	{
		pStation = (CAxStation*)CAxStationHub::GetStationHub()->GetStation(m_nStationNum);
		pStation->PreStart();
	}
}

void CAxControl::SetStationNum(UINT nStation)
{
	m_nStationNum = nStation;
}