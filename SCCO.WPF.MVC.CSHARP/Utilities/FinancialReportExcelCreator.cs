using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Utilities
{
    public static class FinancialReportExcelCreator
    {
        private static Company _cooperative;
        private static string _branch;
        private static DateTime _asOf;

        public static Result GenerateFinancialStatementReport(DateTime asOf, string output)
        {
            try
            {
                var template = GetFinancialStatementReportsTemplate();
                _cooperative = Company.FirstOrDefault();
                _branch = Properties.Settings.Default.BranchName.ToUpper();
                _asOf = asOf;

                File.Copy(template, output, true);

                var newFile = new FileInfo(output);

                using (var package = new ExcelPackage(newFile))
                {
                    ProcessConditionSummary(package.Workbook.Worksheets[1]);
                    ProcessConditionDetails(package.Workbook.Worksheets[2]);
                    ProcessOperation(package.Workbook.Worksheets[3]);
                    package.Save();
                }
                return new Result(true, "Report created successfully.");
            }
            catch (Exception exception)
            {
                return new Result(false, exception.Message);
            }
        }

        private static void ProcessOperation(ExcelWorksheet excelWorksheet)
        {
            const int maxRow = 80;
            const int startRow = 10;
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

            var branch = Properties.Settings.Default.BranchName.ToUpper();
            excelWorksheet.Cells["A5"].Value = string.Format("STATEMENT FINANCIAL OF OPERATION - {0}", branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();
            excelWorksheet.Cells["B8:G8"].Style.Numberformat.Format = "@"; //Format as text
            excelWorksheet.Cells[8, colD].Value = string.Format("{0} BUDGET", _asOf.Year).ToUpper();
            excelWorksheet.Cells[8, colE].Value = string.Format("{0:MMM yyyy}", previousMonth).ToUpper();
            excelWorksheet.Cells[8, colF].Value = string.Format("{0:MMM yyyy}", _asOf).ToUpper();
            excelWorksheet.Cells[8, colG].Value = "TO DATE";

            for (int i = startRow; i < maxRow; i++)
            {
                var code = (string)excelWorksheet.Cells[i, colB].Value;
                if (string.IsNullOrEmpty(code)) continue;

                excelWorksheet.Cells[i, colD].Value = 0m;
                excelWorksheet.Cells[i, colE].Value = 0m;
                excelWorksheet.Cells[i, colF].Value = 0m;

                var budget = GetBudget(code);
                excelWorksheet.Cells[i, colD].Value = budget;

                var codeFilter = (string)excelWorksheet.Cells[i, colJ].Value;
                if (string.IsNullOrEmpty(codeFilter)) continue;
                var parsedCodes = codeFilter.Split(',');
                if (parsedCodes.Length == 0) continue;

                var codeList = parsedCodes.Select(s => s.Replace("\"", "")).ToList();

                var lastMonthBalance = previousMonth.Year < _asOf.Year
                                           ? GetAccountForwardedBalance(codeList)
                                           : GetAccountSummary(codeList, previousMonth);

                excelWorksheet.Cells[i, colE].Value = lastMonthBalance;

                var currentAmountTotal = GetAccountSummaryBetweenDates(codeList, dateStart, _asOf);
                excelWorksheet.Cells[i, colF].Value = currentAmountTotal;
            }
        }

        private static void ProcessConditionDetails(ExcelWorksheet excelWorksheet)
        {
            const int maxRow = 210;
            const int startRow = 8;
            const int colF = 6; // AMOUNT
            const int colJ = 10; // Account Code Filter

            excelWorksheet.Cells["A1"].Value = _cooperative.CompanyName.ToUpper();
            excelWorksheet.Cells["A2"].Value = _cooperative.Address;
            excelWorksheet.Cells["A5"].Value = string.Format("SCHEDULE OF ACCOUNTS - {0}", _branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();

            for (var i = startRow; i < maxRow; i++)
            {
                var codeFilter = (string)excelWorksheet.Cells[i, colJ].Value;
                if (string.IsNullOrEmpty(codeFilter)) continue;

                var parsedCodes = codeFilter.Split(',');
                if (parsedCodes.Length == 0) continue;

                var codeList = parsedCodes.Select(s => s.Replace("\"", "")).ToList();
                var balance = GetAccountEndingBalance(codeList, _asOf);
                excelWorksheet.Cells[i, colF].Value = balance;
            }
        }

        private static void ProcessConditionSummary(ExcelWorksheet excelWorksheet)
        {
            excelWorksheet.Cells["A1"].Value = _cooperative.CompanyName.ToUpper();
            excelWorksheet.Cells["A2"].Value = _cooperative.Address;
            excelWorksheet.Cells["A5"].Value = string.Format("STATEMENT FINANCIAL OF CONDITION - {0}", _branch);
            excelWorksheet.Cells["A6"].Value = string.Format("AS OF {0:MMMM dd, yyyy}", _asOf).ToUpper();
        }

        internal static string GetStatementOfFinancialOperationReportTemplate()
        {
            const string template = "template_statement_of_financial_operation.xlsx";
            var reportFile = Path.Combine(ReportController.ReportFolder, template);
            return reportFile;
        }

        internal static string GetStatementOfFinancialConditionReportTemplate()
        {
            const string template = "template_statement_of_financial_condition.xlsx";
            var reportFile = Path.Combine(ReportController.ReportFolder, template);
            return reportFile;
        }

        internal static string GetFinancialStatementReportsTemplate()
        {
            const string template = "template_financial_statement_reports.xlsx";
            var reportFile = Path.Combine(ReportController.ReportFolder, template);
            return reportFile;
        }

        #region --- Account Processing ---

        private static decimal GetBudget(string code)
        {
            var sql = GetBudgetPerAccountQuery();
            var param = new SqlParameter("?AccountCode", code);
            var dataTable = DatabaseController.ExecuteSelectQuery(sql, param);
            return dataTable.Rows.Cast<DataRow>().Sum(dataRow => DataConverter.ToDecimal(dataRow["amount"]));
        }

        private static decimal GetAccountSummaryBetweenDates(IEnumerable<string> codeList, DateTime startDate,
                                                             DateTime endDate)
        {
            var sql = GetAccountSummaryBetweenDatesQuery();
            var paramList = new List<SqlParameter>
            {
                new SqlParameter("?DateStart", startDate),
                new SqlParameter("?DateEnd", endDate)
            };
            paramList.AddRange(codeList.Select((t, i) => new SqlParameter("?AccountCode" + i, t)));

            var paramKeys = paramList.Select(sqlParameter => sqlParameter.Key).ToArray();
            var formattedSQL = string.Format(sql, string.Join(",", paramKeys));

            var dataTable = DatabaseController.ExecuteSelectQuery(formattedSQL, paramList.ToArray());
            return dataTable.Rows.Cast<DataRow>().Sum(dataRow => DataConverter.ToDecimal(dataRow["amount"]));
        }

        private static decimal GetAccountSummary(IEnumerable<string> codeList, DateTime asOf)
        {
            var sql = GetAccountSummaryQuery();
            var paramList = new List<SqlParameter> {new SqlParameter("?TransactionDate", asOf)};
            paramList.AddRange(codeList.Select((t, i) => new SqlParameter("?AccountCode" + i, t)));

            var paramKeys = paramList.Select(sqlParameter => sqlParameter.Key).ToArray();
            var formattedSQL = string.Format(sql, string.Join(",", paramKeys));
            var dataTable = DatabaseController.ExecuteSelectQuery(formattedSQL, paramList.ToArray());
            return
                (from DataRow dataRow in dataTable.Rows select DataConverter.ToDecimal(dataRow["amount"]))
                    .FirstOrDefault();
        }

        private static decimal GetAccountForwardedBalance(IEnumerable<string> codeList)
        {
            var sql = GetAccountForwardedBalanceQuery();
            var paramList = new List<SqlParameter>();
            paramList.AddRange(codeList.Select((t, i) => new SqlParameter("?AccountCode" + i, t)));

            var paramKeys = paramList.Select(sqlParameter => sqlParameter.Key).ToArray();
            var formattedSQL = string.Format(sql, string.Join(",", paramKeys));
            var dataTable = DatabaseController.ExecuteSelectQuery(formattedSQL, paramList.ToArray());
            return
                (from DataRow dataRow in dataTable.Rows select DataConverter.ToDecimal(dataRow["amount"]))
                    .FirstOrDefault();
        }

        private static decimal GetAccountEndingBalance(IEnumerable<string> codeList, DateTime asOf)
        {
            var sql = GetAccountEndingBalanceQuery();
            var paramList = new List<SqlParameter> {new SqlParameter("?AsOf", asOf)};
            paramList.AddRange(codeList.Select((t, i) => new SqlParameter("?AccountCode" + i, t)));

            var paramKeys = paramList.Select(sqlParameter => sqlParameter.Key).ToArray();
            var formattedSQL = string.Format(sql, string.Join(",", paramKeys));
            var dataTable = DatabaseController.ExecuteSelectQuery(formattedSQL, paramList.ToArray());
            return
                (from DataRow dataRow in dataTable.Rows select DataConverter.ToDecimal(dataRow["amount"]))
                    .FirstOrDefault();
        }

        #endregion

        #region --- SQL ---

        private static string GetBudgetPerAccountQuery()
        {
            return @"SELECT amount FROM budgets WHERE account_code = ?AccountCode";
        }

        private static string GetAccountSummaryBetweenDatesQuery()
        {
            return @"SELECT 
  a.ACC_CODE AS account_code,
  IF( LEFT(b.Nature, 1) = 'D',
    ( SUM(COALESCE(a.debit, 0)) - SUM(COALESCE(a.credit, 0))),
    ( SUM(COALESCE(a.credit, 0)) - SUM(COALESCE(a.debit, 0)))) AS amount 
FROM
  (SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `or` 
  WHERE ACC_CODE IN ({0}) AND (DOC_DATE BETWEEN ?DateStart AND ?DateEnd)
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `jv` 
	WHERE ACC_CODE IN ({0}) AND (DOC_DATE BETWEEN ?DateStart AND ?DateEnd)
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `cv` 
  WHERE ACC_CODE IN ({0}) AND (DOC_DATE BETWEEN ?DateStart AND ?DateEnd)
) as a
  INNER JOIN chart b 
    ON b.`CODE` = a.ACC_CODE 
GROUP BY a.ACC_CODE ;

";
        }

        private static string GetAccountSummaryQuery()
        {
            return @"SELECT 
  a.ACC_CODE AS account_code,
  IF( LEFT(b.Nature, 1) = 'D',
    ( SUM(COALESCE(a.debit, 0)) - SUM(COALESCE(a.credit, 0))),
    ( SUM(COALESCE(a.credit, 0)) - SUM(COALESCE(a.debit, 0)))) AS amount 
FROM
  (SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `or` 
  WHERE ACC_CODE IN ({0}) AND DOC_DATE <= ?TransactionDate
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `jv` 
	WHERE ACC_CODE IN ({0}) AND DOC_DATE <= ?TransactionDate
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `cv` 
  WHERE ACC_CODE IN ({0}) AND DOC_DATE <= ?TransactionDate
) as a
  INNER JOIN chart b 
    ON b.`CODE` = a.ACC_CODE 
GROUP BY a.ACC_CODE ;

";
        }

        private static string GetAccountForwardedBalanceQuery()
        {
            return @"
SELECT 
  a.ACC_CODE AS account_code,
  IF( LEFT(b.Nature, 1) = 'D',
    ( SUM(COALESCE(a.debit, 0)) - SUM(COALESCE(a.credit, 0))),
    ( SUM(COALESCE(a.credit, 0)) - SUM(COALESCE(a.debit, 0)))) AS amount 
FROM
  (SELECT 
    ACC_CODE,
    COALESCE(DEBIT, 0) AS DEBIT,
    COALESCE(CREDIT, 0) AS CREDIT 
  FROM
    `glbal` 
  WHERE ACC_CODE IN ({0})
) as a
  INNER JOIN chart b 
    ON b.`CODE` = a.ACC_CODE 
GROUP BY a.ACC_CODE ;
";
        }

        private static string GetAccountEndingBalanceQuery()
        {
            return @"
SELECT 
  a.ACC_CODE AS account_code,
  IF(
    LEFT(b.Nature, 1) = 'D',
    (
      SUM(COALESCE(a.debit, 0)) - SUM(COALESCE(a.credit, 0))
    ),
    (
      SUM(COALESCE(a.credit, 0)) - SUM(COALESCE(a.debit, 0))
    )
  ) AS amount 
FROM
  (SELECT 
    ACC_CODE,
    SUM(COALESCE(DEBIT, 0)) AS DEBIT,
    SUM(COALESCE(CREDIT, 0)) AS CREDIT 
  FROM
    glbal 
  WHERE ACC_CODE IN ({0}) 
  GROUP BY ACC_CODE 
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    SUM(COALESCE(DEBIT, 0)) AS DEBIT,
    SUM(COALESCE(CREDIT, 0)) AS CREDIT 
  FROM
    `or` 
  WHERE ACC_CODE IN ({0}) 
    AND DOC_DATE <= ?AsOf
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    SUM(COALESCE(DEBIT, 0)) AS DEBIT,
    SUM(COALESCE(CREDIT, 0)) AS CREDIT 
  FROM
    `jv` 
  WHERE ACC_CODE IN ({0}) 
    AND DOC_DATE <= ?AsOf
  UNION
  ALL 
  SELECT 
    ACC_CODE,
    SUM(COALESCE(DEBIT, 0)) AS DEBIT,
    SUM(COALESCE(CREDIT, 0)) AS CREDIT 
  FROM
    `cv` 
  WHERE ACC_CODE IN ({0}) 
    AND DOC_DATE <= ?AsOf) AS a 
  INNER JOIN chart AS b 
    ON a.ACC_CODE = b.`CODE` ;
";
        } 

        #endregion

        //private void Jea()
        //{
        //    // add a new worksheet to the empty workbook
        //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
        //    //Add the headers
        //    worksheet.Cells[1, 1].Value = "ID";
        //    worksheet.Cells[1, 2].Value = "Product";
        //    worksheet.Cells[1, 3].Value = "Quantity";
        //    worksheet.Cells[1, 4].Value = "Price";
        //    worksheet.Cells[1, 5].Value = "Value";

        //    //Add some items...
        //    worksheet.Cells["A2"].Value = 12001;
        //    worksheet.Cells["B2"].Value = "Nails";
        //    worksheet.Cells["C2"].Value = 37;
        //    worksheet.Cells["D2"].Value = 3.99;

        //    worksheet.Cells["A3"].Value = 12002;
        //    worksheet.Cells["B3"].Value = "Hammer";
        //    worksheet.Cells["C3"].Value = 5;
        //    worksheet.Cells["D3"].Value = 12.10;

        //    worksheet.Cells["A4"].Value = 12003;
        //    worksheet.Cells["B4"].Value = "Saw";
        //    worksheet.Cells["C4"].Value = 12;
        //    worksheet.Cells["D4"].Value = 15.37;

        //    //Add a formula for the value-column
        //    worksheet.Cells["E2:E4"].Formula = "C2*D2";

        //    //Ok now format the values;
        //    using (var range = worksheet.Cells[1, 1, 1, 5])
        //    {
        //        range.Style.Font.Bold = true;
        //        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
        //        range.Style.Font.Color.SetColor(Color.White);
        //    }

        //    worksheet.Cells["A5:E5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //    worksheet.Cells["A5:E5"].Style.Font.Bold = true;

        //    worksheet.Cells[5, 3, 5, 5].Formula = string.Format("SUBTOTAL(9,{0})",
        //        new ExcelAddress(2, 3, 4, 3).Address);
        //    worksheet.Cells["C2:C5"].Style.Numberformat.Format = "#,##0";
        //    worksheet.Cells["D2:E5"].Style.Numberformat.Format = "#,##0.00";

        //    //Create an autofilter for the range
        //    worksheet.Cells["A1:E4"].AutoFilter = true;

        //    worksheet.Cells["A2:A4"].Style.Numberformat.Format = "@"; //Format as text

        //    //There is actually no need to calculate, Excel will do it for you, but in some cases it might be useful. 
        //    //For example if you link to this workbook from another workbook or you will open the workbook in a program that hasn't a calculation engine or 
        //    //you want to use the result of a formula in your program.
        //    worksheet.Calculate();

        //    worksheet.Cells.AutoFitColumns(0); //Autofit columns for all cells

        //    // lets set the header text 
        //    worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
        //    // add the page number to the footer plus the total number of pages
        //    worksheet.HeaderFooter.OddFooter.RightAlignedText =
        //        string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
        //    // add the sheet name to the footer
        //    worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
        //    // add the file path to the footer
        //    worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath +
        //                                                       ExcelHeaderFooter.FileName;

        //    worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
        //    worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];

        //    // Change the sheet view to show it in page layout mode
        //    worksheet.View.PageLayoutView = true;

        //    // set some document properties
        //    package.Workbook.Properties.Title = "Invertory";
        //    package.Workbook.Properties.Author = "Jan K�llman";
        //    package.Workbook.Properties.Comments =
        //        "This sample demonstrates how to create an Excel 2007 workbook using EPPlus";

        //    // set some extended property values
        //    package.Workbook.Properties.Company = "AdventureWorks Inc.";

        //    // set some custom property values
        //    package.Workbook.Properties.SetCustomPropertyValue("Checked by", "Jan K�llman");
        //    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
        //    // save our new workbook and we are done!
        //    package.Save();
        //}
    }
}
