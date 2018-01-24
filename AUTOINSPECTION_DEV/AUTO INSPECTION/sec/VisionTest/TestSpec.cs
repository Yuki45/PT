// #define TEST_BAE


using Newtonsoft.Json;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace AutoInspection
{
    public enum TestType
    {
        TestReady,
        TestDust,
        TestLcdArea,
        TestLcdWhite,

        TestLcdRed,
        TestLcdBlue,
        TestLcdGreen,
        TestLcdMCD,
        TestLcdCOPCrack,
        TestLcdReddish,
        TestKeyTouch,
        TestLedBlue,

        TestSvcLed,
        
        TestFront,
        TestRear,

        TestLabel,
        TestLaser,
        TestLogo,

        TestWaitRearFinal,
        TestWaitFrontFinal,

        TestCompleted,
        TestEnd
    }

    #region kepp

    //    public class TestSpec // : ICloneable 
    //    {
    //        public TestSpec()
    //        {
    //        }

    //        [JsonProperty]
    //        public int lightValueImei = 0;// [JsonProperty] public int lightValueImei = 40;
    //        [JsonProperty]
    //        public int lightValueLogo = 20;
    //        [JsonProperty]
    //        public int lightValueLabel = 70;


    //        [JsonProperty]
    //        public MData_LCD_DUST specLcdDust = new MData_LCD_DUST();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_DUST SpecLcdDust
    //        {
    //            get { return specLcdDust; }
    //            set { specLcdDust = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LCD_AREA specLcdArea = new MData_LCD_AREA();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_AREA SpecLcdArea
    //        {
    //            get { return specLcdArea; }
    //            set { specLcdArea = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LCD_RED specLcdRed = new MData_LCD_RED(); // "Lcd Red");
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_RED SpecLcdRed
    //        {
    //            get { return specLcdRed; }
    //            set { specLcdRed = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LCD_BLUE specLcdBlue = new MData_LCD_BLUE(); //"Lcd Blue");
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_BLUE SpecLcdBlue
    //        {
    //            get { return specLcdBlue; }
    //            set { specLcdBlue = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LCD_GREEN specLcdGreen = new MData_LCD_GREEN();

    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_GREEN SpecLcdGreen
    //        {
    //            get { return specLcdGreen; }
    //            set { specLcdGreen = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LCD_WHITE specLcdWhite = new MData_LCD_WHITE();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LCD_WHITE SpecLcdWhite
    //        {
    //            get { return specLcdWhite; }
    //            set { specLcdWhite = value; }
    //        }


    //        [JsonProperty]
    //        public MData_REAR specRear = new MData_REAR();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_REAR SpecRear
    //        {
    //            get { return specRear; }
    //            set { specRear = value; }
    //        }

    //        [JsonProperty]
    //        public MData_TOUCH_KEY specTouchKey = new MData_TOUCH_KEY();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_TOUCH_KEY SpecTouchKey
    //        {
    //            get { return specTouchKey; }
    //            set { specTouchKey = value; }
    //        }

    //        [JsonProperty]
    //        public MData_TOUCH_BACKKEY specTouchBackKey = new MData_TOUCH_BACKKEY();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_TOUCH_BACKKEY SpecTouchBackKey
    //        {
    //            get { return specTouchBackKey; }
    //            set { specTouchBackKey = value; }
    //        }


    //        [JsonProperty]
    //        public MData_TOUCH_MENUKEY specTouchMenuKey = new MData_TOUCH_MENUKEY();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_TOUCH_MENUKEY SpecTouchMenuKey
    //        {
    //            get { return specTouchMenuKey; }
    //            set { specTouchMenuKey = value; }
    //        }

    //        [JsonProperty]
    //        public MData_LED_BLUE specLedBlue = new MData_LED_BLUE();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public MData_LED_BLUE SpecLedBlue
    //        {
    //            get { return specLedBlue; }
    //            set { specLedBlue = value; }
    //        }


    //        [JsonProperty]
    //        public Exposure specExposure = new Exposure();
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public Exposure SpecExposure
    //        {
    //            get { return specExposure; }
    //            set { specExposure = value; }
    //        }

    //#if TEST_BAE
    //        private Exposure testExposure = new Exposure();

    //        [JsonProperty]
    //        [TypeConverter(typeof(ExpandableObjectConverter))]
    //        public Exposure TestExposure
    //        {
    //            get { return testExposure; }
    //            set { testExposure = value; }
    //        }
    //#endif



    //        [JsonProperty]
    //        public Rectangle RectBarcode = new Rectangle(50, 50, 50, 50);
    //        [JsonProperty]
    //        public Rectangle RectLogo1 = new Rectangle(50, 50, 50, 50);
    //        [JsonProperty]
    //        public Rectangle RectLogo2 = new Rectangle(50, 50, 50, 50);
    //        [JsonProperty]
    //        public Rectangle RectOCR = new Rectangle(50, 50, 50, 50);
    //    } 
    #endregion


    public class TestSpec // : ICloneable 
    {
        public TestSpec()
        {
        }
        private MData_LCD_AREA specLcdArea = new MData_LCD_AREA();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_AREA SpecLcdArea
        {
            get { return specLcdArea; }
            set { specLcdArea = value; }
        }

        private MData_LCD_DUST specLcdDust = new MData_LCD_DUST();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_DUST SpecLcdDust
        {
            get { return specLcdDust; }
            set { specLcdDust = value; }
        }

        private MData_LCD_WHITE specLcdWhite = new MData_LCD_WHITE();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_WHITE SpecLcdWhite
        {
            get { return specLcdWhite; }
            set { specLcdWhite = value; }
        }

        private MData_LCD_GREEN specLcdGreen = new MData_LCD_GREEN();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_GREEN SpecLcdGreen
        {
            get { return specLcdGreen; }
            set { specLcdGreen = value; }
        }

        private MData_LCD_RED specLcdRed = new MData_LCD_RED(); // "Lcd Red");
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_RED SpecLcdRed
        {
            get { return specLcdRed; }
            set { specLcdRed = value; }
        }

        private MData_LCD_BLUE specLcdBlue = new MData_LCD_BLUE(); //"Lcd Blue");
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_BLUE SpecLcdBlue
        {
            get { return specLcdBlue; }
            set { specLcdBlue = value; }
        }

        private MData_LED_BLUE specLedBlue = new MData_LED_BLUE();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LED_BLUE SpecLedBlue
        {
            get { return specLedBlue; }
            set { specLedBlue = value; }
        }

        private MData_TOUCH_KEY specTouchKey = new MData_TOUCH_KEY();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_TOUCH_KEY SpecTouchKey
        {
            get { return specTouchKey; }
            set { specTouchKey = value; }
        }

        private MData_LCD_MCD specLcdMCD = new MData_LCD_MCD();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_MCD SpecLcdMCD
        {
            get { return specLcdMCD; }
            set { specLcdMCD = value; }
        }

        private MData_LCD_COPCRACK specLcdCOPCrack = new MData_LCD_COPCRACK();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_COPCRACK SpecLcdCOPCrack
        {
            get { return specLcdCOPCrack; }
            set { specLcdCOPCrack = value; }
        }

        private MData_LCD_REDDISH specLcdReddish = new MData_LCD_REDDISH();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_LCD_REDDISH SpecLcdReddish
        {
            get { return specLcdReddish; }
            set { specLcdReddish = value; }
        }

        private MData_REAR specRear = new MData_REAR();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public MData_REAR SpecRear
        {
            get { return specRear; }
            set { specRear = value; }
        }

        private Exposure specExposure = new Exposure();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Exposure SpecExposure
        {
            get { return specExposure; }
            set { specExposure = value; }
        }

        private TestList specTestList = new TestList();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TestList SpecTestList
        {
            get { return specTestList; }
            set { specTestList = value; }
        }

        //private FrontCam specFrontCam = new FrontCam(); // Nam jh
        //[JsonProperty]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public FrontCam SpecFrontCam
        //{
        //    get { return specFrontCam; }
        //    set { specFrontCam = value; }

        //}
        [JsonProperty]
        public FrontCam SpecFrontCam = new FrontCam();

        [JsonProperty]
        public string AnywayType = "TypeC";
        [JsonProperty]
        public Rectangle RectBarcode = new Rectangle(840, 523, 862, 1341);
        [JsonProperty]
        public Rectangle RectLogo1 = new Rectangle(326, 647, 374, 1105);
        [JsonProperty]
        public Rectangle RectLogo2 = new Rectangle(2190, 615, 374, 1105);
        //[JsonProperty]
        //public Rectangle RectSearchOCR = new Rectangle(50, 50, 50, 50);
        [JsonProperty]
        public Rectangle RectImei = new Rectangle(3366, 1328, 42, 104);

        

        private SpecLight specLight = new SpecLight();
        [JsonProperty]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SpecLight SpecLight
        {
            get { return specLight; }
            set { specLight = value; }
        }
    }
    public class SpecLight
    {
        public int LogoLightValue { get; set; }
        public int DustLightValue { get; set; }
        public int LogoBackLightValue { get; set; }
        public int LogoBarLightValue { get; set; }
        public int LabelBackLightValue { get; set; }
        public int LabelBarLightValue { get; set; }
        public int LaserBackLightValue { get; set; }    // IMEI BACK
        public int LaserBarLightValue { get; set; }     // IMEI BAR
        
        public SpecLight()
        {
            Clear();
        }
        void Clear()
        {
            LogoLightValue = 0;
            DustLightValue = 255;
            LogoBarLightValue = 0;
            LogoBackLightValue = 100;
            LabelBarLightValue = 0;
            LabelBackLightValue = 100;
            LaserBarLightValue = 30;
            LaserBackLightValue = 100;
        }
    }

    public class TestList
    {
        //public bool SkipRearAndKey { get; set; }
        //public bool TestFront { get; set; }
        public bool TestRear { get; set; }
        public bool TestFrontKey { get; set; }
        public bool TestFrontLed { get; set; }
        public bool TestLaser { get; set; }
        public bool TestLogo { get; set; }
        public bool TestRed { get; set; }
        public bool TestMcd { get; set; }
        public bool TestCOPCrack { get; set; }
        public bool TestReddish { get; set; }
        public TestList()
        {
            Clear();
        }
        void Clear()
        {
            //TestFront = true;
            TestRear = false;
            TestFrontKey = false;
            TestFrontLed = true;
            TestLaser = false;
            TestLogo = false;
            TestRed = true;
            TestMcd = false;
            TestCOPCrack = false;
            TestReddish = false;
        }
    }

    public class Exposure
    {
        public int TestArea { get; set; }
        public int TestDust { get; set; }
        public int TestRed { get; set; }
        public int TestBlue { get; set; }
        public int TestGreen { get; set; }
        public int TestMCD { get; set; }
        public int TestCOPCrack { get; set; }
        public int TestReddish { get; set; }
        public int TestImeiLabel { get; set; }
        public int TestImeiLaser { get; set; }
        public int TestLogo { get; set; }

        public Exposure()
        {
            Clear();
        }
        void Clear()
        {
            TestArea = 350000;
            TestDust = 350000;
            TestRed = 75000;
            TestBlue = 75000;
            TestGreen = 70000;
            TestImeiLabel = 35000;
            TestImeiLaser = 35000;
            TestLogo = 35000;
            TestMCD = 35000;
            TestCOPCrack = 35000;
            TestReddish = 35000;
        }
    }

    public class FrontCam // Nam jh
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public FrontCam()
        {
            Clear();
        }
        void Clear()
        {
            Width = 4608;
            Height = 3288;
            OffsetX = 0;
            OffsetY = 0;
        }

    }

    public class MResult_TOUCH_KEY : ResultBase
    {
        public int TouchKeyBrightJudgeArea = 0;
        public double TouchKeyBrightnessJudgeGap = 0;

        public void Clear()
        {
            TouchKeyBrightJudgeArea = 0;
            TouchKeyBrightnessJudgeGap = 0;
        }
    }

    public class MData_TOUCH_KEY
    {
        //public int DefectBoxThickness { get; set; }
        public int DefectBoxThickness;
        public int TouchKeyBrightAreaSpec { get; set; }

        //public int m_nBrightness_BackKeyBrightnessLowSpec { get; set; }
        //public int m_nBrightness_BackKeyBrightnessUpperSpec { get; set; }

        //public int m_nBrightness_MenuKeyBrightnessLowSpec { get; set; }
        //public int m_nBrightness_MenuKeyBrightnessUpperSpec { get; set; }

        public MData_TOUCH_KEY()
        {
            Clear();
        }
        void Clear()
        {
            DefectBoxThickness = 10;
            TouchKeyBrightAreaSpec = 3;
            //m_nBrightness_BackKeyBrightnessLowSpec = 50;
            //m_nBrightness_BackKeyBrightnessUpperSpec = 200;
            //m_nBrightness_MenuKeyBrightnessLowSpec = 50;
            //m_nBrightness_MenuKeyBrightnessUpperSpec = 215;
        }

    }

    public class MResult_TOUCH_BACKKEY : ResultBase
    {
        //public double m_dBackBrightnessMean = 0;
        public double m_dDiffArea = 0;
        public Mat imgNGBright = null;
        public Mat imgNGDiff = null;

        public void Clear()
        {
            //  m_dBackBrightnessMean = 0;
            m_dDiffArea = 0;
            imgNGBright = null;
            imgNGDiff = null;
        }
    }

    public class MResult_TOUCH_MENUKEY : ResultBase
    {
        //  public double m_dBackBrightnessMean = 0;
        public double m_dDiffArea = 0;
        //public Mat imgNGBright = null;
        //public Mat imgNGDiff = null;
        public void Clear()
        {
            //   m_dBackBrightnessMean = 0;
            m_dDiffArea = 0;
            //imgNGBright = null;
            //imgNGDiff = null;
        }
    }

    public class MResult_LCD_DUST : ResultBase
    {
        public double m_dArea = 0;
        public double m_dContour = 0;
        public void Clear()
        {
            m_dArea = 0;
        }
    }

    public class MData_LCD_DUST : SpecBase
    {
        public int DustThreshold { get; set; }
        public int JudgeDustAreaUL { get; set; }
        public int JudgeDustMaxNumUL { get; set; }
        //public int DustOffsetPixel { get; set; }
        public int DustOffsetPixel;
        public MData_LCD_DUST()
        {
            Clear();
        }
        void Clear()
        {
            DustThreshold = 35;
            JudgeDustAreaUL = 50000;
            JudgeDustMaxNumUL = 4000;
            DustOffsetPixel = 4;
        }
    }

    public class MResult_LCD_RED : ResultBase
    {
        public int m_nBlackDot_JudgeSize = 0;
        public void Clear()
        {
            m_nBlackDot_JudgeSize = 0;
        }
    }

    public class MData_LCD_RED : SpecBase
    {
        public int BlackDot_RFindBlackDotBlockSize { get; set; }
        public double BlackDot_RFindBlackDotThreshold { get; set; }
        public int BlackDot_RJudgeSizeUL { get; set; }
        //public int m_nBlackDot_RPsmThreshold { get; set; }
        //public int m_nBlackDot_RPsmThreshold;

        public MData_LCD_RED()
        {
            Clear();
        }
        void Clear()
        {
            BlackDot_RFindBlackDotBlockSize = 41;
            BlackDot_RFindBlackDotThreshold = 20;
            BlackDot_RJudgeSizeUL = 10;
            //m_nBlackDot_RPsmThreshold = 15;
        }
    }

    public class MResult_LED_BLUE : ResultBase
    {
        //public Mat imgNGBright = null;
        //public Mat imgNGDiff = null;
        //public Mat DiffResult = null;
        //public Mat BrightResult = null;
        public double m_dblueLedBrightnessVal = 0;
        public int m_nDiffWH_BLDiffJudgeArea = 0;
        //public double m_dOverBrightArea = 0;

        public void Clear()
        {
            //imgNGBright = null;
            //imgNGDiff = null;
            m_dblueLedBrightnessVal = 0;
            m_nDiffWH_BLDiffJudgeArea = 0;
            //m_dOverBrightArea = 0;
        }
    }

    public class MData_LED_BLUE
    {
        public int Brightness_BLJudgeBrightLL { get; set; }
        public int Brightness_BLJudgeBrightUL { get; set; }

        //public int m_nColorRate_BLJudgeColorRateLL { get; set; }
        //public int m_nColorRate_BLJudgeColorRateUL { get; set; }
        //public int m_nDiffWH_BLDiffThreshold { get; set; }
        public int DiffWH_BLJudgeSizeUL { get; set; }
        public int DiffWH_BLOffsetSize { get; set; }
        public int DiffWH_BLFindBlackDotThreshold { get; set; }
        public MData_LED_BLUE()
        {
            Clear();
        }
        void Clear()
        {
            Brightness_BLJudgeBrightLL = 30;
            Brightness_BLJudgeBrightUL = 150;
            //m_nColorRate_BLJudgeColorRateLL = 20;
            //m_nColorRate_BLJudgeColorRateUL = 50;
            //m_nDiffWH_BLDiffThreshold = 20;
            DiffWH_BLJudgeSizeUL = 6;
            DiffWH_BLOffsetSize = 10;
            DiffWH_BLFindBlackDotThreshold = 30;
        }
    }

    public class MResult_LCD_WHITE : ResultBase
    {
        public int m_nBlackDot_JudgeSize = 0;
        //public int m_nMuraDot_JudgeSize = 0;
        //public Mat imgNGResultCorner = null;
        public int m_nMungNgCnt = 0;
        //public int m_nMungNgCntCorner = 0;

        //public Mat imgNGResultL = null;
        //public Mat imgNGResultR = null;
        //public int m_nMungNgCntL = 0;
        //public int m_nMungNgCntR = 0;

        public void Clear()
        {
            m_nBlackDot_JudgeSize = 0;
            //m_nMuraDot_JudgeSize = 0;
            ImageResult = null;
            m_nMungNgCnt = 0;

            // imgNGResult = null;
            //imgNGResultCorner = null;
            //m_nMungNgCntCorner = 0;
            //imgNGResultL = null;
            //imgNGResultR = null;
            //m_nMungNgCntL = 0;
            //m_nMungNgCntR = 0;
        }
    }

    public class MData_LCD_WHITE : SpecBase
    {
        public bool TestCorner { get; set; }
        public double MungThreshold { get; set; }
        public int Mung_WJudgeSizeUL { get; set; }
        //public int m_nMungAreaHL { get; set; }

        //public double MungThresholdLeft { get; set; }
        //public int Mung_WJudgeSizeULLeft { get; set; }
        //public int m_nMungAreaHLLeft { get; set; }
        //public int Mung_WJudgeSizeULCorner { get; set; }
        public int CornerLength { get; set; }
        //public double m_dAreaThreshold { get; set; }
        //public int CornerLengthPixel { get; set; }
        //public int SideOffsetPixel_white { get; set; }

        //public int m_nMung_MaxArea { get; set; }
        public int GapSpec;
        public MData_LCD_WHITE()
        {
            Clear();
        }
        void Clear()
        {
            MungThreshold = 20;
            Mung_WJudgeSizeUL = 2;
            CornerLength = 200;
            TestCorner = true;
            GapSpec = 30;
        }
    }



    public class MResult_LCD_BLUE : ResultBase
    {
        public int m_nBlackDot_JudgeSize = 0;

        public void Clear()
        {
            m_nBlackDot_JudgeSize = 0;
        }
    }

    public class MData_LCD_BLUE : SpecBase
    {
        public int BlackDot_BFindBlackDotBlockSize { get; set; }
        public double BlackDot_BFindBlackDotThreshold { get; set; }
        public int BlackDot_BJudgeSizeUL { get; set; }
        //public int m_nBlackDot_BPsmThreshold { get; set; }
        //public int m_nBlackDot_BPsmThreshold;

        public MData_LCD_BLUE()
        {
            Clear();
        }

        void Clear()
        {
            BlackDot_BFindBlackDotBlockSize = 21;
            BlackDot_BFindBlackDotThreshold = 11;
            BlackDot_BJudgeSizeUL = 10;
            //m_nBlackDot_BPsmThreshold = 13;
        }
    }

    public class MResult_LCD_GREEN : ResultBase
    {
        //BoundingRect Max(width,height)
        public int m_nBlackDot_JudgeSize = 0;
        //BoundingRect (L+W)/2
        public double m_dBlackDot_JudgeWHAvg = 0;
        //contour area
        public double m_dBlackDot_JudgeArea = 0;
        //MinRect Max(width,height)
        public double m_dBlackDot_JudgeSize_MinRect = 0;
        //MinRect (L+W)/2
        public double m_dBlackdot_JudgeWHAvg_MinRect = 0;
        
        public void Clear()
        {
            m_nBlackDot_JudgeSize = 0;
            m_dBlackDot_JudgeSize_MinRect = 0;
        }
    }

    public class MData_LCD_GREEN : SpecBase
    {
        public int BlackDot_GFindBlackDotBlockSize { get; set; }
        public double BlackDot_GFindBlackDotThreshold { get; set; }
        public int BlackDot_GJudgeSizeUL { get; set; }

        //public int m_nBlackDotSpec_MaxArea { get; set; }    // Bae 2017.05.06. Condition Added : Not only "Dust Rect", But also "Dust Min Area"

        //public int m_nBlackDot_GPsmThreshold { get; set; }
        //public int m_nBlackDot_GOffset { get; set; }
        //public bool useCompare { get; set; }

        public MData_LCD_GREEN()
        {
            Clear();
        }
        void Clear()
        {
            BlackDot_GFindBlackDotBlockSize = 23;
            BlackDot_GFindBlackDotThreshold = 6;
            BlackDot_GJudgeSizeUL = 8;
            //m_nBlackDot_GOffset = 100;
            //useCompare = false;
            //m_nBlackDot_GPsmThreshold = 15;
        }
    }


    public class MResult_LCD_AREA : ResultBase
    {
        public double m_iDisplayArea = 0;
        public double m_iLedArea = 0;
        public double m_iBackArea = 0;
        public double m_iMenuArea = 0;

        public void Clear()
        {
            m_iDisplayArea = 0;
            m_iLedArea = 0;
            m_iBackArea = 0;
            m_iMenuArea = 0;
            ImageResult = null;
        }
    }

    public class MData_LCD_AREA : SpecBase
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CRect LedRoughRoi;//{ get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CRect BackRoughRoi;// { get; set; }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CRect MenuRoughRoi;// { get; set; }

        public int DisplayThreshold;// { get; set; }
        public int LedThreshold; //{ get; set; }
        public int KeyThreshold;// { get; set; }
        //public int SideOffsetPixel { get; set; }
        
        public double JudgeDisplayAreaLL { get; set; }
        public double JudgeDisplayAreaUL { get; set; }
        public double JudgeMenuAreaLL { get; set; }
        public double JudgeMenuAreaUL { get; set; }
        public double JudgeBackAreaLL { get; set; }
        public double JudgeBackAreaUL { get; set; }
        //public double m_dJudgeLedAreaLL { get; set; }
        //public double m_dJudgeLedAreaUL { get; set; }
        public int SideOffsetPixel { get; set; }
        public int SideOffsetPixel_Green { get; set; }
        public int JudgeErrorCount;
        public int JudgeErrorBright; 
        //public int SideOffsetPixel_White { get; set; }

        //    public CRect LedRoughRoi;
        //    public CRect BackRoughRoi;
        //    public CRect MenuRoughRoi;
        //    public int DisplayThreshold;
        //    public int LedThreshold;
        //    public int KeyThreshold;
        //    public int SideOffsetPixel;
        //    public double JudgeDisplayAreaLL;
        //    public double JudgeDisplayAreaUL;
        //    public double JudgeMenuAreaLL;
        //    public double JudgeMenuAreaUL;
        //    public double JudgeBackAreaLL;
        //    public double JudgeBackAreaUL;
        //    public double m_dJudgeLedAreaLL;
        //    public double m_dJudgeLedAreaUL;


        public MData_LCD_AREA()
        {
            Clear();
        }
        void Clear()
        {
            MenuRoughRoi = new CRect(120, 130, 1460, 180);
            BackRoughRoi = new CRect(120, 130, 260, 180);
            LedRoughRoi = new CRect(-140, 100, 1710, 100);

            DisplayThreshold = 150;
            KeyThreshold = 40;
            LedThreshold = 120;
            SideOffsetPixel = 3;
            SideOffsetPixel_Green = 10;
            JudgeDisplayAreaLL = 5000000;
            JudgeDisplayAreaUL = 10000000;
            JudgeMenuAreaLL = 2000;
            JudgeMenuAreaUL = 15000;
            JudgeBackAreaLL = 2500;
            JudgeBackAreaUL = 12000;
            JudgeErrorCount = 5;
            JudgeErrorBright = 253;
            //m_dJudgeLedAreaLL = 1000;
            //m_dJudgeLedAreaUL = 3200;
        }

    }

    public class MResult_REAR : ResultBase
    {
        public string m_nImei = "";
        public Bitmap areaNgImage;
        public void Clear()
        {
            m_nImei = "";
            ImageResult = null;
        }
    }

    public class MData_REAR : SpecBase
    {
        public int LogoRegOffset { get; set; }
        public int ImeiRegOffset { get; set; }
        public int LogoMatchingRate { get; set; }
        public int OcrThreshold { get; set; }
        public int OcrThresIdx { get; set; }
        public int LogoCount { get; set; }
        public int IMEILength { get; set; }
        public int OCROffsetX { get; set; }
        public int OCROffsetY { get; set; }
        public MData_REAR()
        {
            Clear();
        }
        void Clear()
        {
            LogoRegOffset = 20;
            ImeiRegOffset = 100;
            LogoMatchingRate = 70;
            OcrThreshold = 7;
            OcrThresIdx = 3;
            LogoCount = 1;
            IMEILength = 15;
            OCROffsetX = 0;
            OCROffsetY = 0;
        }
    }

    public class MResult_LCD_MCD : ResultBase
    {
        public int BrightLine_McdJudgeSizeLL = 0;

        public void Clear()
        {
            BrightLine_McdJudgeSizeLL = 0;
        }
    }
    public class MData_LCD_MCD : SpecBase
    {
        public int BrightLine_McdThreshold { get;set; }
        public int BrightLine_McdJudgeSizeLL { get; set; }

        public MData_LCD_MCD()
        {
            this.Clear();
        }

        private void Clear()
        {
            this.BrightLine_McdThreshold = 10;
            this.BrightLine_McdJudgeSizeLL = 1000;
        }
    }

    public class MResult_LCD_REDDISH : ResultBase
    {
        public double m_iWhiteColorBalanceRG_TB  = 0;
        public double m_iWhiteColorBalanceRG_LR = 0;
        public double m_iWhiteColorBalanceRG_Center = 0;
        public bool m_bWhiteColorBalanceRG_TB_Result = true;
        public bool m_bWhiteColorBalanceRG_LR_Result = true;
        public bool m_bWhiteColorBalanceRG_Center_Result = true;
        public int m_iWhiteColorBalanceRG_OverStep_3_RegionCnt = 0;
        public int m_iWhiteColorBalanceRG_OverStep_2_RegionCnt = 0;
        public double m_iWhiteColorBalanceRG_Max = 0;

        public void Clear()
        {
            
        }
    }
    public class MData_LCD_REDDISH : SpecBase
    {
        public int ColorShading_Exception_Area { get; set; }
        public int AreaColorDiffBlue_WColumnSplitCnt { get; set; }
        public int AreaColorDiffBlue_WRowSplitCnt { get; set; }
        public int ColorShading_TB_Area { get; set; }
        
        public double ColorBalance_Step_2 { get; set; }
        public double ColorBalance_Step_3 { get; set; }
        //public int m_iColorShading_RG_TB_Rate { get; set; }
        public double ColorShading_RG_TB_Upper_Rate { get; set; }
        public double ColorShading_RG_LR_Upper_Rate { get; set; }
        public double ColorShading_RG_Center_Upper_Rate { get; set; }
        public double ColorShading_RG_TB_Lower_Rate { get; set; }
        public double ColorShading_RG_LR_Lower_Rate { get; set; }
        public double ColorShading_RG_Center_Lower_Rate { get; set; }
        public int ColorShading_LR_Area { get; set; }

        
        public MData_LCD_REDDISH()
        {
            this.Clear();
        }

        private void Clear()
        {
            
        }
    }
    public class MResult_LCD_COPCRACK : ResultBase
    {
        public int m_nBlackDot_JudgeSize = 0;

        public void Clear()
        {
            m_nBlackDot_JudgeSize = 0;
        }
    }

    public class MData_LCD_COPCRACK : SpecBase
    {
        public int BlackDot_BFindBlackDotBlockSize { get; set; }
        public int BlackDot_BFindBlackDotThreshold { get; set; }
        public int BlackDot_BJudgeSizeUL { get; set; }
        public MData_LCD_COPCRACK()
        {
            Clear();
        }

        void Clear()
        {
            BlackDot_BFindBlackDotBlockSize = 31;
            BlackDot_BFindBlackDotThreshold = 11;
            BlackDot_BJudgeSizeUL = 10;
        }
    }



}
