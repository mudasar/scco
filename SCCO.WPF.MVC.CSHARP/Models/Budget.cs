using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class Budget : AModelBase, IModel
    {
        private string _accountCode;
        private decimal _amount;
        private int _year;
        private string _accountTitle;

        public Budget() : base("budgets")
        {}

        #region --- Properties ---

        public string AccountCode
        {
            get { return _accountCode; }
            set
            {
                _accountCode = value;
                OnPropertyChanged("AccountCode");
            }
        }

        public string AccountTitle
        {
            get { return _accountTitle; }
            set
            {
                _accountTitle = value;
                OnPropertyChanged("AccountTitle");
            }
        }

        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }

        #endregion

        #region --- Implementation of IModel ---

        public Result Create()
        {
            SetClassProperties();
            var crudResult = VirtualCreate();
            return new Result(crudResult.Success, crudResult.Message);
        }

        public Result Update()
        {
            SetClassProperties();
            var crudResult = VirtualUpdate();
            return new Result(crudResult.Success, crudResult.Message);
        }

        public Result Destroy()
        {
            var result = VirtualDestroy();
            return new Result(result.Success, result.Message);
        }

        public Result Find(int id)
        {
            try
            {
                var dataRow = base.VirtualFind(id);
                ResetProperties();
                if (dataRow != null)
                {
                    SetPropertiesFromDataRow(dataRow);
                    return new Result(true, "Item found!");
                }
                return new Result(false, "No item found!");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
            
        }

        public void ResetProperties()
        {
            ID = 0;
            AccountCode = "";
            AccountTitle = "";
            Amount = 0m;
            Year = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["id"]);
            AccountCode = DataConverter.ToString(dataRow["account_code"]);
            AccountTitle = DataConverter.ToString(dataRow["account_title"]);
            Amount = DataConverter.ToDecimal(dataRow["amount"]);
            Year = DataConverter.ToInteger(dataRow["year"]);
        }

        #endregion

        #region --- Private ----
        
        private void SetClassProperties()
        {
            // DO NOT INCLUDE KEY !!!
            var sqlParameters = new List<SqlParameter>();
            ModelController.AddParameter(sqlParameters, "?account_code", AccountCode);
            ModelController.AddParameter(sqlParameters, "?account_title", AccountTitle);
            ModelController.AddParameter(sqlParameters, "?amount", Amount);
            ModelController.AddParameter(sqlParameters, "?year", Year);
            Parameters = sqlParameters;
        }

        #endregion
    }

    public class BudgetCollection : System.Collections.ObjectModel.ObservableCollection<Budget>
    {
    }


}
