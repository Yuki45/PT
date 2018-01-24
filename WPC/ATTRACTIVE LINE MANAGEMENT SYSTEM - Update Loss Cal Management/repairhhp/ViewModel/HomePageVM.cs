using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repairhhp.ViewModel
{
    public class HomePageVM : ViewModelBase
    {
        public HomePageVM(Models.HomePage model)
        {
            this.Model = model;
        }

        public Models.HomePage Model { get; private set; }

        public string PageTitle
        {
            get{ return this.Model.PageTitle; }
            set{ this.Model.PageTitle = value;this.OnPropertyChanged("PageTitle"); }
        }
    }
}
