
CREATE PROCEDURE [dbo].[SpDamageInsert]
    @OrganizationId INT,
    @VoucherNo NVARCHAR(50),
    @ProductId INT,
    @DamageQty DECIMAL(18, 2),
    @DamageTypeId INT,
    @Description NVARCHAR(1000),
    @CreatedBy INT,
    @CreatedTime DATETIME
AS
BEGIN
    DECLARE @ErrorMsg NVARCHAR(100) = N'';
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @DamageId INT,
                @MaxId INT,
                @ProductTransactionBookId BIGINT,
                @CurrentProductStock DECIMAL(18, 2),
                @EndUnitPrice DECIMAL(18, 2);

        SELECT TOP (1)
               @ProductTransactionBookId = ptb.ProductTransactionBookId,
               @CurrentProductStock
                   = (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                      - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                      + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                     ),
               @EndUnitPrice
                   = CASE
                         WHEN (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                               - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                               + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0)
                              ) = 0 THEN
                             0
                         ELSE
               (ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
                - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0) - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
                + ISNULL(pr.ReceiveQuantity * ps.SalesUnitPrice, 0) - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0)
                + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0) - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0)
               )
               / (ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0) - ISNULL(bom.UsedQuantity, 0)
                  + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0)
                  - ISNULL(dmg.DamageQty, 0)
                 )
                     END
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
        WHERE ptb.ProductId = @ProductId
        ORDER BY ptb.ProductTransactionBookId DESC;

        IF ISNULL(@DamageQty, 0) > ISNULL(@CurrentProductStock, 0)
        BEGIN
            SET @ErrorMsg = N'Quantity exceeds stock!!';
            RAISERROR(   @ErrorMsg, -- Message text.  
                         20,        -- Severity.  
                         -1         -- State.  
                     );
        END;

        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        IF @VoucherNo IS NULL
        BEGIN
            --N'DMGVCR' + CAST(MAX(dmg.DamageId) AS NVARCHAR(25))
            SELECT @MaxId = ISNULL(MAX(dmg.DamageId), 0)
            FROM dbo.Damage dmg;
            SET @VoucherNo = N'DMGVCR' + CAST((@MaxId + 1) AS NVARCHAR(25));
        END;

        INSERT INTO dbo.Damage
        (
            OrganizationId,
            VoucherNo,
            ProductId,
            ProductTransactionBookId,
            DamageQty,
            DamageTypeId,
            Description,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @OrganizationId,           -- OrganizationId - int
            @VoucherNo,                -- VoucherNo - nvarchar(50)
            @ProductId,                -- ProductId - int
            @ProductTransactionBookId, -- ProductTransactionBookId - bigint
            @DamageQty,                -- DamageQty - decimal(18, 2)
            @DamageTypeId,             -- DamageTypeId - int
            @Description,              -- Description - nvarchar(1000)
            1,                         -- IsActive - bit
            @CreatedBy,                -- CreatedBy - int
            @CreatedTime               -- CreatedTime - datetime
            );

        SET @DamageId = SCOPE_IDENTITY();

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
        (   @OrganizationId,      -- OrganizationId - int
            @ProductId,           -- ProductId - int
            @EndUnitPrice,        -- InitUnitPrice - decimal(19, 4)
            @CurrentProductStock, -- InitQty - decimal(19, 4)
            NULL,                 -- PurchaseDetailId - int
            NULL,                 -- DebitNoteDetailId - int
            NULL,                 -- UsedInProductionId - bigint
            NULL,                 -- ProductionReceiveId - bigint
            NULL,                 -- SalesDetailId - int
            NULL,                 -- CreditNoteDetailId - int
            @DamageId,            -- DamageId - int
            @CreatedTime          -- TransactionTime - datetime
            );

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
        (   40,             -- ObjectTypeId - int
            @DamageId,      -- PrimaryKey - int
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
