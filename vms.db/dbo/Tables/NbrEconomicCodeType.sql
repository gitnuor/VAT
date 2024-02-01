CREATE TABLE [dbo].[NbrEconomicCodeType] (
    [NbrEconomicCodeTypeId] INT           NOT NULL,
    [CodeTypeName]          NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_NbrEconomicCodeType] PRIMARY KEY CLUSTERED ([NbrEconomicCodeTypeId] ASC)
);

