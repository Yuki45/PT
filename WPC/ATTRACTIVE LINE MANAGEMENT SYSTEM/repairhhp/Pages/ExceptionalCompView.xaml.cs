using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ReceipNvView.xaml
    /// </summary>
    public partial class ExceptionalCompView : UserControl
    {
        Controls.Exceptional cnt = new Controls.Exceptional();
        public string _name, _gen, _auth, _team, _group, _tmpPart;

        public ExceptionalCompView()
        {
            InitializeComponent(); 
            this.LoadDataGrid();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
        }

        private void searchChange(object sender, TextChangedEventArgs e)
        {
            string _cn = searchGrid.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getExceptionalCompCN(_cn);
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
        }

        private void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getExceptionalComp();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Exceptional Stock: " + DataGridFiles.Items.Count.ToString();
        }

        private void rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedItem == null) return;
            DataRowView dr = DataGridFiles.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            cnGR.Text = dr1.ItemArray[3].ToString();
            unGR.Text = dr1.ItemArray[4].ToString();
            codeGR.Text = dr1.ItemArray[6].ToString();
            processGR.Text = dr1.ItemArray[5].ToString();
            modelGR.Text = dr1.ItemArray[1].ToString();
            lineGR.Text = dr1.ItemArray[2].ToString();
            itemGR.Text = dr1.ItemArray[7].ToString();
            valueGR.Text = dr1.ItemArray[8].ToString();
            nvlGR.Text = dr1.ItemArray[9].ToString();
            nvuGR.Text = dr1.ItemArray[10].ToString();
            txtID.Text = dr1.ItemArray[14].ToString();
        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            mainview.ShowMessageAsync("Information", "Data table already reloaded");
        }

        private void clearGRField()
        {
            cnGR.Text = "";
            unGR.Text = "";
            codeGR.Text = "";
            itemGR.Text = "";
            lineGR.Text = "";
            processGR.Text = "";
            modelGR.Text = "";
            itemGR.Text = "";
            valueGR.Text = "";
            nvlGR.Text = "";
            nvuGR.Text = "";
        }

        private void clearGR_Click(object sender, RoutedEventArgs e)
        {
            this.clearGRField();
        }

        private void submitGR_Click(object sender, RoutedEventArgs e)
        {
            string defectName = "Not Found";
            if (codeGR.Text != string.Empty)
            {
                string[] dfname = cnt.GetDefectNV(codeGR.Text);
                if (dfname[0] != string.Empty)
                {
                    defectName = dfname[0];
                }
            }
            if (cnGR.Text == string.Empty || nvPartGR.Text == string.Empty || unGR.Text == string.Empty || codeGR.Text == string.Empty || defectName == string.Empty || processGR.Text == string.Empty || modelGR.Text == string.Empty || lineGR.Text == string.Empty || itemGR.Text == string.Empty || valueGR.Text == string.Empty || nvlGR.Text == string.Empty || nvuGR.Text == string.Empty)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please complete data");
            }
            else
            {
                string hasil = cnt.UpdateExceptionalNV(nvPartGR.Text.ToUpper(), cnGR.Text.ToUpper(), unGR.Text.ToUpper(), codeGR.Text.ToUpper(), defectName, processGR.Text.ToUpper(), modelGR.Text.ToUpper(), lineGR.Text.ToUpper(), itemGR.Text.ToUpper(), valueGR.Text.ToUpper(), nvlGR.Text.ToUpper(), nvuGR.Text.ToUpper());
                if (hasil != "OK")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Warning", hasil);
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "GR exceptional success, please released");
                }
                LoadDataGrid();
                this.clearGRField();
            }
        }

    }
}
