/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50148
Source Host           : localhost:3306
Source Database       : bulacan_2013

Target Server Type    : MYSQL
Target Server Version : 50148
File Encoding         : 65001

Date: 2013-12-24 08:57:38
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `loan_deductions`
-- ----------------------------
DROP TABLE IF EXISTS `loan_deductions`;
CREATE TABLE `loan_deductions` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LoanProductId` int(11) NOT NULL,
  `AccountCode` varchar(10) NOT NULL,
  `Amount` double NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of loan_deductions
-- ----------------------------
INSERT INTO `loan_deductions` VALUES ('5', '1', '397.02', '20');
INSERT INTO `loan_deductions` VALUES ('6', '2', '397.02', '10');
