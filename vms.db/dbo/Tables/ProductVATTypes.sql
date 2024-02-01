﻿CREATE TABLE [dbo].[ProductVATTypes] (
    [ProductVATTypeId]             INT             NOT NULL,
    [Name]                         NVARCHAR (500)  NOT NULL,
    [NameInBangla]                 NVARCHAR (500)  NOT NULL,
    [DefaultVatPercent]            DECIMAL (18, 2) CONSTRAINT [DF__tmp_ms_xx__Defau__72C60C4A] DEFAULT ((0)) NOT NULL,
    [EffectiveFrom]                NVARCHAR (50)   NOT NULL,
    [EffectiveTo]                  NVARCHAR (50)   NULL,
    [IsActive]                     BIT             NOT NULL,
    [CreatedBy]                    INT             NULL,
    [CreatedTime]                  DATETIME        NULL,
    [IsApplicableForLocalPurchase] BIT             NOT NULL,
    [IsApplicableForImport]        BIT             NOT NULL,
    [IsApplicableForLocalSale]     BIT             NOT NULL,
    [IsApplicableForExport]        BIT             NOT NULL,
    [IsRequireVDS]                 BIT             NOT NULL,
    [LocalPurchaseNote]            NVARCHAR (50)   NULL,
    [LocalPurchaseNoteInBn]        NVARCHAR (50)   NULL,
    [ImportNote]                   NVARCHAR (50)   NULL,
    [ImportNoteInBn]               NVARCHAR (50)   NULL,
    [LocalSaleNote]                NVARCHAR (50)   NULL,
    [LocalSaleNoteInBn]            NVARCHAR (50)   NULL,
    [ExportNote]                   NVARCHAR (50)   NULL,
    [ExportNoteInBn]               NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProductVATType] PRIMARY KEY CLUSTERED ([ProductVATTypeId] ASC)
);

