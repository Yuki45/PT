using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace Self.Vision.OpenCVSharp.SpecViewer
{
    public struct Spec
    {
        public int BlockSize;
        public int Thres;
        //public double Area;
        public int Pixel;
    }

    public static class Proc
    {
        public static int srcFileIndex = -1;

        static Stopwatch stopWatch = new Stopwatch();

        public static CvSize offsetSize = new CvSize(20, 20);
        public static CvPoint offsetPoint = new CvPoint(-10, -10);
        public static OpenCvSharp.CPlusPlus.Point postionMessage = new OpenCvSharp.CPlusPlus.Point(100, 50);



        static Mat accumulatedImage = new Mat();

        public static void CountReset()
        {
            srcFileIndex = 0;
        }

        public static Mat GetSrcImage()
        {
            srcFileIndex++;
            srcFileIndex = srcFileIndex % 67;

            string srcFile = string.Format(@"D:\srcImage\srcImage{0:D3}.jpg", (srcFileIndex));
            return new Mat(srcFile);
        }
        
        static Mat angleMat;
        
        public static CvRect displayAreaRect;
        public static bool GetArea(Bitmap _srcImage)
        {
            CvBox2D rRect = new CvBox2D();
            Mat rotationImage;

            Mat refNormalImageArea = _srcImage.ToMat();
            Mat origianlImg = refNormalImageArea.Clone();

            if (origianlImg.Channels() == 3)
                Cv2.CvtColor(origianlImg, origianlImg, ColorConversion.BgrToGray);

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
                if (dArea > 5000000)
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
                if (dArea > 5000000)
                {
                    MinAreaRect = Cv2.MinAreaRect(contours[i]);
                }
            }

            CvScalar clr;
            //When it can't find the area
            if (MinAreaRect.Size.Width == 0 || MinAreaRect.Size.Height == 0)
            {
                return false;
                //MessageBox.Show("fail to get the area rect");
            }

            int m_displayWidth = (int)(MinAreaRect.Size.Width > MinAreaRect.Size.Height ? MinAreaRect.Size.Width : MinAreaRect.Size.Height);
            int m_displayHeight = (int)(MinAreaRect.Size.Height < MinAreaRect.Size.Width ? MinAreaRect.Size.Height : MinAreaRect.Size.Width);
            int m_displayX = (int)MinAreaRect.Center.X - (m_displayWidth / 2);
            int m_displayY = (int)MinAreaRect.Center.Y - (m_displayHeight / 2);

            displayAreaRect = new CvRect(m_displayX, m_displayY, m_displayWidth, m_displayHeight);

            MessageBox.Show("It found Display Area");
            return true;
        }


        public static void RunDust(Bitmap _srcImage, Spec spec, ref List<Bitmap> resultImages)
        {
            stopWatch.Restart();
            
            Mat srcImage = _srcImage.ToMat();

            Mat m_DustImage = new Mat();
            Cv2.WarpAffine(srcImage, m_DustImage, angleMat, srcImage.Size());

            srcImage = m_DustImage[displayAreaRect].Clone();


            string _msg;

            // 0. Ready
            resultImages.Clear();
            resultImages.Add(srcImage.ToBitmap());

            if (srcImage.Channels() == 3)
                Cv2.CvtColor(srcImage, srcImage, ColorConversion.BgrToGray);

            // 1. Proc 
            //  srcImage -> threshold 
            //Cv2.Threshold(srcImage, srcImage, spec.Thres, 255, ThresholdType.BinaryInv);
            Cv2.Threshold(srcImage, srcImage, 35, 255, ThresholdType.Binary);

            //_msg = string.Format("Count({0})", srcFileIndex);

            //Cv2.PutText(srcImage, _msg, new OpenCvSharp.CPlusPlus.Point(100, 100), FontFace.HersheyComplex, 2, CvColor.White, 3);
            //resultImages.Add(srcImage.ToBitmap());

            Mat[] contours;
            var hierachy = InputOutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(srcImage.Clone(), out contours, hierachy, ContourRetrieval.CComp, ContourChain.ApproxNone);
            double dArea = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                dArea += Cv2.ContourArea(contours[i]);
                
            }
            stopWatch.Stop();
            
            _msg = string.Format("contour count({3}), contour area({4}), Count({0}), {1}, Time({2})ms", srcFileIndex, "dustImage", 
                stopWatch.ElapsedMilliseconds.ToString("#,#"), contours.Length,dArea);
            Cv2.PutText(srcImage, _msg, postionMessage, FontFace.HersheyComplex, 2, CvColor.White, 3);
            resultImages.Add(srcImage.ToBitmap());

            GC.Collect();
            return;
        }
        public static void RunRGB(Bitmap _srcImage, Spec spec, ref List<Bitmap> resultImages )
        {
            stopWatch.Restart();

            Mat srcImage = _srcImage.ToMat();

            string _msg;

            // 0. Ready
            resultImages.Clear();
            resultImages.Add(srcImage.ToBitmap());

            if (srcImage.Channels() == 3)
                Cv2.CvtColor(srcImage, srcImage, ColorConversion.BgrToGray);

            // 1. Proc 
            //  srcImage -> Adaptive Proc
            Mat srcImage2AdaptiveThres = new Mat();
            Cv2.AdaptiveThreshold(srcImage, srcImage2AdaptiveThres, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv, spec.BlockSize, spec.Thres);


            //  get accumulatedImage 
            if (srcFileIndex == 0)
                accumulatedImage = srcImage2AdaptiveThres.Clone();
            else
                Cv2.BitwiseAnd(srcImage2AdaptiveThres, accumulatedImage, accumulatedImage);

            _msg = string.Format("Count({0})", srcFileIndex);

            Cv2.PutText(srcImage, _msg, new OpenCvSharp.CPlusPlus.Point(100, 100), FontFace.HersheyComplex, 2, CvColor.White, 3);
            resultImages.Add(srcImage.ToBitmap());

            Cv2.PutText(srcImage2AdaptiveThres, _msg, new OpenCvSharp.CPlusPlus.Point(100, 100), FontFace.HersheyComplex, 2, CvColor.White, 3);
            resultImages.Add(srcImage2AdaptiveThres.ToBitmap());

            Mat accumulatedImage4Display = accumulatedImage.Clone();

            Mat[] contours;
            var hierachy = InputOutputArray.Create(new List<Vec4i>());
            Cv2.FindContours(accumulatedImage.Clone(), out contours, hierachy, ContourRetrieval.CComp, ContourChain.ApproxNone);

            for (int i = 0; i < contours.Length; i++)
            {
                CvRect _boundingRect = Cv2.BoundingRect(contours[i]);

                double _area = Cv2.ContourArea(contours[i]);

                int _rectLength = (_boundingRect.Width > _boundingRect.Height) ? (_boundingRect.Width) : (_boundingRect.Height);

                if (_rectLength > spec.Pixel)
                //if (_rectLength > spec.Pixel || _area > spec.Area)
                {
                    CvRect _dispRect = _boundingRect + offsetSize + offsetPoint;
                    Cv2.Rectangle(accumulatedImage4Display, _dispRect, CvColor.White, 2);

                    //if (_area > spec.Area)
                    {
                        //string _position = string.Format("WH({0},{1}),A({2})", _boundingRect.Width, _boundingRect.Height, _area);
                        string _position = string.Format("WH({0},{1}))", _boundingRect.Width, _boundingRect.Height);
                        Cv2.PutText(accumulatedImage4Display, _position, new OpenCvSharp.CPlusPlus.Point(_boundingRect.X - 250, _boundingRect.Y - 20 ), FontFace.HersheyComplex, 2, CvColor.White, 2);
                    }
                }
            }

            stopWatch.Stop();

            _msg = string.Format("Count({0}), {1}, Time({2})ms", srcFileIndex, "accumulatedImage", stopWatch.ElapsedMilliseconds.ToString("#,#"));
            Cv2.PutText(accumulatedImage4Display, _msg, postionMessage, FontFace.HersheyComplex, 2, CvColor.White, 3);
            resultImages.Add(accumulatedImage4Display.ToBitmap());

            GC.Collect();
            return;
        }

        public static void Start()
        {

        }

        public static void Stop()
        {
        }
    }
}
