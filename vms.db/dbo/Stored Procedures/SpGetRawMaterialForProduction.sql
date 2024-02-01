-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpGetRawMaterialForProduction]
    -- Add the parameters for the stored procedure here
    @OrganizationId INT,
    @ProductId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT prcSetupProdCost.RawMaterialId AS Id,
           rawMaterial.Name AS ProdName,
           CAST(ISNULL(ptbLastStock.ProductCurrentStock, 0) AS DECIMAL(18, 4)) AS CurrentStock,
		   prcSetupProdCost.RequiredQty AS RequiredQtyPerUnitProduction,
           prcSetupProdCost.MeasurementUnitId,
           mu.Name AS UnitName
    FROM dbo.PriceSetup prcSetup
        INNER JOIN dbo.PriceSetupProductCost prcSetupProdCost
            ON prcSetupProdCost.PriceSetupId = prcSetup.PriceSetupId
        INNER JOIN dbo.Products rawMaterial
            ON rawMaterial.ProductId = prcSetupProdCost.RawMaterialId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prcSetupProdCost.MeasurementUnitId
        LEFT JOIN
        (
            SELECT ptb.ProductId,
                   ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                   - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                   + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0) AS ProductCurrentStock
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
                        INNER JOIN dbo.PriceSetupProductCost pspc
                            ON pspc.RawMaterialId = ptb.ProductId
                        INNER JOIN dbo.PriceSetup ps
                            ON ps.PriceSetupId = pspc.PriceSetupId
                    WHERE ptb.OrganizationId = @OrganizationId
                          AND ps.ProductId = @ProductId
                          AND ps.IsActive = 1
                    GROUP BY ptb.ProductId
                ) lastPtb
                    ON lastPtb.ProductTransactionBookId = ptb.ProductTransactionBookId
        ) ptbLastStock
            ON ptbLastStock.ProductId = prcSetupProdCost.RawMaterialId
    WHERE prcSetup.OrganizationId = @OrganizationId
          AND prcSetup.ProductId = @ProductId
          AND prcSetup.IsActive = 1;
END;
