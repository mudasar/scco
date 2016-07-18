using System;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Models;
using System.Data;

namespace SCCO.WPF.MVC.CS.Views.ReportsModule
{
    public partial class ScheduleOfAccountsView
    {
        private readonly DateTime _asOf = MainController.LoggedUser.TransactionDate;

        public ScheduleOfAccountsView()
        {
            InitializeComponent();
            SubTitleLabel.Content = string.Format("As of {0:MMMM dd, yyyy}", _asOf);
            InitializeDisplay();
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            Button1.Click += (s, e) => ShowScheduleReport(1, Button1.Content.ToString());
            Button2.Click += (s, e) => ShowScheduleReport(2, Button2.Content.ToString());
            Button3.Click += (s, e) => ShowScheduleReport(3, Button3.Content.ToString());
            Button4.Click += (s, e) => ShowScheduleReport(4, Button4.Content.ToString());
            Button5.Click += (s, e) => ShowScheduleReport(5, Button5.Content.ToString());
            Button6.Click += (s, e) => ShowScheduleReport(6, Button6.Content.ToString());
            Button7.Click += (s, e) => ShowScheduleReport(7, Button7.Content.ToString());
            Button8.Click += (s, e) => ShowScheduleReport(8, Button8.Content.ToString());
            Button9.Click += (s, e) => ShowScheduleReport(9, Button9.Content.ToString());
            Button10.Click += (s, e) => ShowScheduleReport(10, Button10.Content.ToString());
        }

        private void InitializeDisplay()
        {
            for (var i = 1; i <= 10; i++)
            {
                var account = Account.FindByScheduleCode(i);

                if(account == null) continue;

                switch (i)
                {
                    case 1:
                        Button1.Content = account.AccountTitle;
                        break;
                    case 2:
                        Button2.Content = account.AccountTitle;
                        break;
                    case 3:
                        Button3.Content = account.AccountTitle;
                        break;
                    case 4:
                        Button4.Content = account.AccountTitle;
                        break;
                    case 5:
                        Button5.Content = account.AccountTitle;
                        break;
                    case 6:
                        Button6.Content = account.AccountTitle;
                        break;
                    case 7:
                        Button7.Content = account.AccountTitle;
                        break;
                    case 8:
                        Button8.Content = account.AccountTitle;
                        break;
                    case 9:
                        Button9.Content = account.AccountTitle;
                        break;
                    case 10:
                        Button10.Content = account.AccountTitle;
                        break;
                }
            }
        }

        private void ShowScheduleReport(int scheduleNo, string accountTitle)
        {
            var reportTitle = string.Format("Schedule Of {1} As Of {0:MMMM dd, yyyy}", _asOf, accountTitle);

            string sql = ByCodeRadioButton.IsChecked == true
                             ? GetScheduleOfAccountsQueryByMemberCode()
                             : GetScheduleOfAccountsQueryByMemberName();

            var parameters = new[]
            {
                new SqlParameter("?AsOf", _asOf),
                new SqlParameter("?ScheduleNo", scheduleNo)
            };
            var reportTable = DatabaseController.ExecuteSelectQuery(sql, parameters);
            reportTable.TableName = "schedule";

            var comp = Company.GetData();
            comp.TableName = "comp";

            var dataSet = new DataSet();
            dataSet.Tables.Add(reportTable);
            dataSet.Tables.Add(comp);

            var ri = new ReportItem
            {
                ReportFile = "schedule.rpt",
                Title = reportTitle,
                DataSource = dataSet
            };
            var result = ri.LoadReport();
            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }

        private static string GetScheduleOfAccountsQueryByMemberCode()
        {
            return @"
SELECT 
  `sl_merged`.MEM_CODE AS member_code,
  `sl_merged`.MEM_NAME AS member_name,
  `sl_merged`.ACC_CODE AS account_code,
  `sl_merged`.TITLE AS account_title,
  SUM(`sl_merged`.END_BAL) AS ending_balance,
  ?AsOf AS as_of 
FROM
  (SELECT 
    `slbal`.MEM_CODE,
    `slbal`.MEM_NAME,
    `slbal`.ACC_CODE,
    `slbal`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`slbal`.DEBIT, 0)) - SUM(COALESCE(`slbal`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`slbal`.CREDIT, 0)) - SUM(COALESCE(`slbal`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `slbal` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `slbal`.ACC_CODE = ch.CODE 
  GROUP BY `slbal`.MEM_CODE,
    `slbal`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `or`.MEM_CODE,
    `or`.MEM_NAME,
    `or`.ACC_CODE,
    `or`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`or`.DEBIT, 0)) - SUM(COALESCE(`or`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`or`.CREDIT, 0)) - SUM(COALESCE(`or`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `or` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `or`.ACC_CODE = ch.CODE 
  WHERE `or`.DOC_DATE <= ?AsOf 
  GROUP BY `or`.MEM_CODE,
    `or`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `jv`.MEM_CODE,
    `jv`.MEM_NAME,
    `jv`.ACC_CODE,
    `jv`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`jv`.DEBIT, 0)) - SUM(COALESCE(`jv`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`jv`.CREDIT, 0)) - SUM(COALESCE(`jv`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `jv` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `jv`.ACC_CODE = ch.CODE 
  WHERE `jv`.DOC_DATE <= ?AsOf 
  GROUP BY `jv`.MEM_CODE,
    `jv`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `cv`.MEM_CODE,
    `cv`.MEM_NAME,
    `cv`.ACC_CODE,
    `cv`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`cv`.DEBIT, 0)) - SUM(COALESCE(`cv`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`cv`.CREDIT, 0)) - SUM(COALESCE(`cv`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `cv` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `cv`.ACC_CODE = ch.CODE 
  WHERE `cv`.DOC_DATE <= ?AsOf 
  GROUP BY `cv`.MEM_CODE,
    `cv`.ACC_CODE) AS sl_merged 
GROUP BY `sl_merged`.MEM_CODE,
  `sl_merged`.ACC_CODE 
HAVING SUM(`sl_merged`.END_BAL) <> 0 
ORDER BY `sl_merged`.MEM_CODE 
";
        }

        private static string GetScheduleOfAccountsQueryByMemberName()
        {
            return @"
SELECT 
  `sl_merged`.MEM_CODE AS member_code,
  `sl_merged`.MEM_NAME AS member_name,
  `sl_merged`.ACC_CODE AS account_code,
  `sl_merged`.TITLE AS account_title,
  SUM(`sl_merged`.END_BAL) AS ending_balance,
  ?AsOf AS as_of 
FROM
  (SELECT 
    `slbal`.MEM_CODE,
    `slbal`.MEM_NAME,
    `slbal`.ACC_CODE,
    `slbal`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`slbal`.DEBIT, 0)) - SUM(COALESCE(`slbal`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`slbal`.CREDIT, 0)) - SUM(COALESCE(`slbal`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `slbal` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `slbal`.ACC_CODE = ch.CODE 
  GROUP BY `slbal`.MEM_CODE,
    `slbal`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `or`.MEM_CODE,
    `or`.MEM_NAME,
    `or`.ACC_CODE,
    `or`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`or`.DEBIT, 0)) - SUM(COALESCE(`or`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`or`.CREDIT, 0)) - SUM(COALESCE(`or`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `or` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `or`.ACC_CODE = ch.CODE 
  WHERE `or`.DOC_DATE <= ?AsOf 
  GROUP BY `or`.MEM_CODE,
    `or`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `jv`.MEM_CODE,
    `jv`.MEM_NAME,
    `jv`.ACC_CODE,
    `jv`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`jv`.DEBIT, 0)) - SUM(COALESCE(`jv`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`jv`.CREDIT, 0)) - SUM(COALESCE(`jv`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `jv` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `jv`.ACC_CODE = ch.CODE 
  WHERE `jv`.DOC_DATE <= ?AsOf 
  GROUP BY `jv`.MEM_CODE,
    `jv`.ACC_CODE 
  UNION
  ALL 
  SELECT 
    `cv`.MEM_CODE,
    `cv`.MEM_NAME,
    `cv`.ACC_CODE,
    `cv`.TITLE,
    IF(
      ch.Nature = 'D',
      (
        SUM(COALESCE(`cv`.DEBIT, 0)) - SUM(COALESCE(`cv`.CREDIT, 0))
      ),
      (
        SUM(COALESCE(`cv`.CREDIT, 0)) - SUM(COALESCE(`cv`.DEBIT, 0))
      )
    ) AS END_BAL 
  FROM
    `cv` 
    INNER JOIN 
      (SELECT 
        * 
      FROM
        chart 
      WHERE SCODE = ?ScheduleNo) AS ch 
      ON `cv`.ACC_CODE = ch.CODE 
  WHERE `cv`.DOC_DATE <= ?AsOf 
  GROUP BY `cv`.MEM_CODE,
    `cv`.ACC_CODE) AS sl_merged 
GROUP BY `sl_merged`.MEM_CODE,
  `sl_merged`.ACC_CODE 
HAVING SUM(`sl_merged`.END_BAL) <> 0 
ORDER BY `sl_merged`.MEM_NAME
";
        }
    }
}