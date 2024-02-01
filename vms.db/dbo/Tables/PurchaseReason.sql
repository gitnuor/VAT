CREATE TABLE [dbo].[PurchaseReason] (
    [PurchaseReasonId] INT           NOT NULL,
    [Reason]           NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PurchaseReason] PRIMARY KEY CLUSTERED ([PurchaseReasonId] ASC)
);

