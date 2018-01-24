using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data;
using System.Data.SqlClient;

namespace repairhhp.Pages.Popup
{
    /// <summary>
    /// Interaction logic for Popup_gen.xaml
    /// </summary>
    public partial class Popup_gen : MetroWindow
    {
        Controls.Controllers cnt = new Controls.Controllers();
        public event Action<string> CheckG, CheckN;

        public Popup_gen()
        {
            InitializeComponent();
            txtSearch.Focus();
            this.LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = cnt.getuserSearch(txtSearch.Text);
            UserGrid.ItemsSource = new DataView(dt);
        }

        private void searchChange(object sender, TextChangedEventArgs e)
        {
            LoadDataGrid();
        }

        private void popupLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
        }

        private void rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedItem == null) return;
            DataRowView dr = UserGrid.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;
            string _gen = dr1.ItemArray[0].ToString();
            string _name = dr1.ItemArray[1].ToString();
            CheckG(_gen);
            CheckN(_name);
            this.Close();
        }
    }
}
