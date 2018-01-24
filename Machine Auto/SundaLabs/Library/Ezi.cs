using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SundaLabs.Library
{
    class IO
    {
        [DllImport("Inpout32.dll")]
        public static extern short Inp32(int address);
        [DllImport("inpout32.dll", EntryPoint = "Out32")]
        public static extern void Output(int adress, int value); // decimalh

        //FAPI BOOL WINAPI	FAS_Connect(BYTE nPortNo, DWORD dwBaud); 
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_Connect(byte nPortNo, uint dwBaud);    // BOOL 
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern void FAS_Close(byte nPortNo);

        
        //FAPI int WINAPI	FAS_GetCommandPos(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos);
        [DllImport("EziMOTIONPlusR.dll")]
         public static extern int FAS_GetCommandPos(byte nPortNo, byte iSlaveNo,out ulong lCmdPos);
        //FAPI int WINAPI	FAS_GetActualPos(BYTE nPortNo, BYTE iSlaveNo, long* lActPos);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_GetActualPos(byte nPortNo, byte iSlaveNo, out ulong lActPos);

        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_GetAxisStatus(byte nPortNo, byte iSlaveNo, out uint dwAxisStatus);

        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_SetCommandPos(byte nPortNo, byte iSlaveNo, uint dwAxisStatus);

        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_SetActualPos(byte nPortNo, byte iSlaveNo, uint dwAxisStatus);

        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_ClearPosition(byte nPortNo, byte iSlaveNo, uint dwAxisStatus);

        //FAPI int WINAPI	FAS_MoveSingleAxisAbsPos(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MoveSingleAxisAbsPos(byte nPortNo, byte iSlaveNo, long lAbsPos, uint lVelocity);

        //FAPI int WINAPI	FAS_MoveSingleAxisIncPos(BYTE nPortNo, BYTE iSlaveNo, long lIncPos, DWORD lVelocity);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MoveSingleAxisIncPos(byte nPortNo, byte iSlaveNo, long lAbsPos, uint lVelocity);


        #region Table POS .....
        //FAPI int WINAPI	FAS_PosTableReadItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);
        //FAPI int WINAPI	FAS_PosTableWriteItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);

        //FAPI int WINAPI	FAS_PosTableWriteROM(BYTE nPortNo, BYTE iSlaveNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableWriteROM(byte nPortNo, byte iSlaveNo);
        //FAPI int WINAPI	FAS_PosTableReadROM(BYTE nPortNo, BYTE iSlaveNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableReadROM(byte nPortNo, byte iSlaveNo);

        //FAPI int WINAPI	FAS_PosTableReadOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long* lPosItemVal);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableReadOneItem(byte nPortNo, byte iSlaveNo, uint wItemNo, uint wOffset, out long lPosItemVal);

        //FAPI int WINAPI	FAS_PosTableWriteOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long lPosItemVal);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableWriteOneItem(byte nPortNo, byte iSlaveNo, uint wItemNo, uint wOffset, long lPosItemVal);

        //FAPI int WINAPI	FAS_PosTableSingleRunItem(BYTE nPortNo, BYTE iSlaveNo, BOOL bNextMove, WORD wItemNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableSingleRunItem(byte nPortNo, byte iSlaveNo, bool bNextMove, uint wItemNo);

        //FAPI int WINAPI	FAS_PosTableRunItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_PosTableRunItem(byte nPortNo, byte iSlaveNo, uint wItemNo);
        #endregion 

        //FAPI int WINAPI	FAS_MoveVelocity(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iVelDir);  JOG+  JOG-
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MoveVelocity(byte nPortNo, byte iSlaveNo, uint lVelocity, int iVelDir);
        //Servo On
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_ServoEnable(byte nPortNo, byte iSlaveNo, bool bOnOff);

        //FAPI int WINAPI	FAS_ServoAlarmReset(BYTE nPortNo, BYTE iSlaveNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_ServoAlarmReset(byte nPortNo, byte iSlaveNo);

        //FAPI int WINAPI	FAS_MoveOriginSingleAxis(BYTE nPortNo, BYTE iSlaveNo);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MoveOriginSingleAxis(byte nPortNo, byte iSlaveNo);

        //FAPI int WINAPI	FAS_StepAlarmReset(BYTE nPortNo, BYTE iSlaveNo, BOOL bReset);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_StepAlarmReset(byte nPortNo, byte iSlaveNo, bool bOnOff);

        //FAS_MoveStop(BYTE nPortNo, BYTE iSlaveNo);  Stop
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MoveStop(byte nPortNo, byte iSlaveNo);

        //FAS_MoveStop(BYTE nPortNo, BYTE iSlaveNo);  Pause
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_MovePause(byte nPortNo, byte iSlaveNo, bool bpause);

        //FAPI int WINAPI	FAS_SetIOInput(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG uIOSETMask, ULONGLONG uIOCLRMask);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_SetIOInput(byte nPortNo, byte iSlaveNo, UInt64 uIOSETMask, UInt64 uIOCLRMask);

        //FAPI int WINAPI	FAS_GetIOInput(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG* uIOInput);
        [DllImport("EziMOTIONPlusR.dll")]
        public static extern int FAS_GetIOInput(byte nPortNo, byte iSlaveNo, out uint uIOInput);

/*
FAPI BOOL WINAPI	FAS_IsSlaveExist(BYTE nPortNo, BYTE iSlaveNo);

FAPI int WINAPI	FAS_GetSlaveInfo(BYTE nPortNo, BYTE iSlaveNo, DRIVE_INFO* lpDriveInfo);

FAPI int WINAPI	FAS_SaveAllParameters(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_SetParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long lParamValue);
FAPI int WINAPI	FAS_GetParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lParamValue);
FAPI int WINAPI	FAS_GetROMParameter(BYTE nPortNo, BYTE iSlaveNo, BYTE iParamNo, long* lRomParam);

//------------------------------------------------------------------------------
//					IO Functions
//------------------------------------------------------------------------------
FAPI int WINAPI	FAS_SetIOInput(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG uIOSETMask, ULONGLONG uIOCLRMask);
FAPI int WINAPI	FAS_GetIOInput(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG* uIOInput);

FAPI int WINAPI	FAS_SetIOOutput(BYTE nPortNo, BYTE iSlaveNo, DWORD dwIOSETMask, DWORD dwIOCLRMask);
FAPI int WINAPI	FAS_GetIOOutput(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwIOOutput);

FAPI int WINAPI	FAS_GetInputAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iInPinNo, BYTE* nInLogicNo, BYTE* bLevel);
FAPI int WINAPI	FAS_GetOutputAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iOutPinNo, BYTE* nOutLogicNo, BYTE* bLevel);
FAPI int WINAPI	FAS_SetInputAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iInPinNo, BYTE nInLogicNo, BYTE bLevel);
FAPI int WINAPI	FAS_SetOutputAssignMap(BYTE nPortNo, BYTE iSlaveNo, BYTE iOutPinNo, BYTE nOutLogicNo, BYTE bLevel);

FAPI int WINAPI	FAS_IOAssignMapReadROM(BYTE nPortNo, BYTE iSlaveNo);

//------------------------------------------------------------------------------
//					Servo Driver Control Functions
//------------------------------------------------------------------------------		
FAPI int WINAPI	FAS_ServoEnable(BYTE nPortNo, BYTE iSlaveNo, BOOL bOnOff);
FAPI int WINAPI	FAS_ServoAlarmReset(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_StepAlarmReset(BYTE nPortNo, BYTE iSlaveNo, BOOL bReset);

//------------------------------------------------------------------------------
//					Read Status and Position
//------------------------------------------------------------------------------
FAPI int WINAPI	FAS_GetAxisStatus(BYTE nPortNo, BYTE iSlaveNo, DWORD* dwAxisStatus);
FAPI int WINAPI	FAS_GetIOStatus(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG* uInStatus, DWORD* dwOutStatus);
FAPI int WINAPI	FAS_GetMotionStatus(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos, long* lActPos, long* lPosErr, long* lActVel, DWORD* dwAxisStatus);
FAPI int WINAPI	FAS_GetAllStatus(BYTE nPortNo, BYTE iSlaveNo, ULONGLONG* uInStatus, DWORD* dwOutStatus, DWORD* dwAxisStatus, long* lCmdPos, long* lActPos, long* lPosErr, long* lActVel, WORD* wPosItemNo);

FAPI int WINAPI	FAS_SetCommandPos(BYTE nPortNo, BYTE iSlaveNo, long lCmdPos);
FAPI int WINAPI	FAS_SetActualPos(BYTE nPortNo, BYTE iSlaveNo, long lActPos);
FAPI int WINAPI	FAS_ClearPosition(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_GetCommandPos(BYTE nPortNo, BYTE iSlaveNo, long* lCmdPos);
FAPI int WINAPI	FAS_GetActualPos(BYTE nPortNo, BYTE iSlaveNo, long* lActPos);
FAPI int WINAPI	FAS_GetPosError(BYTE nPortNo, BYTE iSlaveNo, long* lPosErr);
FAPI int WINAPI	FAS_GetActualVel(BYTE nPortNo, BYTE iSlaveNo, long* lActVel);

FAPI int WINAPI	FAS_GetAlarmType(BYTE nPortNo, BYTE iSlaveNo, BYTE* nAlarmType);
FAPI int WINAPI	FAS_GetRunPTStatus(BYTE nPortNo, BYTE iSlaveNo, WORD* wPosItemNo);

//------------------------------------------------------------------
//					Motion Functions.
//------------------------------------------------------------------
FAPI int WINAPI	FAS_MoveStop(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_EmergencyStop(BYTE nPortNo, BYTE iSlaveNo);

FAPI int WINAPI	FAS_MovePause(BYTE nPortNo, BYTE iSlaveNo, BOOL bPause);

FAPI int WINAPI	FAS_MoveOriginSingleAxis(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_MoveSingleAxisAbsPos(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity);
FAPI int WINAPI	FAS_MoveSingleAxisIncPos(BYTE nPortNo, BYTE iSlaveNo, long lIncPos, DWORD lVelocity);
FAPI int WINAPI	FAS_MoveToLimit(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iLimitDir);
FAPI int WINAPI	FAS_MoveVelocity(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iVelDir);

FAPI int WINAPI	FAS_PositionAbsOverride(BYTE nPortNo, BYTE iSlaveNo, long lOverridePos);
FAPI int WINAPI	FAS_PositionIncOverride(BYTE nPortNo, BYTE iSlaveNo, long lOverridePos);
FAPI int WINAPI	FAS_VelocityOverride(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity);

FAPI int WINAPI	FAS_MoveLinearAbsPos(BYTE nPortNo, BYTE nNoOfSlaves, BYTE* iSlavesNo, long* lAbsPos, DWORD lFeedrate, WORD wAccelTime);
FAPI int WINAPI	FAS_MoveLinearIncPos(BYTE nPortNo, BYTE nNoOfSlaves, BYTE* iSlavesNo, long* lIncPos, DWORD lFeedrate, WORD wAccelTime);

//------------------------------------------------------------------
//					Ex-Motion Functions.
//------------------------------------------------------------------
FAPI int WINAPI	FAS_MoveSingleAxisAbsPosEx(BYTE nPortNo, BYTE iSlaveNo, long lAbsPos, DWORD lVelocity, MOTION_OPTION_EX* lpExOption);
FAPI int WINAPI	FAS_MoveSingleAxisIncPosEx(BYTE nPortNo, BYTE iSlaveNo, long lIncPos, DWORD lVelocity, MOTION_OPTION_EX* lpExOption);
FAPI int WINAPI	FAS_MoveVelocityEx(BYTE nPortNo, BYTE iSlaveNo, DWORD lVelocity, int iVelDir, VELOCITY_OPTION_EX* lpExOption);

//------------------------------------------------------------------
//					All-Motion Functions.
//------------------------------------------------------------------
FAPI int WINAPI	FAS_AllMoveStop(BYTE nPortNo);
FAPI int WINAPI	FAS_AllEmergencyStop(BYTE nPortNo);
FAPI int WINAPI	FAS_AllMoveOriginSingleAxis(BYTE nPortNo);
FAPI int WINAPI	FAS_AllMoveSingleAxisAbsPos(BYTE nPortNo, long lAbsPos, DWORD lVelocity);
FAPI int WINAPI	FAS_AllMoveSingleAxisIncPos(BYTE nPortNo, long lIncPos, DWORD lVelocity);

//------------------------------------------------------------------
//					Position Table Functions.
//------------------------------------------------------------------
FAPI int WINAPI	FAS_PosTableReadItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);
FAPI int WINAPI	FAS_PosTableWriteItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, LPITEM_NODE lpItem);
FAPI int WINAPI	FAS_PosTableWriteROM(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_PosTableReadROM(BYTE nPortNo, BYTE iSlaveNo);
FAPI int WINAPI	FAS_PosTableRunItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo);

FAPI int WINAPI	FAS_PosTableReadOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long* lPosItemVal);
FAPI int WINAPI	FAS_PosTableWriteOneItem(BYTE nPortNo, BYTE iSlaveNo, WORD wItemNo, WORD wOffset, long lPosItemVal);
FAPI int WINAPI	FAS_PosTableSingleRunItem(BYTE nPortNo, BYTE iSlaveNo, BOOL bNextMove, WORD wItemNo);
 * */

        public static String left(String input, int len)
        {
            return input.Substring(0, len);
        }

        public static String right(String input, int len)
        {
            return input.Substring(input.Length - len);
        }

        public static String mid(String input, int index, int len)
        {
            return input.Substring(index - 1, index + len - 1);
        }

        public static String mid(String input, int index)
        {
            return input.Substring(index - 1);
        }
    }
}
