CREATE TABLE [dbo].[InputOutputCoEfficient] (
    [InputOutputCoEfficientId] INT      IDENTITY (1, 1) NOT NULL,
    [OrganizationId]           INT      NOT NULL,
    [PriceSetupId]             INT      NULL,
    [IsSubmitted]              BIT      NOT NULL,
    [SubmissionDate]           DATETIME NULL,
    [EffectiveFrom]            DATETIME NOT NULL,
    [EffectiveTo]              DATETIME NULL,
    [IsActive]                 BIT      NOT NULL,
    [CreatedBy]                INT      NULL,
    [CreatedTime]              DATETIME NULL,
    CONSTRAINT [PK_InputOutputCoEfficient] PRIMARY KEY CLUSTERED ([InputOutputCoEfficientId] ASC)
);

