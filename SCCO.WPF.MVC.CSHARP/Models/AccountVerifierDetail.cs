using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;

namespace SCCO.WPF.MVC.CS.Models
{
    public class AccountVerifierDetail
    {
        public string AccountCode
        {
            get;
            set;
        }

        public decimal Balance
        {
            get;
            set;
        }

        public decimal Credit
        {
            get;
            set;
        }

        public decimal Debit
        {
            get;
            set;
        }

        public string Explanation
        {
            get;
            set;
        }

        public string Initial
        {
            get;
            set;
        }

        public string ReferenceCode
        {
            get;
            set;
        }

        public string Remark
        {
            get;
            set;
        }

        public bool IsMarked
        {
            get; set; }

        public int TimeDepositDetailId { get; set; }

        public int LoanDetailId { get; set; }

        public int TransactionDetailId
        {
            get;
            set;
        }

        public DateTime VoucherDate
        {
            get;
            set;
        }

        public static List<AccountVerifierDetail> GetAccountDetails(string memberCode, string accountCode,
                                                                    DateTime transactionDate, int timeDepositDetailId)
        {
            //IN tcMemberCode VARCHAR(10), tcAccountCode VARCHAR (10), tdTransactionDate DATE
            var tcMemberCode = new SqlParameter("?tcMemberCode", memberCode);
            var tcAccountCode = new SqlParameter("?tcAccountCode", accountCode);
            var tdTransactionDate = new SqlParameter("?tdTransactionDate",transactionDate);
            var tiTimeDepositDetailId = new SqlParameter("?tiTimeDepositDetailId", timeDepositDetailId);

            DataTable dataTable = Database.DatabaseController.ExecuteStoredProcedure("SP_MemberAccountDetails", tcMemberCode, tcAccountCode,
                                                                  tdTransactionDate, tiTimeDepositDetailId);
            var list = new List<AccountVerifierDetail>(); 
            foreach (DataRow row in dataTable.Rows)
            {
                var newItem = new AccountVerifierDetail();
                newItem.TransactionDetailId = Convert.ToInt32(row["TransactionDetailId"]);
                newItem.VoucherDate = Convert.ToDateTime(row["VoucherDate"]);
                newItem.ReferenceCode = Convert.ToString(row["ReferenceCode"]);
                newItem.AccountCode = (string) (row["AccountCode"]);
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