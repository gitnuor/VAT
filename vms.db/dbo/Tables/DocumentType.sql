﻿CREATE TABLE [dbo].[DocumentType] (
    [DocumentTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId] INT            NULL,
    [Name]           NVARCHAR (50)  NOT NULL,
    [IsActive]       BIT            NOT NULL,
    [ReferenceKey]   NVARCHAR (100) NULL,
    [CreatedBy]      INT            NULL,
    [CreatedTime]    DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    [ModifiedTime]   DATETIME       NULL,
    CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED ([DocumentTypeId] ASC),
    CONSTRAINT [FK_DocumentType_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

