// AxMaster.cpp: implementation of the CAxMaster class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxMaster* CAxMaster::theMaster = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxMaster::CAxMaster()
{
	m_sName = _T("Master");
	m_nNumTasks = 0;
	m_nRunMode = 0;
	m_nState = MS_INIT;
	m_nPrevState = MS_INIT;
	m_nPreManual = MS_INIT;
	m_nPreError = MS_INIT;
	m_nMinErrSpan = 1000;
	m_nErrTask = NO_ERR_TASK;
	m_pErrorMgr = NULL;
	m_pStationHub = NULL;

	for (int i=0; i<NUM_CTS; i++)
		m_event.Add(new CAxEvent());

	m_profile.m_sIniFile = _T("\\Data\\Profile\\System\\System.ini");
}


CAxMaster::~CAxMaster()
{
	m_bTerminate = TRUE;
	m_event[CT_ABORT]->Set();
	DeleteThreads();
	for (int i=0; i<NUM_CTS; i++)
		delete m_event[i];
}


CAxMaster* CAxMaster::GetMaster()
{
	if (theMaster == NULL) 
		theMaster = new CAxMaster();

	return theMaster;
}


BOOL CAxMaster::GetTeminate()
{
	if( theMaster == NULL || m_bTerminate == TRUE )
		return TRUE;
	
	return FALSE;
}

UINT CAxMaster::GetState()
{
	return m_nState;
}

// 2004.11.27. B.M YU
UINT CAxMaster::GetPrevState()
{
	return m_nPrevState;
}

// 2004.11.27. B.M YU
UINT CAxMaster::GetPreManualState()
{
	return m_nPreManual;
}

void CAxMaster::Startup()
{
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxMaster"));
	
	CreateThreads();
}


void CAxMaster::Run()
{
	m_pErrorMgr = CAxErrorMgr::GetErrorMgr();

	PackAllTasks();
	InitTaskEvents();
	SetRunMode(PRI_RUN);
	SetState(MS_INIT);
	ResumeAxThread(m_hPriThread);
}


void CAxMaster::PackAllTasks()
{
	int i, nNumTasks;
	CAxServiceHub* pServiceHub = CAxServiceHub::GetServiceHub();
	nNumTasks = pServiceHub->GetNumServices();
	for (i=0; i<nNumTasks; i++) {
		m_task.Add(pServiceHub->GetService(i));
	}

	CAxSystemHub* pSystemHub = CAxSystemHub::GetSystemHub();
	nNumTasks = pSystemHub->GetNumSystem();
	for (i=0; i<nNumTasks; i++) {
		m_task.Add(pSystemHub->GetSystem(i));
	}

	m_pStationHub = CAxStationHub::GetStationHub();
	nNumTasks = m_pStationHub->GetNumStations();
	for (i=0; i<nNumTasks; i++) {
		m_task.Add(m_pStationHub->GetStation(i));
	}
	m_nNumTasks = m_task.GetSize();
}


void CAxMaster::Start()
{
	m_Trace.Log(_T("---- [Flow] CT_START Set"));

	m_event[CT_START]->Set();
	
	SendMessage(m_pMainWnd->GetSafeHwnd(), m_msgStart, 0, 0);
}


void CAxMaster::Stop()
{
	m_Trace.Log(_T("---- [Flow] CT_STOP Set"));

	m_event[CT_STOP]->Set();
	
	SendMessage(m_pMainWnd->GetSafeHwnd(), m_msgStop, 0, 0);
}


void CAxMaster::Abort()
{
	m_Trace.Log(_T("---- [Flow] CT_ABORT Set"));

	m_event[CT_ABORT]->Set();
	
	SendMessage(m_pMainWnd->GetSafeHwnd(), m_msgAbort, 0, 0);
}

void CAxMaster::Cmd()
{
	m_event[CT_CMD]->Set();
}

void CAxMaster::InitTaskEvents()
{
	m_start.SetSize(m_nNumTasks);
	m_stop.SetSize(m_nNumTasks);
	m_error.SetSize(m_nNumTasks);
	m_idle.SetSize(m_nNumTasks);
	m_stopped.SetSize(m_nNumTasks);
	m_cmd.SetSize(m_nNumTasks);
	m_done.SetSize(m_nNumTasks);
}

UINT CAxMaster::PriRun()
{
	SuspendAxThread(m_hPriThread);
	while (TRUE) {
		try {
			switch (m_nState) {
			case MS_INIT:
				InitState();
				break;
			case MS_IDLE:
				IdleState();
				break;
			case MS_SETUP:
				SetupState();
				break;
			case MS_READY:
				ReadyState();
				break;
			case MS_AUTO:
				AutoState();
				break;
			case MS_MANUAL:
				ManualState();
				break;
			case MS_AUTO_STOP:
				AutoStopState();
				break;
			case MS_MANUAL_STOP:
				ManualStopState();
				break;
			case MS_ERROR:
				ErrorState();
				break;
			default:
				ASSERT(FALSE);
			}
		} catch (int nExp) {
			Sleep(50);
			if (nExp == -1) {
				if (m_bTerminate) AfxEndThread(0);
				AbortTasks();
				SetState(MS_INIT);
				ResetEvents(m_event);
				SetRunMode(PRI_RUN);
			}
		}
	}
	return 0;
}

void CAxMaster::InitState()
{
	m_Trace.Log(_T("[m_nState] Init State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_IDLE], NULL);

	if (pEvent == m_event[CT_IDLE]) {
		m_Trace.Log(_T("---- [Event] Init : CT_IDLE"));

		SetState(MS_IDLE);
	}
	else{
		m_Trace.Log(_T("---- [Event] Init : not defined"));
	}
}

void CAxMaster::IdleState()
{
	m_Trace.Log(_T("[m_nState] Idle State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_START], m_event[CT_CMD], 
								  /*m_event[CT_ERROR],*/ NULL);

	if (pEvent == m_event[CT_START]) {
		m_Trace.Log(_T("---- [Event] Idle : CT_START"));

		//Only single start button should start working
		//SetSetupCmd();
		//StartTasks(SEC_RUN);
		//SetState(MS_SETUP);
		StartTasks(PRI_RUN);
		SetState(MS_AUTO);

	}
	else if (pEvent == m_event[CT_CMD]) {
		m_Trace.Log(_T("---- [Event] Idle : CT_CMD"));

		StartTasks(SEC_RUN);
		m_nPreManual = MS_IDLE;
		SetState(MS_MANUAL);
	}
	else{
		m_Trace.Log(_T("---- [Event] Idle : not defined"));
	}
}

void CAxMaster::SetupState() // 수행 안함
{
	m_Trace.Log(_T("[m_nState] Setup State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_ERROR], m_event[CT_DONE],
								  m_event[CT_STOP], NULL);

	if (pEvent == m_event[CT_ERROR]) {
		m_Trace.Log(_T("---- [Event] Setup : CT_ERROR"));

		StopTasks();
		RaiseError();
		m_nPreError = MS_SETUP;
		SetState(MS_ERROR);
	}
	else if (pEvent == m_event[CT_DONE]) {
		m_Trace.Log(_T("---- [Event] Setup : CT_DONE"));

		SetState(MS_READY);
	}
	else if (pEvent == m_event[CT_STOP]) {
		m_Trace.Log(_T("---- [Event] Setup : CT_STOP"));

		StopTasks();
		SetState(MS_MANUAL_STOP);
	}
	else{
		m_Trace.Log(_T("---- [Event] Setup : not defined"));
	}
}

void CAxMaster::ReadyState() // 수행 안함
{
	m_Trace.Log(_T("[m_nState] Ready State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_ERROR], m_event[CT_START], 
								  m_event[CT_CMD], NULL);

	if (pEvent == m_event[CT_ERROR]) {
		m_Trace.Log(_T("---- [Event] Ready : CT_ERROR"));

		StopTasks();
		RaiseError();
		m_nPreError = MS_READY;
		SetState(MS_ERROR);
	}
	else if (pEvent == m_event[CT_START]) {
		m_Trace.Log(_T("---- [Event] Ready : CT_START"));

		StartTasks(PRI_RUN);
		SetState(MS_AUTO);
	}
	else if (pEvent == m_event[CT_CMD]) {
		m_Trace.Log(_T("---- [Event] Ready : CT_CMD"));

		StartTasks(SEC_RUN);
		m_nPreManual = MS_READY;
		SetState(MS_MANUAL);
	}
	else{
		m_Trace.Log(_T("---- [Event] Ready : not defined"));
	}
}

void CAxMaster::AutoState()
{
	m_Trace.Log(_T("[m_nState] Auto State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_ERROR], m_event[CT_STOP], NULL);

	if (pEvent == m_event[CT_ERROR]) {
		m_Trace.Log(_T("---- [Event] Auto : CT_ERROR"));

		StopTasks();
		RaiseError();
		m_nPreError = MS_AUTO;
		SetState(MS_ERROR);
	}
	else if (pEvent == m_event[CT_STOP]) {
		m_Trace.Log(_T("---- [Event] Auto : CT_STOP"));

		StopTasks();
		SetState(MS_AUTO_STOP);
	}
	else{
		m_Trace.Log(_T("---- [Event] Auto : not defined"));
	}
}

void CAxMaster::ManualState()
{
	m_Trace.Log(_T("[m_nState] Manual State"));

	CAxEvent* pEvent = WaitEvents(/*m_event[CT_ERROR],*/ m_event[CT_DONE],
								  m_event[CT_STOP], NULL);

	if (pEvent == m_event[CT_DONE]) {
		m_Trace.Log(_T("---- [Event] Manual : CT_DONE"));

		SetState(m_nPreManual);
	}
	else if (pEvent == m_event[CT_STOP]) {
		m_Trace.Log(_T("---- [Event] Manual : CT_STOP"));

		StopTasks();
		SetState(MS_MANUAL_STOP);
	}
//	else if (pEvent == m_event[CT_ERROR]) {
//		m_Trace.Log(_T("---- [Event] Manual : CT_ERROR"));
//
//		StopTasks();
//		RaiseError();
//		m_nPreError = MS_MANUAL;
//		SetState(MS_ERROR);
//	}
	else{
		m_Trace.Log(_T("---- [Event] Manual : not defined"));
	}
}

void CAxMaster::AutoStopState()	
{
	m_Trace.Log(_T("[m_nState] AutoStop State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_ERROR], m_event[CT_START],
								  m_event[CT_CMD], NULL);

	if (pEvent == m_event[CT_ERROR]) { // 안들어올것 같은데
		m_Trace.Log(_T("---- [Event] AutoStop : CT_ERROR"));

//		StopTasks();
		RaiseError();
		m_nPreError = MS_AUTO_STOP;
		SetState(MS_ERROR);
	}
	else if (pEvent == m_event[CT_START]) {
		m_Trace.Log(_T("---- [Event] AutoStop : CT_START"));

		StartTasks(PRI_RUN);
		SetState(MS_AUTO);
	}
	else if (pEvent == m_event[CT_CMD]) {
		m_Trace.Log(_T("---- [Event] AutoStop : CT_CMD"));

		StartTasks(SEC_RUN);
		m_nPreManual = MS_AUTO_STOP;
		SetState(MS_MANUAL);
	}
	else{
		m_Trace.Log(_T("---- [Event] AutoStop : not defined"));
	}
}

void CAxMaster::ManualStopState()
{
	m_Trace.Log(_T("[m_nState] ManualStop State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_ERROR], m_event[CT_START], NULL);

	if (pEvent == m_event[CT_ERROR]) { // 안들어올것 같은데
		m_Trace.Log(_T("---- [Event] ManualStop : CT_ERROR"));

//		StopTasks();
		RaiseError();
		m_nPreError = MS_MANUAL_STOP;
		SetState(MS_ERROR);
	}
	else if (pEvent == m_event[CT_START]) {
		m_Trace.Log(_T("---- [Event] ManualStop : CT_START"));

		StartTasks(SEC_RUN);	
		SetState(m_nPrevState);
	}
	else{
		m_Trace.Log(_T("---- [Event] ManualStop : not defined"));
	}
}

void CAxMaster::ErrorState()
{
	m_Trace.Log(_T("[m_nState] Error State"));

	CAxEvent* pEvent = WaitEvents(m_event[CT_START], m_event[CT_CMD], NULL);

	if (pEvent == m_event[CT_START]) {
		m_Trace.Log(_T("---- [Event] Error : CT_START"));

		if(m_nPreError == MS_MANUAL || m_nPreError == MS_MANUAL_STOP)
			ResumeTasks(SEC_RUN);
		else
			ResumeTasks(PRI_RUN);
		StartTasks(m_nRunMode);
		SetState(m_nPreError);
	}
	else if (pEvent == m_event[CT_CMD]) {  
		m_Trace.Log(_T("---- [Event] Error : CT_CMD"));

		StartTasks(SEC_RUN);
		m_nPreManual = MS_ERROR;
		SetState(MS_MANUAL);
	}  
	else{
		m_Trace.Log(_T("---- [Event] Error : not defined"));
	}
}

// START TASK 실행 시 사용
void CAxMaster::StartTasks(UINT nMode)
{
	SetRunMode(nMode);
	SetTaskEvents(FALSE, m_stop);
	SetTaskEvents(TRUE, m_start);
	m_event[CT_STOP]->Reset();
}

void CAxMaster::StopTasks()
{
	SetTaskEvents(FALSE, m_start);
	SetTaskEvents(TRUE, m_stop);

	m_Trace.Log(_T("--------- [Monitor] StopTasks() : before m_stopped"));
	while (MonitorEvents(TRUE, m_stopped) < 0);
	m_Trace.Log(_T("--------- [Monitor] StopTasks() : m_stopped"));

	SetTaskEvents(FALSE, m_stopped);
	RunPostStop();
}

// 정상 STOP + ERROR STOP 후 사용
void CAxMaster::RunPostStop()
{
	if (m_nErrTask == NO_ERR_TASK) {
		m_pStationHub->PostStop(ST_CMD_STOP);
	}
	else if (m_task[m_nErrTask]->m_sName == _T("SystemError")) {
		switch (m_task[m_nErrTask]->m_nErrCode) {
		case ERR_SE_ESTOP_ON:
			m_pStationHub->PostStop(ST_EMG_STOP);
			break;

		case ERR_SE_POWER_OFF:
			m_pStationHub->PostStop(ST_POWER_STOP);
			break;

		case ERR_SE_AIR_MAIN_OFF:
		case ERR_SE_AIR_SUB_OFF:
			m_pStationHub->PostStop(ST_AIR_STOP);
			break;

		case ERR_SE_AREA_1_OFF:
		case ERR_SE_AREA_2_OFF:
		case ERR_SE_AREA_3_OFF:
		case ERR_SE_AREA_4_OFF:
		case ERR_SE_AREA_5_OFF:
		case ERR_SE_AREA_6_OFF:
		case ERR_SE_AREA_7_OFF:
		case ERR_SE_AREA_8_OFF:
		case ERR_SE_AREA_9_OFF:
		case ERR_SE_AREA_10_OFF:
			m_pStationHub->PostStop(ST_AREA_STOP);
			break;

		case ERR_SE_DOOR_1_OFF:
		case ERR_SE_DOOR_2_OFF:
		case ERR_SE_DOOR_3_OFF:
		case ERR_SE_DOOR_4_OFF:
		case ERR_SE_DOOR_5_OFF:
		case ERR_SE_DOOR_6_OFF:
		case ERR_SE_DOOR_7_OFF:
		case ERR_SE_DOOR_8_OFF:
		case ERR_SE_DOOR_9_OFF:
		case ERR_SE_DOOR_10_OFF:
		case ERR_SE_DOOR_11_OFF:
		case ERR_SE_DOOR_12_OFF:
		case ERR_SE_DOOR_13_OFF:
		case ERR_SE_DOOR_14_OFF:
		case ERR_SE_DOOR_15_OFF:
		case ERR_SE_DOOR_16_OFF:
		case ERR_SE_DOOR_17_OFF:
		case ERR_SE_DOOR_18_OFF:
		case ERR_SE_DOOR_19_OFF:
		case ERR_SE_DOOR_20_OFF:
			m_pStationHub->PostStop(ST_DOOR_STOP);
			break;
		}
	}
	else {
		m_pStationHub->PostStop(ST_ERR_STOP);
	}
}

void CAxMaster::AbortTasks()
{
	for (int i=0; i<m_nNumTasks; i++)
		m_task[i]->Abort();
}

void CAxMaster::ResetTasks()
{
	for (int i=0; i<m_nNumTasks; i++)
		m_task[i]->Reset();
}

// ERROR 발생 후 진행 시 사용
void CAxMaster::ResumeTasks(UINT nMode)
{
	SetRunMode(nMode);

	switch(m_nState){
	case MS_ERROR:
		m_pErrorMgr->ClearError();

		// 여러 station 에서 동시에 에러가 발생했을시 동시에 에러 클리어하기 위해
		// 모든 station 에 emRetry response 로 - 추가
		for(int i=0; i<m_nNumTasks; i++){
			m_task[i]->SetResponse(emRetry);
		}

		// 에러난 station 은 사용자가 선택한 response 로 - 유지
		UINT nResp = m_pErrorMgr->GetResponse();
		if(nResp == 0) ASSERT(FALSE);

		m_task[m_nErrTask]->SetResponse(nResp);
		while(m_task[m_nErrTask]->GetResponse() != nResp) {
			Sleep(50);
			m_task[m_nErrTask]->SetResponse(nResp);
		}

		// 에러난 station 만 에러 리셋 - 삭제
		//m_error[m_nErrTask]->Reset();

		// 모든 station 에러 리셋 - 추가 
		SetTaskEvents(FALSE, m_error);

		m_event[CT_ERROR]->Reset();

		RunPreResume();

		m_nErrTask = NO_ERR_TASK;
		m_timer.Start();

//	default:
//			m_task[m_nErrTask]->Start();
	}
}

void CAxMaster::RunPreResume()
{
//	CAxSafety* pSafety = (CAxSafety*)CAxSystemHub::GetSystemHub()->GetSystem("Safety");
	
	if (m_nErrTask == NO_ERR_TASK) {
		m_pStationHub->PreResume(ST_CMD_STOP);
	}
	else if (m_task[m_nErrTask]->m_sName == _T("SystemError")) {
		switch (m_task[m_nErrTask]->m_nErrCode) {
		case ERR_SE_ESTOP_ON:
			m_pStationHub->PreResume(ST_EMG_STOP);
			break;

		case ERR_SE_POWER_OFF:
			m_pStationHub->PreResume(ST_POWER_STOP);
			break;

		case ERR_SE_AIR_MAIN_OFF:
		case ERR_SE_AIR_SUB_OFF:
			m_pStationHub->PreResume(ST_AIR_STOP);
			break;

		case ERR_SE_AREA_1_OFF:
		case ERR_SE_AREA_2_OFF:
		case ERR_SE_AREA_3_OFF:
		case ERR_SE_AREA_4_OFF:
		case ERR_SE_AREA_5_OFF:
		case ERR_SE_AREA_6_OFF:
		case ERR_SE_AREA_7_OFF:
		case ERR_SE_AREA_8_OFF:
		case ERR_SE_AREA_9_OFF:
		case ERR_SE_AREA_10_OFF:
			m_pStationHub->PreResume(ST_AREA_STOP);
			break;

		case ERR_SE_DOOR_1_OFF:
		case ERR_SE_DOOR_2_OFF:
		case ERR_SE_DOOR_3_OFF:
		case ERR_SE_DOOR_4_OFF:
		case ERR_SE_DOOR_5_OFF:
		case ERR_SE_DOOR_6_OFF:
		case ERR_SE_DOOR_7_OFF:
		case ERR_SE_DOOR_8_OFF:
		case ERR_SE_DOOR_9_OFF:
		case ERR_SE_DOOR_10_OFF:
		case ERR_SE_DOOR_11_OFF:
		case ERR_SE_DOOR_12_OFF:
		case ERR_SE_DOOR_13_OFF:
		case ERR_SE_DOOR_14_OFF:
		case ERR_SE_DOOR_15_OFF:
		case ERR_SE_DOOR_16_OFF:
		case ERR_SE_DOOR_17_OFF:
		case ERR_SE_DOOR_18_OFF:
		case ERR_SE_DOOR_19_OFF:
		case ERR_SE_DOOR_20_OFF:
			m_pStationHub->PreResume(ST_DOOR_STOP);
			break;
		}
	}
	else {
		m_pStationHub->PreResume(ST_ERR_STOP);
	}
}

void CAxMaster::SetSetupCmd()
{
	for (int i=0; i<m_nNumTasks; i++)
		m_task[i]->SetCmdCode(1);
}

void CAxMaster::SetRunMode(UINT nMode)
{
	if (nMode == m_nRunMode) return;

	for (int i=0; i<m_nNumTasks; i++) {
		m_task[i]->SetRunMode(nMode);
		m_start[i]	 = m_task[i]->GetCtrlEvent(CT_START);
		m_stop[i]	 = m_task[i]->GetCtrlEvent(CT_STOP);
		m_error[i]	 = m_task[i]->GetCtrlEvent(CT_ERROR);
		m_idle[i]	 = m_task[i]->GetCtrlEvent(CT_IDLE);
		m_stopped[i] = m_task[i]->GetCtrlEvent(CT_STOPPED);
		m_cmd[i]	 = m_task[i]->GetCtrlEvent(CT_CMD);
		m_done[i]	 = m_task[i]->GetCtrlEvent(CT_DONE);
	}
	m_nRunMode = nMode;
}

void CAxMaster::SetTaskEvents(BOOL bSet, CAxEventPtrArray& event)
{
	if (bSet) SetEvents(event);
	else ResetEvents(event);
}

void CAxMaster::RaiseError()
{
	while (!m_timer.IsTimeUp(m_nMinErrSpan)) Sleep(50);
	m_pErrorMgr->RaiseError((CAxErrData)m_task[m_nErrTask]->m_errMsg);
	SendMessage(m_pMainWnd->GetSafeHwnd(), m_msgErr, 0, 0);
}

void CAxMaster::SetState(UINT nState)
{
	m_nPrevState = m_nState;
	m_nState = nState;
}

void CAxMaster::SetEvents(CAxEventPtrArray& event)
{
	int nSize = event.GetSize();
	for (int i=0; i<nSize; i++) event[i]->Set();
}

void CAxMaster::ResetEvents(CAxEventPtrArray& event)
{
	int nSize = event.GetSize();
	for (int i=0; i<nSize; i++) event[i]->Reset();
}

CAxEvent* CAxMaster::WaitEvents(CAxEvent* pFirstEvent, ...)
{
	va_list eventList;
	CAxEventPtrArray event;
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	CAxEvent* pEvent = pFirstEvent;

	if (pEvent != NULL) {
		va_start(eventList, pFirstEvent);
		do {
			event.Add(pEvent);
			pEvent = va_arg(eventList, CAxEvent*);
		} while (pEvent != NULL);
		va_end(eventList);
	}

	ResetEvents(event);

	event.InsertAt(0, m_event[CT_ABORT]);
	int nNumEvents = event.GetSize();
	if (nNumEvents > MAX_WAIT_EVTS) ASSERT(FALSE);

    for (int i=0; i<nNumEvents; i++) {
		pSyncObject[i] = event[i]->GetEvent();
	}

	CMultiLock eventLock(pSyncObject, nNumEvents);

	ResumeAxThread(m_hSecThread);
	DWORD dwRetVal = eventLock.Lock(INFINITE, FALSE);
	pEvent = event[dwRetVal - WAIT_OBJECT_0];
	if (pEvent == m_event[CT_ABORT]) throw -1;
	pEvent->Reset();

	Sleep(10);
	SuspendAxThread(m_hSecThread);
	return pEvent;
}

UINT CAxMaster::SecRun()
{
	int nRetCode;

	SuspendAxThread(m_hSecThread);
	while (TRUE) {
		switch (m_nState) {
			case MS_INIT:
				if (MonitorEvents(TRUE, m_idle) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Init : m_idle"));
					SetTaskEvents(FALSE, m_idle);
					Sleep(10);
					m_event[CT_IDLE]->Set();
				}
				break;

			case MS_IDLE:
				if (MonitorEvents(FALSE, m_cmd) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Idle : m_cmd"));
					SetTaskEvents(FALSE, m_cmd);
					m_event[CT_CMD]->Set();
				}
				break;

			case MS_SETUP: // 수행 안함
				if ((nRetCode = MonitorEvents(FALSE, m_error)) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Setup : m_error"));
					SetTaskEvents(FALSE, m_error); // by pjs 2006.03.10
					m_nErrTask = nRetCode;
					m_event[CT_ERROR]->Set();
					break;
				}
				if (MonitorEvents(TRUE, m_done) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Setup : m_done"));
					SetTaskEvents(FALSE, m_done);
					m_event[CT_DONE]->Set();
				}
				break;

			case MS_READY: // 수행 안함
				if ((nRetCode = MonitorEvents(FALSE, m_error)) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Ready : m_error"));
					SetTaskEvents(FALSE, m_error); // by pjs 2006.03.10
					m_nErrTask = nRetCode;
					m_event[CT_ERROR]->Set();
					break;				
				}
				if (MonitorEvents(FALSE, m_cmd) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Ready : m_cmd"));
					SetTaskEvents(FALSE, m_cmd);
					m_event[CT_CMD]->Set();
				}
				break;

			case MS_AUTO:
				if ((nRetCode = MonitorEvents(FALSE, m_error)) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Auto : m_error"));
					SetTaskEvents(FALSE, m_error); // by pjs 2006.03.10
					m_nErrTask = nRetCode;
					m_event[CT_ERROR]->Set();
				}
				break;

			case MS_MANUAL:
//				if ((nRetCode = MonitorEvents(FALSE, m_error)) >= 0) {
//					m_Trace.Log(_T("---- [Monitor] Manual : m_error"));
//					SetTaskEvents(FALSE, m_error); // by pjs 2006.03.10
//					m_nErrTask = nRetCode;
//					m_event[CT_ERROR]->Set();
//					break;
//				}
 				if (MonitorEvents(TRUE, m_done) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Manual : m_done"));
					SetTaskEvents(FALSE, m_done);
					m_event[CT_DONE]->Set();
				}
				break;

			case MS_AUTO_STOP:
				if (MonitorEvents(FALSE, m_cmd) >= 0) {
					m_Trace.Log(_T("---- [Monitor] AutoStop : m_cmd"));
					SetTaskEvents(FALSE, m_cmd);
					m_event[CT_CMD]->Set();
				}
				break;

			case MS_ERROR:
				if (MonitorEvents(FALSE, m_cmd) >= 0) {
					m_Trace.Log(_T("---- [Monitor] Error : m_cmd"));
					SetTaskEvents(FALSE, m_cmd);
					m_event[CT_CMD]->Set();
				}
				break;
				
			default:
				if (m_bTerminate) AfxEndThread(0);
				else Sleep(100);
		}
	}
	return 0;
}

int CAxMaster::MonitorEvents(BOOL bWaitAll, CAxEventPtrArray& event)
{
	CSyncObject* pSyncObject[MAX_WAIT_EVTS];

	if (m_bTerminate) AfxEndThread(0);

	int nNumEvents = event.GetSize();
	if (nNumEvents > MAX_WAIT_EVTS) ASSERT(FALSE);

    for (int i=0; i<nNumEvents; i++) {
		pSyncObject[i] = event[i]->GetEvent();
	}

	CMultiLock eventLock(pSyncObject, nNumEvents);

	DWORD dwRetVal = eventLock.Lock(100, bWaitAll);

	if (dwRetVal >= WAIT_OBJECT_0 && dwRetVal < WAIT_OBJECT_0+nNumEvents)
		return dwRetVal;
	else
		return -1;
}

void CAxMaster::SetResponse(int nResponse)
{
	m_task[m_nErrTask]->m_control.m_nResponse = nResponse;	
}

void CAxMaster::SetMainWnd(CWnd* pWnd, UINT msgErr, UINT msgStart, UINT msgStop, UINT msgAbort, UINT msgMessage, UINT msgErrMessage)
{
	m_pMainWnd = pWnd;
	m_msgErr = msgErr;
	m_msgStart = msgStart;
	m_msgStop = msgStop;
	m_msgAbort = msgAbort;
	m_msgMessage = msgMessage;
	m_msgErrMessage = msgErrMessage;
}

CWnd* CAxMaster::GetMainWnd()
{
	return m_pMainWnd;
}

