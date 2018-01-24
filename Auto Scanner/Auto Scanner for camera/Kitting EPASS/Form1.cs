using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Threading;

namespace Kitting_EPASS
{
    public partial class Form1 : Form
    {
        string msg, msg2,msg3;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        DB.DB db = new DB.DB();
        SerialPort SP1 = new SerialPort();
        SerialPort SP2 = new SerialPort();
        SerialPort SP3 = new SerialPort();

        SerialPort SP1A = new SerialPort();
        SerialPort SP2A = new SerialPort();
        SerialPort SP3A = new SerialPort();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable tbl = db.getData("SELECT DISTINCT([ITEM_MODEL])  FROM [PROD].[dbo].[TBL_MASTER_EPASS]");
            cmbModel.Items.Clear();
            cmbModel.Items.Add("");
            lockSpec(false);
            loadConfig();
            foreach (DataRow row in tbl.Rows)
            {
                cmbModel.Items.Add(row[0].ToString());
            }

            cmbModel.SelectedItem = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "MODEL NAME");

            cmbItem1.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 1");
            cmbItem2.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 2");
            cmbItem3.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 3");
            cmbItem4.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 4");
            cmbItem5.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 5");
            cmbItem6.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 6");
            cmbItem7.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 7");
            cmbItem8.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 8");
            cmbItem9.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 9");
            cmbItem10.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 10");

            comboBox1.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 1");
            comboBox2.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 2");
            comboBox3.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 3");
            comboBox4.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 4");
            comboBox5.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 5");
            comboBox6.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 6");
            comboBox7.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 7");
            comboBox8.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 8");
            comboBox9.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 9");
            comboBox10.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 10");

        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbModel.Text != "")
            {
                DataTable tbl = db.getData("SELECT * FROM [PROD].[dbo].[TBL_MASTER_EPASS] Where ITEM_MODEL ='"+ cmbModel.Text +"'");
                cmbItem1.Items.Clear();
                cmbItem2.Items.Clear();
                cmbItem3.Items.Clear();
                cmbItem4.Items.Clear();
                cmbItem5.Items.Clear();
                cmbItem6.Items.Clear();
                cmbItem7.Items.Clear();
                cmbItem8.Items.Clear();
                cmbItem9.Items.Clear();
                cmbItem10.Items.Clear();
                cmbItem1.Items.Add("");
                cmbItem2.Items.Add("");
                cmbItem3.Items.Add("");
                cmbItem4.Items.Add("");
                cmbItem5.Items.Add("");
                cmbItem6.Items.Add("");
                cmbItem7.Items.Add("");
                cmbItem8.Items.Add("");
                cmbItem9.Items.Add("");
                cmbItem10.Items.Add("");

                /*cmbItem1.Items.Add("KITTING SN");
                cmbItem2.Items.Add("KITTING SN");
                cmbItem3.Items.Add("KITTING SN");
                cmbItem4.Items.Add("KITTING SN");
                cmbItem5.Items.Add("KITTING SN");
                cmbItem6.Items.Add("KITTING SN");
                cmbItem7.Items.Add("KITTING SN");
                cmbItem8.Items.Add("KITTING SN");
                cmbItem9.Items.Add("KITTING SN");
                cmbItem10.Items.Add("KITTING SN");*/

                foreach (DataRow row in tbl.Rows)
                {
                    cmbItem1.Items.Add(row[2].ToString());
                    cmbItem2.Items.Add(row[2].ToString());
                    cmbItem3.Items.Add(row[2].ToString());
                    cmbItem4.Items.Add(row[2].ToString());
                    cmbItem5.Items.Add(row[2].ToString());
                    cmbItem6.Items.Add(row[2].ToString());
                    cmbItem7.Items.Add(row[2].ToString());
                    cmbItem8.Items.Add(row[2].ToString());
                    cmbItem9.Items.Add(row[2].ToString());
                    cmbItem10.Items.Add(row[2].ToString());
                }
            }
        }
        private void loadConfig()
        {
              try
               {
                   SP1.Close();
                   SP1.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1"); 
                   SP1.Open();
                   this.SP1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SP1_DataReceived);

                   SP2.Close();
                   SP2.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2"); 
                   SP2.Open();
                   this.SP2.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SP2_DataReceived);

                   SP3.Close();
                   SP3.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3"); 
                   SP3.Open();
                   this.SP3.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SP3_DataReceived);


                   SP1A.Close();
                   SP1A.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1 SENDER") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1 SENDER");
                   SP1A.Open();

                   SP2A.Close();
                   SP2A.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2 SENDER") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2 SENDER");
                   SP2A.Open();

                   SP3A.Close();
                   SP3A.PortName = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3 SENDER") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3 SENDER");
                   SP3A.Open();
               }
               catch
               {
                   MessageBox.Show("Error Open Serial Port Check H/W");
               }
              comboBox1.Items.Clear();
              comboBox1.Items.Add(SP1.PortName);
              comboBox1.Items.Add(SP2.PortName);
              comboBox1.Items.Add(SP3.PortName);

              comboBox2.Items.Clear();
              comboBox2.Items.Add(SP1.PortName);
              comboBox2.Items.Add(SP2.PortName);
              comboBox2.Items.Add(SP3.PortName);

              comboBox3.Items.Clear();
              comboBox3.Items.Add(SP1.PortName);
              comboBox3.Items.Add(SP2.PortName);
              comboBox3.Items.Add(SP3.PortName);

              comboBox4.Items.Clear();
              comboBox4.Items.Add(SP1.PortName);
              comboBox4.Items.Add(SP2.PortName);
              comboBox4.Items.Add(SP3.PortName);

              comboBox5.Items.Clear();
              comboBox5.Items.Add(SP1.PortName);
              comboBox5.Items.Add(SP2.PortName);
              comboBox5.Items.Add(SP3.PortName);

              comboBox6.Items.Clear();
              comboBox6.Items.Add(SP1.PortName);
              comboBox6.Items.Add(SP2.PortName);
              comboBox6.Items.Add(SP3.PortName);

              comboBox7.Items.Clear();
              comboBox7.Items.Add(SP1.PortName);
              comboBox7.Items.Add(SP2.PortName);
              comboBox7.Items.Add(SP3.PortName);

              comboBox8.Items.Clear();
              comboBox8.Items.Add(SP1.PortName);
              comboBox8.Items.Add(SP2.PortName);
              comboBox8.Items.Add(SP3.PortName);

              comboBox9.Items.Clear();
              comboBox9.Items.Add(SP1.PortName);
              comboBox9.Items.Add(SP2.PortName);
              comboBox9.Items.Add(SP3.PortName);

              comboBox10.Items.Clear();
              comboBox10.Items.Add(SP1.PortName);
              comboBox10.Items.Add(SP2.PortName);
              comboBox10.Items.Add(SP3.PortName);
        }
        
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fconfig config = new Fconfig();
            DialogResult dg = config.ShowDialog();
            if (dg.ToString() == "OK")
            {
               string com1 = config.COM1; //(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE");
               string com2 = config.COM2; //(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE");
               string com3 = config.COM3; //(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL");
               string gmes = config.GMES; //(Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM GMES") == "") ? "" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL");
               string com1a = config.COM1A;
               string com2a = config.COM2A;
               string com3a = config.COM3A;

               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 1", com1);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 2", com2);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 3", com3);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 4", gmes);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 1 SENDER", com1a);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 2 SENDER", com2a);
               Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG", "COM 3 SENDER", com3a);

               loadConfig();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Serial Port
        private void SP1Write(string sn)
        {
            DataTable tbl1 = db.getData("SELECT ITEM_MATERIAL,ITEM_DESC FROM [PROD].[dbo].[TBL_MASTER_EPASS] Where ITEM_MODEL ='" + cmbModel.Text + "' ");
            int i = 0;
            if (sn.Length >2)
            {
                if (sn.Substring(0, 2) == "P0")
                {
                    SP1A.Write((char)2 + sn + (char)3);
                    SP1.Write("OK\r\n");
                    i++;
                    goto lompat;
                }
            }
            foreach (DataRow r in tbl1.Rows)
            {
                if (sn.IndexOf(r[0].ToString())>=0)
                { 
                    if (SP1.PortName == comboBox1.Text && cmbItem1.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP1.PortName == comboBox2.Text && cmbItem2.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP1.PortName == comboBox3.Text && cmbItem3.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP1.PortName == comboBox4.Text && cmbItem4.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP1.PortName == comboBox5.Text && cmbItem5.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP1.PortName == comboBox6.Text && cmbItem6.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP1.PortName == comboBox7.Text && cmbItem7.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP1.PortName == comboBox8.Text && cmbItem8.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP1.PortName == comboBox9.Text && cmbItem9.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP1.PortName == comboBox10.Text && cmbItem10.Text == r[1].ToString())
                    {
                        SP1A.Write((char)2 + sn + (char)3);
                        SP1.Write("OK\r\n");
                        i++;
                        break;
                    }
                }
            }
             lompat: ;
            if (i <= 0)
            {
                try
                {
                    SP1.Write("NG\r\n");
                }
                catch
                {
                    MessageBox.Show("ERROR");
                }
            }
            return;
            
        }

        private void SP1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = SP1.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblsn.Text = msg));
                    }
                    else
                    {
                        lblsn.Text = msg;
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => SP1Write(msg)));
                    }
                    else
                    {
                        SP1Write(msg);
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

        private void SP2Write(string sn)
        {
            DataTable tbl1 = db.getData("SELECT ITEM_MATERIAL,ITEM_DESC FROM [PROD].[dbo].[TBL_MASTER_EPASS] Where ITEM_MODEL ='" + cmbModel.Text + "' ");
            int i = 0;
            if (sn.Length > 2)
            {
                if (sn.Substring(0, 2) == "P0")
                {
                    SP2A.Write((char)2 + sn + (char)3);
                    SP2.Write("OK\r\n");
                    i++;
                    goto lompat;
                }
            }

            foreach (DataRow r in tbl1.Rows)
            {
                if (sn.IndexOf(r[0].ToString()) >= 0)
                {
                    if (SP2.PortName == comboBox1.Text && cmbItem1.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP2.PortName == comboBox2.Text && cmbItem2.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP2.PortName == comboBox3.Text && cmbItem3.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP2.PortName == comboBox4.Text && cmbItem4.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP2.PortName == comboBox5.Text && cmbItem5.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP2.PortName == comboBox6.Text && cmbItem6.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP2.PortName == comboBox7.Text && cmbItem7.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP2.PortName == comboBox8.Text && cmbItem8.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP2.PortName == comboBox9.Text && cmbItem9.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP2.PortName == comboBox10.Text && cmbItem10.Text == r[1].ToString())
                    {
                        SP2A.Write((char)2 + sn + (char)3);
                        SP2.Write("OK\r\n");
                        i++;
                        break;
                    }
                }
            }
             lompat: ;
            if (i <= 0)
            {
                try
                {
                    SP2.Write("NG\r\n");
                }
                catch
                {
                    MessageBox.Show("ERROR");
                }
            }
            return;
        }

        private void SP2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = SP2.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblsn.Text = msg2));
                    }
                    else
                    {
                        lblsn.Text = msg2;
                    }


                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => SP2Write(msg2)));
                    }
                    else
                    {
                        SP2Write(msg2);
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

        private void SP3Write(string sn)
        {
            DataTable tbl1 = db.getData("SELECT ITEM_MATERIAL,ITEM_DESC FROM [PROD].[dbo].[TBL_MASTER_EPASS] Where ITEM_MODEL ='" + cmbModel.Text + "' ");
            int i = 0;

            if (sn.Length > 2)
            {
                if (sn.Substring(0, 2) == "P0")
                {
                    SP3A.Write((char)2 + sn + (char)3);
                    SP3.Write("OK\r\n");
                    i++;
                    goto lompat;
                }
            }

            foreach (DataRow r in tbl1.Rows)
            {
                if (sn.IndexOf(r[0].ToString()) >= 0)
                {
                    if (SP3.PortName == comboBox1.Text && cmbItem1.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP3.PortName == comboBox2.Text && cmbItem2.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP3.PortName == comboBox3.Text && cmbItem3.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP3.PortName == comboBox4.Text && cmbItem4.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP3.PortName == comboBox5.Text && cmbItem5.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }

                    if (SP3.PortName == comboBox6.Text && cmbItem6.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP3.PortName == comboBox7.Text && cmbItem7.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP3.PortName == comboBox8.Text && cmbItem8.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP3.PortName == comboBox9.Text && cmbItem9.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }
                    if (SP3.PortName == comboBox10.Text && cmbItem10.Text == r[1].ToString())
                    {
                        SP3A.Write((char)2 + sn + (char)3);
                        SP3.Write("OK\r\n");
                        i++;
                        break;
                    }
                }
            }
            lompat: ;
            if (i <= 0)
            {
                try
                {
                    SP3.Write("NG\r\n");
                }
                catch
                {
                    MessageBox.Show("ERROR");
                }
            }
            return;
        }

        private void SP3_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = SP3.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => lblsn.Text = msg3));
                    }
                    else
                    {
                        lblsn.Text = msg3;
                    }


                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => SP3Write(msg3)));
                    }
                    else
                    {
                        SP3Write(msg3);
                    }
                    msg3 = "";
                }
                else if (data[a] == (char)2)
                {
                    msg3 = "";
                }
                else
                {
                    if (data[a].ToString() != "\r")
                        msg3 = msg3 + data[a];
                }
            }
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "MODEL NAME", cmbModel.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 1", cmbItem1.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 2", cmbItem2.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 3", cmbItem3.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 4", cmbItem4.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 5", cmbItem5.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 6", cmbItem6.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 7", cmbItem7.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 8", cmbItem8.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 9", cmbItem9.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "ITEM NAME 10", cmbItem10.Text);

            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 1", comboBox1.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 2", comboBox2.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 3", comboBox3.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 4", comboBox4.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 5", comboBox5.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 6", comboBox6.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 7", comboBox7.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 8", comboBox8.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 9", comboBox9.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG MODEL", "CHILD COM 10", comboBox10.Text);


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

        private void lockSpec(bool s)
        {
            cmbModel.Enabled = s;
            comboBox1.Enabled = s;
            comboBox2.Enabled = s;
            comboBox3.Enabled = s;
            comboBox4.Enabled = s;
            comboBox5.Enabled = s;
            comboBox6.Enabled = s;
            comboBox7.Enabled = s;
            comboBox8.Enabled = s;
            comboBox9.Enabled = s;
            comboBox10.Enabled = s;

            cmbItem1.Enabled = s;
            cmbItem2.Enabled = s;
            cmbItem3.Enabled = s;
            cmbItem4.Enabled = s;
            cmbItem5.Enabled = s;
            cmbItem6.Enabled = s;
            cmbItem7.Enabled = s;
            cmbItem8.Enabled = s;
            cmbItem9.Enabled = s;
            cmbItem10.Enabled = s;
        }

        private void editSpecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!editSpecToolStripMenuItem.Checked)
            {
                lockSpec(true);
                editSpecToolStripMenuItem.Checked = true;
            }
            else
            {
                lockSpec(false);
                editSpecToolStripMenuItem.Checked = false;
            }
        }

        private void sendOKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SP1.Close();
                SP1.PortName = "COM9";
                SP1.Open();
                SP1.Write("OK"+"\r\n");
                SP1.Close();
            }
            catch
            {
                MessageBox.Show("ERROR");
            }

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
