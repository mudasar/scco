using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.Sandbox
{
    /// <summary>
    /// Interaction logic for LoanChargesMaintenanceWindow.xaml
    /// </summary>
    public partial class LoanChargesMaintenanceWindow : IDataEntry
    {
        public LoanChargesMaintenanceWindow(Int32 loanProductId)
        {
            InitializeComponent();
            _currentLoanCharges = new LoanCharge();
            _currentLoanCharges.LoanProductId = loanProductId;
            DataContext = _currentLoanCharges;
        }

        public LoanChargesMaintenanceWindow(LoanCharge loanCharges, Int32 loanProductId)
        {
            InitializeComponent();
            _currentLoanCharges = loanCharges;
            DataContext = _currentLoanCharges;
        }

        private LoanCharge _currentLoanCharges;

        public void Create(object sender, RoutedEventArgs e)
        {
            DataContext = new LoanCharge();
        }

        public void Read(object sender, RoutedEventArgs e)
        {
            List<LoanCharge> loanChargesList = LoanCharge.GetList();
            List<SearchItem> searchItems =
                loanChargesList.Select(charges => new SearchItem(charges.ID, charges.AccountCode)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
        }

        public void Update(object sender, RoutedEventArgs e)
        {
            int loanProductId = _currentLoanCharges.LoanProductId;
            if (string.IsNullOrEmpty(_currentLoanCharges.AccountCode))
            {
                MessageWindow.ShowAlertMessage("Loan Charges of Operation must not be empty!");
                return;
            }
            if (_currentLoanCharges.ID == 0)
            {
                _currentLoanCharges.Create();
                _currentLoanCharges.ID = 0;
            }
            else
            {
                _currentLoanCharges.Update();
            }

            DataContext = _currentLoanCharges = new LoanCharge();
            _currentLoanCharges.LoanProductId = loanProductId;

            MessageWindow.ShowNotifyMessage("Loan Charges information saved!");
            CloseWindow(sender, e);
        }

        public void Delete(object sender, RoutedEventArgs e)
        {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Area information. Do you want to proceed?") ==
                MessageBoxResult.Yes)
            {
                _currentLoanCharges.Destroy();
                MessageWindow.ShowNotifyMessage("Loan Charges information deleted!");
                DataContext = _currentLoanCharges = new LoanCharge();
            }
        }

        private void SelectAccount(object sender, RoutedEventArgs e)
        {
            List<Account> accounts = Account.GetList();

            // create list of search items
            List<SearchItem> searchItems =
                accounts.Select(
                    account =>
                    new SearchItem(account.ID, account.AccountTitle)
                        {
                            ItemCode = account.AccountCode
                        }).ToList();

            var searchByCodeWindow = new SearchByCodeWindow(searchItems);
            searchByCodeWindow.ShowDialog();
            if (searchByCodeWindow.DialogResult != true)
                return;
            var accountSelect = new Account();
            accountSelect.Find(searchByCodeWindow.SelectedItem.ItemId);
            _currentLoanCharges.AccountCode = accountSelect.AccountCode;
            _currentLoanCharges.AccountTitle = accountSelect.AccountTitle;
            DataContext = new LoanCharge();
            DataContext = _currentLoanCharges;
        }

    }
}
