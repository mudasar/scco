using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using System.Linq;

namespace SCCO.WPF.MVC.CS.Controllers
{
    class LoanAmortizationController
    {
        public static LoanAmortizationHeader GenerateLoanAmortization(
            string codeMember, 
            string codeAccount, 
            decimal amountLoan, 
            int termLoan,
            decimal rateAnnualInterest, 
            System.DateTime grantedDate,
            decimal monthlyCapitalBuildUp)
        {
            var schedule = new LoanAmortizationHeader();

            // Member Information
            var member = Nfmb.FindByCode(codeMember);
            schedule.MemberCode = member.MemberCode;
            schedule.MemberName = member.MemberName;
            schedule.MemberAddress = member.CompleteAddress();

            // Account Information
            var account = Account.FindByCode(codeAccount);
            schedule.AccountCode = account.AccountCode;
            schedule.AccountTitle = account.AccountTitle;

            schedule.LoanAmount = amountLoan;
            schedule.MonthlyCapitalBuildUp = monthlyCapitalBuildUp;
            schedule.AnnualInterestRate = rateAnnualInterest;
            schedule.LoanTerm = termLoan;
            schedule.ModeOfPayment = "Monthly";
            schedule.DateGranted = grantedDate;
            schedule.DateMaturity = grantedDate.AddMonths(termLoan);
            schedule.FirstPaymentDate = grantedDate.AddMonths(1);

            var monthlyInterest = System.Math.Round((amountLoan*schedule.AnnualInterestRate)/12, 2);
            var monthlyPayment = System.Math.Round((amountLoan/termLoan), 2);

            schedule.MonthlyAmortization = monthlyPayment + monthlyInterest + monthlyCapitalBuildUp;
            var runningBalance = amountLoan;
            for (int i = 1; i < termLoan; i++)
            {
                var item = new LoanAmortizationDetail();
                item.PaymentDate = grantedDate.AddMonths(i);
                item.PaymentNo = i;
                item.BeginningBalance = runningBalance;
                item.Payment = monthlyPayment;
                item.Interest = monthlyInterest;
                item.CapitalBuildUp = schedule.MonthlyCapitalBuildUp;
                item.Amortization = item.Payment + monthlyInterest + item.CapitalBuildUp;
                item.EndingBalance = item.BeginningBalance - item.Payment;
                schedule.PaymentSchedules.Add(item);

                runningBalance = item.EndingBalance;
            }

            // Add final payment that, balancing discrepancies in monthly payment and amortization
            var lastpayment = new LoanAmortizationDetail();
            lastpayment.PaymentDate = grantedDate.AddMonths(termLoan);
            lastpayment.PaymentNo = termLoan;
            lastpayment.BeginningBalance = runningBalance;
            lastpayment.Payment = amountLoan - schedule.PaymentSchedules.Sum(payment => payment.Payment);
            lastpayment.Interest = monthlyInterest;
            lastpayment.CapitalBuildUp = monthlyCapitalBuildUp;
            lastpayment.Amortization = lastpayment.Payment + monthlyInterest + lastpayment.CapitalBuildUp;
            lastpayment.EndingBalance = lastpayment.BeginningBalance - lastpayment.Payment;
            schedule.PaymentSchedules.Add(lastpayment);

            return schedule;
        }
    }


}
