// AxLoc.cpp: implementation of the CAxLoc class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "AxLoc.h"
#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CAxLoc::CAxLoc()
{
	x  = 0.0;
	y  = 0.0;
	z  = 0.0;
	rz = 0.0;
	ry = 0.0;
	rz = 0.0;
}

CAxLoc::CAxLoc(double x, double y, double z, double rz)
{
	this->x  = x;
	this->y  = y;
	this->z  = z;
	this->rx = 0.0;
	this->ry = 0.0;
	this->rz = rz;
}

CAxLoc::~CAxLoc()
{

}

void CAxLoc::operator+=(const CAxLoc& loc)
{
	x  += loc.x;
	y  += loc.y;
	z  += loc.z;
	rx += loc.rx;
	ry += loc.ry;
	rz += loc.rz;
}

void CAxLoc::operator-=(const CAxLoc& loc)
{
	x  -= loc.x;
	y  -= loc.y;
	z  -= loc.z;
	rx -= loc.rx;
	ry -= loc.ry;
	rz -= loc.rz;
}

CAxLoc CAxLoc::operator+(const CAxLoc& loc)
{
	CAxLoc tLoc;
	tLoc.x  = x  + loc.x;
	tLoc.y  = y  + loc.y;
	tLoc.z  = z  + loc.z;
	tLoc.rx = rx + loc.rx;
	tLoc.ry = ry + loc.ry;
	tLoc.rz = rz + loc.rz;
	return tLoc;
}


CAxLoc CAxLoc::operator-(const CAxLoc& loc)
{
	CAxLoc tLoc;
	tLoc.x  = x  - loc.x;
	tLoc.y  = y  - loc.y;
	tLoc.z  = z  - loc.z;
	tLoc.rx = rx - loc.rx;
	tLoc.ry = ry - loc.ry;
	tLoc.rz = rz - loc.rz;
	return tLoc;
}

void CAxLoc::GetLoc(double p[])
{
	p[0] = x;
	p[1] = y;
	p[2] = z;
	p[4] = rx;
	p[5] = ry;
	p[6] = rz;
}


void CAxLoc::SetLoc(const double p[])
{
	x  = p[0];
	y  = p[1];
	z  = p[2];
	rx = p[3];
	ry = p[4];
	rz = p[5];
}

void CAxLoc::GetLoc(double& x, double& y, double& z, double& rz)
{
	x  = this->x;
	y  = this->y;
	z  = this->z;
	rz = this->rz;
}

double* CAxLoc::GetLoc()
{
	double* p = new double[6];

	p[0] = x;
	p[1] = y;
	p[2] = z;
	p[3] = rx;
	p[4] = ry;
	p[5] = rz;
	return p;
}


void CAxLoc::SetLoc(double x, double y, double z, double rz)
{
    this->x  = x;
    this->y  = y;
    this->z  = z;
    this->rz = rz;
}



void CAxLoc::SetLoc(const CAxLoc& loc)
{
    x  = loc.x;
    y  = loc.y;
    z  = loc.z;
    rx = loc.rx;
    ry = loc.ry;
    rz = loc.rz;
}


void CAxLoc::SetLoc(const CAxLoc* pLoc)
{
    x  = pLoc->x;
    y  = pLoc->y;
    z  = pLoc->z;
    rx = pLoc->rx;
    ry = pLoc->ry;
    rz = pLoc->rz;
}

CAxLoc CAxLoc::GetTrans(const CAxLoc& loc)
{
	CAxLoc tLoc = loc;
	Transform(tLoc);
	return tLoc;
}


CAxLoc CAxLoc::GetTrans(const CAxLoc* pLoc)
{
	CAxLoc tLoc;
	tLoc.SetLoc(pLoc);
	Transform(tLoc);
	return tLoc;
}

void CAxLoc::Trans(CAxLoc& loc)
{
	Transform(loc);
}

void CAxLoc::Trans(CAxLoc* pLoc)
{
	Transform(*pLoc);
}

void CAxLoc::Trans(CAxLocArray& loc)
{
	for(int i=0; i<loc.GetSize(); i++) {
		Transform(loc[i]);
	}
}

void CAxLoc::Inverse()
{
	x = -x;
	y = -y;
	z = -z;
}

void CAxLoc::Shift(const CAxLoc& loc)
{
	x  += loc.x;
	y  += loc.y;
	z  += loc.z;
	rx += loc.rx;
	ry += loc.ry;
	rz += loc.rz;
}

void CAxLoc::Shift(double x, double y, double z)
{
	this->x += x;
	this->y += y;
	this->z += z;
}

double CAxLoc::Distance(const CAxLoc& loc)
{
	double tx = loc.x - x;
	double ty = loc.y - y;
	double tz = loc.z - z;
	return sqrt((tx*tx) + (ty*ty) + (tz*tz));
}

double CAxLoc::DistanceXY(const CAxLoc& loc)
{
	double tx = loc.x - x;
	double ty = loc.y - y;
	return sqrt((tx*tx) + (ty*ty));
}

void CAxLoc::Transform(CAxLoc& loc)
{
	double sinx, siny, sinz;
	double cosx, cosy, cosz;
	double tm[4][4], vv[4];
	double vp[4] = {0.0,0.0,0.0,0.0};

	sinx = sin(ToRad(rx));
	siny = sin(ToRad(ry));
	sinz = sin(ToRad(rz));
	cosx = cos(ToRad(rx));
	cosy = cos(ToRad(ry));
	cosz = cos(ToRad(rz));

	// Set up homogenours transformaton matrix - [Tr] = [Rx].[Ry].[Rz].[T]
	tm[0][0] =	cosy * cosz;
	tm[0][1] = -cosy * sinz;
	tm[0][2] =  siny;
	tm[0][3] =  x;
	tm[1][0] =  cosx * sinz + sinx * siny * cosz;
	tm[1][1] =  cosx * cosz - sinx * siny * sinz;
	tm[1][2] = -sinx * cosy;
	tm[1][3] =  y;
	tm[2][0] =  sinx * sinz - cosx * siny * cosz;
	tm[2][1] =  sinx * cosz + cosx * siny * sinz;
	tm[2][2] =  cosx * cosy;
	tm[2][3] =  z;
	tm[3][0] =  0.0;
	tm[3][1] =  0.0;
	tm[3][2] =  0.0;
	tm[3][3] =  1.0;

	// Assign vector to be transformed
	vv[0] = loc.x;
	vv[1] = loc.y;
	vv[2] = loc.z;
	vv[3] = 1.0;

	// Compute resulting vector [Vp] = [Tr].[Vv]
	for(short r=0; r<4; r++) {
		for(short c=0; c<4; c++) {
			vp[r] += tm[r][c] * vv[c];
		}
	}

	loc.x = vp[0];
	loc.y = vp[1];
	loc.z = vp[2];
	loc.rx += rx;
	loc.ry += ry;
	loc.rz += rz;
}
