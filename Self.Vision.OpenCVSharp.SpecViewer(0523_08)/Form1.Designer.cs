namespace Self.Vision.OpenCVSharp.SpecViewer
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbox1 = new System.Windows.Forms.PictureBox();
            this.pbox4 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pbox3 = new System.Windows.Forms.PictureBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.pbox2 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_BlockSize = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Threshold = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCountReset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Area = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnImageClear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_Pixel = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_Exposure = new System.Windows.Forms.TextBox();
            this.cb_Dust = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_DustThreshold = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox4)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbox3)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbox2)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbox1
            // 
            this.pbox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbox1.Location = new System.Drawing.Point(2, 3);
            this.pbox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pbox1.Name = "pbox1";
            this.pbox1.Size = new System.Drawing.Size(1089, 539);
            this.pbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox1.TabIndex = 4;
            this.pbox1.TabStop = false;
            // 
            // pbox4
            // 
            this.pbox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbox4.Location = new System.Drawing.Point(2, 3);
            this.pbox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pbox4.Name = "pbox4";
            this.pbox4.Size = new System.Drawing.Size(1089, 539);
            this.pbox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox4.TabIndex = 5;
            this.pbox4.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1101, 571);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pbox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage2.Size = new System.Drawing.Size(1093, 545);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pbox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage3.Size = new System.Drawing.Size(1093, 545);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pbox3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage4.Size = new System.Drawing.Size(1093, 545);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pbox3
            // 
            this.pbox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbox3.Location = new System.Drawing.Point(2, 3);
            this.pbox3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pbox3.Name = "pbox3";
            this.pbox3.Size = new System.Drawing.Size(1089, 539);
            this.pbox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox3.TabIndex = 6;
            this.pbox3.TabStop = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.pbox2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPage5.Size = new System.Drawing.Size(1093, 545);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // pbox2
            // 
            this.pbox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbox2.Location = new System.Drawing.Point(2, 3);
            this.pbox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pbox2.Name = "pbox2";
            this.pbox2.Size = new System.Drawing.Size(1089, 539);
            this.pbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox2.TabIndex = 5;
            this.pbox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 100);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1101, 571);
            this.panel2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(702, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Adaptive Block Size";
            // 
            // tb_BlockSize
            // 
            this.tb_BlockSize.Location = new System.Drawing.Point(812, 13);
            this.tb_BlockSize.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_BlockSize.Name = "tb_BlockSize";
            this.tb_BlockSize.Size = new System.Drawing.Size(61, 20);
            this.tb_BlockSize.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 10);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 47);
            this.button3.TabIndex = 0;
            this.button3.Text = "Cam Open";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(702, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Adaptive Threshold";
            // 
            // tb_Threshold
            // 
            this.tb_Threshold.Location = new System.Drawing.Point(812, 40);
            this.tb_Threshold.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_Threshold.Name = "tb_Threshold";
            this.tb_Threshold.Size = new System.Drawing.Size(61, 20);
            this.tb_Threshold.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(121, 10);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(73, 47);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCountReset
            // 
            this.btnCountReset.Location = new System.Drawing.Point(917, 11);
            this.btnCountReset.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCountReset.Name = "btnCountReset";
            this.btnCountReset.Size = new System.Drawing.Size(48, 47);
            this.btnCountReset.TabIndex = 6;
            this.btnCountReset.Text = "Count Reset";
            this.btnCountReset.UseVisualStyleBackColor = true;
            this.btnCountReset.Visible = false;
            this.btnCountReset.Click += new System.EventHandler(this.btnCountReset_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(916, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Spec Area";
            this.label3.Visible = false;
            // 
            // tb_Area
            // 
            this.tb_Area.Location = new System.Drawing.Point(981, 68);
            this.tb_Area.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_Area.Name = "tb_Area";
            this.tb_Area.Size = new System.Drawing.Size(60, 20);
            this.tb_Area.TabIndex = 8;
            this.tb_Area.Visible = false;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(969, 11);
            this.btnApply.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 48);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Spec Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnImageClear
            // 
            this.btnImageClear.Location = new System.Drawing.Point(197, 12);
            this.btnImageClear.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnImageClear.Name = "btnImageClear";
            this.btnImageClear.Size = new System.Drawing.Size(75, 42);
            this.btnImageClear.TabIndex = 10;
            this.btnImageClear.Text = "One Shot";
            this.btnImageClear.UseVisualStyleBackColor = true;
            this.btnImageClear.Click += new System.EventHandler(this.btnImageClear_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(747, 67);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Spec Pixel";
            // 
            // tb_Pixel
            // 
            this.tb_Pixel.Location = new System.Drawing.Point(813, 66);
            this.tb_Pixel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_Pixel.Name = "tb_Pixel";
            this.tb_Pixel.Size = new System.Drawing.Size(60, 20);
            this.tb_Pixel.TabIndex = 12;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 12);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 43);
            this.button1.TabIndex = 13;
            this.button1.Text = "Conti. Shot";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(221, 59);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Exposure";
            // 
            // tb_Exposure
            // 
            this.tb_Exposure.Location = new System.Drawing.Point(275, 57);
            this.tb_Exposure.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_Exposure.Name = "tb_Exposure";
            this.tb_Exposure.Size = new System.Drawing.Size(61, 20);
            this.tb_Exposure.TabIndex = 16;
            this.tb_Exposure.Text = "35000";
            // 
            // cb_Dust
            // 
            this.cb_Dust.AutoSize = true;
            this.cb_Dust.Location = new System.Drawing.Point(501, 12);
            this.cb_Dust.Name = "cb_Dust";
            this.cb_Dust.Size = new System.Drawing.Size(48, 17);
            this.cb_Dust.TabIndex = 17;
            this.cb_Dust.Text = "Dust";
            this.cb_Dust.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(1048, 73);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(49, 17);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "RGB";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(499, 42);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Threshold";
            this.label6.Visible = false;
            // 
            // tb_DustThreshold
            // 
            this.tb_DustThreshold.Location = new System.Drawing.Point(564, 37);
            this.tb_DustThreshold.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tb_DustThreshold.Name = "tb_DustThreshold";
            this.tb_DustThreshold.Size = new System.Drawing.Size(61, 20);
            this.tb_DustThreshold.TabIndex = 20;
            this.tb_DustThreshold.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(554, 6);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(89, 27);
            this.button4.TabIndex = 21;
            this.button4.Text = "Get Area";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.tb_DustThreshold);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.cb_Dust);
            this.panel1.Controls.Add(this.tb_Exposure);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tb_Pixel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnImageClear);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.tb_Area);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnCountReset);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.tb_Threshold);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.tb_BlockSize);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1101, 100);
            this.panel1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 671);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox4)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbox3)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbox2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pbox1;
        private System.Windows.Forms.PictureBox pbox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox pbox3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.PictureBox pbox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_BlockSize;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Threshold;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCountReset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_Area;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnImageClear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_Pixel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_Exposure;
        private System.Windows.Forms.CheckBox cb_Dust;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_DustThreshold;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
    }
}

