using AutoInspection.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;
using OpenCvSharp.Extensions;
using AutoInspection.sec.GUMI;

namespace AutoInspection_GUMI
{
    public class InspectionBarcode : Inspection
    {
        public MData_REAR testSpec;
        public MResult_REAR testResult = new MResult_REAR();
        //public Bitmap bImageInput;
        public bool isFinished = false;

        public override void SetSrcImage(Bitmap bmpImage)
        {
            ImageInput = bmpImage.ToMat();
        }

        public override void ResetResult()
        {
            testResult.Clear();
        }
       
        public InspectionBarcode(string _testName, MData_REAR _testSpec, VisionTest _visionTest,
            string _logFolder, FrmMainCh2 mainForm, Controller _controller)
        {
            testName = _testName;
            testSpec = _testSpec;
            visionTest = _visionTest;
            LogFolder = _logFolder;
            mainFrom = mainForm;
            controller = _controller;
            testThread = new BackgroundWorker();
            testThread.DoWork += new DoWorkEventHandler(TestIamge);
            testThread.RunWorkerCompleted += testThread_RunWorkerCompleted;

            testTimer = new Stopwatch();
        }
        public void DoVisonTest_Synch()
        {
            ResetResult();
            TestIamge(null, null);
            if (testResult.TestResult == false)
                controller.eFinalResult |= InspectionResult.BarcodeResult;
            InspStatus = InspectionState.Done;
        }
        void testThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (testResult.TestResult == false)
                controller.eFinalResult |= InspectionResult.BarcodeResult;

            InspStatus = InspectionState.Done;
        }

        public void TestIamge(object sender, DoWorkEventArgs e)
        {

            InspStatus = InspectionState.Running;

            testTimer.Reset();
            testTimer.Start();

            visionTest.Barcode(ImageInput.ToBitmap(),controller.scenarioManger.testSpec, testSpec, ref testResult);
            isFinished = true;

            testResult.TestResult = true; 
            if(testResult.m_nImei == string.Empty)
            {
                testResult.TestResult = false;
                //string _s = LogFolder + Controller.logFolderName + "Barcode.bmp";
                //ImageInput.SaveImage(_s);
                //mainFrom.DisplayErrorMsg("Barcode", _s);
            }

            if (Config.SaveResultImage)
            {
                Controller.SaveResultImage("16.BarcodeSrc", ImageInput.ToBitmap());
                //if (testResult.TestResult)
                //    Controller.SaveResultImage("17.BarcodeResult", testResult.ImageResult.ToBitmap());
                //else
                //    Controller.SaveResultImage("17.BarcodFailed", testResult.ImageResult.ToBitmap());
            }

            testTimer.Stop();

            mainFrom.DisplayImei(testResult.m_nImei.ToString());
            //if (!controller.scenarioManger.testSpec.SpecTestList.TestFront)
            //{
            //    mainFrom.DisplayTestResult("Barcode", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult);
            //}

            mainFrom.DisplayTestResult("Barcode", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult);
            Log.AddLog(string.Format("IMEI Barcode Test : Measured {0}", testResult.m_nImei.ToString()));
            Log.AddLog(string.Format("({0}) Finished, Elapsed Time : {1:#,#}ms", testName, testTimer.ElapsedMilliseconds));
            Log.AddInfo("IMEI", testResult.m_nImei.ToString(), "-", "-", testResult.TestResult, testTimer.Elapsed.Seconds);

            PmLogger.SetCsvValue(PmLogger.TestItem.Barcode, testResult.m_nImei.ToString());
            // todo : Implement that.
            // PmLogger.SetCsvSetValue(); 

        }
    }
}
