CREATE TABLE [dbo].[CreditNote] (
    [CreditNoteId]       INT            IDENTITY (1, 1) NOT NULL,
    [SalesId]            INT            NOT NULL,
    [VoucherNo]          NVARCHAR (50)  NOT NULL,
    [ReasonOfReturn]     NVARCHAR (500) NULL,
    [ReturnDate]         DATETIME       NOT NULL,
    [MushakGenerationId] INT            NULL,
    [ReferenceKey]       NVARCHAR (100) NULL,
    [CreatedBy]          INT            NULL,
    [CreatedTime]        DATETIME       NULL,
    [ApiTransactionId]   BIGINT         NULL,
    CONSTRAINT [PK_CreditNote] PRIMARY KEY CLUSTERED ([CreditNoteId] ASC),
    CONSTRAINT [FK_CreditNote_MushakGeneration] FOREIGN KEY ([MushakGenerationId]) REFERENCES [dbo].[MushakGeneration] ([MushakGenerationId]),
    CONSTRAINT [FK_CreditNote_Sales] FOREIGN KEY ([SalesId]) REFERENCES [dbo].[Sales] ([SalesId])
);

