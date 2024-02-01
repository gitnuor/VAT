CREATE TABLE [dbo].[OrganizationConfig] (
    [OrganizationConfigId]     INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId]           INT            NOT NULL,
    [OrganizationConfigTypeId] INT            NOT NULL,
    [ConfigValue]              NVARCHAR (500) NOT NULL,
    [EffectiveFrom]            DATETIME       NOT NULL,
    [EffectiveTo]              DATETIME       NULL,
    [IsActive]                 BIT            NOT NULL,
    [CreatedBy]                INT            NULL,
    [CreatedTime]              DATETIME       NULL,
    CONSTRAINT [PK_OrganizationConfig] PRIMARY KEY CLUSTERED ([OrganizationConfigId] ASC),
    CONSTRAINT [FK_OrganizationConfig_OrganizationConfigType] FOREIGN KEY ([OrganizationConfigTypeId]) REFERENCES [dbo].[OrganizationConfigType] ([OrganizationConfigTypeId]),
    CONSTRAINT [FK_OrganizationConfig_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

