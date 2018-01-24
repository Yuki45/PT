#ifndef __AX_AXIS_H__
#define __AX_AXIS_H__

#pragma once

#include "AxProfile.h"
#include "AxMmcCmd.h"
#include "AxTrace.h"

typedef struct ALoc {
	double	pos;
	double	speed;
	double	jerk;
	int		accel;
	int		decel;
	int		dwell;
	int		settle;
} AxisLoc;

enum AXIS_ERR {
	axisNoError					= 0,

	axisEventNone				= 1,
	axisEventHomeSwitch			= 2,
	axisEventPosLimitSw			= 3,
	axisEventNegLimitSw			= 4,
	axisEventAmpFault			= 5,
	axisEventAccelLimit			= 6,
	axisEventVelLimit			= 7,
	axisEventNegLimit			= 8,
	axisEventPosLimit			= 9,
	axisEventErrorLimit			= 10,
	axisEventPcCommand			= 11,
	axisEventOutOfFrame			= 12,
	axisEventAmpPowerOnOff		= 13,
	axisEventAbsCommError		= 14,
	axisEventInpositionSatus	= 15,
	axisEventRunStopCommand		= 16,
	axisEventCollisionState		= 17,

	axisHomeOpFail				= 31,	// NOT USE
	axisAmpEnableFail			= 32,	// NOT USE
	axisAmpDisableFail			= 33,	// NOT USE
	axisNotAmpEnabled			= 34,	// IsAmpEnabled()
	axisNotAmpDisabled			= 35,	// IsAmpDisabled()
	axisAxisDoneTimeout			= 36,	// NOT USE
	axisMotionDoneTimeout		= 37,	// NOT USE
	axisAxisDoneInposFail		= 38,	// IsAxisDone()
	axisMotionDoneInposFail		= 39,	// IsMotionDone()
	axisNotAxisDone				= 40,	// IsAxisDone(), WaitAxisDone(), WaitAxisDoneCheckStopState()
	axisNotMotionDone			= 41,	// IsMotionDone(), WaitMotionDone()
	axisNotAxisReady			= 42,	// WaitAxisReady()
	axisClearStatusFail			= 43,	// NOT USE
	axisStopState				= 44,	// IsAxisReady(), WaitAxisDoneCheckStopState()

	axisMmcError				= 101,
	axisFuncError				= 201,

	axisLastError
};

class __declspec(dllexport) CAxAxis : public CAxObject
{
	enum {
		minSpeed	= 5,
		timePolling	= 5
	};

public:
	UINT	m_nCtr;
	UINT	m_nAxis;
	UINT	m_nAxisError;
	UINT	m_nHomeSeq;
	double	m_dSpeed;
	int		m_nAccel;
	int		m_nDecel;
	AxisLoc	m_CurLoc;

	CAxAxis();
	virtual ~CAxAxis();

	void	SetMoveSpeed(double dSpeed);
	void	SetMoveAccel(int nAccel);
	void	SetMoveDecel(int nDecel);
	void	SetMinSpeed(double nMinSpeed);
	void	SetMaxSpeed(double nMaxSpeed);
	void	SetMaxAccel(int nMaxAccel);
	void	SetMaxDecel(int nMaxDecel);
	void	SetFastHomeSpeed(double nFastHomeSpeed);
	void	SetJogSpeed(double nJogSpeed);
	BOOL	SetPosition(double dPos);
	double	GetMinSpeed();
	double	GetMaxSpeed();
	int		GetMaxAccel();
	int		GetMaxDecel();
	double	GetFastHomeSpeed();
	double	GetJogSpeed();
	BOOL	GetCurLoc(double* pdCurLoc);
	int		GetAxisState();
	int		GetAxisSource();
	double	GetScale();
	BOOL	GetPosition(double *pdPos);
	BOOL	IsAmpEnabled();
	BOOL	IsAmpDisabled();
	BOOL	IsHomeCheck();
	BOOL	IsAxisDone();
	BOOL	IsMotionDone();
	BOOL	IsAxisReady();
	BOOL	IsRotateType();
	double	CheckRotateLoc(double dLoc);
	BOOL	WaitAxisDone(int nTm);
	BOOL	Init(int nAxis, LPCTSTR pszFile);
	void	SaveProfile();
	BOOL	EnableAmp();
	BOOL	DisableAmp();
	BOOL	AlarmClear();
	BOOL	ClearStatus(int nTimeWait=1000, int nTimeClear=1000);
	BOOL	Home();
	BOOL	StartMove(AxisLoc loc, BOOL bSlow);
	BOOL	StartTMove(AxisLoc loc, BOOL bSlow);
	BOOL	Move(AxisLoc loc, BOOL bSlow);
	BOOL	TMove(AxisLoc loc, BOOL bSlow);
	BOOL	VMove(double dSpeed, int dAccel);
	BOOL	VStop();
	BOOL	Jog();
	BOOL	Jog(double dSpeed, int dAccel);
	BOOL	JogPos();
	BOOL	JogPos(double dSpeed,int nAccel);
	BOOL	JogNeg();
	BOOL	JogNeg(double dSpeed,int nAccel);
	BOOL	JogStop();
	BOOL	Stop();
	BOOL	Abort();
	BOOL	Resume();
	BOOL	PositionIOOnOff(UINT nPosNum, int nBitNo, double dPos, int nEncFlag);
	BOOL	PositionIOClear(UINT nPosNum);
	BOOL	PositionIOAllClear();

private:
	CAxMmcCmd*	m_pCmd;
	CAxProfile	m_profile;
	BOOL		m_bHome;
	int			m_nHomeDir;
	UINT		m_nEncoderChk;		// NOT USE
	UINT		m_nHomeConfig;		// NOT USE
	UINT		m_nAutoAmpOn;		// NOT USE
	double		m_dMinSpeed;
	int			m_nMinAccel;
	int			m_nMinDecel;
	double		m_dMaxSpeed;
	int			m_nMaxAccel;
	int			m_nMaxDecel;
	double		m_dJogSpeed;
	int			m_nJogAccel;
	double		m_dStopDecel;		// NOT USE
	double		m_dEStopDecel;		// NOT USE
	double		m_dHomePos;			// NOT USE
	double		m_dParkPos;
	double		m_dHomeToIndex;
	double		m_dCntPerIndex;		// NOT USE
	double		m_dFastHomeSpeed;
	double		m_dSlowHomeSpeed;
	double		m_dPosPositionLim;	// NOT USE
	double		m_dNegPositionLim;	// NOT USE
	double		m_dPosErrLim;		// NOT USE
	double		m_dInPosLim;		// NOT USE
	double		m_dMaxDist;
	double		m_dScale;
	double		m_dCoarseTol;
	double		m_dFineTol;
	double		m_dInPosTol;
	BOOL		m_bIsRotate;
	BOOL		m_bIsBreakType;
	CAxOutput	m_opBreak;
	BOOL		m_bSimulate;
	CAxTrace	m_Trace;

	void	SetAxisError(UINT nErr);
	BOOL	SetParam();
	BOOL	SetCoarseTol();
	BOOL	SetFineTol();
	BOOL	SetInPosTol(double dTol);
	void	GetProfile();
	BOOL	IsAxisStopState();
	BOOL	CheckMasterTerminate();
	BOOL	CheckReady();
	BOOL	WaitAxisReady(UINT nTm);
	BOOL	WaitAxisDoneCheckStopState(int nTm);
	BOOL	WaitMotionDone(int nTm);
	void	HandleException(UINT nExp);
	BOOL	CoarseHome();
	BOOL	FineHome();
	BOOL	SearchHome(int nDir);
	BOOL	SearchFineHome(int nDir);
	double	WorldToCnt(double dVal);
	AxisLoc	WorldToCnt(AxisLoc loc);
};

typedef CArray<CAxAxis, CAxAxis&>			CAxAxisArray;
typedef CTypedPtrArray<CPtrArray, CAxAxis*>	CAxAxisPtrArray;

#endif