using AutoInspection.sec;
using AutoInspection.Utils;
using AutoInspection_GUMI;
using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace AutoInspection
{
    public class BaslerCamera : ICameraInterface
    {
        public Camera camera = null;
        
        private PixelDataConverter converter = new PixelDataConverter();
        private Stopwatch stopWatch = new Stopwatch();

        //PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE();
        private PictureBox thisControl;
        public Bitmap Image_BASLER; 
        public bool isgrabed = false;
        public bool iscontinue = false;

        private int gain;
        private int exposure;
        bool EventHandler1 = false; // 이밴트 종료 핸들러

        // Set up the controls and events to be used and update the device list.
        public bool isContinuous()
        {
            if(iscontinue == true)
                return true;
            return false;
        }
        public void SetPictureBox( PictureBox control)
        {
            this.thisControl = control;
        }

        public void DisplaySetupPannel()
        {
        }

        public BaslerCamera(string modelName)
        {
            List<ICameraInfo> allCameras = CameraFinder.Enumerate();
            int index = 0;
            try
            {
                // Create a new camera object.
                camera = new Camera(allCameras[index]);
                if (!camera.ToString().Contains(modelName))
                {
                    index++;
                    camera = new Camera(allCameras[index]);
                }
                
                camera.CameraOpened += Configuration.AcquireContinuous;

                // Register for the events of the image provider needed for proper operation.
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;

                // Open the connection to the camera device.
                //Thread.Sleep(100);
                camera.Open();
            }
            catch 
            {
                Thread.Sleep(300);
                try
                {
                    #region Skip
                    //camera.CameraOpened += Configuration.AcquireContinuous;
                    //// Register for the events of the image provider needed for proper operation.
                    //camera.ConnectionLost += OnConnectionLost;
                    //camera.CameraOpened += OnCameraOpened;
                    //camera.CameraClosed += OnCameraClosed;
                    //camera.StreamGrabber.GrabStarted += OnGrabStarted;
                    //camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    //camera.StreamGrabber.GrabStopped += OnGrabStopped;
                    #endregion
                    camera.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Log.AddLog("BaslerCamera() in BaslerCamera exception " + ex.ToString());
                    Log.AddPmLog(ex.ToString());


                    #region Skip
                    //try
                    //{
                    //    LanCard("Camera", false);
                    //    LanCard("Camera", true);

                    //    Thread.Sleep(8000);

                    //    camera.CameraOpened += Configuration.AcquireContinuous;

                    //    // Register for the events of the image provider needed for proper operation.
                    //    camera.ConnectionLost += OnConnectionLost;
                    //    camera.CameraOpened += OnCameraOpened;
                    //    camera.CameraClosed += OnCameraClosed;
                    //    camera.StreamGrabber.GrabStarted += OnGrabStarted;
                    //    camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                    //    camera.StreamGrabber.GrabStopped += OnGrabStopped;

                    //    camera.Open();
                    //MessageBox.Show(e.ToString());
                    //}
                    //catch(Exception eee)
                    //{
                    //    m
                    //}
                    #endregion
                }
            }
        }

        public void Start()
        {
            new NotImplementedException();
        }
        
        private void cmdProcess_Exited(object sender, System.EventArgs e)
        {
            EventHandler1 = true;   //외부 프로그램이 종료되면 이밴트 핸들러 전역변수를 true로 전환
        }

        public void LanCard(string 랜카드_이름, bool ON_OFF) //2015-04-24
        {
            string Alive = "disabled";
            if (ON_OFF == true)
            {
                Alive = "enabled";
            }

            System.Diagnostics.Process netsh = new System.Diagnostics.Process();
            netsh.StartInfo.FileName = "Netsh";

            if (랜카드_이름 != "")
            {
                try
                {
                    netsh.StartInfo.Arguments = "interface set interface name=" + '"' + 랜카드_이름 + '"' + " admin=" + Alive;
                    netsh.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    netsh.EnableRaisingEvents = true;
                    netsh.Exited += new EventHandler(cmdProcess_Exited);
                    EventHandler1 = false;
                    netsh.Start();
                    int script_run_time = 12000; //외부 프로그램의 최대 실행 시간 지정
                    int elapsedTime = 0;
                    const int SLEEP_AMOUNT = 100;
                    while (!EventHandler1)
                    {
                        elapsedTime += SLEEP_AMOUNT;
                        if (elapsedTime > script_run_time)
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(SLEEP_AMOUNT);
                        //_Delay(SLEEP_AMOUNT);
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            if (iscontinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnConnectionLost), sender, e);
                    return;
                }
            }
            
            // Close the camera object.
            DestroyCamera();
        }

        // Occurs when the connection to a camera device is opened.
        private void OnCameraOpened(Object sender, EventArgs e)
        {
            if (iscontinue)
            {

                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnCameraOpened), sender, e);
                    return;
                }
            }
        }

        // Occurs when the connection to a camera device is closed.
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            if (iscontinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnCameraClosed), sender, e);
                    return;
                }
            }
        }

        // Occurs when a camera starts grabbing.
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            if (iscontinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<EventArgs>(OnGrabStarted), sender, e);
                    return;
                }
            }
            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.

            stopWatch.Reset();
            
        }

        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            if (iscontinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
                    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
                    thisControl.BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
                    return;
                }
            }
            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();

                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        // Lock the bits of the bitmap.
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        // Place the pointer to the buffer of the bitmap.
                        converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                        bitmap.UnlockBits(bmpData);
                        // bitmap.Save(@".\Detail\Area\cameraraw1.bmp");

                        // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
                        if (iscontinue)
                        {
                            // bitmap.Save(@".\Detail\Area\cameraraw2.bmp");

                            Bitmap bitmapOld = ((PictureBox)thisControl).Image as Bitmap;
                            // Provide the display control with the new bitmap. This action automatically updates the display.
                            ((PictureBox)thisControl).Image = bitmap;
                            if (bitmapOld != null)
                            {
                                // Dispose the bitmap.
                                bitmapOld.Dispose();
                            }
                        }
                        else
                        {
                            // Lay anh tu CAm gan vao bien Image_Basler;
                            if (bitmap != null)
                            {
                                Image_BASLER = bitmap;
                                isgrabed = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddLog("OnImageGrabbed() in BaslerCamera exception " + ex.ToString());
                Log.AddPmLog("OnImageGrabbed() in BaslerCamera exception " + ex.ToString());

            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }

        // Occurs when a camera has stopped grabbing.
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            if (iscontinue)
            {
                if (thisControl.InvokeRequired)
                {
                    // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                    thisControl.BeginInvoke(new EventHandler<GrabStopEventArgs>(OnGrabStopped), sender, e);
                    return;
                }
            }

            // Reset the stopwatch.
            stopWatch.Reset();

            // If the grabbed stop due to an error, display the error message.
            if(e.Reason != GrabStopReason.UserRequest)
            {
                MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Stops the grabbing of images and handles exceptions.
        public void Stop()
        {
            // Stop the grabbing.
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception ex)
            {
               Log.AddLog("Stop() in BaslerCamera exception " + ex.ToString());
                Log.AddPmLog("Stop() in BaslerCamera exceptions " + ex.ToString());

            }
        }

        // Closes the camera object and handles exceptions.
        public void DestroyCamera()
        {
            // Destroy the camera object.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch  ( Exception e ) 
            {
                Log.AddLog(e.ToString()); 
                Log.AddPmLog(e.ToString());

            }
        }

        // Starts the grabbing of a single image and handles exceptions.
        public void OneShot()
        {
            try
            {
                // Starts the grabbing of one image.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                isgrabed = false;
            }
            catch (Exception ex)
            {
                Log.AddLog("OneShot() in BaslerCamera exception " + ex.ToString());
                Log.AddPmLog("OneShot() in BaslerCamera exception " + ex.ToString());

            }
        }

        // Starts the continuous grabbing of images and handles exceptions.
        public void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception ex)
            {
                Log.AddLog("ContinuousShot() in BaslerCamera exception " + ex.ToString());
                Log.AddPmLog("ContinuousShot() in BaslerCamera exception " + ex.ToString());
            }
        }

        public void GetParameter(ref CameraParams cameraParams)
        {
            cameraParams.ExposureValue = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
            cameraParams.MinExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
            cameraParams.MaxExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
            cameraParams.Width = (int)camera.Parameters[PLCamera.Width].GetValue();
            cameraParams.MinWidth = (int)camera.Parameters[PLCamera.Width].GetMinimum();
            cameraParams.MaxWidth = (int)camera.Parameters[PLCamera.Width].GetMaximum();
            cameraParams.Height = (int)camera.Parameters[PLCamera.Height].GetValue();
            cameraParams.MinHeight = (int)camera.Parameters[PLCamera.Height].GetMinimum();
            cameraParams.MaxHeight = (int)camera.Parameters[PLCamera.Height].GetMaximum();
            cameraParams.Xoffset = (int)camera.Parameters[PLCamera.OffsetX].GetValue();
            cameraParams.MinXoff = (int)camera.Parameters[PLCamera.OffsetX].GetMinimum();
            cameraParams.MaxXoff = (int)camera.Parameters[PLCamera.OffsetX].GetMaximum();
            cameraParams.Yoffset = (int)camera.Parameters[PLCamera.OffsetY].GetValue();
            cameraParams.MinYoff = (int)camera.Parameters[PLCamera.OffsetY].GetMinimum();
            cameraParams.MaxYoff = (int)camera.Parameters[PLCamera.OffsetY].GetMaximum();
        }

        public void SaveUserSet()
        {
            SetLivePlay(false);
            Stop();
            //Console.WriteLine(camera.Parameters[PLCamera.UserSetSelector].GetValue() );
            camera.Parameters[PLCamera.UserSetSelector].SetValue("UserSet1");
            camera.Parameters[PLCamera.UserSetSave].TryExecute();
        }
       public void WBAuto()
        {
            camera.Parameters[PLCamera.BalanceWhiteAuto].SetValue("Once");
        }
        public long GetColorBalance(AverageColor clr)
        {
            if (clr == AverageColor.RED)
            {
                camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Red");
                return camera.Parameters[PLCamera.BalanceRatioRaw].GetValue();
            }
            else
            {
                camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Blue");
                return camera.Parameters[PLCamera.BalanceRatioRaw].GetValue();
            }
        }

        public void SetColorBalance(AverageColor clr, long value)
        {
            if(clr == AverageColor.RED)
            {
                camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Red");
                camera.Parameters[PLCamera.BalanceRatioRaw].SetValue(value);
            }
            else
            {
                camera.Parameters[PLCamera.BalanceRatioSelector].SetValue("Blue");
                camera.Parameters[PLCamera.BalanceRatioRaw].SetValue(value);
            }
        }
        //public void GetParameter(out int ExposureValue, out int minExposure, out int maxExposure,
        //   out int Width, out int minWidth, out int maxWidth,
        //   out int Height, out int minHeight, out int maxHeight)
        //{
        //    ExposureValue = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
        //    minExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
        //    maxExposure = (int)camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
        //    Width = (int)camera.Parameters[PLCamera.Width].GetValue();
        //    minWidth = (int)camera.Parameters[PLCamera.Width].GetMinimum();
        //    maxWidth = (int)camera.Parameters[PLCamera.Width].GetMaximum();
        //    Height = (int)camera.Parameters[PLCamera.Height].GetValue();
        //    minHeight = (int)camera.Parameters[PLCamera.Height].GetMinimum();
        //    maxHeight = (int)camera.Parameters[PLCamera.Height].GetMaximum();

        //}
        public void SetLivePlay(bool bLiveMode)
        {
            if (bLiveMode)
                iscontinue = true;
            else
                iscontinue = false;
        }
        public void SetParameter(CameraParams cameraParams)
        {
            try
            {
                //InitCamFOV();

                camera.Parameters[PLCamera.Width].TrySetValue(cameraParams.Width);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.Height].TrySetValue(cameraParams.Height);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.OffsetX].TrySetValue(cameraParams.Xoffset);
                Thread.Sleep(50);
                camera.Parameters[PLCamera.OffsetY].TrySetValue(cameraParams.Yoffset);
                Thread.Sleep(50);
            }
            catch (Exception e)
            {
                Log.AddLog(e.ToString());
                Log.AddPmLog(e.ToString());

            }
        }
        public void InitCamFOV()
        {
            camera.Parameters[PLCamera.OffsetX].TrySetValue(0);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.OffsetY].TrySetValue(0);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.WidthMax].TrySetValue(4608);
            Thread.Sleep(50);
            camera.Parameters[PLCamera.HeightMax].TrySetValue(3288);
            Thread.Sleep(50);
        }
        public void SetExposure(int _exposure)
        {
            exposure = _exposure;
            bool bRet = camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(exposure, IntegerValueCorrection.Nearest);
            return;
        }
        public void SetGainValue(int _gain)
        {
            gain = _gain;
            camera.Parameters[PLCamera.GainRaw].TrySetValue(gain, IntegerValueCorrection.Nearest);
        }

        public Bitmap OneShot_()
        {
            OneShot();
            
            int i = 0;
            try
            {
                while (!isgrabed)
                {
                    i++;
                    Thread.Sleep(10);
                    if (i > 1000)
                    {
                        MessageBox.Show("capture failed");

                        return null;
                    }
                }
                return Image_BASLER;
            }
            catch (Exception ex)
            {
                Log.AddLog("OneShot_() in BaslerCamera exception " + ex.ToString());
                Log.AddPmLog("OneShot_() in BaslerCamera exception " + ex.ToString());
                MessageBox.Show(ex.ToString());

            }
            return null;
        }
        
        public Bitmap OneShot_(int exposure)
        {
            SetExposure(exposure);
            // Thread.Sleep(500);
                
                
                
            return OneShot_();
        }
    }
}
