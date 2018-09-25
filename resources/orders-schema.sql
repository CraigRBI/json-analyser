/* Create database */
CREATE DATABASE TestDB;
GO

/* Change to the database */
USE TestDB;
GO

/* Create table */
CREATE TABLE Orders (
    OrderId UNIQUEIDENTIFIER,
    OrderDate DATETIME NULL,
    Topping VARCHAR(255) NULL,
    IsToppingVegetarian BIT DEFAULT 0 NOT NULL
);
GO