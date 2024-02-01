CREATE TABLE [dbo].[Country] (
    [CountryId] INT            NOT NULL,
    [Name]      NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

