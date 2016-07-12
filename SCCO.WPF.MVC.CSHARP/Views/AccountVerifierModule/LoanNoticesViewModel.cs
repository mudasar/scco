using System;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Utilities;
using SCCO.WPF.MVC.CS.Views.LoanModule;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public class LoanNoticesViewModel
    {
        public LoanNoticesViewModel()
        {
        }

        public LoanNoticesViewModel(LoanDetails loanDetails, DateTime asOf) : this()
        {
            as_of = asOf;
            SetBorrower(loanDetails);
            SetAccount(loanDetails);
            SetDocument(loanDetails);
            SetLoanData(loanDetails);
            SetDaysSpan(loanDetails, asOf);
            SetEndBalance(loanDetails.MemberCode, loanDetails.AccountCode, asOf);
            SetExpectedBalance(loanDetails, asOf);
            SetFinesData(loanDetails, asOf);
            SetCoMakers(loanDetails);
        }

        public string member_code { get; set; }
        public string member_name { get; set; }
        public string account_code { get; set; }
        public string account_title { get; set; }
        public decimal loan_amount { get; set; }
        public decimal ending_balance { get; set; }
        public decimal expected_balance { get; set; }
        public int days_span { get; set; }

        public DateTime document_date { get; set; }
        public string document_type { get; set; }
        public int document_number { get; set; }

        public int release_number { get; set; }
        public int terms { get; set; }
        public string mode_of_terms { get; set; }
        public string mode_of_payment { get; set; }

        public DateTime date_granted { get; set; }
        public DateTime date_maturity { get; set; }
        public DateTime as_of { get; set; }
        public DateTime date_cut_off { get; set; }

        public decimal payment { get; set; }
        public decimal interest_rate { get; set; }


        public decimal interest_amount { get; set; }
        public decimal interest_amortization { get; set; }
        public DateTime date_approved { get; set; }
        public DateTime date_cancelled { get; set; }
        public DateTime date_released { get; set; }
        public DateTime date_applied { get; set; }

        public string comaker_code1 { get; set; }
        public string comaker_name1 { get; set; }
        public string comaker_code2 { get; set; }
        public string comaker_name2 { get; set; }
        public string comaker_code3 { get; set; }
        public string comaker_name3 { get; set; }
        public string comaker_code4 { get; set; }
        public string comaker_name4 { get; set; }
        public string comaker_code5 { get; set; }
        public string comaker_name5 { get; set; }

        public decimal this_month { get; set; }

        public string collector { get; set; }
        public string area { get; set; }

        public bool notice1 { get; set; }
        public bool notice2 { get; set; }
        public bool notice3 { get; set; }

        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }

        public int due_days { get; set; }
        public decimal due_fines { get; set; }
        public decimal due_interest { get; set; }

        public string comaker_code { get; set; }
        public string comaker_name { get; set; }

        private void SetBorrower(LoanDetails loanDetails)
        {
            var member = Nfmb.FindByCode(loanDetails.MemberCode);
            member_code = member.MemberCode;
            member_name = member.MemberName;
            address1 = member.Address1;
            address2 = member.Address2;
            address3 = member.Address3;
            collector = member.CollectorName;
            area = member.AreaCode;
        }

        private void SetAccount(LoanDetails loanDetails)
        {
            var account = Account.FindByCode(loanDetails.AccountCode);
            account_code = account.AccountCode;
            account_title = account.AccountTitle;
        }

        private void SetDocument(LoanDetails loanDetails)
        {
            document_date = loanDetails.DocumentDate;
            document_type = loanDetails.DocumentType;
            document_number = loanDetails.DocumentNo;
        }

        private void SetLoanData(LoanDetails loanDetails)
        {
            loan_amount = loanDetails.LoanAmount;
            terms = loanDetails.LoanTerms;
            mode_of_terms = loanDetails.TermsMode;
            mode_of_payment = loanDetails.ModeOfPayment.ToString();

            date_granted = loanDetails.GrantedDate;
            date_maturity = loanDetails.MaturityDate;
            date_cut_off = loanDetails.CutOffDate;

            date_applied = loanDetails.DateApplied;
            date_approved = loanDetails.DateApproved;
            date_cancelled = loanDetails.DateCancelled;

            date_released = loanDetails.DateReleased;
            release_number = loanDetails.ReleaseNo;

            interest_rate = loanDetails.InterestRate;
            interest_amount = loanDetails.InterestAmount;
            interest_amortization = loanDetails.InterestAmortization;

            payment = loanDetails.Payment;
        }

        private void SetEndBalance(string memberCode, string accountCode, DateTime asOf)
        {
            ending_balance = FinancialReportExcelCreator.GetMemberAccountEndBalance(memberCode, accountCode, asOf);
        }

        private void SetDaysSpan(LoanDetails loanDetails, DateTime asOf)
        {
            days_span = asOf.Subtract(loanDetails.GrantedDate).Days;
        }

        private void SetExpectedBalance(LoanDetails loanDetails, DateTime asOf)
        {
            if (IsOverdue(loanDetails, asOf))
            {
                expected_balance = 0m;
            }
            else
            {
                int days;
                int daysElapsed;
                decimal dailyPayment;

                switch (loanDetails.ModeOfPayment)
                {
                    case ModeOfPayments.Daily:
                        days = loanDetails.MaturityDate.Subtract(loanDetails.GrantedDate).Days;
                        dailyPayment = loanDetails.LoanAmount/days;
                        daysElapsed = asOf.Subtract(loanDetails.GrantedDate).Days;
                        expected_balance = Math.Round(daysElapsed*dailyPayment, 2);
                        break;

                    case ModeOfPayments.Weekly:
                        days = loanDetails.MaturityDate.Subtract(loanDetails.GrantedDate).Days;
                        dailyPayment = loanDetails.LoanAmount/days;
                        daysElapsed = asOf.Subtract(loanDetails.GrantedDate).Days;
                        var weeksElapsed = daysElapsed%7;
                        expected_balance = Math.Round(weeksElapsed*(dailyPayment*7m), 2);
                        break;

                    case ModeOfPayments.SemiMonthly:
                    case ModeOfPayments.Monthly:
                        var grantedDate = loanDetails.GrantedDate;
                        var maturityDate = loanDetails.MaturityDate;

                        var monthsElapsed = (asOf.Year - grantedDate.Year)*12 + (asOf.Month - grantedDate.Month);
                        var monthsTerm = (maturityDate.Year - grantedDate.Year)*12 +
                                         (maturityDate.Month - grantedDate.Month);
                        var totalLoanAmount = loanDetails.LoanAmount + loanDetails.InterestAmount;
                        var monthlyAmortization = totalLoanAmount/monthsTerm;

                        var expectedBalance = totalLoanAmount - (Math.Round(monthlyAmortization*(monthsElapsed - 1), 2));
                        expected_balance = expectedBalance > 0 ? expectedBalance : 0m;
                        break;
                }
            }
        }

        private void SetFinesData(LoanDetails loanDetails, DateTime asOf)
        {
            if (!IsOverdue(loanDetails, asOf))
            {
                return;
            }

            due_days = as_of.Subtract(loanDetails.MaturityDate).Days;
            if (loanDetails.AccountCode == GlobalSettings.CodeOfCoopPurchaseOrder)
            {
                // pag due may fines/penalty
                const decimal finesRate = 3.5m/100m;
                var penalty = ending_balance*finesRate;
                due_fines = Math.Round(penalty, 2);

                // laging may miscellaneous income
                var interestRate = loanDetails.InterestRate/12m;
                due_interest = Math.Round(ending_balance*interestRate, 2);
            }
            else if (loanDetails.AccountCode == GlobalSettings.CodeOfGoNegosyo)
            {
                // may fines kahit indi pa due date basta nagpareconstruct
                const decimal finesRate = 3.5m/100m;
                var penalty = ending_balance*finesRate;
                due_fines = Math.Round(penalty, 2);

                // laging may miscellaneous income
                const decimal interestRate = 1.5m/100m;
                due_interest = Math.Round(ending_balance*interestRate, 2);
            }
            else
            {
                var frc = new FinesRebateCalculatorViewModel
                {
                    LoanDetails = loanDetails,
                    LoanBalance = ending_balance,
                    ProcessDate = asOf
                };
                frc.Calculate();

                due_fines = Math.Round(frc.Fines, 2);
                due_interest = Math.Round(frc.Interest, 2);
            }
        }

        private bool IsOverdue(LoanDetails loanDetails, DateTime asOf)
        {
            return asOf > loanDetails.MaturityDate;
        }

        private void SetCoMakers(LoanDetails loanDetails)
        {
            for (var i = 0; i < loanDetails.CoMakers.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        comaker_code1 = loanDetails.CoMakers[i].MemberCode;
                        comaker_name1 = loanDetails.CoMakers[i].MemberName;
                        break;
                    case 1:
                        comaker_code2 = loanDetails.CoMakers[i].MemberCode;
                        comaker_name2 = loanDetails.CoMakers[i].MemberName;
                        break;
                    case 2:
                        comaker_code3 = loanDetails.CoMakers[i].MemberCode;
                        comaker_name3 = loanDetails.CoMakers[i].MemberName;
                        break;
                    case 3:
                        comaker_code4 = loanDetails.CoMakers[i].MemberCode;
                        comaker_name4 = loanDetails.CoMakers[i].MemberName;
                        break;
                    case 4:
                        comaker_code5 = loanDetails.CoMakers[i].MemberCode;
                        comaker_name5 = loanDetails.CoMakers[i].MemberName;
                        break;
                }
            }
        }
    }
}