CREATE TABLE [dbo].[ApiTransactionType] (
    [ApiTransactionTypeId] INT            NOT NULL,
    [Name]                 NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ApiTransactionType] PRIMARY KEY CLUSTERED ([ApiTransactionTypeId] ASC)
);

