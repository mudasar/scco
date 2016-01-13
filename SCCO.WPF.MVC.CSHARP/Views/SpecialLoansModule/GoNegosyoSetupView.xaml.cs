using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.InitialSetupModule;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class GoNegosyoSetupView
    {
        private readonly GoNegosyoSetupViewModel _goNegosyoSetupViewModel;

        public GoNegosyoSetupView()
        {
            InitializeComponent();
            _goNegosyoSetupViewModel = new GoNegosyoSetupViewModel();
            _goNegosyoSetupViewModel.InitializeProperties();
            InitializeControls();
            DataContext = _goNegosyoSetupViewModel;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Result result = _goNegosyoSetupViewModel.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            Close();
        }

        private void InitializeControls()
        {
            stbGoNegosyoCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _goNegosyoSetupViewModel.GoNegosyoAccount = account;
                };
            stbApMerchandiseAccountCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _goNegosyoSetupViewModel.AccountsPayableMerchandiseAccount = account;
                };
        }

        private Account FindAccount()
        {
            List<Account> accounts = Account.GetList();
            List<SearchItem> searchItems =
                accounts.Select(
                    loan =>
                    new SearchItem(loan.ID, loan.AccountTitle) {ItemCode = loan.AccountCode}).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
            {
                return null;
            }
            Account account = accounts.SingleOrDefault(ac => ac.ID == searchByCodeWindow.SelectedItem.ItemId);
            return account;
        }
    }
}