//#define SAVEIMG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ZXing;
using Tesseract;
using System.Diagnostics;
using BarcodeLib.BarcodeReader;
using PSMNameSpace;
using System.Threading;
using System.Windows;
using AutoInspection.Utils;
using AutoInspection_GUMI;
using AutoInspection.Forms;
using System.Reflection;
using System.Linq;

namespace AutoInspection
{

    public class ErrPoint
    {
        public CvPoint cvPoint;
        public CvRect cvRect;
        public double dArea;

        public ErrPoint(CvPoint _cvPoint, CvRect _cvRect, double _dArea)
        {
            cvPoint = _cvPoint;
            cvRect = _cvRect;
            dArea = _dArea;
        }
    }

    public class VisionTest // <- CVAreaProcess.cs
    {
		double fontScale = 3; // Error Pos Font Scale

        public Mat[] ImageInput = new Mat[100];
        public Bitmap[] BImageInput = new Bitmap[100];
        enum BMP
        {
            CAMERA_DATA_DIMMING,
            CAMERA_DATA_DIMMING_WHITE,
            CAMERA_DATA_DIMMING_GREEN,
            CAMERA_DATA_SOMETHING,
            CAMERA_DATA_AREA,
            CAMERA_DATA_TOUCH_MENU,
            CAMERA_DATA_TOUCH_BACK,
            CAMERA_DATA_SVC_WHITE,
            CAMERA_DATA_BLACK
        }

        VisionTools vt = new VisionTools();
        PSM psm = new PSM();

        public Mat[] ResultImg = new Mat[25];

        int m_displayWidth;
        int m_displayHeight;
        int m_displayX;
        int m_displayY;

        CvRect displayAreaRect;
        CvRect menuAreaRect;
        CvRect backAreaRect;
        CvRect ledAreaRect;
        CvBox2D rRect = new CvBox2D();

        int BLUE_CHANNEL = 0;
        int GREEN_CHANNEL = 1;
        int RED_CHANNEL = 2;
        int MAX_CHANNEL = 3;
        
        Mat angleMat;
        Mat rotationImage;


        Mat m_BlackTmpBack;
        Mat m_BlackTmpMenu;

        Mat[] back_offset_contours;
        Mat[] out_contours;
        Mat[] in_contours;

		public string GetRgbAverage(Bitmap bmprefNormalImageArea, MData_LCD_AREA DataLcdArea, AverageColor ColorMode)
        {
            Mat refNormalImageArea = bmprefNormalImageArea.ToMat();
            Mat origianlImg = refNormalImageArea.Clone();

            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);
            else if (origianlImg.Channels() == 4)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgraToGray);

            //threshold to make displayArea clearly
            Mat displayArea = new Mat();
            Cv2.Threshold(origianlImg, displayArea, 50, 255, ThresholdType.Binary);
            origianlImg.Dispose();

            //get contours
            Mat[] contours;
            var hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

            double dArea;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
				// Console.WriteLine(string.Format("{0} countour area : {1}", i, dArea));
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                    //get the rect from the display sector  
                    rRect = Cv2.MinAreaRect(contours[i]);
            }
            displayArea.Release();

            rotationImage = refNormalImageArea.Clone();

            //rotate the image on tilted angle
            CvPoint2D32f center = new CvPoint2D32f(refNormalImageArea.Cols / 2, refNormalImageArea.Rows / 2);
            angleMat = Cv2.GetRotationMatrix2D(center, rRect.Angle < -45 ? rRect.Angle + 90 : rRect.Angle, 1.0);
            Cv2.WarpAffine(refNormalImageArea, rotationImage, angleMat, refNormalImageArea.Size());

            //get the area using rotated image
            Mat imgArea = rotationImage.Clone();
            Mat tmpDisplay = new Mat();

            if (imgArea.Channels() == 3)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgrToGray);
            else if (imgArea.Channels() == 4)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgraToGray);

            Cv2.Threshold(imgArea, tmpDisplay, 50, 255, ThresholdType.Binary);
            hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(tmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            imgArea.Dispose();
            tmpDisplay.Dispose();
            CvBox2D MinAreaRect = new CvBox2D();

            Mat gray = rotationImage.Clone();
            Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }

            CvScalar clr;
            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                MessageBox.Show("fail to get the area rect");
            }

            m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX, m_displayY, m_displayWidth, m_displayHeight);
            Mat resultImg = rotationImage.Clone();

            Bitmap b = new Mat(resultImg, displayAreaRect).ToBitmap();
            //b.Save(@".\Detail\greenavg.jpg");
            decimal sum = 0;

            for (int j = 0; j < displayAreaRect.Height; j++)
            {
                for (int i = 0; i < displayAreaRect.Width; i++)
                {
                    if (ColorMode == AverageColor.BLUE)
                    {
                        sum += b.GetPixel(i, j).B;
                    }
                    else if (ColorMode == AverageColor.GREEN)
                    {
                        sum += b.GetPixel(i, j).G;
                    }
                    else if (ColorMode == AverageColor.RED)
                    {
                        sum += b.GetPixel(i, j).R;
                    }
                }
            }

            double avg = (double)(sum / (displayAreaRect.Width * displayAreaRect.Height));

            Log.AddLog(string.Format("Avg({0}), Area({1}), Sum({2})"
                , avg.ToString("#")
                , sum.ToString("#,#")
                , (displayAreaRect.Width * displayAreaRect.Height).ToString("#,#")));

            string _result = string.Format(
                "Avg({0}), W({1}), H({2}), Area({3})"
                , avg.ToString("#.#")
                , displayAreaRect.Width.ToString(), displayAreaRect.Height.ToString()
                , (displayAreaRect.Width * displayAreaRect.Height).ToString("#,#"));

            return _result;
        }

        public double GetGreenAverage(Bitmap bmprefNormalImageArea, MData_LCD_AREA DataLcdArea)
        {
            Mat refNormalImageArea = bmprefNormalImageArea.ToMat();
            Mat origianlImg = refNormalImageArea.Clone();

            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);
            else if (origianlImg.Channels() == 4)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgraToGray);

            //threshold to make displayArea clearly
            Mat displayArea = new Mat();
            Cv2.Threshold(origianlImg, displayArea, 50, 255, ThresholdType.Binary);
            origianlImg.Dispose();

            //get contours
            Mat[] contours;
            var hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

            double dArea;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                Console.WriteLine(string.Format("{0} countour area : {1}", i, dArea));
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                    //get the rect from the display sector  
                    rRect = Cv2.MinAreaRect(contours[i]);
            }
            displayArea.Release();

            rotationImage = refNormalImageArea.Clone();

            //rotate the image on tilted angle
            CvPoint2D32f center = new CvPoint2D32f(refNormalImageArea.Cols / 2, refNormalImageArea.Rows / 2);
            angleMat = Cv2.GetRotationMatrix2D(center, rRect.Angle < -45 ? rRect.Angle + 90 : rRect.Angle, 1.0);
            Cv2.WarpAffine(refNormalImageArea, rotationImage, angleMat, refNormalImageArea.Size());

            //get the area using rotated image
            Mat imgArea = rotationImage.Clone();
            Mat tmpDisplay = new Mat();

            if (imgArea.Channels() == 3)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgrToGray);
            else if (imgArea.Channels() == 4)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgraToGray);

            Cv2.Threshold(imgArea, tmpDisplay, 50, 255, ThresholdType.Binary);
            hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(tmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            imgArea.Dispose();
            tmpDisplay.Dispose();
            CvBox2D MinAreaRect = new CvBox2D();

            Mat gray = rotationImage.Clone();
            Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }

            CvScalar clr;
            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                MessageBox.Show("fail to get the area rect");
            }

            m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX, m_displayY, m_displayWidth, m_displayHeight);
            Mat resultImg = rotationImage.Clone();

            Bitmap b = new Mat(resultImg, displayAreaRect).ToBitmap();
            //b.Save(@".\Detail\greenavg.jpg");
            int sum_g = 0;
            for (int j = 0; j < displayAreaRect.Height; j++)
            {
                for (int i = 0; i < displayAreaRect.Width; i++)
                {
                    sum_g += b.GetPixel(i, j).G;
                }
            }
            double avg = sum_g / (displayAreaRect.Width * displayAreaRect.Height);
            //Console.WriteLine(avg);

            return avg;
        }
        public double GetGrayAverage(Bitmap bmprefNormalImageArea, MData_LCD_AREA DataLcdArea)
        {
            Mat refNormalImageArea = bmprefNormalImageArea.ToMat();
            Mat origianlImg = refNormalImageArea.Clone();

            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);
            else if (origianlImg.Channels() == 4)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgraToGray);

            //threshold to make displayArea clearly
            Mat displayArea = new Mat();
            Cv2.Threshold(origianlImg, displayArea, 50, 255, ThresholdType.Binary);
            origianlImg.Dispose();

            //get contours
            Mat[] contours;
            var hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

            double dArea;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                Console.WriteLine(string.Format("{0} countour area : {1}", i, dArea));
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                    //get the rect from the display sector  
                    rRect = Cv2.MinAreaRect(contours[i]);
            }
            displayArea.Release();

            rotationImage = refNormalImageArea.Clone();

            //rotate the image on tilted angle
            CvPoint2D32f center = new CvPoint2D32f(refNormalImageArea.Cols / 2, refNormalImageArea.Rows / 2);
            angleMat = Cv2.GetRotationMatrix2D(center, rRect.Angle < -45 ? rRect.Angle + 90 : rRect.Angle, 1.0);
            Cv2.WarpAffine(refNormalImageArea, rotationImage, angleMat, refNormalImageArea.Size());

            //get the area using rotated image
            Mat imgArea = rotationImage.Clone();
            Mat tmpDisplay = new Mat();

            if (imgArea.Channels() == 3)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgrToGray);
            else if (imgArea.Channels() == 4)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgraToGray);

            Cv2.Threshold(imgArea, tmpDisplay, 50, 255, ThresholdType.Binary);
            hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(tmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            imgArea.Dispose();
            tmpDisplay.Dispose();
            CvBox2D MinAreaRect = new CvBox2D();

            Mat gray = rotationImage.Clone();
            Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea > DataLcdArea.JudgeDisplayAreaLL)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }

            CvScalar clr;
            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                MessageBox.Show("fail to get the area rect");
            }

            m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX, m_displayY, m_displayWidth, m_displayHeight);
            Mat resultImg = rotationImage.Clone();
            if (resultImg.Channels() == 3)
                Cv2.CvtColor(resultImg, resultImg, ColorConversion.BgrToGray);
            else if (resultImg.Channels() == 4)
                Cv2.CvtColor(resultImg, resultImg, ColorConversion.BgraToGray);

            Bitmap b = new Mat(resultImg, displayAreaRect).ToBitmap();
            //b.Save(@".\Detail\greenavg.jpg");
            int sum_g = 0;
            for (int j = 0; j < displayAreaRect.Height; j++)
            {
                for (int i = 0; i < displayAreaRect.Width; i++)
                {
                    sum_g += b.GetPixel(i, j).G;
                }
            }
            double avg = sum_g / (displayAreaRect.Width * displayAreaRect.Height);
            //Console.WriteLine(avg);

            return avg;
        }
        public bool ShowArea(Bitmap bmprefNormalImageArea, TestSpec testSpec, ref Bitmap resultImage,
            ref Bitmap MenuKey, ref Bitmap BackKey, ref Bitmap Led)
        {
            Mat refNormalImageArea = bmprefNormalImageArea.ToMat();
            Mat origianlImg = refNormalImageArea.Clone();


            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);
            else if (origianlImg.Channels() == 4)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgraToGray);

            //threshold to make displayArea clearly
            Mat displayArea = new Mat();
            Cv2.Threshold(origianlImg, displayArea, testSpec.SpecLcdArea.DisplayThreshold, 255, ThresholdType.Binary);
            origianlImg.Dispose();

            //get contours
            Mat[] contours;
            var hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

            double dArea;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                // Log.AddLog(string.Format("{0} countour area : {1}", i, dArea));
                if (dArea > testSpec.SpecLcdArea.JudgeDisplayAreaLL)
                    //get the rect from the display sector  
                    rRect = Cv2.MinAreaRect(contours[i]);
            }
            displayArea.Release();

            rotationImage = refNormalImageArea.Clone();

            //rotate the image on tilted angle
            CvPoint2D32f center = new CvPoint2D32f(refNormalImageArea.Cols / 2, refNormalImageArea.Rows / 2);
            angleMat = Cv2.GetRotationMatrix2D(center, rRect.Angle < -45 ? rRect.Angle + 90 : rRect.Angle, 1.0);
            Cv2.WarpAffine(refNormalImageArea, rotationImage, angleMat, refNormalImageArea.Size());

            //get the area using rotated image
            Mat imgArea = rotationImage.Clone();
            Mat tmpDisplay = new Mat();

            if (imgArea.Channels() == 3)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgrToGray);
            else if (imgArea.Channels() == 4)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgraToGray);

            Cv2.Threshold(imgArea, tmpDisplay, testSpec.SpecLcdArea.DisplayThreshold, 255, ThresholdType.Binary);
            hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(tmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            imgArea.Dispose();
            tmpDisplay.Dispose();
            CvBox2D MinAreaRect = new CvBox2D();

            Mat gray = rotationImage.Clone();
            Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea > testSpec.SpecLcdArea.JudgeDisplayAreaLL)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }


            CvScalar clr;
            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                Mat tempImg = rotationImage.Clone();

                if (tempImg.Channels() == 1)
                    Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);

                clr = new CvScalar(0, 0, 255);
                Cv2.Rectangle(tempImg, new CvRect(4, 4, tempImg.Cols - 4 * 2, tempImg.Rows - 4 * 2), clr, 20);

                tempImg.Dispose();

                return false;
            }

            m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX - 10, m_displayY - 10, m_displayWidth + 20, m_displayHeight + 20);
            Mat resultImg = rotationImage.Clone();
            clr = new CvScalar(0, 255, 0);

            if (testSpec.SpecTestList.TestFrontKey == true)
            {
                menuAreaRect = new CvRect(m_displayX + m_displayWidth + testSpec.SpecLcdArea.MenuRoughRoi.left, m_displayY + testSpec.SpecLcdArea.MenuRoughRoi.top,
                    testSpec.SpecLcdArea.MenuRoughRoi.right, testSpec.SpecLcdArea.MenuRoughRoi.bottom);

                backAreaRect = new CvRect(m_displayX + m_displayWidth + testSpec.SpecLcdArea.BackRoughRoi.left, m_displayY + testSpec.SpecLcdArea.BackRoughRoi.top,
                    testSpec.SpecLcdArea.BackRoughRoi.right, testSpec.SpecLcdArea.BackRoughRoi.bottom);

                try
                {
                    Mat tmpMenu = resultImg[menuAreaRect].Clone();
                    Mat tmpBack = resultImg[backAreaRect].Clone();

                    if (tmpMenu.Channels() == 3)
                        Cv2.CvtColor(tmpMenu, tmpMenu, ColorConversion.BgrToGray);
                    if (tmpBack.Channels() == 3)
                        Cv2.CvtColor(tmpBack, tmpBack, ColorConversion.BgrToGray);

                    Cv2.Threshold(tmpMenu, tmpMenu, testSpec.SpecLcdArea.KeyThreshold, 255.0, ThresholdType.Binary);
                    Cv2.Threshold(tmpBack, tmpBack, testSpec.SpecLcdArea.KeyThreshold, 255.0, ThresholdType.Binary);

                    Cv2.Erode(tmpMenu, tmpMenu, new Mat(), new CvPoint(-1, -1), 2);
                    Cv2.Erode(tmpBack, tmpBack, new Mat(), new CvPoint(-1, -1), 2);

                    MenuKey = tmpMenu.ToBitmap();
                    BackKey = tmpBack.ToBitmap();
                    Cv2.Rectangle(resultImg, new CvRect(backAreaRect.X - 2, backAreaRect.Y - 2, backAreaRect.Width + 4, backAreaRect.Height + 4), clr, 4);
                    Cv2.Rectangle(resultImg, new CvRect(menuAreaRect.X - 2, menuAreaRect.Y - 2, menuAreaRect.Width + 4, menuAreaRect.Height + 4), clr, 4);
                }
                catch
                {
                    System.Windows.MessageBox.Show("out area");
                }


            }

            if (testSpec.SpecTestList.TestFrontLed == true)
            {

                ledAreaRect = new CvRect(m_displayX + testSpec.SpecLcdArea.LedRoughRoi.left, m_displayY + testSpec.SpecLcdArea.LedRoughRoi.top,
               testSpec.SpecLcdArea.LedRoughRoi.right, testSpec.SpecLcdArea.LedRoughRoi.bottom);
                try
                {
                    Mat tmpLed = resultImg[ledAreaRect].Clone();

                    if (tmpLed.Channels() == 3)
                        Cv2.CvtColor(tmpLed, tmpLed, ColorConversion.BgrToGray);

                    Cv2.Threshold(tmpLed, tmpLed, testSpec.SpecLcdArea.LedThreshold, 255.0, ThresholdType.Binary);
                    Cv2.Erode(tmpLed, tmpLed, new Mat(), new CvPoint(-1, -1), 3);
                    Led = tmpLed.ToBitmap();
                    Cv2.Rectangle(resultImg, new CvRect(ledAreaRect.X - 2, ledAreaRect.Y - 2, ledAreaRect.Width + 4, ledAreaRect.Height + 4), clr, 4);
                }
				catch (Exception ex)
				{
					Log.AddLog(ex.ToString());
					Log.AddPmLog(ex.ToString());
					MessageBox.Show("out area");
                }
            }

            Cv2.Rectangle(resultImg, new CvRect(displayAreaRect.X - 2, displayAreaRect.Y - 2, displayAreaRect.Width + 4, displayAreaRect.Height + 4), clr, 4);
            resultImage = resultImg.ToBitmap();
            return true;
        }

        public bool AreaProcess(Mat refNormalImageArea, TestSpec testSpec, ref MResult_LCD_AREA Result)
        {
            Mat origianlImg = refNormalImageArea.Clone();

            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);
            else if (origianlImg.Channels() == 4)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgraToGray);

            //threshold to make displayArea clearly
            Mat displayArea = new Mat();
            Cv2.Threshold(origianlImg, displayArea, testSpec.SpecLcdArea.DisplayThreshold, 255, ThresholdType.Binary);
            //Cv2.Threshold(origianlImg, displayArea, 200, 255, ThresholdType.Binary);
            if (Config.SaveDebugImage == true)
                origianlImg.SaveImage(@".\Detail\Area\00.origianlImg.jpg");

            origianlImg.Release();

            //get contours
            Mat[] contours;
            var hierarchy = OutputArray.Create(new List<Vec4i>());
            //Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            rotationImage = refNormalImageArea.Clone();
            Cv2.FindContours(displayArea, out contours, hierarchy, ContourRetrieval.CComp, ContourChain.ApproxNone);
            double dArea;

            //BT OFF 글자 or idel화면 배출
            if (contours.Length > testSpec.SpecLcdArea.JudgeErrorCount)
            {
                Mat tempImg = rotationImage.Clone();

                if (tempImg.Channels() == 1)
                    Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);

                CvScalar clr = new CvScalar(0, 0, 255);
                Cv2.Rectangle(tempImg, new CvRect(4, 4, tempImg.Cols - 4 * 2, tempImg.Rows - 4 * 2), clr, 20);

                Result.ImageResult = tempImg.Clone();
                tempImg.Dispose();
                displayArea.Release();
                return false;
            }

            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea > testSpec.SpecLcdArea.JudgeDisplayAreaLL)
                { 
                    rRect = Cv2.MinAreaRect(contours[i]);
                  
                    //toast 메세지 있으면 배출
                    m_displayWidth = (int)(rRect.Size.Width > rRect.Size.Height ? rRect.Size.Width : rRect.Size.Height);
                    m_displayHeight = (int)(rRect.Size.Height < rRect.Size.Width ? rRect.Size.Height : rRect.Size.Width);
                    m_displayX = (int)rRect.Center.X - (m_displayWidth / 2);
                    m_displayY = (int)rRect.Center.Y - (m_displayHeight / 2);
                    displayAreaRect = new CvRect(m_displayX+ m_displayWidth*2/3+300, m_displayY+300, m_displayWidth*1/3-600, m_displayHeight-600);
                    
                    Mat tmp = rotationImage[displayAreaRect];
                    Cv2.CvtColor(tmp, tmp, ColorConversion.BgrToGray);
                  
                    if (Config.SaveDebugImage == true)
                        tmp.SaveImage(@".\Detail\Area\001.tmp.jpg");

                    Mat mask = Mat.Zeros(tmp.Size(), MatType.CV_8UC1);
                    Cv2.Rectangle(mask,new CvRect(0,0,mask.Width,mask.Height), Cv.ScalarAll(255), -1);
                    if (Config.SaveDebugImage == true)
                        mask.SaveImage(@".\Detail\Area\002.mask.jpg");
                    double minVal;
                    minVal = Cv2.Mean(tmp, mask).Val0;

                    if(minVal<testSpec.SpecLcdArea.JudgeErrorBright)
                    {
                        Mat tempImg = rotationImage.Clone();

                        if (tempImg.Channels() == 1)
                            Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);

                        CvScalar clr = new CvScalar(0, 0, 255);
                        Cv2.Rectangle(tempImg, new CvRect(4, 4, tempImg.Cols - 4 * 2, tempImg.Rows - 4 * 2), clr, 20);

                        Result.ImageResult = tempImg.Clone();
                        tempImg.Dispose();
                        displayArea.Release();
                        return false;
                    }

                }

               

            }
            displayArea.Release();
            


            rotationImage = refNormalImageArea.Clone();

            Cv2.PutText(rotationImage, (rRect.Angle) < -45 ? ((rRect.Angle) + 90).ToString() : rRect.Angle.ToString(), new OpenCvSharp.CPlusPlus.Point(500, 500), FontFace.HersheyTriplex, 3,
                new CvScalar(0, 0, 255), 8);

            if (Config.SaveDebugImage == true)
                rotationImage.SaveImage(@".\Detail\Area\1_displayangle.jpg");


            //rotate the image on tilted angle
            CvPoint2D32f center = new CvPoint2D32f(refNormalImageArea.Cols / 2, refNormalImageArea.Rows / 2);
            angleMat = Cv2.GetRotationMatrix2D(center, rRect.Angle < -45 ? rRect.Angle + 90 : rRect.Angle, 1.0);
            Cv2.WarpAffine(refNormalImageArea, rotationImage, angleMat, refNormalImageArea.Size());

            if (Config.SaveDebugImage == true)
                rotationImage.SaveImage(@".\Detail\Area\2.rotated.jpg");



            //get the testArea using rotated image

            Mat imgArea = rotationImage.Clone();
            Mat tmpDisplay = new Mat();

            if (imgArea.Channels() == 3)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgrToGray);
            else if (imgArea.Channels() == 4)
                Cv2.CvtColor(imgArea, imgArea, ColorConversion.BgraToGray);

            Cv2.Threshold(imgArea, tmpDisplay, testSpec.SpecLcdArea.DisplayThreshold, 255, ThresholdType.Binary);

            if (Config.SaveDebugImage == true)
                tmpDisplay.SaveImage(@".\Detail\Area\3.tmpDisplay.jpg");

            hierarchy = OutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(tmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            tmpDisplay.Dispose();
            CvBox2D MinAreaRect = new CvBox2D();

            Mat gray = rotationImage.Clone();
            Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                // Log.AddLog(string.Format("contour{0} Area : {1}", i, dArea));
                if (dArea > testSpec.SpecLcdArea.JudgeDisplayAreaLL)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }

            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                Mat tempImg = rotationImage.Clone();

                if (tempImg.Channels() == 1)
                    Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);

                CvScalar clr = new CvScalar(0, 0, 255);
                Cv2.Rectangle(tempImg, new CvRect(4, 4, tempImg.Cols - 4 * 2, tempImg.Rows - 4 * 2), clr, 20);

				Result.ImageResult = tempImg.Clone();
                tempImg.Dispose();

                return false;
            }

            m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX - 10, m_displayY - 10, m_displayWidth + 20, m_displayHeight + 20);
            ledAreaRect = new CvRect(m_displayX + testSpec.SpecLcdArea.LedRoughRoi.left, m_displayY + testSpec.SpecLcdArea.LedRoughRoi.top,
             testSpec.SpecLcdArea.LedRoughRoi.right, testSpec.SpecLcdArea.LedRoughRoi.bottom);
            ResultImg[(int)BMP.CAMERA_DATA_AREA] = imgArea[displayAreaRect].Clone();
            

            hierarchy = OutputArray.Create(new List<Vec4i>());

            //170416
            Mat tmptmpDisplay = ResultImg[(int)BMP.CAMERA_DATA_AREA].Clone();
            Cv2.FindContours(tmptmpDisplay, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea = Cv2.ContourArea(contours[i]);
                if (dArea < 5000000)
                {
                    Cv2.DrawContours(ResultImg[(int)BMP.CAMERA_DATA_AREA], contours, i, Cv.ScalarAll(255), -1);
                }
            }
            tmptmpDisplay.Dispose();

            if (Config.SaveDebugImage == true)
                ResultImg[(int)BMP.CAMERA_DATA_AREA].SaveImage(@".\Detail\Area\4.Display.jpg");

            try
            {

                if (testSpec.SpecTestList.TestFrontKey == true)
                {
                    menuAreaRect = new CvRect(m_displayX + +m_displayWidth + testSpec.SpecLcdArea.MenuRoughRoi.left, m_displayY + testSpec.SpecLcdArea.MenuRoughRoi.top,
                        testSpec.SpecLcdArea.MenuRoughRoi.right, testSpec.SpecLcdArea.MenuRoughRoi.bottom);
                    backAreaRect = new CvRect(m_displayX + +m_displayWidth + testSpec.SpecLcdArea.BackRoughRoi.left, m_displayY + testSpec.SpecLcdArea.BackRoughRoi.top,
                        testSpec.SpecLcdArea.BackRoughRoi.right, testSpec.SpecLcdArea.BackRoughRoi.bottom);

                    ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU] = imgArea[menuAreaRect].Clone();
                    ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK] = imgArea[backAreaRect].Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU].SaveImage(@".\Detail\Area\5.White_Menu_Key_Reference.jpg");
                        ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK].SaveImage(@".\Detail\Area\6.White_Back_Key_Reference.jpg");
                    }
                }
                if (testSpec.SpecTestList.TestFrontLed == true)
                {
                    ledAreaRect = new CvRect(m_displayX + testSpec.SpecLcdArea.LedRoughRoi.left, m_displayY + testSpec.SpecLcdArea.LedRoughRoi.top,
                        testSpec.SpecLcdArea.LedRoughRoi.right, testSpec.SpecLcdArea.LedRoughRoi.bottom);
                    //ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE] = imgArea[ledAreaRect].Clone();
                    //if ( Config.SaveDebugImage ) 
					//    ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE].SaveImage(@".\Detail\Area\7.White_LED_Reference.jpg");
                }

            }
            catch
            {
                Mat tempImg = rotationImage.Clone();

                if (tempImg.Channels() == 1)
                    Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);

                CvScalar clr = new CvScalar(0, 0, 255);
                Cv2.Rectangle(tempImg, new CvRect(4, 4, tempImg.Cols - 4 * 2, tempImg.Rows - 4 * 2), clr, 20);

				Result.ImageResult = tempImg.Clone();
				tempImg.Dispose();

                imgArea.Dispose();
                return false;
            }
            imgArea.Dispose();
            Result.ImageResult = rotationImage.Clone();
            return true;
        }

        public bool DustProcess(Mat refNormatImageDust, TestSpec testSpec,
            ref MResult_LCD_AREA Result,
            ref MResult_LCD_DUST ResultDust)
        {
            if (!LcdDust(refNormatImageDust, testSpec.SpecLcdDust, ref ResultDust))
            {
                return false;
            }

            ///////////////////////////////////////////////////
            {
                Mat tmpDisplay;
                tmpDisplay = ResultImg[(int)BMP.CAMERA_DATA_AREA];

                //White Display 에서 각 영역 Threshold 적용
                Cv2.Threshold(tmpDisplay, tmpDisplay, testSpec.SpecLcdArea.DisplayThreshold, 255.0, ThresholdType.Binary);
                Mat imgTemp = tmpDisplay.Clone();

                if (Config.SaveDebugImage == true)
                    imgTemp.SaveImage(@".\Detail\Area\8.Display_threshold.jpg");

                var hierarchy = OutputArray.Create(new List<Vec4i>());
                Mat[] contours;
                Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                double iDisplayAreaMax = 0;
                double iLedAreaMax = 0;
                double iBackAreaMax = 0;
                double iMenuAreaMax = 0;

                CvRect rect = new CvRect();
                double dArea = 0;

                // Display 영역 검사
                for (int i = 0; i < contours.Length; i++)
                {
                    rect = Cv2.BoundingRect(contours[i]);
                    dArea = Cv2.ContourArea(contours[i]);

                    if (dArea > testSpec.SpecLcdArea.JudgeDisplayAreaLL)
                    {
                        if (iDisplayAreaMax < dArea)
                        {
                            iDisplayAreaMax = dArea;
                        }
                        Result.m_iDisplayArea += dArea; // m_iDisplayArea += dArea;
                    }
                }

                if (testSpec.SpecTestList.TestFrontKey == true)
                {
                    Mat tmpMenu;
                    Mat tmpBack;

                    tmpMenu = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU];
                    tmpBack = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK];

                    Cv2.Threshold(tmpMenu, tmpMenu, testSpec.SpecLcdArea.KeyThreshold, 255.0, ThresholdType.Binary);
                    Cv2.Threshold(tmpBack, tmpBack, testSpec.SpecLcdArea.KeyThreshold, 255.0, ThresholdType.Binary);

                    //Menu key Area 검사
                    imgTemp = tmpMenu.Clone();
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    for (int i = 0; i < contours.Length; i++)
                    {
                        dArea = Cv2.ContourArea(contours[i]);

                        if (dArea > testSpec.SpecLcdArea.JudgeMenuAreaLL)
                        {
                            if (iMenuAreaMax < dArea)
                            {
                                iMenuAreaMax = dArea;
                            }

                            Result.m_iMenuArea += dArea; // m_iMenuArea += dArea;
                        }
                    }

                    //Back key Area 검사
                    imgTemp = tmpBack.Clone();
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    for (int i = 0; i < contours.Length; i++)
                    {
                        dArea = Cv2.ContourArea(contours[i]);

                        if (dArea > testSpec.SpecLcdArea.JudgeBackAreaLL)
                        {
                            if (iBackAreaMax < dArea)
                            {
                                iBackAreaMax = dArea;
                            }
                            Result.m_iBackArea += dArea;// m_iBackArea += dArea;

                        }
                    }

                    Cv2.Erode(tmpMenu, tmpMenu, new Mat(), new CvPoint(-1, -1), 2);
                    Cv2.Erode(tmpBack, tmpBack, new Mat(), new CvPoint(-1, -1), 2);
                    ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU] = tmpMenu.Clone();
                    ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK] = tmpBack.Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU].SaveImage(@".\Detail\Area\10.touch_menu_mask.jpg");
                        ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK].SaveImage(@".\Detail\Area\11.touch_back_mask.jpg");
                    }

                }

                if (testSpec.SpecTestList.TestFrontLed == true)
                {
                    //Mat tmpLed;
                    //tmpLed = ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE];
                    //Cv2.Threshold(tmpLed, tmpLed, testSpec.SpecLcdArea.LedThreshold, 255.0, ThresholdType.Binary);

                    //LED key Area 검사
                    //imgTemp = tmpLed.Clone();
                    //hierarchy = OutputArray.Create(new List<Vec4i>());
                    //Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //LED FAIL
                    //if (contours.Length > 1)
                    //    Result.m_iLedArea += 100000;

                    //for (int i = 0; i < contours.Length; i++)
                    //{
                    //    dArea = Cv2.ContourArea(contours[i]);
                    //    if (dArea > testSpec.SpecLcdArea.m_dJudgeLedAreaLL)
                    //    {
                    //        if (iLedAreaMax < dArea)
                    //        {
                    //            iLedAreaMax = dArea;
                    //        }
                    //    }
                    //    Result.m_iLedArea += dArea; // m_iLedArea += dArea;
                    //}

                    //Cv2.Erode(tmpLed, tmpLed, new Mat(), new CvPoint(-1, -1), 3);

                    //ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE] = tmpLed.Clone();

                    //if (Config.SaveDebugImage == true)
                    //    ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE].SaveImage(@".\Detail\Area\12.led_mask.jpg");
                }

                //외각선 3픽셀 정도 offset
                Mat contoursImg = ResultImg[(int)BMP.CAMERA_DATA_AREA].Clone();
                Mat[] removeContours;
                hierarchy = OutputArray.Create(new List<Vec4i>());
                Cv2.FindContours(contoursImg, out removeContours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                //white 이미지에서 dust 영역 제외한 마스크
                ResultImg[(int)BMP.CAMERA_DATA_DIMMING] = ResultImg[(int)BMP.CAMERA_DATA_SOMETHING][displayAreaRect].Clone();
               
                Cv2.BitwiseAnd(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], ResultImg[(int)BMP.CAMERA_DATA_AREA],
                    ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);
                if (Config.SaveDebugImage == true)
                {
                    ResultImg[(int)BMP.CAMERA_DATA_DIMMING].SaveImage(@".\Detail\Area\10.display_mask_beforeoffset.jpg");
                }
                ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN] = ResultImg[(int)BMP.CAMERA_DATA_DIMMING].Clone();

                for (int i = 0; i < removeContours.Length; i++)
                {
                    if (Cv2.ContourArea(removeContours[i]) > 10000 && testSpec.SpecLcdArea.SideOffsetPixel > 0)
                    {
                        Cv2.DrawContours(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], removeContours, i, Cv.ScalarAll(0),
                            testSpec.SpecLcdArea.SideOffsetPixel);
                        Cv2.DrawContours(ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN], removeContours, i, Cv.ScalarAll(0),
                            testSpec.SpecLcdArea.SideOffsetPixel_Green);
                    }
                }
                if (Config.SaveDebugImage == true)
                {
                    ResultImg[(int)BMP.CAMERA_DATA_AREA].SaveImage(@".\Detail\Area\9.data_area.jpg");
                    ResultImg[(int)BMP.CAMERA_DATA_DIMMING].SaveImage(@".\Detail\Area\13.display_mask.jpg");
					ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN].SaveImage(@".\Detail\Area\14.display_mask_green.jpg");
                }
            }

            Mat resultImg = rotationImage.Clone();
            bool bFinalResult = true;
            //////////////////////////////////////////////////////////

            if (testSpec.SpecTestList.TestFrontKey == true)
            {
                {
                    bool bIsFault = false;

                    if (resultImg.Channels() == 1)
                        Cv2.CvtColor(resultImg, resultImg, ColorConversion.GrayToBgr);


                    if (Result.m_iBackArea < testSpec.SpecLcdArea.JudgeBackAreaLL || Result.m_iBackArea > testSpec.SpecLcdArea.JudgeBackAreaUL)
                        bIsFault = true;
                    else
                        bIsFault = false;

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(backAreaRect.X - 2, backAreaRect.Y - 2, backAreaRect.Width + 4, backAreaRect.Height + 4), clr, 4);


                    if (bIsFault) bFinalResult = false;

                }
                /////////////////////////////////////////////////////////
                {
                    if (resultImg.Channels() == 1)
                        Cv2.CvtColor(resultImg, resultImg, ColorConversion.GrayToBgr);

                    bool bIsFault = false;

                    CvScalar clr;
                    if (Result.m_iMenuArea < testSpec.SpecLcdArea.JudgeMenuAreaLL || Result.m_iMenuArea > testSpec.SpecLcdArea.JudgeMenuAreaUL)
                        bIsFault = true;
                    else
                        bIsFault = false;

                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(menuAreaRect.X - 2, menuAreaRect.Y - 2, menuAreaRect.Width + 4, menuAreaRect.Height + 4), clr, 4);

                    if (bIsFault) bFinalResult = false;
                }
            }
            ////////////////////////////////////////////////////////////

            {
                if (resultImg.Channels() == 1)
                    Cv2.CvtColor(resultImg, resultImg, ColorConversion.GrayToBgr);

                bool bIsFault = false;

                if (Result.m_iDisplayArea < testSpec.SpecLcdArea.JudgeDisplayAreaLL || Result.m_iDisplayArea > testSpec.SpecLcdArea.JudgeDisplayAreaUL)
                    bIsFault = true;
                else
                    bIsFault = false;

                CvScalar clr;

                if (bIsFault)
                    clr = new CvScalar(0, 0, 255);
                else
                    clr = new CvScalar(0, 255, 0);

                Cv2.Rectangle(resultImg, new CvRect(displayAreaRect.X - 2, displayAreaRect.Y - 2, displayAreaRect.Width + 4, displayAreaRect.Height + 4), clr, 4);

                if (bIsFault) bFinalResult = false;
            }

            ////////////////////////////////////////////////////////////
            //if (testSpec.SpecTestList.TestFrontLed == true)
            //{
            //    if (resultImg.Channels() == 1)
            //        Cv2.CvtColor(resultImg, resultImg, ColorConversion.GrayToBgr);

            //    bool bIsFault = false;

            //    if (Result.m_iLedArea < testSpec.SpecLcdArea.m_dJudgeLedAreaLL || Result.m_iLedArea > testSpec.SpecLcdArea.m_dJudgeLedAreaUL)
            //        bIsFault = true;
            //    else
            //        bIsFault = false;

            //    CvScalar clr;
            //    if (bIsFault)
            //        clr = new CvScalar(0, 0, 255);
            //    else
            //        clr = new CvScalar(0, 255, 0);

            //    Cv2.Rectangle(resultImg, new CvRect(ledAreaRect.X - 2, ledAreaRect.Y - 2, ledAreaRect.Width + 4, ledAreaRect.Height + 4), clr, 4);

            //    if (bIsFault) bFinalResult = false;
            //}

            {
                CvScalar clr;
                if (!bFinalResult)
                    clr = new CvScalar(0, 0, 255);
                else
                    clr = new CvScalar(0, 255, 0);

                Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);

                if (Config.SaveDebugImage == true)
                {
                    resultImg.SaveImage(@".\Detail\Area\14.area_result.jpg");
                }

                Result.ImageResult = resultImg.Clone();
            }
            resultImg.Dispose();

            return true;
        }

        public bool LcdDust(Mat refNormalImage, MData_LCD_DUST DataLcdDust, ref MResult_LCD_DUST ResultDust)
        {
            using (Mat m_DustImage = refNormalImage.Clone())
            {

                //rotate the image on tilted angle
                Cv2.WarpAffine(refNormalImage, m_DustImage, angleMat, refNormalImage.Size());
                if (Config.SaveDebugImage == true)
                    m_DustImage.SaveImage(@".\Detail\Dust\2.Rotation.jpg");

                //get the displayarea using rotated image

                Mat m_SomethingImg = m_DustImage[displayAreaRect].Clone();

                if (m_SomethingImg.Channels() == 3)
                    Cv2.CvtColor(m_SomethingImg, m_SomethingImg, ColorConversion.BgrToGray);
                else if (m_SomethingImg.Channels() == 4)
                    Cv2.CvtColor(m_SomethingImg, m_SomethingImg, ColorConversion.BgraToGray);
                if (Config.SaveDebugImage == true)
                    m_SomethingImg.SaveImage(@".\Detail\Dust\3.dust_display.jpg");


                //threshold to make displayArea clearly

                Cv2.Threshold(m_SomethingImg, m_SomethingImg, DataLcdDust.DustThreshold, 255, ThresholdType.BinaryInv);
                if (Config.SaveDebugImage == true)
                    m_SomethingImg.SaveImage(@".\Detail\Dust\3.dust_displaythreshold.jpg");


                Mat[] contours;
                Mat[] countoursdustarea;
                var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                var hierarchy1 = InputOutputArray.Create(new List<Vec4i>());
                Mat imgTemp = m_SomethingImg.Clone();
                Mat imgTempdustarea = m_SomethingImg.Clone();
                //Cv2.Line(imgTemp, new OpenCvSharp.CPlusPlus.Point(0, 0), new OpenCvSharp.CPlusPlus.Point(0, imgTemp.Height),
                // new Scalar(255, 255, 255), 2, LineType.AntiAlias);
                //Cv2.Line(imgTemp, new OpenCvSharp.CPlusPlus.Point(imgTemp.Width - 2, 0), new OpenCvSharp.CPlusPlus.Point(imgTemp.Width - 2, imgTemp.Height),
                // new Scalar(255, 255, 255), 2, LineType.AntiAlias);
                //for (int i = 0; i < imgTemp.Height; i++)
                //{
                //    imgTemp.Set<byte>(i, 0, 255);
                //    imgTemp.Set<byte>(i, 1, 255);
                //    imgTemp.Set<byte>(i, imgTemp.Width - 1, 255);
                //    imgTemp.Set<byte>(i, imgTemp.Width - 2, 255);
                //}
                Cv2.Line(imgTempdustarea, new OpenCvSharp.CPlusPlus.Point(0, 0), new OpenCvSharp.CPlusPlus.Point(0, imgTempdustarea.Height),
                    new Scalar(255, 255, 255), 2, LineType.AntiAlias);
                Cv2.Line(imgTempdustarea, new OpenCvSharp.CPlusPlus.Point(imgTempdustarea.Width - 2, 0), new OpenCvSharp.CPlusPlus.Point(imgTempdustarea.Width - 2, imgTempdustarea.Height),
                     new Scalar(255, 255, 255), 2, LineType.AntiAlias);
                Cv2.Line(imgTempdustarea, new OpenCvSharp.CPlusPlus.Point(0, 0), new OpenCvSharp.CPlusPlus.Point(0, imgTempdustarea.Width), new Scalar(255, 255, 255), 2, LineType.AntiAlias);
                Cv2.Line(imgTempdustarea, new OpenCvSharp.CPlusPlus.Point(0, imgTempdustarea.Height - 2), new OpenCvSharp.CPlusPlus.Point(imgTempdustarea.Width - 2, imgTempdustarea.Height), new Scalar(255, 255, 255), 2, LineType.AntiAlias);

                if (Config.SaveDebugImage == true)
                    imgTemp.SaveImage(@".\Detail\Dust\4.beforcontour.jpg");




                Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                imgTempdustarea = ~(imgTempdustarea.Clone());
                if (Config.SaveDebugImage == true)
                    imgTempdustarea.SaveImage(@".\Detail\Dust\5.dust_area.jpg");


                Cv2.FindContours(imgTempdustarea, out countoursdustarea, hierarchy1, ContourRetrieval.External, ContourChain.ApproxNone);
                double dArea = 0.0;
                double dMaxArea = 0.0;
                int iMax = 0;
                Mat mMax = null;
                // Finding Dust Area Mask
                for (int i = 0; i < contours.Length; i++)
                {
                    dArea = Cv2.ContourArea(contours[i]);
                    if (dArea > dMaxArea)
                    {
                        dMaxArea = dArea;
                        mMax = contours[i];
                        iMax = i;
                    }
                }

                Mat mask = Mat.Zeros(displayAreaRect.Size, MatType.CV_8UC1);
                Cv2.DrawContours(mask, contours, iMax, Cv.ScalarAll(255), -1);
                if (Config.SaveDebugImage == true)
                    mask.SaveImage(@".\Detail\Dust\4.maskbefore.jpg");


                Cv2.Erode(mask, mask, new Mat(), new CvPoint(-1, -1), 4);
                if (Config.SaveDebugImage == true)
                    mask.SaveImage(@".\Detail\Dust\4.mask.jpg");


                Cv2.BitwiseAnd(m_SomethingImg, mask, m_SomethingImg);
                imgTemp = ~(m_SomethingImg.Clone());
                Cv2.BitwiseAnd(imgTemp, mask, imgTemp);
                if (Config.SaveDebugImage == true)
                    imgTemp.SaveImage(@".\Detail\Dust\5.dust_areamask.jpg");


                /*
                Mat mask1 = Mat.Zeros(displayAreaRect.Size, MatType.CV_8UC1);
                Cv2.DrawContours(mask1, countoursdustarea, iMax, Cv.ScalarAll(255), -1);
#if SAVEIMG
                mask1.SaveImage(@".\Detail\Dust\4.mask1before.jpg");
#endif
                Cv2.Erode(mask1, mask1, new Mat(), new CvPoint(-1, -1), 4);
#if SAVEIMG
                mask1.SaveImage(@".\Detail\Dust\4.mask1.jpg");
#endif
                Cv2.BitwiseAnd(imgTempdustarea, mask1, imgTempdustarea);
                imgTempdustarea = ~(imgTempdustarea.Clone());
                Cv2.BitwiseAnd(imgTempdustarea, mask1, imgTempdustarea);
#if SAVEIMG
                imgTempdustarea.SaveImage(@".\Detail\Dust\5.dust_area.jpg");
#endif
                hierarchy1 = InputOutputArray.Create(new List<Vec4i>());
                Cv2.FindContours(imgTempdustarea, out countoursdustarea, hierarchy1, ContourRetrieval.External, ContourChain.ApproxNone);
                dArea = 0.0;
                for (int i = 0; i < countoursdustarea.Length; i++)
                {
                    dArea += Cv2.ContourArea(countoursdustarea[i]);
                    ResultDust.m_dArea = dArea;
                }
                */
                Mat dustAreaMask = imgTemp.Clone();
                hierarchy = InputOutputArray.Create(new List<Vec4i>());
                //Cv2.FindContours(dustAreaMask, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                Cv2.FindContours(imgTemp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                // Finding Dust Area Count
                dArea = 0.0;

                ResultDust.m_dContour = countoursdustarea.Length;

                if (countoursdustarea.Length < DataLcdDust.JudgeDustMaxNumUL)
                {
                    for (int i = 0; i < countoursdustarea.Length; i++)
                    {
                        dArea += Cv2.ContourArea(countoursdustarea[i]);
                        ResultDust.m_dArea = dArea;
                        if (dArea > DataLcdDust.JudgeDustAreaUL)
                            break;
                    }
                    for (int i = 0; i < contours.Length; i++)
                    {
                        if (dArea > DataLcdDust.JudgeDustAreaUL)
                            break;
                        Cv2.DrawContours(dustAreaMask, contours, i, Cv.ScalarAll(255), DataLcdDust.DustOffsetPixel);
                    }
                }
                else
                {
                    dArea = -1;
                    ResultDust.m_dArea = -1;
                    Log.AddLog("Contour num is " + countoursdustarea.Length);
                }

                bool bIsFault = false;

                if ((countoursdustarea.Length > DataLcdDust.JudgeDustMaxNumUL) || (dArea > DataLcdDust.JudgeDustAreaUL))
                    bIsFault = true;
                else
                    bIsFault = false;

                if (Config.SaveDebugImage == true)
                    dustAreaMask.SaveImage(@".\Detail\Dust\6.dust_area+offset.jpg");

                imgTemp = ~(dustAreaMask.Clone());
                if (Config.SaveDebugImage == true)
                    imgTemp.SaveImage(@".\Detail\Dust\7.dust_area+offset.jpg");

                Cv2.BitwiseAnd(imgTemp, mask, mask);

                m_SomethingImg = Mat.Zeros(refNormalImage.Size(), MatType.CV_8UC1);
                //170510 기존
                //imgTemp.CopyTo(m_SomethingImg[displayAreaRect]);
                if (Config.SaveDebugImage == true)
                    mask.SaveImage(@".\Detail\Dust\8.dust_mask.jpg");

                mask.CopyTo(m_SomethingImg[displayAreaRect]);
                if (Config.SaveDebugImage == true)
                    m_SomethingImg.SaveImage(@".\Detail\Dust\9.SomethingImg.jpg");

                /////////////////////////////////////////////////////////////

                ResultImg[(int)BMP.CAMERA_DATA_SOMETHING] = m_SomethingImg.Clone();
                //dArea 가 스펙 이상이면 fail 처리
                m_SomethingImg = Mat.Zeros(refNormalImage.Size(), MatType.CV_8UC1);
                mask.CopyTo(m_SomethingImg[displayAreaRect]);

                if (m_SomethingImg.Channels() == 1)
                    Cv2.CvtColor(m_SomethingImg, m_SomethingImg, ColorConversion.GrayToBgr);

                CvScalar clr;
                if (bIsFault)
                    clr = new CvScalar(0, 0, 255);
                else
                    clr = new CvScalar(0, 255, 0);

                string sTemp = string.Format("Dust Measured : {0}, DustAreaUL : {1}", ResultDust.m_dArea, DataLcdDust.JudgeDustAreaUL);
                Cv2.PutText(m_SomethingImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("Dust Contour Count : {0}, DustMaxNumUL : {1}", countoursdustarea.Length, DataLcdDust.JudgeDustMaxNumUL);
                Cv2.PutText(m_SomethingImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("DustThreshold : {0}", DataLcdDust.DustThreshold);
                Cv2.PutText(m_SomethingImg, sTemp, new CvPoint(700, 650), FontFace.HersheyComplex, 2, Scalar.Blue, 2);

                Cv2.Rectangle(m_SomethingImg, new CvRect(4, 4, m_SomethingImg.Cols - 4 * 2, m_SomethingImg.Rows - 4 * 2), clr, 20);
                ResultDust.ImageResult = m_SomethingImg.Clone();
                if (Config.SaveDebugImage == true)
                {
                    m_SomethingImg.SaveImage(@".\Detail\Dust\10result.jpg");
                }

                m_SomethingImg.Dispose();

                if (bIsFault)
                    return false;

            }
            return true;
        }

		public void LcdRed(Mat refNormalImage, MData_LCD_RED DataLcdRed, ref MResult_LCD_RED Result)
		{
			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");

            using (Mat originalImg = refNormalImage.Clone())
            {

                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

                using (Mat m_RedRotationImage = originalImg.Clone())
                using (Mat m_RedDisplayArea = m_RedRotationImage[displayAreaRect].Clone())
                {

                    if (m_RedDisplayArea.Channels() == 3)
                        Cv2.CvtColor(m_RedDisplayArea, m_RedDisplayArea, ColorConversion.BgrToGray);
                    else if (m_RedDisplayArea.Channels() == 4)
                        Cv2.CvtColor(m_RedDisplayArea, m_RedDisplayArea, ColorConversion.BgraToGray);
                    Cv2.BitwiseAnd(m_RedDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_AREA], m_RedDisplayArea);
                    if (Config.SaveDebugImage == true)
                        m_RedDisplayArea.SaveImage(@".\Detail\Lcd Red\2.Display.jpg");

                    Mat tmpImg = new Mat();
                    Mat resultImg = m_RedRotationImage.Clone();

                    Cv2.AdaptiveThreshold(m_RedDisplayArea, tmpImg, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv,
                        DataLcdRed.BlackDot_RFindBlackDotBlockSize, DataLcdRed.BlackDot_RFindBlackDotThreshold);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Red\3.Blackdot_Adaptive.jpg");

                    Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tmpImg);

                    Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);

                    tmpImg.CopyTo(tmptmp[displayAreaRect]);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Red\4.Blackdot_Adaptive_result.jpg");

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    CvRect rect = new CvRect();
                    int iTotalMaxVal = 0;
                    double dArea;
                    int iMax = 0;
                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.m_nBlackDot_JudgeSize < iMax)
                            Result.m_nBlackDot_JudgeSize = iMax;

                        if (iMax > DataLcdRed.BlackDot_RJudgeSizeUL)
                        {

                            bIsFault = true;
                            iTotalMaxVal += (int)dArea;

                            rect.X = rect.X - 4 / 2;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y - 4 / 2;
                            rect.Height = rect.Height + 40;

                            Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                        }
                    }

                    #region no use
                    //                    if (!bIsFault)
                    //                    {
                    //                        Mat resImg = ot.GetPsmImage(m_RedDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Red\5.blackdot_psm_result.jpg");
                    //#else
                    //#endif

                    //                        Cv2.Threshold(resImg, resImg, DataLcdRed.m_nBlackDot_RPsmThreshold, 255, ThresholdType.Binary);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Red\6.blackdot_psm_threshold.jpg");
                    //#else
                    //#endif

                    //                        Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Red\7.blackdot_psm_threshold+area.jpg");
                    //#else
                    //#endif
                    //                        tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //                        resImg.CopyTo(tmptmp[displayAreaRect]);

                    //                        hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    //                        Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    //                        //sh32.heo
                    //                        resImg.Release();
                    //                        tmptmp.Release();
                    //                        //

                    //                        dArea = 0;
                    //                        iTotalMaxVal = 0;

                    //                        for (int i = 0; i < contours.Length; i++)
                    //                        {
                    //                            rect = Cv2.BoundingRect(contours[i]);
                    //                            dArea = Cv2.ContourArea(contours[i]);

                    //                            iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //                            if (Result.m_nBlackDot_JudgeSize < iMax)
                    //                                Result.m_nBlackDot_JudgeSize = iMax;

                    //                            if (iMax > DataLcdRed.BlackDot_RJudgeSizeUL)
                    //                            {
                    //                                bIsFault = true;
                    //                                iTotalMaxVal += (int)dArea;

                    //                                rect.X = rect.X - 4 / 2;
                    //                                rect.Width = rect.Width + 40;
                    //                                rect.Y = rect.Y - 4 / 2;
                    //                                rect.Height = rect.Height + 40;

                    //                                Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //                            }
                    //                        }
                    //                    }
                    #endregion
                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);

                    Result.ImageResult = resultImg.Clone();

                    if (Config.SaveDebugImage == true)
                    {
                        resultImg.SaveImage(@".\Detail\Lcd Red\8.blackdot_result.jpg");
                    }

                    resultImg.Dispose();
                }

                #region X
                //Mat resultImg = m_RedRotationImage.Clone();
                //bool bIsFault = false;

                //Mat resImg = ot.GetPsmImage(m_RedDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                //resImg.SaveImage(@".\Detail\Lcd Red\5.blackdot_psm_result.jpg");

                //Cv2.Threshold(resImg, resImg, DataLcdRed.m_nBlackDot_RPsmThreshold, 255, ThresholdType.Binary);
                //resImg.SaveImage(@".\Detail\Lcd Red\6.blackdot_psm_threshold.jpg");

                //Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                //resImg.SaveImage(@".\Detail\Lcd Red\7.blackdot_psm_threshold+area.jpg");

                //Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                //resImg.CopyTo(tmptmp[displayAreaRect]);

                //var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                //Mat[] contours;

                //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                ////sh32.heo
                //resImg.Release();
                //tmptmp.Release();
                ////

                //double dArea = 0;
                //int iTotalMaxVal = 0;
                //CvRect rect = new CvRect();
                //int iMax = 0;

                //for (int i = 0; i < contours.Length; i++)
                //{
                //    rect = Cv2.BoundingRect(contours[i]);
                //    dArea = Cv2.ContourArea(contours[i]);

                //    iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                //    if (Result.m_nBlackDot_JudgeSize < iMax)
                //        Result.m_nBlackDot_JudgeSize = iMax;

                //    if (iMax > DataLcdRed.BlackDot_RJudgeSizeUL)
                //    {
                //        bIsFault = true;
                //        iTotalMaxVal += (int)dArea;

                //        rect.X = rect.X - 4 / 2;
                //        rect.Width = rect.Width + 40;
                //        rect.Y = rect.Y - 4 / 2;
                //        rect.Height = rect.Height + 40;

                //        Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                //    }
                //}

                //CvScalar clr;
                //if (bIsFault)
                //    clr = new CvScalar(0, 0, 255);
                //else
                //    clr = new CvScalar(0, 255, 0);

                //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //resultImg.SaveImage(@".\Detail\Lcd Red\8.blackdot_result.jpg");

                //if (bIsFault)
                //{
                //    Result.imgNGResult = resultImg.Clone().ToBitmap();
                //    //Dust Mask Area
                //    //불량처리
                //}
                //{
                //    Mat resultImg = m_RedRotationImage.Clone();
                //    CvScalar m_DisplayMean;
                //    m_DisplayMean = Cv2.Mean(m_RedDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);

                //    bool bIsFault = false;

                //    if (m_DisplayMean.Val0 < DataLcdRed.m_nBrightness_RJudgeBrightnessLL
                //        || m_DisplayMean.Val0 > DataLcdRed.m_nBrightness_RJudgeBrightnessUL)
                //        bIsFault = true;

                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd Red\9.brightness_result.jpg");
                //}


                //{
                //    Mat resultImg = m_RedRotationImage.Clone();
                //    CvScalar meanVal;
                //    double totalVal = 0;

                //    //Mat m_RedROIImage = new Mat(@".\Detail\Lcd Red\1.Rotation.jpg");
                //    Cv2.Split(m_RedRotationImage, out red_splitImg);
                //    for (int i = 0; i < MAX_CHANNEL; i++)
                //    {
                //        red_disSplit.Add(new Mat(red_splitImg[i], displayAreaRect));
                //    }
                //    Mat tmp = red_disSplit[RED_CHANNEL] / (red_disSplit[BLUE_CHANNEL] + red_disSplit[GREEN_CHANNEL] + red_disSplit[RED_CHANNEL]) * 100;
                //    tmp.SaveImage(@".\Detail\Lcd Red\10.colorbalance.jpg");

                //    dredDisplayColorBalance = (double)Cv2.Mean(tmp, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;

                //    bool bIsFault = false;

                //    if (dredDisplayColorBalance < (double)DataLcdRed.m_nColorBalance_RJudgeColorBLL ||
                //        dredDisplayColorBalance > (double)DataLcdRed.m_nColorBalance_RJudgeColorBUL)
                //    {
                //        bIsFault = true;
                //    }

                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd Red\11.colorbalance_result.jpg");
                //}

                // diff 검사
                //{
                //    Mat resultImg = m_RedRotationImage.Clone();

                //    CvRect tempRect = new CvRect(5, 5, m_displayWidth - 10, m_displayHeight - 10);
                //    CvRect tempRect2 = new CvRect(m_displayX + 5, m_displayY + 5, m_displayWidth - 10, m_displayHeight - 10);

                //    Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                //    Mat blueImg = new Mat(red_splitImg[BLUE_CHANNEL], tempRect2);
                //    Mat greenImg = new Mat(red_splitImg[GREEN_CHANNEL], tempRect2);
                //    Mat redImg = new Mat(red_splitImg[RED_CHANNEL], tempRect2);

                //    maskROI.SaveImage(@".\Detail\Lcd Red\12.diffcolorarearate_mask.jpg");
                //    blueImg.SaveImage(@".\Detail\Lcd Red\13.diffcolorarearate_blue.jpg");
                //    greenImg.SaveImage(@".\Detail\Lcd Red\14.diffcolorarearate_green.jpg");
                //    redImg.SaveImage(@".\Detail\Lcd Red\15.diffcolorarearate_red.jpg");

                //    double rateValue = 0;
                //    int rowGap = DataLcdRed.m_nDiffColorAreaRate_RWidthSplitCount;
                //    int colGap = DataLcdRed.m_nDiffColorAreaRate_RHeightSplitCount;

                //    bool  bIsFault = false;

                //    Mat cal = redImg / (blueImg + greenImg + redImg) * 100;
                //    cal.SaveImage(@".\Detail\Lcd Red\16.diffcolorarearate_cal_mat.jpg");

                //    //sh.heo
                //    foreach (Mat m in red_splitImg)
                //    {
                //        m.Release();
                //    }
                //    m_RedDisplayArea.Release();
                //    blueImg.Release();
                //    greenImg.Release();
                //    redImg.Release();
                //    //

                //    Mat lowCal = new Mat();
                //    Cv2.Threshold(cal, lowCal, dredDisplayColorBalance - DataLcdRed.m_nDiffColorAreaRate_RJudgePixelRateUL, 255, ThresholdType.BinaryInv);
                //    lowCal.SaveImage(@".\Detail\Lcd Red\17.diffcolorarearate_cal_mat_low_balance.jpg");

                //    Mat highCal = new Mat();
                //    Cv2.Threshold(cal, highCal, dredDisplayColorBalance + DataLcdRed.m_nDiffColorAreaRate_RJudgePixelRateUL, 255, ThresholdType.Binary);
                //    highCal.SaveImage(@".\Detail\Lcd Red\18.diffcolorarearate_cal_mat_high_balance.jpg");

                //    Cv2.BitwiseOr(lowCal, highCal, cal);
                //    cal.SaveImage(@".\Detail\Lcd Red\19.diffcolorarearate_cal_mat_ng_balance.jpg");

                //    Mat tmp = new Mat();
                //    Mat tmpMask = new Mat();
                //    CvScalar avg;

                //    int cGap = 0;
                //    int rGap = 0;

                //    double dPixelsRate = 0;
                //    double dMaxPixelsRate = 0;
                //    bIsFault = false;

                //    for (int a = 0; a < cal.Rows; a = a + rowGap)
                //    {
                //        for (int b = 0; b < cal.Cols; b = b + colGap)
                //        {
                //            cGap = cal.Cols - b < colGap ? cal.Cols - b : colGap;
                //            rGap = cal.Rows - a < rowGap ? cal.Rows - a : rowGap;
                //            tempRect = new CvRect(b, a, cGap, rGap);
                //            tmp = new Mat(cal, tempRect);
                //            tmpMask = new Mat(maskROI, tempRect);

                //            Cv2.BitwiseAnd(tmp, tmpMask, tmp);

                //            var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                //            Mat[] contours;
                //            Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                //            //sh32.heo
                //            tmp.Release();
                //            tmpMask.Release();
                //            //
                //            for (int i = 0; i < contours.Length; i++)
                //            {
                //                dPixelsRate += Cv2.ContourArea(contours[i]);
                //            }
                //            dMaxPixelsRate = dPixelsRate;
                //            dMaxPixelsRate = (dMaxPixelsRate / ((m_displayWidth - 10) * (m_displayHeight - 10))) * 100;

                //            if (dMaxPixelsRate > DataLcdRed.m_nDiffColorAreaRate_RDiffColorJudgePixelRate)
                //            {
                //                bIsFault = true;
                //                Cv2.Rectangle(resultImg, new CvRect(m_displayX + 5 + b, m_displayY + 5 + a, cGap, rGap), new CvScalar(255, 0, 0), 4);
                //            }

                //        }
                //    }

                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);
                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd Red\21.diffcolorarearate_result.jpg");

                //}

                #endregion
            }
        }

       
        public void LcdGreen2(List<Mat> inputImages, MData_LCD_GREEN DataLcdGreen, ref MResult_LCD_GREEN Result)
        {
            Mat accumulatedImage = null;
            Mat resultImg = inputImages[0].Clone();
            Mat srcImage2AdaptiveThres = new Mat();
            for (int i = 0; i < inputImages.Count; i++)
            {
                Mat srcImage = inputImages[i].Clone();
                if (srcImage.Channels() == 3)
                    Cv2.CvtColor(srcImage, srcImage, ColorConversion.BgrToGray);

                // 1. Proc 
                //  srcImage -> Adaptive Proc
                
                Cv2.AdaptiveThreshold(srcImage, srcImage2AdaptiveThres, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv, DataLcdGreen.BlackDot_GFindBlackDotBlockSize, DataLcdGreen.BlackDot_GFindBlackDotThreshold);

                //  get accumulatedImage 
                if (accumulatedImage == null)
                    accumulatedImage = srcImage2AdaptiveThres.Clone();
                else
                    Cv2.BitwiseAnd(srcImage2AdaptiveThres, accumulatedImage, accumulatedImage);
            }
            if (Config.SaveDebugImage == true)
                accumulatedImage.SaveImage(@".\Detail\Lcd Green\1.accumulatedImage.jpg");

            Mat m_GreenRotationImage = new Mat();
            Cv2.WarpAffine(accumulatedImage, m_GreenRotationImage, angleMat, accumulatedImage.Size());
            Mat m_GreenDisplayArea = m_GreenRotationImage[displayAreaRect].Clone();

            if (Config.SaveDebugImage == true)
            m_GreenDisplayArea.SaveImage(@".\Detail\Lcd Green\2.Display.jpg");

            //170416 change mask image
            if (Config.SaveDebugImage == true)
                ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN].SaveImage(@".\Detail\Lcd Green\3.maskingimage.jpg");

            Mat tmpImg = new Mat();
            Cv2.BitwiseAnd(m_GreenDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN], tmpImg);
            Mat tmptmp = new Mat();
            tmptmp = Mat.Zeros(accumulatedImage.Size(), MatType.CV_8UC1);
            tmpImg.CopyTo(tmptmp[displayAreaRect]);
            if (Config.SaveDebugImage == true)
            {
                tmpImg.SaveImage(@".\Detail\Lcd Green\4.Blackdot_Adaptive_result.jpg");
            }

            Mat[] contours;
            var hierarchy = InputOutputArray.Create(new List<Vec4i>());

            Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

            CvRect rect = new CvRect();
            // int iTotalMaxVal = 0;
            double dArea;
            int iMax = 0;
            bool bIsFault = false;

            for (int i = 0; i < contours.Length; i++)
            {
                
                
                rect = Cv2.BoundingRect(contours[i]);
                CvRect tempRect = rect;
                dArea = Cv2.ContourArea(contours[i]);

                OpenCvSharp.CPlusPlus.Point2f[] vertices = new OpenCvSharp.CPlusPlus.Point2f[4];
                vertices = Cv2.MinAreaRect(contours[i]).Points();
                
                RotatedRect minArea_Rect =  Cv2.MinAreaRect(contours[i]);
                                

                iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                if (Result.m_nBlackDot_JudgeSize < iMax)
                {
                    //BoundingRect Max(width,height)
                    Result.m_nBlackDot_JudgeSize = iMax;
                    //BoundingRect (W+H)/2
                    Result.m_dBlackDot_JudgeWHAvg = (rect.Width + rect.Height) / 2;
                    //MinRect Max(width,height)
                    Result.m_dBlackDot_JudgeSize_MinRect = rect.Width > rect.Height ? rect.Width : rect.Height;
                    //MinRect (W+H)/2
                    Result.m_dBlackdot_JudgeWHAvg_MinRect = (rect.Width + rect.Height) / 2;
                    //contour area
                    Result.m_dBlackDot_JudgeArea = dArea;
                }
                if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                {
                    

                    Log.AddLog(string.Format("................(Green)black dot XY({0},{1}),WH({2},{3}),rotatedWH({4},{5})"
                        , rect.X + rect.Width / 2
                        , rect.Y + rect.Height / 2
                        , rect.Width.ToString("#,#")
                        , rect.Height.ToString("#.#")
                        , minArea_Rect.Size.Width.ToString("#.##")
                        , minArea_Rect.Size.Height.ToString("#.##")
                        ));
                    

                    // iTotalMaxVal += (int)dArea;

                    rect.X = rect.X - 4 / 2 - 20;
                    rect.Width = rect.Width + 40;
                    rect.Y = rect.Y - 4 / 2 - 20;
                    rect.Height = rect.Height + 40;


                    if ( iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                    {
                        bIsFault = true;
                        Result.TestResult = false;

                        Cv2.Rectangle(resultImg, rect, Scalar.Red, 4);
                        string temp = string.Format("({0},{1})", tempRect.Width, tempRect.Height);
                        //string temp = string.Format("({0},{1}),({2},{3})", tempRect.Width, tempRect.Height,
                        //    rRectWidth.ToString("#.#"), rRectHeight.ToString("#.#"));
                        Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Red, 3);
                    }

                }
                #region DON'T USE MAXAREA

                /*
                if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL || dArea > DataLcdGreen.m_nBlackDotSpec_MaxArea)
                {
                    Log.AddLog(string.Format("................(Green)black dot XY({0},{1}),WH({2},{3}),Area({4})"
                        , rect.X + rect.Width / 2
                        , rect.Y + rect.Height / 2
                        , rect.Width.ToString("#,#")
                        , rect.Height.ToString("#.#")
                        , dArea.ToString("###.#")));

                    // iTotalMaxVal += (int)dArea;

                    rect.X = rect.X - 4 / 2 - 20;
                    rect.Width = rect.Width + 40;
                    rect.Y = rect.Y - 4 / 2 - 20;
                    rect.Height = rect.Height + 40;


                    if (dArea > DataLcdGreen.m_nBlackDotSpec_MaxArea && iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                    {
                        bIsFault = true;
                        Result.TestResult = false;

                        Cv2.Rectangle(resultImg, rect, Scalar.Red, 4);
                        string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                        Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Red, 3);
                    }
                    else
                    {
                        Cv2.Rectangle(resultImg, rect, Scalar.Wheat, 4);
                        string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                        Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Wheat, 3);
                    }
                }
                */
                #endregion
            }

            #region Don't use
            //if (Config.SaveDebugImage == true)

            //Mat tmpImage = new Mat();
            //hierarchy = InputOutputArray.Create(new List<Vec4i>());
            //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
            //Mat tmpDimming = ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN].Clone();

            //Cv2.CvtColor(resultImg, tmpImage, ColorConversion.BgrToGray);
            //Mat tmpMat = Mat.Zeros(resultImg.Size(), MatType.CV_8UC1);
            //tmpDimming.CopyTo(tmpMat[displayAreaRect]);
            //tmpMat.SaveImage(@".\Detail\Lcd Green\5.tmpMat.jpg");
            //Cv2.BitwiseAnd(tmpImage, tmpMat, tmpImage);
            //tmpImage.SaveImage(@".\Detail\Lcd Green\6.tmpImage.jpg");
            //for (int i = 0; i < contours.Length; i++)
            //{
            //    Cv2.DrawContours(tmpImage, contours, i, Cv.ScalarAll(255),-1);
            //}
            //tmpImage.SaveImage(@".\Detail\Lcd Green\7.result.jpg");
            #endregion

            CvScalar clr;
            if (bIsFault)
                clr = new CvScalar(0, 0, 255);
            else
                clr = new CvScalar(0, 255, 0);

            Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
            string sTemp = string.Format("BlackDot Measured : {0} , GJudgeSizeUL : {1}", Result.m_nBlackDot_JudgeSize, DataLcdGreen.BlackDot_GJudgeSizeUL);
            Cv2.PutText(resultImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
            sTemp = string.Format("Threshold : {0}. BlockSize : {1}", DataLcdGreen.BlackDot_GFindBlackDotThreshold, DataLcdGreen.BlackDot_GFindBlackDotBlockSize);
            Cv2.PutText(resultImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);

            Result.ImageResult = resultImg.Clone();
            //Result.ImageResultTmp = tmpImage.Clone();
            if (Config.SaveDebugImage == true)
            {
                resultImg.SaveImage(@".\Detail\Lcd Green\8.blackdot_result.jpg");
            }
            resultImg.Dispose();

        }

        #region LcdGreen Original
        
        public void LcdGreen(Mat refNormalImage, MData_LCD_GREEN DataLcdGreen, ref MResult_LCD_GREEN Result, Controller controller)
        {
			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");

            //controller.thisPointError.Clear();

            using (Mat originalImg = refNormalImage.Clone())
            {
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());
                using (Mat m_GreenRotationImage = originalImg.Clone())
                using (Mat m_GreenDisplayArea = m_GreenRotationImage[displayAreaRect].Clone())
                {
                    if (Config.SaveDebugImage == true)
                        m_GreenDisplayArea.SaveImage(@".\Detail\Lcd Green\1.Display.jpg");


                    if (m_GreenDisplayArea.Channels() == 3)
                        Cv2.CvtColor(m_GreenDisplayArea, m_GreenDisplayArea, ColorConversion.BgrToGray);
                    else if (m_GreenDisplayArea.Channels() == 4)
                        Cv2.CvtColor(m_GreenDisplayArea, m_GreenDisplayArea, ColorConversion.BgraToGray);
                    if (Config.SaveDebugImage == true)
                        m_GreenDisplayArea.SaveImage(@".\Detail\Lcd Green\2.Display_Gray.jpg");

                    Cv2.BitwiseAnd(m_GreenDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_AREA], m_GreenDisplayArea);

                    Mat tmpImg = new Mat();
                    Mat resultImg = m_GreenRotationImage.Clone();

                    Cv2.AdaptiveThreshold(m_GreenDisplayArea, tmpImg, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv,
                        DataLcdGreen.BlackDot_GFindBlackDotBlockSize, DataLcdGreen.BlackDot_GFindBlackDotThreshold);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Green\3.Blackdot_Adaptive.jpg");

                    //170416 change mask image
                    if (Config.SaveDebugImage == true)
                        ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN].SaveImage(@".\Detail\Lcd Green\3.maskingimage.jpg");
                    Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN], tmpImg);

                    Mat tmptmp = new Mat();
                    tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    tmpImg.CopyTo(tmptmp[displayAreaRect]);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Green\4.Blackdot_Adaptive_result.jpg");

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    CvRect rect = new CvRect();
                    // int iTotalMaxVal = 0;
                    double dArea;
                    int iMax = 0;
                    bool bIsFault = false;


                    for (int i = 0; i < contours.Length; i++)
                    {
                        

                        rect = Cv2.BoundingRect(contours[i]);
                        CvRect tempRect = rect;
                        dArea = Cv2.ContourArea(contours[i]);

                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.m_nBlackDot_JudgeSize < iMax)
                            Result.m_nBlackDot_JudgeSize = iMax;

                        if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                        {
                            OpenCvSharp.CPlusPlus.Point2f[] vertices = new OpenCvSharp.CPlusPlus.Point2f[4];
                            vertices = Cv2.MinAreaRect(contours[i]).Points();
                            double rRectWidth = vertices[0].DistanceTo(vertices[1]);
                            double rRectHeight = vertices[1].DistanceTo(vertices[2]);
                            Result.m_dBlackDot_JudgeArea = rRectWidth > rRectHeight ? rRectWidth : rRectHeight;

                            Log.AddLog(string.Format("................(Green)black dot XY({0},{1}),WH({2},{3}),rotatedWH({4},{5})"
                                , rect.X + rect.Width / 2
                                , rect.Y + rect.Height / 2
                                , rect.Width.ToString("#,#")
                                , rect.Height.ToString("#.#")
                                , vertices[0].DistanceTo(vertices[1]).ToString("#.#")
                                , vertices[1].DistanceTo(vertices[2]).ToString("#.#")
                                ));

                            // iTotalMaxVal += (int)dArea;

                            rect.X = rect.X - 4 / 2 - 20;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y - 4 / 2 - 20;
                            rect.Height = rect.Height + 40;


                            if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                            {
                                bIsFault = true;
                                Result.TestResult = false;

                                Cv2.Rectangle(resultImg, rect, Scalar.Red, 4);
                                //string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                                string temp = string.Format("({0},{1})", tempRect.Width, tempRect.Height);
                                //string temp = string.Format("({0},{1}),({2},{3})", tempRect.Width, tempRect.Height,
                                //    vertices[0].DistanceTo(vertices[1]).ToString("#.#"), vertices[1].DistanceTo(vertices[2]).ToString("#.#"));
                                Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Red, 3);
                            }
                            

                        }
                        #region DONT USE MAXAREA

                        /*
                        if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL || dArea > DataLcdGreen.m_nBlackDotSpec_MaxArea)
                        {
                            Log.AddLog(string.Format("................(Green)black dot XY({0},{1}),WH({2},{3}),Area({4})"
                                , rect.X + rect.Width / 2
                                , rect.Y + rect.Height / 2
                                , rect.Width.ToString("#,#")
                                , rect.Height.ToString("#.#")
                                , dArea.ToString("###.#")));

                            // iTotalMaxVal += (int)dArea;

                            rect.X = rect.X - 4 / 2 - 20;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y - 4 / 2 - 20;
                            rect.Height = rect.Height + 40;


                            if (dArea > DataLcdGreen.m_nBlackDotSpec_MaxArea && iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                            {
                                bIsFault = true;
                                Result.TestResult = false;

                                Cv2.Rectangle(resultImg, rect, Scalar.Red, 4);
                                string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                                Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Red, 3);
                            }
                            else
                            {
                                Cv2.Rectangle(resultImg, rect, Scalar.Wheat, 4);
                                string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                                Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Wheat, 3);
                            }

                        }
                        */
                        #endregion
                    }


                    //if (Config.SaveDebugImage == true)

                    Mat tmpImage = new Mat();
                    hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    Cv2.CvtColor(resultImg, tmpImage, ColorConversion.BgrToGray);
                    Mat tmpMat = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);

                    ResultImg[(int)BMP.CAMERA_DATA_DIMMING_GREEN].CopyTo(tmpMat[displayAreaRect]);
                    tmpMat.SaveImage(@".\Detail\Lcd Green\5.tmpMat.jpg");
                    Cv2.BitwiseAnd(tmpImage, tmpMat, tmpImage);
                    tmpImage.SaveImage(@".\Detail\Lcd Green\6.tmpImage.jpg");
                    for (int i = 0; i < contours.Length; i++)
                    {
                        Cv2.DrawContours(tmpImage, contours, i, Cv.ScalarAll(255),-1);
                    }
                    tmpImage.SaveImage(@".\Detail\Lcd Green\7.dustfilter+threshold+result.jpg");

                    #region Before 2017.05.06.
                    //if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                    //{
                    //	//170416 sh32.heo
                    //	//compare the previous tested set 
                    //	//string s = string.Format("(test green) black dot loc, x : {0}, y : {1}",
                    //	//   rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                    //	//Console.WriteLine(s);

                    //	Log.AddLog(string.Format("................(Green)black dot XY({0},{1}),WH({2},{3}),Area({4})"
                    //                          , rect.X + rect.Width / 2
                    //                          , rect.Y + rect.Height / 2
                    //                          , rect.Width.ToString("#,#")
                    //                          , rect.Height.ToString("#.#")
                    //                          , dArea.ToString("###.#")));

                    //	bIsFault = true;

                    //                      iTotalMaxVal += (int)dArea;

                    //	rect.X = rect.X - 4 / 2 -20;
                    //	rect.Width = rect.Width + 40;
                    //	rect.Y = rect.Y - 4 / 2 -20;
                    //	rect.Height = rect.Height + 40;


                    //	Cv2.Rectangle(resultImg, rect, Scalar.Yellow, 4);
                    //                      string temp = string.Format("({0},{1}),{2}", tempRect.Width, tempRect.Height, dArea.ToString("###.#"));
                    //	Cv2.PutText(resultImg, temp, new OpenCvSharp.CPlusPlus.Point(rect.X, rect.Y - 10), FontFace.HersheyComplex, 1, CvColor.Yellow, 3);

                    //                      //controller.thisPointError.Add(new CvPoint(rect.X + rect.Width / 2, rect.Y + rect.Height / 2));
                    //                  }
                    #endregion
                    #region no use
                    //                    if (!bIsFault)
                    //                    {
                    //                        Mat resImg = ot.GetPsmImage(m_GreenDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Green\5.lackdot_psm_result.jpg");
                    //#else
                    //#endif
                    //                        Cv2.Threshold(resImg, resImg, DataLcdGreen.m_nBlackDot_GPsmThreshold, 255, ThresholdType.Binary);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Green\6.blackdot_psm_threshold.jpg");
                    //#else
                    //#endif
                    //                        Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Green\7.blackdot_psm_threshold+area.jpg");
                    //#else
                    //#endif
                    //                        tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1); //160906
                    //                        //tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //                        resImg.CopyTo(tmptmp[displayAreaRect]);

                    //                        hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    //                        Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //                        dArea = 0;
                    //                        iTotalMaxVal = 0;

                    //                        for (int i = 0; i < contours.Length; i++)
                    //                        {
                    //                            rect = Cv2.BoundingRect(contours[i]);
                    //                            dArea = Cv2.ContourArea(contours[i]);

                    //                            iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //                            if (Result.m_nBlackDot_JudgeSize < iMax)
                    //                                Result.m_nBlackDot_JudgeSize = iMax;

                    //                            if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                    //                            {
                    //                                bIsFault = true;
                    //                                iTotalMaxVal += (int)dArea;

                    //                                rect.X = rect.X - 4 / 2;
                    //                                rect.Width = rect.Width + 40;
                    //                                rect.Y = rect.Y - 4 / 2;
                    //                                rect.Height = rect.Height + 40;

                    //                                Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //                            }
                    //                        }
                    //                    }
                    #endregion

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    string sTemp = string.Format("BlackDot Measured : {0} , GJudgeSizeUL : {1}", Result.m_nBlackDot_JudgeSize, DataLcdGreen.BlackDot_GJudgeSizeUL);
                    Cv2.PutText(resultImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                    sTemp = string.Format("Threshold : {0}. BlockSize : {1}", DataLcdGreen.BlackDot_GFindBlackDotThreshold, DataLcdGreen.BlackDot_GFindBlackDotBlockSize);
                    Cv2.PutText(resultImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);

                    Result.ImageResult = resultImg.Clone();
                    //Result.ImageResultTmp = tmpImage.Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        resultImg.SaveImage(@".\Detail\Lcd Green\8.blackdot_result.jpg");
                    }
                    resultImg.Dispose();

                    #region X
                    //bool bIsFault = false;


                    //Mat resultImg = m_GreenRotationImage.Clone();
                    //Mat resImg = ot.GetPsmImage(m_GreenDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //resImg.SaveImage(@".\Detail\Lcd Green\5.lackdot_psm_result.jpg");

                    //Cv2.Threshold(resImg, resImg, DataLcdGreen.m_nBlackDot_GPsmThreshold, 255, ThresholdType.Binary);
                    //resImg.SaveImage(@".\Detail\Lcd Green\6.blackdot_psm_threshold.jpg");

                    //Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //resImg.SaveImage(@".\Detail\Lcd Green\7.blackdot_psm_threshold+area.jpg");

                    //Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1); //160906
                    //resImg.CopyTo(tmptmp[displayAreaRect]);

                    //Mat[] contours;
                    //var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    //CvRect rect = new CvRect();

                    //double dArea = 0;
                    //int iTotalMaxVal = 0;
                    //int iMax = 0;


                    //for (int i = 0; i < contours.Length; i++)
                    //{
                    //    rect = Cv2.BoundingRect(contours[i]);
                    //    dArea = Cv2.ContourArea(contours[i]);

                    //    iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //    if (Result.m_nBlackDot_JudgeSize < iMax)
                    //        Result.m_nBlackDot_JudgeSize = iMax;

                    //    if (iMax > DataLcdGreen.BlackDot_GJudgeSizeUL)
                    //    {
                    //        bIsFault = true;
                    //        iTotalMaxVal += (int)dArea;

                    //        rect.X = rect.X - 4 / 2;
                    //        rect.Width = rect.Width + 40;
                    //        rect.Y = rect.Y - 4 / 2;
                    //        rect.Height = rect.Height + 40;

                    //        Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //    }
                    //}

                    /////

                    //CvScalar clr;
                    //if (bIsFault)
                    //    clr = new CvScalar(0, 0, 255);
                    //else
                    //    clr = new CvScalar(0, 255, 0);

                    //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //resultImg.SaveImage(@".\Detail\Lcd Green\8.blackdot_result.jpg");

                    //if (bIsFault)
                    //{
                    //    Result.imgNGResult = resultImg.Clone().ToBitmap();
                    //    //Dust Mask Area
                    //    //불량처리
                    //}

                    ////////////////////////////////////////////////////////////////////////////////////////
                    //{
                    //    Mat resultImg = m_GreenRotationImage.Clone();
                    //    CvScalar m_DisplayMean;
                    //    m_DisplayMean = Cv2.Mean(m_GreenDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);

                    //    bool bIsFault = false;

                    //    if (m_DisplayMean.Val0 < DataLcdGreen.m_nBrightness_GJudgeBrightnessLL
                    //        || m_DisplayMean.Val0 > DataLcdGreen.m_nBrightness_GJudgeBrightnessUL)
                    //        bIsFault = true;

                    //    CvScalar clr;
                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);

                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Green\9.brightness_result.jpg");
                    //}
                    ////////////////////////////////////////////////////////////////////////////
                    //{
                    //    Mat resultImg = m_GreenRotationImage.Clone();

                    //    Cv2.Split(m_GreenRotationImage, out green_splitImg);

                    //    for (int i = 0; i < MAX_CHANNEL; i++)
                    //    {
                    //        green_disSplit.Add(new Mat(green_splitImg[i], displayAreaRect));
                    //    }
                    //    Mat tmp = green_disSplit[GREEN_CHANNEL] / (green_disSplit[BLUE_CHANNEL] + green_disSplit[GREEN_CHANNEL] + green_disSplit[RED_CHANNEL]) * 100;
                    //    tmp.SaveImage(@".\Detail\Lcd Green\10.colorbalance.jpg");

                    //    dGreenDisplayColorBalance = (double)Cv2.Mean(tmp, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;

                    //    bool bIsFault = false;

                    //    if (dGreenDisplayColorBalance < (double)DataLcdGreen.m_nColorBalance_GJudgeColorBLL ||
                    //        dGreenDisplayColorBalance > (double)DataLcdGreen.m_nColorBalance_GJudgeColorBUL)
                    //    {
                    //        bIsFault = true;
                    //    }

                    //    CvScalar clr;
                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);


                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Green\11.colorbalance_result.jpg");
                    //}
                    //////////DIFF///////////////////////////////////////////////////////////////////
                    
                    //{
                    //    Mat resultImg = m_GreenRotationImage.Clone();

                    //    CvRect tempRect = new CvRect(5, 5, m_displayWidth - 10, m_displayHeight - 10);
                    //    CvRect tempRect2 = new CvRect(m_displayX + 5, m_displayY + 5, m_displayWidth - 10, m_displayHeight - 10);

                    //    Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                    //    Mat blueImg = new Mat(green_splitImg[BLUE_CHANNEL], tempRect2);
                    //    Mat greenImg = new Mat(green_splitImg[GREEN_CHANNEL], tempRect2);
                    //    Mat redImg = new Mat(green_splitImg[RED_CHANNEL], tempRect2);

                    //    maskROI.SaveImage(@".\Detail\Lcd Green\12.diffcolorarearate_mask.jpg");
                    //    blueImg.SaveImage(@".\Detail\Lcd Green\13.diffcolorarearate_blue.jpg");
                    //    greenImg.SaveImage(@".\Detail\Lcd Green\14.diffcolorarearate_green.jpg");
                    //    redImg.SaveImage(@".\Detail\Lcd Green\15.diffcolorarearate_red.jpg");

                    //    double rateValue = 0;
                    //    int rowGap = DataLcdGreen.m_nDiffColorAreaRate_GWidthSplitCount;
                    //    int colGap = DataLcdGreen.m_nDiffColorAreaRate_GHeightSplitCount;

                    //    bool bIsFault = false;

                    //    Mat cal = greenImg / (blueImg + greenImg + redImg) * 100;
                    //    cal.SaveImage(@".\Detail\Lcd Green\16.diffcolorarearate_cal_mat.jpg");

                    //    //sh32.heo
                    //    blueImg.Release();
                    //    greenImg.Release();
                    //    redImg.Release();
                    //    foreach (Mat m in green_splitImg)
                    //    {
                    //        m.Release();
                    //    }
                    //    //

                    //    Mat lowCal = new Mat();
                    //    Cv2.Threshold(cal, lowCal, dGreenDisplayColorBalance - DataLcdGreen.m_nDiffColorAreaRate_GJudgePixelRateUL, 255, ThresholdType.BinaryInv);
                    //    lowCal.SaveImage(@".\Detail\Lcd Green\17.diffcolorarearate_cal_mat_low_balance.jpg");

                    //    Mat highCal = new Mat();
                    //    Cv2.Threshold(cal, highCal, dGreenDisplayColorBalance + DataLcdGreen.m_nDiffColorAreaRate_GJudgePixelRateUL, 255, ThresholdType.Binary);
                    //    highCal.SaveImage(@".\Detail\Lcd Green\18.diffcolorarearate_cal_mat_high_balance.jpg");

                    //    Cv2.BitwiseOr(lowCal, highCal, cal);
                    //    cal.SaveImage(@".\Detail\Lcd Green\19.diffcolorarearate_cal_mat_ng_balance.jpg");

                    //    Mat tmp = new Mat();
                    //    Mat tmpMask = new Mat();
                    //    CvScalar avg;

                    //    int cGap = 0;
                    //    int rGap = 0;

                    //    double dPixelsRate = 0;
                    //    double dMaxPixelsRate = 0;
                    //    bIsFault = false;

                    //    for (int a = 0; a < cal.Rows; a = a + rowGap)
                    //    {
                    //        for (int b = 0; b < cal.Cols; b = b + colGap)
                    //        {
                    //            cGap = cal.Cols - b < colGap ? cal.Cols - b : colGap;
                    //            rGap = cal.Rows - a < rowGap ? cal.Rows - a : rowGap;
                    //            tempRect = new CvRect(b, a, cGap, rGap);
                    //            tmp = new Mat(cal, tempRect);
                    //            tmp.SaveImage(@".\Detail\Lcd Green\20.tmp.jpg");
                    //            tmpMask = new Mat(maskROI, tempRect);

                    //            Cv2.BitwiseAnd(tmp, tmpMask, tmp);
                    //            var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    //            Mat[] contours;
                    //            Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //            //sh32.heo
                    //            tmp.Release();
                    //            tmpMask.Release();
                    //            //

                    //            for (int i = 0; i < contours.Length; i++)
                    //            {
                    //                dPixelsRate += Cv2.ContourArea(contours[i]);
                    //            }
                    //            dMaxPixelsRate = dPixelsRate;
                    //            dMaxPixelsRate = (dMaxPixelsRate / ((m_displayWidth - 10) * (m_displayHeight - 10))) * 100;

                    //            if (dMaxPixelsRate > DataLcdGreen.m_nDiffColorAreaRate_GDiffColorJudgePixelRate)
                    //            {
                    //                bIsFault = true;
                    //                Cv2.Rectangle(resultImg, new CvRect(m_displayX + 5 + b, m_displayY + 5 + a, cGap, rGap), new CvScalar(255, 0, 0), 4);
                    //            }

                    //        }
                    //    }
                    //    CvScalar clr;
                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);
                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Green\21.diffcolorarearate_result.jpg");
                    //}
                    
                    #endregion
                }
            }
        }
        
    
        #endregion
        public void LcdBlue(Mat refNormalImage, MData_LCD_BLUE DataLcdBlue, ref MResult_LCD_BLUE Result)
        {
			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");
            using (Mat originalImg = refNormalImage.Clone())
            {
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());
                using (Mat m_BlueRotationImage = originalImg.Clone())
                using (Mat m_BlueDisplayArea = m_BlueRotationImage[displayAreaRect].Clone())
                {
                    if (Config.SaveDebugImage == true)
                        m_BlueDisplayArea.SaveImage(@".\Detail\Lcd Blue\1.Display.jpg");

                    if (m_BlueDisplayArea.Channels() == 3)
                        Cv2.CvtColor(m_BlueDisplayArea, m_BlueDisplayArea, ColorConversion.BgrToGray);
                    else if (m_BlueDisplayArea.Channels() == 4)
                        Cv2.CvtColor(m_BlueDisplayArea, m_BlueDisplayArea, ColorConversion.BgraToGray);
                    Cv2.BitwiseAnd(m_BlueDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_AREA], m_BlueDisplayArea);
                    if (Config.SaveDebugImage == true)
                        m_BlueDisplayArea.SaveImage(@".\Detail\Lcd Blue\2.Display_Gray.jpg");


                    Mat tmpImg = new Mat();
                    Mat resultImg = m_BlueRotationImage.Clone();

                    Cv2.AdaptiveThreshold(m_BlueDisplayArea, tmpImg, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv,
                        DataLcdBlue.BlackDot_BFindBlackDotBlockSize, DataLcdBlue.BlackDot_BFindBlackDotThreshold);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Blue\3.Blackdot_Adaptive.jpg");

                    Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tmpImg);

                    Mat tmptmp = new Mat();
                    tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    tmpImg.CopyTo(tmptmp[displayAreaRect]);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Blue\4.Blackdot_Adaptive_result.jpg");

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    CvRect rect = new CvRect();
                    int iTotalMaxVal = 0;
                    double dArea;
                    int iMax = 0;
                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.m_nBlackDot_JudgeSize < iMax)
                            Result.m_nBlackDot_JudgeSize = iMax;

                        if (iMax > DataLcdBlue.BlackDot_BJudgeSizeUL)
                        {

                            bIsFault = true;
                            iTotalMaxVal += (int)dArea;

                            rect.X = rect.X - 4 / 2;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y - 4 / 2;
                            rect.Height = rect.Height + 40;

                            Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                        }
                    }

                    #region no use
                    //                    if (!bIsFault)
                    //                    {
                    //                        Mat resImg = ot.GetPsmImage(m_BlueDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\5.blackdot_psm_result.jpg");
                    //#else
                    //#endif
                    //#if SAVEIMG
                    //                        Cv2.Threshold(resImg, resImg, DataLcdBlue.m_nBlackDot_BPsmThreshold, 255, ThresholdType.Binary);
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\6.blackdot_psm_threshold.jpg");
                    //#else
                    //#endif
                    //                        Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\7.blackdot_psm_threshold+area.jpg");
                    //#else
                    //#endif
                    //                        tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //                        resImg.CopyTo(tmptmp[displayAreaRect]);

                    //                        hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    //                        Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //                        dArea = 0;
                    //                        iTotalMaxVal = 0;

                    //                        for (int i = 0; i < contours.Length; i++)
                    //                        {
                    //                            rect = Cv2.BoundingRect(contours[i]);
                    //                            dArea = Cv2.ContourArea(contours[i]);

                    //                            iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //                            if (Result.m_nBlackDot_JudgeSize < iMax)
                    //                                Result.m_nBlackDot_JudgeSize = iMax;

                    //                            if (iMax > DataLcdBlue.BlackDot_BJudgeSizeUL)
                    //                            {
                    //                                bIsFault = true;
                    //                                iTotalMaxVal += (int)dArea;

                    //                                rect.X = rect.X - 4 / 2;
                    //                                rect.Width = rect.Width + 40;
                    //                                rect.Y = rect.Y - 4 / 2;
                    //                                rect.Height = rect.Height + 40;

                    //                                Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //                            }
                    //                        }
                    //                    }
                    #endregion

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);

                    Result.ImageResult = resultImg.Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        resultImg.SaveImage(@".\Detail\Lcd Blue\8.blackdot_result.jpg");

                    }

                    resultImg.Dispose();

                    #region X
                    //bool bIsFault = false;
                    //Mat resultImg = m_BlueRotationImage.Clone();

                    //Mat resImg = ot.GetPsmImage(m_BlueDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\5.blackdot_psm_result.jpg");

                    //Cv2.Threshold(resImg, resImg, DataLcdBlue.m_nBlackDot_BPsmThreshold, 255, ThresholdType.Binary);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\6.blackdot_psm_threshold.jpg");

                    //Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\7.blackdot_psm_threshold+area.jpg");

                    ////itmptmp = new IplImage(originalImg.ToIplImage().Size, BitDepth.U8, 1);
                    ////IplImage iresImg = resImg.ToIplImage();
                    ////itmptmp.Zero();
                    ////itmptmp.SetROI(displayAreaRect);
                    ////iresImg.Copy(itmptmp);
                    ////itmptmp.ResetROI();

                    ////tmptmp = itmptmp.ToBitmap().ToMat();
                    ////itmpImg.Dispose();
                    ////iresImg.Dispose();

                    //Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //resImg.CopyTo(tmptmp[displayAreaRect]);

                    //var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    //Mat[] contours;

                    //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //double dArea = 0;
                    //int iTotalMaxVal = 0;
                    //CvRect rect = new CvRect();
                    //int iMax = 0;
                    //for (int i = 0; i < contours.Length; i++)
                    //{
                    //    rect = Cv2.BoundingRect(contours[i]);
                    //    dArea = Cv2.ContourArea(contours[i]);

                    //    iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //    if (Result.m_nBlackDot_JudgeSize < iMax)
                    //        Result.m_nBlackDot_JudgeSize = iMax;

                    //    if (iMax > DataLcdBlue.BlackDot_BJudgeSizeUL)
                    //    {
                    //        bIsFault = true;
                    //        iTotalMaxVal += (int)dArea;

                    //        rect.X = rect.X - 4 / 2;
                    //        rect.Width = rect.Width + 40;
                    //        rect.Y = rect.Y - 4 / 2;
                    //        rect.Height = rect.Height + 40;

                    //        Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //    }
                    //}


                    //CvScalar clr;
                    //if (bIsFault)
                    //    clr = new CvScalar(0, 0, 255);
                    //else
                    //    clr = new CvScalar(0, 255, 0);

                    //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //resultImg.SaveImage(@".\Detail\Lcd Blue\8.blackdot_result.jpg");


                    //if (bIsFault)
                    //{
                    //    Result.imgNGResult = resultImg.Clone().ToBitmap();
                    //    //Dust Mask Area
                    //    //불량처리
                    //}


                    //{
                    //    Mat resultImg = m_BlueRotationImage.Clone();

                    //    CvScalar m_DisplayMean;
                    //    m_DisplayMean = Cv2.Mean(m_BlueDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);

                    //    bool bIsFault = false;

                    //    if (m_DisplayMean.Val0 < DataLcdBlue.m_nBrightness_BJudgeColorBLL
                    //        || m_DisplayMean.Val0 > DataLcdBlue.m_nBrightness_BJudgeColorBUL)
                    //        bIsFault = true;

                    //    CvScalar clr;

                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);

                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Blue\9.brightness_result.jpg");

                    //}

                    //{
                    //    Mat resultImg = m_BlueRotationImage.Clone();

                    //    CvScalar meanVal;
                    //    double totalVal = 0;


                    //    Cv2.Split(m_BlueRotationImage, out blue_splitImg);

                    //    for (int i = 0; i < MAX_CHANNEL; i++)
                    //    {
                    //        blue_disSplit.Add(new Mat(blue_splitImg[i], displayAreaRect));
                    //    }
                    //    Mat tmp = blue_disSplit[BLUE_CHANNEL] / (blue_disSplit[BLUE_CHANNEL] + blue_disSplit[GREEN_CHANNEL] + blue_disSplit[RED_CHANNEL]) * 100;

                    //    tmp.SaveImage(@".\Detail\Lcd Blue\10.colorbalance.jpg");

                    //    dBlueDisplayColorBalance = (double)Cv2.Mean(tmp, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;

                    //    bool bIsFault = false;

                    //    if (dBlueDisplayColorBalance < (double)DataLcdBlue.m_nColorBalance_BJudgeColorBLL ||
                    //        dBlueDisplayColorBalance > (double)DataLcdBlue.m_nColorBalance_BJudgeColorBUL)
                    //    {
                    //        bIsFault = true;
                    //    }

                    //    CvScalar clr;

                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);


                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Blue\11.colorbalance_result.jpg");
                    //    resultImg.Release();


                    /////////////DIFF/////////////////
                    /*
                    {
                        Mat resultImg = m_BlueRotationImage.Clone();

                        CvRect tempRect = new CvRect(5, 5, m_displayWidth - 10, m_displayHeight - 10);
                        CvRect tempRect2 = new CvRect(m_displayX + 5, m_displayY + 5, m_displayWidth - 10, m_displayHeight - 10);

                        Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                        Mat blueImg = new Mat(blue_splitImg[BLUE_CHANNEL], tempRect2);
                        Mat greenImg = new Mat(blue_splitImg[GREEN_CHANNEL], tempRect2);
                        Mat redImg = new Mat(blue_splitImg[RED_CHANNEL], tempRect2);

                        maskROI.SaveImage(@".\Detail\Lcd Blue\12.diffcolorarearate_mask.jpg");
                        blueImg.SaveImage(@".\Detail\Lcd Blue\13.diffcolorarearate_blue.jpg");
                        greenImg.SaveImage(@".\Detail\Lcd Blue\14.diffcolorarearate_green.jpg");
                        redImg.SaveImage(@".\Detail\Lcd Blue\15.diffcolorarearate_red.jpg");

                        double rateValue = 0;
                        int rowGap = DataLcdBlue.m_nDiffColorAreaRate_BHeightSplitCount;
                        int colGap = DataLcdBlue.m_nDiffColorAreaRate_BWidthSplitCount;

                        bool bIsFault = false;

                        Mat cal = blueImg / (blueImg + greenImg + redImg) * 100;
                        cal.SaveImage(@".\Detail\Lcd Blue\16.diffcolorarearate_cal_mat.jpg");

                        Mat lowCal = new Mat();
                        Cv2.Threshold(cal, lowCal, dBlueDisplayColorBalance - DataLcdBlue.m_nDiffColorAreaRate_BJudgePixelRateLL, 255, ThresholdType.BinaryInv);
                        lowCal.SaveImage(@".\Detail\Lcd Blue\17.diffcolorarearate_cal_mat_low_balance.jpg");

                        Mat highCal = new Mat();
                        Cv2.Threshold(cal, highCal, dBlueDisplayColorBalance + DataLcdBlue.m_nDiffColorAreaRate_BJudgePixelRateUL, 255, ThresholdType.Binary);
                        highCal.SaveImage(@".\Detail\Lcd Blue\18.diffcolorarearate_cal_mat_high_balance.jpg");

                        Cv2.BitwiseOr(lowCal, highCal, cal);
                        cal.SaveImage(@".\Detail\Lcd Blue\19.diffcolorarearate_cal_mat_ng_balance.jpg");

                        Mat tmp = new Mat();
                        Mat tmpMask = new Mat();
                        CvScalar avg;

                        int cGap = 0;
                        int rGap = 0;

                        double dPixelsRate = 0;
                        double dMaxPixelsRate = 0;
                        bIsFault = false;

                        for (int a = 0; a < cal.Rows; a = a + rowGap)
                        {
                            for (int b = 0; b < cal.Cols; b = b + colGap)
                            {
                                cGap = cal.Cols - b < colGap ? cal.Cols - b : colGap;
                                rGap = cal.Rows - a < rowGap ? cal.Rows - a : rowGap;
                                tempRect = new CvRect(b, a, cGap, rGap);
                                tmp = new Mat(cal, tempRect);
                                tmpMask = new Mat(maskROI, tempRect);

                                Cv2.BitwiseAnd(tmp, tmpMask, tmp);

                                Mat[] contours;
                                var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                                Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                                for (int i = 0; i < contours.Length; i++)
                                {
                                    dPixelsRate += Cv2.ContourArea(contours[i]);
                                }

                                //sh32.heo
                                foreach (Mat m in contours)
                                {
                                    m.Release();
                                }

                                dMaxPixelsRate = dPixelsRate;
                                dMaxPixelsRate = (dMaxPixelsRate / ((m_displayWidth - 10) * (m_displayHeight - 10))) * 100;

                                if (dMaxPixelsRate > DataLcdBlue.m_nDiffColorAreaRate_BDiffColorJudgePixelRate)
                                {
                                    bIsFault = true;
                                    Cv2.Rectangle(resultImg, new CvRect(m_displayX + 5 + b, m_displayY + 5 + a, cGap, rGap), new CvScalar(255, 0, 0), 4);
                                }

                            }
                        }

                        CvScalar clr;
                        if (bIsFault)
                            clr = new CvScalar(0, 0, 255);
                        else
                            clr = new CvScalar(0, 255, 0);
                        Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                        resultImg.SaveImage(@".\Detail\Lcd Blue\21.diffcolorarearate_result.jpg");
                    }
                    */
                    #endregion
                }

            }

        }

        // Check Corner Mung

        bool[] cornerResult = { true, true, true, true };
        public bool CheckConerMung(ref Mat resultImg, ref Mat mungMat, CvRect displayAreaRect, int mungSpec, int gapSpec)//, int mungPixelCntSpec)
        {

            //int disp_x = displayAreaRect.X;
            //int disp_y = displayAreaRect.Y;

            int disp_x = 0;
            int disp_y = 0;
            int width = displayAreaRect.Width;
            int hight = displayAreaRect.Height;
            int spec = mungSpec;

            int countOfwhite = 0;
            int check_Side = 0;

            bool isPass = true;

            CvRect leftUpRect = new CvRect(disp_x, disp_y, spec, spec);
            CvRect leftDownRect = new CvRect(disp_x, disp_y + hight - spec, spec, spec);
            CvRect rightUpRect = new CvRect(disp_x + width - spec, disp_y, spec, spec);
            CvRect rightDownRect = new CvRect(disp_x + width - spec, disp_y + hight - spec, spec, spec);

            Mat leftUpCorner = new Mat(leftUpRect.Size, MatType.CV_8UC1);
            Mat leftDownCorner = new Mat(leftDownRect.Size, MatType.CV_8UC1);
            Mat rightUpCorner = new Mat(rightUpRect.Size, MatType.CV_8UC1);
            Mat rightDownCorner = new Mat(rightDownRect.Size, MatType.CV_8UC1);

            
            // check leftUp mung
            leftUpCorner = mungMat[leftUpRect].Clone();
            leftDownCorner = mungMat[leftDownRect].Clone();
            rightUpCorner = mungMat[rightUpRect].Clone();
            rightDownCorner = mungMat[rightDownRect].Clone();

            Mat[] tmp_mat = { leftUpCorner, leftDownCorner, rightUpCorner, rightDownCorner };
            CvRect[] tmp_rect = { leftUpRect, leftDownRect, rightUpRect, rightDownRect };

            if (Config.SaveDebugImage == true)
            {
                tmp_mat[0].SaveImage(@".\Detail\Lcd White\91.leftUpCorner.jpg");
                tmp_mat[1].SaveImage(@".\Detail\Lcd White\91.leftDownCorner.jpg");
                tmp_mat[2].SaveImage(@".\Detail\Lcd White\91.rightUpCorner.jpg");
                tmp_mat[3].SaveImage(@".\Detail\Lcd White\91.rightDownCorner.jpg");
            }

            //Console.Clear();

            cornerResult[0] = true;
            cornerResult[1] = true;
            cornerResult[2] = true;
            cornerResult[3] = true;

            for (int k=0;k<tmp_mat.Length;k++) 
            {
                List<List<int>> dataX = new List<List<int>>();

                for (int i = 0; i < spec; i++)
                {
                    dataX.Add(new List<int>());
                    for (int j = 0; j < spec; j++)
                    {
                        Byte color_value = tmp_mat[k].At<byte>(j, i);
                        if (color_value == 0xff)
                        {
                            dataX[i].Add(j);
                        }
                    }
                }
                for (int i = 0; i < spec; i++)
                {
                    if (dataX[i].Count > 1)
                    {
                        for (int j = 0; j < dataX[i].Count - 1; j++)
                        {
                            if((dataX[i][j + 1] - dataX[i][j]) > gapSpec)
                            {
                                cornerResult[k] = false;
                            }
                        }
                    }
                }

                List<List<int>> dataY = new List<List<int>>();
                for (int j = 0; j < spec; j++)
                {
                    dataY.Add(new List<int>());
                    for (int i = 0; i < spec; i++)
                    {
                        Byte color_value = tmp_mat[k].At<byte>(j, i);
                        if (color_value == 0xff)
                        {
                            dataY[j].Add(i);
                        }
                    }
                }
                for (int i = 0; i < spec; i++)
                {
                    if (dataY[i].Count > 1)
                    {
                        for (int j = 0; j < dataY[i].Count - 1; j++)
                        {
                            if ((dataY[i][j + 1] - dataY[i][j]) > gapSpec)
                            {
                                cornerResult[k] = false;
                            }
                        }
                    }
                }
            }
            CvScalar clr;

            if (cornerResult[0] == true)
                clr = new CvScalar(0, 255, 0);
            else
                clr = new CvScalar(0, 0, 255);
            Cv2.Rectangle(resultImg, new OpenCvSharp.CPlusPlus.Rect(displayAreaRect.X + leftUpRect.X, displayAreaRect.Y + leftUpRect.Y, leftUpRect.Width, leftUpRect.Height), clr, 5);

            if (cornerResult[1] == true)
                clr = new CvScalar(0, 255, 0);
            else
                clr = new CvScalar(0, 0, 255);
            Cv2.Rectangle(resultImg, new OpenCvSharp.CPlusPlus.Rect(displayAreaRect.X + leftDownRect.X, displayAreaRect.Y + leftDownRect.Y, leftDownRect.Width, leftDownRect.Height), clr, 5);

            if (cornerResult[2] == true)
                clr = new CvScalar(0, 255, 0);
            else
                clr = new CvScalar(0, 0, 255);
            Cv2.Rectangle(resultImg, new OpenCvSharp.CPlusPlus.Rect(displayAreaRect.X + rightUpRect.X, displayAreaRect.Y + rightUpRect.Y, rightUpRect.Width, rightUpRect.Height), clr, 5);

            if (cornerResult[3] == true)
                clr = new CvScalar(0, 255, 0);
            else
                clr = new CvScalar(0, 0, 255);
            Cv2.Rectangle(resultImg, new OpenCvSharp.CPlusPlus.Rect(displayAreaRect.X + rightDownRect.X, displayAreaRect.Y + rightDownRect.Y, rightDownRect.Width, rightDownRect.Height), clr, 5);
            //foreach (Mat tmpData in tmp_mat)
            //{
            //    // check white value
            //    for (int i = 0; i < spec; i++)
            //    {
            //        for (int j = 0; j < spec; j++)
            //        {
            //            Byte color_value = tmpData.At<byte>(j, i);
            //            if (color_value == 0xff)
            //            {
            //                countOfwhite++;
            //            }
            //        }
            //    }
            //    // Temporary threshold value. should be changed to value type.
            //    Log.AddLog(check_Side.ToString() + " : " + countOfwhite);
            //    if (countOfwhite > mungPixelCntSpec)
            //    {
            //        Logger.Write(string.Format("Found mung in corner. countOfwhite : {0}", countOfwhite));
            //        Log.AddLog(string.Format("Found mung in corner. countOfwhite : {0}", countOfwhite));

            //        isPass = false;
            //        Cv2.Rectangle(resultImg, tmp_rect[check_Side], new CvScalar(0, 0, 255), 10);
            //    }
            //    countOfwhite = 0;
            //    check_Side++;
            //}

            // Clear Memory
            foreach (Mat tmpData in tmp_mat)
            {
                tmpData.Dispose();
            }

            GC.Collect();
            return isPass;
        }


        public void LcdWhite(Mat refNormalImage, MData_LCD_WHITE DataLcdWhite, ref MResult_LCD_WHITE Result)
        {
            if (Config.SaveDebugImage == true)
                refNormalImage.SaveImage(@".\Detail\Lcd White\2.refNormalImage.jpg");

			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");
            Mat originalImg = refNormalImage.Clone();
            if (originalImg.Channels() == 3)
                Cv2.CvtColor(originalImg, originalImg, ColorConversion.BgrToGray);
            else if (originalImg.Channels() == 4)
                Cv2.CvtColor(originalImg, originalImg, ColorConversion.BgraToGray);
            Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

            using (Mat m_WhiteRotationImage = originalImg.Clone())
            using (Mat m_WhiteDisplayArea = m_WhiteRotationImage[displayAreaRect].Clone())
            {
                Mat resultImg = m_WhiteRotationImage.Clone();

                double dArea = 0;
                Mat[] contours;
                var hierarchy = OutputArray.Create(new List<Vec4i>());
                Mat gray = m_WhiteDisplayArea.Clone();
                Cv2.CvtColor(gray, gray, ColorConversion.BgrToGray);

                //check mung 
                List<ErrPoint> pointError = new List<ErrPoint>();
                //List<ErrPoint> pointErrorRisk = new List<ErrPoint>(); // Bae 2017.05.06. Condition Added : Not only "Rect", But also "Min Area"

                Mat psmImage = psm.GetPsmImage(gray, 8, 8, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
               
                if (Config.SaveDebugImage == true)
                    psmImage.SaveImage(@".\Detail\Lcd White\4.tmpDisplay_psm.jpg");

                Mat mask = Mat.Zeros(refNormalImage.Size(), MatType.CV_8UC1);
                Cv2.BitwiseAnd(ResultImg[(int)BMP.CAMERA_DATA_SOMETHING][displayAreaRect], ResultImg[(int)BMP.CAMERA_DATA_AREA], mask);

                if (Config.SaveDebugImage == true)
                    mask.SaveImage(@".\Detail\Lcd White\5.tmpDisplay_mask_bitwiseand.jpg");

                Cv2.BitwiseAnd(psmImage, mask, psmImage);
                if (Config.SaveDebugImage == true)
                    psmImage.SaveImage(@".\Detail\Lcd White\6.tmpDisplay_psm+mask.jpg");

                Cv2.Threshold(psmImage, psmImage, DataLcdWhite.MungThreshold, 255, ThresholdType.Binary);
                
                if (Config.SaveDebugImage == true)
                    psmImage.SaveImage(@".\Detail\Lcd White\7.tmpDisplay_psm+mask+thr.jpg");

                bool bIsFault = false;

                if (DataLcdWhite.TestCorner)
                {
                    CheckConerMung(ref resultImg, ref psmImage, displayAreaRect,
                                           DataLcdWhite.CornerLength, DataLcdWhite.GapSpec);

                    foreach (bool result in cornerResult)
                    {
                        if (result == false)
                        {
                            Result.m_nMungNgCnt++;
                            bIsFault = true;
                        }
                    }
                }
                CvRect rect;
                hierarchy = OutputArray.Create(new List<Vec4i>());

                Mat tmpImage = new Mat(originalImg.Size(), MatType.CV_8UC1);
                psmImage.CopyTo(tmpImage[displayAreaRect]);
                Cv2.FindContours(tmpImage, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

               
                for (int i = 0; i < contours.Length; i++)
                {
                    rect = Cv2.BoundingRect(contours[i]);
                    dArea = Cv2.ContourArea(contours[i]);

                    int tmp_rect_x = rect.X + rect.Width / 2;
                    int tmp_rect_y = rect.Y + rect.Height / 2;

                    if (tmp_rect_x < displayAreaRect.X + DataLcdWhite.CornerLength &&
                        tmp_rect_y < displayAreaRect.Y + DataLcdWhite.CornerLength)
                    {
                        continue;
                    }
                    if (tmp_rect_x < displayAreaRect.X + DataLcdWhite.CornerLength &&
                        tmp_rect_y > displayAreaRect.Y + displayAreaRect.Height - DataLcdWhite.CornerLength)
                    {
                        continue;
                    }
                    if (tmp_rect_x > displayAreaRect.X + displayAreaRect.Width - DataLcdWhite.CornerLength
                        && tmp_rect_y < displayAreaRect.Y + DataLcdWhite.CornerLength)
                    {
                        continue;
                    }
                    if (tmp_rect_x > displayAreaRect.X + displayAreaRect.Width - DataLcdWhite.CornerLength
                        && tmp_rect_y > displayAreaRect.Y + displayAreaRect.Height - DataLcdWhite.CornerLength)
                    {
                        continue;
                    }
                    if (rect.Width <= DataLcdWhite.Mung_WJudgeSizeUL || rect.Height <= DataLcdWhite.Mung_WJudgeSizeUL)
                    //if (Math.Min(rect.Width, rect.Height) < DataLcdWhite.Mung_WJudgeSizeUL || Math.Max(rect.Width, rect.Height) < DataLcdWhite.m_nMungAreaHL)
                    {
                        continue;
                    }

                    //if (dArea > DataLcdWhite.m_nMung_MaxArea)
                        pointError.Add(new ErrPoint(new CvPoint(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), rect, dArea));
                    //else
                    //    pointErrorRisk.Add(new ErrPoint(new CvPoint(rect.X + rect.Width / 2, rect.Y + rect.Height / 2), rect, dArea));

                    // Log.AddLog(string.Format("It found mung... rect.X : {0}, rect.Y : {1}, Area:{2}", tmp_rect_x, tmp_rect_y, dArea.ToString("0.000")));
                    Log.AddLog(string.Format("................(White) : XY({0},{1}), WH({2},{3})"
                        , tmp_rect_x, tmp_rect_y
                        , rect.Width, rect.Height));
                        //, dArea.ToString("0.000")));
				}

                Result.m_nMungNgCnt += pointError.Count;
                
                if (pointError.Count > 0)
                {
                    bIsFault = true;
                    int countdraw = pointError.Count > 20 ? 20 : pointError.Count;
                    for (int i = 0; i < countdraw; i++)
                    {
                        CvRect temp;
                        temp.X = pointError[i].cvRect.X - 20;
                        temp.Width = pointError[i].cvRect.Width + 40;
                        temp.Y = pointError[i].cvRect.Y - 20;
                        temp.Height = pointError[i].cvRect.Height + 40;

                        Cv2.Circle(resultImg, pointError[i].cvPoint , 25, new CvScalar(0, 0, 255), 6);
                        //string putText = string.Format("({0},{1}),{2}", pointError[i].cvRect.Width, pointError[i].cvRect.Height, pointError[i].dArea.ToString("###.#"));
                        string putText = string.Format("({0},{1})", pointError[i].cvRect.Width, pointError[i].cvRect.Height);
                        Cv2.PutText(resultImg, putText, new OpenCvSharp.CPlusPlus.Point(temp.X, temp.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Red, 3);

					}
				}

                //if (pointErrorRisk.Count > 0)   // Bae 2017.05.06. Condition Added : Not only "Rect", But also "Min Area"
                //{
                //    int countdraw = pointErrorRisk.Count > 20 ? 20 : pointErrorRisk.Count;
                //    for (int i = 0; i < countdraw; i++)
                //    {
                //        CvRect temp;
                //        temp.X = pointErrorRisk[i].cvRect.X - 20;
                //        temp.Width = pointErrorRisk[i].cvRect.Width + 40;
                //        temp.Y = pointErrorRisk[i].cvRect.Y - 20;
                //        temp.Height = pointErrorRisk[i].cvRect.Height + 40;

                //        Cv2.Circle(resultImg, pointErrorRisk[i].cvPoint, 25, new CvScalar(0, 0, 255), 6);
                //        string putText = string.Format("({0},{1}),{2}", pointErrorRisk[i].cvRect.Width, pointErrorRisk[i].cvRect.Height, pointErrorRisk[i].dArea.ToString("###.#"));
                //        Cv2.PutText(resultImg, putText, new OpenCvSharp.CPlusPlus.Point(temp.X, temp.Y - 10), FontFace.HersheyComplex, fontScale, CvColor.Black, 3);
                //    }
                //}

                CvScalar clr;

                if (bIsFault)
                {
                    clr = new CvScalar(0, 0, 255);
                    Result.TestResult = false;
                }
                else
                    clr = new CvScalar(0, 255, 0);

				Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);

                string sTemp = string.Format("WJudgeSizeUL : {0}, MungThreshold : {1}", DataLcdWhite.Mung_WJudgeSizeUL, DataLcdWhite.MungThreshold);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("CornerLength : {0}", DataLcdWhite.CornerLength);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);

                if (Config.SaveDebugImage == true)
					resultImg.SaveImage(@".\Detail\Lcd White\13.tmpDisplay_4mungresult.jpg");

				Result.ImageResult = resultImg.Clone();

				resultImg.Dispose();

                #region X

                //bool bIsFault = false;
                //Mat resultImg = m_WhiteRotationImage;

                //Mat resImg = ot.GetPsmImage(m_WhiteDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                //resImg.SaveImage(@".\Detail\Lcd White\5.blackdot_psm_result.jpg");

                //Cv2.Threshold(resImg, resImg, DataLcdWhite.m_nBlackDot_WPsmThreshold, 255, ThresholdType.Binary);
                //resImg.SaveImage(@".\Detail\Lcd White\6.blackdot_psm_threshold.jpg");

                //Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                //resImg.SaveImage(@".\Detail\Lcd White\7.blackdot_psm_threshold+area.jpg");


                //Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                //resImg.CopyTo(tmptmp[displayAreaRect]);
                //var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                //Mat[] contours;

                //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                //double dArea = 0;
                //int iTotalMaxVal = 0;
                //CvRect rect = new CvRect();
                //int iMax = 0;
                //for (int i = 0; i < contours.Length; i++)
                //{
                //    rect = Cv2.BoundingRect(contours[i]);
                //    dArea = Cv2.ContourArea(contours[i]);

                //    iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                //    if (Result.m_nBlackDot_JudgeSize < iMax)
                //        Result.m_nBlackDot_JudgeSize = iMax;

                //    if (iMax > DataLcdWhite.m_nBlackDot_WFindBlackDotJudgeArea)
                //    {
                //        bIsFault = true;
                //        iTotalMaxVal += (int)dArea;

                //        rect.X = rect.X - 4 / 2;
                //        rect.Width = rect.Width + 40;
                //        rect.Y = rect.Y - 4 / 2;
                //        rect.Height = rect.Height + 40;

                //        Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                //    }
                //}


                //CvScalar clr;
                //if (bIsFault)
                //    clr = new CvScalar(0, 0, 255);
                //else
                //    clr = new CvScalar(0, 255, 0);

                //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //resultImg.SaveImage(@".\Detail\Lcd White\8.blackdot_result.jpg");

                //if (bIsFault)
                //{
                //    Result.imgNGResult = resultImg.Clone().ToBitmap();
                //    //Dust Mask Area
                //    //불량처리
                //}
                //{
                //    Mat tmpImg = new Mat();
                //    Mat resultImg = m_WhiteRotationImage;

                //    Cv2.AdaptiveThreshold(m_WhiteDisplayArea, tmpImg, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv,
                //        DataLcdWhite.m_nBlackDot_WFindBlackDotBlockSize, DataLcdWhite.m_nBlackDot_WFindBlackDotThreshold);

                //    tmpImg.SaveImage(@".\Detail\Lcd White\3.Blackdot_Adaptive.jpg");
                //    Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tmpImg);

                //    Mat tmptmp = new Mat();
                //    tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                //    tmpImg.CopyTo(tmptmp[displayAreaRect]);
                //    tmpImg.SaveImage(@".\Detail\Lcd White\4.Blackdot_Adaptive_result.jpg");

                //    Mat[] contours;
                //    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                //    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                //    CvRect rect = new CvRect();
                //    double contourArea;
                //    int maxVal = 0;
                //    int iTotalMaxVal = 0;
                //    int defectCount = 0;
                //    double dArea;
                //    int iMax = 0;
                //    bool bIsFault = false;

                //    for (int i = 0; i < contours.Length; i++)
                //    {
                //        rect = Cv2.BoundingRect(contours[i]);
                //        dArea = Cv2.ContourArea(contours[i]);

                //        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                //        if (Result.m_nBlackDot_JudgeSize < iMax)
                //            Result.m_nBlackDot_JudgeSize = iMax;

                //        if (iMax > DataLcdWhite.m_nBlackDot_WFindBlackDotJudgeArea)
                //        {
                //            bIsFault = true;
                //            iTotalMaxVal += (int)dArea;

                //            rect.X = rect.X - 4 / 2;
                //            rect.Width = rect.Width + 40;
                //            rect.Y = rect.Y - 4 / 2;
                //            rect.Height = rect.Height + 40;

                //            Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                //        }
                //    }
                //    if (!bIsFault)
                //    {
                //        Mat resImg = ot.GetPsmImage(m_WhiteDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                //        resImg.SaveImage(@".\Detail\Lcd White\5.blackdot_psm_result.jpg");

                //        Cv2.Threshold(resImg, resImg, DataLcdWhite.m_nBlackDot_WPsmThreshold, 255, ThresholdType.Binary);
                //        resImg.SaveImage(@".\Detail\Lcd White\6.blackdot_psm_threshold.jpg");

                //        Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                //        resImg.SaveImage(@".\Detail\Lcd White\7.blackdot_psm_threshold+area.jpg");


                //        tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                //        resImg.CopyTo(tmptmp[displayAreaRect]);
                //        hierarchy = InputOutputArray.Create(new List<Vec4i>());

                //        Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                //        dArea = 0;
                //        iTotalMaxVal = 0;

                //        for (int i = 0; i < contours.Length; i++)
                //        {
                //            rect = Cv2.BoundingRect(contours[i]);
                //            dArea = Cv2.ContourArea(contours[i]);

                //            iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                //            if (Result.m_nBlackDot_JudgeSize < iMax)
                //                Result.m_nBlackDot_JudgeSize = iMax;

                //            if (iMax > DataLcdWhite.m_nBlackDot_WFindBlackDotJudgeArea)
                //            {
                //                bIsFault = true;
                //                iTotalMaxVal += (int)dArea;

                //                rect.X = rect.X - 4 / 2;
                //                rect.Width = rect.Width + 40;
                //                rect.Y = rect.Y - 4 / 2;
                //                rect.Height = rect.Height + 40;

                //                Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                //            }
                //        }
                //    }

                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd White\8.blackdot_result.jpg");

                //}

                //////////////////////////////////////////// White Brightness Inspection ///////////////////////////////////////////
                //{
                //    Mat resultImg = m_WhiteRotationImage.Clone();
                //    CvScalar m_DisplayMean;
                //    m_DisplayMean = Cv2.Mean(m_WhiteDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);

                //    bool bIsFault = false;

                //    if (m_DisplayMean.Val0 < DataLcdWhite.m_nBrightness_WJudgeBrightnessLL
                //        || m_DisplayMean.Val0 > DataLcdWhite.m_nBrightness_WJudgeBrightnessUL)
                //        bIsFault = true;

                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd White\9.brightness_result.jpg");
                //}

                /////////////////////////////////////// White Color Avg Diff (전체 디스플레이 영역에서 R,G,B 점유율 구하고, 최대 점유율 색상 비율과 최소 점유율 색상의 비율의 차를 구한다.)//////////
                //resultImg = m_WhiteRotationImage.Clone();
                //Mat[] white_splitImg;
                //List<Mat> white_disSplit = new List<Mat>();


                //CvScalar meanVal;
                //double totalVal = 0;
                //double dwhitedisplayColorBalance = 0;
                //double m_colorMeanAvg = 0;

                //Cv2.Split(m_WhiteRotationImage, out white_splitImg);

                //for (int i = 0; i < MAX_CHANNEL; i++)
                //{
                //    white_disSplit.Add(new Mat(white_splitImg[i], displayAreaRect));
                //}

                //double blueVal = 0, greenVal = 0, redVal = 0;
                //double blueRef = 0, greenRef = 0, redRef = 0;

                //blueVal = Cv2.Mean(white_disSplit[BLUE_CHANNEL], ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;
                //greenVal = Cv2.Mean(white_disSplit[GREEN_CHANNEL], ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;
                //redVal = Cv2.Mean(white_disSplit[RED_CHANNEL], ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;

                //double dTmpSum = blueVal + greenVal + redVal;

                //m_colorMeanAvg = dTmpSum / 3;

                ////전체 디스플레이 영역에서 r,g,b 점유율을 구한다

                //blueRef = blueVal / dTmpSum * 100;
                //greenRef = greenVal / dTmpSum * 100;
                //redRef = redVal / dTmpSum * 100;

                ////최대 점유율 색상 비율과 최소 점유율 색상의 비율의 차를 구한다
                //dwhitedisplayColorBalance = Math.Max(blueRef, Math.Max(greenRef, redRef)) - Math.Min(blueRef, Math.Min(greenRef, redRef));

                //bIsFault = false;
                //if (dwhitedisplayColorBalance > DataLcdWhite.m_nColorAvgDiff_WJudgeColorAvgDiffUL)
                //{
                //    bIsFault = true;
                //}

                //if (bIsFault)
                //    clr = new CvScalar(0, 0, 255);
                //else
                //    clr = new CvScalar(0, 255, 0);

                //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //resultImg.SaveImage(@".\Detail\Lcd White\10.coloravgdiff_result.jpg");

                ///////////////////////////////////////////////////////// White Diff Color Area ///////////////////////////////////////
                //resultImg = m_WhiteRotationImage.Clone();

                //CvRect tempRect = new CvRect(5, 5, m_displayWidth - 10, m_displayHeight - 10);
                //CvRect tempRect2 = new CvRect(m_displayX + 5, m_displayY + 5, m_displayWidth - 10, m_displayHeight - 10);

                //Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                //Mat blueImg = new Mat(white_splitImg[BLUE_CHANNEL], tempRect2);
                //Mat greenImg = new Mat(white_splitImg[GREEN_CHANNEL], tempRect2);
                //Mat redImg = new Mat(white_splitImg[RED_CHANNEL], tempRect2);

                //white_disSplit.Clear();

                ///////////////원본 white_splitImg -> white_disSplit로 변경 (list사용)
                //white_disSplit.Add(blueImg);
                //white_disSplit.Add(greenImg);
                //white_disSplit.Add(redImg);
                //white_disSplit.Add(maskROI);

                //maskROI.SaveImage(@".\Detail\Lcd White\11.diffcolorarearate_mask.jpg");
                //blueImg.SaveImage(@".\Detail\Lcd White\12.diffcolorarearate_blue.jpg");
                //greenImg.SaveImage(@".\Detail\Lcd White\13.diffcolorarearate_green.jpg");
                //redImg.SaveImage(@".\Detail\Lcd White\14.diffcolorarearate_red.jpg");

                //Mat tempMax = new Mat();
                //Mat tempMin = new Mat();

                //Cv2.Max(greenImg, blueImg, tempMax);
                //Cv2.Max(redImg, tempMax, tempMax);
                //Cv2.Min(greenImg, blueImg, tempMin);
                //Cv2.Min(redImg, tempMin, tempMin);
                //Mat gap = tempMax - tempMin;

                //gap.SaveImage(@".\Detail\Lcd White\15.diffcolorarearate_gap.jpg");

                //Cv2.Threshold(gap, gap, DataLcdWhite.m_nDiffColorAreaRate_WGapThreshold, 255, ThresholdType.Binary);
                //gap.SaveImage(@".\Detail\Lcd White\16.diffcolorarearate_gap_threshold.jpg");

                //Cv2.BitwiseAnd(gap, maskROI, gap);
                //gap.SaveImage(@".\Detail\Lcd White\17.diffcolorarearate_gap_threshold_mask.jpg");

                //double rateValue = 0;
                //int rowGap = DataLcdWhite.m_nDiffColorAreaRate_WGapRow;
                //int colGap = DataLcdWhite.m_nDiffColorAreaRate_WGapCol;
                //bIsFault = false;

                //Mat tmp = new Mat();
                //Mat tmpMask = new Mat();
                //CvScalar avg;

                //int cGap = 0;
                //int rGap = 0;

                //double dPixelsRate = 0;
                //double dMaxPixelsRate = 0;
                //bIsFault = false;

                //for (int a = 0; a < gap.Rows; a = a + rowGap)
                //{
                //    for (int b = 0; b < gap.Cols; b = b + colGap)
                //    {
                //        cGap = gap.Cols - b < colGap ? gap.Cols - b : colGap;
                //        rGap = gap.Rows - a < rowGap ? gap.Rows - a : rowGap;
                //        tempRect = new CvRect(b, a, cGap, rGap);
                //        tmp = new Mat(gap, tempRect);
                //        dPixelsRate = Cv2.Mean(tmp).Val0 / 255.0 * 100.0;

                //        if (dMaxPixelsRate < dPixelsRate)
                //            dMaxPixelsRate = dPixelsRate;

                //        if (dMaxPixelsRate > (double)DataLcdWhite.m_nDiffColorAreaRate_WJudgePixelRateUL)
                //        {
                //            bIsFault = true;
                //            Cv2.Rectangle(resultImg, new CvRect(m_displayX + 5 + b, m_displayY + 5 + a, cGap, rGap), new CvScalar(255, 0, 0), 4);
                //        }
                //    }
                //}
                //if (bIsFault)
                //    clr = new CvScalar(0, 0, 255);
                //else
                //    clr = new CvScalar(0, 255, 0);
                //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //resultImg.SaveImage(@".\Detail\Lcd White\18.diffcolorarearate_result.jpg");

                ////////////////////////////////MURA TEST/////////////////////////
                //{
                //    Mat resultImg = m_WhiteRotationImage.Clone();

                //    Mat image1 = new Mat();
                //    Mat image2 = new Mat();
                //    Mat image3 = new Mat();
                //    Mat image4 = new Mat();

                //    image1 = m_WhiteDisplayArea[new CvRect(DataLcdWhite.m_nMura_OffsetX, DataLcdWhite.m_nMura_OffsetY,
                //        displayAreaRect.Width - (2 * DataLcdWhite.m_nMura_OffsetX), displayAreaRect.Height - (2 * DataLcdWhite.m_nMura_OffsetY))].Clone();
                //    image2 = image1.Clone();
                //    image3 = image1.Clone();
                //    image4 = image1.Clone();

                //    image1.SaveImage(@".\Detail\Lcd White\19.mura_original.jpg");

                //    bool iRet = ot.GetPSMImage(image1, out image1, out image2, out image3, DataLcdWhite.m_nMura_WMuraBlockSize, DataLcdWhite.m_nMura_WMuraBlockSize);

                //    int margix_x = DataLcdWhite.m_nMura_OffsetX + DataLcdWhite.m_nMura_WMuraBlockSize;
                //    int margin_y = DataLcdWhite.m_nMura_OffsetY + DataLcdWhite.m_nMura_WMuraBlockSize;

                //    image1.SaveImage(@".\Detail\Lcd White\20.mura psm before threshold.jpg");

                //    Cv2.Threshold(image1, image4, DataLcdWhite.m_nMura_Threshold, 255, ThresholdType.Binary);

                //    image4.SaveImage(@".\Detail\Lcd White\21.mura psm after threshold.jpg");

                //    Cv2.MorphologyEx(image4, image4, MorphologyOperation.Close, new Mat(), null, DataLcdWhite.m_nMura_CloseCount);
                //    Cv2.MorphologyEx(image4, image4, MorphologyOperation.Open, new Mat(), null, DataLcdWhite.m_nMura_OpenCount);

                //    image4.SaveImage(@".\Detail\Lcd White\22.mura open & close image.jpg");

                //    CvRect rect = new CvRect(displayAreaRect.X + margix_x, displayAreaRect.Y + margin_y,
                //        displayAreaRect.Width - (2 * margix_x), displayAreaRect.Height - (2 * margin_y));

                //    Mat tmptmp = Mat.Zeros(m_WhiteRotationImage.Size(), MatType.CV_8UC1);
                //    image4.CopyTo(tmptmp[rect]);
                //    tmptmp.SaveImage(@".\Detail\Lcd White\22.tmptmp.jpg");


                //    bool bIsFault = false;
                //    Mat[] contours;
                //    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                //    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                //    double dArea = 0;
                //    int iTotalMaxVal = 0;
                //    int iMax = 0;

                //    for (int i = 0; i < contours.Length; i++)
                //    {
                //        rect = Cv2.BoundingRect(contours[i]);
                //        dArea = Cv2.ContourArea(contours[i]);

                //        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;
                //        if (Result.m_nMuraDot_JudgeSize < iMax)
                //            Result.m_nMuraDot_JudgeSize = iMax;

                //        if (iMax > DataLcdWhite.m_nBlackDot_WFindBlackDotJudgeArea)
                //        {
                //            if (m_displayX + 10 > rect.X + rect.Width / 2 ||
                //                m_displayX + m_displayWidth - 10 < rect.X + rect.Width / 2 ||
                //                m_displayY + 10 > rect.Y + rect.Height / 2 ||
                //                m_displayY + m_displayHeight - 10 < rect.Y + rect.Height / 2)
                //                continue;


                //            bIsFault = true;
                //            iTotalMaxVal += (int)dArea;

                //            rect.X = rect.X - 4 / 2;
                //            rect.Width = rect.Width + 40;
                //            rect.Y = rect.Y - 4 / 2;
                //            rect.Height = rect.Height + 40;

                //            Cv2.Rectangle(resultImg, rect, new CvScalar(0, 0, 255), 4);
                //        }
                //    }
                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);
                //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Lcd White\23.white_mura_result.jpg");

                //}
                #endregion
            }

        }

        #region Black,MCD
        //public void LcdBlack(Mat refNormalImage, MData_LCD_BLACK DataLcdBlack, out MResult_LCD_BLACK Result)
        //{
        //    Result = new MResult_LCD_BLACK();

        //    Console.WriteLine("start lcdblack");

        //    using (Mat originalImg = refNormalImage.Clone())
        //    {
        //        //Tilt 된 Angle만큼 이미지 회전
        //        Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

        //        using (Mat m_BlackRotationImage = originalImg.Clone())
        //        using (Mat m_BlackDisplayArea = m_BlackRotationImage[displayAreaRect])
        //        {
        //            //Display 영역 추출
        //            {
        //                m_BlackDisplayArea.SaveImage(@".\Detail\Lcd Black\1.Display.jpg");

        //                if (m_BlackDisplayArea.Channels() == 3)
        //                    Cv2.CvtColor(m_BlackDisplayArea, m_BlackDisplayArea, ColorConversion.BgrToGray);
        //                m_BlackDisplayArea.SaveImage(@".\Detail\Lcd Black\2.Display_Gray.jpg");

        //                Mat tmpImg = new Mat();
        //                Mat resultImg = m_BlackRotationImage.Clone();

        //                Cv2.Threshold(m_BlackDisplayArea, tmpImg, DataLcdBlack.m_nBrightDotWH_BrBrightDotWhThreshold, 255.0, ThresholdType.Binary);
        //                tmpImg.SaveImage(@".\Detail\Lcd Black\3.Brightarea_threshold.jpg");
        //                Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tmpImg);

        //                Mat tmptmp = new Mat();
        //                tmptmp = Mat.Zeros(refNormalImage.Size(), MatType.CV_8UC1);
        //                tmpImg.CopyTo(tmptmp[displayAreaRect]);

        //                Mat[] contours;
        //                var hierarchy = InputOutputArray.Create(new List<Vec4i>());

        //                Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

        //                CvRect rect;
        //                double contourArea = 0;
        //                int maxVal = 0;
        //                int iTotalMaxVal = 0;
        //                int defectCount = 0;
        //                double m_dContourAreaTotal = .0; ;

        //                bool bIsFault = false;

        //                for (int i = 0; i < contours.Length; i++)
        //                {
        //                    rect = Cv2.BoundingRect(contours[i]);
        //                    contourArea = Cv2.ContourArea(contours[i]);

        //                    m_dContourAreaTotal += contourArea;
        //                    Result.m_dBrightArea_BrBrightAreaJudgeArea += contourArea;

        //                    if (maxVal < (rect.Width > rect.Height ? rect.Width : rect.Height))
        //                        maxVal = rect.Width > rect.Height ? rect.Width : rect.Height;

        //                    if (Result.m_nBrightDotWH_BrBrightDotWhJudgeSize > maxVal)
        //                        Result.m_nBrightDotWH_BrBrightDotWhJudgeSize = maxVal;

        //                    if ((int)maxVal > DataLcdBlack.m_nBrightDotWH_BrBrightDotWhJudgeSizeLL)
        //                    {

        //                        iTotalMaxVal += (int)contourArea;
        //                        bIsFault = true;

        //                        rect.X = rect.X - 40 / 2;
        //                        rect.Width = rect.Width + 40;
        //                        rect.Y = rect.Y - 40 / 2;
        //                        rect.Height = rect.Height + 40;

        //                        Cv2.Rectangle(resultImg, rect, new CvScalar(0, 0, 255), 4);
        //                    }
        //                }
        //                CvScalar clr;
        //                if (bIsFault)
        //                    clr = new CvScalar(0, 0, 255);
        //                else
        //                    clr = new CvScalar(0, 255, 0);

        //                Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
        //                resultImg.SaveImage(@".\Detail\Lcd Black\4.brightdotwh_result.jpg");

        //                /////////////////////////////////////////////////////////////////////////

        //                resultImg = m_BlackRotationImage.Clone();
        //                bIsFault = false;


        //                if (m_dContourAreaTotal > DataLcdBlack.m_nBrightArea_BrBrightAreaJudgeSizeLL)
        //                {
        //                    bIsFault = true;
        //                }

        //                if (bIsFault)
        //                    clr = new CvScalar(0, 0, 255);
        //                else
        //                    clr = new CvScalar(0, 255, 0);

        //                Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
        //                resultImg.SaveImage(@".\Detail\Lcd Black\5.brightarea_result.jpg");

        //                ResultImg[(int)BMP.CAMERA_DATA_BLACK] = resultImg.Clone();
        //            }
        //        }
        //    }
        //}
        //public bool LcdMCD(Mat refNormalImage, MData_LCD_MCD DataLcdMcd, out MResult_LCD_MCD Result)
        //{
        //    Result = new MResult_LCD_MCD();

        //    using (Mat originalImg = refNormalImage.Clone())
        //    {
        //        //Tilt 된 Angle만큼 이미지 회전
        //        Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

        //        using (Mat m_MCDRotationImage = originalImg.Clone())
        //        using (Mat m_MCDDisplayArea = m_MCDRotationImage[displayAreaRect].Clone())
        //        {
        //            //Display 영역 추출
        //            m_MCDDisplayArea.SaveImage(@".\Detail\Lcd MCD\1.Display.jpg");

        //            if (m_MCDDisplayArea.Channels() == 3)
        //                Cv2.CvtColor(m_MCDDisplayArea, m_MCDDisplayArea, ColorConversion.BgrToGray);

        //            m_MCDDisplayArea.SaveImage(@".\Detail\Lcd MCD\2.Display_Gray.jpg");

        //            Mat tmpImg = new Mat();
        //            Mat resultImg = m_MCDRotationImage.Clone();

        //            Cv2.Threshold(m_MCDDisplayArea, tmpImg, DataLcdMcd.BrightLine_McdThreshold, 255, ThresholdType.Binary);
        //            tmpImg.SaveImage(@".\Detail\Lcd MCD\3.brightarea_threshold.jpg");
        //            Mat tmptmp = new Mat();
        //            tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
        //            tmpImg.CopyTo(tmptmp);

        //            Mat[] contours;
        //            var hierarchy = InputOutputArray.Create(new List<Vec4i>());

        //            Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
        //            CvRect rect = new CvRect();

        //            int iTotalMaxVal = 0;
        //            double dArea;
        //            int iMax = 0;
        //            bool bIsFault = false;

        //            for (int i = 0; i < contours.Length; i++)
        //            {
        //                rect = Cv2.BoundingRect(contours[i]);
        //                dArea = Cv2.ContourArea(contours[i]);

        //                iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

        //                if (Result.BrightLine_McdJudgeSizeLL > iMax)
        //                    Result.BrightLine_McdJudgeSizeLL = iMax;

        //                if (iMax > DataLcdMcd.BrightLine_McdJudgeSizeLL)
        //                {
        //                    bIsFault = true;
        //                    iTotalMaxVal += (int)dArea;

        //                    rect.X = rect.X - 40 / 2;
        //                    rect.Width = rect.Width + 40;
        //                    rect.Y = rect.Y - 40 / 2;
        //                    rect.Height = rect.Height + 40;

        //                    Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
        //                }
        //            }
        //            CvScalar clr;
        //            if (bIsFault)
        //                clr = new CvScalar(0, 0, 255);
        //            else
        //                clr = new CvScalar(0, 255, 0);

        //            Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
        //            resultImg.SaveImage(@".\Detail\Lcd MCD\4.brightdotwh_result.jpg");

        //            return true;
        //        }
        //    }
        //}
        #endregion

        public void TouchKey(Mat refNormalImage, MData_TOUCH_KEY DataTouchKey,
            ref MResult_TOUCH_KEY ResultTouchKey, ref MResult_TOUCH_BACKKEY ResultTouchBackKey, ref MResult_TOUCH_MENUKEY ResultTouchMenuKey)
        {
            ResultTouchKey = new MResult_TOUCH_KEY();
            ResultTouchBackKey = new MResult_TOUCH_BACKKEY();
            ResultTouchMenuKey = new MResult_TOUCH_MENUKEY();

            using (Mat originalImg = refNormalImage.Clone())
            {
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());
                Mat m_Touch_RotationImage = originalImg.Clone();

                // SUB TOUCH_PROCESS_BRIGHT_AREA  
                #region SUB TOUCH_PROCESS_BRIGHT_AREA
                {
                    //check the contours count

                    Mat resultImg = m_Touch_RotationImage.Clone();
                    if (originalImg.Channels() == MAX_CHANNEL)
                        Cv2.CvtColor(originalImg, originalImg, ColorConversion.BgrToGray);

                    m_BlackTmpMenu = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU].Clone();
                    m_BlackTmpBack = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK].Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        m_BlackTmpMenu.SaveImage(@".\Detail\Touch\1.brghtArea_menu_area+and.jpg");
                        m_BlackTmpBack.SaveImage(@".\Detail\Touch\2.brghtArea_back_area+and.jpg");
                    }
                    if (m_BlackTmpMenu.Channels() == MAX_CHANNEL)
                        Cv2.CvtColor(m_BlackTmpMenu, m_BlackTmpMenu, ColorConversion.BgrToGray);
                    if (m_BlackTmpBack.Channels() == MAX_CHANNEL)
                        Cv2.CvtColor(m_BlackTmpBack, m_BlackTmpBack, ColorConversion.BgrToGray);

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(m_BlackTmpMenu, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    double contourArea = 0;
                    int iTotalMaxVal = 0;
                    int contourSize = 0;

                    bool bIsFault = false;

                    Cv2.CvtColor(m_BlackTmpMenu, m_BlackTmpMenu, ColorConversion.GrayToBgr);
                    contourSize += contours.Length;


                    for (int i = 0; i < contours.Length; i++)
                    {
                        contourArea += Cv2.ContourArea(contours[i]);
                        //draw defect sign(red line) in advance for when it is false
                        Cv2.DrawContours(m_BlackTmpMenu, contours, i, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }

                    Mat tmptmp = m_BlackTmpBack.Clone();

                    hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    Cv2.CvtColor(m_BlackTmpBack, m_BlackTmpBack, ColorConversion.GrayToBgr);
                    contourSize += contours.Length;
                    for (int i = 0; i < contours.Length; i++)
                    {
                        contourArea += Cv2.ContourArea(contours[i]);
                        //draw defect sign(red line) in advance for when it is false
                        Cv2.DrawContours(m_BlackTmpBack, contours, i, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }

                    ResultTouchKey.TouchKeyBrightJudgeArea = contourSize;
                    if (contourSize != DataTouchKey.TouchKeyBrightAreaSpec)
                    {
                        bIsFault = true;
                        iTotalMaxVal = contourSize;
                        Log.AddLog("contour count : " + contourSize);
                        m_BlackTmpMenu.CopyTo(resultImg[menuAreaRect]);
                        m_BlackTmpBack.CopyTo(resultImg[backAreaRect]);
                    }

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    if (Config.SaveDebugImage == true)
                        resultImg.SaveImage(@".\Detail\Touch\5.bightarea_result.jpg");

                    ResultTouchKey.ImageResult= resultImg.Clone();

					resultImg.Dispose();
				}
				#endregion

                // SUB_BACK_PROCESS_DIFF_WH 
                // diff test is for check the foreign substance
                #region SUB_BACK_PROCESS_DIFF_WH
                {
                    Mat resultImg = m_Touch_RotationImage.Clone();
                    m_BlackTmpBack = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK].Clone();
                    Mat m_backRef = m_BlackTmpBack.Clone();
                    Mat tmp = m_backRef.Clone();
                    if (Config.SaveDebugImage == true)
                        m_backRef.SaveImage(@".\Detail\Touch\11.m_backRef.jpg");

                    var hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmp, out back_offset_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    Mat tmptmp = new Mat();
                    tmptmp = Mat.Zeros(m_BlackTmpBack.Size(), MatType.CV_8UC1);

                    for (int i = 0; i < back_offset_contours.Length; i++)
                    {
                        Cv2.DrawContours(tmptmp, back_offset_contours, i, Cv.ScalarAll(255), -1);
                    }
                    if (Config.SaveDebugImage == true)
                        tmptmp.SaveImage(@".\Detail\Touch\12.diff contours.jpg");

                    Cv2.BitwiseXor(m_backRef, tmptmp, tmptmp);
                    if (Config.SaveDebugImage == true)
                        tmptmp.SaveImage(@".\Detail\Touch\13.diff result.jpg");

                    Mat[] contours;
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    CvRect rect;

                    double dTotalMax = 0;
                    double dArea;

                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        bIsFault = true;

                        ResultTouchBackKey.m_dDiffArea += dArea;
                        dTotalMax += dArea;

                        Cv2.Rectangle(resultImg, rect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    if (Config.SaveDebugImage == true)
                        resultImg.SaveImage(@".\Detail\Touch\14.back_diffwh_result.jpg");

                    ResultTouchBackKey.ImageResult = resultImg.Clone();

					resultImg.Dispose();
				}
				#endregion


                // SUB_MENU_PROCESS_DIFF_WH
                // diff test is for check the foreign substance
                #region SUB_MENU_PROCESS_DIFF_WH
                {
                    Mat resultImg = m_Touch_RotationImage.Clone();

                    m_BlackTmpMenu = ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU].Clone();

                    Mat m_menuRef = m_BlackTmpMenu.Clone();
                    Mat tmp = m_menuRef.Clone();
                    if (Config.SaveDebugImage == true)
                        m_BlackTmpMenu.SaveImage(@".\Detail\Touch\20.m_blacktmpmenu.jpg");

                    var hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmp, out out_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    Mat outtmptmp = new Mat();
                    outtmptmp = Mat.Zeros(m_BlackTmpMenu.Size(), m_BlackTmpMenu.Type());

                    for (int i = 0; i < out_contours.Length; i++)
                    {
                        Cv2.DrawContours(outtmptmp, out_contours, i, Cv.ScalarAll(255), -1);
                    }
                    if (Config.SaveDebugImage == true)
                        outtmptmp.SaveImage(@".\Detail\Touch\21.outtmptmp.jpg");

                    Mat m_MenuInContours = new Mat();

                    // origine 이미지 안쪽 rect를 구하기 위해 연산함

                    Cv2.BitwiseXor(outtmptmp, m_menuRef, m_MenuInContours);
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(m_MenuInContours, out in_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    Mat intmptmp;
                    intmptmp = Mat.Zeros(m_BlackTmpMenu.Size(), m_BlackTmpMenu.Type());

                    for (int i = 0; i < in_contours.Length; i++)
                    {
                        Cv2.DrawContours(intmptmp, in_contours, i, Cv.ScalarAll(255), -1);
                    }
                    if (Config.SaveDebugImage == true)
                        intmptmp.SaveImage(@".\Detail\Touch\22.incontours.jpg");

                    Mat m_MenuDiffRef = new Mat();
                    Cv2.BitwiseXor(outtmptmp, intmptmp, m_MenuDiffRef);
                    if (Config.SaveDebugImage == true)
                        m_MenuDiffRef.SaveImage(@".\Detail\Touch\23.menudiffref.jpg");

                    Cv2.BitwiseXor(m_menuRef, m_MenuDiffRef, m_menuRef);
                    if (Config.SaveDebugImage == true)
                        m_menuRef.SaveImage(@".\Detail\Touch\24.result.jpg");


                    Mat[] contours;
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(m_menuRef, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    CvRect rect;

                    double dTotalMax = 0;
                    double dArea;
                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        bIsFault = true;
                        dTotalMax += dArea;
                        ResultTouchMenuKey.m_dDiffArea += dArea;
                        Cv2.Rectangle(resultImg, rect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }


                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    if (Config.SaveDebugImage == true)
                        resultImg.SaveImage(@".\Detail\Touch\25.menu_diffwh_result.jpg");

                    ResultTouchMenuKey.ImageResult = resultImg.Clone();
				}
				#endregion



                //////////////no use///////////////

                // SUB_BACK_PROCESS_BRIGHTNESS
                // BackKey 밝기 평균
                #region SUB_BACK_PROCESS_BRIGHTNESS

                //{
                //    //Mat resultImg = ResultImg[(int)BMP.CAMERA_DATA_BLACK].Clone();

                //    //sh32.heo 수정
                //    Mat resultImg = m_Touch_RotationImage.Clone();
                //    m_BlackTmpBack = originalImg[backAreaRect].Clone();

                //    if (m_BlackTmpBack.Channels() == 3)
                //        Cv2.CvtColor(m_BlackTmpBack, m_BlackTmpBack, ColorConversion.BgrToGray);

                //    ////////////////////////////
                //    m_dBackBrightness = Cv2.Mean(m_BlackTmpBack, ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK]).Val0;
                //    bool bIsFault = false;

                //    ResultTouchBackKey.m_dBackBrightnessMean = m_dBackBrightness;

                //    if (m_dBackBrightness < (double)DataTouchBackKey.m_nBrightness_BackKeyBrightnessLowSpec ||
                //        m_dBackBrightness > (double)DataTouchBackKey.m_nBrightness_BackKeyBrightnessUpperSpec)
                //    {
                //        bIsFault = true;
                //        Console.WriteLine("[BackProcessBrightness] lowspec : {1} upperspec : {2} calculated : {0}", m_dBackBrightness,
                //            DataTouchBackKey.m_nBrightness_BackKeyBrightnessLowSpec,
                //            DataTouchBackKey.m_nBrightness_BackKeyBrightnessUpperSpec);
                //        Cv2.Rectangle(resultImg, backAreaRect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                //    }
                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Touch\9.back_bright_result.jpg");

                //    if (bIsFault)
                //    {
                //        ResultTouchBackKey.imgNGBright = resultImg.Clone().ToBitmap();
                //        //Dust Mask Area
                //        //불량처리
                //    }
                //    resultImg.Dispose();
                //}

                #endregion

                // SUB_MENU_PROCESS_BRIGHTNESS
                // MenuKey 밝기 평균
                #region  SUB_MENU_PROCESS_BRIGHTNESS

                //{
                //    Mat resultImg = m_Touch_RotationImage.Clone();
                //    m_BlackTmpMenu = originalImg[menuAreaRect].Clone();

                //    if (m_BlackTmpMenu.Channels() == 3)
                //        Cv2.CvtColor(m_BlackTmpMenu, m_BlackTmpMenu, ColorConversion.BgrToGray);

                //    //////////////////////////////
                //    m_dMenuBrightness = Cv2.Mean(m_BlackTmpMenu, ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU]).Val0;
                //    ResultTouchMenuKey.m_dBackBrightnessMean = m_dMenuBrightness;
                //    bool bIsFault = false;

                //    if (m_dMenuBrightness < (double)DataTouchMenuKey.m_nBrightness_MenuKeyBrightnessLowSpec ||
                //        m_dMenuBrightness > (double)DataTouchMenuKey.m_nBrightness_MenuKeyBrightnessUpperSpec)
                //    {
                //        bIsFault = true;
                //        Console.WriteLine("[MenuProcessBrightness] lowspec : {1} upperspec : {2} calculated : {0}", m_dMenuBrightness,
                //            DataTouchMenuKey.m_nBrightness_MenuKeyBrightnessLowSpec,
                //            DataTouchMenuKey.m_nBrightness_MenuKeyBrightnessUpperSpec);
                //        Cv2.Rectangle(resultImg, menuAreaRect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                //    }
                //    CvScalar clr;
                //    if (bIsFault)
                //        clr = new CvScalar(0, 0, 255);
                //    else
                //        clr = new CvScalar(0, 255, 0);

                //    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                //    resultImg.SaveImage(@".\Detail\Touch\18.menu_bright_result.jpg");

                //    if (bIsFault)
                //    {
                //        ResultTouchMenuKey.imgNGBright = resultImg.Clone().ToBitmap();
                //        //Dust Mask Area
                //        //불량처리
                //    }


                //}

                #endregion

                // backkey menukey 밝기 평균값 비교
                //SUB_TOUCH_PROCESS_BRIGHTNESS_GAP
                #region SUB_TOUCH_PROCESS_BRIGHTNESS_GAP
                /*
                {
                    //Mat resultImg = ResultImg[(int)BMP.CAMERA_DATA_BLACK].Clone();
                    //Mat tmpImg = ResultImg[(int)BMP.CAMERA_DATA_BLACK].Clone();
                    //m_BlackTmpBack = new Mat(tmpImg, backAreaRect);
                    //m_BlackTmpMenu = new Mat(tmpImg, menuAreaRect);

                    //sh32.heo 수정
                    Mat resultImg = m_Touch_RotationImage.Clone();
                    m_BlackTmpMenu = originalImg[menuAreaRect].Clone();
                    m_BlackTmpBack = originalImg[backAreaRect].Clone();

                    m_BlackTmpBack.SaveImage(@".\Detail\Touch\6.BlackTmpBack.jpg");
                    m_BlackTmpMenu.SaveImage(@".\Detail\Touch\7.BlackTmpMenu.jpg");

                    double dBackMean = Cv2.Mean(m_BlackTmpBack, ResultImg[(int)BMP.CAMERA_DATA_TOUCH_BACK]).Val0;
                    double dMenuMean = Cv2.Mean(m_BlackTmpMenu, ResultImg[(int)BMP.CAMERA_DATA_TOUCH_MENU]).Val0;



                    bool bIsFault = false;

                    Console.WriteLine("mean:" + Math.Abs(dBackMean - dMenuMean));

                    ResultTouchKey.TouchKeyBrightnessJudgeGap = Math.Abs(dBackMean - dMenuMean);
                    if (Math.Abs(dBackMean - dMenuMean) > DataTouchKey.TouchKeyBrightnessGapAreaSpec)
                    {
                        bIsFault = true;
                        Cv2.Rectangle(resultImg, backAreaRect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                        Cv2.Rectangle(resultImg, menuAreaRect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    resultImg.SaveImage(@".\Detail\Touch\8.bightnessgap_result.jpg");


                }
                */
                #endregion
                // SUB_BACK_MATCHING_RATE
                // reference 이미지와 template 연산 
                // 일단 쓰지말자
                #region SUB_BACK_MATCHING_RATE
                /*
                {
                    string m_strTmp = @".\Reference\Black_Back_key_Reference.jpg";

                    Mat resultImg = m_Touch_RotationImage.Clone();
                    m_backKeyReferenceLed = Cv2.ImRead(m_strTmp);

                    if (m_backKeyReferenceLed.Rows != 0 && m_backKeyReferenceLed.Cols != 0)
                    {
                        if (m_backKeyReferenceLed.Channels() == 3)
                            Cv2.CvtColor(m_backKeyReferenceLed, m_backKeyReferenceLed, ColorConversion.BgrToGray);
                    }

                    Mat tmp = new Mat();
                    Mat m_backRefThres = new Mat();

                    Cv2.Threshold(m_BlackTmpBack, tmp, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);
                    Cv2.Threshold(m_backKeyReferenceLed, m_backRefThres, 0, 255.0, ThresholdType.Binary | ThresholdType.Otsu);

                    Cv2.Erode(m_backRefThres, m_backRefThres, new Mat(), new CvPoint(-1, -1), 1);
                    Cv2.Erode(tmp, tmp, new Mat(), new CvPoint(-1, -1), 1);

                    Mat referenceTmp = m_backRefThres.Clone();
                    Mat originTmp = tmp.Clone();
                    tmp.SaveImage(@".\Detail\Touch\15.tmp_image.jpg");
                    referenceTmp.SaveImage(@".\Detail\Touch\15.reference_image.jpg");

                    Mat[] reference_contours;
                    Mat[] origin_contours;

                    var hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(referenceTmp, out reference_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    hierarchy = OutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(originTmp, out origin_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    CvPoint rect_point = new CvPoint();

                    if (origin_contours.Length > 0)
                    {
                        rect_point = new CvPoint(Cv2.BoundingRect(reference_contours[0]).X - Cv2.BoundingRect(origin_contours[0]).X,
                            Cv2.BoundingRect(reference_contours[0]).Y - Cv2.BoundingRect(origin_contours[0]).Y);

                        Mat outTmp = Mat.Zeros(tmp.Size(), MatType.CV_8UC1);

                        if (rect_point.X > 0)
                            tmp[new CvRect(0, 0, tmp.Cols - rect_point.X, tmp.Rows)].CopyTo(outTmp[new CvRect(rect_point.X, 0, tmp.Cols - rect_point.X, tmp.Rows)]);
                        else
                        {
                            rect_point.X = Math.Abs(rect_point.X);
                            tmp[new CvRect(rect_point.X, 0, tmp.Cols - rect_point.X, tmp.Rows)].CopyTo(outTmp[new CvRect(0, 0, tmp.Cols - rect_point.X, tmp.Rows)]);
                        }
                        outTmp.SaveImage(@".\Detail\Touch\16.XTmpBack.jpg");

                        Mat mout = Mat.Zeros(tmp.Size(), MatType.CV_8UC1);

                        if (rect_point.Y > 0)
                            outTmp[new CvRect(0, 0, tmp.Cols, tmp.Rows - rect_point.Y)].CopyTo(mout[new CvRect(0, rect_point.Y, tmp.Cols, tmp.Rows - rect_point.Y)]);
                        else
                        {
                            rect_point.Y = Math.Abs(rect_point.Y);
                            outTmp[new CvRect(0, rect_point.Y, tmp.Cols, tmp.Rows - rect_point.Y)].CopyTo(mout[new CvRect(0, 0, tmp.Cols, tmp.Rows - rect_point.Y)]);
                        }

                        //reference 이미지 기준으로 이미지 shift
                        tmp = mout.Clone();

                        tmp.SaveImage(@".\Detail\Touch\16.blackTmpBack.jpg");
                    }
                    ///여기서 부터 다시
                    ///
                    int result_cols = tmp.Cols - m_backRefThres.Cols + 1;
                    int result_rows = tmp.Rows - m_backRefThres.Rows + 1;

                    Mat imgLogoMatch = new Mat();
                    imgLogoMatch.Create(result_rows, result_cols, MatType.CV_32FC1);
                    Cv2.MatchTemplate(tmp, m_backRefThres, imgLogoMatch, MatchTemplateMethod.CCoeffNormed);

                    double minVal;
                    double m_dMatchRate;

                    Cv2.MinMaxLoc(imgLogoMatch, out minVal, out m_dMatchRate);

                    bool bIsFault = false;
                    if (m_dMatchRate * 100 > DataTouchBackKey.m_nMatchingRate_BackKeyMatchingRateSpec)
                        bIsFault = false;
                    else
                    {
                        bIsFault = true;

                        CvRect rect = backAreaRect;
                        Cv2.Rectangle(resultImg, rect, new CvScalar(0, 0, 255), DataTouchKey.DefectBoxThickness);
                    }

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    resultImg.SaveImage(@".\Detail\Touch\17.matchingRate_result.jpg");
                }
                */
                #endregion

            }


        }
        public bool LedBlue(Mat refNormalImage, MData_LED_BLUE DataLedBlue, ref MResult_LED_BLUE Result)
        {
            Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");
            // Result = new MResult_LED_BLUE();
            using (Mat originalImg = refNormalImage.Clone())
            {
                //Tilt 된 Angle만큼 이미지 회전
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

                using (Mat m_Blue_Led_RotationImage = originalImg.Clone())
                using (Mat m_Blue_Led_Area = m_Blue_Led_RotationImage[ledAreaRect].Clone())
                {
                    //Display 영역 추출
                    if (Config.SaveDebugImage == true)
                        m_Blue_Led_Area.SaveImage(@".\Detail\Led Blue\1.BlueLed.jpg");

                    if (m_Blue_Led_Area.Channels() == 3)
                        Cv2.CvtColor(m_Blue_Led_Area, m_Blue_Led_Area, ColorConversion.BgrToGray);
                    else if (m_Blue_Led_Area.Channels() == 4)
                        Cv2.CvtColor(m_Blue_Led_Area, m_Blue_Led_Area, ColorConversion.BgraToGray);
                    if (Config.SaveDebugImage == true)
                        m_Blue_Led_Area.SaveImage(@".\Detail\Led Blue\2.BlueLedGray.jpg");

                    Mat resultImg = m_Blue_Led_RotationImage.Clone();
                    bool bIsFault = false;
                    
                    ////////////////////////////////////// DIFF WH ///////////////////////////////////////////////////////

                    resultImg = m_Blue_Led_RotationImage.Clone();

                    Mat tmptmp = psm.GetPsmImage(m_Blue_Led_Area, 8, 8, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    tmptmp.SaveImage(@".\Detail\Led Blue\3.led_psm.jpg");

                    Cv2.Threshold(tmptmp, tmptmp, DataLedBlue.DiffWH_BLFindBlackDotThreshold, 255, ThresholdType.Binary);
                    tmptmp.SaveImage(@".\Detail\Led Blue\4.led_psm+thres.jpg");

                    //결과이미지에 디버깅용 표시
                    CvRect tmpRect = new CvRect(ledAreaRect.X, ledAreaRect.Y - 200, ledAreaRect.Width, ledAreaRect.Height);
                    Cv2.CvtColor(tmptmp, tmptmp, ColorConversion.GrayToBgr);

                    tmptmp.CopyTo(resultImg[tmpRect]);
                    Cv2.CvtColor(tmptmp, tmptmp, ColorConversion.BgrToGray);

                    Mat tmp = new Mat();
                    Cv2.Threshold(m_Blue_Led_Area, tmp, 255, 255, ThresholdType.Otsu);
                    if (Config.SaveDebugImage == true)
                        tmp.SaveImage(@".\Detail\Led Blue\5.led_threshold.jpg");

                    Mat referTmp = tmp.Clone();

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    Point2f center = new Point2f();
                    float rad = 0;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        if (Cv2.ContourArea(contours[i]) > 100)
                        {
                            Cv2.MinEnclosingCircle(contours[i], out center, out rad);
                            Mat tmpCircle = Mat.Zeros(referTmp.Size(), referTmp.Type());
                            Cv2.Circle(tmpCircle, (int)center.X, (int)center.Y, (int)rad, new Scalar(255, 255, 255), -1);
                            if (Config.SaveDebugImage == true)
                                tmpCircle.SaveImage(@".\Detail\Led Blue\6.led_matchingrate_circle.jpg");

                            ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE] = tmpCircle.Clone();

                            Cv2.BitwiseAnd(tmpCircle, tmptmp, tmptmp);
                            if (Config.SaveDebugImage == true)
                                tmptmp.SaveImage(@".\Detail\Led Blue\7.led_psm+thres+and.jpg");


                            Mat[] offset_contours; ;
                            hierarchy = InputOutputArray.Create(new List<Vec4i>());
                            Cv2.FindContours(tmpCircle, out offset_contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                            for (int j = 0; j < offset_contours.Length; j++)
                            {
                                Cv2.DrawContours(tmptmp, offset_contours, j, Cv.ScalarAll(0), DataLedBlue.DiffWH_BLOffsetSize);
                            }
                            if (Config.SaveDebugImage == true)
                                tmptmp.SaveImage(@".\Detail\Led Blue\8.led_psm+thres+and+offset.jpg");
                        }
                    }


                    int iMax = 0;
                    CvRect rect;
                    double dContourArea = 0;
                    hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    Cv2.FindContours(tmptmp.Clone(), out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    for (int i = 0; i < contours.Length; i++)
                    {
                        dContourArea = Cv2.ContourArea(contours[i]);
                        rect = Cv2.BoundingRect(contours[i]);
                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.m_nDiffWH_BLDiffJudgeArea < iMax)
                            Result.m_nDiffWH_BLDiffJudgeArea = iMax;

                        if (iMax > DataLedBlue.DiffWH_BLJudgeSizeUL)
                        {
                            bIsFault = true;

                            break;
                        }
                    }

                    Cv2.CvtColor(tmptmp, tmptmp, ColorConversion.GrayToBgr);
                    tmpRect = new CvRect(ledAreaRect.X, ledAreaRect.Y - 100, ledAreaRect.Width, ledAreaRect.Height);
                    tmptmp.CopyTo(resultImg[tmpRect]);

                    double m_dblueLedBrightnessVal;
                    m_dblueLedBrightnessVal = Cv2.Mean(m_Blue_Led_Area, ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE]).Val0;

                    Result.m_dblueLedBrightnessVal = m_dblueLedBrightnessVal;

                    if (m_dblueLedBrightnessVal < DataLedBlue.Brightness_BLJudgeBrightLL
                        || m_dblueLedBrightnessVal > DataLedBlue.Brightness_BLJudgeBrightUL)
                    {
                        bIsFault = true;
                    }

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);


                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                    Cv2.Rectangle(resultImg, ledAreaRect, clr, 4);
                    string sTemp = string.Format("Brightness Measured: {0:F2}, LL : {1}, UL : {2}",
                        Result.m_dblueLedBrightnessVal, DataLedBlue.Brightness_BLJudgeBrightLL, DataLedBlue.Brightness_BLJudgeBrightUL);
                    Cv2.PutText(resultImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.White, 2);
                    sTemp = string.Format("DiffJudge Size Measured : {0}, UL : {1}", Result.m_nDiffWH_BLDiffJudgeArea, DataLedBlue.DiffWH_BLJudgeSizeUL);
                    Cv2.PutText(resultImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.White, 2);

                    Result.ImageResult = resultImg.Clone();
                    if (Config.SaveDebugImage == true)
                    {
                        resultImg.SaveImage(@".\Detail\Led Blue\10.led_diffwh_result.jpg");
                    }
                    resultImg.Dispose();


                    #region Colorbalance
                    /*
                    ////////////////////////////////////////////////////////////////////////////////////////
                    {
                        Mat resultImg = m_Blue_Led_RotationImage.Clone();
                        Mat[] split_blue_ledImg;
                        List<Mat> blue_ledSplit = new List<Mat>();
                        double m_dblueLedColorBalanceVal;

                        Cv2.Split(m_Blue_Led_RotationImage, out split_blue_ledImg);

                        for (int i = 0; i < MAX_CHANNEL; i++)
                        {
                            blue_ledSplit.Add(new Mat(split_blue_ledImg[i], ledAreaRect));
                        }

                        Mat tmp = blue_ledSplit[BLUE_CHANNEL] / (blue_ledSplit[BLUE_CHANNEL] + blue_ledSplit[RED_CHANNEL] + blue_ledSplit[GREEN_CHANNEL]) * 100;
                        m_dblueLedColorBalanceVal = Cv2.Mean(tmp, ResultImg[(int)BMP.CAMERA_DATA_SVC_WHITE]).Val0;

                        bool bIsFault = false;

                        if (m_dblueLedColorBalanceVal < DataLedBlue.m_nColorRate_BLJudgeColorRateLL
                            || m_dblueLedColorBalanceVal > DataLedBlue.m_nColorRate_BLJudgeColorRateUL)
                        {
                            bIsFault = true;
                            Cv2.Rectangle(resultImg, ledAreaRect, new CvScalar(255, 0, 0), 4);
                        }

                        CvScalar clr;
                        if (bIsFault)
                            clr = new CvScalar(0, 0, 255);
                        else
                            clr = new CvScalar(0, 255, 0);

                        

                        string sTemp = string.Format("ColorBalance Value : LL({1}) < {0:F2} < UL({2})", m_dblueLedColorBalanceVal, DataLedBlue.m_nColorRate_BLJudgeColorRateLL, DataLedBlue.m_nColorRate_BLJudgeColorRateUL);
                        Cv2.PutText(resultImg, sTemp, new CvPoint(700, 450), FontFace.HersheyComplex, 2, Scalar.White, 2);

                        Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
                        resultImg.SaveImage(@".\Detail\Led Blue\4.led_colorrate.jpg");


                        //sTemp = string.Format("LL({1})<Value<Step3({2}) Count : {0} ) ", Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt,
                        //    100 + DataLcdReddish.ColorBalance_Step_2, 100 + DataLcdReddish.ColorBalance_Step_3);
                        //Cv2.PutText(resultImg, sTemp, new CvPoint(700, 550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                        //sTemp = string.Format("Step3({1})<Value Count : {0} ", Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt,
                        //    100 + DataLcdReddish.ColorBalance_Step_3);
                        //Cv2.PutText(resultImg, sTemp, new CvPoint(700, 650), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                        //sTemp = string.Format("ColorBalance Avg Top : {0:F2}, Top Offset : {1}", dColorBalanceAvgTop, Config.ReddishOffset_Top);
                        //Cv2.PutText(resultImg, sTemp, new CvPoint(700, 750), FontFace.HersheyComplex, 2, Scalar.Blue, 2);



                    }
                    */

                    #endregion
                    #region SUB_BLUE_LED_OVER_BRIGHT_AREA
                    /*
                    {
                        Mat resultImg = m_Blue_Led_RotationImage.Clone();
                        Mat tmp = new Mat();

                        //1. 절대 값 threshold 이상의 pixel 갯수 카운트 해서 spec over 면 Fail
                        Cv2.Threshold(m_Blue_Led_Area, tmp, DataLedBlue.m_nOverBrightArea_BLOverThreshold, 255, ThresholdType.Binary);
                        tmp.SaveImage(@".\Detail\Led Blue\11.led_overbrightarea_threshold.jpg");

                        Mat[] contours;
                        var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                        double dArea = 0;
                        CvRect rect_1 = new CvRect();
                        double iMax_1 = 0;

                        Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                        for (int i = 0; i < contours.Length; i++)
                        {
                            dArea += Cv2.ContourArea(contours[i]);
                            //rect_1 = Cv2.BoundingRect(contours[i]);
                            //iMax_1 = rect_1.Width > rect_1.Height ? rect_1.Width : rect_1.Height;
                        }

                        bool bIsFault = false;


                        Result.m_dOverBrightArea = dArea;

                        if (dArea > DataLedBlue.m_nOverBrightArea_BLJudgeOverAreaUL)
                        {
                            bIsFault = true;
                            Cv2.Rectangle(resultImg, ledAreaRect, new CvScalar(0, 0, 255), 4);
                        }

                        CvScalar clr;
                        if (bIsFault)
                            clr = new CvScalar(0, 0, 255);
                        else
                            clr = new CvScalar(0, 255, 0);

                        Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                        resultImg.SaveImage(@".\Detail\Led Blue\12.led_overbrightarea_result.jpg");
                    }
                     * */
                    #endregion
                    return true;
                }
            }
        }
        public void LcdReddish(Mat refNormalImage, MData_LCD_REDDISH DataLcdReddish, ref MResult_LCD_REDDISH Result)
        {
			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");
            List<double> ListColorBalanceTop = new List<double>();
            List<double> ListColorBalanceBottom = new List<double>();
            List<double> ListColorBalanceLeft = new List<double>();
            List<double> ListColorBalanceRight = new List<double>();
			Mat originalImg = refNormalImage.Clone();
			bool m_bGreenish = false;

            if (originalImg.Channels() == 3)
                Cv2.CvtColor(originalImg, originalImg, ColorConversion.BgrToGray);
            else if (originalImg.Channels() == 4)
                Cv2.CvtColor(originalImg, originalImg, ColorConversion.BgraToGray);
            Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

            using (Mat m_WhiteRotationImage = originalImg.Clone())
            {
                Mat[] white_splitImgs;
                List<Mat> white_splitImg = new List<Mat>();
                Cv2.Split(m_WhiteRotationImage, out white_splitImgs);

                CvRect tempRect = new CvRect(0, 0, m_displayWidth + 20, m_displayHeight + 20);
                CvRect tempRect2 = new CvRect(m_displayX - 10, m_displayY - 10, m_displayWidth + 20, m_displayHeight + 20);

                Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                Mat blueImg = new Mat(white_splitImgs[BLUE_CHANNEL], tempRect2);
                Mat greenImg = new Mat(white_splitImgs[GREEN_CHANNEL], tempRect2);
                Mat redImg = new Mat(white_splitImgs[RED_CHANNEL], tempRect2);

                maskROI.SaveImage(@".\Detail\Lcd Reddish\1.maskroi.jpg");
                blueImg.SaveImage(@".\Detail\Lcd Reddish\2.blueImg.jpg");
                greenImg.SaveImage(@".\Detail\Lcd Reddish\3.greenImg.jpg");
                redImg.SaveImage(@".\Detail\Lcd Reddish\4.redImg.jpg");

                white_splitImg.Clear();

                /////////////원본 white_splitImg -> white_disSplit로 변경 (list사용)
                white_splitImg.Add(blueImg);
                white_splitImg.Add(greenImg);
                white_splitImg.Add(redImg);
                white_splitImg.Add(maskROI);

                Mat resultImg = m_WhiteRotationImage.Clone();

                double meanVal = 0.0;
                double meanVal_R, meanVal_G, meanVal_B;
                string sColorShading;
                double m_dColorBalanceRG_TB = .0;
                double m_dColorBalanceRG_LR = .0;
                double m_dColorBalanceRG_Center = .0;

                int m_iColorBalanceLRStep = 1;
                int m_iColorBalanceTBStep = 1;
                int m_iColorBalanceCenterStep = 1;
                int m_iColorBalanceStep = 1;

                int iColWidth = white_splitImg[BLUE_CHANNEL].Cols / DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt; //// 이미지를 70 등분 함..
                int iRowHeight = (white_splitImg[BLUE_CHANNEL].Rows - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area)) / DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt;

                Mat tmp;
                Mat tmpMask;
                Scalar clr;

                for (int i = 0; i < 5; i++)
                {
                    if (i == 0) // Top Area
                    {
                        m_bGreenish = false;
                        for (int cc = 0; cc < DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt; cc++)
                        {
                            tmp = white_splitImg[BLUE_CHANNEL][new CvRect(cc * iColWidth, DataLcdReddish.ColorShading_Exception_Area,
                            iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[BLUE_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                            DataLcdReddish.ColorShading_TB_Area)].Clone();

                            tmpMask = white_splitImg[MAX_CHANNEL][new CvRect(cc * iColWidth, DataLcdReddish.ColorShading_Exception_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[MAX_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_B = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[GREEN_CHANNEL][new CvRect(cc * iColWidth, DataLcdReddish.ColorShading_Exception_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[GREEN_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_G = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[RED_CHANNEL][new CvRect(cc * iColWidth, DataLcdReddish.ColorShading_Exception_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[RED_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_R = Cv2.Mean(tmp, tmpMask).Val0;

                            meanVal = (meanVal_R / meanVal_G) * 100;
                            meanVal = meanVal + Config.ReddishOffset_Top;
                            ListColorBalanceTop.Add(meanVal);
                            if (m_bGreenish)
                                continue;

                            sColorShading = meanVal.ToString("##.##");

                            if (meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal > DataLcdReddish.ColorShading_RG_TB_Lower_Rate)
                                clr = Scalar.Green;
                            else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                clr = Scalar.OrangeRed;
                            else
                                clr = Scalar.Red;

                            Cv2.PutText(resultImg, sColorShading, new CvPoint(m_displayX + cc * iColWidth, 50 + m_displayY + DataLcdReddish.ColorShading_Exception_Area), FontFace.HersheyComplex, fontScale, clr, 2);
                            Cv2.Rectangle(resultImg, new CvRect(m_displayX + cc * iColWidth, m_displayY + DataLcdReddish.ColorShading_Exception_Area,
                                           iColWidth, DataLcdReddish.ColorShading_TB_Area), clr, 4);

                            if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate)
                            {
                                if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt++;
                                }
                                else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2 
                                    && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt++;
                                }

                                if (m_dColorBalanceRG_TB < meanVal)
                                    m_dColorBalanceRG_TB = meanVal;
                                else
                                {
                                    continue;
                                }

                                if(Result.m_iWhiteColorBalanceRG_Max < m_dColorBalanceRG_TB)
                                {
                                    Result.m_iWhiteColorBalanceRG_Max = m_dColorBalanceRG_TB;
                                }
                                Result.m_iWhiteColorBalanceRG_TB = m_dColorBalanceRG_TB;

                                if (m_dColorBalanceRG_TB > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    m_iColorBalanceTBStep = 3;
                                    Result.m_bWhiteColorBalanceRG_TB_Result = false;
                                }
                            }
                            else if (meanVal < DataLcdReddish.ColorShading_RG_TB_Lower_Rate)
                            {
                                m_bGreenish = true;
                                m_dColorBalanceRG_TB = meanVal;
                                Result.m_iWhiteColorBalanceRG_TB = m_dColorBalanceRG_TB;
                                Result.m_bWhiteColorBalanceRG_TB_Result = false;
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        m_bGreenish = false;
                        for (int cc = 0; cc < DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt; cc++)
                        {
                            tmp = white_splitImg[BLUE_CHANNEL][new CvRect(cc * iColWidth, m_displayHeight - DataLcdReddish.ColorShading_Exception_Area - DataLcdReddish.ColorShading_TB_Area,
                            iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[BLUE_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                            DataLcdReddish.ColorShading_TB_Area)].Clone();

                            tmpMask = white_splitImg[MAX_CHANNEL][new CvRect(cc * iColWidth, m_displayHeight - DataLcdReddish.ColorShading_Exception_Area - DataLcdReddish.ColorShading_TB_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[MAX_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_B = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[GREEN_CHANNEL][new CvRect(cc * iColWidth, m_displayHeight - DataLcdReddish.ColorShading_Exception_Area - DataLcdReddish.ColorShading_TB_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[GREEN_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_G = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[RED_CHANNEL][new CvRect(cc * iColWidth, m_displayHeight - DataLcdReddish.ColorShading_Exception_Area - DataLcdReddish.ColorShading_TB_Area,
                                iColWidth + (cc == DataLcdReddish.AreaColorDiffBlue_WColumnSplitCnt - 1 ? white_splitImg[RED_CHANNEL].Cols - cc * iColWidth - iColWidth : 0),
                                DataLcdReddish.ColorShading_TB_Area)].Clone();

                            meanVal_R = Cv2.Mean(tmp, tmpMask).Val0;

                            meanVal = (meanVal_R / meanVal_G) * 100;
                            meanVal = meanVal + Config.ReddishOffset_Bottom;
                            ListColorBalanceBottom.Add(meanVal);
                            if (m_bGreenish)
                                continue;
                            sColorShading = meanVal.ToString("##.##");

                            if (meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal > DataLcdReddish.ColorShading_RG_TB_Lower_Rate)
                                clr = Scalar.Green;
                            else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                clr = Scalar.OrangeRed;
                            else
                                clr = Scalar.Red;

                            Cv2.PutText(resultImg, sColorShading, new CvPoint(m_displayX + cc * iColWidth
                                , 50 + m_displayY + m_displayHeight - DataLcdReddish.ColorShading_Exception_Area
                                        - DataLcdReddish.ColorShading_TB_Area)
                                        , FontFace.HersheyComplex, fontScale, clr, 2);
                            Cv2.Rectangle(resultImg, new CvRect(m_displayX + cc * iColWidth, m_displayY + m_displayHeight - DataLcdReddish.ColorShading_Exception_Area - DataLcdReddish.ColorShading_TB_Area,
                                        iColWidth, DataLcdReddish.ColorShading_TB_Area), clr, 4);
                            if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate)
                            {
                                if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt++;
                                }
                                else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                    && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt++;
                                }

                                if (m_dColorBalanceRG_TB < meanVal)
                                    m_dColorBalanceRG_TB = meanVal;
                                else
                                {
                                    continue;
                                }
                                if (Result.m_iWhiteColorBalanceRG_Max < m_dColorBalanceRG_TB)
                                {
                                    Result.m_iWhiteColorBalanceRG_Max = m_dColorBalanceRG_TB;
                                }
                                Result.m_iWhiteColorBalanceRG_TB = m_dColorBalanceRG_TB;

                                if (m_dColorBalanceRG_TB > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    m_iColorBalanceTBStep = 3;
                                    Result.m_bWhiteColorBalanceRG_TB_Result = false;
                                }
                            }
                            else if (meanVal < DataLcdReddish.ColorShading_RG_TB_Lower_Rate)
                            {
                                m_bGreenish = true;
                                m_dColorBalanceRG_TB = meanVal;
                                Result.m_iWhiteColorBalanceRG_TB = m_dColorBalanceRG_TB;
                                Result.m_bWhiteColorBalanceRG_TB_Result = false;
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        m_bGreenish = false;
                        for (int rr = 0; rr < DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt; rr++)
                        {
                            tmp = white_splitImg[BLUE_CHANNEL][new CvRect(0, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[BLUE_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            tmpMask = white_splitImg[MAX_CHANNEL][new CvRect(0, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[MAX_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_B = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[GREEN_CHANNEL][new CvRect(0, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[GREEN_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_G = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[RED_CHANNEL][new CvRect(0, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                                                        DataLcdReddish.ColorShading_LR_Area,
                                                        iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[RED_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_R = Cv2.Mean(tmp, tmpMask).Val0;

                            meanVal = (meanVal_R / meanVal_G) * 100;
                            meanVal = meanVal + Config.ReddishOffset_Left;
                            ListColorBalanceLeft.Add(meanVal);
                            if (m_bGreenish)
                                continue;
                            sColorShading = meanVal.ToString("##.##");
                            if (meanVal < DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal > DataLcdReddish.ColorShading_RG_LR_Lower_Rate)
                                clr = Scalar.Green;
                            else if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal < DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                clr = Scalar.OrangeRed;
                            else
                                clr = Scalar.Red;

                            Cv2.PutText(resultImg, sColorShading
                                , new CvPoint(m_displayX, 50 + m_displayY + DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight)
                                , FontFace.HersheyComplex, fontScale, clr, 2);
                            Cv2.Rectangle(resultImg, new CvRect(m_displayX, m_displayY + DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                                       DataLcdReddish.ColorShading_LR_Area, iRowHeight), clr, 4);
                            if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate)
                            {
                                if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt++;
                                }
                                else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                    && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt++;
                                }

                                if (m_dColorBalanceRG_LR < meanVal)
                                    m_dColorBalanceRG_LR = meanVal;
                                else
                                {
                                    continue;
                                }
                                if (Result.m_iWhiteColorBalanceRG_Max < m_dColorBalanceRG_LR)
                                {
                                    Result.m_iWhiteColorBalanceRG_Max = m_dColorBalanceRG_LR;
                                }
                                Result.m_iWhiteColorBalanceRG_LR = m_dColorBalanceRG_LR;

                                if (m_dColorBalanceRG_LR > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    m_iColorBalanceLRStep = 3;
                                    Result.m_bWhiteColorBalanceRG_LR_Result = false;
                                }
                            }
                            else if (meanVal < DataLcdReddish.ColorShading_RG_LR_Lower_Rate)
                            {
                                m_bGreenish = true;
                                m_dColorBalanceRG_LR = meanVal;
                                Result.m_iWhiteColorBalanceRG_LR = m_dColorBalanceRG_LR;

                                Result.m_bWhiteColorBalanceRG_LR_Result = false;
                            }
                        }
                    }
                    else if (i == 3)
                    {
                        m_bGreenish = false;
                        for (int rr = 0; rr < DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt; rr++)
                        {
                            tmp = white_splitImg[BLUE_CHANNEL][new CvRect(m_displayWidth - DataLcdReddish.ColorShading_LR_Area, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[BLUE_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            tmpMask = white_splitImg[MAX_CHANNEL][new CvRect(m_displayWidth - DataLcdReddish.ColorShading_LR_Area, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[MAX_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_B = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[GREEN_CHANNEL][new CvRect(m_displayWidth - DataLcdReddish.ColorShading_LR_Area, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                            DataLcdReddish.ColorShading_LR_Area,
                            iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[GREEN_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_G = Cv2.Mean(tmp, tmpMask).Val0;

                            tmp = white_splitImg[RED_CHANNEL][new CvRect(m_displayWidth - DataLcdReddish.ColorShading_LR_Area, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                                                        DataLcdReddish.ColorShading_LR_Area,
                                                        iRowHeight + (rr == DataLcdReddish.AreaColorDiffBlue_WRowSplitCnt - 1 ? white_splitImg[RED_CHANNEL].Rows - rr * iRowHeight - iRowHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area) : 0))].Clone();

                            meanVal_R = Cv2.Mean(tmp, tmpMask).Val0;

                            meanVal = (meanVal_R / meanVal_G) * 100;
                            meanVal = meanVal + Config.ReddishOffset_Right;
                            ListColorBalanceRight.Add(meanVal);

                            if (m_bGreenish)
                                continue;

                            sColorShading = meanVal.ToString("##.##");
                            if (meanVal < DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                         && meanVal > DataLcdReddish.ColorShading_RG_LR_Lower_Rate)
                                clr = Scalar.Green;
                            else if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal < DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                clr = Scalar.OrangeRed;
                            else
                                clr = Scalar.Red;

                            Cv2.PutText(resultImg, sColorShading
                                , new CvPoint(m_displayX + m_displayWidth - DataLcdReddish.ColorShading_LR_Area, 50 + m_displayY + DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight)
                                , FontFace.HersheyComplex, fontScale, clr, 2);
                            Cv2.Rectangle(resultImg, new CvRect(m_displayX + m_displayWidth - DataLcdReddish.ColorShading_LR_Area, m_displayY + DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area + rr * iRowHeight,
                                        DataLcdReddish.ColorShading_LR_Area, iRowHeight), clr, 4);
                            if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate)
                            {
                                if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt++;
                                }
                                else if (meanVal > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                    && meanVal < DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt++;
                                }

                                if (m_dColorBalanceRG_LR < meanVal)
                                    m_dColorBalanceRG_LR = meanVal;
                                else
                                {
                                    continue;
                                }
                                if (Result.m_iWhiteColorBalanceRG_Max < m_dColorBalanceRG_LR)
                                {
                                    Result.m_iWhiteColorBalanceRG_Max = m_dColorBalanceRG_LR;
                                }
                                Result.m_iWhiteColorBalanceRG_LR = m_dColorBalanceRG_LR;

                                if (m_dColorBalanceRG_LR > DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                                {
                                    m_iColorBalanceLRStep = 3;
                                    Result.m_bWhiteColorBalanceRG_LR_Result = false;
                                }
                            }
                            else if (meanVal < DataLcdReddish.ColorShading_RG_LR_Lower_Rate)
                            {
                                m_bGreenish = true;
                                m_dColorBalanceRG_LR = meanVal;
                                Result.m_iWhiteColorBalanceRG_LR = m_dColorBalanceRG_LR;
                                Result.m_bWhiteColorBalanceRG_LR_Result = false;
                            }
                        }
                    }
                    else
                    {
                        m_bGreenish = false;

                        CvRect rect = new CvRect(DataLcdReddish.ColorShading_LR_Area, DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area, m_displayWidth - 2 * DataLcdReddish.ColorShading_LR_Area, m_displayHeight - 2 * (DataLcdReddish.ColorShading_Exception_Area + DataLcdReddish.ColorShading_TB_Area));
                        tmp = white_splitImg[BLUE_CHANNEL][rect].Clone();
                        tmpMask = white_splitImg[MAX_CHANNEL][rect].Clone();

                        meanVal_B = Cv2.Mean(tmp, tmpMask).Val0;

                        tmp = white_splitImg[GREEN_CHANNEL][rect].Clone();
                        meanVal_G = Cv2.Mean(tmp, tmpMask).Val0;

                        tmp = white_splitImg[RED_CHANNEL][rect].Clone();
                        meanVal_R = Cv2.Mean(tmp, tmpMask).Val0;

                        meanVal = (meanVal_R / meanVal_G) * 100;
                        meanVal = meanVal + Config.ReddishOffset_Center;

                        if (m_bGreenish)
                            continue;
                        sColorShading = meanVal.ToString("##.##");

                        if (meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal > DataLcdReddish.ColorShading_RG_TB_Lower_Rate)
                            clr = Scalar.Green;
                        else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                            && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                            clr = Scalar.OrangeRed;
                        else
                            clr = Scalar.Red;

                        Cv2.PutText(resultImg, sColorShading, new CvPoint(m_displayX + rect.Width / 2, 50 + m_displayY + rect.Height / 2)
                            , FontFace.HersheyComplex, fontScale, clr, 2);
                        Cv2.Rectangle(resultImg, new CvRect(m_displayX + rect.X, m_displayY + rect.Y, rect.Width, rect.Height), clr, 4);
                        if (meanVal > DataLcdReddish.ColorShading_RG_Center_Upper_Rate)
                        {
                            if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                            {
                                Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt++;
                            }
                            else if (meanVal > DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2
                                && meanVal < DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                            {
                                Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt++;
                            }

                            if (m_dColorBalanceRG_Center < meanVal)
                                m_dColorBalanceRG_Center = meanVal;
                            else
                            {
                                continue;
                            }
                            if (Result.m_iWhiteColorBalanceRG_Max < m_dColorBalanceRG_Center)
                            {
                                Result.m_iWhiteColorBalanceRG_Max = m_dColorBalanceRG_Center;
                            }
                            Result.m_iWhiteColorBalanceRG_Center = m_dColorBalanceRG_Center;

                            if (m_dColorBalanceRG_Center > DataLcdReddish.ColorShading_RG_Center_Upper_Rate + DataLcdReddish.ColorBalance_Step_3)
                            {
                                m_iColorBalanceCenterStep = 3;
                                Result.m_bWhiteColorBalanceRG_Center_Result = false;
                            }
                        }
                        else if (meanVal < DataLcdReddish.ColorShading_RG_Center_Lower_Rate)
                        {
                            m_bGreenish = true;
                            m_dColorBalanceRG_Center = meanVal;
                            Result.m_iWhiteColorBalanceRG_Center = m_dColorBalanceRG_Center;
                            Result.m_bWhiteColorBalanceRG_Center_Result = false;
                        }
                    }
                }

                double dColorBalanceSumTop = 0;
                double dColorBalanceAvgTop = 0;
                double dColorBalanceSumBottom = 0;
                double dColorBalanceAvgBottom = 0;
                double dColorBalanceSumLeft = 0;
                double dColorBalanceAvgLeft = 0;
                double dColorBalanceSumRight = 0;
                double dColorBalanceAvgRight = 0;

                foreach (double d in ListColorBalanceTop)
                {
                    dColorBalanceSumTop += d;
                }
                foreach (double d in ListColorBalanceBottom)
                {
                    dColorBalanceSumBottom += d;
                }
                foreach (double d in ListColorBalanceLeft)
                {
                    dColorBalanceSumLeft += d;
                }
                foreach (double d in ListColorBalanceRight)
                {
                    dColorBalanceSumRight += d;
                }

                dColorBalanceAvgTop = dColorBalanceSumTop / ListColorBalanceTop.Count;
                dColorBalanceAvgBottom = dColorBalanceSumBottom / ListColorBalanceBottom.Count;
                dColorBalanceAvgLeft = dColorBalanceSumLeft / ListColorBalanceLeft.Count;
                dColorBalanceAvgRight = dColorBalanceSumRight / ListColorBalanceRight.Count;

                string sTemp = string.Format("Maximum Value: {0:F2}", Result.m_iWhiteColorBalanceRG_Max);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700,450), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("Step2({1})<Value<Step3({2}) Count : {0} ) ", Result.m_iWhiteColorBalanceRG_OverStep_2_RegionCnt,
                    100 + DataLcdReddish.ColorBalance_Step_2, 100 + DataLcdReddish.ColorBalance_Step_3);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700,550), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("Step3({1})<Value Count : {0} ", Result.m_iWhiteColorBalanceRG_OverStep_3_RegionCnt,
                    100 + DataLcdReddish.ColorBalance_Step_3);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 650), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("ColorBalance Avg Top : {0:F2}, Top Offset : {1}", dColorBalanceAvgTop,Config.ReddishOffset_Top);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 750), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("ColorBalance Avg Bottom : {0:F2}, Bottom Offset : {1}", dColorBalanceAvgBottom, Config.ReddishOffset_Bottom);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 850), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("ColorBalance Avg Left : {0:F2}, Left Offset : {1}", dColorBalanceAvgLeft, Config.ReddishOffset_Left);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 950), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("ColorBalance Avg Right : {0:F2}, Right Offset : {1}", dColorBalanceAvgRight, Config.ReddishOffset_Right);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 1050), FontFace.HersheyComplex, 2, Scalar.Blue, 2);
                sTemp = string.Format("Center Offset : {0}", Config.ReddishOffset_Center);
                Cv2.PutText(resultImg, sTemp, new CvPoint(700, 1150), FontFace.HersheyComplex, 2, Scalar.Blue, 2);

                if (Result.m_bWhiteColorBalanceRG_TB_Result && Result.m_bWhiteColorBalanceRG_LR_Result && Result.m_bWhiteColorBalanceRG_Center_Result)
                {
                    if (Result.m_bWhiteColorBalanceRG_TB_Result)
                    {
                        if (Result.m_iWhiteColorBalanceRG_TB <= DataLcdReddish.ColorShading_RG_TB_Upper_Rate)
                        {
                            m_iColorBalanceTBStep = 1;
                        }
                        else if (Result.m_iWhiteColorBalanceRG_TB > DataLcdReddish.ColorShading_RG_TB_Upper_Rate && Result.m_iWhiteColorBalanceRG_TB <= DataLcdReddish.ColorShading_RG_TB_Upper_Rate + DataLcdReddish.ColorBalance_Step_2)
                        {
                            m_iColorBalanceTBStep = 2;
                            Result.m_iWhiteColorBalanceRG_TB = Result.m_iWhiteColorBalanceRG_TB - DataLcdReddish.ColorBalance_Step_2;
                        }
                        else
                        {
                            m_iColorBalanceTBStep = 3;
                            Result.m_iWhiteColorBalanceRG_TB = Result.m_iWhiteColorBalanceRG_TB - DataLcdReddish.ColorBalance_Step_3;
                        }
                    }

                    if (Result.m_bWhiteColorBalanceRG_LR_Result)
                    {
                        if (Result.m_iWhiteColorBalanceRG_LR <= DataLcdReddish.ColorShading_RG_LR_Upper_Rate)
                        {
                            m_iColorBalanceLRStep = 1;
                        }
                        else if (Result.m_iWhiteColorBalanceRG_LR > DataLcdReddish.ColorShading_RG_LR_Upper_Rate && Result.m_iWhiteColorBalanceRG_LR <= DataLcdReddish.ColorShading_RG_LR_Upper_Rate + DataLcdReddish.ColorBalance_Step_2)
                        {
                            m_iColorBalanceLRStep = 2;
                            Result.m_iWhiteColorBalanceRG_LR = Result.m_iWhiteColorBalanceRG_LR - DataLcdReddish.ColorBalance_Step_2;
                        }
                        else
                        {
                            m_iColorBalanceLRStep = 3;
                            Result.m_iWhiteColorBalanceRG_LR = Result.m_iWhiteColorBalanceRG_LR - DataLcdReddish.ColorBalance_Step_3;
                        }
                    }
                    if (Result.m_bWhiteColorBalanceRG_Center_Result)
                    {
                        if (Result.m_iWhiteColorBalanceRG_Center <= DataLcdReddish.ColorShading_RG_Center_Upper_Rate)
                        {
                            m_iColorBalanceCenterStep = 1;
                        }
                        else if (Result.m_iWhiteColorBalanceRG_Center > DataLcdReddish.ColorShading_RG_Center_Upper_Rate && Result.m_iWhiteColorBalanceRG_Center <= DataLcdReddish.ColorShading_RG_Center_Upper_Rate + DataLcdReddish.ColorBalance_Step_2)
                        {
                            m_iColorBalanceCenterStep = 2;
                            Result.m_iWhiteColorBalanceRG_Center = Result.m_iWhiteColorBalanceRG_Center - DataLcdReddish.ColorBalance_Step_2;
                        }
                        else
                        {
                            m_iColorBalanceCenterStep = 3;
                            Result.m_iWhiteColorBalanceRG_Center = Result.m_iWhiteColorBalanceRG_Center - DataLcdReddish.ColorBalance_Step_3;
                        }
                    }
                }
                if (m_iColorBalanceTBStep > m_iColorBalanceLRStep)
                    m_iColorBalanceStep = m_iColorBalanceTBStep;
                else
                    m_iColorBalanceStep = m_iColorBalanceLRStep;

                if (m_iColorBalanceStep < m_iColorBalanceCenterStep)
                    m_iColorBalanceStep = m_iColorBalanceCenterStep;

                CvScalar cvclr;
                if (Result.m_bWhiteColorBalanceRG_LR_Result && Result.m_bWhiteColorBalanceRG_TB_Result && Result.m_bWhiteColorBalanceRG_Center_Result)
                    cvclr = new CvScalar(0, 255, 0);
                else
                    cvclr = new CvScalar(0, 0, 255);

                Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), cvclr, 4 * 5);
                resultImg.SaveImage(@".\Detail\Lcd Reddish\5.resultImg.jpg");
            }
        }
        public void LcdMCD(Mat refNormalImage, MData_LCD_MCD DataLcdMcd, ref MResult_LCD_MCD Result)
        {
            Console.WriteLine("start lcdmcd");
            using (Mat originalImg = refNormalImage.Clone())
            {
                //Tilt 된 Angle만큼 이미지 회전
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());

                using (Mat m_MCDRotationImage = originalImg.Clone())
                using (Mat m_MCDDisplayArea = m_MCDRotationImage[displayAreaRect].Clone())
                {
                    //Display 영역 추출
                    if (Config.SaveDebugImage) { m_MCDDisplayArea.SaveImage(@".\Detail\Lcd MCD\1.Display.jpg"); }

                    if (m_MCDDisplayArea.Channels() == 3)
                        Cv2.CvtColor(m_MCDDisplayArea, m_MCDDisplayArea, ColorConversion.BgrToGray);
                    if (Config.SaveDebugImage)
                    {
                        m_MCDDisplayArea.SaveImage(@".\Detail\Lcd MCD\2.Display_Gray.jpg");
                    }
                    Mat tmpImg = new Mat();
                    Mat resultImg = m_MCDRotationImage.Clone();

                    Cv2.Threshold(m_MCDDisplayArea, tmpImg, DataLcdMcd.BrightLine_McdThreshold, 255, ThresholdType.Binary);
                    if (Config.SaveDebugImage)
                    {
                        tmpImg.SaveImage(@".\Detail\Lcd MCD\3.brightarea_threshold.jpg");
                    }
                    Mat tmptmp = new Mat();
                    tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    tmpImg.CopyTo(tmptmp);

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);
                    CvRect rect = new CvRect();

                    int iTotalMaxVal = 0;
                    double dArea;
                    int iMax = 0;
                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.BrightLine_McdJudgeSizeLL < iMax)
                            Result.BrightLine_McdJudgeSizeLL = iMax;

                        if (iMax > DataLcdMcd.BrightLine_McdJudgeSizeLL)
                        {
                            bIsFault = true;
                            iTotalMaxVal += (int)dArea;

                            rect.X = rect.X + 80;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y + 100;
                            rect.Height = rect.Height + 60;

                            Cv2.Rectangle(resultImg, rect, new Scalar(255, 255, 255), 4);
                        }
                    }
                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(40, 40, resultImg.Cols - 40 * 2, resultImg.Rows - 40 * 2), clr, 4 * 5);
						Result.ImageResult = resultImg.Clone();

                    if (Config.SaveDebugImage)
                    {
                        resultImg.SaveImage(@".\Detail\Lcd MCD\4.brightdotwh_result.jpg");
                    }
                    resultImg.Dispose();
                }
            }
        }

        public void LcdCopCrack(Mat refNormalImage, MData_LCD_COPCRACK DataLcdCopcrack, ref MResult_LCD_COPCRACK Result)
        {
			Log.AddLog(MethodBase.GetCurrentMethod().Name + ", Started");
            using (Mat originalImg = refNormalImage.Clone())
            {
                Cv2.WarpAffine(refNormalImage, originalImg, angleMat, refNormalImage.Size());
                using (Mat m_CopcrackRotationImage = originalImg.Clone())
                using (Mat m_CopcrackDisplayArea = m_CopcrackRotationImage[displayAreaRect].Clone())
                {
                    if (Config.SaveDebugImage == true)
                        m_CopcrackDisplayArea.SaveImage(@".\Detail\Lcd Copcrack\1.Display.jpg");

                    if (m_CopcrackDisplayArea.Channels() == 3)
                        Cv2.CvtColor(m_CopcrackDisplayArea, m_CopcrackDisplayArea, ColorConversion.BgrToGray);
                    else if (m_CopcrackDisplayArea.Channels() == 4)
                        Cv2.CvtColor(m_CopcrackDisplayArea, m_CopcrackDisplayArea, ColorConversion.BgraToGray);
                    Cv2.BitwiseAnd(m_CopcrackDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_AREA], m_CopcrackDisplayArea);
                    if (Config.SaveDebugImage == true)
                        m_CopcrackDisplayArea.SaveImage(@".\Detail\Lcd Copcrack\2.Display_Gray.jpg");


                    Mat tmpImg = new Mat();
                    Mat resultImg = m_CopcrackRotationImage.Clone();

                    Cv2.AdaptiveThreshold(m_CopcrackDisplayArea, tmpImg, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv,
                        DataLcdCopcrack.BlackDot_BFindBlackDotBlockSize, DataLcdCopcrack.BlackDot_BFindBlackDotThreshold);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Copcrack\3.Blackdot_Adaptive.jpg");

                    Cv2.BitwiseAnd(tmpImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tmpImg);

                    Mat tmptmp = new Mat();
                    tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    tmpImg.CopyTo(tmptmp[displayAreaRect]);
                    if (Config.SaveDebugImage == true)
                        tmpImg.SaveImage(@".\Detail\Lcd Copcrack\4.Blackdot_Adaptive_result.jpg");

                    Mat[] contours;
                    var hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    CvRect rect = new CvRect();
                    int iTotalMaxVal = 0;
                    double dArea;
                    int iMax = 0;
                    bool bIsFault = false;

                    for (int i = 0; i < contours.Length; i++)
                    {
                        rect = Cv2.BoundingRect(contours[i]);
                        dArea = Cv2.ContourArea(contours[i]);

                        iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                        if (Result.m_nBlackDot_JudgeSize < iMax)
                            Result.m_nBlackDot_JudgeSize = iMax;

                        if (iMax > DataLcdCopcrack.BlackDot_BJudgeSizeUL)
                        {

                            bIsFault = true;
                            iTotalMaxVal += (int)dArea;

                            rect.X = rect.X - 4 / 2;
                            rect.Width = rect.Width + 40;
                            rect.Y = rect.Y - 4 / 2;
                            rect.Height = rect.Height + 40;

                            Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                        }
                    }

                    #region no use
                    //                    if (!bIsFault)
                    //                    {
                    //                        Mat resImg = ot.GetPsmImage(m_BlueDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\5.blackdot_psm_result.jpg");
                    //#else
                    //#endif
                    //#if SAVEIMG
                    //                        Cv2.Threshold(resImg, resImg, DataLcdBlue.m_nBlackDot_BPsmThreshold, 255, ThresholdType.Binary);
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\6.blackdot_psm_threshold.jpg");
                    //#else
                    //#endif
                    //                        Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //#if SAVEIMG
                    //                        resImg.SaveImage(@".\Detail\Lcd Blue\7.blackdot_psm_threshold+area.jpg");
                    //#else
                    //#endif
                    //                        tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //                        resImg.CopyTo(tmptmp[displayAreaRect]);

                    //                        hierarchy = InputOutputArray.Create(new List<Vec4i>());

                    //                        Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //                        dArea = 0;
                    //                        iTotalMaxVal = 0;

                    //                        for (int i = 0; i < contours.Length; i++)
                    //                        {
                    //                            rect = Cv2.BoundingRect(contours[i]);
                    //                            dArea = Cv2.ContourArea(contours[i]);

                    //                            iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //                            if (Result.m_nBlackDot_JudgeSize < iMax)
                    //                                Result.m_nBlackDot_JudgeSize = iMax;

                    //                            if (iMax > DataLcdBlue.BlackDot_BJudgeSizeUL)
                    //                            {
                    //                                bIsFault = true;
                    //                                iTotalMaxVal += (int)dArea;

                    //                                rect.X = rect.X - 4 / 2;
                    //                                rect.Width = rect.Width + 40;
                    //                                rect.Y = rect.Y - 4 / 2;
                    //                                rect.Height = rect.Height + 40;

                    //                                Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //                            }
                    //                        }
                    //                    }
                    #endregion

                    CvScalar clr;
                    if (bIsFault)
                        clr = new CvScalar(0, 0, 255);
                    else
                        clr = new CvScalar(0, 255, 0);

                    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    if (Config.SaveDebugImage == true)
                    {
                        resultImg.SaveImage(@".\Detail\Lcd CopCrack\8.blackdot_result.jpg");
                    }

                    Result.ImageResult = resultImg.Clone();

                    resultImg.Dispose();

                    #region X
                    //bool bIsFault = false;
                    //Mat resultImg = m_BlueRotationImage.Clone();

                    //Mat resImg = ot.GetPsmImage(m_BlueDisplayArea, m_nPsmShiftPixelX, m_nPsmShiftPixelY, SHIFT_DIRECTION.BOTH, COMPARE_METHOD.SUBTRACT);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\5.blackdot_psm_result.jpg");

                    //Cv2.Threshold(resImg, resImg, DataLcdBlue.m_nBlackDot_BPsmThreshold, 255, ThresholdType.Binary);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\6.blackdot_psm_threshold.jpg");

                    //Cv2.BitwiseAnd(resImg, ResultImg[(int)BMP.CAMERA_DATA_DIMMING], resImg);
                    //resImg.SaveImage(@".\Detail\Lcd Blue\7.blackdot_psm_threshold+area.jpg");

                    ////itmptmp = new IplImage(originalImg.ToIplImage().Size, BitDepth.U8, 1);
                    ////IplImage iresImg = resImg.ToIplImage();
                    ////itmptmp.Zero();
                    ////itmptmp.SetROI(displayAreaRect);
                    ////iresImg.Copy(itmptmp);
                    ////itmptmp.ResetROI();

                    ////tmptmp = itmptmp.ToBitmap().ToMat();
                    ////itmpImg.Dispose();
                    ////iresImg.Dispose();

                    //Mat tmptmp = Mat.Zeros(originalImg.Size(), MatType.CV_8UC1);
                    //resImg.CopyTo(tmptmp[displayAreaRect]);

                    //var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                    //Mat[] contours;

                    //Cv2.FindContours(tmptmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                    //double dArea = 0;
                    //int iTotalMaxVal = 0;
                    //CvRect rect = new CvRect();
                    //int iMax = 0;
                    //for (int i = 0; i < contours.Length; i++)
                    //{
                    //    rect = Cv2.BoundingRect(contours[i]);
                    //    dArea = Cv2.ContourArea(contours[i]);

                    //    iMax = rect.Width > rect.Height ? rect.Width : rect.Height;

                    //    if (Result.m_nBlackDot_JudgeSize < iMax)
                    //        Result.m_nBlackDot_JudgeSize = iMax;

                    //    if (iMax > DataLcdBlue.BlackDot_BJudgeSizeUL)
                    //    {
                    //        bIsFault = true;
                    //        iTotalMaxVal += (int)dArea;

                    //        rect.X = rect.X - 4 / 2;
                    //        rect.Width = rect.Width + 40;
                    //        rect.Y = rect.Y - 4 / 2;
                    //        rect.Height = rect.Height + 40;

                    //        Cv2.Rectangle(resultImg, rect, new CvScalar(255, 0, 0), 4);
                    //    }
                    //}


                    //CvScalar clr;
                    //if (bIsFault)
                    //    clr = new CvScalar(0, 0, 255);
                    //else
                    //    clr = new CvScalar(0, 255, 0);

                    //Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //resultImg.SaveImage(@".\Detail\Lcd Blue\8.blackdot_result.jpg");


                    //if (bIsFault)
                    //{
                    //    Result.imgNGResult = resultImg.Clone().ToBitmap();
                    //    //Dust Mask Area
                    //    //불량처리
                    //}


                    //{
                    //    Mat resultImg = m_BlueRotationImage.Clone();

                    //    CvScalar m_DisplayMean;
                    //    m_DisplayMean = Cv2.Mean(m_BlueDisplayArea, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]);

                    //    bool bIsFault = false;

                    //    if (m_DisplayMean.Val0 < DataLcdBlue.m_nBrightness_BJudgeColorBLL
                    //        || m_DisplayMean.Val0 > DataLcdBlue.m_nBrightness_BJudgeColorBUL)
                    //        bIsFault = true;

                    //    CvScalar clr;

                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);

                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Blue\9.brightness_result.jpg");

                    //}

                    //{
                    //    Mat resultImg = m_BlueRotationImage.Clone();

                    //    CvScalar meanVal;
                    //    double totalVal = 0;


                    //    Cv2.Split(m_BlueRotationImage, out blue_splitImg);

                    //    for (int i = 0; i < MAX_CHANNEL; i++)
                    //    {
                    //        blue_disSplit.Add(new Mat(blue_splitImg[i], displayAreaRect));
                    //    }
                    //    Mat tmp = blue_disSplit[BLUE_CHANNEL] / (blue_disSplit[BLUE_CHANNEL] + blue_disSplit[GREEN_CHANNEL] + blue_disSplit[RED_CHANNEL]) * 100;

                    //    tmp.SaveImage(@".\Detail\Lcd Blue\10.colorbalance.jpg");

                    //    dBlueDisplayColorBalance = (double)Cv2.Mean(tmp, ResultImg[(int)BMP.CAMERA_DATA_DIMMING]).Val0;

                    //    bool bIsFault = false;

                    //    if (dBlueDisplayColorBalance < (double)DataLcdBlue.m_nColorBalance_BJudgeColorBLL ||
                    //        dBlueDisplayColorBalance > (double)DataLcdBlue.m_nColorBalance_BJudgeColorBUL)
                    //    {
                    //        bIsFault = true;
                    //    }

                    //    CvScalar clr;

                    //    if (bIsFault)
                    //        clr = new CvScalar(0, 0, 255);
                    //    else
                    //        clr = new CvScalar(0, 255, 0);


                    //    Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                    //    resultImg.SaveImage(@".\Detail\Lcd Blue\11.colorbalance_result.jpg");
                    //    resultImg.Release();


                    /////////////DIFF/////////////////
                    /*
                    {
                        Mat resultImg = m_BlueRotationImage.Clone();

                        CvRect tempRect = new CvRect(5, 5, m_displayWidth - 10, m_displayHeight - 10);
                        CvRect tempRect2 = new CvRect(m_displayX + 5, m_displayY + 5, m_displayWidth - 10, m_displayHeight - 10);

                        Mat maskROI = new Mat(ResultImg[(int)BMP.CAMERA_DATA_DIMMING], tempRect);
                        Mat blueImg = new Mat(blue_splitImg[BLUE_CHANNEL], tempRect2);
                        Mat greenImg = new Mat(blue_splitImg[GREEN_CHANNEL], tempRect2);
                        Mat redImg = new Mat(blue_splitImg[RED_CHANNEL], tempRect2);

                        maskROI.SaveImage(@".\Detail\Lcd Blue\12.diffcolorarearate_mask.jpg");
                        blueImg.SaveImage(@".\Detail\Lcd Blue\13.diffcolorarearate_blue.jpg");
                        greenImg.SaveImage(@".\Detail\Lcd Blue\14.diffcolorarearate_green.jpg");
                        redImg.SaveImage(@".\Detail\Lcd Blue\15.diffcolorarearate_red.jpg");

                        double rateValue = 0;
                        int rowGap = DataLcdBlue.m_nDiffColorAreaRate_BHeightSplitCount;
                        int colGap = DataLcdBlue.m_nDiffColorAreaRate_BWidthSplitCount;

                        bool bIsFault = false;

                        Mat cal = blueImg / (blueImg + greenImg + redImg) * 100;
                        cal.SaveImage(@".\Detail\Lcd Blue\16.diffcolorarearate_cal_mat.jpg");

                        Mat lowCal = new Mat();
                        Cv2.Threshold(cal, lowCal, dBlueDisplayColorBalance - DataLcdBlue.m_nDiffColorAreaRate_BJudgePixelRateLL, 255, ThresholdType.BinaryInv);
                        lowCal.SaveImage(@".\Detail\Lcd Blue\17.diffcolorarearate_cal_mat_low_balance.jpg");

                        Mat highCal = new Mat();
                        Cv2.Threshold(cal, highCal, dBlueDisplayColorBalance + DataLcdBlue.m_nDiffColorAreaRate_BJudgePixelRateUL, 255, ThresholdType.Binary);
                        highCal.SaveImage(@".\Detail\Lcd Blue\18.diffcolorarearate_cal_mat_high_balance.jpg");

                        Cv2.BitwiseOr(lowCal, highCal, cal);
                        cal.SaveImage(@".\Detail\Lcd Blue\19.diffcolorarearate_cal_mat_ng_balance.jpg");

                        Mat tmp = new Mat();
                        Mat tmpMask = new Mat();
                        CvScalar avg;

                        int cGap = 0;
                        int rGap = 0;

                        double dPixelsRate = 0;
                        double dMaxPixelsRate = 0;
                        bIsFault = false;

                        for (int a = 0; a < cal.Rows; a = a + rowGap)
                        {
                            for (int b = 0; b < cal.Cols; b = b + colGap)
                            {
                                cGap = cal.Cols - b < colGap ? cal.Cols - b : colGap;
                                rGap = cal.Rows - a < rowGap ? cal.Rows - a : rowGap;
                                tempRect = new CvRect(b, a, cGap, rGap);
                                tmp = new Mat(cal, tempRect);
                                tmpMask = new Mat(maskROI, tempRect);

                                Cv2.BitwiseAnd(tmp, tmpMask, tmp);

                                Mat[] contours;
                                var hierarchy = InputOutputArray.Create(new List<Vec4i>());
                                Cv2.FindContours(tmp, out contours, hierarchy, ContourRetrieval.External, ContourChain.ApproxNone);

                                for (int i = 0; i < contours.Length; i++)
                                {
                                    dPixelsRate += Cv2.ContourArea(contours[i]);
                                }

                                //sh32.heo
                                foreach (Mat m in contours)
                                {
                                    m.Release();
                                }

                                dMaxPixelsRate = dPixelsRate;
                                dMaxPixelsRate = (dMaxPixelsRate / ((m_displayWidth - 10) * (m_displayHeight - 10))) * 100;

                                if (dMaxPixelsRate > DataLcdBlue.m_nDiffColorAreaRate_BDiffColorJudgePixelRate)
                                {
                                    bIsFault = true;
                                    Cv2.Rectangle(resultImg, new CvRect(m_displayX + 5 + b, m_displayY + 5 + a, cGap, rGap), new CvScalar(255, 0, 0), 4);
                                }

                            }
                        }

                        CvScalar clr;
                        if (bIsFault)
                            clr = new CvScalar(0, 0, 255);
                        else
                            clr = new CvScalar(0, 255, 0);
                        Cv2.Rectangle(resultImg, new CvRect(4, 4, resultImg.Cols - 4 * 2, resultImg.Rows - 4 * 2), clr, 4 * 5);
                        resultImg.SaveImage(@".\Detail\Lcd Blue\21.diffcolorarearate_result.jpg");
                    }
                    */
                    #endregion
                }

            }

        }

        static public int[] aTemp
                   = { 0, -1,  1, -2,  2, -3,  3, -4,  4, -5,  5,
                   -6,  6, -7,  7, -8,  8, -9,  9,-10, 10,
                  -11, 11,-12, 12,-13, 13,-14, 14,-15, 15,
                  -16 ,16, -17,  17, -18,  18, -19,  19,-20, 20,
                  -21, 21, -22,  22, -23,  23, -24,  24,-25, 25,
                  -26 ,26, -27,  27, -28,  28, -29,  29,-30, 30
                  };

        public bool Barcode(Bitmap refNormalImage, TestSpec testSpec, MData_REAR DataRear, ref MResult_REAR ResultRear)
        {
            ResultRear = new MResult_REAR();
            Bitmap testImage = vt.GetBmpFromRect(refNormalImage, testSpec.RectBarcode, 90);
            string[] datas = BarcodeLib.BarcodeReader.BarcodeReader.read(testImage,
               BarcodeLib.BarcodeReader.BarcodeReader.CODE128);
            if (datas != null)
            {
                foreach (string st in datas)
                {
                    if (st.Length == 15 && DataRear.IMEILength == 15)
                    {
                        string temp;
                        temp = st.Remove(0, 1);
                        temp = temp.Insert(0, "3");

                        ResultRear.m_nImei = temp;

                        return true;
                    }
                    if (st.Length == 11 && DataRear.IMEILength == 11)
                    {
                        string temp;
                        temp = st.Remove(0, 1);
                        temp = temp.Insert(0, "R");

                        ResultRear.m_nImei = temp;
                        return true;
                    }
                }
            }
			Log.AddLog("Barcode() : Can't Find Barcode using BarcodeLib.BarcodeReader");

            IplImage imgSrc = refNormalImage.ToIplImage();
            IplImage imgBinary = new IplImage(imgSrc.Size, BitDepth.U8, 1);
            if (imgSrc.NChannels == 3)
                Cv.CvtColor(imgSrc, imgBinary, ColorConversion.BgrToGray);
            else imgBinary = imgSrc.Clone();
            imgSrc.Dispose();

    //        var barcodereader = new ZXing.BarcodeReader();

    //        Bitmap imgSrcForDecoding = imgBinary.ToBitmap();
    //        for (int i = 0; i < 1; i++)
    //        {
    //            int thres = DataRear.BarcodeThreshold + aTemp[i + 1];
				//Log.AddLog(string.Format("[HHP] {0} : {1}", "threshold", thres));

    //            var tempImg = vt.ThresholdProcess(imgSrcForDecoding, thres, vt.GetThresholdType(DataRear.BarcodeThresIdx));
    //            var result = barcodereader.DecodeMultiple(tempImg);

    //            if (result != null)
    //            {
    //                foreach (Result r in result)
    //                {
    //                    if (r.Text.Length == DataRear.IMEILength)
    //                    {
    //                        ResultRear.m_nImei = r.ToString();
    //                        i = 100;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
            if (ResultRear.m_nImei != "")
                return true;
            else
                return false;
        }

        public int CalcLogoMatchingRate(Bitmap refNormalImage, TestSpec testSpec, int index, Bitmap tmplImage) // , ref MResult_REAR ResultRear)
        {
            // Log.AddLog("CalcLogoMatchingRate.....");

            Bitmap testImage = null;

            if (index == 1)
                testImage = vt.GetBmpFromRect(refNormalImage, testSpec.RectLogo1, 90);
            else
                testImage = vt.GetBmpFromRect(refNormalImage, testSpec.RectLogo2, 90);

            testImage.Save(string.Format(@".\Detail\Rear\logo{0}.bmp", index));
            tmplImage.Save(string.Format(@".\Detail\Rear\tmplate{0}.bmp", index));
            // ResultRear = new MResult_REAR();

            CvPoint minloc1, maxloc1;
            double minVal1, maxVal1;
            CvSize size1;

            IplImage imgSrc = testImage.ToIplImage();
            IplImage imgrefGray = new IplImage(imgSrc.Size, BitDepth.U8, 1);
            if (imgSrc.NChannels == 3)
                Cv.CvtColor(imgSrc, imgrefGray, ColorConversion.BgrToGray);
            else imgrefGray = imgSrc.Clone();

            imgSrc = tmplImage.ToIplImage();
            IplImage imgtmplGray = new IplImage(imgSrc.Size, BitDepth.U8, 1);
            if (imgSrc.NChannels == 3)
                Cv.CvtColor(imgSrc, imgtmplGray, ColorConversion.BgrToGray);
            else imgtmplGray = imgSrc.Clone();
            imgSrc.Dispose();

            size1.Width = imgrefGray.Width - imgtmplGray.Width + 1;
            size1.Height = imgrefGray.Height - imgtmplGray.Height + 1;
            IplImage temp = Cv.CreateImage(size1, BitDepth.F32, 1);
            Cv.MatchTemplate(imgrefGray, imgtmplGray, temp, MatchTemplateMethod.CCoeffNormed);

            Cv.MinMaxLoc(temp, out minVal1, out maxVal1, out minloc1, out maxloc1, null);
            return (int)(maxVal1 * 100);

        }
        public void SaveOcrTemplteImge(Bitmap refNormalImage, TestSpec testSpec)
        {
            Mat image = refNormalImage.ToMat();
            CvRect rect_IMEIBG = new CvRect(testSpec.RectImei.X, testSpec.RectImei.Y, testSpec.RectImei.Width, testSpec.RectImei.Height);
            Mat template_IMEIBG = image[rect_IMEIBG].Clone();
            template_IMEIBG.SaveImage(string.Format(Config.sPathModel+"{0}_TemplateImg.bmp", Config.sCurrnetSpecFile.Replace(".json", "")));
        }
        public bool OCR(Bitmap refNormalImage, TestSpec testSpec, String inspTarget,
            String inspTarget2,  MData_REAR DataRear, FrmMainCh2 _mainform, ref MResult_REAR ResultRear)
        {
            ResultRear = new MResult_REAR();
            Bitmap imgSrc = refNormalImage.Clone() as Bitmap;
            Mat image = refNormalImage.ToMat();
            if (image.Channels() == 3)
                Cv2.CvtColor(image, image, ColorConversion.BgrToGray);
            else if (image.Channels() == 4)
                Cv2.CvtColor(image, image, ColorConversion.BgraToGray);

            CvRect rect_IMEIBG = new CvRect(testSpec.RectImei.X, testSpec.RectImei.Y, testSpec.RectImei.Width, testSpec.RectImei.Height);

            CvRect rect_Matching = new CvRect();
            vt.makeROIMatching(rect_IMEIBG, 170, 140, image.Width, image.Height, DataRear.OCROffsetX, DataRear.OCROffsetY, out rect_Matching);

            Mat searchAreaImg = image[rect_Matching].Clone();
            //Mat template_IMEIBG = new Mat(string.Format(@".\Detail\Rear\{0}TemplateImg.bmp", Config.sCurrnetSpecFile.Replace(".json", "")));
            Mat template_IMEIBG = new Mat(Config.sPathModel+string.Format("{0}_TemplateImg.bmp", Config.sCurrnetSpecFile.Replace(".json", "")));
            Mat tempimei = template_IMEIBG.Clone();
            _mainform.DisplayImage(3, tempimei.ToBitmap());
            tempimei.Dispose();
            if (template_IMEIBG.Channels() == 3)
                Cv2.CvtColor(template_IMEIBG, template_IMEIBG, ColorConversion.BgrToGray);

            searchAreaImg.SaveImage(@".\Detail\Rear\areaImg.bmp");

            Mat result = new Mat();
            double minValues, maxValues;
            OpenCvSharp.CPlusPlus.Point minLocations, maxLocations;
            Mat tempImg = searchAreaImg.Clone();

            tempImg = searchAreaImg.Clone();
            Cv2.MatchTemplate(searchAreaImg, template_IMEIBG, result, MatchTemplateMethod.CCoeffNormed);
            result.MinMaxLoc(out minValues, out maxValues, out minLocations, out maxLocations);
			Log.AddLog(string.Format("CCoeffNormed minValue : {0}, maxValue : {1}", minValues, maxValues));
			Log.AddLog(string.Format("maxLocations X : {0}, Y : {1}", maxLocations.X, maxLocations.Y));
            if (tempImg.Channels() == 1)
                Cv2.CvtColor(tempImg, tempImg, ColorConversion.GrayToBgr);
            Cv2.PutText(tempImg, maxValues.ToString("##.###"), new OpenCvSharp.CPlusPlus.Point(50, 50), FontFace.HersheyComplex, 2, new CvScalar(0, 0, 255), 2);
            Cv2.Rectangle(tempImg, new CvRect(maxLocations.X, maxLocations.Y, template_IMEIBG.Width, template_IMEIBG.Height), new CvScalar(0, 0, 255), 1);
            tempImg.SaveImage(@".\Detail\Rear\tempImg_CCoeffNormed.bmp");

            ResultRear.areaNgImage = tempImg.ToBitmap();
            tempImg.Dispose();

            CvRect rect = new CvRect();
            if (maxValues > 0.3)
            {
                rect = new CvRect(maxLocations.X + rect_Matching.X - 5, maxLocations.Y + rect_Matching.Y, template_IMEIBG.Width + 10, template_IMEIBG.Height);
                rect.Y = rect.Y - rect.Height * 4 - 30;
                rect.Height = rect.Height * 3 - 60;
            }
            else
            {
                rect = new CvRect(rect_IMEIBG.X - 5, rect_IMEIBG.Y, rect_IMEIBG.Width + 10, rect_IMEIBG.Height);
                rect.Y = rect.Y - rect.Height * 4 - 30;
                rect.Height = rect.Height * 3 - 60;
            }

            int xconst = rect.X;
            int yconst = rect.Y;
            int xmove = rect.Width / 6;
            int ymove = rect.Height / 12;
            Cv2.CvtColor(image, image, ColorConversion.GrayToBgr);
            Cv2.Rectangle(image, new OpenCvSharp.CPlusPlus.Rect(rect.X, rect.Y, rect.Width + 2, rect.Height + 2), Scalar.Red, 2);

            //  image[rect].SaveImage(@".\Detail\Rear\IMGBGRect.bmp");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string ketqua = string.Empty;
            bool bIsFound = false;
            bool _showimg = true;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Mat temp = vt.GetBmpFromRect(imgSrc, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), 90).ToMat();
                    string time = string.Format("{0}.{1}", DateTime.Now.Second, DateTime.Now.Millisecond);
                    string _IMEI_BG = "";

                    _IMEI_BG = processIMEIbyBG(temp, testSpec, _mainform, _showimg, DataRear.IMEILength, time);
                    temp.Dispose();
                    if (_IMEI_BG.Contains(inspTarget) || _IMEI_BG.Contains(inspTarget2))
                    {
                        bIsFound = true;
                        if (_IMEI_BG.Contains(inspTarget))
                        {
                            ResultRear.m_nImei = inspTarget;
                        }
                        else if (_IMEI_BG.Contains(inspTarget2))
                        {
                            ResultRear.m_nImei = inspTarget2;
                        }

						Log.AddLog("Found Target");
                        break;
                    }
                    _showimg = false;
                    if (j % 2 == 0) rect.Y = yconst + ymove * (j / 2 + 1);
                    else rect.Y = yconst - ymove * (j / 2 + 1);
                    Thread.Sleep(50);
                }

                if (bIsFound) break;
                if (i % 2 == 0) rect.X = xconst + xmove * (i / 2 + 1);
                else rect.X = xconst - xmove * (i / 2 + 1);
                if (stopwatch.ElapsedMilliseconds / 1000 > 5) break;
                Thread.Sleep(20);
            }
            stopwatch.Stop();
            ResultRear.TestResult = bIsFound;

			if (!bIsFound)
                ResultRear.ImageResult = image;
			
            //image.Dispose();
            return bIsFound;
        }

        private string processIMEIbyBG(Mat img, TestSpec testSpec, FrmMainCh2 _mainform, bool showimg, int imeilenght, string time)
        {
            try
            {
                img.SaveImage(@".\Detail\Rear\preProcessBG.bmp");
                img = vt.preProcessBG(img, testSpec);
                if (showimg) _mainform.DisplayImage(4, img.ToBitmap());
                img.SaveImage(@".\Detail\Rear\GrayProcess.tif");
                //img.Dispose();

                var engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default);
                //string srcimgPath = @".\Detail\Rear\GrayProcess.tif";
                //var tmpimg = Pix.LoadFromFile(srcimgPath);
                var page = engine.Process(img.ToBitmap());
                img.Dispose();
                string getText = page.GetText();
                char[] m_charArray = getText.ToCharArray();
                if (imeilenght == 15)
                {

                    for (int j = 0; j < m_charArray.Length; j++)
                    {
                        if (m_charArray[j] == 'o' || m_charArray[j] == 'O' || m_charArray[j] == 'Q') m_charArray[j] = '0';
                        else if (m_charArray[j] == 'i' || m_charArray[j] == 'I') m_charArray[j] = '1';
                        else if (m_charArray[j] == 'l' || m_charArray[j] == 'L' || m_charArray[j] == 'T') m_charArray[j] = '1';
                        else if (m_charArray[j] == 's' || m_charArray[j] == 'S') m_charArray[j] = '5';
                        else if (m_charArray[j] == 'a' || m_charArray[j] == 'A') m_charArray[j] = '6';
                        else if (m_charArray[j] == 'z' || m_charArray[j] == 'Z') m_charArray[j] = '2';
                        else if (m_charArray[j] == 'B') m_charArray[j] = '8';
                        else if (m_charArray[j] == 'é') m_charArray[j] = '3';
                    }
                }

                string m_sResult = string.Empty;
                foreach (char c in m_charArray)
                {
                    m_sResult += c;
                }
                m_sResult = Regex.Replace(m_sResult, @"\D", "");

				Log.AddLog(string.Format("{0} Code : " + m_sResult, time));
                return m_sResult;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }
    }
}

//string inspTarget = ResultRear.m_nImei.Remove(0, 10);
//imgSrc.Save(@".\Detail\Rear\beforeFilter.bmp");
//imgSrc = vt.ApplyContrastFilter(imgSrc, DataRear.OcrAvgValue);
//imgSrc.Save(@".\Detail\Rear\afterFilter.bmp");
//    bool bIsFound = false;

//    for (int i = 0; i < 20; i++)
//    {
//        string m_sResult = "";
//        int thres = DataRear.OcrThreshold + aTemp[i + 1];
//        string srcimgPath = @".\Detail\Rear\Ocr\" + thres + ".tif";
//        //using (Bitmap imgBinary = ot.ThresholdProcess(imgSrc, thres, ThresholdType.Binary))
//        using (Bitmap imgBinary = vt.ThresholdProcess(imgSrc, thres,
//            vt.GetThresholdType(DataRear.OcrThresIdx)))
//        using (IplImage imgTemp = imgBinary.ToIplImage())
//        {
//            Cv.Smooth(imgTemp, imgTemp, SmoothType.Median);
//            imgTemp.SaveImage(srcimgPath);
//            var engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default);
//            //var engine = new TesseractEngine(@".\tessdata", "custoEng", EngineMode.Default);
//            //var engine = new TesseractEngine(@".\tessdata", "custoImei", EngineMode.Default);
//            var img = Pix.LoadFromFile(srcimgPath);
//            var page = engine.Process(img);

//            string getText = page.GetText();
//            char[] m_charArray = getText.ToCharArray();
//            for (int j = 0; j < m_charArray.Length; j++)   //OCR 강제 보정
//            {
//                if (m_charArray[j] == 'o' || m_charArray[j] == 'O') m_charArray[j] = '0';
//                else if (m_charArray[j] == 'i' || m_charArray[j] == 'I') m_charArray[j] = '1';
//                else if (m_charArray[j] == 'l' || m_charArray[j] == 'L') m_charArray[j] = '1';
//                else if (m_charArray[j] == 's' || m_charArray[j] == 'S') m_charArray[j] = '5';
//                else if (m_charArray[j] == 'a' || m_charArray[j] == 'A') m_charArray[j] = '6';
//                else if (m_charArray[j] == 'z' || m_charArray[j] == 'Z') m_charArray[j] = '2';
//                else if (m_charArray[j] == 'é') m_charArray[j] = '3';
//            }

//            foreach (char c in m_charArray)
//            {
//                m_sResult += c;
//            }
//            m_sResult = Regex.Replace(m_sResult, @"\D", "");

//            Console.WriteLine("i: " + i + " Thres : " + thres + " : Laser Length : " +
//                m_sResult.Length + " Code : " + m_sResult);
//            if (m_sResult.Contains(inspTarget) || m_sResult.Contains(inspTarget2))
//            {
//                bIsFound = true;
//                if (m_sResult.Contains(inspTarget))
//                {
//                    ResultRear.m_nImei = inspTarget;
//                }
//                else
//                    ResultRear.m_nImei = inspTarget2;

//                Console.WriteLine("Found Target");
//                i = 100;
//            }
//        }

//    }
//    if (bIsFound)
//        return true;
//    else
//        return false;
//}

