﻿using System;
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
using System.Deployment.Application;

namespace repairhhp
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard
    {
        public string _auth;
        repairhhp.Pages.Popup.popup_change_pwd popupCP;
        public Dashboard()
        {
            InitializeComponent();
            //string obj = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            //this.Title = "HMS (HHP Management System) v"+obj;
        }

        
        private void WinLoaded(object sender, RoutedEventArgs e)
        {
            string[] _sslog = (string[])Application.Current.Properties["SessionLogin"];
            _auth = _sslog[2];
            //--------------------set up menu auth-------------------//
            if (_auth == "ADMIN")
            {
                menu_receipt.IsEnabled = true;
                menu_report.IsEnabled = true;
            }
            else if (_auth == "SUPER")
            {
                menu_receipt.IsEnabled = true;
                menu_report.IsEnabled = true;
            }
            else if (_auth == "KOORDINATOR")
            {
                menu_receipt.IsEnabled = true;
            }
            else if (_auth == "TECHNICIAN")
            {
                menu_receipt.IsEnabled = true;
                menu_report.IsEnabled = true;
            }

            if (_sslog[5] == "MTC")
            {
                menu_report.IsEnabled = true;
            }

            if (_auth == "GUEST")
            {
                menu_report.IsEnabled = true;
                menu_gathering.IsEnabled = true;
                menu_receipt.IsEnabled = false;
                menu_realtime1.IsEnabled = true;
            }

            if (_auth == "RACK ADMIN")
            {
                menu_receipt.Visibility = System.Windows.Visibility.Collapsed;
                menu_report.Visibility = System.Windows.Visibility.Collapsed;
                repair_menu.Visibility = System.Windows.Visibility.Collapsed;
                menu_master.Visibility = System.Windows.Visibility.Collapsed;
                menu_assets.Visibility = System.Windows.Visibility.Collapsed;
                menu_storages_T.Visibility = System.Windows.Visibility.Collapsed;
                menu_filter_management.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StatusBarItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (popupCP != null)
            {
                popupCP.Close();
            }
            popupCP = new repairhhp.Pages.Popup.popup_change_pwd() { Owner = Window.GetWindow(this) };
            popupCP.ShowDialog();
        }
        
    }
}
