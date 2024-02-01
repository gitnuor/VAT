-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertBulkPurchase]
    -- Add the parameters for the stored procedure here
    @OrganizationId INT,
    @SecurityToken NVARCHAR(100),
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @PurchaseJson NVARCHAR(MAX),
    @PurchaseDetailsJson NVARCHAR(MAX),
    @PurchasePaymentJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ErrorMsg NVARCHAR(500) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;
        --Statement
        DECLARE @NumberOfDuplicateProdInSinglePurchase INT,
                @ApiTransactionId BIGINT,
                @ApiTransactionTypeId INT = 2;

        DECLARE @osdtVatType INT = 14,
                @osdtBankBranch INT = 3,
                @osdtPurchaseType INT = 15,
                @osdtPurchaseReason INT = 18,
                @osdtNBREconomicCode INT = 12,
                @osdtPaymentMethod INT = 13,
                @osdtCustomsAndVATCommissionarate INT = 6;

        DECLARE @PurchDtlRaw TABLE
        (
            [ProductId] [INT] NOT NULL,
            [ProductVATTypeId] [INT] NOT NULL,
            [Quantity] [DECIMAL](18, 2) NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
            [CustomDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [ImportDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [RegulatoryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [VATPercent] [DECIMAL](18, 2) NOT NULL,
            [AdvanceTaxPercent] [DECIMAL](18, 2) NOT NULL,
            [AdvanceIncomeTaxPercent] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL,
            [PurchaseReferenceKey] NVARCHAR(100) NULL
        );

        INSERT INTO @PurchDtlRaw
        (
            ProductId,
            ProductVATTypeId,
            Quantity,
            UnitPrice,
            DiscountPerItem,
            CustomDutyPercent,
            ImportDutyPercent,
            RegulatoryDutyPercent,
            SupplementaryDutyPercent,
            VATPercent,
            AdvanceTaxPercent,
            AdvanceIncomeTaxPercent,
            MeasurementUnitId,
            ReferenceKey,
            PurchaseReferenceKey
        )
        SELECT prod.ProductId,              -- ProductId - int
               osd.DataKey,                 -- ProductVATTypeId - int
               jd.Quantity,                 -- Quantity - decimal(18, 2)
               jd.UnitPrice,                -- UnitPrice - decimal(18, 2)
               jd.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
               jd.CustomDutyPercent,        -- CustomDutyPercent - decimal(18, 2)
               jd.ImportDutyPercent,        -- ImportDutyPercent - decimal(18, 2)
               jd.RegulatoryDutyPercent,    -- RegulatoryDutyPercent - decimal(18, 2)
               jd.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
               jd.Vatpercent,               -- VATPercent - decimal(18, 2)
               jd.AdvanceTaxPercent,        -- AdvanceTaxPercent - decimal(18, 2)
               jd.AdvanceIncomeTaxPercent,  -- AdvanceIncomeTaxPercent - decimal(18, 2)
               mu.MeasurementUnitId,        -- MeasurementUnitId - int
               jd.PurchaseDetailId,         -- ReferenceKey - nvarchar(100)
               jd.PurchaseId                -- PurchaseReferenceKey - nvarchar(100)
        FROM
            OPENJSON(@PurchaseDetailsJson)
            WITH
            (
                [PurchaseDetailId] NVARCHAR(100),
                [PurchaseId] NVARCHAR(100),
                [ProductId] NVARCHAR(100),
                [ProductVattypeId] NVARCHAR(100),
                [Quantity] [DECIMAL](18, 2),
                [UnitPrice] [DECIMAL](18, 2),
                [DiscountPerItem] [DECIMAL](18, 2),
                [CustomDutyPercent] [DECIMAL](18, 2),
                [ImportDutyPercent] [DECIMAL](18, 2),
                [RegulatoryDutyPercent] [DECIMAL](18, 2),
                [SupplementaryDutyPercent] [DECIMAL](18, 2),
                [Vatpercent] [DECIMAL](18, 2),
                [AdvanceTaxPercent] [DECIMAL](18, 2),
                [AdvanceIncomeTaxPercent] [DECIMAL](18, 2),
                [MeasurementUnitId] NVARCHAR(100)
            ) jd
            LEFT JOIN dbo.Products prod
                ON prod.ReferenceKey = jd.ProductId
            LEFT JOIN dbo.OrgStaticData osd
                ON osd.ReferenceKey = jd.ProductVattypeId
            LEFT JOIN dbo.MeasurementUnits mu
                ON mu.ReferenceKey = jd.MeasurementUnitId
        WHERE (
                  (
                      osd.OrgStaticDataTypeId = @osdtVatType
                      AND osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                  )
                  OR osd.OrgStaticDataId IS NULL
              )
              AND mu.OrganizationId = @OrganizationId;

        WITH cteSalesDetails
        AS (SELECT ROW_NUMBER() OVER (PARTITION BY pdr.PurchaseReferenceKey,
                                                   pdr.ProductId
                                      ORDER BY pdr.ProductId
                                     ) AS NumberOfSingleProdInSinglePurchase,
                   pdr.ProductId,
                   pdr.PurchaseReferenceKey
            FROM @PurchDtlRaw pdr)
        SELECT @NumberOfDuplicateProdInSinglePurchase = COUNT(1)
        FROM cteSalesDetails
        WHERE cteSalesDetails.NumberOfSingleProdInSinglePurchase > 1;
        --SELECT * FROM @PurchDtlRaw
        SET @NumberOfDuplicateProdInSinglePurchase = ISNULL(@NumberOfDuplicateProdInSinglePurchase, 0);

        IF @NumberOfDuplicateProdInSinglePurchase > 0
        BEGIN
            SET @ErrorMsg = N'Product duplication in purchase is not allowed!!';
            RAISERROR(   @ErrorMsg, -- Message text.  
                         20,        -- Severity.  
                         -1         -- State.  
                     );
        END;

        DECLARE @PurchasePayment TABLE
        (
            [PaymentMethodId] [INT] NOT NULL,
            [PaidAmount] [DECIMAL](18, 2) NOT NULL,
            [PaymentDate] [DATETIME] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL,
            [PurchaseReferenceKey] NVARCHAR(100) NULL
        );

        IF @PurchasePaymentJson IS NOT NULL
           AND LEN(@PurchasePaymentJson) > 0
        BEGIN
            INSERT INTO @PurchasePayment
            (
                PaymentMethodId,
                PaidAmount,
                PaymentDate,
                ReferenceKey,
                PurchaseReferenceKey
            )
            SELECT osd.DataKey,
                   jd.PaidAmount,
                   jd.PaymentDate,
                   jd.PurchasePaymentId,
                   jd.PurchaseId
            FROM
                OPENJSON(@PurchasePaymentJson)
                WITH
                (
                    [PurchasePaymentId] NVARCHAR(100),
                    [PurchaseId] NVARCHAR(100),
                    [PaymentMethodId] [INT],
                    [PaidAmount] [DECIMAL](18, 2),
                    [PaymentDate] [DATETIME]
                ) jd
                LEFT JOIN dbo.OrgStaticData osd
                    ON osd.ReferenceKey = jd.PaymentMethodId
            WHERE (
                      osd.OrgStaticDataTypeId = @osdtPaymentMethod
                      AND osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                  )
                  OR osd.OrgStaticDataId IS NULL;

        END;

        DECLARE @Purchase TABLE
        (
            [OrganizationId] [INT] NOT NULL,
            [TransferSalesId] [INT] NULL,
            [TransferBranchId] [INT] NULL,
            [VendorId] [INT] NULL,
            [VatChallanNo] [NVARCHAR](50) NULL,
            [VatChallanIssueDate] [DATETIME] NULL,
            [VendorInvoiceNo] [NVARCHAR](50) NULL,
            [InvoiceNo] [NVARCHAR](50) NOT NULL,
            [PurchaseDate] [DATETIME] NOT NULL,
            [PurchaseTypeId] [INT] NOT NULL,
            [PurchaseReasonId] [INT] NOT NULL,
            [NoOfIteams] [INT] NOT NULL,
            [TotalPriceWithoutVat] [DECIMAL](18, 2) NOT NULL,
            [DiscountOnTotalPrice] [DECIMAL](18, 2) NOT NULL,
            [TotalDiscountOnIndividualProduct] [DECIMAL](18, 2) NOT NULL,
            [TotalImportDuty] [DECIMAL](18, 2) NOT NULL,
            [TotalRegulatoryDuty] [DECIMAL](18, 2) NOT NULL,
            [TotalSupplementaryDuty] [DECIMAL](18, 2) NOT NULL,
            [TotalVAT] [DECIMAL](18, 2) NOT NULL,
            [TotalAdvanceTax] [DECIMAL](18, 2) NOT NULL,
            [TotalAdvanceIncomeTax] [DECIMAL](18, 2) NOT NULL,
            [AdvanceTaxPaidAmount] [DECIMAL](18, 2) NULL,
            [ATPDate] [DATETIME] NULL,
            [ATPBankBranchId] [INT] NULL,
            [ATPNbrEconomicCodeId] [INT] NULL,
            [ATPChallanNo] [NVARCHAR](20) NULL,
            [IsVatDeductedInSource] [BIT] NOT NULL,
            [VDSAmount] [DECIMAL](18, 2) NULL,
            [IsVDSCertificatePrinted] [BIT] NULL,
            [VDSCertificateNo] [NVARCHAR](50) NULL,
            [VDSCertificateDate] [DATETIME] NULL,
            [VDSPaymentBookTransferNo] [NVARCHAR](50) NULL,
            [VDSNote] [NVARCHAR](500) NULL,
            [PaidAmount] [DECIMAL](18, 2) NULL,
            [ExpectedDeliveryDate] [DATETIME] NULL,
            [DeliveryDate] [DATETIME] NULL,
            [LcNo] [NVARCHAR](50) NULL,
            [LcDate] [DATETIME] NULL,
            [BillOfEntry] [NVARCHAR](50) NULL,
            [BillOfEntryDate] [DATETIME] NULL,
            [CustomsAndVATCommissionarateId] [INT] NULL,
            [DueDate] [DATETIME] NULL,
            [TermsOfLc] [NVARCHAR](500) NULL,
            [PoNumber] [NVARCHAR](50) NULL,
            [ReferenceKey] [NVARCHAR](100) NULL,
            [IsComplete] [BIT] NOT NULL,
            [CreatedBy] [INT] NULL,
            [CreatedTime] [DATETIME] NULL
        );

        DECLARE @PurchaseReference TABLE
        (
            [PurchaseId] [INT] NOT NULL,
            [ReferenceKey] [NVARCHAR](100) NULL,
            [CreatedBy] [INT] NULL,
            [CreatedTime] [DATETIME] NULL
        );

        --Variables for cursor
        DECLARE @PurcRefPurchaseId INT,
                @PurcRefReferenceKey NVARCHAR(100),
                @PurcRefCreatedBy INT,
                @PurcRefCreatedTime DATETIME;
        --/Variables for cursor

        INSERT INTO @Purchase
        (
            OrganizationId,
            TransferSalesId,
            TransferBranchId,
            VendorId,
            VatChallanNo,
            VatChallanIssueDate,
            VendorInvoiceNo,
            InvoiceNo,
            PurchaseDate,
            PurchaseTypeId,
            PurchaseReasonId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalImportDuty,
            TotalRegulatoryDuty,
            TotalSupplementaryDuty,
            TotalVAT,
            TotalAdvanceTax,
            TotalAdvanceIncomeTax,
            AdvanceTaxPaidAmount,
            ATPDate,
            ATPBankBranchId,
            ATPNbrEconomicCodeId,
            ATPChallanNo,
            IsVatDeductedInSource,
            VDSAmount,
            IsVDSCertificatePrinted,
            VDSCertificateNo,
            VDSCertificateDate,
            VDSPaymentBookTransferNo,
            VDSNote,
            PaidAmount,
            ExpectedDeliveryDate,
            DeliveryDate,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            CustomsAndVATCommissionarateId,
            DueDate,
            TermsOfLc,
            PoNumber,
            ReferenceKey,
            IsComplete,
            CreatedBy,
            CreatedTime
        )
        SELECT @OrganizationId,
               jdPurch.TransferSalesId,
               jdPurch.TransferBranchId,
               vndr.VendorId,
               jdPurch.VatChallanNo,
               jdPurch.VatChallanIssueDate,
               jdPurch.VendorInvoiceNo,
               jdPurch.InvoiceNo,
               jdPurch.PurchaseDate,
               osdPurchaseType.DataKey,
               osdPurchaseReason.DataKey,
               ISNULL(pd.NoOfIteams, 0),
               ISNULL(pd.TotalPriceWithoutVat, 0),
               jdPurch.DiscountOnTotalPrice,
               ISNULL(pd.TotalDiscountOnIndividualProduct, 0),
               ISNULL(pd.TotalImportDuty, 0),
               ISNULL(pd.TotalRegulatoryDuty, 0),
               ISNULL(pd.TotalSupplimentaryDuty, 0),
               ISNULL(pd.TotalVAT, 0),
               ISNULL(pd.TotalAdvanceTax, 0),
               ISNULL(pd.TotalAdvanceIncomeTax, 0),
               jdPurch.AdvanceTaxPaidAmount,
               jdPurch.Atpdate,
               osdATPBankBranch.DataKey,
               osdATPNBREconomicCode.DataKey,
               jdPurch.ATPChallanNo,
               jdPurch.IsVatDeductedInSource,
               jdPurch.Vdsamount,
               jdPurch.IsVdscertificatePrinted,
               jdPurch.VdscertificateNo,
               jdPurch.VdscertificateDate,
               jdPurch.VdspaymentBookTransferNo,
               jdPurch.Vdsnote,
               ISNULL(pmntInfo.PaidAmount, 0),
               jdPurch.ExpectedDeliveryDate,
               jdPurch.DeliveryDate,
               jdPurch.LcNo,
               jdPurch.LcDate,
               jdPurch.BillOfEntry,
               jdPurch.BillOfEntryDate,
               osdCustomsAndVATCommissionarate.DataKey,
               jdPurch.DueDate,
               jdPurch.TermsOfLc,
               jdPurch.PoNumber,
               jdPurch.PurchaseId,
               jdPurch.IsComplete,
               jdPurch.CreatedBy,
               jdPurch.CreatedTime
        FROM
            OPENJSON(@PurchaseJson)
            WITH
            (
                [PurchaseId] NVARCHAR(100),
                [TransferSalesId] NVARCHAR(100),
                [TransferBranchId] NVARCHAR(100),
                [VendorId] NVARCHAR(100),
                [VatChallanNo] [NVARCHAR](50),
                [VatChallanIssueDate] [DATETIME],
                [VendorInvoiceNo] [NVARCHAR](50),
                [InvoiceNo] [NVARCHAR](50),
                [PurchaseDate] [DATETIME],
                [PurchaseTypeId] NVARCHAR(100),
                [PurchaseReasonId] NVARCHAR(100),
                [DiscountOnTotalPrice] [DECIMAL](18, 2),
                [AdvanceTaxPaidAmount] [DECIMAL](18, 2),
                [Atpdate] [DATETIME],
                [AtpbankBranchId] NVARCHAR(100),
                [AtpnbrEconomicCodeId] NVARCHAR(100),
                [ATPChallanNo] [NVARCHAR](20),
                [IsVatDeductedInSource] [BIT],
                [Vdsamount] [DECIMAL](18, 2),
                [IsVdscertificatePrinted] [BIT],
                [VdscertificateNo] [NVARCHAR](50),
                [VdscertificateDate] [DATETIME],
                [VdspaymentBookTransferNo] [NVARCHAR](50),
                [Vdsnote] [NVARCHAR](500),
                [ExpectedDeliveryDate] [DATETIME],
                [DeliveryDate] [DATETIME],
                [LcNo] [NVARCHAR](50),
                [LcDate] [DATETIME],
                [BillOfEntry] [NVARCHAR](50),
                [BillOfEntryDate] [DATETIME],
                [CustomsAndVatcommissionarateId] NVARCHAR(100),
                [DueDate] [DATETIME],
                [TermsOfLc] [NVARCHAR](500),
                [PoNumber] [NVARCHAR](50),
                [IsComplete] [BIT],
                [CreatedBy] [INT],
                [CreatedTime] [DATETIME]
            ) jdPurch
            LEFT JOIN dbo.OrgStaticData osdPurchaseType
                ON osdPurchaseType.ReferenceKey = jdPurch.PurchaseTypeId
            LEFT JOIN dbo.OrgStaticData osdPurchaseReason
                ON osdPurchaseReason.ReferenceKey = jdPurch.PurchaseReasonId
            LEFT JOIN dbo.OrgStaticData osdATPBankBranch
                ON osdATPBankBranch.ReferenceKey = jdPurch.AtpbankBranchId
            LEFT JOIN dbo.OrgStaticData osdATPNBREconomicCode
                ON osdATPNBREconomicCode.ReferenceKey = jdPurch.AtpnbrEconomicCodeId
            LEFT JOIN dbo.OrgStaticData osdCustomsAndVATCommissionarate
                ON osdCustomsAndVATCommissionarate.ReferenceKey = jdPurch.CustomsAndVatcommissionarateId
            LEFT JOIN dbo.Vendor vndr
                ON vndr.ReferenceKey = jdPurch.VendorId
            LEFT JOIN
            (
                SELECT pdr.PurchaseReferenceKey,
                       SUM(pdr.Quantity * pdr.UnitPrice) AS TotalPriceWithoutVat,
                       SUM(pdr.Quantity * pdr.DiscountPerItem) AS TotalDiscountOnIndividualProduct,
                       SUM(dbo.FnGetCalculatedInputVat(
                                                          pdr.Quantity * pdr.UnitPrice,
                                                          pdr.VATPercent,
                                                          pdr.SupplementaryDutyPercent
                                                      )
                          ) AS TotalVAT,
                       SUM(dbo.FnGetCalculatedOutputSuppDuty(pdr.Quantity * pdr.UnitPrice, pdr.SupplementaryDutyPercent)) AS TotalSupplimentaryDuty,
                       SUM(ISNULL(pdr.Quantity * pdr.UnitPrice * pdr.ImportDutyPercent / 100, 0)) AS TotalImportDuty,
                       SUM(ISNULL(pdr.Quantity * pdr.UnitPrice * pdr.RegulatoryDutyPercent / 100, 0)) AS TotalRegulatoryDuty,
                       SUM(ISNULL(pdr.Quantity * pdr.UnitPrice * pdr.AdvanceTaxPercent / 100, 0)) AS TotalAdvanceTax,
                       SUM(ISNULL(pdr.Quantity * pdr.UnitPrice * pdr.AdvanceIncomeTaxPercent / 100, 0)) AS TotalAdvanceIncomeTax,
                       COUNT(1) AS NoOfIteams
                FROM @PurchDtlRaw pdr
                GROUP BY pdr.PurchaseReferenceKey
            ) pd
                ON pd.PurchaseReferenceKey = jdPurch.PurchaseId
            LEFT JOIN
            (
                SELECT SUM(prr.PaidAmount) AS PaidAmount,
                       prr.PurchaseReferenceKey
                FROM @PurchasePayment prr
                GROUP BY prr.PurchaseReferenceKey
            ) pmntInfo
                ON pmntInfo.PurchaseReferenceKey = jdPurch.PurchaseId
        WHERE (
                  (
                      osdPurchaseType.OrgStaticDataTypeId = @osdtPurchaseType
                      AND osdPurchaseType.OrganizationId = @OrganizationId
                      AND osdPurchaseType.IsActive = 1
                  )
                  OR osdPurchaseType.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdPurchaseReason.OrgStaticDataTypeId = @osdtPurchaseReason
                      AND osdPurchaseReason.OrganizationId = @OrganizationId
                      AND osdPurchaseReason.IsActive = 1
                  )
                  OR osdPurchaseReason.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdATPBankBranch.OrgStaticDataTypeId = @osdtBankBranch
                      AND osdATPBankBranch.OrganizationId = @OrganizationId
                      AND osdATPBankBranch.IsActive = 1
                  )
                  OR osdATPBankBranch.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdATPNBREconomicCode.OrgStaticDataTypeId = @osdtNBREconomicCode
                      AND osdATPNBREconomicCode.OrganizationId = @OrganizationId
                      AND osdATPNBREconomicCode.IsActive = 1
                  )
                  OR osdATPNBREconomicCode.OrgStaticDataId IS NULL
              )
              AND
              (
                  (
                      osdCustomsAndVATCommissionarate.OrgStaticDataTypeId = @osdtCustomsAndVATCommissionarate
                      AND osdCustomsAndVATCommissionarate.OrganizationId = @OrganizationId
                      AND osdCustomsAndVATCommissionarate.IsActive = 1
                  )
                  OR osdCustomsAndVATCommissionarate.OrgStaticDataId IS NULL
              );



        INSERT INTO dbo.ApiTransaction
        (
            ApiTransactionTypeId,
            SecurityToken,
            NumberOfItem,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @ApiTransactionTypeId, -- ApiTransactionTypeId - int
            @SecurityToken,        -- SecurityToken - nvarchar(100)
            (
                SELECT COUNT(1) FROM @Purchase purc
            ),                     -- NumberOfItem - int
            @CreatedBy,            -- CreatedBy - int
            @CreatedTime           -- CreatedTime - datetime
            );

        SET @ApiTransactionId = SCOPE_IDENTITY();

        INSERT INTO dbo.Purchase
        (
            OrganizationId,
            TransferSalesId,
            TransferBranchId,
            VendorId,
            VatChallanNo,
            VatChallanIssueDate,
            VendorInvoiceNo,
            InvoiceNo,
            PurchaseDate,
            PurchaseTypeId,
            PurchaseReasonId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalImportDuty,
            TotalRegulatoryDuty,
            TotalSupplementaryDuty,
            TotalVAT,
            TotalAdvanceTax,
            TotalAdvanceIncomeTax,
            AdvanceTaxPaidAmount,
            ATPDate,
            ATPBankBranchId,
            ATPNbrEconomicCodeId,
            ATPChallanNo,
            IsVatDeductedInSource,
            VDSAmount,
            IsVDSCertificatePrinted,
            VDSCertificateNo,
            VDSCertificateDate,
            VDSPaymentBookTransferNo,
            VDSNote,
            PaidAmount,
            ExpectedDeliveryDate,
            DeliveryDate,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            CustomsAndVATCommissionarateId,
            DueDate,
            TermsOfLc,
            PoNumber,
            ReferenceKey,
            IsComplete,
            CreatedBy,
            CreatedTime,
            ApiTransactionId
        )
        OUTPUT Inserted.PurchaseId,
               Inserted.ReferenceKey,
               Inserted.CreatedBy,
               Inserted.CreatedTime
        INTO @PurchaseReference
        (
            PurchaseId,
            ReferenceKey,
            CreatedBy,
            CreatedTime
        )
        SELECT purch.OrganizationId,
               purch.TransferSalesId,
               purch.TransferBranchId,
               purch.VendorId,
               purch.VatChallanNo,
               purch.VatChallanIssueDate,
               purch.VendorInvoiceNo,
               purch.InvoiceNo,
               purch.PurchaseDate,
               purch.PurchaseTypeId,
               purch.PurchaseReasonId,
               purch.NoOfIteams,
               purch.TotalPriceWithoutVat,
               purch.DiscountOnTotalPrice,
               purch.TotalDiscountOnIndividualProduct,
               purch.TotalImportDuty,
               purch.TotalRegulatoryDuty,
               purch.TotalSupplementaryDuty,
               purch.TotalVAT,
               purch.TotalAdvanceTax,
               purch.TotalAdvanceIncomeTax,
               purch.AdvanceTaxPaidAmount,
               purch.ATPDate,
               purch.ATPBankBranchId,
               purch.ATPNbrEconomicCodeId,
               purch.ATPChallanNo,
               purch.IsVatDeductedInSource,
               purch.VDSAmount,
               purch.IsVDSCertificatePrinted,
               purch.VDSCertificateNo,
               purch.VDSCertificateDate,
               purch.VDSPaymentBookTransferNo,
               purch.VDSNote,
               purch.PaidAmount,
               purch.ExpectedDeliveryDate,
               purch.DeliveryDate,
               purch.LcNo,
               purch.LcDate,
               purch.BillOfEntry,
               purch.BillOfEntryDate,
               purch.CustomsAndVATCommissionarateId,
               purch.DueDate,
               purch.TermsOfLc,
               purch.PoNumber,
               purch.ReferenceKey,
               purch.IsComplete,
               purch.CreatedBy,
               purch.CreatedTime,
               @ApiTransactionId
        FROM @Purchase purch;


        --Sales Detail Insert
        DECLARE PURCHASE_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY FOR
        SELECT purcRef.PurchaseId,
               purcRef.ReferenceKey,
               purcRef.CreatedBy,
               purcRef.CreatedTime
        FROM @PurchaseReference purcRef;
        OPEN PURCHASE_CURSOR;
        FETCH NEXT FROM PURCHASE_CURSOR
        INTO @PurcRefPurchaseId,
             @PurcRefReferenceKey,
             @PurcRefCreatedBy,
             @PurcRefCreatedTime;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            --Insert Purchase Details
            INSERT INTO dbo.PurchaseDetails
            (
                PurchaseId,
                ProductId,
                ProductVATTypeId,
                Quantity,
                UnitPrice,
                DiscountPerItem,
                CustomDutyPercent,
                ImportDutyPercent,
                RegulatoryDutyPercent,
                SupplementaryDutyPercent,
                VATPercent,
                AdvanceTaxPercent,
                AdvanceIncomeTaxPercent,
                MeasurementUnitId,
                ReferenceKey,
                CreatedBy,
                CreatedTime
            )
            SELECT @PurcRefPurchaseId,          -- PurchaseId - int
                   pd.ProductId,                -- ProductId - int
                   pd.ProductVATTypeId,         -- ProductVATTypeId - int
                   pd.Quantity,                 -- Quantity - decimal(18, 2)
                   pd.UnitPrice,                -- UnitPrice - decimal(18, 2)
                   pd.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
                   pd.CustomDutyPercent,        -- CustomDutyPercent - decimal(18, 2)
                   pd.ImportDutyPercent,        -- ImportDutyPercent - decimal(18, 2)
                   pd.RegulatoryDutyPercent,    -- RegulatoryDutyPercent - decimal(18, 2)
                   pd.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
                   pd.VATPercent,               -- VATPercent - decimal(18, 2)
                   pd.AdvanceTaxPercent,        -- AdvanceTaxPercent - decimal(18, 2)
                   pd.AdvanceIncomeTaxPercent,  -- AdvanceIncomeTaxPercent - decimal(18, 2)
                   pd.MeasurementUnitId,        -- MeasurementUnitId - int
                   pd.ReferenceKey,             -- ReferenceKey - nvarchar(100)
                   @PurcRefCreatedBy,           -- CreatedBy - int
                   @PurcRefCreatedTime          -- CreatedTime - datetime
            FROM @PurchDtlRaw pd
            WHERE pd.PurchaseReferenceKey = @PurcRefReferenceKey;

            --Insert into ProductTransactionBook
            INSERT INTO dbo.ProductTransactionBook
            (
                OrganizationId,
                ProductId,
                InitUnitPrice,
                InitQty,
                PurchaseDetailId,
                DebitNoteDetailId,
                UsedInProductionId,
                ProductionReceiveId,
                SalesDetailId,
                CreditNoteDetailId,
                DamageId,
                TransactionTime
            )
            SELECT @OrganizationId,                      -- OrganizationId - int
                   pd.ProductId,                         -- ProductId - int
                   ISNULL(ptbLastPrice.EndUnitPrice, 0), -- InitUnitPrice - decimal(19, 4)
                   ISNULL(ptbLastPrice.EndQty, 0),       -- InitQty - decimal(19, 4)
                   pd.PurchaseDetailId,                  -- PurchaseDetailId - int
                   NULL,                                 -- DebitNoteDetailId - int
                   NULL,                                 -- UsedInProductionId - bigint
                   NULL,                                 -- ProductionReceiveId - bigint
                   NULL,                                 -- SalesDetailId - int
                   NULL,                                 -- CreditNoteDetailId - int
                   NULL,                                 -- DamageId - int
                   pd.CreatedTime                        -- TransactionTime - datetime 
            FROM dbo.PurchaseDetails pd
                LEFT JOIN
                (
                    SELECT ptb.ProductId,
                           ComputedColumn.EndQty AS EndQty,
                           CASE
                               WHEN ComputedColumn.EndQty = 0 THEN
                                   0
                               ELSE
                                   ComputedColumn.EndPrice / ComputedColumn.EndQty
                           END AS EndUnitPrice
                    FROM dbo.ProductTransactionBook ptb
                        LEFT JOIN dbo.PurchaseDetails purchDtl
                            ON purchDtl.PurchaseDetailId = ptb.PurchaseDetailId
                        LEFT JOIN dbo.DebitNoteDetail dnd
                            ON dnd.DebitNoteDetailId = ptb.DebitNoteDetailId
                        LEFT JOIN dbo.PurchaseDetails purchDtlDnd
                            ON purchDtlDnd.PurchaseDetailId = dnd.PurchaseDetailId
                        LEFT JOIN dbo.BillOfMaterial bom
                            ON bom.BillOfMaterialId = ptb.UsedInProductionId
                        LEFT JOIN dbo.ProductionReceive pr
                            ON pr.ProductionReceiveId = ptb.ProductionReceiveId
                        LEFT JOIN dbo.PriceSetup ps
                            ON ps.PriceSetupId = pr.PriceSetupId
                        LEFT JOIN dbo.SalesDetails slsDtl
                            ON slsDtl.SalesDetailId = ptb.SalesDetailId
                        LEFT JOIN dbo.CreditNoteDetail cnd
                            ON cnd.CreditNoteDetailId = ptb.CreditNoteDetailId
                        LEFT JOIN dbo.SalesDetails slsDtlCnd
                            ON slsDtlCnd.SalesDetailId = cnd.SalesDetailId
                        LEFT JOIN dbo.Damage dmg
                            ON dmg.DamageId = ptb.DamageId
                        INNER JOIN
                        (
                            SELECT MAX(ptb.ProductTransactionBookId) AS ProductTransactionBookId,
                                   ptb.ProductId
                            FROM dbo.ProductTransactionBook ptb
                                INNER JOIN dbo.PurchaseDetails pdToCompare
                                    ON pdToCompare.ProductId = ptb.ProductId
                                       AND pdToCompare.PurchaseId = @PurcRefPurchaseId
                            WHERE ptb.OrganizationId = @OrganizationId
                            GROUP BY ptb.ProductId
                        ) lastPtb
                            ON lastPtb.ProductTransactionBookId = ptb.ProductTransactionBookId
                        CROSS APPLY
                    (
                        SELECT (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                                - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0)
                                - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                               ) AS EndQty,
                               (ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
                                - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0)
                                - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
                                + ISNULL(pr.ReceiveQuantity * ps.SalesUnitPrice, 0)
                                - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0)
                                + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
                                - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0)
                               ) AS EndPrice
                    ) AS ComputedColumn
                ) ptbLastPrice
                    ON ptbLastPrice.ProductId = pd.ProductId
            WHERE pd.PurchaseId = @PurcRefPurchaseId;


            FETCH NEXT FROM PURCHASE_CURSOR
            INTO @PurcRefPurchaseId,
                 @PurcRefReferenceKey,
                 @PurcRefCreatedBy,
                 @PurcRefCreatedTime;
        END;
        CLOSE PURCHASE_CURSOR;
        DEALLOCATE PURCHASE_CURSOR;

        --/Sales Detail Insert


        INSERT INTO dbo.PurchasePayment
        (
            PurchaseId,
            PaymentMethodId,
            PaidAmount,
            PaymentDate,
            ReferenceKey,
            CreatedBy,
            CreatedTime,
            ApiTransactionId
        )
        SELECT purcRef.PurchaseId,  -- PurchaseId - int
               pp.PaymentMethodId,  -- PaymentMethodId - int
               pp.PaidAmount,       -- PaidAmount - decimal(18, 2)
               pp.PaymentDate,      -- PaymentDate - datetime
               pp.ReferenceKey,     -- ReferenceKey - nvarchar(100)
               purcRef.CreatedBy,   -- CreatedBy - int
               purcRef.CreatedTime, -- CreatedTime - datetime
               @ApiTransactionId    -- ApiTransactionId - bigint
        FROM @PurchasePayment pp
            INNER JOIN @PurchaseReference purcRef
                ON purcRef.ReferenceKey = pp.PurchaseReferenceKey;

        --Insert content
        IF @ContentJson IS NOT NULL
           AND LEN(@ContentJson) > 0
        BEGIN
            INSERT INTO dbo.Content
            (
                DocumentTypeId,
                OrganizationId,
                FileUrl,
                MimeType,
                Node,
                ObjectId,
                ObjectPrimaryKey,
                IsActive,
                CreatedBy,
                CreatedTime
            )
            SELECT dtype.DocumentTypeId,
                   @OrganizationId,
                   jd.FileUrl,
                   jd.MimeType,
                   NULL,
                   21,
                   purcRef.PurchaseId,
                   1,
                   purcRef.CreatedBy,
                   purcRef.CreatedTime
            FROM
                OPENJSON(@ContentJson)
                WITH
                (
                    [PurchaseId] NVARCHAR(100),
                    [DocumentTypeId] NVARCHAR(100),
                    [FileUrl] [NVARCHAR](500),
                    [MimeType] [NVARCHAR](50)
                ) jd
                INNER JOIN @PurchaseReference purcRef
                    ON purcRef.ReferenceKey = jd.PurchaseId
                INNER JOIN dbo.DocumentType dtype
                    ON dtype.ReferenceKey = jd.DocumentTypeId;
        END;


        --/Statement
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        IF @ErrorMsg = N''
        BEGIN
            SELECT @ErrorMsg = ERROR_MESSAGE();
        END;
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;
END;
