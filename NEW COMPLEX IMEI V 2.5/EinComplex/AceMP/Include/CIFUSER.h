/* <St> *******************************************************************

  FILENAME      : CIFUSER.H

  -------------------------------------------------------------------------
  CREATETED     : R. Mayer,  Hilscher GmbH
  DATE          : 28.11.95
  PROJEKT       : CIF device driver
  =========================================================================

  DISCRIPTION

    User interface definition.

 *
 *  Filename:
 *   $Workfile: CIFUSER.H $ $Revision: 354 $
 *  Last Modification:
 *     $Author: MichaelT $
 *    $Modtime: 6.03.07 12:46 $
 *  
 *  Targets:
 *    Win32/ANSI   : yes
 *    Win32/Unicode: yes
 *    WinCE        : yes
 *
  =========================================================================

  CHANGES

  version name        date        description
  -------------------------------------------------------------------------
  V1.301  MY          06.03.07    - CIF 100 relevant entries removed. Including
                                    LARGE_INTEGER redefinition.
  
  V1.300  MY          26.01.04    - Review 
                                  - changes from Harald 
                                    BOARD_INFO and BOARD_INFOEX included
                                  - include "windows.h" removed

  V1.202  MY          10.12.00    - CIF100 Rev.5 changes included

  V1.201  MY          10.12.00    - Parameter definition of DevDMADown changed

  V1.200  MY          15.03.00    - New CIF100 definitions included

  V1.100  MY          19.09.99    - Function for performance test included

  V1.023  MY          06.09.01    - Error number for configuration check
  
  V1.022  MY          23.04.01    - Error number extended and 
                                    DMA driver errors included
                                  - CIF 100 specific definitions included
                                  - Definition for CIF_TKIT included

  V1.021  MY          23.08.99    - Definition against multiple inclusion
                                    added
  
  V1.020  MY          22.03.99    - New Function DevDownload included
                                  - Definitions for download modes included

  V1.011  MY          20.02.98    - SPC_CONTROL_SET/CLEAR included

  V1.010  MY          28.10.97    - Modes in DevExchangeIOErr changed from
                                    0,1,2 to 2,3,4

  V1.009  MY          16.09.97    - MS-C++ support include (#ifdef _cplusplus)
                                  - DevPutMessage(), pvData renamed into
                                    ptMessage
                                  - Function: DevReadWriteRAW()
                                              DevExchangeIOEx()
                                              DevExchangeIOErr() included.
                                  - MSG.data in MSG_STRUCT reduced to
                                    255 Byte, which is the max. data length.
                                  - New reset definition BOOTSTART included,
                                    to save parameters on CIF40
                                  - PcWatchDog into HostWatchDog renamed
                                  - Error numbers -6, -26, -27 included
                                  - Reset mode BOOTSTART included
                                  - New handshake and definitions for
                                    state field transfer included

  V1.008  MY          05.06.97    - GETMESSAGECMD, size of message included,
                                    to prevent overwriting of user buffer

  V1.007  MY          05.06.97    - static definition of the
                                    IOSEND and IORECEIVE data area removed
                                  - BOARD_INFOEX structure and function
                                    DevGetBoardInfoEx included

  V1.006  MY          25.04.97    - function DevGetExtendedInfo included

  V1.005  MY          30.11.96    - function DevExtendedData, DevGetMBXData
                                    included
                                  - error -25 included

  V1.004  MY          18.06.96    - IO function IOCTLREADSEND
                                    included

  V1.003  MY          25.04.96    - IO function IOCTLEXIO
                                                IOGETPARAM
                                                IOSETHOST
                                    included

  V1.002  MY          28.02.96    - function DevGetMBXState included
                                  - Variable name MSG_STRUC.daten
                                    changed into data

  V1.001  MY          31.01.96    - user interface changed

  V1.000  MY


  ******************************************************************** <En> */

/* prevent multiple inclusion */
#ifndef __CIFUSER_H
#define __CIFUSER_H

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifdef __cplusplus
  extern "C" {
#endif  /* _cplusplus */

#include <stdint.h>

/* ------------------------------------------------------------------------------------ */
/*  global definitions                                                                  */
/* ------------------------------------------------------------------------------------ */

#define MAX_DEV_BOARDS               4      /* maximum numbers of boards  */

/* ------------------------------------------------------------------------------------ */
/*  driver errors                                                                       */
/* ------------------------------------------------------------------------------------ */

#define DRV_NO_ERROR                 0      /* no error                                            */
#define DRV_BOARD_NOT_INITIALIZED   -1      /* DRIVER Board not initialized                        */
#define DRV_INIT_STATE_ERROR        -2      /* DRIVER Error in internal init state                 */
#define DRV_READ_STATE_ERROR        -3      /* DRIVER Error in internal read state                 */
#define DRV_CMD_ACTIVE              -4      /* DRIVER Command on this channel is active            */
#define DRV_PARAMETER_UNKNOWN       -5      /* DRIVER Unknown parameter in function occured        */
#define DRV_WRONG_DRIVER_VERSION    -6      /* DRIVER Version is incompatible with DLL             */
                                               
#define DRV_PCI_SET_CONFIG_MODE     -7      /* DRIVER Error during PCI set run mode                */
#define DRV_PCI_READ_DPM_LENGTH     -8      /* DRIVER Could not read PCI dual port memory length   */
#define DRV_PCI_SET_RUN_MODE        -9      /* DRIVER Error during PCI set run mode                */                                                  
                                            
#define DRV_DEV_DPM_ACCESS_ERROR    -10     /* DEVICE Dual port ram not accessable(board not found)*/
#define DRV_DEV_NOT_READY           -11     /* DEVICE Not ready (ready flag failed)                */
#define DRV_DEV_NOT_RUNNING         -12     /* DEVICE Not running (running flag failed)            */
#define DRV_DEV_WATCHDOG_FAILED     -13     /* DEVICE Watchdog test failed                         */
#define DRV_DEV_OS_VERSION_ERROR    -14     /* DEVICE Signals wrong OS version                     */
#define DRV_DEV_SYSERR              -15     /* DEVICE Error in dual port flags                     */
#define DRV_DEV_MAILBOX_FULL        -16     /* DEVICE Send mailbox is full                         */
#define DRV_DEV_PUT_TIMEOUT         -17     /* DEVICE PutMessage timeout                           */
#define DRV_DEV_GET_TIMEOUT         -18     /* DEVICE GetMessage timeout                           */
#define DRV_DEV_GET_NO_MESSAGE      -19     /* DEVICE No message available                         */
#define DRV_DEV_RESET_TIMEOUT       -20     /* DEVICE RESET command timeout                        */
#define DRV_DEV_NO_COM_FLAG         -21     /* DEVICE COM-flag not set                             */
#define DRV_DEV_EXCHANGE_FAILED     -22     /* DEVICE IO data exchange failed                      */
#define DRV_DEV_EXCHANGE_TIMEOUT    -23     /* DEVICE IO data exchange timeout                     */
#define DRV_DEV_COM_MODE_UNKNOWN    -24     /* DEVICE IO data mode unknown                         */
#define DRV_DEV_FUNCTION_FAILED     -25     /* DEVICE Function call failed                         */
#define DRV_DEV_DPMSIZE_MISMATCH    -26     /* DEVICE DPM size differs from configuration          */
#define DRV_DEV_STATE_MODE_UNKNOWN  -27     /* DEVICE State mode unknown                           */
#define DRV_DEV_HW_PORT_IS_USED     -28     /* DEVICE Output port already in use                   */

/* Error from Interface functions */
#define DRV_USR_OPEN_ERROR          -30     /* USER Driver not opened (device driver not loaded)   */
#define DRV_USR_INIT_DRV_ERROR      -31     /* USER Can't connect with device                      */
#define DRV_USR_NOT_INITIALIZED     -32     /* USER Board not initialized (DevInitBoard not called)*/
#define DRV_USR_COMM_ERR            -33     /* USER IOCTRL function failed                         */
#define DRV_USR_DEV_NUMBER_INVALID  -34     /* USER Parameter DeviceNumber  invalid                */
#define DRV_USR_INFO_AREA_INVALID   -35     /* USER Parameter InfoArea unknown                     */
#define DRV_USR_NUMBER_INVALID      -36     /* USER Parameter Number invalid                       */
#define DRV_USR_MODE_INVALID        -37     /* USER Parameter Mode invalid                         */
#define DRV_USR_MSG_BUF_NULL_PTR    -38     /* USER NULL pointer assignment                        */
#define DRV_USR_MSG_BUF_TOO_SHORT   -39     /* USER Message buffer too int16_t                       */
#define DRV_USR_SIZE_INVALID        -40     /* USER Parameter Size invalid                         */
#define DRV_USR_SIZE_ZERO           -42     /* USER Parameter Size with zero length                */
#define DRV_USR_SIZE_TOO_LONG       -43     /* USER Parameter Size too int32_t                        */
#define DRV_USR_DEV_PTR_NULL        -44     /* USER Device address null pointer                    */
#define DRV_USR_BUF_PTR_NULL        -45     /* USER Pointer to buffer is a null pointer            */
                                                
#define DRV_USR_SENDSIZE_TOO_LONG   -46     /* USER SendSize parameter too int32_t                    */
#define DRV_USR_RECVSIZE_TOO_LONG   -47     /* USER ReceiveSize parameter too int32_t                 */
#define DRV_USR_SENDBUF_PTR_NULL    -48     /* USER Pointer to buffer is a null pointer            */
#define DRV_USR_RECVBUF_PTR_NULL    -49     /* USER Pointer to buffer is a null pointer            */

#define DRV_DMA_INSUFF_MEM          -50     /* DMA  Memory allocation error                        */
#define DRV_DMA_TIMEOUT_CH4         -51     /* DMA  Read I/O timeout                               */
#define DRV_DMA_TIMEOUT_CH5         -52     /* DMA  Write I/O timeout                              */
#define DRV_DMA_TIMEOUT_CH6         -53     /* DMA  PCI transfer timeout                           */
#define DRV_DMA_TIMEOUT_CH7         -54     /* DMA  Download timeout                               */

#define DRV_DMA_DB_DOWN_FAIL        -55     /* DMA  Database download failed                       */
#define DRV_DMA_FW_DOWN_FAIL        -56     /* DMA  Firmware download failed                       */
#define DRV_CLEAR_DB_FAIL           -57     /* DMA  Clear database on the device failed            */

#define DRV_DEV_NO_VIRTUAL_MEM      -60     /* USER Virtual memory not available                   */
#define DRV_DEV_UNMAP_VIRTUAL_MEM   -61     /* USER Unmap virtual memory failed                    */

#define DRV_GENERAL_ERROR           -70     /* DRIVER General error                                */
#define DRV_DMA_ERROR               -71     /* DRIVER General DMA error                            */
#define DRV_WDG_IO_ERROR            -74     /* DRIVER I/O WatchDog failed                          */
#define DRV_WDG_DEV_ERROR           -75     /* DRIVER Device WatchDog failed                       */
                                                                                                   
#define DRV_USR_DRIVER_UNKNOWN      -80     /* USER driver unknown                                 */
#define DRV_USR_DEVICE_NAME_INVALID -81     /* USER device name invalid                            */
#define DRV_USR_DEVICE_NAME_UKNOWN  -82     /* USER device name unknown                            */
#define DRV_USR_DEVICE_FUNC_NOTIMPL -83     /* USER device function not implemented                */

#define DRV_USR_FILE_OPEN_FAILED    -100    /* USER File not opened                                */
#define DRV_USR_FILE_SIZE_ZERO      -101    /* USER File size zero                                 */
#define DRV_USR_FILE_NO_MEMORY      -102    /* USER Not enough memory to load file                 */
#define DRV_USR_FILE_READ_FAILED    -103    /* USER File read failed                               */
#define DRV_USR_INVALID_FILETYPE    -104    /* USER File type invalid                              */
#define DRV_USR_FILENAME_INVALID    -105    /* USER File name not valid                            */

#define DRV_FW_FILE_OPEN_FAILED     -110    /* USER Firmware file not opened                       */
#define DRV_FW_FILE_SIZE_ZERO       -111    /* USER Firmware file size zero                        */
#define DRV_FW_FILE_NO_MEMORY       -112    /* USER Not enough memory to load firmware file        */
#define DRV_FW_FILE_READ_FAILED     -113    /* USER Firmware file read failed                      */
#define DRV_FW_INVALID_FILETYPE     -114    /* USER Firmware file type invalid                     */
#define DRV_FW_FILENAME_INVALID     -115    /* USER Firmware file name not valid                   */
#define DRV_FW_DOWNLOAD_ERROR       -116    /* USER Firmware file download error                   */
#define DRV_FW_FILENAME_NOT_FOUND   -117    /* USER Firmware file not found in the internal table  */
#define DRV_FW_BOOTLOADER_ACTIVE    -118    /* USER Firmware file BOOTLOADER active                */
#define DRV_FW_NO_FILE_PATH         -119    /* USER Firmware file no file path                     */

#define DRV_CF_FILE_OPEN_FAILED     -120    /* USER Configuration file not opend                   */
#define DRV_CF_FILE_SIZE_ZERO       -121    /* USER Configuration file size zero                   */
#define DRV_CF_FILE_NO_MEMORY       -122    /* USER Not enough memory to load configuration file   */
#define DRV_CF_FILE_READ_FAILED     -123    /* USER Configuration file read failed                 */
#define DRV_CF_INVALID_FILETYPE     -124    /* USER Configuration file type invalid                */
#define DRV_CF_FILENAME_INVALID     -125    /* USER Configuration file name not valid              */
#define DRV_CF_DOWNLOAD_ERROR       -126    /* USER Configuration file download error              */
#define DRV_CF_FILE_NO_SEGMENT      -127    /* USER No flash segment in the configuration file     */
#define DRV_CF_DIFFERS_FROM_DBM     -128    /* USER Configuration file differs from database       */

#define DRV_DBM_SIZE_ZERO           -131    /* USER Database size zero                             */
#define DRV_DBM_NO_MEMORY           -132    /* USER Not enough memory to upload database           */
#define DRV_DBM_READ_FAILED         -133    /* USER Database read failed                           */
#define DRV_DBM_NO_FLASH_SEGMENT    -136    /* USER Database segment unknown                       */

#define DEV_CF_INVALID_DESCRIPT_VERSION -150/* CONFIG Version of the descript table invalid        */
#define DEV_CF_INVALID_INPUT_OFFSET     -151/* CONFIG Input offset is invalid                      */
#define DEV_CF_NO_INPUT_SIZE            -152/* CONFIG Input size is 0                              */
#define DEV_CF_MISMATCH_INPUT_SIZE      -153/* CONFIG Input size does not match configuration      */
#define DEV_CF_INVALID_OUTPUT_OFFSET    -154/* CONFIG Invalid output offset                        */
#define DEV_CF_NO_OUTPUT_SIZE           -155/* CONFIG Output size is 0                             */
#define DEV_CF_MISMATCH_OUTPUT_SIZE     -156/* CONFIG Output size does not match configuration     */
#define DEV_CF_STN_NOT_CONFIGURED       -157/* CONFIG Station not configured                       */
#define DEV_CF_CANNOT_GET_STN_CONFIG    -158/* CONFIG Cannot get the Station configuration         */
#define DEV_CF_MODULE_DEF_MISSING       -159/* CONFIG Module definition is missing                 */
#define DEV_CF_MISMATCH_EMPTY_SLOT      -160/* CONFIG Empty slot mismatch                          */
#define DEV_CF_MISMATCH_INPUT_OFFSET    -161/* CONFIG Input offset mismatch                        */
#define DEV_CF_MISMATCH_OUTPUT_OFFSET   -162/* CONFIG Output offset mismatch                       */
#define DEV_CF_MISMATCH_DATA_TYPE       -163/* CONFIG Data type mismatch                           */
#define DEV_CF_MODULE_DEF_MISSING_NO_SI -164/* CONFIG Module definition is missing,(no Slot/Idx)   */

#define DRV_RCS_ERROR_OFFSET       1000     /* RCS error number start                              */

/* ------------------------------------------------------------------------------------ */
/*  message definition                                                                  */
/* ------------------------------------------------------------------------------------ */

#pragma pack(1)

/* max. length is 288 Bytes, max message length is 255 + 8 Bytes */
typedef struct tagMSG_STRUC {
  uint8_t   rx;
  uint8_t   tx;
  uint8_t   ln;
  uint8_t   nr;
  uint8_t   a;
  uint8_t   f;
  uint8_t   b;
  uint8_t   e;
  uint8_t   data[255];
  uint8_t   dummy[25];      /* for compatibility with older definitions (288 Bytes) */
} MSG_STRUC;

#pragma pack()

/* ------------------------------------------------------------------------------------ */
/*  INFO structure definitions                                                          */
/* ------------------------------------------------------------------------------------ */

#pragma pack(1)

/* DEVRESET */

#define COLDSTART           2
#define WARMSTART           3
#define BOOTSTART           4

/* DEVMBXINFO */

#define DEVICE_MBX_EMPTY    0
#define DEVICE_MBX_FULL     1
#define HOST_MBX_EMPTY      0
#define HOST_MBX_FULL       1

/* TRIGGERWATCHDOG */

#define WATCHDOG_STOP       0
#define WATCHDOG_START      1

/* GETINFO InfoArea definitions */

#define GET_DRIVER_INFO           1
#define GET_VERSION_INFO          2
#define GET_FIRMWARE_INFO         3
#define GET_TASK_INFO             4
#define GET_RCS_INFO              5
#define GET_DEV_INFO              6
#define GET_IO_INFO               7
#define GET_IO_SEND_DATA          8
#define GET_CIF_PLC_DRIVER_INFO  10

/* HOST mode definition */

#define HOST_NOT_READY      0
#define HOST_READY          1

/* DEVREADWRITERAW / DEVREADWRITEDPMDATA */

#define PARAMETER_READ      1
#define PARAMETER_WRITE     2

/* STATE definition  */

#define STATE_ERR_NON       0
#define STATE_ERR           1

#define STATE_MODE_2        2
#define STATE_MODE_3        3
#define STATE_MODE_4        4

/* DEVSPECIALCONTROL */

#define SPECIAL_CONTROL_CLEAR       0
#define SPECIAL_CONTROL_SET         1

/* DEVDOWNLOAD */

#define FIRMWARE_DOWNLOAD           1
#define CONFIGURATION_DOWNLOAD      2

// DEVHWIOPORT
#define HW_PORT_SET_OUTPUT        1
#define HW_PORT_CLEAR_OUTPUT      2
#define HW_PORT_READ_INPUT        3

/* DEVMEMORYPTR */
#define MEMORY_PTR_CREATE         1
#define MEMORY_PTR_RELEASE        2


/* ------------------------------------------------------------------------------------ */
/*  INFO structure definitions                                                          */
/* ------------------------------------------------------------------------------------ */
/* Device exchange IO information */
typedef struct tagIOINFO {
  uint8_t   bComBit;                /* Actual state of the COM bit                */
  uint8_t   bIOExchangeMode;        /* Actual data exchange mode (0..5)           */
  uint32_t   ulIOExchangeCnt;        /* Exchange IO counter                        */
} IOINFO;

/* Device version information */
typedef struct tagVERSIONINFO {           /* Device serial number and OS versions       */
  uint32_t   ulDate;                 
  uint32_t   ulDeviceNo;
  uint32_t   ulSerialNo;
  uint32_t   ulReserved;
  uint8_t    abPcOsName0[4];
  uint8_t    abPcOsName1[4];
  uint8_t    abPcOsName2[4];
  uint8_t    abOemIdentifier[4];
} VERSIONINFO;

/* Device firmware information */
typedef struct tagFIRMWAREINFO {
  uint8_t   abFirmwareName[16];     /* Firmware name                              */
  uint8_t   abFirmwareVersion[16];  /* Firmware version                           */
} FIRMWAREINFO;

/* Device task state information */
typedef struct tagTASKSTATE {
  uint8_t   abTaskState[64];        /* Task state field                           */
} TASKSTATE;

/* Device task paramater data */
typedef struct tagTASKPARAM {
  uint8_t   abTaskParameter[64];    /* Task parameter field                       */
} TASKPARAM;

/* Device raw data structure */
typedef struct tagRAWDATA {
  uint8_t   abRawData[1022];        /* Definition of the last kByte               */
} RAWDATA;

/* Device task information */
typedef struct tagTASKINFO {
  struct  {
    uint8_t  abTaskName[8];         /* Task name                                  */
    uint16_t usTaskVersion;         /* Task version                               */
    uint8_t  bTaskCondition;        /* Actual task condition                      */
    uint8_t  abreserved[5];         /* n.c.                                       */
  } tTaskInfo [7];
} TASKINFO;

/* Device operating system (RCS) information */
typedef struct tagRCSINFO {
  uint16_t usRcsVersion;            /* Device operating system (RCS) version      */
  uint8_t  bRcsError;               /* Operating system errors                    */
  uint8_t  bHostWatchDog;           /* Host watchdog value                        */
  uint8_t  bDevWatchDog;            /* Device watchdog value                      */
  uint8_t  bSegmentCount;           /* RCS segment free counter                   */
  uint8_t  bDeviceAdress;           /* RCS device base address                    */
  uint8_t  bDriverType;             /* RCS driver type                            */
} RCSINFO;

/* Device description */
typedef struct tagDEVINFO {
  uint8_t  bDpmSize;                /* Device dpm size (2,8...)                   */
  uint8_t  bDevType;                /* Device type  (manufactor code)             */
  uint8_t  bDevModel;               /* Device model (manufactor code)             */
  uint8_t  abDevIdentifier[3];      /* Device identification characters           */
} DEVINFO;

#pragma pack()

/* ------------------------------------------------------------------------------------ */
/*  driver info structure definitions                                                   */
/* ------------------------------------------------------------------------------------ */

#pragma pack(1)

/* Board information structure */
typedef struct tagBOARD {
  uint16_t usBoardNumber;       /* DRV board number                               */
  uint16_t usAvailable;         /* DRV board is available                         */
  uint32_t  ulPhysicalAddress;   /* DRV physical DPM address                       */
  uint16_t usIrqNumber;         /* DRV irq number                                 */
} BOARD;
typedef struct tagBOARD_INFO{
  uint8_t abDriverVersion[16];  /* DRV driver information string                  */
  BOARD         tBoard [MAX_DEV_BOARDS];
} BOARD_INFO;

/* Internal driver state information structure */
typedef struct tagDRIVERINFO{
  uint32_t ulOpenCnt;            /* DevOpen() counter                              */
  uint32_t ulCloseCnt;           /* DevClose() counter                             */
  uint32_t ulReadCnt;            /* Number of DevGetMessage commands               */
  uint32_t ulWriteCnt;           /* Number of DevPutMessage commands               */
  uint32_t ulIRQCnt;             /* Number of board interrupts                     */
  uint8_t  bInitMsgFlag;         /* Actual init sate                               */
  uint8_t  bReadMsgFlag;         /* Actual read mailbox state                      */
  uint8_t  bWriteMsgFlag;        /* Actual write mailbox state                     */
  uint8_t  bLastFunction;        /* Last driver function                           */
  uint8_t  bWriteState;          /* Actual write command state                     */
  uint8_t  bReadState;           /* Actual read command state                      */
  uint8_t  bHostFlags;           /* Actual host flags                              */
  uint8_t  bMyDevFlags;          /* Actual device falgs                            */
  uint8_t  bExIOFlag;            /* Actual IO flags                                */
  uint32_t ulExIOCnt;            /* DevExchangeIO() counter                        */
} DRIVERINFO;

/* Extended board information structure */#
typedef struct tagBOARD_EX {
  uint16_t  usBoardNumber;      /* DRV board number                               */
  uint16_t  usAvailable;        /* DRV board is available                         */
  uint32_t  ulPhysicalAddress;  /* DRV physical DPM address                       */
  uint16_t  usIrqNumber;        /* DRV irq number                                 */
  DRIVERINFO      tDriverInfo;        /* Driver information                             */
  FIRMWAREINFO    tFirmware;
  DEVINFO         tDeviceInfo;
  RCSINFO         tRcsInfo;
  VERSIONINFO     tVersion;
} BOARD_EX;

typedef struct tagBOARD_INFOEX{
  uint8_t     abDriverVersion[16];    /* DRV driver information string            */
  BOARD_EX    tBoard [MAX_DEV_BOARDS];
} BOARD_INFOEX;

/* Communication state field structure */ 
typedef struct tagCOMSTATE {
  uint16_t    usMode;           /* Actual STATE mode                              */
  uint16_t    usStateFlag;      /* State flag                                     */
  uint8_t     abState[64];      /* State area                                     */
} COMSTATE;

/* State information in bLastFunction */
#define  FKT_OPEN       1
#define  FKT_CLOSE      2
#define  FKT_READ       3
#define  FKT_WRITE      4
#define  FKT_IO         5
/* State information in bWriteState and bReadState */
#define  STATE_IN       0x01
#define  STATE_WAIT     0x02
#define  STATE_OUT      0x03
#define  STATE_IN_IRQ   0x04

#pragma pack()

/* ------------------------------------------------------------------------------------ */
/*  CIF100 special function definitions                                                 */
/* ------------------------------------------------------------------------------------ */
#pragma pack(1)

/*--------------------------*/
/* Device DMA download      */
/*--------------------------*/
#define DEV_DMA_DOWN_FW     1
#define DEV_DMA_DOWN_DB     2

/*--------------------------*/
/* Device BACKUP RAM        */
/*--------------------------*/
#define DEV_BACKUP_RAM_READ       1
#define DEV_BACKUP_RAM_WRITE      2

/*--------------------------*/
/* Device Timer             */
/*--------------------------*/
#define DEV_TIMER_CTRL_START        1
#define DEV_TIMER_CTRL_STOP         2
#define DEV_TIMER_MODE_NORMAL       1
#define DEV_TIMER_MODE_NMI          2
#define DEV_TIMER_RESOLUTION_100US  0
#define DEV_TIMER_RESOLUTION_1MS    1

/*----------------------------*/
/* PLC information structure  */
/*----------------------------*/
typedef struct tagCIF_PLC_DRIVER_INFO {
  int8_t*      pbInput;
  uint32_t     ulInputSize;
  int8_t*      pbOutput;
  uint32_t     ulOutputSize;
  TASKSTATE*   ptTaskState;   /* Take this pointer and cast it to the protocol spez. structure */
  uint32_t     ulNVRAMSize;
  int8_t*      pbNVRAM;
} CIF_PLC_DRIVER_INFO;

#pragma pack()

/* ------------------------------------------------------------------------------------ */
/*  Configuration file definition                                                       */
/* ------------------------------------------------------------------------------------ */
/* Descript tabel version definition */
#define RECORD_TYPE_LESS_THEN_2300       0x01         
#define RECORD_TYPE_EQUAL_HIGHER_2300    0x02         

/* ------------------------------------------------------------------------------------ */
/*  Function prototypes                                                                 */
/* ------------------------------------------------------------------------------------ */
#ifndef CIF_TKIT
  int16_t APIENTRY DevOpenDriver        ( uint16_t usDevNumber);

  int16_t APIENTRY DevCloseDriver       ( uint16_t usDevNumber);

  int16_t APIENTRY DevGetBoardInfo      ( uint16_t usDevNumber,
                                          uint16_t usSize,
                                          void           *pvData);

  int16_t APIENTRY DevInitBoard         ( uint16_t usDevNumber,
                                          void           *pDevAddress);

  int16_t APIENTRY DevExitBoard         ( uint16_t usDevNumber);

  int16_t APIENTRY DevPutTaskParameter  ( uint16_t usDevNumber,
                                          uint16_t usNumber,
                                          uint16_t usSize,
                                          void           *pvData);

  int16_t APIENTRY DevReset             ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint32_t  ulTimeout);

  int16_t APIENTRY DevPutMessage        ( uint16_t usDevNumber,
                                          MSG_STRUC      *ptMessage,
                                          uint32_t  ulTimeout);

  int16_t APIENTRY DevGetMessage        ( uint16_t usDevNumber,
                                          uint16_t usSize,
                                          MSG_STRUC* ptMessage,
                                          uint32_t  ulTimeout);

  int16_t APIENTRY DevGetTaskState      ( uint16_t usDevNumber,
                                          uint16_t usNumber,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevGetMBXState       ( uint16_t  usDevNumber,
                                          uint16_t* pusDevMBXState,
                                          uint16_t* pusHostMBXState);

  int16_t APIENTRY DevTriggerWatchDog   ( uint16_t  usDevNumber,
                                          uint16_t  usFunction,
                                          uint16_t* usDevWatchDog);

  int16_t APIENTRY DevGetInfo           ( uint16_t usDevNumber,
                                          uint16_t usFunction,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevGetTaskParameter  ( uint16_t usDevNumber,
                                          uint16_t usNumber,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevExchangeIO        ( uint16_t usDevNumber,
                                          uint16_t usSendOffset,
                                          uint16_t usSendSize,
                                          void*    pvSendData,
                                          uint16_t usReceiveOffset,
                                          uint16_t usReceiveSize,
                                          void*    pvReceiveData,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevReadSendData      ( uint16_t usDevNumber,
                                          uint16_t usOffset,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevSetHostState      ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevExtendedData      ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevGetMBXData        ( uint16_t usDevNumber,
                                          uint16_t usHostSize,
                                          void*    pvHostData,
                                          uint16_t usDevSize,
                                          void*    pvDevData);

  int16_t APIENTRY DevGetBoardInfoEx    ( uint16_t usDevNumber,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevExchangeIOEx      ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint16_t usSendOffset,
                                          uint16_t usSendSize,
                                          void*    pvSendData,
                                          uint16_t usReceiveOffset,
                                          uint16_t usReceiveSize,
                                          void*    pvReceiveData,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevExchangeIOErr     ( uint16_t  usDevNumber,
                                          uint16_t  usSendOffset,
                                          uint16_t  usSendSize,
                                          void*     pvSendData,
                                          uint16_t  usReceiveOffset,
                                          uint16_t  usReceiveSize,
                                          void*     pvReceiveData,
                                          COMSTATE* ptState,
                                          uint32_t  ulTimeout);

  int16_t APIENTRY DevReadWriteDPMRaw   ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint16_t usOffset,
                                          uint16_t usSize,
                                          void*    pvData);

  int16_t APIENTRY DevSpecialControl    ( uint16_t  usDevNumber,
                                          uint16_t  usMode,
                                          uint16_t* pusCtrlAck);

  int16_t APIENTRY DevDownload          ( uint16_t  usDevNumber,
                                          uint16_t  usMode,
                                          uint8_t*  pszFileName,
                                          uint32_t* pulBytes);

  int16_t APIENTRY DevReadWriteDPMData  ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint16_t usOffset,
                                          uint16_t usSize,
                                          void*    pvData);

  /*-------------------*/
  /* Special functions */
  /*-------------------*/
  int16_t APIENTRY DevGetDPMPtr         ( uint16_t  usMode,
                                          uint16_t  usDevNumber,
                                          void*     pvUserData,
                                          uint32_t* pulDPMSize,
                                          uint8_t** pDPMBase,
                                          int32_t*  plError);

  /*---------------------------*/
  /* CIF100 spezific functions */
  /*---------------------------*/
  int16_t APIENTRY DevPerformance       ( void *ptData);
  int16_t APIENTRY DevRAMTest           ( void *ptData);

  int16_t APIENTRY DevTimer             ( uint16_t usDevNumber,
                                          uint16_t usControl,
                                          uint16_t usMode,
                                          uint16_t usResolution,
                                          int8_t   bTickCount);

  int16_t APIENTRY DevBackupRAM         ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint32_t ulOffset,
                                          uint32_t ulSize,
                                          void*    pvData);

  int16_t APIENTRY DevRAMrw             ( uint16_t usDevNumber,
                                          uint32_t ulDevStartAdd,
                                          uint16_t usDataLen,
                                          void*    pvSendData,
                                          void*    pvReceiveData,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevDMAdown           ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevClearConfig       ( uint16_t usDevNumber);

  int16_t APIENTRY DevIsPLCDataReady    ( uint16_t  usDevNumber,
                                          uint16_t* pusState);

  int16_t APIENTRY DevExchangePLCData   ( uint16_t usDevNumber,
                                          uint16_t usSendSize,
                                          uint16_t usReceiveSize,
                                          uint32_t ulTimeout);

  int16_t APIENTRY DevHWPortControl     ( uint16_t  usDevNumber,
                                          uint16_t  usMode,
                                          uint16_t* pusState);

  int16_t APIENTRY DevMemoryPtr         ( uint16_t usDevNumber,
                                          uint16_t usMode,
                                          uint32_t  usSize,
                                          CIF_PLC_DRIVER_INFO* ptPLCData);


#endif  /* !Toolkit definition */

#ifdef __cplusplus
}
#endif

#endif  /* __CIFUSER_H */
