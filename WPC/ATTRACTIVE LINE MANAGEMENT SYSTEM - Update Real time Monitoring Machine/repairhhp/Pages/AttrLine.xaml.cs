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
    /// Interaction logic for AttrLine.xaml
    /// </summary>
    public partial class AttrLine : UserControl
    {
        Controls.Attractive cnt = new Controls.Attractive();
        public string _name, _gen, _auth, _team, _group, _ip;
        public AttrLine()
        {
            InitializeComponent();
            DataTable line = cnt.getData("SELECT [NAME_line]  FROM [PROD].[dbo].[MASTER_LINE] ");
            cmbLineOri.Items.Clear();
            cmbLineOri.Items.Add("ALL");
            cmbLineAtt.Items.Clear();
            cmbLineAtt.Items.Add("ALL");
            foreach (DataRow r in line.Rows)
            {
                cmbLineAtt.Items.Add(r[0].ToString());
                cmbLineOri.Items.Add(r[0].ToString());
            }
        }

        private void clearGI_Click(object sender, RoutedEventArgs e)
        {
            clearField();
        }
        private void clearField()
        {
            txtPoAtt.Text = "";
            txtPoOri.Text = "";
            cmbLineAtt.Text = "";
            cmbLineOri.Text = "";
            txtModelAtt.Text = "";
            txtModelOri.Text = "";
            dtOri.Text = "";
            dtAtt.Text = "";
            rtReason.Document.Blocks.Clear();
            //string richText = new TextRange(richTextBox1.Document.ContentStart, richTextBox1.Document.ContentEnd).Text;
            //richTextBox1.Document.Blocks.Add(new Paragraph(new Run("Text")));
        }
        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
           // this.clearField();
            gettable();
        }

        private void gettable()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = cnt.getAtt();
            da.Fill(ds, "data");
            DataGridFiles.ItemsSource = new DataView(ds.Tables["data"]);
            labelStock.Content = "Total Row: " + DataGridFiles.Items.Count.ToString();
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
          
        }
    }
}
