using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Auto_Scanner.DB;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Auto_Scanner
{
    public partial class Main : Form
    {
        string msg, ws, msg2;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        string SN = "" ;
        bool scanner = false;
        bool status = false;
        DB.DB db = new DB.DB();
        bool config = false;
        bool SignalWS = false;
        private Fconfig uxChildForm;
        double tolerance, usl, lsl,devide;
        int LPT = 0xC010;//0x378;
        private void openconfig()
        {
            tolerance = Convert.ToDouble((Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE") == "") ? "1" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE"));
            devide = Convert.ToDouble((Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE") == "") ? "1000" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE"));
            
            usl = Convert.ToDouble((Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL") == "") ? "0" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL"));
            lsl = Convert.ToDouble((Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL") == "") ? "0" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL"));
        }

        private Double cDoubles(string value)
        {
            if (value.Substring(0, 4) == "0000")
            {
                return Convert.ToDouble(value.Substring(3, 5).Replace('.', ','));
            }
            else if (value.Substring(0, 3) == "000")
            {
                return Convert.ToDouble(value.Substring(2, 6).Replace('.', ','));
            }
            else if (value.Substring(0, 2) == "00")
            {
                return Convert.ToDouble(value.Substring(1, 7).Replace('.', ','));
            }
            else
            {
                return Convert.ToDouble(value.Substring(0, 8).Replace('.', ','));
            }

        }

        private bool wsCheck()
        {
            bool result=false;
            double tl = tolerance / devide;
            double ht1 = cDoubles(txtWeightScale.Text.Substring(4,8));
            double ht2 = cDoubles(txtWeightScale.Text.Substring(4, 8));
           bool t1 =  ht1 <= (usl + tl);
           bool  t2 = ht2 >= (lsl - tl);
            if ( t1 && t2)
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => txtWeightScale.BackColor = Color.Green));
                }
                else
                {
                    txtWeightScale.BackColor = Color.Green;
                }

                /*if (count > 15)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtWeightScale.Text = ""));
                    }
                    else
                    {
                        txtWeightScale.Text = "";
                    }
                    count = 0;
                }
                else count++;*/

                result = true;

            }
            else
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => txtWeightScale.BackColor = Color.Red));
                }
                else
                {
                    txtWeightScale.BackColor = Color.Red;
                }
                status = false;
               /* if (count > 15)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtWeightScale.Text = ""));
                    }
                    else
                    {
                        txtWeightScale.Text = "";
                    }
                    count = 0;
                }
                else count++;*/

                if (!t1)
                {
                    if (InvokeRequired)
                    {
                       // Invoke(new MethodInvoker(() => lblmsg.Text = "Weight Scale Too High"));
                    }
                    else
                    {
                       // lblmsg.Text = "Weight Scale Too High";
                    }
                } else
                    if (!t2)
                    {
                        if (InvokeRequired)
                        {
                          //  Invoke(new MethodInvoker(() => lblmsg.Text = "Weight Scale Too Low"));
                        }
                        else
                        {
                           // lblmsg.Text = "Weight Scale Too Low";
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                           // Invoke(new MethodInvoker(() => lblmsg.Text = ""));
                        }
                        else
                        {
                           // lblmsg.Text = "";
                        }
                    }
            }
            return result;
        }

        public void realtime(int address)
        {
            while (true)
            {
                if (status)
                {
                    //SignalOK();
                   // status = false;
                    goto lompat;
                }

                int decdata = 0;
                decdata = IO.Inp32(LPT);
                int pinstatus = 0;
                pinstatus = IO.Inp32(LPT);
                pinstatus |= address;
                if (pinstatus == address)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblTriger.BackColor = Color.Green));
                    }
                    else
                    {
                        lblTriger.BackColor = Color.Green;
                    }

                    /*
                    while ((pinstatus |= address) == address)
                   {
                        pinstatus = IO.Inp32(LPT);
                        int coba = pinstatus |= address;
                    }
                    */
                    #region create log...
                    /*try
                    {
                        if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "Data.log"))
                        {
                            using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "Data.log", FileMode.Append))
                            {
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write("Start Scan " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
                                sw.Close();
                                fs.Close();
                            }
                        }
                        else
                        {
                            using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "Data.log", FileMode.OpenOrCreate))
                            {
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write("Start Scan " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
                                sw.Close();
                                fs.Close();
                            }
                        }
                    }
                    catch { }
                    */ 
                     #endregion
                    
                        
                }
                else
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblTriger.BackColor = Color.Red));
                    }
                    else
                    {
                        lblTriger.BackColor = Color.Red;
                    }

                   /* decdata &= ~2;
                    IO.Output(0x378, decdata);
                    */
                    

                    goto lompat;
                }
            lompat: ;
                Thread.Sleep(100);
            }
        }

        Thread timelim;
        private void timerOn()
        {
            timelim = new Thread(() => realtime(94));
            timelim.Start();
        }

        private void SignalOK()
        {
            int decdata=0;
            decdata = IO.Inp32(LPT);
            decdata |= 1;
            IO.Output(LPT, decdata);
            Thread.Sleep(1000);

            decdata &= ~1;
            IO.Output(LPT, decdata);
            //status = false;
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => lblstatus.Text = "OK"));
            }
            else
            {
                lblstatus.Text = "OK";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => lblstatus.BackColor = Color.Lime));
            }
            else
            {
                lblstatus.BackColor = Color.Lime;
            }

        }
        
        private void SignalNG()
        {
            int decdata = 0;
            decdata = IO.Inp32(LPT);

            decdata &= ~1;
          //  IO.Output(0x378, decdata);
            
            
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => lblstatus.Text = "NG"));
            }
            else
            {
                lblstatus.Text = "NG";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => lblstatus.BackColor = Color.Red));
            }
            else
            {
                lblstatus.BackColor = Color.Red;
            }


        }

        private void SignalScan()
        {
            int decdata = 0;
            decdata = IO.Inp32(LPT);
            decdata |= 2;
            IO.Output(LPT, decdata);
            Thread.Sleep(3000);
            decdata &= ~2;
            IO.Output(0x378, decdata);
            
        }

        Thread timelim2;
        private void scanOn()
        {
            timelim = new Thread(() => SignalScan());
            timelim.Start();
        }

        public Main()
        {
            InitializeComponent();
            
        }

        int scan = 1;

        public bool AddData(string SN, string value)
        {
            bool hasil = false;
          hasil = db.setData2("INSERT INTO [dbo].[WEIGHT_SCALE_HSTY]    ([WG_SN],[WG_VALUE])  VALUES ('"+SN+"','"+value+"') " );
            
            return hasil;
        }

        private void SPWS_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = SPWS.ReadExisting();
            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                        try
                        {
                            
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() => txtWeightScale.Text = ws));
                                }
                                else
                                {
                                    txtWeightScale.Text = ws;
                                }

                                int decdata = 0;
                                bool result = false;
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() => result = wsCheck()));
                                }
                                else
                                {
                                    result = wsCheck();
                                }
                                if (txtWeightScale.BackColor != Color.Red)
                                {
                                    if (count >= 20)
                                    {
                                        decdata |= 2;
                                        IO.Output(LPT, decdata);
                                        count = 0;  

                                        if (txtSn.Text != "" )
                                        {

                                            
                                            if (InvokeRequired)
                                            {
                                                Invoke(new MethodInvoker(() => sendSN(txtSn.Text)));
                                            }
                                            else
                                            {
                                                sendSN(txtSn.Text);
                                            }
                                              
                                        }
                                        if (status)
                                        {
                                            SignalOK();
                                        }
                                        
                                    }
                                    else count++;


                                    if (InvokeRequired)
                                    {
                                        //Invoke(new MethodInvoker(() => lblmsg.Text = status.ToString()));
                                    }
                                    else
                                    {
                                        //lblmsg.Text = status.ToString();
                                    }
                                }
                                else
                                {
                                    //status = false;
                                    decdata &= ~2;
                                    IO.Output(LPT, decdata);
                                }
                        }
                        catch { return; }
                    
                    ws = "";
                }
                else if (data[a] == (char)2)
                {
                    ws = "";
                }
                else
                {
                    if (data[a].ToString() != "\r")
                        ws = ws + data[a];
                }
            }
        }

        private void SPScanner_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = SPScanner.ReadExisting();
            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    if (msg.Length == nmLenght.Value)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => txtSn.Text = msg));
                        }
                        else
                        {
                            txtSn.Text = msg;
                        }

                       
                        /*
                        try
                        {
                            

                            if (chkWG.Checked)
                            {
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() => AddData(msg, txtWeightScale.Text)));
                                }
                                else
                                {
                                    AddData(msg, txtWeightScale.Text);
                                }

                                if (msg != txtSn.Text)
                                {
                                    SPGMES.Write((char)2 + msg + (char)3);
                                    
                                }
                                else
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblmsg.Text = "Weight Scale Result Not Spec..!"));
                                    }
                                    else
                                    {
                                        lblmsg.Text = "Weight Scale Result Not Spec..!";
                                    }
                                }

                            }
                            else if (msg != txtSn.Text)
                            {
                                SPGMES.Write((char)2 + msg + (char)3);
                                if (InvokeRequired)
                                {
                                    Invoke(new MethodInvoker(() => txtSn.Text = msg));
                                }
                                else
                                {
                                    txtSn.Text = msg;
                                }
                            }

                            if (!chkFP.Checked)
                            {
                                if (Convert.ToInt32(lblcount.Text) < nmSize.Value)
                                {
                                    if (!chkFP.Checked) SignalOK();

                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblcount.Text = Convert.ToString(Convert.ToInt32(lblcount.Text) + 1)));
                                    }
                                    else
                                    {
                                        lblcount.Text = Convert.ToString(Convert.ToInt32(lblcount.Text) + 1);
                                    }
                                }
                                else
                                {
                                    if (!chkFP.Checked) SignalOK();

                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblcount.Text = "1"));
                                    }
                                    else
                                    {
                                        lblcount.Text = "1";
                                    }
                                }
                            }
                            
                            

                            status = true;
                        }
                        catch { return; }*/
                    }
                    else
                    {
                        if (scan >= 3)
                        {
                            SignalNG();
                            scan = 1;
                            goto lompat;
                        }
                        int decdata;
                        decdata = IO.Inp32(0x378);

                        decdata &= ~2;
                        IO.Output(0x378, decdata);
                        Thread.Sleep(500);
                        decdata |= 2;
                        IO.Output(0x378, decdata);

                        scan++;
                    }
                lompat:
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

        private void cmbscanner_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SPScanner.Close();
                SPScanner.PortName = cmbscanner.Text;
                SPScanner.Open();
                lblInScanner.BackColor = Color.Lime;
            }
            catch
            {
                lblInScanner.BackColor = Color.Red;
                MessageBox.Show("Can't Open Comm Port" + cmbscanner.Text);
            }
            txtSn.Focus();
        }

        private void cmbgmes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SPGMES.Close();
                SPGMES.Close();
                SPGMES.PortName = cmbgmes.Text;
                SPGMES.Open();
                lblInGmes.BackColor = Color.Lime;
            }
            catch
            {
                lblInGmes.BackColor = Color.Red;
                MessageBox.Show("Can't Open Comm Port" + cmbgmes.Text);
            }
            txtSn.Focus();
        }

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

        private void lblStatus(string s)
        {
            if (s == "NG\r\n")
            {
                lblstatus.BackColor = Color.Red;
                lblstatus.Text = s.Substring(0,2);
                lblstatus.TextAlign = ContentAlignment.MiddleCenter;
            }
            else if (s == "OK")
            {
                lblstatus.BackColor = Color.Lime;
                lblstatus.Text = s.Substring(0, 2);
                lblstatus.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void SPGMES_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = SPGMES.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblStatus(msg2)));
                    }
                    else
                    {
                        lblStatus(data);
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblmsg.Text = msg2));
                    }
                    else
                    {
                        lblmsg.Text = data;
                    }
                    if (chkFP.Checked && msg2.Length > 0)
                    {
                        if (msg2.Substring(0, 2) != "NG")
                        {
                            
                            if (Convert.ToInt32(lblcount.Text) < nmSize.Value)
                            {
                                SignalOK();
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                            SignalNG();
                        }
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

        private bool checkDigit(string sn)
        {
            
            var specialChar = "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]-@#!$%^&*()<>/?{}";

            for (int i=0; i < sn.Length; i++)
            {
                for (int a = 0; a < specialChar.Length; a++)
                {
                    if (sn[i] == specialChar[a])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void txtSn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)13)
            {
                //SPGMES.Write((char)2+txtSn.Text+(char)3);
                if (checkDigit(txtSn.Text))
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show("NG");
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            int decdata;
            option(false);
           /* Process[] processes = Process.GetProcessesByName("VSPEmulator");

            if (processes.Length <= 0 && File.Exists(appPath + "/auto scanner.vspe"))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = appPath + "/auto scanner.vspe";
                proc.StartInfo.UseShellExecute = true;
               // proc.Start();
                Thread.Sleep(4000);
            }

            */

            foreach (string com in SerialPort.GetPortNames())
            {
                cmbscanner.Items.Add(com);
                cmbgmes.Items.Add(com);
                cmbWS.Items.Add(com);
            }

            lblInGmes.BackColor = Color.Gray;
            lblInScanner.BackColor = Color.Gray;
            lblWS.BackColor = Color.Gray;

            openconfig();
            decdata = 0;
            IO.Output(0x378, decdata);
            timerOn();
            txtLPT.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LPT") == "") ? "0x378" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LPT");
            LPT = (int)new System.ComponentModel.Int32Converter().ConvertFromString(txtLPT.Text);

            chkFP.Checked = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "FOOLPROOF") == "") ? true : Convert.ToBoolean(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "FOOLPROOF"));
            chkWG.Checked = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "WEIGHT SCALE") == "") ? true : Convert.ToBoolean(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "WEIGHT SCALE"));
            nmLenght.Value = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LENGHT")=="") ? 0: Convert.ToInt32(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LENGHT"));
            nmSize.Value = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "SIZE") == "") ? 20 : Convert.ToInt32(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "SIZE"));
            cmbgmes.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM GMES");
            cmbscanner.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM SCANNER");
            cmbWS.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM WEIGHT SCALE");
           // LPT = Convert.ToInt32((Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LPT PORT") == "") ? "0x378" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LPT PORT"));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*Flogin login = new Flogin("WEIGHT SCALE UNCHECK");
            DialogResult db = login.ShowDialog();
            if (db.ToString() != "OK")
            {
                //e.Cancel = true;
            }*/
            
            timelim.Abort(); //timelim2.Abort();
            //Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "CARD", radioButton1.Checked.ToString());
           // Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "BOARD", radioButton2.Checked.ToString());
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "FOOLPROOF", chkFP.Checked.ToString());
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "WEIGHT SCALE", chkWG.Checked.ToString());
            
            byte[] inthex = BitConverter.GetBytes(LPT);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "LPT", txtLPT.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "LENGHT", nmLenght.Value.ToString());
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "SIZE", nmSize.Value.ToString());
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM GMES", cmbgmes.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM SCANNER", cmbscanner.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM WEIGHT SCALE", cmbWS.Text);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch { }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            int decdata = 0;
            decdata = IO.Inp32(0x378);
            decdata |= 1;
        }

        private void lblTriger_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SignalOK();
            lblcount.Text = "0";
            txtSn.Text = "";
            txtWeightScale.Text = "";
        }

        private void cmbWS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SPWS.Close();
                SPWS.PortName = cmbWS.Text;
                SPWS.Open();
                lblWS.BackColor = Color.Lime;
            }
            catch
            {
                lblWS.BackColor = Color.Red;
                MessageBox.Show("Can't Open Comm Port" + cmbWS.Text);
            }
            txtSn.Focus();
        }
               
        private void button2_Click(object sender, EventArgs e)
        {
            string parentValue = this.txtWeightScale.Text;
            uxChildForm = new Fconfig(parentValue);
            DialogResult form = uxChildForm.ShowDialog();
            if (form.ToString() == "Yes")
            {
                openconfig();
            }
        }

        int count = 0;
        private void txtWeightScale_TextChanged(object sender, EventArgs e)
        {
            if (this.uxChildForm != null)
            {
               this.uxChildForm.ValueFromParent = this.txtWeightScale.Text;
            }

            
        }

        private void chkWG_Click(object sender, EventArgs e)
        {
            Flogin login = new Flogin("WEIGHT SCALE UNCHECK");
            DialogResult db = login.ShowDialog();
            if (db.ToString() == "OK")
            {
                
            }
            else
            {
                if (!chkWG.Checked) chkWG.Checked = true;
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.M )
            {
                Application.Exit();
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int decdata;
            SPScanner.Close();
            SPGMES.Close();
            SPWS.Close();
            cmbscanner.Items.Clear();
            cmbgmes.Items.Clear();
            cmbWS.Items.Clear();
            foreach (string com in SerialPort.GetPortNames())
            {
                cmbscanner.Items.Add(com);
                cmbgmes.Items.Add(com);
                cmbWS.Items.Add(com);
            }

            lblInGmes.BackColor = Color.Gray;
            lblInScanner.BackColor = Color.Gray;
            lblWS.BackColor = Color.Gray;

            openconfig();
            decdata = 0;
            IO.Output(0x378, decdata);
            timerOn();
            chkFP.Checked = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "FOOLPROOF") == "") ? true : Convert.ToBoolean(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "FOOLPROOF"));
            chkWG.Checked = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "WEIGHT SCALE") == "") ? true : Convert.ToBoolean(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "WEIGHT SCALE"));
            nmLenght.Value = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LENGHT") == "") ? 0 : Convert.ToInt32(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "LENGHT"));
            nmSize.Value = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "SIZE") == "") ? 20 : Convert.ToInt32(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "SIZE"));
            cmbgmes.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM GMES");
            cmbscanner.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM SCANNER");
            cmbWS.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM WEIGHT SCALE");
        }

        private void loadVSPEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("VSPEmulator");

            if (processes.Length <= 0 && File.Exists(appPath + "/auto scanner.vspe"))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = appPath + "/auto scanner.vspe";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
                Thread.Sleep(2000);
            }

        }
        string tempsn = "";

        private bool ValidSN(string sn)
        {
            for (int i = 0; i <= sn.Length; i++)
            {

            }

            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

            if (regexItem.IsMatch(sn))
            { 
                return false; 
            }
            else
            {
                return true;
            }
        }

        private void sendSN(string SN)
        {
            if (tempsn != SN)
            {
                if (checkDigit(SN))
                {
                    try
                    {
                        AddData(SN, txtWeightScale.Text);
                        SPGMES.Write((char)2 + SN + (char)3);
                        tempsn = txtSn.Text;
                        txtSn.Text = "";
                    }
                    catch { lblmsg.Text = tempsn + " ERROR "; }

                    finally
                    {
                        if (!chkFP.Checked)
                        {
                            status = true;
                            lblmsg.Text = tempsn + " OK ";
                            SignalOK();
                        }
                    }
                }
                else
                {
                    lblmsg.Text = SN + " : ERROR SPECIAL CHARACTER";
                    txtSn.Text = "";
                }
            }
        }

        private void txtSn_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void option(bool status)
        {
            cmbgmes.Enabled = status;
            cmbscanner.Enabled = status;
            cmbWS.Enabled = status;
            nmLenght.Enabled = status;
            nmSize.Enabled = status;
            txtLPT.Enabled = status;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Config")
            {
                option(true);
                button3.Text = "Save";
                timelim.Abort();
            }
            else
            {
                option(false);
                button3.Text = "Config";
                LPT = (int)new System.ComponentModel.Int32Converter().ConvertFromString(txtLPT.Text);
                timerOn();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
           // LPT = (radioButton1.Checked) ? 0xBD00 : 0x378;

        }

    }
}
