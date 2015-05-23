/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50148
Source Host           : localhost:3306
Source Database       : bulacan_2013

Target Server Type    : MYSQL
Target Server Version : 50148
File Encoding         : 65001

Date: 2013-12-24 08:44:24
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `loan_computation`
-- ----------------------------
DROP TABLE IF EXISTS `loan_computation`;
CREATE TABLE `loan_computation` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `member_code` varchar(10),
  `member_name` varchar(60),
  `charge_code1` varchar(10),
  `charge_title1` varchar(50),
  `charge_amount1` double,
  `charge_code2` varchar(10),
  `charge_title2` varchar(50),
  `charge_amount2` double,
  `charge_code3` varchar(10),
  `charge_title3` varchar(50),
  `charge_amount3` double,
  `charge_code4` varchar(10),
  `charge_title4` varchar(50),
  `charge_amount4` double,
  `charge_code5` varchar(10),
  `charge_title5` varchar(50),
  `charge_amount5` double,
  `charge_code6` varchar(10),
  `charge_title6` varchar(50),
  `charge_amount6` double,
  `charge_code7` varchar(10),
  `charge_title7` varchar(50),
  `charge_amount7` double,
  `charge_code8` varchar(10),
  `charge_title8` varchar(50),
  `charge_amount8` double,
  `deduct_code1` varchar(10),
  `deduct_title1` varchar(50),
  `deduct_amount1` double,
  `deduct_code2` varchar(10),
  `deduct_title2` varchar(50),
  `deduct_amount2` double,
  `deduct_code3` varchar(10),
  `deduct_title3` varchar(50),
  `deduct_amount3` double,
  `deduct_code4` varchar(10),
  `deduct_title4` varchar(50),
  `deduct_amount4` double,
  `deduct_code5` varchar(10),
  `deduct_title5` varchar(50),
  `deduct_amount5` double,
  `deduct_code6` varchar(10),
  `deduct_title6` varchar(50),
  `deduct_amount6` double,
  `deduct_code7` varchar(10),
  `deduct_title7` varchar(50),
  `deduct_amount7` double,
  `deduct_code8` varchar(10),
  `deduct_title8` varchar(50),
  `deduct_amount8` double,
  `total_charges` double,
  `total_deductions` double,
  `net_code` varchar(10),
  `net_title` varchar(50),
  `net_amount` double,
  `loan_code` varchar(10),
  `loan_title` varchar(50),
  `loan_amount` double,
  `loan_term` varchar(12),
  `mode_payment` varchar(10),
  `payment` double,
  `release_date` date,
  `granted_date` date,
  `maturity_date` date,
  `interest_rate` double,
  `interest_amount` double,
  `interest_amortization` double,
  `comaker_code1` varchar(10),
  `comaker_name1` varchar(50),
  `comaker_code2` varchar(10),
  `comaker_name2` varchar(50),
  `comaker_code3` varchar(10),
  `comaker_name3` varchar(50),
  `collateral_description` varchar(100),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of loan_computation
-- ----------------------------
