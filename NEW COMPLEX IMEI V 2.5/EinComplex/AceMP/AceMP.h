#if !defined(__ACEMP_H__)
#define __ACEMP_H__

#if _MSC_VER > 1000
#pragma once
#endif

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"

class CAceMPApp : public CWinApp
{
public:
	CAceMPApp();

	//{{AFX_VIRTUAL(CAceMPApp)
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CAceMPApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}

#endif