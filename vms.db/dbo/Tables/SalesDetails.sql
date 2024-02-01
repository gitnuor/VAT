﻿CREATE TABLE [dbo].[SalesDetails] (
    [SalesDetailId]            INT             IDENTITY (1, 1) NOT NULL,
    [SalesId]                  INT             NOT NULL,
    [ProductId]                INT             NOT NULL,
    [ProductVATTypeId]         INT             NOT NULL,
    [ProductTransactionBookId] BIGINT          NOT NULL,
    [Quantity]                 DECIMAL (18, 2) NOT NULL,
    [UnitPrice]                DECIMAL (18, 2) NOT NULL,
    [DiscountPerItem]          DECIMAL (18, 2) NOT NULL,
    [VATPercent]               DECIMAL (18, 2) NOT NULL,
    [SupplementaryDutyPercent] DECIMAL (18, 2) NOT NULL,
    [MeasurementUnitId]        INT             NOT NULL,
    [ReferenceKey]             NVARCHAR (100)  NULL,
    [CreatedBy]                INT             NULL,
    [CreatedTime]              DATETIME        NULL,
    CONSTRAINT [PK_SalesDetails] PRIMARY KEY CLUSTERED ([SalesDetailId] ASC),
    CONSTRAINT [FK_SalesDetails_MeasurementUnits] FOREIGN KEY ([MeasurementUnitId]) REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId]),
    CONSTRAINT [FK_SalesDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]),
    CONSTRAINT [FK_SalesDetails_ProductTransactionBook] FOREIGN KEY ([ProductTransactionBookId]) REFERENCES [dbo].[ProductTransactionBook] ([ProductTransactionBookId]),
    CONSTRAINT [FK_SalesDetails_ProductVATTypes] FOREIGN KEY ([ProductVATTypeId]) REFERENCES [dbo].[ProductVATTypes] ([ProductVATTypeId]),
    CONSTRAINT [FK_SalesDetails_Sales] FOREIGN KEY ([SalesId]) REFERENCES [dbo].[Sales] ([SalesId])
);

