using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class GlobalVariable : INotifyPropertyChanged, IModel
    {
        private string _currentValue;
        private string _description;
        private int _id;
        private string _keyword;

        #region --- PROPERTIES ---

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("ID");
            }
        }

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                _keyword = value;
                OnPropertyChanged("Keyword");
            }
        }

        public string CurrentValue
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
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
                    ID = DatabaseController.CreateRecord(TABLE_NAME, Parameters);
                };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            if (ID == 0) return Create();

            Action updateRecord = () => DatabaseController.UpdateRecord(TABLE_NAME, ParamKey, Parameters);
            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () => DatabaseController.DeleteRecord(TABLE_NAME, ID);
            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                {
                    ResetProperties();
                    ID = id;
                    DataTable dataTable = DatabaseController.FindRecord(TABLE_NAME, id);
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

        public event PropertyChangedEventHandler PropertyChanged;

        public static GlobalVariable FindByKeyword(string keyword)
        {
            var paramKey = new SqlParameter("?Keyword", keyword);
            var sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, paramKey);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, paramKey);

            var globalVariable = new GlobalVariable {Keyword = keyword, CurrentValue = string.Empty};
            foreach (DataRow dataRow in dataTable.Rows)
            {
                globalVariable.SetPropertiesFromDataRow(dataRow);
            }

            return globalVariable;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static GlobalVariable FindByKeyword(GlobalKeys key)
        {
            string keyword = Enum.GetName(typeof (GlobalKeys), key);
            return FindByKeyword(keyword);
        }
    }
}