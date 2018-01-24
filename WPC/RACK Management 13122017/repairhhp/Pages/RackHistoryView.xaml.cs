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
using MahApps.Metro.Controls.Dialogs;

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for Machine.xaml
    /// </summary>
    public partial class RackHistoryView : UserControl
    {
        Controls.RackGR cnt = new Controls.RackGR();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer alarmTimer = new DispatcherTimer();
        string itemCode, strgCode, zoneCode, binCode;
        int ready, warning, danger;
        int notif = 0;
        public RackHistoryView()
        {
            InitializeComponent();
        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            AddItem();
            AddStorage();
            gettable();
        }

        private void gettable()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable stock = cnt.getData(@" SELECT TOP 1000 [item_slip] as [TRX NO]
                                            ,[strg_name] as [STORAGE LOCATION]
                                            ,[zone_name] as [ZONE LOCATION]
                                            ,[bin_code] as [BIN LOCATION]
                                            ,[item_name]  as [ITEM NAME]
                                            ,[qty]  as QTY
                                            ,[item_unit] as [UNIT]
                                            ,[trx_type] as [GR/GI]
                                            ,[trx_date]  as[TRANSACTION TIME]
                                            FROM [MAIN].[dbo].[materialtrx]
                                            INNER JOIN [MAIN].[dbo].STORAGES on [MAIN].[dbo].STORAGES.[strg_code] = [MAIN].[dbo].[materialtrx].[strg_code]
                                            INNER JOIN [MAIN].[dbo].zones on [MAIN].[dbo].zones.zone_code = [MAIN].[dbo].[materialtrx].zone_code And [MAIN].[dbo].zones.strg_code = [MAIN].[dbo].[materialtrx].[strg_code]
                                            INNER JOIN [MAIN].[dbo].items on [MAIN].[dbo].items.id = [MAIN].[dbo].[materialtrx].item_id ");
                DataView rows = stock.AsDataView();
                DataStock.ItemsSource = rows;
               
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
            
            alarmTimer.Start();
        }

        private void submitDec_Click_1(object sender, RoutedEventArgs e)
        {            
            alarmTimer.Interval = new TimeSpan(0, 0,2);
            alarmTimer.Start();
        }


        private void AddItem()
        {
            DataTable tbl = cnt.getItem();
            ItemSearch.Items.Clear(); 
            ItemSearch.Items.Add("");
            foreach (DataRow row in tbl.Rows)
            {
                ItemSearch.Items.Add(row[1].ToString());
            }
        }

        private void AddStorage()
        {
            DataTable tbl = cnt.getStorage();
            cmbStorage.Items.Clear();
            cmbStorage.Items.Add("");
            foreach(DataRow row in tbl.Rows)
            {
                cmbStorage.Items.Add(row[1].ToString());
            }
        }

        private void AddZone()
        {
            DataTable tbl = cnt.getZone(strgCode);
            cmbZone.Items.Clear();
            cmbZone.Items.Add("");
            foreach (DataRow row in tbl.Rows)
            {
                cmbZone.Items.Add(row[1].ToString());
            }
        }

        private void AddBin()
        {
            DataTable tbl = cnt.getBin(strgCode,zoneCode);
            cmbBin.Items.Clear();
            cmbBin.Items.Add("");
            foreach (DataRow row in tbl.Rows)
            {
                cmbBin.Items.Add(row[0].ToString());
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
            AddStorage();
            gettable();
        }

        private void cmbStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedText = (string)cmbStorage.SelectedItem;
            if (selectedText != "-" || selectedText != "")
            {
                strgCode = cnt.getStorageCode(selectedText);
                AddZone();
            }
        }


        private void cmbZone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = (string)cmbZone.SelectedItem;
            if (selectedText != "-" || selectedText != "")
            {
                zoneCode = cnt.getZoneCode(selectedText, strgCode);
                AddBin();
            }
        }
                
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            gettable();
            ready = 0;
            warning = 0;
            danger = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void DataStock_LoadingRow(object sender, DataGridRowEventArgs e)
        {
             //--168  OK  --504 Warning  -- Danger
  
            
        }

    }
}
