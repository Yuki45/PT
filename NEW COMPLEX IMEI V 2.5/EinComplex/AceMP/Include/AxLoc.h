#ifndef __AX_LOC_H__
#define __AX_LOC_H__

#pragma once

#include "AxObject.h"

#define ToRad(ang) (ang * 3.1415926535 / 180)

class __declspec(dllexport) CAxLoc : public CAxObject 
{
public:
	double x;
	double y;
	double z;
	double rx;
	double ry;
	double rz;

	CAxLoc();
	CAxLoc(double x, double y, double z);
	CAxLoc(double x, double y, double z, double rz);
	virtual ~CAxLoc();

	double*	GetLoc();
	void	GetLoc(double p[]);
	void	SetLoc(const double p[]);
	void	GetLoc(double& x, double& y, double& z);
	void	SetLoc(double x, double y, double z);
	void	GetLoc(double& x, double& y, double& z, double& rz);
	void	SetLoc(double x, double y, double z, double rz);
	void	SetLoc(const CAxLoc& loc);
	void	SetLoc(const CAxLoc* pLoc);
	void	Inverse();
	void	Shift(const CAxLoc& loc);
	void	Shift(double x, double y, double z);
	double	Distance(const CAxLoc& loc);
	double	DistanceXY(const CAxLoc& loc);
	CAxLoc	GetTrans(const CAxLoc& loc);
	CAxLoc	GetTrans(const CAxLoc* pLoc);
	void	Trans(CAxLoc& loc);
	void	Trans(CAxLoc* pLoc);
	void	Trans(CArray<CAxLoc, CAxLoc&>& loc);

	void	operator +=	(const CAxLoc& loc);
	void	operator -=	(const CAxLoc& loc);
	CAxLoc	operator +	(const CAxLoc& loc);
	CAxLoc	operator -	(const CAxLoc& loc);

private:
	void Transform(CAxLoc& loc);
};

typedef CArray<CAxLoc, CAxLoc&> CAxLocArray;

#endif