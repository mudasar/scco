DELIMITER $$

USE `polo_2015_production`$$

DROP FUNCTION IF EXISTS `fn_due_interest`$$

CREATE DEFINER=`root`@`localhost` FUNCTION `fn_due_interest`(`as_of` DATE,`date_maturity` DATE,`date_granted` DATE,`interest_amount` DECIMAL(10,2)) RETURNS decimal(12,2)
BEGIN
	DECLARE interest_due DECIMAL(10,2);
	SET interest_due = 0;

	SET @loan_term_in_days = DATEDIFF(date_maturity, date_granted);
	SET @days_over_due = DATEDIFF(as_of, date_maturity);
	SET interest_due = (interest_amount / @loan_term_in_days) * @days_over_due;

	RETURN interest_due;
END$$

DELIMITER ;