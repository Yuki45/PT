using AutoInspection.Forms;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Collections.Generic;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionLcdGreen : Inspection
    {
        public MData_LCD_GREEN testSpec;
        public MResult_LCD_GREEN testResult = new MResult_LCD_GREEN();
        public List<Mat> listInputImages = new List<Mat>();
        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = null;
            ImageInput = bmpImage.ToMat();
        }
        
        public override void ResetResult()
        {
            testResult.Clear();
        }



        public InspectionLcdGreen(string _testName, MData_LCD_GREEN _testSpec, VisionTest _visionTest,
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
                controller.eFinalResult |= InspectionResult.LcdGreenResult;
            }


            InspStatus = InspectionState.Done;
            //ImageInput.Dispose();
            GC.Collect();
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;
            testResult.TestResult = true;

            testTimer.Reset();
            testTimer.Start();

            //if (Config.GreenaAcumulateTest)
                visionTest.LcdGreen2(listInputImages, testSpec, ref testResult);
            //else
            //    visionTest.LcdGreen(ImageInput, testSpec, ref testResult, controller);

            // if (testResult.m_nBlackDot_JudgeSize > testSpec.BlackDot_GJudgeSizeUL)
            if (testResult.TestResult == false ) 
            {
                PmLogger.SetCsvFailItem(testName);

                #region skip
                //170416 sh32.heo
                //compare the previous tested set 
                //if ( (testSpec.useCompare == true) && (controller.UN == controller.PREV_UN ) &&
                //( controller.prevPointError.Count > 0 ) && (controller.thisPointError.Count > 0))
                //{
                //    foreach (CvPoint cvPoint in controller.thisPointError)
                //    {
                //        for (int i = 0; i < controller.prevPointError.Count; i++)
                //        {
                //            if ((cvPoint.X < controller.prevPointError[i].X + testSpec.m_nBlackDot_GOffset)
                //                && (cvPoint.X > controller.prevPointError[i].X - testSpec.m_nBlackDot_GOffset)
                //                && (cvPoint.Y < controller.prevPointError[i].Y + testSpec.m_nBlackDot_GOffset)
                //                && (cvPoint.Y > controller.prevPointError[i].Y - testSpec.m_nBlackDot_GOffset))
                //            {
                //                testResult.TestResult = false;
                //            }
                //            if (testResult.TestResult == false) break;
                //        }
                //    }
                //}
                //else if ((testSpec.useCompare == true) && (controller.UN == controller.PREV2_UN) && controller.prev2PointError.Count > 0
                //&& controller.thisPointError.Count > 0)
                //{
                //    foreach (CvPoint cvPoint in controller.thisPointError)
                //    {
                //        for (int i = 0; i < controller.prev2PointError.Count; i++)
                //        {
                //            if ((cvPoint.X < controller.prev2PointError[i].X + testSpec.m_nBlackDot_GOffset) &&
                //                (cvPoint.X > controller.prev2PointError[i].X - testSpec.m_nBlackDot_GOffset) &&
                //                (cvPoint.Y < controller.prev2PointError[i].Y + testSpec.m_nBlackDot_GOffset) &&
                //                (cvPoint.Y > controller.prev2PointError[i].Y - testSpec.m_nBlackDot_GOffset))
                //            {
                //                testResult.TestResult = false;
                //            }
                //            if (testResult.TestResult == false) break;
                //        }
                //    }
                //}
                //else
                #endregion

                // testResult.TestResult = false;

                //controller.ListImgFail.Add(
                //    new IMGFAIL()
                //    {
                //        bmpimgfail = testResult.imgNGResult.ToBitmap(), index = 0
                //    });

                controller.ResultFront = false;
                controller.ListImgFail.Add(new IMGFAIL() { bmpimgfail = testResult.ImageResult.ToBitmap(), index = 0 });
            }

            if (Config.SaveResultImage )
            {

                //Controller.SaveResultImage("08.LcdGreenDetailResult", testResult.ImageResultTmp.ToBitmap());
                //if (Config.GreenaAcumulateTest)
                {
                    for (int i = 0; i < listInputImages.Count; i++)
                    {
                        Controller.SaveResultImage("07.LcdGreenSrc" + i, listInputImages[i].ToBitmap());
                    }
                }
                //else
                //    Controller.SaveResultImage("07.LcdGreenSrc", ImageInput.ToBitmap());

                if( testResult.TestResult )
                    Controller.SaveResultImage("08.LcdGreenResult", testResult.ImageResult.ToBitmap());
                else
                    Controller.SaveResultImage("08.LcdGreenFail", testResult.ImageResult.ToBitmap());
            }
            testTimer.Stop();



            //mainFrom.DisplayTestResult("LCD GREEN", testResult.m_nBlackDot_JudgeSize.ToString("#,#")
            //    , testSpec.BlackDot_GJudgeSizeUL.ToString("#,#"), "-", testResult.TestResult);
            mainFrom.DisplayTestResult("LCD GREEN", testResult.m_nBlackDot_JudgeSize.ToString("#,#")
                , "0", testSpec.BlackDot_GJudgeSizeUL.ToString("#,#"), testResult.TestResult);
            //Log.AddInfo("LCD GREEN", testResult.m_nBlackDot_JudgeSize.ToString()
            //    , testSpec.BlackDot_GJudgeSizeUL.ToString(), "-", testResult.TestResult, testTimer.Elapsed.Seconds);
            Log.AddInfo("LCD GREEN", testResult.m_nBlackDot_JudgeSize.ToString()
                ,  "0", testSpec.BlackDot_GJudgeSizeUL.ToString(), testResult.TestResult, testTimer.Elapsed.Seconds);

            PmLogger.SetCsvValue( PmLogger.TestItem.LcdGreen, testResult.m_nBlackDot_JudgeSize.ToString());
            PmLogger.SetCsvValue(PmLogger.TestItem.LcdGreenWHAvg, testResult.m_dBlackDot_JudgeWHAvg.ToString("#.##"));
            PmLogger.SetCsvValue(PmLogger.TestItem.LcdGreenMinRect, testResult.m_dBlackDot_JudgeSize_MinRect.ToString("#.##"));
            PmLogger.SetCsvValue(PmLogger.TestItem.LcdGreenMinRectWHAvg, testResult.m_dBlackdot_JudgeWHAvg_MinRect.ToString("#.##"));
            PmLogger.SetCsvValue(PmLogger.TestItem.LcdGreenArea, testResult.m_dBlackDot_JudgeArea.ToString("#.##"));
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
        }
    }
}

