namespace Scanner_Aging_OCV
{
    partial class MasterForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgModel = new System.Windows.Forms.DataGridView();
            this.dgCategory = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMasterModel = new System.Windows.Forms.TextBox();
            this.btnModelSave = new System.Windows.Forms.Button();
            this.btnModelCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCategoryModel = new System.Windows.Forms.TextBox();
            this.txtCategoryPartNo = new System.Windows.Forms.TextBox();
            this.txtCategoryPartName = new System.Windows.Forms.TextBox();
            this.btnCategoryCancel = new System.Windows.Forms.Button();
            this.btnCategorySave = new System.Windows.Forms.Button();
            this.MasterModel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CategoryMaster = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCategory)).BeginInit();
            this.MasterModel.SuspendLayout();
            this.CategoryMaster.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(483, 397);
            this.splitContainer1.SplitterDistance = 174;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnModelCancel);
            this.groupBox1.Controls.Add(this.btnModelSave);
            this.groupBox1.Controls.Add(this.txtMasterModel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dgModel);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Master Model";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCategoryCancel);
            this.groupBox2.Controls.Add(this.btnCategorySave);
            this.groupBox2.Controls.Add(this.txtCategoryPartName);
            this.groupBox2.Controls.Add(this.txtCategoryPartNo);
            this.groupBox2.Controls.Add(this.txtCategoryModel);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dgCategory);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 219);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Master Category";
            // 
            // dgModel
            // 
            this.dgModel.AllowUserToAddRows = false;
            this.dgModel.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgModel.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgModel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgModel.ContextMenuStrip = this.MasterModel;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgModel.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgModel.Location = new System.Drawing.Point(152, 14);
            this.dgModel.Name = "dgModel";
            this.dgModel.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgModel.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgModel.RowTemplate.Height = 23;
            this.dgModel.Size = new System.Drawing.Size(319, 150);
            this.dgModel.TabIndex = 0;
            this.dgModel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgModel_CellClick);
            // 
            // dgCategory
            // 
            this.dgCategory.AllowUserToAddRows = false;
            this.dgCategory.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCategory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCategory.ContextMenuStrip = this.CategoryMaster;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCategory.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCategory.Location = new System.Drawing.Point(152, 15);
            this.dgCategory.Name = "dgCategory";
            this.dgCategory.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgCategory.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgCategory.RowTemplate.Height = 23;
            this.dgCategory.Size = new System.Drawing.Size(319, 198);
            this.dgCategory.TabIndex = 1;
            this.dgCategory.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCategory_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Model Name";
            // 
            // txtMasterModel
            // 
            this.txtMasterModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMasterModel.Location = new System.Drawing.Point(8, 32);
            this.txtMasterModel.Name = "txtMasterModel";
            this.txtMasterModel.Size = new System.Drawing.Size(138, 21);
            this.txtMasterModel.TabIndex = 2;
            // 
            // btnModelSave
            // 
            this.btnModelSave.Location = new System.Drawing.Point(17, 71);
            this.btnModelSave.Name = "btnModelSave";
            this.btnModelSave.Size = new System.Drawing.Size(52, 23);
            this.btnModelSave.TabIndex = 3;
            this.btnModelSave.Text = "Save";
            this.btnModelSave.UseVisualStyleBackColor = true;
            this.btnModelSave.Click += new System.EventHandler(this.btnModelSave_Click);
            // 
            // btnModelCancel
            // 
            this.btnModelCancel.Location = new System.Drawing.Point(76, 71);
            this.btnModelCancel.Name = "btnModelCancel";
            this.btnModelCancel.Size = new System.Drawing.Size(57, 23);
            this.btnModelCancel.TabIndex = 4;
            this.btnModelCancel.Text = "Cancel";
            this.btnModelCancel.UseVisualStyleBackColor = true;
            this.btnModelCancel.Click += new System.EventHandler(this.btnModelCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Model Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Part No";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Part Name";
            // 
            // txtCategoryModel
            // 
            this.txtCategoryModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCategoryModel.Location = new System.Drawing.Point(6, 32);
            this.txtCategoryModel.Name = "txtCategoryModel";
            this.txtCategoryModel.Size = new System.Drawing.Size(138, 21);
            this.txtCategoryModel.TabIndex = 5;
            // 
            // txtCategoryPartNo
            // 
            this.txtCategoryPartNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCategoryPartNo.Location = new System.Drawing.Point(6, 77);
            this.txtCategoryPartNo.Name = "txtCategoryPartNo";
            this.txtCategoryPartNo.Size = new System.Drawing.Size(138, 21);
            this.txtCategoryPartNo.TabIndex = 6;
            // 
            // txtCategoryPartName
            // 
            this.txtCategoryPartName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCategoryPartName.Location = new System.Drawing.Point(6, 119);
            this.txtCategoryPartName.Name = "txtCategoryPartName";
            this.txtCategoryPartName.Size = new System.Drawing.Size(138, 21);
            this.txtCategoryPartName.TabIndex = 7;
            // 
            // btnCategoryCancel
            // 
            this.btnCategoryCancel.Location = new System.Drawing.Point(76, 146);
            this.btnCategoryCancel.Name = "btnCategoryCancel";
            this.btnCategoryCancel.Size = new System.Drawing.Size(57, 23);
            this.btnCategoryCancel.TabIndex = 9;
            this.btnCategoryCancel.Text = "Cancel";
            this.btnCategoryCancel.UseVisualStyleBackColor = true;
            this.btnCategoryCancel.Click += new System.EventHandler(this.btnCategoryCancel_Click);
            // 
            // btnCategorySave
            // 
            this.btnCategorySave.Location = new System.Drawing.Point(17, 146);
            this.btnCategorySave.Name = "btnCategorySave";
            this.btnCategorySave.Size = new System.Drawing.Size(52, 23);
            this.btnCategorySave.TabIndex = 8;
            this.btnCategorySave.Text = "Save";
            this.btnCategorySave.UseVisualStyleBackColor = true;
            this.btnCategorySave.Click += new System.EventHandler(this.btnCategorySave_Click);
            // 
            // MasterModel
            // 
            this.MasterModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.masterDataToolStripMenuItem});
            this.MasterModel.Name = "contextMenuStrip1";
            this.MasterModel.Size = new System.Drawing.Size(106, 48);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
            this.exitToolStripMenuItem.Text = "Insert";
            // 
            // masterDataToolStripMenuItem
            // 
            this.masterDataToolStripMenuItem.Name = "masterDataToolStripMenuItem";
            this.masterDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.masterDataToolStripMenuItem.Text = "Delete";
            this.masterDataToolStripMenuItem.Click += new System.EventHandler(this.masterDataToolStripMenuItem_Click);
            // 
            // CategoryMaster
            // 
            this.CategoryMaster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.CategoryMaster.Name = "contextMenuStrip1";
            this.CategoryMaster.Size = new System.Drawing.Size(153, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Insert";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Delete";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 397);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MasterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MasterForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MasterForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCategory)).EndInit();
            this.MasterModel.ResumeLayout(false);
            this.CategoryMaster.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnModelSave;
        private System.Windows.Forms.TextBox txtMasterModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgModel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgCategory;
        private System.Windows.Forms.Button btnModelCancel;
        private System.Windows.Forms.Button btnCategoryCancel;
        private System.Windows.Forms.Button btnCategorySave;
        private System.Windows.Forms.TextBox txtCategoryPartName;
        private System.Windows.Forms.TextBox txtCategoryPartNo;
        private System.Windows.Forms.TextBox txtCategoryModel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip MasterModel;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem masterDataToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip CategoryMaster;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}