CREATE TABLE [dbo].[OrgStaticData] (
    [OrgStaticDataId]     INT            IDENTITY (1, 1) NOT NULL,
    [OrgStaticDataTypeId] INT            NOT NULL,
    [DataKey]             INT            NOT NULL,
    [OrganizationId]      INT            NOT NULL,
    [IsActive]            BIT            NOT NULL,
    [ReferenceKey]        NVARCHAR (100) NULL,
    [EffectiveFrom]       DATETIME       NOT NULL,
    [EffectiveTo]         DATETIME       NULL,
    [CreatedBy]           INT            NULL,
    [CreatedTime]         DATETIME       NULL,
    [ModifiedBy]          INT            NULL,
    [ModifiedTime]        DATETIME       NULL,
    CONSTRAINT [PK_OrgStaticData] PRIMARY KEY CLUSTERED ([OrgStaticDataId] ASC),
    CONSTRAINT [FK_OrgStaticData_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId]),
    CONSTRAINT [FK_OrgStaticData_OrgStaticDataType] FOREIGN KEY ([OrgStaticDataTypeId]) REFERENCES [dbo].[OrgStaticDataType] ([OrgStaticDataTypeId])
);

