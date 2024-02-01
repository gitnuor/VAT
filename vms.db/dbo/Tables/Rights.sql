CREATE TABLE [dbo].[Rights] (
    [RightId]         INT            IDENTITY (1, 1) NOT NULL,
    [RightName]       NVARCHAR (64)  NOT NULL,
    [Description]     NVARCHAR (128) NULL,
    [IsActive]        BIT            NOT NULL,
    [CreatedBy]       INT            NULL,
    [CreatedTime]     DATETIME       NULL,
    [RightCategoryId] INT            CONSTRAINT [DF__Rights__RightCat__2F1AED73] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED ([RightId] ASC),
    CONSTRAINT [FK_Rights_RightCategory] FOREIGN KEY ([RightCategoryId]) REFERENCES [dbo].[RightCategory] ([RightCategoryId]),
    CONSTRAINT [FK_Rights_RightCategory1] FOREIGN KEY ([RightCategoryId]) REFERENCES [dbo].[RightCategory] ([RightCategoryId])
);

