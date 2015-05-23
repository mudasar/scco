using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    [Serializable]
    public class ForwardedBalanceOld : INotifyPropertyChanged, IModel
    {
        private string _accountCode;
        private string _accountTitle;
        private decimal _creditAmount;
        private decimal _debitAmount;
        private DateTime _documentDate;
        private int _documentNumber;
        private string _documentType;
        private int _forwardedBalanceId;
        private int _forwardedYear;
        private int _loanDetailId;
        private string _memberCode;
        private string _memberName;
        private int _timeDepositDetailId;
        private const string TableName = "ForwardedBalances";

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

        public decimal CreditAmount
        {
            get { return _creditAmount; }
            set
            {
                _creditAmount = value;
                OnPropertyChanged("CreditAmount");
            }
        }

        public decimal DebitAmount
        {
            get { return _debitAmount; }
            set
            {
                _debitAmount = value;
                OnPropertyChanged("DebitAmount");
            }
        }

        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                OnPropertyChanged("VoucherDate");
            }
        }

        public int DocumentNumber
        {
            get { return _documentNumber; }
            set
            {
                _documentNumber = value;
                OnPropertyChanged("DocumentNumber");
            }
        }

        public string DocumentType
        {
            get { return _documentType; }
            set
            {
                _documentType = value;
                OnPropertyChanged("VoucherType");
            }
        }

        public int ForwardedBalanceId
        {
            get { return _forwardedBalanceId; }
            set
            {
                _forwardedBalanceId = value;
                OnPropertyChanged("ForwardedBalanceId");
            }
        }

        public int ForwardedYear
        {
            get { return _forwardedYear; }
            set
            {
                _forwardedYear = value;
                OnPropertyChanged("ForwardedYear");
            }
        }

        public int LoanDetailId
        {
            get { return _loanDetailId; }
            set
            {
                _loanDetailId = value;
                OnPropertyChanged("LoanDetailId");
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

        public int TimeDepositDetailId
        {
            get { return _timeDepositDetailId; }
            set
            {
                _timeDepositDetailId = value;
                OnPropertyChanged("TimeDepositDetailId");
            }
        }

        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var sqlParameter = new List<SqlParameter>();
                                          sqlParameter.Add(new SqlParameter("?AccountCode", AccountCode));
                                          sqlParameter.Add(new SqlParameter("?CreditAmount", CreditAmount));
                                          sqlParameter.Add(new SqlParameter("?DebitAmount", DebitAmount));
                                          sqlParameter.Add(new SqlParameter("?VoucherDate", DocumentDate));
                                          sqlParameter.Add(new SqlParameter("?DocumentNumber", DocumentNumber));
                                          sqlParameter.Add(new SqlParameter("?VoucherType", DocumentType));
                                          sqlParameter.Add(new SqlParameter("?ForwardedYear", ForwardedYear));
                                          sqlParameter.Add(new SqlParameter("?LoanDetailId", LoanDetailId));
                                          sqlParameter.Add(new SqlParameter("?MemberCode", MemberCode));
                                          sqlParameter.Add(new SqlParameter("?MemberName", MemberName));
                                          sqlParameter.Add(new SqlParameter("?TimeDepositDetailId", TimeDepositDetailId));

                                          var sql = DatabaseController.GenerateInsertStatement(TableName, sqlParameter);
                                          ForwardedBalanceId = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          var key = new SqlParameter("?ForwardedBalanceId", ForwardedBalanceId);

                                          var sqlParameter = new List<SqlParameter>();
                                          sqlParameter.Add(key);
                                          sqlParameter.Add(new SqlParameter("?AccountCode", AccountCode));
                                          sqlParameter.Add(new SqlParameter("?CreditAmount", CreditAmount));
                                          sqlParameter.Add(new SqlParameter("?DebitAmount", DebitAmount));
                                          sqlParameter.Add(new SqlParameter("?VoucherDate", DocumentDate));
                                          sqlParameter.Add(new SqlParameter("?DocumentNumber", DocumentNumber));
                                          sqlParameter.Add(new SqlParameter("?VoucherType", DocumentType));
                                          sqlParameter.Add(new SqlParameter("?ForwardedYear", ForwardedYear));
                                          sqlParameter.Add(new SqlParameter("?LoanDetailId", LoanDetailId));
                                          sqlParameter.Add(new SqlParameter("?MemberCode", MemberCode));
                                          sqlParameter.Add(new SqlParameter("?MemberName", MemberName));
                                          sqlParameter.Add(new SqlParameter("?TimeDepositDetailId", TimeDepositDetailId));

                                          var sql = DatabaseController.GenerateUpdateStatement(TableName, sqlParameter,
                                                                                               key);

                                          DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          var key = new SqlParameter("?ForwardedBalanceId", ForwardedBalanceId);

                                          var sql = DatabaseController.GenerateDeleteStatement(TableName, key);

                                          DatabaseController.ExecuteNonQuery(sql, key);
                                      };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                                    {
                                        ResetProperties();
                                        ForwardedBalanceId = id;

                                        var key = new SqlParameter("?ForwardedBalanceId", ForwardedBalanceId);
                                        var sql = DatabaseController.GenerateSelectStatement(TableName, key);

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
            ForwardedBalanceId = 0;
            AccountCode = string.Empty;
            AccountTitle = string.Empty;
            CreditAmount = 0m;
            DebitAmount = 0m;
            DocumentDate = new DateTime();
            DocumentNumber = 0;
            DocumentType = string.Empty;
            ForwardedYear = 0;
            LoanDetailId = 0;
            MemberCode = string.Empty;
            MemberName = string.Empty;
            TimeDepositDetailId = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            if (dataRow["ForwardedBalanceId"] != DBNull.Value)
                ForwardedBalanceId = Convert.ToInt32(dataRow["ForwardedBalanceId"]);

            if (dataRow["MemberCode"] != DBNull.Value)
                MemberCode = Convert.ToString(dataRow["MemberCode"]);
            if (dataRow["MemberName"] != DBNull.Value)
                MemberName = Convert.ToString(dataRow["MemberName"]);

            if (dataRow["AccountCode"] != DBNull.Value)
                AccountCode = Convert.ToString(dataRow["AccountCode"]);
            if (dataRow["AccountTitle"] != DBNull.Value)
                AccountTitle = Convert.ToString(dataRow["AccountTitle"]);

            if (dataRow["DebitAmount"] != DBNull.Value)
                DebitAmount = Convert.ToDecimal(dataRow["DebitAmount"]);
            if (dataRow["CreditAmount"] != DBNull.Value)
                CreditAmount = Convert.ToDecimal(dataRow["CreditAmount"]);

            if (dataRow["VoucherDate"] != DBNull.Value)
                DocumentDate = Convert.ToDateTime(dataRow["VoucherDate"]);
            if (dataRow["VoucherType"] != DBNull.Value)
                DocumentType = Convert.ToString(dataRow["VoucherType"]);
            if (dataRow["DocumentNumber"] != DBNull.Value)
                DocumentNumber = Convert.ToInt32(dataRow["DocumentNumber"]);

            if (dataRow["ForwardedYear"] != DBNull.Value)
                ForwardedYear = Convert.ToInt32(dataRow["ForwardedYear"]);

            if (dataRow["TimeDepositDetailId"] != DBNull.Value)
                TimeDepositDetailId = Convert.ToInt32(dataRow["TimeDepositDetailId"]);

            if (dataRow["LoanDetailId"] != DBNull.Value)
                LoanDetailId = Convert.ToInt32(dataRow["LoanDetailId"]);
        }

        #endregion

        public static List<ForwardedBalanceOld> GetListByYear(int year)
        {
            const string sql = "SELECT * FROM `ForwardedBalances` WHERE ForwardedYear = ?ForwardedYear";
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, new SqlParameter("?ForwardedYear", year));
            var result = new List<ForwardedBalanceOld>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var fb = new ForwardedBalanceOld();
                fb.SetPropertiesFromDataRow(dataRow);
                result.Add(fb);
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
