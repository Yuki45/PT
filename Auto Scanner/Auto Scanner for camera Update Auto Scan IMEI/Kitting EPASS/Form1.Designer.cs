namespace Kitting_EPASS
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSpecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendOKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbItem10 = new System.Windows.Forms.ComboBox();
            this.cmbItem9 = new System.Windows.Forms.ComboBox();
            this.cmbItem8 = new System.Windows.Forms.ComboBox();
            this.cmbItem7 = new System.Windows.Forms.ComboBox();
            this.cmbItem6 = new System.Windows.Forms.ComboBox();
            this.cmbItem5 = new System.Windows.Forms.ComboBox();
            this.cmbItem4 = new System.Windows.Forms.ComboBox();
            this.cmbItem3 = new System.Windows.Forms.ComboBox();
            this.cmbItem2 = new System.Windows.Forms.ComboBox();
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.comboBox9 = new System.Windows.Forms.ComboBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cmbItem1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.SPGMES = new System.IO.Ports.SerialPort(this.components);
            this.lblsn = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(0, 407);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(334, 31);
            this.label6.TabIndex = 15;
            this.label6.Text = "Created By MTC HHP    Ver. 1.0";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.label1.Size = new System.Drawing.Size(334, 50);
            this.label1.TabIndex = 14;
            this.label1.Text = "EPASS KITTING";
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
            this.editSpecToolStripMenuItem,
            this.sendOKToolStripMenuItem,
            this.updateToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 114);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.loadToolStripMenuItem.Text = "Config";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // editSpecToolStripMenuItem
            // 
            this.editSpecToolStripMenuItem.Name = "editSpecToolStripMenuItem";
            this.editSpecToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.editSpecToolStripMenuItem.Text = "Edit Spec";
            this.editSpecToolStripMenuItem.Click += new System.EventHandler(this.editSpecToolStripMenuItem_Click);
            // 
            // sendOKToolStripMenuItem
            // 
            this.sendOKToolStripMenuItem.Name = "sendOKToolStripMenuItem";
            this.sendOKToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.sendOKToolStripMenuItem.Text = "send OK";
            this.sendOKToolStripMenuItem.Visible = false;
            this.sendOKToolStripMenuItem.Click += new System.EventHandler(this.sendOKToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem10, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem9, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem8, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem7, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem6, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox10, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.comboBox9, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.comboBox8, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.comboBox7, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.comboBox6, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBox5, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboBox4, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBox2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbItem1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 81);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(313, 303);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(3, 270);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 33);
            this.label13.TabIndex = 49;
            this.label13.Text = "10";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(3, 240);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(25, 30);
            this.label12.TabIndex = 48;
            this.label12.Text = "9";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(3, 210);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 30);
            this.label11.TabIndex = 47;
            this.label11.Text = "8";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(3, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 30);
            this.label10.TabIndex = 46;
            this.label10.Text = "7";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(3, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 30);
            this.label9.TabIndex = 45;
            this.label9.Text = "6";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(3, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 30);
            this.label8.TabIndex = 44;
            this.label8.Text = "5";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(3, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 30);
            this.label7.TabIndex = 43;
            this.label7.Text = "4";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(3, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 30);
            this.label5.TabIndex = 42;
            this.label5.Text = "3";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(3, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 30);
            this.label4.TabIndex = 41;
            this.label4.Text = "2";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbItem10
            // 
            this.cmbItem10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem10.FormattingEnabled = true;
            this.cmbItem10.Location = new System.Drawing.Point(34, 273);
            this.cmbItem10.Name = "cmbItem10";
            this.cmbItem10.Size = new System.Drawing.Size(181, 20);
            this.cmbItem10.TabIndex = 39;
            // 
            // cmbItem9
            // 
            this.cmbItem9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem9.FormattingEnabled = true;
            this.cmbItem9.Location = new System.Drawing.Point(34, 243);
            this.cmbItem9.Name = "cmbItem9";
            this.cmbItem9.Size = new System.Drawing.Size(181, 20);
            this.cmbItem9.TabIndex = 38;
            // 
            // cmbItem8
            // 
            this.cmbItem8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem8.FormattingEnabled = true;
            this.cmbItem8.Location = new System.Drawing.Point(34, 213);
            this.cmbItem8.Name = "cmbItem8";
            this.cmbItem8.Size = new System.Drawing.Size(181, 20);
            this.cmbItem8.TabIndex = 37;
            // 
            // cmbItem7
            // 
            this.cmbItem7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem7.FormattingEnabled = true;
            this.cmbItem7.Location = new System.Drawing.Point(34, 183);
            this.cmbItem7.Name = "cmbItem7";
            this.cmbItem7.Size = new System.Drawing.Size(181, 20);
            this.cmbItem7.TabIndex = 36;
            // 
            // cmbItem6
            // 
            this.cmbItem6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem6.FormattingEnabled = true;
            this.cmbItem6.Location = new System.Drawing.Point(34, 153);
            this.cmbItem6.Name = "cmbItem6";
            this.cmbItem6.Size = new System.Drawing.Size(181, 20);
            this.cmbItem6.TabIndex = 35;
            // 
            // cmbItem5
            // 
            this.cmbItem5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem5.FormattingEnabled = true;
            this.cmbItem5.Location = new System.Drawing.Point(34, 123);
            this.cmbItem5.Name = "cmbItem5";
            this.cmbItem5.Size = new System.Drawing.Size(181, 20);
            this.cmbItem5.TabIndex = 34;
            // 
            // cmbItem4
            // 
            this.cmbItem4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem4.FormattingEnabled = true;
            this.cmbItem4.Location = new System.Drawing.Point(34, 93);
            this.cmbItem4.Name = "cmbItem4";
            this.cmbItem4.Size = new System.Drawing.Size(181, 20);
            this.cmbItem4.TabIndex = 33;
            // 
            // cmbItem3
            // 
            this.cmbItem3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem3.FormattingEnabled = true;
            this.cmbItem3.Location = new System.Drawing.Point(34, 63);
            this.cmbItem3.Name = "cmbItem3";
            this.cmbItem3.Size = new System.Drawing.Size(181, 20);
            this.cmbItem3.TabIndex = 32;
            // 
            // cmbItem2
            // 
            this.cmbItem2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem2.FormattingEnabled = true;
            this.cmbItem2.Location = new System.Drawing.Point(34, 33);
            this.cmbItem2.Name = "cmbItem2";
            this.cmbItem2.Size = new System.Drawing.Size(181, 20);
            this.cmbItem2.TabIndex = 31;
            // 
            // comboBox10
            // 
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Items.AddRange(new object[] {
            ""});
            this.comboBox10.Location = new System.Drawing.Point(221, 273);
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(89, 20);
            this.comboBox10.TabIndex = 29;
            // 
            // comboBox9
            // 
            this.comboBox9.FormattingEnabled = true;
            this.comboBox9.Items.AddRange(new object[] {
            ""});
            this.comboBox9.Location = new System.Drawing.Point(221, 243);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new System.Drawing.Size(89, 20);
            this.comboBox9.TabIndex = 26;
            // 
            // comboBox8
            // 
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            ""});
            this.comboBox8.Location = new System.Drawing.Point(221, 213);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(89, 20);
            this.comboBox8.TabIndex = 23;
            // 
            // comboBox7
            // 
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            ""});
            this.comboBox7.Location = new System.Drawing.Point(221, 183);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(89, 20);
            this.comboBox7.TabIndex = 20;
            // 
            // comboBox6
            // 
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            ""});
            this.comboBox6.Location = new System.Drawing.Point(221, 153);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(89, 20);
            this.comboBox6.TabIndex = 17;
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            ""});
            this.comboBox5.Location = new System.Drawing.Point(221, 123);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(89, 20);
            this.comboBox5.TabIndex = 14;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            ""});
            this.comboBox4.Location = new System.Drawing.Point(221, 93);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(89, 20);
            this.comboBox4.TabIndex = 11;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            ""});
            this.comboBox3.Location = new System.Drawing.Point(221, 63);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(89, 20);
            this.comboBox3.TabIndex = 8;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            ""});
            this.comboBox2.Location = new System.Drawing.Point(221, 33);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(89, 20);
            this.comboBox2.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            ""});
            this.comboBox1.Location = new System.Drawing.Point(221, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(89, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // cmbItem1
            // 
            this.cmbItem1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbItem1.FormattingEnabled = true;
            this.cmbItem1.Location = new System.Drawing.Point(34, 3);
            this.cmbItem1.Name = "cmbItem1";
            this.cmbItem1.Size = new System.Drawing.Size(181, 20);
            this.cmbItem1.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 30);
            this.label3.TabIndex = 40;
            this.label3.Text = "1";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(21, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "Model Name";
            // 
            // cmbModel
            // 
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbModel.Location = new System.Drawing.Point(115, 55);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(189, 20);
            this.cmbModel.TabIndex = 18;
            this.cmbModel.SelectedIndexChanged += new System.EventHandler(this.cmbModel_SelectedIndexChanged);
            // 
            // lblsn
            // 
            this.lblsn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblsn.Location = new System.Drawing.Point(0, 384);
            this.lblsn.Name = "lblsn";
            this.lblsn.Size = new System.Drawing.Size(334, 23);
            this.lblsn.TabIndex = 19;
            this.lblsn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 438);
            this.Controls.Add(this.lblsn);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Main_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Main_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Main_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBox10;
        private System.Windows.Forms.ComboBox comboBox9;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox cmbItem10;
        private System.Windows.Forms.ComboBox cmbItem9;
        private System.Windows.Forms.ComboBox cmbItem8;
        private System.Windows.Forms.ComboBox cmbItem7;
        private System.Windows.Forms.ComboBox cmbItem6;
        private System.Windows.Forms.ComboBox cmbItem5;
        private System.Windows.Forms.ComboBox cmbItem4;
        private System.Windows.Forms.ComboBox cmbItem3;
        private System.Windows.Forms.ComboBox cmbItem2;
        private System.Windows.Forms.ComboBox cmbItem1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.IO.Ports.SerialPort SPGMES;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem editSpecToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendOKToolStripMenuItem;
        private System.Windows.Forms.Label lblsn;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
    }
}

