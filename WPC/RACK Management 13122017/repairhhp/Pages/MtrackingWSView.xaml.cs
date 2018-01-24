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
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Threading;
using System.Timers;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for WPCAgingView.xaml
    /// </summary>
    public partial class MtrackingWSView : UserControl
    {
        Controls.Losscal cnt = new Controls.Losscal();
        public string _name, _gen, _auth, _team, _group, _ip;
        private string Line;
        repairhhp.Pages.Popup.Popup_Upload_Loss_Cal popupCP;       
        public MtrackingWSView()
        {
            InitializeComponent();

            DataTable model = cnt.getData(" SELECT DISTINCT([BASIC MODEL])  FROM [PROD].[dbo].[LOSS_CAL_VIEW] ");
            
            
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {        
                gettable();
        }
        private void gettable()
        {
            try
            {
                DataTable da = cnt.getTracking(txt_un.Text);
               
                DataView rows =  da.AsDataView();
                DataGridFiles.ItemsSource = rows;

                KeyValuePair<string, int>[] a = new KeyValuePair<string, int>[rows.Count] ;
                int i=0;
                foreach (DataRowView rowView in rows)
                {
                    DataRow row = rowView.Row;
                   
                    i++;
                }

                labelStock.Content = "Total Row: " + DataGridFiles.Items.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void LoadColumnChartData()
        {
           
        }
        private string DateConvert(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", null);

            return dt.ToString("yyyy-MM-dd");
        }

        private void DataGridFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource ;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void export2Excel_Click(object sender, RoutedEventArgs e)
        {
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                Excel.Application xlApp = new Excel.Application();

                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;
                var rows = GetDataGridRows(DataGridFiles);

                rowsTotal = DataGridFiles.Items.Count - 1;
                colsTotal = DataGridFiles.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridFiles.Columns[iC].Header;
                }

                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridFiles.Columns[iC].Header;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ((DataRowView)DataGridFiles.Items[I]).Row.ItemArray[j].ToString();
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;
                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
                xlApp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
        
        private void DataGridFiles_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                if (((System.Data.DataRowView)(e.Row.DataContext)).Row.ItemArray[8].ToString() == "NG")
                {
                    e.Row.Background = new SolidColorBrush(Colors.OrangeRed);
                }
                else
                    e.Row.Background = new SolidColorBrush(Colors.White);
            }
            catch
            {
            } 
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            popupCP = new repairhhp.Pages.Popup.Popup_Upload_Loss_Cal() { Owner = Window.GetWindow(this) };
            popupCP.ShowDialog();

            DataTable line = cnt.getData("  SELECT DISTINCT([TEST_ITEM])  FROM [PROD].[dbo].[TBL_MAPPING_LOSS_CAL] ");
            DataTable model = cnt.getData(" SELECT DISTINCT([BASIC MODEL])  FROM [PROD].[dbo].[LOSS_CAL_VIEW] ");
            
           
        }

    }
}
