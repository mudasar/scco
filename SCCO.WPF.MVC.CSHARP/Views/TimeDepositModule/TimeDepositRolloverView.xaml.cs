using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.AccountVerifier;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

// Allows user to select cash voucher,
// this window validates voucher number
namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    public partial class TimeDepositRolloverView
    {
        private readonly Voucher _voucherDocument;
        private readonly AccountDetail _accountDetail;

        public TimeDepositRolloverView(AccountDetail accountDetail)
        {
            InitializeComponent();
            
            _accountDetail = accountDetail;
            _voucherDocument = new Voucher
                {
                    VoucherDate = GlobalSettings.DateOfOpenTransaction,
                    VoucherType = VoucherTypes.JV,
                    VoucherNo = Voucher.LastDocumentNo(VoucherTypes.JV) + 1
                };
            JournalVoucherNoTextBox.Text = string.Format("{0}", _voucherDocument.VoucherNo);
            DataContext = _voucherDocument;
            PostButton.Click += (sender, args) => PostTimeDepositRollover();
        }

        private void PostTimeDepositRollover()
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
            DialogResult = true;
        }

        private Result PerformPosting()
        {
            // post interest earned debit side
            var result = PostInterestExpense();
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
            result = PostTimeDepositInterestEarned();
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
            JournalVoucher.DeleteAll(_voucherDocument.VoucherNo);
        }

        private Result PostTimeDepositInterestEarned()
        {
            // post time desposit interest earned credit side
            var member = Nfmb.FindByCode(_accountDetail.MemberCode);
            var account = Account.FindByCode(_accountDetail.AccountCode);

            var previousDetail = _accountDetail.TimeDepositDetails;
            var asOf = GlobalSettings.DateOfOpenTransaction;
            var interestEarned = previousDetail.CalculateInterestEarned(asOf) - previousDetail.CalculateServiceFee(asOf);

            var tdDetails = SetNewTimeDepositDetails();
            if (tdDetails == null)
            {
                return new Result(false, "Time deposit details not set.");
            }
            var jv = new JournalVoucher
                {
                    MemberCode = member.MemberCode,
                    MemberName = member.MemberName,
                    AccountCode = account.AccountCode,
                    AccountTitle = account.AccountTitle,
                    Credit = interestEarned,
                    VoucherDate = _voucherDocument.VoucherDate,
                    VoucherNo = _voucherDocument.VoucherNo,
                    TimeDepositDetails = tdDetails,
                };

            var postResult = jv.Create();
            if (!postResult.Success)
            {
                Rollback();
            }
            return postResult;
        }

        private TimeDepositDetails SetNewTimeDepositDetails()
        {
            var previousDetail = _accountDetail.TimeDepositDetails;
            var timeDepositDetail = new TimeDepositDetails();
            timeDepositDetail.CertificateNo = previousDetail.CertificateNo;
            timeDepositDetail.Rate = previousDetail.Rate;
            timeDepositDetail.DateIn = GlobalSettings.DateOfOpenTransaction;
            var totalMonths = ((previousDetail.Maturity.Year - previousDetail.DateIn.Year)*12) +
                              previousDetail.Maturity.Month - previousDetail.DateIn.Month;

            timeDepositDetail.Term = totalMonths;
            timeDepositDetail.TermsMode = "Month";

            var timeDepositDetailsWindow = new TimeDepositEditView(timeDepositDetail);
            if (timeDepositDetailsWindow.ShowDialog() == true)
            {
                return timeDepositDetail;
            }
            return timeDepositDetail;
        }

        private Result PostInterestExpense()
        {
            // post interest expense debit side
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
            var jv = new JournalVoucher
            {
                MemberCode = member.MemberCode,
                MemberName = member.MemberName,
                AccountCode = account.AccountCode,
                AccountTitle = account.AccountTitle,
                Debit = interestEarned,
                VoucherDate = _voucherDocument.VoucherDate,
                VoucherNo = _voucherDocument.VoucherNo,
            };

            var postResult = jv.Create();
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
            var jv = new JournalVoucher
            {
                MemberCode = member.MemberCode,
                MemberName = member.MemberName,
                AccountCode = account.AccountCode,
                AccountTitle = account.AccountTitle,
                Credit = _accountDetail.TimeDepositDetails.CalculateServiceFee(_voucherDocument.VoucherDate),
                VoucherDate = _voucherDocument.VoucherDate,
                VoucherNo = _voucherDocument.VoucherNo,
            };

            var postResult = jv.Create();
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
            var cv = new JournalVoucher
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
            ReportController.JournalVoucherReports.VoucherForm(_voucherDocument.VoucherNo);
        }

    }
}