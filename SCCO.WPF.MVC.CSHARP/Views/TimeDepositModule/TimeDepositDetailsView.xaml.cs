using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositDetailsView
    {
        private readonly TimeDepositDetails _timeDepositDetails;
        public TimeDepositDetailsView(TimeDepositDetails timeDepositDetails)
        {
            InitializeComponent();
            _timeDepositDetails = timeDepositDetails;
            DataContext = _timeDepositDetails;
        }
    }
}
