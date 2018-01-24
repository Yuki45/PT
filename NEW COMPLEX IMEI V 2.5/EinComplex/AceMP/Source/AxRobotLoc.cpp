// AxRobotLoc.cpp: implementation of the CAxRobotLoc class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxRobotLoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxRobotLoc::CAxRobotLoc()
{
	x  = 0;
	y  = 0;
	z  = 0;
	rx = 0;
	ry = 0;
	rz = 0;
	
	m_dSpeed  = 0.0;
	m_dAccel  = 0.0;
	m_dAppro  = 0.0;
	m_dDepart = 0.0;
	m_nSettle = 0;
	m_nDwell  = 0;
}

CAxRobotLoc::CAxRobotLoc(const CAxLoc& loc)
{
	x  = loc.x;
	y  = loc.y;
	z  = loc.z;
	rx = loc.rx;
	ry = loc.ry;
	rz = loc.rz;
	
	m_dSpeed  = 0.0;
	m_dAccel =  0.0;
	m_dAppro =  0.0;
	m_dDepart = 0.0;
	m_nSettle = 0;
	m_nDwell =  0;
}

CAxRobotLoc::CAxRobotLoc(double x, double y, double z)
{
	this->x  = x;
	this->y  = y;
	this->z  = z;
	this->rx = 0;
	this->ry = 0;
	this->rz = 0;

	m_dSpeed =  0.0;
	m_dAccel =  0.0;
	m_dAppro =  0.0;
	m_dDepart = 0.0;
	m_nSettle = 0;
	m_nDwell =  0;
}

CAxRobotLoc::CAxRobotLoc(double x, double y, double z, double rz)
{
	this->x =  x;
	this->y =  y;
	this->z =  z;
	this->rx = 0;
	this->ry = 0;
	this->rz = rz;

	m_dSpeed =  0.0;
	m_dAccel =  0.0;
	m_dAppro =  0.0;
	m_dDepart = 0.0;
	m_nSettle = 0;
	m_nDwell =  0;
}

CAxRobotLoc::~CAxRobotLoc()
{

}

CAxRobotLoc CAxRobotLoc::operator= (CAxLoc& loc)
{
	x = loc.x;
	y = loc.y;
	z = loc.z;
	rx = loc.rx;
	ry = loc.ry;
	rz = loc.rz;

	return *this;
}

CAxRobotLoc* CAxRobotLoc::operator= (CAxLoc* pLoc)
{
	return (CAxRobotLoc*)pLoc;
}

void CAxRobotLoc::TransArray(CAxRobotLocArray& loc)
{
	for(int i=0; i<loc.GetSize(); i++) {
		Trans(loc[i]);
	}
}

void CAxRobotLoc::Tool(CAxRobotLoc& loc)
{
	Trans(loc);
}

BOOL CAxRobotLoc::InRegion(const CAxRobotLoc& loc1, const CAxRobotLoc& loc2)
{
	if ((x < min(loc1.x, loc2.x)) ||
	    (y < min(loc1.y, loc2.y)) ||
	    (z < min(loc1.z, loc2.z))) return FALSE;

	if ((x > max(loc1.x, loc2.x)) ||
		(y > max(loc1.y, loc2.y)) ||
		(z > max(loc1.z, loc2.z))) return FALSE;

    return TRUE;
}

CAxRobotLoc CAxRobotLoc::NearestLoc(const CAxRobotLoc& loc1, const CAxRobotLoc& loc2)
{
    CAxRobotLoc tLoc;
	double m, c, pc;

    m = (loc2.y - loc1.y) / (loc2.x - loc1.x);
    c = (loc2.y - (loc2.x * m));
    pc = (y - (x / m));

    tLoc.x = (pc - c) / ((1 / m) - m);
    tLoc.y = (tLoc.x * m) + c;
    tLoc.z = (loc2.z + loc1.z) / 2;

    return tLoc;
}
