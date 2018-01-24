#ifndef __AX_IOCTRL_H__
#define __AX_IOCTRL_H__

#pragma once

#include "AxInput.h"
#include "AxOutput.h"
#include "AxErrorCtrl.h"

class __declspec(dllexport) CAxIOCtrl  
{
public:
	CAxIOCtrl(CAxErrorCtrl* pCtrl);
	virtual ~CAxIOCtrl();

	void TurnOn(CAxOutput* pOp);
	void TurnOn(CAxOutput& op);
	void TurnOn(CAxOutput* pOp, CAxInput* pIp, BOOL bValue, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOn(CAxOutput& op, CAxInput& ip, BOOL bValue, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOn(CAxOutput* pOp, CAxInput* pOnIp, CAxInput* pOffIp, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOn(CAxOutput& op, CAxInput& onIp, CAxInput& offIp, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnAllOn(CAxOutput* pFirstOp, ...);
	void TurnOff(CAxOutput* pOp);
	void TurnOff(CAxOutput& op);
	void TurnOff(CAxOutput* pOp, CAxInput* pIp, BOOL bValue, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOff(CAxOutput& op, CAxInput& ip, BOOL bValue, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOff(CAxOutput* pOp, CAxInput* pOnIp, CAxInput* pOffIp, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnOff(CAxOutput& op, CAxInput& onIp, CAxInput& offIp, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode);
	void TurnAllOff(CAxOutput* pFirstOp, ...);
	void Turn(CAxOutput* pOp, BOOL bValue);
	void Turn(CAxOutput& op, BOOL bValue);
	void Turns(CAxOutput* pFirstOp, BOOL bFirstValue, ...);
	int  WaitInputOn(CAxInput* pIp);
	int  WaitInputOn(CAxInput& ip);
	int  WaitInputOn(DWORD dwTimeout, CAxInput* pIp);
	int  WaitInputOn(DWORD dwTimeout, CAxInput& ip);
	void WaitInputOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pIp);
	void WaitInputOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput& ip);
	int  WaitInputOnNS(DWORD dwTimeout, CAxInput* pIp);
	int  WaitInputsOn(DWORD dwTimeout, CAxInput* pFirstIp, ...);
	int  WaitInputsOn(DWORD dwTimeout, CAxEventPtrArray& event);
	void WaitInputsOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pFirstIp, ...);
	void WaitInputsOn(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
	int  WaitInputOff(CAxInput* pIp);
	int  WaitInputOff(CAxInput& ip);
	int  WaitInputOff(DWORD dwTimeout, CAxInput* pIp);
	int  WaitInputOff(DWORD dwTimeout, CAxInput& ip);
	void WaitInputOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pIp);
	void WaitInputOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput& ip);
	int  WaitInputOffNS(DWORD dwTimeout, CAxInput* pIp);
	int  WaitInputsOff(DWORD dwTimeout, CAxInput* pFirstIp, ...);
	int  WaitInputsOff(DWORD dwTimeout, CAxEventPtrArray& event);
	void WaitInputsOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pFirstIp, ...);
	void WaitInputsOff(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
	int  WaitInputs(CAxInput* pFirstIp, BOOL bFirstValue, ...);
	int  WaitInputs(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...);
	int  WaitInputs(DWORD dwTimeout, CAxEventPtrArray& event);
	void WaitInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pFirstIp, BOOL bFirstValue, ...);
	void WaitInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
	int  WaitInputsNS(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...);
	int  WaitAllInputs(DWORD dwTimeout, CAxInput* pFirstIp, BOOL bFirstValue, ...);
	int  WaitAllInputs(DWORD dwTimeout, CAxEventPtrArray& event);
	void WaitAllInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxInput* pFirstIp, BOOL bFirstValue, ...);
	void WaitAllInputs(DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);

private:
	CAxErrorCtrl*	m_pErrCtrl;
	CAxControl*		m_pControl;

	int WaitTimeout(BOOL bWaitAll, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
	int WaitAllTimeout(BOOL bAddStop, DWORD dwTimeout, UINT nResp, LPCTSTR pszPath, LPCTSTR pszSource, int nCode, CAxEventPtrArray& event);
};

#endif