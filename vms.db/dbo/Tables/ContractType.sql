CREATE TABLE [dbo].[ContractType] (
    [ContractTypeId] INT           NOT NULL,
    [Name]           NVARCHAR (50) NOT NULL,
    [CreatedBy]      INT           NULL,
    [CreatedTime]    DATETIME      NULL,
    CONSTRAINT [PK_ContractType] PRIMARY KEY CLUSTERED ([ContractTypeId] ASC)
);

