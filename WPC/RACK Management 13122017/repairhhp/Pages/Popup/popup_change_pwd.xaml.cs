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

namespace repairhhp.Pages.Popup
{
    /// <summary>
    /// Interaction logic for popup_change_pwd.xaml
    /// </summary>
    public partial class popup_change_pwd : Window
    {
        Controls.Profile cnt = new Controls.Profile();
        public string _name, _gen, _auth, _team, _group, _ip;
        public popup_change_pwd()
        {
            InitializeComponent();
        }

        private void UCLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _name = _sslog[0];
            _gen = _sslog[1];
            _ip = _sslog[3];
            _team = _sslog[3];
            txtID.Text = _gen;
            txtName.Text = _name;
            cmbDept.Text= _sslog[5];
        }

        private void clearGI_Click(object sender, RoutedEventArgs e)
        {
            txtID.Text = _gen;
            txtName.Text = _name;
            cmbDept.SelectedItem = _team ;
            this.Close();
        }

        private void submitDec_Click(object sender, RoutedEventArgs e)
        {
            string _dept = cmbDept.Text.ToUpper();
            string _name = txtName.Text.ToUpper();
            string _id = txtID.Text.ToUpper();
            string _old = txtoldPw.Password;
            string _New = txtnewPw.Password;
            string _Re = txtrePw.Password;
            if (_id == string.Empty || _name=="" || _dept == "" )
            {
                MessageBox.Show("Information", "Please Insert Data Complete");
                return;
            }
            if(_New != _Re )
            {
                MessageBox.Show("Information", "New Password & Re-Type Password Different");
                return;
            }
            else
            {
                string hasil = cnt.UpProfile(_id, _name, _old,_New,_dept);
                MessageBox.Show( hasil,"Information");
                this.Close();
            } 
        }
    }
}
