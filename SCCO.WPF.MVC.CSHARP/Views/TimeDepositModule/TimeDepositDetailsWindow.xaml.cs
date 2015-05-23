using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositDetailsWindow
    {
        private readonly TimeDepositDetails _timeDepositDetails;
        public TimeDepositDetailsWindow(TimeDepositDetails timeDepositDetails)
        {
            InitializeComponent();
            _timeDepositDetails = timeDepositDetails;
            DataContext = _timeDepositDetails;
        }
    }
}
