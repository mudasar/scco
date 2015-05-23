﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.AccountVerifier
{
    public class AccountSummary
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }

        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }

        public decimal CreditAccount { get; set; }
        public decimal DebitAccount { get; set; }
        public decimal Balance { get; set; }

        public string CertificateNo { get; set; }

        public System.DateTime AsOf { get; set; }


        public static List<AccountSummary> Find(string memberCode, System.DateTime asOf)
        {
            // StoredProcedure Information
            // procedure nane : sp_account_summary
            // parameters     : ps_member_code VARCHAR(10), pd_as_of DATE
            // return fields  : member_code, member_name, account_code, account_title, certificate_no
            //                  debit_account, credit_account, 
            //                  ending_balance, as_of

            var list = new List<AccountSummary>();
            var sqlParams = new SqlParameter[2];
            sqlParams[0]= new SqlParameter("ts_member_code", memberCode);
            sqlParams[1] = new SqlParameter("td_as_of", asOf);


            var dataTable = DatabaseController.ExecuteStoredProcedure("sp_account_summary", sqlParams);
            foreach (DataRow row in dataTable.Rows)
            {
                var accountSummary = new AccountSummary();
                accountSummary.MemberCode = memberCode;
                accountSummary.MemberName = DataConverter.ToString(row["member_name"]);
                accountSummary.AccountCode = DataConverter.ToString(row["account_code"]);
                accountSummary.AccountTitle = DataConverter.ToString(row["account_title"]);
                accountSummary.CertificateNo = DataConverter.ToString(row["certificate_no"]);
                accountSummary.DebitAccount = DataConverter.ToDecimal(row["debit_account"]);
                accountSummary.CreditAccount = DataConverter.ToDecimal(row["credit_account"]);
                accountSummary.Balance = DataConverter.ToDecimal(row["ending_balance"]);
                accountSummary.AsOf = DataConverter.ToDateTime(row["as_of"]);
                list.Add(accountSummary);
            }

            return list;
        }

        public static Collection<AccountSummary> PerAccount(string accountCode, System.DateTime asOf)
        {
            const string sp = "sp_sl_balance_per_account";
            var ps = new[] { new SqlParameter("?ts_account_code", accountCode), new SqlParameter("?td_as_of", asOf) };
            var dt = DatabaseController.ExecuteStoredProcedure(sp, ps);

            var collection = new Collection<AccountSummary>();
            foreach (DataRow dataRow in dt.Rows)
            {
                var item = new AccountSummary();
                item.SetPropertiesFromDataRow(dataRow);
                collection.Add(item);
            }

            return collection;
        }

        private void SetPropertiesFromDataRow(DataRow dataRow)
        {
            MemberCode = DataConverter.ToString(dataRow["member_code"]);
            MemberName = DataConverter.ToString(dataRow["member_name"]);
            AccountCode = DataConverter.ToString(dataRow["account_code"]);
            AccountTitle = DataConverter.ToString(dataRow["account_title"]);
            CertificateNo = DataConverter.ToString(dataRow["certificate_no"]);
            DebitAccount = DataConverter.ToDecimal(dataRow["debit_account"]);
            CreditAccount = DataConverter.ToDecimal(dataRow["credit_account"]);
            Balance = DataConverter.ToDecimal(dataRow["ending_balance"]);
            AsOf = DataConverter.ToDateTime(dataRow["as_of"]);
        }
    }
}


#region --- removed procedures ---

/**
 * 
    //private static List<AccountSummary> Find(string memberCode)
    //{
    //    var result = new List<AccountSummary>();
    //    var sqlParam = new SqlParameter("memberCode", memberCode);
    //    var dataTable = DatabaseController.ExecuteStoredProcedure("sp_account_summary", sqlParam);
    //    foreach (DataRow row in dataTable.Rows)
    //    {
    //        var accountSummary = new AccountSummary();
    //        accountSummary.MemberCode = memberCode;
    //        accountSummary.AccountCode = DataConverter.ToString(row["ACC_CODE"]);
    //        accountSummary.AccountTitle = DataConverter.ToString(row["TITLE"]);
    //        accountSummary.Balance = DataConverter.ToDecimal(row["END_BAL"]);
    //        accountSummary.CertificateNo = DataConverter.ToString(row["CERT_NO"]);
    //        result.Add(accountSummary);
    //    }
    //    return result;
    //}
 * 
* */

#endregion
