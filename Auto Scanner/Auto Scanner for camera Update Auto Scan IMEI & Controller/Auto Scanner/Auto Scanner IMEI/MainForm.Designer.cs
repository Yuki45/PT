namespace Auto_Scanner_IMEI
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.jig1 = new System.Windows.Forms.Label();
            this.jig2 = new System.Windows.Forms.Label();
            this.jig3 = new System.Windows.Forms.Label();
            this.jig4 = new System.Windows.Forms.Label();
            this.jig5 = new System.Windows.Forms.Label();
            this.jig10 = new System.Windows.Forms.Label();
            this.jig9 = new System.Windows.Forms.Label();
            this.jig8 = new System.Windows.Forms.Label();
            this.jig7 = new System.Windows.Forms.Label();
            this.jig6 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.JIG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label11 = new System.Windows.Forms.Label();
            this.txtCom = new System.Windows.Forms.TextBox();
            this.txtActPos = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtIO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtport = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveAbsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveIncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.txtvelocity = new System.Windows.Forms.TextBox();
            this.btnOrigin = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnOn = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.txtCmdPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblJig = new System.Windows.Forms.Label();
            this.btnJogP = new System.Windows.Forms.Button();
            this.btnJogM = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtScanner = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPGM = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SnJig1 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SnJig2 = new System.Windows.Forms.Label();
            this.SnJig3 = new System.Windows.Forms.Label();
            this.SnJig4 = new System.Windows.Forms.Label();
            this.SnJig5 = new System.Windows.Forms.Label();
            this.SnJig6 = new System.Windows.Forms.Label();
            this.SnJig7 = new System.Windows.Forms.Label();
            this.SnJig8 = new System.Windows.Forms.Label();
            this.SnJig9 = new System.Windows.Forms.Label();
            this.SnJig10 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.gbConfig.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // jig1
            // 
            this.jig1.BackColor = System.Drawing.Color.Silver;
            this.jig1.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig1.Location = new System.Drawing.Point(9, 7);
            this.jig1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig1.Name = "jig1";
            this.jig1.Size = new System.Drawing.Size(40, 29);
            this.jig1.TabIndex = 0;
            this.jig1.Text = "1";
            this.jig1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig1.Click += new System.EventHandler(this.jig1_Click);
            // 
            // jig2
            // 
            this.jig2.BackColor = System.Drawing.Color.Silver;
            this.jig2.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig2.Location = new System.Drawing.Point(55, 7);
            this.jig2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig2.Name = "jig2";
            this.jig2.Size = new System.Drawing.Size(40, 29);
            this.jig2.TabIndex = 1;
            this.jig2.Text = "2";
            this.jig2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig2.Click += new System.EventHandler(this.jig2_Click);
            // 
            // jig3
            // 
            this.jig3.BackColor = System.Drawing.Color.Silver;
            this.jig3.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig3.Location = new System.Drawing.Point(102, 7);
            this.jig3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig3.Name = "jig3";
            this.jig3.Size = new System.Drawing.Size(40, 29);
            this.jig3.TabIndex = 2;
            this.jig3.Text = "3";
            this.jig3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig3.Click += new System.EventHandler(this.jig3_Click);
            // 
            // jig4
            // 
            this.jig4.BackColor = System.Drawing.Color.Silver;
            this.jig4.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig4.Location = new System.Drawing.Point(150, 7);
            this.jig4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig4.Name = "jig4";
            this.jig4.Size = new System.Drawing.Size(40, 29);
            this.jig4.TabIndex = 3;
            this.jig4.Text = "4";
            this.jig4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig4.Click += new System.EventHandler(this.jig4_Click);
            // 
            // jig5
            // 
            this.jig5.BackColor = System.Drawing.Color.Silver;
            this.jig5.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig5.Location = new System.Drawing.Point(200, 7);
            this.jig5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig5.Name = "jig5";
            this.jig5.Size = new System.Drawing.Size(40, 29);
            this.jig5.TabIndex = 4;
            this.jig5.Text = "5";
            this.jig5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig5.Click += new System.EventHandler(this.jig5_Click);
            // 
            // jig10
            // 
            this.jig10.BackColor = System.Drawing.Color.Silver;
            this.jig10.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig10.Location = new System.Drawing.Point(200, 54);
            this.jig10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig10.Name = "jig10";
            this.jig10.Size = new System.Drawing.Size(40, 29);
            this.jig10.TabIndex = 9;
            this.jig10.Text = "10";
            this.jig10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig10.Click += new System.EventHandler(this.jig10_Click);
            // 
            // jig9
            // 
            this.jig9.BackColor = System.Drawing.Color.Silver;
            this.jig9.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig9.Location = new System.Drawing.Point(150, 54);
            this.jig9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig9.Name = "jig9";
            this.jig9.Size = new System.Drawing.Size(40, 29);
            this.jig9.TabIndex = 8;
            this.jig9.Text = "9";
            this.jig9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig9.Click += new System.EventHandler(this.jig9_Click);
            // 
            // jig8
            // 
            this.jig8.BackColor = System.Drawing.Color.Silver;
            this.jig8.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig8.Location = new System.Drawing.Point(102, 54);
            this.jig8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig8.Name = "jig8";
            this.jig8.Size = new System.Drawing.Size(40, 29);
            this.jig8.TabIndex = 7;
            this.jig8.Text = "8";
            this.jig8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig8.Click += new System.EventHandler(this.jig8_Click);
            // 
            // jig7
            // 
            this.jig7.BackColor = System.Drawing.Color.Silver;
            this.jig7.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig7.Location = new System.Drawing.Point(55, 54);
            this.jig7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig7.Name = "jig7";
            this.jig7.Size = new System.Drawing.Size(40, 29);
            this.jig7.TabIndex = 6;
            this.jig7.Text = "7";
            this.jig7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig7.Click += new System.EventHandler(this.jig7_Click);
            // 
            // jig6
            // 
            this.jig6.BackColor = System.Drawing.Color.Silver;
            this.jig6.Font = new System.Drawing.Font("Gulim", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(50)));
            this.jig6.Location = new System.Drawing.Point(9, 54);
            this.jig6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.jig6.Name = "jig6";
            this.jig6.Size = new System.Drawing.Size(40, 29);
            this.jig6.TabIndex = 5;
            this.jig6.Text = "6";
            this.jig6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.jig6.Click += new System.EventHandler(this.jig6_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.JIG});
            this.listView1.Location = new System.Drawing.Point(247, 9);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(88, 112);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // JIG
            // 
            this.JIG.Text = "JIG QUE\'E";
            this.JIG.Width = 150;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(3, 133);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 11);
            this.label11.TabIndex = 11;
            this.label11.Text = "Servo";
            // 
            // txtCom
            // 
            this.txtCom.Location = new System.Drawing.Point(5, 144);
            this.txtCom.Margin = new System.Windows.Forms.Padding(2);
            this.txtCom.Name = "txtCom";
            this.txtCom.Size = new System.Drawing.Size(73, 18);
            this.txtCom.TabIndex = 12;
            this.txtCom.Text = "COM16";
            this.txtCom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtActPos
            // 
            this.txtActPos.Location = new System.Drawing.Point(155, 140);
            this.txtActPos.Margin = new System.Windows.Forms.Padding(2);
            this.txtActPos.Name = "txtActPos";
            this.txtActPos.ReadOnly = true;
            this.txtActPos.Size = new System.Drawing.Size(88, 18);
            this.txtActPos.TabIndex = 13;
            this.txtActPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.Yellow;
            this.btnConnect.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.btnConnect.Location = new System.Drawing.Point(81, 113);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(70, 23);
            this.btnConnect.TabIndex = 16;
            this.btnConnect.Text = "Connected";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtIO
            // 
            this.txtIO.Location = new System.Drawing.Point(4, 113);
            this.txtIO.Margin = new System.Windows.Forms.Padding(2);
            this.txtIO.Name = "txtIO";
            this.txtIO.Size = new System.Drawing.Size(75, 18);
            this.txtIO.TabIndex = 18;
            this.txtIO.Text = "COM17";
            this.txtIO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(4, 102);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 11);
            this.label1.TabIndex = 17;
            this.label1.Text = "I/O";
            // 
            // txtport
            // 
            this.txtport.Location = new System.Drawing.Point(155, 118);
            this.txtport.Margin = new System.Windows.Forms.Padding(2);
            this.txtport.Name = "txtport";
            this.txtport.ReadOnly = true;
            this.txtport.Size = new System.Drawing.Size(88, 18);
            this.txtport.TabIndex = 19;
            this.txtport.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtport.TextChanged += new System.EventHandler(this.txtport_TextChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // gbConfig
            // 
            this.gbConfig.ContextMenuStrip = this.contextMenuStrip1;
            this.gbConfig.Controls.Add(this.label3);
            this.gbConfig.Controls.Add(this.txtvelocity);
            this.gbConfig.Controls.Add(this.btnOrigin);
            this.gbConfig.Controls.Add(this.btnReset);
            this.gbConfig.Controls.Add(this.btnOff);
            this.gbConfig.Controls.Add(this.btnOn);
            this.gbConfig.Controls.Add(this.button6);
            this.gbConfig.Controls.Add(this.button5);
            this.gbConfig.Controls.Add(this.txtCmdPos);
            this.gbConfig.Controls.Add(this.label4);
            this.gbConfig.Controls.Add(this.lblJig);
            this.gbConfig.Controls.Add(this.btnJogP);
            this.gbConfig.Controls.Add(this.btnJogM);
            this.gbConfig.Controls.Add(this.label2);
            this.gbConfig.Enabled = false;
            this.gbConfig.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.gbConfig.Location = new System.Drawing.Point(247, 2);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(93, 198);
            this.gbConfig.TabIndex = 23;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "CONFIG";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveAbsToolStripMenuItem,
            this.moveIncToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(122, 48);
            // 
            // moveAbsToolStripMenuItem
            // 
            this.moveAbsToolStripMenuItem.Name = "moveAbsToolStripMenuItem";
            this.moveAbsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.moveAbsToolStripMenuItem.Text = "Move Abs";
            this.moveAbsToolStripMenuItem.Click += new System.EventHandler(this.moveAbsToolStripMenuItem_Click);
            // 
            // moveIncToolStripMenuItem
            // 
            this.moveIncToolStripMenuItem.Name = "moveIncToolStripMenuItem";
            this.moveIncToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.moveIncToolStripMenuItem.Text = "Move Inc";
            this.moveIncToolStripMenuItem.Click += new System.EventHandler(this.moveIncToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(6, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 9);
            this.label3.TabIndex = 38;
            this.label3.Text = "Normal Velocity";
            // 
            // txtvelocity
            // 
            this.txtvelocity.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtvelocity.Location = new System.Drawing.Point(7, 170);
            this.txtvelocity.Margin = new System.Windows.Forms.Padding(2);
            this.txtvelocity.Name = "txtvelocity";
            this.txtvelocity.ReadOnly = true;
            this.txtvelocity.Size = new System.Drawing.Size(79, 18);
            this.txtvelocity.TabIndex = 37;
            this.txtvelocity.Text = "500000";
            this.txtvelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtvelocity.TextChanged += new System.EventHandler(this.txtvelocity_TextChanged);
            // 
            // btnOrigin
            // 
            this.btnOrigin.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOrigin.Location = new System.Drawing.Point(6, 112);
            this.btnOrigin.Margin = new System.Windows.Forms.Padding(2);
            this.btnOrigin.Name = "btnOrigin";
            this.btnOrigin.Size = new System.Drawing.Size(40, 21);
            this.btnOrigin.TabIndex = 36;
            this.btnOrigin.Text = "Origin";
            this.btnOrigin.UseVisualStyleBackColor = true;
            this.btnOrigin.Click += new System.EventHandler(this.btnOrigin_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(6, 134);
            this.btnReset.Margin = new System.Windows.Forms.Padding(2);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(40, 21);
            this.btnReset.TabIndex = 35;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOff
            // 
            this.btnOff.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOff.Location = new System.Drawing.Point(47, 134);
            this.btnOff.Margin = new System.Windows.Forms.Padding(2);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(40, 21);
            this.btnOff.TabIndex = 34;
            this.btnOff.Text = "MOVE";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnOn
            // 
            this.btnOn.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOn.Location = new System.Drawing.Point(47, 113);
            this.btnOn.Margin = new System.Windows.Forms.Padding(2);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(40, 21);
            this.btnOn.TabIndex = 33;
            this.btnOn.Text = "ON/OFF";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button6.Location = new System.Drawing.Point(47, 69);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 21);
            this.button6.TabIndex = 32;
            this.button6.Text = "GET";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Gulim", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.button5.Location = new System.Drawing.Point(6, 69);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(40, 21);
            this.button5.TabIndex = 31;
            this.button5.Text = "SAVE";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // txtCmdPos
            // 
            this.txtCmdPos.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtCmdPos.Location = new System.Drawing.Point(6, 48);
            this.txtCmdPos.Margin = new System.Windows.Forms.Padding(2);
            this.txtCmdPos.Name = "txtCmdPos";
            this.txtCmdPos.ReadOnly = true;
            this.txtCmdPos.Size = new System.Drawing.Size(79, 18);
            this.txtCmdPos.TabIndex = 30;
            this.txtCmdPos.Text = "0";
            this.txtCmdPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(16, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 9);
            this.label4.TabIndex = 29;
            this.label4.Text = "CMD POS";
            // 
            // lblJig
            // 
            this.lblJig.AutoSize = true;
            this.lblJig.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblJig.Location = new System.Drawing.Point(46, 15);
            this.lblJig.Name = "lblJig";
            this.lblJig.Size = new System.Drawing.Size(16, 15);
            this.lblJig.TabIndex = 28;
            this.lblJig.Text = "1";
            // 
            // btnJogP
            // 
            this.btnJogP.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnJogP.Location = new System.Drawing.Point(47, 91);
            this.btnJogP.Margin = new System.Windows.Forms.Padding(2);
            this.btnJogP.Name = "btnJogP";
            this.btnJogP.Size = new System.Drawing.Size(40, 21);
            this.btnJogP.TabIndex = 27;
            this.btnJogP.Text = "JOG+";
            this.btnJogP.UseVisualStyleBackColor = true;
            this.btnJogP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogP_MouseDown);
            this.btnJogP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogP_MouseUp);
            // 
            // btnJogM
            // 
            this.btnJogM.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnJogM.Location = new System.Drawing.Point(6, 90);
            this.btnJogM.Margin = new System.Windows.Forms.Padding(2);
            this.btnJogM.Name = "btnJogM";
            this.btnJogM.Size = new System.Drawing.Size(40, 21);
            this.btnJogM.TabIndex = 26;
            this.btnJogM.Text = "JOG-";
            this.btnJogM.UseVisualStyleBackColor = true;
            this.btnJogM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogM_MouseDown);
            this.btnJogM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogM_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(16, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "JIG";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(104, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(219, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "Created By Yuki.F MTC HHP";
            // 
            // txtScanner
            // 
            this.txtScanner.Location = new System.Drawing.Point(4, 175);
            this.txtScanner.Margin = new System.Windows.Forms.Padding(2);
            this.txtScanner.Name = "txtScanner";
            this.txtScanner.Size = new System.Drawing.Size(73, 18);
            this.txtScanner.TabIndex = 27;
            this.txtScanner.Text = "COM16";
            this.txtScanner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(1, 164);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 11);
            this.label6.TabIndex = 26;
            this.label6.Text = "Scanner";
            // 
            // txtPGM
            // 
            this.txtPGM.Location = new System.Drawing.Point(4, 206);
            this.txtPGM.Margin = new System.Windows.Forms.Padding(2);
            this.txtPGM.Name = "txtPGM";
            this.txtPGM.Size = new System.Drawing.Size(73, 18);
            this.txtPGM.TabIndex = 29;
            this.txtPGM.Text = "COM16";
            this.txtPGM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(2, 195);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 11);
            this.label7.TabIndex = 28;
            this.label7.Text = "PGM";
            // 
            // SnJig1
            // 
            this.SnJig1.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig1.Location = new System.Drawing.Point(9, 39);
            this.SnJig1.Name = "SnJig1";
            this.SnJig1.Size = new System.Drawing.Size(38, 12);
            this.SnJig1.TabIndex = 30;
            this.SnJig1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSN
            // 
            this.txtSN.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtSN.Location = new System.Drawing.Point(81, 181);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(162, 21);
            this.txtSN.TabIndex = 31;
            this.txtSN.TextChanged += new System.EventHandler(this.txtSN_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Gulim", 8F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(118, 167);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 11);
            this.label8.TabIndex = 32;
            this.label8.Text = "Barcode S/N";
            // 
            // SnJig2
            // 
            this.SnJig2.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig2.Location = new System.Drawing.Point(56, 39);
            this.SnJig2.Name = "SnJig2";
            this.SnJig2.Size = new System.Drawing.Size(38, 12);
            this.SnJig2.TabIndex = 33;
            this.SnJig2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig3
            // 
            this.SnJig3.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig3.Location = new System.Drawing.Point(103, 39);
            this.SnJig3.Name = "SnJig3";
            this.SnJig3.Size = new System.Drawing.Size(38, 12);
            this.SnJig3.TabIndex = 34;
            this.SnJig3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig4
            // 
            this.SnJig4.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig4.Location = new System.Drawing.Point(152, 39);
            this.SnJig4.Name = "SnJig4";
            this.SnJig4.Size = new System.Drawing.Size(38, 12);
            this.SnJig4.TabIndex = 35;
            this.SnJig4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig5
            // 
            this.SnJig5.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig5.Location = new System.Drawing.Point(201, 39);
            this.SnJig5.Name = "SnJig5";
            this.SnJig5.Size = new System.Drawing.Size(38, 12);
            this.SnJig5.TabIndex = 36;
            this.SnJig5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig6
            // 
            this.SnJig6.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig6.Location = new System.Drawing.Point(10, 86);
            this.SnJig6.Name = "SnJig6";
            this.SnJig6.Size = new System.Drawing.Size(38, 12);
            this.SnJig6.TabIndex = 37;
            this.SnJig6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig7
            // 
            this.SnJig7.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig7.Location = new System.Drawing.Point(57, 86);
            this.SnJig7.Name = "SnJig7";
            this.SnJig7.Size = new System.Drawing.Size(38, 12);
            this.SnJig7.TabIndex = 38;
            this.SnJig7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig8
            // 
            this.SnJig8.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig8.Location = new System.Drawing.Point(103, 86);
            this.SnJig8.Name = "SnJig8";
            this.SnJig8.Size = new System.Drawing.Size(38, 12);
            this.SnJig8.TabIndex = 39;
            this.SnJig8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig9
            // 
            this.SnJig9.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig9.Location = new System.Drawing.Point(152, 86);
            this.SnJig9.Name = "SnJig9";
            this.SnJig9.Size = new System.Drawing.Size(38, 12);
            this.SnJig9.TabIndex = 40;
            this.SnJig9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SnJig10
            // 
            this.SnJig10.BackColor = System.Drawing.Color.Chartreuse;
            this.SnJig10.Location = new System.Drawing.Point(201, 86);
            this.SnJig10.Name = "SnJig10";
            this.SnJig10.Size = new System.Drawing.Size(38, 12);
            this.SnJig10.TabIndex = 41;
            this.SnJig10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 307);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(337, 150);
            this.richTextBox1.TabIndex = 42;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 386);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 43;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(81, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.TabIndex = 44;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(4, 229);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(336, 62);
            this.richTextBox2.TabIndex = 45;
            this.richTextBox2.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 229);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.SnJig10);
            this.Controls.Add(this.SnJig9);
            this.Controls.Add(this.SnJig8);
            this.Controls.Add(this.SnJig7);
            this.Controls.Add(this.SnJig6);
            this.Controls.Add(this.SnJig5);
            this.Controls.Add(this.SnJig4);
            this.Controls.Add(this.SnJig3);
            this.Controls.Add(this.SnJig2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.SnJig1);
            this.Controls.Add(this.txtPGM);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtScanner);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.gbConfig);
            this.Controls.Add(this.txtport);
            this.Controls.Add(this.txtIO);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtActPos);
            this.Controls.Add(this.txtCom);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.jig10);
            this.Controls.Add(this.jig9);
            this.Controls.Add(this.jig8);
            this.Controls.Add(this.jig7);
            this.Controls.Add(this.jig6);
            this.Controls.Add(this.jig5);
            this.Controls.Add(this.jig4);
            this.Controls.Add(this.jig3);
            this.Controls.Add(this.jig2);
            this.Controls.Add(this.jig1);
            this.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "EIN Semi Comlpex v2.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label jig1;
        private System.Windows.Forms.Label jig2;
        private System.Windows.Forms.Label jig3;
        private System.Windows.Forms.Label jig4;
        private System.Windows.Forms.Label jig5;
        private System.Windows.Forms.Label jig10;
        private System.Windows.Forms.Label jig9;
        private System.Windows.Forms.Label jig8;
        private System.Windows.Forms.Label jig7;
        private System.Windows.Forms.Label jig6;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader JIG;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCom;
        private System.Windows.Forms.TextBox txtActPos;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtIO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtport;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtCmdPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblJig;
        private System.Windows.Forms.Button btnJogP;
        private System.Windows.Forms.Button btnJogM;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnOrigin;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem moveAbsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveIncToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtvelocity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtScanner;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPGM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label SnJig1;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label SnJig2;
        private System.Windows.Forms.Label SnJig3;
        private System.Windows.Forms.Label SnJig4;
        private System.Windows.Forms.Label SnJig5;
        private System.Windows.Forms.Label SnJig6;
        private System.Windows.Forms.Label SnJig7;
        private System.Windows.Forms.Label SnJig8;
        private System.Windows.Forms.Label SnJig9;
        private System.Windows.Forms.Label SnJig10;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}

