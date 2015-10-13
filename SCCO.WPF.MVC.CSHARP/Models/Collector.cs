using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Collector : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _collectorName;
        private int _userId;

        #region --- PROPERTIES ---

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string CollectorName
        {
            get { return _collectorName; }
            set { _collectorName = value; OnPropertyChanged("CollectorName"); }
        }

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged("UserId"); }
        }

        #endregion  --- PROPERTIES ---

        private const string TABLE_NAME = "Collectors";

        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
                {
                    var sqlParameter = new List<SqlParameter>();
                    sqlParameter.Add(new SqlParameter("?CollectorName", CollectorName));

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

                var sqlParameter = new List<SqlParameter>();
                sqlParameter.Add(key);
                sqlParameter.Add(new SqlParameter("?CollectorName", CollectorName));

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
            CollectorName = "";
            UserId = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = Utilities.DataConverter.ToInteger(dataRow["ID"]);
            CollectorName = Utilities.DataConverter.ToString(dataRow["CollectorName"]);
            UserId = Utilities.DataConverter.ToInteger(dataRow["UserId"]);
        }

        #endregion

        #region --- STATIC METHODS ---

        public static List<Collector> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new Collector
                               {
                                   ID = Convert.ToInt32(row["ID"]),
                                   CollectorName = Convert.ToString(row["CollectorName"])
                               }).ToList();
        }

        public static Collector GetCollectorByUserId(int userId)
        {
            var collector = new Collector();
            string sqlCommandText = string.Format("SELECT ID FROM {0} WHERE UserId = ?UserId LIMIT 1",
                                                  TABLE_NAME);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                        new SqlParameter("?UserId", userId));
            foreach (DataRow dataRow in dataTable.Rows)
            {
                collector.SetPropertiesFromDataRow(dataRow);
            }
            
            return collector;
        }

        internal static Collector FindByName(string collectorName)
        {
            Collector collector = null;
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM ");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE CollectorName = ?CollectorName");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParameter = new SqlParameter("?CollectorName", collectorName);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            if (dataTable.Rows.Count > 0)
            {
                collector = new Collector();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    collector.SetPropertiesFromDataRow(dataRow);
                }
            }
            return collector;
        }

        #endregion

        public override string ToString()
        {
            return CollectorName;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        internal static CollectorCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new CollectorCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Collector();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }


    }
    public class CollectorCollection : System.Collections.ObjectModel.ObservableCollection<Collector>
    {
        internal static CollectorCollection SortedByName()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM Collectors ORDER BY CollectorName");
            var collection = new CollectorCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Collector();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }
}
