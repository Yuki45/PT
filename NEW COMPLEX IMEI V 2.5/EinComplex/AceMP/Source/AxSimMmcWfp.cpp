// AxSimMmcCmd.cpp: implementation of the CAxSimMmcWfp class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxSimMmcWfp.h"
#include "AxObject.h"
#include "MMCWHP154.h"
#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// ============================================================================
CAxSimMoveInfo::CAxSimMoveInfo()
{
	m_nMoveType		= SMT_RELATIVE;
	m_nVelProfile	= SVP_TRAPEZOID;
	m_bWait			= FALSE;
	m_dDestPosition	= 0;
	m_dVelocity		= 10;
	m_nAccel		= 10 * 4; // 4 ms
	m_nDecel		= 10 * 4; // 4 ms
	m_bForward		= TRUE;
	m_dMoveDistance	= 0;
}

CAxSimMoveInfo::CAxSimMoveInfo(const CAxSimMoveInfo & clsSimMoveInfo)
{
	m_nMoveType		= clsSimMoveInfo.m_nMoveType;
	m_nVelProfile	= clsSimMoveInfo.m_nVelProfile;
	m_bWait			= clsSimMoveInfo.m_bWait;
	m_dDestPosition	= clsSimMoveInfo.m_dDestPosition;
	m_dVelocity		= clsSimMoveInfo.m_dVelocity;
	m_nAccel		= clsSimMoveInfo.m_nAccel;
	m_nDecel		= clsSimMoveInfo.m_nDecel;
	m_bForward		= clsSimMoveInfo.m_bForward;
	m_dMoveDistance	= clsSimMoveInfo.m_dMoveDistance;
}

CAxSimMoveInfo::CAxSimMoveInfo(
	SimMoveType		nMoveType, 
	SimVelProfile	nVelProfile, 
	BOOL	bWait, 
	double	dDestPosition, 
	double	dVelocity, 
	short	nAccel, 
	short	nDecel,
	BOOL	bForward)
{
	m_nMoveType		= nMoveType;
	m_nVelProfile	= nVelProfile;
	m_bWait			= bWait;
	m_dDestPosition	= dDestPosition;
	m_dVelocity		= dVelocity;
	m_nAccel		= nAccel;
	m_nDecel		= nDecel;
	m_bForward		= bForward;
	m_dMoveDistance	= 0;
}

CAxSimMoveInfo::~CAxSimMoveInfo()
{
}

const CAxSimMoveInfo & CAxSimMoveInfo::operator=(const CAxSimMoveInfo & clsSimMoveInfo)
{
	m_nMoveType		= clsSimMoveInfo.m_nMoveType;
	m_nVelProfile	= clsSimMoveInfo.m_nVelProfile;
	m_bWait			= clsSimMoveInfo.m_bWait;
	m_dDestPosition	= clsSimMoveInfo.m_dDestPosition;
	m_dVelocity		= clsSimMoveInfo.m_dVelocity;
	m_nAccel		= clsSimMoveInfo.m_nAccel;
	m_nDecel		= clsSimMoveInfo.m_nDecel;
	m_bForward		= clsSimMoveInfo.m_bForward;
	m_dMoveDistance	= clsSimMoveInfo.m_dMoveDistance;

	return *this;
}

void CAxSimMoveInfo::SetData(
	SimMoveType		nMoveType, 
	SimVelProfile	nVelProfile, 
	BOOL	bWait, 
	double	dDestPosition, 
	double	dVelocity, 
	short	nAccel, 
	short	nDecel,
	BOOL	bForward)
{
	m_nMoveType		= nMoveType;
	m_nVelProfile	= nVelProfile;
	m_bWait			= bWait;
	m_dDestPosition	= dDestPosition;
	m_dVelocity		= dVelocity;
	m_nAccel		= nAccel;
	m_nDecel		= nDecel;
	m_bForward		= bForward;
	m_dMoveDistance	= 0;
}

// ============================================================================
CAxSimAxis::CAxSimAxis()
{
	m_dPosition			= 500;
	m_dCommand			= 0;
	m_dInposition		= 50;
	m_dPrevPosition		= 500;

	m_nAxisState		= 0;
	m_nAxisSource		= 0;

	m_nInSequence		= 0;
	m_nInMotion			= 0;
	m_nInPosition		= 1;

	m_nAmpEnable		= 0;

	m_nStopRate			= 10 * 4;	// 10 * 4 ms
	m_nEStopRate		= 10 * 4;	// 10 * 4 ms

	m_nHomeEvent		= NO_EVENT;
	m_nPositiveEvent	= NO_EVENT;
	m_nNegativeEvent	= NO_EVENT;
	m_nAmpFaultEvent	= NO_EVENT;
	
	m_bVMoveStop		= FALSE;
	m_bStartAction		= FALSE;
}

CAxSimAxis::~CAxSimAxis()
{
	m_bTerminate = TRUE;
	DeleteThreads();
}

void CAxSimAxis::Startup()
{
	CreateThreads();

	m_wtSleep.SetTimer(0, timePolling);
}

//void CAxSimAxis::AxisResumeThread()
//{
//	ResumeThread(m_hMoveThread);
//}
//
//void CAxSimAxis::AxisSuspendThread()
//{
//	SuspendThread(m_hMoveThread);
//}

//UINT CAxSimAxis::OnMove()
UINT CAxSimAxis::PriRun()
{
//	SuspendAxThread(m_hPriThread);
   	while (TRUE) {
		try {
			AutoRun();
		}
		catch (int nExp) {
			Sleep(10);
			if (nExp == -1) {
				if (m_bTerminate) AfxEndThread(0);
			}
		}
	}
	return 0;
}

UINT CAxSimAxis::AutoRun()
{
	double	dTimeAcc		= 10 * 4;
	double	dTimeVel		= 1000;
	double	dTimeDec		= 10 * 4;
	double	dOldPosition	= 0;
	BOOL	bForward		= TRUE;
	double	dStopDecel		= 10 * 4;
	double	dStopVelocity;

	while( TRUE )
	{
		// --------------------------------------------------------------------
		while( !m_bStartAction ){
			//Sleep(timePolling);
			m_wtSleep.WaitTimer();
			if( m_bTerminate ) throw -1;
		}

		m_dPrevPosition = m_dPosition;
		GetTransformMoveInfo(dTimeAcc, dTimeVel, dTimeDec);
		
		// --------------------------------------------------------------------
		while( TRUE )
		{	
			if( m_clsMoveInfo.m_nMoveType == SMT_RELATIVE || 
				m_clsMoveInfo.m_nMoveType == SMT_ABSOLUTE )
			{
				dStopVelocity = MoveAcc(dTimeAcc, m_clsMoveInfo.m_bForward);
				if( dStopVelocity != -1 ) break;

				dStopVelocity = MoveVel(dTimeVel, m_clsMoveInfo.m_bForward);
				if( dStopVelocity != -1 ) break;

				dStopVelocity = MoveDec(dTimeDec, m_clsMoveInfo.m_bForward, FALSE);
				if( dStopVelocity != -1 ) break;

				// ��Ȯ�� ��ġ�� ���� �������� �־ 
				if(	m_clsMoveInfo.m_bForward )
				{
					m_dPosition = m_dPrevPosition + m_clsMoveInfo.m_dMoveDistance;
					m_dCommand = m_dPrevPosition + m_clsMoveInfo.m_dMoveDistance;
				}
				else{
					m_dPosition = m_dPrevPosition - m_clsMoveInfo.m_dMoveDistance;
					m_dCommand = m_dPrevPosition - m_clsMoveInfo.m_dMoveDistance;
				}
			}
			else if( m_clsMoveInfo.m_nMoveType == SMT_V_MOVE )
			{
				dStopVelocity = MoveAcc(dTimeAcc, m_clsMoveInfo.m_bForward);
				if( dStopVelocity != -1 ) break;

				dStopVelocity = MoveVel(dTimeVel, m_clsMoveInfo.m_bForward);
				if( dStopVelocity != -1 ) break;
			}
			else
			{
			}

			break;
		}

		// --------------------------------------------------------------------
		if( dStopVelocity != -1 )
		{
			dStopDecel = GetDec();
			dTimeDec = dStopDecel * dStopVelocity / m_clsMoveInfo.m_dVelocity;
			
			MoveDec(dTimeDec, m_clsMoveInfo.m_bForward, TRUE);
		}

		SetPostAction();
	}

	return 0;
}

void CAxSimAxis::SetPreAction()
{
	m_nInSequence	= TRUE;
//	Sleep(100);
	m_nInMotion		= TRUE;
	m_nInPosition	= FALSE;
	m_bVMoveStop	= FALSE;
	m_bStartAction	= TRUE;
}

void CAxSimAxis::SetPostAction()
{
	m_nInSequence	= FALSE;
	m_nInMotion		= FALSE;
	m_nInPosition	= TRUE;
	m_bVMoveStop	= FALSE;
	m_bStartAction	= FALSE;
}

void CAxSimAxis::SetMoveInfo(const CAxSimMoveInfo & clsSimMoveInfo)
{
	m_clsMoveInfo = clsSimMoveInfo;
}

void CAxSimAxis::GetTransformMoveInfo(double & dTimeAcc, double & dTimeVel, double & dTimeDec)
{
	// ------------------------------------------------------------------------
	double	dDistanceMove		= 0;
	double	dDistanceVelocity	= 0;
	double	dDistanceAccel		= 0;
	double	dDistanceDecel		= 0;
	double	dDistanceAccel2		= 0;
	double	dDistanceDecel2		= 0;
	LONG	lTimeVelocity		= 0;

	// ------------------------------------------------------------------------
	if( m_clsMoveInfo.m_nMoveType == SMT_RELATIVE )
	{
		if( m_clsMoveInfo.m_dDestPosition >= 0 )
			m_clsMoveInfo.m_bForward = TRUE;
		else
			m_clsMoveInfo.m_bForward = FALSE;
	}
	else if( m_clsMoveInfo.m_nMoveType == SMT_ABSOLUTE )
	{
		if( m_clsMoveInfo.m_dDestPosition >= m_dPosition )
			m_clsMoveInfo.m_bForward = TRUE;
		else
			m_clsMoveInfo.m_bForward = FALSE;
	}
	else if( m_clsMoveInfo.m_nMoveType == SMT_V_MOVE )
	{
		if( m_clsMoveInfo.m_dVelocity >= 0 )
		{
			m_clsMoveInfo.m_bForward = TRUE;
		}
		else
		{
			m_clsMoveInfo.m_dVelocity = -m_clsMoveInfo.m_dVelocity;
			m_clsMoveInfo.m_bForward = FALSE;
		}
	}
	else{
	}

	// ------------------------------------------------------------------------
//	if( m_clsMoveInfo.m_nVelProfile == SVP_TRAPEZOID )
//	{
		if( m_clsMoveInfo.m_nMoveType == SMT_RELATIVE || m_clsMoveInfo.m_nMoveType == SMT_ABSOLUTE )
		{
			if( m_clsMoveInfo.m_nMoveType == SMT_RELATIVE )
			{
				dDistanceMove = fabs(m_clsMoveInfo.m_dDestPosition);
				m_clsMoveInfo.m_dMoveDistance = dDistanceMove;
			}
			else // m_clsMoveInfo.m_nMoveType == SMT_ABSOLUTE
			{
				dDistanceMove = fabs(m_clsMoveInfo.m_dDestPosition - m_dPosition);
				m_clsMoveInfo.m_dMoveDistance = dDistanceMove;
			}

			if( dDistanceMove == 0 )
			{
				dTimeVel = 0;
				dTimeAcc = 0;
				dTimeDec = 0;
			}
			else
			{
				dDistanceAccel = (m_clsMoveInfo.m_nAccel * m_clsMoveInfo.m_dVelocity) / 2;
				dDistanceDecel = (m_clsMoveInfo.m_nDecel * m_clsMoveInfo.m_dVelocity) / 2;

				if( dDistanceMove > (dDistanceAccel + dDistanceDecel) )
				{
					dDistanceVelocity = dDistanceMove - (dDistanceAccel + dDistanceDecel);
					dTimeVel = dDistanceVelocity / m_clsMoveInfo.m_dVelocity;
					dTimeAcc = m_clsMoveInfo.m_nAccel;
					dTimeDec = m_clsMoveInfo.m_nDecel;
				}
				else
				{
					dDistanceAccel2 = dDistanceAccel * dDistanceMove / (dDistanceAccel + dDistanceDecel);
					dDistanceDecel2 = dDistanceDecel * dDistanceMove / (dDistanceAccel + dDistanceDecel);
			
					dTimeVel = 0;
					dTimeAcc = m_clsMoveInfo.m_nAccel * sqrt(dDistanceAccel2 / dDistanceAccel);
					dTimeDec = m_clsMoveInfo.m_nDecel * sqrt(dDistanceDecel2 / dDistanceDecel);
				}
			}
		}
		else if( m_clsMoveInfo.m_nMoveType == SMT_V_MOVE )
		{
			dTimeVel = -1;
			dTimeAcc = m_clsMoveInfo.m_nAccel;
			dTimeDec = m_clsMoveInfo.m_nDecel;
		}
		else // m_clsMoveInfo.m_nMoveType == ETC
		{
		}
//	}
//	else // m_clsMoveInfo.m_nVelProfile == SVP_SCURVE
//	{
//	}

}

double CAxSimAxis::MoveAcc(double dTime, BOOL bForward)
{
	double dTimeSpan = 0;
	double dMovePosition = 0;
	double dOldPosition = m_dPosition;
	double dStopVelocity;

	if( dTime == 0 )
		return -1;

	m_Timer.Start();

	while( TRUE )
	{
		if( m_bTerminate ) throw -1;

		dTimeSpan = m_Timer.IsTimeNow();

		if( dTimeSpan >= dTime )
			dTimeSpan = dTime;

		dMovePosition = (m_clsMoveInfo.m_dVelocity * dTimeSpan * dTimeSpan) / (2 * m_clsMoveInfo.m_nAccel);
		if( bForward )
		{
			m_dPosition = dOldPosition + dMovePosition;
			m_dCommand = dOldPosition + dMovePosition;
		}
		else
		{
			m_dPosition = dOldPosition - dMovePosition;
			m_dCommand = dOldPosition - dMovePosition;
		}

		if( m_dPosition <= 0 && m_nHomeEvent != NO_EVENT )
			SetAxisState(m_nHomeEvent);

		if( m_nAxisState != NO_EVENT || m_bVMoveStop ){
			dStopVelocity = m_clsMoveInfo.m_dVelocity * dTimeSpan / m_clsMoveInfo.m_nAccel;
			return dStopVelocity;
		}

		if( dTimeSpan >= dTime )
			break;
		
		//Sleep(timePolling);
		m_wtSleep.WaitTimer();
	}

	return -1;
}

double CAxSimAxis::MoveVel(double dTime, BOOL bForward)
{
	double dTimeSpan = 0;
	double dMovePosition = 0;
	double dOldPosition = m_dPosition;
	double dStopVelocity;

	if( dTime == 0 )
		return -1;

	m_Timer.Start();

	while( TRUE )
	{
		if( m_bTerminate ) throw -1;

		dTimeSpan = m_Timer.IsTimeNow();

		if( dTime >= 0 && dTimeSpan >= dTime )
			dTimeSpan = dTime;

		dMovePosition = m_clsMoveInfo.m_dVelocity * dTimeSpan;
		if( bForward )
		{
			m_dPosition = dOldPosition + dMovePosition;
			m_dCommand = dOldPosition + dMovePosition;
		}
		else
		{
			m_dPosition = dOldPosition - dMovePosition;
			m_dCommand = dOldPosition - dMovePosition;
		}

		if( m_dPosition <= 0 && m_nHomeEvent != NO_EVENT )
			SetAxisState(m_nHomeEvent);

		if( m_nAxisState != NO_EVENT || m_bVMoveStop ){
			dStopVelocity = m_clsMoveInfo.m_dVelocity;
			return dStopVelocity;
		}

		if( dTime >= 0 && dTimeSpan >= dTime )
			break;
		
		//Sleep(timePolling);
		m_wtSleep.WaitTimer();
	}

	return -1;
}

double CAxSimAxis::MoveDec(double dTime, BOOL bForward, BOOL bStopEventRun)
{
	double dTimeSpan = 0;
	double dMovePosition = 0;
	double dOldPosition = m_dPosition;
	double dStopVelocity;

	if( dTime == 0 )
		return -1;

	m_Timer.Start();

	while( TRUE )
	{
		if( m_bTerminate ) throw -1;

		dTimeSpan = m_Timer.IsTimeNow();

		if( dTimeSpan >= dTime )
			dTimeSpan = dTime;

		dMovePosition = (m_clsMoveInfo.m_dVelocity * (2 * dTime - dTimeSpan) * dTimeSpan) / (2 * m_clsMoveInfo.m_nDecel);
		if( bForward )
		{
			m_dPosition = dOldPosition + dMovePosition;
			m_dCommand = dOldPosition + dMovePosition;
		}
		else
		{
			m_dPosition = dOldPosition - dMovePosition;
			m_dCommand = dOldPosition - dMovePosition;
		}

		if( !bStopEventRun ){
			if( m_dPosition <= 0 && m_nHomeEvent != NO_EVENT )
				SetAxisState(m_nHomeEvent);

			if( m_nAxisState != NO_EVENT || m_bVMoveStop ){
				dStopVelocity = m_clsMoveInfo.m_dVelocity * (dTime - dTimeSpan) / m_clsMoveInfo.m_nDecel;
				return dStopVelocity;
			}
		}

		if( dTimeSpan >= dTime )
			break;
		
		//Sleep(timePolling);
		m_wtSleep.WaitTimer();
	}

	return -1;
}

void CAxSimAxis::SetAxisState(short nState)
{
	if( m_nAxisState < nState )
		m_nAxisState = nState;
}

short CAxSimAxis::GetDec()
{
	if( m_nAxisState >= E_STOP_EVENT )
		return m_nEStopRate;
	else if( m_nAxisState == STOP_EVENT )
		return m_nStopRate;

	return m_clsMoveInfo.m_nDecel;
}

// ============================================================================
// ���� �ؾ��� ���� 
// ----------------------------------------------------------------------------
// 1. �̵� ����� ������ �ö� �ϳ��� �����ϴ� ��ƾ 
// 2. �浹���� ��ƾ
// 3. �̵� ����� �ָ� ��ǥ ��ġ�� m_dCommand �ΰ�?
//    servo off �� �̵���Ű�� inposition�� ��� �ǳ�? (�ǽð�?, �̵��Ҷ���?)
// ============================================================================

// ============================================================================
CAxSimMmcWfp* CAxSimMmcWfp::theSimMmcWfp = NULL;

CAxSimMmcWfp::CAxSimMmcWfp()
{
	m_pSimAxis = NULL;
	m_pInput = NULL;
	m_pOutput = NULL;
}

CAxSimMmcWfp::~CAxSimMmcWfp()
{
	if( m_pSimAxis != NULL )
	{
		delete [] m_pSimAxis;
	}

	if( m_pInput != NULL )
	{
		delete [] m_pInput;
	}

	if( m_pOutput != NULL )
	{
		delete [] m_pOutput;
	}
}

CAxSimMmcWfp* CAxSimMmcWfp::GetSimMmcWfp()
{
	if(theSimMmcWfp == NULL) theSimMmcWfp = new CAxSimMmcWfp();
	return theSimMmcWfp;
}

// ============================================================================
short CAxSimMmcWfp::mmc_initx(short nTotalBdNum, short nNumAxis)
{
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceSimMmcWfp"));
	m_Trace.m_bEnableWriteLog = FALSE;

	m_pSimAxis = new CAxSimAxis[nNumAxis];
	for( int i=0; i<nNumAxis; i++ )
	{
		m_pSimAxis[i].Startup();
	}

	m_pInput = new long[nTotalBdNum];
	m_pOutput = new long[nTotalBdNum];

	for( int i=0; i<nTotalBdNum; i++ )
	{
		m_pInput[i] = 0xFFFFFFFF;
		m_pOutput[i] = 0xFFFFFFFF;
	}

	return 0;
	// MMC_OK(0)
	// MMC_NOT_INITIALIZED(1)
	// MMC_TIMEOUT_ERR(2)
	// MMC_NOT_EXIST(10)
	// MMC_BOOT_OPEN_ERROR(11)
	// MMC_CHKSUM_OPEN_ERROR(12)
}

// ============================================================================
short CAxSimMmcWfp::start_move(short nAxis, double dPos, double dVel, short dAcc)
{
	// ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����δ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_ABSOLUTE, SVP_TRAPEZOID, FALSE,
		dPos, dVel/1000, dAcc*4, dAcc*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::start_r_move(short nAxis, double dDist, double dVel, short dAcc)
{
	// ��ٸ��� �ӵ� profile, vel �ӵ��� dist ��ġ ��ŭ ����̵��� �Ѵ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_RELATIVE, SVP_TRAPEZOID, FALSE,
		dDist, dVel/1000, dAcc*4, dAcc*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::start_t_move(short nAxis, double dPos, double dVel, short dAcc, short dDec)
{
	// ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����δ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_ABSOLUTE, SVP_TRAPEZOID, FALSE,
		dPos, dVel/1000, dAcc*4, dDec*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::start_tr_move(short nAxis, double dDist, double dVel, short dAcc, short dDec)
{
	// ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� dist ��ġ ��ŭ ����̵��� �Ѵ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_RELATIVE, SVP_TRAPEZOID, FALSE,
		dDist, dVel/1000, dAcc*4, dDec*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::move(short nAxis, double dPos, double dVel, short dAcc)
{
	// ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����̸�,
	// ������ �Ϸ�� ������ ��ٸ���.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_ABSOLUTE, SVP_TRAPEZOID, TRUE,
		dPos, dVel/1000, dAcc*4, dAcc*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();

	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::r_move(short nAxis, double dDist, double dVel, short dAcc)
{
	// ��ٸ��� �ӵ� profile, vel �ӵ��� dist ��ġ ��ŭ ����̵��� �ϸ�,
	// ������ �Ϸ�� ������ ��ٸ���.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_RELATIVE, SVP_TRAPEZOID, TRUE,
		dDist, dVel/1000, dAcc*4, dAcc*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::t_move(short nAxis, double dPos, double dVel, short dAcc, short dDec)
{
	// ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����̸�,
	// ������ �Ϸ�� ������ ��ٸ���.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_ABSOLUTE, SVP_TRAPEZOID, TRUE,
		dPos, dVel/1000, dAcc*4, dDec*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::tr_move(short nAxis, double dDist, double dVel, short dAcc, short dDec)
{
	// ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� dist ��ġ ��ŭ ����̵��� �ϸ�,
	// ������ �Ϸ�� ������ ��ٸ���.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_RELATIVE, SVP_TRAPEZOID, TRUE,
		dDist, dVel/1000, dAcc*4, dDec*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::wait_for_done(short nAxis)
{
	// �������� ������ �Ϸ�� ������ ��ٸ���. 

	while( !motion_done(nAxis) );

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

// ============================================================================
short CAxSimMmcWfp::start_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc)
{
	// len ���� ������ �����ŭ ������ ����� ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����δ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	m_Trace.Log(_T("[NOT CODING] start_move_all()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::start_t_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc, short* pnDec)
{
	// len ���� ������ �����ŭ ������ ����� ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����δ�.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	m_Trace.Log(_T("[NOT CODING] start_t_move_all()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc)
{
	// len ���� ������ �����ŭ ������ ����� ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����̸�,
	// ������ �Ϸ�� ������ ��ٸ���. 
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	m_Trace.Log(_T("[NOT CODING] move_all()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::t_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc, short* pnDec)
{
	// len ���� ������ �����ŭ ������ ����� ���Ī ��ٸ��� �ӵ� profile, vel �ӵ��� pos ��ġ ��ŭ �����̸�
	// ������ �Ϸ�� ������ ��ٸ���. 
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	m_Trace.Log(_T("[NOT CODING] t_move_all()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::wait_for_all(short nNumAxis, short* pnAxisMap)
{
	// �������� ������ �Ϸ�� ������ ��ٸ���. 

	m_Trace.Log(_T("[NOT CODING] wait_for_all()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

// ============================================================================
short CAxSimMmcWfp::v_move(short nAxis, double dVel, short dAcc)
{
	// ������ ���Ӻ����� �������� �����ӵ��� �����ϰ� ȸ���Ѵ�. 
	// frame �� �����ؾߵ� ����� ����� �Ѵ�. 
	// frame �� �����ؾߵ� ����� ���� ���� ��� v_move �Լ��� ������� �ʴ´�. 
	// ���� frame_clear() �Լ��� �����Ͽ� frame �� clear ��Ų��.
	// ������ ���� : 4 ms, ������� : 10 �̻�, ���� : 1 ~ 25000
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	CAxSimMoveInfo clsMoveInfo;
	clsMoveInfo.SetData(
		SMT_V_MOVE, SVP_TRAPEZOID, TRUE,
		0, dVel/1000, dAcc*4, dAcc*4, TRUE
	);
	m_pSimAxis[nAxis].SetMoveInfo(clsMoveInfo);
	m_pSimAxis[nAxis].SetPreAction();
//	m_pSimAxis[nAxis].AxisResumeThread();
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::v_move_stop(short nAxis)
{
	// �������� v_move ������ �����Ѵ�.

	m_pSimAxis[nAxis].m_bVMoveStop = TRUE;

// 	wait_for_done(nAxis);

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_AMP_FAULT(8)
}

// ============================================================================
short CAxSimMmcWfp::map_axes(short nNumAxis, short* pnAxisMap)
{
	// ����, ��ȣ, �� ���� ������ ������ �� ��ǥ���� ���� �����Ѵ�. 
	// move_2, move_3, move_4, move_n, arc_2 ���� 
	// coordinated �����Լ��� �����ϱ����� �ݵ�� ���� �����Ͽ� ��ǥ���� �����ؾ� �Ѵ�. 

	m_Trace.Log(_T("[NOT CODING] map_axes()"));

	return 0;
	// MMC_OK(0)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_move_speed(double dSpeed)
{
	// ���� ���۽ÿ� �ӵ��� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] set_move_speed()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_move_accel(short nAccel)
{
	// ����, ��ȣ, �� ���� ���۽ÿ� ������ �ð��� �����Ѵ�.
	// accel �� 0 �� ��� �ӵ������ �����ķ� ������ ������ �ý��ۿ� ����� ������ �� �ִ�. 

	m_Trace.Log(_T("[NOT CODING] set_move_accel()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::all_done()
{
	// ����, ��ȣ, �� ���� �������� �Ϸ�Ǿ����� ���θ� �����ش�. 

	m_Trace.Log(_T("[NOT CODING] all_done()"));

	return 0;
	// 1 : ������ �Ϸ��
	// 0 : ������ ������
}

// ============================================================================
short CAxSimMmcWfp::move_2(double d1, double d2)
{
	// ������ 2 ���� x, y ��ǥ�� ��ŭ �����̵��� �Ѵ�. 
	// �Լ� ������ map_axes, set_move_speed, set_move_accel �Լ��� �����ؾ� �Ѵ�.

	m_Trace.Log(_T("[NOT CODING] move_2()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_NO_MAP(7)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::move_3(double d1, double d2, double d3)
{
	// ������ 3 ���� x, y, z ��ǥ�� ��ŭ �����̵��� �Ѵ�. 
	// �Լ� ������ map_axes, set_move_speed, set_move_accel �Լ��� �����ؾ� �Ѵ�.

	m_Trace.Log(_T("[NOT CODING] move_3()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_NO_MAP(7)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::move_4(double d1, double d2, double d3, double d4)
{
	// ������ 4 ���� x, y, z, w ��ǥ�� ��ŭ �����̵��� �Ѵ�. 
	// �Լ� ������ map_axes, set_move_speed, set_move_accel �Լ��� �����ؾ� �Ѵ�.

	m_Trace.Log(_T("[NOT CODING] move_4()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_NO_MAP(7)
	// MMC_AMP_FAULT(8)
}

short CAxSimMmcWfp::move_n(double* pDn)
{
	// ������ n ���� �־��� ��ǥ�� ��ŭ �����̵��� �Ѵ�. 
	// �Լ� ������ map_axes, set_move_speed, set_move_accel �Լ��� �����ؾ� �Ѵ�.

	m_Trace.Log(_T("[NOT CODING] move_n()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_NO_MAP(7)
	// MMC_AMP_FAULT(8)
}

// ============================================================================
short CAxSimMmcWfp::spl_line_move2ax(short nAxis1, short nAxis2, double* pdPos, double dVel, short nAcc)
{
	// ������ 2���� ������ġ���� �־��� 2���� ������ ��ǥ������ �ڵ� �������ϸ鼭 ���� cp motion���� �̵��Ѵ�.

	m_Trace.Log(_T("[NOT CODING] spl_line_move2ax()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::spl_line_move3ax(short nAxis1, short nAxis2, short nAxis3, double* pdPos, double dVel, UINT nAcc)
{
	// ������ 3���� ������ġ���� �־��� 3���� �������� ��ǥ������ �ڵ� �������ϸ鼭 ���� cp motion���� �̵��Ѵ�.

	m_Trace.Log(_T("[NOT CODING] spl_line_move3ax()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_stop(short nAxis)
{
	// ������ ���� stop_event �� ����, ���� �̵��� �����.

	m_pSimAxis[nAxis].m_nAxisState = STOP_EVENT;
	
	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_stop_rate(short nAxis, short dDec)
{
	// ������ ���� stop_event ����� ���ӽð��� �����Ѵ�. 

	m_pSimAxis[nAxis].m_nStopRate = dDec * 4; // 4 ms

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_e_stop(short nAxis)
{
	// ������ ���� e_stop_event �� ����, ���� �̵��� ����� �����.

	m_pSimAxis[nAxis].m_nAxisState = E_STOP_EVENT;
	
	wait_for_done(nAxis);
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_e_stop_rate(short nAxis, short dDec)
{
	// ������ ���� e_stop_event ����� ���ӽð��� �����Ѵ�. 

	m_pSimAxis[nAxis].m_nEStopRate = dDec * 4; // 4 ms
	
	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::mmc_dwell(short nAxis, LONG lTm)
{
	// �ش� ���� ���������� ������ duration data ��ŭ �����Ѵ�. 

	m_Trace.Log(_T("[NOT CODING] mmc_dwell()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

// ============================================================================
short CAxSimMmcWfp::set_position(short nAxis, double dPos)
{
	// ���� ������ġ �� ��ǥ��ġ�� �����Ѵ�.
	m_pSimAxis[nAxis].m_Timer.Start();

	m_pSimAxis[nAxis].m_dCommand = dPos;
	m_pSimAxis[nAxis].m_dPosition = dPos * 4;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_position(short nAxis, double* pdPos)
{
	// ���� ������ġ(������ġ)�� �о� ���δ�.

	*pdPos = m_pSimAxis[nAxis].m_dPosition;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_command(short nAxis, double dPos)
{
	// ���� ��ǥ��ġ�� �����Ѵ�.

	m_pSimAxis[nAxis].m_dCommand = dPos;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_command(short nAxis, double* pdPos)
{
	// ���� ��ǥ��ġ�� �о� ���δ�.

	*pdPos = m_pSimAxis[nAxis].m_dCommand;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_error(short nAxis, double* pdError)
{
	// ���� ��ǥ��ġ�� ������ġ�� ���̰��� ��ġ������ �о� ���δ�.

	*pdError = fabs(m_pSimAxis[nAxis].m_dPosition - m_pSimAxis[nAxis].m_dCommand);

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

// ============================================================================
short CAxSimMmcWfp::in_sequence(short nAxis)
{
	// ���� �̵����

	return m_pSimAxis[nAxis].m_nInSequence;
	// 0 : �Ϸ�
	// 1 : ������
}

short CAxSimMmcWfp::in_motion(short nAxis)
{
	// ���� �ӵ����

	return m_pSimAxis[nAxis].m_nInMotion;
	// 0 : �ӵ� ��� = 0
	// 1 : �ӵ� ��� != 0
}

short CAxSimMmcWfp::in_position(short nAxis)
{
	// ���� ��ġ����

	return m_pSimAxis[nAxis].m_nInPosition;
	// 0 : ������ 
	// 1 : ������
}

short CAxSimMmcWfp::motion_done(short nAxis)
{
	return !m_pSimAxis[nAxis].m_nInSequence && !m_pSimAxis[nAxis].m_nInMotion;
	// 1 : !in_sequence && !in_motion
}

short CAxSimMmcWfp::axis_done(short nAxis)
{
	return !m_pSimAxis[nAxis].m_nInSequence && !m_pSimAxis[nAxis].m_nInMotion && m_pSimAxis[nAxis].m_nInPosition;
	// 1 : motion_done && in_position
}

short CAxSimMmcWfp::axis_state(short nAxis)
{
	// ���� ���� event �߻����¸� �����ش�.

	return m_pSimAxis[nAxis].m_nAxisState;
	// 0 : NO_EVENT
	// 1 : STOP_EVENT
	// 2 : E_STOP_EVENT
	// 3 : ABORT_EVENT
}

short CAxSimMmcWfp::axis_source(short nAxis)
{
	// mmc ����� ���� ���� ������¸� �о���δ�.

	return m_pSimAxis[nAxis].m_nAxisSource;
	// 0x0000 : ST_NONE
	// 0x0001 : ST_HOME_SWITCH
	// 0x0002 : ST_POS_LIMIT
	// 0x0004 : ST_NEG_LIMIT
	// 0x0008 : ST_AMP_FAULT
	// 0x0010 : ST_A_LIMIT
	// 0x0020 : ST_V_LIMIT
	// 0x0040 : ST_X_NEG_LIMIT
	// 0x0080 : ST_X_POS_LIMIT
	// 0x0100 : ST_ERROR_LIMIT
	// 0x0200 : ST_PC_COMMAND
	// 0x0400 : ST_OUT_OF_FRAMES
	// 0x0800 : ST_AMP_POWER_ONOFF
	// 0x1000 : ST_ABS_COMM_ERROR
	// 0x2000 : ST_INPOSITION_STATUS
	// 0x4000 : ST_RUN_STOP_COMMAND
	// 0x8000 : ST_COLLISION_STATE
}

short CAxSimMmcWfp::clear_status(short nAxis)
{
	// �߻��� event�� �����ϰ� ������ɺ��� �����Ѵ�.

	m_pSimAxis[nAxis].m_nAxisState = 0;
	m_pSimAxis[nAxis].m_nAxisSource = 0;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_ON_MOTION(9)
}

short CAxSimMmcWfp::frames_clear(short nAxis)
{
	// host(pc)���� mmc������ frame buffer�� clear ��Ų��.

	m_Trace.Log(_T("[NOT CODING] frames_clear()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
	// MMC_ON_MOTION(9)
}

// ============================================================================
short CAxSimMmcWfp::set_positive_limit(short nAxis, short nAction)
{
	// + ���� limit switch active �ÿ� ������ event �� �����Ѵ�.

	m_pSimAxis[nAxis].m_nPositiveEvent = nAction;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_positive_limit(short nAxis, short* pnAction)
{
	// + ���� limit switch active �ÿ� ������ event �� �д´�.

	*pnAction = m_pSimAxis[nAxis].m_nPositiveEvent;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_positive_level(short nAxis, short nLevel)
{
	// + ���� limit switch �� active ���¸� high(1) �Ǵ� low(0) �� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] set_positive_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_positive_level(short nAxis, short* pnLevel)
{
	// + ���� limit switch �� active ���¸� �д´�.

	m_Trace.Log(_T("[NOT CODING] get_positive_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_negative_limit(short nAxis, short nAction)
{
	// - ���� limit switch active �ÿ� ������ event �� �����Ѵ�.

	m_pSimAxis[nAxis].m_nNegativeEvent = nAction;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_negative_limit(short nAxis, short* pnAction)
{
	// - ���� limit switch active �ÿ� ������ event �� �д´�.

	*pnAction = m_pSimAxis[nAxis].m_nNegativeEvent;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_negative_level(short nAxis, short nLevel)
{
	// - ���� limit switch �� active ���¸� high(1) �Ǵ� low(0) �� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] set_negative_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_negative_level(short nAxis, short* pnLevel)
{
	// - ���� limit switch �� active ���¸� �д´�.

	m_Trace.Log(_T("[NOT CODING] get_negative_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_home(short nAxis, short nAction)
{
	// home sensor�� �������� ������ event�� �����Ѵ�.

	m_pSimAxis[nAxis].m_nHomeEvent = nAction;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_home(short nAxis, short* pnAction)
{
	// home sensor�� �������� ������ event�� �д´�.

	*pnAction = m_pSimAxis[nAxis].m_nHomeEvent;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_home_level(short nAxis, short nLevel)
{
	// home sensor�� active level ���¸� high(1) �Ǵ� low(0)�� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] set_home_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_home_level(short nAxis, short* pnLevel)
{
	// home sensor�� active level ���¸� high(1) �Ǵ� low(0)�� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] get_home_level()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_index_required(short nAxis, short nIndex)
{
	// �������ͽÿ� encoder�� c(����)�� �޽� �̿� ���θ� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] set_index_required()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
}

short CAxSimMmcWfp::get_index_required(short nAxis, short* nIndex)
{
	// �������ͽÿ� encoder�� c(����)�� �޽� �̿� ���θ� �����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] get_index_required()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
}

// ============================================================================
short CAxSimMmcWfp::controller_run(short nAxis)
{
	// �������� pid ��� �ǽ��Ѵ�.

	m_Trace.Log(_T("[NOT CODING] controller_run()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::controller_idle(short nAxis)
{
	// �������� pid ��� disable ���·� ����� analog ��������� 0 volt�� ����Ѵ�.

	m_Trace.Log(_T("[NOT CODING] controller_idle()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_amp_fault(short nAxis, short nAction)
{
	// amp drive�� fault �߻��� ������ event�� �����Ѵ�.

	m_pSimAxis[nAxis].m_nAmpFaultEvent = nAction;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_amp_fault(short nAxis, short* pnAction)
{
	// amp drive�� fault �߻��� ������ event�� �д´�.

	*pnAction = m_pSimAxis[nAxis].m_nAmpFaultEvent;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::amp_fault_set(short nAxis)
{
	// �������� amp fault�� clear ��Ų��.

	m_Trace.Log(_T("[NOT CODING] amp_fault_set()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::amp_fault_reset(short nAxis)
{
	// �������� amp fault port�� enable ���·� �����.

	m_Trace.Log(_T("[NOT CODING] amp_fault_reset()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_amp_enable(short nAxis, short nAction)
{
	// amp enalble/disable ��带 �����Ѵ�.
	// 0:disable, 1:enable

	m_pSimAxis[nAxis].m_nAmpEnable = nAction;
	m_pSimAxis[nAxis].m_nAxisSource = 0;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
}

short CAxSimMmcWfp::get_amp_enable(short nAxis, short* pnState)
{
	// amp enable/disable ���¸� �о���δ�.
	// 0:disable, 1:enable

	*pnState = m_pSimAxis[nAxis].m_nAmpEnable;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
}

// ============================================================================
short CAxSimMmcWfp::set_in_position(short nAxis, double dInpos)
{
	// ���� ��ġ���� �Ϸ� ���� �����Ѵ�.

	m_pSimAxis[nAxis].m_dInposition = dInpos;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_in_position(short nAxis, double* pdInpos)
{
	// ���� ��ġ���� �Ϸ� ���� �����Ѵ�.

	*pdInpos = m_pSimAxis[nAxis].m_dInposition;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_INVALID_AXIS(3)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_collision_prevent(short max, short sax, short add_sub, short non_equal, double pos)
{
	// �浹���� ����� ����� master/slave�� �� �浹���� �Ÿ� �� ����(+,1,>,<)�� �����ϴ� �Լ���
	// add_sub		0:master �� ������ġ - slave �� ������ġ
	//				1:master �� ������ġ + slave �� ������ġ
	// non_equal	1:pos > add_sub �� ���ġ
	//				0:pos < add_sub �� ���ġ

	m_Trace.Log(_T("[NOT CODING] set_collision_prevent()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_collision_prevent_flag(short bd_num, short mode)
{
	// �浹���� ����� ��뿩�θ� �����ϴ� �Լ��̴�.
	// 0:�������忡�� �浹���� ����� ������� ����.
	// 1:�������忡�� �浹���� ����� �����.

	m_Trace.Log(_T("[NOT CODING] set_collision_prevent_flag()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_collision_prevent_flag(short bd_num, short *pmode)
{
	// �浹���� ����� ��뿩�θ� �о���̴� �Լ��̴�.
	// 0:�������忡�� �浹���� ����� ������� ����.
	// 1:�������忡�� �浹���� ����� �����.

	m_Trace.Log(_T("[NOT CODING] get_collision_prevent_flag()"));

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::home_switch(short nAxis)
{
	// ���������� ���¸� �о���δ�.
	// 1:Active
	// 0:No Active

	return 1;
}

short CAxSimMmcWfp::pos_switch(short nAxis)
{
	// +���� ����Ʈ ������ ���¸� �о���δ�.
	// 1:Active
	// 0:No Active

	return 1;
}

short CAxSimMmcWfp::neg_switch(short nAxis)
{
	// -���� ����Ʈ ������ ���¸� �о���δ�.
	// 1:Active
	// 0:No Active

	return 1;
}

short CAxSimMmcWfp::set_io(short nBoard, long lValue)
{
	m_pOutput[nBoard] = lValue;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_IO(5)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_io(short nBoard, long *plValue)
{
	*plValue = m_pInput[nBoard];

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_IO(5)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::get_out_io(short nBoard, long *plValue)
{
	*plValue = m_pOutput[nBoard];

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_IO(5)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::set_bit(short nBit)
{
	int nBoard = nBit / 32;
	int nBitIndex = nBit % 32;

	long lMask = (1 << nBitIndex);
	m_pOutput[nBoard] |= lMask;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_IO(5)
	// MMC_ILLEGAL_PARAMETER(6)
}

short CAxSimMmcWfp::reset_bit(short nBit)
{
	int nBoard = nBit / 32;
	int nBitIndex = nBit % 32;

	long lMask = ~(1 << nBitIndex);
	m_pOutput[nBoard] &= lMask;

	return 0;
	// MMC_OK(0)
	// MMC_TIMEOUT_ERR(2)
	// MMC_ILLEGAL_IO(5)
	// MMC_ILLEGAL_PARAMETER(6)
}


