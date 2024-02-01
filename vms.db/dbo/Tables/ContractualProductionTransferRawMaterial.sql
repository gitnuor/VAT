CREATE TABLE [dbo].[ContractualProductionTransferRawMaterial] (
    [ContractualProductionTransferRawMaterialId] INT            IDENTITY (1, 1) NOT NULL,
    [ContractualProductionId]                    INT            NOT NULL,
    [TransfereDate]                              DATETIME       NOT NULL,
    [Location]                                   NVARCHAR (50)  NOT NULL,
    [ChallanNo]                                  NVARCHAR (50)  NOT NULL,
    [ChallanIssueDate]                           DATETIME       NOT NULL,
    [ReferenceKey]                               NVARCHAR (100) NULL,
    [CreatedDate]                                DATETIME       NOT NULL,
    [CreatedBy]                                  INT            NOT NULL,
    [ApiTransactionId]                           BIGINT         NULL,
    CONSTRAINT [PK_ContractualProductionTransferRawMaterial] PRIMARY KEY CLUSTERED ([ContractualProductionTransferRawMaterialId] ASC),
    CONSTRAINT [FK_ContractualProductionTransferRawMaterial_ContractualProduction] FOREIGN KEY ([ContractualProductionId]) REFERENCES [dbo].[ContractualProduction] ([ContractualProductionId])
);

