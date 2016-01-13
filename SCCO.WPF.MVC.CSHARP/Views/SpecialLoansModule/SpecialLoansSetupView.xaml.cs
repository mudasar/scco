using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Views.LoanModule;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class SpecialLoansSetupView
    {
        public SpecialLoansSetupView()
        {
            InitializeComponent();

            btnSalaryAdvancesSetup.Click += (sender, args) => ShowSalaryAdvancesSetupView();
            btnSalaryAdvancesProduct.Click += (sender, args) => ShowSalaryAdvanceProductView();
            btnGoNegosyoSetup.Click += (sender, args) => ShowGoNegosyoSetupView();
            btnGoNegosyoProduct.Click += (sender, args) => ShowGoNegosyoProductView();
        }

        private void ShowSalaryAdvancesSetupView()
        {
            var view = new SalaryAdvanceSetupView();
            view.ShowDialog();
        }

        private void ShowGoNegosyoSetupView()
        {
            var view = new GoNegosyoSetupView();
            view.ShowDialog();
        }

        private void ShowSalaryAdvanceProductView()
        {
            string code = GlobalSettings.CodeOfSalaryAdvance;
            LoanProduct loanProduct = LoanProduct.FindBy("ProductCode", code);
            if (loanProduct == null)
            {
                MessageWindow.ShowAlertMessage(
                    "No Loan Product defined for Salary Advance Loan. Please check Loan Products module.");
                return;
            }
            var view = new EditLoanProductView(loanProduct.ID);
            view.ShowDialog();
        }

        private void ShowGoNegosyoProductView()
        {
            string code = GlobalSettings.CodeOfGoNegosyo;
            LoanProduct loanProduct = LoanProduct.FindBy("ProductCode", code);
            if (loanProduct == null)
            {
                MessageWindow.ShowAlertMessage(
                    "No Loan Product defined for Go Negosyo Loan. Please check Loan Products module.");
                return;
            }
            var view = new EditLoanProductView(loanProduct.ID);
            view.ShowDialog();
        }
    }
}