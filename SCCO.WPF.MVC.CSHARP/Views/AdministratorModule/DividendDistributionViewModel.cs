using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    internal class DividendDistributionViewModel : ViewModelBase
    {
        private decimal _amountAllocated;
        private int _journalVoucherNumber;
        private string _interestOnShareCapitalPayableCode;
        private string _shareCapitalCode;
        private string _cooperativeCode;

        public decimal AmountAllocated
        {
            get { return _amountAllocated; }
            set
            {
                _amountAllocated = value;
                OnPropertyChanged("AmountAllocated");
            }
        }

        public int JournalVoucherNumber
        {
            get { return _journalVoucherNumber; }
            set
            {
                _journalVoucherNumber = value;
                OnPropertyChanged("JournalVoucherNumber");
            }
        }

        public string InterestOnShareCapitalPayableCode
        {
            get { return _interestOnShareCapitalPayableCode; }
            set { _interestOnShareCapitalPayableCode = value; OnPropertyChanged("InterestOnShareCapitalPayableCode"); }
        }

        public string ShareCapitalCode
        {
            get { return _shareCapitalCode; }
            set { _shareCapitalCode = value; OnPropertyChanged("ShareCapitalAccountCode"); }
        }

        public string CooperativeCode
        {
            get { return _cooperativeCode; }
            set { _cooperativeCode = value; OnPropertyChanged("CooperativeCode"); }
        }

        public decimal MaintainingBalance
        {
            get { return _maintainingBalance; }
            set { _maintainingBalance = value; OnPropertyChanged("MaintainingBalance"); }
        }

        private Account _shareCapitalAccount;

        private Account _interestOnShareCapitalAccount;

        private Nfmb _cooperative;
        private decimal _maintainingBalance;

        public Result Validate()
        {
            if (AmountAllocated <= 0m)
                return new Result(false, "Amount allocated is not valid.");

            if(MaintainingBalance <= 0m)
                return new Result(false, "Maintaining Balance is not valid.");

            if (JournalVoucherNumber <= 0)
                return new Result(false, "Journal Voucher Number is not valid.");

            if (string.IsNullOrEmpty(ShareCapitalCode))
                return new Result(false, "Share Capital Code is required.");

            if (string.IsNullOrEmpty(InterestOnShareCapitalPayableCode))
                return new Result(false, "Interest on Share Capital Payable Code is required.");

            if (string.IsNullOrEmpty(CooperativeCode))
                return new Result(false, "Cooperative Code is required.");

            // with database related validation
            var collection = JournalVoucher.FindByDocumentNumber(JournalVoucherNumber);
            if (collection.Any()) return new Result(false, "Journal Voucher Number already in use.");

            _shareCapitalAccount = Account.FindByCode(ShareCapitalCode);
            if (string.IsNullOrEmpty(_shareCapitalAccount.AccountTitle))
                return new Result(false, "Share Capital Code is not valid or does not exist.");

            _interestOnShareCapitalAccount = Account.FindByCode(InterestOnShareCapitalPayableCode);
            if (string.IsNullOrEmpty(_interestOnShareCapitalAccount.AccountTitle))
                return new Result(false, "Interest on Share Capital Code is not valid or does not exist.");

            _cooperative = Nfmb.FindByCode(CooperativeCode);
            if (string.IsNullOrEmpty(_cooperative.MemberName))
                return new Result(false, "Cooperatice Code is not valid or does not exist.");

            // after all the above validation succeeds then...
            return new Result(true, "Valid.");
        }

        public void Process()
        {
            var transactionDate = MainController.LoggedUser.TransactionDate;
            var previousYear = transactionDate.Year - 1;

            var montlyEndBalances = GetMontlyEndBalance(previousYear, ShareCapitalCode);

            if (!montlyEndBalances.Any())
            {
                MessageWindow.ShowAlertMessage(string.Format("No transactions were found having account {0}.",
                                                             _shareCapitalAccount.AccountTitle));
                return;
            }

            // total monthly average
            var filteredMonthlyEndBalances = (from meb in montlyEndBalances
                                              where meb.Average >= _maintainingBalance
                                              select meb).ToList();

            var totalMonthlyAverage = filteredMonthlyEndBalances.Sum(item => item.Average);

            // rate
            var rate = AmountAllocated / totalMonthlyAverage;

            foreach (var item in filteredMonthlyEndBalances)
            {
                if (item.Average <= 0) continue;          
                var jvCredit = new JournalVoucher
                {
                    MemberCode = item.MemberCode,
                    MemberName = item.MemberName,
                    AccountCode = _shareCapitalAccount.AccountCode,
                    AccountTitle = _shareCapitalAccount.AccountTitle,
                    Credit = item.Average * rate,
                    VoucherDate = transactionDate,
                    VoucherNo = JournalVoucherNumber,
                    IsPosted = true
                };
                jvCredit.Create();
            }

            var jvDebit = new JournalVoucher
            {
                MemberCode = _cooperative.MemberCode,
                MemberName = _cooperative.MemberName,
                AccountCode = _interestOnShareCapitalAccount.AccountCode,
                AccountTitle = _interestOnShareCapitalAccount.AccountTitle,
                Debit = _amountAllocated,
                VoucherDate = transactionDate,
                VoucherNo = JournalVoucherNumber,
                IsPosted = true,
                Explanation = "Posting Dividend Distribution"
            };

            jvDebit.Create();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.InterestOnShareCapitalPayableAccountCode = _interestOnShareCapitalPayableCode;
            Properties.Settings.Default.InterestOnShareCapitalAmountAllocated = _amountAllocated;
            Properties.Settings.Default.ShareCapitalAccountCode = _shareCapitalCode;
            Properties.Settings.Default.ShareCapitalMaintainingBalance = _maintainingBalance;
            Properties.Settings.Default.CooperativeMemberCode = _cooperativeCode;
            Properties.Settings.Default.Save();
        }

        private void _restoreSettings()
        {
            InterestOnShareCapitalPayableCode = Properties.Settings.Default.InterestOnShareCapitalPayableAccountCode;
            AmountAllocated = Properties.Settings.Default.InterestOnShareCapitalAmountAllocated;
            ShareCapitalCode = Properties.Settings.Default.ShareCapitalAccountCode;
            MaintainingBalance = Properties.Settings.Default.ShareCapitalMaintainingBalance;
            CooperativeCode = Properties.Settings.Default.CooperativeMemberCode;
        }

        internal void Initialize()
        {
            _restoreSettings();
            JournalVoucherNumber = Voucher.LastDocumentNo(VoucherTypes.JV) + 1;
        }
    }
}
