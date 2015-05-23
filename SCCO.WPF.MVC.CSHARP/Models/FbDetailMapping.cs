using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using System.Data;

namespace SCCO.WPF.MVC.CS.Models
{
    public class FbDetailMapping : AMappingDetail
    {
        public int ForwardedBalanceId { get; set; }

        public FbDetailMapping()
        {
            LoanDetailId = 0;
            TimeDepositDetailId = 0;
            ForwardedBalanceId = 0;
        }

        public FbDetailMapping(int forwarderBalancesId)
        {
            ForwardedBalanceId = forwarderBalancesId;
        }

        public override Controllers.Result Find(int id)
        {
            try
            {
                string sqlCommandText = string.Format("SELECT * FROM {0} WHERE ForwardedBalanceId = ?ForwardedBalanceId", TableName);
                DataTable dataTable = Database.DatabaseController.ExecuteSelectQuery(sqlCommandText,
                                                                  new SqlParameter("?ForwardedBalanceId", id));
                if (dataTable.Rows.Count == 0)
                {
                    ForwardedBalanceId = 0;
                    LoanDetailId = 0;
                    TimeDepositDetailId = 0;
                }
                else
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ForwardedBalanceId = (int)row["ForwardedBalanceId"];
                        LoanDetailId = (int)row["LoanDetailId"];
                        TimeDepositDetailId = (int)row["TimeDepositDetailId"];
                    }
                }

                return new Result(true, "Sucessfully record has been found!");
            }
            catch (Exception e)
            {
                
                return  new Result(false,e.Message);
            }
        }

        private const string TableName = "fbdetailsmapping";

        public override Controllers.Result Create()
        {
            try
            {
                string sqlCommandText = string.Format("INSERT INTO {0} (LoanDetailId,TimeDepositDetailId,TransactionDetailId) VALUES (?LoanDetailId,?TimeDepositDetailId,?ForwardedBalanceId)", TableName);
               MappingDetailId = Database.DatabaseController.ExecuteInsertQuery(sqlCommandText, new SqlParameter("?LoanDetailId", LoanDetailId), new SqlParameter("?TimeDepositDetailId", TimeDepositDetailId), new SqlParameter("?ForwardedBalanceId", ForwardedBalanceId));
                return new Result(true, "Sucessfully record has been saved!");
            }
            catch (Exception e)
            {

                return new Result(false, e.Message);
            }
        }
    }
}
