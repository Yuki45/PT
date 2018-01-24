using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace repairhhp.ViewModel
{
    public class MdmCauseVM : ViewModelBase
    {
        public MdmCauseVM(Models.MdmCause model)
        {
            this.Model = model;
        }

        public Models.MdmCause Model { get; private set; }

        public string PageTitle
        {
            get { return this.Model.PageTitle; }
            set { this.Model.PageTitle = value; this.OnPropertyChanged("PageTitle"); }
        }
    }
}
