using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for AttrLine.xaml
    /// </summary>
    public partial class AttrLine : UserControl
    {
        Controls.Attractive cnt = new Controls.Attractive();
        public string _name, _gen, _auth, _team, _group, _ip;
        public AttrLine()
        {
            InitializeComponent();
            DataTable line = cnt.getData("SELECT [NAME_line]  FROM [PROD].[dbo].[MASTER_LINE] ");
            DataTable process = cnt.getData("SELECT [PROC_NAME]  FROM [PROD].[dbo].[MASTER_PROCESS]");
            cmbLineOri.Items.Clear();;
            cmbLineOri.Items.Add("ALL");
            cmbLineAtt.Items.Clear();
            cmbLineAtt.Items.Add("ALL");
            foreach (DataRow r in line.Rows)
            {
                cmbLineAtt.Items.Add(r[0].ToString());
                cmbLineOri.Items.Add(r[0].ToString());
            }
            cmbProcOri.Items.Clear();
            cmbProcOri.Items.Add("ALL");
            foreach (DataRow r in process.Rows)
            {
                cmbProcOri.Items.Add(r[0].ToString());
            }
        }

        private void clearGI_Click(object sender, RoutedEventArgs e)
        {
            clearField();
        }
        private void clearField()
        {
            cmbProcOri.Text = "";
            cmbLineAtt.Text = "";
            cmbLineOri.Text = "";
            txtModelAtt.Text = "";
            txtModelOri.Text = "";
            txtRemark.Text = "";
            //string richText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;
            //richTextBox1.Document.Blocks.Add(new Paragraph(new Run("Text")));
        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            // this.clearField();
            gettable();
        }

        private void gettable()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getSql(@"SELECT [model_actual] as [MODEL ACTUAL]
                                            ,[model_att] AS [MODEL ATTACTIVE]
                                            ,[att_date] AS [ATTACTIVE DATE]
                                            ,[origin_line] AS [LINE ACTUAL]
                                            ,[att_line] AS [LINE ATTACTIVE]
                                            ,[process_att] AS [PROCESS NAME]
                                            ,[Att_Remark] AS [REMARK]
                                            ,[verify_leader] AS [LEADER NAME]
                                            ,[verify_leader_date] AS [LEADER DATE]
                                            ,[verify_pqc_line] AS [PQC NAME]
                                            ,[verify_pqc_date] AS [PQC DATE]
                                            ,[verify_status] AS [STATUS]
                                            FROM [MTC].[dbo].[tbl_att_line]");
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Total Row: " + DataGridFiles.Items.Count.ToString();
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            if (cmbProcOri.Text == ""  || cmbLineOri.Text == "" || cmbLineAtt.Text == "" ||  txtModelAtt.Text == "" || txtModelOri.Text == "")
            {
                //Show Message
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Warning", "Please Input All Field...!");
            }
            else
            {               
                cnt.UpProcAtt(txtModelOri.Text, txtModelAtt.Text, cmbLineOri.Text, cmbLineAtt.Text, cmbProcOri.Text,txtRemark.Text);
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Data Sucess Register...!");
                cmbProcOri.Text = "";
                cmbLineAtt.Text = "";
                cmbLineOri.Text = "";
                txtModelAtt.Text = "";
                txtModelOri.Text = "";
                txtRemark.Text = "";
            }
            gettable();
        }

        private void DataGridFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridFiles.Items.Count > 0)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = DataGridFiles.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();

                if (MessageBox.Show("Close Are You Sure, want to DeActive this Defect ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //cnt.UpFilter("N", dr1.ItemArray[0].ToString());
                }
                else
                {
                    //cnt.UpFilter("Y", dr1.ItemArray[0].ToString());
                }
                gettable();
            }
        }
    }
}
