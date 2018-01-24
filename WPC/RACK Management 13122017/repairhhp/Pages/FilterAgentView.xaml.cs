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
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for Machine.xaml
    /// </summary>
    public partial class FilterAgentView : UserControl
    {
        Controls.CFilterAgent cnt = new Controls.CFilterAgent();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public FilterAgentView()
        {
            InitializeComponent();

            cmbUse.Items.Clear();
            cmbUse.Items.Add("N");
            cmbUse.Items.Add("Y");
            DataTable line = cnt.getData(@"SELECT [ERROR_CODE]
                                        ,[ERROR_NAME]
                                        ,[STATUS_FILTER]
                                        FROM [MAIN].[dbo].[TBL_FLT_DEFT_G]
                                        WHERE [DEL_USE] = 'N'");
            
           // dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
           
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            cmbUse.SelectedIndex = 0;
        }

        DataRow rown ;
        private void gettable()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable da = cnt.getTable(searchGrid.Text, cmbUse.Text);

                DataView rows = da.AsDataView();
                dgSummary.ItemsSource = rows;
                
                
                
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString());
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            
           gettable();
                
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
                DataRowView dr = dgSummary.SelectedItem as DataRowView;
                DataRow dr1 = dr.Row;
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();

                if (MessageBox.Show("Close Are You Sure, want to DeActive this Defect ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    cnt.UpFilter("N", dr1.ItemArray[0].ToString());
                }
                else
                {
                    cnt.UpFilter("Y", dr1.ItemArray[0].ToString());
                }
                gettable();
            }
        }
               
    }
}
