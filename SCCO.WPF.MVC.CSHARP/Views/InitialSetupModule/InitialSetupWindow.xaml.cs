using SCCO.WPF.MVC.CS.Views.AccountModule;
using SCCO.WPF.MVC.CS.Views.AreaModule;
using SCCO.WPF.MVC.CS.Views.BranchModule;
using SCCO.WPF.MVC.CS.Views.BudgetModule;
using SCCO.WPF.MVC.CS.Views.ClassificationModule;
using SCCO.WPF.MVC.CS.Views.CollectorModule;
using SCCO.WPF.MVC.CS.Views.DepartmentModule;
using SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule;
using SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule;
using SCCO.WPF.MVC.CS.Views.LoanModule;
using SCCO.WPF.MVC.CS.Views.MembershipTypeModule;
using SCCO.WPF.MVC.CS.Views.ProductImageModule;
using SCCO.WPF.MVC.CS.Views.ReportItemModule;
using SCCO.WPF.MVC.CS.Views.SavingsDepositModule;
using SCCO.WPF.MVC.CS.Views.SpecialLoansModule;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;
using SCCO.WPF.MVC.CS.Views.UserModule;

namespace SCCO.WPF.MVC.CS.Views.InitialSetupModule
{
    public partial class InitialSetupWindow
    {
        public InitialSetupWindow()
        {
            InitializeComponent();


            //modules
            btnCompany.Click += (sender, args) => ShowCompanyModule();
            btnUserInformation.Click += (sender, args) => ShowUserInformationModule();

            btnChartOfAccounts.Click += (sender, args) => ShowChartOfAccountModule();
            btnLoanReceivables.Click += (sender, args) => ShowAccountsPerGroup("LR", "Loan Receivable Accounts");
            btnTimeDeposits.Click += (sender, args) => ShowAccountsPerGroup("TD", "Time Deposit Accounts");
            btnInterestOnLoans.Click += (sender, args) => ShowAccountsPerGroup("IL", "Interest On Loans");
            btnFines.Click += (sender, args) => ShowAccountsPerGroup("FPS", "Fines, Penalties and Surcharges");
            btnSavingsDeposits.Click += (sender, args) => ShowAccountsPerGroup("SA", "Savings Deposit Accounts");
            btnShareCapital.Click += (sender, args) => ShowAccountsPerGroup("SC", "Share Capital Accounts");

            btnBudget.Click += (sender, args) => ShowBudgetModule();

            btnBranch.Click += (sender, args) => ShowBranchModule();
            btnDepartment.Click += (sender, args) => ShowDepartmentModule();
            btnCollector.Click += (sender, args) => ShowCollectorModule();
            btnAreaOfOperation.Click += (sender, args) => ShowAreaOfOperationModule();
            btnMembershipType.Click += (sender, args) => ShowMembershipTypeModule();
            btnMembershipClassification.Click += (sender, args) => ShowMembershipClassificationModule();

            // other module
            btnDailySavingsWithdrawal.Click += (sender, args) => ShowDailySavingsWithdrawalSetup();
            btnTimeDepositSetup.Click += (sender, args) => ShowTimeDepositSetup();
            btnForwardingBalance.Click += (sender, args) => ShowForwardingBalanceModule();
            btnGeneralLedgerBalance.Click += (sender, e) => ShowGeneralLedgerBalanceModule();
            btnLoanProducts.Click += (sender, args) => ShowLoanProductModule();
            btnProductImage.Click += (sender, args) => ShowProductImageModule();

            //setup

            btnReportManagement.Click += (sender, args) => ShowReportItemModule();
            btnSpecialLoans.Click += (sender, args) => ShowSpecialLoansSetupView();
        }

        private void ShowTimeDepositSetup()
        {
            var view = new TimeDepositSetupView {Owner = this};
            view.ShowDialog();
        }

        private void ShowGeneralLedgerBalanceModule()
        {
            var view = new GeneralLedgerBalanceListView();
            view.ShowDialog();
        }

        private void ShowBudgetModule()
        {
            var view = new BudgetsListView();
            view.ShowDialog();
        }


        private void ShowCompanyModule()
        {
            //var usersMaintenanceWindow = new UserMaintenanceWindow();
            var view = new CompanyView {Owner = this};
            view.ShowDialog();
        }

        private void ShowUserInformationModule()
        {
            //var usersMaintenanceWindow = new UserMaintenanceWindow();
            var view = new UserListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowCollectorModule()
        {
            //var collectorMaintenanceWindow = new CollectorMaintenanceWindow();
            var view = new CollectorListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowMembershipTypeModule()
        {
            //var membershipTypeMaintenanceWindow = new MembershipTypeMaintenanceWindow();
            var view = new MembershipTypeListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowMembershipClassificationModule()
        {
            //var classificationMaintenanceWindow = new ClassificationMaintenanceWindow();
            var view = new ClassificationListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowAreaOfOperationModule()
        {
            //var areaMaintenanceWindow = new AreaMaintenanceWindow();
            var view = new AreaListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowDepartmentModule()
        {
            //var departmentMaintenanceWindow = new DepartmentMaintenanceWindow();
            var view = new DepartmentListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowBranchModule()
        {
            //var branchMaintenanceWindow = new BranchMaintenanceWindow();
            var view = new BranchListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowChartOfAccountModule()
        {
            //var chartOfAccountsWindow = new ChartOfAccountsWindow();
            var view = new AccountListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowAccountsPerGroup(string groupCode, string groupName)
        {
            var view = new AccountsPerGroupView(groupCode, groupName) {Owner = this};
            view.ShowDialog();
        }

        private void ShowForwardingBalanceModule()
        {
            var view = new ForwardedBalanceListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowDailySavingsWithdrawalSetup()
        {
            var view = new DailyWithdrawalSetupView {Owner = this};
            view.ShowDialog();
        }

        private void ShowLoanProductModule()
        {
            var view = new LoanProductsListWindow {Owner = this};
            view.ShowDialog();
        }


        private void ShowProductImageModule()
        {
            var view = new ProductImageListDetailView {Owner = this};
            view.ShowDialog();
        }

        private void ShowReportItemModule()
        {
            var view = new ReportItemsView {Owner = this};
            view.ShowDialog();
        }

        private void ShowSpecialLoansSetupView()
        {
            var view = new SpecialLoansSetupView();
            view.ShowDialog();
        }
    }
}