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
using System.Windows.Threading;
using System.Timers;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for Machine.xaml
    /// </summary>
    public partial class Machine : UserControl
    {
        Controls.Machine cnt = new Controls.Machine();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        repairhhp.Pages.Popup.Popup_detail_rate popupCP;
        repairhhp.Pages.Popup.Popup_error_machine popupEP;
        public Machine()
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
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
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
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable da = cnt.getTable(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);

                DataView rows = da.AsDataView();
                int total=0, ok=0, defect=0;
                double rate=0;
                foreach (DataRowView rowView in rows)
                {
                    DataRow row = rowView.Row;
                    total = total + Convert.ToInt32(row.ItemArray[1].ToString());
                    ok = ok + Convert.ToInt32(row.ItemArray[2].ToString());
                    defect = defect + Convert.ToInt32(row.ItemArray[3].ToString());
                    rate = (Convert.ToDouble(defect)/Convert.ToDouble(total))*100;
                }
                if (rows.Count > 0)
                {
                    rown = da.NewRow();
                    rown["LINE NAME"] = "TOTAL";
                    rown["TOTAL QTY"] = total.ToString();
                    rown["OK QTY"] = ok.ToString();
                    rown["DEFECT QTY"] = defect.ToString();
                    rown["RATE(%)"] = Math.Round(rate, 1).ToString();
                    da.Rows.Add(rown);
                }

                dgRFCal.ItemsSource = rows;
                //string data = new { "Total", total.ToString(),ok.ToString(),defect.ToString(),"0" };

                //dgRFCal.Items.Add(new { Column1 = "TOTAL", Column2 = 10, Column3 = ok, Column4 = defect, Column5= 0 });
                
                DataTable ds = cnt.getFinal(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);
                DataView rowsds = ds.AsDataView();
                int totalds = 0, okds = 0, defectds = 0;
                double rateds = 0;
                foreach (DataRowView rowView in rowsds)
                {
                    DataRow row = rowView.Row;
                    totalds = totalds + Convert.ToInt32(row.ItemArray[1].ToString());
                    okds = okds + Convert.ToInt32(row.ItemArray[2].ToString());
                    defectds = defectds + Convert.ToInt32(row.ItemArray[3].ToString());
                    rateds = (Convert.ToDouble(defectds) / Convert.ToDouble(totalds))*100;
                }
                if (rowsds.Count > 0)
                {
                    rown = ds.NewRow();
                    rown["LINE NAME"] = "TOTAL";
                    rown["TOTAL QTY"] = totalds.ToString();
                    rown["OK QTY"] = okds.ToString();
                    rown["DEFECT QTY"] = defectds.ToString();
                    rown["RATE(%)"] = Math.Round(rateds, 1).ToString();
                    ds.Rows.Add(rown);
                }

                dgFinal.ItemsSource = rowsds;

                DataTable dd = cnt.getLcia(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);
                DataView rowsdd = dd.AsDataView();
                int totaldd = 0, okdd = 0, defectdd = 0;
                double ratedd = 0;
                foreach (DataRowView rowView in rowsdd)
                {
                    DataRow row = rowView.Row;
                    totaldd = totaldd + Convert.ToInt32(row.ItemArray[1].ToString());
                    okdd = okdd + Convert.ToInt32(row.ItemArray[2].ToString());
                    defectdd = defectdd + Convert.ToInt32(row.ItemArray[3].ToString());
                    ratedd = (Convert.ToDouble(defectdd) / Convert.ToDouble(totaldd))*100;
                }

                if (rowsdd.Count > 0)
                {
                    rown = dd.NewRow();
                    rown["LINE NAME"] = "TOTAL";
                    rown["TOTAL QTY"] = totaldd.ToString();
                    rown["OK QTY"] = okdd.ToString();
                    rown["DEFECT QTY"] = defectdd.ToString();
                    rown["RATE(%)"] = Math.Round(ratedd,1).ToString();
                    dd.Rows.Add(rown);
                }
                dgLCIA.ItemsSource = rowsdd;


                DataTable dm = cnt.getErrorMachine(cmbLine.Text, dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd"), cmbTimeStart.Text, cmbtimeEnd.Text);
                dgError.ItemsSource = dm.AsDataView();
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

        }

        private void export2Exceldata_Click(object sender, RoutedEventArgs e)
        {

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
            if (dgRFCal.Items.Count > 0)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgRFCal.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;

                if (popupCP != null)
                {
                    popupCP.Close();
                }
                popupCP = new repairhhp.Pages.Popup.Popup_detail_rate() { Owner = Window.GetWindow(this) };
                popupCP.date = " Where [INSPECT TIME] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbTimeStart.Text + ":00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbtimeEnd.Text + ":59:59.000' ";
                popupCP._line = " And [LINE NAME]= '" + dr1.ItemArray[0].ToString() + "' AND RESULT ='FAIL' ";
                popupCP._process = "RF CAL";
                popupCP.ShowDialog();
            }
        }

        private void dgFinal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgFinal.Items.Count > 0)
            {
                var grid = sender as DataGrid;
                if (grid.SelectedItem == null) return;
                DataRowView dr = dgFinal.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;

                if (popupCP != null)
                {
                    popupCP.Close();
                }
                popupCP = new repairhhp.Pages.Popup.Popup_detail_rate() { Owner = Window.GetWindow(this) };
                popupCP.date = " Where [INSPECT TIME] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbTimeStart.Text + ":00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbtimeEnd.Text + ":59:59.000' ";
                popupCP._line = " And [LINE NAME]= '" + dr1.ItemArray[0].ToString() + "' AND RESULT ='FAIL' ";
                popupCP._process = "FINAL";
                popupCP.ShowDialog();
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

                if (popupCP != null)
                {
                    popupCP.Close();
                }
                popupCP = new repairhhp.Pages.Popup.Popup_detail_rate() { Owner = Window.GetWindow(this) };
                popupCP.date = " Where [INSPECT TIME] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbTimeStart.Text + ":00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + cmbtimeEnd.Text + ":59:59.000' ";
                popupCP._line = " And [LINE NAME]= '" + dr1.ItemArray[0].ToString() + "' AND RESULT ='FAIL' ";
                popupCP._process = "LCIA";
                popupCP.ShowDialog();
            }
        }

        private void dgError_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgError.Items.Count > 0)
            {
                var grid = sender as DataGrid;
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
                popupEP.ShowDialog();
            }
        }
    }
}
