CREATE TABLE [dbo].[ProductGroups] (
    [ProductGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId]   INT            NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [ParentGroupId]    INT            NULL,
    [Node]             NVARCHAR (50)  NULL,
    [IsActive]         BIT            NOT NULL,
    [ReferenceKey]     NVARCHAR (100) NULL,
    [CreatedBy]        INT            NULL,
    [CreatedTime]      DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [ModifiedTime]     DATETIME       NULL,
    [ApiTransactionId] BIGINT         NULL,
    CONSTRAINT [PK_ProductGroups] PRIMARY KEY CLUSTERED ([ProductGroupId] ASC),
    CONSTRAINT [FK_ProductGroups_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

