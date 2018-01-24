using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;
using AutoInspection_GUMI;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionLcdWhite : Inspection 
    {
        public MData_LCD_WHITE testSpec;
        public MResult_LCD_WHITE testResult = new MResult_LCD_WHITE();

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat();
        }


        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionLcdWhite(string _testName, MData_LCD_WHITE _testSpec,
            VisionTest _visionTest, string _logFolder, FrmMainCh2 _mainForm, Controller _controller )
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

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();
            testResult.TestResult = true;
            visionTest.LcdWhite( ImageInput, testSpec, ref testResult);

            if (!testResult.TestResult)
            {
                PmLogger.SetCsvFailItem(testName); 
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
                controller.ResultFront = false;
            }
            

            if (Config.SaveResultImage)
            {
                //Controller.SaveResultImage("11.LcdWhiteSrc", ImageInput.ToBitmap());

                if ( testResult.TestResult )
                    Controller.SaveResultImage("12.LcdWhiteResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("12.LcdWhiteFailed", testResult.ImageResult.ToBitmap());
            }

            int mungCnt = testResult.m_nMungNgCnt;
            mainFrom.DisplayTestResult( "LCD WHITE", mungCnt.ToString(), "0", "0", testResult.TestResult);


            testTimer.Stop();

            Log.AddInfo("LCD WHITE", mungCnt.ToString(), "0", "0", testResult.TestResult, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue( PmLogger.TestItem.LcdWhite, mungCnt.ToString());

            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
        }

        void testThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!testResult.TestResult)
            {
                controller.eFinalResult |= InspectionResult.LcdWhiteResult;
            }

            InspStatus = InspectionState.Done;
            ImageInput.Dispose();
            GC.Collect();
            return;
        }
    }
}
