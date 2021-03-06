﻿using System;
using System.ComponentModel;
using System.Linq;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanDetails : INotifyPropertyChanged
    {
        public LoanDetails(System.Data.DataRow dataRow)
            : this()
        {
            LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            if (LoanAmount == 0) return;

            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);

            DocumentType = DataConverter.ToString(dataRow["DOC_TYPE"]);
            DocumentNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            DocumentDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);

            BankName = DataConverter.ToString(dataRow["BANK_TITLE"]);
            CheckNo = DataConverter.ToString(dataRow["CHECK_NUM1"]);

            ReleaseNo = DataConverter.ToInteger(dataRow["RELEASE_NO"]);
            LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            LoanTerms = DataConverter.ToInteger(dataRow["TERMS"]);
            TermsMode = DataConverter.ToTermsMode(dataRow["TERMS"], dataRow["TERMS_MODE"]);
            GrantedDate = DataConverter.ToDateTime(dataRow["DATE_GRANT"]);
            MaturityDate = DataConverter.ToDateTime(dataRow["MATURITY"]);
            CutOffDate = DataConverter.ToDateTime(dataRow["CUT_OFF"]);
            ModeOfPayment = DataConverter.ToModeOfPayment(dataRow["MODE_PAY"]);
            Payment = DataConverter.ToDecimal(dataRow["PAYMENT"]);
            InterestRate = DataConverter.ToDecimal(dataRow["INT_RATE"]);
            if (InterestRate >= 1)
                InterestRate = InterestRate/100m;
            
            InterestAmount = DataConverter.ToDecimal(dataRow["INT_AMT"]);
            InterestAmortization = DataConverter.ToDecimal(dataRow["INT_AMORT"]);
            DateApproved = DataConverter.ToDateTime(dataRow["APPROVED"]);
            DateCancelled = DataConverter.ToDateTime(dataRow["CANCELLED"]);
            DateReleased = DataConverter.ToDateTime(dataRow["RELEASED"]);
            DateApplied = DataConverter.ToDateTime(dataRow["APPLIED"]);

            ThisMonth = DataConverter.ToDecimal(dataRow["THIS_MONTH"]);
            Collector = DataConverter.ToString(dataRow["COLLECTOR"]);
            Remarks = DataConverter.ToString(dataRow["REMARKS"]);
            IsWithCollateral = DataConverter.ToBoolean(dataRow["COLLAT"]);
            Description = DataConverter.ToString(dataRow["DESC"]);


            CoMakers[0] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE1"]),
                                      DataConverter.ToString(dataRow["CO_NAME1"]));

            CoMakers[1] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE2"]),
                                      DataConverter.ToString(dataRow["CO_NAME2"]));

            CoMakers[2] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE3"]),
                                      DataConverter.ToString(dataRow["CO_NAME3"]));

            CoMakers[3] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE4"]),
                                      DataConverter.ToString(dataRow["CO_NAME4"]));

            CoMakers[4] = new CoMaker(DataConverter.ToString(dataRow["CO_CODE5"]),
                                      DataConverter.ToString(dataRow["CO_NAME5"]));

            Notices[0] = DataConverter.ToBoolean(dataRow["NOTICE1"]);
            Notices[1] = DataConverter.ToBoolean(dataRow["NOTICE2"]);
            Notices[2] = DataConverter.ToBoolean(dataRow["NOTICE3"]);
        }

        public LoanDetails()
        {
            CoMakers = new CoMaker[5];
            for (int i = 0; i < 5; i++)
            {
                CoMakers[i] = new CoMaker();
            }
            Notices = new bool[3];
            for (int i = 0; i < 3; i++)
            {
                Notices[i] = false;
            }
        }

        private int _releaseNo;
        private decimal _loanAmount;
        private int _loanTerms;
        private string _termsMode;
        private DateTime _grantedDate;
        private DateTime _maturityDate;
        private DateTime _cutOffDate;
        private DateTime _dateReleased;
        private DateTime _dateApplied;

        private decimal _payment;
        private decimal _interestRate;
        private decimal _interestAmount;
        private decimal _interestAmortization;
        private DateTime _dateApproved;
        private DateTime _dateCancelled;

        private decimal _thisMonth;
        private string _collector;
        
        private string _remarks;
        private bool _isWithCollateral;
        private string _description;
        private ModeOfPayments _modeOfPayment;
        private CoMaker[] _coMakers;
        private bool[] _notices;
        private string _memberCode;
        private string _memberName;
        private string _accountCode;
        private string _accountTitle;
        private string _documentType;
        private DateTime _documentDate;
        private int _documentNo;
        private string _bankName;
        private string _checkNo;
        private decimal _compulsarySavings;


        public string MemberCode
        {
            get { return _memberCode; }
            set { _memberCode = value; OnPropertyChanged("MemberCode"); }
        }

        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; OnPropertyChanged("MemberName"); }
        }


        public string AccountCode
        {
            get { return _accountCode; }
            set { _accountCode = value; OnPropertyChanged("AccountCode"); }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set { _accountTitle = value; OnPropertyChanged("AccountTitle"); }
        }

        public string DocumentType
        {
            get { return _documentType; }
            set { _documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set { _documentDate = value;
                OnPropertyChanged("DocumentDate");
            }
        }

        public int DocumentNo
        {
            get { return _documentNo; }
            set { _documentNo = value;
                OnPropertyChanged("DocumentNo");
            }
        }

        public string BankName
        {
            get { return _bankName; }
            set { _bankName = value;
                OnPropertyChanged("BankName");
            }
        }

        public string CheckNo
        {
            get { return _checkNo; }
            set { _checkNo = value;
                OnPropertyChanged("CheckNo");
            }
        }

        //RELEASE_NO	int
        public int ReleaseNo
        {
            get { return _releaseNo; }
            set
            {
                _releaseNo = value;
                OnPropertyChanged("ReleaseNo");
            }
        }

        //LOAN_AMT	double
        public decimal LoanAmount
        {
            get { return _loanAmount; }
            set
            {
                _loanAmount = value;
                OnPropertyChanged("LoanAmount");
            }
        }

        //TERMS1	int
        public int LoanTerms
        {
            get { return _loanTerms; }
            set
            {
                _loanTerms = value;
                OnPropertyChanged("LoanTerms");
            }
        }

        //TERMS_MODE	varchar
        public string TermsMode
        {
            get { return _termsMode; }
            set
            {
                _termsMode = value;
                OnPropertyChanged("TermsMode");
            }
        }

        //DATE_GRANT	date
        public DateTime GrantedDate
        {
            get { return _grantedDate; }
            set
            {
                _grantedDate = value;
                OnPropertyChanged("GrantedDate");
            }
        }

        //MATURITY	date
        public DateTime MaturityDate
        {
            get { return _maturityDate; }
            set
            {
                _maturityDate = value;
                OnPropertyChanged("MaturityDate");
            }
        }

        //CUT_OFF	date
        public DateTime CutOffDate
        {
            get { return _cutOffDate; }
            set
            {
                _cutOffDate = value;
                OnPropertyChanged("CutOffDate");
            }
        }

        //PAYMENT	double
        public decimal Payment
        {
            get { return _payment; }
            set
            {
                _payment = value;
                OnPropertyChanged("Payment");
            }
        }

        //MODE_PAY	varchar
        public ModeOfPayments ModeOfPayment
        {
            get { return _modeOfPayment; }
            set
            {
                _modeOfPayment = value;
                OnPropertyChanged("ModeOfPayment");
            }
        }

        //INT_RATE	double
        public decimal InterestRate
        {
            get { return _interestRate; }
            set
            {
                _interestRate = value;
                OnPropertyChanged("InterestRate");
            }
        }

        //INT_AMT	double
        public decimal InterestAmount
        {
            get { return _interestAmount; }
            set
            {
                _interestAmount = value;
                OnPropertyChanged("InterestAmount");
            }
        }

        //INT_AMORT	double
        public decimal InterestAmortization
        {
            get { return _interestAmortization; }
            set
            {
                _interestAmortization = value;
                OnPropertyChanged("InterestAmortization");
            }
        }

        ///APPROVED	date
        public DateTime DateApproved
        {
            get { return _dateApproved; }
            set
            {
                _dateApproved = value;
                OnPropertyChanged("DateApproved");
            }
        }

        //CANCELLED	date
        public DateTime DateCancelled
        {
            get { return _dateCancelled; }
            set
            {
                _dateCancelled = value;
                OnPropertyChanged("DateCancelled");
            }
        }

        //RELEASED	date
        public DateTime DateReleased
        {
            get { return _dateReleased; }
            set
            {
                _dateReleased = value;
                OnPropertyChanged("DateRelease");
            }
        }

        //APPLIED	date
        public DateTime DateApplied
        {
            get { return _dateApplied; }
            set
            {
                _dateApplied = value;
                OnPropertyChanged("DateApplied");
            }
        }

        public CoMaker[] CoMakers
        {
            get { return _coMakers; }
            set { _coMakers = value; OnPropertyChanged("CoMakers"); }
        }

        //THIS_MONTH	double
        public decimal ThisMonth
        {
            get { return _thisMonth; }
            set
            {
                _thisMonth = value;
                OnPropertyChanged("ThisMonth");
            }
        }

        //COLLECTOR	varchar
        public string Collector
        {
            get { return _collector; }
            set
            {
                _collector = value;
                OnPropertyChanged("Collector");
            }
        }


        //REMARKS	varchar
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        //COLLAT	tinyint
        public bool IsWithCollateral
        {
            get { return _isWithCollateral; }
            set
            {
                _isWithCollateral = value;
                OnPropertyChanged("IsWithCollateral");
            }
        }

        //DESC	varchar
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public Decimal CompulsarySavings
        {
            get { return _compulsarySavings; }
            set { _compulsarySavings = value; OnPropertyChanged("CompulsarySavings"); }
        }


        public bool[] Notices
        {
            get { return _notices; }
            set { _notices = value; OnPropertyChanged("Notices");}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetMember(Nfmb member)
        {
            MemberCode = member.MemberCode;
            MemberName = member.MemberName;
        }

        public virtual void SetAccount(Account account)
        {
            AccountCode = account.AccountCode;
            AccountTitle = account.AccountTitle;
        }

        public virtual void SetDocument(VoucherDocument document)
        {
            DocumentType = document.Type.ToString();
            DocumentNo = document.Number;
            DocumentDate = document.Date;
        }

        public string GenerateExplanation()
        {
            var explanationBuilder = new System.Text.StringBuilder();
            explanationBuilder.Append(string.Format("{0} payable within {1} {2} payment{3}", AccountTitle,
                                                    LoanTerms, ModeOfPayment,
                                                    LoanTerms > 1 ? "s" : ""));

            explanationBuilder.AppendLine(string.Format(" due date on {0}", MaturityDate.ToLongDateString()));

            if (CoMakers.Any())
            {
                var comakers = from comaker in CoMakers
                               where !string.IsNullOrEmpty(comaker.MemberCode)
                               select comaker;
                if (comakers.Any())
                {
                    explanationBuilder.AppendLine(string.Format("Co-makers: {0}", string.Join(", ", comakers)));
                }
            }

            return explanationBuilder.ToString();
        }

        internal static LoanDetails ExtractFromDataRow(System.Data.DataRow dataRow)
        {
            var loanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            if (loanAmount == 0) return null;

            return new LoanDetails(dataRow);
        }
    }
}

