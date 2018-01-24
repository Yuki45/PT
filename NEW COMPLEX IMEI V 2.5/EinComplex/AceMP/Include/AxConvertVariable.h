#ifndef __AX_CONVERTVARIABLE_H__
#define __AX_CONVERTVARIABLE_H__

#pragma once

//-----------------------------------------------------------------------------
// Convert BYTE
//-----------------------------------------------------------------------------
CString __declspec(dllexport) GetStrFromByte(int nLen, BYTE* pbyData);
short   __declspec(dllexport) GetShortFromByte(int nLen, BYTE* pbyData);
int     __declspec(dllexport) GetIntFromByte(int nLen, BYTE* pbyData);
void	__declspec(dllexport) SetStrToByte(BYTE* pbyDst, CString sSrc, int nLen);
void    __declspec(dllexport) SetShortToByte(BYTE* pbyDst, short nData, int nLen);
void    __declspec(dllexport) SetIntToByte(BYTE* pbyDst, int nData, int nLen);

#endif