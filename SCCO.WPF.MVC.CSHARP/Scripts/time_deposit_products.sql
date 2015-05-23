/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50148
Source Host           : localhost:3306
Source Database       : minimalist_2013

Target Server Type    : MYSQL
Target Server Version : 50148
File Encoding         : 65001

Date: 2013-12-26 08:15:14
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `time_deposit_products`
-- ----------------------------
DROP TABLE IF EXISTS `time_deposit_products`;
CREATE TABLE `time_deposit_products` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `ProductCode` varchar(10) NOT NULL,
  `MinimumTerm` int(11) NOT NULL,
  `MaximumTerm` int(11) NOT NULL,
  `MinimumAmount` double NOT NULL,
  `MaximumAmount` double NOT NULL,
  `InterestRate` double NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of time_deposit_products
-- ----------------------------
