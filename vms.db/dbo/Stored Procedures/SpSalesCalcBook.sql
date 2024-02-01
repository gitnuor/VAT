-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 14, 2019
-- Description:	Returns Purchase Calculation Book for Mushak 6.1
-- SpSalesCalcBook 5, NULL, NULL, 0, 0
-- =============================================
CREATE PROCEDURE [dbo].[SpSalesCalcBook]
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
    @CustomerId INT,
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @ToDate IS NOT NULL
    BEGIN
        SET @ToDate = DATEADD(DAY, 1, @ToDate);
    END;

    --===========================================================
    DECLARE @salesTypeLocal INT = 1,
            @salesTypeExport INT = 2,
            @salesTypeTrading INT = 4,
            @exportTypeDirect INT = 1,
            @exportTypeInDirect INT = 2;
    --===========================================================

    --===========================================================
    DECLARE @zeroVatProdVatTypeId INT = 1,
            @vatExemptedProdVatTypeId INT = 2,
            @standardVatProdVatTypeId INT = 3,
            @mrpProdVatTypeId INT = 4,
            @notAdmissibleForCreditProdVatTypeId INT = 5,
            @fixedVatProdVatTypeId INT = 6,
            @otherThanStandardVatProdVatTypeId INT = 7,
            @retailOrWholesaleOrTradeProductVatTypeId INT = 8;
    --===========================================================


    SELECT ROW_NUMBER() OVER (ORDER BY ptb.ProductTransactionBookId) AS SlNo,
           ptb.TransactionTime AS OperationTime,
           CAST(CASE
                    WHEN ptb.SalesDetailId IS NOT NULL THEN
                        CASE
                            WHEN sls.SalesTypeId = @salesTypeLocal THEN
                                CASE
                                    WHEN slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                                        N'সরবরাহ (নোট (৩))'
                                    WHEN slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                                        N'সরবরাহ (নোট (৪))'
                                    WHEN slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                                        N'সরবরাহ (নোট (৫))'
                                    WHEN slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                                        N'সরবরাহ (নোট (৬))'
                                    WHEN slsDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                                        N'সরবরাহ (নোট (৭))'
                                    WHEN slsDtl.ProductVATTypeId = @retailOrWholesaleOrTradeProductVatTypeId THEN
                                        N'সরবরাহ (নোট (৮))'
                                    ELSE
                                        N'সরবরাহ'
                                END
                            WHEN sls.SalesTypeId = @salesTypeExport THEN
                                CASE
                                    WHEN sls.ExportTypeId = @exportTypeDirect THEN
                                        N'সরবরাহ (নোট (১))'
                                    ELSE
                                        N'সরবরাহ (নোট (২))'
                                END
                            ELSE
                                N'শাখা স্থানান্তর'
                        END
                    WHEN ptb.CreditNoteDetailId IS NOT NULL THEN
                        N'ক্রেডিট  নোট (নোট (৩১))'
                    WHEN ptb.DamageId IS NOT NULL THEN
                        N'ক্ষতিগ্রস্ত'
                    WHEN ptb.ProductionReceiveId IS NOT NULL THEN
                        N'উৎপাদন'
                    WHEN ptb.PurchaseDetailId IS NOT NULL THEN
                        N'ক্রয়'
                    WHEN ptb.DebitNoteDetailId IS NOT NULL THEN
                        N'ডেবিট নোট'
                    WHEN ptb.UsedInProductionId IS NOT NULL THEN
                        N'উৎপাদনে ব্যবহৃত'
                    ELSE
                        N'ব্যবহার'
                END AS NVARCHAR(100)) AS OperationType,
           ptb.OrganizationId,
           org.Name AS OrganizationName,
           org.Address AS OrganizationAddress,
           org.BIN AS OrganizationBin,
           sls.SalesId,
           slsDtl.SalesDetailId,
           sls.SalesDate,
           ptb.InitQty AS InitialQty,
           ptb.InitUnitPrice * ptb.InitQty AS InitPriceWithoutVat,
           prdcnRcv.ReceiveQuantity AS ProductionQty,
           prdcnRcv.ReceiveQuantity * ISNULL(prcStup.SalesUnitPrice, 0) AS PriceOfProdFromProduction,
           ptb.InitQty + ISNULL(prdcnRcv.ReceiveQuantity, 0) AS TotalProductionQty,
           ptb.InitUnitPrice * ptb.InitQty + ISNULL(prdcnRcv.ReceiveQuantity * ISNULL(prcStup.SalesUnitPrice, 0), 0) AS TotalProductionPrice,
           ISNULL(cust.Name, custCnd.Name) AS CustomerName,
           ISNULL(cust.Address, custCnd.Address) AS CustomerAddress,
           COALESCE(cust.BIN, cust.NIDNo, custCnd.BIN, custCnd.NIDNo) AS CustomerBinOrNid,
           ISNULL(sls.VatChallanNo, slsCnd.VatChallanNo) AS VatChallanNo,
           ISNULL(sls.TaxInvoicePrintedTime, slsCnd.TaxInvoicePrintedTime) AS TaxInvoicePrintedTime,
           ptb.ProductId,
           prod.Name AS ProductName,
           CASE
               WHEN ptb.SalesDetailId IS NOT NULL THEN
                   purchDtl.Quantity
               WHEN ptb.CreditNoteDetailId IS NOT NULL THEN
                   cnd.ReturnQuantity * (-1)
               ELSE
                   CAST(NULL AS DECIMAL(18, 2))
           END AS SalesQty,
           prcStup.SalesUnitPrice AS UnitPrice,
           ISNULL(slsDtl.UnitPrice, slsDtlCnd.UnitPrice) AS SalesUnitPrice,
           CASE
               WHEN ptb.SalesDetailId IS NOT NULL THEN
                   slsDtl.Quantity * slsDtl.UnitPrice
               WHEN ptb.CreditNoteDetailId IS NOT NULL THEN
                   cnd.ReturnQuantity * (-1) * slsDtlCnd.UnitPrice
               ELSE
                   CAST(NULL AS DECIMAL(18, 2))
           END AS TaxablePrice,
           CASE
               WHEN ptb.PurchaseDetailId IS NOT NULL THEN
                   [dbo].[FnGetCalculatedOutputSuppDuty](
                                                            slsDtl.Quantity * slsDtl.UnitPrice,
                                                            ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                                        )
               WHEN ptb.DebitNoteDetailId IS NOT NULL THEN
           ([dbo].[FnGetCalculatedOutputSuppDuty](
                                                     cnd.ReturnQuantity * slsDtlCnd.UnitPrice,
                                                     ISNULL(slsDtlCnd.SupplementaryDutyPercent, 0)
                                                 )
           ) * (-1)
               ELSE
                   CAST(NULL AS DECIMAL(18, 2))
           END AS SupplementaryDuty,
           CASE
               WHEN ptb.SalesDetailId IS NOT NULL THEN
                   CASE
                       WHEN slsDtl.ProductVATTypeId = 6 THEN
                           ISNULL(slsDtl.VATPercent, 0) * ISNULL(slsDtl.Quantity, 0)
                       ELSE
                           [dbo].[FnGetCalculatedOutputVat](
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               ISNULL(slsDtl.VATPercent, 0),
                                                               ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                                           )
                   END
               WHEN ptb.CreditNoteDetailId IS NOT NULL THEN
                   CASE
                       WHEN slsDtlCnd.ProductVATTypeId = 6 THEN
                           ISNULL(slsDtlCnd.VATPercent, 0) * ISNULL(cnd.ReturnQuantity, 0)
                       ELSE
                           [dbo].[FnGetCalculatedOutputVat](
                                                               slsDtlCnd.UnitPrice * cnd.ReturnQuantity,
                                                               ISNULL(slsDtlCnd.VATPercent, 0),
                                                               ISNULL(slsDtlCnd.SupplementaryDutyPercent, 0)
                                                           )
                   END
               ELSE
                   CAST(NULL AS DECIMAL(18, 2))
           END AS ProdVatAmount,
           ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0) - ISNULL(bom.UsedQuantity, 0)
           + ISNULL(prdcnRcv.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0)
           - ISNULL(dmg.DamageQty, 0) AS ClosingProdQty,
           ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
           - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0) - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
           + ISNULL(prdcnRcv.ReceiveQuantity * prcStup.SalesUnitPrice, 0)
           - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0) + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
           - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0) AS ClosingProdPrice,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.ProductTransactionBook ptb
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = ptb.OrganizationId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = ptb.ProductId
        LEFT JOIN dbo.PriceSetup prcStup
            ON prcStup.ProductId = prod.ProductId
               AND prcStup.IsActive = 1
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.ProductionReceive prdcnRcv
            ON prdcnRcv.ProductionReceiveId = ptb.ProductionReceiveId
        LEFT JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesDetailId = ptb.SalesDetailId
        LEFT JOIN dbo.Sales sls
            ON sls.SalesId = slsDtl.SalesId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
        LEFT JOIN dbo.CreditNoteDetail cnd
            ON cnd.CreditNoteDetailId = ptb.CreditNoteDetailId
        LEFT JOIN dbo.SalesDetails slsDtlCnd
            ON cnd.SalesDetailId = slsDtlCnd.SalesDetailId
        LEFT JOIN dbo.Sales slsCnd
            ON slsCnd.SalesId = slsDtlCnd.SalesId
        LEFT JOIN dbo.Customer custCnd
            ON custCnd.CustomerId = slsCnd.CustomerId
        LEFT JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseDetailId = ptb.PurchaseDetailId
        LEFT JOIN dbo.Purchase purch
            ON purch.PurchaseId = purchDtl.PurchaseId
        LEFT JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
        LEFT JOIN dbo.DebitNoteDetail dnd
            ON dnd.DebitNoteDetailId = ptb.DebitNoteDetailId
        LEFT JOIN dbo.DebitNote dn
            ON dn.DebitNoteId = dnd.DebitNoteId
        LEFT JOIN dbo.PurchaseDetails purchDtlDnd
            ON purchDtlDnd.PurchaseDetailId = dnd.PurchaseDetailId
        LEFT JOIN dbo.BillOfMaterial bom
            ON bom.BillOfMaterialId = ptb.UsedInProductionId
        LEFT JOIN dbo.Damage dmg
            ON dmg.DamageId = ptb.DamageId
    WHERE EXISTS
    (
        SELECT fnOrg.OrganizationId
        FROM [dbo].[FnGetListOfOrganizationIdWithChild](@OrganizationId) fnOrg
        WHERE fnOrg.OrganizationId = ptb.OrganizationId
    )
          AND
          (
              @FromDate IS NULL
              OR ptb.TransactionTime >= @FromDate
          )
          AND
          (
              @ToDate IS NULL
              OR ptb.TransactionTime < @ToDate
          )
          AND
          (
              @CustomerId = 0
              OR @CustomerId IS NULL
              OR purch.VendorId = @CustomerId
          )
          AND
          (
              @ProductId = 0
              OR @ProductId IS NULL
              OR ptb.ProductId = @ProductId
          );
END;
