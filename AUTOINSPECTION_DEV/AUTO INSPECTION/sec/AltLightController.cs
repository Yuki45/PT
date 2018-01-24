using System;
using System.IO.Ports;
using System.Windows.Forms;
using AutoInspection.Utils;
using System.Collections.Generic;
using AutoInspection_GUMI;

namespace AutoInspection
{
    public class AltLightController
    {
        int[] lightValues = new int[8];
        public Dictionary<string, int> lightChannel = new Dictionary<string, int>();

        public string sFrontLight1 = "FrontLight1";
        public string sBarLight = "BarLight";
        public string sFrontLight2 = "FrontLight2";
        public string sRearLight1 = "RearLight1";
        public string sRearLight2 = "RearLight2";

        public void InitLight()
        {
            lightChannel.Add(sFrontLight1, 0);
            lightChannel.Add(sBarLight, 1);
            lightChannel.Add(sFrontLight2, 2);
            lightChannel.Add(sRearLight1, 3);
            lightChannel.Add(sRearLight2, 4);
        }

        public void ClearLightValue()
        {
            for (int i = 0; i < 8;i++)
                lightValues[i] = 0; 

        }
        public void SetLightValue(int channel, int value) 
        {
            lightValues[channel] = value; 
        }

        public void TurnOnLight()
        {
            ControlLight8chAll(lightValues[0],
                lightValues[1],
                lightValues[2],
                lightValues[3],
                lightValues[4],
                lightValues[5],
                lightValues[6],
                lightValues[7]);
        }

        public void test()
        {
            // SetLightValue(lightChannel[sLineScan1], scenarioManger.testSpec.SpecLight.DustLightValue);
        }


        private SerialPort serialPort;
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
                    // serialPort.DataReceived += SerialPort_DataReceived;
                }
            }
            catch (Exception e)
            {
                Log.AddLog( e.ToString());
                Log.AddPmLog(e.ToString());


                MessageBox.Show(e.Message);
                return false;
            }

            return serialPort.IsOpen;
        }

        public void Close()
        {
            if (serialPort != null)
                serialPort.Close();
        }

        public void ControlLight4chAll(int chnVal1, int chnVal2, int chnVal3, int chnVal4)
        {
            byte[] tx_arr = new byte[10];

            int check_sum = 0;
            string str = string.Empty;

            if (serialPort.IsOpen)
            {
                tx_arr[0] = 0xEF;    // Header1
                tx_arr[1] = 0xEF;    // Header2a
                tx_arr[2] = 0x00;

                // CHANNEL 1,2,3,4
                tx_arr[3] = (byte)chnVal1;
                tx_arr[4] = (byte)chnVal2;
                tx_arr[5] = (byte)chnVal3;
                tx_arr[6] = (byte)chnVal4;


                check_sum = tx_arr[2] ^ tx_arr[3] ^ tx_arr[4] ^ tx_arr[5] ^ (tx_arr[6] + 0x01);
                tx_arr[7] = (byte)check_sum;
                tx_arr[8] = 0xEE;   //END1
                tx_arr[9] = 0xEE;   //END2

                serialPort.Write(tx_arr, 0, 10);  // Send Light value
            }
            else
            {
                MessageBox.Show("Serial Port Not open");
            }
        }

        public void ControlLight8chAll(int chnVal1, int chnVal2, int chnVal3, int chnVal4, int chnVal5, int chnVal6, int chnVal7, int chnVal8)
        {
            byte[] tx_arr = new byte[14];

            int check_sum = 0;
            string str = string.Empty;

            if (serialPort.IsOpen)
            {
                tx_arr[0] = 0xEF;    // Header1
                tx_arr[1] = 0xEF;    // Header2a
                tx_arr[2] = 0x00;

                // CHANNEL 1,2,3,4
                tx_arr[3] = (byte)chnVal1;
                tx_arr[4] = (byte)chnVal2;
                tx_arr[5] = (byte)chnVal3;
                tx_arr[6] = (byte)chnVal4;
                tx_arr[7] = (byte)chnVal5;
                tx_arr[8] = (byte)chnVal6;
                tx_arr[9] = (byte)chnVal7;
                tx_arr[10] = (byte)chnVal8;


                check_sum = tx_arr[2] ^ tx_arr[3] ^ tx_arr[4] ^ tx_arr[5] ^ tx_arr[6] ^ tx_arr[7] ^ tx_arr[8] ^ tx_arr[9] ^ (tx_arr[10] + 0x01);
                tx_arr[11] = (byte)check_sum;
                tx_arr[12] = 0xEE;   //END1
                tx_arr[13] = 0xEE;   //END2

                serialPort.Write(tx_arr, 0, 14);  // Send Light value
            }
            else
            {
                MessageBox.Show("Serial Port Not open");
            }
        }


        // iLigitVal = 0 ~ 255
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

        //public void Action(TestType testType)
        //{
        //    switch (testType)
        //    {
        //        case TestType.TEST_LCD_AREA:
        //            ChangeLightValue2(0, 100);
        //            ChangeLightValue2(1, 100);
        //            break;
        //    }
        //}

        //private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    SP_Data(0, serialPort.ReadLine());
        //}
        //int readCnt = 0;
        //byte recvByte = 0;
        //byte[] recvBuf = new byte[1024];

        //private void SP_Data(int nChannel, string sData)
        //{
        //    //if (serialPort1.BytesToRead >= 0)
        //    //{
        //    //    readCnt = serialPort1.Read(recvBuf, 0, 1024);
        //    //    recvByte = recvBuf[readCnt - 1];
        //    //    listBox1.Items.Add(readCnt.ToString());
        //    //}
        //}
        
        //void CLightControl::ChangeLightValue8Ch(int iC0, int iC1, int iC2, int iC3, int iC4, int iC5, int iC6, int iC7)
        //{
        //    char msg[14] = { 0x00, };
        //    msg[0] = (char)0xEF;
        //    msg[1] = (char)0xEF;
        //    msg[2] = (char)0x00;
        //    msg[3] = (char)iC0;
        //    msg[4] = (char)iC1;
        //    msg[5] = (char)iC2;
        //    msg[6] = (char)iC3;
        //    msg[7] = (char)iC4;
        //    msg[8] = (char)iC5;
        //    msg[9] = (char)iC6;
        //    msg[10] = (char)iC7;
        //    msg[11] = (char)(msg[2] ^ msg[3] ^ msg[4] ^ msg[5] ^ msg[6] ^ msg[7] ^ msg[8] ^ msg[9] ^ (msg[10] + 0x01));
        //    msg[12] = (char)0xEE;
        //    msg[13] = (char)0xEE;

        //    sio_write(m_iPortNo, msg, 14);

        //    Sleep(80);
        //}

        //void CLightControl::ChangeLightValue8Ch(int chData[])
        //{
        //    char msg[14] = { 0x00, };

        //    msg[0] = (char)0xEF;
        //    msg[1] = (char)0xEF;
        //    msg[2] = (char)0x00;
        //    msg[3] = (char)chData[0];
        //    msg[4] = (char)chData[1];
        //    msg[5] = (char)chData[2];
        //    msg[6] = (char)chData[3];
        //    msg[7] = (char)chData[4];
        //    msg[8] = (char)chData[5];
        //    msg[9] = (char)chData[6];
        //    msg[10] = (char)chData[7];
        //    msg[11] = (char)(msg[2] ^ msg[3] ^ msg[4] ^ msg[5] ^ msg[6] ^ msg[7] ^ msg[8] ^ msg[9] ^ (msg[10] + 0x01));
        //    msg[12] = (char)0xEE;
        //    msg[13] = (char)0xEE;

        //    sio_write(m_iPortNo, msg, 14);

        //    Sleep(80);
        //}

        //void CLightControl::ChangeLightValue4Ch(int iC0, int iC1, int iC2, int iC3)
        //{
        //    char msg[10] = { 0x00, };
        //    msg[0] = (char)0xEF;
        //    msg[1] = (char)0xEF;
        //    msg[2] = (char)0x00;
        //    msg[3] = (char)iC0;
        //    msg[4] = (char)iC1;
        //    msg[5] = (char)iC2;
        //    msg[6] = (char)iC3;
        //    msg[7] = (char)(msg[2] ^ msg[3] ^ msg[4] ^ msg[5] ^ (msg[6] + 0x01));
        //    msg[8] = (char)0xEE;
        //    msg[9] = (char)0xEE;

        //    sio_write(m_iPortNo, msg, 10);

        //    Sleep(80);
        //}

        //void CLightControl::ChangeLightValue2(int iCh, int iLightVal)
        //{
        //    CString strTemp, strTemp1, strTemp2;

        //    CString sMsg;
        //    sMsg.Format(_T("L%d%03d\r\n"), iCh, iLightVal);

        //    if (m_iOldVal[iCh] != iLightVal)
        //    {
        //        SendMessageW(sMsg, 0);
        //        Sleep(100);
        //    }

        //    m_iOldVal[iCh] = iLightVal;
        //}

        //void CLightControl::LightOff8Ch(void)
        //{
        //    ChangeLightValue8Ch(0, 0, 0, 0, 0, 0, 0, 0);
        //}

        //void CLightControl::LightOff4Ch(void)
        //{
        //    ChangeLightValue4Ch(0, 0, 0, 0);
        //}



    }
}
