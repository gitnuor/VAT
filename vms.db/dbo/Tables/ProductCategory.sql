CREATE TABLE [dbo].[ProductCategory] (
    [ProductCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [OrganizationId]    INT            NOT NULL,
    [Name]              NVARCHAR (100) NOT NULL,
    [IsActive]          BIT            NOT NULL,
    [ReferenceKey]      NVARCHAR (100) NULL,
    [CreatedBy]         INT            NULL,
    [CreatedTime]       DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [ModifiedTime]      DATETIME       NULL,
    [ApiTransactionId]  BIGINT         NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([ProductCategoryId] ASC),
    CONSTRAINT [FK_ProductCategory_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

