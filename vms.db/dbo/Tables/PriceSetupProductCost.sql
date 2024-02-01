CREATE TABLE [dbo].[PriceSetupProductCost] (
    [PriceSetupProductCostId] INT             IDENTITY (1, 1) NOT NULL,
    [PriceSetupId]            INT             NOT NULL,
    [RawMaterialId]           INT             NULL,
    [RequiredQty]             DECIMAL (18, 2) NULL,
    [MeasurementUnitId]       INT             NULL,
    [OverHeadCostId]          INT             NULL,
    [Cost]                    DECIMAL (18, 2) NOT NULL,
    [WastagePercentage]       DECIMAL (18, 2) NULL,
    [IsRawMaterial]           BIT             NOT NULL,
    [ReferenceKey]            NVARCHAR (100)  NULL,
    CONSTRAINT [PK_PriceSetupProductCost] PRIMARY KEY CLUSTERED ([PriceSetupProductCostId] ASC),
    CONSTRAINT [FK_PriceSetupProductCost_MeasurementUnits] FOREIGN KEY ([MeasurementUnitId]) REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId]),
    CONSTRAINT [FK_PriceSetupProductCost_OverHeadCost] FOREIGN KEY ([OverHeadCostId]) REFERENCES [dbo].[OverHeadCost] ([OverHeadCostId]),
    CONSTRAINT [FK_PriceSetupProductCost_PriceSetup] FOREIGN KEY ([PriceSetupId]) REFERENCES [dbo].[PriceSetup] ([PriceSetupId]),
    CONSTRAINT [FK_PriceSetupProductCost_Products] FOREIGN KEY ([RawMaterialId]) REFERENCES [dbo].[Products] ([ProductId])
);

