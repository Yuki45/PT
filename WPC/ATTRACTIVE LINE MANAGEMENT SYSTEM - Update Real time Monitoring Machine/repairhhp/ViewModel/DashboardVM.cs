using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace repairhhp.ViewModel
{
    public class DashboardVM : ViewModelBase
    {   
        public DashboardVM()
        {
            this.LoadHomePage();
            this.LoadHomePageCommand = new DelegateCommand(o => this.LoadHomePage());
            this.LoadReceiptNvCommand = new DelegateCommand(o => this.LoadReceiptNv());
            this.LoadgatheringRFCommand = new DelegateCommand(o => this.LoadGatheringRF());
            this.LoadgatheringRealtimeCommand = new DelegateCommand(o => this.LoadRealTime());
            this.LoadAttractiveline = new DelegateCommand(o => this.LoadAttractive());
            this.LoadHwVersionCommand = new DelegateCommand(o => this.LoadHWVersion());
            this.LoadMdmCauseCommand = new DelegateCommand(o => this.LoadMdmCause());
            this.LoadMdmDefectCommand = new DelegateCommand(o => this.LoadMdmDefect());
        }

        public ICommand LoadHomePageCommand { get; private set; }
        public ICommand LoadReceiptNvCommand { get; private set; }
        public ICommand LoadgatheringRFCommand { get; private set; }
        public ICommand LoadAttractiveline { get; private set; }
        public ICommand LoadMdmDefectCommand { get; private set; }
        public ICommand LoadMdmCauseCommand { get; private set; }
        public ICommand LoadHwVersionCommand { get; private set; }

        public ICommand LoadgatheringRealtimeCommand { get; private set; }

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value; 
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        private void LoadHomePage()
        {
            CurrentViewModel = new HomePageVM(
                new Models.HomePage() { PageTitle = "Home"});
        }

        private void LoadRealTime()
        {
            CurrentViewModel = new MachineVM(
                new Models.Machine() { PageTitle = "Monitoring Real Time Machine " });
        }

        private void LoadReceiptNv()
        {
            CurrentViewModel = new WPC(
                new Models.WPC { PageTitle = "WPCAging" });
        }

        private void LoadHWVersion()
        {
            CurrentViewModel = new HWVersionVM(
                new Models.HWVersion { PageTitle = "Monitoring HW Version" });
        }

        private void LoadGatheringRF()
        {
            CurrentViewModel = new RFCalVM(
                new Models.RFCal() { PageTitle = "RFCAL" });
        }
        private void LoadAttractive()
        {
            CurrentViewModel = new AttractiveLineVM(
                new Models.Attractive() { PageTitle = "ATTRACTIVE" });
        }

        private void LoadMdmCause()
        {
            CurrentViewModel = new MdmCauseVM(
                new Models.MdmCause() { PageTitle = "MDM Cause" });
        }

        private void LoadMdmDefect()
        {
            CurrentViewModel = new MdmDefectVM(
                new Models.MdmDefect() { PageTitle = "MDM Cause" });
        }
    }
}
