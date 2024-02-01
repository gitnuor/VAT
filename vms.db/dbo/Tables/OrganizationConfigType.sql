CREATE TABLE [dbo].[OrganizationConfigType] (
    [OrganizationConfigTypeId] INT            NOT NULL,
    [Name]                     NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_OrganizationConfigType] PRIMARY KEY CLUSTERED ([OrganizationConfigTypeId] ASC)
);

