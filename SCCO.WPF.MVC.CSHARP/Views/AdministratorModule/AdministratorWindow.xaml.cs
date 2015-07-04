using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities.DbfMigration.Views;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class AdministratorWindow
    {
        public AdministratorWindow()
        {
            InitializeComponent();

            InterestOnSavingsDepositButton.Click += InterestOnSavingsDepositButtonOnClick;

            OpenTransactionDateButton.Click += (sender, args) => ShowTransactionDateSetup();

            GlobalVariablesButton.Click += GlobalVariablesButtonOnClick;

            UnearnedInterestFromLoansButton.Click += UnearnedInterestFromLoansButtonOnClick;

            BackupDatabaseButton.Click += (sender, args) =>
                {
                    var view = new BackUpDatabaseWindow();
                    view.ShowDialog();
                };

            DividendDistributionButton.Click += (sender, args) =>
                {
                    var view = new DividendDistributionWindow();
                    view.ShowDialog();
                };

            PatronageRefundButton.Click += (sender, args) =>
                {
                    var view = new PatronageRefundWindow();
                    view.ShowDialog();
                };

            UpdateBeginningBalanceButton.Click += (sender, args) =>
                {
                    var view = new UpdateBeginningBalanceWindow();
                    view.ShowDialog();
                };

            MigrateFromDbfButton.Click += (sender, args) =>
            {
                var view = new MigrateDbfToMySqlWindow();
                view.ShowDialog();
            };
        }

        private void UnearnedInterestFromLoansButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var view = new UnearnedInterestFromLoansWindow();
            view.ShowDialog();
        }

        private void GlobalVariablesButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var view = new GlobalVariablesView();
            view.ShowDialog();
        }

        private void InterestOnSavingsDepositButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var view = new InterestOnSavingsDepositWindow();
            view.ShowDialog();
        }

        private void ShowTransactionDateSetup()
        {
            var transactionDatePicker = new TransactionDatePickerWindow { Owner = this };
            transactionDatePicker.ShowDialog();
        }
    }
}