-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForSale 5, 'd'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForBom]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           ISNULL(transBook.EndQty, 0) AS MaxUseQty,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
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
          AND prod.IsRawMaterial = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%';
END;
