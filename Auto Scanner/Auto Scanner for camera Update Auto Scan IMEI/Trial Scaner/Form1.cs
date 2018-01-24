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

namespace Trial_Scaner
{
    public partial class Form1 : Form
    {
        SerialPort COM = new SerialPort();
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string com in SerialPort.GetPortNames())
            {
                cmbCom1.Items.Add(com);
            }

           // cmbCom1.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1");
        }

        private void cmbCom1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                COM.Close();
                COM.PortName = cmbCom1.Text;
                COM.BaudRate = Convert.ToInt32(cmbBaudrate.Text);
                COM.Open();

                this.COM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            
            }
            catch
            {
                MessageBox.Show("Error Open Serial Port Check H/W");
            }
        }

        string msg = "";
        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = COM.ReadExisting();
            if (checkBox1.Checked)
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => txtLog.AppendText(data + "\r\n")));
                }
                else
                {
                    txtLog.AppendText(data + "\r\n");
                }
                msg = "";
            }
            else
            {
                for (int a = 0; a <= data.Length - 1; a++)
                {
                    if (data[a] == (char)3 || data[a].ToString() == "\n")
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => txtLog.Text = msg + "\r\n" + txtLog.Text));
                        }
                        else
                        {
                            txtLog.Text = msg + "\r\n" + txtLog.Text;
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
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rdNormal.Checked)
            {
                COM.Write(textBox1.Text + "\r\n");
            }
            else if (rdSTX.Checked)
            {
                COM.Write((char)2 + textBox1.Text + (char)3);
            }
            else
            {
                COM.Write( textBox1.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }
    }
}
