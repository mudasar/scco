
namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    /// Interaction logic for PatronageRefundWindow.xaml
    /// 
    /// 1. Get the base
    ///     - total Interest on Loan paid minus Rebate
    /// 2. Get the rate
    ///     - amount allocated / total base (all members with Interest on Loan for the previous year)
    /// 3. Get the Patronage Refund
    ///     - individual base * rate
    /// 4. Post in JV
    ///     - Debit: Patronage Refund
    ///     - Credit: Share Capital
    /// </summary>
    public partial class PatronageRefundWindow
    {
        private readonly PatronageRefundViewModel _viewModel;
        public PatronageRefundWindow()
        {
            InitializeComponent();
            _viewModel = new PatronageRefundViewModel();
            _viewModel.Initialize();

            DataContext = _viewModel;

            PostButton.Click += (sender, args) =>
                {
                    var valid = _viewModel.Validate();
                    if (valid.Success)
                    {
                        _viewModel.Process();
                        _viewModel.SaveSettings();

                        var message = "Patronage Refund Payable distribution succesfully posted! ";
                        message += string.Format("Please check JV {0}.", _viewModel.JournalVoucherNumber);
                        MessageWindow.ShowNotifyMessage(message);
                        DialogResult = true;
                        Close();
                    }
                    else
                    {
                        MessageWindow.ShowAlertMessage(valid.Message);
                    }
                };
        }
    }
}
