using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

// Allows user to select cash voucher,
// this window validates voucher number
namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositWithdrawalView
    {
        private readonly Voucher _voucherDocument;
        private readonly AccountDetail _accountDetail;

        public TimeDepositWithdrawalView(AccountDetail accountDetail)
        {
            InitializeComponent();
            
            _accountDetail = accountDetail;
            _voucherDocument = new Voucher
                {
                    VoucherDate = GlobalSettings.DateOfOpenTransaction,
                    VoucherType = VoucherTypes.CV,
                    VoucherNo = Voucher.LastDocumentNo(VoucherTypes.CV) + 1
                };
            CashVoucherNoTextBox.Text = string.Format("{0}", _voucherDocument.VoucherNo);
            DataContext = _voucherDocument;
            PostButton.Click += (sender, args) => PostTimeDepositWithdrawal();
        }

        private void PostTimeDepositWithdrawal()
        {

            if (!IsInputValid())
            {
                return;
            }

            var result = PerformPosting();

            if (!result.Success)
            {
                Rollback();
                MessageWindow.ShowAlertMessage(result.Message);
                return;
            }
            PrintVoucher();
            DialogResult = true;
        }

        private Result PerformPosting()
        {
            // post time desposit end balance debit side
            var result = PostTimeDepositEndBalance();
            if (!result.Success)
            {
                return result;
            }

            // post interest expense debit side
            result = PostInterestExpense();
            if (!result.Success)
            {
                return result;
            }

            // post service fee credit side
            result = PostServiceFee();
            if (!result.Success)
            {
                return result;
            }

            // post cash on hand credit side
            result = PostCashOnHand();
            if (!result.Success)
            {
                return result;
            }

            #region --- Voucher Log ---

            var voucherLog = new VoucherLog();
            voucherLog.Find(_voucherDocument.VoucherType.ToString(), _voucherDocument.VoucherNo);
            voucherLog.Date = _voucherDocument.VoucherDate;
            voucherLog.Initials = MainController.LoggedUser.Initials;

            voucherLog.Save();

            #endregion

            return result;
        }

        private void Rollback()
        {
            CashVoucher.DeleteAll(_voucherDocument.VoucherNo);
        }

        private Result PostTimeDepositEndBalance()
        {
            // post time desposit end balance debit side
            var member = Nfmb.FindByCode(_accountDetail.MemberCode);
            var account = Account.FindByCode(_accountDetail.AccountCode);
            var tdDetails = new TimeDepositDetails {CertificateNo = _accountDetail.TimeDepositDetails.CertificateNo};
            var cv = new CashVoucher
                {
                    MemberCode = member.MemberCode,
                    MemberName = member.MemberName,
                    AccountCode = account.AccountCode,
                    AccountTitle = account.AccountTitle,
                    Debit = _accountDetail.EndingBalance,
                    VoucherDate = _voucherDocument.VoucherDate,
                    VoucherNo = _voucherDocument.VoucherNo,
                    TimeDepositDetails = tdDetails,
                };

            var postResult = cv.Create();
            if (!postResult.Success)
            {
                Rollback();
            }
            return postResult;
        }

        private Result PostInterestExpense()
        {
            // post time desposit end balance debit side
            var interestEarned = _accountDetail.TimeDepositDetails.CalculateInterestEarned(_voucherDocument.VoucherDate);
            if (interestEarned == 0)
            {
                return new Result(true, "No interest earned.");
            }

            var member = Nfmb.FindByCode(_accountDetail.MemberCode);
            var accountCode = GlobalSettings.CodeOfInterestExpenseOnTimeDeposit;
            if (string.IsNullOrWhiteSpace(accountCode))
            {
                return new Result(false, GenerateCodeOfAccountNotSetMessage("Interest Expense On Time Deposit"));
            }
            var account = Account.FindByCode(accountCode);
            var cv = new CashVoucher
            {
                MemberCode = member.MemberCode,
                MemberName = member.MemberName,
                AccountCode = account.AccountCode,
                AccountTitle = account.AccountTitle,
                Debit = interestEarned,
                VoucherDate = _voucherDocument.VoucherDate,
                VoucherNo = _voucherDocument.VoucherNo,
            };

            var postResult = cv.Create();
            if (!postResult.Success)
            {
                Rollback();
            }
            return postResult;
        }

        private Result PostServiceFee()
        {
            // post service fee credit side
            var member = Nfmb.FindByCode(_accountDetail.MemberCode);
            var accountCode = GlobalSettings.CodeOfServiceFee;
            if (string.IsNullOrWhiteSpace(accountCode))
            {
                return new Result(false, GenerateCodeOfAccountNotSetMessage("Service Fee"));
            }
            var account = Account.FindByCode(accountCode);
            var cv = new CashVoucher
            {
                MemberCode = member.MemberCode,
                MemberName = member.MemberName,
                AccountCode = account.AccountCode,
                AccountTitle = account.AccountTitle,
                Credit = _accountDetail.TimeDepositDetails.CalculateServiceFee(_voucherDocument.VoucherDate),
                VoucherDate = _voucherDocument.VoucherDate,
                VoucherNo = _voucherDocument.VoucherNo,
            };

            var postResult = cv.Create();
            if (!postResult.Success)
            {
                Rollback();
            }
            return postResult;
        }

        private Result PostCashOnHand()
        {
            // post cash on hand credit side
            var member = Nfmb.FindByCode(_accountDetail.MemberCode);
            var accountCode = GlobalSettings.CodeOfCashOnHand;
            if (string.IsNullOrWhiteSpace(accountCode))
            {
                return new Result(false, GenerateCodeOfAccountNotSetMessage("Cash on Hand"));
            }
            var account = Account.FindByCode(accountCode);
            var amount = _accountDetail.EndingBalance +
                         _accountDetail.TimeDepositDetails.CalculateInterestEarned(_voucherDocument.VoucherDate) -
                         _accountDetail.TimeDepositDetails.CalculateServiceFee(_voucherDocument.VoucherDate);
            var cv = new CashVoucher
            {
                MemberCode = member.MemberCode,
                MemberName = member.MemberName,
                AccountCode = account.AccountCode,
                AccountTitle = account.AccountTitle,
                Credit = amount,
                VoucherDate = _voucherDocument.VoucherDate,
                VoucherNo = _voucherDocument.VoucherNo,
                Explanation = "Withdrawal of Time Deposit",
                AmountInWords = Converter.AmountToWords(amount),
            };

            var postResult = cv.Create();
            if (!postResult.Success)
            {
                Rollback();
            }
            return postResult;
        }

        private string GenerateCodeOfAccountNotSetMessage(string account)
        {
            return string.Format("{0} Code is not set. Please check Global Variables in Admin Menu.", account);
        }

        private bool IsInputValid()
        {
            if (!TransactionHelper.IsPostingAllowed())
            {
                MessageWindow.ShowAlertMessage("Posting is not allowed. Please check your transaction date.");
                return false;
            }
            if (TransactionHelper.IsVoucherNumberUsed(VoucherTypes.CV, _voucherDocument.VoucherNo))
            {
                MessageWindow.ShowAlertMessage("Voucher Number is already in use.");
                return false;
            }
            return true;
        }

        private void PrintVoucher()
        {
            ReportController.CashVoucherReports.VoucherForm(_voucherDocument.VoucherNo);
        }

    }
}