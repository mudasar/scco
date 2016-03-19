using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SavingsDepositModule
{
    public partial class SavingsDepositInterestPostingSetup
    {
        private readonly SavingsDepositInterestPostingSetupViewModel _viewModel;

        public SavingsDepositInterestPostingSetup()
        {
            InitializeComponent();

            _viewModel = new SavingsDepositInterestPostingSetupViewModel();
            _viewModel.Initialize();
            DataContext = _viewModel;

            btnUpdate.Click += UpdateButtonOnClick;
            stbSavingsDepositAccount.Click += delegate
                {
                    Account account = MainController.SearchAccount();
                    if (account == null) return;
                    _viewModel.CodeOfSavingsDeposit = account.AccountCode;
                };
            stbInterestExpenseOnSavings.Click += delegate
                {
                    Account account = MainController.SearchAccount();
                    if (account == null) return;
                    _viewModel.CodeOfInterestExpenseOnSavingsDeposit = account.AccountCode;
                };
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.Update();

            DialogResult = true;
            Close();
        }
    }
}