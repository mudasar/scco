using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Views.SearchModule;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal partial class PaidInterestReconstructionView
    {
        private readonly LoanReconstructionViewModel _viewModel;

        public Result ActionResult { get; set; }

        public PaidInterestReconstructionView()
        {
            InitializeComponent();
            ActionResult = new Result(false, "No action initiated.");
        }

        public PaidInterestReconstructionView(LoanReconstructionViewModel viewModel)
            : this()
        {
            InitializeComponent();
            ActionResult = new Result(false, "No action");

            _viewModel = viewModel;
            _viewModel.InitializeContext();

            InitializeEventSubscription();
            DataContext = _viewModel;
        }

        private void InitializeEventSubscription()
        {
            cboTerm.SelectionChanged += (sender, args) =>
            {
                _viewModel.UpdateLoanDetails();
                _viewModel.UpdateChargesAndDeductions();
                _viewModel.AddOrEditSmap();
                _viewModel.UpdateTotalChargesAndDeductions();
            };

            btnReconstruct.Click += (sender, args) => Reconstruct();

            btnRefresh.Click += (sender, args) => _viewModel.UpdateLoanDetails();

            #region --- CO-MAKERS ---

            txtCoCode1.LostFocus += (sender, args) =>
            {
                txtCoName1.Text = _viewModel.FindCoMaker(txtCoCode1.Text).MemberName;
            };
            txtCoCode2.LostFocus += (sender, args) =>
            {
                txtCoName2.Text = _viewModel.FindCoMaker(txtCoCode2.Text).MemberName;
            };
            txtCoCode3.LostFocus += (sender, args) =>
            {
                txtCoName3.Text = _viewModel.FindCoMaker(txtCoCode3.Text).MemberName;
            };

            #endregion

            btnChangeLoanProduct.Click += (sender, args) => ChangeLoanProduct();

            btnAddEntry.Click += (sender, args) =>
            {
                var particular = new Particular();
                var view = new AddReconstructionDetailsView(particular);
                if (view.ShowDialog() == true)
                {
                    _viewModel.AddParticular(particular);
                }
            };

            btnRemoveEntry.Click += (sender, args) => _viewModel.RemoveSelectedParticular();

            btnORPosting.Click += (sender, args) => PaidInterestPosting();

        }

        private void Reconstruct()
        {
            ActionResult = _viewModel.Validate();
            if (!ActionResult.Success)
            {
                MessageWindow.ShowAlertMessage(ActionResult.Message);
                return;
            }

            // Check OR Number if present
            if (_viewModel.OrNumber <= 0)
            {
                MessageWindow.ShowAlertMessage("OR Number is required!");
                return;
            }

            var view = new PostJournalVoucherView(_viewModel.ReconstructionDate);
            if (view.ShowDialog() == false)
            {
                return;
            }

            ActionResult = _viewModel.PostPaidInterestReconstruction(view.JournalVoucher.VoucherNo, view.JournalVoucher.VoucherDate);
            if (ActionResult.Success)
            {
                const string message = "Posting successful!";
                MessageWindow.ShowNotifyMessage(message);
                ActionResult = new Result(true, message);
                MainController.ShowJournalVoucherWindow();
                Close();
            }
            else
            {
                MessageWindow.ShowAlertMessage(ActionResult.Message);
            }
        }

        private void PaidInterestPosting()
        {
            // post interest and other charges as loan payment to OR
            var or = new OfficialReceipt();
            or.SetAccount(_viewModel.LoanAccount);
            or.SetMember(_viewModel.Borrower);
            or.VoucherDate = _viewModel.ReconstructionDate;
            or.VoucherNo = _viewModel.OrNumber;
            or.Credit = _viewModel.TotalChargesAndDeductions;

            var view = new PaidInterestPostingView(or);
            if (view.ShowDialog() == false)
            {
                return;
            }

            _viewModel.OrNumber = or.VoucherNo;
        }

        private void ChangeLoanProduct()
        {
            var searchByCodeWindow = new SearchByCodeWindow(_viewModel.GetLoanProductsSearchItems());
            searchByCodeWindow.ShowDialog();

            if (searchByCodeWindow.DialogResult == false)
                return;
            var loanProduct = new Models.Loan.LoanProduct();
            loanProduct.Find(searchByCodeWindow.SelectedItem.ItemId);
            _viewModel.SetLoanProduct(loanProduct);
        }
    }
}