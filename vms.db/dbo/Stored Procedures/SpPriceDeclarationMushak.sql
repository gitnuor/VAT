-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpPriceDeclarationMushak] 2
-- =============================================
CREATE PROCEDURE [dbo].[SpPriceDeclarationMushak] @PricedecID INT
AS
BEGIN
    SET NOCOUNT ON;


    SELECT OrganizationName = org.Name,
           OrganizationAddress = org.Address,
           OrganizationBin = org.BIN,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonDesignation,
		   SubmissionNo = CAST(prcSt.SubmissionSl AS NVARCHAR(50)),
           DateOfSubmission = prcSt.MushakSubmissionDate,
           FirstSupplyDate = prcSt.EffectiveFrom,
           prod.FinishedProductId,
           prod.FinishedProductName,
           prod.FinishedProductCode,
           prod.FinishedProductHsCode,
           prod.FinishedProductModelNo,
           prod.FinishedProductMeasurementUnit,
           cst.RmspcPriceSetupId,
           cst.RawMaterialId,
           cst.RawmaterialName,
           cst.RawmaterialRequiredQtyWithWastage,
           cst.RawmaterialPurchasePrice,
           cst.RawmaterialWastageQty,
           cst.RawmaterialWastagePercentage,
           cst.OhcpspcPriceSetupId,
           cst.OverHeadCostId,
           cst.OverHeadCostName,
           cst.OverHeadCostAmount
    FROM
    (
        SELECT ROW_NUMBER() OVER (ORDER BY prod.ProductId) AS CostId,
               prcSt.PriceSetupId,
               FinishedProductId = prod.ProductId,
               FinishedProductName = prod.Name,
               FinishedProductCode = prod.Code,
               FinishedProductHsCode = prod.HSCode,
               FinishedProductModelNo = prod.ModelNo,
               FinishedProductMeasurementUnit = mu.Name
        FROM dbo.Products prod
            INNER JOIN dbo.MeasurementUnits mu
                ON mu.MeasurementUnitId = prod.MeasurementUnitId
            INNER JOIN dbo.PriceSetup prcSt
                ON prcSt.ProductId = prod.ProductId
        WHERE prcSt.PriceSetupId = @PricedecID
    ) prod
        FULL OUTER JOIN
        (
            SELECT ROW_NUMBER() OVER (ORDER BY cost.CostTypeId, cost.CostId) AS CostId,
                   cost.RmspcPriceSetupId,
                   cost.RawMaterialId,
                   cost.RawmaterialName,
                   cost.RawmaterialRequiredQtyWithWastage,
                   cost.RawmaterialPurchasePrice,
                   cost.RawmaterialWastageQty,
                   cost.RawmaterialWastagePercentage,
                   cost.OhcpspcPriceSetupId,
                   cost.OverHeadCostId,
                   cost.OverHeadCostName,
                   cost.OverHeadCostAmount
            FROM
            (
                SELECT ROW_NUMBER() OVER (ORDER BY rm.PriceSetupProductCostId) AS CostId,
                       1 AS CostTypeId,
                       rm.RmspcPriceSetupId,
                       rm.RawMaterialId,
                       rm.RawmaterialName,
                       rm.RawmaterialRequiredQtyWithWastage,
                       rm.RawmaterialPurchasePrice,
                       rm.RawmaterialWastageQty,
                       rm.RawmaterialWastagePercentage,
                       CAST(NULL AS INT) AS OhcpspcPriceSetupId,
                       CAST(NULL AS INT) AS OverHeadCostId,
                       CAST(NULL AS NVARCHAR(100)) AS OverHeadCostName,
                       CAST(NULL AS DECIMAL(18, 4)) AS OverHeadCostAmount
                FROM
                (
                    SELECT rmpspc.PriceSetupProductCostId,
                           RmspcPriceSetupId = rmpspc.PriceSetupId,
                           rmpspc.RawMaterialId,
                           RawmaterialName = prod.Name,
                           RawmaterialRequiredQtyWithWastage = rmpspc.RequiredQty,
                           RawmaterialPurchasePrice = rmpspc.Cost,
                           RawmaterialWastageQty = rmpspc.RequiredQty * rmpspc.WastagePercentage / 100,
                           RawmaterialWastagePercentage = rmpspc.WastagePercentage
                    FROM dbo.PriceSetupProductCost rmpspc
                        INNER JOIN dbo.Products prod
                            ON prod.ProductId = rmpspc.RawMaterialId
                    WHERE rmpspc.IsRawMaterial = 1
                          AND rmpspc.PriceSetupId = @PricedecID
                    UNION ALL
                    SELECT rmocpspc.PriceSetupProductCostId,
                           RmspcPriceSetupId = rmocpspc.PriceSetupId,
                           rmocpspc.RawMaterialId,
                           RawmaterialName = prod.Name,
                           RawmaterialRequiredQtyWithWastage = rmocpspc.RequiredQty,
                           RawmaterialPurchasePrice = rmocpspc.Cost,
                           RawmaterialWastageQty = rmocpspc.RequiredQty * rmocpspc.WastagePercentage / 100,
                           RawmaterialWastagePercentage = rmocpspc.WastagePercentage
                    FROM dbo.PriceSetupProductCost rmocpspc
                        INNER JOIN dbo.OverHeadCost prod
                            ON prod.OverHeadCostId = rmocpspc.OverHeadCostId
                    WHERE rmocpspc.IsRawMaterial = 1
                          AND rmocpspc.PriceSetupId = @PricedecID
                ) rm
                UNION ALL
                SELECT ROW_NUMBER() OVER (ORDER BY oh.PriceSetupProductCostId) AS CostId,
                       2 AS CostTypeId,
                       CAST(NULL AS INT) AS RmspcPriceSetupId,
                       CAST(NULL AS INT) AS RawMaterialId,
                       CAST(NULL AS NVARCHAR(100)) AS RawmaterialName,
                       CAST(NULL AS DECIMAL(18, 4)) AS RawmaterialRequiredQtyWithWastage,
                       CAST(NULL AS DECIMAL(18, 4)) AS RawmaterialPurchasePrice,
                       CAST(NULL AS DECIMAL(18, 4)) AS RawmaterialWastageQty,
                       CAST(NULL AS DECIMAL(18, 4)) AS RawmaterialWastagePercentage,
                       oh.OhcpspcPriceSetupId,
                       oh.OverHeadCostId,
                       oh.OverHeadCostName,
                       oh.OverHeadCostAmount
                FROM
                (
                    SELECT ohcpspc.PriceSetupProductCostId,
                           OhcpspcPriceSetupId = ohcpspc.PriceSetupId,
                           ohcpspc.OverHeadCostId,
                           OverHeadCostName = ohc.Name,
                           OverHeadCostAmount = ohcpspc.Cost
                    FROM dbo.PriceSetupProductCost ohcpspc
                        INNER JOIN dbo.OverHeadCost ohc
                            ON ohc.OverHeadCostId = ohcpspc.OverHeadCostId
                    WHERE ohcpspc.PriceSetupId = @PricedecID
                          AND ohcpspc.IsRawMaterial = 0
                    UNION ALL
                    SELECT ohcrmpspc.PriceSetupProductCostId,
                           OhcpspcPriceSetupId = ohcrmpspc.PriceSetupId,
                           ohcrmpspc.OverHeadCostId,
                           OverHeadCostName = ohc.Name,
                           OverHeadCostAmount = ohcrmpspc.Cost
                    FROM dbo.PriceSetupProductCost ohcrmpspc
                        INNER JOIN dbo.Products ohc
                            ON ohc.ProductId = ohcrmpspc.RawMaterialId
                    WHERE ohcrmpspc.PriceSetupId = @PricedecID
                          AND ohcrmpspc.IsRawMaterial = 0
                ) oh
            ) cost
        ) cst
            ON cst.CostId = prod.CostId
        INNER JOIN dbo.PriceSetup prcSt
            ON prcSt.PriceSetupId = COALESCE(cst.RmspcPriceSetupId, cst.OhcpspcPriceSetupId, prod.PriceSetupId)
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = prcSt.OrganizationId;
END;

