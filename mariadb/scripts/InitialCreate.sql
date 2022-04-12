CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Articles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ArticleNumber` int NOT NULL,
    `Brand` longtext CHARACTER SET utf8mb4 NOT NULL,
    `IsApproved` tinyint(1) NOT NULL,
    `IsBulky` tinyint(1) NOT NULL,
    `LastChanged` datetime(6) NOT NULL,
    CONSTRAINT `PK_Articles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ArticlesAttributes` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Color` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Country` int NOT NULL,
    `LastChange` datetime(6) NOT NULL,
    `ArticleId` int NOT NULL,
    CONSTRAINT `PK_ArticlesAttributes` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ArticlesAttributes_Articles_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Articles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_ArticlesAttributes_ArticleId` ON `ArticlesAttributes` (`ArticleId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220412083555_InitCreate', '6.0.3');

COMMIT;
