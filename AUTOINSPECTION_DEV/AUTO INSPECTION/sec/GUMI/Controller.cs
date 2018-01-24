//#define TESTMODE

using AutoInspection.Forms;
using AutoInspection.sec;
using AutoInspection.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging; // NJH
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AutoInspection;
using OpenCvSharp;
using System.Collections.Generic;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    [Flags]
    public enum InspectionResult
    {
        Pass = 0,
        LcdDustResult = 1,
        LcdAreaResult = 2,
        LcdWhiteResult = 4,
        LcdRedResult = 8,
        LcdBlueResult = 16,
        LcdGreenResult = 32,
        KeyTouchResult = 64,
        LedBlueResult = 128,
        SvcLedResult = 256,
        BarcodeResult = 512,
        OcrResult = 1024,
        LogoResult = 2048
    };


    public class Controller
    {
        public static Controller instance;

        public FrmMainCh2 mainForm;


        // IO & Comport 
        public AnyWayControl CtrlAnyJig;
        public AltLightController CtrlLight;

        public DIO.W_X16Y16_ CtrlIo;

        // Sequence
        public ScenarioManager scenarioManger;
        public InspectionThread[] InspThreads;
        public BackgroundWorker IOThread;
        bool bThreadLife;

        public ICameraInterface CtrlCamFront;
        public ICameraInterface CtrlCamRear;

        public Stopwatch testTimer;

        public bool isNowTesting = false;
        public bool[] isFinished = new bool[2];
        public bool ResultFront = true;

        public bool FinalResult = true;
        public string UN = "None";
        public string SWVer = "None";


        #region Skip
        //170416 sh32.heo
        //compare the previous tested set 
        //public string PREV_UN = "None";
        //public string PREV2_UN = "None";
        //public List<CvPoint> thisPointError = new List<CvPoint>();
        //public List<CvPoint> prevPointError = new List<CvPoint>();
        //public List<CvPoint> prev2PointError = new List<CvPoint>();
        #endregion

        public static string logFolderName;

        public bool isAreaGrabbed;
        public bool isDustGrabbed;
        public bool isBlueGrabbed;
        public bool isGreenGrabbed;
        public bool isRedGrabbed;
        public bool isMCDGrabbed;
        public bool isCOPCrackGrabbed;
        public bool isReddishGrabbed;

        public bool isDryRunning = false;

        Thread MasterThread;
        Thread preprocessThread;
        public AutoResetEvent EventStart;
        public AutoResetEvent IsPreProcessFisnished;

        public int retryCountUn = 1;
        public int retryCountTest = 1;

        public List<IMGFAIL> ListImgFail;


        public void ResetFlags()
        {
            ResultFront = true;
            isFinished[0] = false;
            isFinished[1] = false;

            mainForm.ClearTestResult();
            mainForm.ClearFailLIst();

            //170514
            InspThreads[0].IsAreaDustFinished.Reset();
            InspThreads[0].IsBlueFinished.Reset();
            InspThreads[0].IsGreenFinished.Reset();
            InspThreads[0].IsRedFinished.Reset();
            InspThreads[0].IsMCDFinished.Reset();
            InspThreads[0].IsCopCrackFinished.Reset();
            InspThreads[0].IsReddishFinished.Reset();

        }

        public Bitmap GetTestImage(int Channel, int Exposure)
        {
            Bitmap bmpImage = null;

            if (Channel == Define.Front)
            {
                bmpImage = CtrlCamFront.OneShot_(Exposure);
                mainForm.DisplayImage(Define.Front, bmpImage);
            }
            else
            {
                bmpImage = CtrlCamRear.OneShot_(Exposure);
                mainForm.DisplayImage(Define.Rear, bmpImage);
            }


            if (bmpImage == null)
            {
                //GetOutStage();
            }
            return bmpImage;
        }

        public Controller(FrmMainCh2 _mainForm)
        {
            mainForm = _mainForm;
            instance = this; 
        }

        public void CreateDevices()
        {
            bThreadLife = true;                     // always true in-service
            testTimer = new Stopwatch();
            scenarioManger = new ScenarioManager();
            CtrlAnyJig = new AnyWayControl();
            CtrlLight = new AltLightController();
            CtrlIo = mainForm.GetIOControl();
            InspThreads = new InspectionThread[2];
            for (int i = 0; i < InspThreads.Length; i++)
            {
                InspThreads[i] = new InspectionThread();
            }
            IOThread = new BackgroundWorker();
            IOThread.DoWork += new DoWorkEventHandler(ThreadIO);
            IOThread.RunWorkerAsync();

            MasterThread = new Thread(new ThreadStart(threadMaster));
            // preprocessThread = new Thread(new ThreadStart(PreProcessing));


            EventStart = new AutoResetEvent(false);

            IsPreProcessFisnished = new AutoResetEvent(false); 
            ListImgFail = new List<IMGFAIL>();

        }

        public void InitDevices()
        {
            // 설정 파일 로드 
            Config.loadFromFile();                                  // 실행 폴더 / 구성 파일 ( setting.json ) 로드 
            scenarioManger.LoadFiles(Config.sCurrnetSpecFile);      // cmd, spec 파일 로드. 

            InspThreads[0].controller = this;
            InspThreads[0].InitThread(0);

            try
            {
                CtrlCamFront = new BaslerCamera("acA4600");
                CtrlCamFront.SetPictureBox(mainForm.GetTeachingPictureBox());
                CtrlCamFront.Start();
            }
            catch( Exception e )
            {
                Log.AddLog(e.ToString());
                Log.AddPmLog(e.ToString());
                MessageBox.Show(e.ToString());
                CtrlCamFront = null;
            }


            InspThreads[1].controller = this;
            InspThreads[1].InitThread(1);   // , scenarioManger.testSpec.testInfo1);

            CtrlCamRear = new BaslerCamera("acA3800");
            CtrlCamRear.SetPictureBox(mainForm.GetTeachingPictureBox());
            CtrlCamRear.Start();
            //// Com Open 
            CtrlAnyJig.Open(Config.sAnyjigComport, Config.nAnyjigBaud);         //  "COM1", 115200

            if (scenarioManger.testSpec.AnywayType == "Type2") CtrlAnyJig.ConvertAnywayUSB2Type();
            if (scenarioManger.testSpec.AnywayType == "TypeC") CtrlAnyJig.ConvertAnywayUSBCType();

            CtrlLight.Open(Config.sLightComport, Config.nLightBaud);            //  "COM2", 9600
            CtrlLight.InitLight();

            CtrlIo._PortName = Config.sIoComport;
            CtrlIo._Connect(true);
            IOStageOut();
            MasterThread.IsBackground = true;
            MasterThread.Start();

        }

        public void DestoryDevices()
        {
            if (CtrlCamFront != null)
            {
                CtrlCamFront.Stop();
                CtrlCamFront.DestroyCamera();
            }
            if (CtrlCamRear != null)
            {
                CtrlCamRear.Stop();
                CtrlCamRear.DestroyCamera();
            }
        }

        DateTime st;
        DateTime et;
        bool isReadyToOut = false;
        bool isReadyToStart = false;
        public bool isEMOMode = false;
        public bool isStarting = false;

        public void ThreadIO(object sender, DoWorkEventArgs e)
        {
            while (bThreadLife)
            {
                if (IoIsEMOPushed() == true || IoIsSomethingInAreaSensor() == true)
                {
                    if(isEMOMode == false)
                    {
                        //Log.AddLog("if(isEMOMode == false)");
                        isEMOMode = true;
                        IOEMO();
                        mainForm.DisplayStatus(MainState.Emergency);
                    }
                }

                else
                {
                    if (isEMOMode == true)
                    {
                        if (CtrlIo.X_01 == 1)
                        {
                            // LampOn(LampColor.GREEN); // NJH 2017.04.23
                            isEMOMode = false;
                            mainForm.DisplayStatus(MainState.Running);
                        }
                        else
                        {
                           //  LampOn(LampColor.YELLOW);
                        }
                    }

                    //When push the button,there should be nothing on area sensor
                    if (CtrlIo.Y_04 == 0 //Y_04 - Cylinder3(Stage) In
                        && CtrlIo.X_01 == 1 //X_01 - SW1
                        && CtrlIo.X_02 == 1 //X_02 - Short AreaSensor
                        && CtrlIo.X_03 == 1
                        && !isStarting)

                    {
                        if (isReadyToStart == false)
                        {
                            st = DateTime.Now;
                            isReadyToStart = true;
                        }

                        et = DateTime.Now;
                        TimeSpan dl = et - st;
                        if (dl.TotalMilliseconds > 100)
                        {
                            isReadyToStart = false;
                            if (mainForm.tabControl1.SelectedIndex == 0)
                            {
                                isStarting = true;
                                StartTest();
                                
                            }
                                
                        }
                    }
                    else
                    {
                        isReadyToStart = false;
                    }


                    if (isNowTesting == false
                        && CtrlIo.Y_04 == 1 
                        && CtrlIo.X_01 == 1 
                        && CtrlIo.X_02 == 1 
                        && CtrlIo.X_03 == 1)
                    {
                        if (isReadyToOut == false)
                        {
                            st = DateTime.Now;
                            isReadyToOut = true;
                        }
                        et = DateTime.Now;
                        TimeSpan dl = et - st;
                        if (dl.TotalMilliseconds > 300)
                        {
                            isReadyToOut = false;
                            GetOutStage();
                        }
                    }
                    else
                    {
                        isReadyToOut = false;
                    }
                    
                    Thread.Sleep(10);       // BAE Added : 2016-11-16 
                }

            }
        }

        /*
        string path;
        public void MakeSavePath(string imageFolder, DateTime date, string UN)
        {
            try
            {
                // logFolderName = date.ToString("yyyyMMdd_HHmm.ss") + "_" + UN +"\\";
                logFolderName = date.ToString("yyyyMMdd") + "\\" + "Time "+ DateTime.Now.ToString(" HHmm_") + UN + "\\"; // NJH
                path = imageFolder + logFolderName; // NJH
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Log.AddLog("MakeSavePath() in Controller exception " + ex.ToString());
                //Logger.Write("MakeSavePath() in Controller exception " + ex.ToString());
            }
        }
        public void RemoveSavePath()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.Delete(true);
                Thread.Sleep(100);
                //Directory.Delete(path);
            }
            catch (Exception ex)
            {
                Log.AddLog("RemoveSavePath() in Controller exception " + ex.ToString());
                // Logger.Write("RemoveSavePath() in Controller exception " + ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        */


        //Y_01 : BUZZER
        //Y_02 : PACK
        //Y_03: Bar Light Moving Cylinder
        //Y_04,05 : Stage Moving Cylinder
        //Y_06,07,08 : LAMP(YELLOW, GREEN, RED)
        //x1, x2 : Switch
        //x3 : Area(Safety) Sensor
        //4,5 : Cylinder Sensor
        //x6 : EMO Switch

        enum IOOutput
        {
            BUZZER01, CYL02, CYL03, CYL04, CYL05, LAMP06, LAMP07, LAMP08
        }

        public bool IoIsSomethingInAreaSensor() // 이물 있으면 true, 이물 없으면 false
        {
            if (CtrlIo.X_02 == 1 && CtrlIo.X_03 == 1) // X_02, X_03 - AreaSensor
                return false;
            return true;
        }

        public bool IoIsStageInserted()
        {

            if (CtrlIo.X_04 == 1) // X_04 - CylinderSensor In
                return true;

            return false;
        }

        public bool IoIsStageOut()
        {
            if (CtrlIo.X_05 == 1) // X_05 - CylinderSensor Out
                return true;

            return false;
        }

        public bool IoIsEMOPushed()
        {
            if (CtrlIo.X_06 == 0) // X_06 - Emergency
                return true;

            return false;
        }

        public void DeleteImage()
        {
            try
            {
                if (Directory.Exists(Config.logImagePath))
                {
                    DirectoryInfo di = new DirectoryInfo(Config.logImagePath);
                    di.Delete(true);
                }

                Directory.CreateDirectory(Config.logImagePath);
                Thread.Sleep(30);
                Directory.CreateDirectory(Config.logImageArea);
                Directory.CreateDirectory(Config.logImageDust);
                Directory.CreateDirectory(Config.logImageLcdBlue);
                Directory.CreateDirectory(Config.logImageLedBlue);
                Directory.CreateDirectory(Config.logImageLcdGreen);
                Directory.CreateDirectory(Config.logImageLcdRed);
                Directory.CreateDirectory(Config.logImageLcdWhite);
                Directory.CreateDirectory(Config.logImageRear);
                Directory.CreateDirectory(Config.logImageLcdTouch);
                Directory.CreateDirectory(Config.logImageLcdMCD);
                Directory.CreateDirectory(Config.logImageLcdCOPCrack);
                Directory.CreateDirectory(Config.logImageLcdReddish);
            }
            catch (Exception ex)
            {
                Log.AddLog(ex.ToString());
                Log.AddPmLog(ex.ToString());
            }
        }
        
        public void IOEMO()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = false; //Y_04 - Cylinder3(Stage) In
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = false; //Y_05 - Cylinder3(Stage) Out
        }

        //public void LampOn(LampColor lc) // NJH, 2017.04.23
        //{
        //    if (lc == LampColor.GREEN)
        //    {
        //        mainForm.GetCheckBox((int)IOOutput.LAMP06).Checked = false;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP07).Checked = true;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP08).Checked = false;
        //    }
        //    else if (lc == LampColor.RED)
        //    {
        //        mainForm.GetCheckBox((int)IOOutput.LAMP06).Checked = false;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP07).Checked = false;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP08).Checked = true;
        //    }
        //    else if(lc == LampColor.YELLOW)
        //    {
        //        mainForm.GetCheckBox((int)IOOutput.LAMP06).Checked = true;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP07).Checked = false;
        //        mainForm.GetCheckBox((int)IOOutput.LAMP08).Checked = false;

        //    }
        //}

        public enum LampColor
        {
            GREEN, YELLOW, RED
        }
        

        public void IOInitiate()
        {

            mainForm.GetCheckBox((int)IOOutput.BUZZER01).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.CYL03).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = true;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.LAMP06).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.LAMP07).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.LAMP08).Checked = false;

        }

        public void IOStageIn()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = true;
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = false;
        }

        public void IoWaitUntilStageInserted()
        {
            //Thread.Sleep(1000);
            //return;

            while (true)
            {

                if (isEMOMode == false && CtrlIo.Y_04 == 0)
                    mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = true;
#if TESTMODE
#else
                if (IoIsStageInserted())
#endif
                    break;
                Thread.Sleep(10);
            }
        }

        public void IoWaitUntilStageOut()
        {
            while (true)
            {
                if (isEMOMode == false && CtrlIo.Y_05 == 0)
                    mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = true;
#if TESTMODE
#else
                if (IoIsStageOut())
#endif
                    break;
                Thread.Sleep(10);
            }
        }

        public void IoPackInsert()
        {
            CtrlAnyJig.Open(Config.sAnyjigComport, Config.nAnyjigBaud);
            if (scenarioManger.testSpec.AnywayType == "Type2") CtrlAnyJig.ConvertAnywayUSB2Type();
            if (scenarioManger.testSpec.AnywayType == "TypeC") CtrlAnyJig.ConvertAnywayUSBCType();
            //Thread.Sleep(200);
            //Thread.Sleep(100);
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = true;
        }

        public void IoPackDisconnect()
        {
            
            CtrlAnyJig.Close();
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = false;
        }


        public void TurnOnFrontLight()
        {
            CtrlLight.ClearLightValue();
            CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sFrontLight1]
                , scenarioManger.testSpec.SpecLight.DustLightValue);

            CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sFrontLight2]
                , scenarioManger.testSpec.SpecLight.DustLightValue);
            CtrlLight.TurnOnLight();
        }

        public void TurnOffAllLight()
        {
            CtrlLight.ClearLightValue();
            CtrlLight.TurnOnLight();
        }

        public void TurnOffFrontLight()
        {
            CtrlLight.ClearLightValue();
            CtrlLight.TurnOnLight();
        }

        public void TurnOffRearLight()
        {
            CtrlLight.ClearLightValue();
            CtrlLight.TurnOnLight();
        }

        public void IoMoveLightRight()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL03).Checked = false;
        }

        public void IoMoveLightLeft()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL03).Checked = true;
        }

        public void IOStageOut()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = false;
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = true;
        }

        public void GetOutStage()
        {
            if (IoIsSomethingInAreaSensor() == false)
            {
                isNowTesting = false;
                IOStageOut();               // CtrlIo.Y_04 = 0;
                Thread.Sleep(10);
                IoPackDisconnect();         // CtrlIo.Y_05 = 0;
                Thread.Sleep(1000);         // delay for out Thread.Sleep(Config.nMainCylinderDelay); 
            }
            else
            {
                MessageBox.Show("check the area sensor");
            }
        }

        public enum RearTestType { Logo, Label, Laser }

        // 0 : bar light 1 ,2 : back light
        public void TurnOnRearLight(RearTestType tt)
        {
            if (tt == RearTestType.Logo)
            {
                CtrlLight.ClearLightValue();

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sBarLight]
                    , scenarioManger.testSpec.SpecLight.LogoBarLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight1]
                    , scenarioManger.testSpec.SpecLight.LogoBackLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight2]
                    , scenarioManger.testSpec.SpecLight.LogoBackLightValue);

                CtrlLight.TurnOnLight();
            }
            else if (tt == RearTestType.Label)
            {
                CtrlLight.ClearLightValue();

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sBarLight]
                    , scenarioManger.testSpec.SpecLight.LabelBarLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight1]
                    , scenarioManger.testSpec.SpecLight.LabelBackLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight2]
                    , scenarioManger.testSpec.SpecLight.LabelBackLightValue);

                CtrlLight.TurnOnLight();
            }
            else if (tt == RearTestType.Laser)
            {
                CtrlLight.ClearLightValue();

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sBarLight]
                    , scenarioManger.testSpec.SpecLight.LaserBarLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight1]
                    , scenarioManger.testSpec.SpecLight.LaserBackLightValue);

                CtrlLight.SetLightValue(CtrlLight.lightChannel[CtrlLight.sRearLight2]
                    , scenarioManger.testSpec.SpecLight.LaserBackLightValue);

                CtrlLight.TurnOnLight();
            }
        }

        public enum MainState
        {
            Running,
            Emergency,
            StageIn,

            HeadInfoCheck,
            UNIsNull,

            ReadyForTest,
            
            GrapRearImages,
            StartRearTest,
            GrapFrontImages,
            StartFrontTest, 
            
            StartTest,
            WaitTestCompletion,
            CheckResult,
            CheckToRetry,
            Failed,
            Passed,
            CleanUp,
            End
        }

        public void PreProcessing()
        {
            // 2017.05.10. BAE : 시간 단축 테스트 ( JHS Ver.)
            
            CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            Thread.Sleep(50);
            CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            Thread.Sleep(200);
            CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            Thread.Sleep(50);
            CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            Thread.Sleep(50);
            
            CtrlAnyJig.Write("AT+DISPTEST=0,3\r");   //  Name="SCRON"/>	9
            Thread.Sleep(200); 
            CtrlAnyJig.Write("AT+DISPTEST=0,6\r");   //  Name="WHITE"/>	5
            Thread.Sleep(800); 
            
            IsPreProcessFisnished.Set();
            Log.AddLog("Controller IsPreProcessFisnished Set");


            // 2017.05.10. BAE : 시간 단축 테스트 이전
            //CtrlAnyJig.Write("AT+DISPTEST=3,0\r");   //  Name="SCRMAXBRIGHT"/>
            //Thread.Sleep(50);
            //CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            //Thread.Sleep(50);
            //CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            //Thread.Sleep(200);
            //CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            //Thread.Sleep(50);
            //CtrlAnyJig.Write("AT+KEY=15\r");         //  Name="back key"/>
            //Thread.Sleep(50);

            //CtrlAnyJig.Write("AT+DISPTEST=0,3\r");   //  Name="SCRON"/>	9
            //Thread.Sleep(600);
            //CtrlAnyJig.Write("AT+DISPTEST=0,6\r");   //  Name="WHITE"/>	5
            //Thread.Sleep(50);
            //CtrlAnyJig.Write("AT+LEDLAMPT=0,1\r");   //  Name="LEDWHITE"/>		14
            //Thread.Sleep(50);
            //CtrlAnyJig.Write("AT+DISPTEST=0,3\r");   //  Name="SCRON"/>	9
            //Thread.Sleep(800);

            //IsPreProcessFisnished.Set();
            //Log.AddLog("Controller IsPreProcessFisnished Set");


        }

        MainState mainState;
        public InspectionResult eFinalResult = InspectionResult.Pass;

        public void threadMaster()
        {
            while (bThreadLife)
            {
                EventStart.Reset();
                EventStart.WaitOne();   // Event Wait
                                        // DeleteImage();
                mainForm.DisplayResult("...");
                mainForm.ClearLogResult(); // Add Nam jh
                retryCountUn = 1;
                retryCountTest = 1;
                ListImgFail.Clear();
                testTimer.Reset();
                
                isNowTesting = true;

                mainState = MainState.StageIn;

                while (mainState != MainState.End)
                {
                    if (isEMOMode == true)
                    {
                        while (true)
                        {
                            if (isEMOMode == false)
                                break;

                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        switch (mainState)
                        {
                            case MainState.StageIn:                         // #1 Stage In
                                Log.AddLog(Environment.NewLine);
                                Log.AddLog("-----------------------------MainState : StageIn");

                                IoPackInsert();    
                                IOStageIn();    
                                Thread.Sleep(100);
                                testTimer.Start();
                                IoWaitUntilStageInserted();

                                ResetFlags();

                                if (isDryRunning == true)
                                    mainState = MainState.ReadyForTest;
                                else
                                    mainState = MainState.HeadInfoCheck;

                                break;

                            case MainState.HeadInfoCheck:                   // #2 Head Info Check 
                                Log.AddLog("-----------------------------MainState : HeadInfoCheck");

                                HeadInfoData _HIData = CtrlAnyJig.NEW_HEAD_INFO();
                                if (_HIData == null)
                                {
                                    UN = "None";
                                }
                                else
                                {
                                    UN = _HIData.UniqueNo;
                                    SWVer = _HIData.Version;
                                    Log.AddLog("UN : (" + UN + ")");

                                    PmLogger.SetCsvClean();
                                    PmLogger.SetCsvTime(DateTime.Now); 
                                    PmLogger.SetCsvUn(UN);
                                }

                                if (UN != "None")
                                {
									Log.AddHeadInfo(UN, SWVer);
                                    mainState = MainState.ReadyForTest;
                                }
                                else
                                {
                                    mainState = MainState.UNIsNull;
                                }

                                break;

                            case MainState.UNIsNull:
                                Console.WriteLine("MainState : UNIsNull");
                                if (retryCountUn > 0)
                                {
                                    IoPackDisconnect();
                                    Thread.Sleep(500);
                                    IoPackInsert();
                                    --retryCountUn;
                                    mainState = MainState.HeadInfoCheck;
                                }
                                else
                                {
                                    mainForm.DisplayResult("Plesase restart.");
                                    mainState = MainState.CleanUp;
                                }

                                break;

                            case MainState.ReadyForTest:
								Log.AddLog("-----------------------------MainState : ReadyForTest");

                                FinalResult = true;
                                eFinalResult = InspectionResult.Pass;

                                preprocessThread = new Thread(new ThreadStart(PreProcessing));
								preprocessThread.IsBackground = true; // BAE 2017.04.23
                                preprocessThread.Start();

                                // "D:\LCD_AutoInspection\IMG\ + 오늘날짜 폴더 \ Current 폴더 삭제 후 생성
                                ResultImageLogger.ResetDefaultLogFoler(Config.ImageLogFolder);



                                mainState = MainState.GrapRearImages;
                                break;

                            case MainState.GrapRearImages:
                                Log.AddLog("-----------------------------MainState : GrapRearImages");

                                if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestRear == false)
                                {
                                    isFinished[Define.Rear] = true;
                                    mainState = MainState.GrapFrontImages;
                                }
                                else if (scenarioManger.testSpec.SpecTestList.TestLogo == false
                                    && scenarioManger.testSpec.SpecTestList.TestLaser == false)
                                {
                                    isFinished[Define.Rear] = true;
                                    mainState = MainState.GrapFrontImages;
                                }
                                else
                                {
                                    Bitmap bmpImage = null;
                                    TestSpec _ModelSpec = scenarioManger.testSpec;

                                    //1. logo 
                                    if (scenarioManger.testSpec.SpecTestList.TestLogo)
                                    {
                                        TurnOnRearLight(Controller.RearTestType.Logo);
										//logo2개 검사 
                                        if (Controller.instance.scenarioManger.testSpec.SpecRear.LogoCount == 2)
                                        {
                                            IoMoveLightLeft();
                                            Thread.Sleep(200);
                                        }
                                        Thread.Sleep(200);

                                        // controller.mainForm.DisplayStatus(Channel, "Logo Test Started");
                                        bmpImage = GetTestImage(Define.Rear, _ModelSpec.SpecExposure.TestLogo);
                                        
                                        InspThreads[Define.Rear].testLogo1.SetSrcImage(bmpImage);
                                        if (scenarioManger.testSpec.SpecRear.LogoCount == 2)
                                        {
                                            InspThreads[Define.Rear].testLogo2.SetSrcImage(bmpImage);
                                            IoMoveLightRight();
                                            Thread.Sleep(200);
                                        }
                                        //mainForm.DisplayImage(Define.Rear, bmpImage);
                                        bmpImage.Dispose();
                                        GC.Collect();
                                        // mainForm.DisplayStatus(Channel, "Barcode Test Started");
                                    }

                                    if (scenarioManger.testSpec.SpecTestList.TestLaser)
                                    {
                                        //IoMoveLightRight();
                                        //Thread.Sleep(500);

                                        //2. Barcode 
                                        TurnOnRearLight(Controller.RearTestType.Label);
                                        Thread.Sleep(500);
                                        bmpImage = GetTestImage(Define.Rear, _ModelSpec.SpecExposure.TestImeiLabel);

                                        InspThreads[Define.Rear].testBarcode.SetSrcImage(bmpImage);
                                        //mainForm.DisplayImage(Define.Rear, bmpImage);
                                        bmpImage.Dispose();
                                        GC.Collect();

                                        //controller.mainForm.DisplayStatus(Channel, "OCR Test Started");
                                        TurnOnRearLight(Controller.RearTestType.Laser);
                                        Thread.Sleep(200);

                                        bmpImage = GetTestImage(Define.Rear, _ModelSpec.SpecExposure.TestImeiLaser);
                                        bmpImage.Save(@".\Detail\Rear\ocr.bmp");

                                        //turn off the light
                                        TurnOffRearLight();
                                        //IoMoveLightLeft();

                                        InspThreads[Define.Rear].testOCR.SetSrcImage(bmpImage);
                                        // controller.mainForm.DisplayImage(Channel, bmpImage);
                                        bmpImage.Dispose();
                                        GC.Collect();
                                    }
                                    if (isDryRunning == true)
                                        mainState = MainState.GrapFrontImages;
                                    else
                                        mainState = MainState.StartRearTest;
                                }
                                break;

                            case MainState.StartRearTest:
                                Log.AddLog("-----------------------------MainState : StartRearTest");

                                InspThreads[Define.Rear].StartTest();

                                //if (scenarioManger.testSpec.SpecTestList.TestFront)
                                    mainState = MainState.GrapFrontImages;
                                //else
                                //{
                                //    isFinished[Define.Front] = true;
                                //    mainState = MainState.WaitTestCompletion;
                                //}
                                break;

                            case MainState.GrapFrontImages:
                                Log.AddLog("-----------------------------MainState : GrapFrontImages");

                                if (InspThreads[Define.Front].GrabImage() != true)
                                {
                                    Log.AddLog("InspThreads[Define.Front].GrabImage() != true");
                                    mainState = MainState.CleanUp;
                                    break;
                                }

                                if (isDryRunning == true)
                                    mainState = MainState.CleanUp;
                                else
                                    mainState = MainState.StartFrontTest;
                                break;

                            case MainState.StartFrontTest:
                                Log.AddLog("-----------------------------MainState : StartFrontTest");

                                //InspThreads[Define.Front].StartTest();

                                mainState = MainState.WaitTestCompletion;
                                break;

                            case MainState.WaitTestCompletion:              // #4 Wait Until Tow Test Thread Ending
                                if (isFinished[Define.Front] == true && isFinished[Define.Rear] == true)
                                {
                                    Log.AddLog("MainState : WaitTestCompletion");
                                    mainState = MainState.CheckResult;
                                }
                                break;

                            case MainState.CheckResult:                     // #5 Check Result
                                Log.AddLog("-----------------------------MainState : CheckResult");
                                if (eFinalResult == InspectionResult.Pass)
                                {
                                    mainState = MainState.Passed;
                                }
                                else
                                {
                                    mainState = MainState.Failed;
                                }
                                break;

                            case MainState.Failed:
                                Log.AddLog("-----------------------------MainState : Failed(" + UN + ")");
                                Log.AddPmLog(string.Format("MainState: FailUN : ({0}), ", UN) + PmLogger.GetLogMsg(UN, false));
                                PmLogger.SetCsvResult("FAIL");

                                mainForm.DisplayResult("FAIL");
                                mainForm.DisplayProductionInfo(false);

                                #region Skip
                                //170416 sh32.heo
                                //compare the previous tested set 
                                //prev2PointError.Clear();
                                //foreach (CvPoint cvp in prevPointError)
                                //{
                                //    prev2PointError.Add(cvp);
                                //}
                                //prevPointError.Clear();
                                //foreach (CvPoint cvp in thisPointError)
                                //{
                                //    prevPointError.Add(cvp);
                                //}
                                //PREV2_UN = PREV_UN;
                                //PREV_UN = UN; 
                                #endregion

                                if (ListImgFail.Count > 0)
                                {
                                    mainForm.DisplayFailImage(ListImgFail);
                                }
                                Log.AddEndInfo(false, Config.CountPass + Config.CountFail, Config.CountFail);

                                // if (Config.SaveResultImage == true) InspThreads[0].SaveImage();


                                string _s = DateTime.Now.ToString("HHmmss.fff_");
                                if ((((int)eFinalResult) & ((int)InspectionResult.LcdDustResult)) == (int)InspectionResult.LcdDustResult
                                    || ((((int)eFinalResult) & ((int)InspectionResult.LcdAreaResult)) == (int)InspectionResult.LcdAreaResult))
                                {
                                    ResultImageLogger.RenameImageLogFolder(_s + UN);
                                }
                                else

                                ResultImageLogger.RenameImageLogFolder(_s + UN + "_Fail");

                                mainState = MainState.CleanUp;
                                break;

                            case MainState.Passed:
                                Log.AddLog("-----------------------------MainState : Passed(" + UN + ")");
                                Log.AddPmLog(string.Format("MainState: Passed UN : ({0})", UN) + PmLogger.GetLogMsg(UN, true));
                                PmLogger.SetCsvResult("PASS");

                                mainForm.DisplayResult("PASS");
                                mainForm.DisplayProductionInfo(true);

                                //170416 don't remove logimg folder
                                //RemoveSavePath();
                                Log.AddEndInfo(true, Config.CountPass + Config.CountFail, Config.CountFail);

                                ResultImageLogger.RenameImageLogFolder(DateTime.Now.ToString("HHmmss.fff_" + UN));

                                mainState = MainState.CleanUp;
                                break;

                            case MainState.CleanUp:
                                Log.AddLog("-----------------------------MainState : CleanUp");

                                isNowTesting = false;
                                testTimer.Stop();
                                TurnOffAllLight();
                                CtrlAnyJig.Write("AT+DISPTEST=3,1\r");
                                Thread.Sleep(100);
                                CtrlAnyJig.Write("AT+DISPTEST=0,4\r");
                                Thread.Sleep(10);
                                CtrlAnyJig.Write("AT+LEDLAMPT=0,0\r");
                                Thread.Sleep(50);

                                //170416 power off if it need
                                if (eFinalResult == InspectionResult.Pass && Config.SetPowerOFF)
                                {
                                    CtrlAnyJig.Write("AT+POWRESET=0,1\r");
                                    Thread.Sleep(10);
                                }
                                IoPackDisconnect();
                                IOStageOut();
                                Thread.Sleep(10);

                                IoWaitUntilStageOut();
								
                                
                                if(preprocessThread!=null)
                                    preprocessThread.Abort();

                                if (isDryRunning == true)
                                {
                                    mainState = MainState.StageIn;
                                    mainForm.DisplayProductionInfo(true);
                                }
                                else
                                    mainState = MainState.End;

                                
                                Log.AddPmLog(Environment.NewLine);
                                Log.SavePmLog();
                                PmLogger.SaveCsvLog();
                                break;

                            case MainState.End:
                                Log.AddLog("-----------------------------MainState : End");
                                preprocessThread.Abort();
                                break;
                        }
                        Log.SaveLog();
                    }
                    Thread.Sleep(10);
                }
                isStarting = false;
            }
        }

        public static void SaveResultImage(string fileName, Bitmap bmpImage)
        {
            string path = ResultImageLogger.GetImageLogFolder();

            if (Directory.Exists(path))
            {
                if (Config.ImageLogFIleExtension == "jpg")
                {
                    bmpImage.Save(path + fileName + "." + Config.ImageLogFIleExtension, ImageFormat.Jpeg);
                }
                else if (Config.ImageLogFIleExtension == "bmp")
                {
                    bmpImage.Save(path + fileName + "." + Config.ImageLogFIleExtension, ImageFormat.Bmp);
                }
            }
            else
            {
                Log.AddLog(string.Format( "Error] SaveLogImage(), Path({0}) Dose not exist.", path)); 
            }
        }

        public void StartTest()
        {
            EventStart.Set();
            return;
        }

        public void Exit()
        {
            bThreadLife = false;
        }

        public void RunDry()
        {

            while (isDryRunning)
            {

                int delay = 500;

                IOStageIn();                                // CtrlIo.Y_04 = 1;
                Thread.Sleep(delay);

                IoPackInsert();                             // insert the pack CtrlIo.Y_05 = 1; // 
                Thread.Sleep(delay);

                IoWaitUntilStageInserted();
                Thread.Sleep(delay);

                TurnOnRearLight(Controller.RearTestType.Label);
                Thread.Sleep(delay);

                IoMoveLightRight();
                Thread.Sleep(delay);

                TurnOnRearLight(Controller.RearTestType.Laser);
                Thread.Sleep(delay);

                IoMoveLightLeft();
                Thread.Sleep(delay);

                TurnOffRearLight();
                Thread.Sleep(delay);

                IoMoveLightLeft();
                Thread.Sleep(delay);

                TurnOffRearLight();
                Thread.Sleep(delay);

                TurnOffFrontLight();
                Thread.Sleep(delay);

                TurnOnFrontLight();
                Thread.Sleep(delay);

                IoPackDisconnect();
                Thread.Sleep(delay);

                IOStageOut();
                Thread.Sleep(delay);

                IoWaitUntilStageOut();
                Thread.Sleep(delay);
            }
        }

    }
    public class IMGFAIL
    {
        public Bitmap bmpimgfail { get; set; }
        public int index { get; set; }
        public IMGFAIL() { }
    }
}

