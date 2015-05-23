using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class GlobalVariable : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _keyword;
        private string _currentValue;
        private string _description;

        #region --- PROPERTIES ---

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; OnPropertyChanged("Keyword"); }
        }

        public string CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; OnPropertyChanged("CurrentValue"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        #endregion --- PROPERTIES ---

        #region --- CRUD ---

        private const string TABLE_NAME = "global_variables";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE ID !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?Keyword", Keyword);
                ModelController.AddParameter(sqlParameters, "?CurrentValue", CurrentValue);
                ModelController.AddParameter(sqlParameters, "?Description", Description);
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
                var sqlParameter = Parameters;

                var sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            if (ID == 0) return Create();

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
            Keyword = string.Empty;
            CurrentValue = string.Empty;
            Description = string.Empty;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            Keyword = DataConverter.ToString(dataRow["Keyword"]);
            CurrentValue = DataConverter.ToString(dataRow["CurrentValue"]);
            Description = DataConverter.ToString(dataRow["Description"]);
        }

        #endregion --- CRUD ---

        public static GlobalVariable FindByKeyword(string keyword)
        {
            var queryBuilder = new System.Text.StringBuilder();
            queryBuilder.AppendLine("SELECT * FROM");
            queryBuilder.AppendLine(TABLE_NAME);
            queryBuilder.AppendLine("WHERE Keyword = ?Keyword LIMIT 1");

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(queryBuilder, new SqlParameter("?Keyword", keyword));

            var globalVariable = new GlobalVariable();
            globalVariable.Keyword = keyword;
            globalVariable.CurrentValue = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                globalVariable.SetPropertiesFromDataRow(dataRow);
            }

            return globalVariable;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static GlobalVariable FindByKeyword(GlobalKeys key)
        {
            var keyword = Enum.GetName(typeof(GlobalKeys), key);
            return FindByKeyword(keyword);
        }
    }
}