using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Auto_Scanner
{
    public partial class Flogin : Form
    {
        string test;
        public Flogin(string test)
        {
            InitializeComponent();
            this.test = test;
        }
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtremark.Text == "" || txtusername.Text == "" || txtpass.Text == "")
            {
                MessageBox.Show("Data Can't Empty..!"); return;
            }
            if (txtpass.Text == "engAdmin")
            {
                logfiles.WriteLogAgent(DateTime.Now.ToString()+" | "+test+" |"+txtusername.Text+" | " +txtremark.Text, appPath + "\\LOG\\" + Application.ProductName +  DateTime.Now.ToString("yyyyMMdd") + ".log");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
            {
                MessageBox.Show("Wrong Password..!");
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }
    }
}
