using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class SalaryAdvanceSetupView
    {
        private readonly SalaryAdvanceSetupViewModel _salaryAdvanceSetupViewModel;

        public SalaryAdvanceSetupView()
        {
            InitializeComponent();
            _salaryAdvanceSetupViewModel = new SalaryAdvanceSetupViewModel();
            _salaryAdvanceSetupViewModel.InitializeProperties();
            InitializeControls();
            DataContext = _salaryAdvanceSetupViewModel;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Result result = _salaryAdvanceSetupViewModel.Update();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            Close();
        }

        private void InitializeControls()
        {
            stbSalaryAdvanceCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _salaryAdvanceSetupViewModel.SalaryAdvanceAccount = account;
                };
            stbMiscellaneousIncomeCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _salaryAdvanceSetupViewModel.MiscellaneousIncomeAccount = account;
                };
            stbCashOnHandCode.Click += delegate
                {
                    Account account = FindAccount();
                    if (account == null) return;
                    _salaryAdvanceSetupViewModel.CashOnHandAccount = account;
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