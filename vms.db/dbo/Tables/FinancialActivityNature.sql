CREATE TABLE [dbo].[FinancialActivityNature] (
    [FinancialActivityNatureId] INT            NOT NULL,
    [Name]                      VARCHAR (100)  NOT NULL,
    [NameInBangla]              NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_FinancialActivityNature] PRIMARY KEY CLUSTERED ([FinancialActivityNatureId] ASC)
);

