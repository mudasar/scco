using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class MembershipType : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _description;

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

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        #endregion

        #region --- CRUD ---

        private const string TABLE_NAME = "membership_types";

        private List<SqlParameter> Parameters
        {
            get
            {
                // DO NOT INCLUDE KEY !!!
                var sqlParameters = new List<SqlParameter>();
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
            Description = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int) dataRow["ID"];
            Description = (string) dataRow["Description"];
        }

        #endregion

        #region --- STATIC METHODS ---

        public static List<MembershipType> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new MembershipType
                               {
                                   ID = (int) row["ID"],
                                   Description = (string) row["Description"],
                               }).ToList();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static MembershipType FindByName(string description)
        {
            MembershipType area = null;
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM ");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE Description = ?Description");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParameter = new SqlParameter("?Description", description);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            if (dataTable.Rows.Count > 0)
            {
                area = new MembershipType();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    area.SetPropertiesFromDataRow(dataRow);
                }
            }
            return area;
        }

        internal static MembershipTypeCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new MembershipTypeCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new MembershipType();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }
    public class MembershipTypeCollection : System.Collections.ObjectModel.ObservableCollection<MembershipType>
    {
    }
}
