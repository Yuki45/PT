using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace Self.Vision.OpenCVSharp.SpecViewer
{

    public partial class Form1 : Form
    {
        bool bThreadLife = false;
        Thread proc;
        List<Bitmap> resultImages = new List<Bitmap>();
        List<PictureBox> pictureBoxes = new List<PictureBox>();
        Spec spec = new Spec();
        bool FoundArea = false;
        BaslerCamera cam; 

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(button3.Text =="Cam Open")
            {
                MessageBox.Show("Cam Open");
                return;

            }
            if (btnStart.Text == "Start")
            {
                if(cb_Dust.Checked)
                {
                    if (!FoundArea)
                    {
                        MessageBox.Show("Find Display Area First");
                        return;
                    }
                }
                btnStart.Text = "Stop";
                bThreadLife = true;

                Proc.srcFileIndex = 0;
                proc = new Thread(new ThreadStart(myThread));
                proc.IsBackground = true;
                proc.Start();

            }
            else
            {
                bThreadLife = false;
                btnStart.Text = "Start";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            
        }

        public void DisplayImage(int channel, Bitmap bmpImage)
        {

        }


        public void myThread()
        {
            while (bThreadLife)
            {
                // Mat srcImage = Proc.GetSrcImage();
                // Bitmap bmpImageSrc = (pbox1.Image) as Bitmap;

                //Bitmap bmpImageSrc = cam.OneShot_(70000); 
                //Bitmap bmpImageSrc = cam.OneShot_(Convert.ToInt32(tb_Exposure.Text));
                

                if (cb_Dust.Checked == true)
                {
                    Bitmap bmpImageSrc = cam.OneShot_(Convert.ToInt32(350000));
                    Proc.RunDust(bmpImageSrc.Clone() as Bitmap, spec, ref resultImages);

                }
                   
                else
                {
                    Bitmap bmpImageSrc = cam.OneShot_(Convert.ToInt32(70000));
                    Proc.RunRGB(bmpImageSrc.Clone() as Bitmap, spec, ref resultImages);
                }
                Proc.srcFileIndex++; 

                for (int i = 0; i < resultImages.Count; i++)
                {
                    pictureBoxes[i].Image = resultImages[i] as Image;
                }

                if (!cb_Dust.Checked && Proc.srcFileIndex == 3)
                {
                    //btnStart_Click(null, null);
                    bThreadLife = false;
                    btnStart.Text = "Start";
                }
            }
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxes.Add(pbox1);
            pictureBoxes.Add(pbox2);
            pictureBoxes.Add(pbox3);
            pictureBoxes.Add(pbox4);

            spec.BlockSize = 23;
            spec.Thres = 6;
            //spec.Area = 3;
            spec.Pixel = 3;

            tb_BlockSize.Text = spec.BlockSize.ToString();
            tb_Threshold.Text = spec.Thres.ToString();
            //tb_Area.Text = spec.Area.ToString("#.#");
            tb_Pixel.Text = spec.Pixel.ToString("#.#");
        }

        private void btnCountReset_Click(object sender, EventArgs e)
        {
            Proc.CountReset();


        }


        private void btnApply_Click(object sender, EventArgs e)
        {
            if (cb_Dust.Checked == true)
            {
                spec.Thres = int.Parse(tb_DustThreshold.Text);
            }
            else
            {
                if (int.Parse(tb_BlockSize.Text) % 2 == 0)
                {
                    MessageBox.Show("BlockSize must be odd!");
                    return;
                }
                spec.BlockSize = int.Parse(tb_BlockSize.Text);
                spec.Thres = int.Parse(tb_Threshold.Text);
                //spec.Area = double.Parse(tb_Area.Text);
                spec.Pixel = int.Parse(tb_Pixel.Text);
            }
            MessageBox.Show("spec applied");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Cam Open")
            {
                button3.Text = "Cam Close";
                cam = new BaslerCamera("acA4600");

                cam.SetPictureBox(pbox1);
            }
            else
            {
                
                if (cam != null)
                {
                    button3.Text = "Cam Open";
                    cam.Stop();
                    cam.DestroyCamera();
                }
            }

            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap areaSrc = pbox1.Image as Bitmap;
            
        }

        // One Shot
        private void btnImageClear_Click(object sender, EventArgs e)
        {
            //Bitmap bmpSrcImage = cam.OneShot_(70000);
            Bitmap bmpSrcImage = cam.OneShot_(Convert.ToInt32(tb_Exposure.Text));
            pbox1.Image = bmpSrcImage as Image;
            return;
        }

        // Conti. Shot
        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Conti. Shot")
            {
                button1.Text = "Conti. Stop";
                cam.SetLivePlay(true);
                cam.ContinuousShot();
            }
            else
            {
                button1.Text = "Conti. Shot";
                cam.SetLivePlay(false);
                cam.Stop();

            }
        }

        // Conti. Shot Stop
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //Bitmap bmpSrcImage = cam.OneShot_(70000);
            Bitmap bmpSrcImage = cam.OneShot_(Convert.ToInt32(350000));
            pbox1.Image = bmpSrcImage as Image;

            Bitmap bmpSrc = pbox1.Image as Bitmap;

            FoundArea = Proc.GetArea(bmpSrc);
        }
    }
}
