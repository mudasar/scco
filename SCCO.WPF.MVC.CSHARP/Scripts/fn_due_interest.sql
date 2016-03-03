DELIMITER $$

USE `polo_2016_production`$$

DROP FUNCTION IF EXISTS `fn_due_interest`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `fn_due_interest`(`as_of` DATE,`date_maturity` DATE,`interest_rate` DECIMAL(5,2),`date_granted` DATE,`loan_amount` DECIMAL) RETURNS DECIMAL(12,2)
BEGIN
	DECLARE interest_due DECIMAL(10,2);
	SET interest_due = 0;

	# What is the date after 12 months
	SET @loan_term_in_days = DATEDIFF(date_maturity, date_granted);

	# For backward compatibility in rate where 0.18 is save as 18.00
	SET @loan_interest_rate = IF(interest_rate < 1, interest_rate, interest_rate/100);

	SET @interest_rate_per_day = @loan_interest_rate / @loan_term_in_days;
	SET @days_over_due = DATEDIFF(as_of, date_maturity);
	SET interest_due = (loan_amount * @interest_rate_per_day) * @days_over_due;

	RETURN interest_due;
END$$

DELIMITER ;