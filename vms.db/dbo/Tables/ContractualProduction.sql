CREATE TABLE [dbo].[ContractualProduction] (
    [ContractualProductionId] INT            IDENTITY (1, 1) NOT NULL,
    [ContractTypeId]          INT            NULL,
    [OrganizationId]          INT            NOT NULL,
    [ChallanNo]               NVARCHAR (50)  NULL,
    [ContractNo]              NVARCHAR (50)  NOT NULL,
    [VendorId]                INT            NULL,
    [CustomerId]              INT            NULL,
    [IssueDate]               DATETIME2 (7)  NULL,
    [ContractDate]            DATETIME       NOT NULL,
    [IsClosed]                BIT            NOT NULL,
    [ClosingDate]             DATETIME       NOT NULL,
    [ClosedBy]                INT            NULL,
    [ReferenceKey]            NVARCHAR (100) NULL,
    [CreatedBy]               INT            NOT NULL,
    [CreatedTime]             DATETIME       NOT NULL,
    [ApiTransactionId]        BIGINT         NULL,
    CONSTRAINT [PK_ContractualProduction] PRIMARY KEY CLUSTERED ([ContractualProductionId] ASC),
    CONSTRAINT [FK_ContractualProduction_ContractType] FOREIGN KEY ([ContractTypeId]) REFERENCES [dbo].[ContractType] ([ContractTypeId]),
    CONSTRAINT [FK_ContractualProduction_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId]),
    CONSTRAINT [FK_ContractualProduction_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendor] ([VendorId])
);

