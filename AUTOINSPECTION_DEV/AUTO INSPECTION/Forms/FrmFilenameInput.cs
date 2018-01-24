using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInspection.Forms
{
    public partial class FrmFilenameInput : Form
    {
        public string NewFileName { get; set; }
        
        public FrmFilenameInput()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            NewFileName = textBoxInput.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
