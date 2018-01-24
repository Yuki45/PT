using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.IO.Ports;

namespace Auto_Scanner_IMEI
{
    public partial class MainForm : Form
    {
        ulong a;
        uint b;
        ulong cmdPos;
        ulong lJig1, lJig2, lJig3, lJig4, lJig5, lJig6, lJig7, lJig8, lJig9, lJig10;
        uint velocity = 0, result =0;
        bool MoveStatus = false;
        int status = 0;
        bool sJig1,sjig2,sjig3,sjig4,sjig5,sjig6,sjig7,sjig8,sjig9,sjig10;
        bool sPort1, sPort2, sPort3, sPort4, sPort5, sPort6, sPort7, sPort8, sPort9, sPort10;
        string Pstatus = "";

        string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        SerialPort COM = new SerialPort();
        SerialPort SCANNER = new SerialPort();
        SerialPort PGM = new SerialPort();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtCom.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM") == "") ? "COM25" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM");
            txtIO.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "I/O COM") == "") ? "COM23" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "I/O COM");
            txtPGM.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "PGM COM") == "") ? "COM26" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "PGM COM");
            txtScanner.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SCANNER COM") == "") ? "COM27" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SCANNER COM");
               
        
        }

        private void loadConfig()
        {
            lJig1 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 1", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 1", "VALUE"));
            lJig2 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 2", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 2", "VALUE"));
            lJig3 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 3", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 3", "VALUE"));
            lJig4 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 4", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 4", "VALUE"));
            lJig5 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 5", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 5", "VALUE"));
            lJig6 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 6", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 6", "VALUE"));
            lJig7 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 7", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 7", "VALUE"));
            lJig8 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 8", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 8", "VALUE"));
            lJig9 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 9", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 9", "VALUE"));
            lJig10 = ulong.Parse((Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 10", "VALUE") == "") ? "0" : Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 10", "VALUE"));

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connected")
            {
                if (IO.FAS_Connect(byte.Parse(txtCom.Text.Substring(3,txtCom.Text.Length - 3)), uint.Parse("115200")) >= 1)
                {
                    btnConnect.Text = "Disconnect";
                    status = 1;
                    timerOn();
                    OpenIO();
                    loadConfig();
                    OpenPGM();
                    OpenScanner();
                    timer1.Enabled = true;
                    txtCom.ReadOnly = true;
                    txtIO.ReadOnly = true;
                    txtScanner.ReadOnly = true;
                    txtPGM.ReadOnly = true;
                    btnConnect.BackColor = Color.Green;
                }
                else
                {
                    status = 0;
                    btnConnect.Text = "Connected";
                    MessageBox.Show("Error");
                    btnConnect.BackColor = Color.Red;
                }
            }
            else
            {
                if (status == 1)
                {
                    COM.Close();
                    timelim.Abort();
                    timer1.Enabled = false;
                    IO.FAS_Close(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)));
                    btnConnect.Text = "Connected";
                    txtCom.ReadOnly = false;
                    txtIO.ReadOnly = false;

                    txtScanner.ReadOnly = false;
                    txtPGM.ReadOnly = false;
                    status = 0;
                    btnConnect.BackColor = Color.Red;
                }
            }
        }

        private void OpenIO()
        {
            try
            {
                COM.Close();
                COM.PortName = txtIO.Text;
                COM.BaudRate = 115200;
                COM.Open();
                txtIO.BackColor = Color.Green;
                this.COM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            }
            catch
            {
                txtIO.BackColor = Color.Red;
            }
        }

        private void OpenScanner()
        {
            try
            {
                SCANNER.Close();
                SCANNER.PortName = txtScanner.Text;
                SCANNER.BaudRate = 9600;
                SCANNER.Open();
                txtScanner.BackColor = Color.Green;
                this.SCANNER.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SCANNER_DataReceived);
            }
            catch
            {
                txtScanner.BackColor = Color.Red;
            }
        }

        private void OpenPGM()
        {
            try
            {
                PGM.Close();
                PGM.PortName = txtPGM.Text;
                PGM.BaudRate = 9600;
                PGM.Open();
                txtPGM.BackColor = Color.Green;
                this.PGM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.PGM_DataReceived);
            }
            catch
            {
                txtPGM.BackColor = Color.Red;
            }
        }
        public void realtime()
        {
            while (true)
            {
                if (status == 1)
                {
                    int command;
                    command = IO.FAS_GetActualPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out a);

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtActPos.Text = a.ToString()));
                    }
                    else
                    {
                        txtActPos.Text = a.ToString();
                    }

                    try
                    {if (!MoveStatus)
                        COM.Write("R01\r\n");
                    }
                    catch { }
                }
                Thread.Sleep(300);
            }
        }

        Thread timelim;
        private void timerOn()
        {
            timelim = new Thread(() => realtime());
            timelim.Start();
        }

        string msg3 ="";
        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            string data = COM.ReadExisting();
            

            if (data.Length >= 16)
            {
                try
                {
                    
                }
                catch (Exception ex) { //logfiles.WriteLogAgent(data.ToString() + "\r\n", appPath + "//App.log"); 
                }
            }
            int no;
            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (msg3.Length >=16)
                {
                   
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtport.Text = msg3));
                    }
                    else
                    {
                        txtport.Text = msg3;
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => ReadJIG(msg3)));
                    }
                    else
                    {
                        ReadJIG(msg3);
                    }

                    msg3 = "";
                }
                else if (data[a] == (char)2)
                {
                    msg3 = "";
                }
                else
                {
                    try {
                        if (int.TryParse(data[a].ToString(),out no))
                            msg3 = msg3 + data[a];
                        else
                            logfiles.WriteLogAgent(data.ToString() + "\r\n", appPath + "//LogIO.log");

                    }catch
                    {
                        logfiles.WriteLogAgent(data.ToString() + "\r\n", appPath + "//LogIO.log");
                    }
                }
            }
            //
        }

        private void ReadStatus(string status)
        {
            // RD <RD104> Status Ready Kuning
            // OK <OK104> Status Masuk Testing Program Emas
            // PS <PS104> Status PASS PGM  Hijau Muda
            // FL <FL104> Status FAIL  Merah
            // LP <LP044>,"FAIL","PACK INSERT/POWER ON Fail[C],0[1~1]","" Status Info Label Print Fail
            if (status.Length > 5)
            {

                if (status == "<RD104>")
                {
                    //jig1.BackColor = Color.Gray;
                    
                }else
                if (status == "<RD094>")
                {
                    //jig2.BackColor = Color.Gray;
                    //SnJig2.Text = "";
                }else
                if (status == "<RD084>")
                {
                    //jig3.BackColor = Color.Gray;
                    //SnJig3.Text = "";
                }
                else
                if (status == "<RD074>")
                {
                    //jig4.BackColor = Color.Gray;
                    //SnJig4.Text = "";
                }
                else
                if (status == "<RD064>")
                {
                    //jig5.BackColor = Color.Gray;
                    //SnJig5.Text = "";
                }
                else
                if (status == "<RD054>")
                {
                    //jig6.BackColor = Color.Gray;
                    //SnJig6.Text = "";
                }
                else
                if (status == "<RD044>")
                {
                    //jig7.BackColor = Color.Gray;
                    //SnJig7.Text = "";
                }
                else
                if (status == "<RD034>")
                {
                    //jig8.BackColor = Color.Gray;
                    //SnJig8.Text = "";
                }

                if (status == "<OK104>")
                {
                jig10.BackColor = Color.Yellow;
                sPort10 = true;
                }
                else
                if (status == "<OK094>")
                {
                    jig9.BackColor = Color.Yellow;
                    sPort9 = true;
                }
                else
                if (status == "<OK084>")
                {
                    jig8.BackColor = Color.Yellow;
                    sPort8 = true;
                }
                else
                if (status == "<OK074>")
                {
                    jig7.BackColor = Color.Yellow;
                    sPort7 = true;
                }
                else
                if (status == "<OK064>")
                {
                    jig6.BackColor = Color.Yellow;
                    sPort6 = true;
                }
                else
                if (status == "<OK054>")
                {
                    jig5.BackColor = Color.Yellow;
                    sPort5 = true;
                }
                else
                if (status == "<OK044>")
                {
                    jig4.BackColor = Color.Yellow;
                    sPort4 = true;
                }
                else
                if (status == "<OK034>")
                {
                    jig3.BackColor = Color.Yellow;
                    sPort3 = true;
                }

                if (status == "<PS104>")
                {
                    jig10.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T70\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig10.Text = "";
                    sPort10 = false;
                }else
                if (status == "<PS094>")
                {
                    jig9.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T60\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig9.Text = "";
                    sPort9 = false;
                }
                else
                if (status == "<PS084>")
                {
                    jig8.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T50\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig8.Text = "";
                    sPort8 = false;
                }
                else
                if (status == "<PS074>")
                {
                    jig7.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T40\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig7.Text = "";
                    sPort7 = false;
                }
                else
                if (status == "<PS064>")
                {
                    jig6.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T30\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig6.Text = "";
                    sPort6 = false;
                }
                else
                if (status == "<PS054>")
                {
                    jig5.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T20\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig5.Text = "";
                    sPort5 = false;
                }
                else
                if (status == "<PS044>")
                {
                    jig4.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T10\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig4.Text = "";
                    sPort4 = false;
                }
                else
                if (status == "<PS034>")
                {
                    jig3.BackColor = Color.Chartreuse;
                    try
                    {
                        COM.Write("T00\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig3.Text = "";
                    sPort3 = false;
                }

                if (status == "<FL104>")
                {
                    jig10.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T70\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig10.Text = "";
                    sPort10 = false;
                }
                else
                if (status == "<FL094>")
                {
                    jig9.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T60\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig9.Text = "";
                    sPort9 = false;
                }
                else
                if (status == "<FL084>")
                {
                    jig8.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T50\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig8.Text = "";
                    sPort8 = false;
                }
                else
                if (status == "<FL074>")
                {
                    jig7.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T40\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig7.Text = "";
                    sPort7 = false;
                }
                else
                if (status == "<FL064>")
                {
                    jig6.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T30\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig6.Text = "";
                    sPort6 = false;
                }
                else
                if (status == "<FL054>")
                {
                    jig5.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T20\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig5.Text = "";
                    sPort5 = false;
                }
                else
                if (status == "<FL044>")
                {
                    jig4.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T10\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig4.Text = "";
                    sPort4 = false;
                }
                else
                if (status == "<FL034>")
                {
                    jig3.BackColor = Color.Red;
                    try
                    {
                        COM.Write("T00\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                    SnJig3.Text = "";
                    sPort3 = false;
                }

                
            }
        }

        string msg = "", msg2;
        private void PGM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = PGM.ReadExisting();
            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {   
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => ReadStatus(msg)));
                        }
                        else
                        {
                            ReadStatus(msg);
                        }

                        
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => richTextBox1.Text = msg+"\r\n"));
                        }
                        else
                        {
                            richTextBox1.Text = msg + "\r\n";
                        }
                    msg = "";
                }
                else if (data[a] == (char)2)
                {
                    msg = "";
                }
                else
                {
                    if (data[a].ToString() != "\r")
                        msg = msg + data[a];
                }
            }
        }

        int CountScan = 0;
        private void SCANNER_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = SCANNER.ReadExisting();
            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    try
                    {
                        if (CountScan >= 3)
                        {
                            reset();
                            CountScan = 0;
                            switch (Pstatus)
                            {
                                case "JIG1" :
                                    {
                                        break;
                                    }
                                case "JIG2":
                                    {
                                        break;
                                    }
                                case "JIG3":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig3.BackColor= Color.Red));
                                        }
                                        else
                                        {
                                            jig3.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG4":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig4.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig4.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG5":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig5.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig5.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG6":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig6.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig6.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG7":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig7.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig7.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG8":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig8.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig8.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG9":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig9.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig9.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                                case "JIG10":
                                    {
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => jig10.BackColor = Color.Red));
                                        }
                                        else
                                        {
                                            jig10.BackColor = Color.Red;
                                        }
                                        break;
                                    }
                            }


                        }else
                        if (msg2 == "NOREAD")
                        {
                            
                            try
                            {
                                SCANNER.Write("L\r\n");

                                int command;
                                if (CountScan == 1)
                                {
                                    command = IO.FAS_MoveVelocity(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10000, 1);
                                    Thread.Sleep(300);
                                    command = IO.FAS_MoveStop(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
                                }else
                                if (CountScan == 2)
                                {
                                    command = IO.FAS_MoveVelocity(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10000, 0);
                                    Thread.Sleep(400);
                                    command = IO.FAS_MoveStop(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
                                }
                            }
                            catch
                            {
                                logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                            }
                            CountScan++;
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtSN.Text = msg2));
                            }
                            else
                            {
                                txtSN.Text = msg2;
                            }
                        }
                    }
                    catch
                    {
                    }
                    msg2 = "";
                }
                else if (data[a] == (char)2)
                {
                    msg2 = "";
                }
                else
                {
                    if (data[a].ToString() != "\r")
                        msg2 = msg2 + data[a];
                }
            }
        }

        private void ReadJIG(string StatusJIG)
        {
            //02224262
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
                            //if (!sJig1)
                            Jig3 = StatusJIG.Substring(0, 2);
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
                            Jig4 = StatusJIG.Substring(2, 2);
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
                            Jig5 = StatusJIG.Substring(4, 2);
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
                            Jig6 = StatusJIG.Substring(6, 2);
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
                            Jig7 = StatusJIG.Substring(8, 2);
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
                            Jig8 = StatusJIG.Substring(10, 2);
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
                            Jig9 = StatusJIG.Substring(12, 2);
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
                            Jig10 = StatusJIG.Substring(14, 2);
                        }
                        catch
                        {
                            //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }
                    //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//AppPort.log");
                }
            }
            else
            {
                //logfiles.WriteLogAgent(StatusJIG.ToString(), appPath + "//AppPort.log");
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (status==1)
            {
            COM.Close();
            timelim.Abort();
            IO.FAS_Close(byte.Parse("16"));
            }

            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM",txtCom.Text) ;
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "I/O COM", txtIO.Text) ;
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "SCANNER COM", txtScanner.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "PGM COM", txtPGM.Text);
        }

        private void txtport_TextChanged(object sender, EventArgs e)
        {
            if (txtport.Text.Length >= 16)
            {
                try
                {
                    if (InvokeRequired)
                    {
                       // Invoke(new MethodInvoker(() => ReadJIG(txtport.Text)));
                    }
                    else
                    {
                        //ReadJIG(txtport.Text);
                    }
                }
                catch (Exception ex) { logfiles.WriteLogAgent(txtport.Text + "\r\n", appPath + "//App.log"); }
            }
        }

        #region Variabel c#
        string _jig3;
        public string Jig3
        {
            get
            {
                return this._jig3;
            }
            set
            {
                if (value == "01")
                {
                    SnJig3.BackColor = Color.Yellow;
                    sjig3 = false;
                }
                else if (value == "00")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig3 && !sPort3 && SnJig3.BackColor != Color.Green)
                    {
                        sjig3 = true;
                        listView1.Items.Add(lvi1);
                    }
                    SnJig3.BackColor = Color.Green;
                }

                this._jig3 = value;
            }
        }

        string _jig4;
        public string Jig4
        {
            get
            {
                return this._jig4;
            }
            set
            {
                if (value == "11")
                {
                    SnJig4.BackColor = Color.Yellow;
                    sjig4 = false;
                }
                else if (value == "10")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig4 && !sPort4 && SnJig4.BackColor != Color.Green)
                    {
                        sjig2 = true;
                        listView1.Items.Add(lvi1);
                    }
                    SnJig4.BackColor = Color.Green;
                }

                this._jig4 = value;
            }
        }

        string _jig5;
        public string Jig5
        {
            get
            {
                return this._jig5;
            }
            set
            {
                if (value == "21")
                {
                    sjig5 = false;
                    SnJig5.BackColor = Color.Yellow;
                }
                else if (value == "20")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig5 && !sPort5 && SnJig5.BackColor != Color.Green)
                    {
                        sjig5 = true;
                        listView1.Items.Add(lvi1);
                    }

                    SnJig5.BackColor = Color.Green;
                }

                this._jig5 = value;
            }
        }

        string _jig6;
        public string Jig6
        {
            get
            {
                return this._jig6;
            }
            set
            {
                if (value == "31")
                {
                    sjig6 = false;
                    SnJig6.BackColor = Color.Yellow;
                }
                else if (value == "30")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig6 && !sPort6 && SnJig6.BackColor != Color.Green)
                    {
                        sjig6 = true;
                        listView1.Items.Add(lvi1);
                    }

                    SnJig6.BackColor = Color.Green;
                }

                this._jig6 = value;
            }
        }

        string _jig7;
        public string Jig7
        {
            get
            {
                return this._jig7;
            }
            set
            {
                if (value == "41")
                {
                    sjig7 = false;
                    SnJig7.BackColor = Color.Yellow;
                }
                else if (value == "40")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig7 && !sPort7 && SnJig7.BackColor != Color.Green)
                    {
                        sjig7 = true;
                        listView1.Items.Add(lvi1);
                    }

                    SnJig7.BackColor = Color.Green;
                }

                this._jig7 = value;
            }
        }

        string _jig8;
        public string Jig8
        {
            get
            {
                return this._jig8;
            }
            set
            {
                if (value == "51")
                {
                    sjig8 = false;
                    SnJig8.BackColor = Color.Yellow;
                }
                else if (value == "50")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig8 && !sPort8 && SnJig8.BackColor != Color.Green)
                    {
                        sjig8 = true;
                        listView1.Items.Add(lvi1);
                    }

                    SnJig8.BackColor = Color.Green;
                }

                this._jig8 = value;
            }
        }

        string _jig9;
        public string Jig9
        {
            get
            {
                return this._jig9;
            }
            set
            {
                if (value == "61")
                {
                    sjig9 = false;
                    SnJig9.BackColor = Color.Yellow;
                }
                else if (value == "60")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig9 && !sPort9 && SnJig9.BackColor != Color.Green)
                    {
                        sjig9 = true;
                        listView1.Items.Add(lvi1);
                    }
                    SnJig9.BackColor = Color.Green;
                }

                this._jig9 = value;
            }
        }

        string _jig10;
        public string Jig10
        {
            get
            {
                return this._jig10;
            }
            set
            {
                if (value == "71")
                {
                    sjig10 = false;
                    SnJig10.BackColor = Color.Yellow;
                }
                else if (value == "70")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig10 && !sPort10 && SnJig10.BackColor != Color.Green)
                    {
                        sjig10 = true;
                        listView1.Items.Add(lvi1);
                    }

                    SnJig10.BackColor = Color.Green;
                }

                this._jig10 = value;
            }
        }
        #endregion

        #region Variabel Scanner...

        bool _Pjig1;
        public bool PJig1
        {
            get
            {
                return this._Pjig1;
            }
            set
            {
                
                this._Pjig1 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig2;
        public bool PJig2
        {
            get
            {
                return this._Pjig2;
            }
            set
            {

                this._Pjig2 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig3;
        public bool PJig3
        {
            get
            {
                return this._Pjig3;
            }
            set
            {

                this._Pjig3 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig4;
        public bool PJig4
        {
            get
            {
                return this._Pjig4;
            }
            set
            {

                this._Pjig4 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig5;
        public bool PJig5
        {
            get
            {
                return this._Pjig5;
            }
            set
            {

                this._Pjig5 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig6;
        public bool PJig6
        {
            get
            {
                return this._Pjig6;
            }
            set
            {

                this._Pjig6 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig7;
        public bool PJig7
        {
            get
            {
                return this._Pjig7;
            }
            set
            {
                this._Pjig7 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig8;
        public bool PJig8
        {
            get
            {
                return this._Pjig8;
            }
            set
            {

                this._Pjig8 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig9;
        public bool PJig9
        {
            get
            {
                return this._Pjig9;
            }
            set
            {

                this._Pjig9 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }

        bool _Pjig10;
        public bool PJig10
        {
            get
            {
                return this._Pjig10;
            }
            set
            {

                this._Pjig10 = value;
                if (value)
                {
                    try
                    {
                        SCANNER.Write("L\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("SCANNER: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
            }
        }
        #endregion

        private bool IsInCollection(ListViewItem lvi)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                bool subItemEqualFlag = true;
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    string sub1 = item.SubItems[i].Text;
                    string sub2 = lvi.SubItems[i].Text;
                    if (sub1 != sub2)
                    {
                        subItemEqualFlag = false;
                    }
                }
                if (subItemEqualFlag)
                    return true;
            }
            return false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (status == 1 && !gbConfig.Enabled)
            {
                int command;
                command = IO.FAS_GetCommandPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out cmdPos);

                if (listView1.Items.Count > 0)
                {
                    if (listView1.Items[0].SubItems[0].Text == "00")
                    {
                        if (!MoveStatus)
                        {
                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 3);
                            MoveStatus = true;
                        }
                        else if (lJig3 == cmdPos && MoveStatus && !PJig3)
                        {
                            PJig3 = true;
                            Pstatus = "JIG3";
                        }
                        else if (lJig3 == cmdPos && PJig3 && MoveStatus && txtSN.Text.Length == 15)
                        {
                            
                                
                                try
                                {
                                    COM.Write("T01\r\n");
                                }
                                catch
                                {
                                    logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
                                }
                                listView1.Items[0].Remove();
                                Thread.Sleep(1000); 
                                SnJig3.Text = txtSN.Text;                               
                                try
                                {
                                    PGM.Write("<ST03[" + SnJig3.Text + "]4F>\r\n");
                                }
                                catch
                                {
                                    logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                }
                                txtSN.Text = "";
                                PJig3 = false;
                                MoveStatus = false;
                            
                        }
                        else
                        {
                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 3);
                        }
                    }
                    else
                        if (listView1.Items[0].SubItems[0].Text == "10")
                        {
                            if (!MoveStatus)
                            {
                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 4);
                                MoveStatus = true;
                            }
                            else if (lJig4 == cmdPos && MoveStatus && !PJig4)
                            {
                                PJig4 = true;
                                Pstatus = "JIG4";
                            }
                            else if (lJig4 == cmdPos && PJig4 && MoveStatus && txtSN.Text.Length == 15)
                            {
                                
                                    
                                    try
                                    {
                                        COM.Write("T11\r\n");
                                    }
                                    catch
                                    {
                                        logfiles.WriteLogAgent("PORT 2 OUT: ERROR WRITE...!", appPath + "//App.log");
                                    }
                                    listView1.Items[0].Remove();
                                    Thread.Sleep(2000);
                                    SnJig4.Text = txtSN.Text;
                                    try
                                    {
                                        PGM.Write("<ST04[" + SnJig4.Text + "]4F>\r\n");
                                    }
                                    catch
                                    {
                                        logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                    }
                                    txtSN.Text = "";
                                    PJig4 = false;
                                    MoveStatus = false;
                                
                            }
                            else
                            {
                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 4);
                            }
                        }
                        else
                        if (listView1.Items[0].SubItems[0].Text == "20")
                          {
                            if (!MoveStatus)
                            {
                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 5);
                                MoveStatus = true;
                            }
                            else if (lJig5 == cmdPos && MoveStatus && !PJig5)
                            {
                                PJig5 = true;
                                Pstatus = "JIG5";
                            }
                            else if (lJig5 == cmdPos && PJig5 && MoveStatus && txtSN.Text.Length == 15)
                            {
                                   
                                       
                                        try
                                        {
                                            COM.Write("T21\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("PORT 3 OUT: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        listView1.Items[0].Remove();
                                        Thread.Sleep(2000);
                                        SnJig5.Text = txtSN.Text;
                                        try
                                        {
                                            PGM.Write("<ST05[" + SnJig5.Text + "]4F>\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        txtSN.Text = "";
                                        PJig5 = false;
                                        MoveStatus = false;
                                   
                                }
                                else
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 5);
                                }
                            }
                        else
                            if (listView1.Items[0].SubItems[0].Text == "30")
                            {
                                if (!MoveStatus)
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 6);
                                    MoveStatus = true;
                                }
                                else if (lJig6 == cmdPos && MoveStatus && !PJig6)
                                {
                                    PJig6 = true;
                                    Pstatus = "JIG6";
                                }
                                else if (PJig6 && MoveStatus && txtSN.Text.Length == 15 && lJig6 == cmdPos)
                                {
                                    if (txtSN.Text.Length > 14)
                                    {
                                      

                                        try
                                        {
                                            COM.Write("T31\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("PORT 4 OUT: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        listView1.Items[0].Remove();
                                        Thread.Sleep(2000);
                                        SnJig6.Text = txtSN.Text;
                                        try
                                        {
                                            PGM.Write("<ST06[" + SnJig6.Text + "]4F>\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        txtSN.Text = "";
                                        PJig6 = false;
                                        MoveStatus = false;
                                    }
                                }
                                else
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 6);
                                }
                            }
                        else
                            if (listView1.Items[0].SubItems[0].Text == "40")
                            {
                                if (!MoveStatus)
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 7);
                                    MoveStatus = true;
                                }
                                else if (lJig7 == cmdPos && MoveStatus && !PJig7)
                                {
                                    PJig7 = true;
                                    Pstatus = "JIG7";
                                }
                                else if (PJig7 && MoveStatus && txtSN.Text.Length == 15 && lJig7 == cmdPos)
                                {
                                        

                                        try
                                        {
                                            COM.Write("T41\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("PORT 5 OUT: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        listView1.Items[0].Remove();
                                        Thread.Sleep(2000);
                                        SnJig7.Text = txtSN.Text;
                                        try
                                        {
                                            PGM.Write("<ST07[" + SnJig7.Text + "]4F>\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        txtSN.Text = "";
                                        PJig7 = false;
                                        MoveStatus = false;
                                    
                                }
                                else
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 7);
                                }
                            }
                            else
                            if (listView1.Items[0].SubItems[0].Text == "50")
                            {
                                if (!MoveStatus)
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 8);
                                    MoveStatus = true;
                                }
                                else if (lJig8 == cmdPos && MoveStatus && !PJig8)
                                {
                                    PJig8= true;
                                    Pstatus = "JIG8";
                                }
                                else if (PJig8 && MoveStatus && txtSN.Text.Length == 15 && lJig8 == cmdPos)
                                {                                   
                                        try
                                        {
                                            COM.Write("T51\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("PORT 5 OUT: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        listView1.Items[0].Remove();
                                        Thread.Sleep(2000);
                                        SnJig8.Text = txtSN.Text;
                                        try
                                        {
                                            PGM.Write("<ST08[" + SnJig8.Text + "]4F>\r\n");
                                        }
                                        catch
                                        {
                                            logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                        }
                                        txtSN.Text = "";
                                        PJig8 = false;
                                        MoveStatus = false;
                                    
                                }
                                else
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 8);
                                }
                            }
                            else
                                if (listView1.Items[0].SubItems[0].Text == "60")
                                {
                                    if (!MoveStatus)
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 9);
                                        MoveStatus = true;
                                    }
                                    else if (lJig9 == cmdPos && MoveStatus && !PJig9)
                                    {
                                        PJig9= true;
                                        Pstatus = "JIG9";
                                    }
                                    else if (PJig9 && MoveStatus && txtSN.Text.Length == 15 && lJig9 == cmdPos)
                                    {
                                        
                                            
                                            try
                                            {
                                                COM.Write("T61\r\n");
                                            }
                                            catch
                                            {
                                                logfiles.WriteLogAgent("PORT 7 OUT: ERROR WRITE...!", appPath + "//App.log");
                                            }
                                            listView1.Items[0].Remove();
                                            Thread.Sleep(2000);
                                            SnJig9.Text = txtSN.Text;
                                            try
                                            {
                                                PGM.Write("<ST09[" + SnJig9.Text + "]4F>\r\n");
                                            }
                                            catch
                                            {
                                                logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                            }
                                            txtSN.Text = "";
                                            PJig9 = false;
                                            MoveStatus = false;
                                        
                                    }
                                    else
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 9);
                                    }
                                }
                            else
                                if (listView1.Items[0].SubItems[0].Text == "70")
                                {
                                    if (!MoveStatus)
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10);
                                        MoveStatus = true;
                                    }
                                    else if (lJig10 == cmdPos && MoveStatus && !PJig10)
                                    {
                                        PJig10 = true;
                                        Pstatus = "JIG10";
                                    }
                                    else if (PJig10 && MoveStatus && txtSN.Text.Length == 15 && lJig10 == cmdPos)
                                    {
                                        
                                            
                                            try
                                            {
                                                COM.Write("T71\r\n");
                                            }
                                            catch
                                            {
                                                logfiles.WriteLogAgent("PORT 8 OUT: ERROR WRITE...!", appPath + "//App.log");
                                            }
                                            listView1.Items[0].Remove();
                                            Thread.Sleep(2000);
                                            SnJig10.Text = txtSN.Text;
                                            try
                                            {
                                                PGM.Write("<ST10[" + SnJig10.Text + "]4F>\r\n");
                                            }
                                            catch
                                            {
                                                logfiles.WriteLogAgent("DASEUL TEST: ERROR WRITE...!", appPath + "//App.log");
                                            }
                                            txtSN.Text = "";
                                            PJig10 = false;
                                            MoveStatus = false;
                                        
                                    }
                                    else
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10);
                                    }
                                }

                }
                else
                {
                    if (cmdPos < 4294967255 && cmdPos > 0)
                    {
                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0);
                        Pstatus = "ORIGIN";
                    }
                }
            }
            timer1.Start();
        }

        #region Configuration....
        private void button6_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                
                command = IO.FAS_GetCommandPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out cmdPos);
                txtCmdPos.Text = cmdPos.ToString();
            }
        }

        private void btnJogP_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (status == 1)
                        {
                            int command;
                            command = IO.FAS_MoveVelocity(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10000, 1);
                        }
                       break;                        
                    }
                default:
                       break;
            } 
        }

        private void btnJogP_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (status == 1)
                        {
                            int command;
                            command = IO.FAS_MoveStop(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
                        }
                        break;
                    }
                default:
                    break;
            } 
        }

        private void btnJogM_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (status == 1)
                        {
                            int command;
                            command = IO.FAS_MoveVelocity(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 10000, 0);
                        }
                        break;
                    }
                default:
                    break;
            } 
        }

        private void btnJogM_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (status == 1)
                        {
                            int command;
                            command = IO.FAS_MoveStop(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void jig1_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "1";

                txtCmdPos.Text = Inifiles.ReadValue(appPath + "/SERVO.INI", "JIG 1", "VALUE");
            }
            else
            if (teach.status == "USE")
            {
                teach.status = "";
                gbConfig.Enabled = false;
                lblJig.Text = "1";
            }
            else
            if (teach.status == "RESET")
            {
                sPort1 = false;
                jig1.BackColor = Color.Silver;
                PJig1 = false;
                try
                {
                    //COM.Write("T00\r\n");
                }
                catch
                {
                    logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                }
            }
        }

        private void jig2_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "2";
            }
            else
            if (teach.status == "USE")
            {
                teach.status = "";
                gbConfig.Enabled = false;
                lblJig.Text = "2";
            }
            else
            if (teach.status == "RESET")
            {
                sPort2 = false;
                jig2.BackColor = Color.Silver;
                PJig2 = false;
                try
                {
                  //  COM.Write("T10\r\n");
                }
                catch
                {
                    logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                }
            }
        }

        private void jig3_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "3";
            }
            else
            if (teach.status == "USE")
            {
                teach.status = "";
                gbConfig.Enabled = false;
                lblJig.Text = "3";
            }
            else
            if (teach.status == "RESET")
            {
                sPort3 = false;
                jig3.BackColor = Color.Silver;
                PJig3 = false;
                try
                {
                    COM.Write("T00\r\n");
                }
                catch
                {
                    logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                }
            }
        }

        private void jig4_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "4";
            }
            else
            if (teach.status == "USE")
            {
                teach.status = "";
                gbConfig.Enabled = false;
                lblJig.Text = "4";
            }
            else
            if (teach.status == "RESET")
            {
                sPort4 = false;
                jig4.BackColor = Color.Silver;
                PJig4 = false;
                try
                {
                    COM.Write("T10\r\n");
                }
                catch
                {
                    logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                }
            }
        }

        private void jig5_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "5";
            }
            else
            if (teach.status == "USE")
            {
                teach.status = "";
                gbConfig.Enabled = false;
                lblJig.Text = "5";
            }
            else
            if (teach.status == "RESET")
            {
                sPort5 = false;
                jig5.BackColor = Color.Silver;
                PJig5 = false;
                try
                {
                    COM.Write("T20\r\n");
                }
                catch
                {
                    logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                }
            }
        }

        private void jig6_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "6";
            }
            else
                if (teach.status == "USE")
                {
                    teach.status = "";
                    gbConfig.Enabled = false;
                    lblJig.Text = "6";
                }
                else
                    if (teach.status == "RESET")
                    {
                        sPort6 = false;
                        jig6.BackColor = Color.Silver;
                        PJig6 = false;
                        try
                        {
                            COM.Write("T30\r\n");
                        }
                        catch
                        {
                            logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                        }
                    }
        }

        private void jig7_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "7";
            }
            else
                if (teach.status == "USE")
                {
                    teach.status = "";
                    gbConfig.Enabled = false;
                    lblJig.Text = "7";
                }
                else
                if (teach.status == "RESET")
                {
                    sPort7 = false;
                    jig7.BackColor = Color.Silver;
                    PJig7 = false;
                    try
                    {
                        COM.Write("T40\r\n");
                    }
                    catch
                    {
                        logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                    }
                }
        }

        private void jig8_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "8";
            }
            else
                if (teach.status == "USE")
                {
                    teach.status = "";
                    gbConfig.Enabled = false;
                    lblJig.Text = "8";
                }
                else
                    if (teach.status == "RESET")
                    {
                        sPort8 = false;
                        jig8.BackColor = Color.Silver;
                        PJig8 = false;
                        try
                        {
                            COM.Write("T50\r\n");
                        }
                        catch
                        {
                            logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                        }
                    }
        }

        private void jig9_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "9";
            }
            else
                if (teach.status == "USE")
                {
                    teach.status = "";
                    gbConfig.Enabled = false;
                    lblJig.Text = "9";
                }
                else
                    if (teach.status == "RESET")
                    {
                        sPort9 = false;
                        jig9.BackColor = Color.Silver;
                        PJig9 = false;
                        try
                        {
                            COM.Write("T60\r\n");
                        }
                        catch
                        {
                            logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                        }
                    }
        }

        private void jig10_Click(object sender, EventArgs e)
        {
            TeachingForm teach = new TeachingForm();
            teach.ShowDialog();
            if (teach.status == "TEACH")
            {
                teach.status = "";
                gbConfig.Enabled = true;
                lblJig.Text = "10";
            }
            else
                if (teach.status == "USE")
                {
                    teach.status = "";
                    gbConfig.Enabled = false;
                    lblJig.Text = "10";
                }
                else
                    if (teach.status == "RESET")
                    {
                        sPort10 = false;
                        jig10.BackColor = Color.Silver;
                        PJig10 = false;
                        try
                        {
                            COM.Write("T70\r\n");
                        }
                        catch
                        {
                            logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
                        }
                    }
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);
                command = IO.FAS_ServoEnable(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"),true);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (status == 1 && cmdPos>0)
            {
                int command;
                command = IO.FAS_PosTableWriteOneItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"),uint.Parse(lblJig.Text), velocity, long.Parse(txtCmdPos.Text));
                Inifiles.WriteValue(appPath + "/SERVO.INI", "JIG " + lblJig.Text, "VALUE", cmdPos.ToString());
                command = IO.FAS_PosTableWriteROM(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
               
                loadConfig();
            }
            gbConfig.Enabled = false;
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);
                //command = IO.FAS_ServoEnable(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), false);
                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), uint.Parse(lblJig.Text));
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);
                command = IO.FAS_ServoAlarmReset(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
            }
        }

        private void btnOrigin_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);
                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"));
            }
        }

        private void moveAbsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);

                command = IO.FAS_MoveSingleAxisAbsPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 30231, velocity);
            }
        }

        private void moveIncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (status == 1)
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0, result);

                command = IO.FAS_MoveSingleAxisIncPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 14030, velocity);
            }
        }
        
        private void txtvelocity_TextChanged(object sender, EventArgs e)
        {            
            Inifiles.WriteValue(appPath + "/SERVO.INI", "SETTING" , "VELOCITY", txtvelocity.Text);
            velocity = uint.Parse(txtvelocity.Text);
        }
        #endregion

        private void txtSN_TextChanged(object sender, EventArgs e)
        {
            if (txtSN.Text.Length > 14)
            {
                int command;
                //command = IO.FAS_GetCommandPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out cmdPos);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                COM.Write("T01\r\n");
            }
            catch
            {
                logfiles.WriteLogAgent("PORT 1 OUT: ERROR WRITE...!", appPath + "//App.log");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                COM.Write("T00\r\n");
                Thread.Sleep(300);
                COM.Write("T10\r\n");
                Thread.Sleep(300);
                COM.Write("T20\r\n");
                Thread.Sleep(300);
                COM.Write("T30\r\n");
                Thread.Sleep(300);
                COM.Write("T40\r\n");
                Thread.Sleep(300);
                COM.Write("T50\r\n");
                Thread.Sleep(300);
                COM.Write("T60\r\n");
                Thread.Sleep(300);
                COM.Write("T70\r\n");
                listView1.Items.Clear(); 
                MoveStatus = false;
                PJig1 = false;
                PJig2 = false;
                PJig3 = false;
                PJig4 = false;
                PJig5 = false;
                PJig6 = false;
                PJig7 = false;
                PJig8 = false;
                PJig9 = false;
                PJig10 = false;
                sPort1 = false;
                sPort2 = false;
                sPort3 = false;
                sPort4 = false;
                sPort5 = false;
                sPort6 = false;
                sPort7 = false;
                sPort8 = false; 
                sPort9 = false; 
                sPort10 = false;
                sJig1 = false;
                sjig2 = false;
                sjig3 = false;
                sjig4 = false;
                sjig5 = false;
                sjig6 = false;
                sjig7 = false;
                sjig8 = false;
                sjig9 = false;
                sjig10 = false;
                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0);
            }
            catch
            {
                logfiles.WriteLogAgent("PORT ALL OUT: ERROR WRITE...!", appPath + "//App.log");
            }
        }

        private void reset()
        {
            listView1.Items[0].Remove();
            MoveStatus = false;
            PJig1 = false;
            PJig2 = false;
            PJig3 = false;
            PJig4 = false;
            PJig5 = false;
            PJig6 = false;
            PJig7 = false;
            PJig8 = false;
            PJig9 = false;
            PJig10 = false; sPort1 = false;
            sPort2 = false;
            sPort3 = false;
            sPort4 = false;
            sPort5 = false;
            sPort6 = false;
            sPort7 = false;
            sPort8 = false;
            sPort9 = false;
            sPort10 = false;
            sJig1 = false;
            sjig2 = false;
            sjig3 = false;
            sjig4 = false;
            sjig5 = false;
            sjig6 = false;
            sjig7 = false;
            sjig8 = false;
            sjig9 = false;
            sjig10 = false;
            txtSN.Text = "";
            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0);
        }

    }
}
