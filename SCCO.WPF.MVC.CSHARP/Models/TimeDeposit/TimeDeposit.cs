using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.TimeDeposit
{
    public class TimeDepositProduct : INotifyPropertyChanged, IModel
    {
        private string _name;
        private string _productCode;
        private int _minimumTerm;
        private int _maximumTerm;
        private decimal _minimumAmount;
        private decimal _maximumAmount;
        private decimal _interestRate;
        private int _id;

        private const string TABLE_NAME = "time_deposit_products";

        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID");}
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name");}
        }

        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; OnPropertyChanged("ProductCode"); }
        }

        public int MinimumTerm
        {
            get { return _minimumTerm; }
            set { _minimumTerm = value; OnPropertyChanged("MinimumTerm"); }
        }

        public int MaximumTerm
        {
            get { return _maximumTerm; }
            set { _maximumTerm = value; OnPropertyChanged("MaximumTerm"); }
        }

        public decimal MinimumAmount
        {
            get { return _minimumAmount; }
            set { _minimumAmount = value; OnPropertyChanged("MinimumAmount");
            }
        }

        public decimal MaximumAmount
        {
            get { return _maximumAmount; }
            set { _maximumAmount = value; OnPropertyChanged("MaximumAmount"); }
        }

        public decimal InterestRate
        {
            get { return _interestRate; }
            set { _interestRate = value; OnPropertyChanged("InterestRate"); }
        }

        private List<SqlParameter> SqlParameters
        {
            get
            {
                var sqlParameters = new List<SqlParameter>();
                ModelController.AddParameter(sqlParameters, "?ID", ID);
                ModelController.AddParameter(sqlParameters, "?Name", Name);
                ModelController.AddParameter(sqlParameters, "?ProductCode", ProductCode);
                ModelController.AddParameter(sqlParameters, "?MinimumTerm", MinimumTerm);
                ModelController.AddParameter(sqlParameters, "?MaximumTerm", MaximumTerm);
                ModelController.AddParameter(sqlParameters, "?MinimumAmount", MinimumAmount);
                ModelController.AddParameter(sqlParameters, "?MaximumAmount", MaximumAmount);
                ModelController.AddParameter(sqlParameters, "?InterestRate", InterestRate);
                return sqlParameters;
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Implementation of IModel

        public Result Create()
        {
            Action createRecord = () =>
            {
                string sql = DatabaseController.GenerateInsertStatement(TABLE_NAME,
                                                                        SqlParameters);
                ID = DatabaseController.ExecuteInsertQuery(sql, SqlParameters.ToArray());
            };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Update()
        {
            Action updateRecord = () =>
            {
                var key = new SqlParameter("?ID", ID);

                List<SqlParameter> sqlParameters = SqlParameters;
                string sql = DatabaseController.GenerateUpdateStatement(TABLE_NAME,
                                                                        sqlParameters, key);

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

        public void ResetProperties()
        {
            ID = 0;
            Name = "";
            ProductCode = "";
            MinimumTerm = 0;
            MaximumTerm = 0;
            MinimumAmount = 0m;
            MaximumAmount = 0m;
            InterestRate = 0m;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            Name = DataConverter.ToString(dataRow["Name"]);
            ProductCode = DataConverter.ToString(dataRow["ProductCode"]);
            MinimumTerm = DataConverter.ToInteger(dataRow["MinimumTerm"]);
            MaximumTerm = DataConverter.ToInteger(dataRow["MaximumTerm"]);
            MinimumAmount = DataConverter.ToDecimal(dataRow["MinimumAmount"]);
            MaximumAmount = DataConverter.ToDecimal(dataRow["MaximumAmount"]);
            InterestRate = DataConverter.ToDecimal(dataRow["InterestRate"]);
        }

        #endregion

        #region Static Methods
        public static ObservableCollection<TimeDepositProduct> GetAll()
        {
            var collection = new ObservableCollection<TimeDepositProduct>();
            const string sql = "SELECT * FROM " + TABLE_NAME;

            DataTable dataTable = DatabaseController.ExecuteSelectQuery(sql);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var item = new TimeDepositProduct();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }
            return collection;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
