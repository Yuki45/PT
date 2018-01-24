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


namespace Auto_Scanner_EPASS
{
    public partial class MainForm : Form
    {
        string msg, msg2;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        int LPT = 0x378;
        bool status = false;
        SerialPort scanner = new SerialPort();
        SerialPort gmes = new SerialPort();

        public MainForm()
        {
            InitializeComponent();
        }

        public void realtime(int address)
        {
            while (true)
            {
                
                int decdata = 0;
                decdata = IO.Inp32(0x379);
                int pinstatus = 0;
                pinstatus = IO.Inp32(0x379);
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

                    
                    while ((pinstatus |= address) == address)
                    {
                        pinstatus = IO.Inp32(0x379);
                        int coba = pinstatus |= address;
                    }

                    try
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => Clear()));
                        }
                        else
                        {
                            Clear();
                        }
                    }
                    catch(Exception e)
                    {
                        #region create log...
                        try
                        {
                            if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write( DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") +":"+e.ToString()+ "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + e.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch { }
                    
                        #endregion
                    }
                    try
                    {
                        scanner.Write("L");
                    }
                    catch (Exception e)
                    {
                        #region create log...
                        try
                        {
                            if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + e.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + e.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch { }

                        #endregion
                    }


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
            timelim = new Thread(() => realtime(110));
            timelim.Start();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void config2()
        {
            
            try
            {
                scanner.Close();
                scanner.PortName = txtScanner.Text;
                scanner.BaudRate = 9600;
                scanner.DataBits = 8;
                scanner.Parity = Parity.None;
                scanner.StopBits = StopBits.One;
                scanner.Open();

                this.scanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
                txtScanner.BackColor = Color.Chartreuse;
            }
            catch
            {
                txtScanner.BackColor = Color.Red;
            }

            try
            {
                gmes.Close();
                gmes.PortName = txtGmes.Text;
                gmes.BaudRate = 9600;
                gmes.DataBits = 8;
                gmes.Parity = Parity.None;
                gmes.StopBits = StopBits.One;
                gmes.Open();
                this.gmes.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.GMES_DataReceived);
                txtGmes.BackColor = Color.Chartreuse;
            }
            catch
            {
                txtGmes.BackColor = Color.Red;
            }

           
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = scanner.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    
                    if (nmBarcode.Value == 3)
                    {
                        string[] row = msg.Split(',');
                        if (row.Length == 3)
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtsn1.Text = row[0].ToString()));
                            }
                            else
                            {
                                txtsn1.Text = row[0].ToString();
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtsn2.Text = row[1].ToString()));
                            }
                            else
                            {
                                txtsn2.Text = row[1].ToString();
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtsn3.Text = row[2].ToString()));
                            }
                            else
                            {
                                txtsn3.Text = row[1].ToString();
                            }
                            getSendSN(txtsn1.Text, txtsn2.Text, txtsn3.Text);


                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Qty Wrong Spec"));
                            }
                            else
                            {
                                lblmsg.Text = "Barcode Qty Wrong Spec";
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                            }
                            else
                            {
                                lblmsg.ForeColor = Color.Red;
                            }
                        }

                    }
                    else
                    if (nmBarcode.Value == 2)
                    {
                        string[] row = msg.Split(',');
                        if (row.Length == 2)
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtsn1.Text = row[0].ToString()));
                            }
                            else{
                                txtsn1.Text = row[0].ToString();
                             }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => txtsn2.Text = row[1].ToString()));
                            }
                            else
                            {
                                txtsn2.Text = row[1].ToString();
                            }
                            getSendSN(txtsn1.Text, txtsn2.Text, txtsn3.Text);
                            
                            
                        }
                        else
                        {

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Qty Wrong Spec"));
                            }
                            else
                            {
                                lblmsg.Text = "Barcode Qty Wrong Spec";
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                            }
                            else
                            {
                                lblmsg.ForeColor = Color.Red;
                            }
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => txtsn1.Text = msg));
                        }
                        else
                        {
                            txtsn1.Text = msg;
                        }
                        getSendSN(txtsn1.Text, txtsn2.Text, txtsn3.Text);
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

        private void GMES_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = gmes.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {

                    if (msg2 == "OK")
                    {
                        if (!chkSimulation.Checked)
                        SignalOK();

                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Sending Complete..!"));
                        }
                        else
                        {
                            lblmsg.Text = "Barcode Sending Complete..!";
                        }

                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Green));
                        }
                        else
                        {
                            lblmsg.ForeColor = Color.Green;
                        }
                    }
                    else
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => SignalNG()));
                        }
                        else
                        {
                            SignalNG();
                        }

                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Sending NG..!"));
                        }
                        else
                        {
                            lblmsg.Text = "Barcode Sending NG..!";
                        }

                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                        }
                        else
                        {
                            lblmsg.ForeColor = Color.Red;
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            config2();
            timerOn();
            nmBarcode.Value = 2;

        }

        private void nmBarcode_ValueChanged(object sender, EventArgs e)
        {
            if (nmBarcode.Value == 3)
            {
                lblsn1.Visible = true;
                lblsn2.Visible = true;
                lblsn3.Visible = true;

                txtsn1.Visible = true;
                txtsn2.Visible = true;
                txtsn3.Visible = true;

            }else
            if (nmBarcode.Value == 2)
            {
                lblsn1.Visible = true;
                lblsn2.Visible = true;
                lblsn3.Visible = false;

                txtsn1.Visible = true;
                txtsn2.Visible = true;
                txtsn3.Visible = false;
            }
            else
            {
                lblsn1.Visible = true;
                lblsn2.Visible = false;
                lblsn3.Visible = false;

                txtsn1.Visible = true;
                txtsn2.Visible = false;
                txtsn3.Visible = false;
            }
        }

        private void SignalOK()
        {
            int decdata = 0;
            decdata = IO.Inp32(LPT);
            decdata |= 2;
            IO.Output(LPT, decdata);
            Thread.Sleep(1000);

            decdata &= ~2;
            IO.Output(LPT, decdata);
            //status = false;
            
        }

        private void SignalNG()
        {
            int decdata = 0;
            decdata = IO.Inp32(LPT);
            decdata |= 4;
            IO.Output(LPT, decdata);
            Thread.Sleep(1500);

            decdata &= ~4;
            IO.Output(LPT, decdata);
                        
        }

        private void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => txtsn1.Text = ""));
            }
            else
            {
                txtsn1.Text = "";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => txtsn2.Text = ""));
            }
            else
            {
                txtsn2.Text = "";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => txtsn3.Text = ""));
            }
            else
            {
                txtsn3.Text = "";
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timelim.Abort();
        }

        private void txtsn1_TextChanged(object sender, EventArgs e)
        {
            if (txtsn1.Text.Length > 5)
            {
                if (nmBarcode.Value == 3)
                {
                    if (txtsn1.Text != "" && txtsn2.Text != "" && txtsn3.Text != "")
                    {
                        try
                        {
                            gmes.Write((char)2+txtsn1.Text+(char)3);
                            Thread.Sleep(200);
                            gmes.Write((char)2 + txtsn2.Text + (char)3);
                            Thread.Sleep(200);
                            gmes.Write((char)2 + txtsn3.Text + (char)3);
                        }
                        catch (Exception ex)
                        {
                            #region create log...
                            try
                            {
                                if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                                else
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                            }
                            catch { }

                            #endregion
                        }
                    }
                }
                else
                    if (nmBarcode.Value == 2)
                    {
                        if (txtsn1.Text != "" && txtsn2.Text != "")
                        {
                            try
                            {
                                gmes.Write((char)2 + txtsn1.Text + (char)3);
                                Thread.Sleep(200);
                                gmes.Write((char)2 + txtsn2.Text + (char)3);
                            }
                            catch (Exception ex)
                            {
                                #region create log...
                                try
                                {
                                    if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                                    {
                                        using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                        {
                                            StreamWriter sw = new StreamWriter(fs);
                                            sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                            sw.Close();
                                            fs.Close();
                                        }
                                    }
                                    else
                                    {
                                        using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                        {
                                            StreamWriter sw = new StreamWriter(fs);
                                            sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                            sw.Close();
                                            fs.Close();
                                        }
                                    }
                                }
                                catch { }

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            gmes.Write((char)2 + txtsn1.Text + (char)3);
                        }
                        catch (Exception ex)
                        {
                            #region create log...
                            try
                            {
                                if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                                else
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                            }
                            catch { }

                            #endregion
                        }
                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                gmes.Write((char)2 + "test1" + (char)3);
            }
            catch { }
        }

        private void txtsn1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                gmes.Write((char)2+txtsn1.Text+(char)3);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Config")
            {

                button1.Text = "Save";
            }
            else
            {
                config2();
                button1.Text = "Config";
            }
        }

        private void txtsn2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void getSendSN(string sn1, string sn2, string sn3)
        {
            
            if (nmBarcode.Value == 3)
            {
                if (txtsn1.Text != "" && txtsn2.Text != "" && txtsn3.Text != "")
                {
                    try
                    {
                        gmes.Write((char)2 + txtsn1.Text + (char)3);
                        Thread.Sleep(200);
                        gmes.Write((char)2 + txtsn2.Text + (char)3);
                        Thread.Sleep(200);
                        gmes.Write((char)2 + txtsn3.Text + (char)3);
                    }
                    catch (Exception ex)
                    {
                        #region create log...
                        try
                        {
                            if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch { }

                        #endregion
                    }
                }
            }
            else
                if (nmBarcode.Value == 2)
                {
                    if (txtsn1.Text != "" && txtsn2.Text != "")
                    {
                        try
                        {
                            if (txtsn1.Text.Length == 9)
                            {
                                if (txtsn1.Text == "NOREAD" || txtsn2.Text == "NOREAD")
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Not Scan"));
                                    }
                                    else
                                    {
                                        lblmsg.Text = "Barcode Not Scan";
                                    }

                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                                    }
                                    else
                                    {
                                        lblmsg.ForeColor = Color.Red;
                                    }
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => SignalNG()));
                                    }
                                    else
                                    {
                                        SignalNG();
                                    }

                                    if (chkSimulation.Checked)
                                    {
                                        //gmes.Write((char)2 + txtsn1.Text + (char)3);
                                        Thread.Sleep(300);
                                       // gmes.Write((char)2 + txtsn2.Text + (char)3);
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        gmes.Write((char)2 + txtsn1.Text + (char)3);
                                        Thread.Sleep(1900);
                                        gmes.Write((char)2 + txtsn2.Text + (char)3);
                                        Thread.Sleep(1100);
                                        
                                    }
                                    catch { }

                                    if (chkSimulation.Checked)
                                    {
                                        SignalOK();
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode OK Scan"));
                                        }
                                        else
                                        {
                                            lblmsg.Text = "Barcode OK Scan";
                                        }

                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Green));
                                        }
                                        else
                                        {
                                            lblmsg.ForeColor = Color.Green;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (txtsn1.Text == "NOREAD" || txtsn2.Text == "NOREAD")
                                {
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Not Scan"));
                                    }
                                    else
                                    {
                                        lblmsg.Text = "Barcode Not Scan";
                                    }

                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                                    }
                                    else
                                    {
                                        lblmsg.ForeColor = Color.Red;
                                    }
                                    if (InvokeRequired)
                                    {
                                        Invoke(new MethodInvoker(() => SignalNG()));
                                    }
                                    else
                                    {
                                        SignalNG();
                                    }

                                    
                                }
                                else
                                {
                                    try
                                    {
                                        gmes.Write((char)2 + txtsn2.Text + (char)3);
                                        Thread.Sleep(800);
                                        gmes.Write((char)2 + txtsn1.Text + (char)3);
                                    }
                                    catch { }

                                    if (chkSimulation.Checked)
                                    {
                                        SignalOK();
                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode OK Scan"));
                                        }
                                        else
                                        {
                                            lblmsg.Text = "Barcode OK Scan";
                                        }

                                        if (InvokeRequired)
                                        {
                                            Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Green));
                                        }
                                        else
                                        {
                                            lblmsg.ForeColor = Color.Green;
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            #region create log...
                            try
                            {
                                if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                                else
                                {
                                    using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                    {
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                        sw.Close();
                                        fs.Close();
                                    }
                                }
                            }
                            catch { }

                            #endregion
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (txtsn1.Text == "NOREAD")
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.Text = "Barcode Not Scan"));
                            }
                            else
                            {
                                lblmsg.Text = "Barcode Not Scan";
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblmsg.ForeColor = Color.Red));
                            }
                            else
                            {
                                lblmsg.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            gmes.Write((char)2 + txtsn1.Text + (char)3);
                        }
                    }
                    catch (Exception ex)
                    {
                        #region create log...
                        try
                        {
                            if (System.IO.File.Exists(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log"))
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.Append))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else
                            {
                                using (FileStream fs = new FileStream(appPath + "/log/" + DateTime.Now.ToString("yyyy-MM-dd") + "DataError.log", FileMode.OpenOrCreate))
                                {
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.Write(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + ex.ToString() + "\r\n");
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch { }

                        #endregion
                    }
                }
            
        }

        private void lblTriger_DoubleClick(object sender, EventArgs e)
        {
            SignalOK();
        }

    }
}
