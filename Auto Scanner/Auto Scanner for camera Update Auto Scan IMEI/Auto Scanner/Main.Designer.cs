namespace Auto_Scanner
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadVSPEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbscanner = new System.Windows.Forms.ComboBox();
            this.cmbgmes = new System.Windows.Forms.ComboBox();
            this.lblInScanner = new System.Windows.Forms.Label();
            this.lblInGmes = new System.Windows.Forms.Label();
            this.txtSn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblstatus = new System.Windows.Forms.Label();
            this.SPScanner = new System.IO.Ports.SerialPort(this.components);
            this.SPGMES = new System.IO.Ports.SerialPort(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.nmLenght = new System.Windows.Forms.NumericUpDown();
            this.lblTriger = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblmsg = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblWS = new System.Windows.Forms.Label();
            this.cmbWS = new System.Windows.Forms.ComboBox();
            this.txtWeightScale = new System.Windows.Forms.TextBox();
            this.SPWS = new System.IO.Ports.SerialPort(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblcount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nmSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkWG = new System.Windows.Forms.CheckBox();
            this.chkFP = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLPT = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmLenght)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(487, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Auto Scanner Manager";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.loadVSPEToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.loadToolStripMenuItem.Text = "Load COM";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // loadVSPEToolStripMenuItem
            // 
            this.loadVSPEToolStripMenuItem.Name = "loadVSPEToolStripMenuItem";
            this.loadVSPEToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.loadVSPEToolStripMenuItem.Text = "Load VSPE";
            this.loadVSPEToolStripMenuItem.Click += new System.EventHandler(this.loadVSPEToolStripMenuItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "COM Scanner";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "COM GMES";
            // 
            // cmbscanner
            // 
            this.cmbscanner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbscanner.FormattingEnabled = true;
            this.cmbscanner.Location = new System.Drawing.Point(136, 60);
            this.cmbscanner.Name = "cmbscanner";
            this.cmbscanner.Size = new System.Drawing.Size(93, 26);
            this.cmbscanner.TabIndex = 3;
            this.cmbscanner.SelectedIndexChanged += new System.EventHandler(this.cmbscanner_SelectedIndexChanged);
            // 
            // cmbgmes
            // 
            this.cmbgmes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbgmes.FormattingEnabled = true;
            this.cmbgmes.Location = new System.Drawing.Point(136, 97);
            this.cmbgmes.Name = "cmbgmes";
            this.cmbgmes.Size = new System.Drawing.Size(93, 26);
            this.cmbgmes.TabIndex = 4;
            this.cmbgmes.SelectedIndexChanged += new System.EventHandler(this.cmbgmes_SelectedIndexChanged);
            // 
            // lblInScanner
            // 
            this.lblInScanner.BackColor = System.Drawing.Color.Lime;
            this.lblInScanner.Location = new System.Drawing.Point(235, 60);
            this.lblInScanner.Name = "lblInScanner";
            this.lblInScanner.Size = new System.Drawing.Size(24, 26);
            this.lblInScanner.TabIndex = 5;
            // 
            // lblInGmes
            // 
            this.lblInGmes.BackColor = System.Drawing.Color.Lime;
            this.lblInGmes.Location = new System.Drawing.Point(235, 97);
            this.lblInGmes.Name = "lblInGmes";
            this.lblInGmes.Size = new System.Drawing.Size(24, 26);
            this.lblInGmes.TabIndex = 6;
            // 
            // txtSn
            // 
            this.txtSn.BackColor = System.Drawing.Color.Yellow;
            this.txtSn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSn.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSn.Location = new System.Drawing.Point(6, 193);
            this.txtSn.Name = "txtSn";
            this.txtSn.Size = new System.Drawing.Size(253, 31);
            this.txtSn.TabIndex = 7;
            this.txtSn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSn.TextChanged += new System.EventHandler(this.txtSn_TextChanged);
            this.txtSn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSn_KeyDown);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(0, 295);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(487, 31);
            this.label6.TabIndex = 8;
            this.label6.Text = "Created By MTC HHP    Ver. 1.5.6";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblstatus);
            this.panel1.Location = new System.Drawing.Point(268, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(102, 102);
            this.panel1.TabIndex = 9;
            // 
            // lblstatus
            // 
            this.lblstatus.BackColor = System.Drawing.Color.Yellow;
            this.lblstatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblstatus.Font = new System.Drawing.Font("Arial Rounded MT Bold", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblstatus.Location = new System.Drawing.Point(0, 0);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(102, 102);
            this.lblstatus.TabIndex = 0;
            this.lblstatus.Text = "RD";
            this.lblstatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SPScanner
            // 
            this.SPScanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SPScanner_DataReceived);
            // 
            // SPGMES
            // 
            this.SPGMES.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SPGMES_DataReceived);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(13, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Lenght";
            // 
            // nmLenght
            // 
            this.nmLenght.Location = new System.Drawing.Point(137, 164);
            this.nmLenght.Name = "nmLenght";
            this.nmLenght.Size = new System.Drawing.Size(120, 26);
            this.nmLenght.TabIndex = 11;
            // 
            // lblTriger
            // 
            this.lblTriger.BackColor = System.Drawing.Color.Red;
            this.lblTriger.Location = new System.Drawing.Point(263, 193);
            this.lblTriger.Name = "lblTriger";
            this.lblTriger.Size = new System.Drawing.Size(26, 31);
            this.lblTriger.TabIndex = 6;
            this.lblTriger.Click += new System.EventHandler(this.lblTriger_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(295, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 12;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblmsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblmsg.Location = new System.Drawing.Point(0, 266);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(487, 29);
            this.lblmsg.TabIndex = 13;
            this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 18);
            this.label5.TabIndex = 14;
            this.label5.Text = "COM W.Scale";
            // 
            // lblWS
            // 
            this.lblWS.BackColor = System.Drawing.Color.Lime;
            this.lblWS.Location = new System.Drawing.Point(236, 132);
            this.lblWS.Name = "lblWS";
            this.lblWS.Size = new System.Drawing.Size(24, 26);
            this.lblWS.TabIndex = 16;
            // 
            // cmbWS
            // 
            this.cmbWS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWS.FormattingEnabled = true;
            this.cmbWS.Location = new System.Drawing.Point(137, 132);
            this.cmbWS.Name = "cmbWS";
            this.cmbWS.Size = new System.Drawing.Size(93, 26);
            this.cmbWS.TabIndex = 15;
            this.cmbWS.SelectedIndexChanged += new System.EventHandler(this.cmbWS_SelectedIndexChanged);
            // 
            // txtWeightScale
            // 
            this.txtWeightScale.BackColor = System.Drawing.Color.Yellow;
            this.txtWeightScale.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWeightScale.Location = new System.Drawing.Point(6, 230);
            this.txtWeightScale.Name = "txtWeightScale";
            this.txtWeightScale.ReadOnly = true;
            this.txtWeightScale.Size = new System.Drawing.Size(253, 31);
            this.txtWeightScale.TabIndex = 17;
            this.txtWeightScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWeightScale.TextChanged += new System.EventHandler(this.txtWeightScale_TextChanged);
            // 
            // SPWS
            // 
            this.SPWS.DataBits = 7;
            this.SPWS.Parity = System.IO.Ports.Parity.Even;
            this.SPWS.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SPWS_DataReceived);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblcount);
            this.panel2.Location = new System.Drawing.Point(376, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(102, 102);
            this.panel2.TabIndex = 18;
            // 
            // lblcount
            // 
            this.lblcount.BackColor = System.Drawing.Color.Yellow;
            this.lblcount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblcount.Font = new System.Drawing.Font("Arial Rounded MT Bold", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcount.Location = new System.Drawing.Point(0, 0);
            this.lblcount.Name = "lblcount";
            this.lblcount.Size = new System.Drawing.Size(102, 102);
            this.lblcount.TabIndex = 0;
            this.lblcount.Text = "0";
            this.lblcount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(267, 358);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 18);
            this.label7.TabIndex = 19;
            this.label7.Text = "Size";
            this.label7.Visible = false;
            // 
            // nmSize
            // 
            this.nmSize.Location = new System.Drawing.Point(314, 365);
            this.nmSize.Name = "nmSize";
            this.nmSize.Size = new System.Drawing.Size(56, 26);
            this.nmSize.TabIndex = 20;
            this.nmSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nmSize.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkWG);
            this.groupBox1.Controls.Add(this.chkFP);
            this.groupBox1.Location = new System.Drawing.Point(378, 193);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(99, 68);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // chkWG
            // 
            this.chkWG.AutoSize = true;
            this.chkWG.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWG.Location = new System.Drawing.Point(7, 30);
            this.chkWG.Name = "chkWG";
            this.chkWG.Size = new System.Drawing.Size(76, 18);
            this.chkWG.TabIndex = 1;
            this.chkWG.Text = "W. Scale";
            this.chkWG.UseVisualStyleBackColor = true;
            this.chkWG.Click += new System.EventHandler(this.chkWG_Click);
            // 
            // chkFP
            // 
            this.chkFP.AutoSize = true;
            this.chkFP.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFP.Location = new System.Drawing.Point(7, 14);
            this.chkFP.Name = "chkFP";
            this.chkFP.Size = new System.Drawing.Size(81, 18);
            this.chkFP.TabIndex = 0;
            this.chkFP.Text = "Foolproof";
            this.chkFP.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Lime;
            this.button2.Location = new System.Drawing.Point(265, 230);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 31);
            this.button2.TabIndex = 22;
            this.button2.Text = "W.Scale Config";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DodgerBlue;
            this.button3.Location = new System.Drawing.Point(378, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 31);
            this.button3.TabIndex = 23;
            this.button3.Text = "Config";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtLPT);
            this.groupBox2.Location = new System.Drawing.Point(377, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(100, 41);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // txtLPT
            // 
            this.txtLPT.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLPT.Location = new System.Drawing.Point(4, 12);
            this.txtLPT.Multiline = true;
            this.txtLPT.Name = "txtLPT";
            this.txtLPT.Size = new System.Drawing.Size(89, 24);
            this.txtLPT.TabIndex = 0;
            this.txtLPT.Text = "0x378";
            this.txtLPT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(487, 326);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nmSize);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtWeightScale);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblWS);
            this.Controls.Add(this.cmbWS);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblmsg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.nmLenght);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSn);
            this.Controls.Add(this.lblInGmes);
            this.Controls.Add(this.lblTriger);
            this.Controls.Add(this.lblInScanner);
            this.Controls.Add(this.cmbgmes);
            this.Controls.Add(this.cmbscanner);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.Text = "Auto Scanner Manager";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmLenght)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nmSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbscanner;
        private System.Windows.Forms.ComboBox cmbgmes;
        private System.Windows.Forms.Label lblInScanner;
        private System.Windows.Forms.Label lblInGmes;
        private System.Windows.Forms.TextBox txtSn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblstatus;
        private System.IO.Ports.SerialPort SPScanner;
        private System.IO.Ports.SerialPort SPGMES;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nmLenght;
        private System.Windows.Forms.Label lblTriger;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblWS;
        private System.Windows.Forms.ComboBox cmbWS;
        private System.Windows.Forms.TextBox txtWeightScale;
        private System.IO.Ports.SerialPort SPWS;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblcount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nmSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkWG;
        private System.Windows.Forms.CheckBox chkFP;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadVSPEToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtLPT;
    }
}

