CREATE TABLE [dbo].[OldAccountCurrentBalance] (
    [OldAccountCurrentBalanceId] INT             IDENTITY (1, 1) NOT NULL,
    [OrganizationId]             INT             NOT NULL,
    [MushakYear]                 INT             NOT NULL,
    [MushakMonth]                INT             NOT NULL,
    [RemainingVATBalance]        DECIMAL (18, 2) NOT NULL,
    [RemainingSDBalance]         DECIMAL (18, 2) NOT NULL,
    [ReferenceKey]               NVARCHAR (100)  NULL,
    [CreatedBy]                  INT             NULL,
    [CreatedTime]                DATETIME        NULL,
    [ApiTransactionId]           BIGINT          NULL,
    CONSTRAINT [PK_OldAccountCurrentBalance] PRIMARY KEY CLUSTERED ([OldAccountCurrentBalanceId] ASC),
    CONSTRAINT [FK_OldAccountCurrentBalance_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId]),
    CONSTRAINT [UK_OldAccountCurrentBalance_Organization_Month] UNIQUE NONCLUSTERED ([OrganizationId] ASC, [MushakYear] ASC, [MushakMonth] ASC)
);

