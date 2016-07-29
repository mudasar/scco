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

        private void Perform()
        {
            try
            {
                var accountCode = _viewModel.SavingsDepositAccount.AccountCode;
                var data = DataRepository.GetMemberAccountBalanceByAccountCodeData(accountCode);
                ProcessData(data);
                Result = new Result(true, "Success!");
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
            var multiplier = _viewModel.InterestRate / 4;
            var progress = 0;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                currentRow++;

                var month1 = 0m;
                var month2 = 0m;
                var month3 = 0m;
                switch (_viewModel.Quarter)
                {
                    case 1:
                        month1 = DataConverter.ToDecimal(dataRow[GetMonthName(1).ToLower()]);
                        month2 = DataConverter.ToDecimal(dataRow[GetMonthName(2).ToLower()]);
                        month3 = DataConverter.ToDecimal(dataRow[GetMonthName(3).ToLower()]);
                        break;
                    case 2:
                        month1 = DataConverter.ToDecimal(dataRow[GetMonthName(4).ToLower()]);
                        month2 = DataConverter.ToDecimal(dataRow[GetMonthName(5).ToLower()]);
                        month3 = DataConverter.ToDecimal(dataRow[GetMonthName(6).ToLower()]);
                        break;
                    case 3:
                        month1 = DataConverter.ToDecimal(dataRow[GetMonthName(7).ToLower()]);
                        month2 = DataConverter.ToDecimal(dataRow[GetMonthName(8).ToLower()]);
                        month3 = DataConverter.ToDecimal(dataRow[GetMonthName(9).ToLower()]);
                        break;
                    case 4:
                        month1 = DataConverter.ToDecimal(dataRow[GetMonthName(10).ToLower()]);
                        month2 = DataConverter.ToDecimal(dataRow[GetMonthName(11).ToLower()]);
                        month3 = DataConverter.ToDecimal(dataRow[GetMonthName(12).ToLower()]);
                        break;
                }

                var interest = 0m;
                var average = (month1 + month2 + month3) / 3;
                if (average >= _viewModel.RequiredBalance)
                {
                    interest = Math.Round(average * multiplier, 2);
                    // dito yung code na mag append ng item
                    var memberCode = DataConverter.ToString(dataRow["member_code"]);
                }

                var percent = (currentRow / (decimal)totalRows) * 100m;

                var floor = (int)Math.Floor(percent);
                if (floor > progress)
                {
                    _backgroundWorker.ReportProgress((int)percent);
                    progress = floor;
                    Console.WriteLine(progress);
                }
            }
            _backgroundWorker.ReportProgress(100);
        }

        private static string GetMonthName(int monthNumber)
        {
            var date = new DateTime(DateTime.Now.Year, monthNumber, 1);
            var monthName = date.ToString("MMMM");
            return monthName;
        }
    }
}