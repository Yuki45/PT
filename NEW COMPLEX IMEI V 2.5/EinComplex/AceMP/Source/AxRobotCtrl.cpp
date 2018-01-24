// AxRobotCtrl.cpp: implementation of the CAxRobotCtrl class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxRobotCtrl.h"
#include "AxErrorMgr.h"
#include "AxSystemHub.h"
#include "AxSystemError.h"
#include "AxMaster.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxRobotCtrl::CAxRobotCtrl(CAxErrorCtrl* pCtrl)
{
	m_pRobot = NULL;
	m_pErrCtrl = pCtrl;
	m_sRobotErrPath = _T("\\Service\\Robot.err");
}

CAxRobotCtrl::~CAxRobotCtrl()
{

}

BOOL CAxRobotCtrl::IsHomeCheck()
{
	return m_pRobot->IsHomeCheck();
}

BOOL CAxRobotCtrl::IsHomeCheck(UINT nAxis)
{
	return m_pRobot->IsHomeCheck(nAxis);
}

BOOL CAxRobotCtrl::IsAmpEnabled()
{
	return m_pRobot->IsAmpEnabled();
}

BOOL CAxRobotCtrl::IsAmpEnabled(UINT nAxis)
{
	return m_pRobot->IsAmpEnabled(nAxis);
}

BOOL CAxRobotCtrl::IsAmpDisabled()
{
	return m_pRobot->IsAmpDisabled();
}

BOOL CAxRobotCtrl::IsAmpDisabled(UINT nAxis)
{
	return m_pRobot->IsAmpDisabled(nAxis);
}

BOOL CAxRobotCtrl::IsRobotReady()
{
	return m_pRobot->IsRobotReady();
}

BOOL CAxRobotCtrl::IsRobotReady(UINT nAxis)
{
	return m_pRobot->IsRobotReady(nAxis);
}

BOOL CAxRobotCtrl::GetCurLoc(CAxRobotLoc* locCur)
{
	return m_pRobot->GetCurLoc(locCur);
}

void CAxRobotCtrl::EnableAmp()
{
	while( !m_pRobot->EnableAmp() ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::EnableAmp(UINT nAxis)
{
	while( !m_pRobot->EnableAmp(nAxis) ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else	
			break;
	}
}

BOOL CAxRobotCtrl::EnableAmpNE()
{
	if( !m_pRobot->EnableAmp() ) 
		return FALSE;

	m_pRobot->m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobotCtrl::EnableAmpNE(UINT nAxis)
{
	if( !m_pRobot->EnableAmp(nAxis) ) 
		return FALSE;

	m_pRobot->m_nRobotError = robotNoError;
	return TRUE;
}

void CAxRobotCtrl::DisableAmp()
{
	while( !m_pRobot->DisableAmp() ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::DisableAmp(UINT nAxis)
{
	while( !m_pRobot->DisableAmp(nAxis) ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::DisableAmpNE()
{
	if( !m_pRobot->DisableAmp() ) 
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DisableAmpNE(UINT nAxis)
{
	if( !m_pRobot->DisableAmp(nAxis) ) 
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::AlarmClear()
{
	while( !m_pRobot->AlarmClear() ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::AlarmClear(UINT nAxis)
{
	while( !m_pRobot->AlarmClear(nAxis) ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::AlarmClearNE()
{
	if( !m_pRobot->AlarmClear() ) 
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::AlarmClearNE(UINT nAxis)
{
	if( !m_pRobot->AlarmClear(nAxis) ) 
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::ClearStatus()
{
	while( !m_pRobot->ClearStatus() ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::ClearStatus(UINT nAxis)
{
	while( !m_pRobot->ClearStatus(nAxis) ) 
	{
		CheckMasterTerminate();

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::ClearStatusNE()
{
	if( !m_pRobot->ClearStatus() ) 
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::ClearStatusNE(UINT nAxis)
{
	if( !m_pRobot->ClearStatus(nAxis) ) 
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::Home()
{
	while( TRUE ) 
	{
		if( m_pRobot->Home() ) 
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}

	WaitMotion(motionScanTime);
}

void CAxRobotCtrl::Home(UINT nAxis)
{
	while( TRUE ) 
	{
		if( m_pRobot->Home(nAxis) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}

	WaitMotion(motionScanTime);
}

BOOL CAxRobotCtrl::HomeNE()
{
	if( !m_pRobot->Home() )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::HomeNE(UINT nAxis)
{
	if( !m_pRobot->Home(nAxis) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::Move(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->Move(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::Move(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Move(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::MoveNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->Move(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::MoveNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->Move(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::MoveXZAx(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->MoveXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::MoveXZAx(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->MoveXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::MoveXZAxNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->MoveXZAx(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::MoveXZAxNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->MoveXZAx(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::MoveWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->Move(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::MoveWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Move(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::MoveWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->Move(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::MoveWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->Move(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::MoveXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->MoveXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::MoveXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->MoveXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::MoveXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->MoveXZAx(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::MoveXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->MoveXZAx(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::Approach(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->Approach(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::Approach(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Approach(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::ApproachNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->Approach(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::ApproachNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->Approach(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::ApproachXZAx(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->ApproachXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::ApproachXZAx(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->ApproachXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::ApproachXZAxNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->ApproachXZAx(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::ApproachXZAxNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->ApproachXZAx(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::ApproachWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->Approach(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::ApproachWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Approach(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::ApproachWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->Approach(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::ApproachWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->Approach(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::ApproachXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->ApproachXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::ApproachXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->ApproachXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::ApproachXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->ApproachXZAx(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::ApproachXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->ApproachXZAx(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::Depart()
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart() )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::Depart(double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::Depart(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::Depart(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::DepartNE()
{
	if( !m_pRobot->Depart() )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartNE(double speed)
{
	if( !m_pRobot->Depart(speed) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->Depart(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->Depart(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::DepartXZAx()
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx() )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::DepartXZAx(double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::DepartXZAx(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::DepartXZAx(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::DepartXZAxNE()
{
	if( !m_pRobot->DepartXZAx() )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartXZAxNE(double speed)
{
	if( !m_pRobot->DepartXZAx(speed) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartXZAxNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->DepartXZAx(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::DepartXZAxNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->DepartXZAx(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::DepartWaitDone(UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart() )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartWaitDone(UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->Depart(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::DepartWaitDoneNE(UINT nTmout)
{
	if( !m_pRobot->Depart() ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartWaitDoneNE(UINT nTmout, double speed)
{
	if( !m_pRobot->Depart(speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->Depart(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->Depart(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::DepartXZAxWaitDone(UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx() )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartXZAxWaitDone(UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::DepartXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->DepartXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::DepartXZAxWaitDoneNE(UINT nTmout)
{
	if( !m_pRobot->DepartXZAx() ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartXZAxWaitDoneNE(UINT nTmout, double speed)
{
	if( !m_pRobot->DepartXZAx(speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->DepartXZAx(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::DepartXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->DepartXZAx(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::JumpXZAx(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::JumpXZAx(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::JumpXZAxNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->JumpXZAx(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::JumpXZAxNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->JumpXZAx(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::JumpXYZAx(const CAxRobotLoc& loc)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXYZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

void CAxRobotCtrl::JumpXYZAx(const CAxRobotLoc& loc, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXYZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
			continue;
		else
			break;
	}
}

BOOL CAxRobotCtrl::JumpXYZAxNE(const CAxRobotLoc& loc)
{
	if( !m_pRobot->JumpXYZAx(loc) )
		return FALSE;

	return TRUE;
}

BOOL CAxRobotCtrl::JumpXYZAxNE(const CAxRobotLoc& loc, double speed)
{
	if( !m_pRobot->JumpXYZAx(loc, speed) )
		return FALSE;

	return TRUE;
}

void CAxRobotCtrl::JumpXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::JumpXZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::JumpXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->JumpXZAx(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::JumpXZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->JumpXZAx(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::JumpXYZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXYZAx(loc) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

void CAxRobotCtrl::JumpXYZAxWaitDone(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	while( TRUE ) 
	{
		if( m_pRobot->JumpXYZAx(loc, speed) )
			break;

		CheckMasterTerminate();

		//if( m_pRobot->m_nRobotError == axisStopState )
		//{
		//	WaitMotion(motionScanTime);
		//	continue;
		//}

		if( RobotError(m_pRobot->m_nRobotError) == emRetry )
			continue;
		else
			break;
	}

	WaitAxisDone(nTmout);
}

BOOL CAxRobotCtrl::JumpXYZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout)
{
	if( !m_pRobot->JumpXYZAx(loc) ) 
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

BOOL CAxRobotCtrl::JumpXYZAxWaitDoneNE(const CAxRobotLoc& loc, UINT nTmout, double speed)
{
	if( !m_pRobot->JumpXYZAx(loc, speed) )
		return FALSE;

	return WaitAxisDoneNE(nTmout);
}

void CAxRobotCtrl::WaitAxisDone(UINT nTime)
{
	m_timer.Start();

	while( !m_pRobot->IsAxisDone() ) 
	{
		CheckMasterTerminate();

		if( IsRobotStopState() )
		{
			m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxRobotCtrl"));
			m_Trace.Log(_T("[%02d Robot] WaitAxisDone() -> RobotStopState"), m_pRobot->m_nRobot);

			m_pRobot->Stop();
			WaitMotion(motionScanTime);
			continue;
		}

		if( m_timer.IsTimeUp(nTime) /*&& !m_pRobot->IsAxisDone()*/ ) 
		{
			if( RobotError(m_pRobot->m_nRobotError) == emRetry ) 
				continue;
			else
				break;
		}
		else
		{
			Sleep(motionScanTime); 
		}
	}

	WaitMotion(motionScanTime); 
}

BOOL CAxRobotCtrl::WaitAxisDoneNE(UINT nTime)
{
	m_timer.Start();
	
	while( !m_pRobot->IsAxisDone() ) 
	{
		CheckMasterTerminate();

		if( IsRobotStopState() )
		{
			m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxRobotCtrl"));
			m_Trace.Log(_T("[%02d Robot] WaitAxisDone() -> RobotStopState"), m_pRobot->m_nRobot);

			m_pRobot->Stop();
			return FALSE;
		}

		if( m_timer.IsTimeUp(nTime) )
			return FALSE;
		else
			Sleep(motionScanTime);
	}

	return TRUE;
}

void CAxRobotCtrl::WaitMotion(UINT nTime)
{
	m_pErrCtrl->m_pControl->Wait(nTime);
}

UINT CAxRobotCtrl::RobotError(UINT nCode, int nType)
{
	CString sFile, sRobot;

	sRobot.Format(_T("Robot-%02d-%02d"), m_pRobot->m_nRobot, m_pRobot->GetErrAxis());
	nType |= emRetry;

	m_pRobot->m_nRobotError = robotNoError;

	return m_pErrCtrl->Error(nCode, nType, sRobot, m_sRobotErrPath);
}

BOOL CAxRobotCtrl::IsRobotStopState()
{
	CAxSystemError* pSystemError = (CAxSystemError*)CAxSystemHub::GetSystemHub()->GetSystem(_T("SystemError"));

	if( pSystemError )
	{
		return pSystemError->GetEmgRobotStop();
	}
	else 
	{
		return FALSE;
	}
}

void CAxRobotCtrl::CheckMasterTerminate()
{
	CAxMaster* pMaster = CAxMaster::GetMaster();

	if( pMaster->GetTeminate() )
	{
		throw -1;
	}
}
