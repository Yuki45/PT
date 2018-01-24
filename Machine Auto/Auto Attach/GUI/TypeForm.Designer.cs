namespace Auto_Attach.GUI
{
    partial class TypeForm
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
            this.btnDryRun = new System.Windows.Forms.Button();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnSimulation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDryRun
            // 
            this.btnDryRun.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnDryRun.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDryRun.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDryRun.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.btnDryRun.Location = new System.Drawing.Point(12, 7);
            this.btnDryRun.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDryRun.Name = "btnDryRun";
            this.btnDryRun.Size = new System.Drawing.Size(88, 61);
            this.btnDryRun.TabIndex = 0;
            this.btnDryRun.Text = "DRY RUN";
            this.btnDryRun.UseVisualStyleBackColor = false;
            this.btnDryRun.Click += new System.EventHandler(this.btnDryRun_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnNormal.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnNormal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnNormal.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.btnNormal.Location = new System.Drawing.Point(110, 7);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(91, 61);
            this.btnNormal.TabIndex = 1;
            this.btnNormal.Text = "NORMAL";
            this.btnNormal.UseVisualStyleBackColor = false;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnSimulation
            // 
            this.btnSimulation.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSimulation.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSimulation.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnSimulation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSimulation.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.btnSimulation.Location = new System.Drawing.Point(211, 7);
            this.btnSimulation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(86, 61);
            this.btnSimulation.TabIndex = 2;
            this.btnSimulation.Text = "SIMULASI";
            this.btnSimulation.UseVisualStyleBackColor = false;
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // TypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(307, 79);
            this.Controls.Add(this.btnSimulation);
            this.Controls.Add(this.btnNormal);
            this.Controls.Add(this.btnDryRun);
            this.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TypeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TypeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDryRun;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnSimulation;
    }
}