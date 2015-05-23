using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class AccountVerifierSummary
    {
        public string MemberCode { get; set; }

        public string AccountCode { get; set; }

        public string AccountTitle { get; set; }

        public Decimal Balance { get; set; }

        public int TimeDepositDetailId { get; set; }

        public static List<AccountVerifierSummary> GetAccountSummary(string memberCode)
        {
            //IN tcMEM_CODE varchar(10),  tdDocDate DATE
            var parameters = new List<SqlParameter>
                                 {
                                     new SqlParameter("?tcMemberCode", memberCode),
                                     new SqlParameter("?tdTransactionDate", Controllers.MainController.UserTransactionDate)
                                 };

            var dataTable = Database.DatabaseController.ExecuteStoredProcedure("SP_MemberAccountSummary", parameters.ToArray());
            return (from DataRow row in dataTable.Rows
                    select new AccountVerifierSummary
                        {
                            MemberCode = memberCode,
                            AccountCode = (string) (row["AccountCode"]),
                            AccountTitle = Convert.ToString(row["AccountTitle"] + " "+Convert.ToString(row["CertificateNumber"])) ,
                            Balance = Convert.ToDecimal(row["Balance"]),
                            TimeDepositDetailId = Convert.ToInt32(row["TimeDepositDetailId"])
                        }).ToList();
        }

        public List<AccountVerifierDetail> TransactionDetails()
        {
            var tcMemberCode = new SqlParameter("?tcMemberCode", MemberCode);
            var tcAccountCode = new SqlParameter("?tcAccountCode", AccountCode);
            var tdTransactionDate = new SqlParameter("?tdTransactionDate", Controllers.MainController.UserTransactionDate);
            var tiTimeDepositDetailId = new SqlParameter("?tiTimeDepositDetailId", TimeDepositDetailId);

            DataTable dataTable = Database.DatabaseController.ExecuteStoredProcedure("SP_MemberAccountDetails", tcMemberCode, tcAccountCode,
                                                                  tdTransactionDate, tiTimeDepositDetailId);
            var list = new List<AccountVerifierDetail>();
            foreach (DataRow row in dataTable.Rows)
            {
                var newItem = new AccountVerifierDetail();
                newItem.TransactionDetailId = Convert.ToInt32(row["TransactionDetailId"]);
                newItem.VoucherDate = Convert.ToDateTime(row["VoucherDate"]);
                newItem.ReferenceCode = Convert.ToString(row["ReferenceCode"]);
                newItem.AccountCode = (string)(row["AccountCode"]);
                newItem.Debit = Convert.ToDecimal(row["Debit"]);
                newItem.Credit = Convert.ToDecimal(row["Credit"]);
                newItem.Balance = Convert.ToDecimal(row["Balance"]);
                newItem.Remark = Convert.ToString(row["Remark"]);
                newItem.Initial = Convert.ToString(row["Initial"]);
                newItem.Explanation = Convert.ToString(row["Explanation"]);
                list.Add(newItem);
            }
            return list;
        }
    }
}
