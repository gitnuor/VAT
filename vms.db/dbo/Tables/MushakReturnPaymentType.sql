CREATE TABLE [dbo].[MushakReturnPaymentType] (
    [MushakReturnPaymentTypeId] INT            NOT NULL,
    [NbrEconomicCodeId]         INT            NOT NULL,
    [SubFormId]                 VARCHAR (50)   NOT NULL,
    [SubFormName]               NVARCHAR (200) NOT NULL,
    [NoteNo]                    NVARCHAR (20)  NOT NULL,
    [TypeName]                  VARCHAR (200)  NOT NULL,
    [TypeNameBn]                NVARCHAR (200) NOT NULL,
    [IsActive]                  BIT            CONSTRAINT [DF_MushakReturnPaymentType_IsActive] DEFAULT ((1)) NOT NULL,
    [EffectiveFrom]             DATETIME       CONSTRAINT [DF_MushakReturnPaymentType_EffectiveFrom] DEFAULT (getdate()) NOT NULL,
    [EffectiveTo]               DATETIME       NULL,
    CONSTRAINT [PK_MushakReturnPaymentType] PRIMARY KEY CLUSTERED ([MushakReturnPaymentTypeId] ASC),
    CONSTRAINT [FK_MushakReturnPaymentType_NbrEconomicCode] FOREIGN KEY ([NbrEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
);

