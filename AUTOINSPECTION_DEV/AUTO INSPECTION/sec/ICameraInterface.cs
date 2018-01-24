
using AutoInspection_GUMI;
using System.Drawing;
using System.Windows.Forms;

namespace AutoInspection.sec
{
    public interface ICameraInterface
    {
        Bitmap OneShot_();
        Bitmap OneShot_(int Exposure);

        void Start();
        void Stop();
        void DestroyCamera();
        void SaveUserSet();
        void SetLivePlay(bool bLiveMode);  
        void GetParameter(ref CameraParams cameraParams);
        void SetParameter(CameraParams cameraParams);
        void SetExposure(int _exposure);
        void SetPictureBox(PictureBox control);
        void ContinuousShot();
        long GetColorBalance(AverageColor clr);
        void SetColorBalance(AverageColor clr, long value);
        void WBAuto();
        void DisplaySetupPannel();
        bool isContinuous();
    }

    public struct CameraParams
    {
        public int ExposureValue;
        public int MinExposure;
        public int MaxExposure;

        public int Width;
        public int MinWidth;
        public int MaxWidth;

        public int Height;
        public int MinHeight;
        public int MaxHeight;

        public int Xoffset;
        public int MinXoff;
        public int MaxXoff;

        public int Yoffset;
        public int MinYoff;
        public int MaxYoff;

        public int RedBalanceRatio;
        public int BlueBalanceRatio;

    }
}
