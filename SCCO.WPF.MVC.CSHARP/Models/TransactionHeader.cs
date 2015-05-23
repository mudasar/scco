using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class TransactionHeader : INotifyPropertyChanged, IModel
    {
        #region --- PROPERTIES ---

        private decimal _amount;

        private int _collectorId;

        private DateTime? _dateModified;

        private string _explanation;

        private bool _isCancelled;

        private bool _isPosted;

        private int _transactionHeaderId;

        private int _userId;

        private DateTime? _voucherDate;

        private int _voucherNumber;

        private string _voucherType;

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public string AmountInWords
        {
            get { return Converter.AmountToWords(Amount); }
        }

        public int CollectorId
        {
            get { return _collectorId; }
            set
            {
                _collectorId = value;
                OnPropertyChanged("CollectorId");
            }
        }

        public DateTime? DateModified
        {
            get { return _dateModified; }
            set
            {
                _dateModified = value;
                OnPropertyChanged("DateModified");
            }
        }

        public string Explanation
        {
            get { return _explanation; }
            set
            {
                _explanation = value;
                OnPropertyChanged("Explanation");
            }
        }

        public bool IsCancelled
        {
            get { return _isCancelled; }
            set
            {
                _isCancelled = value;
                OnPropertyChanged("IsCancelled");
            }
        }

        public bool IsPosted
        {
            get { return _isPosted; }
            set { _isPosted = value; OnPropertyChanged("IsPosted"); }
        }

        public int TransactionHeaderId
        {
            get { return _transactionHeaderId; }
            set { _transactionHeaderId = value; OnPropertyChanged("TransactionHeaderId"); }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged("UserId"); }
        }

        public DateTime? VoucherDate
        {
            get { return _voucherDate; }
            set { _voucherDate = value; OnPropertyChanged("VoucherDate"); }
        }

        public int VoucherNumber
        {
            get { return _voucherNumber; }
            set { _voucherNumber = value; OnPropertyChanged("VoucherNumber"); }
        }

        public string VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; OnPropertyChanged("VoucherType"); }
        }

        #endregion --- PROPERTIES ---

        #region --- CRUD ---

        private const string TableName = "TransactionHeaders";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var list = new List<SqlParameter>
                               {
                                   new SqlParameter("?VoucherDate", VoucherDate),
                                   new SqlParameter("?VoucherType", VoucherType),
                                   new SqlParameter("?VoucherNumber", VoucherNumber),
                                   new SqlParameter("?IsCancelled", IsCancelled),
                                   new SqlParameter("?IsPosted", IsPosted),
                                   new SqlParameter("?UserId", UserId),
                                   new SqlParameter("?CollectorId", CollectorId),
                                   new SqlParameter("?Explanation", Explanation),
                                   new SqlParameter("?Amount", Amount)
                               };
                return list;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?TransactionHeaderId", TransactionHeaderId); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          List<SqlParameter> sqlParameter = Parameters;

                                          string sql = DatabaseController.GenerateInsertStatement(TableName,
                                                                                                  sqlParameter);
                                          TransactionHeaderId = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
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
                                        TransactionHeaderId = id;

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
            TransactionHeaderId = new int();
            VoucherDate = new DateTime();
            VoucherType = new string(' ', 0);
            VoucherNumber = new int();
            IsCancelled = new bool();
            IsPosted = new bool();
            UserId = new int();
            DateModified = new DateTime();
            CollectorId = new int();
            Explanation = new string(' ', 0);
            Amount = new decimal();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            TransactionHeaderId = (int)dataRow["TransactionHeaderId"];
            VoucherDate = (DateTime?)dataRow["VoucherDate"];
            VoucherType = (string)dataRow["VoucherType"];
            VoucherNumber = Convert.ToInt32(dataRow["VoucherNumber"]);
            IsCancelled = Convert.ToBoolean(dataRow["IsCancelled"]);
            IsPosted = Convert.ToBoolean(dataRow["IsPosted"]);
            UserId = Convert.ToInt32(dataRow["UserId"]);
            DateModified = Convert.ToDateTime(dataRow["DateModified"]);
            CollectorId = Convert.ToInt32(dataRow["CollectorId"]);
            Explanation = Convert.ToString(dataRow["Explanation"]);
            Amount = Convert.ToDecimal(dataRow["Amount"]);
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

        #endregion --- CRUD ---

        public event PropertyChangedEventHandler PropertyChanged;

        public static TransactionHeader GetFirstTransactionHeaderByVoucherType(VoucherTypes voucherType)
        {
            string sqlCommandText =
                string.Format(
                    "SELECT * FROM {0} WHERE  VoucherType = ?VoucherType ORDER BY VoucherNumber ASC LIMIT 1",
                    TableName);

            var transactionHeader = new TransactionHeader();
            var sqlParameter = new List<SqlParameter>
                                   {
                                       new SqlParameter("?VoucherType", voucherType),
                                   };
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, sqlParameter.ToArray());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                transactionHeader.SetPropertiesFromDataRow(dataRow);
            }
            return transactionHeader;
        }

        public static int GetFirstVoucherNumber(VoucherTypes voucherType)
        {
            string sqlCommandText = string.Format(
                "SELECT IFNULL(MIN(VoucherNumber),0) as VoucherNumber FROM {0} WHERE VoucherType = '{1}'", TableName,
                voucherType);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return Convert.ToInt32(dataTable.Rows[0]["VoucherNumber"]);
        }

        public static TransactionHeader GetLastTransactionHeaderByVoucherType(VoucherTypes voucherType)
        {
            string sqlCommandText =
                string.Format(
                    "SELECT * FROM {0} WHERE  VoucherType = ?VoucherType ORDER BY VoucherNumber DESC LIMIT 1",
                    TableName);

            var transactionHeader = new TransactionHeader();
            var sqlParameter = new List<SqlParameter>
                                   {
                                       new SqlParameter("?VoucherType", voucherType),
                                   };
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, sqlParameter.ToArray());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                transactionHeader.SetPropertiesFromDataRow(dataRow);
            }
            return transactionHeader;
        }

        public static int GetLastVoucherNumber(VoucherTypes voucherType)
        {
            string sqlCommandText = string.Format(
                "SELECT IFNULL(MAX(VoucherNumber),0) as VoucherNumber FROM {0} WHERE VoucherType = '{1}'", TableName,
                voucherType);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return Convert.ToInt32(dataTable.Rows[0]["VoucherNumber"]);
        }

        public static TransactionHeader GetTransactionHeader(VoucherTypes voucherType, int voucherNumber)
        {
            string sqlCommandText =
                string.Format(
                    "SELECT * FROM {0} WHERE  VoucherType = ?VoucherType AND VoucherNumber = ?VoucherNumber LIMIT 1",
                    TableName);

            var transactionHeader = new TransactionHeader();
            var sqlParameter = new List<SqlParameter>
                                   {
                                       new SqlParameter("?VoucherType", voucherType),
                                       new SqlParameter("?VoucherNumber", voucherNumber),
                                   };
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, sqlParameter.ToArray());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                transactionHeader.SetPropertiesFromDataRow(dataRow);
            }
            return transactionHeader;
        }

        public static bool IsTransactionHeaderExist(VoucherTypes voucherType, int voucherNumber)
        {
            string sqlCommandText =
                string.Format(
                    "SELECT TransactionHeaderId FROM {0} WHERE  VoucherType = ?VoucherType AND VoucherNumber = ?VoucherNumber LIMIT 1",
                    TableName);
            var sqlParameter = new List<SqlParameter>
                                   {
                                       new SqlParameter("?VoucherType", voucherType),
                                       new SqlParameter("?VoucherNumber", voucherNumber),
                                   };
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText, sqlParameter.ToArray());
            return dataTable.Rows.Count != 0;
        }

        public Result CancelTransactionHeader()
        {
            // must delete first to remove any references from transaction details
            Result result = Destroy();

            if (!result.Success)
                return result;

            IsCancelled = true;
            result = Create();

            if (!result.Success)
                return result;

            var transactionDetail = new TransactionDetail();
            transactionDetail.TransactionHeaderId = TransactionHeaderId;
            transactionDetail.MemberCode = "CANCELLED";
            transactionDetail.MemberName = "CANCELLED";
            transactionDetail.AccountCode = "CANCELLED";
            transactionDetail.AccountTitle = "CANCELLED";

            result = transactionDetail.Create();

            return result;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}