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
    public partial class ReceiptNvView : UserControl
    {
        Controls.Controllers cnt = new Controls.Controllers();
        public string _name, _gen, _auth, _team, _group, _dept, _tmpPart;
        Pages.Popup.Popup_gen popupUser;

        public ReceiptNvView()
        {
            InitializeComponent(); 
            this.LoadDataGrid();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _dept = _sslog[5];
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
            labelStock.Content = "Waiting Stock: " + DataGridFiles.Items.Count.ToString();
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
            string _id = dr1.ItemArray[10].ToString();
            scanGI.Text = _nv;
            txtID.Text = _id;

        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            mainview.ShowMessageAsync("Information", "Data table already reloaded");
        }

        private void scanGRChange(object sender, TextChangedEventArgs e)
        {
            string _nv = scanGR.Text.ToUpper();
            var count = _nv.Count(x => x == '|');
            if(count == 12)
            {
                enableNVPartGR();
                string[] arrNV = _nv.Split('|');
                string _defect_code = arrNV[4];
                string _un = arrNV[2];
                string[] dfname = cnt.GetDefectNV(_defect_code);
                string booting = dfname[1];
                if (booting == "Y")
                {
                    List<string> listDataModel = cnt.GetAllModel();
                    modelGR.ItemsSource = listDataModel;
                    modelGR.SelectedIndex = -1;
                }
                else
                {
                    modelGR.ItemsSource = arrNV[11].Split('+').ToList();
                    modelGR.SelectedIndex = 0;
                }
                cnGR.Text = arrNV[1];
                unGR.Text = arrNV[2];
                processGR.Text = arrNV[3];
                codeGR.Text = _defect_code;

                if (dfname[0] != null)
                {
                    dnameGR.ItemsSource = dfname[0].Split('+').ToList();
                }
                else
                {
                    dnameGR.ItemsSource = arrNV[5].Split('+').ToList();
                }
                countRepeatNV(_un, arrNV[11]);
                dnameGR.SelectedIndex = 0;
                lineGR.Text = arrNV[10];
            }
        }

        private void enableNVPartGR()
        {
            nvPartGR.IsEnabled = true;
            if (_dept == "HHP Prod. Kitting P")
            {
                nvPartGR.Items.Clear();
                nvPartGR.Items.Add("OCTA");
                nvPartGR.Items.Add("REAR");
                nvPartGR.SelectedIndex = -1;
            }
            else
            {
                nvPartGR.Items.Clear();
                nvPartGR.Items.Add("PBA");
                nvPartGR.Items.Add("OCTA");
                nvPartGR.Items.Add("REAR");
                nvPartGR.SelectedIndex = -1;
            }
        }

        private void countRepeatNV(string un, string model)
        {
            int countUN = Int32.Parse(cnt.GetCountRepeatNV(un, model));
            if (countUN == 1)
            {
                txtCheckNV.Background = Brushes.Green;
                txtCheckNV.Content = "Repeat - 1X";
                countRepeat.Text = "1";
            }
            else if (countUN == 2)
            {
                txtCheckNV.Background = Brushes.Yellow;
                txtCheckNV.Content = "Repeat - 2X";
                countRepeat.Text = "2";
            }
            else if (countUN > 2)
            {
                txtCheckNV.Background = Brushes.Red;
                txtCheckNV.Content = "Repeat - " + countUN.ToString() + "X";
                countRepeat.Text = countUN.ToString();
            }
            else
            {
                txtCheckNV.Background = Brushes.Silver;
                txtCheckNV.Content = "New NV";
                countRepeat.Text = "0";
            }
        }

        private void clearGRField()
        {
            cnGR.Text = "";
            unGR.Text = "";
            codeGR.Text = "";
            dnameGR.Text = "";
            lineGR.Text = "";
            processGR.Text = "";
            nvPartGR.IsEnabled = false;
            nvPartGR.SelectedIndex = -1;
            txtCheckNV.Background = Brushes.Silver;
            txtCheckNV.Content = "-";
            try
            {
                modelGR.ItemsSource = null;
                modelGR.Items.Clear();
            }
            catch (Exception e) { Console.Write(e); }
        }

        private void clearGR_Click(object sender, RoutedEventArgs e)
        {
            this.clearGRField();
            scanGR.Text = "";
        }

        private void submitGR_Click(object sender, RoutedEventArgs e)
        {
            string _nv = scanGR.Text.ToUpper();
            string _countUn = countRepeat.Text;
            if (dnameGR.Text == string.Empty || lineGR.Text == string.Empty || modelGR.Text == string.Empty || nvPartGR.Text == string.Empty
                || _nv == string.Empty)
            {
                var mainview= System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please complete data");
            }
            else
            {
                if (nvPartGR.Text != "PBA" && partNumber.Text == string.Empty)
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Information", "Please enter part number");
                }
                else
                {
                    string hasil = cnt.AddFieldNV(nvPartGR.Text.ToUpper(), modelGR.Text.ToUpper(), lineGR.Text.ToUpper(), dnameGR.Text.ToUpper(), _nv.ToUpper(), _name.ToUpper(), partNumber.Text.ToUpper(), _countUn);
                    if (hasil != "OK")
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Warning", hasil);
                    }
                    else
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Success", "GR NV success, please assign NV to enginer as soon as posible");
                    }
                    LoadDataGrid();
                    this.clearGRField();
                    scanGR.Text = "";
                }
                
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
            scanGI.Text = "";
        }

        private void submitGI_Click(object sender, RoutedEventArgs e)
        {
            string _nv = scanGI.Text.ToUpper();
            string _gen = genGI.Text.ToUpper();
            string _id = txtID.Text.ToUpper();

            if (_id != string.Empty)
            {
                if (_gen != string.Empty)
                {
                    string hasil = cnt.updateGiEngineering(_nv, _gen, _id);
                    if (hasil == "OK")
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Success", "NV send to Engineer");
                        LoadDataGrid();
                        this.clearGIField();
                        scanGI.Text = "";
                    }
                    else
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Warning", hasil);
                        this.clearGIField();
                        scanGI.Text = "";
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

        private void scanGIChange(object sender, TextChangedEventArgs e)
        {
            string _nv = scanGI.Text.ToUpper();
            var count = _nv.Count(x => x == '|');
            if (count == 12)
            {
                bool cekNVStatus = cnt.GetNVStatusEngineer(_nv);
                if (cekNVStatus == true)
                {
                    string[] arrNV = _nv.Split('|');
                    string _defect_code = arrNV[4];
                    string[] dfnameGI = cnt.GetDefectNV(_defect_code);
                    cnGI.Text = arrNV[1];
                    searchGrid.Text = arrNV[1];
                    unGI.Text = arrNV[2];
                    codeGI.Text = _defect_code;
                    dnameGI.Text = dfnameGI[0];
                    lineGI.Text = arrNV[10];
                    modelGI.Text = arrNV[11];
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Information", "NV not found, please check NV label");
                    this.clearGRField();
                }

            }
        }

        private void partChange(object sender, SelectionChangedEventArgs e)
        {
            _tmpPart = "PBA";
            try
            {
                _tmpPart = (sender as ComboBox).SelectedItem as string; //((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            }
            catch (Exception ex) { }

            if (_tmpPart == "OCTA" || _tmpPart == "REAR")
            {
                dnameGR.IsEnabled = true;
                partNumber.IsEnabled = true;
                List<string> listDataDefect = cnt.GetStdDefect();
                dnameGR.ItemsSource = listDataDefect;
                dnameGR.SelectedIndex = -1;
                codeGR.Text = "";
            }
            else
            {
                dnameGR.IsEnabled = false;
                partNumber.IsEnabled = false;
            }
        }

        private void dnameChange(object sender, SelectionChangedEventArgs e)
        {
            if (_tmpPart == "OCTA" || _tmpPart == "REAR")
            {
                string _tmpDefect = (sender as ComboBox).SelectedItem as string;
                codeGR.Text = cnt.GetStdDefectCode(_tmpDefect);
            }
        }
       
    }
}
