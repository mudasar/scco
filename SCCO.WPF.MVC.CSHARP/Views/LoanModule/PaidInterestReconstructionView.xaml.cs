using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal partial class PaidInterestReconstructionView
    {
        private readonly LoanReconstructionViewModel _viewModel;

        public Result ActionResult { get; private set; }

        public PaidInterestReconstructionView()
        {
            InitializeComponent();
            ActionResult = new Result(false, "No action initiated.");
        }

        public PaidInterestReconstructionView(LoanReconstructionViewModel viewModel) : this()
        {
            _viewModel = viewModel;
            
            InterestRebatePanel.Visibility = _viewModel.InterestRebate == 0 ? Visibility.Collapsed : Visibility.Visible;
            LoanDetailsButton.Click += (sender, args) => ShowLoanDetailsEditView();
            PostButton.Click += (sender, args) => PostToVoucher();
            stbInterestRebateAccountCode.Click += (sender, args) => SearchAccount();

            DataContext = _viewModel;
        }

        private void SearchAccount()
        {
            var account = MainController.SearchAccount();
            stbInterestRebateAccountCode.Text = account.AccountCode;
            Properties.Settings.Default.InterestRebateAccountCode = account.AccountCode;
            Properties.Settings.Default.Save();
        }

        private void PostToVoucher()
        {
            ActionResult = ValidateEntries();
            if (!ActionResult.Success)
            {
                MessageWindow.ShowAlertMessage(ActionResult.Message);
                return;
            }

            var jvEntry = new JournalVoucher
            {
                MemberCode = _viewModel.Member.MemberCode,
                MemberName = _viewModel.Member.MemberName,
                AccountCode = _viewModel.LoanApplied.AccountCode,
                AccountTitle = _viewModel.LoanApplied.AccountTitle,
                Credit = _viewModel.LoanBalance,
                VoucherDate = _viewModel.DocumentDate,
                VoucherNo = _viewModel.DocumentNumber
            };

            ActionResult = jvEntry.Create();
            if (!ActionResult.Success)
            {
                JournalVoucher.DeleteAll(_viewModel.DocumentNumber);
                MessageWindow.ShowAlertMessage(ActionResult.Message);
                return;
            }

            #region --- Interest Rebate ---

            if (_viewModel.InterestRebate != 0m)
            {
                var rebate = Account.FindByCode(Properties.Settings.Default.InterestRebateAccountCode);
                jvEntry = new JournalVoucher
                    {

                        MemberCode = _viewModel.Member.MemberCode,
                        MemberName = _viewModel.Member.MemberName,
                        AccountCode = rebate.AccountCode,
                        AccountTitle = rebate.AccountTitle,
                        Credit = Math.Abs(_viewModel.InterestRebate) * -1,
                        VoucherDate = _viewModel.DocumentDate,
                        VoucherNo = _viewModel.DocumentNumber
                    };
                ActionResult = jvEntry.Create();
                if (!ActionResult.Success)
                {
                    JournalVoucher.DeleteAll(_viewModel.DocumentNumber);
                    MessageWindow.ShowAlertMessage(ActionResult.Message);
                    return;
                }
            } 

            #endregion

            #region --- Finally add entry for the loan applied ---

            var loanVoucherEntry = new JournalVoucher
                {
                    MemberCode = _viewModel.Member.MemberCode,
                    MemberName = _viewModel.Member.MemberName,
                    AccountCode = _viewModel.LoanApplied.AccountCode,
                    AccountTitle = _viewModel.LoanApplied.AccountTitle,
                    Debit = _viewModel.LoanBalance - Math.Abs(_viewModel.InterestRebate),
                    VoucherDate = _viewModel.DocumentDate,
                    VoucherNo = _viewModel.DocumentNumber,
                    LoanDetails = _viewModel.LoanDetails,
                    Explanation =
                        "Paid Interest Loan Reconstruction: OR#" +
                        _viewModel.OfficialReceiptNumber + " " +
                        _viewModel.LoanDetails.GenerateExplanation()
                };

            ActionResult = loanVoucherEntry.Create();
            if (!ActionResult.Success)
            {
                JournalVoucher.DeleteAll(_viewModel.DocumentNumber);
                MessageWindow.ShowAlertMessage(ActionResult.Message);
                return;
            }
            #endregion

            MessageWindow.ShowNotifyMessage("Paid Interest Loan Reconstruction successful!");
            Close();
        }

        private Result ValidateEntries()
        {
            if (string.IsNullOrEmpty(_viewModel.OfficialReceiptNumber))
            {
                return new Result(false, "Official Receipt Number must be provided.");
            }
            if (_viewModel.LoanBalance <= 0)
            {
                return new Result(false, "Invalid Loan Amount.");
            }
            // validate document number
            if (_viewModel.DocumentNumber <= 0)
            {
                return new Result(false, "Journal Voucher Number is invalid.");
            }
            if (Voucher.Exist(VoucherTypes.JV, _viewModel.DocumentNumber))
            {
                return new Result(false, "Journal Voucher Number has been used by another user.");
            }
            // validate document date
            if (_viewModel.DocumentDate != GlobalSettings.DateOfOpenTransaction)
            {
                return new Result(false, "Journal Voucher Date is invalid.");
            }

            if (_viewModel.LoanDetails.Payment == 0m)
            {
                return new Result(false, "Loan details not yet complete. Please check.");
            }

            if (_viewModel.InterestRebate != 0)
            {
                if (string.IsNullOrEmpty(_viewModel.InterestRebateAccountCode))
                {
                    return new Result(false, "Interest Rebate Account Code is not set.");
                }
                if (!Account.IsExist(_viewModel.InterestRebateAccountCode))
                {
                    return new Result(false, "Interest Rebate Account Code does not exist.");
                }
            }
            return new Result(true, "Validation successful.");
        }

        private void ShowLoanDetailsEditView()
        {
            var view = new LoanDetailsEditView(_viewModel.LoanDetails);
            view.ShowDialog();
        }
    }
}