-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Test Execution: SpGetMushakNinePointOne 2
-- =============================================
CREATE PROCEDURE [dbo].[SpGetMushakNinePointOne] @MushakGenerationId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;


    -- Insert statements for procedure here
    DECLARE @Year INT,
            @Month INT;

    SELECT @Year = mg.MushakForYear,
           @Month = mg.MushakForMonth
    FROM dbo.MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId;

    DECLARE @isTransactionOccuredInLastTerm BIT = 0,
            @isTransactionOccuredInLastTermInWords NVARCHAR(10) = N'না',
            @TermOfTax NVARCHAR(80),
            @TypeOfSubmission NVARCHAR(50) = N'মূল দাখিলপত্র';

    --===========================================================
    DECLARE @financialActivitImporter INT = 1,
            @financialActivityExporter INT = 2,
            @financialActivityManufacturer INT = 3,
            @financialActivityRetailer INT = 4,
            @financialActivityWholesaler INT = 5,
            @financialActivityRetailerAndWholesaler INT = 6;
    --===========================================================

    --===========================================================
    DECLARE @salesTypeLocal INT = 1,
            @salesTypeExport INT = 2,
            @salesTypeTrading INT = 4,
            @exportTypeDirect INT = 1,
            @exportTypeInDirect INT = 2;
    --===========================================================

    --===========================================================
    DECLARE @purchaseTypeLocal INT = 1,
            @purchaseTypeImport INT = 2;
    --===========================================================

    --===========================================================
    DECLARE @zeroVatProdVatTypeId INT = 1,
            @vatExemptedProdVatTypeId INT = 2,
            @standardVatProdVatTypeId INT = 3,
            @mrpProdVatTypeId INT = 4,
            @fixedVatProdVatTypeId INT = 6,
            @otherThanStandardVat1p5ProdVatTypeId INT = 8,
            @otherThanStandardVat2p0ProdVatTypeId INT = 9,
            @otherThanStandardVat2p5ProdVatTypeId INT = 10,
            @otherThanStandardVat4p0ProdVatTypeId INT = 11,
            @otherThanStandardVat5p0ProdVatTypeId INT = 12,
            @otherThanStandardVat5p5ProdVatTypeId INT = 13,
            @otherThanStandardVat6p0ProdVatTypeId INT = 14,
            @otherThanStandardVat7p5ProdVatTypeId INT = 15,
            @otherThanStandardVat9p0ProdVatTypeId INT = 16,
            @otherThanStandardVat10p0ProdVatTypeId INT = 17;
    --===========================================================

    --===========================================================
    --Variables for assigning value
    DECLARE @DirectExportAmount DECIMAL(18, 2),
            @IndirectExportAmount DECIMAL(18, 2),
            @VatExemptedProdSellAmount DECIMAL(18, 2),
            @StandardVatRateProdSellAmount DECIMAL(18, 2),
            @StandardVatRateProdSellSdAmount DECIMAL(18, 2),
            @StandardVatRateProdSellVatAmount DECIMAL(18, 2),
            @MrpProdSellAmount DECIMAL(18, 2),
            @MrpProdSellSdAmount DECIMAL(18, 2),
            @MrpProdSellVatAmount DECIMAL(18, 2),
            @FixedVatProdSellAmount DECIMAL(18, 2),
            @FixedVatProdSellSdAmount DECIMAL(18, 2),
            @FixedVatProdSellVatAmount DECIMAL(18, 2),
            @OtherThanStandardVatRateProdSellAmount DECIMAL(18, 2),
            @OtherThanStandardVatRateProdSellSdAmount DECIMAL(18, 2),
            @OtherThanStandardVatRateProdSellVatAmount DECIMAL(18, 2),
            @TradingSellAmount DECIMAL(18, 2),
            @TradingSellSdAmount DECIMAL(18, 2),
            @TradingSellVatAmount DECIMAL(18, 2),
            @TotalAmount DECIMAL(18, 2),
            @TotalSdAmount DECIMAL(18, 2),
            @TotalVatAmount DECIMAL(18, 2),
            @ZeroVatProdLocalPurchaseAmount DECIMAL(18, 2),
            @ZeroVatProdImportAmount DECIMAL(18, 2),
            @VatExemptedProdLocalPurchaseAmount DECIMAL(18, 2),
            @VatExemptedProdImportAmount DECIMAL(18, 2),
            @StandardVatProdLocalPurchaseAmount DECIMAL(18, 2),
            @StandardVatProdLocalPurchaseVatAmount DECIMAL(18, 2),
            @StandardVatProdImportAmount DECIMAL(18, 2),
            @StandardVatProdImportVatAmount DECIMAL(18, 2),
            @OtherThanStandardVatProdLocalPurchaseAmount DECIMAL(18, 2),
            @OtherThanStandardVatProdLocalPurchaseVatAmount DECIMAL(18, 2),
            @OtherThanStandardVatProdImportAmount DECIMAL(18, 2),
            @OtherThanStandardVatProdImportVatAmount DECIMAL(18, 2),
            @FixedVatProdLocalPurchaseAmount DECIMAL(18, 2),
            @FixedVatProdLocalPurchaseVatAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseFromTurnOverOrgAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseFromNonRegOrgAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount DECIMAL(18, 2),
            @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount DECIMAL(18, 2),
            @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount DECIMAL(18, 2),
            @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount DECIMAL(18, 2),
            @TotalRawMaterialPurchaseAmount DECIMAL(18, 2),
            @TotalRawMaterialRebateAmount DECIMAL(18, 2),
            @IncrementalAdjustmentAmountForVdsSale DECIMAL(18, 2),
            @IncrementalAdjustmentAmountForNotPaidInBankingChannel DECIMAL(18, 2),
            @IncrementalAdjustmentVatAmountForDebitNote DECIMAL(18, 2),
            @TotalIncrementalAdjustmentVatAmountWithoutMisc DECIMAL(18, 2),
            @DecrementalAdjustmentAmountForVdsPurchase DECIMAL(18, 2),
            @DecrementalAdjustmentAmountForAdvanceTaxInImport DECIMAL(18, 2),
            @DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2),
            @DecrementalAdjustmentVatAmountForCreditNote DECIMAL(18, 2),
            @TotalDecrementalAdjustmentVatAmountWithoutMisc DECIMAL(18, 2),
            @NetTaxTotalPayableSdAmountInCurrentTaxTerm DECIMAL(18, 2),
            @NetTaxSdAmountForDebitNote DECIMAL(18, 2),
            @NetTaxSdAmountForCreditNote DECIMAL(18, 2),
            @NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2);
    --===========================================================

    --===========================================================
    --Direct Export
    SELECT @DirectExportAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeExport
          AND sls.ExportTypeId = @exportTypeDirect;
    --/Direct Export

    --InDirect Export
    SELECT @IndirectExportAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeExport
          AND sls.ExportTypeId = @exportTypeInDirect;
    --/InDirect Export

    --Vat Exempted Product Sell Amount
    SELECT @VatExemptedProdSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Sell Amount

    --Standard Vat Rate Product Sell
    SELECT @StandardVatRateProdSellAmount
        = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @StandardVatRateProdSellSdAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity)
                               * slsDtl.SupplementaryDutyPercent / 100,
                               0
                           )
                    ),
           @StandardVatRateProdSellVatAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    --/Standard Vat Rate Product Sell

    --MRP Product Sell
    SELECT @MrpProdSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @MrpProdSellSdAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity)
                               * slsDtl.SupplementaryDutyPercent / 100,
                               0
                           )
                    ),
           @MrpProdSellVatAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId;
    --/MRP Product Sell

    --Fixed Vat Product Sell
    SELECT @FixedVatProdSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @FixedVatProdSellSdAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    ),
           @FixedVatProdSellVatAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId;
    --/Fixed Vat Product Sell


    --Other Than Standard Vat Product Sell
    SELECT @OtherThanStandardVatRateProdSellAmount
        = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @OtherThanStandardVatRateProdSellSdAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    ),
           @OtherThanStandardVatRateProdSellVatAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId IN ( @otherThanStandardVat1p5ProdVatTypeId,
                                           @otherThanStandardVat2p0ProdVatTypeId,
                                           @otherThanStandardVat2p5ProdVatTypeId,
                                           @otherThanStandardVat4p0ProdVatTypeId,
                                           @otherThanStandardVat5p0ProdVatTypeId,
                                           @otherThanStandardVat5p5ProdVatTypeId,
                                           @otherThanStandardVat6p0ProdVatTypeId,
                                           @otherThanStandardVat7p5ProdVatTypeId,
                                           @otherThanStandardVat9p0ProdVatTypeId,
                                           @otherThanStandardVat10p0ProdVatTypeId
                                         );
    --/Other Than Standard Vat Product Sell

    --Trading Sell
    SELECT @TradingSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @TradingSellSdAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity)
                               * slsDtl.SupplementaryDutyPercent / 100,
                               0
                           )
                    ),
           @TradingSellVatAmount
               = SUM(ISNULL(
                               ((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) * slsDtl.VATPercent
                               / 100,
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeTrading;
    --/Trading Sell

    --Total Sell
    SET @TotalAmount
        = ISNULL(@DirectExportAmount, 0) + ISNULL(@IndirectExportAmount, 0) + ISNULL(@VatExemptedProdSellAmount, 0)
          + ISNULL(@StandardVatRateProdSellAmount, 0) + ISNULL(@MrpProdSellAmount, 0)
          + ISNULL(@FixedVatProdSellAmount, 0) + ISNULL(@OtherThanStandardVatRateProdSellAmount, 0)
          + ISNULL(@TradingSellAmount, 0);
    --/Total Sell

    --Total SD on Sell
    SET @TotalSdAmount
        = ISNULL(@StandardVatRateProdSellSdAmount, 0) + ISNULL(@MrpProdSellSdAmount, 0)
          + ISNULL(@FixedVatProdSellSdAmount, 0) + ISNULL(@OtherThanStandardVatRateProdSellAmount, 0)
          + ISNULL(@TradingSellSdAmount, 0);
    --/Total SD on Sell

    --Total VAT on Sell
    SET @TotalVatAmount
        = ISNULL(@StandardVatRateProdSellVatAmount, 0) + ISNULL(@MrpProdSellVatAmount, 0)
          + ISNULL(@FixedVatProdSellVatAmount, 0) + ISNULL(@OtherThanStandardVatRateProdSellVatAmount, 0)
          + ISNULL(@TradingSellVatAmount, 0);
    --Total VAT on Sell

    --Zero Vat Product Local Purchase
    SELECT @ZeroVatProdLocalPurchaseAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId;
    --/Zero Vat Product Local Purchase

    --Zero Vat Product Import
    SELECT @ZeroVatProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId;
    --/Zero Vat Product Import

    --Vat Exempted Product Local Purchase
    SELECT @VatExemptedProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Local Purchase

    --Vat Exempted Product Import
    SELECT @VatExemptedProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Import

    --Standard Vat Product Local Purchase
    SELECT @StandardVatProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @StandardVatProdLocalPurchaseVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    ----/Standard Vat Product Local Purchase

    --Standard Vat Product Import
    SELECT @StandardVatProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @StandardVatProdImportVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    --/Standard Vat Product Import

    --Other Than Standard Vat Product Local Purchase
    SELECT @OtherThanStandardVatProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @OtherThanStandardVatProdLocalPurchaseVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId IN ( @otherThanStandardVat1p5ProdVatTypeId,
                                             @otherThanStandardVat2p0ProdVatTypeId,
                                             @otherThanStandardVat2p5ProdVatTypeId,
                                             @otherThanStandardVat4p0ProdVatTypeId,
                                             @otherThanStandardVat5p0ProdVatTypeId,
                                             @otherThanStandardVat5p5ProdVatTypeId,
                                             @otherThanStandardVat6p0ProdVatTypeId,
                                             @otherThanStandardVat7p5ProdVatTypeId,
                                             @otherThanStandardVat9p0ProdVatTypeId,
                                             @otherThanStandardVat10p0ProdVatTypeId
                                           );
    --/Other Than Standard Vat Product Local Purchase

    --Other Than Standard Vat Product Import
    SELECT @OtherThanStandardVatProdImportAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @OtherThanStandardVatProdImportVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId IN ( @otherThanStandardVat1p5ProdVatTypeId,
                                             @otherThanStandardVat2p0ProdVatTypeId,
                                             @otherThanStandardVat2p5ProdVatTypeId,
                                             @otherThanStandardVat4p0ProdVatTypeId,
                                             @otherThanStandardVat5p0ProdVatTypeId,
                                             @otherThanStandardVat5p5ProdVatTypeId,
                                             @otherThanStandardVat6p0ProdVatTypeId,
                                             @otherThanStandardVat7p5ProdVatTypeId,
                                             @otherThanStandardVat9p0ProdVatTypeId,
                                             @otherThanStandardVat10p0ProdVatTypeId
                                           );
    --Other Than Standard Vat Product Import

    --Fixed Vat Product Local Purchase
    SELECT @FixedVatProdLocalPurchaseAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @FixedVatProdLocalPurchaseVatAmount = SUM(purchDtl.Quantity * purchDtl.VATPercent / 100)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId;
    --Fixed Vat Product Local Purchase

    --Non-Rebatable Product Local Purchase From Turnover Organization
    SELECT @NonRebatableProdLocalPurchaseFromTurnOverOrgAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND prod.IsNonRebateable = 1;
    --/Non-Rebatable Product Local Purchase From Turnover Organization

    --Non-Rebatable Product Local Purchase From Non-Registered Organization
    SELECT @NonRebatableProdLocalPurchaseFromNonRegOrgAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND prod.IsNonRebateable = 1;
    --/Non-Rebatable Product Local Purchase From Non-Registered Organization

    --Non-Rebatable Product Local Purchase By Organization Who Sell Other Than Standard Vat Product
    SELECT @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = purch.OrganizationId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND org.IsSellStandardVatProduct = 0
          AND prod.IsNonRebateable = 1;
    --/Non-Rebatable Product Local Purchase By Organization Who Sell Other Than Standard Vat Product

    --Non-Rebatable Product Import By Organization Who Sell Other Than Standard Vat Product
    SELECT @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount
               = SUM(ISNULL(
                               (((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
                                * purchDtl.VATPercent / 100
                               ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = purch.OrganizationId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND org.IsSellStandardVatProduct = 0
          AND prod.IsNonRebateable = 1;
    --/Non-Rebatable Product Import By Organization Who Sell Other Than Standard Vat Product

    SET @TotalRawMaterialPurchaseAmount
        = ISNULL(@ZeroVatProdLocalPurchaseAmount, 0) + ISNULL(@ZeroVatProdImportAmount, 0)
          + ISNULL(@VatExemptedProdLocalPurchaseAmount, 0) + ISNULL(@VatExemptedProdImportAmount, 0)
          + ISNULL(@StandardVatProdLocalPurchaseAmount, 0) + ISNULL(@StandardVatProdImportAmount, 0)
          + ISNULL(@OtherThanStandardVatProdLocalPurchaseAmount, 0) + ISNULL(@OtherThanStandardVatProdImportAmount, 0)
          + ISNULL(@FixedVatProdLocalPurchaseAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount, 0)
          + ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount, 0);

    SET @TotalRawMaterialRebateAmount
        = ISNULL(@StandardVatProdLocalPurchaseVatAmount, 0) + ISNULL(@StandardVatProdImportVatAmount, 0)
          + ISNULL(@OtherThanStandardVatProdLocalPurchaseVatAmount, 0)
          + ISNULL(@OtherThanStandardVatProdImportVatAmount, 0) + ISNULL(@FixedVatProdLocalPurchaseVatAmount, 0);

    --Incremental Adjustment 
    SELECT @IncrementalAdjustmentAmountForVdsSale = SUM(COALESCE(sls.VDSAmount, sls.TotalVAT, 0))
    FROM dbo.Sales sls
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocal
          AND sls.IsVatDeductedInSource = 1;

    SET @IncrementalAdjustmentAmountForNotPaidInBankingChannel = 0;

    SELECT @IncrementalAdjustmentVatAmountForDebitNote
        = SUM(   CASE
                     WHEN purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                         ISNULL(dnd.ReturnQuantity * purchDtl.VATPercent, 0)
                     ELSE
                         ISNULL(
                                   dnd.ReturnQuantity * (purchDtl.UnitPrice - purchDtl.DiscountPerItem)
                                   * purchDtl.VATPercent / 100,
                                   0
                               )
                 END
             ),
           @NetTaxSdAmountForDebitNote
               = SUM(ISNULL(
                               dnd.ReturnQuantity * (purchDtl.UnitPrice - purchDtl.DiscountPerItem)
                               * purchDtl.SupplementaryDutyPercent / 100,
                               0
                           )
                    )
    FROM dbo.DebitNote dn
        INNER JOIN dbo.DebitNoteDetail dnd
            ON dnd.DebitNoteId = dn.DebitNoteId
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseDetailId = dnd.PurchaseDetailId
    WHERE dn.MushakGenerationId = @MushakGenerationId;

    SET @TotalIncrementalAdjustmentVatAmountWithoutMisc
        = ISNULL(@IncrementalAdjustmentAmountForVdsSale, 0)
          + ISNULL(@IncrementalAdjustmentAmountForNotPaidInBankingChannel, 0)
          + ISNULL(@IncrementalAdjustmentVatAmountForDebitNote, 0);
    --/Incremental Adjustment

    --Decremental Adjustment
    SELECT @DecrementalAdjustmentAmountForVdsPurchase = SUM(COALESCE(purch.VDSAmount, purch.TotalVAT, 0))
    FROM dbo.Purchase purch
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purch.IsVatDeductedInSource = 1;

    SELECT @DecrementalAdjustmentAmountForAdvanceTaxInImport = SUM(ISNULL(purch.AdvanceTaxPaidAmount, 0))
    FROM dbo.Purchase purch
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.PurchaseTypeId = @purchaseTypeImport;

    SET @DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd = 0;

    SELECT @DecrementalAdjustmentVatAmountForCreditNote
        = CASE
              WHEN slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                  cnd.ReturnQuantity * slsDtl.VATPercent
              ELSE
                  cnd.ReturnQuantity * (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.VATPercent / 100
          END,
           @NetTaxSdAmountForCreditNote
               = cnd.ReturnQuantity * (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.SupplementaryDutyPercent
                 / 100
    FROM dbo.CreditNote cn
        INNER JOIN dbo.CreditNoteDetail cnd
            ON cnd.CreditNoteId = cn.CreditNoteId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesDetailId = cnd.SalesDetailId;

    SET @NetTaxTotalPayableSdAmountInCurrentTaxTerm
        = ISNULL(@TotalSdAmount, 0) + ISNULL(@NetTaxSdAmountForDebitNote, 0) - ISNULL(@NetTaxSdAmountForCreditNote, 0)
          - ISNULL(@NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd, 0);

    SET @TotalDecrementalAdjustmentVatAmountWithoutMisc
        = ISNULL(@DecrementalAdjustmentAmountForVdsPurchase, 0)
          + ISNULL(@DecrementalAdjustmentAmountForAdvanceTaxInImport, 0)
          + ISNULL(@DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd, 0)
          + ISNULL(@DecrementalAdjustmentVatAmountForCreditNote, 0);
    --/Decremental Adjustment

    --===========================================================

    SELECT @TermOfTax
        = dbo.FnGetNameOfMonthInBanglaByNumber(@Month) + N' / ' + dbo.FnConvertIntToBanglaUnicodeNumber(@Year);

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Sales sls
        WHERE sls.MushakGenerationId = @MushakGenerationId
    )
    BEGIN
        SET @isTransactionOccuredInLastTerm = 1;
        SET @isTransactionOccuredInLastTermInWords = N'হ্যাঁ';
    END;

    DECLARE @MushakReturn TABLE
    (
        MushakReturnId INT NOT NULL,
        MushakGenerationId INT NOT NULL,
        --Part-1: Tax-payers information
        OrganizationId INT NOT NULL,
        OrganizationName NVARCHAR(100) NOT NULL,
        OrganizationVatRegNo NVARCHAR(20) NOT NULL,
        OrganizationBin NVARCHAR(20) NOT NULL,
        OrganizationAddress NVARCHAR(200) NOT NULL,
        OrganizationTypeOfBusiness NVARCHAR(50) NOT NULL,
        OrganizationTypeOfFinancialActivity NVARCHAR(50) NOT NULL,
        --Part-2: Submission Information
        TermOfTax NVARCHAR(80) NOT NULL,
        TypeOfSubmission NVARCHAR(100) NOT NULL,
        IsOperatedInLastTerm BIT NOT NULL,
        IsOperatedInLastTermInWords NVARCHAR(10) NOT NULL,
        DateOfSubmission DATETIME NOT NULL,
        --Part-3: Sell/Supply - Payable Tax
        DirectExportAmount DECIMAL(18, 2) NOT NULL,
        IndirectExportAmount DECIMAL(18, 2) NOT NULL,
        VatExemptedProdSellAmount DECIMAL(18, 2) NOT NULL,
        StandardVatRateProdSellAmount DECIMAL(18, 2) NOT NULL,
        StandardVatRateProdSellSdAmount DECIMAL(18, 2) NOT NULL,
        StandardVatRateProdSellVatAmount DECIMAL(18, 2) NOT NULL,
        MrpProdSellAmount DECIMAL(18, 2) NOT NULL,
        MrpProdSellSdAmount DECIMAL(18, 2) NOT NULL,
        MrpProdSellVatAmount DECIMAL(18, 2) NOT NULL,
        FixedVatProdSellAmount DECIMAL(18, 2) NOT NULL,
        FixedVatProdSellSdAmount DECIMAL(18, 2) NOT NULL,
        FixedVatProdSellVatAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatRateProdSellAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatRateProdSellSdAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatRateProdSellVatAmount DECIMAL(18, 2) NOT NULL,
        TradingSellAmount DECIMAL(18, 2) NOT NULL,
        TradingSellSdAmount DECIMAL(18, 2) NOT NULL,
        TradingSellVatAmount DECIMAL(18, 2) NOT NULL,
        TotalAmount DECIMAL(18, 2) NOT NULL,
        TotalSdAmount DECIMAL(18, 2) NOT NULL,
        TotalVatAmount DECIMAL(18, 2) NOT NULL,
        --Part-4: Purchase - Raw-Material Tax
        ZeroVatProdLocalPurchaseAmount DECIMAL(18, 2) NOT NULL,
        ZeroVatProdImportAmount DECIMAL(18, 2) NOT NULL,
        VatExemptedProdLocalPurchaseAmount DECIMAL(18, 2) NOT NULL,
        VatExemptedProdImportAmount DECIMAL(18, 2) NOT NULL,
        StandardVatProdLocalPurchaseAmount DECIMAL(18, 2) NOT NULL,
        StandardVatProdLocalPurchaseVatAmount DECIMAL(18, 2) NOT NULL,
        StandardVatProdImportAmount DECIMAL(18, 2) NOT NULL,
        StandardVatProdImportVatAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatProdLocalPurchaseAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatProdLocalPurchaseVatAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatProdImportAmount DECIMAL(18, 2) NOT NULL,
        OtherThanStandardVatProdImportVatAmount DECIMAL(18, 2) NOT NULL,
        FixedVatProdLocalPurchaseAmount DECIMAL(18, 2) NOT NULL,
        FixedVatProdLocalPurchaseVatAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseFromTurnOverOrgAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseFromNonRegOrgAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount DECIMAL(18, 2) NOT NULL,
        NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount DECIMAL(18, 2) NOT NULL,
        TotalRawMaterialPurchaseAmount DECIMAL(18, 2) NOT NULL,
        TotalRawMaterialRebateAmount DECIMAL(18, 2) NOT NULL,
        --Part-5: Incremental Adjustment (VAT)
        IncrementalAdjustmentAmountForVdsSale DECIMAL(18, 2) NOT NULL,
        IncrementalAdjustmentAmountForNotPaidInBankingChannel DECIMAL(18, 2) NOT NULL,
        IncrementalAdjustmentAmountForDebitNote DECIMAL(18, 2) NOT NULL,
        IncrementalAdjustmentAmountForOtherReason DECIMAL(18, 2) NOT NULL,
        IncrementalAdjustmentDescriptionForOtherReason NVARCHAR(200) NOT NULL,
        TotalIncrementalAdjustmentAmount DECIMAL(18, 2) NOT NULL,
        --Part-6: Decremental Adjustment
        DecrementalAdjustmentAmountForVdsPurchase DECIMAL(18, 2) NOT NULL,
        DecrementalAdjustmentAmountForAdvanceTaxInImport DECIMAL(18, 2) NOT NULL,
        DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2) NOT NULL,
        DecrementalAdjustmentAmountForCreditNote DECIMAL(18, 2) NOT NULL,
        DecrementalAdjustmentAmountForOtherReason DECIMAL(18, 2) NOT NULL,
        DecrementalAdjustmentDescriptionForOtherReason NVARCHAR(200) NOT NULL,
        TotalDecrementalAdjustmentAmount DECIMAL(18, 2) NOT NULL,
        --Part-7: Net Tax Calculation
        NetTaxTotalPayableVatAmountInCurrentTaxTerm DECIMAL(18, 2) NOT NULL,
        NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance DECIMAL(18, 2) NOT NULL,
        NetTaxTotalPayableSdAmountInCurrentTaxTerm DECIMAL(18, 2) NOT NULL,
        NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance DECIMAL(18, 2) NOT NULL,
        NetTaxSdAmountForDebitNote DECIMAL(18, 2) NOT NULL,
        NetTaxSdAmountForCreditNote DECIMAL(18, 2) NOT NULL,
        NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2) NOT NULL,
        NetTaxInterstAmountForDeuVat DECIMAL(18, 2) NOT NULL,
        NetTaxInterstAmountForDeuSd DECIMAL(18, 2) NOT NULL,
        NetTaxFineAndPenaltyAmount DECIMAL(18, 2) NOT NULL,
        NetTaxExciseDuty DECIMAL(18, 2) NOT NULL,
        NetTaxDevelopmentSurcharge DECIMAL(18, 2) NOT NULL,
        NetTaxInformationTechnologyDevelopmentSurcharge DECIMAL(18, 2) NOT NULL,
        NetTaxHealthProtectionSurcharge DECIMAL(18, 2) NOT NULL,
        NetTaxEnvironmentProtectionSurcharge DECIMAL(18, 2) NOT NULL,
        NetTaxVatEndingBalanceOfLastTerm DECIMAL(18, 2) NOT NULL,
        NetTaxSdEndingBalanceOfLastTerm DECIMAL(18, 2) NOT NULL,
        --Part-8: Tax Payment Schedule (Treasury Submission)
        TreasurySubmissionAmountForCurrentTermVat DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForCurrentTermVat NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForCurrentTermSd DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForCurrentTermSd NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForInterestOfDueVat DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForInterestOfDueVat NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForInterestOfDueSd DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForInterestOfDueSd NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForFineAndPenalty DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForFineAndPenalty NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForExciseDuty DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForExciseDuty NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForDevelopmentSurcharge DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodetForDevelopmentSurcharge NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForInformationTechnologyDevelopmentSurcharge DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForInformationTechnologyDevelopmentSurcharge NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForHealthProtectionSurcharge DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForHealthProtectionSurcharge NVARCHAR(20) NOT NULL,
        TreasurySubmissionAmountForEnvironmentProtectionSurcharge DECIMAL(18, 2) NOT NULL,
        TreasurySubmissionEconomicCodeForEnvironmentProtectionSurcharge NVARCHAR(20) NOT NULL,
        --Part-9: Ending Balance (Initial Balance for next Term of Tax)
        VatEndingBalance DECIMAL(18, 2) NOT NULL,
        SdEndingBalance DECIMAL(18, 2) NOT NULL,
        --Part-10: Return
        IsWantToGetReturnAmountInEndingBalance BIT NOT NULL,
        IsWantToGetReturnAmountInEndingBalanceInWords NVARCHAR(10) NOT NULL,
        --Part-10: Declaration
        VatResponsiblePersonName NVARCHAR(100) NOT NULL,
        VatResponsiblePersonDesignation NVARCHAR(100) NOT NULL,
        DateOfSignatureInSubmission DATETIME NOT NULL,
        VatResponsiblePersonMobileNo NVARCHAR(20) NOT NULL,
        VatResponsiblePersonEmail NVARCHAR(100) NOT NULL
    );

    INSERT INTO @MushakReturn
    (
        MushakReturnId,
        MushakGenerationId,
        OrganizationId,
        OrganizationName,
        OrganizationVatRegNo,
        OrganizationBin,
        OrganizationAddress,
        OrganizationTypeOfBusiness,
        OrganizationTypeOfFinancialActivity,
        TermOfTax,
        TypeOfSubmission,
        IsOperatedInLastTerm,
        IsOperatedInLastTermInWords,
        DateOfSubmission,
        DirectExportAmount,
        IndirectExportAmount,
        VatExemptedProdSellAmount,
        StandardVatRateProdSellAmount,
        StandardVatRateProdSellSdAmount,
        StandardVatRateProdSellVatAmount,
        MrpProdSellAmount,
        MrpProdSellSdAmount,
        MrpProdSellVatAmount,
        FixedVatProdSellAmount,
        FixedVatProdSellSdAmount,
        FixedVatProdSellVatAmount,
        OtherThanStandardVatRateProdSellAmount,
        OtherThanStandardVatRateProdSellSdAmount,
        OtherThanStandardVatRateProdSellVatAmount,
        TradingSellAmount,
        TradingSellSdAmount,
        TradingSellVatAmount,
        TotalAmount,
        TotalSdAmount,
        TotalVatAmount,
        ZeroVatProdLocalPurchaseAmount,
        ZeroVatProdImportAmount,
        VatExemptedProdLocalPurchaseAmount,
        VatExemptedProdImportAmount,
        StandardVatProdLocalPurchaseAmount,
        StandardVatProdLocalPurchaseVatAmount,
        StandardVatProdImportAmount,
        StandardVatProdImportVatAmount,
        OtherThanStandardVatProdLocalPurchaseAmount,
        OtherThanStandardVatProdLocalPurchaseVatAmount,
        OtherThanStandardVatProdImportAmount,
        OtherThanStandardVatProdImportVatAmount,
        FixedVatProdLocalPurchaseAmount,
        FixedVatProdLocalPurchaseVatAmount,
        NonRebatableProdLocalPurchaseFromTurnOverOrgAmount,
        NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount,
        NonRebatableProdLocalPurchaseFromNonRegOrgAmount,
        NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount,
        NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount,
        NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount,
        NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount,
        NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount,
        TotalRawMaterialPurchaseAmount,
        TotalRawMaterialRebateAmount,
        IncrementalAdjustmentAmountForVdsSale,
        IncrementalAdjustmentAmountForNotPaidInBankingChannel,
        IncrementalAdjustmentAmountForDebitNote,
        IncrementalAdjustmentAmountForOtherReason,
        IncrementalAdjustmentDescriptionForOtherReason,
        TotalIncrementalAdjustmentAmount,
        DecrementalAdjustmentAmountForVdsPurchase,
        DecrementalAdjustmentAmountForAdvanceTaxInImport,
        DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd,
        DecrementalAdjustmentAmountForCreditNote,
        DecrementalAdjustmentAmountForOtherReason,
        DecrementalAdjustmentDescriptionForOtherReason,
        TotalDecrementalAdjustmentAmount,
        NetTaxTotalPayableVatAmountInCurrentTaxTerm,
        NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance,
        NetTaxTotalPayableSdAmountInCurrentTaxTerm,
        NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance,
        NetTaxSdAmountForDebitNote,
        NetTaxSdAmountForCreditNote,
        NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd,
        NetTaxInterstAmountForDeuVat,
        NetTaxInterstAmountForDeuSd,
        NetTaxFineAndPenaltyAmount,
        NetTaxExciseDuty,
        NetTaxDevelopmentSurcharge,
        NetTaxInformationTechnologyDevelopmentSurcharge,
        NetTaxHealthProtectionSurcharge,
        NetTaxEnvironmentProtectionSurcharge,
        NetTaxVatEndingBalanceOfLastTerm,
        NetTaxSdEndingBalanceOfLastTerm,
        TreasurySubmissionAmountForCurrentTermVat,
        TreasurySubmissionEconomicCodeForCurrentTermVat,
        TreasurySubmissionAmountForCurrentTermSd,
        TreasurySubmissionEconomicCodeForCurrentTermSd,
        TreasurySubmissionAmountForInterestOfDueVat,
        TreasurySubmissionEconomicCodeForInterestOfDueVat,
        TreasurySubmissionAmountForInterestOfDueSd,
        TreasurySubmissionEconomicCodeForInterestOfDueSd,
        TreasurySubmissionAmountForFineAndPenalty,
        TreasurySubmissionEconomicCodeForFineAndPenalty,
        TreasurySubmissionAmountForExciseDuty,
        TreasurySubmissionEconomicCodeForExciseDuty,
        TreasurySubmissionAmountForDevelopmentSurcharge,
        TreasurySubmissionEconomicCodetForDevelopmentSurcharge,
        TreasurySubmissionAmountForInformationTechnologyDevelopmentSurcharge,
        TreasurySubmissionEconomicCodeForInformationTechnologyDevelopmentSurcharge,
        TreasurySubmissionAmountForHealthProtectionSurcharge,
        TreasurySubmissionEconomicCodeForHealthProtectionSurcharge,
        TreasurySubmissionAmountForEnvironmentProtectionSurcharge,
        TreasurySubmissionEconomicCodeForEnvironmentProtectionSurcharge,
        VatEndingBalance,
        SdEndingBalance,
        IsWantToGetReturnAmountInEndingBalance,
        IsWantToGetReturnAmountInEndingBalanceInWords,
        VatResponsiblePersonName,
        VatResponsiblePersonDesignation,
        DateOfSignatureInSubmission,
        VatResponsiblePersonMobileNo,
        VatResponsiblePersonEmail
    )
    SELECT 0,                                                                                                          -- MushakReturnId - int
           mg.MushakGenerationId,                                                                                      -- MushakGenerationId - int
           org.OrganizationId,                                                                                         -- OrganizationId - int
           org.Name,                                                                                                   -- OrganizationName - nvarchar(100)
           org.VATRegNo,                                                                                               -- OrganizationVatRegNo - nvarchar(20)
           org.BIN,                                                                                                    -- OrganizationBin - nvarchar(20)
           org.Address,                                                                                                -- OrganizationAddress - nvarchar(200)
           bsnsNature.NameInBangla,                                                                                    -- OrganizationTypeOfBusiness - nvarchar(50)
           finActNat.NameInBangla,                                                                                     -- OrganizationTypeOfFinancialActivity - nvarchar(50)
           @TermOfTax,                                                                                                 -- TermOfTax - nvarchar(80)
           @TypeOfSubmission,                                                                                          -- TypeOfSubmission - nvarchar(100)
           @isTransactionOccuredInLastTerm,                                                                            -- IsOperatedInLastTerm - bit
           @isTransactionOccuredInLastTermInWords,                                                                     -- IsOperatedInLastTermInWords - nvarchar(10)
           mg.SubmissionDate,                                                                                          -- DateOfSubmission - datetime
           ISNULL(@DirectExportAmount, 0),                                                                             -- DirectExportAmount - decimal(18, 2)
           ISNULL(@IndirectExportAmount, 0),                                                                           -- IndirectExportAmount - decimal(18, 2)
           ISNULL(@VatExemptedProdSellAmount, 0),                                                                      -- VatExemptedProdSellAmount - decimal(18, 2)
           ISNULL(@StandardVatRateProdSellAmount, 0),                                                                  -- StandardVatRateProdSellAmount - decimal(18, 2)
           ISNULL(@StandardVatRateProdSellSdAmount, 0),                                                                -- StandardVatRateProdSellSdAmount - decimal(18, 2)
           ISNULL(@StandardVatRateProdSellVatAmount, 0),                                                               -- StandardVatRateProdSellVatAmount - decimal(18, 2)
           ISNULL(@MrpProdSellAmount, 0),                                                                              -- MrpProdSellAmount - decimal(18, 2)
           ISNULL(@MrpProdSellSdAmount, 0),                                                                            -- MrpProdSellSdAmount - decimal(18, 2)
           ISNULL(@MrpProdSellVatAmount, 0),                                                                           -- MrpProdSellVatAmount - decimal(18, 2)
           ISNULL(@FixedVatProdSellAmount, 0),                                                                         -- FixedVatProdSellAmount - decimal(18, 2)
           ISNULL(@FixedVatProdSellSdAmount, 0),                                                                       -- FixedVatProdSellSdAmount - decimal(18, 2)
           ISNULL(@FixedVatProdSellVatAmount, 0),                                                                      -- FixedVatProdSellVatAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatRateProdSellAmount, 0),                                                         -- OtherThanStandardVatRateProdSellAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatRateProdSellSdAmount, 0),                                                       -- OtherThanStandardVatRateProdSellSdAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatRateProdSellVatAmount, 0),                                                      -- OtherThanStandardVatRateProdSellVatAmount - decimal(18, 2)
           ISNULL(@TradingSellAmount, 0),                                                                              -- TradingSellAmount - decimal(18, 2)
           ISNULL(@TradingSellSdAmount, 0),                                                                            -- TradingSellSdAmount - decimal(18, 2)
           ISNULL(@TradingSellVatAmount, 0),                                                                           -- TradingSellVatAmount - decimal(18, 2)
           ISNULL(@TotalAmount, 0),                                                                                    -- TotalAmount - decimal(18, 2)
           ISNULL(@TotalSdAmount, 0),                                                                                  -- TotalSdAmount - decimal(18, 2)
           ISNULL(@TotalVatAmount, 0),                                                                                 -- TotalVatAmount - decimal(18, 2)
           ISNULL(@ZeroVatProdLocalPurchaseAmount, 0),                                                                 -- ZeroVatProdLocalPurchaseAmount - decimal(18, 2)
           ISNULL(@ZeroVatProdImportAmount, 0),                                                                        -- ZeroVatProdImportAmount - decimal(18, 2)
           ISNULL(@VatExemptedProdLocalPurchaseAmount, 0),                                                             -- VatExemptedProdLocalPurchaseAmount - decimal(18, 2)
           ISNULL(@VatExemptedProdImportAmount, 0),                                                                    -- VatExemptedProdImportAmount - decimal(18, 2)
           ISNULL(@StandardVatProdLocalPurchaseAmount, 0),                                                             -- StandardVatProdLocalPurchaseAmount - decimal(18, 2)
           ISNULL(@StandardVatProdLocalPurchaseVatAmount, 0),                                                          -- StandardVatProdLocalPurchaseVatAmount - decimal(18, 2)
           ISNULL(@StandardVatProdImportAmount, 0),                                                                    -- StandardVatProdImportAmount - decimal(18, 2)
           ISNULL(@StandardVatProdImportVatAmount, 0),                                                                 -- StandardVatProdImportVatAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatProdLocalPurchaseAmount, 0),                                                    -- OtherThanStandardVatProdLocalPurchaseAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatProdLocalPurchaseVatAmount, 0),                                                 -- OtherThanStandardVatProdLocalPurchaseVatAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatProdImportAmount, 0),                                                           -- OtherThanStandardVatProdImportAmount - decimal(18, 2)
           ISNULL(@OtherThanStandardVatProdImportVatAmount, 0),                                                        -- OtherThanStandardVatProdImportVatAmount - decimal(18, 2)
           ISNULL(@FixedVatProdLocalPurchaseAmount, 0),                                                                -- FixedVatProdLocalPurchaseAmount - decimal(18, 2)
           ISNULL(@FixedVatProdLocalPurchaseVatAmount, 0),                                                             -- FixedVatProdLocalPurchaseVatAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgAmount, 0),                                             -- NonRebatableProdLocalPurchaseFromTurnOverOrgAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount, 0),                                          -- NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgAmount, 0),                                               -- NonRebatableProdLocalPurchaseFromNonRegOrgAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount, 0),                                            -- NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount, 0),                        -- NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount, 0),                     -- NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount, 0),                               -- NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount - decimal(18, 2)
           ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount, 0),                            -- NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount - decimal(18, 2)
           ISNULL(@TotalRawMaterialPurchaseAmount, 0),                                                                 -- TotalRawMaterialPurchaseAmount - decimal(18, 2) 
           ISNULL(@TotalRawMaterialRebateAmount, 0),                                                                   -- TotalRawMaterialRebateAmount - decimal(18, 2) 
           ISNULL(@IncrementalAdjustmentAmountForVdsSale, 0),                                                          -- IncrementalAdjustmentAmountForVdsSale - decimal(18, 2)
           ISNULL(@IncrementalAdjustmentAmountForNotPaidInBankingChannel, 0),                                          -- IncrementalAdjustmentAmountForNotPaidInBankingChannel - decimal(18, 2)
           ISNULL(@IncrementalAdjustmentVatAmountForDebitNote, 0),                                                     -- IncrementalAdjustmentAmountForDebitNote - decimal(18, 2)
           ISNULL(mg.MiscIncrementalAdjustmentAmount, 0),                                                              -- IncrementalAdjustmentAmountForOtherReason - decimal(18, 2)
           ISNULL(mg.MiscIncrementalAdjustmentDesc, N''),                                                              -- IncrementalAdjustmentDescriptionForOtherReason - nvarchar(200)
           ISNULL(@TotalIncrementalAdjustmentVatAmountWithoutMisc, 0) + ISNULL(mg.MiscIncrementalAdjustmentAmount, 0), -- TotalIncrementalAdjustmentAmount - decimal(18, 2)
           ISNULL(@DecrementalAdjustmentAmountForVdsPurchase, 0),                                                      -- DecrementalAdjustmentAmountForVdsPurchase - decimal(18, 2)
           ISNULL(@DecrementalAdjustmentAmountForAdvanceTaxInImport, 0),                                               -- DecrementalAdjustmentAmountForAdvanceTaxInImport - decimal(18, 2)
           ISNULL(@DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd, 0),                           -- DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd - decimal(18, 2)
           ISNULL(@DecrementalAdjustmentVatAmountForCreditNote, 0),                                                    -- DecrementalAdjustmentAmountForCreditNote - decimal(18, 2)
           ISNULL(mg.MiscDecrementalAdjustmentAmount, 0),                                                              -- DecrementalAdjustmentAmountForOtherReason - decimal(18, 2)
           ISNULL(mg.MiscDecrementalAdjustmentDesc, N''),                                                              -- DecrementalAdjustmentDescriptionForOtherReason - nvarchar(200)
           ISNULL(@TotalDecrementalAdjustmentVatAmountWithoutMisc, 0) + ISNULL(mg.MiscDecrementalAdjustmentAmount, 0), -- TotalDecrementalAdjustmentAmount - decimal(18, 2)
           ISNULL(@TotalVatAmount, 0) - ISNULL(@TotalRawMaterialRebateAmount, 0)
           + (ISNULL(@TotalIncrementalAdjustmentVatAmountWithoutMisc, 0)
              + ISNULL(mg.MiscIncrementalAdjustmentAmount, 0)
             )
           - (ISNULL(@TotalDecrementalAdjustmentVatAmountWithoutMisc, 0)
              + ISNULL(mg.MiscDecrementalAdjustmentAmount, 0)
             ),                                                                                                        -- NetTaxTotalPayableVatAmountInCurrentTaxTerm - decimal(18, 2)
           ISNULL(@TotalVatAmount, 0) - ISNULL(@TotalRawMaterialRebateAmount, 0)
           + (ISNULL(@TotalIncrementalAdjustmentVatAmountWithoutMisc, 0)
              + ISNULL(mg.MiscIncrementalAdjustmentAmount, 0)
             )
           - (ISNULL(@TotalDecrementalAdjustmentVatAmountWithoutMisc, 0)
              + ISNULL(mg.MiscDecrementalAdjustmentAmount, 0)
             ) - ISNULL(mg.LastClosingVatAmount, 0),                                                                   -- NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance - decimal(18, 2)
           @NetTaxTotalPayableSdAmountInCurrentTaxTerm,                                                                -- NetTaxTotalPayableSdAmountInCurrentTaxTerm - decimal(18, 2)
           @NetTaxTotalPayableSdAmountInCurrentTaxTerm - ISNULL(mg.LastClosingSuppDutyAmount, 0),                      -- NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance - decimal(18, 2)
           ISNULL(@NetTaxSdAmountForDebitNote, 0),                                                                     -- NetTaxSdAmountForDebitNote - decimal(18, 2)
           ISNULL(@NetTaxSdAmountForCreditNote, 0),                                                                    -- NetTaxSdAmountForCreditNote - decimal(18, 2)
           ISNULL(@NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd, 0),                                    -- NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd - decimal(18, 2)
           mg.InterestForDueVat,                                                                                       -- NetTaxInterstAmountForDeuVat - decimal(18, 2)
           mg.InterestForDueSuppDuty,                                                                                  -- NetTaxInterstAmountForDeuSd - decimal(18, 2)
           mg.FinancialPenalty,                                                                                        -- NetTaxFineAndPenaltyAmount - decimal(18, 2)
           mg.ExciseDuty,                                                                                              -- NetTaxExciseDuty - decimal(18, 2)
           mg.DevelopmentSurcharge,                                                                                    -- NetTaxDevelopmentSurcharge - decimal(18, 2)
           mg.ItDevelopmentSurcharge,                                                                                  -- NetTaxInformationTechnologyDevelopmentSurcharge - decimal(18, 2)
           mg.HealthDevelopmentSurcharge,                                                                              -- NetTaxHealthProtectionSurcharge - decimal(18, 2)
           mg.EnvironmentProtectSurcharge,                                                                             -- NetTaxEnvironmentProtectionSurcharge - decimal(18, 2)
           mg.LastClosingVatAmount,                                                                                    -- NetTaxVatEndingBalanceOfLastTerm - decimal(18, 2)
           mg.LastClosingSuppDutyAmount,                                                                               -- NetTaxSdEndingBalanceOfLastTerm - decimal(18, 2)
           mg.PaidVatAmount,                                                                                           -- TreasurySubmissionAmountForCurrentTermVat - decimal(18, 2)
           necVat.EconomicCode,                                                                                        -- TreasurySubmissionEconomicCodeForCurrentTermVat - nvarchar(20)
           mg.PaidSuppDutyAmount,                                                                                      -- TreasurySubmissionAmountForCurrentTermSd - decimal(18, 2)
           necSd.EconomicCode,                                                                                         -- TreasurySubmissionEconomicCodeForCurrentTermSd - nvarchar(20)
           mg.PaidInterestAmountForDueVat,                                                                             -- TreasurySubmissionAmountForInterestOfDueVat - decimal(18, 2)
           necIfdv.EconomicCode,                                                                                       -- TreasurySubmissionEconomicCodeForInterestOfDueVat - nvarchar(20)
           mg.PaidInterestAmountForDueSuppDuty,                                                                        -- TreasurySubmissionAmountForInterestOfDueSd - decimal(18, 2)
           necIfdsd.EconomicCode,                                                                                      -- TreasurySubmissionEconomicCodeForInterestOfDueSd - nvarchar(20)
           mg.PaidFinancialPenalty,                                                                                    -- TreasurySubmissionAmountForFineAndPenalty - decimal(18, 2)
           necFp.EconomicCode,                                                                                         -- TreasurySubmissionEconomicCodeForFineAndPenalty - nvarchar(20)
           mg.PaidExciseDuty,                                                                                          -- TreasurySubmissionAmountForExciseDuty - decimal(18, 2)
           necEd.EconomicCode,                                                                                         -- TreasurySubmissionEconomicCodeForExciseDuty - nvarchar(20)
           mg.PaidDevelopmentSurcharge,                                                                                -- TreasurySubmissionAmountForDevelopmentSurcharge - decimal(18, 2)
           necDs.EconomicCode,                                                                                         -- TreasurySubmissionEconomicCodetForDevelopmentSurcharge - nvarchar(20)
           mg.PaidItDevelopmentSurcharge,                                                                              -- TreasurySubmissionAmountForInformationTechnologyDevelopmentSurcharge - decimal(18, 2)
           necIds.EconomicCode,                                                                                        -- TreasurySubmissionEconomicCodeForInformationTechnologyDevelopmentSurcharge - nvarchar(20)
           mg.PaidHealthDevelopmentSurcharge,                                                                          -- TreasurySubmissionAmountForHealthProtectionSurcharge - decimal(18, 2)
           necHds.EconomicCode,                                                                                        -- TreasurySubmissionEconomicCodeForHealthProtectionSurcharge - nvarchar(20)
           mg.PaidEnvironmentProtectSurcharge,                                                                         -- TreasurySubmissionAmountForEnvironmentProtectionSurcharge - decimal(18, 2)
           necEps.EconomicCode,                                                                                        -- TreasurySubmissionEconomicCodeForEnvironmentProtectionSurcharge - nvarchar(20)
           ISNULL(mg.PaidVatAmount, 0)
           - (ISNULL(@TotalVatAmount, 0) - ISNULL(@TotalRawMaterialRebateAmount, 0)
              + (ISNULL(@TotalIncrementalAdjustmentVatAmountWithoutMisc, 0)
                 + ISNULL(mg.MiscIncrementalAdjustmentAmount, 0)
                )
              - (ISNULL(@TotalDecrementalAdjustmentVatAmountWithoutMisc, 0)
                 + ISNULL(mg.MiscDecrementalAdjustmentAmount, 0)
                ) - ISNULL(mg.LastClosingVatAmount, 0)
             ),                                                                                                        -- VatEndingBalance - decimal(18, 2)
           ISNULL(mg.PaidSuppDutyAmount, 0)
           - (ISNULL(@NetTaxTotalPayableSdAmountInCurrentTaxTerm, 0) - ISNULL(mg.LastClosingSuppDutyAmount, 0)),       -- SdEndingBalance - decimal(18, 2)
           mg.IsWantToGetBackClosingAmount,                                                                            -- IsWantToGetReturnAmountInEndingBalance - bit
           CASE
               WHEN mg.IsWantToGetBackClosingAmount = 1 THEN
                   N'হ্যাঁ'
               ELSE
                   N'না'
           END,                                                                                                        -- IsWantToGetReturnAmountInEndingBalanceInWords - nvarchar(10)
           org.VatResponsiblePersonName,                                                                               -- VatResponsiblePersonName - nvarchar(100)
           org.VatResponsiblePersonDesignation,                                                                        -- VatResponsiblePersonDesignation - nvarchar(100)
           mg.SubmissionDate,                                                                                          -- DateOfSignatureInSubmission - datetime
           org.VatResponsiblePersonMobileNo,                                                                           -- VatResponsiblePersonMobileNo - nvarchar(20)
           org.VatResponsiblePersonEmailAddress                                                                        -- VatResponsiblePersonEmail - nvarchar(100)
    FROM dbo.MushakGeneration mg
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = mg.OrganizationId
        INNER JOIN dbo.FinancialActivityNature finActNat
            ON finActNat.FinancialActivityNatureId = org.FinancialActivityNatureId
        INNER JOIN dbo.BusinessNature bsnsNature
            ON bsnsNature.BusinessNatureId = org.BusinessNatureId
        LEFT JOIN dbo.NbrEconomicCode necVat
            ON necVat.NbrEconomicCodeId = mg.VatEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necSd
            ON necSd.NbrEconomicCodeId = mg.SuppDutyEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necIfdv
            ON necIfdv.NbrEconomicCodeId = mg.InterestForDueVatEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necIfdsd
            ON necIfdsd.NbrEconomicCodeId = mg.InterestForDueSuppDutyEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necFp
            ON necFp.NbrEconomicCodeId = mg.FinancialPenaltyEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necEd
            ON necEd.NbrEconomicCodeId = mg.ExciseDutyEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necDs
            ON necDs.NbrEconomicCodeId = mg.DevelopmentSurchargeEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necIds
            ON necIds.NbrEconomicCodeId = mg.ItDevelopmentSurchargeEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necHds
            ON necHds.NbrEconomicCodeId = mg.HealthDevelopmentSurchargeEconomicCodeId
        LEFT JOIN dbo.NbrEconomicCode necEps
            ON necEps.NbrEconomicCodeId = mg.EnvironmentProtectSurchargeEconomicCodeId
    WHERE mg.MushakGenerationId = @MushakGenerationId;

    --Return Summerized Information
    SELECT mr.MushakReturnId,
           mr.MushakGenerationId,
           mr.OrganizationId,
           mr.OrganizationName,
           mr.OrganizationVatRegNo,
           mr.OrganizationBin,
           mr.OrganizationAddress,
           mr.OrganizationTypeOfBusiness,
           mr.OrganizationTypeOfFinancialActivity,
           mr.TermOfTax,
           mr.TypeOfSubmission,
           mr.IsOperatedInLastTerm,
           mr.IsOperatedInLastTermInWords,
           mr.DateOfSubmission,
           mr.DirectExportAmount,
           mr.IndirectExportAmount,
           mr.VatExemptedProdSellAmount,
           mr.StandardVatRateProdSellAmount,
           mr.StandardVatRateProdSellSdAmount,
           mr.StandardVatRateProdSellVatAmount,
           mr.MrpProdSellAmount,
           mr.MrpProdSellSdAmount,
           mr.MrpProdSellVatAmount,
           mr.FixedVatProdSellAmount,
           mr.FixedVatProdSellSdAmount,
           mr.FixedVatProdSellVatAmount,
           mr.OtherThanStandardVatRateProdSellAmount,
           mr.OtherThanStandardVatRateProdSellSdAmount,
           mr.OtherThanStandardVatRateProdSellVatAmount,
           mr.TradingSellAmount,
           mr.TradingSellSdAmount,
           mr.TradingSellVatAmount,
           mr.TotalAmount,
           mr.TotalSdAmount,
           mr.TotalVatAmount,
           mr.ZeroVatProdLocalPurchaseAmount,
           mr.ZeroVatProdImportAmount,
           mr.VatExemptedProdLocalPurchaseAmount,
           mr.VatExemptedProdImportAmount,
           mr.StandardVatProdLocalPurchaseAmount,
           mr.StandardVatProdLocalPurchaseVatAmount,
           mr.StandardVatProdImportAmount,
           mr.StandardVatProdImportVatAmount,
           mr.OtherThanStandardVatProdLocalPurchaseAmount,
           mr.OtherThanStandardVatProdLocalPurchaseVatAmount,
           mr.OtherThanStandardVatProdImportAmount,
           mr.OtherThanStandardVatProdImportVatAmount,
           mr.FixedVatProdLocalPurchaseAmount,
           mr.FixedVatProdLocalPurchaseVatAmount,
           mr.NonRebatableProdLocalPurchaseFromTurnOverOrgAmount,
           mr.NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount,
           mr.NonRebatableProdLocalPurchaseFromNonRegOrgAmount,
           mr.NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount,
           mr.NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount,
           mr.NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount,
           mr.NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount,
           mr.NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount,
           mr.TotalRawMaterialPurchaseAmount,
           mr.TotalRawMaterialRebateAmount,
           mr.IncrementalAdjustmentAmountForVdsSale,
           mr.IncrementalAdjustmentAmountForNotPaidInBankingChannel,
           mr.IncrementalAdjustmentAmountForDebitNote,
           mr.IncrementalAdjustmentAmountForOtherReason,
           mr.IncrementalAdjustmentDescriptionForOtherReason,
           mr.TotalIncrementalAdjustmentAmount,
           mr.DecrementalAdjustmentAmountForVdsPurchase,
           mr.DecrementalAdjustmentAmountForAdvanceTaxInImport,
           mr.DecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd,
           mr.DecrementalAdjustmentAmountForCreditNote,
           mr.DecrementalAdjustmentAmountForOtherReason,
           mr.DecrementalAdjustmentDescriptionForOtherReason,
           mr.TotalDecrementalAdjustmentAmount,
           mr.NetTaxTotalPayableVatAmountInCurrentTaxTerm,
           mr.NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance,
           mr.NetTaxTotalPayableSdAmountInCurrentTaxTerm,
           mr.NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance,
           mr.NetTaxSdAmountForDebitNote,
           mr.NetTaxSdAmountForCreditNote,
           mr.NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd,
           mr.NetTaxInterstAmountForDeuVat,
           mr.NetTaxInterstAmountForDeuSd,
           mr.NetTaxFineAndPenaltyAmount,
           mr.NetTaxExciseDuty,
           mr.NetTaxDevelopmentSurcharge,
           mr.NetTaxInformationTechnologyDevelopmentSurcharge,
           mr.NetTaxHealthProtectionSurcharge,
           mr.NetTaxEnvironmentProtectionSurcharge,
           mr.NetTaxVatEndingBalanceOfLastTerm,
           mr.NetTaxSdEndingBalanceOfLastTerm,
           mr.TreasurySubmissionAmountForCurrentTermVat,
           mr.TreasurySubmissionEconomicCodeForCurrentTermVat,
           mr.TreasurySubmissionAmountForCurrentTermSd,
           mr.TreasurySubmissionEconomicCodeForCurrentTermSd,
           mr.TreasurySubmissionAmountForInterestOfDueVat,
           mr.TreasurySubmissionEconomicCodeForInterestOfDueVat,
           mr.TreasurySubmissionAmountForInterestOfDueSd,
           mr.TreasurySubmissionEconomicCodeForInterestOfDueSd,
           mr.TreasurySubmissionAmountForFineAndPenalty,
           mr.TreasurySubmissionEconomicCodeForFineAndPenalty,
           mr.TreasurySubmissionAmountForExciseDuty,
           mr.TreasurySubmissionEconomicCodeForExciseDuty,
           mr.TreasurySubmissionAmountForDevelopmentSurcharge,
           mr.TreasurySubmissionEconomicCodetForDevelopmentSurcharge,
           mr.TreasurySubmissionAmountForInformationTechnologyDevelopmentSurcharge,
           mr.TreasurySubmissionEconomicCodeForInformationTechnologyDevelopmentSurcharge,
           mr.TreasurySubmissionAmountForHealthProtectionSurcharge,
           mr.TreasurySubmissionEconomicCodeForHealthProtectionSurcharge,
           mr.TreasurySubmissionAmountForEnvironmentProtectionSurcharge,
           mr.TreasurySubmissionEconomicCodeForEnvironmentProtectionSurcharge,
           mr.VatEndingBalance,
           mr.SdEndingBalance,
           mr.IsWantToGetReturnAmountInEndingBalance,
           mr.IsWantToGetReturnAmountInEndingBalanceInWords,
           mr.VatResponsiblePersonName,
           mr.VatResponsiblePersonDesignation,
           mr.DateOfSignatureInSubmission,
           mr.VatResponsiblePersonMobileNo,
           mr.VatResponsiblePersonEmail
    FROM @MushakReturn mr;

    --Subform - ka
    SELECT CASE
               WHEN sls.SalesTypeId = @salesTypeExport THEN
                   N'সাবফর্ম- ক(নোট ১,২ এর জন্য প্রযোজ্য)'
               WHEN sls.SalesTypeId = @salesTypeLocal
                    AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ৩ এর জন্য প্রযোজ্য)'
               WHEN sls.SalesTypeId = @salesTypeLocal
                    AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ৪ এর জন্য প্রযোজ্য)'
               WHEN sls.SalesTypeId = @salesTypeLocal
                    AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ৫ এর জন্য প্রযোজ্য)'
               ELSE
                   N'সাবফর্ম- ক(নোট ৭ এর জন্য প্রযোজ্য)'
           END AS SubFormName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100 AS ProductSupplementaryDuty,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity * slsDtl.VATPercent / 100 AS ProductVat,
           N'সরবরাহ' AS Remarks --INTO SpGetMushakNinePointOneSubFormKa
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND
          (
              sls.SalesTypeId = @salesTypeExport
              OR
              (
                  sls.SalesTypeId = @salesTypeLocal
                  AND slsDtl.ProductVATTypeId IN ( @vatExemptedProdVatTypeId, @standardVatProdVatTypeId,
                                                   @mrpProdVatTypeId, @otherThanStandardVat1p5ProdVatTypeId,
                                                   @otherThanStandardVat2p0ProdVatTypeId,
                                                   @otherThanStandardVat2p5ProdVatTypeId,
                                                   @otherThanStandardVat4p0ProdVatTypeId,
                                                   @otherThanStandardVat5p0ProdVatTypeId,
                                                   @otherThanStandardVat5p5ProdVatTypeId,
                                                   @otherThanStandardVat6p0ProdVatTypeId,
                                                   @otherThanStandardVat7p5ProdVatTypeId,
                                                   @otherThanStandardVat9p0ProdVatTypeId,
                                                   @otherThanStandardVat10p0ProdVatTypeId
                                                 )
              )
          )
    UNION ALL
    SELECT CASE
               WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ১০,১১ এর জন্য প্রযোজ্য)'
               WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ১২,১৩ এর জন্য প্রযোজ্য)'
               WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ১৪, ১৫ এর জন্য প্রযোজ্য)'
               WHEN purchDtl.ProductVATTypeId = @otherThanStandardVat2p0ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat2p5ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat4p0ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat5p0ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat5p5ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat6p0ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat7p5ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat9p0ProdVatTypeId
                    OR purchDtl.ProductVATTypeId = @otherThanStandardVat10p0ProdVatTypeId THEN
                   N'সাবফর্ম- ক(নোট ১৬,১৭ এর জন্য প্রযোজ্য)'
               WHEN prod.IsNonRebateable = 1
                    AND org.IsSellStandardVatProduct = 1 THEN
                   N'সাবফর্ম- ক(নোট ১৯,২০ এর জন্য প্রযোজ্য)'
               ELSE
                   N'সাবফর্ম- ক(নোট ২১,২২ এর জন্য প্রযোজ্য)'
           END AS SubFormName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity AS ProductPrice,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity * purchDtl.SupplementaryDutyPercent
           / 100 AS ProductSupplementaryDuty,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity * purchDtl.VATPercent / 100 AS ProductVat,
           N'ক্রয়' AS Remarks
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = purch.OrganizationId
    WHERE purch.MushakGenerationId = @MushakGenerationId;

    --Subform - kha
    SELECT busnsCat.NameInBangla AS BusinessCategoryName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100 AS ProductSupplementaryDuty,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity * slsDtl.VATPercent / 100 AS ProductVat,
           N'সরবরাহ' AS Remarks --INTO SpGetMushakNinePointOneSubFormKha
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.BusinessCategory busnsCat
            ON busnsCat.BusinessCategoryId = org.BusinessCategoryId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND org.FinancialActivityNatureId IN ( @financialActivityRetailer, @financialActivityWholesaler,
                                                 @financialActivityRetailerAndWholesaler
                                               );

    --Subform - ga
    SELECT N'সাবফর্ম- গ(নোট ৬ এর জন্য প্রযোজ্য)' AS SubFormName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           mu.Name AS MeasurementUnitName,
           slsDtl.Quantity,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100 AS ProductSupplementaryDuty,
           slsDtl.Quantity * slsDtl.VATPercent / 100 AS ProductVat,
           N'সরবরাহ' AS Remarks --INTO SpGetMushakNinePointOneSubFormGa
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = slsDtl.MeasurementUnitId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId
    UNION ALL
    SELECT N'সাবফর্ম- গ(নোট ১৮ এর জন্য প্রযোজ্য)' AS SubFormName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           mu.Name AS MeasurementUnitName,
           purchDtl.Quantity,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity AS ProductPrice,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity * purchDtl.SupplementaryDutyPercent
           / 100 AS ProductSupplementaryDuty,
           purchDtl.Quantity * purchDtl.VATPercent / 100 AS ProductVat,
           N'ক্রয়' AS Remarks
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = purchDtl.MeasurementUnitId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId;

    --SubForm - gha
    SELECT vndr.BinNo AS VendorBinNo,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity) AS Price,
           purch.VDSAmount AS DeductedVat,
           purch.InvoiceNo,
           purch.VatChallanNo,
           purch.PurchaseDate,
           purch.VatChallanIssueDate AS TaxInvoicePrintedTime, --TaxInvoicePrintedTime
           nec.EconomicCode                                    --INTO SpGetMushakNinePointOneSubFormGha
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
        INNER JOIN dbo.MushakGeneration mg
            ON mg.MushakGenerationId = purch.MushakGenerationId
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = mg.VatEconomicCodeId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.IsVatDeductedInSource = 1
    GROUP BY vndr.BinNo,
             vndr.Name,
             vndr.Address,
             purch.VDSAmount,
             purch.InvoiceNo,
             purch.VatChallanNo,
             purch.PurchaseDate,
             purch.VatChallanIssueDate,
             nec.EconomicCode;

    --SubForm - uma
    SELECT cust.BIN AS CustomerBinNo,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           SUM((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) AS Price,
           sls.VDSAmount AS DeductedVat,
           sls.InvoiceNo,
           sls.VatChallanNo,
           sls.SalesDate,
           sls.TaxInvoicePrintedTime,
           EconomicCode = sls.VDSPaymentEconomicCode --INTO SpGetMushakNinePointOneSubFormUma
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.MushakGenerationId = @MushakGenerationId
          AND sls.IsVatDeductedInSource = 1
    GROUP BY cust.BIN,
             cust.Name,
             cust.Address,
             sls.VDSAmount,
             sls.InvoiceNo,
             sls.VatChallanNo,
             sls.SalesDate,
             sls.TaxInvoicePrintedTime,
             sls.VDSPaymentEconomicCode;

    --SubForm - cha
    SELECT purch.BillOfEntry AS BillOfEntryNo,
           purch.BillOfEntryDate,
           cavatc.Name AS CustomsAndVATCommissionarateName,
           cavatc.NameInBangla AS CustomsAndVATCommissionarateNameInBangla,
           SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity * purchDtl.AdvanceTaxPercent / 100) AS AdvanceTaxAmount,
           purch.AdvanceTaxPaidAmount --INTO SpGetMushakNinePointOneSubFormCha
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.CustomsAndVATCommissionarate cavatc
            ON cavatc.CustomsAndVATCommissionarateId = purch.CustomsAndVATCommissionarateId
    WHERE purch.MushakGenerationId = @MushakGenerationId
          AND purch.IsVatDeductedInSource = 1
    GROUP BY purch.BillOfEntry,
             purch.BillOfEntryDate,
             cavatc.Name,
             cavatc.NameInBangla,
             purch.AdvanceTaxPaidAmount;

    --Subform - chha
    SELECT CASE
               WHEN pInfo.MushakGenerationInfoId = 1 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৩ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 2 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৪ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 3 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৫ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 4 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৬ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 5 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৭ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 6 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৮ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 7 THEN
                   N'সাবফর্ম- ছ(নোট  ৫৯ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 8 THEN
                   N'সাবফর্ম- ছ(নোট  ৬০ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 9 THEN
                   N'সাবফর্ম- ছ(নোট  ৬১ এর জন্য  প্রযোজ্য)'
               WHEN pInfo.MushakGenerationInfoId = 10 THEN
                   N'সাবফর্ম- ছ(নোট  ৬২ এর জন্য  প্রযোজ্য)'
               ELSE
                   N''
           END AS SubFormName,
           pInfo.MushakGenerationInfoId,
           pInfo.MushakGenerationId,
           pInfo.OrganizationId,
           pInfo.ChallanNo,
           BankName = bnk.NameInBangla,
           BankBranchName = bnkBrnch.NameInBangla,
           pInfo.PaymentDate,
           nec.EconomicCode,
           pInfo.PaidAmount,
           PaidAmountInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(ISNULL(pInfo.PaidAmount, 0))
    FROM dbo.FnGetMushakGenerationDepositInfo(@MushakGenerationId) pInfo
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = pInfo.EconomicCodeId
        INNER JOIN dbo.BankBranch bnkBrnch
            ON bnkBrnch.BankBranchId = pInfo.BankBranchId
        INNER JOIN dbo.Bank bnk
            ON bnk.BankId = bnkBrnch.BankId
    ORDER BY pInfo.MushakGenerationInfoId;

END;
