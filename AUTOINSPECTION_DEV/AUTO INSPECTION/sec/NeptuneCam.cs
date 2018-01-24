using AutoInspection.sec;
using AutoInspection_GUMI;
using NeptuneC_Interface;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoInspection
{
    public class NeptuneCam : ICameraInterface
    {
        // camera counters
        public UInt32 m_nCameras = 0;
        // selected camera index
        public UInt32 m_nCamIndex = 0;
        // camera handle
        public IntPtr m_hCamHandle = IntPtr.Zero;
        // camera information
        public NEPTUNE_CAM_INFO[] m_CamInfo;
        // string ID of selected camera
        string m_strSelectedCam = "";

        // camera type
        public ENeptuneDevType m_eCamType = ENeptuneDevType.NEPTUNE_DEV_TYPE_UNKNOWN;
        // pixel format list
        public Int32[] m_FormatList;
        // frame rate list
        public Int32[] m_FrateList;

        private static UInt64 m_nFrameCount = 0;
        private Thread m_Thread = null;
        private bool m_bPlay;

        private bool bCaptured = false;
        
        public Bitmap m_Bitmap = null;
        public BitmapData m_BitmapData = null;
        
        // image received callback
        private NeptuneC_Interface.NeptuneCFrameCallback NeptuneCFrameCallbackInstance;

        //string PixelFromat;
        public int nGain;
        public int nShutter;
        public bool isContinuous()
        {
            return true;
        }

        public Bitmap OneShot_()
        {
            ClearCapture();
            if (m_hCamHandle != IntPtr.Zero)
                NeptuneC.ntcOneShot(m_hCamHandle);

            if (WaitCapture(2500))
            {
                return m_Bitmap;
            }
            return null;
        }
        
        public Bitmap OneShot_(int Exposure)
        {
            SetExposure(Exposure);
            return OneShot_();
        }

        public void GetParameter(ref CameraParams cameraParams)
        {

        }
        public void SetParameter(CameraParams cameraParams)
        {
        }
        
        public void SetExposure(int _exposure)
        {
            SetParam(ENeptuneFeature.NEPTUNE_FEATURE_SHUTTER, _exposure);
        }
        
        public void SetPictureBox(PictureBox control)
        {
        }
        
        public void Start()
        {
            // ImiPixelFormat_YUV422Packed
            if (InitCamera(Config.ImiPixelFormat_YUV411Packed))
            {
                Play(IntPtr.Zero, IntPtr.Zero);
            }
        }

        public void Stop()
        {
            StopCamera();
        }

        public void DestroyCamera()
        {
            //if (m_bPlay)
            //{
            //    m_bPlay = false;
            //    m_Thread.Join();
            //    Cam1.AcquisitionStop();
            //}
            //if (Cam1 != null)
            //    Cam1.CameraClose();
            //Neptune1.UninitLibrary();
            new NotImplementedException();
        }

        public void SetLivePlay(bool bLiveMode)
        {
            new NotImplementedException();
        }
        
        public void ClearCapture()
        {
            bCaptured = false;
        }
        
        public bool WaitCapture(int nTimeOutMs)
        {
            while (nTimeOutMs > 0)
            {
                if (bCaptured)
                    return true;
                
                nTimeOutMs -= 10;
                Thread.Sleep(10);
            }
            return false;
        }

        unsafe public static IntPtr AllocIntPtrFromArr(Int32[] Arr)
        {
            int size = Marshal.SizeOf(Arr[0]) * Arr.Length;
            IntPtr pUnmanagedPtr = Marshal.AllocHGlobal(size);
            return pUnmanagedPtr;
        }
        unsafe public static void CopyArrFromUnmanagedIntPtr(IntPtr pSrc, Int32[] DestArr)
        {
            Marshal.Copy(pSrc, DestArr, 0, DestArr.Length);
        }
        unsafe public static void FreeIntPtrFromArr(IntPtr pUnmanagedPtr)
        {
            Marshal.FreeHGlobal(pUnmanagedPtr);
        }
        
        private bool FrameCallback(ref NEPTUNE_IMAGE pImage, IntPtr pContext)
        {
            #region Skip
            //string strFileName = string.Format(@"c:\test.bmp");
            //NeptuneC.ntcSaveImage(m_hCamHandle, strFileName, 80);
            //return true; ;
            //byte[] managedArray = new byte[pImage.uiSize];
            //Marshal.Copy(pImage.pData, managedArray, 0, (int)pImage.uiSize);
            //Mat mat = Mat.FromImageData(managedArray, LoadMode.Color);
            //mat.SaveImage(@"c:\test_imi2.bmp");//NeptuneC.ntcSaveImage(m_hCamHandle, @"c:\f.bmp", 80);
            #endregion
            
            UInt32 nRGBSize = pImage.uiWidth * pImage.uiHeight * 3;
            Byte[] RGBArr = new Byte[nRGBSize];
            IntPtr pRGBBuf = Marshal.AllocHGlobal(Marshal.SizeOf(RGBArr[0]) * (int)nRGBSize);
            NeptuneC.ntcGetRGBData(m_hCamHandle, pRGBBuf, nRGBSize); // ntcGetRGBData를 이용하여 RGB 버퍼 가져옴                   
            
            m_Bitmap = new Bitmap((int)pImage.uiWidth, (int)pImage.uiHeight, PixelFormat.Format24bppRgb);
            m_BitmapData = m_Bitmap.LockBits(new Rectangle(0, 0, (int)pImage.uiWidth, (int)pImage.uiHeight),
                                           ImageLockMode.ReadWrite,
                                            PixelFormat.Format24bppRgb);

            NeptuneC.CopyMemory(m_BitmapData.Scan0, pRGBBuf, (uint)(pImage.uiWidth * pImage.uiHeight * 3));
            m_Bitmap.UnlockBits(m_BitmapData);
            
            // m_Bitmap.Save(@"c:\test_imi1.bmp"); // 간단한 예로 Bitmap 클래스를 이용하여 bmp파일 저장
            // m_Bitmap.Dispose();
            Marshal.FreeHGlobal(pRGBBuf);
            
            bCaptured = true;
            return true;
        }
        
        public bool InitCamera(UInt32 IMI_PixelFormat)
        {
            string s = Config.sCurrentWorkingPath;
            ENeptuneError ret = NeptuneC.ntcInit();

            if (ret != ENeptuneError.NEPTUNE_ERR_Success)
            {
                MessageBox.Show("Library initialization error" + ret);
                return false;
            }

            #region Skip
            // comboBayerConversion.Items.Add("None");
            // comboBayerConversion.Items.Add("Bilinear");
            // comboBayerConversion.Items.Add("BilinearHQ");
            //comboBayerConversion.Items.Add("Nearest");
            //comboBayerLayout.Items.Add("GB_RG");
            //comboBayerLayout.Items.Add("BG_GR");
            //comboBayerLayout.Items.Add("RG_GB");
            //comboBayerLayout.Items.Add("GR_BG");
            
            #endregion
            
            UpdateCameraList();

            if (m_nCameras <= 0)
            {
                MessageBox.Show("No Imi Camera");
                return false;
            }

            // SELECT CAMERA
            if (m_hCamHandle != IntPtr.Zero)
            {
                NeptuneC.ntcClose(m_hCamHandle);
                m_hCamHandle = IntPtr.Zero;
            }

            //if (comboCamera.SelectedIndex == -1)
            //    return;
            
            // [ SELECT 0 CAM ] 
            UInt32 Index = 0; 
            ret = NeptuneC.ntcOpen(m_CamInfo[Index].strCamID, ref m_hCamHandle, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
            if (ret != ENeptuneError.NEPTUNE_ERR_Success)
            {
                MessageBox.Show("Can not select camera! - " + ret);
                return false;
            }
            m_nCamIndex = Index;

            m_strSelectedCam = string.Format(m_CamInfo[Index].strCamID);

            ret = NeptuneC.ntcGetCameraType(m_hCamHandle, ref m_eCamType);
            if (ret != ENeptuneError.NEPTUNE_ERR_Success)
            {
                return false;
            }
            
            SetBayerConversion();

            #region Skip
            // UpdatePixelFormat();
            // UpdateFrameRate();
            // UpdateAcquisitionMode();
            
            #endregion

            // cam 선택 완료. 
            
            if (m_hCamHandle != IntPtr.Zero)
            {
                ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;

                NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);
                if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
                    NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

                // get current index 
                ENeptunePixelFormat Format = (ENeptunePixelFormat)IMI_PixelFormat;
                NeptuneC.ntcSetPixelFormat(m_hCamHandle, Format);

                if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
                    NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);

                // UpdateFrameRate();
            }
            else
                MessageBox.Show("Camera is not selected!");

            // set single mode 
            if (m_hCamHandle != IntPtr.Zero)
            {
                ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;
                NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);

                if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
                    NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

                NeptuneC.ntcSetAcquisitionMode(m_hCamHandle, ENeptuneAcquisitionMode.NEPTUNE_ACQ_SINGLEFRAME, 0);

                m_nFrameCount = 0;
                if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
                    NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
            }

            return true;
        }

        // play
        public void Play(IntPtr _hDisplayWnd, IntPtr _Handle) //  ( pictureDisplay.Handle, wiform.Control.Handle )
        {
            if (m_hCamHandle == IntPtr.Zero)
                return;
            
            // frame count initialize
            m_nFrameCount = 0;
            
            // frame count thread start
            m_Thread = new Thread(new ThreadStart(UpdateFrameCount));
            m_Thread.IsBackground = true;

            m_bPlay = true;
            m_Thread.Start();

            // register frame callback
            NeptuneCFrameCallbackInstance = new NeptuneCFrameCallback(FrameCallback);
            NeptuneC.ntcSetFrameCallback(m_hCamHandle, NeptuneCFrameCallbackInstance, _Handle); //  this.Handle);

            // set display handle
            IntPtr hDisplayWnd = _hDisplayWnd;  // pictureDisplay.Handle;
            NeptuneC.ntcSetDisplay(m_hCamHandle, hDisplayWnd);
            
            // play
            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
        }

        public void StopCamera()
        {
            if (m_hCamHandle == IntPtr.Zero)
                return;

            // frame count thread stop
            if (m_bPlay)
            {
                m_bPlay = false;
                m_Thread.Join();
                m_Thread = null;

                // stop camera
                NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

                // unregister frame callback
                NeptuneC.ntcSetFrameCallback(m_hCamHandle, null, IntPtr.Zero);
            }
        }

        public void CloseCameara()
        {
            if (m_hCamHandle != IntPtr.Zero)
                NeptuneC.ntcClose(m_hCamHandle);
            NeptuneC.ntcUninit();
        }

        public bool SetParam( ENeptuneFeature eFeature,Int32 nValue )
        {
            NEPTUNE_FEATURE FeatureInfo = new NEPTUNE_FEATURE();
            FeatureInfo.Value = nValue;
            if (NeptuneC.ntcSetFeature(m_hCamHandle, eFeature, FeatureInfo) == ENeptuneError.NEPTUNE_ERR_Success)
            {
         
                
                return true;
            }
            return false;
        }

        public long GetColorBalance(AverageColor clr)
        {
            return 0;
        }
        public void SetColorBalance(AverageColor clr, long value)
        {

        }
        public void WBAuto()
        {

        }
        public void SaveUserSet()
        {

        }
        public void OneShot()
        {
            ClearCapture();
            if (m_hCamHandle != IntPtr.Zero)
                NeptuneC.ntcOneShot(m_hCamHandle);

            return;
        }

        private void UpdateFrameCount()
        {
            while (m_bPlay)
            {
                try
                {
                    // labelFrames.Text = "Receive Frames : " + m_nFrameCount;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
                Thread.Sleep(10);
            }
        }

        private void UpdateCameraList()
        {
            UInt32 nCameras = 0;

            ENeptuneError ret = NeptuneC.ntcGetCameraCount(ref nCameras);
            if (ret != ENeptuneError.NEPTUNE_ERR_Success)
                return;

            // update camera count
            m_nCameras = nCameras;

            // CheckForIllegalCrossThreadCalls = false;

            if (m_nCameras > 0)
            {
                m_CamInfo = new NEPTUNE_CAM_INFO[m_nCameras];
                IntPtr pCamInfo = NeptuneC.MarshalArrtoIntPtr<NEPTUNE_CAM_INFO>(m_CamInfo);

                ret = NeptuneC.ntcGetCameraInfo(pCamInfo, m_nCameras);
                if (ret == ENeptuneError.NEPTUNE_ERR_Success)
                    NeptuneC.UnmarshalIntPtrToArr<NEPTUNE_CAM_INFO>(pCamInfo, ref m_CamInfo);

                Marshal.FreeHGlobal(pCamInfo);
            }

            #region Skip
		    //comboCamera.SelectedIndex = -1;
            //comboCamera.Items.Clear();
            //for (int i = 0; i < m_nCameras; i++)
            //{
            //    comboCamera.Items.Add(m_CamInfo[i].strVendor + " : " + m_CamInfo[i].strModel + " : " + m_CamInfo[i].strSerial);
            //    if (m_strSelectedCam != null && m_strSelectedCam.Equals(m_CamInfo[i].strCamID, StringComparison.Ordinal))
            //        comboCamera.SelectedIndex = i;
            //}
        	#endregion        
        }

        private void UpdatePixelFormat()
        {
            if (m_hCamHandle != IntPtr.Zero)
            {
                UInt32 nSize = 0;
                ENeptuneError ret = NeptuneC.ntcGetPixelFormatList(m_hCamHandle, IntPtr.Zero, ref nSize);

                // comboPixelFormat.Items.Clear();
                if (ret == ENeptuneError.NEPTUNE_ERR_Success)
                {
                    m_FormatList = new Int32[nSize];
                    IntPtr pFormatList = AllocIntPtrFromArr(m_FormatList);

                    ret = NeptuneC.ntcGetPixelFormatList(m_hCamHandle, pFormatList, ref nSize);
                    if (ret == ENeptuneError.NEPTUNE_ERR_Success)
                        CopyArrFromUnmanagedIntPtr(pFormatList, m_FormatList);
                    FreeIntPtrFromArr(pFormatList);

                    ENeptunePixelFormat PixelFormat = ENeptunePixelFormat.Unknown_PixelFormat;
                    NeptuneC.ntcGetPixelFormat(m_hCamHandle, ref PixelFormat);
                    for (int i = 0; i < m_FormatList.Length; i++)
                    {
                        //comboPixelFormat.Items.Add((ENeptunePixelFormat)m_FormatList[i]);
                        //if (PixelFormat == (ENeptunePixelFormat)m_FormatList[i])
                        //    comboPixelFormat.SelectedIndex = i;
                    }
                }
            }
        }

        private void UpdateFrameRate()
        {
            if (m_hCamHandle != IntPtr.Zero)
            {
                UInt32 nSize = 0;
                ENeptuneError ret = NeptuneC.ntcGetFrameRateList(m_hCamHandle, IntPtr.Zero, ref nSize);

                //comboFRate.Enabled = false;
                //comboFRate.SelectedIndex = -1;
                //comboFRate.Items.Clear();
                if (ret == ENeptuneError.NEPTUNE_ERR_Success)
                {
                    // comboFRate.Enabled = true;

                    ENeptuneFrameRate FrameRate = ENeptuneFrameRate.FPS_UNKNOWN;
                    double dFrameRate = 0.0;
                    NeptuneC.ntcGetFrameRate(m_hCamHandle, ref FrameRate, ref dFrameRate);

                    if (nSize > 0)
                    {
                        m_FrateList = new Int32[nSize];
                        IntPtr pFRateList = AllocIntPtrFromArr(m_FrateList);

                        ret = NeptuneC.ntcGetFrameRateList(m_hCamHandle, pFRateList, ref nSize);
                        if (ret == ENeptuneError.NEPTUNE_ERR_Success)
                            CopyArrFromUnmanagedIntPtr(pFRateList, m_FrateList);
                        FreeIntPtrFromArr(pFRateList);

                        for (int i = 0; i < m_FrateList.Length; i++)
                        {
                            //comboFRate.Items.Add((ENeptuneFrameRate)m_FrateList[i]);
                            //if (FrameRate == (ENeptuneFrameRate)m_FrateList[i])
                            //    comboFRate.SelectedIndex = i;
                        }
                        //comboFRate.Enabled = true;
                        //textFrameRate.Enabled = false;
                        //buttonSetFrameRate.Enabled = false;
                    }
                    else
                    {
                        string s;
                        s = dFrameRate.ToString("0.00");
                        //textFrameRate.Text = s;
                        //textFrameRate.Enabled = true;
                        //buttonSetFrameRate.Enabled = true;
                        //comboFRate.Enabled = false;
                    }
                }
            }
        }

        private void SetBayerConversion()
        {
            if (m_hCamHandle != IntPtr.Zero)
            {
                ENeptuneBayerMethod eMethod = ENeptuneBayerMethod.NEPTUNE_BAYER_METHOD_NONE;
                // NeptuneC.ntcGetBayerConvert(m_hCamHandle, ref eMethod);  
                // comboBayerConversion.SelectedIndex = (Int32)eMethod;
                NeptuneC.ntcSetBayerConvert(m_hCamHandle, (ENeptuneBayerMethod)0);

                ENeptuneBayerLayout eLayout = ENeptuneBayerLayout.NEPTUNE_BAYER_GB_RG;
                // NeptuneC.ntcGetBayerLayout(m_hCamHandle, ref eLayout);
                // comboBayerLayout.SelectedIndex = (Int32)eLayout;
                NeptuneC.ntcSetBayerLayout(m_hCamHandle, (ENeptuneBayerLayout)eLayout);
            }
        }

        public void DisplaySetupPannel()
        {
            if (m_hCamHandle != IntPtr.Zero)
                NeptuneC.ntcShowControlDialog(m_hCamHandle);
        }

        public void ContinuousShot()
        {
            //if (m_Thread == null)
            //{
            //    m_Thread = new Thread(new ThreadStart(AcquisitionThread));
            //    m_bPlay = true;
            //    m_Thread.Start();
            //}
        } 
        
        public void Connect()
        {
            #region Skip
            //if (Cam1 != null)
            //{
            //    Cam1.CameraClose();
            //    Cam1 = null;
            //}

            //NeptuneDevice iDev = DeviceManager.Instance.GetDeviceFromSerial(m_CamInfo[SelectedCam].strSerial);
            //try
            //{
            //    Cam1 = new CameraInstance(iDev, ENeptuneDevAccess.NEPTUNE_DEV_ACCESS_EXCLUSIVE);
            //}
            //catch (System.Exception exp)
            //{
            //    string strExp = "Can not select camera! - " + exp.Message;
            //    MessageBox.Show(strExp);
            //    return;
            //}

            //UpdatePixelFormat();

            ////0 : mono8 1:mono12 2:bayergr8 3:bayergr12 4:yuv411packed 5: yuv422packed
            //Cam1.SetPixelFormat(m_strPixelFormatList[4]);
            //PixelFromat = m_strPixelFormatList[4];
            ////UpdateFrameRate();

            //UpdateFeature(out nGain, out nShutter);

            //m_Display = new DisplayImage(pictureBoxDisplay.Handle);

            //// 1245 : 30ms 1445 : 50ms 1729 100ms 1930 : 300ms 2129 : 500ms
            //if (GrabMode == ENeptuneGrabMode.NEPTUNE_GRAB_ONE)
            //{
            //    if (Cam1.AcquisitionStart(ENeptuneGrabMode.NEPTUNE_GRAB_ONE) != ENeptuneError.NEPTUNE_ERR_Success)
            //    {
            //        MessageBox.Show("Acquisition start error!");
            //        return;
            //    }

            //}
            //else if (GrabMode == ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS)
            //{
            //    if (Cam1.AcquisitionStart(ENeptuneGrabMode.NEPTUNE_GRAB_CONTINUOUS) != ENeptuneError.NEPTUNE_ERR_Success)
            //    {
            //        MessageBox.Show("Acquisition start error!");
            //        return;
            //    }
            //} 
            #endregion
        }
        #region Skip
        //private void UpdateFeature(out int nGain, out int nShutter)
        //{
        //    nGain = 0;
        //    nShutter = 0;

        //    #region Skip
        //    //// Gain
        //    //NEPTUNE_FEATURE featureGain = new NEPTUNE_FEATURE();
        //    //ENeptuneError eErr = Cam1.GetFeature(ENeptuneFeature.NEPTUNE_FEATURE_GAIN, ref featureGain);

        //    //if (eErr != ENeptuneError.NEPTUNE_ERR_Fail && featureGain.bSupport == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //    //{
        //    //    nGain = featureGain.Value;
        //    //}
        //    //// Shutter
        //    //NEPTUNE_FEATURE featureShutter = new NEPTUNE_FEATURE();
        //    //eErr = Cam1.GetFeature(ENeptuneFeature.NEPTUNE_FEATURE_SHUTTER, ref featureShutter);
        //    //if (eErr != ENeptuneError.NEPTUNE_ERR_Fail && featureShutter.bSupport == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //    //{
        //    //    nShutter = featureShutter.Value;
        //    //}
        //    #endregion        
        //}

        //private void AcquisitionThread()
        //{
        //    #region Skip

        //    //string strPixelFormat = PixelFromat;
        //    //bool bYUV = strPixelFormat.Contains("YUV") ? true : false;

        //    //FrameDataPtr data = new FrameDataPtr();

        //    //while (m_bPlay)
        //    //{
        //    //    ENeptuneError eErr = Cam1.WaitEventDataStream(ref data, 1000);
        //    //    if (eErr != ENeptuneError.NEPTUNE_ERR_Success)
        //    //        continue;

        //    //    if (!bYUV && m_eBayerMethod > ENeptuneBayerMethod.NEPTUNE_BAYER_METHOD_NONE)
        //    //    {
        //    //        if (Cam1.SetBayer(m_ConvertedData,
        //    //                       (uint)m_ConvertedData.Length,
        //    //                       data,
        //    //                       m_eBayerLayout,
        //    //                       m_eBayerMethod,
        //    //                       0) == ENeptuneError.NEPTUNE_ERR_Success)
        //    //        {
        //    //            m_Display.DrawConvertImage(m_ConvertedData, (uint)m_ConvertedData.Length, data.GetWidth(), data.GetHeight(), 0);
        //    //        }
        //    //    }

        //    //    else
        //    //        m_Display.DrawRawImage(data);

        //    //    Cam1.QueueBufferDataStream(data.GetBufferIndex());
        //    // } 
        //    #endregion
        //}

        //private void grab(int nGain, int nExposure)
        //{
        //    //if (nGain >= 0 && nGain <= 800 && nExposure >= 300 && nExposure <= 2129)
        //    //{
        //    //    NEPTUNE_FEATURE featureGain = new NEPTUNE_FEATURE();
        //    //    Cam1.GetFeature(ENeptuneFeature.NEPTUNE_FEATURE_GAIN, ref featureGain);
        //    //    featureGain.Value = nGain;
        //    //    Cam1.SetFeature(ENeptuneFeature.NEPTUNE_FEATURE_GAIN, featureGain);
        //    //    NEPTUNE_FEATURE featureShutter = new NEPTUNE_FEATURE();
        //    //    Cam1.GetFeature(ENeptuneFeature.NEPTUNE_FEATURE_SHUTTER, ref featureShutter);
        //    //    featureShutter.Value = nExposure;
        //    //    Cam1.SetFeature(ENeptuneFeature.NEPTUNE_FEATURE_SHUTTER, featureShutter);
        //    //}
        //    //else
        //    //    MessageBox.Show("check gain & exposure time");
        //    //FrameDataPtr data = new FrameDataPtr();
        //    //IntPtr temp = new IntPtr();
        //    //ENeptuneError eErr = Cam1.OneFrameGrab(ref data, 300, temp);
        //    //if (eErr != ENeptuneError.NEPTUNE_ERR_Success)
        //    //    MessageBox.Show("err");
        //    //else
        //    //    m_Display.DrawRawImage(data);
        //}

        //private void UpdateAcquisitionMode()
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        ENeptuneAcquisitionMode eAcqMode = ENeptuneAcquisitionMode.NEPTUNE_ACQ_SINGLEFRAME;
        //        UInt32 nFrames = 0;

        //        if ((ENeptuneError)NeptuneC.ntcGetAcquisitionMode(m_hCamHandle, ref eAcqMode, ref nFrames) == ENeptuneError.NEPTUNE_ERR_Success)
        //        {
        //            switch (eAcqMode)
        //            {
        //                case ENeptuneAcquisitionMode.NEPTUNE_ACQ_SINGLEFRAME:
        //                    //buttonOneShot.Enabled = true;
        //                    //buttonMultiShot.Enabled = false;
        //                    //radioSingleFrame.Checked = true;
        //                    //radioMultiFrame.Checked = false;
        //                    //radioContinuous.Checked = false;
        //                    break;
        //                case ENeptuneAcquisitionMode.NEPTUNE_ACQ_MULTIFRAME:
        //                    //buttonOneShot.Enabled = false;
        //                    //buttonMultiShot.Enabled = true;
        //                    //radioSingleFrame.Checked = false;
        //                    //radioMultiFrame.Checked = true;
        //                    //radioContinuous.Checked = false;
        //                    break;
        //                case ENeptuneAcquisitionMode.NEPTUNE_ACQ_CONTINUOUS:
        //                    //buttonOneShot.Enabled = false;
        //                    //buttonMultiShot.Enabled = false;
        //                    //radioSingleFrame.Checked = false;
        //                    //radioMultiFrame.Checked = false;
        //                    //radioContinuous.Checked = true;
        //                    break;
        //            }
        //        }
        //    }
        //}


        //private void comboPixelFormat_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;

        //        NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

        //        // get current index 
        //        UInt32 Index = (UInt32)comboPixelFormat.SelectedIndex;
        //        ENeptunePixelFormat Format = (ENeptunePixelFormat)m_FormatList[Index];

        //        NeptuneC.ntcSetPixelFormat(m_hCamHandle, Format);

        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);

        //        UpdateFrameRate();
        //    }
        //    else
        //        MessageBox.Show("Camera is not selected!");
        //}

        //private void comboFrate_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        // stop camera
        //        ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;
        //        NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

        //        // set frame rate
        //        UInt32 Index = (UInt32)comboFRate.SelectedIndex;
        //        ENeptuneFrameRate Rate = (ENeptuneFrameRate)m_FrateList[Index];

        //        double dValue = 0;
        //        NeptuneC.ntcSetFrameRate(m_hCamHandle, Rate, dValue);

        //        // play camera
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
        //    }
        //    else
        //        MessageBox.Show("Camera is not selected!");
        //}


        //private void comboBayerConversion_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        UInt32 Index = (UInt32)comboBayerConversion.SelectedIndex;
        //        NeptuneC.ntcSetBayerConvert(m_hCamHandle, (ENeptuneBayerMethod)Index);
        //    }
        //}

        //private void comboBayerLayout_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        UInt32 Index = (UInt32)comboBayerLayout.SelectedIndex;
        //        NeptuneC.ntcSetBayerLayout(m_hCamHandle, (ENeptuneBayerLayout)Index);
        //    }
        //}

        //private void buttonOneShot_Click(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //        NeptuneC.ntcOneShot(m_hCamHandle);
        //}

        //private void buttonMultiShot_Click(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //        NeptuneC.ntcMultiShot(m_hCamHandle);
        //}

        //private void buttonControl_Click(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //        NeptuneC.ntcShowControlDialog(m_hCamHandle);
        //}

        //private void UpdateFrameCount()
        //{
        //    while (m_bPlay)
        //    {
        //        try
        //        {
        //            labelFrames.Text = "Receive Frames : " + m_nFrameCount;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            return;
        //        }
        //        Thread.Sleep(10);
        //    }
        //}

        //private void radioSingleFrame_Click(object sender, EventArgs e)
        //{
        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;
        //        NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);

        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

        //        NeptuneC.ntcSetAcquisitionMode(m_hCamHandle, ENeptuneAcquisitionMode.NEPTUNE_ACQ_SINGLEFRAME, 0);

        //        buttonOneShot.Enabled = true;
        //        buttonMultiShot.Enabled = false;

        //        m_nFrameCount = 0;
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
        //    }
        //}

        //private void radioMultiFrame_Click(object sender, EventArgs e)
        //{

        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;
        //        NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);

        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

        //        NeptuneC.ntcSetAcquisitionMode(m_hCamHandle, ENeptuneAcquisitionMode.NEPTUNE_ACQ_MULTIFRAME, 10);

        //        buttonOneShot.Enabled = false;
        //        buttonMultiShot.Enabled = true;

        //        m_nFrameCount = 0;
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
        //    }
        //}

        //private void radioContinuous_Click(object sender, EventArgs e)
        //{

        //    if (m_hCamHandle != IntPtr.Zero)
        //    {
        //        ENeptuneBoolean eState = ENeptuneBoolean.NEPTUNE_BOOL_FALSE;
        //        NeptuneC.ntcGetAcquisition(m_hCamHandle, ref eState);

        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_FALSE);

        //        NeptuneC.ntcSetAcquisitionMode(m_hCamHandle, ENeptuneAcquisitionMode.NEPTUNE_ACQ_CONTINUOUS, 0);

        //        buttonOneShot.Enabled = false;
        //        buttonMultiShot.Enabled = false;

        //        m_nFrameCount = 0;
        //        if (eState == ENeptuneBoolean.NEPTUNE_BOOL_TRUE)
        //            NeptuneC.ntcSetAcquisition(m_hCamHandle, ENeptuneBoolean.NEPTUNE_BOOL_TRUE);
        //    }
        //}
        //private NeptuneClassLibCLR Neptune1 = null;
        //private NEPTUNE_CAM_INFO[] m_CamInfo = null;
        //private CameraInstance Cam1 = null;
        //private string[] m_strPixelFormatList = null;
        //private string[] m_strFrameRateList = null;
        //private DisplayImage m_Display = null;
        //private ENeptuneBayerMethod m_eBayerMethod = ENeptuneBayerMethod.NEPTUNE_BAYER_METHOD_NONE;
        //private ENeptuneBayerLayout m_eBayerLayout = ENeptuneBayerLayout.NEPTUNE_BAYER_GB_RG;
        //private sbyte[] m_ConvertedData = null;
        //private Thread m_Thread = null;
        //private bool m_bPlay = false;
        #endregion
        #region Skip

        // ENeptuneGrabMode GrabMode;
        //public NeptuneCam(PictureBox _pictureBoxDisplay, ENeptuneGrabMode _GrabMode )
        //{
        //    //pictureBoxDisplay = _pictureBoxDisplay;
        //    //GrabMode = _GrabMode;

        //    //Neptune1 = new NeptuneClassLibCLR();
        //    //Neptune1.InitLibrary();
        //    //UpdateCameraList();
        //    //Connect();
        //}


        //private void UpdatePixelFormat()
        //{
        //    //uint nCount = 0;
        //    //Cam1.GetPixelFormatList(m_strPixelFormatList, ref nCount);

        //    //if (nCount > 0)
        //    //{
        //    //    m_strPixelFormatList = new string[nCount];
        //    //    Cam1.GetPixelFormatList(m_strPixelFormatList, ref nCount);
        //    //}

        //    //string strPixelFormat = "";
        //    //Cam1.GetPixelFormat(ref strPixelFormat);
        //    //for (int i = 0; i < nCount; i++)
        //    //{
        //    //    Console.WriteLine(m_strPixelFormatList[i]);
        //    //}
        //}

        //public void UpdateCameraList()
        //{
        //    //uint nCam = DeviceManager.Instance.GetTotalCamera();
        //    //m_CamInfo = new NEPTUNE_CAM_INFO[nCam];
        //    //DeviceManager.Instance.GetCameraList(m_CamInfo, nCam);
        //}
        
        #endregion
    }
}
