using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Account : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _accountCode;
        private string _accountTitle;
        private string _nature;
        private string _codeNo;
        private string _cashFlow;
        private string _subsidiaryLedger;
        private bool _isClosed;
        private int _scheduleCode;
        private string _groupCode;

        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?CODE", AccountCode);
                ModelController.AddParameter(sqlParameters, "?TITLE", AccountTitle);
                ModelController.AddParameter(sqlParameters, "?NATURE", Nature);
                ModelController.AddParameter(sqlParameters, "?CODE_NO", CodeNo);
                ModelController.AddParameter(sqlParameters, "?FLOW", CashFlow);
                ModelController.AddParameter(sqlParameters, "?SL", SubsidiaryLedger);
                ModelController.AddParameter(sqlParameters, "?CLOSE", IsClosed);
                ModelController.AddParameter(sqlParameters, "?SCODE", ScheduleCode);
                ModelController.AddParameter(sqlParameters, "?CODE1", GroupCode);

                return sqlParameters;
            }
        }

        private const string TABLE_NAME = "chart";

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public static string TableName
        {
            get { return TABLE_NAME; }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string AccountCode //CODE
        {
            get { return _accountCode; }
            set { _accountCode = value; OnPropertyChanged("AccountCode"); }
        }

        public string AccountTitle //TITLE
        {
            get { return _accountTitle; }
            set { _accountTitle = value; OnPropertyChanged("AccountTitle"); }
        }

        public string Nature //NATURE
        {
            get { return _nature; }
            set { _nature = value; OnPropertyChanged("Nature"); }
        }

        public string CodeNo //CODE_NO
        {
            get { return _codeNo; }
            set { _codeNo = value; OnPropertyChanged("CodeNo"); }
        }

        public string CashFlow //FLOW
        {
            get { return _cashFlow; }
            set { _cashFlow = value; OnPropertyChanged("CashFlow"); }
        }

        public string SubsidiaryLedger //SL
        {
            get { return _subsidiaryLedger; }
            set { _subsidiaryLedger = value; OnPropertyChanged("SubsidiaryLedger"); }
        }

        public bool IsClosed //CLOSE
        {
            get { return _isClosed; }
            set { _isClosed = value; OnPropertyChanged("IsClosed"); }
        }

        public int ScheduleCode //SCODE
        {
            get { return _scheduleCode; }
            set { _scheduleCode = value; OnPropertyChanged("ScheduleCode"); }
        }

        public string GroupCode //CODE1
        {
            get { return _groupCode; }
            set { _groupCode = value; OnPropertyChanged("GroupCode"); }
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
            AccountCode = "";
            AccountTitle = "";
            Nature = "";
            CodeNo = "";
            CashFlow = "";
            SubsidiaryLedger = "";
            IsClosed = false;
            ScheduleCode = 0;
            GroupCode = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            AccountCode = DataConverter.ToString(dataRow["CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);
            Nature = DataConverter.ToString(dataRow["NATURE"]);
            CodeNo = DataConverter.ToString(dataRow["CODE_NO"]);
            CashFlow = DataConverter.ToString(dataRow["FLOW"]);
            SubsidiaryLedger = DataConverter.ToString(dataRow["SL"]);
            IsClosed = DataConverter.ToBoolean(dataRow["CLOSE"]);
            ScheduleCode = DataConverter.ToInteger(dataRow["SCODE"]);
            GroupCode = DataConverter.ToString(dataRow["CODE1"]);
        }

        #endregion

        public static Account FindByCode(string code)
        {
            var account = new Account();
            var key = new SqlParameter("?CODE", code);
            string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
            foreach (DataRow dataRow in dataTable.Rows)
            {

                account.SetPropertiesFromDataRow(dataRow);
            }
            return account;
        }

        public static Account FindByName(string name)
        {
            var account = new Account();
            var key = new SqlParameter("?TITLE", name);
            string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
            foreach (DataRow dataRow in dataTable.Rows)
            {

                account.SetPropertiesFromDataRow(dataRow);
            }
            return account;
        }


        public static List<string> GetListOfSavingsDepositCode()
        {
            const string sp = "sp_list_savings_deposit";
            var dataTable = DatabaseController.ExecuteStoredProcedure(sp);
            return (from DataRow dataRow in dataTable.Rows select DataConverter.ToString(dataRow["CODE"])).ToList();
        }
        public static List<string> GetListOfTimeDepositCode()
        {
            const string sp = "sp_list_time_deposit";
            var dataTable = DatabaseController.ExecuteStoredProcedure(sp);
            return (from DataRow dataRow in dataTable.Rows select DataConverter.ToString(dataRow["CODE"])).ToList();
        }

        public static List<string> GetListOfLoanReceivableCode()
        {
            const string sp = "sp_list_loan_receivables";
            var dataTable = DatabaseController.ExecuteStoredProcedure(sp);
            return (from DataRow dataRow in dataTable.Rows select DataConverter.ToString(dataRow["CODE"])).ToList();
        }

        public static List<Account> GetListOfLoanReceivables()
        {
            var loanAccounts = new List<Account>();
            const string sp = "sp_list_loan_receivables";
            var dataTable = DatabaseController.ExecuteStoredProcedure(sp); 
            
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var loanAccount = new Account();
                loanAccount.SetPropertiesFromDataRow(dataRow);
                loanAccounts.Add(loanAccount);
            }
            return loanAccounts;
        }



        #region Commented

        //    private string _accountCode;
        //    private int _accountId;
        //    private int _accountLevel;
        //    private string _accountTitle;
        //    private string _accountType;
        //    private string _category;
        //    private bool? _isCashFlow;
        //    private bool? _isClose;
        //    private bool? _isSubsidiaryLedger;
        //    private string _nature;
        //    private string _parentAccountCode;
        //    private int? _reportScheduleId;

        //    #region --- PROPERTIES ---

        //    public string AccountCode
        //    {
        //        get { return _accountCode; }
        //        set
        //        {
        //            _accountCode = value;
        //            OnPropertyChanged("AccountCode");
        //        }
        //    }

        //    public int AccountId
        //    {
        //        get { return _accountId; }
        //        set
        //        {
        //            _accountId = value;
        //            OnPropertyChanged("AccountId");
        //        }
        //    }

        //    public int AccountLevel
        //    {
        //        get { return _accountLevel; }
        //        set
        //        {
        //            _accountLevel = value;
        //            OnPropertyChanged("AccountLevel");
        //        }
        //    }

        //    public string AccountTitle
        //    {
        //        get { return _accountTitle; }
        //        set
        //        {
        //            _accountTitle = value;
        //            OnPropertyChanged("AccountTitle");
        //        }
        //    }

        //    public string AccountType
        //    {
        //        get { return _accountType; }
        //        set
        //        {
        //            _accountType = value;
        //            OnPropertyChanged("AccountType");
        //        }
        //    }

        //    public string Category
        //    {
        //        get { return _category; }
        //        set
        //        {
        //            _category = value;
        //            OnPropertyChanged("Category");
        //        }
        //    }

        //    public bool? IsCashFlow
        //    {
        //        get { return _isCashFlow; }
        //        set
        //        {
        //            _isCashFlow = value;
        //            OnPropertyChanged("IsCashFlow");
        //        }
        //    }

        //    public bool? IsClose
        //    {
        //        get { return _isClose; }
        //        set
        //        {
        //            _isClose = value;
        //            OnPropertyChanged("IsClose");
        //        }
        //    }

        //    public bool? IsSubsidiaryLedger
        //    {
        //        get { return _isSubsidiaryLedger; }
        //        set
        //        {
        //            _isSubsidiaryLedger = value;
        //            OnPropertyChanged("IsSubsidiaryLedger");
        //        }
        //    }

        //    public string Nature
        //    {
        //        get { return _nature; }
        //        set
        //        {
        //            _nature = value;
        //            OnPropertyChanged("Nature");
        //        }
        //    }

        //    public string ParentAccountCode
        //    {
        //        get { return _parentAccountCode; }
        //        set
        //        {
        //            _parentAccountCode = value;
        //            OnPropertyChanged("ParentAccountCode");
        //        }
        //    }

        //    public int? ReportScheduleId
        //    {
        //        get { return _reportScheduleId; }
        //        set
        //        {
        //            _reportScheduleId = value;
        //            OnPropertyChanged("ReportScheduleId");
        //        }
        //    }

        //    #endregion

        //    #region --- CRUD ---

        //    private const string TABLE_NAME = "AccountOfAccounts";

        //    private List<SqlParameter> Parameters
        //    {
        //        get
        //        {
        //            // DO NOT INCLUDE KEY !!!
        //            var sqlParameters = new List<SqlParameter>();
        //            ModelController.AddParameter(sqlParameters, "?AccountCode", AccountCode);
        //            ModelController.AddParameter(sqlParameters, "?AccountTitle", AccountTitle);
        //            ModelController.AddParameter(sqlParameters, "?Nature", Nature);
        //            ModelController.AddParameter(sqlParameters, "?AccountType", AccountType);
        //            ModelController.AddParameter(sqlParameters, "?Category", Category);
        //            ModelController.AddParameter(sqlParameters, "?AccountLevel", AccountLevel);
        //            ModelController.AddParameter(sqlParameters, "?ParentAccountCode", ParentAccountCode);
        //            ModelController.AddParameter(sqlParameters, "?ReportScheduleId", ReportScheduleId);
        //            ModelController.AddParameter(sqlParameters, "?IsClose", IsClose);
        //            ModelController.AddParameter(sqlParameters, "?IsSubsidiaryLedger", IsSubsidiaryLedger);
        //            ModelController.AddParameter(sqlParameters, "?IsCashFlow", IsCashFlow);
        //            return sqlParameters;
        //        }
        //    }

        //    private SqlParameter ParamKey
        //    {
        //        get { return new SqlParameter("?AccountId", AccountId); }
        //    }

        //    public Result Create()
        //    {
        //        Action createRecord = () =>
        //                                  {
        //                                      List<SqlParameter> sqlParameter = Parameters;

        //                                      string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
        //                                                                                              sqlParameter);
        //                                      DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
        //                                  };

        //        return ActionController.InvokeAction(createRecord);
        //    }

        //    public Result Destroy()
        //    {
        //        Action deleteRecord = () =>
        //                                  {
        //                                      SqlParameter key = ParamKey;

        //                                      string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

        //                                      DatabaseController.ExecuteNonQuery(sql, key);
        //                                  };

        //        return ActionController.InvokeAction(deleteRecord);
        //    }

        //    public Result Find(int id)
        //    {
        //        Action findRecord = () =>
        //                                {
        //                                    ResetProperties();
        //                                    AccountId = id;

        //                                    SqlParameter key = ParamKey;
        //                                    string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

        //                                    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
        //                                    foreach (DataRow dataRow in dataTable.Rows)
        //                                    {
        //                                        SetPropertiesFromDataRow(dataRow);
        //                                    }
        //                                };

        //        return ActionController.InvokeAction(findRecord);
        //    }

        //    public void ResetProperties()
        //    {
        //        AccountId = 0;
        //        AccountCode = string.Empty;
        //        AccountTitle = string.Empty;
        //        Category = string.Empty;
        //        IsCashFlow = false;
        //        IsClose = false;
        //        IsSubsidiaryLedger = false;
        //        Nature = string.Empty;
        //        ReportScheduleId = 0;
        //        ParentAccountCode = string.Empty;
        //        AccountType = string.Empty;
        //        AccountLevel = 0;
        //    }

        //    public void SetPropertiesFromDataRow(DataRow dataRow)
        //    {
        //        if (dataRow["AccountCode"] != DBNull.Value)
        //        {
        //            AccountCode = (string) dataRow["AccountCode"];
        //        }
        //        if (dataRow["AccountTitle"] != DBNull.Value)
        //        {
        //            AccountTitle = (string) dataRow["AccountTitle"];
        //        }
        //        if (dataRow["Nature"] != DBNull.Value)
        //        {
        //            Nature = (string) dataRow["Nature"];
        //        }
        //        if (dataRow["AccountType"] != DBNull.Value)
        //        {
        //            AccountType = (string) dataRow["AccountType"];
        //        }
        //        if (dataRow["Category"] != DBNull.Value)
        //        {
        //            Category = (string) dataRow["Category"];
        //        }
        //        if (dataRow["AccountLevel"] != DBNull.Value)
        //        {
        //            AccountLevel = Convert.ToInt32(dataRow["AccountLevel"]);
        //        }
        //        if (dataRow["ParentAccountCode"] != DBNull.Value)
        //        {
        //            ParentAccountCode = (string) dataRow["ParentAccountCode"];
        //        }
        //        if (dataRow["ReportScheduleId"] != DBNull.Value)
        //        {
        //            ReportScheduleId = Convert.ToInt32(dataRow["ReportScheduleId"]);
        //        }
        //        if (dataRow["IsClose"] != DBNull.Value)
        //        {
        //            IsClose = Convert.ToBoolean(dataRow["IsClose"]);
        //        }
        //        if (dataRow["IsSubsidiaryLedger"] != DBNull.Value)
        //        {
        //            IsSubsidiaryLedger = Convert.ToBoolean(dataRow["IsSubsidiaryLedger"]);
        //        }
        //        if (dataRow["IsCashFlow"] != DBNull.Value)
        //        {
        //            IsCashFlow = Convert.ToBoolean(dataRow["IsCashFlow"]);
        //        }
        //    }

        //    public Result Update()
        //    {
        //        Action updateRecord = () =>
        //                                  {
        //                                      SqlParameter key = ParamKey;

        //                                      List<SqlParameter> sqlParameter = Parameters;
        //                                      sqlParameter.Add(key);

        //                                      string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
        //                                                                                              sqlParameter,
        //                                                                                              key);

        //                                      DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
        //                                  };

        //        return ActionController.InvokeAction(updateRecord);
        //    }

        //    #endregion

        //    #region --- STATIC METHODS ---

        //    public static Account GetByCode(string accountCode)
        //    {
        //        string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountCode = '{1}'", TABLE_NAME,
        //                                              accountCode);

        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);

        //        var result = new Account();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            result.SetPropertiesFromDataRow(dataRow);
        //        }
        //        return result;
        //    }

        //    public static List<Account> GetList()
        //    {
        //        string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        var result = new List<Account>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            var item = new Account();
        //            item.SetPropertiesFromDataRow(dataRow);
        //            result.Add(item);
        //        }
        //        return result;
        //    }

        //    public static List<Account> GetList(string loanAccountCode)
        //    {
        //        string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountCode LIKE '{1}%'", TABLE_NAME,
        //                                              loanAccountCode);
        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        var result = new List<Account>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            var item = new Account();
        //            item.SetPropertiesFromDataRow(dataRow);
        //            result.Add(item);
        //        }
        //        return result;
        //    }

        //    public static List<Account> GetListByReportScheduleId(int reportScheduleId)
        //    {
        //        string sqlCommandText = string.Format(
        //            "SELECT * FROM {0} WHERE ReportScheduleId = ?ReportScheduleId", TABLE_NAME);
        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
        //                                                                    new SqlParameter("?ReportScheduleId",
        //                                                                                     reportScheduleId));
        //        var result = new List<Account>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            var item = new Account();
        //            item.SetPropertiesFromDataRow(dataRow);
        //            result.Add(item);
        //        }
        //        return result;
        //    }

        //    public static List<Account> GetListGeneralAccount()
        //    {
        //        string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountType = 'General'", TABLE_NAME);
        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        var result = new List<Account>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            var item = new Account();
        //            item.SetPropertiesFromDataRow(dataRow);
        //            result.Add(item);
        //        }
        //        return result;
        //    }

        //    public static List<Account> GetLoanAccounts()
        //    {
        //        string sqlCommandText = string.Format("SELECT * FROM `{0}` WHERE `AccountCode` LIKE '%{1}%'", TABLE_NAME,
        //                                              GlobalSettings.LoanAccountCode);
        //        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
        //        var result = new List<Account>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            var item = new Account();
        //            item.SetPropertiesFromDataRow(dataRow);
        //            result.Add(item);
        //        }
        //        return result;
        //    }

        //    #endregion

        //    public event PropertyChangedEventHandler PropertyChanged;

        //    protected virtual void OnPropertyChanged(string propertyName)
        //    {
        //        PropertyChangedEventHandler handler = PropertyChanged;
        //        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        #endregion

        //internal static Account GetByCode(string accountCode)
        //{
        //    var sqlBuilder = new System.Text.StringBuilder();
        //    sqlBuilder.AppendLine("SELECT * FROM");
        //    sqlBuilder.AppendLine(TABLE_NAME);
        //    sqlBuilder.AppendLine("WHERE CODE = ?CODE");

        //    var parameter = new SqlParameter("?CODE", accountCode);

        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, parameter);

        //    var result = new Account();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        result.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return result;
        //}

        public static List<Account> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            var result = new List<Account>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                result.Add(item);
            }
            return result;
        }

        public static List<Account> GetList(string loanAccountCode)
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountCode LIKE '{1}%'", TABLE_NAME,
                                                  loanAccountCode);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            var result = new List<Account>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                result.Add(item);
            }
            return result;
        }

        public static List<Account> GetListByReportScheduleId(int reportScheduleId)
        {
            string sqlCommandText = string.Format(
                "SELECT * FROM {0} WHERE ReportScheduleId = ?ReportScheduleId", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?ReportScheduleId",
                                                                                         reportScheduleId));
            var result = new List<Account>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                result.Add(item);
            }
            return result;
        }

        public static List<Account> GetListGeneralAccount()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE AccountType = 'General'", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            var result = new List<Account>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                result.Add(item);
            }
            return result;
        }

        public static List<Account> GetLoanAccounts()
        {
            string sqlCommandText = string.Format("SELECT * FROM `{0}` WHERE `AccountCode` LIKE '%{1}%'", TABLE_NAME,
                                                  GlobalSettings.CodeOfLoanReceivables);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            var result = new List<Account>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                result.Add(item);
            }
            return result;
        }

        //public static Account FindByCode(string accountCode)
        //{
        //    var sqlBuilder = new System.Text.StringBuilder();
        //    sqlBuilder.AppendLine("SELECT * FROM");
        //    sqlBuilder.AppendLine(TABLE_NAME);
        //    sqlBuilder.AppendLine("WHERE CODE = ?CODE");

        //    var parameter = new SqlParameter("?CODE", accountCode);

        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, parameter);
        //    if (dataTable.Rows.Count == 0) return null;

        //    var result = new Account();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        result.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return result;
        //}
        //public static Account FindByName(string accountTitle)
        //{
        //    var sqlBuilder = new System.Text.StringBuilder();
        //    sqlBuilder.AppendLine("SELECT * FROM");
        //    sqlBuilder.AppendLine(TABLE_NAME);
        //    sqlBuilder.AppendLine("WHERE TITLE = ?TITLE");

        //    var parameter = new SqlParameter("?TITLE", accountTitle);

        //    DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, parameter);

        //    if (dataTable.Rows.Count == 0) return null;

        //    var result = new Account();
        //    foreach (DataRow dataRow in dataTable.Rows)
        //    {
        //        result.SetPropertiesFromDataRow(dataRow);
        //    }
        //    return result;
        //}

        internal static AccountCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new AccountCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", AccountCode, AccountTitle);
        }

        internal static AccountCollection Where(Dictionary<string, object> dictionary)
        {
            var conditions = dictionary.Select(item => string.Format("?{0} = {0}", item.Key)).ToList();

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(TABLE_NAME);
            queryBuilder.AppendLine("WHERE");
            queryBuilder.AppendLine(string.Join(" AND ", conditions.ToArray()));

            var parameters = dictionary.Select(item => new SqlParameter(string.Format("?{0}", item.Key), item.Value)).ToList();

            var dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), parameters.ToArray());
            var collection = new AccountCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        internal static AccountCollection Where(string columnName, object value)
        {
            var condition = new Dictionary<string, object>();
            condition.Add(columnName, value);
            return Where(condition);
        }
    }


    public class AccountCollection : ObservableCollection<Account>
    { }

    public static class Accounts
    {

        internal static AccountCollection Where(Dictionary<string, object> dictionary)
        {
            var conditions = dictionary.Select(item => string.Format("?{0} = {0}", item.Key)).ToList();

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(Account.TableName);
            queryBuilder.AppendLine("WHERE");
            queryBuilder.AppendLine(string.Join(" AND ", conditions.ToArray()));

            var parameters = dictionary.Select(item => new SqlParameter(string.Format("?{0}", item.Key), item.Value)).ToList();

            var dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(), parameters.ToArray());
            var collection = new AccountCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Account();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }
}