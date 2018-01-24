// AxMmcCmd.cpp: implementation of the CAxMmcCmd class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxMmcCmd.h"
#include "MMCWHP154.h"
#include "AxMmcMgr.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CAxMmcCmd* CAxMmcCmd::theMmcCmd = NULL;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxMmcCmd::CAxMmcCmd() //: m_lock(&m_mutex)
{
	m_hMutex = m_mutex.m_hObject;
	m_bMmcMgrSimulate = FALSE;
	m_pSimMmcWfp = NULL;
}

CAxMmcCmd::~CAxMmcCmd()
{
	if( m_pSimMmcWfp != NULL )
		delete m_pSimMmcWfp;
}

CAxMmcCmd* CAxMmcCmd::GetMmcCmd()
{
	if(theMmcCmd == NULL) theMmcCmd = new CAxMmcCmd();
	return theMmcCmd;
}

void CAxMmcCmd::MmcErr(const int nCode)
{
	if( nCode != MMC_OK )
	{
		UINT nErrCode;

		if( nCode == FUNC_ERR )
			nErrCode = 201;
		else
			nErrCode = nCode + 100;
		
		ReleaseMutex(m_hMutex);
		throw nErrCode;
	}
}

void CAxMmcCmd::MmcInitx(const UINT nTotalBdNum, long* lpAddr/*, BOOL bMmcMgrSimulate, const UINT nNumAxis*/)
{
//	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceMmcCmd"));
//	m_Trace.SetEnableWriteLog(FALSE);

	CAxMmcMgr* pMmcMgr = CAxMmcMgr::GetMmcMgr();

	m_bMmcMgrSimulate = pMmcMgr->GetSimulate();
	UINT nNumAxis = pMmcMgr->GetNumAxis();
	
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate ) //(M) TEST
	{
		int e = mmc_initx(nTotalBdNum, lpAddr);
		if( e ) 
		{
			if( e == MMC_TIMEOUT_ERR )  
				set_dpram_addr(0,0xd8000000);
			else
				MmcErr(e);
		}
	}
	else
	{
		m_pSimMmcWfp = CAxSimMmcWfp::GetSimMmcWfp();
		m_pSimMmcWfp->mmc_initx(nTotalBdNum, nNumAxis);
	}
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::StartMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( start_move(nAxis, dPos, dVel, dAcc) );
	else
		m_pSimMmcWfp->start_move(nAxis, dPos, dVel, dAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::StartTMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc, const int dDec)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( start_t_move(nAxis, dPos, dVel, dAcc, dDec) );
	else
		m_pSimMmcWfp->start_t_move(nAxis, dPos, dVel, dAcc, dDec);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::StartRMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( start_r_move(nAxis, dPos, dVel, dAcc) );
	else
		m_pSimMmcWfp->start_r_move(nAxis, dPos, dVel, dAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::Move(const UINT nAxis, const double dPos, const double dVel, const int dAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( move(nAxis, dPos, dVel, dAcc) );
	else
		m_pSimMmcWfp->move(nAxis, dPos, dVel, dAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::TMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc, const int dDec)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( t_move(nAxis, dPos, dVel, dAcc, dDec) );
	else
		m_pSimMmcWfp->t_move(nAxis, dPos, dVel, dAcc, dDec);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::RMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( r_move(nAxis, dPos, dVel, dAcc) );
	else
		m_pSimMmcWfp->r_move(nAxis, dPos, dVel, dAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::WaitForDone(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( wait_for_done(nAxis) );
	else
		m_pSimMmcWfp->wait_for_done(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::StartMoveAll(const UINT nNumAxis, UINT* pnAxisMap, double* pdPos, double* pdVel, int* pnAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( start_move_all(nNumAxis, (short*)pnAxisMap, pdPos, pdVel, (short*)pnAcc) );
	else
		m_pSimMmcWfp->start_move_all(nNumAxis, (short*)pnAxisMap, pdPos, pdVel, (short*)pnAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::VMove(const UINT nAxis, const double dVel, const int dAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate ) //(M) TEST
		MmcErr( v_move(nAxis, dVel, dAcc) );
	else
		m_pSimMmcWfp->v_move(nAxis, dVel, dAcc);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::VStop(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex,INFINITE);
	if( !m_bMmcMgrSimulate ) //(M) TEST
		MmcErr( v_move_stop(nAxis) );
	else
		m_pSimMmcWfp->v_move_stop(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::MapAxes(const UINT nNumAxis, UINT* pnAxisMap)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( map_axes(nNumAxis, (short*)pnAxisMap) );
	else
		m_pSimMmcWfp->map_axes(nNumAxis, (short*)pnAxisMap);
	ReleaseMutex(m_hMutex);
}


void CAxMmcCmd::SetMoveSpeed(const double dSpeed)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_move_speed(dSpeed) );
	else
		m_pSimMmcWfp->set_move_speed(dSpeed);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetMoveAccel(const int nAccel)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_move_accel(nAccel) );
	else
		m_pSimMmcWfp->set_move_accel(nAccel);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::Move2(const double d1, const double d2)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate)
		MmcErr( move_2(d1, d2) );
	else
		m_pSimMmcWfp->move_2(d1, d2);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::Move3(const double d1, const double d2, const double d3)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( move_3(d1, d2, d3) );
	else
		m_pSimMmcWfp->move_3(d1, d2, d3);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::Move4(const double d1, const double d2, const double d3, const double d4)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( move_4(d1, d2, d3, d4) );
	else
		m_pSimMmcWfp->move_4(d1, d2, d3, d4);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetPosition(const UINT nAxis, const double dPos)
{
//	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_position(nAxis, dPos) );
	else
		m_pSimMmcWfp->set_position(nAxis, dPos);
//	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::GetPosition(const UINT nAxis, double* pdPos)
{
//	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate ) //(M) TEST
		MmcErr( get_position(nAxis, pdPos) );
	else
		m_pSimMmcWfp->get_position(nAxis, pdPos);
//	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetCommand(const UINT nAxis, const double dPos)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_command(nAxis, dPos) );
	else
		m_pSimMmcWfp->set_command(nAxis, dPos);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::GetCommand(const UINT nAxis, double* pdPos)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_command(nAxis, pdPos) );
	else
		m_pSimMmcWfp->get_command(nAxis, pdPos);
	ReleaseMutex(m_hMutex);
}

BOOL CAxMmcCmd::InSequence(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = in_sequence(nAxis);
	else
		bRetVal = m_pSimMmcWfp->in_sequence(nAxis);
	ReleaseMutex(m_hMutex);

	return bRetVal;
}

BOOL CAxMmcCmd::InMotion(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = in_motion(nAxis);
	else
		bRetVal = m_pSimMmcWfp->in_motion(nAxis);
	ReleaseMutex(m_hMutex);

	return bRetVal;
}

BOOL CAxMmcCmd::InPosition(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = in_position(nAxis);
	else
		bRetVal = m_pSimMmcWfp->in_position(nAxis);
	ReleaseMutex(m_hMutex);

	return bRetVal;
}

BOOL CAxMmcCmd::MotionDone(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = motion_done(nAxis);
	else
		bRetVal = m_pSimMmcWfp->motion_done(nAxis);
	ReleaseMutex(m_hMutex);

	return bRetVal;
}

BOOL CAxMmcCmd::AxisDone(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = axis_done(nAxis);
	else
		bRetVal = m_pSimMmcWfp->axis_done(nAxis);
	ReleaseMutex(m_hMutex);

	return bRetVal;
}

UINT CAxMmcCmd::AxisState(const UINT nAxis)
{
	UINT nRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		nRetVal = axis_state(nAxis);
	else
		nRetVal = m_pSimMmcWfp->axis_state(nAxis);
	ReleaseMutex(m_hMutex);

	return nRetVal;
}

UINT CAxMmcCmd::AxisSource(const UINT nAxis)
{
	UINT nRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		nRetVal = axis_source(nAxis);
	else
		nRetVal = m_pSimMmcWfp->axis_source(nAxis);
	ReleaseMutex(m_hMutex);

	return nRetVal;
}

void CAxMmcCmd::SetStop(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_stop(nAxis) );
	else
		m_pSimMmcWfp->set_stop(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetStopRate(const UINT nAxis, const int dDec)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_stop_rate(nAxis, dDec) );
	else
		m_pSimMmcWfp->set_stop_rate(nAxis, dDec);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::ControllerIdle(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( controller_idle(nAxis) );
	else
		m_pSimMmcWfp->controller_idle(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::DisableAmp(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_amp_enable(nAxis, 0) );
	else
		m_pSimMmcWfp->set_amp_enable(nAxis, 0);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::ClearStatus(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( clear_status(nAxis) );
	else
		m_pSimMmcWfp->clear_status(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::ControllerRun(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( controller_run(nAxis) );
	else
		m_pSimMmcWfp->controller_run(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::EnableAmp(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_amp_enable(nAxis, 1) );
	else
		m_pSimMmcWfp->set_amp_enable(nAxis, 1);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::MmcDwell(const UINT nAxis, const long lTm)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( mmc_dwell(nAxis, lTm) );
	else
		m_pSimMmcWfp->mmc_dwell(nAxis, lTm);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::FramesClear(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( frames_clear(nAxis) );
	else
		m_pSimMmcWfp->frames_clear(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetHome(const UINT nAxis, const UINT nAction)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_home(nAxis, nAction) );
	else
		m_pSimMmcWfp->set_home(nAxis, nAction);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetHomeLevel(const UINT nAxis, const BOOL bLevel)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_home_level(nAxis, bLevel) );
	else
		m_pSimMmcWfp->set_home_level(nAxis, bLevel);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetHomeIndexRequired(const UINT nAxis, const BOOL bIndex)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_index_required(nAxis, bIndex) );
	else
		m_pSimMmcWfp->set_index_required(nAxis, bIndex);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::GetAmpFault(const UINT nAxis, UINT* pnAction)
{
	*pnAction = 0;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_amp_fault(nAxis, (short*)pnAction) );
	else
		m_pSimMmcWfp->get_amp_fault(nAxis, (short*)pnAction);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::AmpFaultReset(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( amp_fault_reset(nAxis) );
	else
		m_pSimMmcWfp->amp_fault_reset(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::AmpFaultSet(const UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( amp_fault_set(nAxis) );
	else
		m_pSimMmcWfp->amp_fault_set(nAxis);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::GetAmpEnable(const UINT nAxis, UINT* pnState)
{
	*pnState = 0;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_amp_enable(nAxis, (short*)pnState) );
	else
		m_pSimMmcWfp->get_amp_enable(nAxis, (short*)pnState);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetInPosition(const UINT nAxis, const double dTol)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_in_position(nAxis, dTol) );
	else
		m_pSimMmcWfp->set_in_position(nAxis, dTol);
	ReleaseMutex(m_hMutex);
}

void CAxMmcCmd::SetCollisionPrevent(const UINT max, const UINT sax, const UINT add_sub, const UINT non_equal, const double pos)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_collision_prevent(max, sax, add_sub, non_equal, pos) );
	else
		m_pSimMmcWfp->set_collision_prevent(max, sax, add_sub, non_equal, pos);
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::SetCollisionPreventFlag(const UINT bd_num, const UINT mode)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_collision_prevent_flag(bd_num, mode) );
	else
		m_pSimMmcWfp->set_collision_prevent_flag(bd_num, mode);
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::GetCollisionPreventFlag(const UINT bd_num, UINT* pmode)
{
	*pmode = 0;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_collision_prevent_flag(bd_num, (short*)pmode) );
	else
		m_pSimMmcWfp->get_collision_prevent_flag(bd_num, (short*)pmode);
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::SplLineMove2Ax(const UINT nAxis1, const UINT nAxis2, double* pdPos, const double dVel, const int nAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( spl_line_move2ax(nAxis1, nAxis2, pdPos, dVel, nAcc) );
	else
		m_pSimMmcWfp->spl_line_move2ax(nAxis1, nAxis2, pdPos, dVel, nAcc);
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::SplLineMove3Ax(const UINT nAxis1, const UINT nAxis2, const UINT nAxis3, double* pdPos, const double dVel, const int nAcc)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( spl_line_move3ax(nAxis1, nAxis2, nAxis3, pdPos, dVel, nAcc) );
	else
		m_pSimMmcWfp->spl_line_move3ax(nAxis1, nAxis2, nAxis3, pdPos, dVel, nAcc);
	ReleaseMutex(m_hMutex); 
}

BOOL CAxMmcCmd::GetHomeSwitch(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = home_switch(nAxis);
	else
		bRetVal = m_pSimMmcWfp->home_switch(nAxis);
	ReleaseMutex(m_hMutex); 

	return bRetVal;
}

BOOL CAxMmcCmd::GetPosSwitch(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = pos_switch(nAxis);
	else
		bRetVal = m_pSimMmcWfp->pos_switch(nAxis);
	ReleaseMutex(m_hMutex); 

	return bRetVal;
}

BOOL CAxMmcCmd::GetNegSwitch(const UINT nAxis)
{
	BOOL bRetVal;

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		bRetVal = neg_switch(nAxis);
	else
		bRetVal = m_pSimMmcWfp->neg_switch(nAxis);
	ReleaseMutex(m_hMutex); 

	return bRetVal;
}

void CAxMmcCmd::PositionIOOnOff(UINT nPosNum, int nBitNo, UINT nAxis, double dPos, int nEncFlag)
{
	//////////////////////////////////////////////////////////////////////////
	//(M) nEncFlag
	// 0 : Command 값을 읽어서 사용, 현재위치의 절대값이 목표위치의 절대값보다 커졌을 때 출력
	// 1 : Encoder 값을 읽어서 사용, 현재위치의 절대값이 목표위치의 절대값보다 커졌을 때 출력
	// 2 : Command 값을 읽어서 사용, 현재위치의 값이 목표위치의 값보다 커졌을 때 출력
	// 3 : Encoder 값을 읽어서 사용, 현재위치의 값이 목표위치의 값보다 커졌을 때 출력
	// 4 : Command 값을 읽어서 사용, 현재위치의 값이 목표위치의 값보다 작아졌을 때 출력
	// 5 : Encoder 값을 읽어서 사용, 현재위치의 값이 목표위치의 값보다 작아졌을 때 출력
	//////////////////////////////////////////////////////////////////////////

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_io_onoff(nPosNum, nBitNo, nAxis, dPos, nEncFlag) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PositionIOClear(UINT nAxis, UINT nPosNum)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_io_clear(nAxis, nPosNum) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PositionIOAllClear(UINT nAxis)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_io_allclear(nAxis) );
	ReleaseMutex(m_hMutex); 
}

// nIdxSel = 1(1 축만 사용), 2(2축 사용)
// nAxis2  = nIdxSel 이 1 이면 사용안함
void CAxMmcCmd::PosCmpInit(int nIdxSel, UINT nAxis1, UINT nAxis2)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_init(nIdxSel, nAxis1, nAxis2) );
	ReleaseMutex(m_hMutex); 
}

// nBdNum = 0 ~ 7
// nFlag  = 0(Disable), 1(Enable)
void CAxMmcCmd::PosCmpEnable(UINT nBdNum, int nFlag)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_enable(nBdNum, nFlag) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PosCmpReset(UINT n1)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_reset(n1) );
	ReleaseMutex(m_hMutex); 
}

// nBdNum  = 0 ~ 7
// nIdxSel = 1(Only Sel 1 Axis), 2(Only Sel 2 Axis), 3(All Axis)
void CAxMmcCmd::PosCmpIndexClear(UINT nBdNum, int nIdxSel)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_index_clear(nBdNum, nIdxSel) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PosCmpInterval(int nDir, UINT nAxis, int nBitNo, double dStartPos, double dLimitPos, long lInterval, long lTime)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_interval(nDir, nAxis, nBitNo, dStartPos, dLimitPos, lInterval, lTime) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PosCmpRead(int nIdxSel, UINT nAxis, double *pdPos)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_read(nIdxSel, nAxis, pdPos) );
	ReleaseMutex(m_hMutex); 
}

void CAxMmcCmd::PosCmpBit(UINT n1, UINT n2, UINT n3)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare_bit(n1, n2, n3) );
	ReleaseMutex(m_hMutex); 
}

// nPosNum  = 1 ~ 4096
// nBitNo   = 0 ~ // 보드당 16점
// nLatch   = 0(lTime 만큼 지속), 1(계속유지)

// nIdxSel  = 1(1축만 사용), 2(2축 사용)
// nFunc    = 1(엔코더==설정값), 2(엔코더<설정값), 3(엔코더>설정값)
// nOutMove = 0(축별 ON/OFF), 1(2축 AND), 2(2축 OR)
// lTime    = 10 ~ 65535 us
void CAxMmcCmd::PosCmp(UINT nPosNum, int nBitNo, UINT nAxis, int nLatch, double dPos, long lTime)
{
	int nIdxSel  = 1; // half size 에서는 무조건
	int nFunc    = 1; // half size 에서는 무조건
	int nOutMode = 0; // half size 에서는 무조건

	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( position_compare(nIdxSel, nPosNum, nBitNo, nAxis, nAxis, nLatch, nFunc, nOutMode, dPos, lTime) );
	ReleaseMutex(m_hMutex); 
}

// board index 0 ~
void CAxMmcCmd::SetIO(UINT nBdNum, long lValue)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_io(nBdNum, lValue) ); 
	else
		m_pSimMmcWfp->set_io(nBdNum, lValue);
	ReleaseMutex(m_hMutex); 
}

// board index 0 ~
void CAxMmcCmd::GetIO(UINT nBdNum, long* plValue)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_io(nBdNum, plValue) ); 
	else
		m_pSimMmcWfp->get_io(nBdNum, plValue);
	ReleaseMutex(m_hMutex); 
}

// board index 0 ~
void CAxMmcCmd::GetOutIO(UINT nBdNum, long* plValue)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( get_out_io(nBdNum, plValue) ); 
	else
		m_pSimMmcWfp->get_out_io(nBdNum, plValue);
	ReleaseMutex(m_hMutex); 
}

// board index 0 ~, bit index 0 ~
void CAxMmcCmd::GetInputBit(UINT nBdNum, int nBitNo, BOOL *pbValue)
{
	long lValue;
	
	//WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
	{
		MmcErr( get_io(nBdNum, &lValue) ); 
		*pbValue = (BOOL)(lValue & (1 << nBitNo));
	}
	else
	{
		m_pSimMmcWfp->get_io(nBdNum, &lValue);
		*pbValue = (BOOL)(lValue & (1 << nBitNo));
	}
	//ReleaseMutex(m_hMutex); 
}

// board index 0 ~, bit index 0 ~
void CAxMmcCmd::GetOutputBit(UINT nBdNum, int nBitNo, BOOL *pbValue)
{
	long lValue;
	
	//WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
	{
		MmcErr( get_out_io(nBdNum, &lValue) ); 
		*pbValue = (BOOL)(lValue & (1 << nBitNo));
	}
	else
	{
		m_pSimMmcWfp->get_out_io(nBdNum, &lValue);
		*pbValue = (BOOL)(lValue & (1 << nBitNo));
	}
	//ReleaseMutex(m_hMutex); 
}

// board index 0 ~, bit index 0 ~
void CAxMmcCmd::SetBit(UINT nBdNum, int nBitNo)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( set_bit(nBdNum * 32 + nBitNo) ); 
	else
		m_pSimMmcWfp->set_bit(nBdNum * 32 + nBitNo);
	ReleaseMutex(m_hMutex); 
}

// board index 0 ~, bit index 0 ~
void CAxMmcCmd::ResetBit(UINT nBdNum, int nBitNo)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		MmcErr( reset_bit(nBdNum * 32 + nBitNo) ); 
	else
		m_pSimMmcWfp->reset_bit(nBdNum * 32 + nBitNo);
	ReleaseMutex(m_hMutex); 
}

// Set Sampling Time..
void CAxMmcCmd::SetScanTime(int nBNum, int Samplingrate)
{
	WaitForSingleObject(m_hMutex, INFINITE);
	if( !m_bMmcMgrSimulate )
		fset_control_timer(nBNum, Samplingrate);
	else
		
	ReleaseMutex(m_hMutex); 
}
