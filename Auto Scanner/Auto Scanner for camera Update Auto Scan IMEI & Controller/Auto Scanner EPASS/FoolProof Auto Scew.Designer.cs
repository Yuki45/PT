namespace Auto_Scanner_EPASS
{
    partial class FoolProof_Auto_Scew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoolProof_Auto_Scew));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.txtGmes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtScanner = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtsn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblresult = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gray;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 50);
            this.label1.TabIndex = 2;
            this.label1.Text = "FOOLPROOF AUTO SCREW";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConfig);
            this.groupBox1.Controls.Add(this.txtGmes);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtScanner);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(6, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 42);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Config";
            // 
            // btnConfig
            // 
            this.btnConfig.BackColor = System.Drawing.Color.Silver;
            this.btnConfig.Location = new System.Drawing.Point(340, 12);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(75, 23);
            this.btnConfig.TabIndex = 7;
            this.btnConfig.Text = "Config";
            this.btnConfig.UseVisualStyleBackColor = false;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // txtGmes
            // 
            this.txtGmes.Location = new System.Drawing.Point(224, 13);
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
            this.label3.Location = new System.Drawing.Point(177, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "GMES";
            // 
            // txtScanner
            // 
            this.txtScanner.Location = new System.Drawing.Point(71, 13);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(150)));
            this.label4.Location = new System.Drawing.Point(12, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 24);
            this.label4.TabIndex = 5;
            this.label4.Text = "1. Select Model";
            // 
            // cmbModel
            // 
            this.cmbModel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(172, 111);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(257, 23);
            this.cmbModel.TabIndex = 6;
            this.cmbModel.SelectionChangeCommitted += new System.EventHandler(this.cmbModel_SelectionChangeCommitted);
            this.cmbModel.SelectedValueChanged += new System.EventHandler(this.cmbModel_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(150)));
            this.label5.Location = new System.Drawing.Point(12, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(289, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "2. Scan Verify Barcode Screw";
            // 
            // txtsn
            // 
            this.txtsn.BackColor = System.Drawing.Color.Yellow;
            this.txtsn.Font = new System.Drawing.Font("Gulim", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.txtsn.Location = new System.Drawing.Point(16, 183);
            this.txtsn.Name = "txtsn";
            this.txtsn.Size = new System.Drawing.Size(413, 25);
            this.txtsn.TabIndex = 8;
            this.txtsn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(150)));
            this.label6.Location = new System.Drawing.Point(12, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "3. RESULT";
            // 
            // lblresult
            // 
            this.lblresult.BackColor = System.Drawing.Color.Gray;
            this.lblresult.Font = new System.Drawing.Font("Arial", 35F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(150)));
            this.lblresult.ForeColor = System.Drawing.Color.Lime;
            this.lblresult.Location = new System.Drawing.Point(14, 235);
            this.lblresult.Name = "lblresult";
            this.lblresult.Size = new System.Drawing.Size(415, 107);
            this.lblresult.TabIndex = 10;
            this.lblresult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label8.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(0, 344);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(441, 31);
            this.label8.TabIndex = 12;
            this.label8.Text = "Created By MTC HHP    Ver. 1.0";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FoolProof_Auto_Scew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 375);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblresult);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtsn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(447, 400);
            this.MinimumSize = new System.Drawing.Size(447, 400);
            this.Name = "FoolProof_Auto_Scew";
            this.Text = "FoolProof Auto Screw";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FoolProof_Auto_Scew_FormClosing);
            this.Load += new System.EventHandler(this.FoolProof_Auto_Scew_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.TextBox txtGmes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtScanner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtsn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblresult;
        private System.Windows.Forms.Label label8;



    }
}