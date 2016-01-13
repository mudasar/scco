using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SpecialLoansModule
{
    public partial class SpecialLoanApplicationView
    {
        private readonly Nfmb _member;

        public SpecialLoanApplicationView(Nfmb member)
        {
            InitializeComponent();
            _member = member;

            btnSalaryAdvances.Click += (sender, args) => ShowSalaryAdvancesView();
            btnGoNegosyo.Click += (sender, args) => ShowGoNegosyoView();
        }

        private void ShowSalaryAdvancesView()
        {
            var view = new SalaryAdvanceView(_member);
            if (view.ShowDialog() == true)
            {
                DialogResult = true;
            }
        }

        private void ShowGoNegosyoView()
        {
            var view = new GoNegosyoView(_member);
            if (view.ShowDialog() == true)
            {
                DialogResult = true;
            }
        }
    }
}