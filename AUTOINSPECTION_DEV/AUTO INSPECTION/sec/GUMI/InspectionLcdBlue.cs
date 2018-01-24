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
    public class InspectionLcdBlue : Inspection 
    {
        public MData_LCD_BLUE testSpec;
        public MResult_LCD_BLUE testResult = new MResult_LCD_BLUE();

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat(); //.Clone();
        }
        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLcdBlue(string _testName, MData_LCD_BLUE _testSpec, VisionTest _visionTest,
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
                controller.eFinalResult |= InspectionResult.LcdBlueResult;
            }

            InspStatus = InspectionState.Done;
        }
        
        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;
            testResult.TestResult = true;

            testTimer.Reset();
            testTimer.Start();

            visionTest.LcdBlue(ImageInput, testSpec, ref testResult);
            
            if (testResult.m_nBlackDot_JudgeSize > testSpec.BlackDot_BJudgeSizeUL)
            {
                PmLogger.SetCsvFailItem(testName);

                testResult.TestResult = false;
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
                controller.ResultFront = false;
            }

            if (Config.SaveResultImage == true)
            {
                Controller.SaveResultImage("05.LcdBlueSrc", ImageInput.ToBitmap());

                if (testResult.TestResult)
                    Controller.SaveResultImage("06.LcdBlueResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("06.LcdBlueFail", testResult.ImageResult.ToBitmap());
            }

            testTimer.Stop();
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));

            //mainFrom.DisplayTestResult( "LCD BLUE", testResult.m_nBlackDot_JudgeSize.ToString("#,#")
            //    , testSpec.BlackDot_BJudgeSizeUL.ToString(), "-", testResult.TestResult);

            //Log.AddInfo("LCD BLUE", testResult.m_nBlackDot_JudgeSize.ToString()
            //    , testSpec.BlackDot_BJudgeSizeUL.ToString(), "-", testResult.TestResult, testTimer.Elapsed.Seconds);
            mainFrom.DisplayTestResult("LCD BLUE", testResult.m_nBlackDot_JudgeSize.ToString()
                ,  "0", testSpec.BlackDot_BJudgeSizeUL.ToString(), testResult.TestResult);

            Log.AddInfo("LCD BLUE", testResult.m_nBlackDot_JudgeSize.ToString()
                ,  "0", testSpec.BlackDot_BJudgeSizeUL.ToString(), testResult.TestResult, testTimer.Elapsed.Seconds);

            PmLogger.SetCsvValue( PmLogger.TestItem.LcdBlue, testResult.m_nBlackDot_JudgeSize.ToString());
        }
    }
}
