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
    public partial class RackGRView : UserControl
    {
        Controls.RackGR cnt = new Controls.RackGR();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer alarmTimer = new DispatcherTimer();
        string itemCode, strgCode, zoneCode, binCode;
        int notif = 0;
        public RackGRView()
        {
            InitializeComponent();

            cmbUnit.Items.Add("UNIT");
            cmbUnit.Items.Add("BOX");
            cmbUnit.Items.Add("CARTON");
            cmbUnit.Items.Add("PALLET");

        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            Enable(false);
            AddItem();
            this.dtStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.dtEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            gettable();
        }

        private void gettable()
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable stock = cnt.getData(@" SELECT  TOP 1000
                                                [strg_code] as STORAGE
                                                ,[zone_code] as ZONE
                                                ,[bin_code] as BIN
                                                ,[item_name] as [ITEM NAME] 
                                                ,[qty] as QTY
                                                ,[item_unit] as [UNIT]
                                                ,[reg_date] as [REGISTER TIME]
                                                ,[reg_user] as [REGISTER USER]
                                                ,[modify_date] as [UPDATE TIME]
                                                ,[modify_user] as [UPDATE USER]
                                                FROM   [MAIN].[dbo].[materialstocks]
                                                inner join [MAIN].[dbo].[items] on [MAIN].[dbo].[items].id = [MAIN].[dbo].[materialstocks].item_id"+ ((ItemSearch.Text == "") ? "" : " Where [item_name]='"+ItemSearch.Text+"'"));
                DataView rows = stock.AsDataView();
                DataStock.ItemsSource = rows;

                string date = " And [trx_date] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + "00:00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + "23:59:59.000' ";
                DataTable trx = cnt.getData(@" SELECT TOP 1000 [item_slip] as [TRX NO]
                                            ,[strg_name] as [STORAGE LOCATION]
                                            ,[zone_name] as [ZONE LOCATION]
                                            ,[bin_code] as [BIN LOCATION]
                                            ,[item_name]  as [ITEM NAME]
                                            ,[qty]  as QTY
                                            ,[item_unit] as [UNIT]
                                            ,[trx_date]  as[TRANSACTION TIME]
                                            FROM [MAIN].[dbo].[materialtrx]
                                            INNER JOIN [MAIN].[dbo].STORAGES on [MAIN].[dbo].STORAGES.[strg_code] = [MAIN].[dbo].[materialtrx].[strg_code]
                                            INNER JOIN [MAIN].[dbo].zones on [MAIN].[dbo].zones.zone_code = [MAIN].[dbo].[materialtrx].zone_code And [MAIN].[dbo].zones.strg_code = [MAIN].[dbo].[materialtrx].[strg_code]
                                            INNER JOIN [MAIN].[dbo].items on [MAIN].[dbo].items.id = [MAIN].[dbo].[materialtrx].item_id
                                            Where [trx_type]='GR KITTING' "+date);
                DataView trxs = trx.AsDataView();
                DataTRX.ItemsSource = trxs;

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

        private void Enable( bool stat)
        {
            cmbItem.IsEnabled = stat;
            cmbStorage.IsEnabled = stat;
            cmbZone.IsEnabled = stat;
            cmbBin.IsEnabled = stat;
            btnSave.IsEnabled = stat;
            btnCancel.IsEnabled = stat;
            Qty.IsEnabled = stat;
            cmbUnit.IsEnabled = stat;
        }

        private void Clear()
        {
            snSlip.Text = "";
            cmbItem.Text = "";
            cmbStorage.Text = "";
            cmbZone.Text = "";
            cmbBin.Text = "";
            codeItem.Text = "";
            codeStorage.Text = "";
            codeZone.Text = "";
            cmbUnit.Text = "";
            Qty.Text = "";
        }

        private void AddItem()
        {
            DataTable tbl = cnt.getItem();
            cmbItem.Items.Clear();
            ItemSearch.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                cmbItem.Items.Add(row[1].ToString());
                ItemSearch.Items.Add(row[1].ToString());
            }
        }

        private void AddStorage()
        {
            DataTable tbl = cnt.getStorage();
            cmbStorage.Items.Clear();
            foreach(DataRow row in tbl.Rows)
            {
                cmbStorage.Items.Add(row[1].ToString());
            }
        }

        private void AddZone()
        {
            DataTable tbl = cnt.getZone(codeStorage.Text);
            cmbZone.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                cmbZone.Items.Add(row[1].ToString());
            }
        }

        private void AddBin()
        {
            DataTable tbl = cnt.getBin(codeStorage.Text,codeZone.Text);
            cmbBin.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                cmbBin.Items.Add(row[0].ToString());
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Enable(true);
            AddItem();
            AddStorage();
            snSlip.Text = cnt.GenSlipNo();
            gettable();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Enable(false);
        }
        
        private void cmbStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedText = (string)cmbStorage.SelectedItem;
            if (selectedText != "-" || selectedText != "")
            {
                codeStorage.Text = cnt.getStorageCode(selectedText);
                AddZone();
            }
        }

        private void cmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = (string)cmbItem.SelectedItem;
            if (selectedText != "-" || selectedText != "")
            {
                codeItem.Text = cnt.getItemCode(selectedText);
            }
        }

        private void cmbZone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = (string)cmbZone.SelectedItem;
            if (selectedText != "-" || selectedText != "")
            {
                codeZone.Text = cnt.getZoneCode(selectedText, codeStorage.Text);
                AddBin();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (snSlip.Text=="" || codeItem.Text=="" || codeStorage.Text =="" || codeZone.Text =="" || cmbBin.Text =="" || Qty.Text =="" )
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please Input All Data with code = *");
                return ;
            }

            DataTable tbl = cnt.getData("EXEC [MAIN].[dbo].[SP_STORAGE_GR] '" + snSlip.Text + "', '" + codeStorage.Text + "', '" + codeZone.Text + "', '" + cmbBin.Text + "', " + codeItem.Text + ", " + Qty.Text + ", '" + cmbUnit.Text + "', '" + _gen + "'");
            foreach (DataRow row in tbl.Rows)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", row[0].ToString());
            }

            gettable();
            Clear();
            Enable(false);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            gettable();
            Clear();
            Enable(false);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }


    }
}
