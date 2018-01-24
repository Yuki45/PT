using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Blob;
using System.Drawing;
using OpenCvSharp.CPlusPlus;

using System.Windows.Forms;
using AutoInspection_GUMI;

namespace AutoInspection
{
    public class VisionTools 
    {
        public void makeROIMatching(CvRect rect_in, int extraX, int extraY, int width, int height, int OCROffsetX, int OCROffsetY, out CvRect rect_out)
        {
            rect_out = new CvRect();
            try
            {
                rect_out.X = (rect_in.X - extraX - OCROffsetX > 0) ? (rect_in.X - extraX - OCROffsetX) : 0;
                rect_out.Y = (rect_in.Y - extraY - OCROffsetY > 0) ? (rect_in.Y - extraY - OCROffsetY) : 0;
                rect_out.Width = (rect_out.X + rect_in.Width + extraX * 2 < width) ? (rect_in.Width + extraX * 2) : (width - rect_out.X - 1);
                rect_out.Height = (rect_out.Y + rect_in.Height + extraY * 2 < height) ? (rect_in.Height + extraY * 2) : (height - rect_out.Y - 1);
            }
            catch (Exception)
            {
            }
        }
        public Mat preProcessBG(Mat img, TestSpec testSpec)
        {
            Mat tmpImg = img.Clone();
            if (tmpImg.Channels() == 4)
                Cv2.CvtColor(tmpImg, tmpImg, ColorConversion.BgraToGray);
            try
            {
                Cv2.MedianBlur(tmpImg, tmpImg, 3);
                Cv2.FastNlMeansDenoising(tmpImg, tmpImg, 3, 7, 21);
                if (testSpec.SpecRear.OcrThresIdx != 3)
                    Cv2.Threshold(tmpImg, tmpImg, testSpec.SpecRear.OcrThreshold, 255, GetThresholdType(testSpec.SpecRear.OcrThresIdx));
                else
                    Cv2.AdaptiveThreshold(tmpImg, tmpImg, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 41, testSpec.SpecRear.OcrThreshold);

                //if (!Config.sCurrnetSpecFile.Contains("BLACK"))
                {
                    Cv2.Erode(tmpImg, tmpImg, new Mat(), new CvPoint(-1, -1), 1);
                    Cv2.Dilate(tmpImg, tmpImg, new Mat(), new CvPoint(-1, -1), 1);
                }
                return tmpImg;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Bitmap preProcessBG(Bitmap img, TestSpec testSpec)
        {
            Bitmap temp = img.Clone() as Bitmap;
            Mat tmpImg = temp.ToMat();
            temp.Dispose();
            if (tmpImg.Channels() == 4)
                Cv2.CvtColor(tmpImg, tmpImg, ColorConversion.BgraToGray);
            try
            {
                Cv2.MedianBlur(tmpImg, tmpImg, 3);
                Cv2.FastNlMeansDenoising(tmpImg, tmpImg, 3, 7, 21);
                if (testSpec.SpecRear.OcrThresIdx != 3)
                    Cv2.Threshold(tmpImg, tmpImg, testSpec.SpecRear.OcrThreshold, 255, GetThresholdType(testSpec.SpecRear.OcrThresIdx));
                else
                    Cv2.AdaptiveThreshold(tmpImg, tmpImg, 255, AdaptiveThresholdType.GaussianC, ThresholdType.Binary,41, testSpec.SpecRear.OcrThreshold);

                Cv2.Erode(tmpImg, tmpImg, new Mat(), new CvPoint(-1, -1), 1);
                Cv2.Dilate(tmpImg, tmpImg, new Mat(), new CvPoint(-1, -1), 1);
                return tmpImg.ToBitmap();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public OpenCvSharp.ThresholdType GetThresholdType(int cbbIdx)
        {
            if (cbbIdx == 0)
                return OpenCvSharp.ThresholdType.Binary;
            else if (cbbIdx == 1)
                return OpenCvSharp.ThresholdType.BinaryInv;
            else
                return ThresholdType.Otsu;
        }

        IplImage IplReturn;

        public Bitmap ApplyContrastFilter(Bitmap grayBitmap, int CriticalValue)
        {
            Bitmap cBitmap = new Bitmap(grayBitmap.Width, grayBitmap.Height);
            Color color;

            int brightness;
            int averageValue = 0;       // 그레이 영상의 평균 밝기값
            int adjustValue = 35;       // 영상에 적용할(추가 또는 뺄셈) 임의의 일정한 값

            for (int y = 0; y < grayBitmap.Height; y++)
            {
                for (int x = 0; x < grayBitmap.Width; x++)
                {
                    averageValue += grayBitmap.GetPixel(x, y).R;
                }
            }

            // 전체 픽셀의 평균값을 구한다.
            averageValue = averageValue / (grayBitmap.Width * grayBitmap.Height);
            Log.AddLog("averageValue = " + averageValue + ", criticalValue = " + CriticalValue);



            for (int y = 0; y < grayBitmap.Height; y++)
            {
                for (int x = 0; x < grayBitmap.Width; x++)
                {
                    // 전체 픽셀의 평균값이 임계값보다 작으면
                    if (averageValue <= CriticalValue)
                    {
                        // 적용값(adjustValue)을 더한다.
                        adjustValue = CriticalValue - averageValue;
                        brightness = grayBitmap.GetPixel(x, y).R + adjustValue;

                        // 오버플로우
                        if (brightness > 255)
                            brightness = 255;
                        else if (brightness < 0)
                            brightness = 0;

                        color = Color.FromArgb(brightness, brightness, brightness);
                        cBitmap.SetPixel(x, y, color);
                    }
                    // 전체 픽셀의 평균값이 임계값보다 크면
                    else
                    {
                        // 적용값(adjustValue)을 뺀다.
                        adjustValue = CriticalValue - averageValue;
                        brightness = grayBitmap.GetPixel(x, y).R + adjustValue;

                        // 오버플로우
                        if (brightness > 255)
                            brightness = 255;
                        else if (brightness < 0)
                            brightness = 0;

                        color = Color.FromArgb(brightness, brightness, brightness);
                        cBitmap.SetPixel(x, y, color);
                    }
                }
            }

            return cBitmap;
        }
        /// <summary>
        /// Find contours
        /// </summary>
        /// <param name="img"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        private CvSeq<CvPoint> FindContours(IplImage img, CvMemStorage storage)
        {
            // 輪郭抽出
            CvSeq<CvPoint> contours;
            using (IplImage imgClone = img.Clone())
            {
                Cv.FindContours(imgClone, storage, out contours);
                if (contours == null)
                {
                    return null;
                }
                contours = Cv.ApproxPoly(contours, CvContour.SizeOf, storage, ApproxPolyMethod.DP, 3, true);
            }
            // 一番長そうな輪郭のみを得る
            CvSeq<CvPoint> max = contours;
            for (CvSeq<CvPoint> c = contours; c != null; c = c.HNext)
            {
                if (max.Total < c.Total)
                {
                    max = c;
                }
            }
            return max;
        }


        /// <summary>
        /// 이미지 파일 Load(Iplimage 리턴)
        /// </summary>
        /// <param name="address"></param>
        /// <param name="abc"></param>
        /// <returns></returns>
        public IplImage LoadImage(string address, LoadMode abc)
        {
            IplReturn = new IplImage(address, abc);
            return IplReturn;
        }


        /// <summary>
        /// GrayScale 변환(Iplimage 리턴)
        /// </summary>

        public IplImage BgrToGray(IplImage src)
        {
            IplReturn = new IplImage(src.Size, BitDepth.U8, 1);
            Cv.CvtColor(src, IplReturn, ColorConversion.BgrToGray);
            return IplReturn;
        }

        public Bitmap ThresholdProcess(Bitmap OriImgSrc, int Threshold, ThresholdType a)
        {
            lock (OriImgSrc)
            {
                using (IplImage imgSrc = OriImgSrc.ToIplImage())
                using (IplImage imgBinary = new IplImage(imgSrc.Size, BitDepth.U8, 1))
                {
                    imgBinary.Zero();
                    if (imgSrc.ElemChannels == 4)
                    {
                        Cv.CvtColor(imgSrc, imgBinary, ColorConversion.BgraToGray);
                        Cv.Threshold(imgBinary, imgBinary, Threshold, 255, a);
                    }

                    if (imgSrc.ElemChannels == 3)
                    {
                        Cv.CvtColor(imgSrc, imgBinary, ColorConversion.BgrToGray);
                        Cv.Threshold(imgBinary, imgBinary, Threshold, 255, a);
                    }

                    if (imgSrc.ElemChannels == 1) Cv.Threshold(imgSrc, imgBinary, Threshold, 255, a);
                    return imgBinary.ToBitmap();
                }
            }
        }

        public Bitmap BlobProcess(Bitmap OriImgSrc, int Threshold, ThresholdType a, int minArea, int maxArea)
        {
            using (IplImage imgSrc = OriImgSrc.ToIplImage())
            using (IplImage imgRender = new IplImage(imgSrc.Size, BitDepth.U8, 3))
            using (IplImage imgBinary = new IplImage(imgSrc.Size, BitDepth.U8, 1))
            using (var storage = new CvMemStorage())
            using (IplImage imgLabel = new IplImage(imgSrc.Size, BitDepth.F32, 1))
            {
                imgRender.Zero();
                imgBinary.Zero();
                if (imgSrc.ElemChannels == 3)
                {
                    Cv.CvtColor(imgSrc, imgBinary, ColorConversion.BgrToGray);
                    Cv.Threshold(imgBinary, imgBinary, Threshold, 255, a);
                }
                else if (imgSrc.ElemChannels == 1)
                {
                    Cv.Threshold(imgSrc, imgBinary, Threshold, 255, a);
                }

                CvBlobs blobs = new CvBlobs();
                blobs.Label(imgBinary);
                blobs.FilterByArea(minArea, maxArea);

                //CvSeq<CvPoint> contours = FindContours(imgBinary, storage);

                //if(contours != null)
                //{
                //    CvBox2D rect = Cv.MinAreaRect2(contours);
                //    Console.WriteLine("rect angle : " +rect.Angle);

                //}
                //CvBlob b = blobs.GreaterBlob();
                //if (b != null)
                //{
                //    Console.WriteLine("Centroid:{0} Area:{1}", b.Centroid, b.Area);
                //    Console.WriteLine("MaxX : {0} MaxY : {1}, MinX : {2}, MinY : {3}", b.MaxX, b.MaxY, b.MinX, b.MinY);
                //    //int i = 0;
                //foreach (KeyValuePair<int, CvBlob> item in blobs)
                //{
                //    CvBlob b = item.Value;
                //    Console.WriteLine("{0} | Centroid:{1} Area:{2}", item.Key, b.Centroid, b.Area);
                //    Console.WriteLine("MaxX : {0} MaxY : {1}, MinX : {2}, MinY : {3}", b.MaxX, b.MaxY, b.MinX, b.MinY);
                //    //    b.SaveImage(i + "_test.jpg", imgBinary);
                //    //    Console.WriteLine("Angle : " + ((180 / 3.14) * b.Angle()).ToString());
                //    //    imgBinary.SetROI(new CvRect(b.MinX - 100, b.MinY - 100, b.MaxX - b.MinX + 200, b.MaxY - b.MinY + 200));
                //    //    imgBinary.SaveImage(i + "_test2.jpg");
                //    //    rotateImage(new Bitmap(i + "_test2.jpg"), -(180 / 3.14) * b.Angle()).Save(i + "_test3.jpg");
                //    //    CvContourChainCode cc = b.Contour;
                //    //    //    cc.Render(imgRender);
                //    //    i++;
                //}
                //blobs.RenderBlobs(imgSrc, imgRender, RenderBlobsMode.Color);
                blobs.RenderBlobs(imgBinary, imgRender, RenderBlobsMode.Color);

                //blobs.RenderBlobs(imgSrc, imgRender, RenderBlobsMode.BoundingBox);
                //blobs.RenderBlobs(imgSrc, imgRender, RenderBlobsMode.BoundingBox);
                //Cv.Not(imgRender, imgRender);
                //blobs.RenderBlobs(imgBinary, imgRender);
                return imgRender.ToBitmap();
            }
        }

        /// <summary>
        /// 이미지회전
        /// </summary>
        /// <param name="OriImgsrc"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Bitmap RotateImageUseMat(Bitmap OriImgsrc, double angle)
        {
            using (IplImage imgSrc = OriImgsrc.ToIplImage())
            {
                CvMat mat = Cv.GetRotationMatrix2D(new CvPoint2D32f(imgSrc.Width / 2.0F, imgSrc.Height / 2.0F), angle, 1.0);
                Cv.WarpAffine(imgSrc, imgSrc, mat, Interpolation.Cubic);
                mat.Dispose();
                return imgSrc.ToBitmap();
            }
        }

        public Bitmap GetBmpFromRect(Bitmap imgSrc, Rectangle Rect, float angle = 0)
        {
            AutoInspection_GUMI.Log.AddLog("..........GetBmpFromRect");
            

            lock (imgSrc)
            {
                //Rectangle Rect = new Rectangle();
                Bitmap cropped = null;
                try
                {
                    //Rect.X = (int)(_Rect.X * ScaleX);
                    //Rect.Y = (int)(_Rect.Y * ScaleY);
                    //Rect.Width = (int)(_Rect.Width * ScaleX);
                    //Rect.Height = (int)(_Rect.Height * ScaleY);
                    
                    //imgSrc.Save(@"C:\test.bmp");
                    
                    cropped = imgSrc.Clone(Rect, imgSrc.PixelFormat);

                }
                catch( Exception e )
                {
                    MessageBox.Show(e.Message);
                }

                if (angle != 0)
                {
                    Bitmap returnBitmap = new Bitmap(cropped.Height, cropped.Width);

                    Graphics g = Graphics.FromImage(returnBitmap);
                    g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                    g.RotateTransform(angle);
                    g.TranslateTransform(-(float)cropped.Width / 2, -(float)cropped.Height / 2);
                    g.DrawImage(cropped, new System.Drawing.Point(0, 0));

                    g.Dispose();
                    cropped.Dispose(); 

                    return returnBitmap;
                }

                return cropped;
            }
        }

        /// <summary>
        /// 이미지 회전
        /// </summary>
        /// <param name="b">원본이미지</param>
        /// <param name="angle">회전각도</param>
        /// <returns>결과 이미지</returns>
        public Bitmap RotateImageUseTransf(Bitmap b, float angle)
        {
            lock (b)
            {
                Bitmap returnBitmap = new Bitmap(b.Height, b.Width);
                Graphics g = Graphics.FromImage(returnBitmap);
                g.TranslateTransform((float)returnBitmap.Width / 2, (float)returnBitmap.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                g.DrawImage(b, new System.Drawing.Point(0, 0));
                g.Dispose();
                return returnBitmap;
            }
        }

    }
}
