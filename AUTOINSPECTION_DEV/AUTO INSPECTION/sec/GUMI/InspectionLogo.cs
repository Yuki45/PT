using AutoInspection.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AutoInspection;
using System;
using AutoInspection.sec.GUMI;
using OpenCvSharp.Extensions;

namespace AutoInspection_GUMI
{
    public class InspectionLogo : Inspection
    {
        public MData_REAR testSpec;
        public MResult_REAR testResult = new MResult_REAR();
        //Bitmap BImageInput;
        int idx;

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat();
        }

        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLogo(string _testName, MData_REAR _testSpec, VisionTest _visionTest,
            string _logFolder, int _idx, Controller _controller)
        {
            testName = _testName;
            testSpec = _testSpec;
            visionTest = _visionTest;
            LogFolder = _logFolder;

            idx = _idx;
            controller = _controller;
            testThread = new BackgroundWorker();
            testThread.DoWork += new DoWorkEventHandler(TestIamge);
            testThread.RunWorkerCompleted += testThread_RunWorkerCompleted;

            testTimer = new Stopwatch();
        }

        void testThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!testResult.TestResult)
            {
                Controller.instance.eFinalResult |= InspectionResult.LogoResult;
            }
            InspStatus = InspectionState.Done;
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            Bitmap imgLogoTmp = null;
            try
            {
                //imgLogoTmp = new Bitmap(@".\Model\" + Controller.instance.mainForm.GetModelName() + "_rect" + idx + ".bmp");
                imgLogoTmp = new Bitmap(new Bitmap(Config.sPathModel + string.Format("{0}_rect{1}.bmp", Controller.instance.mainForm.GetModelName(), idx)));
            }
            catch (Exception ex)
            {
                Log.AddLog(ex.ToString());
                Log.AddPmLog(ex.ToString());
                MessageBox.Show("Please Save Model File");
                return;
            }
            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();

            testResult.TestResult = true;

            int matchingRate = visionTest.CalcLogoMatchingRate(ImageInput.ToBitmap(), controller.scenarioManger.testSpec, idx, imgLogoTmp); // , ref testResult);

            if (matchingRate < testSpec.LogoMatchingRate)
            {
                // todo : PmLogger fail 사용할 것. 
                testResult.TestResult = false;

                //string _s = LogFolder + Controller.logFolderName + "Logo_" + idx + ".bmp";
                //ImageInput.SaveImage(_s);
                //controller.mainForm.DisplayErrorMsg("Logo_" + idx + "Err", _s);
            }

                if (Config.SaveResultImage == true&&idx==1)
                {
                    Controller.SaveResultImage("14.LogoSrc", ImageInput.ToBitmap());

                    //if (testResult.TestResult)
                    //    Controller.SaveResultImage("15.LogoResult", testResult.ImageResult.ToBitmap());
                    //else
                    //    Controller.SaveResultImage("15.FailedLogo", testResult.ImageResult.ToBitmap());
                }
            //if (!controller.scenarioManger.testSpec.SpecTestList.TestFront)
            //{
            //    controller.mainForm.DisplayTestResult("Logo" + idx, matchingRate.ToString("#,#.###")
            //  , testSpec.LogoMatchingRate.ToString("#,#.###"), "100", testResult.TestResult);
            //}
            testTimer.Stop();
            controller.mainForm.DisplayTestResult("Logo" + idx, matchingRate.ToString("#,#.###")
              , testSpec.LogoMatchingRate.ToString("#,#.###"), "100", testResult.TestResult);
            Log.AddLog(string.Format("Logo Test : Measured {0}, Spec {1} ~ 100", matchingRate, testSpec.LogoMatchingRate));
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
            Log.AddInfo("Logo" + idx, matchingRate.ToString(), testSpec.LogoMatchingRate.ToString(), "100", testResult.TestResult, testTimer.Elapsed.Seconds);

            if (idx == 1)
                PmLogger.SetCsvValue(PmLogger.TestItem.Logo1, matchingRate.ToString());
            else
                PmLogger.SetCsvValue(PmLogger.TestItem.Logo2, matchingRate.ToString());
            // todo : Implement That : PmLogger.SetCsvSetMenuArea(ResultLcdArea.m_iMenuArea.ToString("#,#"));

            //if (idx == 1)
            //{
            //    int matchingRate = visionTest.CalcLogoMatchingRate(BImageInput, imgLogoTmp, ref testResult);
            //    testTimer.Stop();
            //    if (matchingRate < testSpec.Logo1MatchingRate)
            //    {
            //        _result = false;
            //        string _s = LogFolder + Controller.logFolderName + "Logo_" + idx + ".bmp";
            //        BImageInput.Save(_s);
            //        inspectionThread.controller.mainForm.DisplayErrorMsg("Logo_" + idx + "Err", _s,"1");
            //    }

            //    inspectionThread.controller.mainForm.DisplayTestResult("Logo" + idx, matchingRate.ToString()
            //   , testSpec.Logo1MatchingRate.ToString(), "100", _result);

            //    Log.AddInfo("Logo" + idx, matchingRate.ToString()
            //    , testSpec.Logo1MatchingRate.ToString(), "100", _result);

            //    double time = testTimer.ElapsedMilliseconds / (double)1000;
            //    inspectionThread.controller._LOGS.WriteBodyInfo("Logo" + idx,  // Test item
            //          matchingRate.ToString(),                            // Value
            //        testSpec.Logo1MatchingRate.ToString(),                                              // LSL
            //        "100",                                              // USL
            //        testResult.TestResult,                                        // Result : P/F
            //        time.ToString("#0.0"),                          // Time
            //        "",                                             // Code value
            //        "",                                             // Code LSL
            //        "");

            //}
            //else 
            //{
            //    int matchingRate = visionTest.CalcLogoMatchingRate(BImageInput, imgLogoTmp, ref testResult);
            //    testTimer.Stop();
            //    if (matchingRate < testSpec.Logo2MatchingRate)
            //    {
            //        _result = false;
            //        string _s = LogFolder + Controller.logFolderName + "Logo_" + idx + ".bmp";
            //        BImageInput.Save(_s);
            //        UserInterface.DisplayErrorMsg("Logo_" + idx + "Err", _s,"1");
            //    }

            //    UserInterface.DisplayTestResult("Logo" + idx, matchingRate.ToString()
            //   , testSpec.Logo2MatchingRate.ToString(), "100", _result);

            //    Log.AddInfo("Logo" + idx, matchingRate.ToString()
            //    , testSpec.Logo2MatchingRate.ToString(), "100", _result);

            //    double time = testTimer.ElapsedMilliseconds / (double)1000;
            //    controller._LOGS.WriteBodyInfo("Logo" + idx,  // Test item
            //          matchingRate.ToString(),                            // Value
            //        testSpec.Logo2MatchingRate.ToString(),                                              // LSL
            //        "100",                                              // USL
            //        testResult.TestResult,                                        // Result : P/F
            //        time.ToString("#0.0"),                          // Time
            //        "",                                             // Code value
            //        "",                                             // Code LSL
            //        "");

            //}



        }
    }
}
