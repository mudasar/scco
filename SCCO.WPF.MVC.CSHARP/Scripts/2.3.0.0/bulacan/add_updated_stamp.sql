﻿ALTER TABLE `slbal`
ADD COLUMN `UPDATED_BY`  int,
ADD COLUMN `UPDATED_AT`  datetime;

ALTER TABLE `cv`
ADD COLUMN `UPDATED_BY`  int,
ADD COLUMN `UPDATED_AT`  datetime;

ALTER TABLE `jv`
ADD COLUMN `UPDATED_BY`  int,
ADD COLUMN `UPDATED_AT`  datetime;

ALTER TABLE `or`
ADD COLUMN `UPDATED_BY`  int,
ADD COLUMN `UPDATED_AT`  datetime;