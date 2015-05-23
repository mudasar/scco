using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Models
{
    public class TransactionDetail : INotifyPropertyChanged, IModel, ICloneable
    {
        private int _transactionDetailId;
        private int _transactionHeaderId;
        private string _memberCode;
        private string _memberName;
        private string _accountCode;
        private string _accountTitle;
        private decimal _debitAmount;
        private decimal _creditAmount;

        #region --- PROPPERTIES ---

        public int TransactionDetailId
        {
            get { return _transactionDetailId; }
            set
            {
                _transactionDetailId = value;
                OnPropertyChanged("TransactionDetailId");
            }
        }

        public int TransactionHeaderId
        {
            get { return _transactionHeaderId; }
            set
            {
                _transactionHeaderId = value;
                OnPropertyChanged("TransactionHeaderId");
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

        public decimal DebitAmount
        {
            get { return _debitAmount; }
            set
            {
                _debitAmount = value;
                OnPropertyChanged("DebitAmount");
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

        #endregion

        #region METHOD and EVENTS

        #region --- CRUD ---

        private const string TableName = "transactiondetails";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var list = new List<SqlParameter>
                               {
                                   new SqlParameter("?TransactionHeaderId", TransactionHeaderId),
                                   new SqlParameter("?MemberCode", MemberCode),
                                   new SqlParameter("?MemberName", MemberName),
                                   new SqlParameter("?AccountCode", AccountCode),
                                   new SqlParameter("?AccountTitle", AccountTitle),
                                   new SqlParameter("?DebitAmount", DebitAmount),
                                   new SqlParameter("?CreditAmount", CreditAmount)
                               };
                return list;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?TransactionDetailId", TransactionDetailId); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          List<SqlParameter> sqlParameter = Parameters;

                                          string sql = DatabaseController.GenerateInsertStatement(TableName,
                                                                                                  sqlParameter);
                                          TransactionDetailId = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          SqlParameter key = ParamKey;

                                          List<SqlParameter> sqlParameter = Parameters;
                                          sqlParameter.Add(key);

                                          string sql = DatabaseController.GenerateUpdateStatement(TableName,
                                                                                                  sqlParameter,
                                                                                                  key);

                                          DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          SqlParameter key = ParamKey;

                                          string sql = DatabaseController.GenerateDeleteStatement(TableName, key);

                                          DatabaseController.ExecuteNonQuery(sql, key);
                                      };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                                    {
                                        ResetProperties();
                                        TransactionDetailId = id;

                                        SqlParameter key = ParamKey;
                                        string sql = DatabaseController.GenerateSelectStatement(TableName, key);

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
            TransactionDetailId = 0;
            AccountCode = "";
            AccountTitle = "";
            CreditAmount = 0;
            DebitAmount = 0;
            MemberCode = "";
            MemberName = "";
            TransactionHeaderId = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            TransactionDetailId = (int) dataRow["TransactionDetailId"];
            TransactionHeaderId = Convert.ToInt32(dataRow["TransactionHeaderId"]);
            AccountCode = (string) dataRow["AccountCode"];
            AccountTitle = (string) dataRow["AccountTitle"];
            CreditAmount = Convert.ToDecimal(dataRow["CreditAmount"]);
            DebitAmount = Convert.ToDecimal(dataRow["DebitAmount"]);
            MemberCode = (string) dataRow["MemberCode"];
            MemberName = (string) dataRow["MemberName"];
        }

        #endregion

        public TimeDepositDetails GetTimeDepositDetail()
        {
            DataTable dataTable =
                DatabaseController.ExecuteStoredProcedure("SP_GetTimeDepositDetailsByTransactionDetailId",
                                                          new SqlParameter("?TransactionDetailId", TransactionDetailId));
            var td = new TimeDepositDetails();


            if (dataTable.Rows.Count > 0)
            {
                td = TimeDepositDetails.ExtractFromDataRow(dataTable.Rows[0]);
            }

            return td;
        }

        public static List<TransactionDetail> GetTransactionDetailsByTransactionHeaderId(int transactionHeaderId)
        {
            string sqlCommandText =
                string.Format(
                    "SELECT * FROM {0}  WHERE TransactionHeaderId = ?TransactionHeaderId ORDER BY TransactionDetailId",
                    TableName);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?TransactionHeaderId",
                                                                                         transactionHeaderId));
            var transactionDetails = new List<TransactionDetail>();


            foreach (DataRow dataRow in dataTable.Rows)
            {
                var trd = new TransactionDetail();
                trd.SetPropertiesFromDataRow(dataRow);
                transactionDetails.Add(trd);
            }

            return transactionDetails;
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return this;
        }

        #endregion

        //public LoanModule.LoanDetail GetLoanDetail()
        //{
        //    var dataTable = Database.ExecuteStoredProcedure("SP_GetLoanDetailsByTransactionDetailId",
        //                                        new SqlParameter("?TransactionDetailId", TransactionDetailId));
        //    var ld = new LoanModule.LoanDetail();


        //    if (dataTable.Rows.Count > 0)
        //        ld = new LoanModule.LoanDetail(dataTable.Rows[0]);

        //    return ld;
        //}
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
