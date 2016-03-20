DELIMITER $$

USE `polo_2016_production`$$

DROP PROCEDURE IF EXISTS `sp_notice_loan_overdue`$$

CREATE DEFINER = `root` @`localhost` PROCEDURE `sp_notice_loan_overdue` (as_of DATE) 
BEGIN
  #This will return loans that will due within 30 days from a given date
# create a temporary table that will hold the CURRENT loan details information as of given date
  SET @prev = "" ;
  SET @rownum = 0 ;
  DROP TEMPORARY TABLE IF EXISTS `tmp_loan_details` ;
  CREATE TEMPORARY TABLE tmp_loan_details 
  SELECT 
    * 
  FROM
    (SELECT 
      IF(
        @prev != CONCAT_WS(
          "-",
          loan_details.member_code,
          loan_details.account_code
        ),
        @rownum := 1,
        @rownum := @rownum + 1
      ) AS group_precedence,
      @prev := CONCAT_WS(
        "-",
        loan_details.member_code,
        loan_details.account_code
      ) AS group_code,
      loan_details.* 
    FROM
      (SELECT 
        `slbal_loan`.`MEM_CODE` AS `member_code`,
        `slbal_loan`.`MEM_NAME` AS `member_name`,
        `slbal_loan`.`ACC_CODE` AS `account_code`,
        `slbal_loan`.`TITLE` AS `account_title`,
        `slbal_loan`.`DOC_DATE` AS `document_date`,
        `slbal_loan`.`DOC_TYPE` AS `document_type`,
        `slbal_loan`.`DOC_NUM` AS `document_number`,
        `slbal_loan`.`RELEASE_NO` AS `release_number`,
        `slbal_loan`.`LOAN_AMT` AS `loan_amount`,
        `slbal_loan`.`TERMS1` AS `terms`,
        `slbal_loan`.`TERMS_MODE` AS `terms_mode`,
        `slbal_loan`.`MODE_PAY1` AS `payment_mode`,
        `slbal_loan`.`DATE_GRANT` AS `date_granted`,
        `slbal_loan`.`MATURITY` AS `date_maturity`,
        `slbal_loan`.`CUT_OFF` AS `date_cut_off`,
        `slbal_loan`.`PAYMENT` AS `payment`,
        `slbal_loan`.`INT_RATE` AS `interest_rate`,
        `slbal_loan`.`INT_AMT` AS `interest_amount`,
        `slbal_loan`.`INT_AMORT` AS `interest_amortization`,
        `slbal_loan`.`APPROVED` AS `date_approved`,
        `slbal_loan`.`CANCELLED` AS `date_cancelled`,
        `slbal_loan`.`RELEASED` AS `date_released`,
        `slbal_loan`.`APPLIED` AS `date_applied`,
        `slbal_loan`.`CO_CODE1` AS `comaker1_code`,
        `slbal_loan`.`CO_NAME1` AS `comaker1_name`,
        `slbal_loan`.`CO_CODE2` AS `comaker2_code`,
        `slbal_loan`.`CO_NAME2` AS `comaker2_name`,
        `slbal_loan`.`CO_CODE3` AS `comaker3_code`,
        `slbal_loan`.`CO_NAME3` AS `comaker3_name`,
        `slbal_loan`.`CO_CODE4` AS `comaker4_code`,
        `slbal_loan`.`CO_NAME4` AS `comaker4_name`,
        `slbal_loan`.`CO_CODE5` AS `comaker5_code`,
        `slbal_loan`.`CO_NAME5` AS `comaker5_name`,
        `slbal_loan`.`THIS_MONTH` AS `this_month`,
        `slbal_loan`.`COLLECTOR` AS `collector`,
        `slbal_loan`.`NOTICE1` AS `notice1`,
        `slbal_loan`.`NOTICE2` AS `notice2`,
        `slbal_loan`.`NOTICE3` AS `notice3`,
        `slbal_loan`.`COLLAT` AS `collateral`,
        'slbal' AS `table_name`,
        `slbal_loan`.`ID` AS `table_id` 
      FROM
        `slbal` AS `slbal_loan` 
      WHERE (
          `slbal_loan`.`LOAN_AMT` > 0 
          AND `slbal_loan`.DOC_DATE <= as_of
        ) 
      UNION
      ALL 
      SELECT 
        `cv_loan`.`MEM_CODE` AS `member_code`,
        `cv_loan`.`MEM_NAME` AS `member_name`,
        `cv_loan`.`ACC_CODE` AS `account_code`,
        `cv_loan`.`TITLE` AS `account_title`,
        `cv_loan`.`DOC_DATE` AS `document_date`,
        `cv_loan`.`DOC_TYPE` AS `document_type`,
        `cv_loan`.`DOC_NUM` AS `document_number`,
        `cv_loan`.`RELEASE_NO` AS `release_number`,
        `cv_loan`.`LOAN_AMT` AS `loan_amount`,
        `cv_loan`.`TERMS` AS `terms`,
        `cv_loan`.`TERMS_MODE` AS `terms_mode`,
        `cv_loan`.`MODE_PAY` AS `payment_mode`,
        `cv_loan`.`DATE_GRANT` AS `date_granted`,
        `cv_loan`.`MATURITY` AS `date_maturity`,
        `cv_loan`.`CUT_OFF` AS `date_cut_off`,
        `cv_loan`.`PAYMENT` AS `payment`,
        `cv_loan`.`INT_RATE` AS `interest_rate`,
        `cv_loan`.`INT_AMT` AS `interest_amount`,
        `cv_loan`.`INT_AMORT` AS `interest_amortization`,
        `cv_loan`.`APPROVED` AS `date_approved`,
        `cv_loan`.`CANCELLED` AS `date_cancelled`,
        `cv_loan`.`RELEASED` AS `date_released`,
        `cv_loan`.`APPLIED` AS `date_applied`,
        `cv_loan`.`CO_CODE1` AS `comaker1_code`,
        `cv_loan`.`CO_NAME1` AS `comaker1_name`,
        `cv_loan`.`CO_CODE2` AS `comaker2_code`,
        `cv_loan`.`CO_NAME2` AS `comaker2_name`,
        `cv_loan`.`CO_CODE3` AS `comaker3_code`,
        `cv_loan`.`CO_NAME3` AS `comaker3_name`,
        `cv_loan`.`CO_CODE4` AS `comaker4_code`,
        `cv_loan`.`CO_NAME4` AS `comaker4_name`,
        `cv_loan`.`CO_CODE5` AS `comaker5_code`,
        `cv_loan`.`CO_NAME5` AS `comaker5_name`,
        `cv_loan`.`THIS_MONTH` AS `this_month`,
        `cv_loan`.`COLLECTOR` AS `collector`,
        `cv_loan`.`NOTICE1` AS `notice1`,
        `cv_loan`.`NOTICE2` AS `notice2`,
        `cv_loan`.`NOTICE3` AS `notice3`,
        `cv_loan`.`COLLAT` AS `collateral`,
        'cv' AS `table_name`,
        `cv_loan`.`ID` AS `table_id` 
      FROM
        `cv` AS `cv_loan` 
      WHERE (
          `cv_loan`.`LOAN_AMT` > 0 
          AND `cv_loan`.DOC_DATE <= as_of
        ) 
      UNION
      ALL 
      SELECT 
        `jv_loan`.`MEM_CODE` AS `member_code`,
        `jv_loan`.`MEM_NAME` AS `member_name`,
        `jv_loan`.`ACC_CODE` AS `account_code`,
        `jv_loan`.`TITLE` AS `account_title`,
        `jv_loan`.`DOC_DATE` AS `document_date`,
        `jv_loan`.`DOC_TYPE` AS `document_type`,
        `jv_loan`.`DOC_NUM` AS `document_number`,
        `jv_loan`.`RELEASE_NO` AS `release_number`,
        `jv_loan`.`LOAN_AMT` AS `loan_amount`,
        `jv_loan`.`TERMS` AS `terms`,
        `jv_loan`.`TERMS_MODE` AS `terms_mode`,
        `jv_loan`.`MODE_PAY` AS `payment_mode`,
        `jv_loan`.`DATE_GRANT` AS `date_granted`,
        `jv_loan`.`MATURITY` AS `date_maturity`,
        `jv_loan`.`CUT_OFF` AS `date_cut_off`,
        `jv_loan`.`PAYMENT` AS `payment`,
        `jv_loan`.`INT_RATE` AS `interest_rate`,
        `jv_loan`.`INT_AMT` AS `interest_amount`,
        `jv_loan`.`INT_AMORT` AS `interest_amortization`,
        `jv_loan`.`APPROVED` AS `date_approved`,
        `jv_loan`.`CANCELLED` AS `date_cancelled`,
        `jv_loan`.`RELEASED` AS `date_released`,
        `jv_loan`.`APPLIED` AS `date_applied`,
        `jv_loan`.`CO_CODE1` AS `comaker1_code`,
        `jv_loan`.`CO_NAME1` AS `comaker1_name`,
        `jv_loan`.`CO_CODE2` AS `comaker2_code`,
        `jv_loan`.`CO_NAME2` AS `comaker2_name`,
        `jv_loan`.`CO_CODE3` AS `comaker3_code`,
        `jv_loan`.`CO_NAME3` AS `comaker3_name`,
        `jv_loan`.`CO_CODE4` AS `comaker4_code`,
        `jv_loan`.`CO_NAME4` AS `comaker4_name`,
        `jv_loan`.`CO_CODE5` AS `comaker5_code`,
        `jv_loan`.`CO_NAME5` AS `comaker5_name`,
        `jv_loan`.`THIS_MONTH` AS `this_month`,
        `jv_loan`.`COLLECTOR` AS `collector`,
        `jv_loan`.`NOTICE1` AS `notice1`,
        `jv_loan`.`NOTICE2` AS `notice2`,
        `jv_loan`.`NOTICE3` AS `notice3`,
        `jv_loan`.`COLLAT` AS `collateral`,
        'jv' AS `table_name`,
        `jv_loan`.`ID` AS `table_id` 
      FROM
        `jv` AS `jv_loan` 
      WHERE (
          `jv_loan`.`LOAN_AMT` > 0 
          AND `jv_loan`.DOC_DATE <= as_of
        )) AS loan_details 
    ORDER BY loan_details.member_code,
      loan_details.account_code,
      loan_details.document_date DESC) AS current_loan_details 
  WHERE group_precedence = 1 ;
  # create a temporary table that will hold the loan balances as of given date
  DROP TEMPORARY TABLE IF EXISTS `tmp_outstanding_loans` ;
  CREATE TEMPORARY TABLE `tmp_outstanding_loans` (
    group_code VARCHAR (20),
    ending_balance DECIMAL (10, 2),
    as_of DATE
  ) ;
  # find outstanding loans
  INSERT INTO tmp_outstanding_loans 
  SELECT 
    CONCAT_WS(
      "-",
      `sl_merged`.MEM_CODE,
      `sl_merged`.ACC_CODE
    ) AS group_code,
    SUM(`sl_merged`.END_BAL) AS ending_balance,
    as_of AS as_of 
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
    WHERE `or`.DOC_DATE <= as_of 
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
    WHERE `jv`.DOC_DATE <= as_of 
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
    WHERE `cv`.DOC_DATE <= as_of 
    GROUP BY `cv`.MEM_CODE,
      `cv`.ACC_CODE) AS sl_merged 
  GROUP BY `sl_merged`.MEM_CODE,
    `sl_merged`.ACC_CODE 
  HAVING SUM(`sl_merged`.END_BAL) <> 0 
  ORDER BY `sl_merged`.MEM_CODE ;
  # combine results -- FINAL FILTERING HERE ---
  SELECT 
    *,
    DATEDIFF(
      as_of,
      loan_details.date_maturity
    ) AS due_days,
    ROUND(
      fn_due_fines (
        as_of,
        loan_details.date_maturity,
        loan_details.date_granted,
        loan_balance.ending_balance
      ),
      2
    ) AS due_fines,
    ROUND(
      fn_due_interest (
        as_of,
        loan_details.date_maturity,
        loan_details.date_granted,
        COALESCE(loan_details.interest_amount)
      ),
      2
    ) AS due_interest 
  FROM
    tmp_outstanding_loans AS loan_balance 
    JOIN 
      (SELECT 
        * 
      FROM
        tmp_loan_details 
      WHERE tmp_loan_details.date_maturity < as_of) AS loan_details USING (group_code) 
    LEFT JOIN 
      (SELECT 
        mem_code AS member_code,
        address1,
        address2,
        address3 
      FROM
        nfmb) AS member_address USING (member_code) 
  ORDER BY loan_details.member_name,
    loan_details.account_code ;
END $$

DELIMITER ;