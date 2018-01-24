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
    public class InspectionLcdRed : Inspection 
    {
        public MData_LCD_RED testSpec;
        public MResult_LCD_RED testResult = new MResult_LCD_RED();
        
        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat();
        }

        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLcdRed(string _testName, MData_LCD_RED _testSpec, VisionTest _visionTest, 
            string _logFolder, FrmMainCh2 _mainForm, Controller _controller )
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
                controller.eFinalResult |= InspectionResult.LcdRedResult;
            }

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

            visionTest.LcdRed(ImageInput, testSpec, ref testResult);

            if (testResult.m_nBlackDot_JudgeSize > testSpec.BlackDot_RJudgeSizeUL)
            // if( testResult.TestResult == false ) 
            {
                PmLogger.SetCsvFailItem(testName);  // 
                testResult.TestResult = false;

                controller.ResultFront = false;
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
            }

            if (Config.SaveResultImage )
            {
                Controller.SaveResultImage("09.LcdRedSrc", ImageInput.ToBitmap());
                if( testResult.TestResult )
                    Controller.SaveResultImage("10.LcdRedResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("10.LcdRedFailed", testResult.ImageResult.ToBitmap());
            }

            testTimer.Stop();

            //mainFrom.DisplayTestResult( "LCD RED", testResult.m_nBlackDot_JudgeSize.ToString("#,#")
            //    , testSpec.BlackDot_RJudgeSizeUL.ToString("#,#.###"), "-", testResult.TestResult);

            //Log.AddInfo("LCD RED", testResult.m_nBlackDot_JudgeSize.ToString()
            //    , testSpec.BlackDot_RJudgeSizeUL.ToString(), "-", testResult.TestResult, testTimer.Elapsed.Seconds);
            mainFrom.DisplayTestResult("LCD RED", testResult.m_nBlackDot_JudgeSize.ToString()
                ,  "0", testSpec.BlackDot_RJudgeSizeUL.ToString("#,#.###"), testResult.TestResult);

            Log.AddInfo("LCD RED", testResult.m_nBlackDot_JudgeSize.ToString()
                ,  "0", testSpec.BlackDot_RJudgeSizeUL.ToString(), testResult.TestResult, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue( PmLogger.TestItem.LcdRed, testResult.m_nBlackDot_JudgeSize.ToString());

            Log.AddLog(string.Format("[HHP] {0}, Elapsed Time : {1}", testName, testTimer.ElapsedMilliseconds));
        }

    }
}
