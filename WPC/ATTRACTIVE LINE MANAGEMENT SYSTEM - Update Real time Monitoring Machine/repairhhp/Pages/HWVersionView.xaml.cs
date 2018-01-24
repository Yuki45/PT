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
    public partial class HWVersionView : UserControl
    {
        Controls.Machine cnt = new Controls.Machine();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer alarmTimer = new DispatcherTimer();
        int notif = 0;
        public HWVersionView()
        {
            InitializeComponent();
            DataTable line = cnt.getData("SELECT [NAME_line]  FROM [PROD].[dbo].[MASTER_LINE] ");
            cmbLine.Items.Clear();
            cmbLine.Items.Add("ALL");
            foreach (DataRow r in line.Rows)
            {
                cmbLine.Items.Add(r[0].ToString());
            }
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            alarmTimer.Tick += new EventHandler(alarmTimer_Tick);
            
        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            cmbTime.SelectedIndex = 0;
        }

        private void gettable()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable da = cnt.getHW(cmbLine.Text);

                DataView rows = da.AsDataView();
                dgRFCal.ItemsSource = rows;
            }
            catch (Exception ex)
            {
              
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            gettable();
            dispatcherTimer.Start();
        }

        private void alarmTimer_Tick(object sender, EventArgs e)
        {
            alarmTimer.Stop();
            DataTable hw = cnt.getData("SELECT [LINE NAME], [PC NO], [BASIC MODEL], [HW VERSION], [MODEL_INFO].[HW] AS [SPEC HW VERSION]  FROM [PROD].[dbo].[HW_MONITORING] " +
                                        "LEFT JOIN [PROD].[dbo].[MODEL_INFO] on [MODEL_INFO].[BASIC_MODEL] = [HW_MONITORING].[BASIC MODEL] " +
                                        " Where [MODEL_INFO].[HW] <> [HW_MONITORING].[HW VERSION]");

           dgHW.ItemsSource = hw.AsDataView();
           if (dgHW.Items.Count > 0)
           {
               if (notif == 0)
               {
                   dgHW.Background = Brushes.Red;
                   notif = 1;
               }
               else
               {
                   dgHW.Background = Brushes.White;
                   notif = 0;
               }
           }
            alarmTimer.Start();
        }

        private void submitDec_Click_1(object sender, RoutedEventArgs e)
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

            alarmTimer.Interval = new TimeSpan(0, 0, 1);
            alarmTimer.Start();
        }


    }
}
