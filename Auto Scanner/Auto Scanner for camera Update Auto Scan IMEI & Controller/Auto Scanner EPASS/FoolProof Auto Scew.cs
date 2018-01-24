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
    public partial class FoolProof_Auto_Scew : Form
    {
        SerialPort scanner = new SerialPort();
        SerialPort gmes = new SerialPort();
        SerialPort gmes2 = new SerialPort();
        string msg = "", msg2="";
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        DB.DB db = new DB.DB();
        DataTable tModel = new DataTable();
        string type = "", model="", partno="";
        public FoolProof_Auto_Scew()
        {
            InitializeComponent();
        }

        private void FoolProof_Auto_Scew_Load(object sender, EventArgs e)
        {
            DataTable tbl = db.getData(" SELECT DISTINCT([basicmodel]) as model  FROM [MAIN].[dbo].[specscrew]");

            cmbModel.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                cmbModel.Items.Add(row[0].ToString());
            }
            txtScanner.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "SCANNER");
            txtGmes.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "GMES");
            cmbModel.SelectedItem = Inifiles.ReadValue(appPath + "/SETTING.INI", "SETTING", "MODEL");
            Config();
            txtGmes.Enabled = false;
            txtScanner.Enabled = false;
            this.Text = this.Text + "[" + Inifiles.getIP() + "]";
        }

        private bool verifySN(string sn)
        {
            bool status = false;
            if (tModel.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow row in tModel.Rows)
                {
                    type = row[1].ToString();
                    model = row[0].ToString();
                    string partno = row[2].ToString();
                    string tpartno = sn.Substring(0, 11);
                    if (tpartno == row[2].ToString())
                    {
                        if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() =>
                                db.setData2(@" INSERT INTO [MAIN].[dbo].[SC_FL_HISTORY]
                                   ([workstation_ip]
                                   ,[sn_barcode]
                                   ,[scan_time]
                                   ,[status])
                                    VALUES
                                   ('" + Inifiles.getIP() + "', '" + sn + "', GETDATE(),'MATCHING : MODEL(" + cmbModel.Text + "=" + row[0].ToString() + " PART NO = " + row[2].ToString() + "')")));
                            }
                        else
                        {
                                db.setData2(@" INSERT INTO [MAIN].[dbo].[SC_FL_HISTORY]
                                   ([workstation_ip]
                                   ,[sn_barcode]
                                   ,[scan_time]
                                   ,[status])
                                    VALUES
                                   ('" + Inifiles.getIP() + "', '" + sn + "', GETDATE(),'MATCHING : MODEL(" + cmbModel.Text + "=" + row[0].ToString() + " PART NO = " + row[2].ToString() + "')");
                        }
                        count++;
                    }
                }
                if (count > 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = scanner.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtsn.Text = msg));
                    }
                    else
                    {
                        txtsn.Text = msg;
                    }

                    if (msg.Length > 15)
                    {
                        
                        if (verifySN(msg))
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblresult.Text = "SCREW MACHINE " + type + " MATCHING"));
                            }
                            else
                            {
                                lblresult.Text = "SCREW MACHINE " + type + " MATCHING";
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblresult.ForeColor = Color.Chartreuse));
                            }
                            else
                            {
                                lblresult.ForeColor = Color.Chartreuse;
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblresult.Text = "SCREW MACHINE " + type + " UNMATCHING"));
                            }
                            else
                            {
                                lblresult.Text = "SCREW MACHINE " + type + " UNMATCHING";
                            }

                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() => lblresult.ForeColor = Color.Red));
                            }
                            else
                            {
                                lblresult.ForeColor = Color.Red;
                            }
                            if (InvokeRequired)
                            {
                                Invoke(new MethodInvoker(() =>
                            db.setData2(@" INSERT INTO [MAIN].[dbo].[SC_FL_HISTORY]
                                   ([workstation_ip]
                                   ,[sn_barcode]
                                   ,[scan_time]
                                   ,[status])
                                    VALUES
                                   ('" + Inifiles.getIP() + "', '" + msg + "', GETDATE(),'UNMATCHING : MODEL(" + cmbModel.Text + "=" + model + " PART NO = " + partno + "')")));
                            }
                            else
                            {
                                db.setData2(@" INSERT INTO [MAIN].[dbo].[SC_FL_HISTORY]
                                   ([workstation_ip]
                                   ,[sn_barcode]
                                   ,[scan_time]
                                   ,[status])
                                    VALUES
                                   ('" + Inifiles.getIP() + "', '" + msg + "', GETDATE(),'UNMATCHING : MODEL(" + cmbModel.Text + "=" + model + " PART NO = " + partno + "')");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            gmes.Write(msg + "\r\n");
                        }
                        catch { }
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

        private void Config()
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
                Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "SCANNER", txtScanner.Text);
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
                Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "GMES", txtGmes.Text);
                this.gmes.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.GMES_DataReceived);
                txtGmes.BackColor = Color.Chartreuse;
            }
            catch
            {
                txtGmes.BackColor = Color.Red;
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (btnConfig.BackColor == Color.Silver)
            {
               
                txtGmes.Enabled = true;
                txtScanner.Enabled = true;
                btnConfig.BackColor = Color.SkyBlue;
            }
            else
            {
                Config();
                txtGmes.Enabled = false;
                txtScanner.Enabled = false;
                btnConfig.BackColor = Color.Silver;
            }
        }

        private void FoolProof_Auto_Scew_FormClosing(object sender, FormClosingEventArgs e)
        {
            Inifiles.WriteValue(appPath + "/SETTING.INI", "SETTING", "MODEL", cmbModel.Text);
        }

        private void cmbModel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cmbModel_SelectedValueChanged(object sender, EventArgs e)
        {
            tModel = db.getData(" SELECT TOP 1000 [basicmodel],[type_part],[code_part]  FROM [MAIN].[dbo].[specscrew] Where basicmodel = '" + cmbModel.Text + "'");
        }
    }
}
