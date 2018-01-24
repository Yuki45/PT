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
    public partial class Popup_detail_rate : MetroWindow
    {
        Controls.Machine cnt = new Controls.Machine();
        public String _un, _model, _line, _defect, date, _process;
        public Popup_detail_rate()
        {
            InitializeComponent();
        }

        public void LoadDataGrid()
        {
            string sql2="";
            if (_process=="RF CAL")
                sql2= " SELECT  [DEFECT NAME], COUNT([DEFECT NAME]) AS QTY  FROM [PROD].[dbo].[SPC_AGENT] " + date + _line+"  GROUP BY [DEFECT NAME] ORDER BY QTY DESC ";
            else
            if (_process=="FINAL")
                sql2 = " SELECT  [DEFECT NAME], COUNT([DEFECT NAME]) AS QTY  FROM [PROD].[dbo].[SPC_FINAL] " + date + _line + "  GROUP BY [DEFECT NAME] ORDER BY QTY DESC ";
            else
            if (_process == "LCIA")
                sql2 = " SELECT  [DEFECT NAME], COUNT([DEFECT NAME]) AS QTY  FROM [PROD].[dbo].[SPC_LCIA] " + date + _line + "  GROUP BY [DEFECT NAME] ORDER BY QTY DESC ";

            DataTable tbl = cnt.getData(sql2);
            dgWorst.ItemsSource = tbl.AsDataView();
        }

        private void popupLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            this.LoadDataGrid();
        }

        private void dgWorst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           if (dgWorst.Items.Count > 0)
            {
               string sql="";
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgWorst.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;
               if (_process=="RF CAL")
                   sql = "SELECT '" + dr1.ItemArray[0].ToString() + "' AS DEFECT,[PORT NO], COUNT([PORT NO]) AS [QTY]  FROM [PROD].[dbo].[SPC_AGENT]  ";
               else
                if (_process=="FINAL")
                    sql = "SELECT '" + dr1.ItemArray[0].ToString() + "' AS DEFECT,[PORT NO], COUNT([PORT NO]) AS [QTY]  FROM [PROD].[dbo].[SPC_FINAL] ";
                else
               if (_process == "LCIA")
                   sql = "SELECT '" + dr1.ItemArray[0].ToString() + "' AS DEFECT,[PORT NO], COUNT([PORT NO]) AS [QTY]  FROM [PROD].[dbo].[SPC_LCIA] ";

               DataTable tbl = cnt.getData(sql + date + _line + "AND [DEFECT NAME]='" + dr1.ItemArray[0].ToString() + "' GROUP BY [PORT NO] ORDER BY COUNT([PORT NO]) DESC");
                dgDefect.ItemsSource = tbl.AsDataView();
            }
        }

        private void dgDefect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgDefect.Items.Count > 0)
            {
                string sql = "";
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgDefect.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;

                if (_process=="RF CAL")
                sql = "SELECT *  FROM [PROD].[dbo].[SPC_AGENT] ";
                else
                if (_process=="FINAL")
                    sql = "SELECT *  FROM [PROD].[dbo].[SPC_FINAL] ";
                else
                if (_process == "LCIA")
                    sql = "SELECT *  FROM [PROD].[dbo].[SPC_LCIA] ";

                DataTable tbl = cnt.getData(sql + date + _line + "AND [DEFECT NAME]='" + dr1.ItemArray[0].ToString() + "' AND [PORT NO]='" + dr1.ItemArray[1].ToString() + "' ORDER BY [INSPECT TIME] ASC");
                dgDetailPort.ItemsSource = tbl.AsDataView();
            }
        }

        private void export2Excel_Click(object sender, RoutedEventArgs e)
        {
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            if (dgWorst.Items.Count>0)
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                excelWorksheet.Name = "Worst Defect";

                xlApp.Visible = true;

                rowsTotal = dgWorst.Items.Count - 1;
                colsTotal = dgWorst.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dgWorst.Columns[iC].Header;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ((DataRowView)dgWorst.Items[I]).Row.ItemArray[j].ToString();
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
                if (dgDefect.Items.Count > 0)
                {
                    excelWorksheet = (Excel.Worksheet)excelBook.Worksheets.Add();
                    excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                    excelWorksheet.Name = "Defect By Port Qty";
                    xlApp.Visible = true;

                    rowsTotal = dgDefect.Items.Count - 1;
                    colsTotal = dgDefect.Columns.Count - 1;
                    _with1 = excelWorksheet;
                    _with1.Cells.Select();
                    _with1.Cells.Delete();
                    for (iC = 0; iC <= colsTotal; iC++)
                    {
                        _with1.Cells[1, iC + 1].Value = dgDefect.Columns[iC].Header;
                    }
                    for (I = 0; I <= rowsTotal - 1; I++)
                    {
                        for (j = 0; j <= colsTotal; j++)
                        {
                            _with1.Cells[I + 2, j + 1].value = ((DataRowView)dgDefect.Items[I]).Row.ItemArray[j].ToString();
                        }
                    }
                    _with1.Rows["1:1"].Font.FontStyle = "Bold";
                    _with1.Rows["1:1"].Font.Size = 12;

                    _with1.Cells.Columns.AutoFit();
                    _with1.Cells.Select();
                    _with1.Cells.EntireColumn.AutoFit();
                    _with1.Cells[1, 1].Select();
                }

                if (dgDetailPort.Items.Count > 0)
                {
                    excelWorksheet = (Excel.Worksheet)excelBook.Worksheets.Add();
                    excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                    excelWorksheet.Name = "Detail List By Port";
                    xlApp.Visible = true;

                    rowsTotal = dgDetailPort.Items.Count - 1;
                    colsTotal = dgDetailPort.Columns.Count - 1;
                    _with1 = excelWorksheet;
                    _with1.Cells.Select();
                    _with1.Cells.Delete();
                    for (iC = 0; iC <= colsTotal; iC++)
                    {
                        _with1.Cells[1, iC + 1].Value = dgDetailPort.Columns[iC].Header;
                    }
                    for (I = 0; I <= rowsTotal - 1; I++)
                    {
                        for (j = 0; j <= colsTotal; j++)
                        {
                            _with1.Cells[I + 2, j + 1].value = ((DataRowView)dgDetailPort.Items[I]).Row.ItemArray[j].ToString();
                        }
                    }
                    _with1.Rows["1:1"].Font.FontStyle = "Bold";
                    _with1.Rows["1:1"].Font.Size = 12;

                    _with1.Cells.Columns.AutoFit();
                    _with1.Cells.Select();
                    _with1.Cells.EntireColumn.AutoFit();
                    _with1.Cells[1, 1].Select();
                }

                xlApp = null;
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }

            
        }
    }
}
