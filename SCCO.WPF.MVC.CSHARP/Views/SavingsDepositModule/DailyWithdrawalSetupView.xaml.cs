using System.Windows;
using SCCO.WPF.MVC.CS.Models.SavingsDeposit;


namespace SCCO.WPF.MVC.CS.Views.SavingsDepositModule
{
    /// <summary>
    /// Interaction logic for DailySetup.xaml
    /// </summary>
    public partial class DailyWithdrawalSetupView
    {
        private readonly DailyWithdrawalSettings _dailyWithdrawalSettings;
        public DailyWithdrawalSetupView()
        {
            InitializeComponent();
            //TransactionDatePicker.SelectedDate = System.DateTime.Now;
            _dailyWithdrawalSettings = new DailyWithdrawalSettings();
            _dailyWithdrawalSettings.InitializeProperties();
            DataContext = _dailyWithdrawalSettings;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
           var result = _dailyWithdrawalSettings.Update();
            if(!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            Close();
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
