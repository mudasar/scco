using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Properties;

namespace SCCO.WPF.MVC.CS.Utilities.BackgroundTasks
{
    public class FinancialReportWorker : BackgroundTaskViewModel
    {
        private static Company _cooperative;
        private static string _branch;
        private static DateTime _asOf;
        private static string _outputFile;

        private int _currentRow;
        private int _totalRows;

        public Result Result { get; private set; }

        public FinancialReportWorker(DateTime asOf, string outputFile)
        {
            _asOf = asOf;
            _outputFile = outputFile;
        }

        public override void ExecuteTask(object sender, DoWorkEventArgs e)
        {
            OnTaskStarting();
            Perform();
        }

        // private
        private void Perform()
        {
            try
            {
                _cooperative = Company.FirstOrDefault();
                _branch = Settings.Default.BranchName.ToUpper();

                var template = FinancialReportExcelCreator.GetFinancialStatementReportsTemplate();
                File.Copy(template, _outputFile, true);

                var newFile = new FileInfo(_outputFile);

                using (var package = new ExcelPackage(newFile))
                {
                    var ws1 = package.Workbook.Worksheets[1];
                    var ws2 = package.Workbook.Worksheets[2];
                    var ws3 = package.Workbook.Worksheets[3];

                    _currentRow = DataConverter.ToInteger(ws2.Cells["K2"].Value) +
                                 DataConverter.ToInteger(ws3.Cells["K2"].Value);

                    _totalRows = DataConverter.ToInteger(ws2.Cells["L2"].Value) +
                                DataConverter.ToInteger(ws3.Cells["L2"].Value);

                    ProcessConditionSummary(ws1);
                    ProcessConditionDetails(ws2);
                    ProcessOperation(ws3);
                    package.Save();
                }
                Result = new Result(true, "Report created successfully.");
            }
            catch (Exception exception)
            {
                Result =  new Result(false, exception.Message);
                _backgroundWorker.CancelAsync();
            }
        }

        private void ProcessOperation(ExcelWorksheet excelWorksheet)
        {
            var startRow = DataConverter.ToInteger(excelWorksheet.Cells["K2"].Value);
            var endRow = DataConverter.ToInteger(excelWorksheet.Cells["L2"].Value);
            const int colB = 2; // CODE
            const int colD = 4; // BUDGET
            const int colE = 5; // PREVIOUS MONTH
            const int colF = 6; // CURRENT MONTH
            const int colG = 7; // TO DATE
            const int colJ = 10; // Account Code Filter
            var dateStart = new DateTime(_asOf.Year, _asOf.Month, 1);
            var previousMonth = dateStart.AddDays(-1);

            excelWorksheet.Cells["A1"].Value = _cooperative.CompanyName.ToUpper();
            excelWorksheet.Cells["A2"].Value = _cooperative.Address;

            var branch = Settings.Default.BranchName.ToUpper();
            excelWorksheet.Cells["A5"].Value = string.Format("STATEMENT FINANCIAL OF OPERATION - {0}", branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();
            excelWorksheet.Cells["B8:G8"].Style.Numberformat.Format = "@"; //Format as text
            excelWorksheet.Cells[8, colD].Value = string.Format("{0} BUDGET", _asOf.Year).ToUpper();
            excelWorksheet.Cells[8, colE].Value = string.Format("{0:MMM yyyy}", previousMonth).ToUpper();
            excelWorksheet.Cells[8, colF].Value = string.Format("{0:MMM yyyy}", _asOf).ToUpper();
            excelWorksheet.Cells[8, colG].Value = "TO DATE";

            for (var i = startRow; i < endRow; i++)
            {
                _currentRow++;
                var code = (string) excelWorksheet.Cells[i, colB].Value;
                if (string.IsNullOrEmpty(code)) continue;

                excelWorksheet.Cells[i, colD].Value = 0m;
                excelWorksheet.Cells[i, colE].Value = 0m;
                excelWorksheet.Cells[i, colF].Value = 0m;

                var budget = FinancialReportExcelCreator.GetBudget(code);
                excelWorksheet.Cells[i, colD].Value = budget;

                var codeFilter = (string) excelWorksheet.Cells[i, colJ].Value;
                if (string.IsNullOrEmpty(codeFilter)) continue;
                var parsedCodes = codeFilter.Split(',');
                if (parsedCodes.Length == 0) continue;

                var codeList = parsedCodes.Select(s => s.Replace("\"", "")).ToList();

                var lastMonthBalance = previousMonth.Year < _asOf.Year
                                           ? FinancialReportExcelCreator.GetAccountForwardedBalance(codeList)
                                           : FinancialReportExcelCreator.GetAccountSummary(codeList, previousMonth);

                excelWorksheet.Cells[i, colE].Value = lastMonthBalance;

                var currentAmountTotal = FinancialReportExcelCreator.GetAccountSummaryBetweenDates(codeList, dateStart,
                                                                                                   _asOf);
                excelWorksheet.Cells[i, colF].Value = currentAmountTotal;

                var percent = (_currentRow/(decimal)_totalRows) * 100m;
                _backgroundWorker.ReportProgress((int)percent);
            }
        }

        private void ProcessConditionDetails(ExcelWorksheet excelWorksheet)
        {
            var startRow = DataConverter.ToInteger(excelWorksheet.Cells["K2"].Value);
            var endRow = DataConverter.ToInteger(excelWorksheet.Cells["L2"].Value);
            const int colF = 6; // AMOUNT
            const int colJ = 10; // Account Code Filter

            excelWorksheet.Cells["A1"].Value = _cooperative.CompanyName.ToUpper();
            excelWorksheet.Cells["A2"].Value = _cooperative.Address;
            excelWorksheet.Cells["A5"].Value = string.Format("SCHEDULE OF ACCOUNTS - {0}", _branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();

            for (var i = startRow; i < endRow; i++)
            {
                _currentRow++;
                var codeFilter = (string) excelWorksheet.Cells[i, colJ].Value;
                if (string.IsNullOrEmpty(codeFilter)) continue;

                var parsedCodes = codeFilter.Split(',');
                if (parsedCodes.Length == 0) continue;

                var codeList = parsedCodes.Select(s => s.Replace("\"", "")).ToList();
                var balance = FinancialReportExcelCreator.GetAccountEndingBalance(codeList, _asOf);
                excelWorksheet.Cells[i, colF].Value = balance;

                var percent = (_currentRow / (decimal)_totalRows) * 100m;
                _backgroundWorker.ReportProgress((int)percent);
            }
        }

        private static void ProcessConditionSummary(ExcelWorksheet excelWorksheet)
        {
            excelWorksheet.Cells["A1"].Value = _cooperative.CompanyName.ToUpper();
            excelWorksheet.Cells["A2"].Value = _cooperative.Address;
            excelWorksheet.Cells["A5"].Value = string.Format("STATEMENT FINANCIAL OF CONDITION - {0}", _branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();
        }
    }
}