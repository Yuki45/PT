using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionLedBlue : Inspection
    {
        public MData_LED_BLUE testSpec;
        public MResult_LED_BLUE testResult = new MResult_LED_BLUE();

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat(); // BitmapConverter.ToMat(bmpImage); // .Clone();
        }
        
        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLedBlue(string _testName, MData_LED_BLUE _testSpec, VisionTest _visionTest, 
            string _logFolder, FrmMainCh2 _mainForm, Controller _controller)
        {
            testName = _testName;
            testSpec = _testSpec;
            visionTest = _visionTest;
            LogFolder = _logFolder;
            mainFrom = _mainForm;
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
                controller.eFinalResult |= InspectionResult.LedBlueResult;
            }

            InspStatus = InspectionState.Done;
            ImageInput.Dispose();
            GC.Collect();
        }

        bool _result = false;
        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();

            visionTest.LedBlue(ImageInput, testSpec, ref testResult);

            testTimer.Stop();
            _result = true;
            testResult.TestResult = true;

            if (testResult.m_dblueLedBrightnessVal < testSpec.Brightness_BLJudgeBrightLL
                || testResult.m_dblueLedBrightnessVal > testSpec.Brightness_BLJudgeBrightUL)
            {
                //PmLogger.SetCsvFailItem(testName);

                testResult.TestResult = false;
                _result = false;
                //controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });

                // string _s = LogFolder + Controller.logFolderName + "LedBrightFail.jpg";
                // mainFrom.DisplayErrorMsg("LedBright", _s);
                // testResult.imgNGBright.SaveImage(_s);
            }

            //if (Config.SaveResultImage)
            //{
            //    //Controller.SaveResultImage("13.LedBlueSrc", ImageInput.ToBitmap());
            //    if (_result)
            //        Controller.SaveResultImage("14.LedBrightResult", testResult.BrightResult.ToBitmap());
            //    else
            //        Controller.SaveResultImage("14.LedBrightFailed", testResult.BrightResult.ToBitmap());
            //}
           
            mainFrom.DisplayTestResult("SVC LED BRIGHT", testResult.m_dblueLedBrightnessVal.ToString("#.#")
               , testSpec.Brightness_BLJudgeBrightLL.ToString(), testSpec.Brightness_BLJudgeBrightUL.ToString(), _result);
            Log.AddInfo("SVC LED BRIGHT", testResult.m_dblueLedBrightnessVal.ToString("0.00")
                , testSpec.Brightness_BLJudgeBrightLL.ToString(), testSpec.Brightness_BLJudgeBrightUL.ToString(), _result, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue(PmLogger.TestItem.LedBright, testResult.m_dblueLedBrightnessVal.ToString());

            _result = true;
            if (testResult.m_nDiffWH_BLDiffJudgeArea > testSpec.DiffWH_BLJudgeSizeUL)
            {
                //PmLogger.SetCsvFailItem(testName);

                testResult.TestResult = false;
                _result = false;

                //string _s = LogFolder + Controller.logFolderName + "LedDiff.bmp";
                //mainFrom.DisplayErrorMsg("LedDiff", _s);
                //controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
            }

            if (Config.SaveResultImage)
            {
                if (testResult.TestResult)
                    Controller.SaveResultImage("13.LedBlueResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("13.LedBlueFail", testResult.ImageResult.ToBitmap());
            }

            //mainFrom.DisplayTestResult("SVC LED DIFF", testResult.m_nDiffWH_BLDiffJudgeArea.ToString("#,#.###")
            //  , "-", testSpec.DiffWH_BLJudgeSizeUL.ToString("#,#.###"), _result);
            mainFrom.DisplayTestResult("SVC LED DIFF", testResult.m_nDiffWH_BLDiffJudgeArea.ToString()
              , "0", testSpec.DiffWH_BLJudgeSizeUL.ToString("#,#.###"), _result);
            Log.AddInfo("SVC LED DIFF", testResult.m_nDiffWH_BLDiffJudgeArea.ToString()
                , "0", testSpec.DiffWH_BLJudgeSizeUL.ToString(), _result, testTimer.Elapsed.Seconds);

            PmLogger.SetCsvValue(PmLogger.TestItem.LedDust, testResult.m_nDiffWH_BLDiffJudgeArea.ToString());

            if (testResult.TestResult == false)
            {
                PmLogger.SetCsvFailItem(testName);
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
                controller.ResultFront = false;
            }

            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
        }
    }
}
