using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class FinancialConditionReportConfiguration : INotifyPropertyChanged, IModel
    {
        private int _id;
        private decimal _orderNo;
        private int _indent;
        private string _accountTitle;
        private string _accountCode;
        private string _endBalanceFormula;
        private string _groupBalanceFormula;
        private string _operator;
        private bool _isEndBalanceUnderlined;
        private bool _isGroupBalanceUnderlined;

        private const string TableName = "statement_of_financial_condition";

        public int ID
        {
            get { return _id; }
            set { _id = value;
                OnPropertyChanged("ID");
            }
        }

        public decimal OrderNo
        {
            get { return _orderNo; }
            set { _orderNo = value;
            OnPropertyChanged("OrderNo");}
        }

        public int Indent
        {
            get { return _indent; }
            set { _indent = value; OnPropertyChanged("Indent");}
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set { _accountTitle = value; OnPropertyChanged("AccountTitle"); }
        }

        public string AccountCode
        {
            get { return _accountCode; }
            set { _accountCode = value; OnPropertyChanged("AccountCode"); }
        }

        public string EndBalanceFormula
        {
            get { return _endBalanceFormula; }
            set { _endBalanceFormula = value; OnPropertyChanged("EndBalanceFormula"); }
        }

        public string GroupBalanceFormula
        {
            get { return _groupBalanceFormula; }
            set { _groupBalanceFormula = value; OnPropertyChanged("GroupBalanceFormula"); }
        }

        public string Operator
        {
            get { return _operator; }
            set { _operator = value; OnPropertyChanged("Operator"); }
        }

        public bool IsEndBalanceUnderlined
        {
            get { return _isEndBalanceUnderlined; }
            set { _isEndBalanceUnderlined = value; OnPropertyChanged("IsEndBalanceUnderlined"); }
        }

        public bool IsGroupBalanceUnderlined
        {
            get { return _isGroupBalanceUnderlined; }
            set { _isGroupBalanceUnderlined = value; OnPropertyChanged("IsGroupBalanceUnderlined"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Result Create()
        {
            Action createRecord = () =>
            {
                var sqlParameters = GetSqlParameters();

                var sql = DatabaseController.GenerateInsertStatement(TableName, sqlParameters);
                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameters.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);
                var sqlParameters = GetSqlParameters();
                var sql = DatabaseController.GenerateUpdateStatement(TableName, sqlParameters, key);
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

                DataTable dataTable = DatabaseController.FindRecord(TableName, id);
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
            OrderNo = 0m;
            Indent = 0;
            AccountCode = "";
            AccountTitle = "";
            EndBalanceFormula = "";
            GroupBalanceFormula = "";
            Operator = "";
            IsEndBalanceUnderlined = false;
            IsGroupBalanceUnderlined = false;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = Utilities.DataConverter.ToInteger(dataRow["id"]);
            OrderNo = Utilities.DataConverter.ToDecimal(dataRow["order_no"]);
            Indent = Utilities.DataConverter.ToInteger(dataRow["indent"]);
            AccountCode = Utilities.DataConverter.ToString(dataRow["account_code"]);
            AccountTitle = Utilities.DataConverter.ToString(dataRow["account_title"]);
            EndBalanceFormula = Utilities.DataConverter.ToString(dataRow["end_balance_formula"]);
            GroupBalanceFormula = Utilities.DataConverter.ToString(dataRow["group_balance_formula"]);
            Operator = Utilities.DataConverter.ToString(dataRow["operator"]);
            IsEndBalanceUnderlined = Utilities.DataConverter.ToBoolean(dataRow["is_end_balance_underlined"]);
            IsGroupBalanceUnderlined = Utilities.DataConverter.ToBoolean(dataRow["is_group_balance_underlined"]);
        }


        private List<SqlParameter> GetSqlParameters()
        {
            var sqlParameters = new List<SqlParameter>();
            ModelController.AddParameter(sqlParameters, "?order_no", OrderNo);
            ModelController.AddParameter(sqlParameters, "?indent", Indent);
            ModelController.AddParameter(sqlParameters, "?account_code", AccountCode);
            ModelController.AddParameter(sqlParameters, "?account_title", AccountTitle);
            ModelController.AddParameter(sqlParameters, "?end_balance_formula", EndBalanceFormula);
            ModelController.AddParameter(sqlParameters, "?group_balance_formula", GroupBalanceFormula);
            ModelController.AddParameter(sqlParameters, "?operator", Operator);
            ModelController.AddParameter(sqlParameters, "?is_end_balance_underlined", IsEndBalanceUnderlined);
            ModelController.AddParameter(sqlParameters, "?is_group_balance_underlined", IsGroupBalanceUnderlined);

            return sqlParameters;
        }

        internal static FinancialConditionReportConfigurationCollection CollectAll()
        {
            var query = string.Format("SELECT * FROM `{0}` ORDER BY order_no", TableName);
            var collection = new FinancialConditionReportConfigurationCollection();
            var dataTable = DatabaseController.ExecuteSelectQuery(query);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new FinancialConditionReportConfiguration();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }


    public class FinancialConditionReportConfigurationCollection : ObservableCollection<FinancialConditionReportConfiguration>
    {}
}
