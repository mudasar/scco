using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Branch : INotifyPropertyChanged, IModel
    {
        #region --- PROPERTIES ---

        private int _id;

        private string _branchName;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }
        public string BranchName
        {
            get { return _branchName; }
            set
            {
                _branchName = value;
                OnPropertyChanged("BranchName");
            }
        }

        #endregion

        #region --- CRUD ---

        private const string TABLE_NAME = "Branches";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?BranchName", BranchName);
                return sqlParameters;
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
                                          var sqlParameters = Parameters;
                                          var sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameters);
                                          ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameters.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          var key = ParamKey;

                                          var sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

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
                                        var sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

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
            BranchName = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int)dataRow["ID"];
            BranchName = (string)dataRow["BranchName"];
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          var key = ParamKey;

                                          var sqlParameter = Parameters;
                                          sqlParameter.Add(key);

                                          var sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME, sqlParameter,
                                                                                               key);

                                          DatabaseController.ExecuteNonQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }
        #endregion

        #region --- STATIC METHODS ---

        public static List<Branch> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new Branch
                               {
                                   ID = (int) row["ID"],
                                   BranchName = (string) row["BranchName"],
                               }).ToList();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static Branch FindByName(string branchName)
        {
            Branch branch = null;
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM ");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE BranchName = ?BranchName");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParameter = new SqlParameter("?BranchName", branchName);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            if (dataTable.Rows.Count > 0)
            {
                branch = new Branch();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    branch.SetPropertiesFromDataRow(dataRow);
                }
            }
            return branch;
        }

        internal static BranchCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new BranchCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Branch();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }

    public class BranchCollection : ObservableCollection<Branch>
    {
    }
}
