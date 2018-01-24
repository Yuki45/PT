//#define TESTMODE

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
using System.Collections.Generic;

namespace Auto_Attach.Library
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

        public MainForm mainForm;


        // IO & Comport 
        public IOboard CtrlIOPort;
       

        // Servo
        public Servo CtrlServoPort;
        
        // Sequence
        public BackgroundWorker IOThread;
        bool bThreadLife;

        public int DelayAttach;
        public int DelayPunch;
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



        public Controller(MainForm _mainForm)
        {
            mainForm = _mainForm;
            instance = this; 
        }

        public void CreateDevices()
        {
            bThreadLife = true;                     // always true in-service
            testTimer = new Stopwatch();
            CtrlServoPort = new Servo();
            //CtrlIOPort = new IOboard();
            IOThread = new BackgroundWorker();
            IOThread.DoWork += new DoWorkEventHandler(ThreadIO);
            IOThread.RunWorkerAsync();
            


            EventStart = new AutoResetEvent(false);

            IsPreProcessFisnished = new AutoResetEvent(false); 
           

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
                
                Thread.Sleep(10);       // BAE Added : 2016-11-16 
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

            while (true)
            {
                if (isDryRunning)
                {
                    int delay = 4000;
                    
                    CtrlServoPort.Moving("STANDBY Z");
                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("STANDBY X");
                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("ATTACH LABEL X");
                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("ATTACH LABEL Z");
                    //Vacum ON
                    CtrlIOPort.Output("T01");
                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("STANDBY Z");

                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("ATTACH SET X");
                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("ATTACH SET Z");

                    Thread.Sleep(delay);
                    //Vacum OFF
                    CtrlIOPort.Output("T00");
                    Thread.Sleep(delay);
                    ////Punch On
                    CtrlIOPort.Output("T11");
                    Thread.Sleep(delay);
                    ////Punch On
                    CtrlIOPort.Output("T10");

                    Thread.Sleep(delay);
                    CtrlServoPort.Moving("STANDBY Z");

                }
            }
        }

    }
}


