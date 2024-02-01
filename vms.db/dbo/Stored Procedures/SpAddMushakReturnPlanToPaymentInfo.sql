-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnPlanToPaymentInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT,
    @PaidVatAmount DECIMAL(18, 2),
    @VatEconomicCodeId INT,
    @VatPaymentDate DATETIME,
    @VatPaymentBankBranchId INT,
    @PaidSuppDutyAmount DECIMAL(18, 2),
    @SuppDutyEconomicCodeId INT,
    @SuppDutyPaymentDate DATETIME,
    @SuppDutyBankBranchId INT,
    @PaidInterestAmountForDueVat DECIMAL(18, 2),
    @InterestForDueVatEconomicCodeId INT,
    @InterestForDueVatPaymentDate DATETIME,
    @InterestForDueVatBankBranchId INT,
    @PaidInterestAmountForDueSuppDuty DECIMAL(18, 2),
    @InterestForDueSuppDutyEconomicCodeId INT,
    @InterestForDueSuppDutyPaymentDate DATETIME,
    @InterestForDueSuppDutyBankBranchId INT,
    @PaidFinancialPenalty DECIMAL(18, 2),
    @FinancialPenaltyEconomicCodeId INT,
    @FinancialPenaltyPaymentDate DATETIME,
    @FinancialPenaltyBankBranchId INT,
    @PaidExciseDuty DECIMAL(18, 2),
    @ExciseDutyEconomicCodeId INT,
    @ExciseDutyPaymentDate DATETIME,
    @ExciseDutyBankBranchId INT,
    @PaidDevelopmentSurcharge DECIMAL(18, 2),
    @DevelopmentSurchargeEconomicCodeId INT,
    @DevelopmentSurchargePaymentDate DATETIME,
    @DevelopmentSurchargeBankBranchId INT,
    @PaidItDevelopmentSurcharge DECIMAL(18, 2),
    @ItDevelopmentSurchargeEconomicCodeId INT,
    @ItDevelopmentSurchargePaymentDate DATETIME,
    @ItDevelopmentSurchargeBankBranchId INT,
    @PaidHealthDevelopmentSurcharge DECIMAL(18, 2),
    @HealthDevelopmentSurchargeEconomicCodeId INT,
    @HealthDevelopmentSurchargePaymentDate DATETIME,
    @HealthDevelopmentSurchargeBankBranchId INT,
    @PaidEnvironmentProtectSurcharge DECIMAL(18, 2),
    @EnvironmentProtectSurchargeEconomicCodeId INT,
    @EnvironmentProtectSurchargePaymentDate DATETIME,
    @EnvironmentProtectSurchargeBankBranchId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[MushakGeneration]
    SET PaidVatAmount = @PaidVatAmount,
        VatEconomicCodeId = @VatEconomicCodeId,
        VatPaymentDate = @VatPaymentDate,
        VatPaymentBankBranchId = @VatPaymentBankBranchId,
        PaidSuppDutyAmount = @PaidSuppDutyAmount,
        SuppDutyEconomicCodeId = @SuppDutyEconomicCodeId,
        SuppDutyPaymentDate = @SuppDutyPaymentDate,
        SuppDutyBankBranchId = @SuppDutyBankBranchId,
        PaidInterestAmountForDueVat = @PaidInterestAmountForDueVat,
        InterestForDueVatEconomicCodeId = @InterestForDueVatEconomicCodeId,
        InterestForDueVatPaymentDate = @InterestForDueVatPaymentDate,
        InterestForDueVatBankBranchId = @InterestForDueVatBankBranchId,
        PaidInterestAmountForDueSuppDuty = @PaidInterestAmountForDueSuppDuty,
        InterestForDueSuppDutyEconomicCodeId = @InterestForDueSuppDutyEconomicCodeId,
        InterestForDueSuppDutyPaymentDate = @InterestForDueSuppDutyPaymentDate,
        InterestForDueSuppDutyBankBranchId = @InterestForDueSuppDutyBankBranchId,
        PaidFinancialPenalty = @PaidFinancialPenalty,
        FinancialPenaltyEconomicCodeId = @FinancialPenaltyEconomicCodeId,
        FinancialPenaltyPaymentDate = @FinancialPenaltyPaymentDate,
        FinancialPenaltyBankBranchId = @FinancialPenaltyBankBranchId,
        PaidExciseDuty = @PaidExciseDuty,
        ExciseDutyEconomicCodeId = @ExciseDutyEconomicCodeId,
        ExciseDutyPaymentDate = @ExciseDutyPaymentDate,
        ExciseDutyBankBranchId = @ExciseDutyBankBranchId,
        PaidDevelopmentSurcharge = @PaidDevelopmentSurcharge,
        DevelopmentSurchargeEconomicCodeId = @DevelopmentSurchargeEconomicCodeId,
        DevelopmentSurchargePaymentDate = @DevelopmentSurchargePaymentDate,
        DevelopmentSurchargeBankBranchId = @DevelopmentSurchargeBankBranchId,
        PaidItDevelopmentSurcharge = @PaidItDevelopmentSurcharge,
        ItDevelopmentSurchargeEconomicCodeId = @ItDevelopmentSurchargeEconomicCodeId,
        ItDevelopmentSurchargePaymentDate = @ItDevelopmentSurchargePaymentDate,
        ItDevelopmentSurchargeBankBranchId = @ItDevelopmentSurchargeBankBranchId,
        PaidHealthDevelopmentSurcharge = @PaidHealthDevelopmentSurcharge,
        HealthDevelopmentSurchargeEconomicCodeId = @HealthDevelopmentSurchargeEconomicCodeId,
        HealthDevelopmentSurchargePaymentDate = @HealthDevelopmentSurchargePaymentDate,
        HealthDevelopmentSurchargeBankBranchId = @HealthDevelopmentSurchargeBankBranchId,
        PaidEnvironmentProtectSurcharge = @PaidEnvironmentProtectSurcharge,
        EnvironmentProtectSurchargeEconomicCodeId = @EnvironmentProtectSurchargeEconomicCodeId,
        EnvironmentProtectSurchargePaymentDate = @EnvironmentProtectSurchargePaymentDate,
        EnvironmentProtectSurchargeBankBranchId = @EnvironmentProtectSurchargeBankBranchId,
        MushakGenerationStageId = 2
    WHERE MushakGenerationId = @MushakGenerationId;
END;
