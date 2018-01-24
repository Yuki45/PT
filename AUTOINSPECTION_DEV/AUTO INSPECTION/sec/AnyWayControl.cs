using AutoInspection.Utils;

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoInspection_GUMI;

namespace AutoInspection
{
    public class AnyWayControl
    {
        public List<byte[]> AnywayConvertUsb = new List<byte[]>();
        public List<byte[]> AnywayConvertUart = new List<byte[]>();
        public List<byte[]> AnywayConvert2Type = new List<byte[]>();
        public List<byte[]> AnywayConvertCType = new List<byte[]>();

        public SerialPort serialPort;
        private string sReturn;


        public AnyWayControl()
        {
            InitUmts();

        }

        void InitUmts()
        {
            // (USB 모드 셋팅 -비젼)
            AnywayConvertUsb.Add(String2ByteArray("3A5331534F4C32DE")); // VISION
            AnywayConvertUsb.Add(String2ByteArray("3A533152314F4EDE")); // SDS OFF
            AnywayConvertUsb.Add(String2ByteArray("3A533152314F46D6")); // SDS ON
            AnywayConvertUsb.Add(String2ByteArray("3A533150414F46E4")); //      VBATT ON
            AnywayConvertUsb.Add(String2ByteArray("3A533152314F4EDE")); //      SDS OFF
            AnywayConvertUsb.Add(String2ByteArray("3A533152374F46DC")); //      VBUS ON
            AnywayConvertUsb.Add(String2ByteArray("3A533152384F46DD")); //      D +/ D ON
            AnywayConvertUsb.Add(String2ByteArray("3A53314D553255E7")); //      USB BOOT OFF
            AnywayConvertUsb.Add(String2ByteArray("3A5331534F4C3FEB")); //      SOL CHECK
            AnywayConvertUsb.Add(String2ByteArray("3A53314D55324EE0")); //      NOT USE

            // (UART MODE 셋팅)
            AnywayConvertUart.Add(String2ByteArray("3A5331534F4C32DE")); // VISION
            AnywayConvertUart.Add(String2ByteArray("3A533152314F4EDE")); // SDS OFF
            AnywayConvertUart.Add(String2ByteArray("3A533152314F46D6")); // SDS ON
            AnywayConvertUart.Add(String2ByteArray("3A533150414F46E4")); // VBATT ON
            AnywayConvertUart.Add(String2ByteArray("3A533152384F4EE5")); // D +/ D OFF
            AnywayConvertUart.Add(String2ByteArray("3A533152374F4EE4")); // VBUS OFF
            AnywayConvertUart.Add(String2ByteArray("3A533152314F46D6")); // SDS ON
            AnywayConvertUart.Add(String2ByteArray("3A53314D553253E5")); // UART BOOT OFF
            AnywayConvertUart.Add(String2ByteArray("3A5331534F4C3FEB")); // SOL CHECK
            // (UART CONVERT 2.0 Type)
            AnywayConvert2Type.Add(String2ByteArray("3A5331534F4C32DE"));
            AnywayConvert2Type.Add(String2ByteArray("3A533156425642EE"));
            AnywayConvert2Type.Add(String2ByteArray("3A533143434F46D9"));
            AnywayConvert2Type.Add(String2ByteArray("3A494453454C30DB"));
            AnywayConvert2Type.Add(String2ByteArray("3A533142544F46E9"));
            AnywayConvert2Type.Add(String2ByteArray("3A533150414F46E4"));
            AnywayConvert2Type.Add(String2ByteArray("3A533152374F4EE4"));
            AnywayConvert2Type.Add(String2ByteArray("3A533152384F4EE5"));
            AnywayConvert2Type.Add(String2ByteArray("3A533152314F46D6"));
            AnywayConvert2Type.Add(String2ByteArray("3A53314346564CE9"));
            AnywayConvert2Type.Add(String2ByteArray("3A53314D553253E5"));
            AnywayConvert2Type.Add(String2ByteArray("3A53314A474F4EEC"));
            AnywayConvert2Type.Add(String2ByteArray("3A53314346564CE9"));
            //(UART CONVERT C)
            /*
            AnywayConvertCType.Add(String2ByteArray("3A5331534F4C32DE"));
            AnywayConvertCType.Add(String2ByteArray("3A533156425642EE"));
            AnywayConvertCType.Add(String2ByteArray("3A5044525345540C"));
            AnywayConvertCType.Add(String2ByteArray("3A494453454C31DC"));
            AnywayConvertCType.Add(String2ByteArray("3A533142544F46E9"));
            AnywayConvertCType.Add(String2ByteArray("3A533150414F46E4"));
            AnywayConvertCType.Add(String2ByteArray("3A533152374F4EE4"));
            AnywayConvertCType.Add(String2ByteArray("3A533152384F4EE5"));
            AnywayConvertCType.Add(String2ByteArray("3A533152314F46D6"));
            AnywayConvertCType.Add(String2ByteArray("3A53314346564CE9"));
            AnywayConvertCType.Add(String2ByteArray("3A533143434F4EE1"));
            AnywayConvertCType.Add(String2ByteArray("3A53314D553246D8"));
            AnywayConvertCType.Add(String2ByteArray("3A53314A474F4EEC"));
            AnywayConvertCType.Add(String2ByteArray("3A53314346564CE9"));
            */
            AnywayConvertCType.Add(String2ByteArray("3A533143434F46D9"));
            AnywayConvertCType.Add(String2ByteArray("3A5044525345540C"));
            AnywayConvertCType.Add(String2ByteArray("3A5331534F4C32DE"));
            AnywayConvertCType.Add(String2ByteArray("3A533156425642EE"));
            AnywayConvertCType.Add(String2ByteArray("3A5044525345540C"));
            AnywayConvertCType.Add(String2ByteArray("3A494453454C31DC"));
            AnywayConvertCType.Add(String2ByteArray("3A533142544F46E9"));
            AnywayConvertCType.Add(String2ByteArray("3A533150414F46E4"));
            AnywayConvertCType.Add(String2ByteArray("3A533152374F4EE4"));
            AnywayConvertCType.Add(String2ByteArray("3A533152384F4EE5"));
            AnywayConvertCType.Add(String2ByteArray("3A533152314F46D6"));
            AnywayConvertCType.Add(String2ByteArray("3A53314346564CE9"));
            AnywayConvertCType.Add(String2ByteArray("3A533143434F4EE1"));
            AnywayConvertCType.Add(String2ByteArray("3A53314D553246D8"));
            AnywayConvertCType.Add(String2ByteArray("3A53314A474F4EEC"));
        }


        public void Close()
        {
            if(serialPort!=null)
                serialPort.Close();
        }

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
 //                   serialPort.DataReceived += SerialPort_DataReceived;
                }
            }
            catch( Exception e )
            {
                // Logger.Write("Open() in AnyWayControl exception " + e.ToString());
                Log.AddLog(e.ToString());
                MessageBox.Show(e.Message);
                return false;
            }

            return serialPort.IsOpen;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string _tmp = serialPort.ReadExisting();

                sReturn += _tmp.Trim();
            }
            catch (Exception ex)
            {
                // Logger.Write("SerialPort_DataReceived() in AnyWayControl exception " + e.ToString());
                Log.AddLog(e.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        public bool Write(string _sCmd, string _sExpectedReturn = null, int _nTimeOutMs = 0 )
        {
            if (!serialPort.IsOpen)
                return false;

            bool bRet = false;
            int _nCount = 0;

            if (serialPort.IsOpen)
            {
                sReturn = string.Empty;
                serialPort.Write(_sCmd );

                if (_nTimeOutMs <= 0 || _sExpectedReturn == null )
                {
                    bRet = true;
                }
                else
                {
                    while (_nCount < _nTimeOutMs)
                    {
                        if (sReturn.Contains(_sExpectedReturn))
                        {
                            bRet = true;
                            break;
                        }
                        _nCount += 10;
                        Thread.Sleep(10);
                    }
                }
            }
            return bRet;
        }
        
        public bool WriteNV_P(string _sNv)
        {
            bool result = false;
            for (int i = 0; i < 3; i++)
            {
                Write(string.Format("AT+SETTESTNV={0}\r", _sNv));   // Serial_Phone.Write("AT+SETTESTNV=" + nv + ",P\r");
                Thread.Sleep(200);

                result = Write(string.Format("AT+GETTESTNV={0}\r", _sNv), "+GETTESTNV:", 30);
                if (result)
                    break;
            }
            return result;
        }

        public bool Function15()
        {
            bool bRet = false;
            for (int i = 0; i < 3; i++)
            {
                bRet = Write("AT+FUNCTEST=0,2\r", "+FUNCTEST:0,OK", 100);
                if (bRet)
                    break;
            }
            return bRet;
        }

        public bool HeadInfo()
        {
            bool bRet = false;
            for (int i = 0; i < 3; i++)
            {
                bRet = Write("AT+HEADINFO=1,1\r", "+HEADINFO:", 100);
                if (bRet)
                    break;
            }
            return bRet;
        }
        public HeadInfoData NEW_HEAD_INFO()
        {
            if (serialPort.IsOpen)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        serialPort.Write("AT+HEADINFO=1,0\r\n");
                        Thread.Sleep(200);
                        if (serialPort.BytesToRead > 0)
                        {
                            string res = serialPort.ReadExisting();
                            if (res.StartsWith("AT+HEADINFO=1,0\r\n") && res.EndsWith("\r\n\r\nOK\r\n"))
                            {
                                res = res.Replace("AT+HEADINFO=1,0\r\n\r\n+HEADINFO:1,", "");
                                res = res.Replace("\r\n\r\nOK\r\n", "");
                                HeadInfoData hid = new HeadInfoData();
                                hid.ProcessData(res);
                                if (hid.DataOK)
                                {
                                    //serialPort.DataReceived += SerialPort_DataReceived;  // hunsibi
                                    return hid;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // Logger.Write("NEW_HEAD_INFO() in AnyWayControl exception " + e.ToString());
                        Log.AddLog(e.ToString());
                        Log.AddPmLog(e.ToString());
                    }
                }
            }
//        serialPort.DataReceived += SerialPort_DataReceived;
            return null;
        }
        public string GetUN()
        {
            if (serialPort.IsOpen)
            {
                serialPort.DataReceived -= SerialPort_DataReceived;

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Write("AT+HEADINFO=1,0\r\n");
                        Thread.Sleep(200);
                        if (serialPort.BytesToRead > 0)
                        {
                            string res = serialPort.ReadExisting();
                            if (res.StartsWith("AT+HEADINFO=1,0\r\n") && res.EndsWith("\r\n\r\nOK\r\n"))
                            {
                                res = res.Replace("AT+HEADINFO=1,0\r\n\r\n+HEADINFO:1,", "");
                                res = res.Replace("\r\n\r\nOK\r\n", "");
                                HeadInfoData hid = new HeadInfoData();
                                hid.ProcessData(res);
                                if (hid.DataOK)
                                {
                                    return hid.UniqueNo;
                                }
                            }
                        }
                    }
                    catch (Exception )
                    {
                        return "None";
                    }
                }
                serialPort.DataReceived += SerialPort_DataReceived;
            }
            return "None";
        }

        // DispTest("0,1");
        public bool DispTest(string _param)
        {
            bool bRet = false;
            for (int i = 0; i < 3; i++)
            {
                bRet = Write(string.Format("AT+DISPTEST={0}\r", _param), "+DISPTEST:", 100);
                if (bRet)
                    break;
            }
            return bRet;
        }
        

        public string GetUniqueNumber()
        {
            string _sUn = "None";
            bool bRet = Write(string.Format("AT+DISPTEST=0,1\r"), "Unique Number", 300);

            if (bRet)
            {
                string _t = sReturn.Substring(sReturn.LastIndexOf("Unique Number") + 16);
                _sUn = _t.Substring(0, _t.IndexOf('\r'));
            }
            return _sUn;
        }

        public byte[] String2ByteArray(string sHex)
        {
            byte[] bytes = new byte[sHex.Length / 2];
            for (int i = 0; i < sHex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(sHex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public void ConvertAnyWayUart()
        {
            ConvertAnyway(AnywayConvertUart);
        }
        public void ConvertAnyWayUsb()
        {
            ConvertAnyway(AnywayConvertUart);
        }
        public void ConvertAnywayUSB2Type()
        {
            ConvertAnyway(AnywayConvert2Type);
        }
        public void ConvertAnywayUSBCType()
        {
            ConvertAnyway(AnywayConvertCType);
        }


        // USB C TYPE intialization 실시한다.
        public void InitUsbCType()
        {
            if (!serialPort.IsOpen)
                return;

            // 이하 USB CTYPE init sequence.
            byte[] S1SOL2   = String2ByteArray("3A5331534F4C32DE");
            byte[] S1VBBT   = String2ByteArray("3A533156424254EC");
            byte[] IDSEL1     = String2ByteArray("3A494453454C31DC");
            byte[] S1PAOF   = String2ByteArray("3A533150414F46E4");
            byte[] S1R8ON   = String2ByteArray("3A533152384F4EE5");
            byte[] S1R7ON   = String2ByteArray("3A533152374F4EE4");
            byte[] S1R1ON   = String2ByteArray("3A533152314F4EDE");
            byte[] S1PAON   = String2ByteArray("3A533150414F4EEC");
            // S1R1ON  중복 Sequence.
            byte[] S1MU2S   = String2ByteArray("3A53314D553253E5");
            byte[] S1CCOF   = String2ByteArray("3A533143434F46D9");
            byte[] PDRSET   = String2ByteArray("3A5044525345540C");
            
            // serial port write
            serialPort.Write(S1SOL2, 0, S1SOL2.Length);
            Thread.Sleep(100);
            serialPort.Write(S1VBBT, 0, S1VBBT.Length);
            Thread.Sleep(100);
            serialPort.Write(IDSEL1, 0, IDSEL1.Length);
            Thread.Sleep(100);
            serialPort.Write(S1PAOF, 0, S1PAOF.Length);
            Thread.Sleep(100);
            serialPort.Write(S1R8ON, 0, S1R8ON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1R7ON, 0, S1R7ON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1R1ON, 0, S1R1ON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1PAON, 0, S1PAON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1R1ON, 0, S1R1ON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1MU2S, 0, S1MU2S.Length);
            Thread.Sleep(100);
            serialPort.Write(S1CCOF, 0, S1CCOF.Length);
            Thread.Sleep(100);
            serialPort.Write(PDRSET, 0, PDRSET.Length);
            Thread.Sleep(500);
        }

        //  USB C type pack 삽입 후 at command 보내기 사전작업
        public void PrepareStartAtCommand()
        {
            if(!serialPort.IsOpen)
                return;

            byte[] S1SOL2       = String2ByteArray("3A5331534F4C32DE");
            byte[] IDSEL1         = String2ByteArray("3A494453454C31DC");
            byte[] S1MU2F       = String2ByteArray("3A53314D553246D8");
            byte[] S1CCON       = String2ByteArray("3A533143434F4EE1");  
            byte[] S1R1OF       = String2ByteArray("3A533152314F46D6");

            Thread.Sleep(100);
            serialPort.Write(S1SOL2, 0, S1SOL2.Length);
            Thread.Sleep(100);
            serialPort.Write(IDSEL1, 0, IDSEL1.Length);
            Thread.Sleep(100);
            serialPort.Write(S1MU2F, 0, S1MU2F.Length);
            Thread.Sleep(500);
            serialPort.Write(S1CCON, 0, S1CCON.Length);
            Thread.Sleep(200);
            serialPort.Write(S1R1OF, 0, S1R1OF.Length);
            Thread.Sleep(600);
        }

        //  USB C type pack 제거 전 AnyWay Jig 셋업
        public void PrepareFinishCommand()
        {

            byte[] S1PAON       = String2ByteArray("3A533150414F4EEC");
            byte[] S1R1ON       = String2ByteArray("3A533152314F4EDE");
            byte[] S1CCOF       = String2ByteArray("3A533143434F46E1");
            byte[] PDRSET       = String2ByteArray("3A5044525345540C");

            serialPort.Write(S1PAON, 0, S1PAON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1R1ON, 0, S1R1ON.Length);
            Thread.Sleep(100);
            serialPort.Write(S1CCOF, 0, S1CCOF.Length);
            Thread.Sleep(500);
            serialPort.Write(PDRSET, 0, PDRSET.Length);
        }

        public void ConvertAnyway(List<byte[]> Umts = null)
        {
            if (Umts == null || !serialPort.IsOpen )
                return;

            try
            {
                foreach (var bytes in Umts)
                {
                    serialPort.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(20);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void ResetAnyway(List<byte[]> Umts = null)
        {
            //try
            //{
            //    if (Umts == null)
            //        Umts = AnywayReset;

            //    foreach (var bytes in Umts)
            //    {
            //        serialPort.Write(bytes, 0, bytes.Length);
            //        Thread.Sleep(20);
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}
        }

        //{
        //    string str = (string)temp;
        //    if (str.Contains("Unique Number"))
        //    {
        //        try
        //        {
        //            string t = str.Substring(str.LastIndexOf("Unique Number") + 16);
        //            UN = t.Substring(0, t.IndexOf('\r'));
        //        }
        //        catch (Exception)
        //        { UN = "None"; }
        //    }
        //    return sUn;
        //}
        //string UN = "";
        //public List<string> DATA_Way = new List<string>();
        //private void UpdateInfomation(object temp)
        //{
        //    string str = (string)temp;
        //    if (str.Contains("Unique Number"))
        //    {
        //        try
        //        {
        //            string t = str.Substring(str.LastIndexOf("Unique Number") + 16);
        //            UN = t.Substring(0, t.IndexOf('\r'));
        //        }
        //        catch (Exception)
        //        { UN = "None"; }
        //    }
        //}

    }
}


//private bool waitCMD(string reply, List<string> lst, int time)
//{
//    string temp = "";
//    int cout = 0;
//    while (!temp.Contains(reply))
//    {
//        cout++;
//        if (lst.Count > 0)
//        {
//            temp = lst[0].Trim();
//            lst.RemoveAt(0);
//        }

//        if (cout == time)
//        {
//            return false;
//        }
//        Thread.Sleep(10);
//    }
//    return true;
//}


//
//public static byte[] StringToByteArray(String hex)
//{
//    int NumberChars = hex.Length;
//    byte[] bytes = new byte[NumberChars / 2];
//    for (int i = 0; i < NumberChars; i += 2)
//        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
//    return bytes;
//}

//public void InitUmts()
//{
//    // AnywayConvert.Clear();
//    //AnywayConvert.Add("3A5331534F4C32DE");
//    //AnywayConvert.Add("3A533152314F4EDE");
//    //AnywayConvert.Add("3A533152314F46D6");
//    //AnywayConvert.Add("3A533150414F46E4");
//    //AnywayConvert.Add("3A533152384F4EE5");
//    //AnywayConvert.Add("3A533152374F4EE4");
//    //AnywayConvert.Add("3A533152314F46D6");
//    //AnywayConvert.Add("3A53314D553253E5");
//    //AnywayConvert.Add("3A5331534F4C3FEB");
//    //AnywayConvert.Add("3A533142544F46E9");
//    //AnywayConvert.Add("3A533150414F4EEC");
//}