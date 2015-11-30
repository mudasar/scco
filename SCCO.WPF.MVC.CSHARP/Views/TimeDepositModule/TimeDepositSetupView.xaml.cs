using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositSetupView
    {
        private readonly TimeDepositSetupViewModel _timeDepositSetupViewModel;

        public TimeDepositSetupView()
        {
            InitializeComponent();
            _timeDepositSetupViewModel = new TimeDepositSetupViewModel();
            _timeDepositSetupViewModel.InitializeProperties();
            InitializeControls();
            DataContext = _timeDepositSetupViewModel;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            var result = _timeDepositSetupViewModel.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            Close();
        }

        private void InitializeControls()
        {
            stbInterestExpenseCode.Click += delegate
                {
                    var account = FindAccount();
                    if (account == null) return;
                    _timeDepositSetupViewModel.InterestExpenseAccount = account;
                };
            stbServiceFeeCode.Click += delegate
            {
                var account = FindAccount();
                if (account == null) return;
                _timeDepositSetupViewModel.ServiceFeeAccount = account;
            };
        }

        private Account FindAccount()
        {
            var accounts = Account.GetList();
            var searchItems =
                accounts.Select(
                    loan =>
                    new SearchItem(loan.ID, loan.AccountTitle) {ItemCode = loan.AccountCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
            {
                return null;
            }
            var account = accounts.SingleOrDefault(ac => ac.ID == searchByCodeWindow.SelectedItem.ItemId);
            return account;
        }
    }
}
