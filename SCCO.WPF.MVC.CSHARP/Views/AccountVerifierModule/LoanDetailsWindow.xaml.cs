using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
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
            var viewModel = new LoanReconstructionViewModel
            {
                PreviousLoanDetails = _loanDetails,
                LoanBalance = LoanBalance,
                ReconstructionDate = MainController.LoggedUser.TransactionDate,
                LoanManager = MainController.LoggedUser,
                ReconstructionType = ReconstructionTypes.PaidInterest
            };

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
            var view = new PaidInterestReconstructionView(viewModel);
            view.ShowDialog();
            if (view.ActionResult.Success)
            {
                Close();
            }
        }

        private void ShowAddOnInterestLoanReconstruction()
        {
            var viewModel = new LoanReconstructionViewModel
                {
                    PreviousLoanDetails = _loanDetails,
                    LoanBalance = LoanBalance,
                    ReconstructionDate = MainController.LoggedUser.TransactionDate,
                    LoanManager = MainController.LoggedUser,
                    ReconstructionType = ReconstructionTypes.AddOnInterest
                };

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
    }
}