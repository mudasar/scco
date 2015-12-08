using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public partial class GeneralLedgerAccountVerifierWindow
    {
        private AccountVerifierGeneralLedgerViewModel _viewModel;

        public GeneralLedgerAccountVerifierWindow()
        {
            InitializeComponent();

            _viewModel = new AccountVerifierGeneralLedgerViewModel();

            DataContext = _viewModel;

            btnSearchAccount.Click += (sender, args) => OnSearchAccount();
        }

        private void OnSearchAccount()
        {
            Account account = MainController.SearchAccount();
            if (account == null) return;

            DateTime transactionDate = MainController.LoggedUser.TransactionDate;
            _viewModel = AccountVerifierGeneralLedgerViewModel.Refresh(account, transactionDate);
            DataContext = _viewModel;
        }
    }
}