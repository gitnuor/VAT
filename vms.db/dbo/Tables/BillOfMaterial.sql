CREATE TABLE [dbo].[BillOfMaterial] (
    [BillOfMaterialId]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [ProductionReceiveId]      BIGINT          NOT NULL,
    [RawMaterialId]            INT             NOT NULL,
    [UsedQuantity]             DECIMAL (18, 2) NOT NULL,
    [MeasurementUnitId]        INT             NOT NULL,
    [ProductTransactionBookId] BIGINT          NOT NULL,
    [ReferenceKey]             NVARCHAR (100)  NULL,
    [IsActive]                 BIT             NOT NULL,
    [CreatedBy]                INT             NULL,
    [CreatedTime]              DATETIME        NULL,
    CONSTRAINT [PK_BillOfMaterial] PRIMARY KEY CLUSTERED ([BillOfMaterialId] ASC),
    CONSTRAINT [FK_BillOfMaterial_MeasurementUnits] FOREIGN KEY ([MeasurementUnitId]) REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId]),
    CONSTRAINT [FK_BillOfMaterial_ProductionReceive] FOREIGN KEY ([ProductionReceiveId]) REFERENCES [dbo].[ProductionReceive] ([ProductionReceiveId]),
    CONSTRAINT [FK_BillOfMaterial_Products] FOREIGN KEY ([RawMaterialId]) REFERENCES [dbo].[Products] ([ProductId]),
    CONSTRAINT [FK_BillOfMaterial_ProductTransactionBook] FOREIGN KEY ([ProductTransactionBookId]) REFERENCES [dbo].[ProductTransactionBook] ([ProductTransactionBookId])
);

