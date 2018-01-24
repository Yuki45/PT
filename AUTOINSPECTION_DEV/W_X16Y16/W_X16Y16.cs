using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Drawing;
using System.Text;

namespace DIO
{
    public partial class W_X16Y16_ : UserControl
    {
        SerialPort SP = new SerialPort();

        #region ** 1.ADDRESS
        public byte[] X16 = { 0, 0, 0, 0, 0, 0, 0, 0, // 5 / 6
                               0, 0, 0, 0, 0, 0, 0, 0  // 7 / 8
                             };

        public byte[] Y16 = { 0, 0, 0, 0, 0, 0, 0, 0, // 1 / 2
                               0, 0, 0, 0 ,0, 0, 0, 0  // 3 / 4
                             };

        public byte X_01
        {
            get
            {
                return X16[0];
            }
            set { }
        }
        public byte X_02
        {
            get
            {
                return X16[1];
            }
            set { }
        }
        public byte X_03
        {
            get
            {
                return X16[2];
            }
            set { }
        }
        public byte X_04
        {
            get
            {
                return X16[3];
            }
            set { }
        }
        public byte X_05
        {
            get
            {
                return X16[4];
            }
            set { }
        }
        public byte X_06
        {
            get
            {
                return X16[5];
            }
            set { }
        }
        public byte X_07
        {
            get
            {
                return X16[6];
            }
            set { }
        }
        public byte X_08
        {
            get
            {
                return X16[7];
            }
            set { }
        }
        public byte X_09
        {
            get
            {
                return X16[8];
            }
            set { }
        }
        public byte X_10
        {
            get
            {
                return X16[9];
            }
            set { }
        }
        public byte X_11
        {
            get
            {
                return X16[10];
            }
            set { }
        }
        public byte X_12
        {
            get
            {
                return X16[11];
            }
            set { }
        }
        public byte X_13
        {
            get
            {
                return X16[12];
            }
            set { }
        }
        public byte X_14
        {
            get
            {
                return X16[13];
            }
            set { }
        }
        public byte X_15
        {
            get
            {
                return X16[14];
            }
            set { }
        }
        public byte X_16
        {
            get
            {
                return X16[15];
            }
            set { }
        }

        public byte Y_01
        {
            get
            {
                return Y16[0];
            }
            set
            {
                Y16[0] = value;
            }
        }
        public byte Y_02
        {
            get
            {
                return Y16[1];
            }
            set
            {
                Y16[1] = value;
            }
        }
        public byte Y_03
        {
            get
            {
                return Y16[2];
            }
            set
            {
                Y16[2] = value;
            }
        }
        public byte Y_04
        {
            get
            {
                return Y16[3];
            }
            set
            {
                Y16[3] = value;
            }
        }
        public byte Y_05
        {
            get
            {
                return Y16[4];
            }
            set
            {
                Y16[4] = value;
            }
        }
        public byte Y_06
        {
            get
            {
                return Y16[5];
            }
            set
            {
                Y16[5] = value;
            }
        }
        public byte Y_07
        {
            get
            {
                return Y16[6];
            }
            set
            {
                Y16[6] = value;
            }
        }
        public byte Y_08
        {
            get
            {
                return Y16[7];
            }
            set
            {
                Y16[7] = value;
            }
        }
        public byte Y_09
        {
            get
            {
                return Y16[8];
            }
            set
            {
                Y16[8] = value;
            }
        }
        public byte Y_10
        {
            get
            {
                return Y16[9];
            }
            set
            {
                Y16[9] = value;
            }
        }
        public byte Y_11
        {
            get
            {
                return Y16[10];
            }
            set
            {
                Y16[10] = value;
            }
        }
        public byte Y_12
        {
            get
            {
                return Y16[11];
            }
            set
            {
                Y16[11] = value;
            }
        }
        public byte Y_13
        {
            get
            {
                return Y16[12];
            }
            set
            {
                Y16[12] = value;
            }
        }
        public byte Y_14
        {
            get
            {
                return Y16[13];
            }
            set
            {
                Y16[13] = value;
            }
        }
        public byte Y_15
        {
            get
            {
                return Y16[14];
            }
            set
            {
                Y16[14] = value;
            }
        }
        public byte Y_16
        {
            get
            {
                return Y16[15];
            }
            set
            {
                Y16[15] = value;
            }
        }

        #endregion

        bool UI_Control = false;
        public bool _Conn_OK = false; // 연결여부
        private string portname = "COM1"; // 초기 포트 지정


        public W_X16Y16_()
        {
            InitializeComponent();
            SP.BaudRate = 115200;
        }

        private void W_X16Y16_Load(object sender, EventArgs e)
        {
            SP.BaudRate = 115200;
        }


        #region ** 버튼 클릭 관련

        // 드라이런
        public bool _FreeRun = false;
        private void FR_button_Click(object sender, EventArgs e)
        {
            if (_Conn_OK == true)
            {
                if (_FreeRun == true)
                {
                    if (UI_Control == true)
                    {
                        FR_button.Text = "Dry Stop";
                    }
                    _FreeRun = false;

                }
                else
                {
                    if (UI_Control == true)
                    {
                        FR_button.Text = "Dry Run";
                    }
                    _FreeRun = true;

                }
            }
        }

        // 전체 ON/off
        private void ON_button_Click(object sender, EventArgs e)
        {
            if (_Conn_OK == true)
            {
                if (ON_button.Text.Contains("OFF") == true)
                {
                    for (int i = 0; i <= 15; i++) // 데이타 타입 맞추기 (역순 출력)
                    {
                        Y16[i] = 1;
                    }

                    if (UI_Control == true)
                    {
                        Control.ControlCollection P = this.panel_OUT.Controls;

                        for (int I = 0; I < P.Count; I++)
                        {
                            Panel CU = (Panel)P[I];
                            CU.BackColor = System.Drawing.Color.GreenYellow;
                        }
                        ON_button.Text = "All ON";
                    }
                }
                else
                {
                    for (int i = 0; i <= 15; i++) // 데이타 타입 맞추기 (역순 출력)
                    {
                        Y16[i] = 0;
                    }

                    if (UI_Control == true)
                    {
                        Control.ControlCollection P = this.panel_OUT.Controls;
                        for (int I = 0; I < P.Count; I++)
                        {
                            Panel CU = (Panel)P[I];

                            CU.BackColor = System.Drawing.Color.DarkRed;

                        }
                        ON_button.Text = "All OFF";
                    }
                }
                _Out();
            }
        }

        // LED ON/off
        private void LED(object sender, EventArgs e)
        {
            if (_Conn_OK == true)
            {
                Panel LED = (Panel)sender;
                if (Y16[Convert.ToInt32(LED.Tag)] == 1)
                {
                    Y16[Convert.ToInt32(LED.Tag)] = 0;
                    if (UI_Control == true)
                    {
                        LED.BackColor = System.Drawing.Color.DarkRed;
                    }
                }
                else
                {
                    Y16[Convert.ToInt32(LED.Tag)] = 1;
                    if (UI_Control == true)
                    {
                        LED.BackColor = System.Drawing.Color.GreenYellow;
                    }
                }
                _Out();
            }
        }

        #endregion


        //public void _Connect(bool UI_연동)
        public void _Connect(bool isLinkedUI)
        {
            UI_Control = isLinkedUI;

            if (isLinkedUI == true)
            {
                this.Visible = true;
                COM_LED.BackgroundImage = global::DIO.Properties.Resources.TowerLamp_Gray; 
                timer_Main.Start();
            }
            else
            {
                this.Visible = false;
                TM.Elapsed += new ElapsedEventHandler(TM_Elapsed);
                TM.Interval = _Interval_T;
                TM.Start();
            }

            try // Serial_포트 열기
            {
                if (portname != null)
                {
                    if (SP.IsOpen == false)
                    {
                        SP.PortName = portname;

                        SP.DataReceived += new SerialDataReceivedEventHandler(SP_DataReceived);
                        SP.Open();
                        _Conn_OK = true;

                        if (UI_Control == true)
                        {
                            this.COM_LED.BackgroundImage = global::DIO.Properties.Resources.TowerLamp_Green;
                            Status.Text = portname + " connected";
                            panel_OUT.Enabled = true;
                            ON_button.Enabled = true;
                            FR_button.Enabled = true;
                        }

                    }
                }
            }
            catch
            {
                _Conn_OK = false;
                if (UI_Control == true)
                {
                    this.COM_LED.BackgroundImage = global::DIO.Properties.Resources.TowerLamp_Red;
                    Status.Text = portname + " isn't connected";
                    panel_OUT.Enabled = false;
                    ON_button.Enabled = false;
                    FR_button.Enabled = false;
                }
            }
        }

        public void _Disconnect()
        {
            
            _Conn_OK = false;
            SP.Close();
                

        }

        public void _Out()
        {
            byte[] outData = Y16;
            //  Tx_Alive = true;
            try
            {

                string Data_Sort = "";

                byte[] STX = { 0x02,   
                               0x46,   
                               0x30 };

                byte[] ETX = { 0x03 };

                if (SP.IsOpen)
                {
                    for (int i = 0; i <= 15; i++) // 데이타 타입 맞추기 (역순 출력)
                    {
                        Data_Sort += outData[15 - i].ToString();
                    }

                    if (UI_Control == true)
                    {
                        Control.ControlCollection P = this.panel_OUT.Controls;
                        for (int I = 0; I < P.Count; I++)
                        {
                            Panel CU = (Panel)P[I];

                            if (Y16[Convert.ToUInt32(CU.Tag)].ToString() == "1")
                            {
                                CU.BackColor = System.Drawing.Color.GreenYellow;
                            }
                            else
                            {
                                CU.BackColor = System.Drawing.Color.DarkRed;
                            }
                        }
                    }

                    int int2 = Convert.ToInt32(Data_Sort, 2);
                    string Hex = Convert.ToString(int2, 16);

                    //  SP.Write(ETX, 0, 1);

                    SP.Write(STX, 0, 3);

                    SP.Write(Hex.PadLeft(4, '0').ToUpper());
                    string crc = CheckSum.CRC(Hex.PadLeft(4, '0'));
                    SP.Write(crc.ToUpper());
                    SP.Write(ETX, 0, 1);
                }

            }
            catch { }
            //   Tx_Alive = false;
        }

        public string _PortName
        {
            get
            {
                return portname;
            }
            set
            {
                portname = value;
            }
        }

        public int _Interval
        {
            get
            {
                return _Interval_T;
            }
            set
            {
                _Interval_T = value;
            }
        }


        #region ** 수신 관련



        string RCV_Pre_1 = "";
        string RCV_Data = "";

        private void SERIAL_RCV(object s, EventArgs e)
        {
            try
            {
                RCV_Pre_1 += SP.ReadExisting();
                char[] RCV_Pre_2 = RCV_Pre_1.ToCharArray();

                if (RCV_Pre_2.Length >= 24)
                {
                    RCV_Pre_1 = "";
                    RCV_Data = "";
                }

                #region ** 처리함수
                if (RCV_Pre_2.Length >= 8)
                {
                    try
                    {
                        int index = RCV_Pre_1.IndexOf(Convert.ToChar(0x03));

                        if (RCV_Pre_2[index - 7] == 0x02 && RCV_Pre_2[index + 0] == 0x03)
                        {
                            RCV_Data = RCV_Pre_2[index - 7].ToString() + RCV_Pre_2[index - 6].ToString() + RCV_Pre_2[index - 5].ToString() + RCV_Pre_2[index - 4].ToString() +
                                       RCV_Pre_2[index - 3].ToString() + RCV_Pre_2[index - 2].ToString() + RCV_Pre_2[index - 1].ToString() + RCV_Pre_2[index + 0].ToString();

                            // RCV_Data = RCV_Pre_2[index + 0].ToString() + RCV_Pre_2[index + 1].ToString() + RCV_Pre_2[index + 2].ToString() + RCV_Pre_2[index + 3].ToString() +
                            //         RCV_Pre_2[index + 4].ToString() + RCV_Pre_2[index + 5].ToString() + RCV_Pre_2[index + 6].ToString() + RCV_Pre_2[index + 7].ToString();
                            RCV_Pre_1 = "";

                            #region ** 진수 변환

                            string HEX = Convert.ToString(RCV_Data.Substring(1, 4));
                            char[] HEX_2 = HEX.ToCharArray();
                            string INPUT_bit = "";
                            string INPUT_bit_Temp = "";

                            foreach (char HEX_3 in HEX_2)
                            {
                                string dfa = HEX_3.ToString();

                                byte Hex2byte = Convert.ToByte(dfa, 16); // 16진수 변환

                                string byte2bit = Convert.ToString(Hex2byte, 2); // 2진수 변환

                                char[] charLength = byte2bit.ToCharArray();

                                switch (charLength.Length)
                                {
                                    case 4:

                                        INPUT_bit_Temp += byte2bit;
                                        break;
                                    case 3:

                                        INPUT_bit_Temp += "0" + byte2bit;
                                        break;
                                    case 2:

                                        INPUT_bit_Temp += "00" + byte2bit;
                                        break;
                                    case 1:

                                        INPUT_bit_Temp += "000" + byte2bit;
                                        break;
                                }
                            }
                            #endregion

                            #region ** 데이타 타입 맞추기 (역순 출력)

                            for (int i = 0; i <= 15; i++)
                            {
                                INPUT_bit += INPUT_bit_Temp[15 - i].ToString();

                                if (INPUT_bit_Temp[15 - i].ToString() == "1")
                                {
                                    X16[i] = 1;
                                }
                                else
                                {
                                    X16[i] = 0;
                                }
                            }
                            #endregion

                            #region ** UI 표현하기
                            if (UI_Control == true)
                            {
                                Control.ControlCollection P = this.panel_INPUT.Controls;

                                for (int I = 0; I < P.Count; I++)
                                {
                                    Panel CU = (Panel)P[I];

                                    if (X16[Convert.ToUInt32(CU.Tag)].ToString() == "1")
                                    {
                                        CU.BackColor = System.Drawing.Color.GreenYellow;
                                    }
                                    else
                                    {
                                        CU.BackColor = System.Drawing.Color.DarkRed;
                                    }
                                }
                            }
                            #endregion


                        }
                        else
                        {
                            RCV_Pre_1 = "";
                        }
                    }

                    catch
                    {
                        RCV_Pre_1 = "";
                    }
                }
                #endregion

            }
            catch { }
        }

        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(SERIAL_RCV));
        }

        #endregion


        /// <summary>
        /// 분주 타이머 코드 사용하기
        /// </summary>
        int TIMER_COINT = 0;
        private int _Timer(int division)
        {
            int AAA = TIMER_COINT % division;
            TIMER_COINT++;

            if (TIMER_COINT >= 100)
            {
                TIMER_COINT = 1;
            }
            return AAA;
        }



        private int _Interval_T = 25;

        private void timer_Main_Tick(object sender, EventArgs e)
        {
            timer_Main.Interval = _Interval_T;
            if (_Conn_OK == true)
            {
                byte[] ETX = { 0x03 };

                switch (_Timer(2))
                {
                    case 0: // RX
                        try
                        {
                            byte[] STX = { 0x02, 0x46, 0x31 };
                            SP.Write(STX, 0, 3);
                            SP.Write(ETX, 0, 1);
                        }
                        catch { }

                        break;

                    case 1: // TX
                        if (_FreeRun == true)
                        {
                            FreeRun();
                        }
                        else
                        {
                            _Out();
                        }
                        break;
                }
            }
        }

        System.Timers.Timer TM = new System.Timers.Timer();

        private void TM_Elapsed(object sender, ElapsedEventArgs e)
        {
            TM.Interval = _Interval_T;
            if (_Conn_OK == true)
            {
                byte[] ETX = { 0x03 };

                switch (_Timer(2))
                {
                    case 0: // RX
                        try
                        {
                            byte[] STX = { 0x02, 0x46, 0x31 };
                            SP.Write(STX, 0, 3);
                            SP.Write(ETX, 0, 1);
                        }
                        catch { }

                        break;

                    case 1: // TX
                        if (_FreeRun == true)
                        {
                            FreeRun();
                        }
                        else
                        {
                            _Out();
                        }
                        break;
                }
            }
        }

        int cnt = 0;
        private void FreeRun()
        {
            try
            {
                if (_Conn_OK == true)
                {
                    for (int i = 0; i <= 15; i++) // 데이타 타입 맞추기 (역순 출력)
                    {
                        Y16[i] = 0;
                    }

                    if (UI_Control == true)
                    {
                        Control.ControlCollection CUI = this.panel_OUT.Controls;
                        for (int I = 0; I < CUI.Count; I++)
                        {
                            Panel LED = (Panel)CUI[I];

                            LED.BackColor = System.Drawing.Color.DarkRed;
                        }

                        for (int I = 0; I < CUI.Count; I++)
                        {
                            Panel LED = (Panel)CUI[I];
                            if (LED.Tag.ToString() == cnt.ToString())
                            {
                                LED.BackColor = System.Drawing.Color.GreenYellow;
                            }
                        }
                    }

                    if (cnt <= 15)
                    {
                        Y16[cnt] = 1;
                        cnt++;
                    }
                    else
                    {
                        cnt = 0;
                    }

                    _Out();
                }
            }
            catch { }
        }
    } // 마지막
}




