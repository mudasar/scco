using System;
using System.Collections.Generic;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    internal class PatronageRefundViewModel : ViewModelBase
    {
        private int _journalVoucherNumber;
        private string _patronageRefundCode;
        private string _cooperativeCode;
        private decimal _amountAllocated;
        private string _interestRebateCode;
        private string _interestOnLoanCode;
        private string _shareCapitalCode;

        public string InterestOnLoanCode
        {
            get { return _interestOnLoanCode; }
            set { _interestOnLoanCode = value; OnPropertyChanged("InterestOnLoanCode"); }
        }

        public string InterestRebateCode
        {
            get { return _interestRebateCode; }
            set { _interestRebateCode = value; OnPropertyChanged("InterestRebateCode"); }
        }

        public decimal AmountAllocated
        {
            get { return _amountAllocated; }
            set { _amountAllocated = value; OnPropertyChanged("AmountAllocated"); }
        }

        public int JournalVoucherNumber
        {
            get { return _journalVoucherNumber; }
            set { _journalVoucherNumber = value; OnPropertyChanged("JournalVoucherNumber"); }
        }

        public string PatronageRefundCode
        {
            get { return _patronageRefundCode; }
            set { _patronageRefundCode = value; OnPropertyChanged("PatronageRefundCode"); }
        }

        public string CooperativeCode
        {
            get { return _cooperativeCode; }
            set { _cooperativeCode = value; OnPropertyChanged("CooperativeCode"); }
        }

        public string ShareCapitalCode
        {
            get { return _shareCapitalCode; }
            set { _shareCapitalCode = value; OnPropertyChanged("ShareCapitalCode"); }
        }

        private Account _patronageRefundAccount;
        private Account _shareCapitalAccount;
        private Nfmb _cooperative;


        public Result Validate()
        {
            if (string.IsNullOrEmpty(InterestOnLoanCode))
                return new Result(false, "Interest on Loan Code is required.");
           
            if (string.IsNullOrEmpty(InterestRebateCode))
                return new Result(false, "Interest Rebate Code is required.");
            
            if(AmountAllocated <= 0m)
                return new Result(false, "Amount allocated is not valid.");
            
            if(JournalVoucherNumber <= 0)
                return new Result(false, "Journal Voucher Number is not valid.");
            
            if(string.IsNullOrEmpty(PatronageRefundCode))
                return new Result(false, "Patronage Refund Code is required.");
            
            if (string.IsNullOrEmpty(ShareCapitalCode))
                return new Result(false, "Share Capital Code is required.");

            if(string.IsNullOrEmpty(CooperativeCode))
                return new Result(false, "Cooperative Code is required.");

            // with database related validation
            var interestOnLoanAccount = Account.FindByCode(InterestOnLoanCode);
            if(string.IsNullOrEmpty(interestOnLoanAccount.AccountTitle))
                return new Result(false, "Interest On Loan Code is not valid or does not exist.");

            var interestRebateAccount = Account.FindByCode(InterestRebateCode);
            if(string.IsNullOrEmpty(interestRebateAccount.AccountTitle))
                return new Result(false, "Interest Rebate Code is not valid or does not exist.");

            var collection = JournalVoucher.WhereDocumentNumberIs(JournalVoucherNumber);
            if(collection.Any()) return new Result(false, "Journal Voucher Number already in use.");

            _patronageRefundAccount = Account.FindByCode(PatronageRefundCode);
            if (string.IsNullOrEmpty(_patronageRefundAccount.AccountTitle))
                return new Result(false, "Patronage Refund Code is not valid or does not exist.");

            _shareCapitalAccount = Account.FindByCode(ShareCapitalCode);
            if (string.IsNullOrEmpty(_shareCapitalAccount.AccountTitle))
                return new Result(false, "Share Capital Code is not valid or does not exist.");

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

            var interestOnLoans = GetMontlyEndBalance(previousYear, InterestOnLoanCode);
            var interestRebates = GetMontlyEndBalance(previousYear, InterestRebateCode);

            // loop each interest on loans, subtract any interest rebates per member
            var endBalances = new List<Tuple<string, decimal>>();
            foreach (var item in interestOnLoans)
            {
                var realBalance = item.December - item.Beginning;
                if (realBalance > 0)
                {
                    var rebate = interestRebates.SingleOrDefault(t => t.MemberCode.Trim() == item.MemberCode.Trim());
                    var totalRebate = 0m;

                    if (rebate != null)
                    {
                        totalRebate = rebate.December - rebate.Beginning;
                    }

                    var endBalance = realBalance - totalRebate;
                    if (endBalance > 0)
                    {
                        endBalances.Add(new Tuple<string, decimal>(item.MemberCode, endBalance));
                    }
                }
            }

            var totalInterestFromLoans = endBalances.Sum(t => t.Item2);
            var rate = _amountAllocated/totalInterestFromLoans;

            foreach (var eb in endBalances)
            {
                var item = interestOnLoans.SingleOrDefault(t => t.MemberCode.Trim() == eb.Item1.Trim());
                if (item == null)
                {
                    Console.WriteLine(@"This must not happen!!!");
                    continue;
                }
                var jvCredit = new JournalVoucher
                {
                    MemberCode = item.MemberCode,
                    MemberName = item.MemberName,
                    AccountCode = _shareCapitalAccount.AccountCode,
                    AccountTitle = _shareCapitalAccount.AccountTitle,
                    Credit = eb.Item2 * rate,
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
                AccountCode = _patronageRefundAccount.AccountCode,
                AccountTitle = _patronageRefundAccount.AccountTitle,
                Debit = _amountAllocated,
                VoucherDate = transactionDate,
                VoucherNo = JournalVoucherNumber,
                IsPosted = true,
                Explanation = "Posting Patronage Refund"
            };

            jvDebit.Create();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.InterestOnLoanAccountCode = _interestOnLoanCode;
            Properties.Settings.Default.InterestRebateAccountCode = _interestRebateCode;
            Properties.Settings.Default.PatronageRefundAmountAllocated = _amountAllocated;
            Properties.Settings.Default.PatronageRefundAccountCode = _patronageRefundCode;
            Properties.Settings.Default.ShareCapitalAccountCode = _shareCapitalCode;
            Properties.Settings.Default.CooperativeMemberCode = _cooperativeCode;
            Properties.Settings.Default.Save();
        }

        public void RestoreSettings()
        {
            InterestOnLoanCode = Properties.Settings.Default.InterestOnLoanAccountCode;
            InterestRebateCode = Properties.Settings.Default.InterestRebateAccountCode;
            AmountAllocated = Properties.Settings.Default.PatronageRefundAmountAllocated;
            ShareCapitalCode = Properties.Settings.Default.ShareCapitalAccountCode;
            PatronageRefundCode = Properties.Settings.Default.PatronageRefundAccountCode;
            CooperativeCode = Properties.Settings.Default.CooperativeMemberCode;
        }

        internal void Initialize()
        {
            RestoreSettings();
            JournalVoucherNumber = Voucher.LastDocumentNo(VoucherTypes.JV) + 1;
        }
    }
}
