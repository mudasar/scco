using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    /*
    * What is Amortization?
    * Amortization is process of paying off a debt (often from a loan or mortgage) over time through regular payments. 
    * A portion of each payment is for interest while the remaining amount is applied towards the principal balance. 
    * The percentage of interest versus principal in each payment is determined in an amortization schedule.
    */

    public class LoanAmortization
    {
        private List<ScheduledPayment> _amortizationSchedule;

        public LoanAmortization()
        {
            PayOffDate = StartDate = DateTime.Now;
        }

        public List<ScheduledPayment> AmortizationSchedule
        {
            get
            {
                return _amortizationSchedule;
            }
        }

        public decimal AnnualInterestRate { get; set; }

        public decimal LoanAmount { get; set; }

        public int NumberOfPayments { get; set; }

        public DateTime StartDate { get; set; }

        public int Term { get; set; }

        #region --- LOAN SUMMARY ---

        public decimal MonthlyPayment { get; set; }

        public DateTime PayOffDate { get; set; }

        public decimal TotalInterestPaid { get; set; }

        public decimal TotalPayments { get; set; }

        public string ModeOfPayment { get; set; }

        public Nfmb Borrower
        { get; set; }


        #endregion --- LOAN SUMMARY ---


        public void CreateDiminishingAmortizationSchedule()
        {
            if (LoanAmount == 0) return;
            _amortizationSchedule = new List<ScheduledPayment>();

            decimal monthlyInterestRate = AnnualInterestRate/12m;
            decimal presentValueOfAnnuity =
                Convert.ToDecimal(Math.Pow((double) (1 + monthlyInterestRate), NumberOfPayments));
            decimal monthlyPayment = ((monthlyInterestRate*LoanAmount*presentValueOfAnnuity)/(presentValueOfAnnuity - 1));

            decimal runningBalance = LoanAmount;
            for (int i = 1; i <= NumberOfPayments; i++)
            {
                var scheduledPayment = new ScheduledPayment
                                           {
                                               PaymentNo = i,
                                               Amount = monthlyPayment,
                                               Interest = runningBalance*monthlyInterestRate
                                           };
                scheduledPayment.Principal = monthlyPayment - scheduledPayment.Interest;
                scheduledPayment.Date = StartDate.AddMonths(i);
                scheduledPayment.Balance = runningBalance - scheduledPayment.Principal;
                _amortizationSchedule.Add(scheduledPayment);
                runningBalance = scheduledPayment.Balance;
            }

            // loan summary
            MonthlyPayment = monthlyPayment;
            TotalPayments = _amortizationSchedule.Sum(scheduledPayment => scheduledPayment.Amount);
            TotalInterestPaid = _amortizationSchedule.Sum(scheduledPayment => scheduledPayment.Interest);
            PayOffDate = StartDate.AddMonths(NumberOfPayments);
        }

        public Result CreateStraightLineAmortizationSchedule()
        {
            if (LoanAmount == 0)
				return new Result(false,"Loan Amount cannot be zero.");

            _amortizationSchedule = new List<ScheduledPayment>();

            decimal monthlyInterestRate = AnnualInterestRate/12m;
            decimal monthlyInterestAmount = LoanAmount*monthlyInterestRate;
	        decimal principal = LoanAmount/NumberOfPayments; // eto ung ibabawas monthly sa loan
			decimal monthlyPayment = principal + monthlyInterestAmount;
            decimal runningBalance = LoanAmount;

            for (int i = 1; i <= NumberOfPayments; i++)
            {
                var scheduledPayment = new ScheduledPayment
                {
                    PaymentNo = i,
					Amount = monthlyPayment,
                    Interest = monthlyInterestAmount
                };
                scheduledPayment.Principal = principal;
                scheduledPayment.Date = StartDate.AddMonths(i);
                scheduledPayment.Balance = runningBalance - principal;
                _amortizationSchedule.Add(scheduledPayment);
                runningBalance = scheduledPayment.Balance;
            }
            // loan summary
            MonthlyPayment = monthlyPayment;
			TotalPayments = _amortizationSchedule.Sum(scheduledPayment => scheduledPayment.Amount);
            TotalInterestPaid = _amortizationSchedule.Sum(scheduledPayment => scheduledPayment.Interest);
            PayOffDate = StartDate.AddMonths(NumberOfPayments);

			return new Result(true, "CreateStraightLineAmortizationSchedule");
        }

        public static DataTable GetInitializedReportHeader()
        {
            var queryBuilder = new System.Text.StringBuilder();
            queryBuilder.AppendLine("SELECT '' as MemberCode, '' as MemberName, '' AS MemberAddress,");
            queryBuilder.AppendLine("'' as AccountCode, '' as Title,");
            queryBuilder.AppendLine("0 as Term,0 as NumberOfPayment,");
            queryBuilder.AppendLine("0.00 as Principal, 0.00 as LoanAmount, 0.00 as InterestRate,");
            queryBuilder.AppendLine("0.00 as Payment,0.00 as MonthlySD,");
            queryBuilder.AppendLine("'' as ModePay,");
            queryBuilder.AppendLine("NOW() as DateGranted,NOW() as MaturityDate");
            queryBuilder.AppendLine("FROM members m");
            queryBuilder.AppendLine("LIMIT 1");

            return Database.DatabaseController.ExecuteSelectQuery(queryBuilder.ToString());
        }

        public Account LoanApplied { get; set; }
    }
}
