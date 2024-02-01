CREATE TABLE [dbo].[OrgStaticDataType] (
    [OrgStaticDataTypeId] INT            NOT NULL,
    [Name]                NVARCHAR (200) NOT NULL,
    [Description]         NVARCHAR (500) NULL,
    CONSTRAINT [PK_OrgStaticDataType] PRIMARY KEY CLUSTERED ([OrgStaticDataTypeId] ASC)
);

