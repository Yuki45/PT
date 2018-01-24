namespace Auto_Scanner_EPASS
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
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSimulation = new System.Windows.Forms.CheckBox();
            this.nmBarcode = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtGmes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtScanner = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblmsg = new System.Windows.Forms.Label();
            this.txtsn3 = new System.Windows.Forms.TextBox();
            this.lblsn3 = new System.Windows.Forms.Label();
            this.txtsn2 = new System.Windows.Forms.TextBox();
            this.lblsn2 = new System.Windows.Forms.Label();
            this.txtsn1 = new System.Windows.Forms.TextBox();
            this.lblsn1 = new System.Windows.Forms.Label();
            this.lblTriger = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmBarcode)).BeginInit();
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
            this.label1.Size = new System.Drawing.Size(475, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "Auto Scanner EPASS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(93, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkSimulation);
            this.groupBox1.Controls.Add(this.nmBarcode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtGmes);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtScanner);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 240);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config";
            // 
            // chkSimulation
            // 
            this.chkSimulation.AutoSize = true;
            this.chkSimulation.Location = new System.Drawing.Point(8, 172);
            this.chkSimulation.Name = "chkSimulation";
            this.chkSimulation.Size = new System.Drawing.Size(93, 16);
            this.chkSimulation.TabIndex = 11;
            this.chkSimulation.Text = "Simulation";
            this.chkSimulation.UseVisualStyleBackColor = true;
            // 
            // nmBarcode
            // 
            this.nmBarcode.Location = new System.Drawing.Point(7, 209);
            this.nmBarcode.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nmBarcode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmBarcode.Name = "nmBarcode";
            this.nmBarcode.Size = new System.Drawing.Size(99, 21);
            this.nmBarcode.TabIndex = 9;
            this.nmBarcode.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nmBarcode.ValueChanged += new System.EventHandler(this.nmBarcode_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(6, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "QTY Barcode";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Config";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtGmes
            // 
            this.txtGmes.Location = new System.Drawing.Point(6, 75);
            this.txtGmes.Name = "txtGmes";
            this.txtGmes.Size = new System.Drawing.Size(100, 21);
            this.txtGmes.TabIndex = 6;
            this.txtGmes.Text = "COM15";
            this.txtGmes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(6, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "GMES";
            // 
            // txtScanner
            // 
            this.txtScanner.Location = new System.Drawing.Point(6, 32);
            this.txtScanner.Name = "txtScanner";
            this.txtScanner.Size = new System.Drawing.Size(100, 21);
            this.txtScanner.TabIndex = 4;
            this.txtScanner.Text = "COM7";
            this.txtScanner.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Scanner";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblmsg);
            this.groupBox2.Controls.Add(this.txtsn3);
            this.groupBox2.Controls.Add(this.lblsn3);
            this.groupBox2.Controls.Add(this.txtsn2);
            this.groupBox2.Controls.Add(this.lblsn2);
            this.groupBox2.Controls.Add(this.txtsn1);
            this.groupBox2.Controls.Add(this.lblsn1);
            this.groupBox2.Location = new System.Drawing.Point(136, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 240);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // lblmsg
            // 
            this.lblmsg.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblmsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblmsg.Font = new System.Drawing.Font("Gulim", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblmsg.Location = new System.Drawing.Point(3, 150);
            this.lblmsg.Name = "lblmsg";
            this.lblmsg.Size = new System.Drawing.Size(321, 87);
            this.lblmsg.TabIndex = 14;
            this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtsn3
            // 
            this.txtsn3.Location = new System.Drawing.Point(8, 117);
            this.txtsn3.Name = "txtsn3";
            this.txtsn3.Size = new System.Drawing.Size(313, 21);
            this.txtsn3.TabIndex = 9;
            this.txtsn3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblsn3
            // 
            this.lblsn3.AutoSize = true;
            this.lblsn3.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsn3.Location = new System.Drawing.Point(6, 102);
            this.lblsn3.Name = "lblsn3";
            this.lblsn3.Size = new System.Drawing.Size(36, 12);
            this.lblsn3.TabIndex = 8;
            this.lblsn3.Text = "SN 3";
            // 
            // txtsn2
            // 
            this.txtsn2.Location = new System.Drawing.Point(8, 74);
            this.txtsn2.Name = "txtsn2";
            this.txtsn2.Size = new System.Drawing.Size(313, 21);
            this.txtsn2.TabIndex = 7;
            this.txtsn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtsn2.TextChanged += new System.EventHandler(this.txtsn2_TextChanged);
            // 
            // lblsn2
            // 
            this.lblsn2.AutoSize = true;
            this.lblsn2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsn2.Location = new System.Drawing.Point(6, 59);
            this.lblsn2.Name = "lblsn2";
            this.lblsn2.Size = new System.Drawing.Size(36, 12);
            this.lblsn2.TabIndex = 6;
            this.lblsn2.Text = "SN 2";
            // 
            // txtsn1
            // 
            this.txtsn1.Location = new System.Drawing.Point(8, 32);
            this.txtsn1.Name = "txtsn1";
            this.txtsn1.Size = new System.Drawing.Size(313, 21);
            this.txtsn1.TabIndex = 5;
            this.txtsn1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtsn1.TextChanged += new System.EventHandler(this.txtsn1_TextChanged);
            this.txtsn1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtsn1_KeyDown);
            // 
            // lblsn1
            // 
            this.lblsn1.AutoSize = true;
            this.lblsn1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsn1.Location = new System.Drawing.Point(6, 17);
            this.lblsn1.Name = "lblsn1";
            this.lblsn1.Size = new System.Drawing.Size(36, 12);
            this.lblsn1.TabIndex = 4;
            this.lblsn1.Text = "SN 1";
            // 
            // lblTriger
            // 
            this.lblTriger.BackColor = System.Drawing.Color.Red;
            this.lblTriger.Location = new System.Drawing.Point(10, 9);
            this.lblTriger.Name = "lblTriger";
            this.lblTriger.Size = new System.Drawing.Size(26, 31);
            this.lblTriger.TabIndex = 10;
            this.lblTriger.DoubleClick += new System.EventHandler(this.lblTriger_DoubleClick);
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(0, 298);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(475, 31);
            this.label6.TabIndex = 11;
            this.label6.Text = "Created By MTC HHP    Ver. 1.0";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 114);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "COM17";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(5, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "GMES";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 329);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblTriger);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmBarcode)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nmBarcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtGmes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtScanner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtsn3;
        private System.Windows.Forms.Label lblsn3;
        private System.Windows.Forms.TextBox txtsn2;
        private System.Windows.Forms.Label lblsn2;
        private System.Windows.Forms.TextBox txtsn1;
        private System.Windows.Forms.Label lblsn1;
        private System.Windows.Forms.Label lblTriger;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label lblmsg;
        private System.Windows.Forms.CheckBox chkSimulation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}