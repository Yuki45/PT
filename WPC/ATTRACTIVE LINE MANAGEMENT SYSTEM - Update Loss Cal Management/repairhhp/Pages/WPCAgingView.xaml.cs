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

namespace repairhhp.Pages
{
    /// <summary>
    /// Interaction logic for WPCAgingView.xaml
    /// </summary>
    public partial class WPCAgingView : UserControl
    {
        Controls.Attractive cnt = new Controls.Attractive();
        public string _name, _gen, _auth, _team, _group, _ip;
        public WPCAgingView()
        {
            InitializeComponent();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            txtID.Text = _gen;
            txtIP.Text = _sslog[3]; 
            this.clearField();
            gettable();
        }

        private void gettable()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getTable();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Total Row: " + DataGridFiles.Items.Count.ToString();
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

         
        private void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private void clearGI_Click(object sender, RoutedEventArgs e)
        {
            this.clearField();
            
        }

        private void clearField()
        {
            txtReason.Text = "";
            txtReq.Text = "";
            txtAfter.Text = "0";
            txtIP.Text = _ip;
            txtID.Text = _gen;
            txtBefore.Text = cnt.getTime();
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            string _dept = cmbDept.Text.ToUpper();
            string _ip = txtIP.Text.ToUpper();
            string _reason = txtReason.Text+ " Time Change Before: "+txtBefore.Text+" After:"+txtAfter.Text;
            string _req = txtReq.Text.ToUpper();
            string _id = txtID.Text.ToUpper();
            string _before = txtBefore.Text;
            string _after = txtAfter.Text;
            if (_id == string.Empty || _reason.Length <=15 || _req =="" ||_after=="" ||_after =="0")
            {
                var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                mainview.ShowMessageAsync("Information", "Please Insert Data Complete");
            }
            else
            {
                string hasil = cnt.UpTimeWPC(_id,_req,_reason,_dept, _ip, _before, _after);

                if (hasil == "OK")
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Success", "WPC Time finish Change :"+txtAfter.Text );
                    gettable();
                    this.clearField();
                }
                else
                {
                    var mainview = System.Windows.Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                    mainview.ShowMessageAsync("Warning", hasil);
                }
            } 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gettable();
        }

    }
}
