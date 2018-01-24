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
    public class InspectionLcdArea : Inspection 
    {
        public MData_LCD_AREA specLcdArea;

        public MResult_LCD_AREA ResultLcdArea = new MResult_LCD_AREA();
        public bool isFinished = false;
        

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat();
        }
        
        public override void ResetResult()
        {
            ResultLcdArea.Clear();
            isFinished = false;
        }

        public void tmpDoVisionTest()
        {
            ResetResult();
            testThread.RunWorkerAsync();
        }

        public void DoVisonTest_Synch()
        {
            ResetResult();
            TestIamge(null, null);
            InspStatus = InspectionState.Done;
        }


        public InspectionLcdArea(string _testName
            , VisionTest _visionTest, string _logFolder,  Controller _controller )
        {
            testName = _testName;
            controller = _controller;
            visionTest = _visionTest;
            LogFolder = _logFolder;

            testThread = new BackgroundWorker();
            testThread.DoWork += new DoWorkEventHandler(TestIamge);
            testThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(TestCompleted);
            
            testTimer = new Stopwatch();
        }

        public void TestCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!ResultLcdArea.TestResult)
            {
                controller.eFinalResult |= InspectionResult.LcdAreaResult;
            }

            InspStatus = InspectionState.Done;
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            Log.AddLog("image test start");
            InspStatus = InspectionState.Running;
          

            testTimer.Reset();
            testTimer.Start();
            ResultLcdArea.TestResult = true;

            bool _result = visionTest.AreaProcess(ImageInput, 
                controller.scenarioManger.testSpec,
                ref ResultLcdArea);

            if (!_result)
            {
                PmLogger.SetCsvFailItem(testName);

                ResultLcdArea.TestResult = false;
                FrmMainCh2.instance.DisplayTestResult("LCD AREA", "-", "-", "-", _result);
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = ResultLcdArea.ImageResult.ToBitmap(), index = 0 });
                Log.AddInfo("LCD AREA", "-", "-", "-", _result, testTimer.Elapsed.Seconds);
                PmLogger.SetCsvValue(PmLogger.TestItem.LcdArea, ResultLcdArea.m_iDisplayArea.ToString());
            }


            if (Config.SaveResultImage == true)
            {
                Controller.SaveResultImage("01.LcdAreaSrc", ImageInput.ToBitmap());
                if (_result)
                    Controller.SaveResultImage("02.LcdAreaResult", ResultLcdArea.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("02.LcdAreaFail", ResultLcdArea.ImageResult.ToBitmap());
            }
            isFinished = true;
            testTimer.Stop();

            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
        }
    }
}
