USE master;
GO
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'CoffeeShop')
BEGIN
    CREATE DATABASE [CoffeeShop];
END;
GO
USE [CoffeeShop];
