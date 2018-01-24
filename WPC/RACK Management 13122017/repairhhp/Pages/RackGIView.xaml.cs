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
    public partial class RackGIView : UserControl
    {
        Controls.RackGR cnt = new Controls.RackGR();
        public string _name, _gen, _auth, _team, _group, _ip;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer alarmTimer = new DispatcherTimer();
        string itemCode, strgCode, zoneCode, binCode;
        int notif = 0;
        public RackGIView()
        {
            InitializeComponent();
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
                                                inner join [MAIN].[dbo].[items] on [MAIN].[dbo].[items].id = [MAIN].[dbo].[materialstocks].item_id" + ((cmbItem.Text == "") ? "" : " Where [item_name]='" + cmbItem.Text + "'"));
                DataView rows = stock.AsDataView();
                DataStock.ItemsSource = rows;

                string date = " Where [out_date] between '" + dtStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + "00:00:00.000' And '" + dtEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd") + " " + "23:59:59.000' ";
                DataTable trx = cnt.getData(@" SELECT [items_slip] as [TRX No.]
                                                ,[item_name] as [Names]
                                                ,[qty] as Qty
                                                ,[loc_req] as [Location Request]
                                                ,[sender_pic] as [Sender Name]
                                                ,[recepient_pic] as [Recepient Name]
                                                ,[out_date] as [Transaction Date]
                                                ,[rmk] as [Remark]
                                                FROM [MAIN].[dbo].[materialout]
                                                INNER JOIN [MAIN].[dbo].items on [MAIN].[dbo].items.id = [MAIN].[dbo].[materialout].item_id " + date);
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

        private void Enable(bool stat)
        {
            locReq.IsEnabled = stat;
            Qty.IsEnabled = stat;
            PicReq.IsEnabled = stat;
            Rmk.IsEnabled = stat;
            btnSave.IsEnabled = stat;
            btnCancel.IsEnabled = stat;
        }

        private void Clear()
        {
            snSlip.Text = "";
            itemName.Text = "";
            codeItem.Text = "";
            Qty.Text = "";
            Qty_Stock.Text = "";
            locReq.Text = "";
            PicReq.Text = "";
            Rmk.Text = "";
        }

        private void AddItem()
        {
            DataTable tbl = cnt.getItem();
            cmbItem.Items.Clear();
            foreach (DataRow row in tbl.Rows)
            {
                cmbItem.Items.Add(row[1].ToString());
            }
        }
        private void snSlip_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataTable trx = cnt.getData(@" SELECT 
                                                [MAIN].[dbo].[materialtrx].[item_id]
                                                ,[item_name] 
                                                ,[qty]-ISNULL(h.QtyStock,0) as Qty 
                                                FROM [MAIN].[dbo].[materialtrx]
                                                INNER JOIN [MAIN].[dbo].items on [MAIN].[dbo].items.id = [MAIN].[dbo].[materialtrx].item_id
                                                LEFT JOIN (SELECT item_slip,SUM(Qty) as QtyStock FROM  [MAIN].[dbo].[materialtrx]Where [trx_type]='GI' GROUP BY [item_slip]) as h on  h.item_slip = [MAIN].[dbo].[materialtrx].item_slip
                                                Where [trx_type]='GR KITTING' And [MAIN].[dbo].[materialtrx].[item_slip]='" + snSlip.Text + "'");
                foreach (DataRow row in trx.Rows)
                {
                    if (row["Qty"].ToString() != "0")
                    {
                        itemName.Text = row["item_name"].ToString();
                        codeItem.Text = row["item_id"].ToString();
                        Qty_Stock.Text = row["Qty"].ToString();
                        Enable(true);
                        Qty.Focus();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Enable(false);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (snSlip.Text == "" || codeItem.Text == "" || Qty_Stock.Text == "" || Qty.Text == "" || locReq.Text == "" || PicReq.Text == "" || Rmk.Text.Length <=40)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please Input All Data with code = * And Remark less than 15 character ");
                return;
            }
            if (Convert.ToInt32(Qty.Text)>Convert.ToInt32(Qty_Stock.Text))
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please Input Qty less than or same with Qty Stock ");
                return;
            }
            //EXEC [dbo].[SP_STORAGE_GI] '20171215001', 1, 'OQC', 2,'Test Human', 'Test Human', 'dsfsdf fdsdfds fdsf '
            DataTable tbl = cnt.getData("EXEC [MAIN].[dbo].[SP_STORAGE_GI] '" + snSlip.Text + "', " + codeItem.Text + ", '" + locReq.Text + "', " + Qty.Text + ", '" + PicReq.Text + "', '" + _name + "', '" + Rmk.Text + "'");
            foreach (DataRow row in tbl.Rows)
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", row[0].ToString());
            }

            gettable();
            Clear();
            Enable(false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gettable();
            Clear();
            Enable(false);
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }
        
    }
}
