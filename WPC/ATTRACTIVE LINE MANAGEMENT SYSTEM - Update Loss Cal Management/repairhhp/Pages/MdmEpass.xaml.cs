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
    /// Interaction logic for MdmCauseView.xaml
    /// </summary>
    public partial class MdmEpass : UserControl
    {
        Controls.CMdmEpass cnt = new Controls.CMdmEpass();
        public string _name, _gen, _auth, _team, _group;
        private bool update = false;

        public MdmEpass()
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
            string _id = searchGrid.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getEpassID(_id);
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
        }

        private void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getMdmEpass();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "QTY: " + DataGridFiles.Items.Count.ToString();
        }

        private void rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedItem == null) return;
            DataRowView dr = DataGridFiles.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            searchGrid.Text = dr1.ItemArray[0].ToString();
            txtID.Text = dr1.ItemArray[0].ToString();
            txtDesc.Text = dr1.ItemArray[2].ToString();
            txtkey.Text = dr1.ItemArray[1].ToString();
            
            update = true;
        }

        private void refreshClick(object sender, RoutedEventArgs e)
        {
            this.LoadDataGrid();
            var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            mainview.ShowMessageAsync("Information", "Data table already reloaded");
        }

        private void clearField()
        {
            txtID.Text = "";
            txtDesc.Text = "";
            txtkey.Text = "";
            searchGrid.Text = "";
            update = false;
        }

        private void clear_click(object sender, RoutedEventArgs e)
        {
            this.clearField();
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            string _id = txtID.Text.ToUpper();
            string _desc = txtDesc.Text.ToUpper();
            string hasil;

            if (_desc == string.Empty)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please complete data");
            }
            else
            {
                if (!update)
                {
                    hasil = cnt.saveCause(txtID.Text,txtDesc.Text, txtkey.Text,_name);
                }
                else
                {
                    hasil = cnt.updateCause(txtID.Text, txtDesc.Text, txtkey.Text, _name);
                }

                if (hasil == "SAVE")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "Save data complete");
                    LoadDataGrid();
                    this.clearField();
                }
                else if (hasil == "UPDATE")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "Update data complete");
                    LoadDataGrid();
                    this.clearField();
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Warning", hasil);
                }
            }
            update = false;
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            string _id = txtID.Text.ToUpper();

            if (_id == string.Empty)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please double click table to get ID");
            }
            else
            {
                string hasil = cnt.deleteCause(_id, txtDesc.Text );

                if (hasil == "OK")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "Delete data complete");
                    LoadDataGrid();
                    this.clearField();
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
