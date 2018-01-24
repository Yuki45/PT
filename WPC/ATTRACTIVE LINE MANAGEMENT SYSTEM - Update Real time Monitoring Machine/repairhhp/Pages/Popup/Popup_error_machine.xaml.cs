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
using Excel = Microsoft.Office.Interop.Excel;

namespace repairhhp.Pages.Popup
{
    /// <summary>
    /// Interaction logic for Popup_history.xaml
    /// </summary>
    public partial class Popup_error_machine : MetroWindow
    {
        Controls.Machine cnt = new Controls.Machine();
        public String _un, _model, _line, _defect, date, sql;
        public Popup_error_machine()
        {
            InitializeComponent();
        }

        public void LoadDataGrid()
        {
            DataTable tbl = cnt.getData(sql);
            dataGrid.ItemsSource = tbl.AsDataView();
        }

        private void popupLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.LoadDataGrid();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            if (dataGrid.Items.Count>0)
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                excelWorksheet.Name = "Detail Error Machine";

                xlApp.Visible = true;

                rowsTotal = dataGrid.Items.Count - 1;
                colsTotal = dataGrid.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGrid.Columns[iC].Header;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ((DataRowView)dataGrid.Items[I]).Row.ItemArray[j].ToString();
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
            catch { }
            finally
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
    }
}
