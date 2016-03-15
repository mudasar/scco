using System;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public class FinesRebateCalculatorViewModel : INotifyPropertyChanged
    {
        private decimal _fines;
        private decimal _finesRatePerMonth;
        private decimal _interest;
        private decimal _loanBalance;
        private LoanDetails _loanDetails;
        private DateTime _processDate;
        private decimal _rebate;
        private string _status = "Current";

        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set
            {
                _loanDetails = value;
                OnPropertyChanged("LoanDetails");
            }
        }

        public decimal LoanBalance
        {
            get { return _loanBalance; }
            set
            {
                _loanBalance = value;
                OnPropertyChanged("LoanBalance");
            }
        }

        public decimal Interest
        {
            get { return _interest; }
            set
            {
                _interest = value;
                OnPropertyChanged("Interest");
            }
        }

        public decimal Rebate
        {
            get { return _rebate; }
            set
            {
                _rebate = value;
                OnPropertyChanged("Rebate");
            }
        }

        public decimal Fines
        {
            get { return _fines; }
            set
            {
                _fines = value;
                OnPropertyChanged("Fines");
            }
        }

        public DateTime ProcessDate
        {
            get { return _processDate; }
            set
            {
                _processDate = value;
                OnPropertyChanged("ProcessDate");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public decimal FinesRatePerMonth
        {
            get { return _finesRatePerMonth; }
            set
            {
                _finesRatePerMonth = value;
                OnPropertyChanged("FinesRate");
            }
        }

        public string ReportTitle
        {
            get { return Fines > 0 ? "Fines" : "Rebate"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Calculate()
        {
            DateTime dateStart = LoanDetails.GrantedDate;
            DateTime dateEnd = dateStart.AddMonths(12);
            int totalDaysInYear = dateEnd.Subtract(dateStart).Days;
            int loanTermInDays = LoanDetails.MaturityDate.Subtract(LoanDetails.GrantedDate).Days;

            Rebate = 0;
            Interest = 0;
            Fines = 0;
            Status = "Current";

            decimal finesRate = GlobalSettings.RateOfFines;
            FinesRatePerMonth = finesRate/12;

            decimal interestPerDay = 0m;
            if (LoanDetails.InterestRate > 0)
            {
                interestPerDay = LoanDetails.InterestAmount / loanTermInDays;
            }

            // REBATE - Loan must be paid (zero balance) before the maturity date
            if (LoanBalance == 0)
            {
                if (ProcessDate <= LoanDetails.MaturityDate)
                {
                    int daysBeforeMaturity = LoanDetails.MaturityDate.Subtract(ProcessDate).Days;
                    Rebate = interestPerDay*daysBeforeMaturity;
                    Status = "Settled";
                }
            }
            else
            {
                if (ProcessDate > LoanDetails.MaturityDate)
                {
                    int daysOverDue = ProcessDate.Subtract(LoanDetails.MaturityDate).Days;
                    Interest = interestPerDay*daysOverDue;
                    Fines = ((LoanBalance*finesRate)/totalDaysInYear)*daysOverDue;
                    Status = "Overdue";
                }
            }
        }

        public DataTable GetReportData()
        {
            var reportData = new DataTable("fines_rebate");
            reportData.Columns.Add("borrower", typeof (string));
            reportData.Columns.Add("loan_applied", typeof (string));
            reportData.Columns.Add("granted_date", typeof (DateTime));
            reportData.Columns.Add("maturity_date", typeof (DateTime));
            reportData.Columns.Add("process_date", typeof (DateTime));
            reportData.Columns.Add("loan_amount", typeof (decimal));
            reportData.Columns.Add("payment", typeof (decimal));
            reportData.Columns.Add("payment_made", typeof (decimal));
            reportData.Columns.Add("balance_on_due_date", typeof (decimal));
            reportData.Columns.Add("rebate", typeof (decimal));
            reportData.Columns.Add("interest", typeof (decimal));
            reportData.Columns.Add("fines", typeof (decimal));
            reportData.Columns.Add("payables", typeof (decimal));

            DataRow newRow = reportData.NewRow();
            newRow["borrower"] = LoanDetails.MemberCode + " - " + LoanDetails.MemberName;
            newRow["loan_applied"] = LoanDetails.AccountCode + " - " + LoanDetails.AccountTitle;
            newRow["granted_date"] = LoanDetails.GrantedDate;
            newRow["maturity_date"] = LoanDetails.MaturityDate;
            newRow["loan_amount"] = LoanDetails.LoanAmount;
            newRow["payment"] = LoanDetails.Payment;
            newRow["process_date"] = ProcessDate;
            newRow["payment_made"] = LoanDetails.LoanAmount - LoanBalance;
            newRow["balance_on_due_date"] = LoanBalance;
            newRow["rebate"] = Rebate;
            newRow["interest"] = Interest;
            newRow["fines"] = Fines;
            newRow["payables"] = Interest + Fines;

            reportData.Rows.Add(newRow);
            return reportData;
        }
    }
}