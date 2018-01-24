using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Trial_Ezi_Servo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int status = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Connect")
            {
                if (IO.FAS_Connect(byte.Parse("16"), uint.Parse("115200")) >= 1)
                {
                    button1.Text = "Disconnect";
                    status = 1;
                    timerOn();
                }
                else
                {
                    status = 0;
                    button1.Text = "Connect";
                    MessageBox.Show("Error");
                }
            }
            else
            {
                IO.FAS_Close(byte.Parse("16"));
                button1.Text = "Connect";
                status = 0;
            }
        }

        ulong a ;
        uint b;
        private void button2_Click(object sender, EventArgs e)
        {
            
            int command;
            command = IO.FAS_GetCommandPos(byte.Parse("16"), byte.Parse("0"),out a);

            MessageBox.Show(command.ToString()+ " "+ a.ToString());

            int command2;
            command2 = IO.FAS_GetActualPos(byte.Parse("16"), byte.Parse("0"), out a);

            MessageBox.Show(command2.ToString() + " " + a.ToString());

            int command3;
            command3 = IO.FAS_GetAxisStatus(byte.Parse("16"), byte.Parse("0"), out b);

            MessageBox.Show(command3.ToString() + " " + b.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timelim.Abort();
            IO.FAS_Close(byte.Parse("16"));
        }

        public void realtime()
        {
            while (true)
            {
                int command;
                command = IO.FAS_GetActualPos(byte.Parse("16"), byte.Parse("0"), out a);

                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => txtActPos.Text = a.ToString()));
                }
                else
                {
                    txtActPos.Text = a.ToString();
                }
                Thread.Sleep(100);
            }
        }

        Thread timelim;
        private void timerOn()
        {
            timelim = new Thread(() => realtime());
            timelim.Start();
        }

        private void A(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int status = IO.FAS_MoveSingleAxisAbsPos(byte.Parse("16"), byte.Parse("0"), long.Parse(txtPosition.Text),10000);
            textBox1.Text = status.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            IO.FAS_ServoEnable(byte.Parse("16"), byte.Parse("0"), true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            IO.FAS_ServoEnable(byte.Parse("16"), byte.Parse("0"), false);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            IO.FAS_MoveStop(byte.Parse("16"), byte.Parse("0"));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            IO.FAS_MovePause(byte.Parse("16"), byte.Parse("0"), true);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
