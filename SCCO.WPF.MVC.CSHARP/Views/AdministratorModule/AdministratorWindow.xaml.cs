using System.Windows;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class AdministratorWindow
    {
        public AdministratorWindow()
        {
            InitializeComponent();

            RefreshDisplay();

            if (Controllers.MainController.LoggedUser.LoginName == "jess.alejo")
            {
                MigrateFromDbfButton.Visibility = Visibility.Visible;
            }

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
                    var view = new Utilities.DbfMigration.Views.MigrateDbfToMySqlWindow();
                    view.ShowDialog();
                };

            RestoreFromBackupButton.Click += (sender, args) =>
                {
                    var view = new RestoreFromBackupWindow();
                    view.ShowDialog();
                };

            FinancialConditionReportConfigurationButton.Click += (sender, args) =>
                {
                    var view = new FinancialConditionReportConfigurationModule.ListItemsView();
                    view.ShowDialog();
                };
        }

        private void RefreshDisplay()
        {
            var currentDate = DatabaseUtility.CurrentDate();
            var userDate = Controllers.MainController.LoggedUser.TransactionDate;
            if (currentDate.Year != userDate.Year)
            {
                UpdateBeginningBalanceButton.IsEnabled = false;
                UnearnedInterestFromLoansButton.IsEnabled = false;
                DividendDistributionButton.IsEnabled = false;
                PatronageRefundButton.IsEnabled = false;

                if (currentDate.Month >= 2)
                {
                    InterestOnSavingsDepositButton.IsEnabled = false;
                }
            }           
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