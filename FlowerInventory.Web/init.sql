SET NOCOUNT ON;

IF DB_ID('FlowerInventory') IS NULL
    BEGIN
        PRINT 'Creating database FlowerInventory';
        CREATE DATABASE FlowerInventory;
    END
ELSE
    BEGIN
        PRINT 'Database FlowerInventory already exists';
    END
GO

USE FlowerInventory;
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
    BEGIN
        CREATE TABLE [__EFMigrationsHistory] (
                                                 [MigrationId] nvarchar(150) NOT NULL,
                                                 [ProductVersion] nvarchar(32) NOT NULL,
                                                 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
        );
    END;
GO


IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Categories
        (
            Id           INT IDENTITY(1,1) NOT NULL
                CONSTRAINT PK_Categories PRIMARY KEY,
            PublicId     UNIQUEIDENTIFIER  NOT NULL
                CONSTRAINT DF_Categories_PublicId DEFAULT NEWID(),
            Name         NVARCHAR(100)     NOT NULL,
            Description  NVARCHAR(400)     NULL,
            IsActive     BIT               NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT (1),
            DisplayOrder INT               NOT NULL CONSTRAINT DF_Categories_DisplayOrder DEFAULT (0),
            ImagePath    NVARCHAR(500)     NULL
        );

        CREATE UNIQUE INDEX IX_Categories_PublicId ON dbo.Categories (PublicId);
    END
GO

-- Flowers
IF OBJECT_ID(N'dbo.Flowers', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.Flowers
        (
            Id             INT IDENTITY(1,1) NOT NULL
                CONSTRAINT PK_Flowers PRIMARY KEY,
            PublicId       UNIQUEIDENTIFIER  NOT NULL
                CONSTRAINT DF_Flowers_PublicId DEFAULT NEWID(),
            Name           NVARCHAR(120)     NOT NULL,
            Type           NVARCHAR(80)      NOT NULL,
            Price          DECIMAL(10,2)     NOT NULL,
            CategoryId     INT               NOT NULL,
            ImagePath      NVARCHAR(500)     NULL,
            -- optional fields matching your model
            Sku            NVARCHAR(60)      NULL,
            StockQuantity  INT               NOT NULL CONSTRAINT DF_Flowers_StockQuantity DEFAULT (0),
            IsActive       BIT               NOT NULL CONSTRAINT DF_Flowers_IsActive DEFAULT (1),
            Description    NVARCHAR(1000)    NULL,

            CONSTRAINT FK_Flowers_Categories_CategoryId
                FOREIGN KEY (CategoryId) REFERENCES dbo.Categories (Id)
                    ON DELETE NO ACTION
                    ON UPDATE NO ACTION
        );

        CREATE INDEX IX_Flowers_CategoryId ON dbo.Flowers (CategoryId);
        CREATE UNIQUE INDEX IX_Flowers_PublicId ON dbo.Flowers (PublicId);
    END
GO


IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE Name = N'Roses')
    INSERT dbo.Categories (Name, Description, IsActive, DisplayOrder, ImagePath)
    VALUES (N'Roses', N'All rose varieties', 1, 1, N'default/default-category.jpg');

IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE Name = N'Tulips')
    INSERT dbo.Categories (Name, Description, IsActive, DisplayOrder, ImagePath)
    VALUES (N'Tulips', N'Seasonal tulips', 1, 2, N'default/default-category.jpg');

IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE Name = N'Lilies')
    INSERT dbo.Categories (Name, Description, IsActive, DisplayOrder, ImagePath)
    VALUES (N'Lilies', N'Asiatic & Oriental lilies', 1, 3, N'default/default-category.jpg');
GO

DECLARE @RosesId  INT = (SELECT TOP(1) Id FROM dbo.Categories WHERE Name = N'Roses');
DECLARE @TulipsId INT = (SELECT TOP(1) Id FROM dbo.Categories WHERE Name = N'Tulips');
DECLARE @LiliesId INT = (SELECT TOP(1) Id FROM dbo.Categories WHERE Name = N'Lilies');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Red Rose' AND Type = N'Single')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Red Rose', N'Single', 2.50, @RosesId, N'default/default-flower.jpg', N'RR-001', 100, 1, N'Classic red rose stem');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Rose Bouquet' AND Type = N'Bouquet')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Rose Bouquet', N'Bouquet', 24.99, @RosesId, N'default/default-flower.jpg', N'RB-010', 25, 1, N'Dozen mixed rose bouquet');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'White Rose' AND Type = N'Single')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'White Rose', N'Single', 2.75, @RosesId, N'default/default-flower.jpg', N'WR-002', 80, 1, N'Elegant white rose stem');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Yellow Tulip' AND Type = N'Single')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Yellow Tulip', N'Single', 1.80, @TulipsId, N'default/default-flower.jpg', N'YT-101', 150, 1, N'Bright yellow tulip stem');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Tulip Bunch' AND Type = N'Bouquet')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Tulip Bunch', N'Bouquet', 12.00, @TulipsId, N'default/default-flower.jpg', N'TB-201', 40, 1, N'Bunch of mixed tulips');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Purple Tulip' AND Type = N'Single')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Purple Tulip', N'Single', 1.95, @TulipsId, N'default/default-flower.jpg', N'PT-102', 120, 1, N'Rich purple tulip stem');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Asiatic Lily' AND Type = N'Stem')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Asiatic Lily', N'Stem', 3.20, @LiliesId, N'default/default-flower.jpg', N'AL-301', 60, 1, N'Vibrant Asiatic lily stem');

IF NOT EXISTS (SELECT 1 FROM dbo.Flowers WHERE Name = N'Oriental Lily' AND Type = N'Stem')
    INSERT dbo.Flowers (Name, Type, Price, CategoryId, ImagePath, Sku, StockQuantity, IsActive, Description)
    VALUES (N'Oriental Lily', N'Stem', 3.60, @LiliesId, N'default/default-flower.jpg', N'OL-302', 55, 1, N'Fragrant Oriental lily stem');
GO


SELECT COUNT(*) AS CategoryCount FROM dbo.Categories;
SELECT COUNT(*) AS FlowerCount   FROM dbo.Flowers;
GO
