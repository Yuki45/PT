using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace repairhhp.ViewModel
{
    public class RackGRVM : ViewModelBase
    {
        public RackGRVM(Models.RackGR model)
        {
            this.Model = model;
        }

        public Models.RackGR Model { get; private set; }

        public string PageTitle
        {
            get { return this.Model.PageTitle; }
            set { this.Model.PageTitle = value; this.OnPropertyChanged("PageTitle"); }
        }
    }
}
