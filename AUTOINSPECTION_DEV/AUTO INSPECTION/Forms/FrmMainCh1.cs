using AutoInspection.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using VisionTestNameSpace;

namespace AutoInspection
{
    public partial class FrmMainCh1 : Form, IUserInterface
    {
        Controller controller;

        public FrmMainCh1()
        {
            InitializeComponent();
        }
        public void DisplayModelName(string modelName)
        {
            throw new NotImplementedException();
        }
        public string GetModelName()
        {
            throw new NotImplementedException();
        }
        private void FrmMainCh1_Load(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(tabPage1.Left, tabPage1.Top, tabPage1.Width, tabPage1.Height);
            tabControl1.Region = new Region(rect);

            controller = new Controller(this);
            controller.CreateDevices();
            controller.InitDevices();
        }

        private void FrmMainCh1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if( controller != null )
            {
                controller.DestoryDevices();
            }
        }

        public PictureBox GetTeachingPictureBox()
        {
            return picTeaching;
        }


        void ClearBtn()
        {
            btnAuto.Image = Properties.Resources.but_01auto_disable;
            btnManual.Image = Properties.Resources.but_02manual_disable;
            btnTeach.Image = Properties.Resources.but_04teach_disable;
            btnData.Image = Properties.Resources.but_03data_disable;
            btnLog.Image = Properties.Resources.but_05log_disable;
            btnExit.Image = Properties.Resources.but_07exit_disable;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnAuto.Image = Properties.Resources.but_01auto_select;
            tabControl1.SelectTab(0);
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnManual.Image = Properties.Resources.but_02manual_select;
            tabControl1.SelectTab(1);
        }

        private void btnTeach_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnTeach.Image = Properties.Resources.but_04teach_select;
            tabControl1.SelectTab(2);
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnData.Image = Properties.Resources.but_03data_select;
            tabControl1.SelectTab(3);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnLog.Image = Properties.Resources.but_05log_select;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ClearBtn();
            btnExit.Image = Properties.Resources.but_07exit_select;
            if (MessageBox.Show("Do you want to close?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
        }

        public void DisplayImage(int Channel, Bitmap Image)
        {
        }
        public void DisplayProductionInfo( bool IsPass )
        {
            if (IsPass)
                Config.CountPass++;
            else
                Config.CountFail++;
        }


        public void DisplayElapsedTime(string time)
        {
        }

        public void DisplayElapsedTime(long timeMs)
        {
        }


        public void ClearTestResult()
        {
        }

        //public void DisplayTestResult(string testName, SpecBase spec, ResultBase result)
        //{
        //    ListViewItem item = new ListViewItem("test");

        //    item.SubItems.Add(result.Measured);
        //    item.SubItems.Add(spec.SpecMin);
        //    item.SubItems.Add(spec.SpecMax);
        //    item.SubItems.Add(result.TestResult.ToString());

        //    listItem_Whole.Items.Add(item);
        //    if (result.TestResult)
        //    {
        //        listItem_Whole.Items[listItem_Whole.Items.Count - 1].ForeColor = Color.Red;
        //    }
        //    else
        //    {
        //        listItem_Whole.Items[listItem_Whole.Items.Count - 1].ForeColor = Color.Black;
        //    }

        //    listItem_Whole.View = View.Details;
        //}

        public void DisplayTestResult(string testName, string measure, string min, string max, bool result)
        {
            return;
        }
        public void DisplayErrorMsg(string testName, string msg)
        {
        }


        public void DisplayStatus(string status)
        {
        }
        public void DisplayResult(string result)
        {
        }
        public string GetImei()
        {
            return "";
        }
        public void DisplayImei(string imei)
        {
        }
        public void DisplayFailInfo(string info)
        {
        }
    }
}
