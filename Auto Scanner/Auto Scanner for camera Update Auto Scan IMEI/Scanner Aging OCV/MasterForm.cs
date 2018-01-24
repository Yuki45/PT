using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scanner_Aging_OCV
{
    public partial class MasterForm : Form
    {
        DB.DB db = new DB.DB();
        bool masterInsert, MasterUpdate;
        bool categoryInsert, CategoryUpdate;
        public MasterForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void loadmodel()
        {
            dgModel.DataSource = db.getData("SELECT id as [ID], model_name as [MODEL NAME] FROM tbl_master_model");
        }

        private void loadCategory()
        {
            dgCategory.DataSource = db.getData("SELECT model_name as [MODEL NAME], part_no as [PART NO], part_name as [PART NAME] FROM tbl_master_part");
        }

        private void MasterForm_Load(object sender, EventArgs e)
        {
            loadCategory();
            loadmodel();
        }

        private void btnModelSave_Click(object sender, EventArgs e)
        {
            if (txtMasterModel.Text!="")
            db.setData2("insert into tbl_master_model(model_name) values('"+txtMasterModel.Text+"')");
            loadmodel();
        }

        private void btnModelCancel_Click(object sender, EventArgs e)
        {
            txtMasterModel.Text = "";
        }

        private void btnCategorySave_Click(object sender, EventArgs e)
        {
            if (txtCategoryModel.Text != ""  || txtCategoryPartNo.Text !="")
                db.setData2("insert into tbl_master_part(model_name, part_no, part_name) values('" + txtCategoryModel.Text + "','"+txtCategoryPartNo.Text+"','"+txtCategoryPartName.Text+"')");
           
            loadCategory();
        }

        private void btnCategoryCancel_Click(object sender, EventArgs e)
        {
            txtCategoryModel.Text = "";
            txtCategoryPartName.Text = "";
            txtCategoryPartNo.Text = "";
        }

        private void masterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtMasterModel.Text!="")
            db.setData2("delete from tbl_model_master Where model_name='" + txtMasterModel.Text + "'");
            loadmodel();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (txtCategoryPartNo.Text!="" || txtCategoryModel.Text!="")
            db.setData2("delete from tbl_master_part Where model_name='"+txtCategoryModel.Text+"' AND part_no='"+txtCategoryPartNo.Text+"'");
            loadCategory();
        }

        private void dgCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgCategory.Rows[e.RowIndex];

                txtCategoryModel.Text = row.Cells["MODEL NAME"].Value.ToString();
                txtCategoryPartNo.Text = row.Cells["PART NO"].Value.ToString();
                txtCategoryPartName.Text = row.Cells["PART NAME"].Value.ToString();
            }
        }

        private void dgModel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgModel.Rows[e.RowIndex];

                txtMasterModel.Text = row.Cells["MODEL NAME"].Value.ToString();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            txtCategoryModel.Text = "";
            txtCategoryPartName.Text = "";
            txtCategoryPartNo.Text = "";
            txtCategoryModel.Focus();
        }
    }
}
