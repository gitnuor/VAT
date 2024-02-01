CREATE TABLE [dbo].[AdjustmentType] (
    [AdjustmentTypeId] INT           NOT NULL,
    [Name]             NVARCHAR (50) NOT NULL,
    [IsActive]         BIT           NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    [CreatedTime]      DATETIME      NOT NULL,
    CONSTRAINT [PK_AdjustmentType] PRIMARY KEY CLUSTERED ([AdjustmentTypeId] ASC)
);

