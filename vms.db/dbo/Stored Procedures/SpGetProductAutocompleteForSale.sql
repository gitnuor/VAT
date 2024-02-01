-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForSale 5, 'f'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForSale]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           ISNULL(prodPrice.SalesUnitPrice, 0) AS SalesUnitPrice,
           ISNULL(pv.ProductVATTypeId, 3) AS ProductVATTypeId,
           ISNULL(pvt.DefaultVatPercent, 15) AS DefaultVatPercent,
           ISNULL(sd.SdPercent, 0) AS SupplimentaryDutyPercent,
           ISNULL(transBook.EndQty, 0) AS MaxSaleQty,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.PriceSetup prodPrice
            ON prodPrice.ProductId = prod.ProductId
               AND prodPrice.IsActive = 1
        LEFT JOIN dbo.ProductVATs pv
            ON pv.ProductId = prod.ProductId
               AND pv.IsActive = 1
        LEFT JOIN dbo.ProductVATTypes pvt
            ON pvt.ProductVATTypeId = pv.ProductVATTypeId
               AND pvt.IsActive = 1
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.SupplimentaryDuty sd
            ON sd.ProductId = prod.ProductId
               AND sd.IsActive = 1
        LEFT JOIN
        (
            SELECT ptb.ProductTransactionBookId,
                   ptb.ProductId,
                   ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0)
                   - ISNULL(bom.UsedQuantity, 0) + ISNULL(pr.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0)
                   + ISNULL(cnd.ReturnQuantity, 0) - ISNULL(dmg.DamageQty, 0) AS EndQty
            FROM dbo.ProductTransactionBook ptb
                LEFT JOIN dbo.PurchaseDetails purchDtl
                    ON purchDtl.PurchaseDetailId = ptb.PurchaseDetailId
                LEFT JOIN dbo.DebitNoteDetail dnd
                    ON dnd.DebitNoteDetailId = ptb.DebitNoteDetailId
                LEFT JOIN dbo.BillOfMaterial bom
                    ON bom.BillOfMaterialId = ptb.UsedInProductionId
                LEFT JOIN dbo.ProductionReceive pr
                    ON pr.ProductionReceiveId = ptb.ProductionReceiveId
                LEFT JOIN dbo.SalesDetails slsDtl
                    ON slsDtl.SalesDetailId = ptb.SalesDetailId
                LEFT JOIN dbo.CreditNoteDetail cnd
                    ON cnd.CreditNoteDetailId = ptb.CreditNoteDetailId
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
        ) transBook
            ON transBook.ProductId = prod.ProductId
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.IsSellable = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + N'%';
END;
