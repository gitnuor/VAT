CREATE TABLE [dbo].[Purchase] (
    [PurchaseId]                       INT             IDENTITY (1, 1) NOT NULL,
    [OrganizationId]                   INT             NOT NULL,
    [TransferSalesId]                  INT             NULL,
    [TransferBranchId]                 INT             NULL,
    [VendorId]                         INT             NULL,
    [VatChallanNo]                     NVARCHAR (50)   NULL,
    [VatChallanIssueDate]              DATETIME        NULL,
    [VendorInvoiceNo]                  NVARCHAR (50)   NULL,
    [InvoiceNo]                        NVARCHAR (50)   NOT NULL,
    [PurchaseDate]                     DATETIME        NOT NULL,
    [PurchaseTypeId]                   INT             NOT NULL,
    [PurchaseReasonId]                 INT             NOT NULL,
    [NoOfIteams]                       INT             NOT NULL,
    [TotalPriceWithoutVat]             DECIMAL (18, 2) NOT NULL,
    [DiscountOnTotalPrice]             DECIMAL (18, 2) NOT NULL,
    [TotalDiscountOnIndividualProduct] DECIMAL (18, 2) NOT NULL,
    [TotalImportDuty]                  DECIMAL (18, 2) NOT NULL,
    [TotalRegulatoryDuty]              DECIMAL (18, 2) NOT NULL,
    [TotalSupplementaryDuty]           DECIMAL (18, 2) NOT NULL,
    [TotalVAT]                         DECIMAL (18, 2) NOT NULL,
    [TotalAdvanceTax]                  DECIMAL (18, 2) NOT NULL,
    [TotalAdvanceIncomeTax]            DECIMAL (18, 2) NOT NULL,
    [AdvanceTaxPaidAmount]             DECIMAL (18, 2) NULL,
    [ATPDate]                          DATETIME        NULL,
    [ATPBankBranchId]                  INT             NULL,
    [ATPNbrEconomicCodeId]             INT             NULL,
    [ATPChallanNo]                     NVARCHAR (20)   NULL,
    [IsVatDeductedInSource]            BIT             NOT NULL,
    [VDSAmount]                        DECIMAL (18, 2) NULL,
    [IsVDSCertificatePrinted]          BIT             NULL,
    [VDSCertificateNo]                 NVARCHAR (50)   NULL,
    [VDSCertificateDate]               DATETIME        NULL,
    [VDSPaymentBookTransferNo]         NVARCHAR (50)   NULL,
    [VDSNote]                          NVARCHAR (500)  NULL,
    [PayableAmount]                    AS              (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)),
    [PaidAmount]                       DECIMAL (18, 2) NULL,
    [DueAmount]                        AS              (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)-[PaidAmount]),
    [ExpectedDeliveryDate]             DATETIME        NULL,
    [DeliveryDate]                     DATETIME        NULL,
    [LcNo]                             NVARCHAR (50)   NULL,
    [LcDate]                           DATETIME        NULL,
    [BillOfEntry]                      NVARCHAR (50)   NULL,
    [BillOfEntryDate]                  DATETIME        NULL,
    [CustomsAndVATCommissionarateId]   INT             NULL,
    [DueDate]                          DATETIME        NULL,
    [TermsOfLc]                        NVARCHAR (500)  NULL,
    [PoNumber]                         NVARCHAR (50)   NULL,
    [MushakGenerationId]               INT             NULL,
    [ReferenceKey]                     NVARCHAR (100)  NULL,
    [IsComplete]                       BIT             NOT NULL,
    [CreatedBy]                        INT             NULL,
    [CreatedTime]                      DATETIME        NULL,
    [ApiTransactionId]                 BIGINT          NULL,
    CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED ([PurchaseId] ASC),
    CONSTRAINT [FK_Purchase_BankBranch_ATPBankBranchId] FOREIGN KEY ([ATPBankBranchId]) REFERENCES [dbo].[BankBranch] ([BankBranchId]),
    CONSTRAINT [FK_Purchase_CustomsAndVATCommissionarate] FOREIGN KEY ([CustomsAndVATCommissionarateId]) REFERENCES [dbo].[CustomsAndVATCommissionarate] ([CustomsAndVATCommissionarateId]),
    CONSTRAINT [FK_Purchase_NbrEconomicCodeATPNbrEconomicCodeId] FOREIGN KEY ([ATPNbrEconomicCodeId]) REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId]),
    CONSTRAINT [FK_Purchase_Organizations] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId]),
    CONSTRAINT [FK_Purchase_Organizations_TransferBranch] FOREIGN KEY ([TransferBranchId]) REFERENCES [dbo].[Organizations] ([OrganizationId]),
    CONSTRAINT [FK_Purchase_PurchaseReason] FOREIGN KEY ([PurchaseReasonId]) REFERENCES [dbo].[PurchaseReason] ([PurchaseReasonId]),
    CONSTRAINT [FK_Purchase_PurchaseTypes] FOREIGN KEY ([PurchaseTypeId]) REFERENCES [dbo].[PurchaseTypes] ([PurchaseTypeId]),
    CONSTRAINT [FK_Purchase_Sales_Transfer] FOREIGN KEY ([TransferSalesId]) REFERENCES [dbo].[Sales] ([SalesId]),
    CONSTRAINT [FK_Purchase_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendor] ([VendorId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Advance Tax Payment Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Purchase', @level2type = N'COLUMN', @level2name = N'ATPDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Advance Tax Payment Bank Branch ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Purchase', @level2type = N'COLUMN', @level2name = N'ATPBankBranchId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Advance Tax Payment Economic Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Purchase', @level2type = N'COLUMN', @level2name = N'ATPNbrEconomicCodeId';

