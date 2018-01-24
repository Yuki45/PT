// AxRobot.cpp: implementation of the CAxRobot class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxRobot.h"
#include "MMCWHP154.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxRobot::CAxRobot()
{
	m_nNumAxis = 0;
	m_nRobot = 0;
//	m_nAxisMap = 0;
	m_sAxisMap = _T(""); //(M) 2010.04.11 - Modified
	m_nRobotError = robotNoError;
	m_dAppro = 0.0;
	m_dDepart = 0.0;

	m_pnAxisMap = NULL;
	m_pAxisLoc = NULL;
	
	m_profile.m_sIniFile = _T("\\Data\\Profile\\Service\\MmcMgr.ini");
	m_nErrAxis = 0;
}

CAxRobot::~CAxRobot()
{
	delete [] m_pnAxisMap;
	delete [] m_pAxisLoc;
}

BOOL CAxRobot::Init(int nRobot, CAxAxis* pAxis, LPCTSTR pszFile)
{
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), _T("TraceAxRobot"));

	m_pCmd = CAxMmcCmd::GetMmcCmd();
	m_nRobot = nRobot;
	GetProfile();
	MapAxis(pAxis);
	m_pAxisLoc = new ALoc[m_nNumAxis];

	return TRUE;
}

void CAxRobot::GetProfile()
{
	m_profile.m_sSect.Format(_T("Robot%02d"), m_nRobot);
	m_nNumAxis	= m_profile.ReadInt(_T("NumAxis"));
	m_bAxisMapToggle = m_profile.ReadBool(_T("AxisMapToggle"));
//	m_nAxisMap	= m_profile.ReadInt(_T("AxisMap"));
	m_sAxisMap	= m_profile.ReadStr(_T("AxisMap")); //(M) 2010.04.11 - Modified
	m_dAppro	= m_profile.ReadDouble(_T("Appro"));
	m_dDepart	= m_profile.ReadDouble(_T("Depart"));
	m_bSimulate	= m_profile.ReadInt(_T("Simulate"));
	m_sName		= m_profile.ReadStr(_T("Name"));
}

void CAxRobot::MapAxis(CAxAxis* pAxis)
{
	//////////////////////////////////////////////////////////////////////////
	//(M) 2010.04.11 - Modified
// 	UINT nAxis = 0;
// 
// 	m_pnAxisMap = new UINT[m_nNumAxis];
// 	if( !m_bAxisMapToggle )
// 	{
// 		for( UINT i=0; i<32; i++ ) 
// 		{
// 			if( m_nAxisMap & (0x1 << i) ) 
// 			{
// 				m_pnAxisMap[nAxis] = i;
// 				m_Axis.Add(&pAxis[i]);
// 				if( ++nAxis >= m_nNumAxis ) break;
// 			}
// 		}
// 	}
// 	else
// 	{
// 		for( UINT i=31; i>=0; i-- ) 
// 		{
// 			if( m_nAxisMap & (0x1 << i) ) 
// 			{
// 				m_pnAxisMap[nAxis] = i;
// 				m_Axis.Add(&pAxis[i]);
// 				if( ++nAxis >= m_nNumAxis ) break;
// 			}
// 		}
// 	}
	//////////////////////////////////////////////////////////////////////////

	CString sAxisMap = _T("");
	sAxisMap = m_sAxisMap;

	TCHAR* pszNextToken = NULL;
	int nAxis = _ttoi(_tcstok_s(sAxisMap.GetBuffer(50), _T(","), &pszNextToken));
	m_Axis.Add(&pAxis[nAxis]);

	for( UINT i=1; i<m_nNumAxis; i++ ) 
	{
		nAxis = _ttoi(_tcstok_s(NULL, _T(","), &pszNextToken));
		m_Axis.Add(&pAxis[nAxis]);
	}
}

void CAxRobot::HandleException(UINT nExp)
{
	m_Trace.Log(_T("[%02d Robot] HandleException(%d)"), m_nRobot, nExp);

	m_nErrAxis = 0;
	m_nRobotError = nExp;
}

void CAxRobot::GetAxisError(const UINT nAxis)
{
//	CString str;

	m_nErrAxis = nAxis;
	m_nRobotError = m_Axis[nAxis]->m_nAxisError;

//	str.Format(_T("-Robot:%02d-Axis:%d"), m_nRobot, nAxis);
//	m_sErrStr[m_nErr] += m_Axis[nAxis]->m_sErrStr[m_Axis[nAxis]->m_nErr] + str;
//	m_sErrStr[m_nErr] = m_Axis[nAxis]->m_sErrStr[m_Axis[nAxis]->m_nErr] + str;
}

void CAxRobot::SetRobotError(UINT nErr)
{
	m_nRobotError = nErr;
}

UINT CAxRobot::GetErrAxis()
{
	return m_nErrAxis;
}

UINT CAxRobot::GetNumAxis()
{
	return m_nNumAxis;
}

BOOL CAxRobot::IsHomeCheck()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsHomeCheck() )
		{
			return FALSE;
		}
	}

	return TRUE;
}

BOOL CAxRobot::IsHomeCheck(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->IsHomeCheck() )
	{
		return FALSE;
	}
	
	return TRUE;
}

BOOL CAxRobot::IsAmpEnabled()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsAmpEnabled() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsAmpEnabled(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->IsAmpEnabled() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsAmpDisabled()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsAmpDisabled() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsAmpDisabled(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->IsAmpDisabled() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsMotionDone()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsMotionDone() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsAxisDone()
{
	if( m_bSimulate ) return TRUE;
	
	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsAxisDone() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsRobotReady()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->IsAxisReady() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::IsRobotReady(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->IsAxisReady() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::GetCurLoc(CAxRobotLoc* locCur)
{
	if( m_bSimulate ) return TRUE;

	//if(m_nNumAxis > 0) 
	//{
		if( !m_Axis[0]->GetCurLoc(&locCur->x) )
		{
			GetAxisError(0);
			return FALSE;
		}
	//}
	if(m_nNumAxis > 1) 
	{
		if( !m_Axis[1]->GetCurLoc(&locCur->y) )
		{
			GetAxisError(1);
			return FALSE;
		}
	}
	if(m_nNumAxis > 2) 
	{
		if( !m_Axis[2]->GetCurLoc(&locCur->z) )
		{
			GetAxisError(2);
			return FALSE;
		}
	}
	if(m_nNumAxis > 3) 
	{
		if( !m_Axis[3]->GetCurLoc(&locCur->rz) )
		{
			GetAxisError(3);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::GetCurLocXZAx(CAxRobotLoc* locCur)
{
	if( m_bSimulate ) return TRUE;

	//if(m_nNumAxis > 0) 
	//{
		if( !m_Axis[0]->GetCurLoc(&locCur->x) )
		{
			GetAxisError(0);
			return FALSE;
		}
	//}
	if(m_nNumAxis > 1) 
	{
		if( !m_Axis[1]->GetCurLoc(&locCur->z) )
		{
			GetAxisError(1);
			return FALSE;
		}
	}
	if(m_nNumAxis > 2) 
	{
		if( !m_Axis[2]->GetCurLoc(&locCur->rz) )
		{
			GetAxisError(2);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::EnableAmp()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->EnableAmp() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::EnableAmp(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->EnableAmp() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::DisableAmp()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->DisableAmp() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::DisableAmp(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->DisableAmp() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::AlarmClear()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->AlarmClear() ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::AlarmClear(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->AlarmClear() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::ClearStatus()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->ClearStatus() )
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}


BOOL CAxRobot::ClearStatus(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->ClearStatus() )
	{
		GetAxisError(nAxis);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Home()
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		for( UINT j=0; j<m_nNumAxis; j++ ) 
		{
			if( m_Axis[j]->m_nHomeSeq == i ) 
			{
				if( !m_Axis[j]->Home() ) 
				{
					GetAxisError(j);
					return FALSE;
				}
			}
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Home(UINT nAxis)
{
	if( m_bSimulate ) return TRUE;

	if( !m_Axis[nAxis]->Home() ) 
	{
		GetAxisError(nAxis);
		return FALSE;
	}
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::MoveMulti(AxisLoc* pAxisLoc, BOOL bSlow)
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->StartMove(pAxisLoc[i], bSlow) ) 
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}
	
BOOL CAxRobot::Move(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = loc.y;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = loc.z;
	if( m_nNumAxis > 3 ) m_pAxisLoc[3].pos = loc.rz;
	
	if( !MoveMulti(m_pAxisLoc) )
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Move(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 ) 
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 ) 
	{
		m_pAxisLoc[1].pos = loc.y; 
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 ) 
	{
		m_pAxisLoc[2].pos = loc.z; 
		m_pAxisLoc[2].speed = speed;
	}
	if( m_nNumAxis > 3 ) 
	{
		m_pAxisLoc[3].pos = loc.rz;
		m_pAxisLoc[3].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc, TRUE) ) 
		return FALSE;
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::MoveXZAx(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = loc.z;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = loc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::MoveXZAx(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = loc.z;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 ) 
	{
		m_pAxisLoc[2].pos = loc.rz;
		m_pAxisLoc[2].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Approach(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = loc.y;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = m_dAppro;
	if( m_nNumAxis > 3 ) m_pAxisLoc[3].pos = loc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Approach(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 ) 
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 ) 
	{
		m_pAxisLoc[1].pos = loc.y; 
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 ) 
	{
		m_pAxisLoc[2].pos = m_dAppro; 
		m_pAxisLoc[2].speed = speed;
	}
	if( m_nNumAxis > 3 ) 
	{
		m_pAxisLoc[3].pos = loc.rz;
		m_pAxisLoc[3].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc, TRUE) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::ApproachXZAx(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = m_dAppro;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = loc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::ApproachXZAx(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = m_dAppro;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 ) 
	{
		m_pAxisLoc[2].pos = loc.rz;
		m_pAxisLoc[2].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Depart()
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	CAxRobotLoc curLoc;
	if( !GetCurLoc(&curLoc) )
		return FALSE;

	m_pAxisLoc[0].pos = curLoc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = curLoc.y;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = m_dDepart;
	if( m_nNumAxis > 3 ) m_pAxisLoc[3].pos = curLoc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Depart(double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	CAxRobotLoc curLoc;
	if( !GetCurLoc(&curLoc) )
		return FALSE;
	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = curLoc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = curLoc.y;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 )
	{
		m_pAxisLoc[2].pos = m_dDepart;
		m_pAxisLoc[2].speed = speed;
	}
	if( m_nNumAxis > 3 )
	{
		m_pAxisLoc[3].pos = curLoc.rz;
		m_pAxisLoc[3].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Depart(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = loc.y;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = m_dDepart;
	if( m_nNumAxis > 3 ) m_pAxisLoc[3].pos = loc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;  
}

BOOL CAxRobot::Depart(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = loc.y;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 )
	{
		m_pAxisLoc[2].pos = m_dDepart;
		m_pAxisLoc[2].speed = speed;
	}
	if( m_nNumAxis > 3 )
	{
		m_pAxisLoc[3].pos = loc.rz;
		m_pAxisLoc[3].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc, TRUE) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::DepartXZAx()
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	CAxRobotLoc curLoc;
	if( !GetCurLocXZAx(&curLoc) )
		return FALSE;

	m_pAxisLoc[0].pos = curLoc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = m_dDepart;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = curLoc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::DepartXZAx(double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	CAxRobotLoc curLoc;
	if( !GetCurLocXZAx(&curLoc) )
		return FALSE;
	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = curLoc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = m_dDepart;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 )
	{
		m_pAxisLoc[2].pos = curLoc.rz;
		m_pAxisLoc[2].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;
	
	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::DepartXZAx(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	m_pAxisLoc[0].pos = loc.x;
	if( m_nNumAxis > 1 ) m_pAxisLoc[1].pos = m_dDepart;
	if( m_nNumAxis > 2 ) m_pAxisLoc[2].pos = loc.rz;

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;  
}

BOOL CAxRobot::DepartXZAx(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	if( !IsRobotReady() ) 
		return FALSE;

	//if( m_nNumAxis > 0 )
	//{
		m_pAxisLoc[0].pos = loc.x;
		m_pAxisLoc[0].speed = speed;
	//}
	if( m_nNumAxis > 1 )
	{
		m_pAxisLoc[1].pos = m_dDepart;
		m_pAxisLoc[1].speed = speed;
	}
	if( m_nNumAxis > 2 )
	{
		m_pAxisLoc[2].pos = loc.rz;
		m_pAxisLoc[2].speed = speed;
	}

	if( !MoveMulti(m_pAxisLoc) ) 
		return FALSE;

	m_nRobotError = robotNoError;
	return TRUE;  
}

BOOL CAxRobot::JumpXZAx(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	try
	{
		if( !IsRobotReady() ) 
			return FALSE;

		CAxRobotLoc curLoc;
		if( !GetCurLocXZAx(&curLoc) ) 
			return FALSE;

		double pdPos1[2] = {curLoc.x * m_Axis[0]->GetScale(), m_dDepart * m_Axis[1]->GetScale()};
		double pdPos2[2] = {loc.x * m_Axis[0]->GetScale(), m_dAppro * m_Axis[1]->GetScale()};
		double pdPos3[2] = {loc.x * m_Axis[0]->GetScale(), loc.z * m_Axis[1]->GetScale()};
		
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos1, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos2, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos3, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);

		m_Axis[0]->m_CurLoc.pos = loc.x;
		m_Axis[1]->m_CurLoc.pos = loc.z;
	}
	catch(UINT nExp) 
	{
		HandleException(nExp);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::JumpXZAx(const CAxRobotLoc& loc, double speed)
{
	if( m_bSimulate ) return TRUE;

	try
	{
		if( !IsRobotReady() ) 
			return FALSE;

		CAxRobotLoc curLoc;
		if( !GetCurLocXZAx(&curLoc) )
			return FALSE;

		double pdPos1[2] = {curLoc.x * m_Axis[0]->GetScale(), m_dDepart * m_Axis[1]->GetScale()};
		double pdPos2[2] = {loc.x * m_Axis[0]->GetScale(), m_dAppro * m_Axis[1]->GetScale()};
		double pdPos3[2] = {loc.x * m_Axis[0]->GetScale(), loc.z * m_Axis[1]->GetScale()};
		
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos1, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos2, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove2Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, 
			pdPos3, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);

		m_Axis[0]->m_CurLoc.pos = loc.x;
		m_Axis[1]->m_CurLoc.pos = loc.z;
	}
	catch(UINT nExp) 
	{
		HandleException(nExp);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::JumpXYZAx(const CAxRobotLoc& loc)
{
	if( m_bSimulate ) return TRUE;

	try
	{
		if( !IsRobotReady() ) 
			return FALSE;

		CAxRobotLoc curLoc;
		if( !GetCurLoc(&curLoc) )
			return FALSE;

		double pdPos1[3] = {curLoc.x * m_Axis[0]->GetScale(), curLoc.y * m_Axis[1]->GetScale(), m_dDepart * m_Axis[2]->GetScale()};
		double pdPos2[3] = {loc.x * m_Axis[0]->GetScale(), loc.y * m_Axis[1]->GetScale(), m_dAppro * m_Axis[2]->GetScale()};
		double pdPos3[3] = {loc.x * m_Axis[0]->GetScale(), loc.y * m_Axis[1]->GetScale(), loc.z * m_Axis[2]->GetScale()};
		
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos1, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos2, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos3, m_Axis[0]->m_dSpeed, m_Axis[0]->m_nAccel
		);

		m_Axis[0]->m_CurLoc.pos = loc.x;
		m_Axis[1]->m_CurLoc.pos = loc.y;
		m_Axis[2]->m_CurLoc.pos = loc.z;
	}
	catch(UINT nExp) 
	{
		HandleException(nExp);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::JumpXYZAx(const CAxRobotLoc& loc, double speed)
{
	if(m_bSimulate) return TRUE;

	try
	{
		if( !IsRobotReady() ) 
			return FALSE;

		CAxRobotLoc curLoc;
		if( !GetCurLoc(&curLoc) )
			return FALSE;

		double pdPos1[3] = {curLoc.x * m_Axis[0]->GetScale(), curLoc.y * m_Axis[1]->GetScale(), m_dDepart * m_Axis[2]->GetScale()};
		double pdPos2[3] = {loc.x * m_Axis[0]->GetScale(), loc.y * m_Axis[1]->GetScale(), m_dAppro * m_Axis[2]->GetScale()};
		double pdPos3[3] = {loc.x * m_Axis[0]->GetScale(), loc.y * m_Axis[1]->GetScale(), loc.z * m_Axis[2]->GetScale()};
		
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos1, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos2, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);
		m_pCmd->SplLineMove3Ax(
			m_Axis[0]->m_nAxis, m_Axis[1]->m_nAxis, m_Axis[2]->m_nAxis,
			pdPos3, speed*m_Axis[0]->GetScale(), m_Axis[0]->m_nAccel
		);

		m_Axis[0]->m_CurLoc.pos = loc.x;
		m_Axis[1]->m_CurLoc.pos = loc.y;
		m_Axis[2]->m_CurLoc.pos = loc.z;
	}
	catch(UINT nExp) 
	{
		HandleException(nExp);
		return FALSE;
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Stop()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->Stop() )
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Abort()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->Abort() )
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}

BOOL CAxRobot::Resume()
{
	if( m_bSimulate ) return TRUE;

	for( UINT i=0; i<m_nNumAxis; i++ ) 
	{
		if( !m_Axis[i]->Resume() )
		{
			GetAxisError(i);
			return FALSE;
		}
	}

	m_nRobotError = robotNoError;
	return TRUE;
}




