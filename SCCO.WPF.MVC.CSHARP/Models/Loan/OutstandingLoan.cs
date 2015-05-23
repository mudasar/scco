using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class OutstandingLoan : INotifyPropertyChanged
    {
        #region --- Privates ----

        private string _memberCode;
        private string _memberName;
        private string _accountCode;
        private string _accountTitle;
        private decimal _endingBalance;
        private DateTime _asOf;
        private DateTime _documentDate;
        private string _documentType;
        private int _documentNo;
        private int _releaseNo;
        private decimal _loanAmount;
        private int _terms;
        private string _termsMode;
        private string _modePay;
        private DateTime _grantedDate;
        private DateTime _maturityDate;
        private DateTime _cutOffDate;
        private decimal _payment;
        private decimal _interestRate;
        private decimal _interestAmount;
        private decimal _interestAmortization;
        private DateTime _approvedDate;
        private DateTime _cancelledDate;
        private DateTime _releasedDate;
        private DateTime _appliedDate;
        private string _comakerCode1;
        private string _comakerName1;
        private string _comakerCode2;
        private string _comakerName2;
        private string _comakerCode3;
        private string _comakerName3;
        private string _comakerCode4;
        private string _comakerName4;
        private string _comakerCode5;
        private string _comakerName5;
        private decimal _thisMonth;
        private string _collector;
        private bool _notice1;
        private bool _notice2;
        private bool _notice3;
        private bool _flag;

        #endregion


        #region --- Public Properties ---

        public string MemberCode
        {
            get { return _memberCode; }
            set
            {
                _memberCode = value;
                OnPropertyChanged("MemberCode");
            }
        }

        public string MemberName
        {
            get { return _memberName; }
            set
            {
                _memberName = value;
                OnPropertyChanged("MemberName");
            }
        }

        public string AccountCode
        {
            get { return _accountCode; }
            set
            {
                _accountCode = value;
                OnPropertyChanged("AccountCode");
            }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set
            {
                _accountTitle = value;
                OnPropertyChanged(AccountTitle);
            }
        }

        public decimal EndingBalance
        {
            get { return _endingBalance; }
            set
            {
                _endingBalance = value;
                OnPropertyChanged("EndingBalance");
            }
        }

        public DateTime AsOf
        {
            get { return _asOf; }
            set
            {
                _asOf = value;
                OnPropertyChanged("AsOf");
            }
        }

        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                OnPropertyChanged("DocumentDate");
            }
        }

        public string DocumentType
        {
            get { return _documentType; }
            set
            {
                _documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        public int DocumentNo
        {
            get { return _documentNo; }
            set
            {
                _documentNo = value;
                OnPropertyChanged("DocumentNo");
            }
        }

        public int ReleaseNo
        {
            get { return _releaseNo; }
            set
            {
                _releaseNo = value;
                OnPropertyChanged("ReleaseNo");
            }
        }

        public decimal LoanAmount
        {
            get { return _loanAmount; }
            set
            {
                _loanAmount = value;
                OnPropertyChanged("LoanAmount");
            }
        }

        public int Terms
        {
            get { return _terms; }
            set
            {
                _terms = value;
                OnPropertyChanged("LoanAmount");
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

        public string ModePay
        {
            get { return _modePay; }
            set
            {
                _modePay = value;
                OnPropertyChanged("TermsMode");
            }
        }

        public DateTime GrantedDate
        {
            get { return _grantedDate; }
            set
            {
                _grantedDate = value;
                OnPropertyChanged("TermsMode");
            }
        }

        public DateTime MaturityDate
        {
            get { return _maturityDate; }
            set
            {
                _maturityDate = value;
                OnPropertyChanged("MaturityDate");
            }
        }

        public DateTime CutOffDate
        {
            get { return _cutOffDate; }
            set
            {
                _cutOffDate = value;
                OnPropertyChanged("MaturityDate");
            }
        }

        public decimal Payment
        {
            get { return _payment; }
            set
            {
                _payment = value;
                OnPropertyChanged("Payment");
            }
        }

        public decimal InterestRate
        {
            get { return _interestRate; }
            set
            {
                _interestRate = value;
                OnPropertyChanged("InterestRate");
            }
        }

        public decimal InterestAmount
        {
            get { return _interestAmount; }
            set
            {
                _interestAmount = value;
                OnPropertyChanged("InterestAmount");
            }
        }

        public decimal InterestAmortization
        {
            get { return _interestAmortization; }
            set
            {
                _interestAmortization = value;
                OnPropertyChanged("InterestAmortization");
            }
        }

        public DateTime ApprovedDate
        {
            get { return _approvedDate; }
            set
            {
                _approvedDate = value;
                OnPropertyChanged("ApprovedDate");
            }
        }

        public DateTime CancelledDate
        {
            get { return _cancelledDate; }
            set
            {
                _cancelledDate = value;
                OnPropertyChanged("CancelledDate");
            }
        }

        public DateTime ReleasedDate
        {
            get { return _releasedDate; }
            set
            {
                _releasedDate = value;
                OnPropertyChanged("CancelledDate");
            }
        }

        public DateTime AppliedDate
        {
            get { return _appliedDate; }
            set
            {
                _appliedDate = value;
                OnPropertyChanged("AppliedDate");
            }
        }

        public string ComakerCode1
        {
            get { return _comakerCode1; }
            set
            {
                _comakerCode1 = value;
                OnPropertyChanged("AppliedDate");
            }
        }

        public string ComakerName1
        {
            get { return _comakerName1; }
            set
            {
                _comakerName1 = value;
                OnPropertyChanged("AppliedDate");
            }
        }

        public string ComakerCode2
        {
            get { return _comakerCode2; }
            set
            {
                _comakerCode2 = value;
                OnPropertyChanged("AppliedDate");
            }
        }

        public string ComakerName2
        {
            get { return _comakerName2; }
            set
            {
                _comakerName2 = value;
                OnPropertyChanged("ComakerName2");
            }
        }

        public string ComakerCode3
        {
            get { return _comakerCode3; }
            set
            {
                _comakerCode3 = value;
                OnPropertyChanged("ComakerCode3");
            }
        }

        public string ComakerName3
        {
            get { return _comakerName3; }
            set
            {
                _comakerName3 = value;
                OnPropertyChanged("ComakerName3");
            }
        }

        public string ComakerCode4
        {
            get { return _comakerCode4; }
            set
            {
                _comakerCode4 = value;
                OnPropertyChanged("ComakerCode4");
            }
        }

        public string ComakerName4
        {
            get { return _comakerName4; }
            set
            {
                _comakerName4 = value;
                OnPropertyChanged("ComakerName4");
            }
        }

        public string ComakerCode5
        {
            get { return _comakerCode5; }
            set
            {
                _comakerCode5 = value;
                OnPropertyChanged("ComakerCode5");
            }
        }

        public string ComakerName5
        {
            get { return _comakerName5; }
            set
            {
                _comakerName5 = value;
                OnPropertyChanged("ComakerName5");
            }
        }

        public decimal ThisMonth
        {
            get { return _thisMonth; }
            set
            {
                _thisMonth = value;
                OnPropertyChanged("ThisMonth");
            }
        }

        public string Collector
        {
            get { return _collector; }
            set
            {
                _collector = value;
                OnPropertyChanged("Collector");
            }
        }

        public bool Notice1
        {
            get { return _notice1; }
            set
            {
                _notice1 = value;
                OnPropertyChanged("Notice1");
            }
        }

        public bool Notice2
        {
            get { return _notice2; }
            set
            {
                _notice2 = value;
                OnPropertyChanged("Notice2");
            }
        }

        public bool Notice3
        {
            get { return _notice3; }
            set
            {
                _notice3 = value;
                OnPropertyChanged("Notice3");
            }
        }

        #endregion

        public bool Flag
        {
            get { return _flag; }
            set { _flag = value; OnPropertyChanged("Flag");}
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            MemberCode = DataConverter.ToString(dataRow["member_code"]);
            MemberName = DataConverter.ToString(dataRow["member_name"]);
            AccountCode = DataConverter.ToString(dataRow["account_code"]);
            AccountTitle = DataConverter.ToString(dataRow["account_title"]);

            EndingBalance = DataConverter.ToDecimal(dataRow["ending_balance"]);
            AsOf = DataConverter.ToDateTime(dataRow["as_of"]);

            DocumentType = DataConverter.ToString(dataRow["document_type"]);
            DocumentNo = DataConverter.ToInteger(dataRow["document_no"]);
            DocumentDate = DataConverter.ToDateTime(dataRow["document_date"]);

            ReleaseNo = DataConverter.ToInteger(dataRow["release_no"]);
            LoanAmount = DataConverter.ToDecimal(dataRow["loan_amount"]);

            Terms = DataConverter.ToInteger(dataRow["terms"]);
            TermsMode = DataConverter.ToString(dataRow["terms_mode"]);
            ModePay = DataConverter.ToString(dataRow["mode_pay"]);

            GrantedDate = DataConverter.ToDateTime(dataRow["granted_date"]);
            MaturityDate = DataConverter.ToDateTime(dataRow["maturity_date"]);
            CutOffDate = DataConverter.ToDateTime(dataRow["cut_off_date"]);

            Payment = DataConverter.ToDecimal(dataRow["payment"]);
            InterestRate = DataConverter.ToDecimal(dataRow["interest_rate"]);
            InterestAmount = DataConverter.ToDecimal(dataRow["interest_amount"]);
            InterestAmortization = DataConverter.ToDecimal(dataRow["interest_amortization"]);

            ApprovedDate = DataConverter.ToDateTime(dataRow["approved_date"]);
            CancelledDate = DataConverter.ToDateTime(dataRow["cancelled_date"]);
            ReleasedDate = DataConverter.ToDateTime(dataRow["released_date"]);
            AppliedDate = DataConverter.ToDateTime(dataRow["applied_date"]);

            ComakerCode1 = DataConverter.ToString(dataRow["comaker_code1"]);
            ComakerName1 = DataConverter.ToString(dataRow["comaker_name1"]);
            ComakerCode2 = DataConverter.ToString(dataRow["comaker_code2"]);
            ComakerName2 = DataConverter.ToString(dataRow["comaker_name2"]);
            ComakerCode3 = DataConverter.ToString(dataRow["comaker_code3"]);
            ComakerName3 = DataConverter.ToString(dataRow["comaker_name3"]); 
            ComakerCode4 = DataConverter.ToString(dataRow["comaker_code4"]);
            ComakerName4 = DataConverter.ToString(dataRow["comaker_name4"]); 
            ComakerCode5 = DataConverter.ToString(dataRow["comaker_code5"]);
            ComakerName5 = DataConverter.ToString(dataRow["comaker_name5"]);

            ThisMonth = DataConverter.ToDecimal(dataRow["this_month"]);

            Collector = DataConverter.ToString(dataRow["collector"]);

            Notice1 = DataConverter.ToBoolean(dataRow["notice1"]);
            Notice2 = DataConverter.ToBoolean(dataRow["notice2"]);
            Notice3 = DataConverter.ToBoolean(dataRow["notice3"]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OutstandingLoans : ObservableCollection<OutstandingLoan>
    {
        public static OutstandingLoans AsOf(DateTime asOf)
        {
            var collection = new OutstandingLoans();

            const string sp = "sp_outstanding_loans";
            var param = new Database.SqlParameter("as_of", asOf);
            var dataTable = Database.DatabaseController.ExecuteStoredProcedure(sp, param);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new OutstandingLoan();
                item.SetPropertiesFromDataRow(dataRow);
                item.Flag = item.EndingBalance > 0;
                collection.Add(item);
            }

            return collection;
        }
    }
}
