CREATE PROCEDURE [dbo].[spInsertDebitNote]
    -- Add the parameters for the stored procedure here
    @PurchaseId INT,
    @VoucherNo NVARCHAR(50),
    @ReasonOfReturn NVARCHAR(50),
    @ReturnDate DATETIME,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @DebitNoteDetails NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(100) = N'',
                @OrganizationId INT,
                @MaxId INT;
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        IF @VoucherNo IS NULL
        BEGIN
            --N'DMGVCR' + CAST(MAX(dmg.DamageId) AS NVARCHAR(25))
            SELECT @MaxId = ISNULL(MAX(dbtnt.DebitNoteId), 0)
            FROM dbo.DebitNote dbtnt;
            SET @VoucherNo = N'DBTNT' + CAST((@MaxId + 1) AS NVARCHAR(25));
        END;
        SELECT @OrganizationId = purch.OrganizationId
        FROM dbo.Purchase purch
        WHERE purch.PurchaseId = @PurchaseId;
        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        DECLARE @DebitNoteDetail TABLE
        (
            [PurchaseDetailId] [INT] NOT NULL,
            [ReturnQuantity] [DECIMAL] NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );

        DECLARE @DebitNoteId INT;
        INSERT INTO @DebitNoteDetail
        (
            PurchaseDetailId,
            ReturnQuantity,
            MeasurementUnitId
        )
        SELECT jd.[PurchaseDetailId],
               jd.[ReturnQuantity],
               jd.[MeasurementUnitId]
        FROM
            OPENJSON(@DebitNoteDetails)
            WITH
            (
                [PurchaseDetailId] [INT],
                [ReturnQuantity] [DECIMAL],
                [MeasurementUnitId] [INT]
            ) jd;

        -- Insert statements for procedure here
        INSERT INTO dbo.DebitNote
        (
            PurchaseId,
            VoucherNo,
            ReasonOfReturn,
            ReturnDate,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (@PurchaseId, @VoucherNo, @ReasonOfReturn, @ReturnDate, @CreatedBy, @CreatedTime);

        SET @DebitNoteId = SCOPE_IDENTITY();
        --Insert CreditNote Details
        INSERT INTO dbo.DebitNoteDetail
        (
            DebitNoteId,
            PurchaseDetailId,
            ReturnQuantity,
            MeasurementUnitId,
            CreatedBy,
            CreatedTime
        )
        SELECT @DebitNoteId,
               dn.PurchaseDetailId,
               dn.ReturnQuantity,
               dn.MeasurementUnitId,
               @CreatedBy,
               @CreatedTime
        FROM @DebitNoteDetail dn;


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
        SELECT @OrganizationId,        -- OrganizationId - int
               purchDtl.ProductId,     -- ProductId - int
               transBook.EndUnitPrice, -- InitUnitPrice - decimal(19, 4)
               transBook.EndQty,       -- InitQty - decimal(19, 4)
               NULL,                   -- PurchaseDetailId - int
               dnd.DebitNoteDetailId,  -- DebitNoteDetailId - int
               NULL,                   -- UsedInProductionId - bigint
               NULL,                   -- ProductionReceiveId - bigint
               NULL,                   -- SalesDetailId - int
               NULL,                   -- CreditNoteDetailId - int
               NULL,                   -- DamageId - int
               dnd.CreatedTime
        FROM dbo.DebitNoteDetail dnd
            INNER JOIN dbo.PurchaseDetails purchDtl
                ON purchDtl.PurchaseDetailId = dnd.PurchaseDetailId
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
                            INNER JOIN dbo.PurchaseDetails bomToPurchDtl
                                ON ptb.ProductId = bomToPurchDtl.ProductId
                            INNER JOIN dbo.DebitNoteDetail dndToCompare
                                ON dndToCompare.PurchaseDetailId = bomToPurchDtl.PurchaseDetailId
                        WHERE dndToCompare.DebitNoteId = @DebitNoteId
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
                ON transBook.ProductId = purchDtl.ProductId
        WHERE dnd.DebitNoteId = @DebitNoteId;


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
        (   39,             -- ObjectTypeId - int
            @DebitNoteId,   -- PrimaryKey - int
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
