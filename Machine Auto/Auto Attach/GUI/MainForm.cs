using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Auto_Attach.Library;
using System.IO;

namespace Auto_Attach
{
    public partial class MainForm : Form
    {
        public static MainForm instance;
        public Controller controller;
        private bool status = false;
        private string Position="";
        private string TeachPos;
        private string type="";
        private string page = "";
        private int isRetry = 0;
        uint velocity = 0, result = 0;
        public MainForm()
        {
            instance = this; 
            InitializeComponent();
        }

        #region Moving Form
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string CvyrX, CvyrZ, DspnX, DspnZ, stdbX, stdbZ;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }
        #endregion

        void loadConfig()
        {
            controller.CtrlIOPort.com = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "I/O COM") == "") ? "COM11" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "I/O COM"); //Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "I/O COM");
            controller.CtrlServoPort.com = (Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM") == "") ? "COM10" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM");
            controller.CtrlServoPort.baudrate = "115200";
            velocity = uint.Parse((Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "VELOCITY") == "") ? "50000" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "VELOCITY"));
            controller.DelayAttach = Convert.ToInt32((Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "DELAY ATTACH")=="")?"1000":Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "DELAY ATTACH"));
            controller.DelayPunch = Convert.ToInt32((Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "DELAY PUNCH") == "") ? "1000" : Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "DELAY PUNCH"));

        }

        void loadTeaching()
        {
            CvyrX = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 0", "ITEM2");
            CvyrZ = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 1", "ITEM2");
            DspnX = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 0", "ITEM1");
            DspnZ = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 1", "ITEM1");
            stdbX = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 0", "ITEM0");
            stdbZ = Inifiles.ReadValue(appPath + "/SERVO.INI", "SLAVE 1", "ITEM0");
        }
        void ClearBtn()
        {
            btnAuto.Image = Auto_Attach.Properties.Resources.but_01auto_disable;
            btnManual.Image = Auto_Attach.Properties.Resources.but_02manual_disable;
            btnTeach.Image = Auto_Attach.Properties.Resources.but_04teach_disable;
            btnData.Image = Auto_Attach.Properties.Resources.but_03data_disable;
            btnExit.Image = Auto_Attach.Properties.Resources.but_07exit_disable;
        }
        void btnUnderBarIcon_Click(object sender, EventArgs e)
        {
            ClearBtn();
            PictureBox btn = sender as PictureBox;
            if (btn.Name.Contains("Auto"))
            {
                btnAuto.Image = Auto_Attach.Properties.Resources.but_01auto_select;
                tabControl1.SelectTab(0);
                loadTeaching();
                page = "Auto";
            }
            else if (btn.Name.Contains("Manual"))
            {
                btnManual.Image = Auto_Attach.Properties.Resources.but_02manual_select;
                tabControl1.SelectTab(1);
                page = "Manual";
                status = false;
                Position = "";
            }
            else if (btn.Name.Contains("Teach"))
            {
                btnTeach.Image = Auto_Attach.Properties.Resources.but_04teach_select;
                tabControl1.SelectTab(2);
                page = "Teach";
                status = false;
                Position = "";

            }
            else if (btn.Name.Contains("Data"))
            {
                btnData.Image = Auto_Attach.Properties.Resources.but_03data_select;
                tabControl1.SelectTab(3);
                page = "Data";
                status = false;
                Position ="";
            }
            else
            {
                
                Thread.Sleep(300);
                btnExit.Image = Auto_Attach.Properties.Resources.but_07exit_select;
                if (MessageBox.Show("Do you want to terminate this program?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    Log.AddLog("Program Ended...");
                    Log.AddPmLog("Program Ended...");
                    Log.SaveLog();
                    try
                    {
                        controller.CtrlServoPort.timelim.Abort();
                        timeDryRun.Abort();
                        timelim.Abort();
                    }
                    catch { }
                    Application.Exit();
                }
            }
        }

        public void DisplayLog(string log)
        {
            if (lbLog.InvokeRequired)
            {
                lbLog.Invoke(new MethodInvoker(() => { DisplayLog(log); }));
            }
            else
            {
                lbLog.Items.Add(log);
                lbLog.SelectedIndex = (lbLog.Items.Count - 1);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            controller = new Controller(this);
            controller.CtrlServoPort = new Servo();
            controller.CtrlIOPort = new IOboard();
            loadConfig();
            controller.CtrlServoPort.ServoCon();
            controller.CtrlIOPort.Open(controller.CtrlIOPort.com, 115200);
            if (controller.CtrlServoPort.statusServo)
            {
                picServo.Image = Auto_Attach.Properties.Resources.bitmap51;
                RunDryRun();
            }
            else { picServo.Image = Auto_Attach.Properties.Resources.bitmap31; }

            //controller.CreateDevices();
            //controller.IOInitiate();

            string title = "Auto Attach Label Seal  ( Build : 2017-12-20 13:31:50)";
            Log log = new Log(this, Path.GetDirectoryName(Application.ExecutablePath)+@"\Log", "Auto Attach Label Seal");
            Log.AddLog(Environment.NewLine);
            Log.AddLog(Environment.NewLine);
            Log.AddLog(title);
            Log.AddPmLog(Environment.NewLine);
            Log.AddPmLog(Environment.NewLine);
            Log.AddPmLog(title);

            page = "Auto";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            GUI.TypeForm dlg = new GUI.TypeForm();
            dlg.ShowDialog();
            if (dlg.DialogResult.ToString() == "OK")
            {
                btnType.Text = dlg.result;
                if (dlg.result =="DRY RUN")
                {
                    controller.isDryRunning = true;
                }else
                {
                    controller.isDryRunning = false;
                }
            }
        }

        private void btnOrigin_Click(object sender, EventArgs e)
        {
            if (status && type == "NORMAL")
            {
                int command;
                command = IO.FAS_SetIOInput(byte.Parse(txtServo.Text.Substring(3, txtServo.Text.Length - 3)), byte.Parse("0"), 0, result);
                command = IO.FAS_MoveOriginSingleAxis(byte.Parse(txtServo.Text.Substring(3, txtServo.Text.Length - 3)), byte.Parse("0"));
            }
        }

        private void btnInfoSave_Click(object sender, EventArgs e)
        {
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "I/O COM", txtIO.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "SERVO COM", txtIO.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "VELOCITY", txtVelocity.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "DELAY ATTACH", txtDAttach.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "DELAY PUNCH", txtDPunch.Text);
        }

        #region Bagian Theaching
        private void btnJogP_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (controller.CtrlServoPort.statusServo)
                        {
                            controller.CtrlServoPort.Jogmove(lblservo.Text, 1);
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
                        if (controller.CtrlServoPort.statusServo)
                        {
                            controller.CtrlServoPort.JogStop(lblservo.Text);
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
                        if (controller.CtrlServoPort.statusServo)
                        {
                            controller.CtrlServoPort.Jogmove(lblservo.Text, 0);
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
                        if (controller.CtrlServoPort.statusServo)
                        {
                            controller.CtrlServoPort.JogStop(lblservo.Text);
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnCvyX_Click(object sender, EventArgs e)
        {
            lblservo.Text = "0";
            lblitem.Text = "2";
            TeachPos = "CONVEYOR X";
            btnCvyX.BackColor = Color.Chartreuse;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Transparent;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btncvyZ_Click(object sender, EventArgs e)
        {
            lblservo.Text = "1";
            lblitem.Text = "2";
            TeachPos = "CONVEYOR Z";
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Chartreuse;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Transparent;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btndspnX_Click(object sender, EventArgs e)
        {
            lblservo.Text = "0";
            lblitem.Text = "1";
            TeachPos = "DISPENSER X";
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Chartreuse;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Transparent;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btndspnZ_Click(object sender, EventArgs e)
        {
            lblservo.Text = "1";
            lblitem.Text = "1";
            TeachPos = "DISPENSER Z";
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Chartreuse;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Transparent;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btnStdbX_Click(object sender, EventArgs e)
        {
            lblservo.Text = "0";
            lblitem.Text = "0";
            TeachPos = "STANDBY X";
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Chartreuse;
            btnStdbZ.BackColor = Color.Transparent;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btnStdbZ_Click(object sender, EventArgs e)
        {
            lblservo.Text = "1";
            lblitem.Text = "0";
            TeachPos = "STANDBY Z";
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Chartreuse;
            gbConfig.Enabled = true;
            timerOn();
        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                Thread.Sleep(1500);
                controller.CtrlServoPort.MovingZaxis("ORIGIN");               
                controller.CtrlServoPort.MovingXaxis("ORIGIN");
            }
        }

        private void btnServoOn_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo )
            {
                //controller.
            }
        }

        private void btnResetAlarm_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                controller.CtrlServoPort.AlarmReset();
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                controller.CtrlServoPort.MovePosition(lblservo.Text, lblitem.Text);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            switch(TeachPos)
            {
                case "CONVEYOR X":
                    {
                        txtCmdCvyX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
                case "CONVEYOR Z":
                    {
                        txtCmdCvyZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
                case "DISPENSER X":
                    {
                        txtCmdDspnX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
                case "DISPENSER Z":
                    {
                        txtCmdDspnZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
                case "STANDBY X":
                    {
                        txtCmdStdbX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
                case "STANDBY Z":
                    {
                        txtCmdStdbZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                        break;
                    }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo )
            {
                switch (TeachPos)
                {
                    case "CONVEYOR X":
                        {
                            //txtCmdCvyX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdCvyX.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdCvyX.Text);
                            
                            break;
                        }
                    case "CONVEYOR Z":
                        {

                            //txtCmdCvyZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdCvyZ.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdCvyZ.Text);
                            
                            break;
                        }
                    case "DISPENSER X":
                        {

                            //txtCmdDspnX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdDspnX.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdDspnX.Text);
                            
                            break;
                        }
                    case "DISPENSER Z":
                        {
                            //txtCmdDspnZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdDspnZ.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdDspnZ.Text);
                            
                            break;
                        }
                    case "STANDBY X":
                        {
                            //txtCmdDspnX.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdDspnX.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdStdbX.Text);
                            break;
                        }
                    case "STANDBY Z":
                        {
                            //txtCmdStdbZ.Text = controller.CtrlServoPort.GetPos(lblservo.Text);
                            Inifiles.WriteValue(appPath + "/SERVO.INI", "SLAVE " + lblservo.Text, "ITEM" + lblitem.Text, txtCmdStdbZ.Text);
                            Thread.Sleep(1000);
                            controller.CtrlServoPort.SavePos(lblservo.Text, lblitem.Text, velocity, txtCmdStdbZ.Text);
                            break;
                        }
                }

                gbConfig.Enabled = false;
                btnCvyX.BackColor = Color.Transparent;
                btncvyZ.BackColor = Color.Transparent;
                btndspnX.BackColor = Color.Transparent;
                btndspnZ.BackColor = Color.Transparent;
                btnStdbX.BackColor = Color.Transparent;
                btnStdbZ.BackColor = Color.Transparent;
                ClearTeaching();
                try
                {
                    timelim.Abort();
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gbConfig.Enabled = false;
            btnCvyX.BackColor = Color.Transparent;
            btncvyZ.BackColor = Color.Transparent;
            btndspnX.BackColor = Color.Transparent;
            btndspnZ.BackColor = Color.Transparent;
            btnStdbX.BackColor = Color.Transparent;
            btnStdbZ.BackColor = Color.Transparent;
            ClearTeaching();
            try
            {
                timelim.Abort();
            }
            catch { }
        }

        private void ClearTeaching()
        {
            txtActCvyX.Text = "0";
            txtActCvyZ.Text = "0";
            txtActDspnX.Text = "0";
            txtActdspnZ.Text = "0";
            txtActStdbX.Text = "0";
            txtActStdbZ.Text = "0";
            txtCmdCvyX.Text = "0";
            txtCmdCvyZ.Text = "0";
            txtCmdDspnX.Text = "0";
            txtCmdDspnZ.Text = "0";
            txtCmdStdbX.Text = "0";
            txtCmdStdbZ.Text = "0";
            TeachPos = "";
        }

        public void realtime()
        {
            while (true)
            {
                if (controller.CtrlServoPort.statusServo)
                {
                    
                    switch (TeachPos)
                        {
                            case "CONVEYOR X":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActCvyX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActCvyX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    
                                    break;
                                }
                            case "CONVEYOR Z":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActCvyZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActCvyZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    break;
                                }
                            case "DISPENSER X":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActDspnX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActDspnX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    break;
                                }
                            case "DISPENSER Z":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActdspnZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActdspnZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    break;
                                }
                            case "STANDBY X":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActStdbX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActStdbX.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    break;
                                }
                            case "STANDBY Z":
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => txtActStdbZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text)));
                                    }
                                    else
                                    {
                                        txtActStdbZ.Text = controller.CtrlServoPort.GetActPos(lblservo.Text);
                                    }
                                    break;
                                }
                        }

                    
                        switch (Position)
                        {
                            case "ORIGIN X":
                                {
                                    
                                    break;
                                }
                            case "":
                                {
                                    break;
                                } 
                            default :{
                                break;
                            }
                        }
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

        Thread timeDryRun;
        private void RunDryRun()
        {
            timeDryRun = new Thread(() => controller.RunDry());
            timeDryRun.Start();
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.BackColor == Color.Chartreuse)
            {
                button3.BackColor = Color.Transparent;
                controller.isDryRunning = false;
            }else
            {
                button3.BackColor = Color.Chartreuse;
                controller.isDryRunning = true;
            }
        }

        #region Manual ..
        private void button8_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                //controller.CtrlServoPort.MovePosition("0", "2");
                controller.CtrlServoPort.Moving("ATTACH LABEL X");
            }
        }

        private void btnManCvyrX_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
               //controller.CtrlServoPort.MovePosition("0", "1");
               controller.CtrlServoPort.Moving("ATTACH SET X");
            }
        }

        private void btnManStbyX_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                //controller.CtrlServoPort.MovePosition("0", "0");
                controller.CtrlServoPort.Moving("STANDBY X");
            }
        }

        private void btnManCvyrZ_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                //controller.CtrlServoPort.MovePosition("1", "1");
                controller.CtrlServoPort.Moving("ATTACH SET Z");
            }
        }

        private void btnManDspnZ_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                //controller.CtrlServoPort.MovePosition("1", "2");
                controller.CtrlServoPort.Moving("ATTACH LABEL Z");
            }
        }

        private void btnManStbyZ_Click(object sender, EventArgs e)
        {
            if (controller.CtrlServoPort.statusServo)
            {
                //controller.CtrlServoPort.MovePosition("1", "0");
                controller.CtrlServoPort.Moving("STANDBY Z");
            }
        }

        private void btnManVacum_Click(object sender, EventArgs e)
        {
            if (btnManVacum.BackColor == Color.Chartreuse)
            {
                btnManVacum.BackColor = Color.White;
                //Vacum ON
                controller.CtrlIOPort.Output("T01");

            }
            else
            {
                btnManVacum.BackColor = Color.Chartreuse;
                //Vacum ON
                controller.CtrlIOPort.Output("T00");
            }
        }

        private void btnManSelP_Click(object sender, EventArgs e)
        {
            if (btnManSelP.BackColor == Color.Chartreuse)
            {
                btnManSelP.BackColor = Color.White;
                //Vacum ON
                controller.CtrlIOPort.Output("T10");

            }
            else
            {
                btnManSelP.BackColor = Color.Chartreuse;
                //Vacum ON
                controller.CtrlIOPort.Output("T11");
            }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (!controller.isDryRunning && page == "Auto" || page =="")
            {
                controller.CtrlIOPort.Output("R01\r\n");
                lbltime.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                if (controller.CtrlIOPort.Port1)
                {
                    btnVacum.BackColor = Color.Chartreuse;
                }else
                {
                    btnVacum.BackColor = Color.White;
                }

                if (controller.CtrlIOPort.Port3)
                {
                    btnGuider.BackColor = Color.Chartreuse;
                }
                else
                {
                    btnGuider.BackColor = Color.White;
                }

                if (controller.CtrlIOPort.Port4)
                {
                    btnDispenser.BackColor = Color.Chartreuse;
                }
                else
                {
                    btnDispenser.BackColor = Color.White;
                }

                if ( controller.CtrlIOPort.Port5)
                {
                    btnCvy.BackColor = Color.Chartreuse;
                }
                else
                {
                    btnCvy.BackColor = Color.White;
                }

                if (Position == "" && controller.CtrlIOPort.Port4 && !controller.CtrlIOPort.Port1)
                {
                    Position = "TAKE LABEL";
                }

                if (Position== "TAKE LABEL")
                {
                    //1. Label Ada
                    //2. Posisi  X Moving ke Dispenser 
                    if (!status && controller.CtrlIOPort.Port4 && !controller.CtrlIOPort.Port1)
                    {
                        controller.CtrlServoPort.Moving("ATTACH LABEL X");
                        status = true;
                    }
                    else
                    //3. Posisi Z Attach Moving ke Dispenser
                    if (status && controller.CtrlServoPort.GetPos("0") == DspnX)
                    {
                        controller.CtrlServoPort.Moving("ATTACH LABEL Z");
                    }

                    //4. On Vacum dan validasi Label
                    //5. Posisi Z Moving Ke Standby
                    if (status && controller.CtrlServoPort.GetPos("1") == DspnZ)
                    {
                        controller.CtrlIOPort.Output("T01");
                        Thread.Sleep(300);
                        if (controller.CtrlIOPort.Port1)
                        {
                            controller.CtrlServoPort.Moving("STANDBY Z"); 
                            Thread.Sleep(300);
                            if (controller.CtrlIOPort.Port1)
                            {                                
                                    btnReset_Click(0, null);
                                
                            }else
                            {
                                if (isRetry >= 3)
                                {
                                    controller.CtrlServoPort.Moving("STANDBY Z");
                                }
                                else
                                {
                                    isRetry++;
                                    controller.CtrlServoPort.Moving("ATTACH LABEL Z");
                                }
                            }
                        }
                    }
                }

                if (Position == "" && controller.CtrlIOPort.Port5 && controller.CtrlIOPort.Port1)
                {
                    Position = "ATTACH LABEL";
                }
                
                
                //12. Posisi Z Moving Standby
                //13. Posisi X Moving ke Dispenser
                if (Position == "ATTACH LABEL")
                {
                    //6. Validasi Set Conveyor
                    //7. Posisi X Moving ke SET Conveyor
                    if (!status && controller.CtrlIOPort.Port5 && controller.CtrlIOPort.Port1 && controller.CtrlServoPort.GetPos("0") == DspnX)
                    {
                        controller.CtrlServoPort.Moving("ATTACH SET X");
                        status = true;
                    }
                    else
                    //3. Posisi Z Attach Moving ke Dispenser
                    if (status && controller.CtrlServoPort.GetPos("0") == CvyrX)
                    {
                        controller.CtrlServoPort.Moving("ATTACH SET Z");
                    }

                    //8.Posisi Z Moving ke SET Conveyor
                    //9. On Guider Punch
                    //10. Validasi Punch
                    //11. Off Vacum
                    if (status && controller.CtrlServoPort.GetPos("1") == CvyrZ)
                    {
                        ////Punch On
                        controller.CtrlIOPort.Output("T11");
                        Thread.Sleep(500);
                        ////Punch OFF
                        controller.CtrlIOPort.Output("T10");

                        controller.CtrlIOPort.Output("T00");

                        if (!controller.CtrlIOPort.Port1)
                        {
                            controller.CtrlServoPort.Moving("STANDBY Z"); Position = "";
                            status = false;
                        }
                    }
                }

            }
            timer1.Start();
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            status = false;
            Position = "";
            isRetry = 0;
            controller.CtrlIOPort.Output("T01");
            controller.CtrlIOPort.Output("T10");
        }
    }
}
