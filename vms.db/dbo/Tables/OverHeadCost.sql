CREATE TABLE [dbo].[OverHeadCost] (
    [OverHeadCostId]            INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId]            INT            NULL,
    [Name]                      NVARCHAR (100) NOT NULL,
    [IsActive]                  BIT            NOT NULL,
    [IsApplicableAsRawMaterial] BIT            NOT NULL,
    [ReferenceKey]              NVARCHAR (100) NULL,
    [CreatedBy]                 INT            NULL,
    [CreatedTime]               DATETIME       NULL,
    [ModifiedBy]                INT            NULL,
    [ModifiedTime]              DATETIME       NULL,
    [ApiTransactionId]          BIGINT         NULL,
    CONSTRAINT [PK_OverHeadCost] PRIMARY KEY CLUSTERED ([OverHeadCostId] ASC),
    CONSTRAINT [FK_OverHeadCost_ApiTransaction] FOREIGN KEY ([ApiTransactionId]) REFERENCES [dbo].[ApiTransaction] ([ApiTransactionId])
);

