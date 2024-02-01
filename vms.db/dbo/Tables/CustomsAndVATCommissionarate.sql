CREATE TABLE [dbo].[CustomsAndVATCommissionarate] (
    [CustomsAndVATCommissionarateId] INT            NOT NULL,
    [Name]                           VARCHAR (250)  NOT NULL,
    [NameInBangla]                   NVARCHAR (250) NOT NULL,
    [DistrictId]                     INT            NOT NULL,
    [Address]                        NVARCHAR (500) NOT NULL,
    [OperationalCode]                NVARCHAR (10)  NOT NULL,
    [OperationalCode1stDigit]        NVARCHAR (2)   NOT NULL,
    [OperationalCode2ndDigit]        NVARCHAR (2)   NOT NULL,
    [OperationalCode3rdDigit]        NVARCHAR (2)   NOT NULL,
    [OperationalCode4thDigit]        NVARCHAR (2)   NOT NULL,
    [IsActive]                       BIT            NOT NULL,
    CONSTRAINT [PK_CustomsAndVATCommissionarate] PRIMARY KEY CLUSTERED ([CustomsAndVATCommissionarateId] ASC)
);

