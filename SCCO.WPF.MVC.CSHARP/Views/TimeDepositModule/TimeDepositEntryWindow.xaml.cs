namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositEntryWindow
    {
        private readonly Models.TimeDeposit.TimeDepositDetails _timeDepositDetails;

        public TimeDepositEntryWindow(Models.TimeDeposit.TimeDepositDetails timeDepositDetails)
        {
            InitializeComponent();
            _timeDepositDetails = timeDepositDetails;
            DataContext = _timeDepositDetails;
        }
    }
}
