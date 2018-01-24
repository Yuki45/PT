using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AutoInspection_GUMI;

namespace AutoInspection
{
    // Property Grid 표시 용 임시 클래스
    //  Config <-> ConfigInfo <-> PropertyGrid Control
    public class ConfigInfo
    {
        public bool SaveResultImage { get; set; }
        public bool SaveDebugImage { get; set; }
        public string AnyjigComport { get; set; }
        public int AnyjigBaud { get; set; }
        public string IoComport { get; set; }
        public int CamFrontWidth { get; set; }
        public int CamFrontHeight { get; set; }
        public int CamFrontOffsetX { get; set; }
        public int CamFrontOffsetY { get; set; }
        public string LightComport { get; set; }
        public int LightBaud { get; set; }
        //public int MainCylinderDelay { get; set; }
        //public int AnyJigCylinderDelay { get; set; }
        //public int LightCylinderDelay { get; set; }
        public bool SetPowerOff { get; set; }
        public int GreenaAcumulateCount { get; set; }
        //public bool GreenaAcumulateTest { get; set; }
        public string ImageLogFolder { get; set; } // NJH

        public string ImageLogFIleExtension { get; set; } // NJH
		
        public int GreenExpOffset { get; set; }
        public int ReddishOffset_Top { get; set; }
        public int ReddishOffset_Bottom { get; set; }
        public int ReddishOffset_Left { get; set; }
        public int ReddishOffset_Right { get; set; }

        public int ReddishOffset_Center { get; set; }

        //public int CamFront { get; set; }
        //public int CamRear { get; set; }
        //public string CurrnetModelFile { get; set; }
        //public string CurrnetModelName { get; set; }
        //public string CurrnetSpecFile { get; set; }

        public ConfigInfo()
        {
            GetFromConfig();
        }

        public void SetToConfig()
        {
            Config.SaveResultImage = SaveResultImage;
            Config.SaveDebugImage = SaveDebugImage;


            Config.sAnyjigComport = AnyjigComport;
            Config.nAnyjigBaud = AnyjigBaud;

            Config.sIoComport = IoComport;

            Config.sLightComport = LightComport;
            Config.nLightBaud = LightBaud;

            //Config.nMainCylinderDelay = MainCylinderDelay;
            //Config.nAnyJigCylinderDelay = AnyJigCylinderDelay;
            //Config.nLightCylinderDelay = LightCylinderDelay;

            Config.CamFrontWidth = CamFrontWidth;
            Config.CamFrontHeight = CamFrontHeight;
            Config.CamFrontOffsetX = CamFrontOffsetX;
            Config.CamFrontOffsetY = CamFrontOffsetY;

            Config.SetPowerOFF = SetPowerOff;
            Config.GreenExpOffset = GreenExpOffset;
            Config.GreenaAcumulateCount = GreenaAcumulateCount;
            //Config.GreenaAcumulateTest = GreenaAcumulateTest;
            Config.ReddishOffset_Top = ReddishOffset_Top;
            Config.ReddishOffset_Bottom = ReddishOffset_Bottom;
            Config.ReddishOffset_Left = ReddishOffset_Left;
            Config.ReddishOffset_Right = ReddishOffset_Right;
            Config.ReddishOffset_Center = ReddishOffset_Center;
            Config.ImageLogFolder = ImageLogFolder; // NJH
            Config.ImageLogFIleExtension = ImageLogFIleExtension; // NJH
        }

        public void GetFromConfig()
        {
            SaveResultImage = Config.SaveResultImage;
            SaveDebugImage = Config.SaveDebugImage;
            AnyjigComport = Config.sAnyjigComport;
            AnyjigBaud = Config.nAnyjigBaud;

            IoComport = Config.sIoComport;

            LightComport = Config.sLightComport;
            LightBaud = Config.nLightBaud;

            //MainCylinderDelay = Config.nMainCylinderDelay;
            //AnyJigCylinderDelay = Config.nAnyJigCylinderDelay;
            //LightCylinderDelay = Config.nLightCylinderDelay;
            CamFrontWidth = Config.CamFrontWidth;
            CamFrontHeight = Config.CamFrontHeight;
            CamFrontOffsetX = Config.CamFrontOffsetX;
            CamFrontOffsetY = Config.CamFrontOffsetY;

            SetPowerOff = Config.SetPowerOFF;
            GreenExpOffset = Config.GreenExpOffset;
            GreenaAcumulateCount = Config.GreenaAcumulateCount;
            //GreenaAcumulateTest = Config.GreenaAcumulateTest;
            ReddishOffset_Top = Config.ReddishOffset_Top;
            ReddishOffset_Bottom = Config.ReddishOffset_Bottom;
            ReddishOffset_Left = Config.ReddishOffset_Left;
            ReddishOffset_Right = Config.ReddishOffset_Right;
            ReddishOffset_Center = Config.ReddishOffset_Center;
            ImageLogFolder = Config.ImageLogFolder; // NJH
            ImageLogFIleExtension = Config.ImageLogFIleExtension; // NJH
        }
    }

    public class Config
    {
        public static readonly string Version = "0.9.0";

        [JsonProperty]
        public static int CountPass = 0;
        [JsonProperty]
        public static int CountFail = 0;
        [JsonProperty]
        public static bool SaveResultImage = true;
        [JsonProperty]
        public static bool SaveDebugImage = false;


        // motion
        [JsonProperty]
        public static string sAnyjigComport = "COM5";
        [JsonProperty]
        public static int nAnyjigBaud = 115200;
        [JsonProperty]
        public static string sIoComport = "COM1";
        [JsonProperty]
        public static int nIoBaud = 9600;
        [JsonProperty]
        public static string sLightComport = "COM2";
        [JsonProperty]
        public static int nLightBaud = 19200;
        //[JsonProperty]
        //public static string sCurrnetModelFile = "default.json";
        //[JsonProperty]
        //public static string sCurrnetModelName = "default";
        [JsonProperty]
        public static string sCurrnetSpecFile = "default.json";
        [JsonProperty]
        public static int nMainCylinderDelay = 500;
        [JsonProperty]
        public static int nAnyJigCylinderDelay = 500;
        [JsonProperty]
        public static int nLightCylinderDelay = 500;

        // [JsonProperty] public static string sCurrnetModelFile = "default.json";
        [JsonProperty]
        public static int CamFrontWidth = 1;
        [JsonProperty]
        public static int CamFrontHeight = 1;
        [JsonProperty]
        public static int CamFrontOffsetX = 1;
        [JsonProperty]
        public static int CamFrontOffsetY = 1;
        [JsonProperty]
        public static bool SetPowerOFF = false;
        [JsonProperty]
        public static int GreenExpOffset = 0;
        [JsonProperty]
        public static int GreenaAcumulateCount = 3;
        //[JsonProperty]
        //public static bool GreenaAcumulateTest = false;
        [JsonProperty]
        public static int ReddishOffset_Top = 0;
        [JsonProperty]
        public static int ReddishOffset_Bottom = 0;
        [JsonProperty]
        public static int ReddishOffset_Left = 0;
        [JsonProperty]
        public static int ReddishOffset_Right = 0;
        [JsonProperty]
        public static int ReddishOffset_Center = 0;
        [JsonProperty]
        public static string logImagePath = @".\Detail\";
        [JsonProperty]
        public static string logImageArea = @".\Detail\Area\";
        [JsonProperty]
        public static string logImageDust = @".\Detail\Dust\";
        [JsonProperty]
        public static string logImageLcdBlue = @".\Detail\Lcd Blue\";
        [JsonProperty]
        public static string logImageLedBlue = @".\Detail\Led Blue\";
        [JsonProperty]
        public static string logImageLcdRed = @".\Detail\Lcd Red\";
        [JsonProperty]
        public static string logImageLcdGreen = @".\Detail\Lcd Green\";
        [JsonProperty]
        public static string logImageLcdWhite = @".\Detail\Lcd White\";
        [JsonProperty]
        public static string logImageRear = @".\Detail\Rear\";
        [JsonProperty]
        public static string logImageLcdTouch = @".\Detail\Touch\";
		
		// NJH
        [JsonProperty]
        public static string LCDLogFolder = @"D:\LCD_AutoInspection";
        // NJH
        [JsonProperty]
        public static string ImageLogFolder = @"D:\LCD_AutoInspection\IMG\";

        // NJH
        [JsonProperty]
        public static string ImageLogFIleExtension = "jpg";
		
        [JsonProperty]
        public static string logImageLcdMCD = @".\Detail\Lcd MCD\";
        [JsonProperty]
        public static string logImageLcdCOPCrack = @".\Detail\Lcd Copcrack\";
        [JsonProperty]
        public static string logImageLcdReddish = @".\Detail\Lcd Reddish\";

        public static string sCurrentWorkingPath = Directory.GetCurrentDirectory();
        //public static string sSettingFile = Directory.GetCurrentDirectory() + @"\setting.json";

        public static string sSettingFile = @"D:\LCD_AutoInspection\setting.json";
        [JsonProperty]
        //public static string sPathModel = Directory.GetCurrentDirectory() + @"\MODEL\";
        public static string sPathModel = @"D:\LCD_AutoInspection\MODEL\";
        public static string sPathLog = @"C:\DGS\LOGS\";
        //public static string sPathBehaviorLog = Directory.GetCurrentDirectory() + @"D:\LCD_AutoInspection\Log\";
        public static string sPathBehaviorLog =  @"D:\LCD_AutoInspection\Log\";
        //public static string sSettingFile = LCDLogFolder + @"\setting.json";
        //public static string sPathModel = LCDLogFolder + "\\" + @"Model\";
        // public static string ImageLogFolder = @"D:\IMG\";     //설정 옵션에 따라 FAIL 이미지 저장되는 폴더


        public static UInt32 ImiPixelFormat_Mono8 = 101;
        public static UInt32 ImiPixelFormat_Mono12 = 103;
        public static UInt32 ImiPixelFormat_BayerGR8 = 105;
        public static UInt32 ImiPixelFormat_BayerGR12 = 107;
        public static UInt32 ImiPixelFormat_YUV411Packed = 108;
        public static UInt32 ImiPixelFormat_YUV422Packed = 109;

        public static void saveTofile()
        {
            string _s = JsonConvert.SerializeObject(new Config());
            string _sIndented = JToken.Parse(_s).ToString(Formatting.Indented);
            File.WriteAllText(sSettingFile, _sIndented, Encoding.UTF8);
        }

        public static void loadFromFile()
        {
            if (!Directory.Exists(LCDLogFolder))
            {
                Directory.CreateDirectory(sPathModel);
                Directory.CreateDirectory(sPathLog);
                Directory.CreateDirectory(Config.LCDLogFolder);
                Directory.CreateDirectory(Config.ImageLogFolder);
                Directory.CreateDirectory(Config.sPathBehaviorLog);
            }

            if (File.Exists(sSettingFile))
            {
                try
                {
                    string _s = File.ReadAllText(sSettingFile, Encoding.UTF8);
                    Config _t = JsonConvert.DeserializeObject<Config>(_s);
                    // SetPathName(m_sModelName);  // 20160527

                }
                catch (Exception ex)
                {
                    Log.AddLog(ex.ToString());
                    Log.SaveLog();
                    return;
                }

            }
            else
            {
                saveTofile();
            }
        }




        //public static void Reset()
        //{
        //    m_nCountPass = 0;
        //    m_nCountFail = 0;
        //}
        //public static void SetPathName(string _sModelName)
        //{
        //    ConfigMachine.m_sModelName = _sModelName; // G930

        //    // m_sPathModel = @"Model\";        // .\MODEL\
        //    m_sPathModelHome = m_sPathModel + _sModelName + @"\";               // .\MODEL\G930\
        //    m_sPathSpecFile = m_sPathModelHome + _sModelName + @"_spec.json";   // CMD LIST LOAD 
        //    m_sPathCmdFile = m_sPathModelHome + _sModelName + @".json";         // CMD LIST LOAD 

        //    //m_sPathModelHome = Directory.GetCurrentDirectory() + @".\Model\" + _sModelName + @"\";               // .\MODEL\G930\
        //    //m_sPathSpecFile = m_sPathModelHome + _sModelName + @"_spec.json";   // CMD LIST LOAD 
        //    //m_sPathCmdFile = m_sPathModelHome + _sModelName + @".json";         // CMD LIST LOAD 
        //}


    }
}
