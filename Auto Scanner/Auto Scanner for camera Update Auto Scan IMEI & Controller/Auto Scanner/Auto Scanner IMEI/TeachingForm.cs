using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Auto_Scanner_IMEI
{
    public partial class TeachingForm : Form
    {
        public int jig;
        public string status;
        public TeachingForm()
        {
            InitializeComponent();
        }

        private void TeachingForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = "TEACH";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = "RESET";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = "USE";
        }
    }
}
