CREATE TABLE [dbo].[Adjustment] (
    [AdjustmentId]     INT             IDENTITY (1, 1) NOT NULL,
    [AdjustmentTypeId] INT             NOT NULL,
    [OrganizationId]   INT             NOT NULL,
    [Amount]           DECIMAL (18, 2) NOT NULL,
    [Month]            INT             NOT NULL,
    [Year]             INT             NOT NULL,
    [IsActive]         BIT             NOT NULL,
    [Description]      NVARCHAR (500)  NULL,
    [ReferenceKey]     NVARCHAR (100)  NULL,
    [CreatedBy]        INT             NOT NULL,
    [CreatedTime]      DATETIME        NOT NULL,
    [ApiTransactionId] BIGINT          NULL,
    CONSTRAINT [PK_Adjustment] PRIMARY KEY CLUSTERED ([AdjustmentId] ASC),
    CONSTRAINT [FK_Adjustment_AdjustmentType] FOREIGN KEY ([AdjustmentTypeId]) REFERENCES [dbo].[AdjustmentType] ([AdjustmentTypeId]),
    CONSTRAINT [FK_Adjustment_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

