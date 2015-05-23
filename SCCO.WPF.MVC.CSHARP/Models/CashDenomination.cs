using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class CashDenomination : INotifyPropertyChanged, IModel
    {
        private int _cashDenominationId;
        private int _transactionHeaderId;
        private int _denomination;
        private int _quantity;

        #region --- CONSTRUCTOR ---

        public CashDenomination()
        {
            CashDenominationId = 0;
            TransactionHeaderId = 0;
            Denomination = 0;
            Quantity = 0;
        }

        #endregion

        #region --- PROPERTIES ---

        public int CashDenominationId
        {
            get { return _cashDenominationId; }
            set { _cashDenominationId = value; OnPropertyChanged("CashDenominationId"); }
        }

        public int TransactionHeaderId
        {
            get { return _transactionHeaderId; }
            set { _transactionHeaderId = value; OnPropertyChanged("TransactionHeaderId"); }
        }

        public int Denomination
        {
            get { return _denomination; }
            set { _denomination = value; OnPropertyChanged("CashAndCheckBreakDown"); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity"); }
        }

        #endregion

        #region --- FIELDS ---

        private const string TableName = "CashDenominations";

        #endregion

        #region --- METHODS & EVENTS

        #region IModel Members

        public Result Create()
        {
            Action createRecord = () =>
                                      {
                                          var queryBuilder = new StringBuilder();
                                          queryBuilder.Append("INSERT INTO ");
                                          queryBuilder.Append("`" + TableName + "` ");
                                          queryBuilder.Append("(TransactionHeaderId,CashAndCheckBreakDown,Quantity) ");
                                          queryBuilder.Append("VALUES ");
                                          queryBuilder.Append("(?TransactionHeaderId,?CashAndCheckBreakDown,?Quantity)");

                                          var sqlParameter = new List<SqlParameter>
                                                                 {
                                                                     new SqlParameter("?TransactionHeaderId", TransactionHeaderId),
                                                                     new SqlParameter("?CashAndCheckBreakDown", Denomination),
                                                                     new SqlParameter("?Quantity", Quantity)
                                                                 };

                                          CashDenominationId =
                                              DatabaseController.ExecuteInsertQuery(queryBuilder.ToString(),
                                                                                    sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(createRecord);
        }

        public Result Refresh()
        {
            return Find(CashDenominationId);
        }

        public Result Update()
        {

            Action updateRecord = () =>
                                      {
                                          var queryBuilder = new StringBuilder();
                                          queryBuilder.Append("UPDATE ");
                                          queryBuilder.Append("`" + TableName + "` ");
                                          queryBuilder.Append("SET ");
                                          queryBuilder.Append("CashAndCheckBreakDown = ?CashAndCheckBreakDown, ");
                                          queryBuilder.Append("Quantity = ?Quantity, ");
                                          queryBuilder.Append("TransactionHeaderId = ?TransactionHeaderId ");
                                          queryBuilder.Append(
                                              "WHERE CashDenominationId = ?CashDenominationId");



                                          var sqlParameter = new List<SqlParameter>
                                                                 {
                                                                     new SqlParameter("?CashDenominationId",
                                                                                      CashDenominationId),
                                                                     new SqlParameter("?TransactionHeaderId",
                                                                                      TransactionHeaderId),
                                                                     new SqlParameter("?CashAndCheckBreakDown", Denomination),
                                                                     new SqlParameter("?Quantity", Quantity)
                                                                 };

                                          DatabaseController.ExecuteNonQuery(queryBuilder.ToString(),
                                                                             sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(updateRecord);
        }

        public Result Destroy()
        {
            Action deleteRecord = () =>
                                      {
                                          var queryBuilder = new StringBuilder();
                                          queryBuilder.Append("DELETE FROM ");
                                          queryBuilder.Append("`" + TableName + "` ");
                                          queryBuilder.Append("WHERE CashDenominationId = ?CashDenominationId");

                                          var sqlParameter = new List<SqlParameter>
                                                                 {
                                                                     new SqlParameter("?CashDenominationId",
                                                                                      CashDenominationId),
                                                                 };

                                          DatabaseController.ExecuteNonQuery(queryBuilder.ToString(),
                                                                             sqlParameter.ToArray());
                                      };

            return ActionController.InvokeAction(deleteRecord);
        }

        public Result Find(int id)
        {
            Action findRecord = () =>
                                    {
                                        ResetProperties();
                                        CashDenominationId = id;

                                        var queryBuilder = new StringBuilder();
                                        queryBuilder.Append("SELECT * FROM ");
                                        queryBuilder.Append(TableName);
                                        queryBuilder.Append(" WHERE CashDenominationId = ?id LIMIT 1");


                                        DataTable dataTable =
                                            DatabaseController.ExecuteSelectQuery(queryBuilder.ToString(),
                                                                                  new SqlParameter("?id", id));

                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            SetPropertiesFromDataRow(dataRow);
                                        }
                                    };

            return ActionController.InvokeAction(findRecord);
        }

        public void ResetProperties()
        {
            CashDenominationId = 0;
            TransactionHeaderId = 0;
            Denomination = 0;
            Quantity = 0;
        }

        public void SetPropertiesFromDataRow(DataRow dataRow)
        {
            CashDenominationId = (int)dataRow["CashDenominationId"];
            TransactionHeaderId = (int)dataRow["TransactionHeaderId"];
            Denomination = (int)dataRow["CashAndCheckBreakDown"];
            Quantity = (int)dataRow["Quantity"];
        }

        #endregion

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
