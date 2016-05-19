ALTER TABLE `cv` 
  MODIFY COLUMN `RATE`		DECIMAL (6, 4),
  MODIFY COLUMN `AMOUNT`	DECIMAL (10, 2),
  MODIFY COLUMN `LOAN_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `PAYMENT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_RATE`	DECIMAL (6, 4),
  MODIFY COLUMN `INT_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_AMORT` DECIMAL (10, 2),
  MODIFY COLUMN `POSTED`	TINYINT (1);

ALTER TABLE `jv` 
  MODIFY COLUMN `RATE`		DECIMAL (6, 4),
  MODIFY COLUMN `AMOUNT`	DECIMAL (10, 2),
  MODIFY COLUMN `LOAN_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `PAYMENT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_RATE`	DECIMAL (6, 4),
  MODIFY COLUMN `INT_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_AMORT` DECIMAL (10, 2),
  MODIFY COLUMN `POSTED`	TINYINT (1);
  
ALTER TABLE `or`
  MODIFY COLUMN `RATE`  DECIMAL(6,4);
  
ALTER TABLE `slbal` 
  MODIFY COLUMN `RATE`		DECIMAL (6, 4),
  MODIFY COLUMN `AMOUNT`	DECIMAL (10, 2),
  MODIFY COLUMN `LOAN_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `PAYMENT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_RATE`	DECIMAL (6, 4),
  MODIFY COLUMN `INT_AMT`	DECIMAL (10, 2),
  MODIFY COLUMN `INT_AMORT` DECIMAL (10, 2),
  MODIFY COLUMN `POSTED`	TINYINT (1);
  
ALTER TABLE `loan_products` 
  MODIFY COLUMN `AnnualInterestRate` DECIMAL (6, 4) NOT NULL,
  MODIFY COLUMN `MinimumLoanableAmount` DECIMAL (10, 2) NOT NULL,
  MODIFY COLUMN `MaximumLoanableAmount` DECIMAL (10, 2) NOT NULL;
