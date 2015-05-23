using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    class VoucherLog : ModelBase, IModel
    {
        private int _number;
        private DateTime _date;
        private string _type;
        private string _initials;
        private string _remarks;

        public VoucherLog()
            : base("voucher_logs")
        {
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; OnPropertyChanged("Remarks"); }
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged("Number"); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("Date"); }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        public string Initials
        {
            get { return _initials; }
            set { _initials = value; OnPropertyChanged("Initials"); }
        }

        #region Implementation of IModel

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?DOC_NUM", Number);
                ModelController.AddParameter(sqlParameters, "?DOC_DATE", Date);
                ModelController.AddParameter(sqlParameters, "?DOC_TYPE", Type);
                ModelController.AddParameter(sqlParameters, "?INITIALS", Initials);
                ModelController.AddParameter(sqlParameters, "?REMARKS", Remarks);
                return sqlParameters;
            }
        }


        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var sqlParameter = Parameters;
                                          var sql = DatabaseController.GenerateInsertStatement(_tableName, sqlParameter);

                                          ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          var key = _paramKey;
                                          var sqlParameters = Parameters;
                                          var sql = DatabaseController.GenerateUpdateStatement(_tableName, Parameters,
                                                                                               _paramKey);
                                          sqlParameters.Add(key);
                                          DatabaseController.ExecuteNonQuery(sql, sqlParameters.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          var key = _paramKey;
                                          var sql = DatabaseController.GenerateDeleteStatement(_tableName, key);
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
                var sql = DatabaseController.GenerateSelectStatement(_tableName, key);

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
            Number = 0;
            Date = new DateTime();
            Type = "";
            Initials = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = Utilities.DataConverter.ToInteger(dataRow["ID"]);
            Number = Utilities.DataConverter.ToInteger(dataRow["DOC_NUM"]);
            Date = Utilities.DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            Type = Utilities.DataConverter.ToString(dataRow["DOC_TYPE"]);
            Initials = Utilities.DataConverter.ToString(dataRow["INITIALS"]);
            Remarks = Utilities.DataConverter.ToString(dataRow["REMARKS"]);
        }

        #endregion

        #endregion


        public void Find(string type, int number)
        {
            ResetProperties();
            
            Type = type;
            Number = number;

            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM");
            sqlBuilder.AppendLine(_tableName);
            sqlBuilder.AppendLine("WHERE DOC_TYPE = ?DOC_TYPE");
            sqlBuilder.AppendLine("AND DOC_NUM = ?DOC_NUM");

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("?DOC_TYPE", type));
            parameters.Add(new SqlParameter("?DOC_NUM", number));

            var dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder.ToString(), parameters.ToArray());

            foreach (DataRow dataRow in dataTable.Rows)
            {
                SetPropertiesFromDataRow(dataRow);
            }
        }

        public Result Save()
        {
           if(ID == 0)
           {
               return Create();
           }
            return Update();
        }
    }
}
