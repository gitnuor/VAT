CREATE TABLE [dbo].[BusinessCategory] (
    [BusinessCategoryId] INT            NOT NULL,
    [Name]               VARCHAR (250)  NOT NULL,
    [NameInBangla]       NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BusinessCategory] PRIMARY KEY CLUSTERED ([BusinessCategoryId] ASC)
);

