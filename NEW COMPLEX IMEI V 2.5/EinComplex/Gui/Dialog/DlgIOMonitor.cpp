#include "StdAfx.h"
#include "EinComplex.h"
#include "DlgIOMonitor.h"

#include <BtnEnh.h>
#include "MainFrm.h"
#include "CommWorld.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

void CDlgIOMonitor::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CDlgIOMonitor, CDialog)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CDlgIOMonitor, CDialog)
	ON_EVENT(CDlgIOMonitor, IDC_BTN_INPUT_PREV,		DISPID_CLICK, OnClickBtnInputPrev,	VTS_NONE)
	ON_EVENT(CDlgIOMonitor, IDC_BTN_INPUT_NEXT,		DISPID_CLICK, OnClickBtnInputNext,	VTS_NONE)
	ON_EVENT(CDlgIOMonitor, IDC_BTN_OUTPUT_PREV,	DISPID_CLICK, OnClickBtnOutputPrev,	VTS_NONE)
	ON_EVENT(CDlgIOMonitor, IDC_BTN_OUTPUT_NEXT,	DISPID_CLICK, OnClickBtnOutputNext,	VTS_NONE)
	ON_EVENT(CDlgIOMonitor, IDC_BTN_EXIT,			DISPID_CLICK, OnClickBtnExit,		VTS_NONE)

	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT00, IDC_BTN_OUTPUT00, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT01, IDC_BTN_OUTPUT01, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT02, IDC_BTN_OUTPUT02, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT03, IDC_BTN_OUTPUT03, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT04, IDC_BTN_OUTPUT04, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT05, IDC_BTN_OUTPUT05, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT06, IDC_BTN_OUTPUT06, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT07, IDC_BTN_OUTPUT07, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT08, IDC_BTN_OUTPUT08, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT09, IDC_BTN_OUTPUT09, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT10, IDC_BTN_OUTPUT10, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT11, IDC_BTN_OUTPUT11, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT12, IDC_BTN_OUTPUT12, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT13, IDC_BTN_OUTPUT13, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT14, IDC_BTN_OUTPUT14, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
	ON_EVENT_RANGE(CDlgIOMonitor, IDC_BTN_OUTPUT15, IDC_BTN_OUTPUT15, DISPID_CLICK, OnClickBtnOutput, VTS_I4)
END_EVENTSINK_MAP()

CDlgIOMonitor::CDlgIOMonitor(CWnd* pParent) : CDialog(CDlgIOMonitor::IDD, pParent)
{
}

BOOL CDlgIOMonitor::Create(CWnd* pParentWnd) 
{
	return CDialog::Create(IDD, pParentWnd);
}

void CDlgIOMonitor::PostNcDestroy() 
{
	CDialog::PostNcDestroy();
}

BOOL CDlgIOMonitor::PreTranslateMessage(MSG* pMsg)
{
	if( (pMsg->message == WM_KEYDOWN) ||
		(pMsg->message == WM_KEYUP) )
	{
		if( (pMsg->wParam == VK_ESCAPE) ||
			(pMsg->wParam == VK_RETURN) ||
			(pMsg->wParam == VK_SPACE) ) return 0;
	}

	return CDialog::PreTranslateMessage(pMsg);
}

BOOL CDlgIOMonitor::OnInitDialog() 
{
	CDialog::OnInitDialog();

	m_pAxIOMgr = CAxIOMgr::GetIOMgr();
	m_pController = CMCPController::GetController();
#ifdef DEF_EIN_48_LCA
	m_iniIOList.m_sIniFile.Format(_T("%s\\%s\\IOList48.ini"), m_pController->m_sIOListPath, _T("Kor"));
#else
	m_iniIOList.m_sIniFile.Format(_T("%s\\%s\\IOList56.ini"), m_pController->m_sIOListPath, _T("Kor"));
#endif

	InitResInfo();

	m_nInputPage = 0;
	m_nOutputPage = 0;

	m_nMaxInputPage  = m_iniIOList.ReadInt(_T("General"), _T("Inputs"), 0);
	m_nMaxOutputPage = m_iniIOList.ReadInt(_T("General"), _T("Outputs"), 0);

	UpdateInput();
	UpdateOutput();

	SetTimer(0, 50, NULL);

	return TRUE;
}

void CDlgIOMonitor::OnClose() 
{
	KillTimer(0);

	DestroyWindow();
}

void CDlgIOMonitor::OnTimer(UINT nIDEvent) 
{
	if( nIDEvent == 0 )
	{
		KillTimer(0);

		UpdateStatus();

		SetTimer(0, 50, NULL);
	}

	CDialog::OnTimer(nIDEvent);
}

BOOL CDlgIOMonitor::OnEraseBkgnd(CDC* pDC) 
{
	CDialog::OnEraseBkgnd(pDC);

	return TRUE;
}

void CDlgIOMonitor::OnClickBtnInputPrev()
{
	if( (m_nInputPage <= 0) || (m_nInputPage >= m_nMaxInputPage) ) m_nInputPage = m_nMaxInputPage - 1;
	else m_nInputPage--;

	UpdateInput();
}

void CDlgIOMonitor::OnClickBtnInputNext()
{
	if( (m_nInputPage >= (m_nMaxInputPage - 1)) || (m_nInputPage < 0) ) m_nInputPage = 0;
	else m_nInputPage++;

	UpdateInput();
}

void CDlgIOMonitor::OnClickBtnOutputPrev()
{
	if( (m_nOutputPage <= 0) || (m_nOutputPage >= m_nMaxOutputPage) ) m_nOutputPage = m_nMaxOutputPage - 1;
	else m_nOutputPage--;

	UpdateOutput();
}

void CDlgIOMonitor::OnClickBtnOutputNext()
{
	if( (m_nOutputPage >= (m_nMaxOutputPage - 1)) || (m_nOutputPage < 0) ) m_nOutputPage = 0;
	else m_nOutputPage++;

	UpdateOutput();
}

void CDlgIOMonitor::OnClickBtnExit()
{
	OnCancel();
}

void CDlgIOMonitor::OnClickBtnOutput(UINT nID)
{
	if( CAxMaster::GetMaster()->GetState() == MS_AUTO ) return;

	int nIndex = 0;
	DWORD dwOutput = 0;

	for( int i = 0; i < DEF_MAX_IO_LINE; i++ )
	{
		if( m_nResOutputID[i] == nID )
		{
			nIndex = i;
			break;
		}
	}

	if( m_nOutputPage < 8 )
	{
		if( m_nOutputPage < 4 )
		{
			if( FAS_GetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, &dwOutput) != FMM_OK ) return;

			if( nIndex == 0 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT0);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT0, 0);
			}
			else if( nIndex == 1 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT1);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT1, 0);
			}
			else if( nIndex == 2 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT2);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT2, 0);
			}
			else if( nIndex == 3 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT3);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT3, 0);
			}
			else if( nIndex == 4 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT4);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT4, 0);
			}
			else if( nIndex == 5 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT5);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT5, 0);
			}
			else if( nIndex == 6 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT6);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT6, 0);
			}
			else if( nIndex == 7 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT7);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT7, 0);
			}
			else if( nIndex == 8 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, 0, SERVO_OUT_BITMASK_USEROUT8);
				else FAS_SetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, SERVO_OUT_BITMASK_USEROUT8, 0);
			}
		}
		else
		{
			if( FAS_GetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, &dwOutput) != FMM_OK ) return;

			if( nIndex == 0 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT0 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT0);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT0, 0);
			}
			else if( nIndex == 1 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT1 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT1);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT1, 0);
			}
			else if( nIndex == 2 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT2 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT2);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT2, 0);
			}
			else if( nIndex == 3 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT3 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT3);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT3, 0);
			}
			else if( nIndex == 4 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT4 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT4);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT4, 0);
			}
			else if( nIndex == 5 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT5 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT5);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT5, 0);
			}
			else if( nIndex == 6 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT6 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT6);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT6, 0);
			}
			else if( nIndex == 7 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT7 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT7);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT7, 0);
			}
			else if( nIndex == 8 )
			{
				if( dwOutput & SERVO_OUT_BITMASK_USEROUT8 ) FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, 0, SERVO_OUT_BITMASK_USEROUT8);
				else FAS_SetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, SERVO_OUT_BITMASK_USEROUT8, 0);
			}
		}
	}
	else
	{
		CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
		CCommWorld* pWorld = NULL;

		if( m_nOutputPage == 8 ) pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		else if( m_nOutputPage == 9 ) pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		else return;

		pWorld->m_bOutput[nIndex] = !pWorld->m_bOutput[nIndex];
	}
}

void CDlgIOMonitor::UpdateStatus()
{
	ULONGLONG dwInput = 0;
	DWORD dwOutput = 0;
	int i = 0;
	BOOL bInputState[16] = {FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE,
							FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE};
	BOOL bOutputState[16] = {FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE,
							 FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE, FALSE};

	if( m_nInputPage < 8 )
	{
		if( m_nInputPage < 4 )
		{
			if( FAS_GetIOInput(DEF_FAS_RIGHT, m_nInputPage, &dwInput) != FMM_OK ) dwInput = 0;
		}
		else
		{
			if( FAS_GetIOInput(DEF_FAS_LEFT, m_nInputPage - 4, &dwInput) != FMM_OK ) dwInput = 0;
		}

		if( dwInput & SERVO_IN_BITMASK_USERIN0 ) bInputState[0] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN1 ) bInputState[1] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN2 ) bInputState[2] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN3 ) bInputState[3] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN4 ) bInputState[4] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN5 ) bInputState[5] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN6 ) bInputState[6] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN7 ) bInputState[7] = TRUE;
		if( dwInput & SERVO_IN_BITMASK_USERIN8 ) bInputState[8] = TRUE;
	}

	if( m_nOutputPage < 8 )
	{
		if( m_nOutputPage < 4 )
		{
			if( FAS_GetIOOutput(DEF_FAS_RIGHT, m_nOutputPage, &dwOutput) != FMM_OK ) dwOutput = 0;
		}
		else
		{
			if( FAS_GetIOOutput(DEF_FAS_LEFT, m_nOutputPage - 4, &dwOutput) != FMM_OK ) dwOutput = 0;
		}

		if( dwOutput & SERVO_OUT_BITMASK_USEROUT0 ) bOutputState[0] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT1 ) bOutputState[1] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT2 ) bOutputState[2] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT3 ) bOutputState[3] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT4 ) bOutputState[4] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT5 ) bOutputState[5] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT6 ) bOutputState[6] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT7 ) bOutputState[7] = TRUE;
		if( dwOutput & SERVO_OUT_BITMASK_USEROUT8 ) bOutputState[8] = TRUE;
	}
	else
	{
		CMainFrame* pMainFrm = (CMainFrame*)AfxGetApp()->GetMainWnd();
		CCommWorld* pWorld = NULL;

		if( m_nOutputPage == 8 ) pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));
		else if( m_nOutputPage == 9 ) pWorld = (CCommWorld*)pMainFrm->m_pSerialHub->GetSerial(_T("World_Left"));
		else return;

		for( i = 0; i < DEF_MAX_IO_LINE; i++ ) bOutputState[i] = pWorld->m_bOutput[i];
	}

	for( i = 0; i < DEF_MAX_IO_LINE; i++ )
	{
		((CBtnEnh*)GetDlgItem(m_nResInputID[i]))->SetBackColorInterior(bInputState[i] ? COLOR_YELLOW : COLOR_WHITE);
		((CBtnEnh*)GetDlgItem(m_nResOutputID[i]))->SetBackColorInterior(bOutputState[i] ? COLOR_YELLOW : COLOR_WHITE);
	}
}

void CDlgIOMonitor::UpdateInput()
{
	CString sKey = _T("");
	CString sNumber = _T("");
	CString sComment = _T("");

	for( int i = 0; i < DEF_MAX_IO_LINE; i++ )
	{
		if( (m_nInputPage < 0) || (m_nInputPage > (m_nMaxInputPage - 1)) )
		{
			sNumber.Format(_T("X----"));
			sComment = _T("Not Defined");
		}
		else
		{
			sKey.Format(_T("DX%03X"), (m_nInputPage * DEF_MAX_IO_LINE) + i);
			
			if( m_nInputPage < 4 )
			{
				if( i < 9 )
				{
					sNumber.Format(_T("X%04d"), (m_nInputPage * 10) + i);
					sComment = m_iniIOList.ReadStr(_T("Input"), sKey, _T(""));
				}
				else
				{
					sNumber.Format(_T("X----"));
					sComment = _T("Not Defined");
				}
			}
			else if( m_nInputPage < 8 )
			{
				if( i < 9 )
				{
					sNumber.Format(_T("X%04d"), ((m_nInputPage - 4) * 10) + i + 100);
					sComment = m_iniIOList.ReadStr(_T("Input"), sKey, _T(""));
				}
				else
				{
					sNumber.Format(_T("X----"));
					sComment = _T("Not Defined");
				}
			}
			else if( m_nInputPage < 9 )
			{
				sNumber.Format(_T("X%04d"), i + 1000);
				sComment = m_iniIOList.ReadStr(_T("Input"), sKey, _T(""));
			}
			else if( m_nInputPage < 10 )
			{
				sNumber.Format(_T("X%04d"), i + 1100);
				sComment = m_iniIOList.ReadStr(_T("Input"), sKey, _T(""));
			}
			else
			{
				sNumber.Format(_T("X----"));
				sComment = _T("Not Defined");
			}
		}
		
		((CBtnEnh*)GetDlgItem(m_nResInputID[i]))->SetCaption(sNumber);
		((CBtnEnh*)GetDlgItem(m_nResInputNameID[i]))->SetCaption(sComment);
	}

	sComment.Format(_T("%d / %d"), (m_nInputPage + 1), m_nMaxInputPage);
	((CBtnEnh*)GetDlgItem(IDC_STT_INPUT_PAGE))->SetCaption(sComment);
}

void CDlgIOMonitor::UpdateOutput()
{
	CString sKey = _T("");
	CString sNumber = _T("");
	CString sComment = _T("");

	for( int i = 0; i < DEF_MAX_IO_LINE; i++ )
	{
		if( (m_nOutputPage < 0) || (m_nOutputPage > (m_nMaxOutputPage - 1)) )
		{
			sNumber.Format(_T("OT----"));
			sComment = _T("Not Defined");
		}
		else
		{
			sKey.Format(_T("DY%03X"), (m_nOutputPage * DEF_MAX_IO_LINE) + i);

			if( m_nOutputPage < 4 )
			{
				if( i < 9 )
				{
					sNumber.Format(_T("Y%04d"), (m_nOutputPage * 10) + i);
					sComment = m_iniIOList.ReadStr(_T("Output"), sKey, _T(""));
				}
				else
				{
					sNumber.Format(_T("Y----"));
					sComment = _T("Not Defined");
				}
			}
			else if( m_nOutputPage < 8 )
			{
				if( i < 9 )
				{
					sNumber.Format(_T("Y%04d"), ((m_nOutputPage - 4) * 10) + i + 100);
					sComment = m_iniIOList.ReadStr(_T("Output"), sKey, _T(""));
				}
				else
				{
					sNumber.Format(_T("Y----"));
					sComment = _T("Not Defined");
				}
			}
			else if( m_nOutputPage < 9 )
			{
				sNumber.Format(_T("Y%04d"), i + 1000);
				sComment = m_iniIOList.ReadStr(_T("Output"), sKey, _T(""));
			}
			else if( m_nOutputPage < 10 )
			{
				sNumber.Format(_T("Y%04d"), i + 1100);
				sComment = m_iniIOList.ReadStr(_T("Output"), sKey, _T(""));
			}
			else
			{
				sNumber.Format(_T("Y----"));
				sComment = _T("Not Defined");
			}
		}

		((CBtnEnh*)GetDlgItem(m_nResOutputID[i]))->SetCaption(sNumber);
		((CBtnEnh*)GetDlgItem(m_nResOutputNameID[i]))->SetCaption(sComment);
	}

	sComment.Format(_T("%d / %d"), (m_nOutputPage + 1), m_nMaxOutputPage);
	((CBtnEnh*)GetDlgItem(IDC_STT_OUTPUT_PAGE))->SetCaption(sComment);
}

void CDlgIOMonitor::InitResInfo()
{
	m_nResInputID[0] = IDC_STT_INPUT00;
	m_nResInputID[1] = IDC_STT_INPUT01;
	m_nResInputID[2] = IDC_STT_INPUT02;
	m_nResInputID[3] = IDC_STT_INPUT03;
	m_nResInputID[4] = IDC_STT_INPUT04;
	m_nResInputID[5] = IDC_STT_INPUT05;
	m_nResInputID[6] = IDC_STT_INPUT06;
	m_nResInputID[7] = IDC_STT_INPUT07;
	m_nResInputID[8] = IDC_STT_INPUT08;
	m_nResInputID[9] = IDC_STT_INPUT09;
	m_nResInputID[10] = IDC_STT_INPUT10;
	m_nResInputID[11] = IDC_STT_INPUT11;
	m_nResInputID[12] = IDC_STT_INPUT12;
	m_nResInputID[13] = IDC_STT_INPUT13;
	m_nResInputID[14] = IDC_STT_INPUT14;
	m_nResInputID[15] = IDC_STT_INPUT15;

	m_nResInputNameID[0] = IDC_STT_NAME_INPUT00;
	m_nResInputNameID[1] = IDC_STT_NAME_INPUT01;
	m_nResInputNameID[2] = IDC_STT_NAME_INPUT02;
	m_nResInputNameID[3] = IDC_STT_NAME_INPUT03;
	m_nResInputNameID[4] = IDC_STT_NAME_INPUT04;
	m_nResInputNameID[5] = IDC_STT_NAME_INPUT05;
	m_nResInputNameID[6] = IDC_STT_NAME_INPUT06;
	m_nResInputNameID[7] = IDC_STT_NAME_INPUT07;
	m_nResInputNameID[8] = IDC_STT_NAME_INPUT08;
	m_nResInputNameID[9] = IDC_STT_NAME_INPUT09;
	m_nResInputNameID[10] = IDC_STT_NAME_INPUT10;
	m_nResInputNameID[11] = IDC_STT_NAME_INPUT11;
	m_nResInputNameID[12] = IDC_STT_NAME_INPUT12;
	m_nResInputNameID[13] = IDC_STT_NAME_INPUT13;
	m_nResInputNameID[14] = IDC_STT_NAME_INPUT14;
	m_nResInputNameID[15] = IDC_STT_NAME_INPUT15;

	m_nResOutputID[0] = IDC_BTN_OUTPUT00;
	m_nResOutputID[1] = IDC_BTN_OUTPUT01;
	m_nResOutputID[2] = IDC_BTN_OUTPUT02;
	m_nResOutputID[3] = IDC_BTN_OUTPUT03;
	m_nResOutputID[4] = IDC_BTN_OUTPUT04;
	m_nResOutputID[5] = IDC_BTN_OUTPUT05;
	m_nResOutputID[6] = IDC_BTN_OUTPUT06;
	m_nResOutputID[7] = IDC_BTN_OUTPUT07;
	m_nResOutputID[8] = IDC_BTN_OUTPUT08;
	m_nResOutputID[9] = IDC_BTN_OUTPUT09;
	m_nResOutputID[10] = IDC_BTN_OUTPUT10;
	m_nResOutputID[11] = IDC_BTN_OUTPUT11;
	m_nResOutputID[12] = IDC_BTN_OUTPUT12;
	m_nResOutputID[13] = IDC_BTN_OUTPUT13;
	m_nResOutputID[14] = IDC_BTN_OUTPUT14;
	m_nResOutputID[15] = IDC_BTN_OUTPUT15;

	m_nResOutputNameID[0] = IDC_STT_NAME_OUTPUT00;
	m_nResOutputNameID[1] = IDC_STT_NAME_OUTPUT01;
	m_nResOutputNameID[2] = IDC_STT_NAME_OUTPUT02;
	m_nResOutputNameID[3] = IDC_STT_NAME_OUTPUT03;
	m_nResOutputNameID[4] = IDC_STT_NAME_OUTPUT04;
	m_nResOutputNameID[5] = IDC_STT_NAME_OUTPUT05;
	m_nResOutputNameID[6] = IDC_STT_NAME_OUTPUT06;
	m_nResOutputNameID[7] = IDC_STT_NAME_OUTPUT07;
	m_nResOutputNameID[8] = IDC_STT_NAME_OUTPUT08;
	m_nResOutputNameID[9] = IDC_STT_NAME_OUTPUT09;
	m_nResOutputNameID[10] = IDC_STT_NAME_OUTPUT10;
	m_nResOutputNameID[11] = IDC_STT_NAME_OUTPUT11;
	m_nResOutputNameID[12] = IDC_STT_NAME_OUTPUT12;
	m_nResOutputNameID[13] = IDC_STT_NAME_OUTPUT13;
	m_nResOutputNameID[14] = IDC_STT_NAME_OUTPUT14;
	m_nResOutputNameID[15] = IDC_STT_NAME_OUTPUT15;
}
