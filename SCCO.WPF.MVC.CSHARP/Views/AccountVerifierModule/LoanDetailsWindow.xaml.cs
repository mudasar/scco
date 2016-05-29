using System.Collections.Generic;
using System.Text;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Views.LoanModule;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    /// <summary>
    /// Interaction logic for LoanDetailsWindow.xaml
    /// </summary>
    public partial class LoanDetailsWindow
    {
        private readonly LoanDetails _loanDetails;
        private bool _enableCompromiseSettlement;

        public LoanDetailsWindow(LoanDetails loanDetails)
        {
            InitializeComponent();
            _loanDetails = loanDetails;
            DataContext = _loanDetails;

            var count = _loanDetails.CoMakers.Length;
            CoMaker1Label.DataContext = count >= 1 ? _loanDetails.CoMakers[0] : new CoMaker();

            CoMaker2Label.DataContext = count >= 2 ? _loanDetails.CoMakers[1] : new CoMaker();

            CoMaker3Label.DataContext = count >= 3 ? _loanDetails.CoMakers[2] : new CoMaker();

            btnNotices.Click += (s, e) =>
                {
                    var view = new LoanNoticesView(_loanDetails);
                    view.ShowDialog();
                };

            //btnCompromiseSettlement.Click += (sender, args) =>
            //    {
            //        var finesAndPenalty = new FinesRebateCalculatorViewModel
            //        {
            //            LoanDetails = _loanDetails,
            //            LoanBalance = LoanBalance,
            //            ProcessDate = MainController.LoggedUser.TransactionDate
            //        };
            //        //model.FinesRatePerMonth = 2/100; // 2% per month
            //        //model.FinesRatePerMonth = GlobalSettings.RateOfFinesPerMonth;

            //        finesAndPenalty.Calculate();
            //        var viewModel = new LoanCompromiseAgreementViewModel
            //            {
            //                JournalVoucherNumber = Voucher.LastDocumentNo(VoucherTypes.JV) + 1,
            //                LoanBalance = LoanBalance,
            //                LoanDetails = _loanDetails,
            //                FinesAndPenalty = finesAndPenalty.Fines
            //            };
            //        var view = new LoanCompromiseAgreementView(viewModel);
            //        view.ShowDialog();
            //    };

            PaidReconstructionButton.Click += (sender, args) => ShowPaidInterestLoanReconstruction();
            AddOnReconstructionButton.Click += (sender, args) => ShowAddOnInterestLoanReconstruction();

            var allowLoanReconstruction = MainController.LoggedUser.CanAccessJournalVoucher;
            PaidReconstructionButton.IsEnabled = allowLoanReconstruction;
            AddOnReconstructionButton.IsEnabled = allowLoanReconstruction;
        }

        public bool EnableCompromiseSettlement
        {
            get { return _enableCompromiseSettlement; }
            set
            {
                _enableCompromiseSettlement = value;
                btnCompromiseSettlement.IsEnabled = value;
            }
        }

        public decimal LoanBalance { get; set; }

        private void ShowPaidInterestLoanReconstruction()
        {
            var view = new PaidInterestReconstructionView(InitializeLoanReconstructionViewModel());
            view.ShowDialog();
            if(view.ActionResult.Success)
            {
                Close();
            }
        }

        private void ShowAddOnInterestLoanReconstruction()
        {
            var viewModel = new AddOnInterestLoanReconstructionViewModel();
            viewModel.PreviousLoanDetails = _loanDetails;
            viewModel.LoanBalance = LoanBalance;
            viewModel.ReconstructionDate = MainController.LoggedUser.TransactionDate;
            viewModel.LoanManager = MainController.LoggedUser;

            viewModel.ConfigureAccounts();
            var result = viewModel.Validate();
            if (!result.Success)
            {
                const string message = "Some settings required by this application are not set. " +
                                       "Do you want to run setup?";
                if (MessageWindow.ShowConfirmMessage(message) == MessageBoxResult.Yes)
                {
                    var setup = new LoanReconstructionSetupView();
                    setup.ShowDialog();
                    return;
                }
            }
            var view = new AddOnInterestLoanReconstructionView(viewModel);
            view.ShowDialog();
            if (view.ActionResult.Success)
            {
                Close();
            }
        }

        private LoanReconstructionViewModel InitializeLoanReconstructionViewModel()
        {
            var viewModel = new LoanReconstructionViewModel
                {
                    DocumentDate = MainController.LoggedUser.TransactionDate,
                    DocumentNumber = Voucher.LastDocumentNo(VoucherTypes.JV) + 1,
                    LoanApplied = Account.FindByCode(_loanDetails.AccountCode),
                    Member = Nfmb.FindByCode(_loanDetails.MemberCode),
                    LoanBalance = LoanBalance
                };

            var model = new FinesRebateCalculatorViewModel
            {
                LoanDetails = _loanDetails,
                LoanBalance = 0m,
                ProcessDate = viewModel.DocumentDate
            };
            model.Calculate();
            viewModel.InterestRebate = model.Rebate;
            viewModel.InterestRebateAccountCode = Properties.Settings.Default.InterestRebateAccountCode;

            var loanDetails = InitializeLoanDetails();
            loanDetails.SetDocument(new VoucherDocument(VoucherTypes.JV, viewModel.DocumentNumber,
                                            viewModel.DocumentDate));
            loanDetails.LoanAmount = viewModel.LoanBalance - System.Math.Abs(viewModel.InterestRebate);

            viewModel.LoanDetails = loanDetails;

            return viewModel;
        }

        private LoanDetails InitializeLoanDetails()
        {
            var userDate = MainController.LoggedUser.TransactionDate;
            var loanDetails = new LoanDetails();
            loanDetails.SetMember(Nfmb.FindByCode(_loanDetails.MemberCode));
            loanDetails.SetAccount(Account.FindByCode(_loanDetails.AccountCode));

            loanDetails.LoanAmount = LoanBalance;
            loanDetails.LoanTerms = _loanDetails.LoanTerms;
            loanDetails.InterestRate = _loanDetails.InterestRate;

            loanDetails.ReleaseNo = ModelController.Releases.MaxReleaseNumber();
            loanDetails.DateReleased = userDate;
            loanDetails.GrantedDate = userDate;
            loanDetails.MaturityDate = loanDetails.GrantedDate.AddMonths(_loanDetails.LoanTerms);
            loanDetails.TermsMode = "Months";
            loanDetails.ModeOfPayment = ModeOfPayments.Monthly;

            return loanDetails;
        }
    }
}