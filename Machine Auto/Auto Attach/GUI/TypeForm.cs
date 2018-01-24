using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Attach.GUI
{
    public partial class TypeForm : Form
    {
        public string result = "";
        public TypeForm()
        {
            InitializeComponent();
        }

        private void btnDryRun_Click(object sender, EventArgs e)
        {
            result = "DRY RUN";
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            result = "NORMAL";
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            result = "SIMULASI";
        }
    }
}
