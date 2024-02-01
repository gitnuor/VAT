CREATE TABLE [dbo].[ContractualProductionProductDetails] (
    [ContractualProductionProductDetailsId] INT             IDENTITY (1, 1) NOT NULL,
    [ContractualProductionId]               INT             NOT NULL,
    [ProductId]                             INT             NOT NULL,
    [Quantity]                              DECIMAL (18, 2) NOT NULL,
    [MeasurementUnitId]                     INT             NOT NULL,
    [ReferenceKey]                          NVARCHAR (100)  NULL,
    CONSTRAINT [PK_ContractualProductionProductDetails] PRIMARY KEY CLUSTERED ([ContractualProductionProductDetailsId] ASC),
    CONSTRAINT [FK_ContractualProductionProductDetails_ContractualProduction] FOREIGN KEY ([ContractualProductionId]) REFERENCES [dbo].[ContractualProduction] ([ContractualProductionId]),
    CONSTRAINT [FK_ContractualProductionProductDetails_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId])
);

