CREATE TABLE [dbo].[DebitNote] (
    [DebitNoteId]        INT            IDENTITY (1, 1) NOT NULL,
    [PurchaseId]         INT            NOT NULL,
    [VoucherNo]          NVARCHAR (50)  NOT NULL,
    [ReasonOfReturn]     NVARCHAR (500) NULL,
    [ReturnDate]         DATETIME       NOT NULL,
    [MushakGenerationId] INT            NULL,
    [ReferenceKey]       NVARCHAR (100) NULL,
    [CreatedBy]          INT            NULL,
    [CreatedTime]        DATETIME       NULL,
    [ApiTransactionId]   BIGINT         NULL,
    CONSTRAINT [PK_DebitNote] PRIMARY KEY CLUSTERED ([DebitNoteId] ASC),
    CONSTRAINT [FK_DebitNote_MushakGeneration] FOREIGN KEY ([MushakGenerationId]) REFERENCES [dbo].[MushakGeneration] ([MushakGenerationId]),
    CONSTRAINT [FK_DebitNote_Purchase] FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchase] ([PurchaseId])
);

