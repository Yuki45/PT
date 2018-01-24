using AutoInspection.Forms;
using OpenCvSharp.CPlusPlus;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using AutoInspection;

namespace AutoInspection_GUMI
{
    public enum InspectionState
    {
        Ready, 
        Running,
        Done
    }
    
    public abstract class Inspection
    {
        public string testName;
        public InspectionState InspStatus;
        //public InspectionThread inspectionThread;
        public FrmMainCh2 mainFrom;
 
        public VisionTest visionTest;
        public BackgroundWorker testThread;
        public Mat ImageInput;
        public Stopwatch testTimer;
        public string LogFolder = @"C:\IMG";
        public Controller controller;

        public void DoVisionTest()
        {
            ResetResult();
            testThread.RunWorkerAsync();
        }

        public abstract void ResetResult();
        public abstract void SetSrcImage(Bitmap bmpImage);
    }
}
