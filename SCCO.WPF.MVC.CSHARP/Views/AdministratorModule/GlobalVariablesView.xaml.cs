using System.Windows;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class GlobalVariablesView
    {
        private readonly GlobalVariablesViewModel _model;

        public GlobalVariablesView()
        {
            InitializeComponent();

            _model = new GlobalVariablesViewModel();
            _model.Initialize();
            DataContext = _model;

            btnUpdate.Click += UpdateButtonOnClick;
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _model.CodeOfCapitalBuildUp.Update();
            _model.CodeOfCashOnHand.Update();
            _model.CodeOfCompany.Update();
            _model.CodeOfInterestExpenseOnSavingsDeposit.Update();
            _model.CodeOfInterestIncomeFromLoans.Update();
            _model.CodeOfLoanReceivables.Update();
            _model.CodeOfMiscellaneousIncome.Update();
            _model.CodeOfSalaryAdvance.Update();
            _model.CodeOfSavingsDeposit.Update();
            _model.CodeOfTimeDeposit.Update();
            _model.CodeOfUnearnedIncome.Update();
            DialogResult = true;
            Close();
        }
    }
}