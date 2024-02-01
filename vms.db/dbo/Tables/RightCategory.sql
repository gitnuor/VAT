CREATE TABLE [dbo].[RightCategory] (
    [RightCategoryId]   INT            NOT NULL,
    [RightCategoryName] NVARCHAR (100) NOT NULL,
    [Description]       NVARCHAR (200) NULL,
    [IsActive]          BIT            NOT NULL,
    [CreatedBy]         INT            NULL,
    [CreatedTime]       DATETIME       NULL,
    CONSTRAINT [PK_RightCategorys] PRIMARY KEY CLUSTERED ([RightCategoryId] ASC)
);

