using AutoInspection.Forms;
using AutoInspection.sec;
using AutoInspection.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AutoInspection;
using OpenCvSharp.CPlusPlus;
using AutoInspection.sec.GUMI;
using OpenCvSharp.Extensions;

namespace AutoInspection_GUMI
{
    public enum AverageColor
    {
        NONE,
        BLUE,
        GREEN,
        RED
    }
    public partial class FrmMainCh2 : Form // , IUserInterface
    {
        public static FrmMainCh2 instance; 
        public Controller controller;

        string sImei;
        int[] picwidth = { 0, 0 };
        int[] picheight = { 0, 0 };

        public Rect_Obj RectInsp;
        double scaleX;
        double scaleY;
        public int wheel_cnt = 0;
        string modelName;
        public int Channel;

        public FrmMainCh2()
        {
            instance = this; 
            InitializeComponent();
        }

        private void frmMainCh2_Load(object sender, EventArgs e)
        {
            controller = new Controller(this);
            controller.CreateDevices();
            controller.InitDevices();

            controller.DeleteImage();
			//form load,close 때 조명 off
            controller.TurnOffAllLight();

            InitFormControl();

            UpdateProductionInfo();
            modelName = Path.GetFileNameWithoutExtension(Config.sCurrnetSpecFile);
            DisplayModelName(modelName);
            Log log = new Log(this, Config.sPathLog, modelName);


            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            DateTime dt = System.IO.File.GetLastWriteTime(assembly.Location);

			string title = "PQC Inspector " + Config.Version + " ( Build :" + dt.ToString("yyyy/MM/dd HH:mm:ss") + ")";
			Text = title;

			Log.AddLog(Environment.NewLine);
			Log.AddLog(Environment.NewLine);
			Log.AddLog(title);

			Log.AddPmLog(Environment.NewLine);
			Log.AddPmLog(Environment.NewLine);
			Log.AddPmLog(title);

			CheckForIllegalCrossThreadCalls = false;
		}

        void InitFormControl()
        {
            //Init Form
            //탭컨트롤 탭 숨기기
            Rectangle rect = new Rectangle(tabPage1.Left, tabPage1.Top, tabPage1.Width, tabPage1.Height);
            tabControl1.Region = new Region(rect);

            //Teaching Rect
            RectInsp = new Rect_Obj(picTeaching, 50, 50, 100, 100, "region", true);

            picwidth[0] = picTeaching.Width;
            picwidth[1] = picTeaching.Width * 2;
            picheight[0] = picTeaching.Height;
            picheight[1] = picTeaching.Height * 2;

            DisplaySpecInfo();
            DisplaySystemInfo();


            cbbOCRThresType.SelectedIndex = controller.scenarioManger.testSpec.SpecRear.OcrThresIdx;

            tbxCtrlLight.Text = controller.scenarioManger.testSpec.SpecLight.DustLightValue.ToString();

            tbxBackLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBackLightValue.ToString();
            tbxBarLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBarLightValue.ToString();
            tbxBackLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBackLightValue.ToString();
            tbxBarLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBarLightValue.ToString();
            tbxBackLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBackLightValue.ToString();
            tbxBarLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBarLightValue.ToString();

            trbOCR.Value = controller.scenarioManger.testSpec.SpecRear.OcrThreshold;

        }



        #region Layout
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                timer1.Start();
            }
            else
            {
                StopLiveCam();
                timer1.Stop();
            }
        }
        void btnUnderBarIcon_Click(object sender, EventArgs e)
        {
            ClearBtn();
            PictureBox btn = sender as PictureBox;
            if (btn.Name.Contains("Auto"))
            {
                btnAuto.Image = AutoInspection.Properties.Resources.but_01auto_select;
                tabControl1.SelectTab(0);
            }
            else if (btn.Name.Contains("Manual"))
            {
                btnManual.Image = AutoInspection.Properties.Resources.but_02manual_select;
                tabControl1.SelectTab(1);
            }
            else if (btn.Name.Contains("Teach"))
            {
                btnTeach.Image = AutoInspection.Properties.Resources.but_04teach_select;
                tabControl1.SelectTab(2);
            }
            else if (btn.Name.Contains("Data"))
            {
                btnData.Image = AutoInspection.Properties.Resources.but_03data_select;
                DisplaySpecInfo();
                DisplaySystemInfo();
                tabControl1.SelectTab(3);
            }
            else
            {
                controller.IOInitiate();
                Thread.Sleep(300);
                btnExit.Image = AutoInspection.Properties.Resources.but_07exit_select;
                if (MessageBox.Show("Do you want to terminate this program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

					Log.AddLog("Program Ended...");
					Log.AddPmLog("Program Ended...");
					Log.SaveLog();
                    Application.Exit();
                }
            }
        }

        void btnUser_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btnUser.Text == "User")
            {
                Password pw = new Password();
                pw.ShowDialog();
                if (PW.Equals("samsung"))
                {
                    btnUser.Text = "ADMIN";
                    trbOCR.Enabled = true;
                    tbxOCR.Enabled = true;
                    trbWidth.Enabled = true;
                    trbHeight.Enabled = true;
                    trbXoffset.Enabled = true;
                    trbYoffset.Enabled = true;
                }
            }
            else
            {
                btnUser.Text = "User";
                trbOCR.Enabled = false;
                tbxOCR.Enabled = false;
                trbWidth.Enabled = false;
                trbHeight.Enabled = false;
                trbXoffset.Enabled = false;
                trbYoffset.Enabled = false;
                PW = "";
            }
        }

        void ClearBtn()
        {
            btnAuto.Image = AutoInspection.Properties.Resources.but_01auto_disable;
            btnManual.Image = AutoInspection.Properties.Resources.but_02manual_disable;
            btnTeach.Image = AutoInspection.Properties.Resources.but_04teach_disable;
            btnData.Image = AutoInspection.Properties.Resources.but_03data_disable;
            btnExit.Image = AutoInspection.Properties.Resources.but_07exit_disable;
        }
        #endregion

        #region AutoTab
        void btnAutoTab_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Contains("Start"))
            {
                controller.StartTest();
            }

            else if (btn.Name.Contains("Stop"))
            {
                if (btnStop.Text == "TEST")
                {
                    
                    controller.isDryRunning = true;
                    controller.StartTest();
                    btnStop.Text = "STOP";

                }
                else
                {
                    
                    btnStop.Text = "TEST";
                    controller.isDryRunning = false;
                }
				Log.AddLog("test started");
            }
            else if (btn.Name.Contains("Reset"))
            {
                if (MessageBox.Show("Do you want to reset?", "Check", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Config.CountPass = 0;
                    Config.CountFail = 0;
                    Config.saveTofile();
                }
                UpdateProductionInfo();
            }
        }
        #endregion

        #region ManualTab
        
        private void btnLcdCapture_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                if(btn.Name.Contains("LcdArea"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_AREA] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if(btn.Name.Contains("LcdDust"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_DUST] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if (btn.Name.Contains("LcdReddish"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_REDDISH] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if(btn.Name.Contains("LcdRed"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_RED] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if(btn.Name.Contains("LcdBlue"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_BLUE] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if (btn.Name.Contains("LcdGreen"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_GREEN] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if (btn.Name.Contains("LcdMCD"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_MCD] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);
                else if (btn.Name.Contains("LcdCOPCrack"))
                    controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_COPCRACK] = new OpenCvSharp.CPlusPlus.Mat(f.FileName);

                WriteLog("Load Lcd Image - " + f.FileName);
            }
        }
        private void btnLcdAreaTest_Click(object sender, EventArgs e)
        {
            //
            Stopwatch timer = new Stopwatch();
            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd Area start...");

            MResult_LCD_AREA ResultLcd = new MResult_LCD_AREA();

            bool _result = controller.scenarioManger.visionTest.AreaProcess
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_AREA],
                controller.scenarioManger.testSpec,
                ref ResultLcd
                );

            WriteLog(string.Format("Result     : {0}", _result == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Lcd Area end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLcdDustTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            WriteLog("--------------------------------------------");
            WriteLog("Lcd Dust start...");

            MData_LCD_AREA specLcdArea = controller.scenarioManger.testSpec.SpecLcdArea;
            MData_LCD_DUST specLcdDust = controller.scenarioManger.testSpec.SpecLcdDust;
            MResult_LCD_AREA ResultLcd = new MResult_LCD_AREA();
            MResult_LCD_DUST ResultDust = new MResult_LCD_DUST();
            //sh32.heo
            //use empty image for dust
            bool _result = true;
            if (controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_DUST] != null)
            {
                _result = controller.scenarioManger.visionTest.DustProcess(
                          controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_DUST],
                          controller.scenarioManger.testSpec, ref ResultLcd, ref ResultDust);
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_DUST] = null;
            }
            else
            {
                _result = controller.scenarioManger.visionTest.DustProcess(
                new Mat(controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_AREA].Size(), MatType.CV_8UC3),
                controller.scenarioManger.testSpec, ref ResultLcd, ref ResultDust);
            }


            WriteLog(string.Format("Dust    : {0}, Spec( 0 ~ {1} ), Result : {2}",
                   ResultDust.m_dArea, specLcdDust.JudgeDustAreaUL, _result.ToString()));

            bool bFinalresult = true;
			//Led 결과 표시 X 
            //_result = true;
            //if (ResultLcd.m_iLedArea < specLcdArea.m_dJudgeLedAreaLL || ResultLcd.m_iLedArea > specLcdArea.m_dJudgeLedAreaUL)
            //{
            //    bFinalresult = false;
            //    _result = false;
            //}
            //WriteLog(string.Format("LedArea    : {0}, Spec( {1} ~ {2} ), Result : {3}",
            //    ResultLcd.m_iLedArea, specLcdArea.m_dJudgeLedAreaLL, specLcdArea.m_dJudgeLedAreaUL, _result.ToString()));

            _result = true;
            if (ResultLcd.m_iDisplayArea < specLcdArea.JudgeDisplayAreaLL || ResultLcd.m_iDisplayArea > specLcdArea.JudgeDisplayAreaUL)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("DisplayArea: {0}, Spec( {1} ~ {2} ), Result : {3}",
                ResultLcd.m_iDisplayArea, specLcdArea.JudgeDisplayAreaLL, specLcdArea.JudgeDisplayAreaUL, _result.ToString()));

            _result = true;
            if (ResultLcd.m_iBackArea < specLcdArea.JudgeBackAreaLL || ResultLcd.m_iBackArea > specLcdArea.JudgeBackAreaUL)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("BackArea   : {0}, Spec( {1} ~ {2} ), Result : {3}",
                ResultLcd.m_iBackArea, specLcdArea.JudgeBackAreaLL, specLcdArea.JudgeBackAreaUL, _result.ToString()));

            _result = true;
            if (ResultLcd.m_iMenuArea < specLcdArea.JudgeMenuAreaLL || ResultLcd.m_iMenuArea > specLcdArea.JudgeMenuAreaUL)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("MenuArea   : {0}, Spec( {1} ~ {2} ), Result : {3}",
                ResultLcd.m_iMenuArea, specLcdArea.JudgeMenuAreaLL, specLcdArea.JudgeMenuAreaUL, _result.ToString()));


            WriteLog(string.Format("Result     : {0}", bFinalresult == true ? "PASS" : "FAIL"));

            timer.Stop();
            WriteLog(string.Format("Lcd Dust end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLcdTouchTest_Click(object sender, EventArgs e)
        {
            //
            Stopwatch timer = new Stopwatch();

            MData_TOUCH_KEY specTouchKey = controller.scenarioManger.testSpec.SpecTouchKey;
            MResult_TOUCH_KEY ResultTouchKey = new MResult_TOUCH_KEY();
            MResult_TOUCH_BACKKEY ResultTouchBackKey = new MResult_TOUCH_BACKKEY();
            MResult_TOUCH_MENUKEY ResultTouchMenuKey = new MResult_TOUCH_MENUKEY();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Touch Key Start...");
            WriteLog(Json.GetString(specTouchKey));

            controller.scenarioManger.visionTest.TouchKey
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_AREA],
                specTouchKey,
                ref ResultTouchKey,
                ref ResultTouchBackKey,
                ref ResultTouchMenuKey);

            bool bFinalresult = true;

            bool _result = true;
            if (ResultTouchKey.TouchKeyBrightJudgeArea != specTouchKey.TouchKeyBrightAreaSpec)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("Contour Count  : {0}, Spec( {1} ), Result : {2}",
                ResultTouchKey.TouchKeyBrightJudgeArea, specTouchKey.TouchKeyBrightAreaSpec, _result.ToString()));

            _result = true;
            if (ResultTouchBackKey.m_dDiffArea > 0)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("BackDiffArea   : {0}, Spec( 0 ), Result : {1}",
                ResultTouchBackKey.m_dDiffArea, _result.ToString()));

            _result = true;
            if (ResultTouchMenuKey.m_dDiffArea > 0)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("MenuDiffArea   : {0}, Spec( 0 ), Result : {1}",
                ResultTouchMenuKey.m_dDiffArea, _result.ToString()));

            WriteLog(string.Format("Result         : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Touch Key end {0}ms...", timer.ElapsedMilliseconds));

            return;
        }
        private void btnLcdWhiteTest_Click(object sender, EventArgs e)
        {

            Stopwatch timer = new Stopwatch();

            MData_LCD_WHITE specLcdWhite = controller.scenarioManger.testSpec.SpecLcdWhite;
            MResult_LCD_WHITE Result = new MResult_LCD_WHITE();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd White start...");
            WriteLog(Json.GetString(specLcdWhite));

            controller.scenarioManger.visionTest.LcdWhite
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_AREA],
                specLcdWhite,
                ref Result);

            bool bFinalresult = true;
            if (Result.m_nMungNgCnt > 0 )
            {
                bFinalresult = false;
            }
            WriteLog(string.Format("Mung Count : {0}, Spec( 0 ), Result : {1}",
                Result.m_nMungNgCnt, bFinalresult));
            WriteLog(string.Format("Result   : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Lcd White end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLcdRedTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();

            MData_LCD_RED specLcdRed = controller.scenarioManger.testSpec.SpecLcdRed;
            MResult_LCD_RED Result = new MResult_LCD_RED();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd Red start...");
            WriteLog(Json.GetString(specLcdRed));

            controller.scenarioManger.visionTest.LcdRed
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_RED],
                specLcdRed,
                ref Result);

            bool bFinalresult = true;
            if (Result.m_nBlackDot_JudgeSize > specLcdRed.BlackDot_RJudgeSizeUL)
            {
                bFinalresult = false;
            }

            WriteLog(string.Format("BlackDot : {0}, Spec( 0 ~ {1})", Result.m_nBlackDot_JudgeSize, specLcdRed.BlackDot_RJudgeSizeUL));
            WriteLog(string.Format("Result   : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Lcd Red end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLcdBlueTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();

            MData_LCD_BLUE specLcdBlue = controller.scenarioManger.testSpec.SpecLcdBlue;
            MResult_LCD_BLUE Result = new MResult_LCD_BLUE();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd Blue start...");
            WriteLog(Json.GetString(specLcdBlue));

            controller.scenarioManger.visionTest.LcdBlue
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_BLUE],
                specLcdBlue,
                ref Result);


            bool bFinalresult = true;
            if (Result.m_nBlackDot_JudgeSize > specLcdBlue.BlackDot_BJudgeSizeUL)
                bFinalresult = false;

            WriteLog(string.Format("BlackDot  : {0}, Spec( 0 ~ {1})", Result.m_nBlackDot_JudgeSize, specLcdBlue.BlackDot_BJudgeSizeUL));
            WriteLog(string.Format("Result    : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Lcd Blue end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLedBlueTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();

            MData_LED_BLUE specLedBlue = controller.scenarioManger.testSpec.SpecLedBlue;
            MResult_LED_BLUE Result = new MResult_LED_BLUE();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Led White start...");
            WriteLog(Json.GetString(specLedBlue));

            controller.scenarioManger.visionTest.LedBlue
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_BLUE],
                specLedBlue,
                ref Result);

            bool bFinalresult = true;
            bool _result = true;

            if (Result.m_dblueLedBrightnessVal < specLedBlue.Brightness_BLJudgeBrightLL
                || Result.m_dblueLedBrightnessVal > specLedBlue.Brightness_BLJudgeBrightUL)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("Brightness     : {0}, Spec({0} ~ {1}), Result : {2}", Result.m_dblueLedBrightnessVal,
                specLedBlue.Brightness_BLJudgeBrightLL, specLedBlue.Brightness_BLJudgeBrightUL, _result));

            if (Result.m_nDiffWH_BLDiffJudgeArea > specLedBlue.DiffWH_BLJudgeSizeUL)
            {
                bFinalresult = false;
                _result = false;
            }
            WriteLog(string.Format("DiffArea       : {0}, Spec( 0 ~ {1}), Result : {2}", Result.m_nDiffWH_BLDiffJudgeArea,
                specLedBlue.DiffWH_BLJudgeSizeUL, _result));

            WriteLog(string.Format("Result         : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Led Blue end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        private void btnLcdGreenTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();

            MData_LCD_GREEN specLcdGreen = controller.scenarioManger.testSpec.SpecLcdGreen;
            MResult_LCD_GREEN Result = new MResult_LCD_GREEN();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd Green start...");
            WriteLog(Json.GetString(specLcdGreen));

            controller.scenarioManger.visionTest.LcdGreen
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_GREEN],
                specLcdGreen,
                ref Result,
                controller);

            bool bFinalresult = true;
            if (Result.m_nBlackDot_JudgeSize > specLcdGreen.BlackDot_GJudgeSizeUL)
                bFinalresult = false;

            WriteLog(string.Format("BlackDot : {0}, Spec( 0 ~ {1})", Result.m_nBlackDot_JudgeSize, specLcdGreen.BlackDot_GJudgeSizeUL));
            WriteLog(string.Format("Result   : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            timer.Stop();
            WriteLog(string.Format("Lcd Green end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }
        #endregion

        #region TeachingTab
        int zoomCnt = 0;
        enum InspectionArea
        {
            LED, MENUKEY, BACKKEY
        }
        enum Direction
        {
            LEFT, RIGHT, UP, DOWN
        }

		CameraParams param;

        //CtrlCam
        void btnTeachingTab_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Contains("Live"))
            {
                if (btnLive.Text == "LIVE START")
                {
                    StartLiveCam();
                }
                else
                {
                    StopLiveCam();
                }
            }
            else if (btn.Name.Contains("ExposureLoad"))
            {
                try
                {
                    if (cbbCtrlCam.Text == "TEST_DUST")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestDust;
                    else if (cbbCtrlCam.Text == "TEST_AREA")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestArea;
                    else if (cbbCtrlCam.Text == "TEST_RED")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestRed;
                    else if (cbbCtrlCam.Text == "TEST_BLUE")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestBlue;
                    else if (cbbCtrlCam.Text == "TEST_GREEN")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestGreen;
                    else if (cbbCtrlCam.Text == "TEST_LASER")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestImeiLaser;
                    else if (cbbCtrlCam.Text == "TEST_LABEL")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestImeiLabel;
                    else if (cbbCtrlCam.Text == "TEST_LOGO")
                        trbExposure.Value = controller.scenarioManger.testSpec.SpecExposure.TestLogo;
                }
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
				}
            }
            else if (btn.Name.Contains("ExposureSave"))
            {
                try
                {
                    if (!PW.Equals("samsung"))
                    {
                        MessageBox.Show("User can't save");
                        return;
                    }
                    if (cbbCtrlCam.Text == "TEST_DUST")
                        controller.scenarioManger.testSpec.SpecExposure.TestDust = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_AREA")
                        controller.scenarioManger.testSpec.SpecExposure.TestArea = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_RED")
                        controller.scenarioManger.testSpec.SpecExposure.TestRed = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_BLUE")
                        controller.scenarioManger.testSpec.SpecExposure.TestBlue = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_GREEN")
                        controller.scenarioManger.testSpec.SpecExposure.TestGreen = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_LASER")
                        controller.scenarioManger.testSpec.SpecExposure.TestImeiLaser = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_LABEL")
                        controller.scenarioManger.testSpec.SpecExposure.TestImeiLabel = trbExposure.Value;
                    else if (cbbCtrlCam.Text == "TEST_LOGO")
                        controller.scenarioManger.testSpec.SpecExposure.TestLogo = trbExposure.Value;
                }
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
                }
                controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            }
            else if (btn.Name.Contains("LoadImage"))
            {
                OpenFileDialog f = new OpenFileDialog();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    picTeaching.Image = new Bitmap(f.FileName);
                }
            }
            else if (btn.Name.Contains("SaveImage"))
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "Bitmap File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    picTeaching.Image.Save(f.FileName);
                }
            }

            else if (btn.Name.Contains("ZoomIn"))
            {
                try
                {
                    if (picTeaching.Image != null && zoomCnt == 0)
                    {
                        zoomCnt++;
                        RectInsp._Rect.X = (int)(RectInsp._Rect.X * (1 + zoomCnt));
                        RectInsp._Rect.Y = (int)(RectInsp._Rect.Y * (1 + zoomCnt));
                        RectInsp._Rect.Width = (int)(RectInsp._Rect.Width * (1 + zoomCnt));
                        RectInsp._Rect.Height = (int)(RectInsp._Rect.Height * (1 + zoomCnt));
                    }

                    picTeaching.Width = picwidth[zoomCnt];
                    picTeaching.Height = picheight[zoomCnt];

                    scaleX = (double)picTeaching.Image.Width / (double)picTeaching.Width;
                    scaleY = (double)picTeaching.Image.Height / (double)picTeaching.Height;

                }
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
				}
            }
            else if (btn.Name.Contains("ZoomOut"))
            {
                try
                {
                    if (picTeaching.Image != null && zoomCnt > 0)
                    {
                        zoomCnt--;
                        RectInsp._Rect.X = (int)(RectInsp._Rect.X / 2);
                        RectInsp._Rect.Y = (int)(RectInsp._Rect.Y / 2);
                        RectInsp._Rect.Width = (int)(RectInsp._Rect.Width / 2);
                        RectInsp._Rect.Height = (int)(RectInsp._Rect.Height / 2);
                    }
                    picTeaching.Width = picwidth[wheel_cnt];
                    picTeaching.Height = picheight[wheel_cnt];

                    scaleX = (double)picTeaching.Image.Width / (double)picTeaching.Width;
                    scaleY = (double)picTeaching.Image.Height / (double)picTeaching.Height;
                }
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
                }
            }
        }
        private void trbExposure_ValueChanged(object sender, EventArgs e)
        {
            tbxExposure.Text = trbExposure.Value.ToString();
            if (tabControl2.SelectedIndex == 0)
                controller.CtrlCamFront.SetExposure(trbExposure.Value);
            else if (tabControl2.SelectedIndex == 1)
                controller.CtrlCamRear.SetExposure(trbExposure.Value);
        }
        private void tbxExposure_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c)) newText += c;
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <=
                textBox.Text.Length ? selectionStart : textBox.Text.Length;
                trbExposure.Value = (Convert.ToInt32(tbxExposure.Text));
            }
			catch (Exception ex)
			{
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
			}
        }
        private void cbbCtrlCam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (controller.CtrlAnyJig.serialPort.IsOpen)
                {
                    if (cbbCtrlCam.Text == "TEST_DUST")
                    {

                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,4\r"); //  Name="SCROFF"/>
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,0\r"); //  Name="LEDOFF"/>
                        Thread.Sleep(50);

                    }
                    else if (cbbCtrlCam.Text == "TEST_AREA")
                    {
                        // Anyway
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,4\r"); //  Name="SCROFF"/>
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+KEY=15\r"); //  Name="back key"/>
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r"); //  Name="SCRON"/>	9
                        Thread.Sleep(500);
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,6\r"); //  Name="WHITE"/>	5
                        Thread.Sleep(50);
                        //controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,1\r"); //  Name="LEDWHITE"/>		14
                        controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,4\r"); //  Name="LEDBLUE"/>		14
                        Thread.Sleep(50);
                    }
                    else if (cbbCtrlCam.Text == "TEST_RED")
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r"); //  Name="SCRON"/>	9
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,0\r"); // Name="RED"/>		1
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,2\r"); //  Name="LEDRED"/>			15
                        Thread.Sleep(50);
                    }
                    else if (cbbCtrlCam.Text == "TEST_BLUE")
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r"); //  Name="SCRON"/>	9
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,2\r"); //  Name="BLUE"/>	3
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,4\r"); //  Name="LEDBLUE"/>		17
                        Thread.Sleep(50);
                    }
                    else if (cbbCtrlCam.Text == "TEST_GREEN")
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r"); //  Name="SCRON"/>	9
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,1\r"); //  Name="GREEN"/>	2
                        Thread.Sleep(50);
                        controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,3\r"); //  Name="LEDGREEN"/>
                        Thread.Sleep(50);
                    }
					
                }
                //선택 시 조명 같이 ON
                if (cbbCtrlCam.Text == "TEST_LOGO")
                {
                    btnLedLogo.Text = "Led On";
                    btnLedOnOff_Click(btnLedLogo, null);
                }
                else if (cbbCtrlCam.Text == "TEST_LABEL")
                {
                    btnLedOnOffLabel.Text = "Led On";
                    btnLedOnOff_Click(btnLedOnOffLabel, null);
                }
                else if (cbbCtrlCam.Text == "TEST_LASER")
                {
                    btnLedOnOffLaser.Text = "Led On";
                    btnLedOnOff_Click(btnLedOnOffLaser, null);
                }
            }
            catch (Exception ex)
            {
				Log.AddLog("comboBox2_SelectedIndexChanged() in FrmMainCh2 exception " + ex.ToString());
				Log.AddPmLog("comboBox2_SelectedIndexChanged() in FrmMainCh2 exception " + ex.ToString());
                MessageBox.Show(ex.ToString());

            }
        }
        void StartLiveCam()
        {
            btnLive.Text = "LIVE STOP";

            if (tabControl2.SelectedIndex == 0) // FRONT
            {
                controller.CtrlCamFront.GetParameter(ref param);
                controller.CtrlCamFront.SetLivePlay(true);
                controller.CtrlCamFront.ContinuousShot();
            }
            else if (tabControl2.SelectedIndex == 1) //REAR
            {
                controller.CtrlCamRear.GetParameter(ref param);
                controller.CtrlCamRear.SetLivePlay(true);
                controller.CtrlCamRear.ContinuousShot();
            }
            SetControlValue();

        }
        void StopLiveCam()
        {
            if (btnLive.Text == "LIVE STOP")
            {
                btnLive.Text = "LIVE START";
                picTeaching.Image = null;
                if (tabControl2.SelectedIndex == 0 && controller.CtrlCamFront.isContinuous() == true) //FRONT
                {
                    controller.CtrlCamFront.SetLivePlay(false);
                    controller.CtrlCamFront.Stop();
                }
                if (tabControl2.SelectedIndex == 1 && controller.CtrlCamRear.isContinuous() == true) //REAR
                {
                    controller.CtrlCamRear.SetLivePlay(false);
                    controller.CtrlCamRear.Stop();
                }
            }
        }

        //CtrlIO
        public CheckBox GetCheckBox(int idx)
        {
            CheckBox[] cbList = {checkBox_Out01,checkBox_Out02,checkBox_Out03,checkBox_Out04,
            checkBox_Out05,checkBox_Out06,checkBox_Out07,checkBox_Out08};

            return cbList[idx];
        }
        public PictureBox GetIOInputIcon(int idx)
        {
            PictureBox[] InputIcons = { pictureBox_In01, pictureBox_In02, pictureBox_In03, pictureBox_In04, pictureBox_In05, pictureBox_In06 };
            return InputIcons[idx];
        }
        public void IOOutputCheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            CheckBox[] cbList = {checkBox_Out01,checkBox_Out02,checkBox_Out03,checkBox_Out04,
            checkBox_Out05,checkBox_Out06,checkBox_Out07,checkBox_Out08};

            for (int i = 0; i < cbList.Length; i++)
            {
                if (cb.Name == cbList[i].Name)
                {
                    if (cb.Checked == true)
                    {
                        if(i == 1)
                        {
                            controller.CtrlAnyJig.Open(Config.sAnyjigComport, Config.nAnyjigBaud);         //  "COM1", 115200
                            if (controller.scenarioManger.testSpec.AnywayType == "Type2") controller.CtrlAnyJig.ConvertAnywayUSB2Type();
                            if (controller.scenarioManger.testSpec.AnywayType == "TypeC") controller.CtrlAnyJig.ConvertAnywayUSBCType();
                            

                        }
                        w_X16Y16_1.Y16[i] = 1;
                        cbList[i].BackgroundImage = AutoInspection.Properties.Resources.bitmap51;
                    }
                    else
                    {
                        if( i == 1)
                        {
                            controller.CtrlAnyJig.Close();
                        }

                        w_X16Y16_1.Y16[i] = 0;
                        cbList[i].BackgroundImage = AutoInspection.Properties.Resources.bitmap7;
                    }
                }
            }

        }

        //FrontTab
        void btnFrontTab_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name.Contains("LedOnOff"))
            {
                if (btn.Text == "Led On")
                {
                    btn.Text = "Led Off";
                    controller.CtrlLight.ClearLightValue(); 
                    
                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sFrontLight1]
                        , int.Parse(tbxCtrlLight.Text));
                    
                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sFrontLight2]
                        , int.Parse(tbxCtrlLight.Text));

                    controller.CtrlLight.TurnOnLight(); 
                }
                else
                {
                    btn.Text = "Led On";
                    controller.TurnOffFrontLight(); 
                }
            }
            else if (btn.Name.Contains("AreaTest"))
            {
                
                //live stop
                StopLiveCam();
                //grab
                Bitmap bmpImage = controller.GetTestImage(0, controller.scenarioManger.testSpec.SpecExposure.TestArea);
                Bitmap result = null;
                Bitmap MenuKey = null;
                Bitmap BackKey = null;
                Bitmap Led = null;
                //test
                bool _result = controller.scenarioManger.visionTest.ShowArea
                (
                    bmpImage,
                    controller.scenarioManger.testSpec,
                    ref result,
                    ref MenuKey,
                    ref BackKey,
                    ref Led
                );

                //view
                if (_result)
                {
                    picTeaching.Image = result;
                    pictureBox_BackKey.Image = BackKey;
                    pictureBox_MenuKey.Image = MenuKey;
                    pictureBox_LED.Image = Led;
                }
                else
                    MessageBox.Show("fail to find display area");
            }
            else if (btn.Name.Contains("AreaSave"))
            {
                controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            }
            else if (btn.Name.Contains("GetGreenAvg"))
            {
                try
                {
                    //grab
                    Bitmap bmpImage = picTeaching.Image as Bitmap;
                    double _result = 0;
                    //test
					if (bmpImage == null)
						return;

                    AverageColor colorMode = AverageColor.NONE;
                    if(cbbCtrlCam.Text == "TEST_BLUE")
                    {
                        colorMode = AverageColor.BLUE;
					}
					else if(cbbCtrlCam.Text == "TEST_GREEN")
                    {
                        colorMode = AverageColor.GREEN;
					}
					else if(cbbCtrlCam.Text == "TEST_RED")
                    {
                        colorMode = AverageColor.RED;
                    }
                    
                    else if (cbbCtrlCam.Text == "TEST_AREA")
                    {
                        _result = controller.scenarioManger.visionTest.GetGrayAverage
                        (
                            bmpImage,
                            controller.scenarioManger.testSpec.SpecLcdArea
                        );
                        tbxGreenAvg.Text = _result.ToString("##.##");
                        return;
                    }

					//_result = controller.scenarioManger.visionTest.GetRgbAverage
					//    (
					//        bmpImage,
					//        controller.scenarioManger.testSpec.SpecLcdArea,
					//        colorMode
					//    );

					// tbxGreenAvg.Text = "Calcurating....";

					string __result = controller.scenarioManger.visionTest.GetRgbAverage
                        (
                            bmpImage,
                            controller.scenarioManger.testSpec.SpecLcdArea,
                            colorMode
                        );

                    //double _result = controller.scenarioManger.visionTest.GetGreenAverage
                    //(
                    //    bmpImage,
                    //    controller.scenarioManger.testSpec.SpecLcdArea
                    //);
					Log.AddPmLog(__result);
					Log.SavePmLog();

					tbxGreenAvg.Text = __result;
                }
                catch (Exception ex)
                {
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
            }
            else if(btn.Name.Contains("GetBalance"))
            {
                if(cbbColor.Text =="RED")
                {
                    tbxBalance.Text = controller.CtrlCamFront.GetColorBalance(AverageColor.RED).ToString();
                }
                else if(cbbColor.Text == "BLUE")
                {
                    tbxBalance.Text = controller.CtrlCamFront.GetColorBalance(AverageColor.BLUE).ToString();
                }
            }
            else if (btn.Name.Contains("SetBalance"))
            {
                if (cbbColor.Text == "RED")
                {
                    controller.CtrlCamFront.SetColorBalance(AverageColor.RED,long.Parse( tbxBalance.Text));
                }
                else if (cbbColor.Text == "BLUE")
                {
                    controller.CtrlCamFront.SetColorBalance(AverageColor.BLUE, long.Parse(tbxBalance.Text));
                }
            }
            else if (btn.Name.Contains("UserSave"))
            {
                controller.CtrlCamFront.SaveUserSet();
            }
            else if (btn.Name.Contains("WBAuto"))
            {
                controller.CtrlCamFront.WBAuto();
            }
            else
            {
                Button[] btList = { btnAreaLeft, btnAreaRight, btnAreaUp, btnAreaDown };

                for (int i = 0; i < btList.Length; i++)
                {
                    if (btn.Name == btList[i].Name)
                    {
                        if (comboBox1.SelectedIndex == 0)
                            SetArea(InspectionArea.LED, (Direction)i);
                        else if (comboBox1.SelectedIndex == 1)
                            SetArea(InspectionArea.MENUKEY, (Direction)i);
                        else if (comboBox1.SelectedIndex == 2)
                            SetArea(InspectionArea.BACKKEY, (Direction)i);
                        else
                            MessageBox.Show("select region");
                    }
                }

            }
        }
        void SetArea(InspectionArea _area, Direction _dir)
        {
            CRect target;
            if (_area == InspectionArea.LED)
                target = controller.scenarioManger.testSpec.SpecLcdArea.LedRoughRoi;
            else if (_area == InspectionArea.MENUKEY)
                target = controller.scenarioManger.testSpec.SpecLcdArea.MenuRoughRoi;
            else
                target = controller.scenarioManger.testSpec.SpecLcdArea.BackRoughRoi;
			
			//15 -> 10 
            if (_dir == Direction.UP)
                target.top = target.top - 10;
            else if (_dir == Direction.LEFT)
                target.left = target.left - 10;
            else if (_dir == Direction.RIGHT)
                target.left = target.left + 10;
            else
                target.top = target.top + 10;
        }
        // 1 ,2 : 면조명
        // 3 : 띠조명
        void btnLedOnOff_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            
            if (btn.Text == "Led On")
            {
                btn.Text = "Led Off";
                

                if (btn.Name.Contains("Logo"))
                {
                    controller.CtrlLight.ClearLightValue();

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sBarLight]
                        , Convert.ToInt32(tbxBarLightLogo.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight1]
                        , Convert.ToInt32(tbxBackLightLogo.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight2]
                        , Convert.ToInt32(tbxBackLightLogo.Text));

                    controller.CtrlLight.TurnOnLight();

                }
                else if (btn.Name.Contains("Label"))
                {
                    controller.CtrlLight.ClearLightValue();

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sBarLight]
                        , Convert.ToInt32(tbxBarLightLabel.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight1]
                        , Convert.ToInt32(tbxBackLightLabel.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight2]
                        , Convert.ToInt32(tbxBackLightLabel.Text));

                    controller.CtrlLight.TurnOnLight();
                }
                else if (btn.Name.Contains("Laser"))
                {

                    controller.CtrlLight.ClearLightValue();

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sBarLight]
                        , Convert.ToInt32(tbxBarLightLaser.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight1]
                        , Convert.ToInt32(tbxBackLightLaser.Text));

                    controller.CtrlLight.SetLightValue(
                        controller.CtrlLight.lightChannel[controller.CtrlLight.sRearLight2]
                        , Convert.ToInt32(tbxBackLightLaser.Text));

                    controller.CtrlLight.TurnOnLight();
                }
            }
            else
            {
                btn.Text = "Led On";
                controller.TurnOffRearLight();
            }
        }

        private void btnSaveFrontValue_Click(object sender, EventArgs e)
        {
            controller.scenarioManger.testSpec.SpecLight.DustLightValue = Convert.ToInt32(tbxCtrlLight.Text);
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            MessageBox.Show("Front CtrlLight is saved.");
        }

        private void btnSaveBackglassValue_Click(object sender, EventArgs e)
        {
            controller.scenarioManger.testSpec.SpecLight.LogoBackLightValue = Convert.ToInt32(tbxBackLightLogo.Text);
            controller.scenarioManger.testSpec.SpecLight.LogoBarLightValue = Convert.ToInt32(tbxBarLightLogo.Text);
            controller.scenarioManger.testSpec.SpecLight.LabelBackLightValue = Convert.ToInt32(tbxBackLightLabel.Text);
            controller.scenarioManger.testSpec.SpecLight.LabelBarLightValue = Convert.ToInt32(tbxBarLightLabel.Text);
            controller.scenarioManger.testSpec.SpecLight.LaserBackLightValue = Convert.ToInt32(tbxBackLightLaser.Text);
            controller.scenarioManger.testSpec.SpecLight.LaserBarLightValue = Convert.ToInt32(tbxBarLightLaser.Text);

            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            MessageBox.Show("Backglass light is saved.");
        }

        void trbCamera_MouseUp(object sender, MouseEventArgs e)
        {
            controller.CtrlCamFront.Stop();

            TrackBar trb = (TrackBar)sender;
            if (trb.AccessibleName == "Width")
            {
                int temp = trbWidth.Value;
                if (temp >= 65 && temp % 2 == 1)
                    temp--;

                tbxWidth.Text = temp.ToString();
                param.Width = temp;
               
                //Config.CamFrontWidth = temp;
                controller.scenarioManger.testSpec.SpecFrontCam.Width = temp;

            }
            else if (trb.AccessibleName == "Height")
            {
                int temp = trbHeight.Value;
                if (temp >= 65 && temp % 2 == 1)
                    temp--;
                tbxHeight.Text = temp.ToString();
                param.Height = temp;
                controller.scenarioManger.testSpec.SpecFrontCam.Height = temp;
                //Config.CamFrontHeight = temp;
            }
            else if (trb.AccessibleName == "OffsetX")
            {
                int temp = trbXoffset.Value;
                if (temp >= 1 && temp % 2 == 1)
                    temp--;
                tbxXoffset.Text = temp.ToString();
                param.Xoffset = temp;
                controller.scenarioManger.testSpec.SpecFrontCam.OffsetX = temp;
               // Config.CamFrontOffsetX = temp;
            }
            else if (trb.AccessibleName == "OffsetY")
            {
                int temp = trbYoffset.Value;
                if (temp >= 1 && temp % 2 == 1)
                    temp--;
                tbxYoffset.Text = temp.ToString();
                param.Yoffset = temp;
                controller.scenarioManger.testSpec.SpecFrontCam.OffsetY = temp;
                //Config.CamFrontOffsetY = temp;
            }
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            //Config.saveTofile();
            controller.CtrlCamFront.SetParameter(param);
            controller.CtrlCamFront.GetParameter(ref param);
            SetControlValue();
            controller.CtrlCamFront.ContinuousShot();
        }
        void SetControlValue()
        {
            try
            {
                trbExposure.Minimum = param.MinExposure;
                trbExposure.Maximum = param.MaxExposure;
                trbExposure.Value = param.ExposureValue;

                trbWidth.Minimum = param.MinWidth;
                trbWidth.Maximum = param.MaxWidth;
                trbWidth.Value = param.Width;
                tbxWidth.Text = param.Width.ToString();

                trbHeight.Minimum = param.MinHeight;
                trbHeight.Maximum = param.MaxHeight;
                trbHeight.Value = param.Height;
                tbxHeight.Text = param.Height.ToString();

                trbXoffset.Minimum = param.MinXoff;
                trbXoffset.Maximum = param.MaxXoff;
                trbXoffset.Value = param.Xoffset;
                tbxXoffset.Text = param.Xoffset.ToString();

                trbYoffset.Minimum = param.MinYoff;
                trbYoffset.Maximum = param.MaxYoff;
                trbYoffset.Value = param.Yoffset;
                tbxYoffset.Text = param.Yoffset.ToString();
            }
            catch (Exception ex)
            {
				Log.AddLog("SetControlValue() in FrmMainCh2 exception " + ex.ToString());
				Log.AddPmLog("SetControlValue() in FrmMainCh2 exception " + ex.ToString());
                MessageBox.Show(ex.ToString());
            }

        }

        //BackGlassTab
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            picTeaching.Image = null;
            StopLiveCam();
            if (tabControl2.SelectedIndex == 0)
            {
                if (controller.CtrlCamRear.isContinuous() == true)
                {
                    controller.CtrlCamRear.SetLivePlay(false);
                    controller.CtrlCamRear.Stop();
                }
                RectInsp.RectColor = Color.Transparent;
                RectInsp.PenColor = Color.Transparent;
            }
            if (tabControl2.SelectedIndex == 1)
            {
                if (controller.CtrlCamFront.isContinuous() == true)
                {
                    controller.CtrlCamFront.SetLivePlay(false);
                    controller.CtrlCamFront.Stop();
                }
                RectInsp.RectColor = Color.Blue;
                RectInsp.PenColor = Color.Red;
            }
            picTeaching.Refresh();
        }
        void btnTestRegionSave_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                GetImgScale();
                TestSpec testSpec = controller.scenarioManger.testSpec;
                MData_REAR specRear = controller.scenarioManger.testSpec.SpecRear;
                if (btn.Name.Contains("LogoReg"))
                {
                    Bitmap temp = null;
                    if (cbbLogoSelect.SelectedIndex == 0)
                    {
                        testSpec.RectLogo1.X = (int)((RectInsp._Rect.X - specRear.LogoRegOffset) * scaleX);
                        testSpec.RectLogo1.Y = (int)((RectInsp._Rect.Y - specRear.LogoRegOffset) * scaleY);
                        testSpec.RectLogo1.Width = (int)((RectInsp._Rect.Width + (2 * specRear.LogoRegOffset)) * scaleX);
                        testSpec.RectLogo1.Height = (int)((RectInsp._Rect.Height + (2 * specRear.LogoRegOffset)) * scaleY);
                        temp = controller.scenarioManger.visionTools.GetBmpFromRect(
                                    picTeaching.Image as Bitmap, testSpec.RectLogo1, 90);
                    }
                    else
                    {
                        testSpec.RectLogo2.X = (int)((RectInsp._Rect.X - specRear.LogoRegOffset) * scaleX);
                        testSpec.RectLogo2.Y = (int)((RectInsp._Rect.Y - specRear.LogoRegOffset) * scaleY);
                        testSpec.RectLogo2.Width = (int)((RectInsp._Rect.Width + (2 * specRear.LogoRegOffset)) * scaleX);
                        testSpec.RectLogo2.Height = (int)((RectInsp._Rect.Height + (2 * specRear.LogoRegOffset)) * scaleY);
                        temp = controller.scenarioManger.visionTools.GetBmpFromRect(
                                    picTeaching.Image as Bitmap, testSpec.RectLogo2, 90);
                    }

                    picTest.Image = temp;

                }
                else if (btn.Name.Contains("LogoTemp"))
                {
                    Bitmap temp = RectInsp.GetBmpFromRect(picTeaching.Image as Bitmap, scaleX, scaleY, 90.0f);
                    //temp.Save(string.Format(@".\Model\{0}_rect{1}.bmp", modelName, cbbLogoSelect.SelectedIndex + 1));
                    temp.Save(Config.sPathModel+string.Format("{0}_rect{1}.bmp", modelName, cbbLogoSelect.SelectedIndex + 1));
                    picTest.Image = temp;
                }
                else if (btn.Name.Contains("Bar"))
                {
                    testSpec.RectBarcode.X = (int)(RectInsp._Rect.X * scaleX);
                    testSpec.RectBarcode.Y = (int)(RectInsp._Rect.Y * scaleY);
                    testSpec.RectBarcode.Width = (int)(RectInsp._Rect.Width * scaleX);
                    testSpec.RectBarcode.Height = (int)(RectInsp._Rect.Height * scaleY);

                    picTest.Image = controller.scenarioManger.visionTools.GetBmpFromRect(
                      picTeaching.Image as Bitmap, testSpec.RectBarcode, 90);
                }
                else if (btn.Name.Contains("OCR"))
                {
                    testSpec.RectImei.X = (int)(RectInsp._Rect.X * scaleX);
                    testSpec.RectImei.Y = (int)(RectInsp._Rect.Y * scaleY);
                    testSpec.RectImei.Width = (int)(RectInsp._Rect.Width * scaleX);
                    testSpec.RectImei.Height = (int)(RectInsp._Rect.Height * scaleY);
                    controller.scenarioManger.visionTest.SaveOcrTemplteImge(picTeaching.Image as Bitmap, testSpec);
                    controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);

                    picImei.Image = controller.scenarioManger.visionTools.GetBmpFromRect(
                       picTeaching.Image as Bitmap, testSpec.RectImei, 90);
                }
                controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            }
            catch(Exception ex)
            {
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        void btnTestRun_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                TestSpec testSpec = controller.scenarioManger.testSpec;
                MData_REAR specRear = controller.scenarioManger.testSpec.SpecRear;
                MResult_REAR Result = new MResult_REAR();

                if (btn.Name.Contains("Barcode"))
                {
                    sImei = "";

                    bool bResult;

                    Bitmap testImg = picTeaching.Image as Bitmap;
                    bResult = controller.scenarioManger.visionTest.Barcode(
                        testImg,
                        testSpec,
                        specRear,
                        ref Result);

                    if (bResult)
                    {
                        listBoxTestResult.Items.Add("IMEI in Barcode : " + Result.m_nImei.ToString());
                        sImei = Result.m_nImei.ToString();
                    }
                    else
                        listBoxTestResult.Items.Add("Check Barcode Region & Setting Parameter");
                    listBoxTestResult.SelectedIndex = listBoxTestResult.Items.Count - 1;

                    picTest.Image = controller.scenarioManger.visionTools.GetBmpFromRect(
                      picTeaching.Image as Bitmap, testSpec.RectBarcode, 90);
                }
                else if (btn.Name.Contains("Logo"))
                {
                    Bitmap imgSrc = picTeaching.Image as Bitmap;
                    int matchingRate = controller.scenarioManger.visionTest.CalcLogoMatchingRate(
                    imgSrc,
                    testSpec,
                    cbbLogoSelect.SelectedIndex + 1,
                    new Bitmap(Config.sPathModel + string.Format("{0}_rect{1}.bmp", modelName, cbbLogoSelect.SelectedIndex + 1))); 

                    listBoxTestResult.Items.Add(string.Format("Logo{0} Matching Rate : {1}%", cbbLogoSelect.SelectedIndex + 1, matchingRate.ToString()));
                    listBoxTestResult.SelectedIndex = listBoxTestResult.Items.Count - 1;
                }
                else if (btn.Name.Contains("OCR"))
                {
                    bool bResult;
                    Bitmap testImg = picTeaching.Image as Bitmap;
                    string temp = sImei.Remove(0, 9);
                    temp = temp.Substring(0, 3);
                    bResult = controller.scenarioManger.visionTest.OCR(
                        testImg,
                        controller.scenarioManger.testSpec,
                        sImei.Remove(0, 10),
                        temp,
                        specRear, controller.mainForm,
                        ref Result);

                    if (bResult)
                        listBoxTestResult.Items.Add("OCR in Laser");
                    else
                        listBoxTestResult.Items.Add("OCR Not found");
                    listBoxTestResult.SelectedIndex = listBoxTestResult.Items.Count - 1;
                }
            }
            catch(Exception ex)
            {
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        private void cbbLogoCnt_SelectedIndexChanged(object sender, EventArgs e)
        {

            cbbLogoSelect.Items.Clear();
            for (int i = 0; i < cbbLogoCnt.SelectedIndex + 1; i++)
                cbbLogoSelect.Items.Add(string.Format("LOGO{0}", i + 1));

            cbbLogoSelect.SelectedIndex = 0;
            TestSpec testSpec = controller.scenarioManger.testSpec;
            testSpec.SpecRear.LogoCount = cbbLogoCnt.SelectedIndex + 1;
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
        }
        private void trbOCR_ValueChanged(object sender, EventArgs e)
        {
            int temp;
            if (trbOCR.Value % 2 == 0)
                temp = trbOCR.Value - 1;
            else
                temp = trbOCR.Value;
            VisionTools vt = controller.scenarioManger.visionTools;
            tbxOCR.Text = temp.ToString();
            controller.scenarioManger.testSpec.SpecRear.OcrThreshold = temp;
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
            GetImgScale();
            if (picTeaching.Image != null)
            {
                picTest.Image = controller.scenarioManger.visionTools.preProcessBG(
                    RectInsp.GetBmpFromRect(picTeaching.Image as Bitmap, scaleX, scaleY, 90.0f),
                    controller.scenarioManger.testSpec);
            }
        }
        //Idx = 0 : Binary / Idx = 1 BinaryInv
        private void cbbOCRThresType_SelectedIndexChanged(object sender, EventArgs e)
        {
            controller.scenarioManger.testSpec.SpecRear.OcrThresIdx = cbbOCRThresType.SelectedIndex;
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);
        }
        private void tbxOCR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                Int32 selectionStart = textBox.SelectionStart;
                Int32 selectionLength = textBox.SelectionLength;
                String newText = String.Empty;
                foreach (Char c in textBox.Text.ToCharArray())
                {
                    if (Char.IsDigit(c) || Char.IsControl(c)) newText += c;
                }
                textBox.Text = newText;
                textBox.SelectionStart = selectionStart <=
                textBox.Text.Length ? selectionStart : textBox.Text.Length;
                trbOCR.Value = (Convert.ToInt32(tbxOCR.Text));
            }
			catch (Exception ex)
			{
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
			}
        }
        private void GetImgScale()
        {
            if (picTeaching.Image != null)
            {
                scaleX = (double)picTeaching.Image.Width / (double)picTeaching.Width;
                scaleY = (double)picTeaching.Image.Height / (double)picTeaching.Height;
            }
        }

#endregion

        #region _____________________#4 DATA TAB_____________________

        ConfigInfo configInfo = new ConfigInfo();
        void DisplaySpecInfo()
        {
            propertyGridInspectionSpec.PropertySort = PropertySort.NoSort;
            propertyGridInspectionSpec.SelectedObject = controller.scenarioManger.testSpec;

            UpdateUserInterface();
        }

        void UpdateUserInterface()
        {
            cbbOCRThresType.SelectedIndex = controller.scenarioManger.testSpec.SpecRear.OcrThresIdx;


            tbxCtrlLight.Text = controller.scenarioManger.testSpec.SpecLight.DustLightValue.ToString();

            tbxBackLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBackLightValue.ToString();
            tbxBarLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBarLightValue.ToString();
            tbxBackLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBackLightValue.ToString();
            tbxBarLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBarLightValue.ToString();
            tbxBackLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBackLightValue.ToString();
            tbxBarLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBarLightValue.ToString();

            trbOCR.Value = controller.scenarioManger.testSpec.SpecRear.OcrThreshold;
            cbbLogoCnt.SelectedIndex = controller.scenarioManger.testSpec.SpecRear.LogoCount - 1;

        }

        void DisplaySystemInfo()
        {
            propertyGridSystemSetting.PropertySort = PropertySort.NoSort;

            configInfo.GetFromConfig();
            propertyGridSystemSetting.SelectedObject = configInfo;
        }

        private void btnLoadModel_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Config.sPathModel;
            ofd.Filter = "Json Files (.json)|*.json|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Config.sCurrnetSpecFile = ofd.SafeFileName;
                controller.scenarioManger.LoadFiles(Config.sCurrnetSpecFile);
                if (controller.scenarioManger.testSpec.AnywayType == "Type2") controller.CtrlAnyJig.ConvertAnywayUSB2Type();
                if (controller.scenarioManger.testSpec.AnywayType == "TypeC") controller.CtrlAnyJig.ConvertAnywayUSBCType();
                string _modelName = Path.GetFileNameWithoutExtension(Config.sCurrnetSpecFile);  // 확장자 제거 -> 모델 이름만 가져오기
                controller.mainForm.DisplayModelName(_modelName);                               // 모델명, 화면에 표시 

                Config.saveTofile();    // 재 실행 시, 변경된 모델로 자동 로드하도록 저장
                DisplaySpecInfo();      // 화면 반영
                //if (!File.Exists(string.Format(@".\Detail\Rear\{0}_TemplateImg.bmp", Config.sCurrnetSpecFile.Replace(".json", ""))))
                if (!File.Exists(string.Format(Config.sPathModel+"{0}_TemplateImg.bmp", Config.sCurrnetSpecFile.Replace(".json", ""))))
                {
                    MessageBox.Show("There is no Template IMEI LASER. Teaching IMEI LASER");
                }
            }
        }

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            FrmFilenameInput inputForm = new FrmFilenameInput();

            inputForm.ShowDialog(this);
            if (inputForm.DialogResult == DialogResult.OK)
            {
                string _modelFile = inputForm.NewFileName + ".json";

                if (File.Exists(_modelFile) == false)
                {
                    Config.sCurrnetSpecFile = _modelFile;

                    controller.scenarioManger.LoadFiles(Config.sCurrnetSpecFile);                   // model.json 파일 로드 
                    string _modelName = Path.GetFileNameWithoutExtension(Config.sCurrnetSpecFile);  // 확장자 제거 -> 모델 이름만 가져오기
                    controller.mainForm.DisplayModelName(_modelName);                               // 모델명, 화면에 표시 

                    Config.saveTofile();    // 재 실행 시, 변경된 모델로 자동 로드하도록 저장

                    DisplaySpecInfo();      // 화면 반영
                }
                else
                {
                    MessageBox.Show("Error : File already Exist \n" + _modelFile);
                }
            }
        }

        private void btnSpecSave_Click(object sender, EventArgs e)
        {
            controller.scenarioManger.SaveFiles(Config.sPathModel + Config.sCurrnetSpecFile);

            tbxCtrlLight.Text = controller.scenarioManger.testSpec.SpecLight.DustLightValue.ToString();

            tbxBackLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBackLightValue.ToString();
            tbxBarLightLogo.Text = controller.scenarioManger.testSpec.SpecLight.LogoBarLightValue.ToString();
            tbxBackLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBackLightValue.ToString();
            tbxBarLightLabel.Text = controller.scenarioManger.testSpec.SpecLight.LabelBarLightValue.ToString();
            tbxBackLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBackLightValue.ToString();
            tbxBarLightLaser.Text = controller.scenarioManger.testSpec.SpecLight.LaserBarLightValue.ToString();

            MessageBox.Show("Spec Info Saved");
        }

        private void btnSpecCancle_Click(object sender, EventArgs e)
        {
            TestSpec _testSpec = controller.scenarioManger.testSpec;

            controller.scenarioManger.LoadFiles(Config.sCurrnetSpecFile);
            propertyGridInspectionSpec.SelectedObject = _testSpec;
        }

        private void btnInfoSave_Click(object sender, EventArgs e)
        {
            if(configInfo.GreenaAcumulateCount <= 0)
            {
                MessageBox.Show("GreenaAcumulateCount > 0");
                return;
            }

            configInfo.SetToConfig();
            Config.saveTofile();
            MessageBox.Show("System Info Saved");
        }

        private void btnInfoCancle_Click(object sender, EventArgs e)
        {
            Config.loadFromFile();
            DisplaySystemInfo();
        }

#endregion

        #region Update

        public void UpdateProductionInfo()
        {
            try
            {
                //labelTotalCount.Text = (Config.CountPass + Config.CountFail).ToString();
                //labelPassCount.Text = Config.CountPass.ToString();
                //labelFailCount.Text = Config.CountFail.ToString();
                //labelPassRate.Text = (Config.CountPass / (Config.CountPass + Config.CountFail)).ToString();

                labelTotalCount.Text = (Config.CountPass + Config.CountFail).ToString();
                labelPassCount.Text = Config.CountPass.ToString();
                labelFailCount.Text = Config.CountFail.ToString();
                labelPassRate.Text = string.Format("{0:F1}", (float)Config.CountPass * 100 / (float)(Config.CountPass + Config.CountFail));
            }
			catch (Exception ex)
            {
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
                return;
            }
        }

        public void DisplayModelName(string _modelName)
        {
            if (btnModel.InvokeRequired)
            {
                btnModel.Invoke(new MethodInvoker(() => { DisplayModelName(_modelName); }));
            }
            else
            {
                btnModel.Text = "Model : [ " + _modelName + " ]";
                labelModelName.Text = "Model : " + _modelName;
                modelName = _modelName;
                
            }
        }
        
        public void DisplayImage(int PicNo, Bitmap bmpImage)
        {
            if (bmpImage == null)
                return;

            switch (PicNo)
            {
                case 0:
                    camBox1.Image = bmpImage.Clone() as Image;
                    break;
                case 1:
                    camBox2.Image = bmpImage.Clone() as Image;
                    break;
                case 2:
                    picTeaching.Image = bmpImage.Clone() as Image;
                    break;
                case 3:
                    picimeiMatching.Image = bmpImage.Clone() as Image;
                    break;
                case 4:
                    picImeiResult.Image = bmpImage.Clone() as Image;
                    break;
            }
        }

        public void DisplayProductionInfo(bool IsPass)
        {
            if ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdDustResult)) == (int)InspectionResult.LcdDustResult)
            {
                DisplayResult("Please clean the device and restart");
                return;
            }
            else if ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdAreaResult)) == (int)InspectionResult.LcdAreaResult)
            {
                DisplayResult("Please restart");
                return;
            }
            if (labelTotalCount.InvokeRequired)
            {
                labelTotalCount.Invoke(new MethodInvoker(() => { DisplayProductionInfo(IsPass); }));
            }
            else
            {
                if (IsPass)
                    Config.CountPass++;
                else
                    Config.CountFail++;

                Config.saveTofile();

                UpdateProductionInfo();
            }

        }
        // Modify Nam jh
        public void ClearLogResult()
        {
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke(new MethodInvoker(() => { ClearLogResult(); }));
            }
            else
            {
                lbLog.Items.Clear();
            }
        }

        public void ClearTestResult()
        {
            if (listItem_Whole.InvokeRequired)
            {
                listItem_Whole.Invoke(new MethodInvoker(() => { ClearTestResult(); }));
            }
            
            else
            {
                camBox1.Image = null;
                camBox2.Image = null;
                listItem_Whole.Items.Clear();
            }
        }
        public void ClearFailLIst()
        {
            if (listViewItemImg.InvokeRequired)
            {
                listViewItemImg.Invoke(new MethodInvoker(() => { ClearFailLIst(); }));
            }
            else
            {
                listViewItemImg.Items.Clear();
            }
        }

        public void DisplayElapsedTime(string _time)
        {
            if (labelTimer.InvokeRequired)
            {
                labelTimer.Invoke(new MethodInvoker(() => { DisplayElapsedTime(_time); }));
            }
            else
            {
                labelTimer.Text = _time;
            }
        }

        public void DisplayElapsedTime(long _time)
        {
            if (labelTimer.InvokeRequired)
            {
                labelTimer.Invoke(new MethodInvoker(() => { DisplayElapsedTime(_time); }));
            }
            else
            {
                labelTimer.Text = string.Format("{0:F2} secs", ((float)_time / 1000));
            }
        }

        public void GetResult()
        {
            if (listItem_Whole.InvokeRequired)
            {
                listItem_Whole.Invoke(new MethodInvoker(() => { GetResult(); }));
            }
            else
            {
                foreach (ListViewItem item in listItem_Whole.Items)
                {
                    if (item.ForeColor == Color.Red)
                    {
                        controller.FinalResult = false;
                        return;
                    }
                }
                controller.FinalResult = true;
            }
        }


        public void DisplayTestResult(string testName, string measure, string min, string max, bool result)
        {
            if (listItem_Whole.InvokeRequired)
            {
                listItem_Whole.Invoke(new MethodInvoker(() => { DisplayTestResult(testName, measure, min, max, result); }));
            }
            else
            {
                ListViewItem item = new ListViewItem(testName);

                item.SubItems.Add(testName);
                //item.SubItems.Add(string.Format("{0:n0}", Convert.ToDouble(measure)));
                //item.SubItems.Add(string.Format("{0:n0}", Convert.ToDouble(min)));
                //item.SubItems.Add(string.Format("{0:n0}", Convert.ToDouble(max)));
                //item.SubItems.Add(result.ToString());
                item.SubItems.Add(measure);
                item.SubItems.Add(min);
                item.SubItems.Add(max);

                item.SubItems.Add(result.ToString());
                listItem_Whole.Items.Add(item);

                if (result)
                {
                    listItem_Whole.Items[listItem_Whole.Items.Count - 1].ForeColor = Color.Black;
                }
                else
                {
                    listItem_Whole.Items[listItem_Whole.Items.Count - 1].ForeColor = Color.Red;
                }

                listItem_Whole.View = View.Details;
				string _s = string.Format("Test Result:({0}), Measure:({1}), Min:({2}), Max:({3}), Result({4})"
					, testName, measure, min, max, result.ToString());
				Log.AddLog(_s);
				Log.AddPmLog(_s); 
            }
        }

        public void DisplayErrorMsg(string testName, string msg)
        {
            if (listViewItemImg.InvokeRequired)
            {
                listViewItemImg.Invoke(new MethodInvoker(() => { DisplayErrorMsg(testName, msg); }));
            }
            else
            {
                listViewItemImg.Items.Add(new ListViewItem(new string[] { testName, msg }));
            }
        }

        public void DisplayExceptionMsg(string testName, string msg)
        {
            if (listFailedItem.InvokeRequired)
            {
                listFailedItem.Invoke(new MethodInvoker(() => { DisplayExceptionMsg(testName, msg); }));
            }
            else
            {
                listFailedItem.Items.Add(new ListViewItem(new string[] { testName, msg }));
				Log.AddLog(testName + "exception " + msg);
            }
        }


        public void DisplayStatus(int channel,string status)
        {
            if (channel == 0)
            {
                if (lblStatusFront.InvokeRequired)
                {
                    lblStatusFront.Invoke(new MethodInvoker(() => { DisplayStatus(channel, status); }));
                }
                else
                {
                    lblStatusFront.Text = status;
                }
            }
            else
            {
                if (lblStatusRear.InvokeRequired)
                {
                    lblStatusRear.Invoke(new MethodInvoker(() => { DisplayStatus(channel, status); }));
                }
                else
                {
                    lblStatusRear.Text = status;
                }
            }

        }
        public void DisplayImei(string imei)
        {
            if (labelImei.InvokeRequired)
            {
                labelImei.Invoke(new MethodInvoker(() => { DisplayImei(imei); }));
            }
            else
            {
                sImei = imei;
                labelImei.Text = imei;
            }
        }
        public void DisplayResult(string result)
        {
            if (labelResult.InvokeRequired)
            {
                labelResult.Invoke(new MethodInvoker(() => { DisplayResult(result); }));
            }
            else
            {
                labelResult.Text = result;
                if (result == "PASS")
                    labelResult.BackColor = Color.Lime;
                else if (result == "FAIL" || result == "UN IS NULL")
                    labelResult.BackColor = Color.Red;
                else
                    labelResult.BackColor = Color.Gold;
            }
        }


        public void DisplayStatus(Controller.MainState state )
        {
            if (btnStatus.InvokeRequired)
            {
                btnStatus.Invoke(new MethodInvoker(() => { DisplayStatus(state); }));
            }
            else
            {
                if (state == Controller.MainState.Emergency)
                {
                    btnStatus.BackgroundImage = AutoInspection.Properties.Resources.GLOBES_RED;
                    btnStatusText.Text = "STOP";
                }
                else if (state == Controller.MainState.Running)
                {
                    btnStatus.BackgroundImage = AutoInspection.Properties.Resources.GLOBES_GREEN;
                    btnStatusText.Text = "RUNNING";
                }
            }
        }
#endregion

        #region _____________________USERINTERFACE_____________________
        public DIO.W_X16Y16_ GetIOControl()
        {
            return w_X16Y16_1;
        }
        public PictureBox GetTeachingPictureBox()
        {
            return picTeaching;
        }

        public string GetImei()
        {
            return sImei;
        }

        public string GetModelName()
        {
            return modelName;
        }

#endregion

        private void frmMainCh2_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.TurnOffAllLight();
            Thread.Sleep(200);
            try
            {
                controller.CtrlCamFront.DestroyCamera();
                controller.CtrlCamRear.DestroyCamera();
            }
			catch (Exception ex)
			{
				Log.AddLog(ex.ToString());
				Log.AddPmLog(ex.ToString());
			}
            controller.Exit();
        }

        private void listViewItemImg_MouseClick(object sender, MouseEventArgs e)
        {
            string path = listViewItemImg.GetItemAt(e.X, e.Y).SubItems[1].Text;
            
            try
            {
                Bitmap img = new Bitmap(path);
                if (img != null)
                {
                    camBox2.Image = img;
                }
            }
            catch(Exception ex)
            {
				Log.AddLog("listViewItemImg_MouseClick() in FrmMainCh2 exception " + ex.ToString());
				Log.AddPmLog("listViewItemImg_MouseClick() in FrmMainCh2 exception " + ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
      
        public void DisplayLog(string log)
        {
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke(new MethodInvoker(() => { DisplayLog(log); }));
            }
            else
            {
                lbLog.Items.Add(log);
                lbLog.SelectedIndex = (lbLog.Items.Count - 1);
            }
        }

    
        
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] InputChannels = { w_X16Y16_1.X_01, w_X16Y16_1.X_02, w_X16Y16_1.X_03, w_X16Y16_1.X_04, w_X16Y16_1.X_05, w_X16Y16_1.X_06 };
            PictureBox[] InputIcons = { pictureBox_In01, pictureBox_In02, pictureBox_In03, pictureBox_In04, pictureBox_In05, pictureBox_In06 };

            for (int i = 0; i < InputChannels.Length; i++)
            {
                if (InputChannels[i] == 1)
                    InputIcons[i].Image = AutoInspection.Properties.Resources.bitmap51;
                else
                    InputIcons[i].Image = AutoInspection.Properties.Resources.bitmap7;
            }
        }

        public void WriteLog(string log)
        {
            if (listBoxLog.InvokeRequired)
            {
                listBoxLog.Invoke(new MethodInvoker(() => { WriteLog(log); }));
            }
            else
            {
                DateTime now = DateTime.Now;
                listBoxLog.Items.Add(now.ToShortTimeString() + "> " + log);
                listBoxLog.SelectedIndex = (listBoxLog.Items.Count - 1);
                File.AppendAllText(now.ToShortDateString() + ".log", now.ToShortTimeString() + "> " + log + "\r\n");
            }
        }
        
        public static string PW = "";

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            switch (cbATcmd.Text)
            {
                case "SCREEN ON":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r");
                    break;
                case "SCREEN OFF":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,4\r");
                    break;
                case "BACK KEY":
                    controller.CtrlAnyJig.Write("AT+KEY=15\r");
                    break;
                case "LED OFF":
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,0\r");
                    break;
                case "WHITE":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,6\r");
                    break;
                case "LED WHITE":
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,1\r");
                    break;
                case "RED":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,0\r");
                    break;
                case "LED RED":
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,2\r");
                    break;
                case "BLUE":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,2\r");
                    break;
                case "LED BLUE":
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,4\r");
                    break;
                case "GREEN":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,1\r");
                    break;
                case "LED GREEN":
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,3\r");
                    break;
                case "LCD BRIGHT HIGH":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=3,0\r");
                    break;
                case "LCD BRIGHT MID":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=3,1\r");
                    break;
                case "LCD BRIGHT LOW":
                    controller.CtrlAnyJig.Write("AT+DISPTEST=3,2\r");
                    break;
            }
        }

        
        private void RunDryMode_Click(object sender, EventArgs e)
        {
            Thread dryRun = new Thread(new ThreadStart(controller.RunDry));
            dryRun.IsBackground = true;
            dryRun.Start(); 
        }

        private void btnDetailOpen_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", @".\Detail\");

        }
        public void DisplayFailImage(List<IMGFAIL> listimgfail)
        {

            if (listimgfail.Count > 0)
            {
                int _showindex1 = -1;
                int _showindex2 = -1;
                for (int i = 0; i < listimgfail.Count; i++)
                {
                    if (listimgfail[i].index == 0) _showindex1 = i;
                    if (listimgfail[i].index == 1) _showindex2 = i;
                }
                if (_showindex1 != -1)
                {
                    this.DisplayImage(0, listimgfail[_showindex1].bmpimgfail);
                }
                if (_showindex2 != -1)
                {
                    this.DisplayImage(1, listimgfail[_showindex2].bmpimgfail);
                }
            }

        }

        private void btnLcdReddishTest_Click(object sender, EventArgs e)
        {
            Stopwatch timer = new Stopwatch();

            MData_LCD_REDDISH specLcdReddish = controller.scenarioManger.testSpec.SpecLcdReddish;
            MResult_LCD_REDDISH Result = new MResult_LCD_REDDISH();

            timer.Reset();
            timer.Start();

            WriteLog("--------------------------------------------");
            WriteLog("Lcd Reddish start...");
            WriteLog(Json.GetString(specLcdReddish));

            controller.scenarioManger.visionTest.LcdReddish
                (
                controller.scenarioManger.visionTest.ImageInput[Define.IMAGE_LCD_REDDISH],
                specLcdReddish,
                ref Result);

            //bool bFinalresult = true;
            //if (Result.m_nBlackDot_JudgeSize > specLcdRed.BlackDot_RJudgeSizeUL)
            //{
            //    bFinalresult = false;
            //}

            //WriteLog(string.Format("BlackDot : {0}, Spec( 0 ~ {1})", Result.m_nBlackDot_JudgeSize, specLcdRed.BlackDot_RJudgeSizeUL));
            //WriteLog(string.Format("Result   : {0}", bFinalresult == true ? "PASS" : "FAIL"));
            //timer.Stop();
            //WriteLog(string.Format("Lcd Red end {0}ms...", timer.ElapsedMilliseconds));
            return;
        }

        private void btnLcdMCDTest_Click(object sender, EventArgs e)
        {

        }

        private void btnLcdCOPCrackTest_Click(object sender, EventArgs e)
        {

        }

		private void listItem_Whole_DoubleClick(object sender, EventArgs e)
		{
			return; 
		}

		private void listItem_Whole_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void listItem_Whole_Click(object sender, EventArgs e)
		{
			return;
		}


		// BAE 2017.05.06. Cam Box Click -> Change And Show Failed Image
		static int FailedImageIndex = 0; 
		private void camBox1_Click(object sender, EventArgs e)
		{
			if( controller.ListImgFail.Count > 0 )
			{
				if (FailedImageIndex >= controller.ListImgFail.Count)
					FailedImageIndex = 0;

				DisplayImage(0, controller.ListImgFail[FailedImageIndex].bmpimgfail);
				FailedImageIndex++;
			}

			return;
		}

		private void camBox2_Click(object sender, EventArgs e)
		{
			string _s = ResultImageLogger.GetLastLogFolder();
			if (Directory.Exists(_s))
			{
				Process.Start("explorer.exe", _s);
				return;
			}
		}

        private void btnFrontCamOpen_Click(object sender, EventArgs e)
        {
            if (btnFrontCamOpen.Text == "Front Cam Open")
            {
                controller.CtrlCamFront = new BaslerCamera("acA4600");
                controller.CtrlCamFront.SetPictureBox(GetTeachingPictureBox());
                controller.CtrlCamFront.Start();
                btnFrontCamOpen.Text = "Front Cam Close";
            }
            else
            {
                controller.CtrlCamFront.Stop();
                controller.CtrlCamFront.DestroyCamera();
                controller.CtrlCamFront = null;
                btnFrontCamOpen.Text = "Front Cam Open";
            }
        }

        public Bitmap GetTestImage(TestType testType, string path, int idx = 0)
        {
            Bitmap bmpImage = null;

            switch (testType)
            {
                case TestType.TestLcdArea:
                    bmpImage = new Bitmap(path + @".\01.LcdAreaSrc.jpg");
                    break;
                case TestType.TestDust:
                    bmpImage = new Bitmap(path + @".\03.DustSrc.jpg");
                    break;
                case TestType.TestLcdRed:
                    bmpImage = new Bitmap(path + @".\09.LcdRedSrc.jpg");
                    break;
                case TestType.TestLcdBlue:
                    bmpImage = new Bitmap(path + @".\05.LcdBlueSrc.jpg");
                    break;
                case TestType.TestLcdGreen:
                    bmpImage = new Bitmap(path + @".\07.LcdGreenSrc" + idx + ".jpg");
                    break;
                case TestType.TestLedBlue:
                    bmpImage = new Bitmap(path + @".\05.LcdBlueSrc.jpg");
                    break;
                case TestType.TestEnd:
                    break;
            }

            return bmpImage;
        }
        enum RunStatus
        {
            step1,
            step2,
            step3,
            step4,
        }
        int idx = 0;
        RunStatus runStatus = RunStatus.step1;
        DirectoryInfo dirInfo;
        DirectoryInfo[] dirList;
        int totalCnt;
        string UN;
        private void btnbrowsefolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            string path;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                dirInfo = new DirectoryInfo(path);
                totalCnt = dirInfo.GetDirectories().Length;
                dirList = dirInfo.GetDirectories();
                
                runStatus = RunStatus.step1;
                idx = 0;
                
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            switch (runStatus)
            {
                case RunStatus.step1:
                    controller.eFinalResult = InspectionResult.Pass;
                    ClearTestResult();
                    ClearLogResult();

                    string fullName = dirList[idx].FullName.ToString();
                    string[] split = fullName.Split('_');
                    UN = split[1];


                    Log.AddLog(Environment.NewLine);
                    Log.AddLog("-----------------------------------------------");
                    Log.AddLog(dirList[idx].ToString());
                    if (dirList[idx].GetFiles().Length == 16)
                    {
                        
                        Log.AddLog("--------test started(" + (idx+ 1) + "/" + totalCnt+")---------");
                        
                        // "D:\LCD_AutoInspection\IMG\ + 오늘날짜 폴더 \ Current 폴더 삭제 후 생성
                        ResultImageLogger.ResetDefaultLogFoler(Config.ImageLogFolder);

                        

                        PmLogger.SetCsvClean();
                        PmLogger.SetCsvTime(DateTime.Now);
                        PmLogger.SetCsvUn(UN);

                        controller.InspThreads[0].UpdateTestSpec();
                        controller.InspThreads[0].testArea.ImageInput = GetTestImage(TestType.TestLcdArea, dirList[idx].FullName).ToMat();
                        controller.InspThreads[0].testArea.DoVisonTest_Synch();

                        if (!controller.InspThreads[0].testArea.ResultLcdArea.TestResult)
                        {
                            controller.eFinalResult |= InspectionResult.LcdAreaResult;
                            runStatus = RunStatus.step2;
                            break;
                        }

                        controller.InspThreads[0].testDust.ImageInput = GetTestImage(TestType.TestDust, dirList[idx].FullName).ToMat();
                        controller.InspThreads[0].testDust.DoVisonTest_Synch();
                        if (!controller.InspThreads[0].testDust.ResultLcdArea.TestResult)
                        {
                            controller.eFinalResult |= InspectionResult.LcdAreaResult;
                            runStatus = RunStatus.step2;
                            break;
                        }
                        if (!controller.InspThreads[0].testDust.ResultDust.TestResult)
                        {
                            controller.eFinalResult |= InspectionResult.LcdDustResult;
                            runStatus = RunStatus.step2;
                            break;
                        }
                        controller.InspThreads[0].testLcdGreen.listInputImages.Clear();
                        {
                            for (int i = 0; i < Config.GreenaAcumulateCount; i++)
                            {
                                controller.InspThreads[0].testLcdGreen.listInputImages.Add(GetTestImage(TestType.TestLcdGreen, dirList[idx].FullName, i).ToMat());
                            }
                        }
                        controller.InspThreads[0].testLcdGreen.DoVisionTest();

                        controller.InspThreads[0].testLedBlue.ImageInput = GetTestImage(TestType.TestLedBlue, dirList[idx].FullName).ToMat();
                        controller.InspThreads[0].testLedBlue.DoVisionTest();

                        controller.InspThreads[0].testLcdWhite.ImageInput = GetTestImage(TestType.TestLcdArea, dirList[idx].FullName).ToMat();
                        controller.InspThreads[0].testLcdWhite.DoVisionTest();

                        runStatus = RunStatus.step2;
                    }
                    else
                    {
                        
                        Log.AddLog("--------test skipped(" + (idx + 1) + "/" + totalCnt + ")---------");

                        if (idx < totalCnt - 1)
                            idx++;
                        else if (idx == totalCnt - 1)
                            runStatus = RunStatus.step3;
                    }
                    break;

                case RunStatus.step2:

                    if (
                        controller.InspThreads[0].testLcdWhite.InspStatus == InspectionState.Done
                     //&& controller.InspThreads[0].testLcdRed.InspStatus == InspectionState.Done
                     //controller.InspThreads[0].testLcdBlue.InspStatus == InspectionState.Done
                      && controller.InspThreads[0].testLcdGreen.InspStatus == InspectionState.Done
                      &&controller.InspThreads[0].testLedBlue.InspStatus == InspectionState.Done)
                    {
                        PmLogger.SaveCsvLog();
                        string _s = DateTime.Now.ToString("HHmmss.fff_");

                        if (controller.eFinalResult == InspectionResult.Pass)
                        {
                            ResultImageLogger.RenameImageLogFolder(_s + UN);
                        }
                        else
                        {
                            if (controller.ListImgFail.Count > 0)
                            {
                                DisplayFailImage(controller.ListImgFail);
                            }

                            if ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdDustResult)) == (int)InspectionResult.LcdDustResult
                                || ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdAreaResult)) == (int)InspectionResult.LcdAreaResult))
                            {
                                ResultImageLogger.RenameImageLogFolder(_s + UN);
                            }
                            else
                                ResultImageLogger.RenameImageLogFolder(_s + UN + "_Fail");
                        }
                        if (idx < totalCnt - 1)
                        {
                            idx++;
                            runStatus = RunStatus.step1;
                        }
                        else if (idx == totalCnt - 1)
                            runStatus = RunStatus.step3;

                        break;
                    }
                    break;
            }
            timer2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (btnRearCamOpen.Text == "Rear Cam Open")
            {
                controller.CtrlCamRear = new BaslerCamera("acA3800");
                controller.CtrlCamRear.SetPictureBox(GetTeachingPictureBox());
                controller.CtrlCamRear.Start();
                btnRearCamOpen.Text = "Rear Cam Close";
            }
            else
            {
                controller.CtrlCamRear.Stop();
                controller.CtrlCamRear.DestroyCamera();
                controller.CtrlCamRear = null;
                btnRearCamOpen.Text = "Rear Cam Open";
            }
        }

        private void cbbCtrlCam_DropDown(object sender, EventArgs e)
        {
            cbbCtrlCam.Items.Clear();
            if (tabControl2.SelectedIndex == 0)
            {
                cbbCtrlCam.Items.Add("TEST_DUST");
                cbbCtrlCam.Items.Add("TEST_AREA");
                cbbCtrlCam.Items.Add("TEST_BLUE");
                cbbCtrlCam.Items.Add("TEST_GREEN");
                cbbCtrlCam.Items.Add("TEST_RED");
            }
            else
            {
                cbbCtrlCam.Items.Add("TEST_LOGO");
                cbbCtrlCam.Items.Add("TEST_LABEL");
                cbbCtrlCam.Items.Add("TEST_LASER");
            }
        }

        private void btnTimerStop_Click(object sender, EventArgs e)
        {
            if (btnTimerStop.Text == "Timer Start")
            {
                btnTimerStop.Text = "Timer Stop";
                timer2.Enabled = true;
            }
            else
            {
                btnTimerStop.Text = "Timer Start";
                timer2.Enabled = false;
            }
        }
    }
}
