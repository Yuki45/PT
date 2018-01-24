#include "stdafx.h"
#include "AxErrMsg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define BUF_LEN 256
#define IsSpace(a) ( (a == _T(' ')) || (a == _T('\n')) || (a == _T('\t')) || (a == _T('\a')) )

void CAxErrData::Reset()
{
	m_nNumber = 0;
	m_nType = 0;
	m_sDesc = _T("");
	m_sHelpFile = _T("");
	for( int i = 0; i < 4; i++ ) {
		m_sParams[i] = _T("");
		m_dParams[i] = 0;
	}
}

const CAxErrData& CAxErrData::operator = (const CAxErrData& src)
{
	m_nNumber = src.m_nNumber;
	m_nType = src.m_nType;
	m_sDesc = src.m_sDesc;
	m_sHelpFile = src.m_sHelpFile;
	m_sSource = src.m_sSource;
	for( int i = 0; i < 4; i++ ) {
		m_sParams[i] = src.m_sParams[i];
		m_dParams[i] = src.m_dParams[i];
	}

	return *this;
}

CAxErrMsg::CAxErrMsg()
{
	m_filename = _T("\\Data\\ErrMsg\\ErrMsg.txt");
}

BOOL CAxErrMsg::Init(TCHAR* filename)
{
	m_filename = filename;

	return TRUE;
}

void CAxErrMsg::ResetMsg()
{
	Reset();
}

void CAxErrMsg::ReadMsg(TCHAR* item, CString type, CString jamModule)
{
//	m_msgType = type;
	m_sSource = jamModule;

	FILE* fp;
	int nFileRet = _tfopen_s(&fp, m_filename, _T("r"));

	if( !fp ) {
		m_sDesc = _T("Jam Message Fail, Not Available");
		m_sHelpFile = _T("Default.htm");
		return;
	}

	TCHAR buf[BUF_LEN];

	while( _fgetts(buf, BUF_LEN, fp) ) {
		TCHAR* str = _tcsstr(buf, item);

		if( str ) {
			str += _tcslen(item);

			while( *str && IsSpace(*str) ) str++;

			if( *str && (*str == _T('=')) ) {
				str++;

				// remove leading space
				while( *str && IsSpace(*str) ) str++;

				if( *str ) {
					// remove trailing space
					TCHAR* s = str + _tcslen(str) - 1;

					while( (s != str) && IsSpace(*s) ) {
						*s = _T('\0');
						s--;
					}
					
					while( (s != str) && (*s != _T('@')) ) s--;

					if( *s && (*s == _T('@')) ) {
						s++;
						m_sHelpFile = s;
						s--;
						*s = _T('\0');
					}

					if( s != str ) m_sDesc = str;

					fclose(fp);
					return;
				}
			}
		}
	}

	fclose(fp);
}