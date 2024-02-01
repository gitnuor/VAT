-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetMushakGenerationDepositInfo]
(
    -- Add the parameters for the function here
    @MushakGenerationId INT
)
RETURNS @PaymentInfo TABLE
(
    MushakGenerationInfoId INT NOT NULL,
    MushakGenerationId INT NOT NULL,
    OrganizationId INT NOT NULL,
    PurposeOfPayment NVARCHAR(200) NOT NULL,
    PaidAmount DECIMAL(18, 2) NULL,
    EconomicCodeId INT NULL,
    PaymentDate DATETIME NULL,
    BankBranchId INT NULL,
    ChallanNo NVARCHAR(20) NULL
)
AS
BEGIN
    DECLARE @MushakGeneration TABLE
    (
        [MushakGenerationId] [INT] NOT NULL,
        [OrganizationId] [INT] NOT NULL,
        [PaidVatAmount] [DECIMAL](18, 2) NULL,
        [VatEconomicCodeId] [INT] NULL,
        [VatPaymentDate] [DATETIME] NULL,
        [VatPaymentBankBranchId] [INT] NULL,
        [VatPaymentChallanNo] [NVARCHAR](20) NULL,
        [PaidSuppDutyAmount] [DECIMAL](18, 2) NULL,
        [SuppDutyEconomicCodeId] [INT] NULL,
        [SuppDutyPaymentDate] [DATETIME] NULL,
        [SuppDutyBankBranchId] [INT] NULL,
        [SuppDutyChallanNo] [NVARCHAR](20) NULL,
        [PaidInterestAmountForDueVat] [DECIMAL](18, 2) NULL,
        [InterestForDueVatEconomicCodeId] [INT] NULL,
        [InterestForDueVatPaymentDate] [DATETIME] NULL,
        [InterestForDueVatBankBranchId] [INT] NULL,
        [InterestForDueVatChallanNo] [NVARCHAR](20) NULL,
        [PaidInterestAmountForDueSuppDuty] [DECIMAL](18, 2) NULL,
        [InterestForDueSuppDutyEconomicCodeId] [INT] NULL,
        [InterestForDueSuppDutyPaymentDate] [DATETIME] NULL,
        [InterestForDueSuppDutyBankBranchId] [INT] NULL,
        [InterestForDueSuppDutyChallanNo] [NVARCHAR](20) NULL,
        [PaidFinancialPenalty] [DECIMAL](18, 2) NULL,
        [FinancialPenaltyEconomicCodeId] [INT] NULL,
        [FinancialPenaltyPaymentDate] [DATETIME] NULL,
        [FinancialPenaltyBankBranchId] [INT] NULL,
        [FinancialPenaltyChallanNo] [NVARCHAR](20) NULL,
        [PaidExciseDuty] [DECIMAL](18, 2) NULL,
        [ExciseDutyEconomicCodeId] [INT] NULL,
        [ExciseDutyPaymentDate] [DATETIME] NULL,
        [ExciseDutyBankBranchId] [INT] NULL,
        [ExciseDutyChallanNo] [NVARCHAR](20) NULL,
        [PaidDevelopmentSurcharge] [DECIMAL](18, 2) NULL,
        [DevelopmentSurchargeEconomicCodeId] [INT] NULL,
        [DevelopmentSurchargePaymentDate] [DATETIME] NULL,
        [DevelopmentSurchargeBankBranchId] [INT] NULL,
        [DevelopmentSurchargeChallanNo] [NVARCHAR](20) NULL,
        [PaidItDevelopmentSurcharge] [DECIMAL](18, 2) NULL,
        [ItDevelopmentSurchargeEconomicCodeId] [INT] NULL,
        [ItDevelopmentSurchargePaymentDate] [DATETIME] NULL,
        [ItDevelopmentSurchargeBankBranchId] [INT] NULL,
        [ItDevelopmentSurchargeChallanNo] [NVARCHAR](20) NULL,
        [PaidHealthDevelopmentSurcharge] [DECIMAL](18, 2) NULL,
        [HealthDevelopmentSurchargeEconomicCodeId] [INT] NULL,
        [HealthDevelopmentSurchargePaymentDate] [DATETIME] NULL,
        [HealthDevelopmentSurchargeBankBranchId] [INT] NULL,
        [HealthDevelopmentSurchargeChallanNo] [NVARCHAR](20) NULL,
        [PaidEnvironmentProtectSurcharge] [DECIMAL](18, 2) NULL,
        [EnvironmentProtectSurchargeEconomicCodeId] [INT] NULL,
        [EnvironmentProtectSurchargePaymentDate] [DATETIME] NULL,
        [EnvironmentProtectSurchargeBankBranchId] [INT] NULL,
        [EnvironmentProtectSurchargeChallanNo] [NVARCHAR](20) NULL
    );

    INSERT INTO @MushakGeneration
    (
        MushakGenerationId,
        OrganizationId,
        PaidVatAmount,
        VatEconomicCodeId,
        VatPaymentDate,
        VatPaymentBankBranchId,
        VatPaymentChallanNo,
        PaidSuppDutyAmount,
        SuppDutyEconomicCodeId,
        SuppDutyPaymentDate,
        SuppDutyBankBranchId,
        SuppDutyChallanNo,
        PaidInterestAmountForDueVat,
        InterestForDueVatEconomicCodeId,
        InterestForDueVatPaymentDate,
        InterestForDueVatBankBranchId,
        InterestForDueVatChallanNo,
        PaidInterestAmountForDueSuppDuty,
        InterestForDueSuppDutyEconomicCodeId,
        InterestForDueSuppDutyPaymentDate,
        InterestForDueSuppDutyBankBranchId,
        InterestForDueSuppDutyChallanNo,
        PaidFinancialPenalty,
        FinancialPenaltyEconomicCodeId,
        FinancialPenaltyPaymentDate,
        FinancialPenaltyBankBranchId,
        FinancialPenaltyChallanNo,
        PaidExciseDuty,
        ExciseDutyEconomicCodeId,
        ExciseDutyPaymentDate,
        ExciseDutyBankBranchId,
        ExciseDutyChallanNo,
        PaidDevelopmentSurcharge,
        DevelopmentSurchargeEconomicCodeId,
        DevelopmentSurchargePaymentDate,
        DevelopmentSurchargeBankBranchId,
        DevelopmentSurchargeChallanNo,
        PaidItDevelopmentSurcharge,
        ItDevelopmentSurchargeEconomicCodeId,
        ItDevelopmentSurchargePaymentDate,
        ItDevelopmentSurchargeBankBranchId,
        ItDevelopmentSurchargeChallanNo,
        PaidHealthDevelopmentSurcharge,
        HealthDevelopmentSurchargeEconomicCodeId,
        HealthDevelopmentSurchargePaymentDate,
        HealthDevelopmentSurchargeBankBranchId,
        HealthDevelopmentSurchargeChallanNo,
        PaidEnvironmentProtectSurcharge,
        EnvironmentProtectSurchargeEconomicCodeId,
        EnvironmentProtectSurchargePaymentDate,
        EnvironmentProtectSurchargeBankBranchId,
        EnvironmentProtectSurchargeChallanNo
    )
    SELECT mg.MushakGenerationId,
           mg.OrganizationId,
           mg.PaidVatAmount,
           mg.VatEconomicCodeId,
           mg.VatPaymentDate,
           mg.VatPaymentBankBranchId,
           mg.VatPaymentChallanNo,
           mg.PaidSuppDutyAmount,
           mg.SuppDutyEconomicCodeId,
           mg.SuppDutyPaymentDate,
           mg.SuppDutyBankBranchId,
           mg.SuppDutyChallanNo,
           mg.PaidInterestAmountForDueVat,
           mg.InterestForDueVatEconomicCodeId,
           mg.InterestForDueVatPaymentDate,
           mg.InterestForDueVatBankBranchId,
           mg.InterestForDueVatChallanNo,
           mg.PaidInterestAmountForDueSuppDuty,
           mg.InterestForDueSuppDutyEconomicCodeId,
           mg.InterestForDueSuppDutyPaymentDate,
           mg.InterestForDueSuppDutyBankBranchId,
           mg.InterestForDueSuppDutyChallanNo,
           mg.PaidFinancialPenalty,
           mg.FinancialPenaltyEconomicCodeId,
           mg.FinancialPenaltyPaymentDate,
           mg.FinancialPenaltyBankBranchId,
           mg.FinancialPenaltyChallanNo,
           mg.PaidExciseDuty,
           mg.ExciseDutyEconomicCodeId,
           mg.ExciseDutyPaymentDate,
           mg.ExciseDutyBankBranchId,
           mg.ExciseDutyChallanNo,
           mg.PaidDevelopmentSurcharge,
           mg.DevelopmentSurchargeEconomicCodeId,
           mg.DevelopmentSurchargePaymentDate,
           mg.DevelopmentSurchargeBankBranchId,
           mg.DevelopmentSurchargeChallanNo,
           mg.PaidItDevelopmentSurcharge,
           mg.ItDevelopmentSurchargeEconomicCodeId,
           mg.ItDevelopmentSurchargePaymentDate,
           mg.ItDevelopmentSurchargeBankBranchId,
           mg.ItDevelopmentSurchargeChallanNo,
           mg.PaidHealthDevelopmentSurcharge,
           mg.HealthDevelopmentSurchargeEconomicCodeId,
           mg.HealthDevelopmentSurchargePaymentDate,
           mg.HealthDevelopmentSurchargeBankBranchId,
           mg.HealthDevelopmentSurchargeChallanNo,
           mg.PaidEnvironmentProtectSurcharge,
           mg.EnvironmentProtectSurchargeEconomicCodeId,
           mg.EnvironmentProtectSurchargePaymentDate,
           mg.EnvironmentProtectSurchargeBankBranchId,
           mg.EnvironmentProtectSurchargeChallanNo
    FROM dbo.MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 1,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'মূসক',
           mg.PaidVatAmount,
           mg.VatEconomicCodeId,
           mg.VatPaymentDate,
           mg.VatPaymentBankBranchId,
           mg.VatPaymentChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidVatAmount > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 2,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'সম্পূরক শুল্ক',
           mg.PaidSuppDutyAmount,
           mg.SuppDutyEconomicCodeId,
           mg.SuppDutyPaymentDate,
           mg.SuppDutyBankBranchId,
           mg.SuppDutyChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidSuppDutyAmount > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 3,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'অপরিশোধিত মূসকের জন্য সুদ',
           mg.PaidInterestAmountForDueVat,
           mg.InterestForDueVatEconomicCodeId,
           mg.InterestForDueVatPaymentDate,
           mg.InterestForDueVatBankBranchId,
           mg.InterestForDueVatChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidInterestAmountForDueVat > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 4,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'অপরিশোধিত সম্পূরক শুল্ক এর জন্য সুদ',
           mg.PaidInterestAmountForDueSuppDuty,
           mg.InterestForDueSuppDutyEconomicCodeId,
           mg.InterestForDueSuppDutyPaymentDate,
           mg.InterestForDueSuppDutyBankBranchId,
           mg.InterestForDueSuppDutyChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidInterestAmountForDueSuppDuty > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 5,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'অর্থদন্ড ও জরিমানা',
           mg.PaidFinancialPenalty,
           mg.FinancialPenaltyEconomicCodeId,
           mg.FinancialPenaltyPaymentDate,
           mg.FinancialPenaltyBankBranchId,
           mg.FinancialPenaltyChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidFinancialPenalty > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 6,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'আবগারি শুল্ক',
           mg.PaidExciseDuty,
           mg.ExciseDutyEconomicCodeId,
           mg.ExciseDutyPaymentDate,
           mg.ExciseDutyBankBranchId,
           mg.ExciseDutyChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidExciseDuty > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 7,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'উন্নয়ন সারচার্জ',
           mg.PaidDevelopmentSurcharge,
           mg.DevelopmentSurchargeEconomicCodeId,
           mg.DevelopmentSurchargePaymentDate,
           mg.DevelopmentSurchargeBankBranchId,
           mg.DevelopmentSurchargeChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidDevelopmentSurcharge > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 8,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'তথ্য প্রযুক্তি উন্নয়ন সারচার্জ',
           mg.PaidItDevelopmentSurcharge,
           mg.ItDevelopmentSurchargeEconomicCodeId,
           mg.ItDevelopmentSurchargePaymentDate,
           mg.ItDevelopmentSurchargeBankBranchId,
           mg.ItDevelopmentSurchargeChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidItDevelopmentSurcharge > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 9,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'স্বাস্থ্য সুরক্ষা সারচার্জ',
           mg.PaidHealthDevelopmentSurcharge,
           mg.HealthDevelopmentSurchargeEconomicCodeId,
           mg.HealthDevelopmentSurchargePaymentDate,
           mg.HealthDevelopmentSurchargeBankBranchId,
           mg.HealthDevelopmentSurchargeChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidHealthDevelopmentSurcharge > 0;

    INSERT INTO @PaymentInfo
    (
        MushakGenerationInfoId,
        MushakGenerationId,
        OrganizationId,
        PurposeOfPayment,
        PaidAmount,
        EconomicCodeId,
        PaymentDate,
        BankBranchId,
        ChallanNo
    )
    SELECT 10,
           mg.MushakGenerationId,
           mg.OrganizationId,
           N'পরিবেশ সুরক্ষা সারচার্জ',
           mg.PaidEnvironmentProtectSurcharge,
           mg.EnvironmentProtectSurchargeEconomicCodeId,
           mg.EnvironmentProtectSurchargePaymentDate,
           mg.EnvironmentProtectSurchargeBankBranchId,
           mg.EnvironmentProtectSurchargeChallanNo
    FROM @MushakGeneration mg
    WHERE mg.MushakGenerationId = @MushakGenerationId AND mg.PaidEnvironmentProtectSurcharge > 0;
    RETURN;
END;
