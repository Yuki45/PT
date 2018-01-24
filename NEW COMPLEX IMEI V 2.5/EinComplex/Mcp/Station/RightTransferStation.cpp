#include "StdAfx.h"
#include "RightTransferStation.h"

#include "MainControlStation.h"
#include "RightPPStation.h"
#include "RightNGCVStation.h"
#include "JigSystem.h"
#include "Resource.h"
#include "MainFrm.h"

#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CRightTransferStation::CRightTransferStation()
{
	m_sName = _T("RightTransferStation");
	m_profile.m_sIniFile.Format(_T("\\Data\\Profile\\Station\\%s.ini"), m_sName);
	m_recipe.m_sSect = m_sName;
	m_sErrPath.Format(_T("\\Station\\%s.err"), m_sName);
	m_sMsgPath.Format(_T("\\Station\\%s.msg"), m_sName);
	m_Trace.SetPathFile(CAxObject::GetRootPath() + _T("\\Data\\Trace"), m_sName);

	m_nAutoState = AS_INIT;
}

CRightTransferStation::~CRightTransferStation()
{
	DeleteThreads();
}

void CRightTransferStation::InitProfile()
{
	CAxStation::InitProfile();

	//// INPUTS /////////////////////////////////////////////////////////////////////////

	//// OUTPUTS ////////////////////////////////////////////////////////////////////////

	//// EVENTS /////////////////////////////////////////////////////////////////////////
	m_profile.AddEvent(_T("evtInitStart"), m_evtInitStart);
	m_profile.AddEvent(_T("evtInitComp"), m_evtInitComp);
	m_profile.AddEvent(_T("evtEnablePosBuff"), m_evtEnablePosBuff);
	m_profile.AddEvent(_T("evtEnableUseLoadBuff"), m_evtEnableUseLoadBuff);
	m_profile.AddEvent(_T("evtEnableUseUnloadBuff"), m_evtEnableUseUnloadBuff);
	m_profile.AddEvent(_T("evtUnloadComp"), m_evtUnloadComp);

	//// SETTINGS ///////////////////////////////////////////////////////////////////////
	m_profile.AddDouble(_T("dScaleX"), m_dScaleX);
	m_profile.AddDouble(_T("dScaleY"), m_dScaleY);
	m_profile.AddDouble(_T("dScaleZ"), m_dScaleZ);
	m_profile.AddDouble(_T("dVelocityX"), m_dVelocityX);
	m_profile.AddDouble(_T("dVelocityY"), m_dVelocityY);
	m_profile.AddDouble(_T("dVelocityZ"), m_dVelocityZ);
	m_profile.AddDouble(_T("dSlowSpdX"), m_dSlowSpdX);
	m_profile.AddDouble(_T("dSlowSpdY"), m_dSlowSpdY);
	m_profile.AddDouble(_T("dSlowSpdZ"), m_dSlowSpdZ);
	m_profile.AddDouble(_T("dSWLimitPosX"), m_dSWLimitPosX);
	m_profile.AddDouble(_T("dSWLimitPosY"), m_dSWLimitPosY);
	m_profile.AddDouble(_T("dSWLimitPosZ"), m_dSWLimitPosZ);
	m_profile.AddDouble(_T("dSWLimitNegX"), m_dSWLimitNegX);
	m_profile.AddDouble(_T("dSWLimitNegY"), m_dSWLimitNegY);
	m_profile.AddDouble(_T("dSWLimitNegZ"), m_dSWLimitNegZ);
}

void CRightTransferStation::LoadProfile()
{
	CAxStation::LoadProfile();
}

void CRightTransferStation::SaveProfile()
{
	CAxStation::SaveProfile();
}

void CRightTransferStation::SaveProfileJigSystem()
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	pJigSys4->SaveProfile();
	pJigSys5->SaveProfile();
	pJigSys6->SaveProfile();
}

void CRightTransferStation::InitRecipe()
{
	CAxStation::InitRecipe();

	m_recipe.AddRobotLoc(_T("locLoad"), m_locLoad);
	m_recipe.AddRobotLoc(_T("locUnload"), m_locUnload);
	m_recipe.AddRobotLoc(_T("locBCR"), m_locBCR);
	m_recipe.AddRobotLoc(_T("locNG"), m_locNG);

	CString sTemp = _T("");
	for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		sTemp.Format(_T("locJig_%02d"), i);
		m_recipe.AddRobotLoc(sTemp, m_locJig[i]);
	}
}

void CRightTransferStation::LoadRecipe()
{
	CAxStation::LoadRecipe();
}

void CRightTransferStation::SaveRecipe()
{
	CAxStation::SaveRecipe();
}

void CRightTransferStation::Startup()
{
	CAxStation::Startup();

	m_pMaster = CAxMaster::GetMaster();
	m_pMainStn = (CMainControlStation*)CAxStationHub::GetStationHub()->GetStation(_T("MainControlStation"));
	m_pRightPPStn = (CRightPPStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightPPStation"));
	m_pRightNGCVStn = (CRightNGCVStation*)CAxStationHub::GetStationHub()->GetStation(_T("RightNGCVStation"));
	m_pMainFrm = (CMainFrame*)m_pMaster->GetMainWnd();

	m_nTransferResult = JR_UNKNOWN;
	m_sTransferBarcode = _T("NoBCR");
	m_sTransferFailName = _T("");

	InitVariable();
}

void CRightTransferStation::Setup()
{
	Wait(100);
}

void CRightTransferStation::PostAbort()
{
	InitVariable();
}

void CRightTransferStation::PostStop(UINT nMode)
{
}

void CRightTransferStation::PreStart()
{
}

UINT CRightTransferStation::AutoRun()
{
	while( TRUE ) 
	{
		WaitRight(10);

		switch( m_nAutoState )
		{
			case AS_INIT:				AsInit();				break;
			case AS_JOB_CHECK:			AsJobCheck();			break;
			case AS_WAIT:				AsWait();				break;
			case AS_LOAD_PICK_UP:		AsLoadPickUp();			break;
			case AS_READ_BARCODE:		AsReadBarcode();		break;
			case AS_JIG_PLACE_DOWN:		AsJigPlaceDown();		break;
			case AS_JIG_PICK_UP:		AsJigPickUp();			break;
			case AS_UNLOAD_PLACE_DOWN:	AsUnloadPlaceDown();	break;
			case AS_NG_PLACE_DOWN:		AsNGPlaceDown();		break;
			default:											break;
		}
	}

	return 0;
}

UINT CRightTransferStation::ManualRun()
{
	Sleep(100);

	switch( m_nCmdCode ) 
	{
		case CMD_NONE:	break;
		default:		break;
	}

	return 0;
}

void CRightTransferStation::AsInit()
{
	m_pMainFrm->m_bInitializing_RTR = TRUE;

	while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveOriginZ() ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	m_evtInitComp.Set();
	while( !moveOriginXY() ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);

	m_evtEnablePosBuff.Set();
	m_evtEnableUseLoadBuff.Set();
	m_evtEnableUseUnloadBuff.Set();

	m_nAutoState = AS_JOB_CHECK;

	m_pMainFrm->m_bInitializing_RTR = FALSE;
}

void CRightTransferStation::AsJobCheck()
{
	if( m_pRightPPStn->m_evtLoadBuffPlaceComp.IsSet() &&
		m_pRightPPStn->m_evtEnablePosBuff.IsSet() &&
		chkLoadableJig() )
	{
		m_nAutoState = AS_LOAD_PICK_UP;
		return;
	}

	if( m_pRightPPStn->m_evtEnablePosBuff.IsSet() && chkUnloadableJig() )
	{
		if( m_pMainStn->IsDryRunMode() || IsRetestSet(m_nJigNo) )
		{
			m_nAutoState = AS_JIG_PICK_UP;
			return;
		}
		else
		{
			if( GetJigResult(m_nJigNo) == JR_PASS )
			{
				if( m_evtEnableUseUnloadBuff.IsSet() )
				{
					m_nAutoState = AS_JIG_PICK_UP;
					return;
				}
			}
			else
			{
				if( !m_pRightNGCVStn->chkEnterSensor() )
				{
					m_nAutoState = AS_JIG_PICK_UP;
					return;
				}
			}
		}
	}

	m_nAutoState = AS_WAIT;
}

void CRightTransferStation::AsWait()
{
	int nLastLoadedJig = 0;
	long lTestTime = 0;
	CAxRobotLoc locTemp;

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( chkOnlyLoadable() ) locTemp = m_locLoad;
	else
	{
		for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
		{
			if( GetJigExist(i + 1) &&
				(GetJigTestTime(i + 1) > lTestTime) )
			{
				nLastLoadedJig = i + 1;
				lTestTime = GetJigTestTime(i + 1);
			}
		}

		if( (nLastLoadedJig > 0) && (nLastLoadedJig <= DEF_MAX_JIG_ONE_SIDE) ) locTemp = m_locJig[nLastLoadedJig - 1];
		else locTemp = m_locLoad;
	}

	while( !moveXY(locTemp.x, locTemp.y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosBuff.Set();

	m_nAutoState = AS_JOB_CHECK;
}

void CRightTransferStation::AsLoadPickUp()
{
	m_nTransferResult = JR_UNKNOWN;
	m_nTransferRetestCnt = 0;
	m_sTransferBarcode = _T("NoBCR");
	m_sTransferFailName = _T("");

	if( !m_pMainStn->IsDryRunMode() && !chkGrip() )
	{
		m_sTransferFailName = _T("Set exist before loading");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveXY(m_locLoad.x, m_locLoad.y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	m_evtEnablePosBuff.Reset();
	Sleep(10);
	if( !m_pRightPPStn->m_evtEnablePosBuff.IsSet() )
	{
		m_evtEnablePosBuff.Set();
		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	while( !moveZ(m_locLoad.z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(m_locLoad.z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !ioGrip() ) ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() )
	{
		while( chkGrip() )
		{
			while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);

			if( m_pRightPPStn->chkLoadBuffExist() )
			{
				while( !moveZ(m_locLoad.z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				while( !moveZ(m_locLoad.z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				while( !ioGrip() ) ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
				WaitRight(10);
			}
			else
			{
				m_evtEnablePosBuff.Set();

				m_pRightPPStn->m_evtLoadBuffPlaceComp.Reset();
				m_evtEnableUseLoadBuff.Set();

				m_nAutoState = AS_JOB_CHECK;
				return;
			}
		}
		WaitRight(10);
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosBuff.Set();
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() && chkGrip() )
	{
		ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);

		if( !m_pRightPPStn->chkLoadBuffExist() )
		{
			m_pRightPPStn->m_evtLoadBuffPlaceComp.Reset();
			m_evtEnableUseLoadBuff.Set();
		}

		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	m_pRightPPStn->m_evtLoadBuffPlaceComp.Reset();
	m_evtEnableUseLoadBuff.Set();

	m_nAutoState = AS_READ_BARCODE;
}

void CRightTransferStation::AsReadBarcode()
{
	CCommBCR* pBCR = (CCommBCR*)m_pMainFrm->m_pSerialHub->GetSerial(_T("BCR_Right"));
	CAxTimer tmBCR;

	if( !m_pMainStn->IsDryRunMode() && chkGrip() )
	{
		ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
		m_sTransferFailName = _T("Set not exist before barcode reading");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	pBCR->m_bRecvComp = FALSE;
	while( !moveXY(m_locBCR.x, m_locBCR.y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(m_locBCR.z) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( m_pMainStn->IsDryRunMode() ||
		(m_pMainStn->IsBypassMode() && !m_pMainStn->m_bUseBCR_OnPassRun) )
	{
		WaitRight(m_pMainStn->m_nBCRTimeout);

		while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
		WaitRight(10);

		m_nAutoState = AS_JIG_PLACE_DOWN;
		return;
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #1
	BOOL bReadFlag1 = TRUE;
	tmBCR.Start();
	while( !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			bReadFlag1 = FALSE;
			break;
		}
		WaitRight(10);
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #2
	if( !bReadFlag1 )
	{
		while( !moveZ(m_locBCR.z + m_pMainStn->m_dBCRRetryDist, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bReadFlag2 = TRUE;
	tmBCR.Start();
	while( !bReadFlag1 && !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			bReadFlag2 = FALSE;
			break;
		}
		WaitRight(10);
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #3
	if( !bReadFlag2 )
	{
		while( !moveXY(m_locBCR.x + m_pMainStn->m_dBCRRetryDist, m_locBCR.y, TRUE) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bReadFlag3 = TRUE;
	tmBCR.Start();
	while( !bReadFlag2 && !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			bReadFlag3 = FALSE;
			break;
		}
		WaitRight(10);
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #4
	if( !bReadFlag3 )
	{
		while( !moveXY(m_locBCR.x, m_locBCR.y + m_pMainStn->m_dBCRRetryDist, TRUE) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bReadFlag4 = TRUE;
	tmBCR.Start();
	while( !bReadFlag3 && !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			bReadFlag4 = FALSE;
			break;
		}
		WaitRight(10);
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #5
	if( !bReadFlag4 )
	{
		while( !moveXY(m_locBCR.x - m_pMainStn->m_dBCRRetryDist, m_locBCR.y, TRUE) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	BOOL bReadFlag5 = TRUE;
	tmBCR.Start();
	while( !bReadFlag4 && !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			bReadFlag5 = FALSE;
			break;
		}
		WaitRight(10);
	}

	//////////////////////////////////////////////////////////////////////////
	// Try #6
	if( !bReadFlag5 )
	{
		while( !moveXY(m_locBCR.x, m_locBCR.y - m_pMainStn->m_dBCRRetryDist, TRUE) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	tmBCR.Start();
	while( !bReadFlag5 && !pBCR->m_bRecvComp )
	{
		if( tmBCR.IsTimeUp(m_pMainStn->m_nBCRTimeout) )
		{
			m_sTransferFailName = _T("Failed to read barcode");
			m_nAutoState = AS_NG_PLACE_DOWN;
			return;
		}
		WaitRight(10);
	}

	m_sTransferBarcode = pBCR->m_sRecv;

	if( m_sTransferBarcode.GetLength() != m_pMainStn->m_nBarcodeLength )
	{
		m_sTransferFailName = _T("Incorrect barcode length");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	m_nTransferRetestCnt = 0;
	m_nAutoState = AS_JIG_PLACE_DOWN;
}

void CRightTransferStation::AsJigPlaceDown()
{
	if( (m_nJigNo <= 0) || (m_nJigNo > DEF_MAX_JIG_ONE_SIDE) )
	{
		m_sTransferFailName = _T("Incorrect jig number");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveXY(m_locJig[m_nJigNo - 1].x, m_locJig[m_nJigNo - 1].y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() && chkGrip() )
	{
		ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);

		if( chkGrip() )
		{
			m_nAutoState = AS_JOB_CHECK;
			return;
		}
	}

	if( GetJigUse(m_nJigNo) != JU_USE )
	{
		m_sTransferFailName = _T("Jig is blocked during move");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	_oPackOut(m_nJigNo - 1);
	while( !moveZ(m_locJig[m_nJigNo - 1].z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(m_locJig[m_nJigNo - 1].z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() )
	{
		if( chkDoublePlace() )
		{
			while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			m_bDoubleOut = TRUE;
			ErrorRight(ERR_DOUBLE_PLACE, emRetry, m_sName, m_sErrPath);
			m_sTransferFailName = _T("Double place checked while place down to jig");
			m_nAutoState = AS_NG_PLACE_DOWN;
			return;
		}

		while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}
	else
	{
		while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	_oPackIn(m_nJigNo - 1);
	if( m_pMainStn->m_bZUpAfterPackIn ) WaitRight(m_pMainStn->m_nPackInsertDelay);
	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	SetJigExist(m_nJigNo, TRUE);
	SetJigBarcode(m_nJigNo, m_sTransferBarcode);
	SetJigRetestCnt(m_nJigNo, m_nTransferRetestCnt);
	
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	if( GetPCNumber(m_nJigNo) == 1 ) pJigSys4->m_bEnableStart[GetSlotNumber(m_nJigNo) - 1] = TRUE;
	else if( GetPCNumber(m_nJigNo) == 2 ) pJigSys5->m_bEnableStart[GetSlotNumber(m_nJigNo) - 1] = TRUE;
	else if( GetPCNumber(m_nJigNo) == 3 ) pJigSys6->m_bEnableStart[GetSlotNumber(m_nJigNo) - 1] = TRUE;

	m_nAutoState = AS_JOB_CHECK;
}

void CRightTransferStation::AsJigPickUp()
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(m_nJigNo);
	int nSlot = GetSlotNumber(m_nJigNo);

	if( (m_nJigNo <= 0) || (m_nJigNo > DEF_MAX_JIG_ONE_SIDE) )
	{
		m_nAutoState = AS_JOB_CHECK;
		return;
	}

	if( !m_pMainStn->IsDryRunMode() && !chkGrip() )
	{
		m_sTransferFailName = _T("Set exist before pick up from jig");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveXY(m_locJig[m_nJigNo - 1].x, m_locJig[m_nJigNo - 1].y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	_oPackOut(m_nJigNo - 1);
	while( !moveZ(m_locJig[m_nJigNo - 1].z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(m_locJig[m_nJigNo - 1].z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !ioGrip() ) ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() )
	{
		if( chkGrip() )
		{
			while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			if( m_bDoubleOut )
			{
				m_bDoubleOut = FALSE;
				SetJigExist(m_nJigNo, FALSE);

				if( nPC == 1 ) pJigSys4->m_bEnableUnload[nSlot - 1] = FALSE;
				else if( nPC == 2 ) pJigSys5->m_bEnableUnload[nSlot - 1] = FALSE;
				else if( nPC == 3 ) pJigSys6->m_bEnableUnload[nSlot - 1] = FALSE;

				m_nAutoState = AS_JOB_CHECK;
				return;
			}

			ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);

			while( !moveZ(m_locJig[m_nJigNo - 1].z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			while( !moveZ(m_locJig[m_nJigNo - 1].z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			while( !ioGrip() ) ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			if( chkGrip() )
			{
				while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
				WaitRight(10);

				ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);

				SetJigExist(m_nJigNo, FALSE);

				if( nPC == 1 ) pJigSys4->m_bEnableUnload[nSlot - 1] = FALSE;
				else if( nPC == 2 ) pJigSys5->m_bEnableUnload[nSlot - 1] = FALSE;
				else if( nPC == 3 ) pJigSys6->m_bEnableUnload[nSlot - 1] = FALSE;

				m_nAutoState = AS_JOB_CHECK;
				return;
			}
		}
		WaitRight(10);
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	int nTempJigNo = m_nJigNo;
	m_nTransferResult = GetJigResult(m_nJigNo);
	m_nTransferRetestCnt = GetJigRetestCnt(m_nJigNo);
	m_sTransferBarcode = GetJigBarcode(m_nJigNo);
	m_sTransferFailName = GetJigFailName(m_nJigNo);

	if( nPC == 1 ) pJigSys4->m_bEnableUnload[nSlot - 1] = FALSE;
	else if( nPC == 2 ) pJigSys5->m_bEnableUnload[nSlot - 1] = FALSE;
	else if( nPC == 3 ) pJigSys6->m_bEnableUnload[nSlot - 1] = FALSE;

	if( !m_pMainStn->IsDryRunMode() && chkGrip() )
	{
		SetJigExist(m_nJigNo, FALSE);
		ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
		m_sTransferFailName = _T("Set not exist after pick up from jig");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	if( m_bDoubleOut )
	{
		SetJigExist(m_nJigNo, FALSE);
		m_bDoubleOut = FALSE;
		m_sTransferFailName = _T("Double place checked after pick up from jig");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	if( GetJigResult(m_nJigNo) == JR_PASS )
	{
		m_nAutoState = AS_UNLOAD_PLACE_DOWN;
	}
	else if( IsRetestSet(m_nJigNo) && chkLoadableJig() )
	{
		m_nTransferRetestCnt++;
		m_nAutoState = AS_JIG_PLACE_DOWN;
	}
	else
	{
		m_nAutoState = AS_NG_PLACE_DOWN;
	}

	SetJigExist(nTempJigNo, FALSE);
}

void CRightTransferStation::AsUnloadPlaceDown()
{
	if( !m_pMainStn->IsDryRunMode() && chkGrip() )
	{
		ErrorRight(ERR_GRIP, emRetry, m_sName, m_sErrPath);
		m_sTransferFailName = _T("Set not exist before place down to unload buffer");
		m_nAutoState = AS_NG_PLACE_DOWN;
		return;
	}

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveXY(m_locUnload.x, m_locUnload.y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !m_pMainStn->IsDryRunMode() && m_pRightPPStn->chkUnloadBuffExist() )
	{
		ErrorRight(ERR_UNLOAD_EXIST, emRetry, m_sName, m_sErrPath);
	}

	m_evtEnablePosBuff.Reset();
	Sleep(10);
	if( !m_pRightPPStn->m_evtEnablePosBuff.IsSet() )
	{
		m_evtEnablePosBuff.Set();
		return;
	}

	while( !moveZ(m_locUnload.z - m_pMainStn->m_dSlowDownDist) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(m_locUnload.z, TRUE) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( !m_pMainStn->IsDryRunMode() )
	{
		if( chkDoublePlace() )
		{
			while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
			WaitRight(10);

			ErrorRight(ERR_DOUBLE_PLACE, emRetry, m_sName, m_sErrPath);
			m_sTransferFailName = _T("Double place checked while place down to unload buffer");
			m_nAutoState = AS_NG_PLACE_DOWN;
			return;
		}

		while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}
	else
	{
		while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
		WaitRight(10);
	}

	m_evtEnableUseUnloadBuff.Reset();

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	m_evtEnablePosBuff.Set();
	m_evtUnloadComp.Set();
	WaitRight(10);

	m_nAutoState = AS_JOB_CHECK;
}

void CRightTransferStation::AsNGPlaceDown()
{
	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveXY(m_locNG.x, m_locNG.y) ) ErrorRight(ERR_MOVE_XY, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( m_pRightNGCVStn->chkEnterSensor() ||
		!m_pRightNGCVStn->m_evtEnableUse.IsSet() ) return;

	while( !moveZ(m_locNG.z) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	CString sTemp = _T("");
	sTemp.Format(_T("[%02d][%s] %s"), m_nJigNo + DEF_MAX_JIG_ONE_SIDE, m_sTransferBarcode.Right(5), m_sTransferFailName);
	m_pMainFrm->m_sRightNGListBuff[m_pMainFrm->m_nRightNGListCnt % DEF_MAX_NG_LIST_CNT] = sTemp;
	m_pMainFrm->m_nRightNGListCnt++;

	CCommLabelPrinter* pLabelPrinter = (CCommLabelPrinter*)m_pMainFrm->m_pSerialHub->GetSerial(_T("LabelPrinter2"));
	pLabelPrinter->m_nJigNo = m_nJigNo;
	pLabelPrinter->m_sBarcode = m_sTransferBarcode;
	pLabelPrinter->m_sFailName = m_sTransferFailName;
	pLabelPrinter->SendPacket(CCommLabelPrinter::CC_PRINT);

	while( !ioUngrip() ) ErrorRight(ERR_UNGRIP, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	while( !moveZ(0.0) ) ErrorRight(ERR_MOVE_Z, emRetry, m_sName, m_sErrPath);
	WaitRight(10);

	if( m_bDoubleOut ) m_nAutoState = AS_JIG_PICK_UP;
	else m_nAutoState = AS_JOB_CHECK;
}

void CRightTransferStation::InitVariable()
{
	m_evtInitStart.Reset();
	m_evtInitComp.Reset();
	m_evtEnablePosBuff.Reset();
	m_evtEnableUseLoadBuff.Reset();
	m_evtEnableUseUnloadBuff.Reset();
	m_evtUnloadComp.Reset();

	m_bDoubleOut = FALSE;
	m_nJigNo = 0;

	m_nAutoState = AS_INIT;
}

void CRightTransferStation::WaitRight(DWORD dwTime)
{
	CAxTimer timer;
	timer.Start();

	while( TRUE )
	{
		if( (m_pMainStn->m_nStateRight == MS_AUTO) &&
			(timer.IsTimeUp((LONG)dwTime)) ) break;

		if( m_pMainStn->m_nStateLeft == MS_AUTO ) Sleep(1);
		else Wait(1);
	}
}

int CRightTransferStation::ErrorRight(int nNumber, UINT nType, LPCTSTR pszSource, LPCTSTR pszPath,
									  LPCTSTR pszParam1, LPCTSTR pszParam2, LPCTSTR pszParam3, LPCTSTR pszParam4)
{
	if( m_pMainStn->m_nStateRight == MS_ERROR )
	{
		WaitRight(10);
		return emRetry;
	}

	m_pMainStn->m_nStateRight = MS_ERROR;

	m_errCtrl.m_pErrMsg->m_nNumber = nNumber;
	m_errCtrl.m_pErrMsg->m_nType = nType;
	m_errCtrl.m_pErrMsg->m_sSource = pszSource;
	m_errCtrl.m_pErrMsg->m_sHelpFile = pszPath;
	m_errCtrl.m_pErrMsg->m_sParams[0] = pszParam1;
	m_errCtrl.m_pErrMsg->m_sParams[1] = pszParam2;
	m_errCtrl.m_pErrMsg->m_sParams[2] = pszParam3;
	m_errCtrl.m_pErrMsg->m_sParams[3] = pszParam4;
	CAxErrorMgr::GetErrorMgr()->RaiseError((CAxErrData)(*m_errCtrl.m_pErrMsg));

	m_pMainStn->m_nResponseRight = emNone;
	SendMessage(m_pMainFrm->GetSafeHwnd(), UM_MCP_ERROR, 0, 0);

	while( TRUE )
	{
		if( m_pMainStn->m_nResponseRight != emNone ) break;

		if( m_pMainStn->m_nStateRight != MS_ERROR )
		{
			WaitRight(10);
			return emRetry;
		}

		if( m_pMainStn->m_nStateLeft == MS_AUTO ) Sleep(1);
		else Wait(1);
	}

	int nRet = m_pMainStn->m_nResponseRight;
	m_pMainStn->m_nResponseRight = emNone;
	m_pMainStn->m_nStateRight = MS_AUTO;

	return nRet;
}

BOOL CRightTransferStation::moveX(double dPos, BOOL bSlow)
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	if( (dPos > m_dSWLimitPosX) ||
		(dPos < m_dSWLimitNegX) ) return FALSE;

	long lCurPosZ = 0;
	double dCurPosZ = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurPosZ) != FMM_OK ) return FALSE;
	dCurPosZ = (double)lCurPosZ / m_dScaleZ;
	if( dCurPosZ > m_pMainStn->m_dInterlockPosZ ) return FALSE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	long lAbsPos, lVelocity;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_X);

	if( stAxisStatus.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, TRUE);

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	lAbsPos = (long)(dPos * m_dScaleX);
	lVelocity = bSlow ? (long)(m_dSlowSpdX * m_dScaleX) : (long)(m_dVelocityX * m_dScaleX);
	nRtn = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_X, lAbsPos, lVelocity);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp((long)m_pMainStn->m_nMotionTimeout)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_MOTIONING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPos) != FMM_OK ) return FALSE;
	
	dCurPos = (double)lCurPos / m_dScaleX;

	if( fabs(dPos - dCurPos) < m_pMainStn->m_dInposition ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::moveY(double dPos, BOOL bSlow)
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	if( (dPos > m_dSWLimitPosY) ||
		(dPos < m_dSWLimitNegY) ) return FALSE;

	long lCurPosZ = 0;
	double dCurPosZ = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurPosZ) != FMM_OK ) return FALSE;
	dCurPosZ = (double)lCurPosZ / m_dScaleZ;
	if( dCurPosZ > m_pMainStn->m_dInterlockPosZ ) return FALSE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	long lAbsPos, lVelocity;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Y);

	if( stAxisStatus.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, TRUE);

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	lAbsPos = (long)(dPos * m_dScaleY);
	lVelocity = bSlow ? (long)(m_dSlowSpdY * m_dScaleY) : (long)(m_dVelocityY * m_dScaleY);
	nRtn = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_Y, lAbsPos, lVelocity);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp((long)m_pMainStn->m_nMotionTimeout)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_MOTIONING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPos) != FMM_OK ) return FALSE;

	dCurPos = (double)lCurPos / m_dScaleY;

	if( fabs(dPos - dCurPos) < m_pMainStn->m_dInposition ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::moveZ(double dPos, BOOL bSlow)
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	if( (dPos > m_dSWLimitPosZ) ||
		(dPos < m_dSWLimitNegZ) )
	{
		m_Trace.Log(_T("moveZ Failed : Over Software Limit"));
		return FALSE;
	}

	long lCurPosPP = 0;
	long lCurPosX = 0;
	long lCurPosY = 0;

	double dCurPosPP = 0.0;
	double dCurPosX = 0.0;
	double dCurPosY = 0.0;

	if( dPos > m_pMainStn->m_dInterlockPosZ )
	{
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_PP, &lCurPosPP) != FMM_OK ) Sleep(10);
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPosX) != FMM_OK ) Sleep(10);
		while( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurPosY) != FMM_OK ) Sleep(10);

		dCurPosPP = (double)lCurPosPP / m_pRightPPStn->m_dScaleX;
		dCurPosX = (double)lCurPosX / m_dScaleX;
		dCurPosY = (double)lCurPosY / m_dScaleY;

		if( (dCurPosX <= (m_locLoad.x + m_pMainStn->m_dInposition)) &&
			(dCurPosX >= (m_locLoad.x - m_pMainStn->m_dInposition)) )
		{
			if( (dCurPosY <= (m_locLoad.y + m_pMainStn->m_dInposition)) &&
				(dCurPosY >= (m_locLoad.y - m_pMainStn->m_dInposition)) )
			{
				if( (dCurPosPP <= (m_pRightPPStn->m_locBuff.x + m_pMainStn->m_dInposition)) &&
					(dCurPosPP >= (m_pRightPPStn->m_locBuff.x - m_pMainStn->m_dInposition)) )
				{
					m_Trace.Log(_T("moveZ Failed : Loading Buffer Interlock"));
					return FALSE;
				}
			}
		}

		if( (dCurPosX <= (m_locUnload.x + m_pMainStn->m_dInposition)) &&
			(dCurPosX >= (m_locUnload.x - m_pMainStn->m_dInposition)) )
		{
			if( (dCurPosY <= (m_locUnload.y + m_pMainStn->m_dInposition)) &&
				(dCurPosY >= (m_locUnload.y - m_pMainStn->m_dInposition)) )
			{
				if( (dCurPosPP <= (m_pRightPPStn->m_locBuff.x + m_pMainStn->m_dInposition)) &&
					(dCurPosPP >= (m_pRightPPStn->m_locBuff.x - m_pMainStn->m_dInposition)) )
				{
					m_Trace.Log(_T("moveZ Failed : Unloading Buffer Interlock"));
					return FALSE;
				}
			}
		}
	}

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	long lAbsPos, lVelocity;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected )
	{
		m_Trace.Log(_T("moveZ Failed : EziServo Not Connected"));
		return FALSE;
	}

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwAxisStatus);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveZ Failed : Get Axis Status Failed"));
		return FALSE;
	}

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Z);

	if( stAxisStatus.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Z, TRUE);

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveZ Failed : Get Input Failed"));
		return FALSE;
	}

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	lAbsPos = (long)(dPos * m_dScaleZ);
	lVelocity = bSlow ? (long)(m_dSlowSpdZ * m_dScaleZ) : (long)(m_dVelocityZ * m_dScaleZ);
	nRtn = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_Z, lAbsPos, lVelocity);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveZ Failed : Send Command Failed"));
		return FALSE;
	}

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp((long)m_pMainStn->m_nMotionTimeout)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Z);

			m_Trace.Log(_T("moveZ Failed : Wait Done Failed"));
			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_MOTIONING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurPos) != FMM_OK )
	{
		m_Trace.Log(_T("moveZ Failed : Get Position Failed"));
		return FALSE;
	}

	dCurPos = (double)lCurPos / m_dScaleZ;

	if( fabs(dPos - dCurPos) < m_pMainStn->m_dInposition ) return TRUE;

	m_Trace.Log(_T("moveZ Failed : Check Inposition Failed"));
	return FALSE;
}

BOOL CRightTransferStation::moveXY(double dPosX, double dPosY, BOOL bSlow)
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	if( (dPosX > m_dSWLimitPosX) ||
		(dPosX < m_dSWLimitNegX) ||
		(dPosY > m_dSWLimitPosY) ||
		(dPosY < m_dSWLimitNegY) ) return FALSE;

	long lCurPosZ = 0;
	double dCurPosZ = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurPosZ) != FMM_OK ) return FALSE;
	dCurPosZ = (double)lCurPosZ / m_dScaleZ;
	if( dCurPosZ > m_pMainStn->m_dInterlockPosZ ) return FALSE;

	DWORD dwAxisStatusX, dwAxisStatusY;
	ULONGLONG dwInputX, dwInputY;
	EZISERVO_AXISSTATUS stAxisStatusX, stAxisStatusY;
	long lAbsPosX, lAbsPosY;
	long lVelocityX, lVelocityY;
	int nRtnX, nRtnY;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtnX = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatusX);
	nRtnY = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatusY);
	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) ) return FALSE;

	stAxisStatusX.dwValue = dwAxisStatusX;
	stAxisStatusY.dwValue = dwAxisStatusY;

	if( stAxisStatusX.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_X);
	if( stAxisStatusY.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Y);

	if( stAxisStatusX.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, TRUE);
	if( stAxisStatusY.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, TRUE);

	nRtnX = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInputX);
	nRtnY = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInputY);
	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) ) return FALSE;

	if( dwInputX & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	if( dwInputY & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	lAbsPosX = (long)(dPosX * m_dScaleX);
	lAbsPosY = (long)(dPosY * m_dScaleY);
	lVelocityX = bSlow ? (long)(m_dSlowSpdX * m_dScaleX) : (long)(m_dVelocityX * m_dScaleX);
	lVelocityY = bSlow ? (long)(m_dSlowSpdY * m_dScaleY) : (long)(m_dVelocityY * m_dScaleY);
	nRtnX = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_X, lAbsPosX, lVelocityX);
	nRtnY = FAS_MoveSingleAxisAbsPos(DEF_FAS_RIGHT, DEF_AXIS_Y, lAbsPosY, lVelocityY);

	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) )
	{
		FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);
		FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

		return FALSE;
	}

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtnX = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatusX);
		nRtnY = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatusY);

		if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) || (tmMotion.IsTimeUp((long)m_pMainStn->m_nMotionTimeout)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

			return FALSE;
		}

		stAxisStatusX.dwValue = dwAxisStatusX;
		stAxisStatusY.dwValue = dwAxisStatusY;
	}
	while( stAxisStatusX.FFLAG_MOTIONING || stAxisStatusY.FFLAG_MOTIONING );

	long lCurPosX = 0;
	long lCurPosY = 0;
	double dCurPosX = 0.0;
	double dCurPosY = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPosX) != FMM_OK ) return FALSE;
	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurPosY) != FMM_OK ) return FALSE;

	dCurPosX = (double)lCurPosX / m_dScaleX;
	dCurPosY = (double)lCurPosY / m_dScaleY;

	if( (fabs(dPosX - dCurPosX) < m_pMainStn->m_dInposition) &&
		(fabs(dPosY - dCurPosY) < m_pMainStn->m_dInposition) ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::moveOriginX()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_X);

	if( stAxisStatus.FFLAG_SERVOON == 0 )
	{
		FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, TRUE);
		Sleep(3000);
	}

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	nRtn = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_X);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp(30000)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_ORIGINRETURNING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPos) != FMM_OK ) return FALSE;

	dCurPos = (double)lCurPos / m_dScaleX;

	if( fabs(dCurPos) < m_pMainStn->m_dInposition) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::moveOriginY()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatus);
	if( nRtn != FMM_OK ) return FALSE;

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Y);

	if( stAxisStatus.FFLAG_SERVOON == 0 )
	{
		FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, TRUE);
		Sleep(3000);
	}

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);
	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	nRtn = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_Y);
	if( nRtn != FMM_OK ) return FALSE;

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp(30000)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_ORIGINRETURNING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurPos) != FMM_OK ) return FALSE;

	dCurPos = (double)lCurPos / m_dScaleY;

	if( fabs(dCurPos) < m_pMainStn->m_dInposition) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::moveOriginZ()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	DWORD dwAxisStatus;
	ULONGLONG dwInput;
	EZISERVO_AXISSTATUS stAxisStatus;
	int nRtn;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected )
	{
		m_Trace.Log(_T("moveOriginZ Failed : EziServo Not Connected"));
		return FALSE;
	}

	nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwAxisStatus);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveOriginZ Failed : Get Axis Status Failed"));
		return FALSE;
	}

	stAxisStatus.dwValue = dwAxisStatus;

	if( stAxisStatus.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Z);

	if( stAxisStatus.FFLAG_SERVOON == 0 )
	{
		FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Z, TRUE);
		Sleep(3000);
	}

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwInput);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveOriginZ Failed : Get Input Failed"));
		return FALSE;
	}

	if( dwInput & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	nRtn = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_Z);
	if( nRtn != FMM_OK )
	{
		m_Trace.Log(_T("moveOriginZ Failed : Send Command Failed"));
		return FALSE;
	}

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtn = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Z, &dwAxisStatus);

		if( (nRtn != FMM_OK) || (tmMotion.IsTimeUp(30000)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Z);

			m_Trace.Log(_T("moveOriginZ Failed : Wait Done Failed"));
			return FALSE;
		}

		stAxisStatus.dwValue = dwAxisStatus;
	}
	while( stAxisStatus.FFLAG_ORIGINRETURNING );

	long lCurPos = 0;
	double dCurPos = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Z, &lCurPos) != FMM_OK )
	{
		m_Trace.Log(_T("moveOriginZ Failed : Get Position Failed"));
		return FALSE;
	}

	dCurPos = (double)lCurPos / m_dScaleZ;

	if( fabs(dCurPos) < m_pMainStn->m_dInposition) return TRUE;

	m_Trace.Log(_T("moveOriginZ Failed : Check Inposition Failed"));
	return FALSE;
}

BOOL CRightTransferStation::moveOriginXY()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	DWORD dwAxisStatusX, dwAxisStatusY;
	ULONGLONG dwInputX, dwInputY;
	EZISERVO_AXISSTATUS stAxisStatusX, stAxisStatusY;
	int nRtnX, nRtnY;
	CAxTimer tmMotion;

	if( !m_pMainFrm->m_bEziServoConnected ) return FALSE;

	nRtnX = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatusX);
	nRtnY = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatusY);
	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) ) return FALSE;

	stAxisStatusX.dwValue = dwAxisStatusX;
	stAxisStatusY.dwValue = dwAxisStatusY;

	if( stAxisStatusX.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_X);
	if( stAxisStatusY.FFLAG_ERRORALL ) FAS_ServoAlarmReset(DEF_FAS_RIGHT, DEF_AXIS_Y);

	if( stAxisStatusX.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_X, TRUE);
	if( stAxisStatusY.FFLAG_SERVOON == 0 ) FAS_ServoEnable(DEF_FAS_RIGHT, DEF_AXIS_Y, TRUE);
	if( (stAxisStatusX.FFLAG_SERVOON == 0) || (stAxisStatusY.FFLAG_SERVOON == 0) ) Sleep(3000);

	nRtnX = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInputX);
	nRtnY = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInputY);
	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) ) return FALSE;

	if( dwInputX & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	if( dwInputY & (SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP) )
		FAS_SetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_IN_LOGIC_STOP | SERVO_IN_LOGIC_PAUSE | SERVO_IN_LOGIC_ESTOP);

	nRtnX = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_X);
	nRtnY = FAS_MoveOriginSingleAxis(DEF_FAS_RIGHT, DEF_AXIS_Y);

	if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) )
	{
		FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);
		FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

		return FALSE;
	}

	tmMotion.Start();

	do
	{
		Sleep(1);

		nRtnX = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_X, &dwAxisStatusX);
		nRtnY = FAS_GetAxisStatus(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwAxisStatusY);

		if( (nRtnX != FMM_OK) || (nRtnY != FMM_OK) || (tmMotion.IsTimeUp(30000)) )
		{
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_X);
			FAS_MoveStop(DEF_FAS_RIGHT, DEF_AXIS_Y);

			return FALSE;
		}

		stAxisStatusX.dwValue = dwAxisStatusX;
		stAxisStatusY.dwValue = dwAxisStatusY;
	}
	while( stAxisStatusX.FFLAG_ORIGINRETURNING || stAxisStatusY.FFLAG_ORIGINRETURNING );

	long lCurPosX = 0;
	long lCurPosY = 0;
	double dCurPosX = 0.0;
	double dCurPosY = 0.0;

	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_X, &lCurPosX) != FMM_OK ) return FALSE;
	if( FAS_GetActualPos(DEF_FAS_RIGHT, DEF_AXIS_Y, &lCurPosY) != FMM_OK ) return FALSE;

	dCurPosX = (double)lCurPosX / m_dScaleX;
	dCurPosY = (double)lCurPosY / m_dScaleY;

	if( (fabs(dCurPosX) < m_pMainStn->m_dInposition) &&
		(fabs(dCurPosY) < m_pMainStn->m_dInposition) ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::ioGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_TR_GRIP] = TRUE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( dwInput & SERVO_IN_BITMASK_USERIN6 );

	Sleep(500);

	return TRUE;
}

BOOL CRightTransferStation::ioUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;
	CAxTimer tmCylinder;
	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_TR_GRIP] = FALSE;

	tmCylinder.Start();

	do 
	{
		Sleep(1);

		nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

		if( (nRtn != FMM_OK) || (tmCylinder.IsTimeUp((long)m_pMainStn->m_nCylinderTimeout)) ) return FALSE;
	}
	while( !(dwInput & SERVO_IN_BITMASK_USERIN6) );

	return TRUE;
}

void CRightTransferStation::_oGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_TR_GRIP] = TRUE;
}

void CRightTransferStation::_oUngrip()
{
	if( m_pMainStn->IsSimulateMode() ) return;

	CCommWorld* pWorld = (CCommWorld*)m_pMainFrm->m_pSerialHub->GetSerial(_T("World_Right"));

	pWorld->m_bOutput[DEF_IO_TR_GRIP] = FALSE;
}

void CRightTransferStation::_oPackIn(int nIndex)
{
	if( (nIndex < 0) || (nIndex >= DEF_MAX_JIG_ONE_SIDE) ) return;

	if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT0, 0);
	else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT1, 0);
	else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT2, 0);
	else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT3, 0);
	else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT4, 0);
	else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT5, 0);
	else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT6, 0);
	else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT7, 0);
	else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, SERVO_OUT_BITMASK_USEROUT8, 0);
	else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT0, 0);
	else if( nIndex == 10 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT1, 0);
	else if( nIndex == 11 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT2, 0);
	else if( nIndex == 12 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT3, 0);
	else if( nIndex == 13 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT4, 0);
	else if( nIndex == 14 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT5, 0);
	else if( nIndex == 15 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT6, 0);
	else if( nIndex == 16 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT7, 0);
	else if( nIndex == 17 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, SERVO_OUT_BITMASK_USEROUT8, 0);
	else if( nIndex == 18 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT0, 0);
	else if( nIndex == 19 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT1, 0);
	else if( nIndex == 20 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT2, 0);
	else if( nIndex == 21 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT3, 0);
	else if( nIndex == 22 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT4, 0);
	else if( nIndex == 23 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT5, 0);
#ifndef DEF_EIN_48_LCA
	else if( nIndex == 24 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT6, 0);
	else if( nIndex == 25 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT7, 0);
	else if( nIndex == 26 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, SERVO_OUT_BITMASK_USEROUT8, 0);
	else if( nIndex == 27 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_PP, SERVO_OUT_BITMASK_USEROUT0, 0);
#endif
}

void CRightTransferStation::_oPackOut(int nIndex)
{
	if( (nIndex < 0) || (nIndex >= DEF_MAX_JIG_ONE_SIDE) ) return;

	if( nIndex == 0 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT0);
	else if( nIndex == 1 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT1);
	else if( nIndex == 2 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT2);
	else if( nIndex == 3 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT3);
	else if( nIndex == 4 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT4);
	else if( nIndex == 5 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT5);
	else if( nIndex == 6 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT6);
	else if( nIndex == 7 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT7);
	else if( nIndex == 8 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_X, 0, SERVO_OUT_BITMASK_USEROUT8);
	else if( nIndex == 9 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT0);
	else if( nIndex == 10 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT1);
	else if( nIndex == 11 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT2);
	else if( nIndex == 12 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT3);
	else if( nIndex == 13 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT4);
	else if( nIndex == 14 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT5);
	else if( nIndex == 15 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT6);
	else if( nIndex == 16 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT7);
	else if( nIndex == 17 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Y, 0, SERVO_OUT_BITMASK_USEROUT8);
	else if( nIndex == 18 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT0);
	else if( nIndex == 19 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT1);
	else if( nIndex == 20 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT2);
	else if( nIndex == 21 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT3);
	else if( nIndex == 22 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT4);
	else if( nIndex == 23 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT5);
#ifndef DEF_EIN_48_LCA
	else if( nIndex == 24 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT6);
	else if( nIndex == 25 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT7);
	else if( nIndex == 26 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_Z, 0, SERVO_OUT_BITMASK_USEROUT8);
	else if( nIndex == 27 ) FAS_SetIOOutput(DEF_FAS_RIGHT, DEF_AXIS_PP, 0, SERVO_OUT_BITMASK_USEROUT0);
#endif
}

BOOL CRightTransferStation::chkGrip()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_Y, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( dwInput & SERVO_IN_BITMASK_USERIN6 ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::chkDoublePlace()
{
	if( m_pMainStn->IsSimulateMode() ) return TRUE;

	ULONGLONG dwInput;
	int nRtn;

	nRtn = FAS_GetIOInput(DEF_FAS_RIGHT, DEF_AXIS_X, &dwInput);

	if( nRtn != FMM_OK ) return FALSE;

	if( !(dwInput & SERVO_IN_BITMASK_USERIN4) ) return TRUE;

	return FALSE;
}

BOOL CRightTransferStation::chkLoadableJig()
{
	for( int i = (DEF_MAX_JIG_ONE_SIDE - 1); i >= 0; i-- )
	{
		if( (i % 2) == 0 ) continue;

		if( GetJigUse(i + 1) != JU_USE ) continue;

		if( m_pMainFrm->m_bInStop[GetPCNumber(i + 1) + 2] ) continue;

		if( !GetJigExist(i + 1) && (GetJigStatus(i + 1) == JS_READY) )
		{
			m_nJigNo = i + 1;
			return TRUE;
		}
	}

	for( int i = (DEF_MAX_JIG_ONE_SIDE - 1); i >= 0; i-- )
	{
		if( (i % 2) == 1 ) continue;

		if( GetJigUse(i + 1) != JU_USE ) continue;

		if( m_pMainFrm->m_bInStop[GetPCNumber(i + 1) + 2] ) continue;

		if( !GetJigExist(i + 1) && (GetJigStatus(i + 1) == JS_READY) )
		{
			m_nJigNo = i + 1;
			return TRUE;
		}
	}

	return FALSE;
}

BOOL CRightTransferStation::chkUnloadableJig()
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nNumWaitingJig = 0;
	int nFirstWaitingJig = 0;

	for( int i = (DEF_MAX_JIG_ONE_SIDE - 1); i >= 0; i-- )
	{
		if( (i % 2) == 0 ) continue;

		if( m_pMainFrm->m_bOutStop[GetPCNumber(i + 1) + 2] ) continue;

		if( GetPCNumber(i + 1) == 1 )
		{
			if( GetJigExist(i + 1) && pJigSys4->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
		else if( GetPCNumber(i + 1) == 2 )
		{
			if( GetJigExist(i + 1) && pJigSys5->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
		else if( GetPCNumber(i + 1) == 3 )
		{
			if( GetJigExist(i + 1) && pJigSys6->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
	}

	for( int i = (DEF_MAX_JIG_ONE_SIDE - 1); i >= 0; i-- )
	{
		if( (i % 2) == 1 ) continue;

		if( m_pMainFrm->m_bOutStop[GetPCNumber(i + 1) + 2] ) continue;

		if( GetPCNumber(i + 1) == 1 )
		{
			if( GetJigExist(i + 1) && pJigSys4->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
		else if( GetPCNumber(i + 1) == 2 )
		{
			if( GetJigExist(i + 1) && pJigSys5->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
		else if( GetPCNumber(i + 1) == 3 )
		{
			if( GetJigExist(i + 1) && pJigSys6->m_bEnableUnload[GetSlotNumber(i + 1) - 1] )
			{
				if( IsRetestSet(i + 1) && chkOnlyLoadable() )
				{
					m_nJigNo = i + 1;
					return TRUE;
				}

				if( IsRetestSet(i + 1) && !chkOnlyLoadable() )
				{
					nNumWaitingJig++;
					if( nFirstWaitingJig == 0 ) nFirstWaitingJig = i + 1;
					continue;
				}

				m_nJigNo = i + 1;
				return TRUE;
			}
		}
	}

	int nNotUseCnt = 0;
	for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		if( GetJigUse(i + 1) == JU_NOT_USE )
		{
			nNotUseCnt++;
		}
		else
		{
			if( !GetJigExist(i + 1) &&
				(GetJigStatus(i + 1) != JS_READY) )
			{
				nNotUseCnt++;
			}
		}
	}

	if( (nNumWaitingJig >= (DEF_MAX_JIG_ONE_SIDE - nNotUseCnt)) &&
		(nFirstWaitingJig != 0) )
	{
		m_nJigNo = nFirstWaitingJig;
		return TRUE;
	}

	return FALSE;
}

BOOL CRightTransferStation::chkOnlyLoadable()
{
	for( int i = 0; i < DEF_MAX_JIG_ONE_SIDE; i++ )
	{
		if( GetJigUse(i + 1) != JU_USE ) continue;
		if( m_pMainFrm->m_bInStop[GetPCNumber(i + 1) + 2] ) continue;
		if( !GetJigExist(i + 1) && (GetJigStatus(i + 1) == JS_READY) ) return TRUE;
	}

	return FALSE;
}

int CRightTransferStation::GetPCNumber(int nIndex)
{
#ifdef DEF_EIN_48_LCA
	if( (nIndex >= 1) && (nIndex <= 9) ) return 1;
	else if( (nIndex >= 10) && (nIndex <= 17) ) return 2;
	else if( (nIndex >= 18) && (nIndex <= 24) ) return 3;
	else return 0;
#else
	if( (nIndex >= 1) && (nIndex <= 9) ) return 1;
	else if( (nIndex >= 10) && (nIndex <= 18) ) return 2;
	else if( (nIndex >= 19) && (nIndex <= 28) ) return 3;
	else return 0;
#endif
}

int CRightTransferStation::GetSlotNumber(int nIndex)
{
#ifdef DEF_EIN_48_LCA
	if( (nIndex >= 1) && (nIndex <= 9) ) return nIndex;
	else if( (nIndex >= 10) && (nIndex <= 17) ) return (nIndex - 9);
	else if( (nIndex >= 18) && (nIndex <= 24) ) return (nIndex - 17);
	else return 0;
#else
	if( (nIndex >= 1) && (nIndex <= 9) ) return nIndex;
	else if( (nIndex >= 10) && (nIndex <= 18) ) return (nIndex - 9);
	else if( (nIndex >= 19) && (nIndex <= 28) ) return (nIndex - 18);
	else return 0;
#endif
}

BOOL CRightTransferStation::GetJigExist(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return FALSE;

	if( nPC == 1 ) return pJigSys4->m_bJigExist[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_bJigExist[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_bJigExist[nSlot - 1];
	else return FALSE;
}

int CRightTransferStation::GetJigUse(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return JU_NOT_USE;

	if( nPC == 1 ) return pJigSys4->m_nJigUse[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigUse[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigUse[nSlot - 1];
	else return JU_NOT_USE;
}

int CRightTransferStation::GetJigStatus(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return JS_NONE;

	if( nPC == 1 ) return pJigSys4->m_nJigStatus[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigStatus[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigStatus[nSlot - 1];
	else return JS_NONE;
}

int CRightTransferStation::GetJigResult(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return JR_UNKNOWN;

	if( nPC == 1 ) return pJigSys4->m_nJigResult[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigResult[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigResult[nSlot - 1];
	else return JR_UNKNOWN;
}

int CRightTransferStation::GetJigRetestCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigRetestCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigRetestCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigRetestCnt[nSlot - 1];
	else return 0;
}

CString CRightTransferStation::GetJigBarcode(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return _T("NoBCR");

	if( nPC == 1 ) return pJigSys4->m_sJigBarcode[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_sJigBarcode[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_sJigBarcode[nSlot - 1];
	else return _T("NoBCR");
}

CString CRightTransferStation::GetJigFailName(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return _T("");

	if( nPC == 1 ) return pJigSys4->m_sJigFailName[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_sJigFailName[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_sJigFailName[nSlot - 1];
	else return _T("");
}

long CRightTransferStation::GetJigTestTime(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 )
	{
		if( pJigSys4->m_nJigStatus[nSlot - 1] == JS_WRITING ) return pJigSys4->m_tmResult[nSlot - 1].IsTimeNow();
		else return pJigSys4->m_lTestTime[nSlot - 1];
	}
	else if( nPC == 2 )
	{
		if( pJigSys5->m_nJigStatus[nSlot - 1] == JS_WRITING ) return pJigSys5->m_tmResult[nSlot - 1].IsTimeNow();
		else return pJigSys5->m_lTestTime[nSlot - 1];
	}
	else if( nPC == 3 )
	{
		if( pJigSys6->m_nJigStatus[nSlot - 1] == JS_WRITING ) return pJigSys6->m_tmResult[nSlot - 1].IsTimeNow();
		else return pJigSys6->m_lTestTime[nSlot - 1];
	}
	else return 0;
}

double CRightTransferStation::GetJigAvgTestTime(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0.0;

	if( nPC == 1 ) return pJigSys4->m_dAvgTestTime[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_dAvgTestTime[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_dAvgTestTime[nSlot - 1];
	else return 0.0;
}

int CRightTransferStation::GetJigPassCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigPassCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigPassCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigPassCnt[nSlot - 1];
	else return 0;
}

int CRightTransferStation::GetJigFailCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigFailCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigFailCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigFailCnt[nSlot - 1];
	else return 0;
}

int CRightTransferStation::GetJigRetestPassCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigRetestPassCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigRetestPassCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigRetestPassCnt[nSlot - 1];
	else return 0;
}

int CRightTransferStation::GetJigRetestFailCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigRetestFailCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigRetestFailCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigRetestFailCnt[nSlot - 1];
	else return 0;
}

int CRightTransferStation::GetJigPackOutCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigPackOutCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigPackOutCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigPackOutCnt[nSlot - 1];
	else return 0;
}

int CRightTransferStation::GetJigSameFailCnt(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return 0;

	if( nPC == 1 ) return pJigSys4->m_nJigSameFailCnt[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_nJigSameFailCnt[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_nJigSameFailCnt[nSlot - 1];
	else return 0;
}

BOOL CRightTransferStation::GetJigRateBlocked(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return FALSE;

	if( nPC == 1 ) return pJigSys4->m_bJigRateBlocked[nSlot - 1];
	else if( nPC == 2 ) return pJigSys5->m_bJigRateBlocked[nSlot - 1];
	else if( nPC == 3 ) return pJigSys6->m_bJigRateBlocked[nSlot - 1];
	else return FALSE;
}

void CRightTransferStation::SetJigExist(int nIndex, BOOL bExist)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_bJigExist[nSlot - 1] = bExist;
	else if( nPC == 2 ) pJigSys5->m_bJigExist[nSlot - 1] = bExist;
	else if( nPC == 3 ) pJigSys6->m_bJigExist[nSlot - 1] = bExist;
	else return;
}

void CRightTransferStation::SetJigUse(int nIndex, int nUse)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigUse[nSlot - 1] = nUse;
	else if( nPC == 2 ) pJigSys5->m_nJigUse[nSlot - 1] = nUse;
	else if( nPC == 3 ) pJigSys6->m_nJigUse[nSlot - 1] = nUse;
	else return;
}

void CRightTransferStation::SetJigStatus(int nIndex, int nStatus)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigStatus[nSlot - 1] = nStatus;
	else if( nPC == 2 ) pJigSys5->m_nJigStatus[nSlot - 1] = nStatus;
	else if( nPC == 3 ) pJigSys6->m_nJigStatus[nSlot - 1] = nStatus;
	else return;
}

void CRightTransferStation::SetJigResult(int nIndex, int nResult)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigResult[nSlot - 1] = nResult;
	else if( nPC == 2 ) pJigSys5->m_nJigResult[nSlot - 1] = nResult;
	else if( nPC == 3 ) pJigSys6->m_nJigResult[nSlot - 1] = nResult;
	else return;
}

void CRightTransferStation::SetJigRetestCnt(int nIndex, int nRetestCnt)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigRetestCnt[nSlot - 1] = nRetestCnt;
	else if( nPC == 2 ) pJigSys5->m_nJigRetestCnt[nSlot - 1] = nRetestCnt;
	else if( nPC == 3 ) pJigSys6->m_nJigRetestCnt[nSlot - 1] = nRetestCnt;
	else return;
}

void CRightTransferStation::SetJigBarcode(int nIndex, CString sBarcode)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_sJigBarcode[nSlot - 1] = sBarcode;
	else if( nPC == 2 ) pJigSys5->m_sJigBarcode[nSlot - 1] = sBarcode;
	else if( nPC == 3 ) pJigSys6->m_sJigBarcode[nSlot - 1] = sBarcode;
	else return;
}

void CRightTransferStation::SetJigFailName(int nIndex, CString sFailName)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_sJigFailName[nSlot - 1] = sFailName;
	else if( nPC == 2 ) pJigSys5->m_sJigFailName[nSlot - 1] = sFailName;
	else if( nPC == 3 ) pJigSys6->m_sJigFailName[nSlot - 1] = sFailName;
	else return;
}

void CRightTransferStation::SetJigInit(int nIndex)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nAutoState[nSlot - 1] = CJigSystem::AS_INIT;
	else if( nPC == 2 ) pJigSys5->m_nAutoState[nSlot - 1] = CJigSystem::AS_INIT;
	else if( nPC == 3 ) pJigSys6->m_nAutoState[nSlot - 1] = CJigSystem::AS_INIT;
	else return;
}

void CRightTransferStation::SetJigPassCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigPassCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigPassCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigPassCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigFailCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigFailCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigFailCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigFailCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigRetestPassCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigRetestPassCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigRetestPassCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigRetestPassCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigRetestFailCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigRetestFailCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigRetestFailCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigRetestFailCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigPackOutCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigPackOutCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigPackOutCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigPackOutCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigSameFailCnt(int nIndex, int nCount)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_nJigSameFailCnt[nSlot - 1] = nCount;
	else if( nPC == 2 ) pJigSys5->m_nJigSameFailCnt[nSlot - 1] = nCount;
	else if( nPC == 3 ) pJigSys6->m_nJigSameFailCnt[nSlot - 1] = nCount;
	else return;
}

void CRightTransferStation::SetJigRateBlocked(int nIndex, BOOL bBlocked)
{
	CJigSystem* pJigSys4 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem4"));
	CJigSystem* pJigSys5 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem5"));
	CJigSystem* pJigSys6 = (CJigSystem*)CAxSystemHub::GetSystemHub()->GetSystem(_T("JigSystem6"));

	int nPC = GetPCNumber(nIndex);
	int nSlot = GetSlotNumber(nIndex);

	if( (nPC == 0) || (nSlot == 0) ) return;

	if( nPC == 1 ) pJigSys4->m_bJigRateBlocked[nSlot - 1] = bBlocked;
	else if( nPC == 2 ) pJigSys5->m_bJigRateBlocked[nSlot - 1] = bBlocked;
	else if( nPC == 3 ) pJigSys6->m_bJigRateBlocked[nSlot - 1] = bBlocked;
	else return;
}

BOOL CRightTransferStation::IsRetestSet(int nIndex)
{
	CString sTemp = _T("");

	if( (GetJigResult(nIndex) == JR_WRITE_FAIL) &&
		(GetJigRetestCnt(nIndex) < m_pMainStn->m_nMaxRetestCnt) )
	{
		for( int i = 0; i < DEF_MAX_RETEST_NAME; i++ )
		{
			sTemp = m_pMainStn->m_sRetestName[i];
			sTemp.TrimLeft(_T(" "));

			if( (sTemp.GetLength() > 0) &&
				(GetJigFailName(nIndex).Find(sTemp) == 0) )
			{
				return TRUE;
			}
		}
	}

	return FALSE;
}
