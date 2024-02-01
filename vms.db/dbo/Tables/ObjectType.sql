﻿CREATE TABLE [dbo].[ObjectType] (
    [ObjectTypeId] INT           NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [Type]         NVARCHAR (50) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [CreatedBy]    INT           NULL,
    [CreatedTime]  DATETIME      NULL,
    CONSTRAINT [PK_ObjectType] PRIMARY KEY CLUSTERED ([ObjectTypeId] ASC)
);

