using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanDetailsWindow
    {
        private LoanDetails _loanDetails;
        private readonly VoucherTypes _voucherType;
        private readonly int _voucherId;

        public LoanDetailsWindow(VoucherTypes voucherType, int voucherId)
        {
            _voucherType = voucherType;
            _voucherId = voucherId;
            _loanDetails = LoanDetails.FindByVoucher(voucherType, voucherId);

            InitializeComponent();
            InitializeControls();
            DataContext = _loanDetails;
            PopulateCoMakers();
        }
        public LoanDetailsWindow(LoanDetails loanDetails)
        {
            InitializeComponent();
            InitializeControls();
            _loanDetails = loanDetails;
            DataContext = _loanDetails;
            PopulateCoMakers();
        }

        private void PopulateCoMakers()
        {
            var count = _loanDetails.CoMakers.Length;
            if (count >= 1)
                lblCoMaker1.DataContext = _loanDetails.CoMakers[0];
            else
                lblCoMaker1.DataContext = new CoMaker();

            if (count >= 2)
                lblCoMaker2.DataContext = _loanDetails.CoMakers[1];
            else
                lblCoMaker2.DataContext = new CoMaker();

            if (count >= 3)
                lblCoMaker3.DataContext = _loanDetails.CoMakers[2];
            else
                lblCoMaker3.DataContext = new CoMaker();
        }

        private void InitializeControls()
        {
            btnEdit.Visibility = Controllers.MainController.LoggedUser.IsLoanAccountsManager
                                     ? Visibility.Visible
                                     : Visibility.Collapsed;

            btnEdit.Click += (sender, args) => ShowLoanDetailsEditView();
        }

        private void ShowLoanDetailsEditView()
        {
            if (_voucherId == 0) return;

            var view = new LoanDetailsEditView(_loanDetails) ;
            if (view.ShowDialog() == true)
            {
                _loanDetails.Update(_voucherType, _voucherId);
                Voucher.Touch(_voucherType, _voucherId, Controllers.MainController.LoggedUser.ID);
            }
            _loanDetails = LoanDetails.FindByVoucher(_voucherType, _voucherId);
            DataContext = _loanDetails;
        }
    }
}
