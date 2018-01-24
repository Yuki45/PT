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
using System.IO;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;

namespace repairhhp.Pages.Popup
{
    /// <summary>
    /// Interaction logic for Popup_history.xaml
    /// </summary>
    public partial class Popup_Upload_Loss_Cal : MetroWindow
    {
        Controls.Losscal cnt = new Controls.Losscal ();
        public String _un, _model, _line, _defect, date;
        public Popup_Upload_Loss_Cal()
        {
            InitializeComponent();
        }

        public void LoadDataGrid()
        {
            string sql = "SELECT '"+_defect+"' AS DEFECT,[PORT NO], COUNT([PORT NO]) AS [QTY DEFECT]  FROM [PROD].[dbo].[SPC_AGENT] ";
            DataTable tbl = cnt.getData(sql + date + _line +" GROUP BY [PORT NO]");
            dataGrid.ItemsSource = tbl.AsDataView();
        }

        private void popupLoaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            bool status = false;
            string un= "";
            DirectoryInfo dir = new DirectoryInfo(txtFolder.Text );
            FileInfo[] files = dir.GetFiles().OrderByDescending(p => p.LastWriteTime).ToArray();
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                foreach (FileInfo file in files)
                {
                    string filename = file.Name;
                    string[] info = filename.Split('_');
                    string model = info[1];
                    string temp = filename.Substring(filename.Length - 4, 4);
                    if (temp == ".dec")
                    {
                        var lines = File.ReadAllLines(txtFolder.Text + @"\" + filename);
                        var dict = new Dictionary<string, string>();
                        try
                        {
                            status = false;
                            foreach (var s in lines)
                            {
                                if (s.Substring(0, 3) == "[UN")
                                {
                                    string a = s.Replace("[", "");
                                    a = a.Replace("]", "");
                                    var section = a.Split('_');
                                    if (section[1].Length > 5)
                                    {
                                        un = section[1];
                                        status = true;
                                    }

                                }
                                else
                                    if (status)
                                    {
                                        try
                                        {
                                            var split = s.Split('=');
                                            string hasil = model + un + split[0] + split[1];
                                            var data = new Test { Modeln = model, unq = un, item = split[0], vsl = split[1] };
                                            dataGrid.Items.Add(data);
                                            string i = cnt.LossCal(model, un, split[0], split[1]);
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        status = false;
                                    }
                            }
                        }
                        catch { }
                    }
                }
                MessageBox.Show( "Upload Data Complete...! \r\n Thank You","Information"); 
            }
            catch { }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void Source_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                txtFolder.Text = dialog.SelectedPath.ToString();
            }
        }
    }

    public class Test
    {
        public string Modeln { get; set; }
        public string unq { get; set; }

        public string item { get; set; }

        public string vsl { get; set; }


    }
}
