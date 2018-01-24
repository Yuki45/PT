#ifndef __AX_MMCCMD_H__
#define __AX_MMCCMD_H__

#pragma once

#include "AxSimMmcWfp.h"

class __declspec(dllexport) CAxMmcCmd  
{
public:
	CAxSimMmcWfp* m_pSimMmcWfp;

	~CAxMmcCmd();
	
	void MmcErr(const int nCode);

	// Initialization
	void MmcInitx(const UINT nTotalBdNum, long* lpAddr);

	// Error Handling

	// Single Axis-Point to Point Motion
	void StartMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc);
	void StartTMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc, const int dDec);
	void StartRMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc);
	void Move(const UINT nAxis, const double dPos, const double dVel, const int dAcc);
	void TMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc, const int dDec);
	void RMove(const UINT nAxis, const double dPos, const double dVel, const int dAcc);
	void WaitForDone(const UINT nAxis);

	// Multiple Axis-Point to Point Motion
	void StartMoveAll(const UINT nNumAxis, UINT* pnAxisMap, double* pdPos, double* pdVel, int* pnAcc);
	
	// S-Curve Profile Motion

	// Velocity Move
	void VMove(const UINT nAxis, const double dVel, const int dAcc);

	// Velocity Move Stop
	void VStop(const UINT nAxis);

	// Coordinated Motion Configuration 
	void MapAxes(const UINT nNumAxis, UINT* pnAxisMap);
	void SetMoveSpeed(const double dSpeed);
	void SetMoveAccel(const int nAccel);

	// Coordinated Profile Motion 
	void Move2(const double d1, const double d2);
	void Move3(const double d1, const double d2, const double d3);
	void Move4(const double d1, const double d2, const double d3, const double d4);

	// Position Control
	void SetPosition(const UINT nAxis, const double dPos);
	void GetPosition(const UINT nAxis, double* pdPos);
	void SetCommand(const UINT nAxis, const double dPos);
	void GetCommand(const UINT nAxis, double* pdPos);

	// Motion Status
	BOOL InSequence(const UINT nAxis);
	BOOL InMotion(const UINT nAxis);
	BOOL InPosition(const UINT nAxis);
	BOOL MotionDone(const UINT nAxis);
	BOOL AxisDone(const UINT nAxis);

	// Controller Status
	UINT AxisState(const UINT nAxis);
	UINT AxisSource(const UINT nAxis);

	// Stop Event
	void SetStop(const UINT nAxis);
	void SetStopRate(const UINT nAxis, const int dDec);

	// Emergence Stop Event
	
	// Abort Event
	void ControllerIdle(const UINT nAxis);
	void DisableAmp(const UINT nAxis);

	// Event Recovery
	void ClearStatus(const UINT nAxis);
	void ControllerRun(const UINT nAxis);
	void EnableAmp(const UINT nAxis);

	// Motion Control
	void MmcDwell(const UINT nAxis, const long lTm);

	// Frames Clear
	void FramesClear(const UINT nAxis);

	// Home Inputs
	void SetHome(const UINT nAxis, const UINT sAction);
	void SetHomeLevel(const UINT nAxis, const BOOL bLevel);
	void SetHomeIndexRequired(const UINT nAxis, const BOOL bIndex);

	// Limit Inputs
	BOOL GetHomeSwitch(const UINT nAxis);
	BOOL GetPosSwitch(const UINT nAxis);
	BOOL GetNegSwitch(const UINT nAxis);

	// Amp Fault Inputs
	void GetAmpFault(const UINT nAxis, UINT* pnAction);
	void AmpFaultReset(const UINT nAxis);
	void AmpFaultSet(const UINT nAxis);

	// Amp Enable Outputs
	void GetAmpEnable(const UINT nAxis, UINT* pnState);

	// Software Limits
	void SetInPosition(const UINT nAxis, const double dTol);

	// Interrupt Configuration
	void SetCollisionPreventFlag(const UINT bd_num, const UINT mode);
	void GetCollisionPreventFlag(const UINT bd_num, UINT* pmode);
	void SetCollisionPrevent(const UINT max, const UINT sax, const UINT add_sub, const UINT non_equal, const double pos);
	void SplLineMove2Ax(const UINT nAxis1, const UINT nAxis2, double* pdPos, const double dVel, const int nAcc);
	void SplLineMove3Ax(const UINT nAxis1, const UINT nAxis2, const UINT nAxis3, double* pdPos, const double dVel, const int nAcc);
	void PositionIOOnOff(UINT nPosNum, int nBitNo, UINT nAxis, double dPos, int nEncFlag);
	void PositionIOClear(UINT nAxis, UINT nPosNum);
	void PositionIOAllClear(UINT nAxis);
	void PosCmpInit(int nIdxSel, UINT nAxis1, UINT nAxis2);
	void PosCmpEnable(UINT nBdNum, int nFlag);
	void PosCmpReset(UINT n1); // not coding
	void PosCmpIndexClear(UINT nBdNum, int nIdxSel);
	void PosCmpInterval(int nDir, UINT nAxis, int nBitNo, double dStartPos, double dLimitPos, long lInterval, long lTime);
	void PosCmpRead(int nIdxSel, UINT nAxis, double *pdPos);
	void PosCmpBit(UINT n1, UINT n2, UINT n3); // not coding
	void PosCmp(UINT nPosNum, int nBitNo, UINT nAxis, int nLatch, double dPos, long lTime);
	void SetIO(UINT nBdNum, long lValue);
	void GetIO(UINT nBdNum, long* plValue);
	void GetOutIO(UINT nBdNum, long* plValue);
	void GetInputBit(UINT nBdNum, int nBitNo, BOOL* pbValue);
	void GetOutputBit(UINT nBdNum, int nBitNo, BOOL* pbValue);
	void SetBit(UINT nBdNum, int nBitNo);
	void ResetBit(UINT nBdNum, int nBitNo);
	void SetScanTime(int nBNum, int Samplingrate); // Set Sampling Time

	static CAxMmcCmd* GetMmcCmd();

protected:
	CAxMmcCmd();
	
private:
	CMutex	m_mutex;
	HANDLE	m_hMutex;
	BOOL	m_bMmcMgrSimulate;

	static CAxMmcCmd* theMmcCmd;
};

#endif