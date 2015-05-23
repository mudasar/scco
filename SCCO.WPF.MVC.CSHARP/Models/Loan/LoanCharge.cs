using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class LoanCharge : INotifyPropertyChanged, IModel
    {
        #region --- FIELDS ---

        private string _accountCode;
        private string _accountTitle;
        private decimal _amount;
        private int _id;
        private int _loanProductId;
        private decimal _rate;

        private const string TableName = "loan_charges";

        #endregion

        #region --- CONSTRUCTOR ---

        public LoanCharge()
        {
            ID = 0;
            LoanProductId = 0;
            AccountCode = "";
            Rate = 0;
        }

        public LoanCharge(int chargesId, int loanProductId, string accountCode, decimal rate)
        {
            ID = chargesId;
            LoanProductId = loanProductId;
            AccountCode = accountCode;
            Rate = rate;
            AccountTitle = Account.FindByCode(AccountCode).AccountTitle;
        }

        #endregion

        #region  --- PROPERTIES ---

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

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("LoanChargeId"); }
        }

        public int LoanProductId
        {
            get { return _loanProductId; }
            set { _loanProductId = value; OnPropertyChanged("LoanProductId"); }
        }

        public decimal Rate
        {
            get { return _rate; }
            set { _rate = value; OnPropertyChanged("Rate"); }
        }

        #endregion


        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?LoanProductId", LoanProductId);
                ModelController.AddParameter(sqlParameters, "?AccountCode", AccountCode);
                ModelController.AddParameter(sqlParameters, "?Rate", Rate);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?ID", ID); }
        }

        #region METHODS & EVENTS

        #region IModel Members

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var sqlParameter = Parameters;
                                          var sql = DatabaseController.GenerateInsertStatement(TableName, sqlParameter);
                                          ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          var key = new SqlParameter("?ID", ID);
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
                ID = id;

                var key = ParamKey;
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
            ID = 0;
            LoanProductId = 0;
            AccountCode = "";
            Rate = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int)dataRow["ID"];
            LoanProductId = (int)dataRow["LoanProductId"];
            AccountCode = (string)dataRow["AccountCode"];
            Rate = (decimal)dataRow["Rate"];

            AccountTitle = Account.FindByCode(AccountCode).AccountTitle;
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {

                                          var key = ParamKey;
                                          var sqlParameter = Parameters;

                                          var sql = DatabaseController.GenerateUpdateStatement(TableName, sqlParameter,
                                                                                               key);
                                          DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        #endregion

        public static List<LoanCharge> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TableName);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            var list = new List<LoanCharge>();

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new LoanCharge();
                item.SetPropertiesFromDataRow(row);
                list.Add(item);
            }
            return list;
        }

        public static List<LoanCharge> GetListByLoanProductId(int loanProductId)
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE LoanProductId = ?LoanProductId ORDER BY AccountCode" ,
                                                  TableName);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                              new SqlParameter("?LoanProductId", loanProductId));

            var list = new List<LoanCharge>();

            foreach (DataRow row in dataTable.Rows) 
            {
                var item = new LoanCharge();
                item.SetPropertiesFromDataRow(row);
                list.Add(item);
            }
            return list;
            //return (from DataRow row in dataTable.Rows
            //        select new LoanCharge
            //            ((int)row["LoanChargeId"], (int)row["LoanProductId"], (string)row["AccountCode"], (decimal)row["Rate"])).ToList();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}