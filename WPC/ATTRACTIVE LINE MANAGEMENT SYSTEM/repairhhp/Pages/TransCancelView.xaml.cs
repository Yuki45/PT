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
    public partial class TransCancelView : UserControl
    {
        Controls.Transaction cnt = new Controls.Transaction();
        public string _name, _gen, _auth, _team, _group;

        public TransCancelView()
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
            SqlDataAdapter da = cnt.getTableCN(_cn);
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
        }

        private void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getTable();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Stock: " + DataGridFiles.Items.Count.ToString();
        }

        private void rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedItem == null) return;
            DataRowView dr = DataGridFiles.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            string _cn = dr1.ItemArray[3].ToString();
            string _un = dr1.ItemArray[4].ToString();
            string _nv = dr1.ItemArray[9].ToString();
            txtID.Text = dr1.ItemArray[11].ToString();

            if (_un != "-")
            {
                scanNV.Text = _nv;
            }
            else
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "NV Not Found");
                clearField();
            }       

        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            mainview.ShowMessageAsync("Information", "Data table already reloaded");
        }

        private void scanNVChange(object sender, TextChangedEventArgs e)
        {
            string _nv = scanNV.Text.ToUpper();
            var count = _nv.Count(x => x == '|');
            if(count == 12)
            {
                string[] arrNV = _nv.Split('|');
                string _defect_code = arrNV[4];
                string _un = arrNV[2];
                string[] dfname = cnt.GetDefectNV(_defect_code);
                cnNV.Text = arrNV[1];
                searchGrid.Text = arrNV[1];
                unNV.Text = arrNV[2];
                processNV.Text = arrNV[3];
                codeNV.Text = _defect_code;
                dnameNV.Text = dfname[0];
                lineNV.Text = arrNV[10];
                modelNV.Text = arrNV[11];
                DataTable dtnv = cnt.getDtRowNV(_nv);
                if (dtnv.Rows.Count > 0)
                {
                    r_status.Text = dtnv.Rows[0][3].ToString();
                    partNumber.Text = dtnv.Rows[0][4].ToString();
                }
                else
                {
                    r_status.Text = "";
                    partNumber.Text = "";
                }
            }
        }

        private void clearField()
        {
            cnNV.Text = "";
            unNV.Text = "";
            processNV.Text = "";
            codeNV.Text = "";
            dnameNV.Text = "";
            lineNV.Text = "";
            modelNV.Text = "";
            partNumber.Text = "";
            txtID.Text = "";
            txtCheckNV.Background = Brushes.Silver;
            txtCheckNV.Content = "-";
        }

        private void clear_click(object sender, RoutedEventArgs e)
        {
            this.clearField();
            scanNV.Text = "";
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            string _id = txtID.Text.ToUpper();

            //var xx = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            //var mySettings = new MetroDialogSettings()  
            //{  
            //    AffirmativeButtonText = "DELETE",  
            //    AnimateShow = true,  
            //    NegativeButtonText = "Cancel",
            //    ColorScheme = xx.MetroDialogOptions.ColorScheme
            //};
            //MessageDialogResult result = await xx.ShowMessageAsync("Delete Records", "Are you sure you want to delete this records?",
            //    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            //Console.Write(result);

            if (_id == string.Empty)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please double click row table to get ID");
            }
            else
            {
                string hasil = cnt.deleteTransaction(_id);

                if (hasil == "OK")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "Item deleted");
                    LoadDataGrid();
                    this.clearField();
                    scanNV.Text = "";
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Warning", hasil);
                }
            }      
        }

       
    }
}
