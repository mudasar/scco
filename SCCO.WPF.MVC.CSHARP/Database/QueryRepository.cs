using System.Data;

namespace SCCO.WPF.MVC.CS.Database
{
    public static class DataRepository
    {
        public static DataTable GetMemberAccountBalanceByAccountCodeData(string accountCode)
        {
            var sqlCommand = GetMemberAccountBalanceByAccountCodeQuery();
            var sqlParameters = new SqlParameter[]
            {
                new SqlParameter("?AccountCode", accountCode)
            };

            return DatabaseController.ExecuteSelectQuery(sqlCommand, sqlParameters);
        }

        #region --- PRIVATES ---
        
        private static string GetMemberAccountBalanceByAccountCodeQuery()
        {
            return @"
SELECT
  a.mem_code AS member_code,
  b.mem_name AS member_name,
  c.`code` AS account_code,
  c.title AS account_title,
  a.cert_no AS certificate_no,
  a.`beg` AS beginning,
  a.`jan` AS january,
  a.`feb` AS february,
  a.`mar` AS march,
  a.`apr` AS april,
  a.`may` AS may,
  a.`jun` AS june,
  a.`jul` AS july,
  a.`aug` AS august,
  a.`sep` AS september,
  a.`oct` AS october,
  a.`nov` AS november,
  a.`dec` AS december
FROM
  (SELECT
    reference_id,
    mem_code,
    acc_code,
    cert_no,
    COALESCE(`beg`, 0) AS `BEG`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) AS `JAN`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) AS `FEB`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) AS `MAR`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) AS `APR`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) AS `MAY`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) AS `JUN`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) AS `JUL`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) + COALESCE(`aug`, 0) AS `AUG`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) + COALESCE(`aug`, 0) + COALESCE(`sep`, 0) AS `SEP`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) + COALESCE(`aug`, 0) + COALESCE(`sep`, 0) + COALESCE(`oct`, 0) AS `OCT`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) + COALESCE(`aug`, 0) + COALESCE(`sep`, 0) + COALESCE(`oct`, 0) + COALESCE(`nov`, 0) AS `NOV`,
    COALESCE(`beg`, 0) + COALESCE(`jan`, 0) + COALESCE(`feb`, 0) + COALESCE(`mar`, 0) + COALESCE(`apr`, 0) + COALESCE(`may`, 0) + COALESCE(`jun`, 0) + COALESCE(`jul`, 0) + COALESCE(`aug`, 0) + COALESCE(`sep`, 0) + COALESCE(`oct`, 0) + COALESCE(`nov`, 0) + COALESCE(`dec`, 0) AS `DEC`
  FROM
    (SELECT
      CONCAT(
        LPAD(mem_code, 10, '0'),
        '-',
        RPAD(acc_code, 10, '0'),
        '-',
        LPAD(cert_no, 10, '0')
      ) AS REFERENCE_ID,
      mem_code,
      acc_code,
      cert_no,
      IF(
        chart.nature = 'D',
        COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
        COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
      ) AS BEG
    FROM
      `slbal`
      INNER JOIN chart
        ON chart.`code` = slbal.acc_code
    WHERE chart.`code` = ?AccountCode
    GROUP BY mem_code,
      acc_code,
      cert_no) AS tbl_beg
    LEFT JOIN
      (SELECT
        reference_id,
        jan
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS JAN
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 1
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 1
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 1) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_jan) AS tbl_jan USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `feb`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `FEB`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 2
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 2
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 2) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_feb) AS tbl_feb USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `mar`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `MAR`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 3
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 3
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 3) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_mar) AS tbl_mar USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `apr`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `APR`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 4
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 4
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 4) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_apr) AS tbl_apr USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `may`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `MAY`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 5
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 5
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 5) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_may) AS tbl_may USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `jun`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `JUN`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 6
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 6
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 6) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_jun) AS tbl_jun USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `jul`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `JUL`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 7
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 7
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 7) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_jul) AS tbl_jul USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `aug`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `AUG`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 8
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 8
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 8) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_aug) AS tbl_aug USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `sep`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `SEP`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 9
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 9
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 9) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_sep) AS tbl_sep USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `oct`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `OCT`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 10
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 10
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 10) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_oct) AS tbl_oct USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `nov`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `NOV`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 11
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 11
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 11) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_nov) AS tbl_nov USING (reference_id)
    LEFT JOIN
      (SELECT
        reference_id,
        `dec`
      FROM
        (SELECT
          CONCAT(
            LPAD(mem_code, 10, '0'),
            '-',
            RPAD(acc_code, 10, '0'),
            '-',
            LPAD(cert_no, 10, '0')
          ) AS REFERENCE_ID,
          mem_code,
          acc_code,
          cert_no,
          IF(
            chart.nature = 'D',
            COALESCE(SUM(debit), 0) - COALESCE(SUM(credit), 0),
            COALESCE(SUM(credit), 0) - COALESCE(SUM(debit), 0)
          ) AS `DEC`
        FROM
          (SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `or`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 12
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `jv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 12
          UNION
          ALL
          SELECT
            mem_code,
            acc_code,
            cert_no,
            debit,
            credit
          FROM
            `cv`
          WHERE acc_code = ?AccountCode
            AND MONTH(doc_date) = 12) AS tbl_merged_by_month
          LEFT JOIN chart
            ON chart.`code` = tbl_merged_by_month.acc_code
        WHERE chart.`code` = ?AccountCode
        GROUP BY mem_code,
          acc_code,
          cert_no) AS tbl_dec) AS tbl_dec USING (reference_id)) AS a
  INNER JOIN nfmb AS b USING (mem_code)
  INNER JOIN chart AS c
    ON a.acc_code = c.`code`;

";
        }

        #endregion
    }
}
