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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data.SqlClient;
using System.Collections;

namespace repairhhp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            userid.Focus();
            this.KeyDown += new KeyEventHandler(enterPress);
        }

        public static Conn _Conn = new Conn();
        public static SqlCommand command;
        public string _userid;
        public string _password;
        public string _name;
        public string _gen;
        string[] sslog = new string[6];

        private void verLogin(object sender, RoutedEventArgs e)
        {
            _userid = userid.Text;
            _password = password.Password;
            if (_userid != null && _password != null)
            {
                try
                {
                    SqlConnection connection = _Conn.Connect();
                    string sql = "SELECT TOP 1000 [USER_ID],[USER_NAME],[USER_DEPT],[USER_AUTH]  FROM [WPC].[dbo].[TBL_LOGIN] WHERE [USER_ID]='" + _userid + "' AND [USER_PASS] = '" + _password + "'";
                    command = new SqlCommand(sql, connection);
                    connection.Open();
                    SqlDataReader dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        _name = dataReader["USER_NAME"].ToString();
                        _gen = dataReader["USER_ID"].ToString();
                        sslog[0] = _name;
                        sslog[1] = _gen;
                        sslog[2] = dataReader["USER_AUTH"].ToString();
                        sslog[3] = xIp.Content.ToString();
                        sslog[4] = "";
                        sslog[5] = dataReader["USER_DEPT"].ToString();
                        Application.Current.Properties["SessionLogin"] = sslog;
                    }
                    connection.Close();
                    if (_name != null)
                    {
                        Dashboard window = new Dashboard() { DataContext = new ViewModel.DashboardVM() };
                        window.xName.Content = _name;
                        window.xGen.Content = _gen;
                        window.xIp.Content = xIp.Content;
                        window.Show();
                        Application.Current.MainWindow.Close();
                    }
                    else
                    {
                        MessageBox.Show("Login Failed");
                        userid.Text = "";
                        password.Password = "";
                    }
                }
                catch (Exception)
                {
                    // MessageBox.Show("Login Failed");
                    userid.Text = "";
                    password.Password = "";
                }

            }
            else
            {
                MessageBox.Show("Login Failed");
                userid.Text = "";
                password.Password = "";
            }

        }

        private void enterPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.verLogin(sender, e);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            sslog[0] = "GUEST";
            sslog[1] = "GUEST";
            sslog[2] = "GUEST";
            sslog[3] = xIp.Content.ToString();
            sslog[4] = "";// dataReader["P_GROUP"].ToString();
            sslog[5] = "GUEST";
            Application.Current.Properties["SessionLogin"] = sslog;

            Dashboard window = new Dashboard() { DataContext = new ViewModel.DashboardVM() };
            window.xName.Content = _name;
            window.xGen.Content = _gen;
            window.xIp.Content = xIp.Content;
            window.Show();
            Application.Current.MainWindow.Close();

        }
    }

}
