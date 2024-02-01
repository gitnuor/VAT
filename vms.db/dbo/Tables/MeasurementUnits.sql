CREATE TABLE [dbo].[MeasurementUnits] (
    [MeasurementUnitId] INT            IDENTITY (1040, 1) NOT NULL,
    [Name]              NVARCHAR (50)  NOT NULL,
    [OrganizationId]    INT            NOT NULL,
    [IsActive]          BIT            NOT NULL,
    [ReferenceKey]      NVARCHAR (100) NULL,
    [CreatedBy]         INT            NULL,
    [CreatedTime]       DATETIME       NULL,
    [ModifiedBy]        INT            NULL,
    [ModifiedTime]      DATETIME       NULL,
    [ApiTransactionId]  BIGINT         NULL,
    CONSTRAINT [PK_MeasurementUnits] PRIMARY KEY CLUSTERED ([MeasurementUnitId] ASC)
);

