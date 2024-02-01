-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 9, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertTransferReceive]
    @TransferSalesId INT,
    @OrganizationId INT,
    @InvoiceNo NVARCHAR(50),
    @PurchaseDate DATETIME,
    @PurchaseReasonId INT,
    @DeliveryDate DATETIME,
    @IsComplete BIT,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    DECLARE @ErrorMsg NVARCHAR(500) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @PurchaseTypeTransReceive INT = 3;
        --PRINT '1'
        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        IF @InvoiceNo IS NULL
        BEGIN
            DECLARE @maxPurchaseId INT;
            SELECT @maxPurchaseId = MAX(purch.PurchaseId)
            FROM dbo.Purchase purch
            WHERE purch.OrganizationId = @OrganizationId;
            SET @InvoiceNo = N'TRINV' + CAST((ISNULL(@maxPurchaseId, 0) + 1) AS NVARCHAR(25));
        END;
        DECLARE @PurchaseId INT;


        --Insert Purchase Information
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
            VDSCertificateNo,
            VDSCertificateDate,
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
            IsComplete,
            CreatedBy,
            CreatedTime
        )
        SELECT @OrganizationId,                       -- OrganizationId - int
               trns.SalesId,                          -- TransferSalesId - int
               trns.OrganizationId,                   -- TransferBranchId - int
               NULL,                                  -- VendorId - int
               NULL,                                  -- VatChallanNo - nvarchar(50)
               NULL,                                  -- VatChallanIssueDate - datetime
               NULL,                                  -- VendorInvoiceNo - nvarchar(50)
               @InvoiceNo,                            -- InvoiceNo - nvarchar(50)
               @PurchaseDate,                         -- PurchaseDate - datetime
               @PurchaseTypeTransReceive,             -- PurchaseTypeId - int
               @PurchaseReasonId,                     -- PurchaseReasonId - int
               trns.NoOfIteams,                       -- NoOfIteams - int
               trns.TotalPriceWithoutVat,             -- TotalPriceWithoutVat - decimal(18, 2)
               trns.DiscountOnTotalPrice,             -- DiscountOnTotalPrice - decimal(18, 2)
               trns.TotalDiscountOnIndividualProduct, -- TotalDiscountOnIndividualProduct - decimal(18, 2)
               0,                                     -- TotalImportDuty - decimal(18, 2)
               0,                                     -- TotalRegulatoryDuty - decimal(18, 2)
               trns.TotalSupplimentaryDuty,           -- TotalSupplementaryDuty - decimal(18, 2)
               trns.TotalVAT,                         -- TotalVAT - decimal(18, 2)
               0,                                     -- TotalAdvanceTax - decimal(18, 2)
               0,                                     -- TotalAdvanceIncomeTax - decimal(18, 2)
               NULL,                                  -- AdvanceTaxPaidAmount - decimal(18, 2)
               NULL,                                  -- ATPDate - datetime
               NULL,                                  -- ATPBankBranchId - int
               NULL,                                  -- ATPNbrEconomicCodeId - int
               NULL,                                  -- ATPChallanNo - nvarchar(20)
               0,                                     -- IsVatDeductedInSource - bit
               NULL,                                  -- VDSAmount - decimal(18, 2)
               NULL,                                  -- VDSCertificateNo - nvarchar(50)
               NULL,                                  -- VDSCertificateDate - datetime
               NULL,                                  -- PaidAmount - decimal(18, 2)
               NULL,                                  -- ExpectedDeliveryDate - datetime
               @DeliveryDate,                         -- DeliveryDate - datetime
               NULL,                                  -- LcNo - nvarchar(50)
               NULL,                                  -- LcDate - datetime
               NULL,                                  -- BillOfEntry - nvarchar(50)
               NULL,                                  -- BillOfEntryDate - datetime
               NULL,                                  -- CustomsAndVATCommissionarateId - int
               NULL,                                  -- DueDate - datetime
               NULL,                                  -- TermsOfLc - nvarchar(500)
               NULL,                                  -- PoNumber - nvarchar(50)
               NULL,                                  -- MushakGenerationId - int
               @IsComplete,                           -- IsComplete - bit
               @CreatedBy,                            -- CreatedBy - int
               @CreatedTime                           -- CreatedTime - datetime
        FROM dbo.Sales trns
        WHERE trns.SalesId = @TransferSalesId;
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
            CreatedBy,
            CreatedTime
        )
        SELECT @PurchaseId,                      -- PurchaseId - int
               trnsDtl.ProductId,                -- ProductId - int
               trnsDtl.ProductVATTypeId,         -- ProductVATTypeId - int
               trnsDtl.Quantity,                 -- Quantity - decimal(18, 2)
               trnsDtl.UnitPrice,                -- UnitPrice - decimal(18, 2)
               trnsDtl.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
               0,                                -- CustomDutyPercent - decimal(18, 2)
               0,                                -- ImportDutyPercent - decimal(18, 2)
               0,                                -- RegulatoryDutyPercent - decimal(18, 2)
               trnsDtl.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
               trnsDtl.VATPercent,               -- VATPercent - decimal(18, 2)
               0,                                -- AdvanceTaxPercent - decimal(18, 2)
               0,                                -- AdvanceIncomeTaxPercent - decimal(18, 2)
               trnsDtl.MeasurementUnitId,        -- MeasurementUnitId - int
               @CreatedBy,                       -- CreatedBy - int
               @CreatedTime                      -- CreatedTime - datetime
        FROM dbo.SalesDetails trnsDtl
        WHERE trnsDtl.SalesId = @TransferSalesId;

        --SELECT

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
