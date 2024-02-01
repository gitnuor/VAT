CREATE TABLE [dbo].[BankBranch] (
    [BankBranchId] INT            IDENTITY (1, 1) NOT NULL,
    [BankId]       INT            NOT NULL,
    [DistrictId]   INT            NOT NULL,
    [Name]         VARCHAR (250)  NOT NULL,
    [NameInBangla] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_BankBranch] PRIMARY KEY CLUSTERED ([BankBranchId] ASC),
    CONSTRAINT [FK_BankBranch_Bank] FOREIGN KEY ([BankId]) REFERENCES [dbo].[Bank] ([BankId]),
    CONSTRAINT [FK_BankBranch_District] FOREIGN KEY ([DistrictId]) REFERENCES [dbo].[District] ([DistrictId])
);

