﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace repairhhp.ViewModel
{
    public class TransferPEVM : ViewModelBase
    {
        public TransferPEVM(Models.TransferPE model)
        {
            this.Model = model;
        }

        public Models.TransferPE Model { get; private set; }

        public string PageTitle
        {
            get { return this.Model.PageTitle; }
            set { this.Model.PageTitle = value; this.OnPropertyChanged("PageTitle"); }
        }
    }
}