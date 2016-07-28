using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Views.AdministratorModule;
using System;
using System.ComponentModel;
using System.Data;

namespace SCCO.WPF.MVC.CS.Utilities.BackgroundTasks
{
    public class SavingsDepositInterestPostingWorker : BackgroundTaskViewModel
    {
        private DateTime _transactionDate;
        private InterestOnSavingsDepositViewModel _viewModel;

        public SavingsDepositInterestPostingWorker(InterestOnSavingsDepositViewModel viewModel, DateTime transactionDate)
        {
            _viewModel = viewModel;
            _transactionDate = transactionDate;
        }

        public Result Result { get; private set; }

        public override void ExecuteTask(object sender, DoWorkEventArgs e)
        {
            OnTaskStarting();
            Perform();
        }

        private DataTable GetAccountsWithRequiredBalance()
        {
            int quarter = _viewModel.Quarter;
            var transactionYear = _transactionDate.Year;
            var asOf = new DateTime(transactionYear, 1, 1);
            switch (_viewModel.Quarter)
            {
                case 1:
                    asOf = new DateTime(transactionYear, 3, 31);
                    break;

                case 2:
                    asOf = new DateTime(transactionYear, 6, 30);
                    break;

                case 3:
                    asOf = new DateTime(transactionYear, 9, 30);
                    break;

                case 4:
                    asOf = new DateTime(transactionYear, 12, 31);
                    break;
            }

            // get list of members that meets required balance for that quarter
            var sqlCommand = GetAccountEndBalancesAsOfQuery();
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("?AsOf", asOf),
                new SqlParameter("?AccountCode", _viewModel.SavingsDepositAccount.AccountCode),
                new SqlParameter("?RequiredBalance", 1)
            };

            return DatabaseController.ExecuteSelectQuery(sqlCommand, sqlParameters);
        }

        private void Perform()
        {
            try
            {
                var accountsWithRequiredBalance = GetAccountsWithRequiredBalance();
                ProcessData(accountsWithRequiredBalance);
            }
            catch (Exception exception)
            {
                Result = new Result(false, exception.Message);
                _backgroundWorker.CancelAsync();
            }
        }

        private void ProcessData(DataTable dataTable)
        {
            var currentRow = 0;
            var totalRows = dataTable.Rows.Count;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                currentRow++;

                var memberCode = DataConverter.ToString(dataRow["member_code"]);
                var interestEarned = GetInterestEarned(memberCode);
                var percent = (currentRow / (decimal)totalRows) * 100m;
                _backgroundWorker.ReportProgress((int)percent);
            }
        }

        private decimal GetInterestEarned(string memberCode)
        {
            // collect average balance per quarter
            // then apply interest
            var accountCode = _viewModel.SavingsDepositAccount.AccountCode;
            var year = _transactionDate.Year;
            var month1 = 0m;
            var month2 = 0m;
            var month3 = 0m;
            if (_viewModel.Quarter == 1)
            {
                month1 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 1));
                month2 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 2));
                month3 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 3));
            }
            if (_viewModel.Quarter == 2)
            {
                month1 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 4));
                month2 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 5));
                month3 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 6));
            }
            if (_viewModel.Quarter == 3)
            {
                month1 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 7));
                month2 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 8));
                month3 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 9));
            }
            if (_viewModel.Quarter == 4)
            {
                month1 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 10));
                month2 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 11));
                month3 = GetMemberAccountEndBalanceAsOf(memberCode, accountCode, GetMaxDateOfMonth(year, 12));
            }

            var multiplier = _viewModel.InterestRate / 4;
            var average = (month1 + month2 + month3) / 3;

            var interest = 0m;
            if(average >= _viewModel.RequiredBalance)
            {
                interest = Math.Round(average * multiplier, 2);
            }
            return interest;
        }

        private decimal GetMemberAccountEndBalanceAsOf(string memberCode, string accountCode, DateTime asOf)
        {
            var sqlCommand = GetMemberAccountEndBalanceAsOfQuery();
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("?MemberCode", memberCode),
                new SqlParameter("?AccountCode", accountCode),
                new SqlParameter("?AsOf", asOf)
            };

            var dataTable = DatabaseController.ExecuteSelectQuery(sqlCommand, sqlParameters);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                return DataConverter.ToDecimal(dataRow["balance"]);
            }

            return 0m;
        }

        private DateTime GetMaxDateOfMonth(int year, int month)
        {
            switch (month)
            {
                case 0:
                    return new DateTime(year - 1, 12, 31);
                case 1:
                    return new DateTime(year, 1, 31);
                case 2:
                    return new DateTime(year, 3, 1).AddDays(-1);
                case 3:
                    return new DateTime(year, 3, 31);
                case 4:
                    return new DateTime(year, 4, 30);
                case 5:
                    return new DateTime(year, 5, 31);
                case 6:
                    return new DateTime(year, 6, 30);
                case 7:
                    return new DateTime(year, 7, 31);
                case 8:
                    return new DateTime(year, 8, 31);
                case 9:
                    return new DateTime(year, 9, 30);
                case 10:
                    return new DateTime(year, 10, 31);
                case 11:
                    return new DateTime(year, 11, 30);
                case 12:
                    return new DateTime(year, 12, 31);
            }

            return new DateTime();
        }

        #region --- QUERIES ---

        private static string GetAccountEndBalancesAsOfQuery()
        {
            return @"
SELECT a.mem_code as member_code,
       a.acc_code as account_code,
       IF(b.nature = 'D', IFNULL(SUM(a.debit), 0) - IFNULL(SUM(a.credit), 0),
       IFNULL(
       SUM(a.credit), 0) - IFNULL(SUM(a.debit), 0)) AS balance
FROM   (SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `slbal`
        WHERE  ( doc_date <= ?AsOf
                 AND acc_code = ?AccountCode )
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `or`
        WHERE  ( doc_date <= ?AsOf
                 AND acc_code = ?AccountCode )
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `cv`
        WHERE  ( doc_date <= ?AsOf
                 AND acc_code = ?AccountCode )
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `jv`
        WHERE  ( doc_date <= ?AsOf
                 AND acc_code = ?AccountCode )) AS a
       JOIN chart b
         ON a.acc_code = b.`code`
GROUP BY a.mem_code
HAVING balance >= ?RequiredBalance
";
        }

        private static string GetMemberAccountEndBalanceAsOfQuery()
        {
            return @"
SELECT a.mem_code AS member_code,
       a.acc_code AS account_code,
       ?AsOf AS as_of,
       IF(b.nature = 'D', COALESCE(SUM(a.debit), 0) - COALESCE(SUM(a.credit), 0),
       COALESCE(
       SUM(a.credit), 0) - COALESCE(SUM(a.debit), 0)) AS balance
FROM   (SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   slbal
        WHERE  mem_code = ?MemberCode
               AND acc_code = ?AccountCode
               AND doc_date <= ?AsOf
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `or`
        WHERE  mem_code = ?MemberCode
               AND acc_code = ?AccountCode
               AND doc_date <= ?AsOf
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `jv`
        WHERE  mem_code = ?MemberCode
               AND acc_code = ?AccountCode
               AND doc_date <= ?AsOf
        UNION ALL
        SELECT mem_code,
               acc_code,
               debit,
               credit
        FROM   `cv`
        WHERE  mem_code = ?MemberCode
               AND acc_code = ?AccountCode
               AND doc_date <= ?AsOf) AS a
       JOIN chart b
         ON a.acc_code = b.`code` 
";
        }

        #endregion --- QUERIES ---
    }
}