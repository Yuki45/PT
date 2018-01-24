using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace Scanner_Aging_OCV
{
    public partial class MainForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        DB.DB db = new DB.DB();
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);
        SerialPort scanner = new SerialPort();
        SerialPort gmes = new SerialPort();
        string msg = "";

        public MainForm()
        {
            InitializeComponent();

            DataTable model = db.getData("SELECT model_name FROM tbl_master_model");
            cmbModel.Items.Clear();
            foreach (DataRow row in model.Rows)
            {
                cmbModel.Items.Add(row[0].ToString());
            }
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string[] row = txtBarcode.Text.Split('+');

                if (row.Length > 1)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtRPart.Text = row[0]));
                    }
                    else
                    {
                        txtRPart.Text = row[0];
                    }

                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => ValidNo(txtRPart.Text)));
                    }
                    else
                    {
                        ValidNo(txtRPart.Text);
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            config();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (btnConfig.Text == "CONFIG")
            {
                txtScanner.ReadOnly = false;
                cmbModel.Enabled = true;
                btnConfig.Text = "SAVE";
            }
            else
            {
                db.setData2("UPDATE tbl_configuration SET scanner='" + txtScanner.Text + "',gmes ='"+txtgmes.Text+"', model='" + cmbModel.Text + "'");
                config();
                txtScanner.ReadOnly = true;
                cmbModel.Enabled = false;
                btnConfig.Text = "CONFIG";
            }
        }

        private void ValidNo(string partNo)
        {
            DataTable tbl = db.getData("SELECT model_name from tbl_master_part");
            foreach (DataRow row in tbl.Rows)
            {
                txtRModel.Text = row[0].ToString();
            }

            if (txtRPart.Text != txtRSpec.Text)
            {
                lblResult.Text = "NG";
                lblResult.ForeColor = Color.Red;
                txtBarcode.Text = "";
            }
            else
            {
                try
                {
                    gmes.Write((char)2+txtBarcode.Text+(char)3);
                }
                catch { }
                lblQty.Text = Convert.ToString(Convert.ToInt32(lblQty.Text)+1);
                lblResult.Text = "PASS";
                lblResult.ForeColor = Color.Lime;
                txtBarcode.Text = "";
            }
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = scanner.ReadExisting();

            for (int a = 0; a <= data.Length - 1; a++)
            {
                if (data[a] == (char)3 || data[a].ToString() == "\n")
                {
                    string[] row = msg.Split('+');
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => txtBarcode.Text = msg));
                    }
                    else
                    {
                        txtBarcode.Text = msg;
                    }

                    if (row.Length>1)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => txtRPart.Text = row[0]));
                        }
                        else
                        {
                            txtRPart.Text = row[0];
                        }

                        if (InvokeRequired)
                        {
                            Invoke(new MethodInvoker(() => ValidNo(txtRPart.Text)));
                        }
                        else
                        {
                            ValidNo(txtRPart.Text);
                        }
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

        private void config()
        {            
            DataTable tbl = db.getData("select scanner, model, gmes from tbl_configuration ");
            foreach (DataRow row in tbl.Rows)
            {
                txtScanner.Text = row[0].ToString();
                cmbModel.Text = row[1].ToString();
                txtgmes.Text = row[2].ToString();
            }

            try
            {
                scanner.Close();
                scanner.PortName = txtScanner.Text;
                scanner.BaudRate = 9600;
                scanner.DataBits = 8;
                scanner.Parity = Parity.None;
                scanner.StopBits = StopBits.One;
                scanner.Open();

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
                gmes.PortName = txtgmes.Text;
                gmes.BaudRate = 9600;
                gmes.DataBits = 8;
                gmes.Parity = Parity.None;
                gmes.StopBits = StopBits.One;
                gmes.Open();

                txtgmes.BackColor = Color.Chartreuse;
            }
            catch
            {
                txtgmes.BackColor = Color.Red;
            }

            if (cmbModel.Text != "")
            {
                DataTable result = db.getData("select part_no from tbl_master_part where model_name='"+cmbModel.Text+"'");
                foreach (DataRow row in result.Rows)
                {
                    txtRSpec.Text = row[0].ToString();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void masterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flogin login = new Flogin();
            DialogResult dg = login.ShowDialog();
            if (dg.ToString() == "OK")
            {
                MasterForm master = new MasterForm();
                master.ShowDialog();
            }

            DataTable model = db.getData("SELECT model_name FROM tbl_master_model");
            cmbModel.Items.Clear();
            foreach (DataRow row in model.Rows)
            {
                cmbModel.Items.Add(row[0].ToString());
            }
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (txtBarcode.Text.Length > 0)
            {
                
            }
        }
    }
}
