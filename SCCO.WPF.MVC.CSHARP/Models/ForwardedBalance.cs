using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class ForwardedBalance : Voucher, IModel
    {
        private int _id;
        private string _checkNo;

        private DateTime _dueDate;
        private string _purchaseOrderNo;
        private string _transactionType;
        private decimal _beginningBalance;
        private decimal _endingBalance;

        private string _bankTitle;
        private string _checkNo1;
        private LoanDetails _loanDetails;
        private TimeDepositDetails _timeDepositDetails;

        private const string TABLE_NAME = "slbal";

        public ForwardedBalance()
        {
            VoucherType = VoucherTypes.BG;
        }

        //ID	int
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

         
        //CHECK_NUM	varchar
        public string CheckNo
        {
            get { return _checkNo; }
            set { _checkNo = value; OnPropertyChanged("CheckNo");}
        }

        //DUE_DATE	date
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }

        //PO_NO	varchar
        public string PurchaseOrderNo
        {
            get { return _purchaseOrderNo; }
            set
            {
                _purchaseOrderNo = value;
                OnPropertyChanged("PurchaseOrderNo");
            }
        }



        //TRN_TYPE	varchar
        public string TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                OnPropertyChanged("TransactionType");
            }
        }

        //BEG_BAL	double
        public decimal BeginningBalance
        {
            get { return _beginningBalance; }
            set
            {
                _beginningBalance = value;
                OnPropertyChanged("BeginningBalance");
            }
        }

        //END_BAL	double
        public decimal EndingBalance
        {
            get { return _endingBalance; }
            set
            {
                _endingBalance = value;
                OnPropertyChanged("EndingBalance");
            }
        }

        //BANK_TITLE	varchar
        public string BankTitle
        {
            get { return _bankTitle; }
            set
            {
                _bankTitle = value;
                OnPropertyChanged("BankTitle");
            }
        }

        //CHECK_NUM1	varchar
        public string CheckNo1
        {
            get { return _checkNo1; }
            set
            {
                _checkNo1 = value;
                OnPropertyChanged("CheckNo1");
            }
        }

        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set { _loanDetails = value; OnPropertyChanged("LoanDetails"); }
        }

        public TimeDepositDetails TimeDepositDetails
        {
            get { return _timeDepositDetails; }
            set { _timeDepositDetails = value; OnPropertyChanged("TimeDepositDetails"); }
        }

        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?MEM_CODE", MemberCode);
                ModelController.AddParameter(sqlParameters, "?MEM_NAME", MemberName);
                ModelController.AddParameter(sqlParameters, "?CHECK_NUM", CheckNo);
                ModelController.AddParameter(sqlParameters, "?CHECK_NUM1", CheckNo1);
                ModelController.AddParameter(sqlParameters, "?ACC_CODE", AccountCode);
                ModelController.AddParameter(sqlParameters, "?TITLE", AccountTitle);
                ModelController.AddParameter(sqlParameters, "?DEBIT", Debit);
                ModelController.AddParameter(sqlParameters, "?CREDIT", Credit);
                ModelController.AddParameter(sqlParameters, "?DOC_DATE", VoucherDate);
                ModelController.AddParameter(sqlParameters, "?DOC_TYPE", VoucherType.ToString());
                ModelController.AddParameter(sqlParameters, "?DOC_NUM", VoucherNo);
                ModelController.AddParameter(sqlParameters, "?TRN_TYPE", TransactionType);

                //ModelController.AddParameter(sqlParameters, "?EXPLAIN", Explanation);
                //ModelController.AddParameter(sqlParameters, "?PAY_TO", PayableTo);
                //ModelController.AddParameter(sqlParameters, "?DCP", DateCheckPosted);
                //ModelController.AddParameter(sqlParameters, "?DCR", DateCheckReceived);
                //ModelController.AddParameter(sqlParameters, "?DD", DateCheckDue);
                //ModelController.AddParameter(sqlParameters, "?BANK", Bank);
                ModelController.AddParameter(sqlParameters, "?BANK_TITLE", BankTitle);
                //ModelController.AddParameter(sqlParameters, "?AMOUNT", Amount);
                //ModelController.AddParameter(sqlParameters, "?AMT_WORDS", AmountInWords);
                //ModelController.AddParameter(sqlParameters, "?POSTED", IsPosted);

                if (TimeDepositDetails != null)
                {
                    ModelController.AddParameter(sqlParameters, "?CERT_NO", TimeDepositDetails.CertificateNo);
                    ModelController.AddParameter(sqlParameters, "?TERM", TimeDepositDetails.Term);
                    ModelController.AddParameter(sqlParameters, "?RATE", TimeDepositDetails.Rate);
                    ModelController.AddParameter(sqlParameters, "?DATE_IN", TimeDepositDetails.DateIn);
                }

                if (LoanDetails != null)
                {
                    ModelController.AddParameter(sqlParameters, "?RELEASE_NO", LoanDetails.ReleaseNo);
                    ModelController.AddParameter(sqlParameters, "?LOAN_AMT", LoanDetails.LoanAmount);
                    ModelController.AddParameter(sqlParameters, "?TERMS", LoanDetails.LoanTerms);
                    ModelController.AddParameter(sqlParameters, "?TERMS_MODE", LoanDetails.TermsMode.PadRight(2).Substring(0, 2));
                    ModelController.AddParameter(sqlParameters, "?DATE_GRANT", LoanDetails.GrantedDate);
                    ModelController.AddParameter(sqlParameters, "?MATURITY", LoanDetails.MaturityDate);
                    ModelController.AddParameter(sqlParameters, "?CUT_OFF", LoanDetails.CutOffDate);
                    ModelController.AddParameter(sqlParameters, "?MODE_PAY1",
                                                       LoanDetails.ModeOfPayment.ToString().PadRight(3).Substring(0, 3));
                    ModelController.AddParameter(sqlParameters, "?PAYMENT", LoanDetails.Payment);
                    ModelController.AddParameter(sqlParameters, "?INT_RATE", LoanDetails.InterestRate);
                    ModelController.AddParameter(sqlParameters, "?INT_AMT", LoanDetails.InterestAmount);
                    ModelController.AddParameter(sqlParameters, "?INT_AMORT", LoanDetails.InterestAmortization);
                    ModelController.AddParameter(sqlParameters, "?APPROVED", LoanDetails.DateApproved);
                    ModelController.AddParameter(sqlParameters, "?CANCELLED", LoanDetails.DateCancelled);
                    ModelController.AddParameter(sqlParameters, "?RELEASED", LoanDetails.DateReleased);
                    ModelController.AddParameter(sqlParameters, "?APPLIED", LoanDetails.DateApplied);
                    ModelController.AddParameter(sqlParameters, "?THIS_MONTH", LoanDetails.ThisMonth);
                    ModelController.AddParameter(sqlParameters, "?COLLECTOR", LoanDetails.Collector);
                    ModelController.AddParameter(sqlParameters, "?REMARKS", LoanDetails.Remarks);
                    ModelController.AddParameter(sqlParameters, "?COLLAT", LoanDetails.IsWithCollateral);
                    ModelController.AddParameter(sqlParameters, "?DESC", LoanDetails.Description);

                    ModelController.AddParameter(sqlParameters, "?CO_CODE1", LoanDetails.CoMakers[0].MemberCode);
                    ModelController.AddParameter(sqlParameters, "?CO_NAME1", LoanDetails.CoMakers[0].MemberName);
                    ModelController.AddParameter(sqlParameters, "?CO_CODE2", LoanDetails.CoMakers[1].MemberCode);
                    ModelController.AddParameter(sqlParameters, "?CO_NAME2", LoanDetails.CoMakers[1].MemberName);
                    ModelController.AddParameter(sqlParameters, "?CO_CODE3", LoanDetails.CoMakers[2].MemberCode);
                    ModelController.AddParameter(sqlParameters, "?CO_NAME3", LoanDetails.CoMakers[2].MemberName);
                    ModelController.AddParameter(sqlParameters, "?CO_CODE4", LoanDetails.CoMakers[3].MemberCode);
                    ModelController.AddParameter(sqlParameters, "?CO_NAME4", LoanDetails.CoMakers[3].MemberName);
                    ModelController.AddParameter(sqlParameters, "?CO_CODE5", LoanDetails.CoMakers[4].MemberCode);
                    ModelController.AddParameter(sqlParameters, "?CO_NAME5", LoanDetails.CoMakers[4].MemberName);

                    ModelController.AddParameter(sqlParameters, "?NOTICE1", LoanDetails.Notices[0]);
                    ModelController.AddParameter(sqlParameters, "?NOTICE2", LoanDetails.Notices[1]);
                    ModelController.AddParameter(sqlParameters, "?NOTICE3", LoanDetails.Notices[2]);
                }


                return sqlParameters;
            }
        }

        #region Implementation of IModel

        public Result Create()
        {
            Action createRecord = () =>
            {
                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                        SqlParameters);
                ID = DatabaseController.ExecuteInsertQuery(sql, SqlParameters.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                List<SqlParameter> sqlParameters = SqlParameters;
                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                        sqlParameters, key);

                sqlParameters.Add(key);
                DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

                DatabaseController.ExecuteNonQuery(sql, key);
            };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
            {
                ResetProperties();
                ID = id;

                var key = new SqlParameter("?ID", ID);
                string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SetPropertiesFromDataRow(dataRow);
                }
            };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            ID = 0;
            MemberCode = "";
            MemberName = "";
            CheckNo = "";
            CheckNo1 = "";
            AccountCode = "";
            AccountTitle = "";
            Debit = 0m;
            Credit = 0m;
            VoucherDate = new DateTime();
            VoucherType = VoucherTypes.CV;
            VoucherNo = 0;
            TransactionType = "";
            IsPosted = false;

            CheckNo = "";
            DueDate = new DateTime();
            PurchaseOrderNo = "";
            TransactionType = "";
            BeginningBalance = 0m;
            EndingBalance = 0m;
            BankTitle = "";
            CheckNo1 = "";

            LoanDetails = new LoanDetails();
            TimeDepositDetails = new TimeDepositDetails();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);
            Debit = DataConverter.ToDecimal(dataRow["DEBIT"]);
            Credit = DataConverter.ToDecimal(dataRow["CREDIT"]);

            VoucherDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            VoucherType = DataConverter.ToVoucherType(dataRow["DOC_TYPE"]);
            VoucherNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            
            TransactionType = DataConverter.ToString(dataRow["TRN_TYPE"]);
            //IsPosted = DataConverter.ToBoolean(dataRow["POSTED"]);

            CheckNo = DataConverter.ToString(dataRow["CHECK_NUM"]);
            DueDate = DataConverter.ToDateTime(dataRow["DUE_DATE"]);
            BankTitle = DataConverter.ToString(dataRow["BANK_TITLE"]);
            CheckNo1 = DataConverter.ToString(dataRow["CHECK_NUM1"]);

            PurchaseOrderNo = DataConverter.ToString(dataRow["PO_NO"]);
            BeginningBalance = DataConverter.ToDecimal(dataRow["BEG_BAL"]);
            EndingBalance = DataConverter.ToDecimal(dataRow["END_BAL"]);

            TimeDepositDetails = TimeDepositDetails.ExtractFromDataRow(dataRow);
            LoanDetails = LoanDetails.ExtractFromDataRow(dataRow);
            if (LoanDetails != null)
            {
                // slbal column is different from cv and jv regarding loan terms
                LoanDetails.LoanTerms = DataConverter.ToInteger(dataRow["TERMS1"]);
                LoanDetails.ModeOfPayment = DataConverter.ToModeOfPayment(dataRow["MODE_PAY1"]);
                LoanDetails.TermsMode = DataConverter.ToTermsMode(dataRow["TERMS1"], dataRow["TERMS_MODE"]);
            }
        }

        #endregion

        public static ObservableCollection<ForwardedBalance> WhereMemberCodeIs(string memberCode)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE MEM_CODE = ?MEM_CODE", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?MEM_CODE", memberCode));
            var listRecord = new ObservableCollection<ForwardedBalance>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var foundRecord = new ForwardedBalance();
                foundRecord.SetPropertiesFromDataRow(dataRow);
                listRecord.Add(foundRecord);
            }
            return listRecord;
        }


        internal static ForwardedBalanceCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new ForwardedBalanceCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new ForwardedBalance();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

    }

    public class ForwardedBalanceCollection : ObservableCollection<ForwardedBalance>
    {
    }
}