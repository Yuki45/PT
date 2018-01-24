#ifndef __AX_SIMMMCWFP_H__
#define __AX_SIMMMCWFP_H__

#pragma once

#include "AxTimer.h"
#include "AxTrace.h"
#include "AxThread.h"

enum SimMoveType {	
	SMT_RELATIVE,	// 상대위치 이동
	SMT_ABSOLUTE,	// 절대위치 이동
	SMT_V_MOVE		// 속도값으로 이동
};

enum SimVelProfile {
	SVP_TRAPEZOID,	// 사다리꼴
	SVP_SCURVE		// S 커브
};

enum {
	timePolling	= 1
};

class __declspec(dllexport) CAxSimMoveInfo
{
public:
	SimMoveType		m_nMoveType;
	SimVelProfile	m_nVelProfile;
	BOOL			m_bWait;
	double			m_dDestPosition;
	double			m_dVelocity;
	short			m_nAccel;
	short			m_nDecel;
	BOOL			m_bForward;
	double			m_dMoveDistance;

	CAxSimMoveInfo();
	CAxSimMoveInfo(const CAxSimMoveInfo& clsSimMoveInfo);
	CAxSimMoveInfo(SimMoveType nMoveType, SimVelProfile nVelProfile, BOOL bWait, double dDestPosition, double dVelocity, short nAccel, short nDecel, BOOL bForward);
	virtual ~CAxSimMoveInfo();

	void SetData(SimMoveType nMoveType, SimVelProfile nVelProfile, BOOL bWait, double dDestPosition, double dVelocity, short nAccel, short nDecel, BOOL bForward);

	const CAxSimMoveInfo& operator = (const CAxSimMoveInfo& clsSimMoveInfo);
};

class __declspec(dllexport) CAxSimAxis : public CAxThread
{
public:
	double			m_dPosition;
	double			m_dCommand;
	double			m_dInposition;
	double			m_dPrevPosition;
	short			m_nAxisState;
	short			m_nAxisSource;
	short			m_nInSequence;
	short			m_nInMotion;
	short			m_nInPosition;
	short			m_nAmpEnable;
	short			m_nStopRate;
	short			m_nEStopRate;
	short			m_nHomeEvent;
	short			m_nPositiveEvent;
	short			m_nNegativeEvent;
	short			m_nAmpFaultEvent;
	CAxSimMoveInfo	m_clsMoveInfo;
	BOOL			m_bVMoveStop;
	BOOL			m_bStartAction;
	CAxTimer		m_Timer;
	CAxWaitTimer	m_wtSleep;

	CAxSimAxis();
	~CAxSimAxis();

	UINT	PriRun();
	UINT	SecRun() { return 0; }
	UINT	AutoRun();
	void	Startup();
	void	SetPreAction();
	void	SetPostAction();
	void	SetMoveInfo(const CAxSimMoveInfo& clsSimMoveInfo);
	void	GetTransformMoveInfo(double& dTimeAcc, double& dTimeVel, double& dTimeDec);
	double	MoveAcc(double dTime, BOOL bForward);
	double	MoveVel(double dTime, BOOL bForward);
	double	MoveDec(double dTime, BOOL bForward, BOOL bStopEventRun);
	short	GetDec();
	void	SetAxisState(short nState);
};

class __declspec(dllexport) CAxSimMmcWfp
{
public:
	CAxSimMmcWfp();
	~CAxSimMmcWfp();

	short mmc_initx(short nTotalBdNum, short nNumAxis);
	short start_move(short nAxis, double dPos, double dVel, short dAcc);
	short start_r_move(short nAxis, double dDist, double dVel, short dAcc);
	short start_t_move(short nAxis, double dPos, double dVel, short dAcc, short dDec);
	short start_tr_move(short nAxis, double dDist, double dVel, short dAcc, short dDec);
	short move(short nAxis, double dPos, double dVel, short dAcc);
	short r_move(short nAxis, double dDist, double dVel, short dAcc);
	short t_move(short nAxis, double dPos, double dVel, short dAcc, short dDec);
	short tr_move(short nAxis, double dDist, double dVel, short dAcc, short dDec);
	short wait_for_done(short nAxis);
	short start_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc);
	short start_t_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc, short* pnDec);
	short move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc);
	short t_move_all(short nNumAxis, short* pnAxisMap, double* pdPos, double* pdVel, short* pnAcc, short* pnDec);
	short wait_for_all(short nNumAxis, short* pnAxisMap);
	short v_move(short nAxis, double dVel, short dAcc);
	short v_move_stop(short nAxis);
	short map_axes(short nNumAxis, short* pnAxisMap);
	short set_move_speed(double dSpeed);
	short set_move_accel(short nAccel);
	short all_done();
	short move_2(double d1, double d2);
	short move_3(double d1, double d2, double d3);
	short move_4(double d1, double d2, double d3, double d4);
	short move_n(double* pDn);
	short spl_line_move2ax(short nAxis1, short nAxis2, double* pdPos, double dVel, short nAcc);
	short spl_line_move3ax(short nAxis1, short nAxis2, short nAxis3, double* pdPos, double dVel, UINT nAcc);
	short set_stop(short nAxis);
	short set_stop_rate(short nAxis, short dDec);
	short set_e_stop(short nAxis);
	short set_e_stop_rate(short nAxis, short dDec);
	short mmc_dwell(short nAxis, long lduration);
	short mmcDelay(long lduration);
	short set_position(short nAxis, double dPos);
	short get_position(short nAxis, double* pdPos);
	short set_command(short nAxis, double dPos);
	short get_command(short nAxis, double* pdPos);
	short get_error(short nAxis, double* pdError);
	short in_sequence(short nAxis);
	short in_motion(short nAxis);
	short in_position(short nAxis);
	short motion_done(short nAxis);
	short axis_done(short nAxis);
	short axis_state(short nAxis);
	short axis_source(short nAxis);
	short clear_status(short nAxis);
	short frames_clear(short nAxis);
	short set_positive_limit(short nAxis, short nAction);
	short get_positive_limit(short nAxis, short* nAction);
	short set_positive_level(short nAxis, short nLevel);
	short get_positive_level(short nAxis, short* nLevel);
	short set_negative_limit(short nAxis, short nAction);
	short get_negative_limit(short nAxis, short* nAction);
	short set_negative_level(short nAxis, short nLevel);
	short get_negative_level(short nAxis, short* nLevel);
	short set_home(short nAxis, short nAction);
	short get_home(short nAxis, short* pnAction);
	short set_home_level(short nAxis, short nLevel);
	short get_home_level(short nAxis, short* nLevel);
	short set_index_required(short nAxis, short nIndex);
	short get_index_required(short nAxis, short* nIndex);
	short controller_run(short nAxis);
	short controller_idle(short nAxis);
	short set_amp_fault(short nAxis, short nAction);
	short get_amp_fault(short nAxis, short* pnAction);
	short amp_fault_set(short nAxis);
	short amp_fault_reset(short nAxis);
	short set_amp_enable(short nAxis, short nAction);
	short get_amp_enable(short nAxis, short* pnState);
	short set_in_position(short nAxis, double dInpos);
	short get_in_position(short nAxis, double* pdInpos);
	short set_collision_prevent(short max, short sax, short add_sub, short non_equal, double pos);
	short set_collision_prevent_flag(short bd_num, short mode);
	short get_collision_prevent_flag(short bd_num, short* pmode);
	short home_switch(short nAxis);
	short pos_switch(short nAxis);
	short neg_switch(short nAxis);
	short set_io(short nBoard, long lValue);
	short get_io(short nBoard, long* plValue);
	short get_out_io(short nBoard, long* plValue);
	short set_bit(short nBit);
	short reset_bit(short nBit);

	static CAxSimMmcWfp* GetSimMmcWfp();

private:
	CAxSimAxis*	m_pSimAxis;
	CAxTrace	m_Trace;
	long*		m_pOutput;
	long*		m_pInput;

	static CAxSimMmcWfp* theSimMmcWfp;
};

#endif