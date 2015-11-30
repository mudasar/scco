using System;
using System.ComponentModel;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.TimeDeposit
{
    public class TimeDepositDetails : INotifyPropertyChanged
    {
        private decimal _amount;

        private string _certificateNo;

        private DateTime _maturity;

        private decimal _rate;

        private int _term;

        private string _termsMode;

        private DateTime _timeDepositDateIn;

        public TimeDepositDetails(System.Data.DataRow dataRow)
            : this()
        {
            var certificateNo = DataConverter.ToString(dataRow["CERT_NO"]);
            if (string.IsNullOrEmpty(certificateNo)) return;

            var amount = DataConverter.ToDecimal(dataRow["DEBIT"]) + DataConverter.ToDecimal(dataRow["CREDIT"]);
            if (amount == 0) return;
            CertificateNo = certificateNo;

            DateIn = DataConverter.ToDateTime(dataRow["DATE_IN"]);

            // current implementation uses rate in percentage (i.e 0.375)
            Rate = DataConverter.ToDecimal(dataRow["RATE"]);
            if (Rate >= 1)
                Rate = Rate/100m;

            Term = DataConverter.ToInteger(dataRow["TERM"]);
            TermsMode = Term%30 == 0 ? "Days" : "Months";
            Amount = amount;
            Maturity = CalculateMaturity(DateIn, Term, TermsMode);
        }

        public TimeDepositDetails()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public string CertificateNo //CERT_NO
        {
            get { return _certificateNo; }
            set
            {
                _certificateNo = value;
                OnPropertyChanged("CertificateNo");
            }
        }

        public DateTime DateIn //DATE_IN
        {
            get { return _timeDepositDateIn; }
            set
            {
                _timeDepositDateIn = value;
                OnPropertyChanged("DateIn");
            }
        }

        public DateTime Maturity
        {
            get { return _maturity; }
            set
            {
                _maturity = value;
                OnPropertyChanged("Maturity");
            }
        }

        public decimal Rate //RATE
        {
            get { return _rate; }
            set
            {
                _rate = value;
                OnPropertyChanged("Rate");
            }
        }

        public int Term //TERM
        {
            get { return _term; }
            set
            {
                _term = value;
                OnPropertyChanged("Term");
            }
        }

        public string TermsMode
        {
            get { return _termsMode; }
            set
            {
                _termsMode = value;
                OnPropertyChanged("TermsMode");
            }
        }

        public decimal EndingBalance(DateTime asOf)
        {
            return _amount + (CalculateInterestEarned(asOf) - CalculateServiceFee(asOf));
        }

        public decimal CalculateInterestEarned(DateTime processingDate)
        {
            var savingsDepositInterestRatePerAnnum = GlobalSettings.RateOfInterestOnSavingsDeposit;
            var savingsDepositInterestEarnedPerAnnum = Amount*savingsDepositInterestRatePerAnnum;
            var savingsDepositInterestEarnedPerDay = savingsDepositInterestEarnedPerAnnum/
                                                     CountDaysInBetweenDates(DateIn, DateIn.AddYears(1));

            var timeDepositInterestEarnedPerAnnum = Amount*Rate;
            var timeDepositInterestEarnedPerDay = timeDepositInterestEarnedPerAnnum/
                                                  CountDaysInBetweenDates(DateIn, DateIn.AddYears(1));

            if (IsMature(processingDate))
            {
                // get days until maturity
                var timeDepositDays = CountDaysInBetweenDates(DateIn, Maturity);
                var timeDepositInterestEarned = timeDepositInterestEarnedPerDay*timeDepositDays;

                // get days a day after maturity until processing date
                var daysAfterMaturity = CountDaysInBetweenDates(Maturity.AddDays(1), processingDate);
                if (daysAfterMaturity >= 1)
                {
                    timeDepositInterestEarned += savingsDepositInterestEarnedPerDay*daysAfterMaturity;
                }
                return Math.Round(timeDepositInterestEarned);
            }

            // premature
            return Math.Round(savingsDepositInterestEarnedPerDay*CountDaysInBetweenDates(DateIn, processingDate));
        }

        public decimal CalculateServiceFee(DateTime processingDate)
        {
            var minimumServiceFeeAmount = GlobalSettings.AmountOfMinimumTimeDepositServiceFee;
            var interestEarned = CalculateInterestEarned(processingDate);
            var serviceFeeRate = GlobalSettings.RateOfTimeDepositServiceFee;
            var serviceFee = Math.Round(interestEarned*serviceFeeRate);
            return serviceFee > minimumServiceFeeAmount ? serviceFee : minimumServiceFeeAmount;
        }

        public int CountDaysFromEntry(DateTime processingDate)
        {
            return (processingDate - DateIn).Days;
        }

        public bool IsPremature(DateTime processingDate)
        {
            return Maturity > processingDate;
        }

        public bool IsMature(DateTime processingDate)
        {
            return Maturity <= processingDate;
        }

        private int CountDaysInBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            return (dateTo - dateFrom).Days;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private DateTime CalculateMaturity(DateTime dateIn, int term, string termsMode)
        {
            switch (termsMode.Substring(0, 1))
            {
                case "d":
                case "D":
                    return dateIn.AddDays(term);

                case "m":
                case "M":
                    return dateIn.AddMonths(term);
            }
            return new DateTime();
        }

        internal static TimeDepositDetails ExtractFromDataRow(System.Data.DataRow dataRow)
        {
            var certificateNo = DataConverter.ToString(dataRow["CERT_NO"]);
            if (string.IsNullOrEmpty(certificateNo)) return null;

            return new TimeDepositDetails(dataRow);
        }

        public bool IsValid
        {
            get { return DateIn != Maturity; }
        }
    }
}