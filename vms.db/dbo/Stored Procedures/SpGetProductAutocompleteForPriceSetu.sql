-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForPriceSetu 5, 'f'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForPriceSetu]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    DECLARE @ProductPrice TABLE
    (
        ProductId INT NOT NULL,
        UnitPrice DECIMAL(18, 2) NOT NULL
    );
    INSERT INTO @ProductPrice
    (
        ProductId,
        UnitPrice
    )
    SELECT prodPrice.ProductId,
           prodPrice.UnitPrice
    FROM
    (
        SELECT ROW_NUMBER() OVER (PARTITION BY pd.ProductId
                                  ORDER BY pd.ProductId ASC,
                                           pd.PurchaseDetailId DESC
                                 ) AS ProdSl,
               pd.ProductId,
               pd.UnitPrice
        FROM dbo.PurchaseDetails pd
    ) prodPrice
    WHERE prodPrice.ProdSl = 1;

    SELECT rm.ProductId,
           rm.ProductName,
           rm.ModelNo,
           rm.Code,
           rm.MaxUseQty,
           rm.UnitPrice,
           rm.MeasurementUnitId,
           rm.MeasurementUnitName,
           rm.IsApplicableAsRawMaterial,
           rm.ItemType
    FROM
    (
        SELECT prod.ProductId,
               prod.Name AS ProductName,
               prod.ModelNo,
               prod.Code,
               ISNULL(ptbLastPrice.EndQty, 0) AS MaxUseQty,
               pp.UnitPrice,
               prod.MeasurementUnitId,
               mu.Name AS MeasurementUnitName,
               CAST(1 AS BIT) AS IsApplicableAsRawMaterial,
               'RM' AS ItemType
        FROM dbo.Products prod
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
                ON ptbLastPrice.ProductId = prod.ProductId
            LEFT JOIN dbo.MeasurementUnits mu
                ON mu.MeasurementUnitId = prod.MeasurementUnitId
            INNER JOIN @ProductPrice pp
                ON pp.ProductId = prod.ProductId
        WHERE prod.OrganizationId = @OrganizationId
              AND prod.IsRawMaterial = 1
              AND prod.Name LIKE N'%' + @ProductSearchTerm + '%'
        UNION ALL
        SELECT ohc.OverHeadCostId AS ProductId,
               ohc.Name AS ProductName,
               '' AS ModelNo,
               '' AS Code,
               0 AS MaxUseQty,
               0 AS UnitPrice,
               0 AS MeasurementUnitId,
               '' AS MeasurementUnitName,
               ohc.IsApplicableAsRawMaterial AS IsApplicableAsRawMaterial,
               'OH' AS ItemType
        FROM dbo.OverHeadCost ohc
        WHERE ohc.OrganizationId = @OrganizationId
              AND ohc.IsActive = 1
              AND ohc.Name LIKE N'%' + @ProductSearchTerm + '%'
    ) rm;
END;
