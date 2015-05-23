using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanDeduction : INotifyPropertyChanged, IModel
    {
        private int _id;
        private int _loanProductId;
        private string _accountCode;
        private decimal _amount;
        private string _accountTitle;

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID");}
        }

        public int LoanProductId
        {
            get { return _loanProductId; }
            set { _loanProductId = value; OnPropertyChanged("LoanProductId"); }
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

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region --- CRUD ---

        private const string TABLE_NAME = "loan_deductions";


        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var list = new List<SqlParameter>
                               {
                                   new SqlParameter("?LoanProductId", LoanProductId),
                                   new SqlParameter("?AccountCode", AccountCode),
                                   new SqlParameter("?Amount", Amount)
                               };
                return list;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        public Result Create()
        {
            Action createRecord = () =>
            {
                List<SqlParameter> sqlParameter = Parameters;

                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                        sqlParameter);
                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
            {
                SqlParameter key = ParamKey;

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

                SqlParameter key = ParamKey;
                //string sql = DatabaseController.GenerateSelectStatement(TableName, key);
                var sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine("SELECT ld.ID, ld.LoanProductId, ld.AccountCode, ch.TITLE, ld.Amount");
                sqlBuilder.AppendLine("FROM loandeductions ld");
                sqlBuilder.AppendLine("LEFT JOIN chart ch");
                sqlBuilder.AppendLine("ON ld.AccountCode = ch.`CODE`");
                sqlBuilder.AppendLine("WHERE ld.ID = ?ID");

                DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, key);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SetPropertiesFromDataRow(dataRow);
                }
            };

            return ActionController.InvokeAction(findRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                SqlParameter key = ParamKey;

                List<SqlParameter> sqlParameter = Parameters;
                sqlParameter.Add(key);

                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                        sqlParameter,
                                                                        key);

                DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion --- CRUD ---

        public void ResetProperties()
        {
            ID = 0;
            LoanProductId = 0;
            AccountCode = string.Empty;
            Amount = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = Utilities.DataConverter.ToInteger(dataRow["ID"]);
            LoanProductId = Utilities.DataConverter.ToInteger(dataRow["LoanProductId"]);
            AccountCode = Utilities.DataConverter.ToString(dataRow["AccountCode"]);
            Amount = Utilities.DataConverter.ToDecimal(dataRow["Amount"]);

            var account = Account.FindByCode(AccountCode);
            AccountTitle = account.AccountTitle;
        }

        internal static List<LoanDeduction> GetListByLoanProductId(int loanProductId)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT ld.ID, ld.LoanProductId, ld.AccountCode, ch.TITLE, ld.Amount");
            sqlBuilder.AppendLine("FROM " + TABLE_NAME + " ld");
            sqlBuilder.AppendLine("LEFT JOIN chart ch");
            sqlBuilder.AppendLine("ON ld.AccountCode = ch.`CODE`");
            sqlBuilder.AppendLine("WHERE ld.LoanProductId = ?LoanProductId");
            sqlBuilder.AppendLine("ORDER BY ld.AccountCode");
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder,
                                                              new SqlParameter("?LoanProductId", loanProductId));
            var loanDeductions = new List<LoanDeduction>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var loanDeduction = new LoanDeduction();
                loanDeduction.SetPropertiesFromDataRow(dataRow);
                loanDeductions.Add(loanDeduction);
            }

            return loanDeductions;
        }
    }
}
