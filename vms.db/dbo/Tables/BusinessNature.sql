CREATE TABLE [dbo].[BusinessNature] (
    [BusinessNatureId] INT            NOT NULL,
    [Name]             VARCHAR (100)  NOT NULL,
    [NameInBangla]     NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_BusinessNature] PRIMARY KEY CLUSTERED ([BusinessNatureId] ASC)
);

