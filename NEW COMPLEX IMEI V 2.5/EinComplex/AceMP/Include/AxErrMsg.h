#ifndef __AX_ERRMSG_H__
#define __AX_ERRMSG_H__

#pragma once

enum ErrorResponse {
	emNone		= 0x00,
	emOk		= 0x01,
	emCancel	= 0x02,
	emAbort		= 0x04,
	emRetry		= 0x08,
	emIgnore	= 0x10,
	emYes		= 0x20,
	emNo		= 0x40
};

enum ErrorType {
	emInfo		= 0x100,
	emWarning	= 0x200,
	emError		= 0x400,
	emFatal		= 0x800
};

class __declspec(dllexport) CAxErrData
{
public:
	int		m_nNumber;
	UINT	m_nType;
	CString m_sSource;
	CString m_sDesc;
	CString	m_sHelpFile;
	CString m_sParams[4];
	double	m_dParams[4];

	CAxErrData() { m_sSource = _T(""); Reset(); }

	const CAxErrData& operator = (const CAxErrData& src); // Copy operator

	void Reset();
};

class __declspec(dllexport) CAxErrMsg : public CAxErrData 
{
public:
	CAxErrMsg();

	BOOL Init(TCHAR* filename);
	void ReadMsg(TCHAR* item, CString type, CString jamModule);
	void ResetMsg();

private:
	CString m_filename;
};

#endif