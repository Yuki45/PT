using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;


namespace NeptuneC_Interface
{
    // enumerations
    public enum ENeptuneError
    {
        NEPTUNE_ERR_Fail = -1,
        NEPTUNE_ERR_Success = 0,
        NEPTUNE_ERR_AlreadyInitialized = -100,
        NEPTUNE_ERR_APINotInitialized = -101,
        NEPTUNE_ERR_NotInitialized = -102,
        NEPTUNE_ERR_GC = -103,
        NEPTUNE_ERR_TimeOut = -104,
        NEPTUNE_ERR_TLInitFail = -200,
        NEPTUNE_ERR_NoInterface = -201,
        NEPTUNE_ERR_DeviceCheck = -202,
        NEPTUNE_ERR_InvalidParameter = -203,
        NEPTUNE_ERR_NotSupport = -204,
        NEPTUNE_ERR_AccessDenied = -205,
        NEPTUNE_ERR_InvalidAddress = -206,
        NEPTUNE_ERR_InvalidArraySize = -207,
        NEPTUNE_ERR_Interface = -208,
        NEPTUNE_ERR_DeviceInfo = -209,
        NEPTUNE_ERR_MemoryAlloc = -210,
        NEPTUNE_ERR_DeviceOpen = -211,
        NEPTUNE_ERR_DevicePort = -212,
        NEPTUNE_ERR_DeviceURL = -213,
        NEPTUNE_ERR_DeviceWrite = -214,
        NEPTUNE_ERR_DeviceXML = -215,
        NEPTUNE_ERR_DeviceHeartbeat = -216,
        NEPTUNE_ERR_DeviceClose = -217,
        NEPTUNE_ERR_DeviceStream = -218,
        NEPTUNE_ERR_DeviceNotStreaming = -219,
        // XML
        NEPTUNE_ERR_InvalidXMLNode = -300,
        NEPTUNE_ERR_StreamCount = -303,
        NEPTUNE_ERR_AccessTimeOut = -304,
        NEPTUNE_ERR_OutOfRange = -305,
        NEPTUNE_ERR_InvalidChannel = -306,
        NEPTUNE_ERR_InvalidBuffer = -307,
        // File
        NEPTUNE_ERR_FileAccessError = -400,
    };

    public enum ENeptuneBoolean
    {
        NEPTUNE_BOOL_FALSE = 0,
        NEPTUNE_BOOL_TRUE = 1
    };

    public enum ENeptuneEffect
    {
        NEPTUNE_EFFECT_NONE = 0,
        NEPTUNE_EFFECT_FLIP = 0x01,		// flip image
        NEPTUNE_EFFECT_MIRROR = 0x02,		// mirror
        NEPTUNE_EFFECT_NEGATIVE = 0x04		// negative
    } ;

    public enum ENeptuneAutoMode
    {
        NEPTUNE_AUTO_OFF = 0,		// manual mode
        NEPTUNE_AUTO_ONCE = 1,		// once(one-shot) mode
        NEPTUNE_AUTO_CONTINUOUS = 2,		// auto mode
    };

    public enum ENeptunePixelFormat
    {
        Unknown_PixelFormat = -1,
        // 1394 Camera pixel format list.
        Format0_320x240_YUV422 = 0,
        Format0_640x480_YUV411 = 1,
        Format0_640x480_YUV422 = 2,
        Format0_640x480_Mono8 = 3,
        Format0_640x480_Mono16 = 4,
        Format1_800x600_YUV422 = 5,
        Format1_800x600_Mono8 = 6,
        Format1_1024x768_YUV422 = 7,
        Format1_1024x768_Mono8 = 8,
        Format1_800x600_Mono16 = 9,
        Format1_1024x768_Mono16 = 10,

        Format2_1280x960_YUV422 = 11,
        Format2_1280x960_Mono8 = 12,
        Format2_1600x1200_YUV422 = 13,
        Format2_1600x1200_Mono8 = 14,
        Format2_1280x960_Mono16 = 15,
        Format2_1600x1200_Mono16 = 16,

        Format7_Mode0_Mono8 = 17,
        Format7_Mode0_YUV411 = 18,
        Format7_Mode0_YUV422 = 19,
        Format7_Mode0_Mono16 = 20,
        Format7_Mode0_Raw8 = 21,
        Format7_Mode0_Raw16 = 22,
        Format7_Mode0_Mono12 = 23,
        Format7_Mode0_Raw12 = 24,

        Format7_Mode1_Mono8 = 25,
        Format7_Mode1_YUV411 = 26,
        Format7_Mode1_YUV422 = 27,
        Format7_Mode1_Mono16 = 28,
        Format7_Mode1_Raw8 = 29,
        Format7_Mode1_Raw16 = 30,
        Format7_Mode1_Mono12 = 31,
        Format7_Mode1_Raw12 = 32,

        Format7_Mode2_Mono8 = 33,
        Format7_Mode2_YUV411 = 34,
        Format7_Mode2_YUV422 = 35,
        Format7_Mode2_Mono16 = 36,
        Format7_Mode2_Raw8 = 37,
        Format7_Mode2_Raw16 = 38,
        Format7_Mode2_Mono12 = 39,
        Format7_Mode2_Raw12 = 40,

        // GigE Camera pixel format list.
        Mono8 = 101,
        Mono10 = 102,
        Mono12 = 103,
        Mono16 = 104,
        BayerGR8 = 105,
        BayerGR10 = 106,
        BayerGR12 = 107,
        YUV411Packed = 108,
        YUV422Packed = 109
    };

    public enum ENeptuneFrameRate
    {
        FPS_UNKNOWN = -1,
        FPS_1_875 = 0,			// 1.875 frame (1394 camera)
        FPS_3_75 = 1,			// 3.75 frame (1394 camera)
        FPS_7_5 = 2,			// 7.5 frame (1394 camera)
        FPS_15 = 3,			// 15 frame (1394 camera)
        FPS_30 = 4,			// 30 frame (1394 camera)
        FPS_60 = 5,			// 60 frame (1394 camera)
        FPS_120 = 6,			// 120 frame (1394 camera)
        FPS_240 = 7,			// 240 frame (1394 camera)
        FPS_VALUE = 20			// frame rate value(GigE camera)
    };

    public enum ENeptunePixelType
    {
        NEPTUNE_PIXEL_MONO = 1,
        NEPTUNE_PIXEL_BAYER = 2,
        NEPTUNE_PIXEL_RGB = 3,
        NEPTUNE_PIXEL_YUV = 4,
        NEPTUNE_PIXEL_RGBPLANAR = 5
    };

    public enum ENeptuneBayerLayout
    {
        NEPTUNE_BAYER_GB_RG = 0,		// GB/RG layout
        NEPTUNE_BAYER_BG_GR = 1,		// BG/GR layout
        NEPTUNE_BAYER_RG_GB = 2,		// RG/GB layout
        NEPTUNE_BAYER_GR_BG = 3,		// GR/BG layout
    };


    public enum ENeptuneBayerMethod
    {
        NEPTUNE_BAYER_METHOD_NONE = 0,			// no bayer conversion
        NEPTUNE_BAYER_METHOD_BILINEAR = 1,		// bilinear conversion
        NEPTUNE_BAYER_METHOD_HQ = 2,			// HQ conversion
        NEPTUNE_BAYER_METHOD_NEAREST = 3		// nearest conversion
    };


    public enum ENeptuneAcquisitionMode
    {
        NEPTUNE_ACQ_CONTINUOUS = 0,			// continuous
        NEPTUNE_ACQ_MULTIFRAME = 1,			// multi frame
        NEPTUNE_ACQ_SINGLEFRAME = 2,			// single frame
    };


    public enum ENeptuneStreamMode
    {
        NEPTUNE_STRM_UNICAST = 0,
        NEPTUNE_STRM_MULTICAST = 1
    };


    public enum ENeptuneImageFormat
    {
        NEPTUNE_IMAGE_FORMAT_BMP = 0,
        NEPTUNE_IMAGE_FORMAT_JPG = 1,
        NEPTUNE_IMAGE_FORMAT_TIF = 2
    };


    public enum ENeptuneGrabFormat
    {
        NEPTUNE_GRAB_RAW = 0,
        NEPTUNE_GRAB_RGB = 1
    };

    public enum ENeptuneDeviceChangeState
    {
        NEPTUNE_DEVICE_ADDED = 0,	// camera count is increased
        NEPTUNE_DEVICE_REMOVED = 1		// camera count is decreased
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_IMAGE_SIZE
    {
        public Int32 nStartX;	// start point of X coordinate(width direction)
        public Int32 nStartY;	// start point of Y coordinate(height direction)
        public Int32 nSizeX;		// width 
        public Int32 nSizeY;		// height
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_IMAGE
    {
        public UInt32 uiWidth;		// image data width
        public UInt32 uiHeight;		// image data height
        public UInt32 uiBitDepth;		// data bits per pixel
        public IntPtr pData;
        public UInt32 uiSize;			// buffer length
        public UInt32 uiIndex;		// buffer index
        public UInt64 uiTimestamp;	// data timestamp
    };



    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for XML node handling
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneNodeType
    {
        NEPTUNE_NODE_TYPE_UKNOWN = -1,
        NEPTUNE_NODE_TYPE_CATEGORY = 0,
        NEPTUNE_NODE_TYPE_COMMAND,				// command type node
        NEPTUNE_NODE_TYPE_RAW,					// raw node
        NEPTUNE_NODE_TYPE_STRING,				// string node
        NEPTUNE_NODE_TYPE_ENUM,					// enumeration node
        NEPTUNE_NODE_TYPE_INT,					// int type node
        NEPTUNE_NODE_TYPE_FLOAT,				// float type node
        NEPTUNE_NODE_TYPE_BOOLEAN				// boolean type node
    };

    public enum ENeptuneNodeAccessMode
    {
        NEPTUNE_NODE_ACCESSMODE_NI = 0,		// Not Implemented
        NEPTUNE_NODE_ACCESSMODE_NA = 1,		// Not Available
        NEPTUNE_NODE_ACCESSMODE_WO = 2,		// Write Only
        NEPTUNE_NODE_ACCESSMODE_RO = 3,		// Read Only
        NEPTUNE_NODE_ACCESSMODE_RW = 4,		// Read, Write
        NEPTUNE_NODE_ACCESSMODE_UNDEFINED = 5			// undefined
    };

    public enum ENeptuneNodeVisibility
    {
        NEPTUNE_NODE_VISIBLE_UNKNOWN = -1,
        NEPTUNE_NODE_VISIBLE_BEGINNER = 0,		// beginner
        NEPTUNE_NODE_VISIBLE_EXPERT = 1,		// expert
        NEPTUNE_NODE_VISIBLE_GURU = 2			// guru
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_XML_NODE_INFO
    {
        public ENeptuneNodeType Type;			// node type(int, float, boolean, string, enumeration, command)
        public ENeptuneNodeAccessMode AccessMode;		// access mode
        public ENeptuneNodeVisibility Visibility;		// node visibility
        public Byte bHasChild;	// has child ?

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strDisplayName;	// node name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strTooltip;		// node tooltip
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strDescription;	// node description
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_XML_INT_VALUE_INFO
    {
        public Int64 nValue;				// current value
        public Int64 nMin;				// minimum value supported
        public Int64 nMax;				// maximum value supported
        public Int64 nInc;				// increment step
        public ENeptuneNodeAccessMode AccessMode; 		// access mode
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_XML_FLOAT_VALUE_INFO
    {
        public Double dValue;			// current value
        public Double dMin;			// minimum
        public Double dMax;			// maximum
        public Double dInc;			// increment
        public ENeptuneNodeAccessMode AccessMode;
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for Device Control
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneDevType
    {
        NEPTUNE_DEV_TYPE_UNKNOWN = -1,	// unknown camera
        NEPTUNE_DEV_TYPE_GIGE = 0,	// GigE camera
        NEPTUNE_DEV_TYPE_1394 = 1,	// 1394 camera
        NEPTUNE_DEV_TYPE_USB3 = 2,
    };


    public enum ENeptuneDevAccess
    {
        NEPTUNE_DEV_ACCESS_UNKNOWN = -1,
        NEPTUNE_DEV_ACCESS_EXCLUSIVE = 0,
        NEPTUNE_DEV_ACCESS_CONTROL = 1,
        NEPTUNE_DEV_ACCESS_MONITOR = 2
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NEPTUNE_CAM_INFO
    {
        //_char_t				strVendor[MAX_STRING_LENGTH];	// camera vendor name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strVendor;	// camera vendor name

        //_char_t				strModel[MAX_STRING_LENGTH];	// camera model name
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strModel;	// camera model name

        //_char_t				strSerial[MAX_STRING_LENGTH];	// camera serial number
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strSerial;	// camera serial number

        //_char_t				strUserID[MAX_STRING_LENGTH];	// user ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strUserID;	// user ID

        //_char_t				strIP[MAX_STRING_LENGTH];		// IP address(GigE camera)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strIP;		// IP address(GigE camera)

        //_char_t				strMAC[MAC_LENGTH];				// MAC address(GigE camera)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string strMAC;				// MAC address(GigE camera)

        //_char_t				strSubnet[MAX_STRING_LENGTH];	// Subnet address(GigE camera)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strSubnet;	// Subnet address(GigE camera)

        //_char_t				strGateway[MAX_STRING_LENGTH];	// Gateway address(GigE camera)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strGateway;	// Gateway address(GigE camera)

        //_char_t				strCamID[MAX_STRING_LENGTH];	// unique Camera ID
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string strCamID;	// unique Camera ID
    };



    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for trigger & strobe
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneTriggerSource
    {
        NEPTUNE_TRIGGER_SOURCE_LINE1 = 0,	// external(H/W trigger)
        NEPTUNE_TRIGGER_SOURCE_SW = 7		// software trigger
    };


    public enum ENeptuneTriggerMode
    {
        NEPTUNE_TRIGGER_MODE_0 = 0,		// trigger mode 0
        NEPTUNE_TRIGGER_MODE_1,			// trigger mode 1
        NEPTUNE_TRIGGER_MODE_2,			// trigger mode 2
        NEPTUNE_TRIGGER_MODE_3,			// trigger mode 3
        NEPTUNE_TRIGGER_MODE_4,			// trigger mode 4
        NEPTUNE_TRIGGER_MODE_5,			// trigger mode 5
        NEPTUNE_TRIGGER_MODE_6,			// trigger mode 6
        NEPTUNE_TRIGGER_MODE_7,			// trigger mode 7
        NEPTUNE_TRIGGER_MODE_8,			// trigger mode 8
        NEPTUNE_TRIGGER_MODE_9,			// trigger mode 9
        NEPTUNE_TRIGGER_MODE_10,		// trigger mode 10
        NEPTUNE_TRIGGER_MODE_11,		// trigger mode 11
        NEPTUNE_TRIGGER_MODE_12,		// trigger mode 12
        NEPTUNE_TRIGGER_MODE_13,		// trigger mode 13
        NEPTUNE_TRIGGER_MODE_14,		// trigger mode 14
        NEPTUNE_TRIGGER_MODE_15			// trigger mode 15
    };


    public enum ENeptunePolarity
    {
        NEPTUNE_POLARITY_RISINGEDGE = 0,	// rising edge
        NEPTUNE_POLARITY_FALLINGEDGE = 1,	// falling edge
        NEPTUNE_POLARITY_ANYEDGE = 2,	// any edge
        NEPTUNE_POLARITY_LEVELHIGH = 3,	// high level
        NEPTUNE_POLARITY_LEVELLOW = 4		// low level
    };


    public enum ENeptuneStrobe
    {
        NEPTUNE_STROBE0 = 0,
        NEPTUNE_STROBE1 = 1,
        NEPTUNE_STROBE2 = 2,
        NEPTUNE_STROBE3 = 3,
        NEPTUNE_STROBE4 = 4,
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_TRIGGER_INFO
    {
        public ENeptuneBoolean bSupport;		// trigger support flag
        public UInt16 nModeFlag;		// bit mask for trigger mode
        public UInt16 nSourceFlag;	// bit mask for trigger source (bit0 = H/W, bit7 = S/W)
        public UInt16 nPolarityFlag;	// bit mask for polarity
        public UInt16 nParamMin;		// trigger parameter minimum value
        public UInt16 nParamMax;		// trigger parameter maximum value
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_TRIGGER
    {
        public ENeptuneTriggerSource Source;		// trigger source value
        public ENeptuneTriggerMode Mode;		// trigger mode value
        public ENeptunePolarity Polarity;	// trigger polarity value
        public ENeptuneBoolean OnOff;		// trigger on/off
        public UInt16 nParam;		// trigger parameter
    };


    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct NEPTUNE_TRIGGER_PARAM
    {
        public UInt32 nFrameOrder;		// frame sequence number
        public UInt32 nIncrement;			// end of table(0) or continuous(1)
        public UInt32 nGainValue;			// gain feature value
        public UInt32 nShutterValue;		// shutter feature value
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_TRIGGER_TABLE
    {
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 255)]
        public NEPTUNE_TRIGGER_PARAM[] Param;	// trigger parameter : max 255
        public UInt32 Index;						// 0 ~ 15
    };


    ////////////////////// strobe //////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_STROBE_INFO
    {
        public ENeptuneBoolean bSupport;		// support of strobe
        public UInt16 nStrobeFlag;	// support strobes bit flag
        public UInt16 nPolarityFlag;	// strobe polarity support bit flag
        public UInt16 nDurationMin;	// strobe duration minimum value
        public UInt16 nDurationMax;	// strobe duration maximum value
        public UInt16 nDelayMin;		// strobe delay minimum value
        public UInt16 nDelayMax;		// strobe delay maximum value
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_STROBE
    {
        public ENeptuneBoolean OnOff;		// strobe on/off control
        public ENeptuneStrobe Strobe;		// strobe index
        public UInt16 nDuration;	// strobe duration value
        public UInt16 nDelay;		// strobe delay value
        public ENeptunePolarity Polarity;	// strobe polarity
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for GPIO
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneGPIO
    {
        NEPTUNE_GPIO_LINE0 = 0,		// GPIO 0
        NEPTUNE_GPIO_LINE1			// GPIO 1
    };


    public enum ENeptuneGPIOSource
    {
        NEPTUNE_GPIO_SOURCE_STROBE = 0,	// strobe
        NEPTUNE_GPIO_SOURCE_USER		// user defined
    };


    public enum ENeptuneGPIOValue
    {
        NEPTUNE_GPIO_VALUE_LOW = 0,		// low level
        NEPTUNE_GPIO_VALUE_HIGH			// high level
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_GPIO
    {
        public ENeptuneGPIO Gpio;		// GPIO index
        public ENeptuneGPIOSource Source;		// GPIO source
        public ENeptuneGPIOValue Value;		// GPIO value
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for feature
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneFeature
    {
        NEPTUNE_FEATURE_GAMMA = 0,	// AnalogControls, Gamma
        NEPTUNE_FEATURE_GAIN = 1,	// AnalogControls, Gain or GainRaw
        NEPTUNE_FEATURE_RGAIN = 2,	// AnalogControls, Gain or GainRaw
        NEPTUNE_FEATURE_GGAIN = 3,	// AnalogControls, Gain or GainRaw
        NEPTUNE_FEATURE_BGAIN = 4,	// AnalogControls, Gain or GainRaw
        NEPTUNE_FEATURE_BLACKLEVEL = 5,	// AnalogControls, BlackLevel or BlackLevelRaw
        NEPTUNE_FEATURE_SHARPNESS = 6,	// AnalogControls, Sharpness or SharpnessRaw
        NEPTUNE_FEATURE_SATURATION = 7,	// AnalogControls, Saturation or SaturationRaw
        NEPTUNE_FEATURE_AUTOEXPOSURE = 8,	// AcquisitionControl, AutoExposure
        NEPTUNE_FEATURE_SHUTTER = 9,	// AcquisitionControl, ExposureTime
        NEPTUNE_FEATURE_HUE = 10,	// AnalogControls, Hue or HueRaw
        NEPTUNE_FEATURE_PAN = 11,	// AcquisitionControl, PanCtrl
        NEPTUNE_FEATURE_TILT = 12,	// AcquisitionControl, TiltCtrl
        NEPTUNE_FEATURE_OPTFILTER = 13,	// AnalogControls, OpticalFilter
        NEPTUNE_FEATURE_AUTOSHUTTER_MIN = 14,	// CustomControl, AutoShutterSpeedMin
        NEPTUNE_FEATURE_AUTOSHUTTER_MAX = 15,	// CustomControl, AutoShutterSpeedMin
        NEPTUNE_FEATURE_AUTOGAIN_MIN = 16,	// CustomControl, AutoGainMin
        NEPTUNE_FEATURE_AUTOGAIN_MAX = 17,	// CustomControl, AutoGainMax
        NEPTUNE_FEATURE_TRIGNOISEFILTER = 18,	// CustomControl, TriggerNoiseFilter
        NEPTUNE_FEATURE_BRIGHTLEVELIRIS = 19,	// CustomControl, BrightLevelForIRIS
        NEPTUNE_FEATURE_SNOWNOISEREMOVE = 20,	// CustomControl, SnowNosieRemoveControl
        NEPTUNE_FEATURE_WATCHDOG = 21,	// CustomControl, WDGControl
        NEPTUNE_FEATURE_WHITEBALANCE = 22,	// AnalogControls, BalanceWhiteAudo
        NEPTUNE_FEATURE_CONTRAST = 23,	// CustomControl, Contrast
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_FEATURE
    {
        public ENeptuneBoolean bSupport;
        public ENeptuneBoolean bOnOff;				// on/off state, on/off control(SnowNoiseRemove only)
        //_uint8_t				SupportAutoModes;	// bit flag for support(bit0:Off, bit1:Once, bit2:Continuous)
        public Byte SupportAutoModes;	// bit flag for support(bit0:Off, bit1:Once, bit2:Continuous)
        public ENeptuneAutoMode AutoMode;			// current Auto mode, valid only when "SuporeAutoMode != 0"
        public Int32 Min;				// minimum value
        public Int32 Max;				// maximum value
        public Int32 Inc;				// increment step
        public Int32 Value;				// current value
        public ENeptuneNodeAccessMode ValueAccessMode;	// access state of the value		
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for UserSet
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneUserSet
    {
        NEPTUNE_USERSET_DEFAULT = 0,
        NEPTUNE_USERSET_1 = 1,
        NEPTUNE_USERSET_2 = 2,
        NEPTUNE_USERSET_3 = 3,
        NEPTUNE_USERSET_4 = 4,
        NEPTUNE_USERSET_5 = 5,
        NEPTUNE_USERSET_6 = 6,
        NEPTUNE_USERSET_7 = 7,
        NEPTUNE_USERSET_8 = 8,
        NEPTUNE_USERSET_9 = 9,
        NEPTUNE_USERSET_10 = 10,
        NEPTUNE_USERSET_11 = 11,
        NEPTUNE_USERSET_12 = 12,
        NEPTUNE_USERSET_13 = 13,
        NEPTUNE_USERSET_14 = 14,
        NEPTUNE_USERSET_15 = 15
    };


    public enum ENeptuneUserSetCommand
    {
        NEPTUNE_USERSET_CMD_LOAD = 0,
        NEPTUNE_USERSET_CMD_SAVE = 1,
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_USERSET
    {
        public UInt16 SupportUserSet;		// bit flag for supported user set, 0 is "Default"
        public ENeptuneUserSet UserSetIndex;		// user set index to save or load
        public ENeptuneUserSetCommand Command;			// save or load
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for Auto Iris
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneAutoIrisMode
    {
        NEPTUNE_AUTOIRIS_MODE_MANUAL = 0,
        NEPTUNE_AUTOIRIS_MODE_AUTO = 1,
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for Look-up Table
    //
    ///////////////////////////////////////////////////////////////////////
    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_POINT
    {
        public UInt32 x;		// x-coordinate
        public UInt32 y;		// y-coordinate
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_KNEE_LUT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public NEPTUNE_POINT[] Points;		// 4 points
        public ENeptuneBoolean bEnable;							// enable/disable state/control
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct NEPTUNE_USER_LUT
    {
        public UInt16 SupportLUT;					// bit Flag
        public UInt16 LUTIndex;					// current LUT index
        public ENeptuneBoolean bEnable;						// enable/disable state/control
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public UInt16[] Data;	// LUT data, valid only in Set function
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for SIO
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneSIOParity
    {
        NEPTUNE_SIO_PARITY_NONE = 0,
        NEPTUNE_SIO_PARITY_ODD = 1,
        NEPTUNE_SIO_PARITY_EVEN = 2,
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_SIO_PROPERTY
    {
        public ENeptuneBoolean bEnable;	// RS232 enable
        public UInt32 Baudrate;	// serial baudrate
        public ENeptuneSIOParity Parity;		// parity bit
        public UInt32 DataBit;	// data bit
        public UInt32 StopBit;	// stop bit
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_SIO
    {
        public UInt32 TextCount;		// should be smaller than or equal to 256
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string strText;	// RS232 data
        public UInt32 TimeOut;		// in ms unit
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for Auto Area Control
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneAutoAreaSelect
    {
        NEPTUNE_AUTOAREA_SELECT_AE = 0,	// for AutoExposure
        NEPTUNE_AUTOAREA_SELECT_AWB = 1,	// for AutoWhiteBalance
        NEPTUNE_AUTOAREA_SELECT_AF = 2		// for AutoFocus
    };


    public enum ENeptuneAutoAreaSize
    {
        NEPTUNE_AUTOAREA_SIZE_SELECTED = 0,	// selected size
        NEPTUNE_AUTOAREA_SIZE_FULL = 1,	// full image size
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEPTUNE_AUTOAREA
    {
        public ENeptuneBoolean OnOff;
        public ENeptuneAutoAreaSize SizeControl;
        public NEPTUNE_IMAGE_SIZE AreaSize;
    };


    ///////////////////////////////////////////////////////////////////////
    //
    //		Enumeration types and structures for Auto Focus Control
    //
    ///////////////////////////////////////////////////////////////////////
    public enum ENeptuneAFMode
    {
        NEPTUNE_AF_ORIGIN = 0,		// set focus to origin point
        NEPTUNE_AF_ONEPUSH = 1,		// one-push auto focus
        NEPTUNE_AF_STEP_FORWARD = 2,		// move one step forward
        NEPTUNE_AF_STEP_BACKWARD = 3			// move one step backward
    };



    ///////////////////////////////////////////////////////////////////////
    //
    //		Callback function delegation
    //
    ///////////////////////////////////////////////////////////////////////
    // device check callback
    public delegate bool NeptuneCDevCheckCallback(ENeptuneDeviceChangeState eState, IntPtr pContext);

    // camera unplug/plug callback
    public delegate bool NeptuneCUnplugCallback(IntPtr pContext);

    // image received callback
    public delegate bool NeptuneCFrameCallback(ref NEPTUNE_IMAGE pImage, IntPtr pContext);

    // frame drop callback
    public delegate bool NeptuneCFrameDropCallback(IntPtr pContext);


    class NeptuneC
    {

        ///////////////////////////////////////////////////////////////////////
        //
        //		          NeptuneC API Function Definition
        //
        ///////////////////////////////////////////////////////////////////////
        //
        // initialization functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcInit();
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcInit();

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcUninit();
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcUninit();

        //
        // camera information functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetCameraCount(_uint32_t* pnNumbers);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetCameraCount(ref UInt32 pnNumbers);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetCameraInfo(NEPTUNE_CAM_INFO* pInfo, _uint32_t nLength);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetCameraInfo(IntPtr pInfo, UInt32 nLength);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetDeviceCheckCallback(NeptuneCDevCheckCallback DeviceCallback, void* pContext);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetDeviceCheckCallback(NeptuneCDevCheckCallback DeviceCallback, IntPtr pContext);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcOpen(_char_t* pstrDevID, NeptuneCamHandle* phCamHandle, ENeptuneDevAccess eAccessFlag=NEPTUNE_DEV_ACCESS_EXCLUSIVE);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcOpen(string pstrDevID, ref IntPtr phCamHandle, ENeptuneDevAccess eAccessFlag /* = ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcClose(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcClose(IntPtr hCamHandle);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetCameraType(NeptuneCamHandle hCamHandle, ENeptuneDevType* peType);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetCameraType(IntPtr hCamHandle, ref ENeptuneDevType peType);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetHeartbeatTime(NeptuneCamHandle hCamHandle, _ulong32_t nTime);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetHeartbeatTime(IntPtr hCamHandle, UInt32 nTime);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetUnplugCallback(NeptuneCamHandle hCamHandle, NeptuneCUnplugCallback UnplugCallback, void* pContext);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetUnplugCallback(IntPtr hCamHandle, NeptuneCUnplugCallback UnplugCallback, IntPtr pContext);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcShowControlDialog(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcShowControlDialog(IntPtr hCamHandle);

        //
        //	Image Information & Settings functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetPixelFormatList(NeptuneCamHandle hCamHandle, ENeptunePixelFormat* peList, _uint32_t* pnSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetPixelFormatList(IntPtr hCamHandle, IntPtr peList, ref UInt32 pnSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetPixelFormat(NeptuneCamHandle hCamHandle, ENeptunePixelFormat* peFormat);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetPixelFormat(IntPtr hCamHandle, ref ENeptunePixelFormat peFormat);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetPixelFormatString(NeptuneCamHandle hCamHandle, const ENeptunePixelFormat eFormat, _char_t* pStr, _uint32_t nSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetPixelFormatString(IntPtr hCamHandle, ENeptunePixelFormat eFormat, StringBuilder pStr, UInt32 nSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetPixelFormat(NeptuneCamHandle hCamHandle, const ENeptunePixelFormat eFormat);	
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetPixelFormat(IntPtr hCamHandle, ENeptunePixelFormat eFormat);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetBitsPerPixel(NeptuneCamHandle hCamHandle, _uint32_t* pnBits);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetBitsPerPixel(IntPtr hCamHandle, ref UInt32 pnBits);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetFrameRateList(NeptuneCamHandle hCamHandle, ENeptuneFrameRate* peList, _uint32_t* pnSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetFrameRateList(IntPtr hCamHandle, IntPtr peList, ref UInt32 pnSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetFrameRate(NeptuneCamHandle hCamHandle, ENeptuneFrameRate* peRate, _double_t* pfValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetFrameRate(IntPtr hCamHandle, ref ENeptuneFrameRate peRate, ref Double pfValue);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetFrameRateString(NeptuneCamHandle hCamHandle, const ENeptuneFrameRate eRate, _char_t* pStr, _uint32_t nSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetFrameRateString(IntPtr hCamHandle, ENeptuneFrameRate eRate, StringBuilder pStr, UInt32 nSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetFrameRate(NeptuneCamHandle hCamHandle, ENeptuneFrameRate eRate, _double_t fValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetFrameRate(IntPtr hCamHandle, ENeptuneFrameRate eRate, Double fValue);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetImageSize(NeptuneCamHandle hCamHandle, PNEPTUNE_IMAGE_SIZE pImageSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetImageSize(IntPtr hCamHandle, ref NEPTUNE_IMAGE_SIZE pImageSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetMaxImageSize(NeptuneCamHandle hCamHandle, PNEPTUNE_IMAGE_SIZE pImageSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetMaxImageSize(IntPtr hCamHandle, ref NEPTUNE_IMAGE_SIZE pImageSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetImageSize(NeptuneCamHandle hCamHandle, NEPTUNE_IMAGE_SIZE ImageSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetImageSize(IntPtr hCamHandle, NEPTUNE_IMAGE_SIZE ImageSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetBayerConvert(NeptuneCamHandle hCamHandle, ENeptuneBayerMethod* peMethod);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetBayerConvert(IntPtr hCamHandle, ref ENeptuneBayerMethod peMethod);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetBayerConvert(NeptuneCamHandle hCamHandle, ENeptuneBayerMethod eMethod);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetBayerConvert(IntPtr hCamHandle, ENeptuneBayerMethod eMethod);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetBayerLayout(NeptuneCamHandle hCamHandle, ENeptuneBayerLayout* peLayout);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetBayerLayout(IntPtr hCamHandle, ref ENeptuneBayerLayout peLayout);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetBayerLayout(NeptuneCamHandle hCamHandle, ENeptuneBayerLayout ePattern);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetBayerLayout(IntPtr hCamHandle, ENeptuneBayerLayout ePattern);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetBytePerPacket(NeptuneCamHandle hCamHandle, _uint32_t* pnBpp, _uint32_t* pnMin=NULL, _uint32_t* pnMax=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetBytePerPacket(IntPtr hCamHandle, ref UInt32 pnBpp, ref UInt32 pnMin, ref UInt32 pnMax);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetBytePerPacket(NeptuneCamHandle hCamHandle, _uint32_t nBpp);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetBytePerPacket(IntPtr hCamHandle, UInt32 nBpp);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetPacketSize(NeptuneCamHandle hCamHandle, _uint32_t* pnPacketSize, _uint32_t* pnMin=NULL, _uint32_t* pnMax=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetPacketSize(IntPtr hCamHandle, ref UInt32 pnPacketSize, ref UInt32 pnMin, ref UInt32 pnMax);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetPacketSize(NeptuneCamHandle hCamHandle, _uint32_t nPacketSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetPacketSize(IntPtr hCamHandle, UInt32 nPacketSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetAcquisitionMode(NeptuneCamHandle hCamHandle, ENeptuneAcquisitionMode* peMode, _uint32_t* pnFrames);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetAcquisitionMode(IntPtr hCamHandle, ref ENeptuneAcquisitionMode peMode, ref UInt32 pnFrames);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAcquisitionMode(NeptuneCamHandle hCamHandle, ENeptuneAcquisitionMode eMode, _uint32_t nFrames=2);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAcquisitionMode(IntPtr hCamHandle, ENeptuneAcquisitionMode eMode, UInt32 nFrames);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcOneShot(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcOneShot(IntPtr hCamHandle);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcMultiShot(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcMultiShot(IntPtr hCamHandle);

        //
        // data acquisition functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetBufferCount(NeptuneCamHandle hCamHandle, _uint32_t nCount);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetBufferCount(IntPtr hCamHandle, UInt32 nCount);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetBufferSize(NeptuneCamHandle hCamHandle, _uint32_t* pnSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetBufferSize(IntPtr hCamHandle, ref UInt32 pnSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetUserBuffer(NeptuneCamHandle hCamHandle, _void_t* pBuffer, _uint32_t nSize, _uint32_t nCount);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetUserBuffer(IntPtr hCamHandle, IntPtr pBuffer, UInt32 nFrameSize, UInt32 nCount);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetEffect(NeptuneCamHandle hCamHandle, _int32_t nEffect);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetEffect(IntPtr hCamHandle, Int32 nEffect);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGrab(NeptuneCamHandle hCamHandle, PNEPTUNE_IMAGE pImage, ENeptuneGrabFormat eGrabFormat, _uint32_t nTimeOut=1000);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGrab(IntPtr hCamHandle, ref NEPTUNE_IMAGE pImage, ENeptuneGrabFormat eGrabFormat, UInt32 nTimeOut/*use 1000 as default*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetFrameCallback(NeptuneCamHandle hCamHandle, NeptuneCFrameCallback callback, void* pContext=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetFrameCallback(IntPtr hCamHandle, NeptuneCFrameCallback callback, IntPtr pContext);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetFrameDropCallback(NeptuneCamHandle hCamHandle, NeptuneCFrameDropCallback callback, void* pContext=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetFrameDropCallback(IntPtr hCamHandle, NeptuneCFrameDropCallback callback, IntPtr pContext);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetDisplay(NeptuneCamHandle hCamHandle, HWND hWnd);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetDisplay(IntPtr hCamHandle, IntPtr hWnd);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAcquisition(NeptuneCamHandle hCamHandle, ENeptuneBoolean eState);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAcquisition(IntPtr hCamHandle, ENeptuneBoolean eState);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetAcquisition(NeptuneCamHandle hCamHandle, ENeptuneBoolean* peState);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetAcquisition(IntPtr hCamHandle, ref ENeptuneBoolean peState);

        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetMulticastAddress(IntPtr hCamHandle, string pstrAddress);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetReceiveFrameRate(NeptuneCamHandle hCamHandle, _float32_t* pfRate);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetReceiveFrameRate(IntPtr hCamHandle, ref float pfRate);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetRGBData(NeptuneCamHandle hCamHandle, _uchar_t* pBuffer, _uint32_t nSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetRGBData(IntPtr hCamHandle, IntPtr pBuffer, UInt32 nSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSaveImage(NeptuneCamHandle hCamHandle, _char_t* strFileName, _uint32_t nQuality=80);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSaveImage(IntPtr hCamHandle, string strFileName, UInt32 nQuality/* = 80*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcStartStreamCapture(NeptuneCamHandle hCamHandle, _char_t* strFileName, ENeptuneBoolean eCompress, _uint32_t nBitrate=1000);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcStartStreamCapture(IntPtr hCamHandle, string strFileName, ENeptuneBoolean eCompress, UInt32 nBitrate/* = 1000*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcStopStreamCapture(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcStopStreamCapture(IntPtr hCamHandle);

        //
        // feature control functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetFeature(NeptuneCamHandle hCamHandle, ENeptuneFeature eFeature, PNEPTUNE_FEATURE pInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetFeature(IntPtr hCamHandle, ENeptuneFeature eFeature, ref NEPTUNE_FEATURE pInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetFeature(NeptuneCamHandle hCamHandle, ENeptuneFeature eFeature, NEPTUNE_FEATURE Info);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetFeature(IntPtr hCamHandle, ENeptuneFeature eFeature, NEPTUNE_FEATURE Info);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetShutterString(NeptuneCamHandle hCamHandle, _char_t* pstrShutter);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetShutterString(IntPtr hCamHandle, string pstrShutter);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetAutoAreaControl(NeptuneCamHandle hCamHandle, ENeptuneAutoAreaSelect eSelect, PNEPTUNE_AUTOAREA pArea);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetAutoAreaControl(IntPtr hCamHandle, ENeptuneAutoAreaSelect eSelect, ref NEPTUNE_AUTOAREA pArea);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAutoAreaControl(NeptuneCamHandle hCamHandle, ENeptuneAutoAreaSelect eSelect, NEPTUNE_AUTOAREA Area);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAutoAreaControl(IntPtr hCamHandle, ENeptuneAutoAreaSelect eSelect, NEPTUNE_AUTOAREA Area);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAFControl(NeptuneCamHandle hCamHandle, ENeptuneAFMode eControlMode);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAFControl(IntPtr hCamHandle, ENeptuneAFMode eControlMode);

        //
        // trigger information & control
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetTriggerInfo(NeptuneCamHandle hCamHandle, PNEPTUNE_TRIGGER_INFO pTriggerInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetTriggerInfo(IntPtr hCamHandle, ref NEPTUNE_TRIGGER_INFO pTriggerInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetTrigger(NeptuneCamHandle hCamHandle, PNEPTUNE_TRIGGER pTrigger);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetTrigger(IntPtr hCamHandle, ref NEPTUNE_TRIGGER pTrigger);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetTrigger(NeptuneCamHandle hCamHandle, NEPTUNE_TRIGGER Trigger);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetTrigger(IntPtr hCamHandle, NEPTUNE_TRIGGER Trigger);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetTriggerMode14Exposure(NeptuneCamHandle hCamHandle, _uint32_t nExposure, _uint32_t nInterval);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetTriggerMode14Exposure(IntPtr hCamHandle, UInt32 nExposure, UInt32 nInterval);

        //NEPTUNE_C_API ENeptuneError	API_CALLTYPE ntcGetTriggerDelay(NeptuneCamHandle hCamHandle, _uint32_t* pnDelay, _uint32_t* pnMin=NULL, _uint32_t* pnMax=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetTriggerDelay(IntPtr hCamHandle, ref UInt32 pnDelay, ref UInt32 pnMin/* = NULL*/, ref UInt32 pnMax/* = NULL*/);

        //NEPTUNE_C_API ENeptuneError	API_CALLTYPE ntcSetTriggerDelay(NeptuneCamHandle hCamHandle, _uint32_t nDelay);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetTriggerDelay(IntPtr hCamHandle, UInt32 nDelay);

        //NEPTUNE_C_API ENeptuneError	API_CALLTYPE ntcRunSWTrigger(NeptuneCamHandle hCamHandle);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcRunSWTrigger(IntPtr hCamHandle);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcReadTriggerTable(NeptuneCamHandle hCamHandle, PNEPTUNE_TRIGGER_TABLE pTriggerTable)
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcReadTriggerTable(IntPtr hCamHandle, ref NEPTUNE_TRIGGER_TABLE pTriggerTable);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSaveTriggerTable(NeptuneCamHandle hCamHandle, NEPTUNE_TRIGGER_TABLE TriggerTable)
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSaveTriggerTable(IntPtr hCamHandle, NEPTUNE_TRIGGER_TABLE TriggerTable);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcLoadTriggerTable(NeptuneCamHandle hCamHandle, _uint32_t nIndex)
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcLoadTriggerTable(IntPtr hCamHandle, UInt32 nIndex);

        //
        // strobe information & control
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetStrobeInfo(NeptuneCamHandle hCamHandle, PNEPTUNE_STROBE_INFO pStrobeInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetStrobeInfo(IntPtr hCamHandle, ref NEPTUNE_STROBE_INFO pStrobeInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetStrobe(NeptuneCamHandle hCamHandle, PNEPTUNE_STROBE pStrobe);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetStrobe(IntPtr hCamHandle, ref NEPTUNE_STROBE pStrobe);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetStrobe(NeptuneCamHandle hCamHandle, NEPTUNE_STROBE Strobe);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetStrobe(IntPtr hCamHandle, NEPTUNE_STROBE Strobe);


        //
        // auto iris control functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAutoIrisMode(NeptuneCamHandle hCamHandle, ENeptuneAutoIrisMode eMode);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAutoIrisMode(IntPtr hCamHandle, ENeptuneAutoIrisMode eMode);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetAutoIrisAverageFrame(NeptuneCamHandle hCamHandle, _uint32_t* pnValue, _uint32_t* pnMin=NULL, _uint32_t* pnMax=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetAutoIrisAverageFrame(IntPtr hCamHandle, ref UInt32 pnValue, ref UInt32 pnMin/* = NULL*/, ref UInt32 pnMax/* = NULL*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAutoIrisAverageFrame(NeptuneCamHandle hCamHandle, _uint32_t nValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAutoIrisAverageFrame(IntPtr hCamHandle, UInt32 nValue);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetAutoIrisTargetValue(NeptuneCamHandle hCamHandle, _uint32_t* pnValue, _uint32_t* pnMin=NULL, _uint32_t* pnMax=NULL);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetAutoIrisTargetValue(IntPtr hCamHandle, ref UInt32 pnValue, ref UInt32 pnMin/* = NULL*/, ref UInt32 pnMax/* = NULL*/);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetAutoIrisTargetValue(NeptuneCamHandle hCamHandle, _uint32_t nValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetAutoIrisTargetValue(IntPtr hCamHandle, UInt32 nValue);


        //
        // GPIO control functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetGPIO(NeptuneCamHandle hCamHandle, NEPTUNE_GPIO Gpio);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetGPIO(IntPtr hCamHandle, NEPTUNE_GPIO Gpio);


        // user set handling functions
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetUserSet(NeptuneCamHandle hCamHandle, PNEPTUNE_USERSET pUserSet);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetUserSet(IntPtr hCamHandle, ref NEPTUNE_USERSET pUserSet);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetUserSet(NeptuneCamHandle hCamHandle, NEPTUNE_USERSET UserSet);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetUserSet(IntPtr hCamHandle, NEPTUNE_USERSET UserSet);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetDefaultUserSet(NeptuneCamHandle hCamHandle, ENeptuneUserSet eUserSet);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetDefaultUserSet(IntPtr hCamHandle, ENeptuneUserSet eUserSet);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetPowerOnDefaultUserSet(NeptuneCamHandle hCamHandle, ENeptuneUserSet eUserSet);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetPowerOnDefaultUserSet(IntPtr hCamHandle, ENeptuneUserSet eUserSet);

        //
        // look-up table functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetKneeLUT(NeptuneCamHandle hCamHandle, PNEPTUNE_KNEE_LUT pLUTPoints);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetKneeLUT(IntPtr hCamHandle, ref NEPTUNE_KNEE_LUT pLUTPoints);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetKneeLUT(NeptuneCamHandle hCamHandle, NEPTUNE_KNEE_LUT LUTPoints);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetKneeLUT(IntPtr hCamHandle, NEPTUNE_KNEE_LUT LUTPoints);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetUserLUT(NeptuneCamHandle hCamHandle, PNEPTUNE_USER_LUT pLUTData);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetUserLUT(IntPtr hCamHandle, ref NEPTUNE_USER_LUT pLUTData);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetUserLUT(NeptuneCamHandle hCamHandle, NEPTUNE_USER_LUT LUTData);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetUserLUT(IntPtr hCamHandle, NEPTUNE_USER_LUT LUTData);

        //
        // frame save control functions(1394 camera only)
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetFrameSave(NeptuneCamHandle hCamHandle, ENeptuneBoolean* peOnOff, _uint32_t* pnFrameRemained);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetFrameSave(IntPtr hCamHandle, ref ENeptuneBoolean peOnOff, ref UInt32 pnFrameRemained);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetFrameSave(NeptuneCamHandle hCamHandle, ENeptuneBoolean eOnOff, ENeptuneBoolean eTransfer, _uint32_t nFrames);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetFrameSave(IntPtr hCamHandle, ENeptuneBoolean eOnOff, ENeptuneBoolean eTransfer, UInt32 nFrames);


        // SIO control functions
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetSIO(NeptuneCamHandle hCamHandle,NEPTUNE_SIO_PROPERTY Property);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetSIO(IntPtr hCamHandle, NEPTUNE_SIO_PROPERTY Property);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcWriteSIO(NeptuneCamHandle hCamHandle, NEPTUNE_SIO Data);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcWriteSIO(IntPtr hCamHandle, NEPTUNE_SIO Data);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcReadSIO(NeptuneCamHandle hCamHandle, PNEPTUNE_SIO pData);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcReadSIO(IntPtr hCamHandle, ref NEPTUNE_SIO pData);


        //
        // register access functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcReadRegister(NeptuneCamHandle hCamHandle, _ulong32_t ulAddr, _ulong32_t* pulVal);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcReadRegister(IntPtr hCamHandle, UInt32 ulAddr, ref UInt32 pulVal);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcWriteRegister(NeptuneCamHandle hCamHandle, _ulong32_t ulAddr, _ulong32_t ulVal);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcWriteRegister(IntPtr hCamHandle, UInt32 ulAddr, UInt32 ulVal);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcReadBlock(NeptuneCamHandle hCamHandle, _ulong32_t ulAddr, _uint8_t* pBuf, _ulong32_t* pnSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcReadBlock(IntPtr hCamHandle, UInt32 ulAddr, Byte[] pBuf, ref UInt32 pnSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcWriteBlock(NeptuneCamHandle hCamHandle, _ulong32_t ulAddr, _uint8_t* pBuf, _ulong32_t nSize);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcWriteBlock(IntPtr hCamHandle, UInt32 ulAddr, Byte[] pBuf, UInt32 nSize);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcWriteBroadcast(NeptuneCamHandle hCamHandle, _ulong32_t ulAddr, _ulong32_t ulVal);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcWriteBroadcast(IntPtr hCamHandle, UInt32 ulAddr, UInt32 ulVal);


        //
        // XML control functions
        //
        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeVisibility(NeptuneCamHandle hCamHandle, ENeptuneNodeVisibility* peVisibility);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeVisibility(IntPtr hCamHandle, ref ENeptuneNodeVisibility peVisibility);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeVisibility(NeptuneCamHandle hCamHandle, ENeptuneNodeVisibility eVisibility);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeVisibility(IntPtr hCamHandle, ENeptuneNodeVisibility eVisibility);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeListChar(NeptuneCamHandle hCamHandle, const _char_t* pstrCategory, _char_t* pstrList, _uint32_t nSize, _uint32_t* pnCount);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        // nSize should be MAX_XML_NODE_STRING_LENGTH(256)
        // nCount is the number of strings
        // the size of the "pstrList" should be the nSize*nCount
        public static extern ENeptuneError ntcGetNodeListChar(IntPtr hCamHandle, string pstrCategory, IntPtr pstrList, UInt32 nSize, ref UInt32 nCount);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeInfo(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, PNEPTUNE_XML_NODE_INFO pInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeInfo(IntPtr hCamHandle, string pstrNode, ref NEPTUNE_XML_NODE_INFO pInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeInt(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, PNEPTUNE_XML_INT_VALUE_INFO pValueInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeInt(IntPtr hCamHandle, string pstrNode, ref NEPTUNE_XML_INT_VALUE_INFO pValueInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeInt(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, _int64_t nValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeInt(IntPtr hCamHandle, string pstrNode, long nValue);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeFloat(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, PNEPTUNE_XML_FLOAT_VALUE_INFO pValueInfo);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeFloat(IntPtr hCamHandle, string pstrNode, ref NEPTUNE_XML_FLOAT_VALUE_INFO pValueInfo);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeFloat(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, _double_t dValue);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeFloat(IntPtr hCamHandle, string pstrNode, double dValue);

        // NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeEnumChar(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, _char_t* pstrList, _uint32_t nSize, _uint32_t* pnCount, _uint32_t* pnIndex);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        // nSize should be MAX_XML_NODE_STRING_LENGTH(256)
        // nCount is the number of strings allocated
        // the size of the "pstrList" should be the nSize*nCount
        public static extern ENeptuneError ntcGetNodeEnumChar(IntPtr hCamHandle, string pstrNode, IntPtr pstrList, UInt32 nSize, ref UInt32 nCount, ref UInt32 nIndex);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeEnum(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, const _char_t* pstrEnum);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeEnum(IntPtr hCamHandle, string pstrNode, string pstrEnum);

        // NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeString(NeptuneCamHandle hCamHandle, const _char_t *pstrNode, _char_t *pstrString, _uint32_t *pnSize, ENeptuneNodeAccessMode *peAccessMode)
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeString(IntPtr hCamHandle, string pstrNode, StringBuilder pstrString, ref UInt32 pnSize, ref ENeptuneNodeAccessMode peAccessMode);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeString(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, const _char_t* pstrString);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeString(IntPtr hCamHandle, string pstrNode, string pstrString);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcGetNodeBoolean(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, ENeptuneBoolean* peState);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcGetNodeBoolean(IntPtr hCamHandle, string pstrNode, ref ENeptuneBoolean peState);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeBoolean(NeptuneCamHandle hCamHandle, const _char_t* pstrNode, ENeptuneBoolean eState);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeBoolean(IntPtr hCamHandle, string pstrNode, ENeptuneBoolean eState);

        //NEPTUNE_C_API ENeptuneError API_CALLTYPE ntcSetNodeCommand(NeptuneCamHandle hCamHandle, const _char_t* pstrNode);
        [DllImport(".\\NeptuneC_MD_VC80.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ENeptuneError ntcSetNodeCommand(IntPtr hCamHandle, string pstrNode);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);



        //
        // Marshaling functions
        //
        public static IntPtr MarshalArrtoIntPtr<T>(T[] st)
        {
            int nSize = Marshal.SizeOf(st[0]) * st.Length;
            IntPtr buffer = Marshal.AllocHGlobal(nSize);
            for (int i = 0; i < st.Length; i++)
            {
                IntPtr ptrTmp = new IntPtr(buffer.ToInt64() + i * Marshal.SizeOf(st[0]));
                Marshal.StructureToPtr(st[i], ptrTmp, false);
            }

            return buffer;
        }

        public static void UnmarshalIntPtrToArr<T>(IntPtr pUnmangedPtr, ref T[] st)
        {
            int iSize = Marshal.SizeOf(typeof(T));
            for (int i = 0; i < st.Length; i++)
            {
                IntPtr ptrTmp = new IntPtr(pUnmangedPtr.ToInt64() + i * Marshal.SizeOf(st[0]));
                st[i] = (T)(Marshal.PtrToStructure(ptrTmp, typeof(T)));
            }

            return;
        }

        public static IntPtr MarshalByteArrToPtr(ref Byte[] Arr, int Size)
        {
            int iTotalSize = Size * Marshal.SizeOf(typeof(Byte));
            IntPtr pUnmangedPtr = Marshal.AllocHGlobal(iTotalSize);

            return pUnmangedPtr;
        }
    }
}

