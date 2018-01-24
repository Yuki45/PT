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
    public partial class ExceptionalGRView : UserControl
    {
        Controls.Exceptional cnt = new Controls.Exceptional();
        public string _name, _gen, _auth, _team, _group, _tmpPart;
        Pages.Popup.Popup_gen popupUser;

        public ExceptionalGRView()
        {
            InitializeComponent(); 
            this.LoadDataGrid();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            List<string> listModel = cnt.GetAllModel();
            modelGR.ItemsSource = listModel;
            List<string> listLine = cnt.GetAllLine();
            lineGR.ItemsSource = listLine;
        }

        private void searchChange(object sender, TextChangedEventArgs e)
        {
            string _cn = searchGrid.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getStockWaitingCN(_cn);
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
        }

        private void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getStockWaiting();
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
            cnGI.Text = dr1.ItemArray[3].ToString();
            unGI.Text = dr1.ItemArray[4].ToString();
            codeGI.Text = dr1.ItemArray[6].ToString();
            dnameGI.Text = dr1.ItemArray[7].ToString();
            modelGI.Text = dr1.ItemArray[1].ToString();
            lineGI.Text = dr1.ItemArray[2].ToString();
            txtID.Text = dr1.ItemArray[10].ToString();

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
            try
            {
                modelGR.ItemsSource = null;
            }
            catch (Exception e) { Console.Write(e); }
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
            if (nvPartGR.Text == string.Empty || cnGR.Text == string.Empty || lineGR.Text == string.Empty || modelGR.Text == string.Empty || codeGR.Text == string.Empty)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please complete data");
            }
            else
            {
                string hasil = cnt.AddExceptionalNV(nvPartGR.Text.ToUpper(), cnGR.Text.ToUpper(), unGR.Text.ToUpper(), codeGR.Text.ToUpper(), defectName, processGR.Text.ToUpper(), modelGR.Text.ToUpper(), lineGR.Text.ToUpper(), itemGR.Text.ToUpper(), valueGR.Text.ToUpper(), nvlGR.Text.ToUpper(), nvuGR.Text.ToUpper(), _name.ToUpper());
                if (hasil != "OK")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Warning", hasil);
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "GR exceptional success, please assign to enginer as soon as posible");
                }
                LoadDataGrid();
                this.clearGRField();
            }
        }

        //-----------------------------------------GI TO ENGINEER---------------------------------------------//
        private void clearGIField()
        {
            cnGI.Text = "";
            unGI.Text = "";
            codeGI.Text = "";
            dnameGI.Text = "";
            modelGI.Text = "";
            lineGI.Text = "";
            genGI.Text = "";
            nameGI.Text = "";
            txtID.Text = "";
        }

        private void clearGI_Click(object sender, RoutedEventArgs e)
        {
            this.clearGIField();
        }

        private void submitGI_Click(object sender, RoutedEventArgs e)
        {
            string _gen = genGI.Text.ToUpper();
            string _id = txtID.Text.ToUpper();

            if (_id != string.Empty)
            {
                if (_gen != string.Empty)
                {
                    string hasil = cnt.updateGiEngineeringEx(_gen, _id);
                    if (hasil == "OK")
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Success", "NV send to Engineer");
                        LoadDataGrid();
                        this.clearGIField();
                    }
                    else
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Warning", hasil);
                        this.clearGIField();
                    }
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Information", "Please select engineer");
                }
            }
            else
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please double click row table to get ID");
            }
            
        }

        private void searchGI_Click(object sender, RoutedEventArgs e)
        {
            if (popupUser != null)
            {
                popupUser.Close();
            }
            popupUser = new Pages.Popup.Popup_gen() { Owner = Window.GetWindow(this) };
            popupUser.CheckG += value => genGI.Text = value;
            popupUser.CheckN += value => nameGI.Text = value;
            popupUser.ShowDialog();
        }

    }
}
