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
    public class CashVoucher : Voucher, IModel
    {
        public CashVoucher()
        {
            ResetProperties();
        }
        private string _checkNo;
        private string _checkNo1;
        private int _id;
        private string _transactionType;
        private string _explanation;
        private string _payableTo;
        private DateTime _dateCheckPosted;
        private DateTime _dateCheckReceived;
        private DateTime _dateCheckDue;
        private string _bank;
        private string _bankTitle;
        private decimal _amount;
        private string _amountInWords;
        private int _withdrawalSlipNo;
        private TimeDepositDetails _timeDepositDetails;
        private LoanDetails _loanDetails;

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

                ModelController.AddParameter(sqlParameters, "?EXPLAIN", Explanation);
                ModelController.AddParameter(sqlParameters, "?PAY_TO", PayableTo);
                
                ModelController.AddParameter(sqlParameters, "?DCP", DateCheckPosted);
                ModelController.AddParameter(sqlParameters, "?DCR", DateCheckReceived);
                ModelController.AddParameter(sqlParameters, "?DD", DateCheckDue);

                ModelController.AddParameter(sqlParameters, "?BANK", Bank);
                ModelController.AddParameter(sqlParameters, "?BANK_TITLE", BankTitle);
                ModelController.AddParameter(sqlParameters, "?AMOUNT", Amount);
                ModelController.AddParameter(sqlParameters, "?AMT_WORDS", AmountInWords);
                ModelController.AddParameter(sqlParameters, "?WS_NO", WithdrawalSlipNo);
                ModelController.AddParameter(sqlParameters, "?POSTED", IsPosted);

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

                    ModelController.AddParameter(sqlParameters, "?MODE_PAY", LoanDetails.ModeOfPayment.ToString().PadRight(3).Substring(0, 3));
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

        private const string TABLE_NAME = "CV";

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string CheckNo //CHECK_NUM
        {
            get { return _checkNo; }
            set
            {
                _checkNo = value;
                OnPropertyChanged("CheckNo");
            }
        }

        public string CheckNo1 //CHECK_NUM1
        {
            get { return _checkNo1; }
            set
            {
                _checkNo1 = value;
                OnPropertyChanged("CheckNo1");
            }
        }

        public string TransactionType //TRN_TYPE
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                OnPropertyChanged("TransactionType");
            }
        }

        public string Explanation //EXPLAIN
        {
            get { return _explanation; }
            set
            {
                _explanation = value;
                OnPropertyChanged("Explanation");
            }
        }

        public string PayableTo //PAY_TO
        {
            get { return _payableTo; }
            set
            {
                _payableTo = value;
                OnPropertyChanged("PayableTo");
            }
        }

        public DateTime DateCheckPosted //DCP
        {
            get { return _dateCheckPosted; }
            set
            {
                _dateCheckPosted = value;
                OnPropertyChanged("DateCheckPosted");
            }
        }

        public DateTime DateCheckReceived //DCR
        {
            get { return _dateCheckReceived; }
            set
            {
                _dateCheckReceived = value;
                OnPropertyChanged("DateCheckReceived");
            }
        }

        public DateTime DateCheckDue //DD
        {
            get { return _dateCheckDue; }
            set
            {
                _dateCheckDue = value;
                OnPropertyChanged("DateCheckDue");
            }
        }

        public string Bank //BANK
        {
            get { return _bank; }
            set
            {
                _bank = value;
                OnPropertyChanged("Bank");
            }
        }

        public string BankTitle //BANK_TITLE
        {
            get { return _bankTitle; }
            set
            {
                _bankTitle = value;
                OnPropertyChanged("BankTitle");
            }
        }

        public decimal Amount //AMOUNT
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public string AmountInWords //AMT_WORDS
        {
            get { return _amountInWords; }
            set
            {
                _amountInWords = value;
                OnPropertyChanged("AmountInWords");
            }
        }

        public int WithdrawalSlipNo //WS_NO 
        {
            get { return _withdrawalSlipNo; }
            set
            {
                _withdrawalSlipNo = value;
                OnPropertyChanged("WithdrawalSlipNo");
            }
        }

        public TimeDepositDetails TimeDepositDetails
        {
            get { return _timeDepositDetails; }
            set { _timeDepositDetails = value; OnPropertyChanged("TimeDepositDetails"); }
        }

        public LoanDetails LoanDetails
        {
            get { return _loanDetails; }
            set { _loanDetails = value; OnPropertyChanged("LoanDetails"); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                {
                    string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, SqlParameters);
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

        public static List<CashVoucher> FindBy(string columnName, object value)
        {
            var key = new SqlParameter("?" + columnName, value);
            string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

            var list = new List<CashVoucher>();
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var cv = new CashVoucher();
                cv.SetPropertiesFromDataRow(dataRow);
                list.Add(cv);
            }
            return list;
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

            Explanation = "";
            PayableTo = "";
            DateCheckPosted = new DateTime();
            DateCheckReceived = new DateTime();
            DateCheckDue = new DateTime();
            Bank = "";
            BankTitle = "";
            Amount = 0m;
            AmountInWords = "";
            WithdrawalSlipNo = 0;
            IsPosted = false;

            LoanDetails = null;
            TimeDepositDetails = null;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]);
            MemberName = DataConverter.ToString(dataRow["MEM_NAME"]);
            CheckNo = DataConverter.ToString(dataRow["CHECK_NUM"]);
            CheckNo1 = DataConverter.ToString(dataRow["CHECK_NUM1"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);
            Debit = DataConverter.ToDecimal(dataRow["DEBIT"]);
            Credit = DataConverter.ToDecimal(dataRow["CREDIT"]);
            VoucherDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            VoucherType = VoucherTypes.CV;
            VoucherNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            TransactionType = DataConverter.ToString(dataRow["TRN_TYPE"]);

            //Explanation = DataConverter.ToString(dataRow["EXPLAIN"]);
            Explanation = Encoding.UTF8.GetString(DataConverter.ToByteArray(dataRow["EXPLAIN"]));

            PayableTo = DataConverter.ToString(dataRow["PAY_TO"]);
            DateCheckPosted = DataConverter.ToDateTime(dataRow["DCP"]);
            DateCheckReceived = DataConverter.ToDateTime(dataRow["DCR"]);
            DateCheckDue = DataConverter.ToDateTime(dataRow["DD"]);
            Bank = DataConverter.ToString(dataRow["BANK"]);
            BankTitle = DataConverter.ToString(dataRow["BANK_TITLE"]);
            Amount = DataConverter.ToDecimal(dataRow["AMOUNT"]);
            AmountInWords = DataConverter.ToString(dataRow["AMT_WORDS"]);
            WithdrawalSlipNo = DataConverter.ToInteger(dataRow["WS_NO"]);
            IsPosted = DataConverter.ToBoolean(dataRow["POSTED"]);

            TimeDepositDetails = TimeDepositDetails.ExtractFromDataRow(dataRow);
            LoanDetails = LoanDetails.ExtractFromDataRow(dataRow);
            
        }

        public static ObservableCollection<CashVoucher> WhereDocumentNumberIs(int documentNo)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE DOC_NUM = ?DOC_NUM", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?DOC_NUM", documentNo));
            var listRecord = new ObservableCollection<CashVoucher>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var foundRecord = new CashVoucher();
                foundRecord.SetPropertiesFromDataRow(dataRow);
                listRecord.Add(foundRecord);
            }
            return listRecord;
        }

        public static int DeleteAll(int documentNo)
        {
            string sql = string.Format("DELETE FROM `CV` WHERE DOC_NUM = ?DOC_NUM");
            return DatabaseController.ExecuteNonQuery(sql, new SqlParameter("?DOC_NUM", documentNo));
        }

        internal static ObservableCollection<CashVoucher> WhereMemberCodeIs(string memberCode)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT * FROM `{0}` WHERE MEM_CODE = ?MEM_CODE", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), new SqlParameter("?MEM_CODE", memberCode));
            var listRecord = new ObservableCollection<CashVoucher>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var foundRecord = new CashVoucher();
                foundRecord.SetPropertiesFromDataRow(dataRow);
                listRecord.Add(foundRecord);
            }
            return listRecord;
        }
    }
}
