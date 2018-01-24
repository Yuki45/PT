using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AutoInspection;

namespace AutoInspection_GUMI
{
    public class InspectionLcdReddish : Inspection
    {
        public MData_LCD_REDDISH testSpec;
        public MResult_LCD_REDDISH testResult = new MResult_LCD_REDDISH();
        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat();
        }
        public override void ResetResult()
        {
            testResult.Clear();
        }
        public InspectionLcdReddish(string _testName, MData_LCD_REDDISH _testSpec, VisionTest _visionTest,
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

            visionTest.LcdReddish(ImageInput, testSpec, ref testResult);

            //if (testResult.BrightLine_McdJudgeSizeLL > testSpec.BrightLine_McdJudgeSizeLL)
            //{
            //    testResult.TestResult = false;

            //    string _s = LogFolder + Controller.logFolderName + "LcdMCDFail.bmp";
            //    testResult.imgNGResult.SaveImage(_s);
            //    controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.imgNGResult.ToBitmap(), index = 0 });
            //     mainFrom.DisplayErrorMsg("LcdMCDFail", _s);
            //    controller.ResultFront = false;
            //}
            //else if (Config.SaveResultImage == true)
            //{
            //    string _s = Config.ImageLogFolder + Controller.logFolderName + "LcdWhiteResult.bmp";
            //    testResult.imgOKResult.SaveImage(_s);
            //}

            //    testTimer.Stop();
            //Log.AddLog(string.Format("[HHP] {0}, Elapsed Time : {1}", testName, testTimer.ElapsedMilliseconds));

            //mainFrom.DisplayTestResult("LCD MCD", testResult.BrightLine_McdJudgeSizeLL.ToString()
            //    , testSpec.BrightLine_McdJudgeSizeLL.ToString(), "-", testResult.TestResult);

        }
    }
}
