-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 9, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertPurchase]
    @OrganizationId INT,
    @VendorId INT,
    @VatChallanNo NVARCHAR(50),
    @VatChallanIssueDate DATETIME,
    @VendorInvoiceNo NVARCHAR(50),
    @InvoiceNo NVARCHAR(50),
    @PurchaseDate DATETIME,
    @PurchaseTypeId INT,
    @PurchaseReasonId INT,
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
    @MushakGenerationId INT,
    @IsComplete BIT,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @AdvanceTaxPaidAmount DECIMAL(18, 2),
    @ATPDate DATETIME,
    @ATPBankBranchId INT,
    @ATPNbrEconomicCodeId INT,
    @ATPChallanNo NVARCHAR(20),
    @CustomsAndVATCommissionarateId INT,
    @ReferenceKey NVARCHAR(100),
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
            [PaymentDate] [DATETIME] NOT NULL
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
                @PurchaseId INT;


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
        SELECT jd.[ProductId],
               jd.[ProductVattypeId],
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
               jd.[MeasurementUnitId],
               jd.ReferenceKey
        FROM
            OPENJSON(@PurchaseOrderDetailsJson)
            WITH
            (
                [ProductId] [INT],
                [ProductVattypeId] [INT],
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
                [MeasurementUnitId] [INT],
                [ReferenceKey] NVARCHAR(100)
            ) jd;

        IF @PurchasePaymentJson IS NOT NULL
           AND LEN(@PurchasePaymentJson) > 0
        BEGIN
            INSERT INTO @PurchasePayment
            (
                PaymentMethodId,
                PaidAmount,
                PaymentDate
            )
            SELECT jd.PaymentMethodId,
                   jd.PaidAmount,
                   jd.PaymentDate
            FROM
                OPENJSON(@PurchasePaymentJson)
                WITH
                (
                    [PaymentMethodId] [INT],
                    [PaidAmount] [DECIMAL](18, 2),
                    [PaymentDate] [DATETIME]
                ) jd;

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
            MushakGenerationId,
            ReferenceKey,
            IsComplete,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (@OrganizationId, @VendorId, @VatChallanNo, @VatChallanIssueDate, @VendorInvoiceNo, @InvoiceNo, @PurchaseDate,
         @PurchaseTypeId, @PurchaseReasonId, @NoOfIteams, @TotalPriceWithoutVat, @DiscountOnTotalPrice,
         @TotalDiscountOnIndividualProduct, @TotalImportDuty, @TotalRegulatoryDuty, @TotalSupplementaryDuty, @TotalVAT,
         @TotalAdvanceTax, @TotalAdvanceIncomeTax, @AdvanceTaxPaidAmount, @ATPDate, @ATPBankBranchId,
         @ATPNbrEconomicCodeId, @ATPChallanNo, @IsVatDeductedInSource, @VDSAmount, @PaidAmount, @ExpectedDeliveryDate,
         @DeliveryDate, @LcNo, @LcDate, @BillOfEntry, @BillOfEntryDate, @CustomsAndVATCommissionarateId, @DueDate,
         @TermsOfLc, @PoNumber, @MushakGenerationId, @ReferenceKey, @IsComplete, @CreatedBy, @CreatedTime);



        --PRINT '3'

        --Get PurchaseId
        SET @PurchaseId = SCOPE_IDENTITY();


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
        SELECT @PurchaseId,                 -- PurchaseId - int
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
                                   AND pdToCompare.PurchaseId = @PurchaseId
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
        WHERE pd.PurchaseId = @PurchaseId;

        INSERT INTO dbo.PurchasePayment
        (
            PurchaseId,
            PaymentMethodId,
            PaidAmount,
            PaymentDate,
            CreatedBy,
            CreatedTime
        )
        SELECT @PurchaseId,        -- PurchaseId - int
               pp.PaymentMethodId, -- PaymentMethodId - int
               pp.PaidAmount,      -- PaidAmount - decimal(18, 2)
               pp.PaymentDate,     -- PaymentDate - datetime
               @CreatedBy,         -- CreatedBy - int
               @CreatedTime        -- CreatedTime - datetime
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
            SELECT jd.DocumentTypeId,
                   @OrganizationId,
                   jd.FileUrl,
                   jd.MimeType,
                   NULL,
                   21,
                   @PurchaseId,
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
                ) jd;
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
        (   21,             -- ObjectTypeId - int
            @PurchaseId,    -- PrimaryKey - int
            1,              -- AuditOperationId - int
            @CreatedBy,     -- UserId - int
            SYSDATETIME(),  -- Datetime - datetime2(7)
            N'',            -- Descriptions - nvarchar(4000)
            1,              -- IsActive - bit
            @CreatedBy,     -- CreatedBy - int
            @CreatedTime,   -- CreatedTime - datetime
            @OrganizationId -- OrganizationId - int
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
