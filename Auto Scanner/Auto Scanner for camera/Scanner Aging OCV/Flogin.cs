using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Scanner_Aging_OCV
{
    public partial class Flogin : Form
    {
        string test;
        public Flogin()
        {
            InitializeComponent();
        }
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                MessageBox.Show("Data Can't Empty..!"); return;
            }
            if (txtpass.Text == "engAdmin")
            {
                 this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
            {
                MessageBox.Show("Wrong Password..!");
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        private void txtpass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            button2_Click( 0, null);
        }
    }
}
