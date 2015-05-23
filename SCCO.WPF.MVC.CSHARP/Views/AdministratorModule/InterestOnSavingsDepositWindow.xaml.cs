using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    /// <summary>
    /// Interaction logic for InterestOnSavingsDepositWindow.xaml
    /// 
    /// 1. Get the average monthly end balance for a quarter
    ///     - Average end balance = (Jan Balance + Feb Balance + Mar Balance ) / 3
    /// 2. Calculate the interest earned
    ///     - Average end balance x Savings Deposit Interest Rate
    /// 3. Post in JV
    ///     - DEBIT: Coop - Interest on Savings Deposit 
    ///     - CREDIT: Member -Savings Deposit
    ///
    /// </summary>
    public partial class InterestOnSavingsDepositWindow
    {
        private readonly InterestOnSavingsDepositViewModel _viewModel;
        public InterestOnSavingsDepositWindow()
        {
            InitializeComponent();
            
            ProcessButton.Click += ProcesButtonOnClick;

            _viewModel = new InterestOnSavingsDepositViewModel();
            SavingsDepositController.InitializeModel(_viewModel);

            DataContext = _viewModel;
        }

        private void ProcesButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (QuarterComboBox.SelectedItem == null) return;
            _viewModel.Quarter = QuarterComboBox.SelectedIndex + 1;
            SavingsDepositController.ProcessInterestOnSavingsDeposit(_viewModel);
        }

        private void PostingButtonOnClick(object sender, RoutedEventArgs e)
        {
            var view = new PostInterestOnSavingsDepositView(_viewModel);
            if (view.ShowDialog() != true) return;

            // update settings in global variables
            GlobalSettings.Update(GlobalKeys.RateOfInterestOnSavingsDeposit.ToKeyword(),_viewModel.InterestRate);
            GlobalSettings.Update(GlobalKeys.AmountOfInterestOnSavingsDepositRequiredBalance.ToKeyword(), _viewModel.RequiredBalance);
            GlobalSettings.Update(GlobalKeys.CodeOfInterestExpenseOnSavingsDeposit.ToKeyword(), _viewModel.InterestExpenseOnSavingsDepositAccount.AccountCode);
        }
    }
}
