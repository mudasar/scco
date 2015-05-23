using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class TrDetailMapping : AMappingDetail
    {
        public int TransactionDetailId { get; set; }

        public TrDetailMapping(int transactionDetailId)
        {
            TransactionDetailId = transactionDetailId;
        }

        public TrDetailMapping()
        {
            LoanDetailId = 0;
            TimeDepositDetailId = 0;
            TransactionDetailId = 0;
        }

        public override Controllers.Result Find(int id)
        {
            try
            {
                string sqlCommandText = string.Format("SELECT * FROM {0} WHERE TransactionDetailId = ?TransactionDetailId", TableName);
                DataTable dataTable = Database.DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                  new SqlParameter("?TransactionDetailId", id));
                if (dataTable.Rows.Count == 0)
                {
                    TransactionDetailId = 0;
                    LoanDetailId = 0;
                    TimeDepositDetailId = 0;
                }
                else
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        TransactionDetailId = (int)row["TransactionDetailId"];
                        LoanDetailId = (int)row["LoanDetailId"];
                        TimeDepositDetailId = (int)row["TimeDepositDetailId"];
                    }
                }



                return new Result(true, "Sucessfully record has been found!");
            }
            catch (Exception e)
            {

                return new Result(false, e.Message);
            }
        }

        private const string TableName = "trdetailsmapping";

        public override Controllers.Result Create()
        {
            try
            {
                string sqlCommandText = string.Format("INSERT INTO {0} (LoanDetailId,TimeDepositDetailId,TransactionDetailId) VALUES (?LoanDetailId,?TimeDepositDetailId,?TransactionDetailId)", TableName);
                MappingDetailId = DatabaseController.ExecuteInsertQuery(sqlCommandText, new SqlParameter("?LoanDetailId", LoanDetailId), new SqlParameter("?TimeDepositDetailId", TimeDepositDetailId), new SqlParameter("?TransactionDetailId", TransactionDetailId));
                return new Result(true, "Sucessfully record has been saved!");
            }
            catch (Exception e)
            {

                return new Result(false, e.Message);
            }
        }

    }
}
