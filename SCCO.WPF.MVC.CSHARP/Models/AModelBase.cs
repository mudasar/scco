using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public abstract class AModelBase : INotifyPropertyChanged
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        private readonly string _tableName;

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?id", _id); }
        }

        protected AModelBase(string tableName)
        {
            _tableName = tableName;
        }

        public bool IsNewRecord()
        {
            return _id == 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }



        #region Implementation of IModel


        protected List<SqlParameter> Parameters { get; set; }

        protected virtual CrudResult VirtualCreate()
        {
            Action createRecord = () =>
                {
                    var sql = DatabaseController.GenerateInsertStatement(_tableName, Parameters);
                    ID = DatabaseController.ExecuteInsertQuery(sql, Parameters.ToArray());
                };

            return InvokeAction(createRecord);
        }

        protected virtual CrudResult VirtualUpdate()
        {
            Action updateRecord = () =>
                {
                    var key = ParamKey;

                    List<SqlParameter> sqlParameters = Parameters;
                    string sql = DatabaseController.GenerateUpdateStatement(_tableName,
                                                                            sqlParameters, key);

                    sqlParameters.Add(key);
                    DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
                };

            return InvokeAction(updateRecord);
        }

        protected virtual CrudResult VirtualDestroy()
        {
            Action deleteRecord = () =>
                {
                    var key = ParamKey;

                    string sql = DatabaseController.GenerateDeleteStatement(_tableName, key);

                    DatabaseController.ExecuteNonQuery(sql, key);
                };

            return InvokeAction(deleteRecord);
        }

        protected virtual DataRow VirtualFind(int id)
        {
            var key = new SqlParameter("?id", id);
            string sql = DatabaseController.GenerateSelectStatement(_tableName, key);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);

            return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
        }

        #endregion

        private static CrudResult InvokeAction(Action action)
        {
            try
            {
                action.Invoke();
                return new CrudResult(true, "Successful: " + action.Method);
            }
            catch (Exception exception)
            {
                Utilities.Logger.ExceptionLogger(action, exception);
                return new CrudResult(false, exception.Message);
            }
        }
    }

    public struct CrudResult
    {
        public bool Success;
        public string Message;

        public CrudResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}

