using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;

namespace AutoInspection_GUMI
{
    public class InspectionKeyTouch : Inspection
    {
        public MData_TOUCH_KEY specTouchKey;
        public MData_LCD_AREA specLcdArea;

        public MResult_TOUCH_KEY ResultTouchKey = new MResult_TOUCH_KEY();
        MResult_TOUCH_BACKKEY ResultTouchBackKey = new MResult_TOUCH_BACKKEY();
        MResult_TOUCH_MENUKEY ResultTouchMenuKey = new MResult_TOUCH_MENUKEY();

        public override void SetSrcImage(Bitmap bmpImage)
        {
            //lock (bmpImage)
            {
                // ImageInput = BitmapConverter.ToMat(bmpImage).Clone();
                ImageInput = null;
                ImageInput = bmpImage.ToMat();
            }
        }

        public override void ResetResult()
        {
            ResultTouchKey.Clear();
            ResultTouchBackKey.Clear();
            ResultTouchMenuKey.Clear();
        }

        //public override void DoVisionTest()
        //{
        //    ResetResult();
        //    testThread.RunWorkerAsync();
        //}

        public InspectionKeyTouch(string _testName,
            MData_LCD_AREA DataLcdArea, MData_TOUCH_KEY DataTouchKey
            , VisionTest _visionTest, string _logFolder, FrmMainCh2 _mainForm, Controller _controller)
        {
            testName = _testName;
            visionTest = _visionTest;
            LogFolder = _logFolder;
            mainFrom = _mainForm;

            specLcdArea = DataLcdArea;
            specTouchKey = DataTouchKey;
            controller = _controller;
            testThread = new BackgroundWorker();
            testThread.DoWork += new DoWorkEventHandler(TestIamge);
            testThread.RunWorkerCompleted += testThread_RunWorkerCompleted;

            testTimer = new Stopwatch();
        }

        void testThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ResultTouchKey.TestResult == false)
                controller.eFinalResult |= InspectionResult.KeyTouchResult;


            InspStatus = InspectionState.Done;
            ImageInput.Dispose();
            GC.Collect();
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;
            bool _result = true;

            testTimer.Reset();
            testTimer.Start();

            visionTest.TouchKey
                (
                ImageInput,
                specTouchKey,
                ref ResultTouchKey,
                ref ResultTouchBackKey,
                ref ResultTouchMenuKey);

            testTimer.Stop();
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));

            _result = true;
            if (ResultTouchKey.TouchKeyBrightJudgeArea != specTouchKey.TouchKeyBrightAreaSpec)
            {
                _result = false;
                ResultTouchKey.TestResult = false;
                controller.ResultFront = false;

                string _s = LogFolder + Controller.logFolderName + "TouchBright.bmp";
                mainFrom.DisplayErrorMsg("TouchBright", _s);
            }

            if (Config.SaveResultImage == true)
            {
                Controller.SaveResultImage("TouchBrightSrc", ImageInput.ToBitmap());

                if (_result)
                    Controller.SaveResultImage("TouchBrightResult", ResultTouchKey.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("TouchBrightResultFail", ResultTouchKey.ImageResult.ToBitmap());
            }
            mainFrom.DisplayTestResult("TOUCH KEY(BRIGHT AREA)", ResultTouchKey.TouchKeyBrightJudgeArea.ToString("#,#")
                , specTouchKey.TouchKeyBrightAreaSpec.ToString(), specTouchKey.TouchKeyBrightAreaSpec.ToString(), _result);
            Log.AddInfo("TOUCH KEY(BRIGHT AREA)", ResultTouchKey.TouchKeyBrightJudgeArea.ToString()
                , specTouchKey.TouchKeyBrightAreaSpec.ToString(), specTouchKey.TouchKeyBrightAreaSpec.ToString(), _result, testTimer.Elapsed.Seconds);
            // todo : Implement that.
            // PmLogger.SetCsvValue(); 


            _result = true;
            if (ResultTouchBackKey.m_dDiffArea > 0)
            {
                ResultTouchKey.TestResult = false;
                controller.ResultFront = false;
                _result = false;

                string _s = LogFolder + Controller.logFolderName + "BackDiff.bmp";
                mainFrom.DisplayErrorMsg("BackDiff", _s);
            }

            if (Config.SaveResultImage == true)
            {
                if (_result)
                    Controller.SaveResultImage("BackDiffResult", ResultTouchBackKey.ImageResult.ToBitmap()); // NJH
                else
                    Controller.SaveResultImage("BackDiffResultFail", ResultTouchBackKey.ImageResult.ToBitmap()); // NJH
            }



            mainFrom.DisplayTestResult("BACK KEY DUST ", ResultTouchBackKey.m_dDiffArea.ToString()
                , "0", "0", _result);
            Log.AddInfo("BACK KEY(Diff Area) ", ResultTouchBackKey.m_dDiffArea.ToString()
                , "0", "0", _result, testTimer.Elapsed.Seconds);
            // todo : Implement that.
            // PmLogger.SetCsvValue(); 

            _result = true;
            if (ResultTouchMenuKey.m_dDiffArea > 0)
            {
                _result = false;
                ResultTouchKey.TestResult = false;
                controller.ResultFront = false;

                // string _s = LogFolder + Controller.logFolderName + "MenuDiff.bmp";
                // ResultTouchMenuKey.ImageResult.SaveImage(_s);
                // mainFrom.DisplayErrorMsg("MenuDiff", _s);
            }

            if (Config.SaveResultImage == true)
            {
                if (_result)
                    Controller.SaveResultImage("MenuDiffResult", ResultTouchMenuKey.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("MenuDiffResultFail", ResultTouchMenuKey.ImageResult.ToBitmap());
            }

            mainFrom.DisplayTestResult("MENU KEY DUST ", ResultTouchMenuKey.m_dDiffArea.ToString()
                , "0", "0", _result);
            Log.AddInfo("MENU KEY(Diff Area) ", ResultTouchMenuKey.m_dDiffArea.ToString()
                , "0", "0", _result, testTimer.Elapsed.Seconds);

            // todo : Implement that.
            // PmLogger.SetCsvValue(); 

        }
    }
}