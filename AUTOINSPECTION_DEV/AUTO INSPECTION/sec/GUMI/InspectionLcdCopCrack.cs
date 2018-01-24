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
    public class InspectionLcdCopCrack : Inspection
    {
        public MData_LCD_COPCRACK testSpec;
        public MResult_LCD_COPCRACK testResult = new MResult_LCD_COPCRACK();

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat(); 
        }

        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLcdCopCrack(string _testName, MData_LCD_COPCRACK _testSpec, VisionTest _visionTest,
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

            visionTest.LcdCopCrack(ImageInput, testSpec, ref testResult);
            
            if (testResult.m_nBlackDot_JudgeSize > testSpec.BlackDot_BJudgeSizeUL)
            {
                PmLogger.SetCsvFailItem(testName);

                testResult.TestResult = false;
                controller.ResultFront = false;
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
            }

            if (Config.SaveResultImage)
            {
                Controller.SaveResultImage("LcdCopcrackSrc", ImageInput.ToBitmap());

                if (testResult.TestResult)
                    Controller.SaveResultImage("LcdCopcrackResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("LcdCopcrackFail", testResult.ImageResult.ToBitmap());
            }

            testTimer.Stop();
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));

            controller.mainForm.DisplayTestResult("LCD COPCRACK", testResult.m_nBlackDot_JudgeSize.ToString("#,#.###")
                , testSpec.BlackDot_BJudgeSizeUL.ToString("#,#.###"), "0", testResult.TestResult);

            Log.AddInfo("LCD COPCRACK", testResult.m_nBlackDot_JudgeSize.ToString()
                , testSpec.BlackDot_BJudgeSizeUL.ToString(), "0", testResult.TestResult, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue( PmLogger.TestItem.LcdCopCrack, testResult.m_nBlackDot_JudgeSize.ToString());
        }
    }
}
