using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class SpecialLoanApplicationView
    {
        private readonly Nfmb _member;
        private readonly bool _transactionOpen;

        public SpecialLoanApplicationView(Nfmb member)
        {
            InitializeComponent();
            _member = member;

            btnSalaryAdvances.Click += (sender, args) => ShowSalaryAdvancesView();
            btnGoNegosyo.Click += (sender, args) => ShowGoNegosyoView();
            _transactionOpen = MainController.LoggedUser.TransactionDate == GlobalSettings.DateOfOpenTransaction;
        }

        private void ShowSalaryAdvancesView()
        {
            if (!Validate()) return;

            var view = new SalaryAdvanceView(_member);
            if (view.ShowDialog() == true)
            {
                DialogResult = true;
            }
        }

        private void ShowGoNegosyoView()
        {
            if (!Validate()) return;

            var view = new GoNegosyoView(_member);
            if (view.ShowDialog() == true)
            {
                DialogResult = true;
            }
        }

        private bool Validate()
        {
            if (!_transactionOpen)
            {
                MessageWindow.ShowAlertMessage("Cannot create transactions using current date settings.");
                return false;
            }
            return true;
        }
    }
}