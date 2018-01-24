using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.IO.Ports;

namespace Auto_Scanner_IMEI
{
    public partial class MainForm : Form
    {
        ulong a;
        uint b;
        ulong cmdPos;
        bool MoveStatus = false;
        int status = 0;
        bool sJig1,sjig2,sjig3,sjig4,sjig5,sjig6,sjig7,sjig8,sjig9,sjig10;
        string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        SerialPort COM = new SerialPort();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                if (IO.FAS_Connect(byte.Parse(txtCom.Text.Substring(3,txtCom.Text.Length - 3)), uint.Parse("115200")) >= 1)
                {
                    btnConnect.Text = "Disconnect";
                    status = 1;
                    timerOn();
                    OpenIO();
                    timer1.Enabled = true;
                    txtCom.ReadOnly = true;
                    txtIO.ReadOnly = true;
                }
                else
                {
                    status = 0;
                    btnConnect.Text = "Connect";
                    MessageBox.Show("Error");
                }
            }
            else
            {
                COM.Close();
                timelim.Abort();
                timer1.Enabled = false;
                IO.FAS_Close(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)));
                btnConnect.Text = "Connect";
                txtCom.ReadOnly = false;
                txtIO.ReadOnly = false;
                status = 0;
            }
        }

        private void OpenIO()
        {
            try
            {
                COM.Close();
                COM.PortName = txtIO.Text;
                COM.BaudRate = 115200;
                COM.Open();
                this.COM.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.COM_DataReceived);
            }
            catch
            {
                MessageBox.Show("Error Open Serial Port Check H/W");
            }
        }

        public void realtime()
        {
            while (true)
            {
                int command;
                command = IO.FAS_GetActualPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out a);

                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => txtActPos.Text = a.ToString()));
                }
                else
                {
                    txtActPos.Text = a.ToString();
                }

                try
                {
                    COM.Write("R01\r\n");
                }
                catch { }
                Thread.Sleep(300);
            }
        }

        Thread timelim;
        private void timerOn()
        {
            timelim = new Thread(() => realtime());
            timelim.Start();
        }

        private void COM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            string data = COM.ReadExisting();
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => txtport.Text = data.ToString()));
            }
            else
            {
                txtport.Text = data.ToString();
            }

            if (data.Length >= 16)
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new MethodInvoker(() => ReadJIG(data.ToString())));
                    }
                    else
                    {
                        ReadJIG(data.ToString());
                    }
                }
                catch (Exception ex) { //logfiles.WriteLogAgent(data.ToString() + "\r\n", appPath + "//App.log"); 
                }
            }
        }

        string[] jigs;
        private void ReadJIG(string StatusJIG)
        {
            //02224262
            //0111213141516171 !6 bit coy
            string stemp = "";
            if (StatusJIG.Length >= 16)
            {
                for (int i = 0; i < StatusJIG.Length; i++)
                {
                    //  Console.WriteLine(s[i]);
                    if (i == 1)
                    {
                        try
                        {
                            Jig1 = StatusJIG.Substring(0, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(Jig1 + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 3)
                    {
                        try
                        {
                            Jig2 = StatusJIG.Substring(2, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(Jig2 + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 5)
                    {
                        try
                        {
                            Jig3 = StatusJIG.Substring(4, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 7)
                    {
                        try
                        {
                            Jig4 = StatusJIG.Substring(6, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 9)
                    {
                        try
                        {
                            Jig5 = StatusJIG.Substring(8, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 11)
                    {
                        try
                        {
                            Jig6 = StatusJIG.Substring(10, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 13)
                    {
                        try
                        {
                            Jig7 = StatusJIG.Substring(12, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }

                    if (i == 15)
                    {
                        try
                        {
                            Jig8 = StatusJIG.Substring(14, 2);
                        }
                        catch
                        {
                            logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//App.log");
                        }
                    }
                    //logfiles.WriteLogAgent(jigs[i].ToString() + "\r\n", appPath + "//AppPort.log");
                }
            }
            else
            {
                //logfiles.WriteLogAgent(StatusJIG.ToString(), appPath + "//AppPort.log");
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (status==1)
            {
            COM.Close();
            timelim.Abort();
            IO.FAS_Close(byte.Parse("16"));
            }
        }

        private void txtport_TextChanged(object sender, EventArgs e)
        {
            if (txtport.Text.Length >= 16)
            {
                try
                {
                    if (InvokeRequired)
                    {
                       // Invoke(new MethodInvoker(() => ReadJIG(txtport.Text)));
                    }
                    else
                    {
                        //ReadJIG(txtport.Text);
                    }
                }
                catch (Exception ex) { logfiles.WriteLogAgent(txtport.Text + "\r\n", appPath + "//App.log"); }
            }
        }

        #region Variabel c#
        string _jig1;
        public string Jig1
        {
            get
            {
                return this._jig1;
            }
            set
            {
                if (value == "01")
                {
                    jig1.BackColor = Color.Yellow;
                    sJig1 = false;
                }
                else if (value == "00")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1)&& !sJig1)
                    {
                        sJig1 = true;
                        listView1.Items.Add(lvi1);
                    }
                    jig1.BackColor = Color.Green;
                }

                this._jig1 = value;
            }
        }

        string _jig2;
        public string Jig2
        {
            get
            {
                return this._jig2;
            }
            set
            {
                if (value == "11")
                {
                    jig2.BackColor = Color.Yellow;
                    sjig2 = false;
                }
                else if (value == "10")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig2)
                    {
                        sjig2 = true;
                        listView1.Items.Add(lvi1);
                    }
                    jig2.BackColor = Color.Green;
                }

                this._jig2 = value;
            }
        }

        string _jig3;
        public string Jig3
        {
            get
            {
                return this._jig3;
            }
            set
            {
                if (value == "21")
                {
                    sjig3 = false;
                    jig3.BackColor = Color.Yellow;
                }
                else if (value == "20")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig3)
                    {
                        sjig3 = true;
                        listView1.Items.Add(lvi1);
                    }

                    jig3.BackColor = Color.Green;
                }

                this._jig3 = value;
            }
        }

        string _jig4;
        public string Jig4
        {
            get
            {
                return this._jig4;
            }
            set
            {
                if (value == "31")
                {
                    sjig4 = false;
                    jig4.BackColor = Color.Yellow;
                }
                else if (value == "30")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig4)
                    {
                        sjig4 = true;
                        listView1.Items.Add(lvi1);
                    }

                    jig4.BackColor = Color.Green;
                }

                this._jig4 = value;
            }
        }

        string _jig5;
        public string Jig5
        {
            get
            {
                return this._jig5;
            }
            set
            {
                if (value == "41")
                {
                    sjig5 = false;
                    jig5.BackColor = Color.Yellow;
                }
                else if (value == "40")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig5)
                    {
                        sjig5 = true;
                        listView1.Items.Add(lvi1);
                    }

                    jig5.BackColor = Color.Green;
                }

                this._jig5 = value;
            }
        }

        string _jig6;
        public string Jig6
        {
            get
            {
                return this._jig6;
            }
            set
            {
                if (value == "51")
                {
                    sjig6 = false;
                    jig6.BackColor = Color.Yellow;
                }
                else if (value == "50")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig6)
                    {
                        sjig6 = true;
                        listView1.Items.Add(lvi1);
                    }

                    jig6.BackColor = Color.Green;
                }

                this._jig6 = value;
            }
        }

        string _jig7;
        public string Jig7
        {
            get
            {
                return this._jig7;
            }
            set
            {
                if (value == "61")
                {
                    sjig7 = false;
                    jig7.BackColor = Color.Yellow;
                }
                else if (value == "60")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig7)
                    {
                        sjig7 = true;
                        listView1.Items.Add(lvi1);
                    }
                    jig7.BackColor = Color.Green;
                }

                this._jig7 = value;
            }
        }

        string _jig8;
        public string Jig8
        {
            get
            {
                return this._jig8;
            }
            set
            {
                if (value == "71")
                {
                    sjig8 = false;
                    jig8.BackColor = Color.Yellow;
                }
                else if (value == "70")
                {
                    ListViewItem lvi1 = new ListViewItem(value);
                    lvi1.Name = "item1";
                    if (!IsInCollection(lvi1) && !sjig8)
                    {
                        sjig8 = true;
                        listView1.Items.Add(lvi1);
                    }

                    jig8.BackColor = Color.Green;
                }

                this._jig8 = value;
            }
        }
        #endregion

        private bool IsInCollection(ListViewItem lvi)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                bool subItemEqualFlag = true;
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    string sub1 = item.SubItems[i].Text;
                    string sub2 = lvi.SubItems[i].Text;
                    if (sub1 != sub2)
                    {
                        subItemEqualFlag = false;
                    }
                }
                if (subItemEqualFlag)
                    return true;
            }
            return false;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            int command;
            command = IO.FAS_GetCommandPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out cmdPos);

            /*if (a == cmdPos)
            {
                MoveStatus = false;

            }*/
            if (listView1.Items.Count > 0)
            {
                if (listView1.Items[0].SubItems[0].Text == "00")
                {
                    if (!MoveStatus)
                    {
                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 8);
                        MoveStatus = true;
                    }
                    else if (129029 == cmdPos)
                    {
                        listView1.Items[0].Remove();
                        MoveStatus = false;
                    }
                    else
                    {
                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 8);
                    }
                }
                else
                    if (listView1.Items[0].SubItems[0].Text == "10")
                    {
                        if (!MoveStatus)
                        {
                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 7);
                            MoveStatus = true;
                        }
                        else if (112628 == cmdPos && MoveStatus)
                        {
                            listView1.Items[0].Remove();
                            MoveStatus = false;
                        }
                        else
                        {
                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 7);
                        }
                    }
                    else
                        if (listView1.Items[0].SubItems[0].Text == "20")
                        {
                            if (!MoveStatus)
                            {
                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 6);
                                MoveStatus = true;
                            }
                            else if (96033 == cmdPos && MoveStatus)
                            {
                                listView1.Items[0].Remove();
                                MoveStatus = false;
                            }
                            else
                            {
                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 6);
                            }
                        }
                        else
                            if (listView1.Items[0].SubItems[0].Text == "30")
                            {
                                if (!MoveStatus)
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 5);
                                    MoveStatus = true;
                                }
                                else if (79632 == cmdPos && MoveStatus)
                                {
                                    listView1.Items[0].Remove();
                                    MoveStatus = false;
                                }
                                else
                                {
                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 5);
                                }
                            }
                            else
                                if (listView1.Items[0].SubItems[0].Text == "40")
                                {
                                    if (!MoveStatus)
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 4);
                                        MoveStatus = true;
                                    }
                                    else if (63026 == cmdPos && MoveStatus)
                                    {
                                        listView1.Items[0].Remove();
                                        MoveStatus = false;
                                    }
                                    else
                                    {
                                        IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 4);
                                    }
                                }
                                else
                                    if (listView1.Items[0].SubItems[0].Text == "50")
                                    {
                                        if (!MoveStatus)
                                        {
                                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 3);
                                            MoveStatus = true;
                                        }
                                        else if (46633 == cmdPos && MoveStatus)
                                        {
                                            listView1.Items[0].Remove();

                                        }
                                        else
                                        {
                                            IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 3);
                                        }
                                    }
                                    else
                                        if (listView1.Items[0].SubItems[0].Text == "60")
                                        {
                                            if (!MoveStatus)
                                            {
                                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 2);
                                                MoveStatus = true;
                                            }
                                            else if (30231 == cmdPos && MoveStatus)
                                            {
                                                listView1.Items[0].Remove();
                                                MoveStatus = false;
                                            }
                                            else
                                            {
                                                IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 2);
                                            }
                                        }
                                        else
                                            if (listView1.Items[0].SubItems[0].Text == "70")
                                            {
                                                if (!MoveStatus)
                                                {
                                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 1);
                                                    MoveStatus = true;
                                                }
                                                else if (14030 == cmdPos && MoveStatus)
                                                {
                                                    listView1.Items[0].Remove();
                                                    MoveStatus = false;
                                                }
                                                else
                                                {
                                                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 1);
                                                }
                                            }
            }
            else
            {
                if (cmdPos != 4294967255)
                    IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), 0);
            }
            timer1.Start();
        }

        
        private void btnTeach_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // IO.FAS_PosTableRunItem(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"),2);
           /* ListViewItem lvi1 = new ListViewItem("1");
            lvi1.Name = "item1";
            listView1.Items.Add(lvi1);   */         
        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            int command;
            command = IO.FAS_GetActualPos(byte.Parse(txtCom.Text.Substring(3, txtCom.Text.Length - 3)), byte.Parse("0"), out cmdPos);

            txtCmdPos.Text = cmdPos.ToString();
        }

    }
}
