using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class UnearnedInterestFromLoansSetupView
    {
        private readonly UnearnedInterestFromLoansPostingSetupViewModel _viewModel;

        public UnearnedInterestFromLoansSetupView()
        {
            InitializeComponent();
            _viewModel = new UnearnedInterestFromLoansPostingSetupViewModel();
            _viewModel.InitializeData();
            DataContext = _viewModel;

            WireEvents();
        }

        private void WireEvents()
        {
            ShareCapitalCodeSearchControl.Click += (sender, args) =>
                {
                    var account = MainController.SearchAccount();
                    if (account != null && !string.IsNullOrEmpty(account.AccountCode))
                    {
                        _viewModel.CodeOfShareCapital = account.AccountCode;
                        GlobalSettings.Update(GlobalKeys.CodeOfShareCapital.ToString(), account.AccountCode);
                    }
                };

            ShareCapitalRequiredAmount.LostFocus += (s, e) =>
                {
                    if (_viewModel.AmountOfShareCapitalRequiredBalance >= 0)
                    {
                        GlobalSettings.Update(GlobalKeys.AmountOfShareCapitalRequiredBalance.ToString(),
                                              _viewModel.AmountOfShareCapitalRequiredBalance);
                    }
                };

            UnearnedIncomeCodeSearchControl.Click += (sender, args) =>
                {
                    var account = MainController.SearchAccount();
                    if (account != null && !string.IsNullOrEmpty(account.AccountCode))
                    {
                        _viewModel.CodeOfUnearnedIncome = account.AccountCode;
                        GlobalSettings.Update(GlobalKeys.CodeOfUnearnedIncome.ToString(), account.AccountCode);
                    }
                };

            InterestIncomeFromLoansCodeSearchControl.Click += (sender, args) =>
                {
                    var account = MainController.SearchAccount();
                    if (account != null && !string.IsNullOrEmpty(account.AccountCode))
                    {
                        _viewModel.CodeOfInterestIncomeFromLoans = account.AccountCode;
                        GlobalSettings.Update(GlobalKeys.CodeOfInterestIncomeFromLoans.ToString(), account.AccountCode);
                    }
                };

            MiscellaneousIncomeCodeSearchControl.Click += (sender, args) =>
                {
                    var account = MainController.SearchAccount();
                    if (account != null && !string.IsNullOrEmpty(account.AccountCode))
                    {
                        _viewModel.CodeOfMiscellaneousIncome = account.AccountCode;
                        GlobalSettings.Update(GlobalKeys.CodeOfMiscellaneousIncome.ToString(), account.AccountCode);
                    }
                };
        }
    }
}