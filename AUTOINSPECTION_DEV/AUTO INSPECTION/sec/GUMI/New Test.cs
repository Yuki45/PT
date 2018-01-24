
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
using AutoInspection_GUMI;

namespace AutoInspection
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

    public class New_Test
    {
        public static New_Test instance;

        public FrmMainCh2 mainForm;

        // IO & Comport
        public AnyWayControl anyway;
        public AltLightController lightControl;
        public DIO.W_X16Y16_ CtrlIo;

        // Sequence
        public ScenarioManager scenarioManager;
        public BackgroundWorker IOThread;

        bool bThreadLife;
        public InspectionThread[] InspThreads;


        public ICameraInterface ctrlCamFront;
        public ICameraInterface ctrlCamRear;

        public Stopwatch testTimer;

        public bool isNowTesting = false;
        public bool[] isFinshed = new bool[2];
        public bool ResultFront = true;

        public bool FinalResult = true;
        public string UN = "None";
        public string SWVer = "None";

        public bool isDryRunnin = false;

        Thread MasterThread;
        Thread preprocessThread;
        public AutoResetEvent EventStart;
        public AutoResetEvent IsPreProcessFisinished;

        public int retryCountUn = 1;
        public int retryCountTest = 1;

        public List<IMGFAIL> ListImgFaile;

        public void ResetFlags()
        {
            ResultFront = true;
            isFinshed[0] = false;
            isFinshed[1] = false;

            mainForm.ClearTestResult();
            mainForm.ClearFailLIst();

            InspThreads[0].IsAreaDustFinished.Reset();
            InspThreads[0].IsBlueFinished.Reset();
            InspThreads[0].IsGreenFinished.Reset();
            InspThreads[0].IsReddishFinished.Reset();
            InspThreads[0].IsMCDFinished.Reset();
            InspThreads[0].IsCopCrackFinished.Reset();
            InspThreads[0].IsReddishFinished.Reset();
        }

        public New_Test(FrmMainCh2 _mainForm)
        {
            mainForm = _mainForm;
            instance = this;
        }

        public Bitmap GetTestImage(int Channel, int Exposure)
        {
            Bitmap bmpImage = null;

            if (Channel == Define.Front)
            {
                bmpImage = ctrlCamFront.OneShot_(Exposure);
                mainForm.DisplayImage(Define.Front, bmpImage);
            }
            else
            {
                bmpImage = ctrlCamRear.OneShot_(Exposure);
                mainForm.DisplayImage(Define.Rear, bmpImage);
            }

            return bmpImage;
        }

        public void CreateDevices()
        {
            bThreadLife = true;
            testTimer = new Stopwatch();
            scenarioManager = new ScenarioManager();
            anyway = new AnyWayControl();
            lightControl = new AltLightController();
            CtrlIo = mainForm.GetIOControl();
            InspThreads = new InspectionThread[2];
            for (int i = 0; i < InspThreads.Length; i++)
            {
                InspThreads[i] = new InspectionThread();
            }
            IOThread = new BackgroundWorker();
            IOThread.DoWork += new DoWorkEventHandler(ThreadIO);
            IOThread.RunWorkerAsync();


        }

        DateTime st;
        DateTime et;
        bool isReadyToOut = false;
        bool isReadyToStart = false;
        public bool isEMOMode = false;
        public bool isStarting = false;

        public void ThreadIO(object sender, DoWorkEventHandler e)
        {
            while (bThreadLife)
            {
                if (IoIsEMOPushed() == true || IoIsSomethingInAreaSensor() == true)
                {
                    if (isEMOMode == false)
                    {
                        isEMOMode = true;
                        IOEMO();
                    }
                    else
                    {
                        if (isEMOMode == true)
                        {
                            if (CtrlIo.X_01 == 1)
                            {

                            }
                        }

                        if(CtrlIo.Y_04 == 0 
                            && CtrlIo.X_01 == 1
                            && CtrlIo.X_02 == 1
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

                    }
            
                }
                
            }
        }

        enum IOOutput
        {
            BUZZER01, CYL02, CYL03, CYL04, CYL05, LAMP06, LAMP07, LAMP08
        }

        public void StartTest()
        {
            EventStart.Set();
            return;
        }

        public bool IoIsSomethingInAreaSensor()
        {
            if (CtrlIo.X_02 == 1 && CtrlIo.X_03 == 1)
                return false;
            return true;
        }

        public bool IoIsStageInserted()
        {
            if (CtrlIo.X_04 == 1)
                return true;

            return false;
        }

        public bool IoIsStageOut()
        {
            if (CtrlIo.X_05 == 1)
                return true;

            return false;
        }

        public bool IoIsEMOPushed()
        {
            if (CtrlIo.X_06 == 0)
                return true;

            return false;
        }

        public void IOEMO()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = false;
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = false;
        }

        public void IOInitiate()
        {
            mainForm.GetCheckBox((int)IOOutput.BUZZER01).Checked = false;
            Thread.Sleep(50);
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = false;
            Thread.Sleep(50);

        }

        public void IOStageIn()
        {
            mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = true;
            mainForm.GetCheckBox((int)IOOutput.CYL05).Checked = false;
        }

        public void IoWaitUntilStageInserted()
        {
            while (true)
            {
                if (isEMOMode == false && CtrlIo.Y_04 == 0)
                    mainForm.GetCheckBox((int)IOOutput.CYL04).Checked = true;

                if (IoIsStageInserted())
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

                if (IoIsStageOut())
                    break;
                Thread.Sleep(10);
            }
        }

        public void IoPackInsert()
        {
            anyway.Open(Config.sAnyjigComport, Config.nAnyjigBaud);
            if (scenarioManager.testSpec.AnywayType == "Type2")
                anyway.ConvertAnywayUSB2Type();
            if (scenarioManager.testSpec.AnywayType == "TypeC")
                anyway.ConvertAnywayUSBCType();
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = true;
        }

        public void IoPackDisconnect()
        {
            anyway.Close();
            mainForm.GetCheckBox((int)IOOutput.CYL02).Checked = false;
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
                IOStageOut();
                Thread.Sleep(10);
                IoPackDisconnect();
                Thread.Sleep(1000);
            }
        }

        public void Exit()
        {
            bThreadLife = false;
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
            anyway.Write("AT+KEY=15\r");
            Thread.Sleep(50);
            anyway.Write("AT+KEY=15\r");
            Thread.Sleep(200);


            anyway.Write("AT+DISPTEST=0,3\r");
            Thread.Sleep(200);
            anyway.Write("AT+DISPTEST=0,6\r");
            Thread.Sleep(800);

            IsPreProcessFisinished.Set();
            Log.AddLog("dasds");


        }
        MainState mainState;
        public Inspection eFinalResult = InspectionResult.Pass;

        public void threadMaster()
        {
            while (bThreadLife)
            {
                EventStart.Reset();
                EventStart.WaitOne();

                mainForm.DisplayResult("...");
                mainForm.ClearLogResult();
                retryCountUn = 1;
                retryCountTest = 1;
                ListImgFaile.Clear();
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
                            case MainState.StageIn:
                                Log.AddLog(Environment.NewLine);

                                IoPackInsert();
                                IOStageIn();
                                testTimer.Start();
                                IoWaitUntilStageInserted();
                                ResetFlags();
                                

                                if (isDryRunnin == true)
                                    mainState = MainState.ReadyForTest;
                                else
                                    mainState = MainState.HeadInfoCheck;

                                break;

                            case MainState.HeadInfoCheck:
                                Log.AddLog("-------------------------MainState : HeadInfoCheck");
                                
                                break;

                            case MainState.UNIsNull:

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
                                    mainForm.DisplayResult("Please restart");
                                    mainState = MainState.CleanUp;
                                }
                                break;

                            case MainState.ReadyForTest:
                                Log.AddLog("-----------------MainState : ReadyForTest");

                                FinalResult = true;
                                eFinalResult = InspectionResult.Pass;

                                preprocessThread = new Thread(new ThreadStart(PreProcessing));
                                preprocessThread.IsBackground = true;
                                preprocessThread.Start();

                                ResultImageLogger.ResetDefaultLogFoler(Config.ImageLogFolder);

                                mainState = MainState.GrapRearImages;
                                break;

                            case MainState.GrapRearImages:

                                break;

                            case MainState.StartRearTest:

                                InspThreads[Define.Rear].StartTest();

                                mainState = MainState.GrapFrontImages;

                                break;

                            case MainState.GrapFrontImages:
                                Log.AddLog("MainState : GrapFrontImages");

                                if (InspThreads[Define.Front].GrabImage() != true)
                                {
                                    Log.AddLog("ss");
                                    mainState = MainState.CleanUp;
                                    break;
                                }

                                if (isDryRunnin == true)
                                    mainState = MainState.CleanUp;
                                else
                                    mainState = MainState.StartFrontTest;

                                break;
                            case MainState.StartFrontTest:
                                mainState = MainState.WaitTestCompletion;
                                break;

                            case MainState.WaitTestCompletion:
                                if (isFinshed[Define.Front] && isFinshed[Define.Rear])
                                {
                                    Log.AddLog("MainState : WaitTestCompletion");
                                    mainState = MainState.CheckResult;
                                }
                                break;

                            case MainState.CheckResult:
                                Log.AddLog("-----------------MainState : CheckResult");

                                if (eFinalResult == InspectionResult.Pass)
                                {
                                    mainState = MainState.Passed;
                                }
                                else
                                {
                                    mainState = MainState.Failed;
                                }
                                break;

                            case MainState.Passed:
                                mainForm.DisplayResult("PASS");
                                mainForm.DisplayProductionInfo(true);

                                ResultImageLogger.RenameImageLogFolder(DateTime.Now.ToString("HHmmss.fff_" + UN));
                                mainState = MainState.CleanUp;
                                break;

                        }
                    }
                }

            }
        }


        public void RunDry()
        {
            while (isDryRunnin)
            {
                int delay = 500;

                IOInitiate();
                Thread.Sleep(delay);

                IOEMO();
                Thread.Sleep(delay);

                IoIsEMOPushed();
                Thread.Sleep(delay);

                IoIsSomethingInAreaSensor();
                Thread.Sleep(delay);

                IoIsStageInserted();
                Thread.Sleep(delay);

                IoMoveLightLeft();
                Thread.Sleep(delay);

                IoPackDisconnect();
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
