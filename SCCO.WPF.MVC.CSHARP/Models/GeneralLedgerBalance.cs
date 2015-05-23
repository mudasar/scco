using System;
using System.Collections.Generic;
using System.Data;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models
{
    public class GeneralLedgerBalance : AModelBase, IModel
    {
        private string _documentType;
        private int _documentNo;
        private DateTime _documentDate;
        private string _accountCode;
        private string _accountTitle;
        private decimal _debit;
        private decimal _credit;

        public GeneralLedgerBalance()
            : base("glbal")
        {
        }

        public string DocumentType
        {
            get { return _documentType; }
            set
            {
                _documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        public int DocumentNo
        {
            get { return _documentNo; }
            set
            {
                _documentNo = value;
                OnPropertyChanged("DocumentNo");
            }
        }

        public DateTime DocumentDate
        {
            get { return _documentDate; }
            set
            {
                _documentDate = value;
                OnPropertyChanged("DocumentDate");
            }
        }

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

        public decimal Debit
        {
            get { return _debit; }
            set
            {
                _debit = value;
                OnPropertyChanged("Debit");
            }
        }

        public decimal Credit
        {
            get { return _credit; }
            set
            {
                _credit = value;
                OnPropertyChanged("Credit");
            }
        }



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
            DocumentDate = new DateTime();
            DocumentNo = 0;
            DocumentType = "";
            Debit = 0m;
            Credit = new decimal();
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            ID = DataConverter.ToInteger(dataRow["ID"]);
            AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]);
            AccountTitle = DataConverter.ToString(dataRow["TITLE"]);
            DocumentDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]);
            DocumentNo = DataConverter.ToInteger(dataRow["DOC_NUM"]);
            DocumentType = DataConverter.ToString(dataRow["DOC_TYPE"]);
            Debit = DataConverter.ToDecimal(dataRow["DEBIT"]);
            Credit = DataConverter.ToDecimal(dataRow["CREDIT"]);
        }

        #endregion

        #region --- Private ----

        private void SetClassProperties()
        {
            // DO NOT INCLUDE KEY !!!
            var sqlParameters = new List<SqlParameter>();
            ModelController.AddParameter(sqlParameters, "?ACC_CODE", AccountCode);
            ModelController.AddParameter(sqlParameters, "?TITLE", AccountTitle);
            ModelController.AddParameter(sqlParameters, "?DOC_DATE", DocumentDate);
            ModelController.AddParameter(sqlParameters, "?DOC_NUM", DocumentNo);
            ModelController.AddParameter(sqlParameters, "?DOC_TYPE", DocumentType);
            ModelController.AddParameter(sqlParameters, "?DEBIT", Debit);
            ModelController.AddParameter(sqlParameters, "?CREDIT", Credit);
            Parameters = sqlParameters;
        }

        #endregion
    }
}