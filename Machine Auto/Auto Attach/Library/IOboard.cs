using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Attach.Library
{
    public class IOboard
    {
        #region Variabel...
        int[] lightValues = new int[8];
        public Dictionary<string, int> lightChannel = new Dictionary<string, int>();
        public bool IOActive = false;
        public bool Port1 = false;
        public bool Port2 = false;
        public bool Port3 = false;
        public bool Port4 = false;
        public bool Port5 = false;
        public bool Port6 = false;
        public bool Port7 = false;
        public bool Port8 = false;
        public bool Port9 = false;
        public bool Port10 = false;
        public string sReturn = "";
        public string com = "";
        private SerialPort serialPort;
        #endregion
        public bool Open(string _portName, int _baudRate)
        {
            try
            {
                if (serialPort == null)
                    serialPort = new SerialPort(_portName, _baudRate);
                else
                    serialPort.Close();

                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    IOActive = true;
                    serialPort.DataReceived += SerialPort_DataReceived;

                    Log.AddLog("Open COM:" + _portName + " " + _baudRate);
                    Log.AddPmLog("Open COM:" + _portName + " " + _baudRate);
                }
                else IOActive = false;

            }
            catch (Exception e)
            {
                Log.AddLog( e.ToString());
                Log.AddPmLog(e.ToString());
                IOActive = false;
                MessageBox.Show(e.Message);
                return false;
            }

            return serialPort.IsOpen;
        }

        public bool Output(string msg)
        {
            bool result=false;
            try
            {
                serialPort.Write(msg + "\r\n");
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public void Close()
        {
            if (serialPort != null)
                serialPort.Close();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string _tmp = serialPort.ReadExisting();
                //SP_Data(0, serialPort.ReadLine());
                //sReturn += _tmp.Trim();
                if (_tmp.Length >= 16)
                {

                    ReadJIG(_tmp);
                }
            }
            catch (Exception ex)
            {
                // Logger.Write("SerialPort_DataReceived() in AnyWayControl exception " + e.ToString());
                Log.AddLog(e.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReadJIG(string StatusJIG)
        {
            //0111213141516171 !6 bit coy
            string stemp = "";
            if (StatusJIG.Length >= 16)
            {

                for (int i = 0; i < StatusJIG.Length; i++)
                {
                    //  Console.WriteLine(s[i]);
                    if (i == 1)
                    {
                        try
                        {
                            if (StatusJIG.Substring(0, 2) == "01")
                            {
                                Port1 = false;
                            }
                            else if (StatusJIG.Substring(0, 2) == "00")
                            {
                                Port1 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent( Jig1, appPath + "//App.log");
                        }
                    }

                    if (i == 3)
                    {
                        try
                        {
                            if (StatusJIG.Substring(2, 2) == "11")
                            {
                                Port2 = false;
                            }
                            else if (StatusJIG.Substring(2, 2) == "10")
                            {
                                Port2 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(Jig2 , appPath + "//App.log");
                        }
                    }

                    if (i == 5)
                    {

                        try
                        {
                            if (StatusJIG.Substring(4, 2) == "21")
                            {
                                Port3 = false;
                            }
                            else if (StatusJIG.Substring(4, 2) == "20")
                            {
                                Port3 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 7)
                    {
                        try
                        {
                            if (StatusJIG.Substring(6, 2) == "31")
                            {
                                Port4 = false;
                            }
                            else if (StatusJIG.Substring(6, 2) == "30")
                            {
                                Port4 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 9)
                    {
                        try
                        {
                            if (StatusJIG.Substring(8, 2) == "41")
                            {
                                Port5 = false;
                            }
                            else if (StatusJIG.Substring(8, 2) == "40")
                            {
                                Port5 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 11)
                    {
                        try
                        {
                            if (StatusJIG.Substring(10, 2) == "51")
                            {
                                Port6 = false;
                            }
                            else if (StatusJIG.Substring(10, 2) == "50")
                            {
                                Port6 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 13)
                    {
                        try
                        {
                            if (StatusJIG.Substring(12, 2) == "61")
                            {
                                Port7 = false;
                            }
                            else if (StatusJIG.Substring(12, 2) == "60")
                            {
                                Port7 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 15)
                    {
                        try
                        {
                            if (StatusJIG.Substring(14, 2) == "71")
                            {
                                Port8 = false;
                            }
                            else if (StatusJIG.Substring(14, 2) == "70")
                            {
                                Port8 = true;
                            }
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }
                    
                }
            }
            else
            {
                //logfiles.WriteLogAgent(StatusJIG.ToString(), appPath + "//AppPort.log");
            }

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SP_Data(0, serialPort.ReadLine());
        }
        int readCnt = 0;
        byte recvByte = 0;
        byte[] recvBuf = new byte[1024];

        private void SP_Data(int nChannel, string sData)
        {
            if (serialPort.BytesToRead >= 0)
            {
                readCnt = serialPort.Read(recvBuf, 0, 1024);
                recvByte = recvBuf[readCnt - 1];
                //listBox1.Items.Add(readCnt.ToString());
            }
        }
        /*
         public bool ChangeLightValue2(int iCh, int iLightVal)
        {
            
            if (!serialPort.IsOpen)
                return false;

            byte[] txBytes = new byte[7];
            txBytes[0] = (byte)'L';         //  0x4C;
            txBytes[1] = (byte)(iCh+0x30);  //  0x30;

            txBytes[2] = (byte)((iLightVal/100) + 0x30);
            txBytes[3] = (byte)(((iLightVal%100)/10)+ 0x30);
            txBytes[4] = (byte)((iLightVal%10) + 0x30);
            txBytes[5] = 0x0D;
            txBytes[6] = 0x0A;

            try
            {
                serialPort.Write(txBytes, 0, txBytes.Length);
            }
            catch( Exception e )
            {
                Log.AddLog(e.ToString());
                Log.AddPmLog(e.ToString());


                MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }
         * */
    }
}
