using AutoInspection.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using OpenCvSharp.Extensions;
using AutoInspection;
using AutoInspection.Utils;

namespace AutoInspection_GUMI
{
    public class InspectionThread
    {
        public TestType testType;
        public int Channel;

        public Controller controller;
        public BackgroundWorker workerThreadMain;

        VisionTest visionTest;
        FrmMainCh2 mainForm;
        VisionTools visionTools;

        private List<Inspection> inspectionList;

        //Thread grabImg;
        public InspectionLcdArea testArea;
        public InspectionLcdDust testDust;
        public InspectionLcdWhite testLcdWhite;
        public InspectionLcdRed testLcdRed;
        public InspectionLcdBlue testLcdBlue;
        public InspectionLcdGreen testLcdGreen;
        public InspectionKeyTouch testKeyTouch;
        public InspectionLedBlue testLedBlue;
        public InspectionLogo testLogo1;
        public InspectionLogo testLogo2;
        public InspectionBarcode testBarcode;
        public InspectionOcr testOCR;
        public InspectionLcdMCD testLcdMCD;
        public InspectionLcdCopCrack testLcdCOPCrack;
        public InspectionLcdReddish testLcdReddish;
        TestSpec testSpec;

        // hunsibi
        public AutoResetEvent IsAreaDustFinished;
        public AutoResetEvent IsBlueFinished;
        public AutoResetEvent IsGreenFinished;
        public AutoResetEvent IsRedFinished;
        public AutoResetEvent IsMCDFinished;
        public AutoResetEvent IsCopCrackFinished;
        public AutoResetEvent IsReddishFinished;

        int retryCountGrapImage = 1;

        public long GetElasedTime()
        {
            if (workerThreadMain.IsBusy)
            {
                return controller.testTimer.ElapsedMilliseconds;
            }
            return 0;
        }

        public InspectionThread()
        {
            workerThreadMain = new BackgroundWorker();
            //grabImg = new Thread(new ThreadStart(GrabImage));
            inspectionList = new List<Inspection>();

            IsAreaDustFinished = new AutoResetEvent(false);
            IsBlueFinished     = new AutoResetEvent(false);
            IsGreenFinished    = new AutoResetEvent(false);
            IsRedFinished      = new AutoResetEvent(false);
            IsMCDFinished      = new AutoResetEvent(false);
            IsCopCrackFinished = new AutoResetEvent(false);
            IsReddishFinished  = new AutoResetEvent(false);
        }

        public void InitThread(int _channel)// public void InitThread( int _channel, List<TestType> _testInfo)
        {
            testSpec = controller.scenarioManger.testSpec;
            visionTools = controller.scenarioManger.visionTools;
            visionTest = controller.scenarioManger.visionTest;
            mainForm = controller.mainForm;

            Channel = _channel; // testInfo = _testInfo;
            workerThreadMain.DoWork += new DoWorkEventHandler(Run);
            workerThreadMain.RunWorkerCompleted += EndTest;
            //grabImg.WorkerSupportsCancellation = true;
            //grabImg.DoWork += new DoWorkEventHandler(GrabImage);

            testArea = new InspectionLcdArea("LcdArea", visionTest, Config.ImageLogFolder, controller);
            testDust = new InspectionLcdDust("LcdDust", testSpec.SpecLcdArea, testSpec.SpecLcdDust, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLcdWhite = new InspectionLcdWhite("LcdWhite", testSpec.SpecLcdWhite, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLcdRed = new InspectionLcdRed("LcdRed", testSpec.SpecLcdRed, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLcdGreen = new InspectionLcdGreen("LcdGreen", testSpec.SpecLcdGreen, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLcdBlue = new InspectionLcdBlue("LcdBlue", testSpec.SpecLcdBlue, visionTest, Config.ImageLogFolder, mainForm, controller);
            testKeyTouch = new InspectionKeyTouch("Test Key Touch", testSpec.SpecLcdArea, testSpec.SpecTouchKey, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLedBlue = new InspectionLedBlue("LedBlue", testSpec.SpecLedBlue, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLogo1 = new InspectionLogo("Logo1", testSpec.SpecRear, visionTest, Config.ImageLogFolder, 1, controller);
            testLogo2 = new InspectionLogo("Logo2", testSpec.SpecRear, visionTest, Config.ImageLogFolder, 2, controller);
            testBarcode = new InspectionBarcode("Barcode", testSpec.SpecRear, visionTest, Config.ImageLogFolder, mainForm, controller);
            testOCR = new InspectionOcr("OCR", testSpec.SpecRear, visionTest, Config.ImageLogFolder, mainForm, controller);
            testLcdMCD = new InspectionLcdMCD("LcdMCD", testSpec.SpecLcdMCD, visionTest, Config.ImageLogFolder,mainForm, controller);
            testLcdCOPCrack = new InspectionLcdCopCrack("LcdCOPCrack", testSpec.SpecLcdCOPCrack, visionTest, Config.ImageLogFolder,mainForm, controller);
            testLcdReddish = new InspectionLcdReddish("LcdReddish", testSpec.SpecLcdReddish, visionTest, Config.ImageLogFolder, mainForm, controller);
        }


        public void UpdateTestSpec()
        {
            testSpec = controller.scenarioManger.testSpec;

            testArea.specLcdArea = testSpec.SpecLcdArea;
            testDust.specLcdArea = testSpec.SpecLcdArea;
            testDust.specLcdDust = testSpec.SpecLcdDust;
            testLcdWhite.testSpec = testSpec.SpecLcdWhite;
            testLcdRed.testSpec = testSpec.SpecLcdRed;
            testLcdGreen.testSpec = testSpec.SpecLcdGreen;
            testLcdBlue.testSpec = testSpec.SpecLcdBlue;
            testKeyTouch.specLcdArea = testSpec.SpecLcdArea;
            testKeyTouch.specTouchKey = testSpec.SpecTouchKey;
            testLedBlue.testSpec = testSpec.SpecLedBlue;
            testLogo1.testSpec = testSpec.SpecRear;
            testLogo2.testSpec = testSpec.SpecRear;
            testBarcode.testSpec = testSpec.SpecRear;
            testOCR.testSpec = testSpec.SpecRear;
        }

        public void StartTest()
        {
            try
            {
                if (!workerThreadMain.IsBusy)
                {
                    SetTestType(TestType.TestReady);
                    workerThreadMain.RunWorkerAsync();
                }
                else
                {
                    Log.AddLog(string.Format("StartTest() failed : {0}", Channel));
                }
            }
            catch (Exception ex)
            {
                Log.AddLog("StartTest() in TestThread exception " + ex.ToString());
                Log.AddPmLog("StartTest() in TestThread exception " + ex.ToString());
            }
        }

        /*
        public void StopGrap()
        {
            try
            {
                //grabImg.Join();
                //grabImg.Abort();
                //grabImg = new Thread(new ThreadStart(GrabImage));
                //grabImg.CancelAsync();
                //while (grabImg.IsBusy == true)
                //{
                //    Thread.Sleep(10);
                //}
            }
            catch (Exception ex)
            {
                Log.AddLog("StopGrab() in TestThread exception " + ex.ToString());
            }
            return;
        }
        */

        //public void StartGrab()
        //{
            //try
            //{
            //    if (grabImg.ThreadState != System.Threading.ThreadState.Running)
            //        grabImg.Start();
            //}
            //catch (Exception ex)
            //{
            //    Logger.Write("StartGrab() in TestThread exception " + ex.ToString());
            //}
            //if (!grabImg.IsBusy)
            //{
            //    grabImg.RunWorkerAsync();
            //}
            //else
            //{
            //    Console.WriteLine(string.Format("{0} Capture thread failed", Channel));
            //}
        //}

        private void EndTest(object sender, RunWorkerCompletedEventArgs e)
        {
            return;
        }

        public void StopRequested()
        {
            workerThreadMain.CancelAsync();
        }

        void SetTestType(TestType _testType)
        {
            testType = _testType;
        }

        ////need to modify
        //public void SaveImage()
        //{
        //    try
        //    {
        //        if (controller.ResultFront == false)
        //        //if (testDust.ResultDust.TestResult == false ||
        //        //    testDust.ResultLcdArea.TestResult == false ||
        //        //    testKeyTouch.ResultTouchKey.TestResult == false ||
        //        //    testLcdWhite.testResult.TestResult == false ||
        //        //    testLcdBlue.testResult.TestResult == false ||
        //        //    testLcdGreen.testResult.TestResult == false ||
        //        //    testLcdRed.testResult.TestResult == false ||
        //        //    testLedBlue.testResult.TestResult == false)
        //        // if (testArea.ResultLcdArea.TestResult == true)
        //        {
        //            testArea.ImageInput.SaveImage(@"C:\IMG\" + Controller.logFolderName + "AreaInput.bmp");
        //            Thread.Sleep(20);
        //            testDust.ImageInput.SaveImage(@"C:\IMG\" + Controller.logFolderName + "DustInput.bmp");
        //            Thread.Sleep(20);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.AddLog("SaveImage() in TestThread exception " + ex.ToString());
        //    }
        //}
        void makeStateDone()
        {
            testArea.InspStatus = InspectionState.Done;
            testDust.InspStatus = InspectionState.Done;
            testLcdWhite.InspStatus = InspectionState.Done;
            testLcdRed.InspStatus = InspectionState.Done;
            testLcdBlue.InspStatus = InspectionState.Done;
            testLcdGreen.InspStatus = InspectionState.Done;
            testKeyTouch.InspStatus = InspectionState.Done;
            testLedBlue.InspStatus = InspectionState.Done;
            testLcdCOPCrack.InspStatus = InspectionState.Done;
            testLcdMCD.InspStatus = InspectionState.Done;
            testLcdReddish.InspStatus = InspectionState.Done;
        }

        void Run(object sender, DoWorkEventArgs e)
        {
            UpdateTestSpec();
            Bitmap bmpImage = null;
            
            while (testType != TestType.TestEnd)
            {
                if (controller.isEMOMode)
                {
                    while (controller.isEMOMode)
                    {
                        Thread.Sleep(10);
                    }
                }

                mainForm.DisplayElapsedTime(controller.testTimer.ElapsedMilliseconds);

                switch (testType)
                {
                    case TestType.TestReady:
                       
                        controller.mainForm.DisplayStatus(Channel, "Test Started");

                        if (Channel == Define.Front)
                        {
                            testBarcode.isFinished = false;
                            testArea.isFinished = false;
                            testDust.isFinished = false;

                            SetTestType(TestType.TestFront);
                        }
                        else
                        {
                            testBarcode.isFinished = false;
                            testOCR.isFinished = false;
                            if (controller.CtrlIo.Y_03 == 1)
                            {
                                controller.IoMoveLightLeft();
                                Thread.Sleep(200);
                            }
                            controller.TurnOffRearLight();

                            SetTestType(TestType.TestRear);
                        }
                        break;

                    #region REAR
                    case TestType.TestRear:
                        Log.AddLog("..........case TestType.TestRear:");

                        if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestLogo)
                        {
                            testLogo1.DoVisionTest();
                            if (Controller.instance.scenarioManger.testSpec.SpecRear.LogoCount == 2)
                                testLogo2.DoVisionTest();
							else
								testLogo2.InspStatus = InspectionState.Done;
								
                        }
                        else
                        {
                            testLogo1.InspStatus = InspectionState.Done; 
                            testLogo2.InspStatus = InspectionState.Done;
                        }

                        if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestLaser)
                        {
                            //testBarcode.DoVisionTest();
                            testBarcode.DoVisonTest_Synch();
                            if ( (((int)controller.eFinalResult) & ((int)InspectionResult.BarcodeResult)) == (int)InspectionResult.BarcodeResult)
                            //if(!testBarcode.testResult.TestResult)        
                            {
                                testOCR.InspStatus = InspectionState.Done;
                                SetTestType(TestType.TestWaitRearFinal);
                                break;
                            }
                            testOCR.DoVisionTest();
                        }
                        else
                        {
                            testBarcode.InspStatus = InspectionState.Done;
                            testOCR.InspStatus = InspectionState.Done; 
                        }
                        SetTestType(TestType.TestWaitRearFinal);
                        break;

                    case TestType.TestWaitRearFinal:
                        if (testLogo1.InspStatus == InspectionState.Done 
                            && testLogo2.InspStatus == InspectionState.Done
                            && testBarcode.InspStatus == InspectionState.Done 
                            && testOCR.InspStatus == InspectionState.Done ) 
                        {
                            controller.FinalResult = controller.FinalResult 
                                && testLogo1.testResult.TestResult 
                                && testBarcode.testResult.TestResult 
                                && testOCR.testResult.TestResult;
                            SetTestType(TestType.TestCompleted);
                        }
                        break;

                    case TestType.TestWaitFrontFinal:
                        if( testArea.InspStatus == InspectionState.Done 
                            && testDust .InspStatus == InspectionState.Done 
                            && testLcdWhite.InspStatus == InspectionState.Done 
                            && testLcdRed.InspStatus == InspectionState.Done 
                            && testLcdBlue.InspStatus == InspectionState.Done 
                            && testLcdGreen.InspStatus == InspectionState.Done 
                            && testKeyTouch.InspStatus == InspectionState.Done 
                            && testLedBlue.InspStatus == InspectionState.Done 
                            && testLcdReddish.InspStatus == InspectionState.Done
                            && testLcdMCD.InspStatus == InspectionState.Done
                            && testLcdCOPCrack.InspStatus == InspectionState.Done)
                        {
                            SetTestType(TestType.TestCompleted);
                        }
                        break;

                    case TestType.TestCompleted:
                        controller.isFinished[Channel] = true;
                        controller.mainForm.DisplayStatus(Channel, "Finished");
                        SetTestType(TestType.TestEnd);
                        break;

                    case TestType.TestEnd:
                        break;

                    case TestType.TestLogo:
                        //controller.TurnOnRearLight(Controller.RearTestType.Label);
                        //Thread.Sleep(200);
                        //controller.mainForm.DisplayStatus(Channel, "Logo Test Started");
                        //bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestLogo);
                        //testLogo1.SetSrcImage(visionTools.GetBmpFromRect(
                        //    bmpImage, testSpec.RectLogo1, 90));
                        //controller.mainForm.DisplayImage(Channel, bmpImage);
                        //bmpImage.Dispose();
                        //GC.Collect();
                        // SetTestType(TestType.TestLabel);
                        break;


                    case TestType.TestLabel:

                        controller.mainForm.DisplayStatus(Channel, "Barcode Test Started");

                        //turn on the light
                        controller.IoMoveLightRight();
                        Thread.Sleep(500);

                        bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestImeiLabel);

                        bmpImage.Save(@"C:\\test.bmp");

                        testBarcode.SetSrcImage(visionTools.GetBmpFromRect(bmpImage, testSpec.RectBarcode, 90));

                        controller.mainForm.DisplayImage(Channel, bmpImage);
                        bmpImage.Dispose();
                        GC.Collect();

                        testBarcode.DoVisionTest();
                        inspectionList.Add(testBarcode);
                        SetTestType(TestType.TestLaser);

                        break;

                    case TestType.TestLaser:
                        if (testBarcode.isFinished == false)
                            break;

                        if (testBarcode.testResult.m_nImei == string.Empty || testSpec.SpecTestList.TestLaser == false)
                        {
                            //turn off the light
                            controller.TurnOffRearLight();
                            controller.IoMoveLightLeft();
                            //controller.isRearGrabbed = true;

                            SetTestType(TestType.TestWaitRearFinal);
                            break;
                        }
                        else
                        {
                            controller.mainForm.DisplayStatus(Channel, "OCR Test Started");
                            controller.TurnOnRearLight(Controller.RearTestType.Laser);
                            controller.IoMoveLightLeft();
                            Thread.Sleep(500);

                            bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestImeiLaser);
                            bmpImage.Save(@".\Detail\Rear\ocr.bmp");

                            //turn off the light
                            controller.TurnOffRearLight();
                            controller.IoMoveLightLeft();
                            //controller.isRearGrabbed = true;

                            testOCR.SetSrcImage(bmpImage.Clone() as Bitmap);
                            controller.mainForm.DisplayImage(Channel, bmpImage);
                            bmpImage.Dispose();
                            GC.Collect();

                            testOCR.DoVisionTest();
                            inspectionList.Add(testOCR);

                            while (true)
                            {
                                if (testOCR.isFinished == true)
                                    break;
                                Thread.Sleep(10);
                            }

                            if (!testOCR.testResult.TestResult)
                            {
                                if (controller.retryCountTest == 1)
                                    controller.InspThreads[0].testType = TestType.TestCompleted;
                            }
                            SetTestType(TestType.TestWaitRearFinal);
                        }
                        break;
                    #endregion

                    #region FRONT
                    case TestType.TestFront:
                        // hunsibi
                        IsAreaDustFinished.WaitOne();
                        Log.AddLog("InspectionThread AreadDustFinised Release");

                        testArea.DoVisonTest_Synch();
                        if(!testArea.ResultLcdArea.TestResult)
                        {
                            makeStateDone();
                            controller.eFinalResult |= InspectionResult.LcdAreaResult;
                            SetTestType(TestType.TestWaitFrontFinal);
                            break;
                        }
                        testDust.DoVisonTest_Synch();
                        
                        IsAreaDustFinished.Reset();

                        if (testDust.ResultLcdArea.TestResult && testDust.ResultDust.TestResult)
                        {
                            testLcdWhite.DoVisionTest();

                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestFrontKey)
                            {
                                testKeyTouch.DoVisionTest();
                            }
                            else
                                testKeyTouch.InspStatus = InspectionState.Done;
                            // hunsibi
                            IsBlueFinished.WaitOne();
                            Log.AddLog("InspectionThread IsBlueFinished Released");

                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestFrontLed)
                            {
                                testLedBlue.DoVisionTest();
                            }
                            else
                                testLedBlue.InspStatus = InspectionState.Done;

                            testLcdBlue.DoVisionTest();
                            IsBlueFinished.Reset(); 

                            // hunsibi
                            IsGreenFinished.WaitOne();
                            Log.AddLog("InspectionThread IsGreenFinished Released");
                            testLcdGreen.DoVisionTest();
                            IsGreenFinished.Reset(); 

                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestRed)
                            {
                                // hunsibi
                                IsRedFinished.WaitOne();
                                Log.AddLog("InspectionThread IsRedFinished Released");
                                testLcdRed.DoVisionTest();
                                IsRedFinished.Reset(); 
                            }
                            else
                                testLcdRed.InspStatus = InspectionState.Done;



                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestMcd)
                            {
                                // hunsibi
                                IsMCDFinished.WaitOne();
                                Log.AddLog("InspectionThread IsMCDFinished Released");
                                testLcdMCD.DoVisionTest();
                                IsMCDFinished.Reset(); 
                            }
                            else
                                testLcdMCD.InspStatus = InspectionState.Done;

                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestCOPCrack)
                            {
                                IsCopCrackFinished.WaitOne();
                                Log.AddLog("InspectionThread IsCopCrackFinished Released");
                                testLcdCOPCrack.DoVisionTest();
                                IsCopCrackFinished.Reset(); 
                            }
                            else
                                testLcdCOPCrack.InspStatus = InspectionState.Done;

                            if (Controller.instance.scenarioManger.testSpec.SpecTestList.TestReddish)
                            {
                                IsReddishFinished.WaitOne();
                                Log.AddLog("InspectionThread IsReddishFinished Released");
                                testLcdReddish.DoVisionTest();
                                IsReddishFinished.Reset();
                            }
                            else
                                testLcdReddish.InspStatus = InspectionState.Done;

                            GC.Collect();
                            SetTestType(TestType.TestWaitFrontFinal);
                        }
                        else
                        {
                            makeStateDone();
                            if (!testDust.ResultLcdArea.TestResult)
                            {
                                controller.eFinalResult |= InspectionResult.LcdAreaResult;
                            }
                            if(!testDust.ResultDust.TestResult)
                            {
                                controller.eFinalResult |= InspectionResult.LcdDustResult;
                            }
                            SetTestType(TestType.TestWaitFrontFinal);
                        }
                        break;

                    case TestType.TestLcdArea:
                        Log.AddLog("Area Test Started");
                        controller.mainForm.DisplayStatus(Channel, "Area Test Started");
                        controller.mainForm.DisplayImage(Channel, testArea.ImageInput.ToBitmap());
                        testArea.tmpDoVisionTest(); // BAE TEST
                        GC.Collect();

                        SetTestType(TestType.TestDust); // SetTestType(TestType.TestLcdWhite);
                        break;

                    case TestType.TestDust:

                        while (true)
                        {
                            if (testArea.isFinished == true && controller.isDustGrabbed == true)
                                break;

                            Thread.Sleep(10);
                        }

                        if (testArea.ResultLcdArea.TestResult == false)
                        {
                            SetTestType(TestType.TestWaitRearFinal);
                            break;
                        }
                        controller.mainForm.DisplayStatus(Channel, "Dust Test Started");
                        controller.mainForm.DisplayImage(Channel, testDust.ImageInput.ToBitmap());
                        testDust.DoVisionTest();
                        inspectionList.Add(testDust);

                        GC.Collect();

                        SetTestType(TestType.TestLcdWhite);
                        break;

                    case TestType.TestLcdWhite:

                        while (true)
                        {
                            if (testDust.isFinished == true) break;
                        }
                        if (testDust.ResultDust.TestResult == false || testDust.ResultLcdArea.TestResult == false)
                        {
                            SetTestType(TestType.TestWaitRearFinal);
                            break;
                        }
                        controller.mainForm.DisplayStatus(Channel, "White Test Started");
                        testLcdWhite.DoVisionTest();
                        inspectionList.Add(testLcdWhite);

                        if (testSpec.SpecTestList.TestFrontKey == true)
                            SetTestType(TestType.TestKeyTouch);
                        else
                            SetTestType(TestType.TestLcdBlue);
                        break;


                    case TestType.TestKeyTouch:
                        mainForm.DisplayStatus(Channel, "Touch Key Test Started");
                        testKeyTouch.DoVisionTest();
                        inspectionList.Add(testKeyTouch);
                        SetTestType(TestType.TestLcdBlue);
                        break;

                    case TestType.TestLcdBlue:

                        while (true)
                        {
                            if (controller.isBlueGrabbed == true)
                                break;
                            Thread.Sleep(10);
                        }
                        mainForm.DisplayStatus(Channel, "Lcd Blue Test Started");
                        controller.mainForm.DisplayImage(Channel, testLcdBlue.ImageInput.ToBitmap());
                        testLcdBlue.DoVisionTest();
                        inspectionList.Add(testLcdBlue);

                        if (testSpec.SpecTestList.TestFrontLed == true)
                            SetTestType(TestType.TestLedBlue);
                        else
                            SetTestType(TestType.TestLcdGreen);
                        break;

                    case TestType.TestLedBlue:
                        mainForm.DisplayStatus(Channel, "Led Blue Test Started");
                        testLedBlue.DoVisionTest();
                        inspectionList.Add(testLedBlue);
                        SetTestType(TestType.TestLcdGreen);
                        break;

                    case TestType.TestLcdGreen:

                        while (true)
                        {
                            if (controller.isGreenGrabbed == true)
                                break;
                            Thread.Sleep(10);
                        }

                        mainForm.DisplayStatus(Channel, "Lcd Green Test Started");
                        testLcdGreen.DoVisionTest();
                        controller.mainForm.DisplayImage(Channel, testLcdGreen.ImageInput.ToBitmap());
                        inspectionList.Add(testLcdGreen);
                        if (controller.scenarioManger.testSpec.SpecTestList.TestRed)
                            SetTestType(TestType.TestLcdRed);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestMcd)
                            SetTestType(TestType.TestLcdMCD);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestCOPCrack)
                            SetTestType(TestType.TestLcdCOPCrack);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestReddish)
                            SetTestType(TestType.TestLcdReddish);
                        else
                            SetTestType(TestType.TestWaitRearFinal);
                        break;


                    case TestType.TestLcdRed:

                        while (true)
                        {
                            if (controller.isRedGrabbed == true)
                                break;
                        }

                        mainForm.DisplayStatus(Channel, "Lcd Red Test Started");
                        testLcdRed.DoVisionTest();
                        controller.mainForm.DisplayImage(Channel, testLcdRed.ImageInput.ToBitmap());
                        inspectionList.Add(testLcdRed);

                        if (controller.scenarioManger.testSpec.SpecTestList.TestMcd)
                            SetTestType(TestType.TestLcdMCD);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestCOPCrack)
                            SetTestType(TestType.TestLcdCOPCrack);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestReddish)
                            SetTestType(TestType.TestLcdReddish);
                        else
                            SetTestType(TestType.TestWaitRearFinal);
                        
                        break;

                    case TestType.TestLcdMCD:
                        while (true)
                        {
                            if (controller.isMCDGrabbed == true)
                                break;
                            Thread.Sleep(10);
                        }

                        mainForm.DisplayStatus(Channel, "Lcd MCD Test Started");
                        testLcdMCD.DoVisionTest();
                        controller.mainForm.DisplayImage(Channel, testLcdMCD.ImageInput.ToBitmap());
                        inspectionList.Add(testLcdMCD);
                        GC.Collect();

                        if (controller.scenarioManger.testSpec.SpecTestList.TestCOPCrack)
                            SetTestType(TestType.TestLcdCOPCrack);
                        else if (controller.scenarioManger.testSpec.SpecTestList.TestReddish)
                            SetTestType(TestType.TestLcdReddish);
                        else
                            SetTestType(TestType.TestWaitRearFinal);

                        break;

                    case TestType.TestLcdCOPCrack:

                        while (true)
                        {
                            if (controller.isCOPCrackGrabbed == true)
                                break;
                            Thread.Sleep(10);
                        }

                        mainForm.DisplayStatus(Channel, "Lcd COP Crack Test Started");
                        testLcdCOPCrack.DoVisionTest();
                        controller.mainForm.DisplayImage(Channel, testLcdCOPCrack.ImageInput.ToBitmap());
                        inspectionList.Add(testLcdCOPCrack);
                        GC.Collect();
                        
                        if (controller.scenarioManger.testSpec.SpecTestList.TestReddish)
                            SetTestType(TestType.TestLcdReddish);
                        else
                            SetTestType(TestType.TestWaitRearFinal);


                        break;
                        #endregion


                }
            }
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", "Thread Main", controller.testTimer.ElapsedMilliseconds));
        }

        public bool GrabImage()
        {
            retryCountGrapImage = 1;

            // hunsibi
            // front thread 시작하고 Grab하는 즉시 각 모드별 검사 thread 실행
            StartTest();

            while (retryCountGrapImage >= 0)
            {
                try
                {
                    TestSpec testSpec = controller.scenarioManger.testSpec;

                    //init 
                    controller.isAreaGrabbed = false;
                    controller.isDustGrabbed = false;
                    controller.isBlueGrabbed = false;
                    controller.isGreenGrabbed = false;
                    controller.isRedGrabbed = false;
                    controller.isReddishGrabbed = false;
                    controller.isCOPCrackGrabbed = false;
                    controller.isMCDGrabbed = false;

                    Bitmap bmpImage = null;
                    testArea.ImageInput = null;
                    testLcdWhite.ImageInput = null;
                    testKeyTouch.ImageInput = null;
                    testLcdGreen.listInputImages.Clear();

                    GC.Collect();

                    controller.TurnOffFrontLight();
                    Log.AddLog("!!!!!!!!!!!!!!!Front Image grab start!!!!!!!!!!!!!!!");
                    // preprocessing과 동기화
                    Log.AddLog("InspectionThread IsPreProcessFisnished WaitOne");
                    controller.IsPreProcessFisnished.WaitOne();

                    Log.AddLog("InspectionThread IsPreProcessFisnished Release");
                    bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestArea);
                    mainForm.DisplayImage(Define.Front, bmpImage);
                    Log.AddLog("!!!!!!!!!!!!!!!LCD WHITE Grab!!!!!!!!!!!!!!!");
                    controller.IsPreProcessFisnished.Reset();
                    Log.AddLog("InspectionThread IsPreProcessFisnished Reset");

                    testArea.SetSrcImage(bmpImage.Clone() as Bitmap);
                    testLcdWhite.SetSrcImage(bmpImage.Clone() as Bitmap);
                    testKeyTouch.SetSrcImage(bmpImage.Clone() as Bitmap); // 미리 받아 놓는다. 
                    bmpImage.Dispose();

                    controller.TurnOnFrontLight();

                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,0\r");
                    Thread.Sleep(10);
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,4\r");
                    Thread.Sleep(400);

                    bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestDust);
                    testDust.SetSrcImage(bmpImage.Clone() as Bitmap);
                    Log.AddLog("!!!!!!!!!!!!!!!LCD BLACK Grab!!!!!!!!!!!!!!!");
                    // hunsibi
                    IsAreaDustFinished.Set();
                    Log.AddLog("InspectionThread  IsAreaDustFinished set");

                    bmpImage.Dispose();

                    controller.TurnOffFrontLight();

                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,3\r"); //  Name="SCRON"/>	9
                    Thread.Sleep(50);
                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,2\r"); //  Name="BLUE"/>	3
                    Thread.Sleep(50);
                    controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,4\r"); //  Name="LEDBLUE"/>		17
                    Thread.Sleep(800);

                    if ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdDustResult)) == (int)InspectionResult.LcdDustResult
                                    || ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdAreaResult)) == (int)InspectionResult.LcdAreaResult))
                    {
                        Log.AddLog("!!!!!!!!!!!!!!!Front Image grab Finished!!!!!!!!!!!!!!!");
                        return true;
                    }

                    bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestBlue);
                    testLcdBlue.SetSrcImage(bmpImage.Clone() as Bitmap);
                    testLedBlue.SetSrcImage(bmpImage.Clone() as Bitmap);
                    Log.AddLog("!!!!!!!!!!!!!!!LCD BLUE Grab!!!!!!!!!!!!!!!");
                    // hunsibi
                    IsBlueFinished.Set();
                    Log.AddLog("InspectionThread  IsBlueFinished set");

                    bmpImage.Dispose();

                    controller.CtrlAnyJig.Write("AT+DISPTEST=0,1\r"); //  Name="GREEN"/>	2
                                                                      // Thread.Sleep(10);
                                                                      // controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,3\r"); //  Name="LEDGREEN"/>

                    Thread.Sleep(700);
                    if ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdDustResult)) == (int)InspectionResult.LcdDustResult
                || ((((int)controller.eFinalResult) & ((int)InspectionResult.LcdAreaResult)) == (int)InspectionResult.LcdAreaResult))
                    {
                        Log.AddLog("!!!!!!!!!!!!!!!Front Image grab Finished!!!!!!!!!!!!!!!");
                        return true;
                    }


                    //170510 누적 테스트

                    //if (Config.GreenaAcumulateTest)
                    {
                        for (int i = 0; i < Config.GreenaAcumulateCount; i++)
                        {
                            bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestGreen + Config.GreenExpOffset);
                            testLcdGreen.listInputImages.Add(bmpImage.ToMat());
                            bmpImage.Dispose();
                        }
                    }
                    //else
                    //{
                    //    //기존
                    //    bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestGreen + Config.GreenExpOffset);
                    //    testLcdGreen.SetSrcImage(bmpImage.Clone() as Bitmap);
                    //    controller.isGreenGrabbed = true;
                    //    bmpImage.Dispose();
                    //}
                    controller.isGreenGrabbed = true;
                    //bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestGreen + Config.GreenExpOffset);
                    //testLcdGreen.SetSrcImage(bmpImage.Clone() as Bitmap);
                    Log.AddLog("!!!!!!!!!!!!!!!LCD GREEN Grab!!!!!!!!!!!!!!!");
                    // hunsibi
                    IsGreenFinished.Set();

                    bmpImage.Dispose();

                    if (controller.scenarioManger.testSpec.SpecTestList.TestRed)
                    {
                        //controller.CtrlAnyJig.Write("AT+LEDLAMPT=0,2\r"); //  Name="LEDRED"/>			15
                        //Thread.Sleep(10);

                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,0\r"); // Name="RED"/>		1
                        Thread.Sleep(700);

                        bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestRed);
                        testLcdRed.SetSrcImage(bmpImage.Clone() as Bitmap);
                        Log.AddLog("!!!!!!!!!!!!!!!LCD RED Grab!!!!!!!!!!!!!!!");
                        // hunsibi
                        IsRedFinished.Set();
                        Log.AddLog("InspectionThread  IsRedFinished set");

                        bmpImage.Dispose();
                    }

                    if (controller.scenarioManger.testSpec.SpecTestList.TestMcd)
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=3,5\r"); // Name="MCD"/>		1
                        Thread.Sleep(850);

                        bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestMCD);
                        testLcdMCD.SetSrcImage(bmpImage.Clone() as Bitmap);

                        // hunsibi
                        IsMCDFinished.Set();
                        Log.AddLog("InspectionThread  IsMCDFinished set");

                        bmpImage.Dispose();
                    }

                    if (controller.scenarioManger.testSpec.SpecTestList.TestCOPCrack)
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=4,0\r"); // Name="COP Crack"/>		1
                        Thread.Sleep(800);

                        bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestCOPCrack);
                        testLcdCOPCrack.SetSrcImage(bmpImage.Clone() as Bitmap);
                        // hunsibi
                        IsCopCrackFinished.Set();
                        Log.AddLog("InspectionThread  IsCopCrackFinished set");

                        bmpImage.Dispose();
                    }

                    if (controller.scenarioManger.testSpec.SpecTestList.TestReddish)
                    {
                        controller.CtrlAnyJig.Write("AT+DISPTEST=0,6\r");   //  Name="WHITE"/>	5
                        Thread.Sleep(700);

                        bmpImage = controller.GetTestImage(Channel, testSpec.SpecExposure.TestReddish);
                        testLcdReddish.SetSrcImage(bmpImage.Clone() as Bitmap);
                        //hunsibi
                        IsReddishFinished.Set();
                        Log.AddLog("InspectionThread  IsReddishFinished set");

                        bmpImage.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write("GrabImage() in TestThread exception " + ex.ToString());
                    Log.AddLog("GrabImage() in TestThread exception " + ex.ToString());
                    Log.AddPmLog("GrabImage() in TestThread exception " + ex.ToString());
                    --retryCountGrapImage;
                    controller.IsPreProcessFisnished.Set();
                    Log.AddLog("InspectionThread  IsPreProcessFisnished set");
                    continue;
                }
                Log.AddLog("!!!!!!!!!!!!!!!Front Image grab Finished!!!!!!!!!!!!!!!");
                return true;
            }
            controller.IsPreProcessFisnished.Reset();
            Log.AddLog("InspectionThread  IsPreProcessFisnished Reset");
            return false;
        }
    }
}
