DELIMITER $$

USE `bulacan_2015_production`$$

DROP FUNCTION IF EXISTS `fn_due_fines`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `fn_due_fines`(`as_of` DATE,`date_maturity` DATE,`date_granted` DATE,`loan_balance` DECIMAL(10,2)) RETURNS DECIMAL(12,2)
BEGIN
	DECLARE fines_due DECIMAL(10,2);
	SET fines_due = 0;

	SET @date_end = DATE_ADD(date_granted, INTERVAL 12 MONTH) ;
	SET @total_days_in_year = DATEDIFF(@date_end, date_granted);
	SET @days_due = DATEDIFF(as_of, date_maturity);
	SET @fines_rate = 0.0;
	SELECT CAST(COALESCE(CurrentValue,0.00) AS DECIMAL(5, 2)) AS rate_fines FROM global_variables WHERE Keyword = 'RateOfFines' INTO @fines_rate;

	SET @daily_fines = (loan_balance * @fines_rate) / @total_days_in_year;
	SET fines_due = ((loan_balance * @fines_rate) / @total_days_in_year) * @days_due;

	RETURN fines_due;
END$$

DELIMITER ;