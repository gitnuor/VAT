-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 29, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertProductionReceive]
    @BatchNo NVARCHAR(50),
    @OrganizationId INT,
    @ProductionId INT,
    @ProductId INT,
    @ReceiveQuantity DECIMAL(18, 2),
    @MeasurementUnitId INT,
    @ReceiveTime DATETIME,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @IsContractual BIT,
    @ContractualProductionId INT,
    @ContractualProductionChallanNo NVARCHAR(50),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    DECLARE @ErrorMsg NVARCHAR(100) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @NoOfRawMaterialExceedStock INT = 0;


        DECLARE @Bom TABLE
        (
            [RawMaterialId] [INT] NOT NULL,
            [ProductTransactionBookId] [INT] NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [UsedQuantity] [DECIMAL](18, 2) NOT NULL,
            [CurrentStock] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );

        INSERT INTO @Bom
        (
            RawMaterialId,
            ProductTransactionBookId,
            UnitPrice,
            UsedQuantity,
            CurrentStock,
            MeasurementUnitId
        )
        SELECT pspc.RawMaterialId,                            -- RawMaterialId - int
               ISNULL(transBook.ProductTransactionBookId, 0), -- ProductTransactionBookId - int
               ISNULL(transBook.EndUnitPrice, 0),             -- UnitPrice - decimal(18, 2)
               pspc.RequiredQty * @ReceiveQuantity,           -- UsedQuantity - decimal(18, 2)
               ISNULL(transBook.EndQty, 0),                   -- CurrentStock - decimal(18, 2)
               pspc.MeasurementUnitId                         -- MeasurementUnitId - int
        FROM dbo.PriceSetupProductCost pspc
            INNER JOIN dbo.PriceSetup ps
                ON ps.PriceSetupId = pspc.PriceSetupId
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
                        SELECT MAX(ptb.ProductTransactionBookId) AS ProductTransactionBookId,
                               ptb.ProductId
                        FROM dbo.ProductTransactionBook ptb
                            INNER JOIN dbo.PriceSetupProductCost pspcToCompare
                                ON ptb.ProductId = pspcToCompare.RawMaterialId
                            INNER JOIN dbo.PriceSetup psToCompare
                                ON psToCompare.PriceSetupId = pspcToCompare.PriceSetupId
                        WHERE ptb.OrganizationId = @OrganizationId
                              AND psToCompare.ProductId = @ProductId
                              AND psToCompare.IsActive = 1
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
                ON transBook.ProductId = pspc.RawMaterialId
        WHERE ps.ProductId = @ProductId
              AND pspc.RawMaterialId IS NOT NULL
              AND ps.IsActive = 1;
        --SELECT * FROM @Bom
        SELECT @NoOfRawMaterialExceedStock = COUNT(1)
        FROM @Bom bm
        WHERE bm.UsedQuantity > bm.CurrentStock;

        IF @NoOfRawMaterialExceedStock > 0
        BEGIN
            SET @ErrorMsg = N'Used quantity exceeds stock!!';
            RAISERROR(   @ErrorMsg, -- Message text.  
                         20,        -- Severity.  
                         -1         -- State.  
                     );
        END;

        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        SET @ReceiveTime = ISNULL(@ReceiveTime, @CreatedTime);

        DECLARE @MaterialCost DECIMAL(18, 2),
                @ProductionReceiveId INT;

        SELECT @MaterialCost = ISNULL(SUM(bm.UnitPrice * bm.UsedQuantity), 0)
        FROM @Bom bm;


        --Insert Production Receive
        INSERT INTO dbo.ProductionReceive
        (
            BatchNo,
            OrganizationId,
            ProductionId,
            ProductId,
            PriceSetupId,
            ReceiveQuantity,
            MeasurementUnitId,
            ReceiveTime,
            MaterialCost,
            IsActive,
            CreatedBy,
            CreatedTime,
            IsContractual,
            ContractualProductionId,
            ContractualProductionChallanNo
        )
        VALUES
        (   @BatchNo,                       -- BatchNo - nvarchar(50)
            @OrganizationId,                -- OrganizationId - int
            @ProductionId,                  -- ProductionId - int
            @ProductId,                     -- ProductId - int
            (
                SELECT MAX(ps.PriceSetupId)
                FROM dbo.PriceSetup ps
                WHERE ps.ProductId = @ProductId
                      AND ps.IsActive = 1
            ),                              -- PriceSetupId - int
            @ReceiveQuantity,               -- ReceiveQuantity - decimal(18, 2)
            @MeasurementUnitId,             -- MeasurementUnitId - int
            @ReceiveTime,                   -- ReceiveTime - datetime
            @MaterialCost,                  -- MaterialCost - decimal(18, 2)
            1,                              -- IsActive - bit
            @CreatedBy,                     -- CreatedBy - int
            @CreatedTime,                   -- CreatedTime - datetime
            @IsContractual,                 -- IsContractual - bit
            @ContractualProductionId,       -- ContractualProductionId - int
            @ContractualProductionChallanNo -- ContractualProductionChallanNo - nvarchar(50)
            );
        --End Insert Production Receive
        -- Set Procuction Receive Id
        SET @ProductionReceiveId = SCOPE_IDENTITY();

        --Insert finisted product in ProductTransactionBook
        DECLARE @FinishedProductInitQty DECIMAL(18, 2) = 0,
                @FinishedProductInitUnitPrice DECIMAL(18, 2) = 0;
        SELECT TOP (1)
               @FinishedProductInitUnitPrice = CASE
                                                   WHEN ComputedColumn.EndQty = 0 THEN
                                                       0
                                                   ELSE
                                                       ComputedColumn.EndPrice / ComputedColumn.EndQty
                                               END,
               @FinishedProductInitQty = ComputedColumn.EndQty
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
            CROSS APPLY
        (
            SELECT (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                    - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                    + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                   ) AS EndQty,
                   (ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
                    - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0)
                    - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
                    + ISNULL(pr.ReceiveQuantity * ps.SalesUnitPrice, 0) - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0)
                    + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
                    - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0)
                   ) AS EndPrice
        ) AS ComputedColumn
        WHERE ptb.OrganizationId = @OrganizationId
              AND ptb.ProductId = @ProductId
        ORDER BY ptb.ProductTransactionBookId DESC;

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
        VALUES
        (   @OrganizationId,                          -- OrganizationId - int
            @ProductId,                               -- ProductId - int
            ISNULL(@FinishedProductInitUnitPrice, 0), -- InitUnitPrice - decimal(19, 4)
            ISNULL(@FinishedProductInitQty, 0),       -- InitQty - decimal(19, 4)
            NULL,                                     -- PurchaseDetailId - int
            NULL,                                     -- DebitNoteDetailId - int
            NULL,                                     -- UsedInProductionId - bigint
            @ProductionReceiveId,                     -- ProductionReceiveId - bigint
            NULL,                                     -- SalesDetailId - int
            NULL,                                     -- CreditNoteDetailId - int
            NULL,                                     -- DamageId - int
            @CreatedTime                              -- TransactionTime - datetime
            );


        --Insert BOM
        INSERT INTO dbo.BillOfMaterial
        (
            ProductionReceiveId,
            RawMaterialId,
            UsedQuantity,
            MeasurementUnitId,
            ProductTransactionBookId,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        SELECT @ProductionReceiveId,        -- ProductionReceiveId - bigint
               bm.RawMaterialId,            -- RawMaterialId - int
               bm.UsedQuantity,             -- UsedQuantity - decimal(18, 2)
               bm.MeasurementUnitId,        -- MeasurementUnitId - int
               bm.ProductTransactionBookId, -- ProductTransactionBookId - bigint
               1,                           -- IsActive - bit
               @CreatedBy,                  -- CreatedBy - int
               @CreatedTime                 -- CreatedTime - datetime 
        FROM @Bom bm;
        --Insert BOM

        --Insert used Raw Material in ProductTransactionBook
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
               bom.RawMaterialId,                 -- ProductId - int
               ISNULL(transBook.EndUnitPrice, 0), -- InitUnitPrice - decimal(19, 4)
               ISNULL(transBook.EndQty, 0),       -- InitQty - decimal(19, 4)
               NULL,                              -- PurchaseDetailId - int
               NULL,                              -- DebitNoteDetailId - int
               bom.BillOfMaterialId,              -- UsedInProductionId - bigint
               NULL,                              -- ProductionReceiveId - bigint
               NULL,                              -- SalesDetailId - int
               NULL,                              -- CreditNoteDetailId - int
               NULL,                              -- DamageId - int
               @CreatedTime                       -- TransactionTime - datetime
        FROM dbo.BillOfMaterial bom
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
                            INNER JOIN dbo.BillOfMaterial bomToCompare
                                ON ptb.ProductId = bomToCompare.RawMaterialId
                        WHERE ptb.OrganizationId = @OrganizationId
                              AND bomToCompare.ProductionReceiveId = @ProductionReceiveId
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
                ON transBook.ProductId = bom.RawMaterialId
        WHERE bom.ProductionReceiveId = @ProductionReceiveId;
        --Insert used Raw Material in ProductTransactionBook

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
                   35,
                   @ProductionReceiveId,
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
        (   33,                   -- ObjectTypeId - int
            @ProductionReceiveId, -- PrimaryKey - int
            1,                    -- AuditOperationId - int
            @CreatedBy,           -- UserId - int
            SYSDATETIME(),        -- Datetime - datetime2(7)
            N'',                  -- Descriptions - nvarchar(4000)
            1,                    -- IsActive - bit
            @CreatedBy,           -- CreatedBy - int
            @CreatedTime,         -- CreatedTime - datetime
            @OrganizationId       -- OrganizationId - int
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
