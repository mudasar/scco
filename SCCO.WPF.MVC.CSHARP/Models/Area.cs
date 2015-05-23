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
    public class Area : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _areaName;
        private const string TABLE_NAME = "Areas";

        #region --- PROPERTIES ---

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string AreaName
        {
            get { return _areaName; }
            set { _areaName = value; OnPropertyChanged("AreaName"); }
        }

        #endregion

        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
            {
                var sqlParameter = new List<SqlParameter>();
                sqlParameter.Add(new SqlParameter("?AreaName", AreaName));

                var sql = DatabaseController.GenerateInsertStatement(TABLE_NAME, sqlParameter);
                ID = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?AreaName", AreaName);

                var sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME, sqlParameters,key);
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

                var key = new SqlParameter("?ID", ID);
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
            AreaName = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int)dataRow["ID"];
            AreaName = (string)dataRow["AreaName"];
        }

        #endregion

        #region --- STATIC METHODS ---

        public static List<Area> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new Area
                               {
                                   ID = (int) row["ID"],
                                   AreaName = (string) row["AreaName"],
                               }).ToList();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static Area FindByName(string areaName)
        {
            Area area = null;
            var sqlBuilder = new System.Text.StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM ");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE AreaName = ?AreaName");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParameter = new SqlParameter("?AreaName", areaName);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            if (dataTable.Rows.Count > 0)
            {
                area = new Area();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    area.SetPropertiesFromDataRow(dataRow);
                }
            }
            return area;
        }

        internal static AreaCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new AreaCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Area();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }

    public class AreaCollection : ObservableCollection<Area>
    {
    }
}
