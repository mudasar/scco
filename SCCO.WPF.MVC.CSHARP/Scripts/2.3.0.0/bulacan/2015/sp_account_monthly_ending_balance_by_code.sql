DELIMITER $$

USE `bulacan_2015_production`$$

DROP PROCEDURE IF EXISTS `sp_account_monthly_ending_balance_by_code`$$

CREATE DEFINER = `root` @`localhost` PROCEDURE `sp_account_monthly_ending_balance_by_code` (tc_account_code VARCHAR (10)) 
BEGIN
  # Query the monthly end balance of accounts
  SELECT 
    b.MEM_CODE AS member_code,
    b.MEM_NAME AS member_name,
    c.`CODE` AS account_code,
    c.TITLE AS account_title,
    a.CERT_NO AS certificate_no,
    a.`BEG` AS beginning,
    a.`JAN` AS january,
    a.`FEB` AS february,
    a.`MAR` AS march,
    a.`APR` AS april,
    a.`MAY` AS may,
    a.`JUN` AS june,
    a.`JUL` AS july,
    a.`AUG` AS august,
    a.`SEP` AS september,
    a.`OCT` AS october,
    a.`NOV` AS november,
    a.`DEC` AS december 
  FROM
    (SELECT 
      REFERENCE_ID,
      MEM_CODE,
      ACC_CODE,
      CERT_NO,
      COALESCE(`BEG`, 0) AS `BEG`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) AS `JAN`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) AS `FEB`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) AS `MAR`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) AS `APR`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) AS `MAY`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) AS `JUN`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) AS `JUL`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) + COALESCE(`AUG`, 0) AS `AUG`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) + COALESCE(`AUG`, 0) + COALESCE(`SEP`, 0) AS `SEP`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) + COALESCE(`AUG`, 0) + COALESCE(`SEP`, 0) + COALESCE(`OCT`, 0) AS `OCT`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) + COALESCE(`AUG`, 0) + COALESCE(`SEP`, 0) + COALESCE(`OCT`, 0) + COALESCE(`NOV`, 0) AS `NOV`,
      COALESCE(`BEG`, 0) + COALESCE(`JAN`, 0) + COALESCE(`FEB`, 0) + COALESCE(`MAR`, 0) + COALESCE(`APR`, 0) + COALESCE(`MAY`, 0) + COALESCE(`JUN`, 0) + COALESCE(`JUL`, 0) + COALESCE(`AUG`, 0) + COALESCE(`SEP`, 0) + COALESCE(`OCT`, 0) + COALESCE(`NOV`, 0) + COALESCE(`DEC`, 0) AS `DEC` 
    FROM
      (########## BEGINNING BALANCE ##########
      SELECT 
        CONCAT(
          LPAD(MEM_CODE, 10, '0'),
          '-',
          RPAD(ACC_CODE, 10, '0'),
          '-',
          LPAD(COALESCE(CERT_NO, ''), 10, '0')
        ) AS REFERENCE_ID,
        MEM_CODE,
        ACC_CODE,
        CERT_NO,
        IF(
          chart.NATURE = 'D',
          SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
          SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
        ) AS BEG 
      FROM
        (########## INCLUDE ALL MEMBERS HAVING THAT ACCOUNT ##########
        SELECT 
          MEM_CODE,
          ACC_CODE,
          '' AS CERT_NO,
          0 AS DEBIT,
          0 AS CREDIT 
        FROM
          (SELECT 
            slbal.MEM_CODE,
            slbal.ACC_CODE 
          FROM
            slbal 
            LEFT JOIN chart 
              ON slbal.ACC_CODE = chart.`CODE` 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY slbal.MEM_CODE,
            slbal.ACC_CODE 
          UNION
          ALL 
          SELECT 
            cv.MEM_CODE,
            cv.ACC_CODE 
          FROM
            cv 
            LEFT JOIN chart 
              ON cv.ACC_CODE = chart.`CODE` 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY cv.MEM_CODE,
            cv.ACC_CODE 
          UNION
          ALL 
          SELECT 
            jv.MEM_CODE,
            jv.ACC_CODE 
          FROM
            jv 
            LEFT JOIN chart 
              ON jv.ACC_CODE = chart.`CODE` 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY jv.MEM_CODE,
            jv.ACC_CODE 
          UNION
          ALL 
          SELECT 
            `or`.MEM_CODE,
            `or`.ACC_CODE 
          FROM
            `or` 
            LEFT JOIN chart 
              ON `or`.ACC_CODE = chart.`CODE` 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY `or`.MEM_CODE,
            `or`.ACC_CODE) AS lookup ########## INCLUDE ALL MEMBERS HAVING THAT ACCOUNT ##########
        UNION
        ALL 
        SELECT 
          MEM_CODE,
          ACC_CODE,
          CERT_NO,
          DEBIT,
          CREDIT 
        FROM
          `slbal` 
        WHERE slbal.ACC_CODE = tc_account_code) AS slbal 
        INNER JOIN chart 
          ON chart.`CODE` = slbal.ACC_CODE 
      WHERE chart.`CODE` = tc_account_code 
      GROUP BY MEM_CODE,
        ACC_CODE,
        CERT_NO ########## BEGINNING BALANCE ##########
      ) AS tbl_beg 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          JAN 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS JAN 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 1 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 1 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 1) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_jan) AS tbl_jan USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `FEB` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `FEB` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 2 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 2 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 2) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_feb) AS tbl_feb USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `MAR` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `MAR` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 3 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 3 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 3) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_mar) AS tbl_mar USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `APR` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `APR` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 4 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 4 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 4) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_apr) AS tbl_apr USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `MAY` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `MAY` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 5 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 5 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 5) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_may) AS tbl_may USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `JUN` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `JUN` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 6 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 6 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 6) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_jun) AS tbl_jun USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `JUL` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `JUL` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 7 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 7 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 7) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_jul) AS tbl_jul USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `AUG` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `AUG` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 8 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 8 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 8) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_aug) AS tbl_aug USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `SEP` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `SEP` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 9 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 9 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 9) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_sep) AS tbl_sep USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `OCT` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `OCT` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 10 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 10 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 10) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_oct) AS tbl_oct USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `NOV` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `NOV` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 11 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 11 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 11) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_nov) AS tbl_nov USING (REFERENCE_ID) 
      LEFT JOIN 
        (SELECT 
          REFERENCE_ID,
          `DEC` 
        FROM
          (SELECT 
            CONCAT(
              LPAD(MEM_CODE, 10, '0'),
              '-',
              RPAD(ACC_CODE, 10, '0'),
              '-',
              LPAD(COALESCE(CERT_NO, ''), 10, '0')
            ) AS REFERENCE_ID,
            MEM_CODE,
            ACC_CODE,
            CERT_NO,
            IF(
              chart.NATURE = 'D',
              SUM(COALESCE(DEBIT, 0)) - SUM(COALESCE(CREDIT, 0)),
              SUM(COALESCE(CREDIT, 0)) - SUM(COALESCE(DEBIT, 0))
            ) AS `DEC` 
          FROM
            (SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `or` 
            WHERE MONTH(DOC_DATE) = 12 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `jv` 
            WHERE MONTH(DOC_DATE) = 12 
            UNION
            ALL 
            SELECT 
              MEM_CODE,
              ACC_CODE,
              CERT_NO,
              DEBIT,
              CREDIT 
            FROM
              `cv` 
            WHERE MONTH(DOC_DATE) = 12) AS tbl_merged_by_month 
            LEFT JOIN chart 
              ON chart.`CODE` = tbl_merged_by_month.ACC_CODE 
          WHERE chart.`CODE` = tc_account_code 
          GROUP BY MEM_CODE,
            ACC_CODE,
            CERT_NO) AS tbl_dec) AS tbl_dec USING (REFERENCE_ID)) AS a 
    INNER JOIN nfmb AS b 
      ON a.MEM_CODE = b.MEM_CODE 
    INNER JOIN chart AS c 
      ON a.ACC_CODE = c.`CODE` ;
END $$

DELIMITER ;