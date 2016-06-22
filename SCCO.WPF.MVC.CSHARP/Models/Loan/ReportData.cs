using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SCCO.WPF.MVC.CS.Database;
using SCCO.WPF.MVC.CS.Utilities;

namespace SCCO.WPF.MVC.CS.Models.Loan
{
    public class ReportData
    {
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string AccountCode { get; set; }
        public string AccountTitle { get; set; }

        public string DocumentType { get; set; }
        public DateTime DocumentDate { get; set; }
        public int DocumentNumber { get; set; }

        public int ReleaseNumber { get; set; }
        public DateTime ReleasedDate { get; set; }

        public decimal LoanAmount { get; set; }
        public int Terms { get; set; }
        public string TermsMode { get; set; }
        public string ModeOfPayment { get; set; }

        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal InterestAmortization { get; set; }
        public decimal Payment { get; set; }

        public DateTime GrantedDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime CutOffDate { get; set; }

        public string CoMakerCode1 { get; set; }
        public string CoMakerName1 { get; set; }

        public string CoMakerCode2 { get; set; }
        public string CoMakerName2 { get; set; }

        public string CoMakerCode3 { get; set; }
        public string CoMakerName3 { get; set; }

        public string CoMakerCode4 { get; set; }
        public string CoMakerName4 { get; set; }

        public string CoMakerCode5 { get; set; }
        public string CoMakerName5 { get; set; }


        // Current/Overdue
        public string Status { get; set; }
        public int Age { get; set; }
        public int AgeCategoryID { get; set; }
        public string AgeCategoryDescription { get; set; }

        public decimal EndingBalance { get; set; }
        public DateTime ReportDate { get; set; }
        public string AreaCode { get; set; }
        public decimal Deposits { get; set; }

        public static List<ReportData> GetLoanDetails(DateTime asOf)
        {
            // area
            var areas = GetMembersWithAreaCode();

            // loan details
            var latestLoanDetails = GetLatestLoanDetails(asOf);

            // age bracket
            var loanAgeCategories = GetLoanAgesCategories();

            var result = new List<ReportData>();
            var endingBalance = GetEndingBalance(asOf);
            foreach (DataRow dataRow in endingBalance.Rows)
            {
                var item = new ReportData();
                item.MemberCode = DataConverter.ToString(dataRow["member_code"]);
                item.MemberName = DataConverter.ToString(dataRow["member_name"]);
                item.AccountCode = DataConverter.ToString(dataRow["account_code"]);
                item.AccountTitle = DataConverter.ToString(dataRow["account_title"]);
                item.EndingBalance = DataConverter.ToDecimal(dataRow["ending_balance"]);
                item.ReportDate = asOf;

                // area
                var area = areas.Select(string.Format("MEM_CODE = '{0}'", item.MemberCode));
                if (area.Length > 0)
                {
                    item.AreaCode = DataConverter.ToString(area[0]["AREA_CODE"]);
                }

                // loan details
                var loanDetail =
                    latestLoanDetails.Select(string.Format("MEM_CODE = '{0}' AND ACC_CODE = '{1}'", item.MemberCode,
                                                           item.AccountCode));
                if (loanDetail.Length > 0)
                {
                    item.DocumentDate = DataConverter.ToDateTime(loanDetail[0]["DOC_DATE"]);
                    item.DocumentType = DataConverter.ToString(loanDetail[0]["DOC_TYPE"]);
                    item.DocumentNumber = DataConverter.ToInteger(loanDetail[0]["DOC_NUM"]);
                    item.ReleaseNumber = DataConverter.ToInteger(loanDetail[0]["RELEASE_NO"]);
                    item.ReleasedDate = DataConverter.ToDateTime(loanDetail[0]["RELEASED"]);

                    item.LoanAmount = DataConverter.ToDecimal(loanDetail[0]["LOAN_AMT"]);
                    item.Terms = DataConverter.ToInteger(loanDetail[0]["TERMS"]);
                    item.TermsMode = DataConverter.ToString(loanDetail[0]["TERMS_MODE"]);
                    item.ModeOfPayment = DataConverter.ToModePay(loanDetail[0]["MODE_PAY"]);

                    item.GrantedDate = DataConverter.ToDateTime(loanDetail[0]["DATE_GRANT"]);
                    item.MaturityDate = DataConverter.ToDateTime(loanDetail[0]["MATURITY"]);

                    if (item.MaturityDate >= asOf)
                    {
                        item.Status = "Current";
                        item.Age = (asOf - item.GrantedDate).Days;
                    }
                    else
                    {
                        item.Status = "Overdue";
                        item.Age = (asOf - item.MaturityDate).Days;
                    }

                    var ageCategory = loanAgeCategories.Select(string.Format("{0} >= min AND {0} <= max", item.Age));
                    if (ageCategory.Length > 0)
                    {
                        item.AgeCategoryID = DataConverter.ToInteger(ageCategory[0]["id"]);
                        item.AgeCategoryDescription = DataConverter.ToString(ageCategory[0]["description"]);
                    }
                    item.CutOffDate = DataConverter.ToDateTime(loanDetail[0]["CUT_OFF"]);

                    item.Payment = DataConverter.ToDecimal(loanDetail[0]["PAYMENT"]);
                    item.InterestRate = DataConverter.ToDecimal(loanDetail[0]["INT_RATE"]);
                    item.InterestAmount = DataConverter.ToDecimal(loanDetail[0]["INT_AMT"]);
                    item.InterestAmortization = DataConverter.ToDecimal(loanDetail[0]["INT_AMORT"]);

                    item.CoMakerCode1 = DataConverter.ToString(loanDetail[0]["CO_CODE1"]);
                    item.CoMakerName1 = DataConverter.ToString(loanDetail[0]["CO_NAME1"]);
                    item.CoMakerCode2 = DataConverter.ToString(loanDetail[0]["CO_CODE2"]);
                    item.CoMakerName2 = DataConverter.ToString(loanDetail[0]["CO_NAME2"]);
                    item.CoMakerCode3 = DataConverter.ToString(loanDetail[0]["CO_CODE3"]);
                    item.CoMakerName3 = DataConverter.ToString(loanDetail[0]["CO_NAME3"]);
                }

                result.Add(item);
            }
            return result;
        }

        public static List<ReportData> GetLoanReleases()
        {
            var loanReleased = DatabaseController.ExecuteSelectQuery(GetLoanReleasesQuery());
            return (from DataRow dataRow in loanReleased.Rows
                    select new ReportData
                    {
                        MemberCode = DataConverter.ToString(dataRow["MEM_CODE"]),
                        MemberName = DataConverter.ToString(dataRow["MEM_NAME"]),
                        AccountCode = DataConverter.ToString(dataRow["ACC_CODE"]),
                        AccountTitle = DataConverter.ToString(dataRow["TITLE"]),
                        DocumentDate = DataConverter.ToDateTime(dataRow["DOC_DATE"]),
                        DocumentType = DataConverter.ToString(dataRow["DOC_TYPE"]),
                        DocumentNumber = DataConverter.ToInteger(dataRow["DOC_NUM"]),
                        ReleaseNumber = DataConverter.ToInteger(dataRow["RELEASE_NO"]),
                        ReleasedDate = DataConverter.ToDateTime(dataRow["RELEASED"]),
                        LoanAmount = DataConverter.ToDecimal(dataRow["LOAN_AMT"]),
                        Terms = DataConverter.ToInteger(dataRow["TERMS"]),
                        TermsMode = DataConverter.ToString(dataRow["TERMS_MODE"]),
                        ModeOfPayment = DataConverter.ToModePay(dataRow["MODE_PAY"]),
                        GrantedDate = DataConverter.ToDateTime(dataRow["DATE_GRANT"]),
                        MaturityDate = DataConverter.ToDateTime(dataRow["MATURITY"]),
                        CutOffDate = DataConverter.ToDateTime(dataRow["CUT_OFF"]),
                        Payment = DataConverter.ToDecimal(dataRow["PAYMENT"]),
                        InterestRate = DataConverter.ToDecimal(dataRow["INT_RATE"]),
                        InterestAmount = DataConverter.ToDecimal(dataRow["INT_AMT"]),
                        InterestAmortization = DataConverter.ToDecimal(dataRow["INT_AMORT"]),
                        CoMakerCode1 = DataConverter.ToString(dataRow["CO_CODE1"]),
                        CoMakerName1 = DataConverter.ToString(dataRow["CO_NAME1"]),
                        CoMakerCode2 = DataConverter.ToString(dataRow["CO_CODE2"]),
                        CoMakerName2 = DataConverter.ToString(dataRow["CO_NAME2"]),
                        CoMakerCode3 = DataConverter.ToString(dataRow["CO_CODE3"]),
                        CoMakerName3 = DataConverter.ToString(dataRow["CO_NAME3"])
                    }).ToList();
        }

        private static DataTable GetEndingBalance(DateTime asOf)
        {
            var parameter = new[] { new SqlParameter("?asOf", asOf) };
            return DatabaseController.ExecuteSelectQuery(GetEndingBalanceQuery(), parameter);
        }

        private static DataTable GetLatestLoanDetails(DateTime asOf)
        {
            var parameter = new[] {new SqlParameter("?asOf", asOf)};
            return DatabaseController.ExecuteSelectQuery(GetLatestLoanDetailsQuery(), parameter);
        }

        private static DataTable GetMembersWithAreaCode()
        {
            return DatabaseController.ExecuteSelectQuery("SELECT MEM_CODE, AREA_CODE FROM nfmb");
        }

        private static DataTable GetLoanAgesCategories()
        {
            return DatabaseController.ExecuteSelectQuery("SELECT * FROM `loan_age_bracket`");
        }

        #region --- SQL ---

        private static string GetLatestLoanDetailsQuery()
        {
            return
                @"
SELECT 
  t1.* 
FROM
  (SELECT 
    MEM_CODE, MEM_NAME, ACC_CODE, TITLE, DOC_DATE, DOC_TYPE, DOC_NUM, RELEASE_NO, LOAN_AMT, TERMS,
    TERMS_MODE, MODE_PAY1 AS MODE_PAY, DATE_GRANT, MATURITY, CUT_OFF, PAYMENT, INT_RATE, INT_AMT,
    INT_AMORT, RELEASED, CO_CODE1, CO_NAME1, CO_CODE2, CO_NAME2, CO_CODE3, CO_NAME3 
  FROM
    slbal 
  WHERE LOAN_AMT > 0 
    AND DOC_DATE <= ?asOf 
  UNION
  ALL 
  SELECT 
    MEM_CODE, MEM_NAME, ACC_CODE, TITLE, DOC_DATE, DOC_TYPE, DOC_NUM, RELEASE_NO, LOAN_AMT, TERMS,
    TERMS_MODE, MODE_PAY, DATE_GRANT, MATURITY, CUT_OFF, PAYMENT, INT_RATE, INT_AMT, INT_AMORT,
    RELEASED, CO_CODE1, CO_NAME1, CO_CODE2, CO_NAME2, CO_CODE3, CO_NAME3 
  FROM
    jv 
  WHERE LOAN_AMT > 0 
    AND DOC_DATE <= ?asOf 
  UNION
  ALL 
  SELECT 
    MEM_CODE, MEM_NAME, ACC_CODE, TITLE, DOC_DATE, DOC_TYPE, DOC_NUM, RELEASE_NO, LOAN_AMT, TERMS,
    TERMS_MODE, MODE_PAY, DATE_GRANT, MATURITY, CUT_OFF, PAYMENT, INT_RATE, INT_AMT, INT_AMORT,
    RELEASED, CO_CODE1, CO_NAME1, CO_CODE2, CO_NAME2, CO_CODE3, CO_NAME3 
  FROM
    cv 
  WHERE LOAN_AMT > 0 
    AND DOC_DATE <= ?asOf 
  ORDER BY MEM_CODE,
    ACC_CODE) t1 
  JOIN 
    (SELECT 
      t.MEM_CODE,
      t.ACC_CODE,
      MAX(DOC_DATE) AS DOC_DATE 
    FROM
      (SELECT 
        MEM_CODE,  ACC_CODE, DOC_DATE 
      FROM
        slbal 
      WHERE LOAN_AMT > 0 
        AND DOC_DATE <= ?asOf 
      UNION
      ALL 
      SELECT 
        MEM_CODE, ACC_CODE, DOC_DATE 
      FROM
        jv 
      WHERE LOAN_AMT > 0 
        AND DOC_DATE <= ?asOf 
      UNION
      ALL 
      SELECT 
        MEM_CODE, ACC_CODE, DOC_DATE 
      FROM
        cv 
      WHERE LOAN_AMT > 0 
        AND DOC_DATE <= ?asOf 
      ORDER BY MEM_CODE,
        ACC_CODE) t 
    GROUP BY t.MEM_CODE,
      t.ACC_CODE) t2 
    ON t1.MEM_CODE = t2.MEM_CODE 
    AND t1.ACC_CODE = t2.ACC_CODE 
    AND t1.DOC_DATE = t2.DOC_DATE 
";
        }

        private static string GetLoanReleasesQuery()
        {
            return @"
SELECT 
    MEM_CODE, MEM_NAME, ACC_CODE, TITLE, DOC_DATE, DOC_TYPE, DOC_NUM, RELEASE_NO, LOAN_AMT, TERMS,
    TERMS_MODE, MODE_PAY, DATE_GRANT, MATURITY, CUT_OFF, PAYMENT, INT_RATE, INT_AMT, INT_AMORT,
    RELEASED, CO_CODE1, CO_NAME1, CO_CODE2, CO_NAME2, CO_CODE3, CO_NAME3
  FROM
    cv 
	INNER JOIN (SELECT `CODE` FROM chart WHERE CODE1 = 'LR') as ch
	ON `cv`.ACC_CODE = ch.`CODE`
  WHERE LOAN_AMT > 0
UNION ALL
SELECT 
    MEM_CODE, MEM_NAME, ACC_CODE, TITLE, DOC_DATE, DOC_TYPE, DOC_NUM, RELEASE_NO, LOAN_AMT, TERMS,
    TERMS_MODE, MODE_PAY, DATE_GRANT, MATURITY, CUT_OFF, PAYMENT, INT_RATE, INT_AMT, INT_AMORT,
    RELEASED, CO_CODE1, CO_NAME1, CO_CODE2, CO_NAME2, CO_CODE3, CO_NAME3
  FROM
    jv 
	INNER JOIN (SELECT `CODE` FROM chart WHERE CODE1 = 'LR') as ch
	ON `jv`.ACC_CODE = ch.`CODE`
  WHERE LOAN_AMT > 0
";
        }

        private static string GetTotalDepositPerMemberQuery()
        {
            return @"
SELECT 
  MEM_CODE,
  MEM_NAME,
  SUM(END_BAL) AS DEPOSIT 
FROM
  (SELECT 
    `slbal`.MEM_CODE,
    `slbal`.MEM_NAME,
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
      WHERE CODE1 IN ('SA', 'TD', 'SC')) AS ch 
      ON `slbal`.ACC_CODE = ch.CODE 
  GROUP BY `slbal`.MEM_CODE 
  UNION
  ALL 
  SELECT 
    `cv`.MEM_CODE,
    `cv`.MEM_NAME,
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
      WHERE CODE1 IN ('SA', 'TD', 'SC')) AS ch 
      ON `cv`.ACC_CODE = ch.`CODE` 
  WHERE `cv`.DOC_DATE <= ?asOf 
  GROUP BY `cv`.MEM_CODE 
  UNION
  ALL 
  SELECT 
    `jv`.MEM_CODE,
    `jv`.MEM_NAME,
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
      WHERE CODE1 IN ('SA', 'TD', 'SC')) AS ch 
      ON `jv`.ACC_CODE = ch.`CODE` 
  WHERE `jv`.DOC_DATE <= ?asOf 
  GROUP BY `jv`.MEM_CODE 
  UNION
  ALL 
  SELECT 
    `or`.MEM_CODE,
    `or`.MEM_NAME,
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
      WHERE CODE1 IN ('SA', 'TD', 'SC')) AS ch 
      ON `or`.ACC_CODE = ch.`CODE` 
  WHERE `or`.DOC_DATE <= ?asOf 
  GROUP BY `or`.MEM_CODE) AS summary 
GROUP BY MEM_CODE 
HAVING DEPOSIT <> 0 
";
        }

        private static string GetEndingBalanceQuery()
        {
            return @"
SELECT 
  `sl_merged`.MEM_CODE AS member_code,
  `sl_merged`.MEM_NAME AS member_name,
  `sl_merged`.ACC_CODE AS account_code,
  `sl_merged`.TITLE AS account_title,
  SUM(`sl_merged`.END_BAL) AS ending_balance,
  ?asOf AS as_of 
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
      WHERE CODE1 = 'LR') AS ch 
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
      WHERE CODE1 = 'LR') AS ch 
      ON `or`.ACC_CODE = ch.CODE 
  WHERE `or`.DOC_DATE <= ?asOf 
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
      WHERE CODE1 = 'LR') AS ch 
      ON `jv`.ACC_CODE = ch.CODE 
  WHERE `jv`.DOC_DATE <= ?asOf 
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
      WHERE CODE1 = 'LR') AS ch 
      ON `cv`.ACC_CODE = ch.CODE 
  WHERE `cv`.DOC_DATE <= ?asOf  
  GROUP BY `cv`.MEM_CODE,
    `cv`.ACC_CODE) AS sl_merged 
GROUP BY `sl_merged`.MEM_CODE,
  `sl_merged`.ACC_CODE 
HAVING SUM(`sl_merged`.END_BAL) <> 0 
ORDER BY `sl_merged`.MEM_CODE 
";
        }

        #endregion
    }
}