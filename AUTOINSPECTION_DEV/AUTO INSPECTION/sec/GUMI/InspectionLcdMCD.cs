using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AutoInspection;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionLcdMCD : Inspection
    {
        public MData_LCD_MCD testSpec;
        public MResult_LCD_MCD testResult = new MResult_LCD_MCD();
        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat();
        }
        public override void ResetResult()
        {
            testResult.Clear();
        }
        public InspectionLcdMCD(string _testName, MData_LCD_MCD _testSpec, VisionTest _visionTest,
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
            InspStatus = InspectionState.Done;
            ImageInput.Dispose();
            GC.Collect();
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;
            testResult.TestResult = true;

            testTimer.Reset();
            testTimer.Start();

            visionTest.LcdMCD(ImageInput, testSpec, ref testResult);

            if (testResult.BrightLine_McdJudgeSizeLL > testSpec.BrightLine_McdJudgeSizeLL)
            // if( testResult.TestResult == false ) 
            {
                // testResult.TestResult = false;
                PmLogger.SetCsvFailItem(testName);

                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
                controller.ResultFront = false;
            }

            if (Config.SaveResultImage == true)
            {
                Controller.SaveResultImage("LcdMCDSrc", ImageInput.ToBitmap());
                if (testResult.TestResult)
                    Controller.SaveResultImage("LcdMCDResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("LcdMCDFail", testResult.ImageResult.ToBitmap());
            }

            testTimer.Stop();
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));

            mainFrom.DisplayTestResult("LCD MCD", testResult.BrightLine_McdJudgeSizeLL.ToString("#,#.###")
                , testSpec.BrightLine_McdJudgeSizeLL.ToString("#,#.###"), "0", testResult.TestResult);
            Log.AddInfo("LCD MCD", testResult.BrightLine_McdJudgeSizeLL.ToString()
                , testSpec.BrightLine_McdJudgeSizeLL.ToString(), "0", testResult.TestResult, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue( PmLogger.TestItem.LcdMcd, testResult.BrightLine_McdJudgeSizeLL.ToString());
        }
    }
}
