-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- Test Execution: SpGetMushakReturn 5, 2020, 1
-- =============================================
CREATE PROCEDURE [dbo].[SpGetMushakReturn]
    @OrganizationId INT,
    @Year INT,
    @Month INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;


    -- Insert statements for procedure here
    DECLARE @nextMonth INT = @Month + 1,
            @nextMonthYear INT = @Year;

    IF @Month >= 12
    BEGIN
        SET @nextMonth = 1;
        SET @nextMonthYear = @Year + 1;
    END;

    DECLARE @firstDayOfMonth DATETIME
        = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01',
            @firstDayOfNextMonth DATETIME = CAST(@nextMonthYear AS VARCHAR(4)) + '-' + CAST(@nextMonth AS VARCHAR(2))
                                            + '-01';


    DECLARE @isTransactionOccuredInLastTerm BIT = 0,
            @isTransactionOccuredInLastTermInWords NVARCHAR(10) = N'না',
            @isTransactionOccuredInLastTermInWordsEng NVARCHAR(10) = N'No',
            @isWantToGetBackClosingBalance BIT = 0,
            @isWantToGetBackClosingBalanceInWords NVARCHAR(10) = N'না',
            @isWantToGetBackClosingBalanceInWordsEng NVARCHAR(10) = N'No',
            @TermOfTax NVARCHAR(80),
            @TermOfTaxEng NVARCHAR(80),
            @TypeOfSubmission NVARCHAR(50) = N'মূল দাখিলপত্র',
            @TypeOfSubmissionEng NVARCHAR(50) = N'Main/Original Return (Section 64)';

    SELECT @TermOfTax
        = dbo.FnGetNameOfMonthInBanglaByNumber(@Month) + N' / ' + dbo.FnConvertIntToBanglaUnicodeNumber(@Year),
           @TermOfTaxEng = DATENAME(MONTH, DATEADD(MONTH, @Month, 0) - 1) + N' / ' + CAST(@Year AS NVARCHAR(10));

    IF EXISTS
    (
        SELECT 1
        FROM dbo.ProductTransactionBook ptb
        WHERE ptb.TransactionTime >= @firstDayOfMonth
              AND ptb.TransactionTime < @firstDayOfNextMonth
    )
    BEGIN
        SET @isTransactionOccuredInLastTerm = 1;
        SELECT @isTransactionOccuredInLastTermInWords = N'হ্যাঁ',
               @isTransactionOccuredInLastTermInWordsEng = N'Yes';
    END;

    --===========================================================
    DECLARE @financialActivitImporter INT = 1,
            @financialActivityExporter INT = 2,
            @financialActivityManufacturer INT = 3,
            @financialActivityRetailer INT = 4,
            @financialActivityWholesaler INT = 5,
            @financialActivityRetailerAndWholesaler INT = 6;
    --===========================================================

    --===========================================================
    DECLARE @adjustmentTypeIncremental INT = 1,
            @adjustmentTypeDecremental INT = 2;
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
    DECLARE @paymentMethodCash INT = 1;
    --===========================================================
    --===========================================================

    --===========================================================
    DECLARE @upperLimitForPaymentThroughCash DECIMAL(18, 2) = 100000;
    --===========================================================

    --===========================================================
    DECLARE @NetPayableVATForTreasurySubmission DECIMAL(18, 2) = 0,
            @NetPayableSDForTreasurySubmission DECIMAL(18, 2) = 0,
            @LastClosingVatAmount DECIMAL(18, 2) = 0,
            @LastClosingSuppDutyAmount DECIMAL(18, 2) = 0,
            @ClaimedRefundVatAmount DECIMAL(18, 2) = 0,
            @ClaimedRefundSDAmount DECIMAL(18, 2) = 0,
            @RefusedClaimedRefundVatAmount DECIMAL(18, 2) = 0,
            @RefusedClaimedRefundSDAmount DECIMAL(18, 2) = 0,
            @CurrentClosingVatAmount DECIMAL(18, 2) = 0,
            @CurrentClosingSuppDutyAmount DECIMAL(18, 2) = 0,
            @InterestForDueVat DECIMAL(18, 2) = 0,
            @InterestForDueSuppDuty DECIMAL(18, 2) = 0,
            @FineAndPenaltyForDelaySubmission DECIMAL(18, 2) = 0,
            @FineAndPenaltyOthers DECIMAL(18, 2) = 0,
            @ExciseDuty DECIMAL(18, 2) = 0,
            @DevelopmentSurcharge DECIMAL(18, 2) = 0,
            @ItDevelopmentSurcharge DECIMAL(18, 2) = 0,
            @HealthCareSurcharge DECIMAL(18, 2) = 0,
            @EnvironmentProtectionSurcharge DECIMAL(18, 2) = 0,
            @RemainingVATAmountFromMushak18p6 DECIMAL(18, 2) = 0,
            @RemainingSDAmountFromMushak18p6 DECIMAL(18, 2) = 0,
            @AdjustableAmountFromNote54 DECIMAL(18, 2) = 0,
            @AdjustableAmountFromNote55 DECIMAL(18, 2) = 0,
            @TotalVatPaidAmount DECIMAL(18, 2) = 0,
            @TotalSdPaidAmount DECIMAL(18, 2) = 0;
    --===========================================================

    --===========================================================
    DECLARE @zeroVatProdVatTypeId INT = 1,
            @vatExemptedProdVatTypeId INT = 2,
            @standardVatProdVatTypeId INT = 3,
            @mrpProdVatTypeId INT = 4,
            @notAdmissibleForCreditProdVatTypeId INT = 5,
            @fixedVatProdVatTypeId INT = 6,
            @otherThanStandardVatProdVatTypeId INT = 7,
            @retailOrWholesaleOrTradeProductVatTypeId INT = 8,
            @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId INT = 9;
    --===========================================================

    --===========================================================
    DECLARE @PaymentTypeVATDepositForTheTaxPeriod INT = 1,
            @PaymentTypeSDDepositForTheTaxPeriod INT = 2,
            @PaymentTypeInterestOnOverdueVAT INT = 3,
            @PaymentTypeInterestOnOverdueSD INT = 4,
            @PaymentTypeFineAndPenalty INT = 5,
            @PaymentTypeExciseDuty INT = 6,
            @PaymentTypeDevelopmentSurcharge INT = 7,
            @PaymentTypeICTDevelopmentSurcharge INT = 8,
            @PaymentTypeHealthCareSurcharge INT = 9,
            @PaymentTypeEnvironmentalProtectionSurcharge INT = 10;
    --===========================================================

    --===========================================================
    DECLARE @NBREconomicCodeForLocalProdVat INT = 13; --,
    --@PaymentTypeSDDepositForTheTaxPeriod INT = 2,
    --@PaymentTypeInterestOnOverdueVAT INT = 3,
    --@PaymentTypeInterestOnOverdueSD INT = 4,
    --@PaymentTypeFineAndPenalty INT = 5,
    --@PaymentTypeExciseDuty INT = 6,
    --@PaymentTypeDevelopmentSurcharge INT = 7,
    --@PaymentTypeICTDevelopmentSurcharge INT = 8,
    --@PaymentTypeHealthCareSurcharge INT = 9,
    --@PaymentTypeEnvironmentalProtectionSurcharge INT = 10;
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
            @IncrementalAdjustmentAmountForVdsPurchase DECIMAL(18, 2),
            @IncrementalAdjustmentAmountForNotPaidInBankingChannel DECIMAL(18, 2),
            @IncrementalAdjustmentVatAmountForDebitNote DECIMAL(18, 2),
            @MiscIncrementalAdjustmentAmount DECIMAL(18, 2),
            @MiscIncrementalAdjustmentDescription NVARCHAR(500),
            @TotalIncrementalAdjustmentVatAmount DECIMAL(18, 2),
            @DecrementalAdjustmentAmountForVdsSale DECIMAL(18, 2),
            @DecrementalAdjustmentAmountForAdvanceTaxInImport DECIMAL(18, 2),
            @DecrementalAdjustmentVatAmountForCreditNote DECIMAL(18, 2),
            @MiscDecrementalAdjustmentAmount DECIMAL(18, 2),
            @MiscDecrementalAdjustmentDescription NVARCHAR(500),
            @TotalDecrementalAdjustmentVatAmount DECIMAL(18, 2),
            @NetTaxTotalPayableVatAmountInCurrentTaxTerm DECIMAL(18, 2),
            @NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance DECIMAL(18, 2),
            @NetTaxTotalPayableSdAmountInCurrentTaxTerm DECIMAL(18, 2),
            @NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance DECIMAL(18, 2),
            @NetTaxSdAmountForDebitNote DECIMAL(18, 2),
            @NetTaxSdAmountForCreditNote DECIMAL(18, 2),
            @NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2),
            --Previous
            @TotalPreviousRawMaterialRebateAmount DECIMAL(18, 2),
            @TotalPreviousDecrementalAdjustmentAmountForVdsSale DECIMAL(18, 2),
            @TotalPreviousIncrementalAdjustmentAmountForNotPaidInBankingChannel DECIMAL(18, 2),
            @TotalPreviousIncrementalAdjustmentVatAmountForDebitNote DECIMAL(18, 2),
            @TotalPreviousMiscIncrementalAdjustmentAmount DECIMAL(18, 2),
            @TotalPreviousIncrementalAdjustmentAmountForVdsPurchase DECIMAL(18, 2),
            @TotalPreviousDecrementalAdjustmentAmountForAdvanceTaxInImport DECIMAL(18, 2),
            @TotalPreviousDecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd DECIMAL(18, 2),
            @TotalPreviousDecrementalAdjustmentVatAmountForCreditNote DECIMAL(18, 2),
            @TotalPreviousMiscDecrementalAdjustmentAmount DECIMAL(18, 2),
            @PreviousVatAmount DECIMAL(18, 2),
            @TotalPreviousVatAmount DECIMAL(18, 2),
            @TotalPreviousVatPaymentAmount DECIMAL(18, 2),
            @TotalPreviousSdAmount DECIMAL(18, 2),
            @TotalPreviousSdPaymentAmount DECIMAL(18, 2);

    --===========================================================


    --===========================================================
    --Direct Export
    SELECT @DirectExportAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeExport
          AND sls.ExportTypeId = @exportTypeDirect;
    --/Direct Export

    --InDirect Export
    SELECT @IndirectExportAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
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
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Sell Amount

    --Standard Vat Rate Product Sell
    SELECT @StandardVatRateProdSellAmount
        = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @StandardVatRateProdSellSdAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    ),
           @StandardVatRateProdSellVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputVat(
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               slsDtl.VATPercent,
                                                               slsDtl.SupplementaryDutyPercent
                                                           ),
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    --/Standard Vat Rate Product Sell

    --MRP Product Sell
    SELECT @MrpProdSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @MrpProdSellSdAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    ),
           @MrpProdSellVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputVat(
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               slsDtl.VATPercent,
                                                               slsDtl.SupplementaryDutyPercent
                                                           ),
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId;
    --/MRP Product Sell

    --Fixed Vat Product Sell
    SELECT @FixedVatProdSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @FixedVatProdSellSdAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    ),
           @FixedVatProdSellVatAmount = SUM(ISNULL(slsDtl.Quantity * slsDtl.VATPercent, 0))
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId;
    --/Fixed Vat Product Sell


    --Other Than Standard Vat Product Sell
    SELECT @OtherThanStandardVatRateProdSellAmount
        = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @OtherThanStandardVatRateProdSellSdAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    ),
           @OtherThanStandardVatRateProdSellVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputVat(
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               slsDtl.VATPercent,
                                                               slsDtl.SupplementaryDutyPercent
                                                           ),
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND slsDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId;
    --/Other Than Standard Vat Product Sell

    --Trading Sell
    SELECT @TradingSellAmount = SUM(ISNULL(((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity), 0)),
           @TradingSellSdAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    ),
           @TradingSellVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputVat(
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               slsDtl.VATPercent,
                                                               slsDtl.SupplementaryDutyPercent
                                                           ),
                               0
                           )
                    )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
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
          + ISNULL(@FixedVatProdSellSdAmount, 0) + ISNULL(@OtherThanStandardVatRateProdSellSdAmount, 0)
          + ISNULL(@TradingSellSdAmount, 0);
    --/Total SD on Sell
    --PRINT @TotalSdAmount
    --Total VAT on Sell
    SET @TotalVatAmount
        = ISNULL(@StandardVatRateProdSellVatAmount, 0) + ISNULL(@MrpProdSellVatAmount, 0)
          + ISNULL(@FixedVatProdSellVatAmount, 0) + ISNULL(@OtherThanStandardVatRateProdSellVatAmount, 0)
          + ISNULL(@TradingSellVatAmount, 0);
    --Total VAT on Sell
    --===========================================================


    --===========================================================
    --Zero Vat Product Local Purchase
    SELECT @ZeroVatProdLocalPurchaseAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId;
    --/Zero Vat Product Local Purchase

    --Zero Vat Product Import
    SELECT @ZeroVatProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId;
    --/Zero Vat Product Import

    --Vat Exempted Product Local Purchase
    SELECT @VatExemptedProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Local Purchase

    --Vat Exempted Product Import
    SELECT @VatExemptedProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity)
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId;
    --/Vat Exempted Product Import

    --Standard Vat Product Local Purchase
    SELECT @StandardVatProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @StandardVatProdLocalPurchaseVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    ----/Standard Vat Product Local Purchase

    --Standard Vat Product Import
    SELECT @StandardVatProdImportAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @StandardVatProdImportVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @standardVatProdVatTypeId;
    --/Standard Vat Product Import

    --Other Than Standard Vat Product Local Purchase
    SELECT @OtherThanStandardVatProdLocalPurchaseAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @OtherThanStandardVatProdLocalPurchaseVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId;
    --/Other Than Standard Vat Product Local Purchase

    --Other Than Standard Vat Product Import
    SELECT @OtherThanStandardVatProdImportAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @OtherThanStandardVatProdImportVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
          AND purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId;
    --Other Than Standard Vat Product Import

    --Fixed Vat Product Local Purchase
    SELECT @FixedVatProdLocalPurchaseAmount = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @FixedVatProdLocalPurchaseVatAmount = SUM(ISNULL(purchDtl.Quantity * purchDtl.VATPercent, 0))
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId;
    --Fixed Vat Product Local Purchase

    --Non-Rebatable Product Local Purchase From Turnover Organization
    SELECT @NonRebatableProdLocalPurchaseFromTurnOverOrgAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
          AND vndr.IsRegisteredAsTurnOverOrg = 1;

    SET @NonRebatableProdLocalPurchaseFromTurnOverOrgAmount
        = ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount, 0);
    SET @NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount = 0;
    --/Non-Rebatable Product Local Purchase From Turnover Organization

    --Non-Rebatable Product Local Purchase From Non-Registered Organization
    SELECT @NonRebatableProdLocalPurchaseFromNonRegOrgAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
                                                          ),
                               0
                           )
                    )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        LEFT JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
          AND vndr.IsRegisteredAsTurnOverOrg = 0;

    SET @NonRebatableProdLocalPurchaseFromNonRegOrgAmount
        = ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount, 0);
    SET @NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount = 0;
    --/Non-Rebatable Product Local Purchase From Non-Registered Organization

    --Non-Rebatable Product Local Purchase By Organization Who Sell Other Than Standard Vat Product
    SELECT @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
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
        LEFT JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          --AND org.IsSellStandardVatProduct = 0
          AND purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId;

    SET @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount
        = ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount, 0)
          + ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount, 0);
    SET @NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount = 0;
    --/Non-Rebatable Product Local Purchase By Organization Who Sell Other Than Standard Vat Product

    --Non-Rebatable Product Import By Organization Who Sell Other Than Standard Vat Product
    SELECT @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount
        = SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity),
           @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputVat(
                                                              purchDtl.UnitPrice * purchDtl.Quantity,
                                                              purchDtl.VATPercent,
                                                              purchDtl.SupplementaryDutyPercent
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
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
          --AND org.IsSellStandardVatProduct = 0
          AND purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId;

    SET @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount
        = ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount, 0)
          + ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount, 0);
    SET @NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount = 0;
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

    --===========================================================


    --===========================================================
    --Adjustment 

    --===========================================================
    --Incremental Adjustment   
    SELECT @IncrementalAdjustmentAmountForVdsPurchase = SUM(COALESCE(purch.VDSAmount, purch.TotalVAT, 0))
    FROM dbo.Purchase purch
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purch.IsVatDeductedInSource = 1;



    SELECT @IncrementalAdjustmentAmountForNotPaidInBankingChannel = SUM(ISNULL(purch.TotalVAT, 0))
    FROM dbo.Purchase purch
        INNER JOIN
        (
            SELECT pp.PurchaseId,
                   pp.PaymentMethodId
            FROM dbo.PurchasePayment pp
                INNER JOIN dbo.Purchase purch
                    ON purch.PurchaseId = pp.PurchaseId
            WHERE pp.PaymentMethodId = @paymentMethodCash
                  AND purch.OrganizationId = @OrganizationId
                  AND purch.CreatedTime >= @firstDayOfMonth
                  AND purch.CreatedTime < @firstDayOfNextMonth
                  AND purch.PurchaseTypeId = @purchaseTypeLocal
                  AND purch.TotalPriceWithoutVat + purch.TotalVAT + purch.TotalSupplementaryDuty > @upperLimitForPaymentThroughCash
        ) pt
            ON pt.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal
          AND purch.TotalPriceWithoutVat + purch.TotalVAT + purch.TotalSupplementaryDuty > @upperLimitForPaymentThroughCash;

    --SET @IncrementalAdjustmentAmountForNotPaidInBankingChannel = 0;

    SELECT @IncrementalAdjustmentVatAmountForDebitNote
        = SUM(   CASE
                     WHEN purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                         ISNULL(dnd.ReturnQuantity * purchDtl.VATPercent, 0)
                     ELSE
                         ISNULL(
                                   dbo.FnGetCalculatedInputVat(
                                                                  purchDtl.UnitPrice * dnd.ReturnQuantity,
                                                                  purchDtl.VATPercent,
                                                                  purchDtl.SupplementaryDutyPercent
                                                              ),
                                   0
                               )
                 END
             ),
           @NetTaxSdAmountForDebitNote
               = SUM(ISNULL(
                               dbo.FnGetCalculatedInputSuppDuty(
                                                                   purchDtl.UnitPrice * purchDtl.Quantity,
                                                                   purchDtl.SupplementaryDutyPercent
                                                               ),
                               0
                           )
                    )
    FROM dbo.DebitNote dn
        INNER JOIN dbo.DebitNoteDetail dnd
            ON dnd.DebitNoteId = dn.DebitNoteId
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseDetailId = dnd.PurchaseDetailId
        INNER JOIN dbo.Purchase purch
            ON purch.PurchaseId = dn.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND dn.CreatedTime >= @firstDayOfMonth
          AND dn.CreatedTime < @firstDayOfNextMonth;

    SELECT @MiscIncrementalAdjustmentAmount = ISNULL(SUM(adj.Amount), 0),
           @MiscIncrementalAdjustmentDescription = SUBSTRING(
                                                   (
                                                       SELECT ', ' + adjIn.Description -- AS [text()]
                                                       FROM dbo.Adjustment adjIn
                                                       WHERE adjIn.AdjustmentTypeId = adj.AdjustmentTypeId
                                                             AND adjIn.[Month] = @Month
                                                             AND adjIn.[Year] = @Year
                                                             AND adjIn.AdjustmentTypeId = @adjustmentTypeIncremental
                                                             AND adjIn.IsActive = 1
                                                             AND adjIn.OrganizationId = @OrganizationId
                                                       ORDER BY adjIn.AdjustmentTypeId
                                                       FOR XML PATH('')
                                                   ),
                                                   2,
                                                   1000
                                                            )
    FROM dbo.Adjustment adj
    WHERE adj.[Month] = @Month
          AND adj.[Year] = @Year
          AND adj.AdjustmentTypeId = @adjustmentTypeIncremental
          AND adj.IsActive = 1
          AND adj.OrganizationId = @OrganizationId
    GROUP BY adj.AdjustmentTypeId;


    SET @TotalIncrementalAdjustmentVatAmount
        = ISNULL(@IncrementalAdjustmentAmountForVdsPurchase, 0)
          + ISNULL(@IncrementalAdjustmentAmountForNotPaidInBankingChannel, 0)
          + ISNULL(@IncrementalAdjustmentVatAmountForDebitNote, 0) + ISNULL(@MiscIncrementalAdjustmentAmount, 0);
    --/Incremental Adjustment
    --===========================================================

    --===========================================================
    --Decremental Adjustment

    SELECT @DecrementalAdjustmentAmountForVdsSale
        = SUM(COALESCE(
                          sls.VDSAmount,
                          dbo.FnGetCalculatedOutputSuppDuty(
                                                               slsDtl.UnitPrice * slsDtl.Quantity,
                                                               slsDtl.SupplementaryDutyPercent
                                                           ),
                          0
                      )
             )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.SalesTypeId = @salesTypeLocal
          AND sls.IsVatDeductedInSource = 1;

    SELECT @DecrementalAdjustmentAmountForAdvanceTaxInImport = SUM(ISNULL(purch.AdvanceTaxPaidAmount, 0))
    FROM dbo.Purchase purch
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport;


    SELECT @DecrementalAdjustmentVatAmountForCreditNote
        = SUM(   CASE
                     WHEN slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                         cnd.ReturnQuantity * slsDtl.VATPercent
                     ELSE
                         ISNULL(
                                   dbo.FnGetCalculatedOutputVat(
                                                                   slsDtl.UnitPrice * cnd.ReturnQuantity,
                                                                   slsDtl.VATPercent,
                                                                   slsDtl.SupplementaryDutyPercent
                                                               ),
                                   0
                               )
                 END
             ),
           @NetTaxSdAmountForCreditNote
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * cnd.ReturnQuantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    )
    FROM dbo.CreditNote cn
        INNER JOIN dbo.CreditNoteDetail cnd
            ON cnd.CreditNoteId = cn.CreditNoteId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesDetailId = cnd.SalesDetailId
        INNER JOIN dbo.Sales sls
            ON sls.SalesId = cn.SalesId
    WHERE sls.OrganizationId = @OrganizationId
          AND cn.CreatedTime >= @firstDayOfMonth
          AND cn.CreatedTime < @firstDayOfNextMonth;

    SELECT @MiscDecrementalAdjustmentAmount = ISNULL(SUM(adj.Amount), 0),
           @MiscDecrementalAdjustmentDescription = SUBSTRING(
                                                   (
                                                       SELECT ', ' + adjIn.Description -- AS [text()]
                                                       FROM dbo.Adjustment adjIn
                                                       WHERE adjIn.AdjustmentTypeId = adj.AdjustmentTypeId
                                                             AND adjIn.[Month] = @Month
                                                             AND adjIn.[Year] = @Year
                                                             AND adjIn.AdjustmentTypeId = @adjustmentTypeDecremental
                                                             AND adjIn.IsActive = 1
                                                             AND adjIn.OrganizationId = @OrganizationId
                                                       ORDER BY adjIn.AdjustmentTypeId
                                                       FOR XML PATH('')
                                                   ),
                                                   2,
                                                   1000
                                                            )
    FROM dbo.Adjustment adj
    WHERE adj.[Month] = @Month
          AND adj.[Year] = @Year
          AND adj.AdjustmentTypeId = @adjustmentTypeDecremental
          AND adj.IsActive = 1
          AND adj.OrganizationId = @OrganizationId
    GROUP BY adj.AdjustmentTypeId;

    SET @TotalDecrementalAdjustmentVatAmount
        = ISNULL(@DecrementalAdjustmentAmountForVdsSale, 0)
          + ISNULL(@DecrementalAdjustmentAmountForAdvanceTaxInImport, 0)
          + ISNULL(@DecrementalAdjustmentVatAmountForCreditNote, 0) + ISNULL(@MiscDecrementalAdjustmentAmount, 0);
    --/Decremental Adjustment
    --===========================================================
    --/Adjustment
    --===========================================================


    --===========================================================
    --Previous Tax Calculation
    --VAT


    SET @TotalPreviousDecrementalAdjustmentAmountForVdsSale = 0;
    SET @TotalPreviousIncrementalAdjustmentAmountForNotPaidInBankingChannel = 0;
    SET @TotalPreviousIncrementalAdjustmentVatAmountForDebitNote = 0;


    SET @TotalPreviousIncrementalAdjustmentAmountForVdsPurchase = 0;
    SET @TotalPreviousDecrementalAdjustmentAmountForAdvanceTaxInImport = 0;
    SET @TotalPreviousDecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd = 0;
    SET @TotalPreviousDecrementalAdjustmentVatAmountForCreditNote = 0;

    SELECT @TotalPreviousRawMaterialRebateAmount
        = SUM(   CASE
                     WHEN purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                         ISNULL(purchDtl.Quantity * purchDtl.VATPercent, 0)
                     ELSE
                         ISNULL(
                                   dbo.FnGetCalculatedInputVat(
                                                                  purchDtl.UnitPrice * purchDtl.Quantity,
                                                                  purchDtl.VATPercent,
                                                                  purchDtl.SupplementaryDutyPercent
                                                              ),
                                   0
                               )
                 END
             )
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime < @firstDayOfMonth
          AND purch.PurchaseTypeId = @purchaseTypeLocal;
    --AND purchDtl.ProductVATTypeId = @standardVatProdVatTypeId;


    SELECT @PreviousVatAmount
        = SUM(   CASE
                     WHEN slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId THEN
                         slsDtl.VATPercent * slsDtl.Quantity
                     ELSE
                         dbo.FnGetCalculatedOutputVat(
                                                         slsDtl.UnitPrice * slsDtl.Quantity,
                                                         slsDtl.VATPercent,
                                                         slsDtl.SupplementaryDutyPercent
                                                     )
                 END
             )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime < @firstDayOfMonth
          AND sls.SalesTypeId = @salesTypeLocal;


    SELECT @TotalPreviousMiscIncrementalAdjustmentAmount = ISNULL(SUM(adj.Amount), 0)
    FROM dbo.Adjustment adj
    WHERE (
              adj.[Year] < @Year
              OR
              (
                  adj.[Year] = @Year
                  AND adj.[Month] < @Month
              )
          )
          AND adj.AdjustmentTypeId = @adjustmentTypeIncremental
    GROUP BY adj.AdjustmentTypeId;


    SELECT @TotalPreviousMiscDecrementalAdjustmentAmount = ISNULL(SUM(adj.Amount), 0)
    FROM dbo.Adjustment adj
    WHERE (
              adj.[Year] < @Year
              OR
              (
                  adj.[Year] = @Year
                  AND adj.[Month] < @Month
              )
          )
          AND adj.AdjustmentTypeId = @adjustmentTypeDecremental
    GROUP BY adj.AdjustmentTypeId;



    SET @TotalPreviousVatAmount
        = ISNULL(@PreviousVatAmount, 0) + ISNULL(@TotalPreviousIncrementalAdjustmentAmountForVdsPurchase, 0)
          + ISNULL(@TotalPreviousIncrementalAdjustmentAmountForNotPaidInBankingChannel, 0)
          + ISNULL(@TotalPreviousIncrementalAdjustmentVatAmountForDebitNote, 0)
          + ISNULL(@TotalPreviousMiscIncrementalAdjustmentAmount, 0) - ISNULL(@TotalPreviousRawMaterialRebateAmount, 0)
          - ISNULL(@TotalPreviousDecrementalAdjustmentAmountForVdsSale, 0)
          - ISNULL(@TotalPreviousDecrementalAdjustmentAmountForAdvanceTaxInImport, 0)
          - ISNULL(@TotalPreviousDecrementalAdjustmentAmountForRawMaterialPurchaseToProduceExportProd, 0)
          - ISNULL(@TotalPreviousDecrementalAdjustmentVatAmountForCreditNote, 0)
          - ISNULL(@TotalPreviousMiscDecrementalAdjustmentAmount, 0);

    SELECT @TotalPreviousVatPaymentAmount = SUM(mskRtnPmnt.PaidAmount)
    FROM dbo.MushakReturnPayment mskRtnPmnt
    WHERE mskRtnPmnt.OrganizationId = @OrganizationId
          AND
          (
              mskRtnPmnt.MushakYear < @Year
              OR
              (
                  mskRtnPmnt.MushakYear = @Year
                  AND mskRtnPmnt.MushakMonth < @Month
              )
          )
          AND mskRtnPmnt.IsSubmitted = 1
          AND mskRtnPmnt.MushakReturnPaymentTypeId = @PaymentTypeSDDepositForTheTaxPeriod;

    SET @LastClosingVatAmount = ISNULL(@TotalPreviousVatAmount, 0) - ISNULL(@TotalPreviousVatPaymentAmount, 0);


    --SD
    SELECT @TotalPreviousSdAmount
        = SUM(dbo.FnGetCalculatedOutputSuppDuty(
                                                   (slsDtl.UnitPrice - slsDtl.DiscountPerItem),
                                                   slsDtl.SupplementaryDutyPercent
                                               )
             )
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime < @firstDayOfMonth
          AND sls.SalesTypeId = @salesTypeLocal;

    SELECT @TotalPreviousSdPaymentAmount = SUM(mskRtnPmnt.PaidAmount)
    FROM dbo.MushakReturnPayment mskRtnPmnt
    WHERE mskRtnPmnt.OrganizationId = @OrganizationId
          AND
          (
              mskRtnPmnt.MushakYear < @Year
              OR
              (
                  mskRtnPmnt.MushakYear = @Year
                  AND mskRtnPmnt.MushakMonth < @Month
              )
          )
          AND mskRtnPmnt.IsSubmitted = 1
          AND mskRtnPmnt.MushakReturnPaymentTypeId = @PaymentTypeSDDepositForTheTaxPeriod;
    SET @LastClosingSuppDutyAmount = ISNULL(@TotalPreviousSdAmount, 0) - ISNULL(@TotalPreviousSdPaymentAmount, 0);
    --/Previous Tax Calculation
    --===========================================================



    --===========================================================
    --Net tax calculation



    SET @NetTaxTotalPayableVatAmountInCurrentTaxTerm
        = ISNULL(@TotalVatAmount, 0) - ISNULL(@TotalRawMaterialRebateAmount, 0)
          + ISNULL(@TotalIncrementalAdjustmentVatAmount, 0) - ISNULL(@TotalDecrementalAdjustmentVatAmount, 0);
    SET @NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance
        = ISNULL(@NetTaxTotalPayableVatAmountInCurrentTaxTerm, 0) + ISNULL(@LastClosingVatAmount, 0);
    SET @NetPayableVATForTreasurySubmission
        = ISNULL(@NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance, 0) + ISNULL(@InterestForDueVat, 0)
          + ISNULL(@FineAndPenaltyForDelaySubmission, 0) + ISNULL(@FineAndPenaltyOthers, 0);



    SET @NetTaxTotalPayableSdAmountInCurrentTaxTerm
        = ISNULL(@TotalSdAmount, 0) + ISNULL(@NetTaxSdAmountForDebitNote, 0) - ISNULL(@NetTaxSdAmountForCreditNote, 0)
          - ISNULL(@NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd, 0);
    SET @NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance
        = ISNULL(@NetTaxTotalPayableSdAmountInCurrentTaxTerm, 0) + ISNULL(@LastClosingSuppDutyAmount, 0);
    SET @NetPayableSDForTreasurySubmission
        = ISNULL(@NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance, 0) + ISNULL(@InterestForDueSuppDuty, 0);
    --/Net tax calculation
    --===========================================================


    --===========================================================
    --Refund calculation
    SELECT @isWantToGetBackClosingBalance = ISNULL(mskRtnRfnd.InterestedToRefundVATAmount, 0),
           @ClaimedRefundVatAmount = ISNULL(mskRtnRfnd.InterestedToRefundVATAmount, 0),
           @ClaimedRefundSDAmount = ISNULL(mskRtnRfnd.InterestedToRefundSDAmount, 0),
           @RefusedClaimedRefundVatAmount
               = CASE
                     WHEN mskRtnRfnd.ApprovedToRefundVATAmount IS NULL THEN
                         ISNULL(mskRtnRfnd.InterestedToRefundVATAmount, 0)
                     ELSE
                         ISNULL(mskRtnRfnd.InterestedToRefundVATAmount, 0)
                         - ISNULL(mskRtnRfnd.ApprovedToRefundVATAmount, 0)
                 END,
           @RefusedClaimedRefundSDAmount
               = CASE
                     WHEN mskRtnRfnd.ApprovedToRefundSDAmount IS NULL THEN
                         ISNULL(mskRtnRfnd.InterestedToRefundSDAmount, 0)
                     ELSE
                         ISNULL(mskRtnRfnd.InterestedToRefundSDAmount, 0)
                         - ISNULL(mskRtnRfnd.ApprovedToRefundSDAmount, 0)
                 END
    FROM dbo.MushakReturnRefund mskRtnRfnd
    WHERE mskRtnRfnd.OrganizationId = @OrganizationId
          AND mskRtnRfnd.MushakYear = @Year
          AND mskRtnRfnd.MushakMonth = @Month;

    SET @isWantToGetBackClosingBalance = ISNULL(@isWantToGetBackClosingBalance, 0);

    IF @isWantToGetBackClosingBalance = 1
    BEGIN
        SELECT @ClaimedRefundVatAmount = ISNULL(@ClaimedRefundVatAmount, 0),
               @ClaimedRefundSDAmount = ISNULL(@ClaimedRefundSDAmount, 0),
               @RefusedClaimedRefundVatAmount = ISNULL(@RefusedClaimedRefundVatAmount, 0),
               @RefusedClaimedRefundSDAmount = ISNULL(@RefusedClaimedRefundSDAmount, 0),
               @isWantToGetBackClosingBalanceInWords = N'হ্যাঁ',
               @isWantToGetBackClosingBalanceInWordsEng = N'Yes';
    END;
    ELSE
    BEGIN
        SELECT @ClaimedRefundVatAmount = 0,
               @ClaimedRefundSDAmount = 0,
               @RefusedClaimedRefundVatAmount = 0,
               @RefusedClaimedRefundSDAmount = 0;
    END;
    --/Refund calculation
    --===========================================================





    --===========================================================
    --OLD calculation
    SELECT @RemainingVATAmountFromMushak18p6 = oacb.RemainingVATBalance,
           @AdjustableAmountFromNote54 = @NetTaxTotalPayableVatAmountInCurrentTaxTerm * 0.1,
           @RemainingSDAmountFromMushak18p6 = oacb.RemainingSDBalance,
           @AdjustableAmountFromNote55 = @NetTaxTotalPayableSdAmountInCurrentTaxTerm * 0.1
    FROM dbo.OldAccountCurrentBalance oacb
    WHERE oacb.OrganizationId = @OrganizationId
          AND oacb.MushakYear = @Year
          AND oacb.MushakMonth = @Month;

    IF @AdjustableAmountFromNote54 > @RemainingVATAmountFromMushak18p6
    BEGIN
        SET @AdjustableAmountFromNote54 = @RemainingVATAmountFromMushak18p6;
    END;

    IF @AdjustableAmountFromNote55 > @RemainingSDAmountFromMushak18p6
    BEGIN
        SET @AdjustableAmountFromNote55 = @RemainingSDAmountFromMushak18p6;
    END;

    --OLD calculation
    --===========================================================

    --============================================================
    --TODO: Have to modify

    SET @NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance
        = ISNULL(@NetTaxTotalPayableVatAmountInCurrentTaxTerm, 0)
          - (ISNULL(@LastClosingVatAmount, 0) + ISNULL(@AdjustableAmountFromNote54, 0));
    SET @NetPayableVATForTreasurySubmission
        = ISNULL(@NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance, 0) + ISNULL(@InterestForDueVat, 0)
          + ISNULL(@FineAndPenaltyForDelaySubmission, 0) + ISNULL(@FineAndPenaltyOthers, 0);
    SET @NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance
        = ISNULL(@NetTaxTotalPayableSdAmountInCurrentTaxTerm, 0)
          - (ISNULL(@LastClosingSuppDutyAmount, 0) + ISNULL(@AdjustableAmountFromNote55, 0));
    SET @NetPayableSDForTreasurySubmission
        = ISNULL(@NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance, 0) + ISNULL(@InterestForDueSuppDuty, 0);
    --============================================================

    --===========================================================
    --Closing calculation
    SELECT @TotalVatPaidAmount = SUM(mskRtnPmnt.PaidAmount)
    FROM dbo.MushakReturnPayment mskRtnPmnt
    WHERE mskRtnPmnt.OrganizationId = @OrganizationId
          AND mskRtnPmnt.MushakYear = @Year
          AND mskRtnPmnt.MushakMonth = @Month
          AND mskRtnPmnt.IsSubmitted = 1
          AND mskRtnPmnt.MushakReturnPaymentTypeId = @PaymentTypeVATDepositForTheTaxPeriod;
    SELECT @TotalSdPaidAmount = SUM(mskRtnPmnt.PaidAmount)
    FROM dbo.MushakReturnPayment mskRtnPmnt
    WHERE mskRtnPmnt.OrganizationId = @OrganizationId
          AND mskRtnPmnt.MushakYear = @Year
          AND mskRtnPmnt.MushakMonth = @Month
          AND mskRtnPmnt.IsSubmitted = 1
          AND mskRtnPmnt.MushakReturnPaymentTypeId = @PaymentTypeSDDepositForTheTaxPeriod;

    SELECT @CurrentClosingVatAmount
        = ISNULL(@TotalVatPaidAmount, 0)
          - (ISNULL(@NetPayableVATForTreasurySubmission, 0) + ISNULL(@ClaimedRefundVatAmount, 0)
             + ISNULL(@RefusedClaimedRefundVatAmount, 0)
            );
    SELECT @CurrentClosingSuppDutyAmount
        = ISNULL(@TotalSdPaidAmount, 0)
          - (ISNULL(@NetPayableSDForTreasurySubmission, 0) + ISNULL(@ClaimedRefundSDAmount, 0)
             + ISNULL(@RefusedClaimedRefundSDAmount, 0)
            );
    --/Closing calculation
    --===========================================================

    --===========================================================
    --Part-1 & Part-12
    SELECT org.OrganizationId,
           org.Name AS TaxPayersName,
           org.Name AS TaxPayersNameEng,
           org.VATRegNo,
           org.VATRegNo AS VATRegNoEng,
           org.BIN AS OrganizationBin,
           org.BIN AS OrganizationBinEng,
           org.Address AS OrganizationAddress,
           org.Address AS OrganizationAddressEng,
           bsnsNature.NameInBangla AS BusinessNature,
           bsnsNature.Name AS BusinessNatureEng,
           finActNat.NameInBangla AS FinancialActivityNature,
           finActNat.Name AS FinancialActivityNatureEng,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonName AS VatResponsiblePersonNameEng,
           org.VatResponsiblePersonDesignation,
           org.VatResponsiblePersonDesignation AS VatResponsiblePersonDesignationEng,
           org.VatResponsiblePersonMobileNo,
           org.VatResponsiblePersonMobileNo AS VatResponsiblePersonMobileNoEng,
           org.VatResponsiblePersonEmailAddress
    --INTO MushakReturnPartOne
    FROM dbo.Organizations org
        INNER JOIN dbo.BusinessNature bsnsNature
            ON bsnsNature.BusinessNatureId = org.BusinessNatureId
        INNER JOIN dbo.FinancialActivityNature finActNat
            ON finActNat.FinancialActivityNatureId = org.FinancialActivityNatureId
    WHERE org.OrganizationId = @OrganizationId;
    --===========================================================



    --===========================================================
    --Part-2
    SELECT @TermOfTax AS TermOfTax,
           @TermOfTaxEng AS TermOfTaxEng,
           @TypeOfSubmission AS TypeOfSubmission,
           @TypeOfSubmissionEng AS TypeOfSubmissionEng,
           @isTransactionOccuredInLastTerm AS IsTransactionOccuredInLastTerm,
           @isTransactionOccuredInLastTermInWords AS IsTransactionOccuredInLastTermInWords,
           @isTransactionOccuredInLastTermInWordsEng AS isTransactionOccuredInLastTermInWordsEng,
           CAST(NULL AS DATETIME) AS SubmissionDate;
    --INTO MushakReturnPartTwo;
    --===========================================================



    --===========================================================
    --Part-3
    SELECT ISNULL(@DirectExportAmount, 0) AS DirectExportAmount,
           ISNULL(@IndirectExportAmount, 0) AS IndirectExportAmount,
           ISNULL(@VatExemptedProdSellAmount, 0) AS VatExemptedProdSellAmount,
           ISNULL(@StandardVatRateProdSellAmount, 0) AS StandardVatRateProdSellAmount,
           ISNULL(@StandardVatRateProdSellSdAmount, 0) AS StandardVatRateProdSellSdAmount,
           ISNULL(@StandardVatRateProdSellVatAmount, 0) AS StandardVatRateProdSellVatAmount,
           ISNULL(@MrpProdSellAmount, 0) AS MrpProdSellAmount,
           ISNULL(@MrpProdSellSdAmount, 0) AS MrpProdSellSdAmount,
           ISNULL(@MrpProdSellVatAmount, 0) AS MrpProdSellVatAmount,
           ISNULL(@FixedVatProdSellAmount, 0) AS FixedVatProdSellAmount,
           ISNULL(@FixedVatProdSellSdAmount, 0) AS FixedVatProdSellSdAmount,
           ISNULL(@FixedVatProdSellVatAmount, 0) AS FixedVatProdSellVatAmount,
           ISNULL(@OtherThanStandardVatRateProdSellAmount, 0) AS OtherThanStandardVatRateProdSellAmount,
           ISNULL(@OtherThanStandardVatRateProdSellSdAmount, 0) AS OtherThanStandardVatRateProdSellSdAmount,
           ISNULL(@OtherThanStandardVatRateProdSellVatAmount, 0) AS OtherThanStandardVatRateProdSellVatAmount,
           ISNULL(@TradingSellAmount, 0) AS TradingSellAmount,
           ISNULL(@TradingSellSdAmount, 0) AS TradingSellSdAmount,
           ISNULL(@TradingSellVatAmount, 0) AS TradingSellVatAmount,
           ISNULL(@TotalAmount, 0) AS TotalAmount,
           ISNULL(@TotalSdAmount, 0) AS TotalSdAmount,
           ISNULL(@TotalVatAmount, 0) AS TotalVatAmount;
    --INTO MushakReturnPartThree;
    --===========================================================



    --===========================================================
    --Part-4
    SELECT ISNULL(@ZeroVatProdLocalPurchaseAmount, 0) AS ZeroVatProdLocalPurchaseAmount,
           ISNULL(@ZeroVatProdImportAmount, 0) AS ZeroVatProdImportAmount,
           ISNULL(@VatExemptedProdLocalPurchaseAmount, 0) AS VatExemptedProdLocalPurchaseAmount,
           ISNULL(@VatExemptedProdImportAmount, 0) AS VatExemptedProdImportAmount,
           ISNULL(@StandardVatProdLocalPurchaseAmount, 0) AS StandardVatProdLocalPurchaseAmount,
           ISNULL(@StandardVatProdLocalPurchaseVatAmount, 0) AS StandardVatProdLocalPurchaseVatAmount,
           ISNULL(@StandardVatProdImportAmount, 0) AS StandardVatProdImportAmount,
           ISNULL(@StandardVatProdImportVatAmount, 0) AS StandardVatProdImportVatAmount,
           ISNULL(@OtherThanStandardVatProdLocalPurchaseAmount, 0) AS OtherThanStandardVatProdLocalPurchaseAmount,
           ISNULL(@OtherThanStandardVatProdLocalPurchaseVatAmount, 0) AS OtherThanStandardVatProdLocalPurchaseVatAmount,
           ISNULL(@OtherThanStandardVatProdImportAmount, 0) AS OtherThanStandardVatProdImportAmount,
           ISNULL(@OtherThanStandardVatProdImportVatAmount, 0) AS OtherThanStandardVatProdImportVatAmount,
           ISNULL(@FixedVatProdLocalPurchaseAmount, 0) AS FixedVatProdLocalPurchaseAmount,
           ISNULL(@FixedVatProdLocalPurchaseVatAmount, 0) AS FixedVatProdLocalPurchaseVatAmount,
           ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgAmount, 0) AS NonRebatableProdLocalPurchaseFromTurnOverOrgAmount,
           ISNULL(@NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount, 0) AS NonRebatableProdLocalPurchaseFromTurnOverOrgVatAmount,
           ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgAmount, 0) AS NonRebatableProdLocalPurchaseFromNonRegOrgAmount,
           ISNULL(@NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount, 0) AS NonRebatableProdLocalPurchaseFromNonRegOrgVatAmount,
           ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount, 0) AS NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdAmount,
           ISNULL(@NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount, 0) AS NonRebatableProdLocalPurchaseByOrgWhoSellOtherThanStandardVatProdVatAmount,
           ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount, 0) AS NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdAmount,
           ISNULL(@NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount, 0) AS NonRebatableProdImportByOrgWhoSellOtherThanStandardVatProdVatAmount,
           ISNULL(@TotalRawMaterialPurchaseAmount, 0) AS TotalRawMaterialPurchaseAmount,
           ISNULL(@TotalRawMaterialRebateAmount, 0) AS TotalRawMaterialRebateAmount;
    --INTO MushakReturnPartFour;
    --===========================================================



    --===========================================================
    --Part-5
    SELECT ISNULL(@IncrementalAdjustmentAmountForVdsPurchase, 0) AS IncrementalAdjustmentAmountForVdsSale,
           ISNULL(@IncrementalAdjustmentAmountForNotPaidInBankingChannel, 0) AS IncrementalAdjustmentAmountForNotPaidInBankingChannel,
           ISNULL(@IncrementalAdjustmentVatAmountForDebitNote, 0) AS IncrementalAdjustmentVatAmountForDebitNote,
           ISNULL(@MiscIncrementalAdjustmentAmount, 0) AS MiscIncrementalAdjustmentAmount,
           @MiscIncrementalAdjustmentDescription AS MiscIncrementalAdjustmentDesc,
           ISNULL(@TotalIncrementalAdjustmentVatAmount, 0) AS TotalIncrementalAdjustmentVatAmount;
    --INTO MushakReturnPartFive;
    --===========================================================



    --===========================================================
    --Part-6
    SELECT ISNULL(@DecrementalAdjustmentAmountForVdsSale, 0) AS DecrementalAdjustmentAmountForVdsPurchase,
           ISNULL(@DecrementalAdjustmentAmountForAdvanceTaxInImport, 0) AS DecrementalAdjustmentAmountForAdvanceTaxInImport,
           ISNULL(@DecrementalAdjustmentVatAmountForCreditNote, 0) AS DecrementalAdjustmentVatAmountForCreditNote,
           ISNULL(@MiscDecrementalAdjustmentAmount, 0) AS MiscDecrementalAdjustmentAmount,
           @MiscDecrementalAdjustmentDescription AS MiscDecrementalAdjustmentDesc,
           ISNULL(@TotalDecrementalAdjustmentVatAmount, 0) AS TotalDecrementalAdjustmentVatAmount;
    --INTO MushakReturnPartSix;
    --===========================================================



    --===========================================================
    --Part-7
    SELECT ISNULL(@NetTaxTotalPayableVatAmountInCurrentTaxTerm, 0) AS NetTaxTotalPayableVatAmountInCurrentTaxTerm,
           ISNULL(@NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance, 0) AS NetTaxTotalPayableVatAmountAfterAdjustmentWithEndingBalance,
           ISNULL(@NetTaxTotalPayableSdAmountInCurrentTaxTerm, 0) AS NetTaxTotalPayableSdAmountInCurrentTaxTerm,
           ISNULL(@NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance, 0) AS NetTaxTotalPayableSdAmountAfterAdjustmentWithEndingBalance,
           ISNULL(@NetTaxSdAmountForDebitNote, 0) AS NetTaxSdAmountForDebitNote,
           ISNULL(@NetTaxSdAmountForCreditNote, 0) AS NetTaxSdAmountForCreditNote,
           ISNULL(@NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd, 0) AS NetTaxPaidSdAmountForRawMaterialPurchaseToProduceExportProd,
           ISNULL(@InterestForDueVat, 0) AS NetTaxInterstAmountForDeuVat,
           ISNULL(@InterestForDueSuppDuty, 0) AS NetTaxInterstAmountForDeuSuppDuty,
           ISNULL(@FineAndPenaltyForDelaySubmission, 0) AS NetTaxFineAndPenaltyForDelaySubmission,
           ISNULL(@FineAndPenaltyOthers, 0) AS NetTaxFineAndPenaltyOthers,
           ISNULL(@ExciseDuty, 0) AS NetTaxExciseDuty,
           ISNULL(@DevelopmentSurcharge, 0) AS NetTaxDevelopmentSurcharge,
           ISNULL(@ItDevelopmentSurcharge, 0) AS NetTaxInformationTechnologyDevelopmentSurcharge,
           ISNULL(@HealthCareSurcharge, 0) AS NetTaxHealthCareSurcharge,
           ISNULL(@EnvironmentProtectionSurcharge, 0) AS NetTaxEnvironmentProtectionSurcharge,
           ISNULL(@NetPayableVATForTreasurySubmission, 0) AS NetPayableVATForTreasurySubmission,
           ISNULL(@NetPayableSDForTreasurySubmission, 0) AS NetPayableSDForTreasurySubmission,
           ISNULL(@LastClosingVatAmount, 0) AS NetTaxVatEndingBalanceOfLastTerm,
           ISNULL(@LastClosingSuppDutyAmount, 0) AS NetTaxSdEndingBalanceOfLastTerm;
    --INTO MushakReturnPartSeven;
    --===========================================================




    --===========================================================
    --Part-8 
    SELECT ISNULL(@RemainingVATAmountFromMushak18p6, 0) AS RemainingVATAmountFromMushak18p6,
           ISNULL(@RemainingSDAmountFromMushak18p6, 0) AS RemainingSDAmountFromMushak18p6,
           ISNULL(@AdjustableAmountFromNote54, 0) AS AdjustableAmountFromNote54,
           ISNULL(@AdjustableAmountFromNote55, 0) AS AdjustableAmountFromNote55;
    --INTO MushakReturnPartEight;
    --===========================================================

    --===========================================================
    --Part-9
    SELECT mskRtnPmntType.MushakReturnPaymentTypeId,
           mskRtnPmntType.NbrEconomicCodeId,
           mskRtnPmntType.SubFormId,
           mskRtnPmntType.TypeName,
           mskRtnPmntType.TypeNameBn,
           mskRtnPmntType.NoteNo,
           nec.EconomicCode,
           ISNULL(mrp.PaidAmount, 0) AS PaidAmount
    --INTO MushakReturnPartNine
    FROM dbo.MushakReturnPaymentType mskRtnPmntType
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = mskRtnPmntType.NbrEconomicCodeId
        LEFT JOIN
        (
            SELECT mskRtnPmnt.OrganizationId,
                   mskRtnPmnt.MushakReturnPaymentTypeId,
                   SUM(mskRtnPmnt.PaidAmount) AS PaidAmount
            FROM dbo.MushakReturnPayment mskRtnPmnt
            WHERE mskRtnPmnt.OrganizationId = @OrganizationId
                  AND mskRtnPmnt.MushakYear = @Year
                  AND mskRtnPmnt.MushakMonth = @Month
                  AND mskRtnPmnt.IsSubmitted = 1
            GROUP BY mskRtnPmnt.OrganizationId,
                     mskRtnPmnt.MushakReturnPaymentTypeId
        ) mrp
            ON mrp.MushakReturnPaymentTypeId = mskRtnPmntType.MushakReturnPaymentTypeId
    WHERE mskRtnPmntType.IsActive = 1;
    --===========================================================




    --===========================================================
    --Part-10
    SELECT @CurrentClosingVatAmount AS CurrentClosingVatAmount,
           @CurrentClosingSuppDutyAmount AS CurrentClosingSuppDutyAmount; --INTO MushakReturnPartTen;
    --===========================================================




    --===========================================================
    --Part-11
    SELECT ISNULL(@isWantToGetBackClosingBalance, 0) AS IsWantToGetBackClosingBalance,
           @isWantToGetBackClosingBalanceInWords AS IsWantToGetBackClosingBalanceInWords,
           @isWantToGetBackClosingBalanceInWordsEng AS IsWantToGetBackClosingBalanceInWordsEng,
           ISNULL(@ClaimedRefundVatAmount, 0) AS ClaimedRefundVatAmount,
           ISNULL(@ClaimedRefundSDAmount, 0) AS ClaimedRefundSDAmount; --INTO MushakReturnPartEleven;
    --===========================================================




    --===========================================================
    --Part-12: Will get data from Part-1
    --===========================================================

    --===========================================================
    --Subform-Ka
    SELECT ROW_NUMBER() OVER (PARTITION BY trans.SubFormId ORDER BY trans.ProductId) AS SlNo,
           trans.SubFormName,
           trans.ProductCommercialDescription,
           trans.ProductCode,
           trans.ProductName,
           ISNULL(trans.ProductPrice, 0) AS ProductPrice,
           ISNULL(trans.ProductSupplementaryDuty, 0) AS ProductSupplementaryDuty,
           ISNULL(trans.ProductVat, 0) AS ProductVat,
           trans.Remarks,
           trans.RemarksEng --INTO MushakReturnSubFormKa
    FROM
    (
        SELECT slsDtl.SalesDetailId AS DetailId,
               sls.SalesId AS OperationId,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport THEN
                       N'সাবফর্ম: ক(নোট- ১,২ এর জন্য প্রযোজ্য)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'সাবফর্ম: ক(নোট- ৩ এর জন্য প্রযোজ্য)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'সাবফর্ম: ক(নোট- ৪ এর জন্য প্রযোজ্য)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       N'সাবফর্ম: ক(নোট- ৫ এর জন্য প্রযোজ্য)'
                   ELSE
                       N'সাবফর্ম: ক(নোট- ৭ এর জন্য প্রযোজ্য)'
               END AS SubFormName,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport THEN
                       N'Subform: (For Note- 1, 2)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'Subform: (For Note- 3)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'Subform: (For Note- 4)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       N'Subform: (For Note- 5)'
                   ELSE
                       N'Subform: (For Note- 7)'
               END AS SubFormNameEng,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport THEN
                       1
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       2
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       3
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       4
                   ELSE
                       5
               END AS SubFormSeq,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport THEN
                       N'note1And2SubForm'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'note3SubForm'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'note4SubForm'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       N'note5SubForm'
                   ELSE
                       N'note7SubForm'
               END AS SubFormId,
               prod.ProductId,
               prod.Name AS ProductCommercialDescription,
               prod.Code AS ProductCode,
               prod.Name AS ProductName,
               (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
               dbo.FnGetCalculatedOutputSuppDuty(
                                                    (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity,
                                                    slsDtl.SupplementaryDutyPercent
                                                ) AS ProductSupplementaryDuty,
               dbo.FnGetCalculatedOutputVat(
                                               (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity,
                                               slsDtl.VATPercent,
                                               slsDtl.SupplementaryDutyPercent
                                           ) AS ProductVat,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport
                        AND sls.ExportTypeId = @exportTypeDirect THEN
                       N'সরবরাহ (নোট ১)'
                   WHEN sls.SalesTypeId = @salesTypeExport
                        AND sls.ExportTypeId = @exportTypeInDirect THEN
                       N'সরবরাহ (নোট ২)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'সরবরাহ (নোট ৩)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'সরবরাহ (নোট ৪)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       N'সরবরাহ (নোট ৫)'
                   ELSE
                       N'সরবরাহ (নোট ৭)'
               END AS Remarks,
               CASE
                   WHEN sls.SalesTypeId = @salesTypeExport
                        AND sls.ExportTypeId = @exportTypeDirect THEN
                       N'Supply (Note- 1)'
                   WHEN sls.SalesTypeId = @salesTypeExport
                        AND sls.ExportTypeId = @exportTypeInDirect THEN
                       N'Supply (Note- 2)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'Supply (Note- 3)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'Supply (Note- 4)'
                   WHEN sls.SalesTypeId = @salesTypeLocal
                        AND slsDtl.ProductVATTypeId = @mrpProdVatTypeId THEN
                       N'Supply (Note- 5)'
                   ELSE
                       N'Supply (Note- 7)'
               END AS RemarksEng --INTO SpGetMushakNinePointOneSubFormKa
        FROM dbo.Sales sls
            INNER JOIN dbo.SalesDetails slsDtl
                ON slsDtl.SalesId = sls.SalesId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = slsDtl.ProductId
        WHERE sls.OrganizationId = @OrganizationId
              AND sls.CreatedTime >= @firstDayOfMonth
              AND sls.CreatedTime < @firstDayOfNextMonth
              AND
              (
                  sls.SalesTypeId = @salesTypeExport
                  OR
                  (
                      sls.SalesTypeId = @salesTypeLocal
                      AND slsDtl.ProductVATTypeId IN ( @vatExemptedProdVatTypeId, @standardVatProdVatTypeId,
                                                       @mrpProdVatTypeId, @otherThanStandardVatProdVatTypeId
                                                     )
                  )
              )
        UNION ALL
        SELECT purchDtl.PurchaseDetailId AS DetailId,
               purch.PurchaseId AS OperationId,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১০,১১ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১২,১৩ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১৪, ১৫ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১৬,১৭ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১৬,১৭ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ১৯,২০ এর জন্য প্রযোজ্য)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId THEN
                       N'সাবফর্ম- ক(নোট ২১,২২ এর জন্য প্রযোজ্য)'
                   ELSE
                       N'সাবফর্ম- ক'
               END AS SubFormName,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                       N'Subform: (For Note- 10, 11)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'Subform: (For Note- 12, 13)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'Subform: (For Note- 14, 15)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                       N'Subform: (For Note- 16, 17)'
                   WHEN prod.IsNonRebateable = 1
                        AND org.IsSellStandardVatProduct = 1 THEN
                       N'Subform: (For Note- 19, 20)'
                   ELSE
                       N'Subform: (For Note- 21, 22)'
               END AS SubFormNameEng,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                       6
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       7
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       8
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                       9
                   WHEN prod.IsNonRebateable = 1
                        AND org.IsSellStandardVatProduct = 1 THEN
                       10
                   ELSE
                       11
               END AS SubFormSeq,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId THEN
                       N'note10And11SubForm'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId THEN
                       N'note12And13SubForm'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId THEN
                       N'note14And15SubForm'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId THEN
                       N'note16And17SubForm'
                   WHEN prod.IsNonRebateable = 1
                        AND org.IsSellStandardVatProduct = 1 THEN
                       N'note19And20SubForm'
                   ELSE
                       N'note21And22SubForm'
               END AS SubFormId,
               prod.ProductId,
               prod.Name AS ProductCommercialDescription,
               prod.Code AS ProductCode,
               prod.Name AS ProductName,
               (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity AS ProductPrice,
               dbo.FnGetCalculatedInputSuppDuty(
                                                   (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity,
                                                   purchDtl.SupplementaryDutyPercent
                                               ) AS ProductSupplementaryDuty,
               dbo.FnGetCalculatedInputVat(
                                              (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity,
                                              purchDtl.VATPercent,
                                              purchDtl.SupplementaryDutyPercent
                                          ) AS ProductVat,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'ক্রয় (নোট ১০)'
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'ক্রয় (নোট ১১)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'ক্রয় (নোট ১২)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'ক্রয় (নোট ১৩)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'ক্রয় (নোট ১৪)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'ক্রয় (নোট ১৫)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'ক্রয় (নোট ১৬)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'ক্রয় (নোট ১৭)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal
                        AND vndr.IsRegisteredAsTurnOverOrg = 1 THEN
                       N'ক্রয় (নোট ১৯)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal
                        AND vndr.IsRegisteredAsTurnOverOrg = 0 THEN
                       N'ক্রয় (নোট ২০)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'ক্রয় (নোট ২১)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'ক্রয় (নোট ২২)'
                   ELSE
                       N'ক্রয়'
               END AS Remarks,
               CASE
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'Purchase (Note- 10)'
                   WHEN purchDtl.ProductVATTypeId = @zeroVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'Purchase (Note- 11)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'Purchase (Note- 12)'
                   WHEN purchDtl.ProductVATTypeId = @vatExemptedProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'Purchase (Note- 13)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'Purchase (Note- 14)'
                   WHEN purchDtl.ProductVATTypeId = @standardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'Purchase (Note- 15)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'Purchase (Note- 16)'
                   WHEN purchDtl.ProductVATTypeId = @otherThanStandardVatProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'Purchase (Note- 17)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal
                        AND vndr.IsRegisteredAsTurnOverOrg = 1 THEN
                       N'Purchase (Note- 19)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal
                        AND vndr.IsRegisteredAsTurnOverOrg = 0 THEN
                       N'Purchase (Note- 20)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeLocal
                        AND purch.PurchaseTypeId = @purchaseTypeLocal THEN
                       N'Purchase (Note- 21)'
                   WHEN purchDtl.ProductVATTypeId = @notAdmissibleForCreditProdForWhoDoesNotSellStandardProdVatTypeId
                        AND purch.PurchaseTypeId = @purchaseTypeImport THEN
                       N'Purchase (Note- 22)'
                   ELSE
                       N'Purchase'
               END AS RemarksEng
        FROM dbo.Purchase purch
            INNER JOIN dbo.PurchaseDetails purchDtl
                ON purchDtl.PurchaseId = purch.PurchaseId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = purchDtl.ProductId
            INNER JOIN dbo.Organizations org
                ON org.OrganizationId = purch.OrganizationId
            LEFT JOIN dbo.Vendor vndr
                ON vndr.VendorId = purch.VendorId
        WHERE purch.OrganizationId = @OrganizationId
              AND purch.CreatedTime >= @firstDayOfMonth
              AND purch.CreatedTime < @firstDayOfNextMonth
              AND purchDtl.ProductVATTypeId <> @fixedVatProdVatTypeId
    ) trans
    --GROUP BY --ROW_NUMBER() OVER (PARTITION BY trans.SubFormId ORDER BY trans.ProductId),
    --    trans.OperationId,
    --    trans.SubFormId,
    --    trans.ProductId,
    --    trans.SubFormName,
    --    trans.SubFormSeq,
    --    trans.ProductCommercialDescription,
    --    trans.ProductCode,
    --    trans.ProductName,
    --    trans.Remarks,
    --    trans.RemarksEng
    ORDER BY trans.SubFormSeq;
    --===========================================================

    --===========================================================
    --Subform-Kha
    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS SlNo,
           busnsCat.NameInBangla AS BusinessCategoryName,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
           dbo.FnGetCalculatedOutputSuppDuty(
                                                (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity,
                                                slsDtl.SupplementaryDutyPercent
                                            ) AS ProductSupplementaryDuty,
           dbo.FnGetCalculatedOutputVat(
                                           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity,
                                           slsDtl.VATPercent,
                                           slsDtl.SupplementaryDutyPercent
                                       ) AS ProductVat,
           N'সরবরাহ' AS Remarks,
           N'Supply' AS RemarksEng --INTO MushakReturnSubFormKha --INTO SpGetMushakNinePointOneSubFormKha
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.BusinessCategory busnsCat
            ON busnsCat.BusinessCategoryId = org.BusinessCategoryId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND slsDtl.ProductVATTypeId = @retailOrWholesaleOrTradeProductVatTypeId;
    --===========================================================

    --===========================================================
    --Subform-Ga

    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS SlNo,
           N'সাবফর্ম- গ(নোট ৬ এর জন্য প্রযোজ্য)' AS SubFormName,
           N'Subform: (For Note- 6)' AS SubFormNameEng,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           mu.Name AS MeasurementUnitName,
           slsDtl.Quantity,
           (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity AS ProductPrice,
           dbo.FnGetCalculatedOutputSuppDuty(
                                                (slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity,
                                                slsDtl.SupplementaryDutyPercent
                                            ) AS ProductSupplementaryDuty,
           slsDtl.Quantity * slsDtl.VATPercent AS ProductVat,
           N'সরবরাহ' AS Remarks,
           N'Supply' AS RemarksEng --INTO MushakReturnSubFormGa --INTO SpGetMushakNinePointOneSubFormGa
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = slsDtl.MeasurementUnitId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND slsDtl.ProductVATTypeId = @fixedVatProdVatTypeId
    UNION ALL
    SELECT ROW_NUMBER() OVER (ORDER BY purchDtl.PurchaseDetailId) AS SlNo,
           N'সাবফর্ম- গ(নোট ১৮ এর জন্য প্রযোজ্য)' AS SubFormName,
           N'Subform: (For Note- 18)' AS SubFormNameEng,
           prod.Name AS ProductCommercialDescription,
           prod.Code AS ProductCode,
           prod.Name AS ProductName,
           mu.Name AS MeasurementUnitName,
           purchDtl.Quantity,
           (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity AS ProductPrice,
           dbo.FnGetCalculatedInputSuppDuty(
                                               (purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity,
                                               purchDtl.SupplementaryDutyPercent
                                           ) AS ProductSupplementaryDuty,
           purchDtl.Quantity * purchDtl.VATPercent AS ProductVat,
           N'ক্রয়' AS Remarks,
           N'Purchase' AS RemarksEng
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purchDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = purchDtl.MeasurementUnitId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purchDtl.ProductVATTypeId = @fixedVatProdVatTypeId;
    --===========================================================

    --===========================================================
    --Subform-Gha
    SELECT ROW_NUMBER() OVER (ORDER BY purch.PurchaseId) AS SlNo,
           purch.PurchaseId,
           vndr.BinNo AS VendorBinNo,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           SUM((purchDtl.UnitPrice - purchDtl.DiscountPerItem) * purchDtl.Quantity) AS Price,
           purch.VDSAmount AS DeductedVat,
           purch.InvoiceNo,
           ISNULL(purch.VatChallanNo, purch.InvoiceNo) AS VatChallanNo,
           purch.PurchaseDate,
           ISNULL(purch.VatChallanIssueDate, purch.PurchaseDate) AS TaxInvoicePrintedTime, --TaxInvoicePrintedTime
           purch.VDSCertificateNo,
           purch.VDSCertificateDate,
           dbo.FnGetEconomicCode(
                                    nec.Code1stDisit,
                                    nec.Code2ndDisit,
                                    nec.Code3rdDisit,
                                    nec.Code4thDisit,
                                    nec.Code5thDisit,
                                    cavc.OperationalCode1stDigit,
                                    cavc.OperationalCode2ndDigit,
                                    cavc.OperationalCode3rdDigit,
                                    cavc.OperationalCode4thDigit,
                                    nec.Code10thDisit,
                                    nec.Code11thDisit,
                                    nec.Code12thDisit,
                                    nec.Code13thDisit
                                ) AS EconomicCode,
           purch.VDSPaymentBookTransferNo AS VDSPaymentBookTransferNo,
           CAST(NULL AS DATETIME) AS VDSPaymentDate,
           purch.VDSNote AS Note                                                           --INTO MushakReturnSubFormGha

    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = purch.OrganizationId
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
        LEFT JOIN dbo.CustomsAndVATCommissionarate cavc
            ON cavc.CustomsAndVATCommissionarateId = ISNULL(
                                                               vndr.CustomsAndVATCommissionarateId,
                                                               org.CustomsAndVATCommissionarateId
                                                           )
        LEFT JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = @NBREconomicCodeForLocalProdVat
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.IsVatDeductedInSource = 1
    GROUP BY purch.PurchaseId,
             vndr.BinNo,
             vndr.Name,
             vndr.Address,
             purch.VDSAmount,
             purch.InvoiceNo,
             purch.VatChallanNo,
             purch.PurchaseDate,
             purch.VatChallanIssueDate,
             purch.VDSCertificateNo,
             purch.VDSCertificateDate,
             nec.Code1stDisit,
             nec.Code2ndDisit,
             nec.Code3rdDisit,
             nec.Code4thDisit,
             nec.Code5thDisit,
             cavc.OperationalCode1stDigit,
             cavc.OperationalCode2ndDigit,
             cavc.OperationalCode3rdDigit,
             cavc.OperationalCode4thDigit,
             nec.Code10thDisit,
             nec.Code11thDisit,
             nec.Code12thDisit,
             nec.Code13thDisit,
             purch.VDSPaymentBookTransferNo,
             purch.VDSNote;
    --===========================================================

    --===========================================================
    --Subform-Uma
    SELECT ROW_NUMBER() OVER (ORDER BY sls.SalesId) AS SlNo,
           sls.SalesId,
           cust.BIN AS CustomerBinNo,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           SUM((slsDtl.UnitPrice - slsDtl.DiscountPerItem) * slsDtl.Quantity) AS Price,
           sls.VDSAmount AS DeductedVat,
           sls.InvoiceNo,
           sls.VatChallanNo,
           sls.SalesDate,
           sls.TaxInvoicePrintedTime,
           sls.VDSCertificateNo,
           sls.VDSCertificateIssueTime,
           sls.VDSPaymentEconomicCode AS EconomicCode,
           sls.VDSPaymentBookTransferNo AS VDSPaymentBookTransferNo,
           sls.VDSPaymentDate AS VDSPaymentDate,
           sls.VDSNote AS Note --INTO MushakReturnSubFormUma
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.OrganizationId = @OrganizationId
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.IsVatDeductedInSource = 1
    GROUP BY sls.SalesId,
             cust.BIN,
             cust.Name,
             cust.Address,
             sls.VDSAmount,
             sls.InvoiceNo,
             sls.VatChallanNo,
             sls.SalesDate,
             sls.TaxInvoicePrintedTime,
             sls.VDSCertificateNo,
             sls.VDSCertificateIssueTime,
             sls.VDSPaymentEconomicCode,
             sls.VDSPaymentDate,
             sls.VDSPaymentBookTransferNo,
             sls.VDSNote;
    --===========================================================

    --===========================================================
    --Subform-Cha
    SELECT ROW_NUMBER() OVER (ORDER BY purch.BillOfEntry) AS SlNo,
           purch.BillOfEntry AS BillOfEntryNo,
           purch.BillOfEntryDate,
           cavatc.Name AS CustomsAndVATCommissionarateName,
           cavatc.NameInBangla AS CustomsAndVATCommissionarateNameInBangla,
           SUM(purchDtl.UnitPrice * purchDtl.Quantity * purchDtl.AdvanceTaxPercent / 100) AS AdvanceTaxAmount,
           purch.AdvanceTaxPaidAmount --INTO MushakReturnSubFormCha
    FROM dbo.Purchase purch
        INNER JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseId = purch.PurchaseId
        INNER JOIN dbo.CustomsAndVATCommissionarate cavatc
            ON cavatc.CustomsAndVATCommissionarateId = purch.CustomsAndVATCommissionarateId
    WHERE purch.OrganizationId = @OrganizationId
          AND purch.CreatedTime >= @firstDayOfMonth
          AND purch.CreatedTime < @firstDayOfNextMonth
          AND purch.PurchaseTypeId = @purchaseTypeImport
    GROUP BY purch.BillOfEntry,
             purch.BillOfEntryDate,
             cavatc.Name,
             cavatc.NameInBangla,
             purch.AdvanceTaxPaidAmount;
    --===========================================================

    --===========================================================
    --Subform-Chha
    SELECT ROW_NUMBER() OVER (ORDER BY mskRtnPmnt.MushakReturnPaymentId) AS SlNo,
           mskRtnPmnt.MushakReturnPaymentId,
           mskRtnPmnt.OrganizationId,
           mskRtnPmnt.MushakYear,
           mskRtnPmnt.MushakMonth,
           mskRtnPmnt.MushakReturnPaymentTypeId,
           mskRtnPmntType.SubFormId,
           mskRtnPmntType.SubFormName,
           mskRtnPmnt.PaidAmount,
           mskRtnPmnt.PaymentDate,
           bnk.BankId,
           bnk.Name AS BankName,
           bnk.NameInBangla AS BankNameInBangla,
           bnkBrnch.BankBranchId,
           bnkBrnch.Name AS BankBranchName,
           bnkBrnch.NameInBangla AS BankBranchNameInBangla,
           mskRtnPmnt.IsSubmitted,
           mskRtnPmnt.TreasuryChallanNo,
           mskRtnPmnt.SubimissionDate,
           nec.NbrEconomicCodeId,
           dbo.FnGetEconomicCode(
                                    nec.Code1stDisit,
                                    nec.Code2ndDisit,
                                    nec.Code3rdDisit,
                                    nec.Code4thDisit,
                                    nec.Code5thDisit,
                                    cavc.OperationalCode1stDigit,
                                    cavc.OperationalCode2ndDigit,
                                    cavc.OperationalCode3rdDigit,
                                    cavc.OperationalCode4thDigit,
                                    nec.Code10thDisit,
                                    nec.Code11thDisit,
                                    nec.Code12thDisit,
                                    nec.Code13thDisit
                                ) AS EconomicCode --INTO MushakReturnSubFormChha
    FROM dbo.MushakReturnPayment mskRtnPmnt
        INNER JOIN dbo.MushakReturnPaymentType mskRtnPmntType
            ON mskRtnPmntType.MushakReturnPaymentTypeId = mskRtnPmnt.MushakReturnPaymentTypeId
        INNER JOIN dbo.CustomsAndVATCommissionarate cavc
            ON cavc.CustomsAndVATCommissionarateId = mskRtnPmnt.CustomsAndVATCommissionarateId
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = mskRtnPmntType.NbrEconomicCodeId
        INNER JOIN dbo.BankBranch bnkBrnch
            ON bnkBrnch.BankBranchId = mskRtnPmnt.BankBranchId
        INNER JOIN dbo.Bank bnk
            ON bnk.BankId = bnkBrnch.BankId
    WHERE mskRtnPmnt.OrganizationId = @OrganizationId
          AND mskRtnPmnt.MushakYear = @Year
          AND mskRtnPmnt.MushakMonth = @Month
          AND mskRtnPmnt.IsSubmitted = 1;
--===========================================================

END;
