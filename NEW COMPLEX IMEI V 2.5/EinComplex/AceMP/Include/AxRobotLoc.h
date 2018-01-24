#ifndef __AX_ROBOTLOC_H__
#define __AX_ROBOTLOC_H__

#pragma once

#include "AxLoc.h"

class __declspec(dllexport) CAxRobotLoc : public CAxLoc 
{
public:
	double	m_dSpeed;
	double	m_dAccel;
	double	m_dAppro;
	double	m_dDepart;
	int		m_nDwell;
	int		m_nSettle;
	CString	sName;

	CAxRobotLoc();
	CAxRobotLoc(const CAxLoc& loc);
	CAxRobotLoc(double x, double y, double z);
	CAxRobotLoc(double x, double y, double z, double rz);
	virtual ~CAxRobotLoc();

	void		TransArray(CArray<CAxRobotLoc, CAxRobotLoc&>& loc);
	void		Tool(CAxRobotLoc& loc);
	BOOL		InRegion(const CAxRobotLoc& loc1, const CAxRobotLoc& loc2);
	CAxRobotLoc	NearestLoc(const CAxRobotLoc& loc1, const CAxRobotLoc& loc2);

	CAxRobotLoc		operator = (CAxLoc& loc);
	CAxRobotLoc*	operator = (CAxLoc* pLoc);
};

typedef CArray<CAxRobotLoc, CAxRobotLoc&>		CAxRobotLocArray;
typedef CTypedPtrArray<CPtrArray, CAxRobotLoc*>	CAxRobotLocPtrArray;

#endif