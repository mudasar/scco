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
    public class Department : INotifyPropertyChanged, IModel
    {
        private int _id;
        private string _departmentName;

        #region --- PROPERTIES ---

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        public string DepartmentName
        {
            get { return _departmentName; }
            set { _departmentName = value; OnPropertyChanged("DepartmentName"); }
        }

        #endregion

        private const string TABLE_NAME = "Departments";

        #region --- CRUD ---

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var sqlParameter = new List<SqlParameter>();
                                          sqlParameter.Add(new SqlParameter("?DepartmentName", DepartmentName));

                                          string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                                                  sqlParameter);

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
                                          sqlParameter.Add(new SqlParameter("?DepartmentName", DepartmentName));

                                          string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                                                  sqlParameter,
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

                                          string sql = DatabaseController.GenerateDeleteStatement(TABLE_NAME, key);

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
                                        string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

                                        DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql, key);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            SetPropertiesFromDataRow(dataRow);
                                        }
                                    };

            return ActionController.InvokeAction(findRecord);
        }

        #endregion

        public void ResetProperties()
        {
            ID = 0;
            DepartmentName = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = (int) dataRow["ID"];
            DepartmentName = (string) dataRow["DepartmentName"];
        }

        #region --- STATIC METHODS ---

        public static List<Department> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new Department
                               {
                                   ID = (int) row["ID"],
                                   DepartmentName = (string) row["DepartmentName"],
                               }).ToList();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static Department FindByName(string departmentName)
        {
            Department department = null;
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("SELECT * FROM ");
            sqlBuilder.AppendLine(TABLE_NAME);
            sqlBuilder.AppendLine("WHERE DepartmentName = ?DepartmentName");
            sqlBuilder.AppendLine("LIMIT 1");
            var sqlParameter = new SqlParameter("?DepartmentName", departmentName);

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlBuilder, sqlParameter);
            if (dataTable.Rows.Count > 0)
            {
                department = new Department();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    department.SetPropertiesFromDataRow(dataRow);
                }
            }
            return department;
        }

        internal static DepartmentCollection CollectAll()
        {
            var dataTable = DatabaseController.ExecuteSelectQuery("SELECT * FROM " + TABLE_NAME);
            var collection = new DepartmentCollection();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new Department();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }
    }
    public class DepartmentCollection : System.Collections.ObjectModel.ObservableCollection<Department>
    {
    }
}
