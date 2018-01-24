using AutoInspection.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;
using OpenCvSharp.Extensions;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionOcr : Inspection
    {
        public MData_REAR testSpec;
        public MResult_REAR testResult = new MResult_REAR();
        //Bitmap bImageInput;
        public bool isFinished = false;

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat();
        }

        public override void ResetResult()
        {
            testResult.Clear();
        }

        public InspectionOcr(string _testName, MData_REAR _testSpec, VisionTest _visionTest,
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
                controller.eFinalResult |= InspectionResult.OcrResult;
            }
            InspStatus = InspectionState.Done;
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {
            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();

            string sTarget = controller.mainForm.GetImei().Remove(0, 12);
            string sTarget2 = controller.mainForm.GetImei().Remove(0, 11);
            sTarget2 = sTarget2.Substring(0, 3);
            //bool bResult = true;
            bool result = visionTest.OCR(ImageInput.ToBitmap(),
                controller.scenarioManger.testSpec,
                sTarget,
                sTarget2,
                testSpec,
                controller.mainForm,
                ref testResult);

            isFinished = true;

            if (testResult.TestResult == false)
            {
                // todo : PmLogger fail 사용할 것. 

                // string _s = LogFolder + Controller.logFolderName + "ocr.bmp";
                // controller.mainForm.DisplayErrorMsg("OCR", _s);
                // testResult.ImageResult.SaveImage(_s);

                //controller.mainForm.DisplayImage(1, testResult.ImageResult.ToBitmap());
                //testResult.areaNgImage.Save(LogFolder + Controller.logFolderName + "areaImg.bmp");
            }

            if (Config.SaveResultImage)
            {
                Controller.SaveResultImage("18.OcrSrc", ImageInput.ToBitmap());
                //if (testResult.TestResult)
                //    Controller.SaveResultImage("19.OcrResult", testResult.ImageResult.ToBitmap());
                //else
                //    Controller.SaveResultImage("19.OcrFailed", testResult.ImageResult.ToBitmap());
            }
            //if (!controller.scenarioManger.testSpec.SpecTestList.TestFront)
            //{
            //    controller.mainForm.DisplayTestResult("OCR", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult);
            //}
                testTimer.Stop();
            controller.mainForm.DisplayTestResult("OCR", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult);
            Log.AddInfo("OCR", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult, testTimer.Elapsed.Seconds);
            // todo : Implement Taht : PmLogger.SetCsvValue() 
            Log.AddLog(string.Format("Ocr Test : Measured {0}", testResult.m_nImei.ToString()));
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
            PmLogger.SetCsvValue(PmLogger.TestItem.Ocr, testResult.m_nImei.ToString());
        }
    }
}
