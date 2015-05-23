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
            var account = MainController.SearchAccount();
            if (account == null) return;

            _viewModel = AccountVerifierGeneralLedgerViewModel.Refresh(account,
                                                                       MainController.LoggedUser.TransactionDate);
            DataContext = _viewModel;
        }
    }
}