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
    public class InspectionLcdDust : Inspection 
    {
        public MData_LCD_AREA specLcdArea;
        public MData_LCD_DUST specLcdDust;

        public MResult_LCD_AREA ResultLcdArea = new MResult_LCD_AREA();
        public MResult_LCD_DUST ResultDust = new MResult_LCD_DUST();

        public bool isFinished = false;

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat();  // BitmapConverter.ToMat(bmpImage).Clone();
        }

        public override void ResetResult()
        {
            ResultLcdArea.Clear();
            ResultDust.Clear();
        }

        public InspectionLcdDust(string _testName
            , MData_LCD_AREA _testSpec, MData_LCD_DUST _testSpecDust, VisionTest _visionTest,
            string _logFolder, FrmMainCh2 _mainForm, Controller _controller )
        {
            testName = _testName;

            specLcdArea = _testSpec;
            specLcdDust = _testSpecDust;
            visionTest = _visionTest;
            LogFolder = _logFolder;
            mainFrom = _mainForm;

            controller = _controller;
            testThread = new BackgroundWorker();
            testThread.DoWork += new DoWorkEventHandler(TestIamge);
            testThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(TestCompleted);
            
            testTimer = new Stopwatch();
        }

        public void TestCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if( !ResultDust.TestResult )
            {
                controller.eFinalResult |= InspectionResult.LcdDustResult;
            }

            InspStatus = InspectionState.Done;
            GC.Collect();
        }

        public void DoVisonTest_Synch()
        {
            ResetResult();
            TestIamge(null, null);
            InspStatus = InspectionState.Done;
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();
            ResultLcdArea.TestResult = true;
            ResultDust.TestResult = true;

            bool _result = visionTest.DustProcess(ImageInput, controller.scenarioManger.testSpec, ref ResultLcdArea, ref ResultDust);

            testTimer.Stop();

            if (!_result)
            {
                PmLogger.SetCsvFailItem(testName);

                ResultDust.TestResult = false;
                controller.ResultFront = false;

                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = ResultDust.ImageResult.ToBitmap(), index = 0 });
                
				 if (ResultDust.m_dArea != -1)
                {
                    mainFrom.DisplayTestResult("Dust_Area", ResultDust.m_dArea.ToString("#,#.###")
                        , "0", specLcdDust.JudgeDustAreaUL.ToString("#,#.###"), false);
                    Log.AddInfo("Dust_Area", ResultDust.m_dArea.ToString() 
                    , "0", specLcdDust.JudgeDustAreaUL.ToString(),  false, testTimer.Elapsed.Seconds);
                    PmLogger.SetCsvValue( PmLogger.TestItem.LcdDust, specLcdDust.JudgeDustAreaUL.ToString());
                }
                else
                {
                    mainFrom.DisplayTestResult("Dust_Contour", ResultDust.m_dContour.ToString("#,#.###")
                    , specLcdDust.JudgeDustMaxNumUL.ToString("#,#.###"), "-", false);
                    Log.AddInfo("Dust_Contour", ResultDust.m_dContour.ToString("#,#.###")
                    , specLcdDust.JudgeDustMaxNumUL.ToString("#,#.###"), "-", false, testTimer.Elapsed.Seconds);

                    PmLogger.SetCsvValue(PmLogger.TestItem.LcdDustContour, specLcdDust.JudgeDustAreaUL.ToString());
                }
				

                isFinished = true;
                if (Config.SaveResultImage)
                {
                    Controller.SaveResultImage("03.DustSrc", ImageInput.ToBitmap());
                    Controller.SaveResultImage("04.DustFail", ResultDust.ImageResult.ToBitmap());
                }

                return;
            }

            if(Config.SaveResultImage )
            {
                Controller.SaveResultImage("03.DustSrc", ImageInput.ToBitmap());
                Controller.SaveResultImage("04.DustResult", ResultDust.ImageResult.ToBitmap());
            }


            //_result = true;
            //if (controller.scenarioManger.testSpec.SpecTestList.TestFrontLed == true)
            //{
            //    if ((ResultLcdArea.m_iLedArea < specLcdArea.m_dJudgeLedAreaLL || ResultLcdArea.m_iLedArea > specLcdArea.m_dJudgeLedAreaUL))
            //    {
            //        PmLogger.SetCsvFailItem(testName);

            //        ResultLcdArea.TestResult = false;
            //        _result = false;
            //    }
            //    mainFrom.DisplayTestResult("Led Area", ResultLcdArea.m_iLedArea.ToString("#,#.###")
            //        , specLcdArea.m_dJudgeLedAreaLL.ToString("#,#.###"), specLcdArea.m_dJudgeLedAreaUL.ToString("#,#.###"), _result);

            //    Log.AddInfo("LED AREA", ResultLcdArea.m_iLedArea.ToString()
            //        , specLcdArea.m_dJudgeLedAreaLL.ToString(), specLcdArea.m_dJudgeLedAreaUL.ToString(), _result, testTimer.Elapsed.Seconds);
            //    // todo : Implement That 
            //    // PmLogger.SetCsvValue(PmLogger.TestItem.LcdArea, ResultLcdArea.m_iDisplayArea.ToString(""));
            //}

            _result = true;
            if ( ResultLcdArea.m_iDisplayArea < specLcdArea.JudgeDisplayAreaLL || ResultLcdArea.m_iDisplayArea > specLcdArea.JudgeDisplayAreaUL)
            {
                ResultLcdArea.TestResult = false;
                _result = false;     // bDisplayArea = false;
            }
            mainFrom.DisplayTestResult("LCD DISPLAY AREA", ResultLcdArea.m_iDisplayArea.ToString("#,#")
                , specLcdArea.JudgeDisplayAreaLL.ToString("#,#"), specLcdArea.JudgeDisplayAreaUL.ToString("#,#"), _result);
            Log.AddInfo("LCD DISPLAY AREA", ResultLcdArea.m_iDisplayArea.ToString()
                , specLcdArea.JudgeDisplayAreaLL.ToString(), specLcdArea.JudgeDisplayAreaUL.ToString(), _result, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue(PmLogger.TestItem.LcdArea, ResultLcdArea.m_iDisplayArea.ToString()); 

            _result = true;
            if (controller.scenarioManger.testSpec.SpecTestList.TestFrontKey == true)
            {

                if ((ResultLcdArea.m_iBackArea < specLcdArea.JudgeBackAreaLL || ResultLcdArea.m_iBackArea > specLcdArea.JudgeBackAreaUL))
                {
                    ResultLcdArea.TestResult = false;
                    _result = false;    // bBackArea = false;
                }
                mainFrom.DisplayTestResult("BackArea", ResultLcdArea.m_iBackArea.ToString()
                    , specLcdArea.JudgeBackAreaLL.ToString(), specLcdArea.JudgeBackAreaUL.ToString(), _result);
                Log.AddInfo("BackArea", ResultLcdArea.m_iBackArea.ToString()
                    , specLcdArea.JudgeBackAreaLL.ToString(), specLcdArea.JudgeBackAreaUL.ToString(), _result, testTimer.Elapsed.Seconds);
                // todo : Implement that.
                // PmLogger.SetCsvSetMenuArea(ResultLcdArea.m_iMenuArea.ToString("#,#"));
            }

            _result = true;
            if (controller.scenarioManger.testSpec.SpecTestList.TestFrontKey == true)
            {
                if ((ResultLcdArea.m_iMenuArea < specLcdArea.JudgeMenuAreaLL || ResultLcdArea.m_iMenuArea > specLcdArea.JudgeMenuAreaUL))
                {
                    ResultLcdArea.TestResult = false;
                    _result = false;
                }
                mainFrom.DisplayTestResult("MenuArea", ResultLcdArea.m_iMenuArea.ToString()
                    , specLcdArea.JudgeMenuAreaLL.ToString(), specLcdArea.JudgeMenuAreaUL.ToString(), _result);
                Log.AddInfo("MenuArea", ResultLcdArea.m_iMenuArea.ToString()
                    , specLcdArea.JudgeMenuAreaLL.ToString()
                    , specLcdArea.JudgeMenuAreaUL.ToString(), _result, testTimer.Elapsed.Seconds);
                // todo : Implement that.
                // PmLogger.SetCsvSetMenuArea(ResultLcdArea.m_iMenuArea.ToString("#,#"));
            }

            _result = true;
            mainFrom.DisplayTestResult("LCD DUST", ResultDust.m_dArea.ToString("#,#")
                , "0", specLcdDust.JudgeDustAreaUL.ToString(), _result);

            Log.AddInfo("LCD DUST", ResultDust.m_dArea.ToString()
                , "0", specLcdDust.JudgeDustAreaUL.ToString(), _result, testTimer.Elapsed.Seconds);
            PmLogger.SetCsvValue( PmLogger.TestItem.LcdDust, ResultDust.m_dArea.ToString());


            if (ResultLcdArea.TestResult == false)
            {
                PmLogger.SetCsvFailItem(testName);
                controller.ResultFront = false;
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = ResultLcdArea.ImageResult.ToBitmap(), index = 0 });
            }

            if(Config.SaveResultImage == true)
            {
                if ( ResultLcdArea.TestResult )
                    Controller.SaveResultImage("04.LcdAreaPass", ResultLcdArea.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("04.LcdAreaFail", ResultLcdArea.ImageResult.ToBitmap());
            }

            isFinished = true;
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));

        }
    }
}
