CREATE TABLE [dbo].[PaymentMethod] (
    [PaymentMethodId] INT           NOT NULL,
    [Name]            NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED ([PaymentMethodId] ASC)
);

