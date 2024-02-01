CREATE TABLE [dbo].[ApiTransaction] (
    [ApiTransactionId]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [ApiTransactionTypeId] INT            NOT NULL,
    [SecurityToken]        NVARCHAR (100) NULL,
    [NumberOfItem]         INT            NOT NULL,
    [CreatedBy]            INT            NULL,
    [CreatedTime]          DATETIME       NULL,
    CONSTRAINT [PK_ApiTransaction] PRIMARY KEY CLUSTERED ([ApiTransactionId] ASC),
    CONSTRAINT [FK_ApiTransaction_ApiTransactionType] FOREIGN KEY ([ApiTransactionTypeId]) REFERENCES [dbo].[ApiTransactionType] ([ApiTransactionTypeId])
);

