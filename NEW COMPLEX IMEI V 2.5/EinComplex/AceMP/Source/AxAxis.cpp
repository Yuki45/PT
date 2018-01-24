#include "stdafx.h"
#include "AxAxis.h"
#include "AxSystemHub.h"
#include "AxSystemError.h"
#include "AxTimer.h"
#include "MMCWHP154.h"
#include "AxMaster.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxAxis::CAxAxis()
{
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\MmcMgr.ini");
	
	m_nAxis = 0;
	m_nCtr = 0;
	m_nAxis = 0;
	m_nHomeDir = 0;
	m_nEncoderChk = 0;
	m_nHomeConfig = 0;
	m_dHomePos = 0.0;
	m_dParkPos = 0.0;
	m_dHomeToIndex = 0.0;
	m_dCntPerIndex = 0.0;
	m_dFastHomeSpeed = 0.0;
	m_dSlowHomeSpeed = 0.0;
	m_dPosPositionLim = 0.0;
	m_dNegPositionLim = 0.0;
	m_dPosErrLim = 0.0;
	m_dInPosLim = 0.0;
	m_dMinSpeed = 0.0;
	m_dMaxSpeed = 0.0;
	m_dJogSpeed = 0.0;
	m_nMinAccel = 0;
	m_nMaxAccel = 0;
	m_nJogAccel = 0;
	m_nMinDecel = 0;
	m_nMaxDecel = 0;
	m_dScale = 0.0;
	m_dCoarseTol = 0.0;
	m_dFineTol = 0.0;
	m_dStopDecel = 0.0;
	m_dEStopDecel = 0.0;
	m_dInPosTol = 0.0;
	m_nAxisError = axisNoError;
	m_bHome = FALSE;
	m_bIsBreakType = FALSE;
	m_bIsRotate = FALSE;
}	

CAxAxis::~CAxAxis()
{
}

BOOL CAxAxis::Init(int nAxis, LPCTSTR pszFile)
{
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxAxis"));

	m_pCmd = CAxMmcCmd::GetMmcCmd();
	m_nAxis = nAxis;

	GetProfile();

	m_CurLoc.pos = 0.0;
	m_CurLoc.speed = m_dMinSpeed;
	m_CurLoc.accel = m_nMinAccel;
	m_CurLoc.decel = m_nMinDecel;
	m_dSpeed = WorldToCnt(m_dMaxSpeed);
	m_nAccel = m_nMaxAccel;
	m_nDecel = m_nMaxDecel;

	return TRUE;
}

void CAxAxis::GetProfile()
{
	m_profile.m_sSect.Format(_T("Axis%d"), m_nAxis);

	m_nCtr				= m_profile.ReadInt(_T("CtrNum"));
	m_nHomeDir			= m_profile.ReadInt(_T("HomeDir"));
	m_nHomeSeq			= m_profile.ReadInt(_T("HomeSeq"));
	m_dHomePos			= m_profile.ReadInt(_T("HomePos"));
	m_dParkPos			= m_profile.ReadInt(_T("ParkPos"));
	m_dHomeToIndex		= m_profile.ReadInt(_T("HomeToIndex"));
	m_dCntPerIndex		= m_profile.ReadInt(_T("CntPerIndex"));
	m_dSlowHomeSpeed	= m_profile.ReadInt(_T("SlowHomeSpeed"));
	m_dFastHomeSpeed	= m_profile.ReadInt(_T("FastHomeSpeed"));
	m_dMinSpeed			= m_profile.ReadInt(_T("MinSpeed"));
	m_dMaxSpeed			= m_profile.ReadInt(_T("MaxSpeed"));
	m_nMinAccel			= m_profile.ReadInt(_T("MinAccel"));
	m_nMaxAccel			= m_profile.ReadInt(_T("MaxAccel"));
	m_nMinDecel			= m_profile.ReadInt(_T("MinDecel"));
	m_nMaxDecel			= m_profile.ReadInt(_T("MaxDecel"));
	m_dJogSpeed			= m_profile.ReadInt(_T("JogSpeed"));
	m_dMaxDist			= m_profile.ReadInt(_T("MaxDist"));
	m_dScale			= m_profile.ReadDouble(_T("Scale"));
	m_dCoarseTol		= m_profile.ReadInt(_T("CoarseTol"));
	m_dFineTol			= m_profile.ReadInt(_T("FineTol"));
	m_nEncoderChk		= m_profile.ReadInt(_T("EncoderChk"));
	m_nAutoAmpOn		= m_profile.ReadInt(_T("AutoAmpOn"));
	m_sName				= m_profile.ReadStr(_T("Name"));
	m_bSimulate			= m_profile.ReadBool(_T("Simulate"));
	m_bIsBreakType		= m_profile.ReadBool(_T("IsBreakType"));
	m_bIsRotate			= m_profile.ReadBool(_T("IsRotate"));

	m_profile.SetupOutput(m_profile.m_sSect, _T("opBreak"), m_opBreak);
}

void CAxAxis::SaveProfile()
{
	m_profile.m_sSect.Format(_T("Axis%d"), m_nAxis);
	m_profile.WriteDouble(_T("FastHomeSpeed"), m_dFastHomeSpeed);
	m_profile.WriteDouble(_T("MinSpeed"), m_dMinSpeed);
	m_profile.WriteDouble(_T("MaxSpeed"), m_dMaxSpeed);
	m_profile.WriteInt(_T("MaxAccel"), m_nMaxAccel);
	m_profile.WriteInt(_T("MaxDecel"), m_nMaxDecel);
	m_profile.WriteDouble(_T("JogSpeed"), m_dJogSpeed);
}

void CAxAxis::HandleException(UINT nExp)
{
	m_Trace.Log(_T("[%02d Axis] HandleException(%d)"), m_nAxis, nExp);

	m_nAxisError = nExp;
}

void CAxAxis::SetAxisError(UINT nErr)
{
	m_nAxisError = nErr;
}

BOOL CAxAxis::SetParam()
{
	try {
		m_pCmd->SetInPosition(m_nAxis, m_dCoarseTol);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

double CAxAxis::WorldToCnt(double dVal)
{
	return dVal * m_dScale;
}

AxisLoc CAxAxis::WorldToCnt(AxisLoc loc)
{
	AxisLoc aLoc;

	aLoc.pos = WorldToCnt(loc.pos);
	aLoc.speed = WorldToCnt(loc.speed);
	aLoc.accel = (int)WorldToCnt(loc.accel);
	aLoc.decel = (int)WorldToCnt(loc.decel);

	return aLoc;
}

void CAxAxis::SetMoveSpeed(double dSpeed)
{
	if(dSpeed <= 0) dSpeed = minSpeed;
	m_dSpeed = WorldToCnt(dSpeed);
}

void CAxAxis::SetMoveAccel(int nAccel)
{
	m_nAccel = nAccel;
}

void CAxAxis::SetMoveDecel(int nDecel)
{
	m_nDecel = nDecel;
}

BOOL CAxAxis::SetCoarseTol()
{
	BOOL bSuccess = TRUE;

	bSuccess = SetInPosTol(m_dCoarseTol);
	if( !bSuccess ) return FALSE;

	return TRUE;
}

BOOL CAxAxis::SetFineTol()
{
	BOOL bSuccess = TRUE;

	bSuccess = SetInPosTol(m_dFineTol);
	if( !bSuccess ) return FALSE;

	return TRUE;
}

BOOL CAxAxis::SetInPosTol(double dTol)
{
	if( m_bSimulate ) return TRUE;

	try	{
		m_dInPosTol = dTol;
		m_pCmd->SetInPosition(m_nAxis, WorldToCnt(dTol));
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

void CAxAxis::SetMinSpeed(double nMinSpeed)
{
	m_dMinSpeed = nMinSpeed;
}

double CAxAxis::GetMinSpeed()
{
	return m_dMinSpeed;
}

void CAxAxis::SetMaxSpeed(double nMaxSpeed)
{
	m_dMaxSpeed = nMaxSpeed;
}

double CAxAxis::GetMaxSpeed()
{
	return m_dMaxSpeed;
}

void CAxAxis::SetMaxAccel(int nMaxAccel)
{
	m_nMaxAccel = nMaxAccel;
}

int CAxAxis::GetMaxAccel()
{
	return m_nMaxAccel;
}

void CAxAxis::SetMaxDecel(int nMaxDecel)
{
	m_nMaxDecel = nMaxDecel;
}

int CAxAxis::GetMaxDecel()
{
	return m_nMaxDecel;
}

void CAxAxis::SetFastHomeSpeed(double nFastHomeSpeed)
{
	m_dFastHomeSpeed = nFastHomeSpeed;
}

double CAxAxis::GetFastHomeSpeed()
{
	return m_dFastHomeSpeed;
}

void CAxAxis::SetJogSpeed(double nJogSpeed)
{
	m_dJogSpeed = nJogSpeed;
}

double CAxAxis::GetJogSpeed()
{
	return m_dJogSpeed;
}

BOOL CAxAxis::IsAmpEnabled()
{
	if( m_bSimulate ) return TRUE;

	UINT nState;
	BOOL bBreak;

	try {
		m_pCmd->GetAmpEnable(m_nAxis, &nState);

		if( m_bIsBreakType ) bBreak = m_opBreak.GetValue();
		else bBreak = ON;

		if( !(((BOOL)nState == ON) && (bBreak == ON)) ) {
			SetAxisError(axisNotAmpEnabled);
			return FALSE;
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::IsAmpDisabled()
{
	if( m_bSimulate ) return FALSE;
	
	UINT nState;
	BOOL bBreak;

	try {
		m_pCmd->GetAmpEnable(m_nAxis, &nState);

		if( m_bIsBreakType ) bBreak = m_opBreak.GetValue();
		else bBreak = OFF;

		if( !(((BOOL)nState == OFF) && (bBreak == OFF)) ) {
			SetAxisError(axisNotAmpDisabled);
			return FALSE;
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::IsHomeCheck()
{
	return m_bHome;
}

BOOL CAxAxis::IsAxisDone()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	double dTargetPos, dEncoderPos;

	try {
		bSuccess = CheckReady();
		if( !bSuccess ) return FALSE;

		if( m_pCmd->AxisDone(m_nAxis) ) {
			dTargetPos = WorldToCnt(m_CurLoc.pos);
			m_pCmd->GetPosition(m_nAxis, &dEncoderPos);

			if( fabs(dEncoderPos - dTargetPos) > m_dCoarseTol ) {
				SetAxisError(axisAxisDoneInposFail);
				return FALSE;
			}
			else {
				m_nAxisError = axisNoError;
				return TRUE;
			}
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	SetAxisError(axisNotAxisDone);

	return FALSE;
}

BOOL CAxAxis::IsMotionDone()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	double dTargetPos, dEncoderPos;

	try {
		bSuccess = CheckReady();
		if( !bSuccess ) return FALSE;

		if( m_pCmd->MotionDone(m_nAxis) ) {
			dTargetPos = WorldToCnt(m_CurLoc.pos);
			m_pCmd->GetPosition(m_nAxis, &dEncoderPos);

			if( fabs(dEncoderPos - dTargetPos) > m_dCoarseTol ) {
				SetAxisError(axisMotionDoneInposFail);
				return FALSE;
			}
			else { 
				m_nAxisError = axisNoError;
				return TRUE;
			}
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	SetAxisError(axisNotMotionDone);

	return FALSE;
}

BOOL CAxAxis::IsAxisReady()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	bSuccess = WaitAxisDone(0);
	if( !bSuccess ) return FALSE;

	bSuccess = CheckReady();
	if( !bSuccess ) {
		bSuccess = ClearStatus();
		if( !bSuccess ) return FALSE;
	}

	bSuccess = IsAmpEnabled();
	if( !bSuccess ) {
		bSuccess = EnableAmp();
		if( !bSuccess ) return FALSE;

		bSuccess = IsAmpEnabled();
		if( !bSuccess ) return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::CheckReady()
{
	if( m_bSimulate ) return TRUE;

	UINT nAxisState = m_pCmd->AxisState(m_nAxis);
	UINT nAxisSource = m_pCmd->AxisSource(m_nAxis);

	switch( nAxisState ) {
		case NO_EVENT:
			break;

		case STOP_EVENT:
		case E_STOP_EVENT:
		case ABORT_EVENT:
			if( nAxisSource & ST_POS_LIMIT ) SetAxisError(axisEventPosLimitSw);
			else if( nAxisSource & ST_NEG_LIMIT ) SetAxisError(axisEventNegLimitSw);
			else if( nAxisSource & ST_AMP_FAULT ) SetAxisError(axisEventAmpFault);
			else if( nAxisSource & ST_A_LIMIT ) SetAxisError(axisEventAccelLimit);
			else if( nAxisSource & ST_V_LIMIT ) SetAxisError(axisEventVelLimit);
			else if( nAxisSource & ST_X_NEG_LIMIT ) SetAxisError(axisEventNegLimit);
			else if( nAxisSource & ST_X_POS_LIMIT ) SetAxisError(axisEventPosLimit);
			else if( nAxisSource & ST_ERROR_LIMIT ) SetAxisError(axisEventErrorLimit);
			else if( nAxisSource & ST_OUT_OF_FRAMES ) SetAxisError(axisEventOutOfFrame);
			else if( nAxisSource & ST_ABS_COMM_ERROR ) SetAxisError(axisEventAbsCommError);
			else if( nAxisSource & ST_RUN_STOP_COMMAND ) SetAxisError(axisEventAbsCommError);
			else if( nAxisSource & ST_COLLISION_STATE ) SetAxisError(axisEventCollisionState);
			else if( nAxisSource & ST_HOME_SWITCH ) SetAxisError(axisEventHomeSwitch);
			else {
				m_Trace.Log(_T("[%02d Axis] CheckReady() -> SourceCheck AxisState : %d, AxisSource : %d"), m_nAxis, nAxisState, nAxisSource);
				break;
			}

			return FALSE;

		default:
			m_Trace.Log(_T("[%02d Axis] CheckReady() -> StateCheck AxisState : %d, AxisSource : %d"), m_nAxis, nAxisState, nAxisSource);
			break;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::WaitAxisReady(UINT nTm)
{
	if( m_bSimulate ) return TRUE;

	CAxTimer timer;

	timer.Start();
	while( !CheckReady() ) { 
		if( CheckMasterTerminate() ) return FALSE;

		if( timer.IsTimeUp(nTm) ) return FALSE;

		Sleep(timePolling);
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::IsRotateType()
{
	return m_bIsRotate;
}

BOOL CAxAxis::GetCurLoc(double* pdCurLoc)
{
	if( m_bSimulate ) {
		*pdCurLoc = 0.0;
		return TRUE;
	}

	try {
		if( m_nEncoderChk ) m_pCmd->GetPosition(m_nAxis, pdCurLoc);
		else m_pCmd->GetCommand(m_nAxis, pdCurLoc);

		*pdCurLoc /= m_dScale;

		if( m_bIsRotate ) {
			int nQuotient;

			if( *pdCurLoc < 0 ) nQuotient = (int)((*pdCurLoc + 1) / 360) - 1;
			else nQuotient = (int)(*pdCurLoc / 360);

			*pdCurLoc = *pdCurLoc - nQuotient * 360;
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		*pdCurLoc = 0.0;

		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

double CAxAxis::CheckRotateLoc(double dLoc)
{
	if( m_bIsRotate ) {
		int nQuotient;

		if( dLoc < 0 ) nQuotient = (int)((dLoc + 1) / 360) - 1;
		else nQuotient = (int)(dLoc / 360);

		dLoc = dLoc - nQuotient * 360;
	}

	return dLoc;
}

int CAxAxis::GetAxisState()
{
	if( m_bSimulate ) return 0;

	return m_pCmd->AxisState(m_nAxis);
}

int CAxAxis::GetAxisSource()
{
	if( m_bSimulate ) return 0;

	return m_pCmd->AxisSource(m_nAxis);
}

double CAxAxis::GetScale()
{
	return m_dScale;
}

BOOL CAxAxis::EnableAmp()
{
	if( m_bSimulate ) return TRUE;

	try {
		if( m_bIsBreakType ) {
			m_opBreak.Off();
			m_pCmd->EnableAmp(m_nAxis);
			Sleep(100);
			m_opBreak.On();
		}
		else m_pCmd->EnableAmp(m_nAxis);

		Sleep(50);

		double dCurPos;
		m_pCmd->GetPosition(m_nAxis, &dCurPos);
		m_pCmd->SetCommand(m_nAxis, dCurPos);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::DisableAmp()
{
	if( m_bSimulate ) return TRUE;

	try {
		if( m_bIsBreakType ) {
			m_opBreak.Off();
			Sleep(100);
			m_pCmd->DisableAmp(m_nAxis);
		}
		else m_pCmd->DisableAmp(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::AlarmClear()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	
	try {
		bSuccess = DisableAmp();
		if( !bSuccess ) return FALSE;

		m_pCmd->AmpFaultReset(m_nAxis);
		Sleep(50);

		m_pCmd->AmpFaultSet(m_nAxis);
		Sleep(50);

		bSuccess = EnableAmp();
		if( !bSuccess ) return FALSE;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::ClearStatus(int nTimeWait, int nTimeClear)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	
	try {
		bSuccess = WaitAxisDone(nTimeWait);
		if( !bSuccess ) return FALSE;

		m_pCmd->ClearStatus(m_nAxis);
		
		bSuccess = WaitAxisReady(nTimeClear);
		if( !bSuccess ) return FALSE;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Home()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;
	
	try {
		m_bHome = FALSE;

		bSuccess = AlarmClear();
		if( !bSuccess ) return FALSE;

		bSuccess = ClearStatus();
		if( !bSuccess ) return FALSE;

		m_pCmd->SetHome(m_nAxis, NO_EVENT);
		m_pCmd->SetStopRate(m_nAxis, m_nMinDecel);

		bSuccess = CoarseHome();
		if( !bSuccess ) return FALSE;

		Sleep(100);

		bSuccess = FineHome();
		if( !bSuccess ) return FALSE;
		
		Sleep(100);
		
		m_pCmd->SetPosition(m_nAxis, 0);
		m_pCmd->SetCommand(m_nAxis, 0);

		m_pCmd->Move(m_nAxis, m_dParkPos, m_dFastHomeSpeed, m_nMinAccel);
		m_CurLoc.pos = m_dParkPos;

		bSuccess = SetParam();
		if( !bSuccess ) return FALSE;

		m_bHome = TRUE;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}


BOOL CAxAxis::CoarseHome()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;

	bSuccess = SearchHome(m_nHomeDir);
	if( !bSuccess ) return FALSE;

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::FineHome()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;

	bSuccess = SearchFineHome(m_nHomeDir);
	if( !bSuccess ) return FALSE;

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::SearchHome(int nDir)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;

	try {
		m_pCmd->ControllerRun(m_nAxis);
		m_pCmd->SetHome(m_nAxis, STOP_EVENT);
		m_pCmd->FramesClear(m_nAxis);

		m_pCmd->VMove(m_nAxis, nDir * m_dFastHomeSpeed, m_nMinAccel);

		if( !WaitAxisDoneCheckStopState((int)(m_dMaxDist / m_dFastHomeSpeed + 5) * 1000) ) return FALSE;

		m_pCmd->SetHome(m_nAxis, NO_EVENT);

		bSuccess = ClearStatus();
		if( !bSuccess ) return FALSE;

		double dPosition;
		m_pCmd->GetPosition(m_nAxis, &dPosition);
		BOOL bHomeSwitch = m_pCmd->GetHomeSwitch(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::SearchFineHome(int nDir)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;

	try {
		m_pCmd->StartRMove(m_nAxis, nDir * m_dHomeToIndex * (-1), m_dFastHomeSpeed, m_nMinAccel);

		if( !WaitAxisDoneCheckStopState((int)(m_dHomeToIndex / m_dFastHomeSpeed + 5) * 1000) ) return FALSE;

		m_pCmd->SetHome(m_nAxis, STOP_EVENT);
		m_pCmd->FramesClear(m_nAxis);

		m_pCmd->VMove(m_nAxis, nDir * m_dSlowHomeSpeed, m_nMinAccel);
		
		if( !WaitAxisDoneCheckStopState((int)(m_dHomeToIndex / m_dSlowHomeSpeed + 5) * 1000) ) return FALSE;

		m_pCmd->SetHome(m_nAxis, NO_EVENT);

		bSuccess = ClearStatus();
		if( !bSuccess ) return FALSE;

		double dPosition;
		m_pCmd->GetPosition(m_nAxis, &dPosition);
		BOOL bHomeSwitch = m_pCmd->GetHomeSwitch(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::StartMove(AxisLoc loc, BOOL bSlow)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;
	AxisLoc aLoc;

	try {
		bSuccess = IsAxisReady();
		if( !bSuccess ) return FALSE;
		
		aLoc = WorldToCnt(loc);
		if( bSlow ) SetMoveSpeed(loc.speed);
		else SetMoveSpeed(m_dMaxSpeed);

		m_pCmd->StartMove(m_nAxis, aLoc.pos, m_dSpeed, m_nAccel);
		m_CurLoc = loc;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Move(AxisLoc loc, BOOL bSlow)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;
	AxisLoc aLoc;

	try {
		bSuccess = IsAxisReady();
		if( !bSuccess ) return FALSE;
		
		aLoc = WorldToCnt(loc);
		if( bSlow ) SetMoveSpeed(loc.speed);
		else SetMoveSpeed(m_dMaxSpeed);

		m_pCmd->Move(m_nAxis, aLoc.pos, m_dSpeed, m_nAccel);
		m_CurLoc = loc;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::StartTMove(AxisLoc loc, BOOL bSlow)
{
	if( m_bSimulate) return TRUE;

	BOOL bSuccess;
	AxisLoc aLoc;

	try {
		bSuccess = IsAxisReady();
		if( !bSuccess ) return FALSE;
		
		aLoc = WorldToCnt(loc);
		if( bSlow ) SetMoveSpeed(loc.speed);
		else SetMoveSpeed(m_dMaxSpeed);

		m_pCmd->StartTMove(m_nAxis, aLoc.pos, m_dSpeed, m_nAccel, m_nDecel);
		m_CurLoc = loc;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::TMove(AxisLoc loc, BOOL bSlow)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess;
	AxisLoc aLoc;

	try {
		bSuccess = IsAxisReady();
		if( !bSuccess ) return FALSE;
		
		aLoc = WorldToCnt(loc);
		if( bSlow ) SetMoveSpeed(loc.speed);
		else SetMoveSpeed(m_dMaxSpeed);

		m_pCmd->TMove(m_nAxis, aLoc.pos, m_dSpeed, m_nAccel, m_nDecel);
		m_CurLoc = loc;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::VMove(double dSpeed, int nAccel)
{
	if( m_bSimulate ) return TRUE;

	try {
		BOOL bSuccess = TRUE;

		bSuccess = IsAxisReady();
		if( !bSuccess ) return FALSE;

		m_pCmd->VMove(m_nAxis, WorldToCnt(dSpeed), nAccel);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::VStop()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	try {
		m_pCmd->VStop(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Jog()
{
	if( m_bSimulate ) return TRUE;

	return VMove(m_dJogSpeed, m_nJogAccel);
}

BOOL CAxAxis::Jog(double dSpeed, int dAccel)
{
	if( m_bSimulate ) return TRUE;

	return VMove(dSpeed, dAccel);
}

BOOL CAxAxis::JogPos()
{
	if( m_bSimulate ) return TRUE;

	return VMove(m_dJogSpeed, m_nMaxAccel);
}

BOOL CAxAxis::JogPos(double dSpeed, int nAccel)
{
	if( m_bSimulate ) return TRUE;

	return VMove(dSpeed, nAccel);
}

BOOL CAxAxis::JogNeg()
{
	if( m_bSimulate ) return TRUE;

	return VMove(m_dJogSpeed * (-1), m_nMaxAccel);
}

BOOL CAxAxis::JogNeg(double dSpeed,int nAccel)
{
	if( m_bSimulate ) return TRUE;

	return VMove(dSpeed * (-1), nAccel);
}

BOOL CAxAxis::JogStop()
{
	if( m_bSimulate ) return TRUE;

	return VStop();
}

BOOL CAxAxis::WaitAxisDone(int nTm)
{
	if( m_bSimulate ) return TRUE;

	CAxTimer timer;
	
	if( nTm != INFINITE ) timer.Start();

	while( !m_pCmd->AxisDone(m_nAxis) ) {
		if( CheckMasterTerminate() ) return FALSE;

		if( nTm != INFINITE ) {
			if( timer.IsTimeUp(nTm) ) {
				SetAxisError(axisNotAxisDone);
				return FALSE;
			}
		}
		
		Sleep(timePolling);
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::WaitAxisDoneCheckStopState(int nTm)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	CAxTimer timer;

	if( nTm != INFINITE ) timer.Start();

	while( !m_pCmd->AxisDone(m_nAxis) ) {
		if( CheckMasterTerminate() ) return FALSE;

		if( IsAxisStopState() ) {
			m_Trace.Log(_T("[%02d Axis] WaitAxisDoneCheckStopState() -> AxisStopState"), m_nAxis);

			bSuccess = Stop();
			if( !bSuccess ) return FALSE;

			return FALSE;
		}

		if( nTm != INFINITE ) {
			if( timer.IsTimeUp(nTm) ) {
				SetAxisError(axisNotAxisDone);
				return FALSE;
			}
		}

		Sleep(timePolling);
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::WaitMotionDone(int nTm)
{
	if( m_bSimulate ) return TRUE;

	CAxTimer timer;

	if( nTm != INFINITE ) timer.Start();

	while( !m_pCmd->MotionDone(m_nAxis) ) {
		if( CheckMasterTerminate() ) return FALSE;

		if( nTm != INFINITE ) {
			if( timer.IsTimeUp(nTm) ) {
				SetAxisError(axisNotMotionDone);
				return FALSE;
			}
		}

		Sleep(timePolling);
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Stop()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;
	
	try {
		m_pCmd->SetStop(m_nAxis);

		bSuccess = WaitAxisDone(INFINITE);
		if( !bSuccess ) return FALSE;

		bSuccess = ClearStatus();
		if( !bSuccess ) return FALSE;
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Resume()
{
	if( m_bSimulate ) return TRUE;

	double dTargetPos, dEncoderPos;

	try {
		dTargetPos = WorldToCnt(m_CurLoc.pos);
		m_pCmd->GetPosition(m_nAxis, &dEncoderPos);
		if( fabs(dEncoderPos - dTargetPos) > m_dCoarseTol ) {
			m_CurLoc.speed = m_dMaxSpeed;
			if( !StartMove(m_CurLoc, TRUE) ) return FALSE;
		}
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::Abort()
{
	if( m_bSimulate ) return TRUE;

	try {
		m_pCmd->ControllerIdle(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}

	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::IsAxisStopState()
{
	CAxSystemError* pSystemError = (CAxSystemError*)CAxSystemHub::GetSystemHub()->GetSystem(_T("SystemError"));

	if( pSystemError ) {
		SetAxisError(axisStopState);
		return pSystemError->GetEmgRobotStop();
	}
	else return FALSE;
}

BOOL CAxAxis::CheckMasterTerminate()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();

	return pMaster->GetTeminate();
}

BOOL CAxAxis::PositionIOOnOff(UINT nPosNum, int nBitNo, double dPos, int nEncFlag)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	double dCmd, dEnc;
	m_pCmd->GetCommand(m_nAxis, &dCmd);
	m_pCmd->GetPosition(m_nAxis, &dEnc);

	try {
		m_pCmd->PositionIOOnOff(nPosNum, nBitNo, m_nAxis, WorldToCnt(dPos), nEncFlag);
		m_Trace.Log(_T("<P> %d, %d, %d, %f, %f, %f, %d"), nPosNum, nBitNo, m_nAxis, WorldToCnt(dPos), WorldToCnt(dCmd), WorldToCnt(dEnc), nEncFlag);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::PositionIOClear(UINT nPosNum)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	try {
		m_pCmd->PositionIOClear(m_nAxis, nPosNum);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::PositionIOAllClear()
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	try {
		m_pCmd->PositionIOAllClear(m_nAxis);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::SetPosition(double dPos)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	try {
		m_pCmd->SetPosition(m_nAxis, dPos);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}

BOOL CAxAxis::GetPosition(double* pdPos)
{
	if( m_bSimulate ) return TRUE;

	BOOL bSuccess = TRUE;

	try {
		if( m_nEncoderChk ) m_pCmd->GetPosition(m_nAxis, pdPos);
		else m_pCmd->GetCommand(m_nAxis, pdPos);
	}
	catch( UINT nExp ) {
		HandleException(nExp);
		return FALSE;
	}
	
	m_nAxisError = axisNoError;
	return TRUE;
}