#ifndef __DEFINE_H__
#define __DEFINE_H__

#pragma once
#pragma pack(push, 1)

#define DEF_PGM_VERSION				2.01 // 2017.04.10

#define UM_MCP_ERROR				WM_USER + 11
#define UM_MCP_START				WM_USER + 12
#define UM_MCP_STOP					WM_USER + 13
#define UM_MCP_ABORT				WM_USER + 14
#define UM_MAINMENU_CHANGE			WM_USER + 15
#define UM_SUBMENU_CHANGE			WM_USER + 16
#define UM_MESSAGE					WM_USER + 17
#define UM_ERR_MESSAGE				WM_USER + 18

//////////////////////////////////////////////////////////////////////////
// Machine Type Define
//#define DEF_EIN_48_LCA
//////////////////////////////////////////////////////////////////////////

#define DEF_FAS_RIGHT				3
#define DEF_FAS_LEFT				4

#define DEF_AXIS_X					0
#define DEF_AXIS_Y					1
#define DEF_AXIS_Z					2
#define DEF_AXIS_PP					3

#define DEF_IO_START_LAMP			0
#define DEF_IO_STOP_LAMP			1
#define DEF_IO_RESET_LAMP			2
#define DEF_IO_TOWER_RED			3
#define DEF_IO_TOWER_YELLOW			4
#define DEF_IO_TOWER_GREEN			5
#define DEF_IO_LDPP_DOWN			6
#define DEF_IO_UDPP_DOWN			7
#define DEF_IO_TR_GRIP				8
#define DEF_IO_LD_GRIP				9
#define DEF_IO_UD_GRIP				10
#define DEF_IO_NGCV_RUN				11
#define DEF_IO_LDCV_RUN				12
#define DEF_IO_UDCV_RUN				13
#define DEF_IO_INCV_RUN				14
#define DEF_IO_BUZZER				15

#define DEF_IO_LDCV2_RUN			12
#define DEF_IO_STOPPER_DOWN			13

#ifdef DEF_EIN_48_LCA
	#define DEF_MAX_JIG				48
	#define DEF_MAX_JIG_ONE_SIDE	24
#else
	#define DEF_MAX_JIG				56
	#define DEF_MAX_JIG_ONE_SIDE	28
#endif

#define DEF_MAX_IO_IN_WORLD			16
#define DEF_MAX_TEST_PC				6
#define DEF_MAX_TESTER_IN_PC		10
#define DEF_MAX_RETEST_NAME			10
#define DEF_MAX_NG_LIST_CNT			100

#define COLOR_WHITE					RGB(255, 255, 255)
#define COLOR_BLACK					RGB(0, 0, 0)
#define COLOR_GRAY					RGB(128, 128, 128)
#define COLOR_SILVER				RGB(209, 209, 209)
#define COLOR_RED					RGB(255, 0, 0)
#define COLOR_GREEN					RGB(0, 255, 0)
#define COLOR_BLUE					RGB(0, 0, 255)
#define COLOR_CYAN					RGB(0, 255, 255)
#define COLOR_MAGENTA				RGB(255, 0, 255)
#define COLOR_YELLOW				RGB(255, 255, 0)

enum WorkTimeShift {
	WT_SHIFT_A,
	WT_SHIFT_B,
	WT_SHIFT_C,
	WT_MAX_SHIFT
};

enum WorkTimeList {
	WT_WORK_START,
	WT_REST1_START,
	WT_REST1_END,
	WT_LUNCH_START,
	WT_LUNCH_END,
	WT_REST2_START,
	WT_REST2_END,
	WT_WORK_END,
	WT_MAX_LIST
};

enum WorkTimeType {
	WT_HOUR,
	WT_MINUTE,
	WT_SECOND,
	WT_MAX_TYPE
};

enum LogType {
	LOG_MCP,
	LOG_GUI,
	LOG_COMM
};

enum JigUse {
	JU_NOT_USE,
	JU_USE,
	JU_CHECK_BLOCK,
	JU_FAIL_BLOCK
};

enum JigStatus {
	JS_NONE,
	JS_READY,
	JS_WAIT_OCCUPIED,
	JS_OCCUPIED,
	JS_REQUEST_PRD,
	JS_WRITING,
	JS_WRITING_DONE,
	JS_WORK_DONE,
	JS_WORK_RETEST
};

enum JigResult {
	JR_UNKNOWN,
	JR_PASS,
	JR_WRITE_FAIL,
	JR_PACK_INSERT_FAIL,
	JR_WRITE_TIME_OUT,
	JR_BARCODE_FAIL,
	JR_JIG_PN_FAIL
};

typedef BOOL (WINAPI* SetLayer)(HWND hWnd, COLORREF crKey, BYTE bAlpha, DWORD dwFlags);

#pragma pack(pop)

#endif
