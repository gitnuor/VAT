-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 14, 2019
-- Description:	Returns Purchase Calculation Book for Mushak 6.1
-- SpPurchaseSaleCalcBook 5, NULL, NULL, 0, 0, 18
-- =============================================
CREATE PROCEDURE [dbo].[SpPurchaseSaleCalcBook]
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
    @VendorId INT,
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
    DECLARE @purchaseTypeLocal INT = 1,
            @purchaseTypeImport INT = 2;
    --===========================================================

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
           org.OrganizationId,
           org.Name AS OrganizationName,
           org.Address AS OrganizationAddress,
           org.BIN AS OrganizationBin,
           ptb.TransactionTime AS TransactionTime,
           ptb.InitQty AS InitialQty,
           ptb.InitUnitPrice * ptb.InitQty AS InitPriceWithoutVat,
           ptb.ProductId,
           prod.Name AS ProductName,
           prod.Code AS ProductCode,
           prod.HSCode AS ProductHsCode,
           prod.ModelNo AS ProductModel,
           ptb.PurchaseDetailId,
           purch.PurchaseId,
           purchDtl.Quantity AS PurchaseQty,
           purchDtl.Quantity * purchDtl.UnitPrice AS PurchasePriceWithoutVat,
           ptb.InitQty + purchDtl.Quantity AS QtyAfterPurchase,
           (ptb.InitUnitPrice * ptb.InitQty) + (purchDtl.Quantity * purchDtl.UnitPrice) AS ProductPriceWithoutVatAfterPurchase,
           vndr.VendorId,
           vndr.Name AS VendorName,
           ISNULL(vndr.BinNo, vndr.NationalIdNo) AS VendorBinOrNidNo,
           vndr.Address AS VendorAddress,
           ISNULL(purch.VatChallanNo, purch.BillOfEntry) AS PurcVatChallanOrBillOfEntryNo,
           ISNULL(purch.VatChallanIssueDate, purch.BillOfEntryDate) AS PurcVatChallanOrBillOfEntryDate,
           sls.SalesId,
           slsDtl.SalesDetailId,
           slsDtl.Quantity AS SoldQty,
           slsDtl.Quantity * slsDtl.UnitPrice AS SalesPriceWithoutVat,
           slsDtl.SupplementaryDutyPercent AS SalesSupplimentaryDuty,
           slsDtl.VATPercent AS SalesVat,
           cust.Name AS CustomerName,
           cust.Name AS CustomerAddress,
           cust.Name AS CustomerBinOrNidNo,
           sls.VatChallanNo AS SalesVatChallanNo,
           sls.TaxInvoicePrintedTime AS SalesVatChallanDate,
           ptb.InitQty + ISNULL(purchDtl.Quantity, 0) - ISNULL(dnd.ReturnQuantity, 0) - ISNULL(bom.UsedQuantity, 0)
           + ISNULL(prdcnRcv.ReceiveQuantity, 0) - ISNULL(slsDtl.Quantity, 0) + ISNULL(cnd.ReturnQuantity, 0)
           - ISNULL(dmg.DamageQty, 0) AS ClosingProdQty,
           ptb.InitQty * ptb.InitUnitPrice + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0)
           - ISNULL(dnd.ReturnQuantity * purchDtlDnd.UnitPrice, 0) - ISNULL(bom.UsedQuantity * ptb.InitUnitPrice, 0)
           + ISNULL(prdcnRcv.ReceiveQuantity * prcStup.SalesUnitPrice, 0)
           - ISNULL(slsDtl.Quantity * slsDtl.UnitPrice, 0) + ISNULL(cnd.ReturnQuantity * slsDtlCnd.UnitPrice, 0)
           - ISNULL(dmg.DamageQty * ptb.InitUnitPrice, 0) AS ClosingTotalPriceWithoutVat,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName,
           CAST(CASE
                    WHEN ptb.PurchaseDetailId IS NOT NULL THEN
                        CASE
                            WHEN purch.PurchaseTypeId = @purchaseTypeLocal THEN
                                CASE
                                    WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১০))'
                                    WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                                        N'ক্রয় (নোট (১২))'
                                    WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৪))'
                                    WHEN purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৮))'
                                    WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৬))'
                                    WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId THEN
                                        CASE
                                            WHEN org.IsSellStandardVatProduct = 0 THEN
                                                N'ক্রয় (নোট (২১))'
                                            ELSE
                                                CASE
                                                    WHEN vndr.IsRegisteredAsTurnOverOrg = 1 THEN
                                                        N'ক্রয় (নোট (১৯))'
                                                    WHEN vndr.IsRegistered = 1 THEN
                                                        N'ক্রয় (নোট (২০))'
                                                    ELSE
                                                        N'ক্রয়'
                                                END
                                        END
                                    ELSE
                                        N'ক্রয়'
                                END
                            WHEN purch.PurchaseTypeId = @purchaseTypeImport THEN
                                CASE
                                    WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১১))'
                                    WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৩))'
                                    WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৫))'
                                    WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                                        N'ক্রয় (নোট (১৭))'
                                    WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId THEN
                                        CASE
                                            WHEN org.IsSellStandardVatProduct = 0 THEN
                                                N'ক্রয় (নোট (২২))'
                                            ELSE
                                                N'ক্রয়'
                                        END
                                    ELSE
                                        N'ক্রয়'
                                END
                            ELSE
                                N'শাখা স্থানান্তর'
                        END
                    WHEN ptb.DebitNoteDetailId IS NOT NULL THEN
                        N'ডেবিট নোট (নোট (২৬))'
                    WHEN ptb.DamageId IS NOT NULL THEN
                        N'ক্ষতিগ্রস্ত'
                    WHEN ptb.UsedInProductionId IS NOT NULL THEN
                        N'উৎপাদনে ব্যবহৃত'
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
                    WHEN ptb.ProductionReceiveId IS NOT NULL THEN
                        N'উৎপাদন'
                    ELSE
                        N'ব্যবহার'
                END AS NVARCHAR(100)) AS OperationType
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
              @VendorId = 0
              OR @VendorId IS NULL
              OR purch.VendorId = @VendorId
          )
          AND
          (
              @CustomerId = 0
              OR @CustomerId IS NULL
              OR sls.CustomerId = @CustomerId
          )
          AND
          (
              @ProductId = 0
              OR @ProductId IS NULL
              OR ptb.ProductId = @ProductId
          );
END;

