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
    public partial class ReleasedView : UserControl
    {
        Controls.Released cnt = new Controls.Released();
        Pages.Popup.Popup_history popupHistory;
        public string _name, _gen, _auth, _team, _group, _tmpStatus, _tmpCategory, _tmpAbolish;

        public ReleasedView()
        {
            InitializeComponent(); 
            this.LoadDataGrid();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];

            List<string> listCause = cnt.GetCause();
            causeDec.ItemsSource = listCause;
            causeDec.SelectedIndex = -1;
        }

        private void searchChange(object sender, TextChangedEventArgs e)
        {
            string _cn = searchGrid.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getStockRepairCN(_cn);
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
        }

        private void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getStockRepair();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Repair Stock: " + DataGridFiles.Items.Count.ToString();
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
            causeDec.Text = dr1.ItemArray[10].ToString();
            locDec.Text = dr1.ItemArray[11].ToString();
            remarkDec.Text = dr1.ItemArray[12].ToString();
            txtID.Text = dr1.ItemArray[13].ToString();
            partNV.Text = dr1.ItemArray[14].ToString();
            partNumber.Text = dr1.ItemArray[15].ToString();

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
                countRepeatNV(_un, arrNV[11]);
                string[] dfname = cnt.GetDefectNV(_defect_code);
                cnNV.Text = arrNV[1];
                searchGrid.Text = arrNV[1];
                unNV.Text = arrNV[2];
                codeNV.Text = _defect_code;
                dnameNV.Text = dfname[0];
                lineNV.Text = arrNV[10];
                modelNV.Text = arrNV[11];
                DataTable dtnv = cnt.getDtRowNV(_nv);
                if (dtnv.Rows.Count > 0)
                {
                    causeDec.Text = dtnv.Rows[0][0].ToString();
                    locDec.Text = dtnv.Rows[0][1].ToString();
                    remarkDec.Text = dtnv.Rows[0][2].ToString();
                    r_status.Text = dtnv.Rows[0][3].ToString();
                    repairGen.Text = dtnv.Rows[0][5].ToString();
                    repairName.Text = dtnv.Rows[0][6].ToString();
                }
                else
                {
                    causeDec.Text = "";
                    locDec.Text = "";
                    remarkDec.Text = "";
                    r_status.Text = "";
                    repairGen.Text = "";
                    repairName.Text = "";
                }
            }
        }

        private void countRepeatNV(string un, string model)
        {
            int countUN = Int32.Parse(cnt.GetCountRepeatNV(un, model)) - 1;
            
            if (countUN == 1)
            {
                txtCheckNV.Background = Brushes.Green;
                txtCheckNV.Content = "Repeat - 1X";
                repeatQty.Text = "1";
            }
            else if (countUN == 2)
            {
                txtCheckNV.Background = Brushes.Yellow;
                txtCheckNV.Content = "Repeat - 2X";
                repeatQty.Text = "2";
            }
            else if (countUN > 2)
            {
                txtCheckNV.Background = Brushes.Red;
                txtCheckNV.Content = "Repeat - "+countUN.ToString()+"X";
                repeatQty.Text = countUN.ToString();
            }
            else
            {
                txtCheckNV.Background = Brushes.Silver;
                txtCheckNV.Content = "New NV";
                repeatQty.Text = "0";
            }
        }

        private void clearField()
        {
            cnNV.Text = "";
            unNV.Text = "";
            partNV.Text = "";
            codeNV.Text = "";
            dnameNV.Text = "";
            lineNV.Text = "";
            modelNV.Text = "";
            causeDec.Text = "";
            locDec.Text = "";
            remarkDec.Text = "";
            partNumber.Text = "";
            repairGen.Text = "";
            repairName.Text = "";
            repeatQty.Text = "";
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
            string _nv = scanNV.Text.ToUpper();
            string _statusDec = _tmpStatus;
            string _absDec = _tmpAbolish;
            string _r_status = r_status.Text.ToUpper();
            string _un = unNV.Text.ToUpper();
            string _categoryDec = _tmpCategory;
            string _part = partNV.Text.ToUpper();
            string _partnumber = partNumber.Text.ToUpper();
            string _id = txtID.Text.ToUpper();

            if (!string.IsNullOrWhiteSpace(_id))
            {
                if (!string.IsNullOrWhiteSpace(_un))
                {
                    if (!string.IsNullOrWhiteSpace(_categoryDec))
                    {
                        if (!string.IsNullOrWhiteSpace(_statusDec))
                        {
                            if (_part != "PBA" && _partnumber == string.Empty)
                            {

                                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                                mainview.ShowMessageAsync("Information", "Please enter part number");
                            }
                            else
                            {
                                if (cnt.EditFieldNV(_nv, causeDec.Text.ToUpper(), locDec.Text.ToUpper(), _statusDec, _name.ToUpper(), cnNV.Text.ToUpper(), remarkDec.Text.ToUpper(), _absDec, _r_status, _un, _categoryDec, _partnumber, _id))
                                {
                                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                                    mainview.ShowMessageAsync("Success", "Released NV success");
                                    LoadDataGrid();
                                    this.clearField();
                                    scanNV.Text = "";
                                }
                                else
                                {
                                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                                    mainview.ShowMessageAsync("Warning", "Released NV failed, please check data");
                                }
                            }
                        }
                        else
                        {
                            var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                            mainview.ShowMessageAsync("Information", "Please select repair status");
                        }
                    }
                    else
                    {
                        var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                        mainview.ShowMessageAsync("Information", "Please select defect category");
                    }
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Information", "Please insert UN");
                }
            }
            else
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please double click row table to get ID");
            }
            
        }

        private void statusChange(object sender, SelectionChangedEventArgs e)
        {
            _tmpStatus = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            if (_tmpStatus == "NO")
            {
                absDec.Visibility = Visibility.Visible;
                lblAbolish.Visibility = Visibility.Visible;
            }
            else
            {
                absDec.Visibility = Visibility.Hidden;
                lblAbolish.Visibility = Visibility.Hidden;
                absDec.SelectedIndex = -1;
            }
        }

        private void categoryChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _tmpCategory = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            }
            catch (Exception ex) { }
        }

        private void abolishChange(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _tmpAbolish = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as string;
            }
            catch (Exception ex) { }
        }

        private void history_click(object sender, RoutedEventArgs e)
        {
            string _un = unNV.Text.ToUpper();
            string _model = modelNV.Text.ToUpper();

            if (popupHistory != null)
            {
                popupHistory.Close();
            }
            popupHistory = new Pages.Popup.Popup_history() { Owner = Window.GetWindow(this) };
            popupHistory.txtUN.Text = _un;
            popupHistory.txtModel.Text = _model;
            popupHistory.ShowDialog();
        }

        private void repeatQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            string _un = unNV.Text.ToUpper();
            int _cq = 0;
            try { _cq = Convert.ToInt32(repeatQty.Text); }
            catch (Exception ex) { }
            if (_cq>0)
            {
                history.IsEnabled = true;
            }
            else
            {
                history.IsEnabled = false;
            }
        }

        private void partChange(object sender, TextChangedEventArgs e)
        {
            string _tmpPart = partNV.Text;
            if (_tmpPart == "OCTA" || _tmpPart == "REAR")
            {
                partNumber.IsReadOnly = false;
                history.IsEnabled = false;
            }
            else
            {
                partNumber.IsReadOnly = true;
                history.IsEnabled = true;
            }
        }

    }
}
