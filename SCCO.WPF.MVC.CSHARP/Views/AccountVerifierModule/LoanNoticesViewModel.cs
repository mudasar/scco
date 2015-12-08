using System;
using System.Data;

namespace SCCO.WPF.MVC.CS.Views.AccountVerifierModule
{
    public class LoanNoticesViewModel
    {
        public DataTable GetNonPerformLoansData(Models.Loan.LoanDetails loanDetails)
        {
           var dataTable = new DataTable("notice_loan_non_performing");
            //dataTable.Columns.Add("mem_code", typeof(string));
            //dataTable.Columns.Add("member_name", typeof(string));

            //dataTable.Columns.Add("address1", typeof(string));
            //dataTable.Columns.Add("address2", typeof(string));
            //dataTable.Columns.Add("address3", typeof(string));

            //dataTable.Columns.Add("account_code", typeof(string));
            //dataTable.Columns.Add("account_title", typeof(string));
            //dataTable.Columns.Add("ending_balance", typeof(decimal));

            //dataTable.Columns.Add("document_date", typeof(string));
            //dataTable.Columns.Add("document_type", typeof(string));
            //dataTable.Columns.Add("document_number", typeof(Int32));

            //dataTable.Columns.Add("release_number", typeof(Int32));
            //dataTable.Columns.Add("loan_amount", typeof(decimal));
            //dataTable.Columns.Add("terms", typeof(int));

            //dataTable.Columns.Add("terms_mode", typeof(string));
            //dataTable.Columns.Add("payment_mode", typeof(string));

            //dataTable.Columns.Add("date_granted", typeof(DateTime));
            //dataTable.Columns.Add("date_maturity", typeof(DateTime));
            //dataTable.Columns.Add("date_cut_off", typeof(DateTime));

            //dataTable.Columns.Add("payment", typeof(decimal));
            //dataTable.Columns.Add("interest_rate", typeof(decimal));
            //dataTable.Columns.Add("interest_amount", typeof(decimal));
            //dataTable.Columns.Add("interest_amortization", typeof(decimal));

            //dataTable.Columns.Add("date_approved", typeof(DateTime));
            //dataTable.Columns.Add("date_cancelled", typeof(DateTime));
            //dataTable.Columns.Add("date_released", typeof(DateTime));
            //dataTable.Columns.Add("date_applied", typeof(DateTime));

            //dataTable.Columns.Add("comaker1_code", typeof(string));
            //dataTable.Columns.Add("comaker1_name", typeof(string));

            //dataTable.Columns.Add("comaker2_code", typeof(string));
            //dataTable.Columns.Add("comaker2_name", typeof(string));

            //dataTable.Columns.Add("comaker3_code", typeof(string));
            //dataTable.Columns.Add("comaker3_name", typeof(string));

            //dataTable.Columns.Add("comaker4_code", typeof(string));
            //dataTable.Columns.Add("comaker4_name", typeof(string));

            //dataTable.Columns.Add("comaker5_code", typeof(string));
            //dataTable.Columns.Add("comaker5_name", typeof(string));

            //dataTable.Columns.Add("this_month", typeof(decimal));
            //dataTable.Columns.Add("collector", typeof(string));

            //dataTable.Columns.Add("notice1", typeof(bool));
            //dataTable.Columns.Add("notice2", typeof(bool));
            //dataTable.Columns.Add("notice3", typeof(bool));

            //dataTable.Columns.Add("collateral", typeof(bool));
            //dataTable.Columns.Add("collateral_description", typeof(string));

            //dataTable.Columns.Add("as_of", typeof(DateTime));
            //dataTable.Columns.Add("expected_balance", typeof(decimal));

            //System.Data.DataRow row = new ()
            //dataTable.Rows.Add(row);

            return dataTable;
        }
    }
}
