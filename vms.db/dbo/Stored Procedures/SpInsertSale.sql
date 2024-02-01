-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 29, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertSale]
    @InvoiceNo NVARCHAR(50),
    @VatChallanNo NVARCHAR(50),
    @OrganizationId INT,
    @DiscountOnTotalPrice DECIMAL(18, 2),
    @IsVatDeductedInSource BIT,
    @VDSAmount DECIMAL(18, 2),
    @CustomerId INT,
    @ReceiverName NVARCHAR(200),
    @ReceiverContactNo VARCHAR(20),
    @ShippingAddress NVARCHAR(200),
    @ShippingCountryId INT,
    @SalesTypeId INT,
    @SalesDeliveryTypeId INT,
    @WorkOrderNo NVARCHAR(50),
    @SalesDate DATETIME,
    @ExpectedDeliveryDate DATETIME,
    @DeliveryDate DATETIME,
    @DeliveryMethodId INT,
    @ExportTypeId INT,
    @LcNo NVARCHAR(50),
    @LcDate DATETIME,
    @BillOfEntry NVARCHAR(50),
    @BillOfEntryDate DATETIME,
    @DueDate DATETIME,
    @TermsOfLc NVARCHAR(500),
    @CustomerPoNumber NVARCHAR(50),
    @OtherBranchOrganizationId INT,
    @IsComplete BIT,
    @IsTaxInvoicePrined BIT,
    @TaxInvoicePrintedTime DATETIME,
    @ReferenceKey NVARCHAR(100),
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @SalesDetailsJson NVARCHAR(MAX),
    @PaymentReceiveJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(100) = N'';
        DECLARE @NoOfIteams INT,
                @TotalPriceWithoutVat DECIMAL(18, 2),
                @TotalDiscountOnIndividualProduct DECIMAL(18, 2),
                @TotalVAT DECIMAL(18, 2),
                @TotalSupplimentaryDuty DECIMAL(18, 2),
                @PaymentReceiveAmount DECIMAL(18, 2),
                @ChallanNumber INT = 0,
                @NoOfItemExceedStock INT = 0,
                @SalesId INT;

        IF @IsTaxInvoicePrined = 1
        BEGIN
            IF @VatChallanNo IS NULL
               OR @VatChallanNo = N''
            BEGIN
                SELECT @ChallanNumber = COUNT(1)
                FROM dbo.Sales sls
                WHERE sls.IsTaxInvoicePrined = 1
                      AND sls.OrganizationId = @OrganizationId;
                SET @ChallanNumber = ISNULL(@ChallanNumber, 0) + 1;
                SET @VatChallanNo = N'VCN' + CAST(@ChallanNumber AS NVARCHAR(25));
                SET @TaxInvoicePrintedTime = GETDATE();
            END;
        END;
        ELSE
        BEGIN
            SET @VatChallanNo = NULL;
            SET @TaxInvoicePrintedTime = NULL;
        END;


        DECLARE @SlsDtlMain TABLE
        (
            [SlsDtlMainSl] [INT] NOT NULL,
            [ProductId] [INT] NOT NULL,
            [ProductVATTypeId] [INT] NOT NULL,
            [ProductTransactionBookId] [BIGINT] NOT NULL,
            [Quantity] [DECIMAL](18, 2) NOT NULL,
            [CurrentStock] [DECIMAL](18, 2) NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
            [VATPercent] [DECIMAL](18, 2) NOT NULL,
            [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL,
            [ReferenceKey] NVARCHAR(100) NULL
        );

        DECLARE @PaymentReceive TABLE
        (
            [ReceivedPaymentMethodId] [INT] NOT NULL,
            [ReceiveAmount] [DECIMAL](18, 2) NOT NULL,
            [ReceiveDate] [DATETIME] NOT NULL
        );

        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        SET @SalesDate = ISNULL(@SalesDate, @CreatedTime);


        DECLARE @MaxSalseId INT;
        SELECT @MaxSalseId = MAX(SalesId)
        FROM dbo.Sales;
        SET @InvoiceNo = ISNULL(@InvoiceNo, N'SINV' + CAST(ISNULL(@MaxSalseId, 0) + 1 AS NVARCHAR(25)));


        IF @PaymentReceiveJson IS NOT NULL
           AND LEN(@PaymentReceiveJson) > 0
        BEGIN
            INSERT INTO @PaymentReceive
            (
                ReceivedPaymentMethodId,
                ReceiveAmount,
                ReceiveDate
            )
            SELECT jd.ReceivedPaymentMethodId,
                   jd.ReceiveAmount,
                   jd.ReceiveDate
            FROM
                OPENJSON(@PaymentReceiveJson)
                WITH
                (
                    [ReceivedPaymentMethodId] [INT],
                    [ReceiveAmount] [DECIMAL](18, 2),
                    [ReceiveDate] [DATETIME]
                ) jd;

        END;


        INSERT INTO @SlsDtlMain
        (
            SlsDtlMainSl,
            ProductId,
            ProductVATTypeId,
            ProductTransactionBookId,
            Quantity,
            CurrentStock,
            UnitPrice,
            DiscountPerItem,
            VATPercent,
            SupplementaryDutyPercent,
            MeasurementUnitId,
            ReferenceKey
        )
        SELECT ROW_NUMBER() OVER (ORDER BY jd.ProductId) AS SlsDtlMainSl, -- SlsDtlMainSl - int
               jd.ProductId,                                              -- ProductId - int
               jd.ProductVattypeId,                                       -- ProductVATTypeId - int
               ISNULL(transBook.ProductTransactionBookId, 0),             -- ProductTransactionBookId - bigint
               jd.Quantity,                                               -- Quantity - decimal(18, 2)
               ISNULL(transBook.EndQty, 0),                               -- CurrentStock - decimal(18, 2)
               jd.UnitPrice,                                              -- UnitPrice - decimal(18, 2)
               jd.DiscountPerItem,                                        -- DiscountPerItem - decimal(18, 2)
               jd.Vatpercent,                                             -- VATPercent - decimal(18, 2)
               jd.SupplementaryDutyPercent,                               -- SupplementaryDutyPercent - decimal(18, 2)
               jd.MeasurementUnitId,                                      -- MeasurementUnitId - int
               jd.ReferenceKey                                            -- ReferenceKey - nvarchar(100)
        FROM
            OPENJSON(@SalesDetailsJson)
            WITH
            (
                [ProductId] [INT],
                [ProductVattypeId] [INT],
                [Quantity] [DECIMAL](18, 2),
                [UnitPrice] [DECIMAL](18, 2),
                [DiscountPerItem] [DECIMAL](18, 2),
                [Vatpercent] [DECIMAL](18, 2),
                [SupplementaryDutyPercent] [DECIMAL](18, 2),
                [MeasurementUnitId] [INT],
                [ReferenceKey] NVARCHAR(100)
            ) jd
            LEFT JOIN
            (
                SELECT ptb.ProductTransactionBookId,
                       ptb.ProductId,
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
                        SELECT MAX(ptb.ProductTransactionBookId) AS ProductTransactionBookId
                        FROM dbo.ProductTransactionBook ptb
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
            ) transBook
                ON transBook.ProductId = jd.ProductId;

        SELECT @PaymentReceiveAmount = ISNULL(SUM(pr.ReceiveAmount), 0)
        FROM @PaymentReceive pr;
        SELECT @NoOfIteams = MAX(sdm.SlsDtlMainSl),
               @TotalPriceWithoutVat = SUM(sdm.UnitPrice * sdm.Quantity),
               @TotalDiscountOnIndividualProduct = SUM(sdm.DiscountPerItem * sdm.Quantity),
               @TotalVAT
                   = SUM(dbo.FnGetCalculatedOutputVat(
                                                         sdm.UnitPrice * sdm.Quantity,
                                                         sdm.VATPercent,
                                                         sdm.SupplementaryDutyPercent
                                                     )
                        ),
               @TotalSupplimentaryDuty
                   = SUM(dbo.FnGetCalculatedOutputSuppDuty(sdm.UnitPrice * sdm.Quantity, sdm.SupplementaryDutyPercent))
        FROM @SlsDtlMain sdm;

        IF @NoOfIteams < 1
        BEGIN
            SET @ErrorMsg = N'Sale is not possible without products!!';
            RAISERROR(   'Sale is not possible without products!!', -- Message text.  
                         20,                                        -- Severity.  
                         -1                                         -- State.  
                     );
        END;

        SELECT @NoOfItemExceedStock = COUNT(1)
        FROM @SlsDtlMain bm
        WHERE ISNULL(bm.Quantity, 0) > ISNULL(bm.CurrentStock, 0);

        IF @NoOfItemExceedStock > 0
        BEGIN
            SET @ErrorMsg = N'Sales quantity exceeds stock!!';
            RAISERROR(   @ErrorMsg, -- Message text.  
                         20,        -- Severity.  
                         -1         -- State.  
                     );
        END;


        --Insert Sales
        INSERT INTO dbo.Sales
        (
            InvoiceNo,
            VatChallanNo,
            OrganizationId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalVAT,
            TotalSupplimentaryDuty,
            IsVatDeductedInSource,
            VDSAmount,
            PaymentReceiveAmount,
            CustomerId,
            ReceiverName,
            ReceiverContactNo,
            ShippingAddress,
            ShippingCountryId,
            SalesTypeId,
            SalesDeliveryTypeId,
            WorkOrderNo,
            SalesDate,
            ExpectedDeliveryDate,
            DeliveryDate,
            DeliveryMethodId,
            ExportTypeId,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            DueDate,
            TermsOfLc,
            CustomerPoNumber,
            IsComplete,
            IsTaxInvoicePrined,
            TaxInvoicePrintedTime,
            MushakGenerationId,
            ReferenceKey,
            CreatedBy,
            CreatedTime,
            OtherBranchOrganizationId
        )
        VALUES
        (   @InvoiceNo,                        -- InvoiceNo - nvarchar(50)
            @VatChallanNo, @OrganizationId,    -- OrganizationId - int
            @NoOfIteams,                       -- NoOfIteams - int
            @TotalPriceWithoutVat,             -- TotalPriceWithoutVat - decimal(18, 2)
            @DiscountOnTotalPrice,             -- DiscountOnTotalPrice - decimal(18, 2)
            @TotalDiscountOnIndividualProduct, -- TotalDiscountOnIndividualProduct - decimal(18, 2)
            @TotalVAT,                         -- TotalVAT - decimal(18, 2)
            @TotalSupplimentaryDuty,           -- TotalSupplimentaryDuty - decimal(18, 2)
            @IsVatDeductedInSource,            -- IsVatDeductedInSource - bit
            @VDSAmount,                        -- VDSAmount - decimal(18, 2)
            @PaymentReceiveAmount,             -- PaymentReceiveAmount - decimal(18, 2)
            @CustomerId,                       -- CustomerId - int
            @ReceiverName,                     -- ReceiverName - nvarchar(200)
            @ReceiverContactNo,                -- ReceiverContactNo - varchar(20)
            @ShippingAddress,                  -- ShippingAddress - nvarchar(200)
            @ShippingCountryId,                -- ShippingCountryId - int
            @SalesTypeId,                      -- SalesTypeId - int
            @SalesDeliveryTypeId,              -- SalesDeliveryTypeId - int
            @WorkOrderNo,                      -- WorkOrderNo - nvarchar(50)
            @SalesDate,                        -- SalesDate - datetime
            @ExpectedDeliveryDate,             -- ExpectedDeleveryDate - datetime
            @DeliveryDate,                     -- DeliveryDate - datetime
            @DeliveryMethodId,                 -- DeliveryMethodId - int
            @ExportTypeId,                     -- ExportTypeId - int
            @LcNo,                             -- LcNo - nvarchar(50)
            @LcDate,                           -- LcDate - datetime
            @BillOfEntry,                      -- BillOfEntry - nvarchar(50)
            @BillOfEntryDate,                  -- BillOfEntryDate - datetime
            @DueDate,                          -- DueDate - datetime
            @TermsOfLc,                        -- TermsOfLc - nvarchar(500)
            @CustomerPoNumber,                 -- CustomerPoNumber - nvarchar(50)
            @IsComplete,                       -- IsComplete - bit
            @IsTaxInvoicePrined,               -- IsTaxInvoicePrined - bit
            @TaxInvoicePrintedTime,            -- TaxInvoicePrintedTime - datetime
            NULL,                              -- MushakGenerationId - int
            @ReferenceKey,                     -- ReferenceKey - nvarchar(100)
            @CreatedBy,                        -- CreatedBy - int
            @CreatedTime,                      -- CreatedTime - datetime
            @OtherBranchOrganizationId         -- OtherBranchOrganizationId - int
            );


        --Get SalesId
        SET @SalesId = SCOPE_IDENTITY();

        --Insert sales details
        INSERT INTO dbo.SalesDetails
        (
            SalesId,
            ProductId,
            ProductVATTypeId,
            ProductTransactionBookId,
            Quantity,
            UnitPrice,
            DiscountPerItem,
            VATPercent,
            SupplementaryDutyPercent,
            MeasurementUnitId,
            ReferenceKey,
            CreatedBy,
            CreatedTime
        )
        SELECT @SalesId,                    -- SalesId - int
               sd.ProductId,                -- ProductId - int
               sd.ProductVATTypeId,         -- ProductVATTypeId - int
               sd.ProductTransactionBookId, -- ProductTransactionBookId - bigint
               sd.Quantity,                 -- Quantity - decimal(18, 2)
               sd.UnitPrice,                -- UnitPrice - decimal(18, 2)
               sd.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
               sd.VATPercent,               -- VATPercent - decimal(18, 2)
               sd.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
               sd.MeasurementUnitId,        -- MeasurementUnitId - int
               sd.ReferenceKey,             -- ReferenceKey - nvarchar(100)
               @CreatedBy,                  -- CreatedBy - int
               @CreatedTime                 -- CreatedTime
        FROM @SlsDtlMain sd;
        --/Insert sales details

        --Insert sales details in ProductTransactionBook
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
        SELECT @OrganizationId,                   -- OrganizationId - int
               slsDtl.ProductId,                  -- ProductId - int
               ISNULL(transBook.EndUnitPrice, 0), -- InitUnitPrice - decimal(19, 4)
               ISNULL(transBook.EndQty, 0),       -- InitQty - decimal(19, 4)
               NULL,                              -- PurchaseDetailId - int
               NULL,                              -- DebitNoteDetailId - int
               NULL,                              -- UsedInProductionId - bigint
               NULL,                              -- ProductionReceiveId - bigint
               slsDtl.SalesDetailId,              -- SalesDetailId - int
               NULL,                              -- CreditNoteDetailId - int
               NULL,                              -- DamageId - int
               @CreatedTime                       -- TransactionTime - datetime
        FROM dbo.SalesDetails slsDtl
            INNER JOIN
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
                            INNER JOIN dbo.SalesDetails slsDtlToCompare
                                ON ptb.ProductId = slsDtlToCompare.ProductId
                        WHERE ptb.OrganizationId = @OrganizationId
                              AND slsDtlToCompare.SalesId = @SalesId
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
            ) transBook
                ON transBook.ProductId = slsDtl.ProductId
        WHERE slsDtl.SalesId = @SalesId;
        --Insert sales details in ProductTransactionBook





        --Insert Payment

        INSERT INTO dbo.SalesPaymentReceive
        (
            SalesId,
            ReceivedPaymentMethodId,
            ReceiveAmount,
            ReceiveDate,
            CreatedBy,
            CreatedTime
        )
        SELECT @SalesId,                   -- SalesId - int
               pr.ReceivedPaymentMethodId, -- ReceivedPaymentMethodId - int
               pr.ReceiveAmount,           -- ReceiveAmount - decimal(18, 2)
               pr.ReceiveDate,             -- ReceiveDate - datetime
               @CreatedBy,                 -- CreatedBy - int
               @CreatedTime                -- CreatedTime - datetime
        FROM @PaymentReceive pr;
        --Insert Payment

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
                   24,
                   @SalesId,
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
        (   24,             -- ObjectTypeId - int
            @SalesId,       -- PrimaryKey - int
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
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;

END;
