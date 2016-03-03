DELIMITER $$

USE `polo_2016_production`$$

DROP FUNCTION IF EXISTS `fn_due_fines`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `fn_due_fines`(`as_of` DATE,`date_maturity` DATE,`date_granted` DATE,`loan_balance` DECIMAL(10,2)) RETURNS DECIMAL(12,2)
BEGIN
	DECLARE fines_due DECIMAL(10,2);
	SET fines_due = 0;

	# What is the date after 12 months
	SET @date_end = DATE_ADD(date_granted, INTERVAL 12 MONTH) ;
	# What is the total days in a year from date granted
	SET @total_days_in_year = DATEDIFF(@date_end, date_granted);
	# How many days after maturity
	SET @days_due = DATEDIFF(as_of, date_maturity);
	# What is the rate of fines
	SET @fines_rate = 0.0;
	SELECT CAST(COALESCE(CurrentValue,0.00) AS DECIMAL(5, 2)) AS rate_fines FROM global_variables WHERE Keyword = 'RateOfFines' INTO @fines_rate;

	# What is the fines per day rate
	SET @daily_fines = (loan_balance * @fines_rate) / @total_days_in_year;
	# What is the total fines due
	SET fines_due = ((loan_balance * @fines_rate) / @total_days_in_year) * @days_due;

	RETURN fines_due;
END$$

DELIMITER ;