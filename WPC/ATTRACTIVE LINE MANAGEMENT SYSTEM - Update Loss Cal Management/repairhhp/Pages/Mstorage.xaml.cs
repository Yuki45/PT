using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Threading;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for Machine.xaml
    /// </summary>
    public partial class Mstorage : UserControl
    {
        Controls.Mstorage cnt = new Controls.Mstorage();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public Mstorage()
        {
            InitializeComponent();

            for (int no = 0; no < 24; no++)
            {
                cmbtimeEnd.Items.Add((no.ToString().Length == 1) ? "0" + no.ToString() : no.ToString());
                cmbTimeStart.Items.Add((no.ToString().Length == 1) ? "0" + no.ToString() : no.ToString());
            }

            DataTable line = cnt.getData("SELECT [NAME_line]  FROM [PROD].[dbo].[MASTER_LINE] ");
            cmbLine.Items.Clear();
            cmbLine.Items.Add("ALL");
            foreach (DataRow r in line.Rows)
            {
                cmbLine.Items.Add(r[0].ToString());
            }
           // dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            for (int x = 1; x <= 3; x++)
            {
                if (x == 1) cmbTime.Items.Add("5");
                else if (x == 2) cmbTime.Items.Add("10");
                else if (x == 3) cmbTime.Items.Add("20");
            }
        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            this.dtStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.dtEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cmbTimeStart.SelectedValue = "07";
            cmbtimeEnd.SelectedValue = "23";
            cmbTime.SelectedIndex = 0;
        }

        DataRow rown ;
        private void gettable()
        {
            try
            {
                if (chkhistory2.IsChecked== true)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    DataTable da = cnt.getHistory(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);

                    DataView rows = da.AsDataView();
                    dgSummary.ItemsSource = rows;
                }
                else
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    DataTable da = cnt.getTable(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);

                    DataView rows = da.AsDataView();
                    dgSummary.ItemsSource = rows;
                }
                
                
                
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            if (checkTimer.IsChecked == true)
            {
                
                    gettable();
                    dispatcherTimer.Interval = new TimeSpan(0, Convert.ToInt32(cmbTime.Text), 0);
                    dispatcherTimer.Start();

                
            }
            else
            {
                dispatcherTimer.Stop();
                gettable();
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
                var rows = GetDataGridRows(dgError);

                rowsTotal = dgError.Items.Count - 1;
                colsTotal = dgError.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dgError.Columns[iC].Header;
                }

                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dgError.Columns[iC].Header;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ((DataRowView)dgError.Items[I]).Row.ItemArray[j].ToString();
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
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void export2Exceldata_Click(object sender, RoutedEventArgs e)
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
                var rows = GetDataGridRows(dgLCIA);

                rowsTotal = dgLCIA.Items.Count - 1;
                colsTotal = dgLCIA.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dgLCIA.Columns[iC].Header;
                }

                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dgLCIA.Columns[iC].Header;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = ((DataRowView)dgLCIA.Items[I]).Row.ItemArray[j].ToString();
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
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        private void export2Excelworst_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            gettable();
            dispatcherTimer.Start();
        }

        private void dgRFCal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgSummary.Items.Count > 0)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                if (chkhistory2.IsChecked == true)
                {
                    DataTable ds = cnt.getFinalH(dgSummary.CurrentCell.Column.Header.ToString(), dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);
                    DataView rowsds = ds.AsDataView();
                    dgLCIA.ItemsSource = rowsds;
                    grINFO.Header = "Detail INFO Process " + dgSummary.CurrentCell.Column.Header.ToString() + " Row= " + rowsds.Count.ToString();
                }
                else
                {
                    DataTable ds = cnt.getFinal(dgSummary.CurrentCell.Column.Header.ToString(), dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);
                    DataView rowsds = ds.AsDataView();
                    dgLCIA.ItemsSource = rowsds;
                    grINFO.Header = "Detail INFO Process " + dgSummary.CurrentCell.Column.Header.ToString() + " Row= " + rowsds.Count.ToString();
                }
                
            }
        }

        private void dgLCIA_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgLCIA.Items.Count > 0)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgLCIA.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;

                DataTable dm = cnt.getErrorMachine(dr1.ItemArray[0].ToString());
                dgError.ItemsSource = dm.AsDataView();
            }
        }

        private void dgError_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgError.Items.Count > 0)
            {
                /*var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgError.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;

                if (popupEP != null)
                {
                    popupEP.Close();
                }
                popupEP = new repairhhp.Pages.Popup.Popup_error_machine() { Owner = Window.GetWindow(this) };
                popupEP.date = " Where [ERROR DATE] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbTimeStart.Text + ":00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbtimeEnd.Text + ":59:59.000' ";
                popupEP._line = " And [LINE NAME]= '" + dr1.ItemArray[0].ToString() + "'  ";
                popupEP.sql = "SELECT TOP 1000 [LINE NAME],[PROCESS NAME],[ERROR NAME], SUM(QTY) AS QTY  FROM [DB_LOST_HUNTER].[dbo].[MESIN_ERROR] "+ popupEP.date + popupEP._line +
                              "GROUP BY [LINE NAME], [PROCESS NAME] ,[ERROR NAME]  ORDER BY QTY DESC";
                popupEP.ShowDialog();*/
            }
        }

        private void dgSummary_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
        }
    }
}
