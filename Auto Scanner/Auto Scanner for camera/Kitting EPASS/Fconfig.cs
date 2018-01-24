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

namespace Kitting_EPASS
{
    public partial class Fconfig : Form
    {
        public string COM1, COM2, COM3,COM1A, COM2A, COM3A, GMES;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        public Fconfig()
        {
            InitializeComponent();
        }

        private void Fconfig_Load(object sender, EventArgs e)
        {
            foreach (string com in SerialPort.GetPortNames())
            {
                cmbCom1.Items.Add(com);
                cmbCom2.Items.Add(com);
                cmbCom3.Items.Add(com);
                cmbCom4.Items.Add(com);
                cmbCom1A.Items.Add(com);
                cmbCom2A.Items.Add(com);
                cmbCom3A.Items.Add(com);
            }

            cmbCom1.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1");
            cmbCom2.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2");
            cmbCom3.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3");
            cmbCom4.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 4");
            cmbCom1A.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 1 SENDER");
            cmbCom2A.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 2 SENDER");
            cmbCom3A.Text = Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG", "COM 3 SENDER");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            COM1 = cmbCom1.Text;
            COM2 = cmbCom2.Text;
            COM3 = cmbCom3.Text;
            COM3A = cmbCom3A.Text;
            COM2A = cmbCom2A.Text;
            COM1A = cmbCom1A.Text;
            GMES = cmbCom4.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
