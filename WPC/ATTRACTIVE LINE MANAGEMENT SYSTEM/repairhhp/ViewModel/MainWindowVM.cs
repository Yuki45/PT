using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace repairhhp.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly Models.MainWindow _item = new Models.MainWindow();

        public MainWindowVM()
        {
            this.Get_IP();
        }

        public string UserIP
        {
            get{ return _item.UserIP; }
            set{ _item.UserIP = value; this.OnPropertyChanged("UserIP"); }
        }

        private void Get_IP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                _item.UserIP = addr.ToString();
            }
            
        }

    }
}
