﻿CREATE TABLE [dbo].[ProductionReceive] (
    [ProductionReceiveId]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [BatchNo]                        NVARCHAR (50)   NULL,
    [OrganizationId]                 INT             NOT NULL,
    [ProductionId]                   INT             NULL,
    [ProductId]                      INT             NOT NULL,
    [PriceSetupId]                   INT             CONSTRAINT [DF_ProductionReceive_PriceSetupId] DEFAULT ((2)) NOT NULL,
    [ReceiveQuantity]                DECIMAL (18, 2) NOT NULL,
    [MeasurementUnitId]              INT             NOT NULL,
    [ReceiveTime]                    DATETIME        NOT NULL,
    [MaterialCost]                   DECIMAL (18, 2) NOT NULL,
    [IsActive]                       BIT             NOT NULL,
    [IsContractual]                  BIT             CONSTRAINT [DF_ProductionReceive_IsContractual] DEFAULT ((0)) NOT NULL,
    [ContractualProductionId]        INT             NULL,
    [ContractualProductionChallanNo] NVARCHAR (50)   NULL,
    [ReferenceKey]                   NVARCHAR (100)  NULL,
    [CreatedBy]                      INT             NULL,
    [CreatedTime]                    DATETIME        NULL,
    [ApiTransactionId]               BIGINT          NULL,
    CONSTRAINT [PK_ProductionReceive] PRIMARY KEY CLUSTERED ([ProductionReceiveId] ASC),
    CONSTRAINT [FK_ProductionReceive_ContractualProduction] FOREIGN KEY ([ContractualProductionId]) REFERENCES [dbo].[ContractualProduction] ([ContractualProductionId]),
    CONSTRAINT [FK_ProductionReceive_MeasurementUnits] FOREIGN KEY ([MeasurementUnitId]) REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId]),
    CONSTRAINT [FK_ProductionReceive_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId]),
    CONSTRAINT [FK_ProductionReceive_PriceSetup] FOREIGN KEY ([PriceSetupId]) REFERENCES [dbo].[PriceSetup] ([PriceSetupId]),
    CONSTRAINT [FK_ProductionReceive_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId]),
    CONSTRAINT [FK_ProductionReceive_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([ProductId])
);

