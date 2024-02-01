CREATE TABLE [dbo].[ContractualProductionTransferRawMaterialDetails] (
    [ContractualProductionTransferRawMaterialDetailsId] INT             IDENTITY (1, 1) NOT NULL,
    [ContractualProductionTransferRawMaterialId]        INT             NOT NULL,
    [RawMaterialId]                                     INT             NOT NULL,
    [MeasurementUnitId]                                 INT             NOT NULL,
    [Quantity]                                          DECIMAL (18, 2) NOT NULL,
    [ReferenceKey]                                      NVARCHAR (100)  NULL,
    CONSTRAINT [PK_ContractualProductionTransferRawMaterialDetails] PRIMARY KEY CLUSTERED ([ContractualProductionTransferRawMaterialDetailsId] ASC),
    CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_ContractualProductionTransferRawMaterial] FOREIGN KEY ([ContractualProductionTransferRawMaterialId]) REFERENCES [dbo].[ContractualProductionTransferRawMaterial] ([ContractualProductionTransferRawMaterialId]),
    CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_MeasurementUnits] FOREIGN KEY ([MeasurementUnitId]) REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId]),
    CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_Products] FOREIGN KEY ([RawMaterialId]) REFERENCES [dbo].[Products] ([ProductId])
);

