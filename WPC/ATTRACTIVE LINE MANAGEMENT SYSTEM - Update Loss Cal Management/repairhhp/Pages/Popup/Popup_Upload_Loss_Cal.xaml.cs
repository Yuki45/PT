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
    /// Interaction logic for Popup_history.xaml
    /// </summary>
    public partial class Popup_history : MetroWindow
    {
        Controls.Machine cnt = new Controls.Machine();
        public String _un, _model, _line, _defect, date;
        public Popup_history()
        {
            InitializeComponent();
        }

        public void LoadDataGrid()
        {
            string sql = "SELECT '"+_defect+"' AS DEFECT,[PORT NO], COUNT([PORT NO]) AS [QTY DEFECT]  FROM [PROD].[dbo].[SPC_AGENT] ";
            DataTable tbl = cnt.getData(sql + date + _line +" GROUP BY [PORT NO]");
            dataGrid.ItemsSource = tbl.AsDataView();
        }

        private void popupLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.LoadDataGrid();
        }
    }
}
