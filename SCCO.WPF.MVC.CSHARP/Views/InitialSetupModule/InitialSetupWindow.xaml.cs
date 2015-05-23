using SCCO.WPF.MVC.CS.Views.AccountModule;
using SCCO.WPF.MVC.CS.Views.BudgetModule;
using SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule;
using SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule;
using SCCO.WPF.MVC.CS.Views.SavingsDepositModule;

namespace SCCO.WPF.MVC.CS.Views.InitialSetupModule {

    public partial class InitialSetupWindow {
        public InitialSetupWindow() {
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
            btnForwardingBalance.Click += (sender, args) => ShowForwardingBalanceModule();
            btnGeneralLedgerBalance.Click += (sender, e) => ShowGeneralLedgerBalanceModule();
            btnLoanProducts.Click += (sender, args) => ShowLoanProductModule();
            btnProductImage.Click += (sender, args) => ShowProductImageModule();

            //setup

            btnReportManagement.Click += (sender, args) => ShowReportItemModule();
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
            var view = new UserModule.UserListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowCollectorModule()
        {
            //var collectorMaintenanceWindow = new CollectorMaintenanceWindow();
            var view = new CollectorModule.CollectorListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowMembershipTypeModule()
        {
            //var membershipTypeMaintenanceWindow = new MembershipTypeMaintenanceWindow();
            var view = new MembershipTypeModule.MembershipTypeListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowMembershipClassificationModule()
        {
            //var classificationMaintenanceWindow = new ClassificationMaintenanceWindow();
            var view = new ClassificationModule.ClassificationListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowAreaOfOperationModule()
        {
            //var areaMaintenanceWindow = new AreaMaintenanceWindow();
            var view = new AreaModule.AreaListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowDepartmentModule()
        {
            //var departmentMaintenanceWindow = new DepartmentMaintenanceWindow();
            var view = new DepartmentModule.DepartmentListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowBranchModule()
        {
            //var branchMaintenanceWindow = new BranchMaintenanceWindow();
            var view = new BranchModule.BranchListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowChartOfAccountModule()
        {
            //var chartOfAccountsWindow = new ChartOfAccountsWindow();
            var view = new AccountListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowAccountsPerGroup(string groupCode, string groupName)
        {
            var view = new AccountsPerGroupView(groupCode, groupName) { Owner = this };
            view.ShowDialog();
        }

        private void ShowForwardingBalanceModule()
        {
            var view = new ForwardedBalanceListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowDailySavingsWithdrawalSetup()
        {
            var view = new DailyWithdrawalSetupView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowLoanProductModule()
        {
            var view = new LoanModule.LoanProductsListWindow();
            view.Owner = this;
            view.ShowDialog();
        }


        private void ShowProductImageModule()
        {
            var view = new ProductImageModule.ProductImageListDetailView();
            view.Owner = this;
            view.ShowDialog();
        }

        private void ShowReportItemModule()
        {
            var view = new ReportItemModule.ReportItemsView();
            view.Owner = this;
            view.ShowDialog();
        }

    }
}
