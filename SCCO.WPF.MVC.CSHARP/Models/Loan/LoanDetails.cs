using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanDetails : INotifyPropertyChanged
    {
        #region --- Private Variables ---

        private string _accountCode;
        private string _accountTitle;
        private string _bankName;
        private string _checkNo;
        private CoMaker[] _coMakers;
        private string _collector;
        private decimal _compulsarySavings;
        private DateTime _cutOffDate;
        private DateTime _dateApplied;

        private DateTime _dateApproved;
        private DateTime _dateCancelled;
        private DateTime _dateReleased;

        private string _description;
        private DateTime _documentDate;
        private int _documentNo;
        private string _documentType;
        private DateTime _grantedDate;
        private decimal _interestAmortization;
        private decimal _interestAmount;
        private decimal _interestRate;
        private bool _isWithCollateral;
        private decimal _loanAmount;
        private int _loanTerms;
        private DateTime _maturityDate;
        private string _memberCode;
        private string _memberName;
        private ModeOfPayments _modeOfPayment;
        private bool[] _notices;
        private decimal _payment;
        private int _releaseNo;
        private string _remarks;
        private string _termsMode;
        private decimal _thisMonth;

        #endregion

        public LoanDetails(DataRow dataRow):this()
        {
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);

            DocumentType = DataConverter.ToString(dataRow["DOC_TYPE"]);
            DocumentNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            DocumentDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);

            LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            if (LoanAmount == 0) return;

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
                OnPropertyChanged("AccountTitle");
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

        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                OnPropertyChanged("DocumentDate");
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

        public string BankName
        {
            get { return _bankName; }
            set
            {
                _bankName = value;
                OnPropertyChanged("BankName");
            }
        }

        public string CheckNo
        {
            get { return _checkNo; }
            set
            {
                _checkNo = value;
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

        /// APPROVED	date
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
            set
            {
                _coMakers = value;
                OnPropertyChanged("CoMakers");
            }
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
            set
            {
                _compulsarySavings = value;
                OnPropertyChanged("CompulsarySavings");
            }
        }


        public bool[] Notices
        {
            get { return _notices; }
            set
            {
                _notices = value;
                OnPropertyChanged("Notices");
            }
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
            var explanationBuilder = new StringBuilder();
            explanationBuilder.Append(string.Format("{0} payable within {1} {2} payment{3}", AccountTitle,
                                                    LoanTerms, ModeOfPayment,
                                                    LoanTerms > 1 ? "s" : ""));

            explanationBuilder.AppendLine(string.Format(" due date on {0}", MaturityDate.ToLongDateString()));

            if (CoMakers.Any())
            {
                IEnumerable<CoMaker> comakers = from comaker in CoMakers
                                                where !string.IsNullOrEmpty(comaker.MemberCode)
                                                select comaker;
                if (comakers.Any())
                {
                    explanationBuilder.AppendLine(string.Format("Co-makers: {0}", string.Join(", ", comakers)));
                }
            }

            return explanationBuilder.ToString();
        }

        internal static LoanDetails ExtractFromDataRow(DataRow dataRow)
        {
            decimal loanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]);
            if (loanAmount == 0) return null;

            return new LoanDetails(dataRow);
        }

        internal static LoanDetails GenerateCompromiseLoanDetails(Voucher voucher, decimal loanAmount,
                                                                  decimal finesAndPenalty, int term,
                                                                  CoMaker[] comakers)
        {
            var loanDetails = new LoanDetails
                {
                    LoanTerms = term,
                    TermsMode = "Month",
                    GrantedDate = voucher.VoucherDate,
                    MaturityDate = voucher.VoucherDate.AddMonths(term),
                    CutOffDate = voucher.VoucherDate.AddDays(7),
                    ModeOfPayment = ModeOfPayments.Monthly,
                    DateReleased = voucher.VoucherDate,
                    LoanAmount = loanAmount + finesAndPenalty
                };

            loanDetails.Payment = loanDetails.LoanAmount/term;

            loanDetails.AccountCode = voucher.AccountCode;
            loanDetails.AccountTitle = voucher.AccountTitle;

            loanDetails.MemberCode = voucher.MemberCode;
            loanDetails.MemberName = voucher.MemberName;

            loanDetails.ReleaseNo = ModelController.Releases.MaxReleaseNumber() + 1;

            loanDetails.CoMakers = comakers;

            loanDetails.DocumentDate = voucher.VoucherDate;
            loanDetails.DocumentNo = voucher.VoucherNo;
            loanDetails.DocumentType = voucher.VoucherType.ToString();

            return loanDetails;
        }

        public static LoanDetails FindByVoucher(VoucherTypes voucherType, int voucherId)
        {
            var item = new LoanDetails();
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("SELECT * FROM `{0}` ", Voucher.GetTableName(voucherType));
            queryBuilder.AppendFormat("WHERE ID = ?Id");
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder, new SqlParameter("?Id", voucherId));

            foreach (DataRow dataRow in dataTable.Rows)
            {
                item = new LoanDetails(dataRow);
                if (voucherType == VoucherTypes.BG)
                {
                    item.LoanTerms = DataConverter.ToInteger(dataRow["TERMS1"]);
                    item.ModeOfPayment = DataConverter.ToModeOfPayment(dataRow["MODE_PAY1"]);
                }
            }
            return item;
        }

        public void Update(VoucherTypes voucherType, int voucherId)
        {
            var sqlParameters = new List<SqlParameter>();

            ModelController.AddParameter(sqlParameters, "?RELEASE_NO", ReleaseNo);
            ModelController.AddParameter(sqlParameters, "?LOAN_AMT", LoanAmount);
            if (voucherType == VoucherTypes.BG)
            {
                ModelController.AddParameter(sqlParameters, "?TERMS1", LoanTerms);
                ModelController.AddParameter(sqlParameters, "?MODE_PAY1",
                                             ModeOfPayment.ToString().PadRight(3).Substring(0, 3));
            }
            else
            {
                ModelController.AddParameter(sqlParameters, "?TERMS", LoanTerms);
                ModelController.AddParameter(sqlParameters, "?MODE_PAY",
                                         ModeOfPayment.ToString().PadRight(3).Substring(0, 3));
            }

            ModelController.AddParameter(sqlParameters, "?TERMS_MODE", TermsMode.PadRight(2).Substring(0, 2));
            ModelController.AddParameter(sqlParameters, "?DATE_GRANT", GrantedDate);
            ModelController.AddParameter(sqlParameters, "?MATURITY", MaturityDate);
            ModelController.AddParameter(sqlParameters, "?CUT_OFF", GrantedDate.AddDays(7));
            

            ModelController.AddParameter(sqlParameters, "?PAYMENT", Payment);

            ModelController.AddParameter(sqlParameters, "?INT_RATE", InterestRate);
            ModelController.AddParameter(sqlParameters, "?INT_AMT", InterestAmount);
            ModelController.AddParameter(sqlParameters, "?INT_AMORT", InterestAmortization);

            if (CoMakers != null)
            {
                ModelController.AddParameter(sqlParameters, "?CO_CODE1", CoMakers[0].MemberCode);
                ModelController.AddParameter(sqlParameters, "?CO_NAME1", CoMakers[0].MemberName);

                ModelController.AddParameter(sqlParameters, "?CO_CODE2", CoMakers[1].MemberCode);
                ModelController.AddParameter(sqlParameters, "?CO_NAME2", CoMakers[1].MemberName);

                ModelController.AddParameter(sqlParameters, "?CO_CODE3", CoMakers[2].MemberCode);
                ModelController.AddParameter(sqlParameters, "?CO_NAME3", CoMakers[2].MemberName);
            }

            var paramId = new SqlParameter("ID", voucherId);
            string tableName = Voucher.GetTableName(voucherType);
            string query = DatabaseController.GenerateUpdateStatement(tableName, sqlParameters, paramId);

            sqlParameters.Add(paramId);
            DatabaseController.ExecuteNonQuery(query, sqlParameters.ToArray());
        }
    }
}