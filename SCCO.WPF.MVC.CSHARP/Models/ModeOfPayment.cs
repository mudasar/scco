using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class ModeOfPayment : INotifyPropertyChanged, IModel
    {
        private int _modeOfPaymentId;
        private string _description;

        #region --- PROPERTIES ---

        public int ModeOfPaymentId
        {
            get { return _modeOfPaymentId; }
            set
            {
                _modeOfPaymentId = value;
                OnPropertyChanged("ModeOfPaymentId");
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

        private const string TABLE_NAME = "ModeOfPayments";

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
            get { return new SqlParameter("?ModeOfPaymentId", ModeOfPaymentId); }
        }

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          List<SqlParameter> sqlParameter = Parameters;

                                          string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                                                  sqlParameter);
                                          ModeOfPaymentId = DatabaseController.ExecuteInsertQuery(sql, sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
                                      {
                                          SqlParameter key = ParamKey;

                                          List<SqlParameter> sqlParameter = Parameters;
                                          sqlParameter.Add(key);

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
                                          SqlParameter key = ParamKey;

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
                                        ModeOfPaymentId = id;

                                        SqlParameter key = ParamKey;
                                        string sql = DatabaseController.GenerateSelectStatement(TABLE_NAME, key);

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
            ModeOfPaymentId = 0;
            Description = "";
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ModeOfPaymentId = (int) dataRow["ModeOfPaymentId"];
            Description = (string) dataRow["Description"];
        }

        #endregion

        public static List<ModeOfPayment> GetList()
        {
            string sqlCommandText = string.Format("SELECT * FROM {0}", TABLE_NAME);
            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sqlCommandText);
            return (from DataRow row in dataTable.Rows
                    select new ModeOfPayment
                               {
                                   ModeOfPaymentId = (int) row["ModeOfPaymentId"],
                                   Description = (string) row["Description"],
                               }).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
