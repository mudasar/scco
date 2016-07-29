using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.SavingsDepositModule;
using SCCO.WPF.MVC.CS.Views.SearchModule;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    ///     Interaction logic for InterestOnSavingsDepositWindow.xaml
    ///     1. Get the average monthly end balance for a quarter
    ///     - Average end balance = (Jan Balance + Feb Balance + Mar Balance ) / 3
    ///     2. Calculate the interest earned
    ///     - Average end balance x Savings Deposit Interest Rate
    ///     3. Post in JV
    ///     - DEBIT: Coop - Interest on Savings Deposit
    ///     - CREDIT: Member -Savings Deposit
    /// </summary>
    public partial class InterestOnSavingsDepositWindow
    {
        private readonly InterestOnSavingsDepositViewModel _viewModel;

        public InterestOnSavingsDepositWindow()
        {
            InitializeComponent();

            ProcessButton.Click += ProcesButtonOnClick;
            SetupButton.Click += (sender, args) => ShowSetup();

            _viewModel = new InterestOnSavingsDepositViewModel();
            SavingsDepositController.InitializeModel(_viewModel);

            DataContext = _viewModel;
        }

        private void ShowSetup()
        {
            var setup = new SavingsDepositInterestPostingSetup();
            if (setup.ShowDialog() == true)
            {
                _viewModel.InterestRate = GlobalSettings.RateOfInterestOnSavingsDeposit;
                _viewModel.RequiredBalance = GlobalSettings.AmountOfInterestOnSavingsDepositRequiredBalance;
                _viewModel.InterestExpenseOnSavingsDepositAccount =
                    Account.FindByCode(GlobalSettings.CodeOfInterestExpenseOnSavingsDeposit);
                _viewModel.SavingsDepositAccount =
                    Account.FindByCode(GlobalSettings.CodeOfSavingsDeposit);
            }
        }

        private void ProcesButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (QuarterComboBox.SelectedItem == null) return;
            _viewModel.Quarter = QuarterComboBox.SelectedIndex + 1;
            if (_viewModel.SavingsDepositAccount == null ||
                string.IsNullOrEmpty(_viewModel.SavingsDepositAccount.AccountCode))
            {
                MessageWindow.ShowAlertMessage("Please select a Savings Deposit account.");
                return;
            }
            //SavingsDepositController.ProcessInterestOnSavingsDeposit(_viewModel);

            Mouse.OverrideCursor = Cursors.Wait;
            var transactionDate = MainController.LoggedUser.TransactionDate;
            var viewModel = new Utilities.BackgroundTasks.SavingsDepositInterestPostingWorker(_viewModel, transactionDate);
            var view = new ProgressView(viewModel, $"Interest on Savings Deposit ({_viewModel.Quarter})");
            view.ShowDialog();

            Mouse.OverrideCursor = Cursors.Arrow;

            if (!viewModel.Result.Success)
            {
                MessageWindow.ShowAlertMessage(viewModel.Result.Message);
                return;
            }
        }

        private void PostingButtonOnClick(object sender, RoutedEventArgs e)
        {
            var view = new PostInterestOnSavingsDepositView(_viewModel);
            if (view.ShowDialog() != true) return;

            // update settings in global variables
            GlobalSettings.Update(GlobalKeys.RateOfInterestOnSavingsDeposit.ToKeyword(), _viewModel.InterestRate);
            GlobalSettings.Update(GlobalKeys.AmountOfInterestOnSavingsDepositRequiredBalance.ToKeyword(),
                                  _viewModel.RequiredBalance);
            GlobalSettings.Update(GlobalKeys.CodeOfInterestExpenseOnSavingsDeposit.ToKeyword(),
                                  _viewModel.InterestExpenseOnSavingsDepositAccount.AccountCode);
        }
    }
}