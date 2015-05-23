using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class AccessLevel : INotifyPropertyChanged, IModel
    {
        #region ---CONSTRUCTOR---

        //public AccessLevel()
        //{
        //}

        //public AccessLevel(int accessLevelId, int userId, string moduleCode)
        //{
        //    AccessLevelId = accessLevelId;
        //    UserId = userId;
        //    ModuleCode = moduleCode;
        //}

        #endregion

        #region --- PROPERTIES ---

        private int _accessLevelId;
        private string _moduleCode;
        private int _userId;

        public int AccessLevelId
        {
            get { return _accessLevelId; }
            set
            {
                _accessLevelId = value;
                OnPropertyChanged("AccessLevelId");
            }
        }

        public string ModuleCode
        {
            get { return _moduleCode; }
            set
            {
                _moduleCode = value;
                OnPropertyChanged("ModuleCode");
            }
        }

        public int UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }

        #endregion

        #region --- METHODS & EVENTS ---

        #region IModel Members

        private const string TABLE_NAME = "AccessLevel";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?ModuleCode", ModuleCode);
                ModelController.AddParameter(sqlParameters, "?UserId", UserId);
                return sqlParameters;
            }
        }

        private SqlParameter ParamKey
        {
            get { return new SqlParameter("?AccessLevelId", AccessLevelId); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var sqlParameter = Parameters;

                                          var sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
                                          DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
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
                                        AccessLevelId = id;

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

        public static List<AccessLevel> GetAccessLevelByUserId(int userId)
        {
            string sqlCommandText = string.Format("SELECT * FROM {0} WHERE UserId = ?UserId", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?UserId", userId));
            return (from DataRow row in dataTable.Rows
                    select new AccessLevel
                               {
                                   AccessLevelId = (int) row["AccessLevelId"],
                                   ModuleCode = (string) row["ModuleCode"],
                                   UserId = (int) row["UserId"]
                               }
                   ).ToList();
        }

        public void ResetProperties()
        {
            AccessLevelId = 0;
            UserId = 0;
            ModuleCode = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            AccessLevelId = (int) dataRow["AccessLevelId"];
            UserId = (int) dataRow["UserId"];
            ModuleCode = (string) dataRow["ModuleCode"];
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
