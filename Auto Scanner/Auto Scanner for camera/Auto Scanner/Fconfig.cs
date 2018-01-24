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
    public partial class Fconfig : Form
    {
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        string s = "";
        public Fconfig(string initialValue)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            ValueFromParent = initialValue;
        }

        public string ValueFromParent
        {
            set
            {
                // This could be setting a field variable, or in this case
                // it is setting the Text property of a control directly.
                if (value.Length >= 15)
                {
                    //Double d = value;//Convert.ToDouble(value.Substring(4,8).Replace('.',','));
                    this.txtvalue.Text = cDoubles(value.Substring(4, 8));//Convert.ToString(d);

                }
            }
        }

        private void openconfig()
        {
            cmbQty.SelectedItem = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "QTY BASE")=="")?"1":Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "QTY BASE");
            cmbdevided.SelectedItem = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE") == "") ? "1000" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE");
            cmbTolerance.SelectedItem = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE") == "") ? "1" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE");
            txtusl.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL") == "") ? "0" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL");
            txtlsl.Text = (Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL") == "") ? "0" : Inifiles.ReadValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "USL", txtusl.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "LSL", txtlsl.Text );
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "QTY BASE", cmbQty.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "QTY PREVIEW", txtbase.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "TOLERANCE", cmbTolerance.Text);
            Inifiles.WriteValue(appPath + "/SETTING.INI", "CONFIG WEIGHT", "DEVIDE", cmbdevided.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void Fconfig_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 50; i++)
            {
                cmbQty.Items.Add(i.ToString());
                cmbTolerance.Items.Add(i.ToString());
            }

            openconfig();

        }
        private string cDoubles(string value)
        {
            if (value.Substring(0, 4) == "0000")
            {
                return value.Substring(3, 5).Replace('.', ',');
            }
            else if (value.Substring(0, 3) == "000")
            {
                return value.Substring(2, 6).Replace('.', ',');
            }
            else if (value.Substring(0, 2) == "00")
            {
                return value.Substring(1, 7).Replace('.', ',');
            }
            else
            {
                return value.Substring(0, 8).Replace('.', ',');
            }
           
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (txtvalue.Text != "" && Convert.ToInt32(txtbase.Text) < Convert.ToInt32(cmbQty.Text))
            {
                listView1.Items.Add(txtvalue.Text);
                txtbase.Text = listView1.Items.Count.ToString();
            }

            if (Convert.ToInt32(txtbase.Text) == Convert.ToInt32(cmbQty.Text))
            {
                listView1.Sorting = SortOrder.Descending;

                double max = 0;
                double min = 1000;
                foreach (ListViewItem item in listView1.Items)
                {
                    if (Convert.ToDouble(item.Text) > max)
                    {
                        max = Convert.ToDouble(item.Text);
                    }
                    else if (Convert.ToDouble(item.Text) < min)
                    {
                        min = Convert.ToDouble(item.Text);
                    }
                }

                txtusl.Text = max.ToString();
                txtlsl.Text = min.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            cmbQty.SelectedItem = "1";
            cmbTolerance.SelectedItem = "1";
            txtusl.Text =  "0" ;
            txtlsl.Text = "0" ;
            txtbase.Text = "0";     
        }
    }

    
}
