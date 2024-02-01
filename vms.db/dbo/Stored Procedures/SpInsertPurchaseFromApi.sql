
-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 9, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertPurchaseFromApi]
    @PurchaseId NVARCHAR(100),
    @OrganizationId INT,
    @VendorId NVARCHAR(100),
    @VatChallanNo NVARCHAR(50),
    @VatChallanIssueDate DATETIME,
    @VendorInvoiceNo NVARCHAR(50),
    @InvoiceNo NVARCHAR(50),
    @PurchaseDate DATETIME,
    @PurchaseTypeId NVARCHAR(100),
    @PurchaseReasonId NVARCHAR(100),
    @DiscountOnTotalPrice DECIMAL(18, 2),
    @IsVatDeductedInSource BIT,
    @VDSAmount DECIMAL(18, 2),
    @ExpectedDeliveryDate DATETIME,
    @DeliveryDate DATETIME,
    @LcNo NVARCHAR(50),
    @LcDate DATETIME,
    @BillOfEntry NVARCHAR(50),
    @BillOfEntryDate DATETIME,
    @DueDate DATETIME,
    @TermsOfLc NVARCHAR(500),
    @PoNumber NVARCHAR(50),
    @IsComplete BIT,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @ApiCreatedBy INT,
    @ApiCreatedTime DATETIME,
    @AdvanceTaxPaidAmount DECIMAL(18, 2),
    @ATPDate DATETIME,
    @ATPBankBranchId NVARCHAR(100),
    @ATPNbrEconomicCodeId NVARCHAR(100),
    @ATPChallanNo NVARCHAR(20),
    @CustomsAndVATCommissionarateId NVARCHAR(100),
    @SecurityToken NVARCHAR(100),
    @PurchaseOrderDetailsJson NVARCHAR(MAX),
    @PurchasePaymentJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    DECLARE @ErrorMsg NVARCHAR(500) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;
        --PRINT '1'
        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        IF @InvoiceNo IS NULL
        BEGIN
            DECLARE @maxPurchaseId INT;
            SELECT @maxPurchaseId = MAX(purch.PurchaseId)
            FROM dbo.Purchase purch;
            SET @InvoiceNo = N'PINV' + CAST((ISNULL(@maxPurchaseId, 0) + 1) AS NVARCHAR(25));
        END;
        DECLARE @osdtVatType INT = 14,
                @osdtBankBranch INT = 3,
                @osdtPurchaseType INT = 15,
                @osdtPurchaseReason INT = 18,
                @osdtNBREconomicCode INT = 12,
                @osdtPaymentMethod INT = 13,
                @osdtCustomsAndVATCommissionarate INT = 6;
        DECLARE @PurchaseDetails TABLE
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
            [ReferenceKey] NVARCHAR(100) NULL
        );
        DECLARE @PurchasePayment TABLE
        (
            [PaymentMethodId] [INT] NOT NULL,
            [PaidAmount] [DECIMAL](18, 2) NOT NULL,
            [PaymentDate] [DATETIME] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL
        );
        DECLARE @NoOfIteams INT,
                @TotalDiscountOnIndividualProduct DECIMAL(18, 2),
                @TotalImportDuty DECIMAL(18, 2),
                @TotalRegulatoryDuty DECIMAL(18, 2),
                @TotalSupplementaryDuty DECIMAL(18, 2),
                @TotalVAT DECIMAL(18, 2),
                @TotalAdvanceTax DECIMAL(18, 2),
                @TotalAdvanceIncomeTax DECIMAL(18, 2),
                @TotalPriceWithoutVat DECIMAL(18, 2),
                @PaidAmount DECIMAL(18, 2),
                @MainPurchaseId INT,
                @ApiTransactionId BIGINT,
                @ApiTransactionTypeId INT = 4;


        INSERT INTO @PurchaseDetails
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
            ReferenceKey
        )
        SELECT prod.ProductId,
               osd.DataKey,
               jd.[Quantity],
               jd.[UnitPrice],
               jd.[DiscountPerItem],
               jd.[CustomDutyPercent],
               jd.[ImportDutyPercent],
               jd.[RegulatoryDutyPercent],
               jd.[SupplementaryDutyPercent],
               jd.[Vatpercent],
               jd.[AdvanceTaxPercent],
               jd.[AdvanceIncomeTaxPercent],
               mu.MeasurementUnitId,
               jd.PurchaseDetailId
        FROM
            OPENJSON(@PurchaseOrderDetailsJson)
            WITH
            (
                [PurchaseDetailId] NVARCHAR(100),
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
                  osd.OrgStaticDataTypeId = @osdtVatType
                  AND osd.OrganizationId = @OrganizationId
                  AND osd.IsActive = 1
              )
              AND mu.OrganizationId = @OrganizationId
              AND mu.IsActive = 1;
        --OR osd.OrgStaticDataId IS NULL;

        IF @PurchasePaymentJson IS NOT NULL
           AND LEN(@PurchasePaymentJson) > 0
        BEGIN
            INSERT INTO @PurchasePayment
            (
                PaymentMethodId,
                PaidAmount,
                PaymentDate,
                ReferenceKey
            )
            SELECT osd.DataKey,
                   jd.PaidAmount,
                   jd.PaymentDate,
                   jd.PurchasePaymentId
            FROM
                OPENJSON(@PurchasePaymentJson)
                WITH
                (
                    [PurchasePaymentId] NVARCHAR(100),
                    [PaymentMethodId] NVARCHAR(100),
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

        --PRINT '2'

        SELECT @NoOfIteams = COUNT(pod.ProductId),
               @TotalDiscountOnIndividualProduct = SUM(pod.Quantity * pod.DiscountPerItem),
               @TotalImportDuty = SUM((pod.ImportDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalRegulatoryDuty = SUM((pod.RegulatoryDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalSupplementaryDuty = SUM((pod.SupplementaryDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalVAT = SUM((pod.VATPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalAdvanceTax = SUM((pod.VATPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalAdvanceIncomeTax = SUM((pod.AdvanceIncomeTaxPercent * pod.Quantity * pod.UnitPrice) / 100),
               @TotalPriceWithoutVat = SUM(pod.Quantity * pod.UnitPrice)
        FROM @PurchaseDetails pod;

        SELECT @PaidAmount = ISNULL(SUM(pp.PaidAmount), 0)
        FROM @PurchasePayment pp;



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
            1,                     -- NumberOfItem - int
            @ApiCreatedBy,         -- CreatedBy - int
            @ApiCreatedTime        -- CreatedTime - datetime
            );

        SET @ApiTransactionId = SCOPE_IDENTITY();
        --Insert Purchase Information
        INSERT INTO dbo.Purchase
        (
            OrganizationId,
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
        VALUES
        (   @OrganizationId,
            (
                SELECT TOP (1)
                       vndr.VendorId
                FROM dbo.Vendor vndr
                WHERE vndr.OrganizationId = @OrganizationId
                      AND vndr.ReferenceKey IS NOT NULL
                      AND vndr.ReferenceKey = @VendorId
                ORDER BY vndr.VendorId
            ), @VatChallanNo, @VatChallanIssueDate, @VendorInvoiceNo, @InvoiceNo, @PurchaseDate,
            (
                SELECT TOP (1)
                       osd.DataKey
                FROM dbo.OrgStaticData osd
                WHERE osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                      AND osd.ReferenceKey = @PurchaseTypeId
                      AND osd.OrgStaticDataTypeId = @osdtPurchaseType
                ORDER BY osd.OrgStaticDataId
            ),
            (
                SELECT TOP (1)
                       osd.DataKey
                FROM dbo.OrgStaticData osd
                WHERE osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                      AND osd.ReferenceKey = @PurchaseReasonId
                      AND osd.OrgStaticDataTypeId = @osdtPurchaseReason
                ORDER BY osd.OrgStaticDataId
            ), @NoOfIteams, @TotalPriceWithoutVat, @DiscountOnTotalPrice, @TotalDiscountOnIndividualProduct,
            @TotalImportDuty, @TotalRegulatoryDuty, @TotalSupplementaryDuty, @TotalVAT, @TotalAdvanceTax,
            @TotalAdvanceIncomeTax, @AdvanceTaxPaidAmount, @ATPDate,
            (
                SELECT TOP (1)
                       osd.DataKey
                FROM dbo.OrgStaticData osd
                WHERE osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                      AND osd.ReferenceKey = @ATPBankBranchId
                      AND osd.OrgStaticDataTypeId = @osdtBankBranch
                ORDER BY osd.OrgStaticDataId
            ),
            (
                SELECT TOP (1)
                       osd.DataKey
                FROM dbo.OrgStaticData osd
                WHERE osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                      AND osd.ReferenceKey = @ATPNbrEconomicCodeId
                      AND osd.OrgStaticDataTypeId = @osdtNBREconomicCode
                ORDER BY osd.OrgStaticDataId
            ), @ATPChallanNo, @IsVatDeductedInSource, @VDSAmount, @PaidAmount, @ExpectedDeliveryDate, @DeliveryDate,
            @LcNo, @LcDate, @BillOfEntry, @BillOfEntryDate,
            (
                SELECT TOP (1)
                       osd.DataKey
                FROM dbo.OrgStaticData osd
                WHERE osd.OrganizationId = @OrganizationId
                      AND osd.IsActive = 1
                      AND osd.ReferenceKey = @CustomsAndVATCommissionarateId
                      AND osd.OrgStaticDataTypeId = @osdtCustomsAndVATCommissionarate
                ORDER BY osd.OrgStaticDataId
            ), @DueDate, @TermsOfLc, @PoNumber, @PurchaseId, @IsComplete, @CreatedBy, @CreatedTime, @ApiTransactionId);



        --PRINT '3'

        --Get PurchaseId
        SET @MainPurchaseId = SCOPE_IDENTITY();


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
        SELECT @MainPurchaseId,             -- PurchaseId - int
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
               @CreatedBy,                  -- CreatedBy - int
               @CreatedTime                 -- CreatedTime - datetime
        FROM @PurchaseDetails pd;

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
                                   AND pdToCompare.PurchaseId = @MainPurchaseId
                        WHERE ptb.OrganizationId = @OrganizationId
                        GROUP BY ptb.ProductId
                    ) lastPtb
                        ON lastPtb.ProductTransactionBookId = ptb.ProductTransactionBookId
                    CROSS APPLY
                (
                    SELECT (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                            - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                            + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
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
        WHERE pd.PurchaseId = @MainPurchaseId;

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
        SELECT @MainPurchaseId,    -- PurchaseId - int
               pp.PaymentMethodId, -- PaymentMethodId - int
               pp.PaidAmount,      -- PaidAmount - decimal(18, 2)
               pp.PaymentDate,     -- PaymentDate - datetime
               pp.ReferenceKey,    -- ReferenceKey - nvarchar(100)
               @CreatedBy,         -- CreatedBy - int
               @CreatedTime,       -- CreatedTime - datetime
               @ApiTransactionId   -- ApiTransactionId - bigint
        FROM @PurchasePayment pp;


        --PRINT '4'

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
                   @MainPurchaseId,
                   1,
                   @CreatedBy,
                   @CreatedTime
            FROM
                OPENJSON(@ContentJson)
                WITH
                (
                    [DocumentTypeId] [INT],
                    [FileUrl] [NVARCHAR](500),
                    [MimeType] [NVARCHAR](50)
                ) jd
                INNER JOIN dbo.DocumentType dtype
                    ON dtype.ReferenceKey = jd.DocumentTypeId;
        END;

        INSERT INTO dbo.AuditLog
        (
            ObjectTypeId,
            PrimaryKey,
            AuditOperationId,
            UserId,
            Datetime,
            Descriptions,
            IsActive,
            CreatedBy,
            CreatedTime,
            OrganizationId
        )
        VALUES
        (   21,              -- ObjectTypeId - int
            @MainPurchaseId, -- PrimaryKey - int
            1,               -- AuditOperationId - int
            @ApiCreatedBy,   -- UserId - int
            SYSDATETIME(),   -- Datetime - datetime2(7)
            N'',             -- Descriptions - nvarchar(4000)
            1,               -- IsActive - bit
            @ApiCreatedBy,   -- CreatedBy - int
            @ApiCreatedTime, -- CreatedTime - datetime
            @OrganizationId  -- OrganizationId - int
            );
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
