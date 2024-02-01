
USE [vms_empty]
GO
/****** Object:  User [BRACITS\Administrator]    Script Date: 12/1/2019 2:25:54 PM ******/
CREATE USER [BRACITS\Administrator] FOR LOGIN [BRACITS\Administrator] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Schema [acc]    Script Date: 12/1/2019 2:25:54 PM ******/
CREATE SCHEMA [acc]
GO
/****** Object:  UserDefinedFunction [dbo].[FnConvertIntToBanglaUnicodeNumber]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnConvertIntToBanglaUnicodeNumber]
(
    -- Add the parameters for the function here
    @intNumber INT
)
RETURNS NVARCHAR(50)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @position INT = 1,
            @intNumberInNvarchar NVARCHAR(50) = CAST(@intNumber AS NVARCHAR(50)),
            @result NVARCHAR(50) = N'',
            @unicodeDiff INT,
            @charUnicode INT;

    SET @unicodeDiff = (UNICODE(N'১') - UNICODE(N'1'));
    WHILE @position <= LEN(@intNumberInNvarchar)
    BEGIN
        SET @charUnicode = UNICODE(SUBSTRING(@intNumberInNvarchar, @position, 1)) + @unicodeDiff;
        SET @result += NCHAR(@charUnicode);
        SET @position += 1;
    END;

    -- Return the result of the function
    RETURN @result;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetCalculatedInputSuppDuty]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetCalculatedInputSuppDuty]
(
    -- Add the parameters for the function here
    @DutiableValue DECIMAL(18, 2),
    @SupplimentaryDutyPercent DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Return the result of the function
    RETURN @DutiableValue * @SupplimentaryDutyPercent / 100;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetCalculatedInputVat]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetCalculatedInputVat]
(
    -- Add the parameters for the function here
    @DutiableValue DECIMAL(18, 2),
    @VatPercent DECIMAL(18, 2),
    @SupplimentaryDutyPercent DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Return the result of the function
    RETURN (@DutiableValue + (@DutiableValue * @SupplimentaryDutyPercent / 100)) * @VatPercent / 100;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetCalculatedOutputSuppDuty]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetCalculatedOutputSuppDuty]
(
    -- Add the parameters for the function here
    @DutiableValue DECIMAL(18, 2),
    @SupplimentaryDutyPercent DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Return the result of the function
    RETURN @DutiableValue * @SupplimentaryDutyPercent / 100;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetCalculatedOutputVat]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetCalculatedOutputVat]
(
    -- Add the parameters for the function here
    @DutiableValue DECIMAL(18, 2),
    @VatPercent DECIMAL(18, 2),
    @SupplimentaryDutyPercent DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Return the result of the function
    RETURN (@DutiableValue + (@DutiableValue * @SupplimentaryDutyPercent / 100)) * @VatPercent / 100;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetCalculatedVat]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetCalculatedVat]
(
    -- Add the parameters for the function here
    @DutiableValue DECIMAL(18, 2),
    @VatPercent DECIMAL(18, 2),
    @SupplimentaryDutyPercent DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @CalculatedVat DECIMAL(18, 2),
            @BaseValue DECIMAL(18, 2);

    SET @BaseValue = @DutiableValue + (@DutiableValue * @SupplimentaryDutyPercent / (@SupplimentaryDutyPercent + 100));

    -- Add the T-SQL statements to compute the return value here
    SET @CalculatedVat = @BaseValue * @VatPercent / (@VatPercent + 100);

    -- Return the result of the function
    RETURN @CalculatedVat;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetListOfOrganizationIdWithChild]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetListOfOrganizationIdWithChild]
(
    -- Add the parameters for the function here
    @OrganizationId INT
)
RETURNS @Organization TABLE
(
    OrganizationId INT NOT NULL
)
AS
BEGIN
    INSERT INTO @Organization
    (
        OrganizationId
    )
    SELECT org.OrganizationId
    FROM dbo.Organizations org
    WHERE org.OrganizationId = @OrganizationId;
    RETURN;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetMushakGenerationDepositInfo]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
    WHERE mg.MushakGenerationId = @MushakGenerationId;
    RETURN;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetNameOfMonthInBanglaByNumber]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetNameOfMonthInBanglaByNumber]
(
    -- Add the parameters for the function here
    @numberOfMonth INT
)
RETURNS NVARCHAR(20)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @nameOfMonthInBangla NVARCHAR(20);

    -- Add the T-SQL statements to compute the return value here
    IF @numberOfMonth = 1
    BEGIN
        SET @nameOfMonthInBangla = N'জানুয়ারী';
    END;
    ELSE IF @numberOfMonth = 2
    BEGIN
        SET @nameOfMonthInBangla = N'ফেব্রুয়ারি';
    END;
    ELSE IF @numberOfMonth = 3
    BEGIN
        SET @nameOfMonthInBangla = N'মার্চ';
    END;
    ELSE IF @numberOfMonth = 4
    BEGIN
        SET @nameOfMonthInBangla = N'এপ্রিল';
    END;
    ELSE IF @numberOfMonth = 5
    BEGIN
        SET @nameOfMonthInBangla = N'মে';
    END;
    ELSE IF @numberOfMonth = 6
    BEGIN
        SET @nameOfMonthInBangla = N'জুন';
    END;
    ELSE IF @numberOfMonth = 7
    BEGIN
        SET @nameOfMonthInBangla = N'জুলাই';
    END;
    ELSE IF @numberOfMonth = 8
    BEGIN
        SET @nameOfMonthInBangla = N'অগাস্ট';
    END;
    ELSE IF @numberOfMonth = 9
    BEGIN
        SET @nameOfMonthInBangla = N'সেপ্টেম্বর';
    END;
    ELSE IF @numberOfMonth = 10
    BEGIN
        SET @nameOfMonthInBangla = N'অক্টোবর';
    END;
    ELSE IF @numberOfMonth = 11
    BEGIN
        SET @nameOfMonthInBangla = N'নভেম্বর';
    END;
    ELSE IF @numberOfMonth = 12
    BEGIN
        SET @nameOfMonthInBangla = N'ডিসেম্বর';
    END;
    ELSE
    BEGIN
        SET @nameOfMonthInBangla = N'ভুল ইনপুট';
    END;

    -- Return the result of the function
    RETURN @nameOfMonthInBangla;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetProductLastPurchasePrice]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetProductLastPurchasePrice]
(
    -- Add the parameters for the function here
    @ProductId INT
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @lastPurchaseRate DECIMAL(18, 2);

    -- Add the T-SQL statements to compute the return value here
    SELECT TOP (1)
           @lastPurchaseRate = purchDtl.UnitPrice
    FROM dbo.PurchaseDetails purchDtl
    WHERE purchDtl.ProductId = @ProductId
    ORDER BY purchDtl.PurchaseDetailId DESC;

    -- Return the result of the function
    RETURN @lastPurchaseRate;

END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetPurchaseCalcBookBase]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetPurchaseCalcBookBase]
(
    -- Add the parameters for the function here
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
    @VendorId INT,
    @ProductId INT
)
RETURNS @PurchaseCalcBookBase TABLE
(
    SlNo BIGINT NOT NULL,
    StockInId BIGINT NOT NULL,
    ProductId INT NOT NULL,
    OperationTime DATETIME NOT NULL,
    OperationType VARCHAR(10) NOT NULL,
    OrganizationId INT NOT NULL,
    InitUnitPriceWithoutVat DECIMAL(18, 2) NULL,
    EndUnitPriceWithoutVat DECIMAL(18, 2) NULL,
    PurchaseId INT NOT NULL,
    PurchaseDetailId INT NOT NULL,
    UsedInProductionQty DECIMAL(18, 2) NULL,
    PurchaseQty DECIMAL(18, 2) NULL,
    PurchaseUnitPrice DECIMAL(18, 2) NULL,
    MeasurementUnitId INT NOT NULL
)
AS
BEGIN
    IF @ToDate IS NOT NULL
    BEGIN
        SET @ToDate = DATEADD(DAY, 1, @ToDate);
    END;

    INSERT INTO @PurchaseCalcBookBase
    (
        SlNo,
        StockInId,
        ProductId,
        OperationTime,
        OperationType,
        OrganizationId,
        InitUnitPriceWithoutVat,
        EndUnitPriceWithoutVat,
        PurchaseId,
        PurchaseDetailId,
        UsedInProductionQty,
        PurchaseQty,
        PurchaseUnitPrice,
        MeasurementUnitId
    )
    SELECT ROW_NUMBER() OVER (ORDER BY trns.OperationTime) AS SlNo,
           trns.StockInId,
           trns.ProductId,
           trns.OperationTime,
           trns.OperationType,
           trns.OrganizationId,
           trns.InitUnitPriceWithoutVat,
           trns.EndUnitPriceWithoutVat,
           trns.PurchaseId,
           trns.PurchaseDetailId,
           trns.UsedInProductionQty,
           trns.PurchaseQty,
           trns.PurchaseUnitPrice,
           trns.MeasurementUnitId
    FROM
    (
        SELECT si.StockInId,
               si.ProductId,
               si.CreatedTime AS OperationTime,
               'Purchase' AS OperationType,
               si.OrganizationId,
               si.InitUnitPriceWithoutVat,
               si.EndUnitPriceWithoutVat,
               purch.PurchaseId AS PurchaseId,
               purchDtl.PurchaseDetailId AS PurchaseDetailId,
               CAST(NULL AS DECIMAL(18, 2)) AS UsedInProductionQty,
               purchDtl.Quantity AS PurchaseQty,
               purchDtl.UnitPrice AS PurchaseUnitPrice,
               si.MeasurementUnitId
        FROM dbo.StockIn si
            INNER JOIN dbo.Organizations org
                ON org.OrganizationId = si.OrganizationId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = si.ProductId
            INNER JOIN dbo.PurchaseDetails purchDtl
                ON purchDtl.PurchaseDetailId = si.PurchaseDetailId
            INNER JOIN dbo.Purchase purch
                ON purch.PurchaseId = purchDtl.PurchaseId
        WHERE si.IsFinishedGoods = 0
              AND EXISTS
        (
            SELECT fnOrg.OrganizationId
            FROM [dbo].[FnGetListOfOrganizationIdWithChild](@OrganizationId) fnOrg
            WHERE fnOrg.OrganizationId = si.OrganizationId
        )
              --AND sls.SalesTypeId <> 3
              AND
              (
                  @FromDate IS NULL
                  OR purch.PurchaseDate >= @FromDate
              )
              AND
              (
                  @ToDate IS NULL
                  OR purch.PurchaseDate < @ToDate
              )
              AND
              (
                  @VendorId = 0
                  OR @VendorId IS NULL
                  OR purch.VendorId = @VendorId
              )
              AND
              (
                  @ProductId = 0
                  OR @ProductId IS NULL
                  OR si.ProductId = @ProductId
              )
        UNION ALL
        SELECT si.StockInId,
               si.ProductId,
               prdcnRcv.CreatedTime AS OperationTime,
               'Production' AS OperationType,
               si.OrganizationId,
               si.InitUnitPriceWithoutVat,
               si.EndUnitPriceWithoutVat,
               CAST(0 AS INT) AS PurchaseId,
               CAST(0 AS INT) AS PurchaseDetailId,
               bom.UsedQuantity AS UsedInProductionQty,
               CAST(NULL AS DECIMAL(18, 2)) AS PurchaseQty,
               CAST(NULL AS DECIMAL(18, 2)) AS PurchaseUnitPrice,
               si.MeasurementUnitId
        FROM dbo.StockIn si
            INNER JOIN dbo.Organizations org
                ON org.OrganizationId = si.OrganizationId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = si.ProductId
            INNER JOIN dbo.BillOfMaterial bom
                ON bom.StockInId = si.StockInId
            INNER JOIN dbo.ProductionReceive prdcnRcv
                ON prdcnRcv.ProductionReceiveId = bom.ProductionReceiveId
        WHERE si.IsFinishedGoods = 0
              AND EXISTS
        (
            SELECT fnOrg.OrganizationId
            FROM [dbo].[FnGetListOfOrganizationIdWithChild](@OrganizationId) fnOrg
            WHERE fnOrg.OrganizationId = si.OrganizationId
        )
              AND
              (
                  @FromDate IS NULL
                  OR si.CreatedTime >= @FromDate
              )
              AND
              (
                  @ToDate IS NULL
                  OR si.CreatedTime < @ToDate
              )
              AND
              (
                  @VendorId = 0
                  OR @VendorId IS NULL
                  OR si.CreatedTime = @VendorId
              )
              AND
              (
                  @ProductId = 0
                  OR @ProductId IS NULL
                  OR si.ProductId = @ProductId
              )
    ) trns
    ORDER BY trns.OperationTime;

    RETURN;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[FnGetSalesCalcBookBase]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetSalesCalcBookBase]
(
    -- Add the parameters for the function here
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
    @CustomerId INT,
    @ProductId INT
)
RETURNS @PurchaseCalcBookBase TABLE
(
    SlNo BIGINT NOT NULL,
    StockInId BIGINT NOT NULL,
    ProductId INT NOT NULL,
    OperationTime DATETIME NOT NULL,
    OperationType VARCHAR(10) NOT NULL,
    OrganizationId INT NOT NULL,
    SalesId INT NOT NULL,
    SalesDetailId INT NOT NULL,
    ProductionQty DECIMAL(18, 2) NULL,
    SalesQty DECIMAL(18, 2) NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL,
    MeasurementUnitId INT NOT NULL
)
AS
BEGIN
    IF @ToDate IS NOT NULL
    BEGIN
        SET @ToDate = DATEADD(DAY, 1, @ToDate);
    END;

    INSERT INTO @PurchaseCalcBookBase
    (
        SlNo,
        StockInId,
        ProductId,
        OperationTime,
        OperationType,
        OrganizationId,
        SalesId,
        SalesDetailId,
        ProductionQty,
        SalesQty,
        UnitPrice,
        MeasurementUnitId
    )
    SELECT ROW_NUMBER() OVER (ORDER BY trns.OperationTime) AS SlNo,
           trns.StockInId,
           trns.ProductId,
           trns.OperationTime,
           trns.OperationType,
           trns.OrganizationId,
           trns.SalesId,
           trns.SalesDetailId,
           trns.ProductionQty,
           trns.SalesQty,
           trns.UnitPrice,
           trns.MeasurementUnitId
    FROM
    (
        SELECT si.StockInId,
               si.ProductId,
               si.CreatedTime AS OperationTime,
               'Production' AS OperationType,
               si.OrganizationId,
               CAST(0 AS INT) AS SalesId,
               CAST(0 AS INT) AS SalesDetailId,
               si.InQuantity AS ProductionQty,
               CAST(NULL AS DECIMAL(18, 2)) AS SalesQty,
               prcStup.SalesUnitPrice AS UnitPrice,
               si.MeasurementUnitId
        FROM dbo.StockIn si
            INNER JOIN dbo.Organizations org
                ON org.OrganizationId = si.OrganizationId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = si.ProductId
            INNER JOIN dbo.PriceSetup prcStup
                ON prcStup.ProductId = prod.ProductId
        WHERE si.IsFinishedGoods = 1
              AND si.CreatedTime >= prcStup.EffectiveFrom
              AND
              (
                  prcStup.EffectiveTo IS NULL
                  OR prcStup.EffectiveTo <= si.CreatedTime
              )
              AND EXISTS
        (
            SELECT fnOrg.OrganizationId
            FROM [dbo].[FnGetListOfOrganizationIdWithChild](@OrganizationId) fnOrg
            WHERE fnOrg.OrganizationId = si.OrganizationId
        )
              AND
              (
                  @FromDate IS NULL
                  OR si.CreatedTime >= @FromDate
              )
              AND
              (
                  @ToDate IS NULL
                  OR si.CreatedTime < @ToDate
              )
              AND
              (
                  @CustomerId = 0
                  OR @CustomerId IS NULL
                  OR si.CreatedTime = @CustomerId
              )
              AND
              (
                  @ProductId = 0
                  OR @ProductId IS NULL
                  OR si.ProductId = @ProductId
              )
        UNION ALL
        SELECT si.StockInId,
               si.ProductId,
               sls.CreatedTime AS OperationTime,
               'Sales' AS OperationType,
               si.OrganizationId,
               sls.SalesId AS SalesId,
               slsDtl.SalesDetailId AS SalesDetailId,
               CAST(NULL AS DECIMAL(18, 2)) AS ProductionQty,
               slsDtl.Quantity AS SalesQty,
               slsDtl.UnitPrice AS UnitPrice,
               si.MeasurementUnitId
        FROM dbo.StockIn si
            INNER JOIN dbo.Organizations org
                ON org.OrganizationId = si.OrganizationId
            INNER JOIN dbo.Products prod
                ON prod.ProductId = si.ProductId
            INNER JOIN dbo.SalesDetails slsDtl
                ON slsDtl.StockInId = si.StockInId
            INNER JOIN dbo.Sales sls
                ON sls.SalesId = slsDtl.SalesId
        WHERE si.IsFinishedGoods = 1
              AND EXISTS
        (
            SELECT fnOrg.OrganizationId
            FROM [dbo].[FnGetListOfOrganizationIdWithChild](@OrganizationId) fnOrg
            WHERE fnOrg.OrganizationId = si.OrganizationId
        )
              --AND sls.SalesTypeId <> 3
              AND
              (
                  @FromDate IS NULL
                  OR sls.SalesDate >= @FromDate
              )
              AND
              (
                  @ToDate IS NULL
                  OR sls.SalesDate < @ToDate
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
                  OR si.ProductId = @ProductId
              )
    ) trns
    ORDER BY trns.OperationTime;

    RETURN;
END;
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[CustomerOrganizationId] [int] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[BIN] [nvarchar](20) NULL,
	[NIDNo] [nvarchar](50) NULL,
	[CountryId] [int] NULL,
	[Address] [nvarchar](200) NULL,
	[PhoneNo] [nvarchar](20) NULL,
	[EmailAddress] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[OrganizationId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentOrganizationId] [int] NULL,
	[Code] [nvarchar](50) NULL,
	[VATRegNo] [nvarchar](50) NULL,
	[BIN] [nvarchar](50) NULL,
	[FinancialActivityNatureId] [int] NOT NULL,
	[BusinessNatureId] [int] NOT NULL,
	[BusinessCategoryId] [int] NOT NULL,
	[BusinessCategoryDescription] [nvarchar](500) NULL,
	[IsDeductVatInSource] [bit] NOT NULL,
	[IsSellStandardVatProduct] [bit] NOT NULL,
	[CertificateNo] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NOT NULL,
	[CountryId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[VatResponsiblePersonName] [nvarchar](100) NOT NULL,
	[VatResponsiblePersonDesignation] [nvarchar](50) NOT NULL,
	[VatResponsiblePersonMobileNo] [nvarchar](100) NOT NULL,
	[VatResponsiblePersonEmailAddress] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[EnlistedNo] [int] NULL,
	[PostalCode] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SalesId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNo] [nvarchar](50) NOT NULL,
	[VatChallanNo] [nvarchar](50) NULL,
	[OrganizationId] [int] NOT NULL,
	[NoOfIteams] [int] NOT NULL,
	[TotalPriceWithoutVat] [decimal](18, 2) NOT NULL,
	[DiscountOnTotalPrice] [decimal](18, 2) NOT NULL,
	[TotalDiscountOnIndividualProduct] [decimal](18, 2) NOT NULL,
	[TotalVAT] [decimal](18, 2) NOT NULL,
	[TotalSupplimentaryDuty] [decimal](18, 2) NOT NULL,
	[IsVatDeductedInSource] [bit] NOT NULL,
	[VDSAmount] [decimal](18, 2) NULL,
	[VDSPaymentBankBranchId] [int] NULL,
	[VDSPaymentDate] [datetime] NULL,
	[VDSPaymentChallanNo] [nvarchar](20) NULL,
	[VDSPaymentEconomicCode] [nvarchar](20) NULL,
	[ReceivableAmount]  AS (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)),
	[PaymentReceiveAmount] [decimal](18, 2) NULL,
	[PaymentDueAmount]  AS (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)-[PaymentReceiveAmount]),
	[CustomerId] [int] NULL,
	[ReceiverName] [nvarchar](200) NULL,
	[ReceiverContactNo] [varchar](20) NULL,
	[ShippingAddress] [nvarchar](200) NULL,
	[ShippingCountryId] [int] NULL,
	[SalesTypeId] [int] NOT NULL,
	[SalesDeliveryTypeId] [int] NOT NULL,
	[WorkOrderNo] [nvarchar](50) NULL,
	[SalesDate] [datetime] NOT NULL,
	[ExpectedDeliveryDate] [datetime] NULL,
	[DeliveryDate] [datetime] NULL,
	[DeliveryMethodId] [int] NULL,
	[ExportTypeId] [int] NULL,
	[LcNo] [nvarchar](50) NULL,
	[LcDate] [datetime] NULL,
	[BillOfEntry] [nvarchar](50) NULL,
	[BillOfEntryDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[TermsOfLc] [nvarchar](500) NULL,
	[CustomerPoNumber] [nvarchar](50) NULL,
	[IsComplete] [bit] NOT NULL,
	[IsTaxInvoicePrined] [bit] NOT NULL,
	[TaxInvoicePrintedTime] [datetime] NULL,
	[MushakGenerationId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[OtherBranchOrganizationId] [int] NULL,
 CONSTRAINT [PK_Sales] PRIMARY KEY CLUSTERED 
(
	[SalesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[SaleView]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SaleView] as
SELECT 
ROW_NUMBER() OVER (ORDER BY SL.SalesDate DESC) AS Serial,
SL.SalesId,
SL.InvoiceNo,
SL.SalesDate,
SL.ExpectedDeliveryDate,
SL.DeliveryDate,
ORG.OrganizationId,
ORG.Name AS OrganizationName

 FROM Sales SL
LEFT JOIN Customer CUS ON SL.CustomerId=CUS.CustomerId
LEFT JOIN Organizations ORG ON SL.OrganizationId=ORG.OrganizationId
GO
/****** Object:  Table [acc].[COAGroups]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [acc].[COAGroups](
	[COAGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[ParentGroupId] [int] NULL,
	[Node] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_COAGroups] PRIMARY KEY CLUSTERED 
(
	[COAGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLog]    Script Date: 12/1/2019 2:25:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[AuditLogId] [int] IDENTITY(1,1) NOT NULL,
	[ObjectTypeId] [int] NOT NULL,
	[PrimaryKey] [int] NULL,
	[AuditOperationId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Datetime] [datetime2](7) NOT NULL,
	[Descriptions] [nvarchar](4000) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[OrganizationId] [int] NULL,
 CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED 
(
	[AuditLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditOperation]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditOperation](
	[AuditOperationID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_AuditOperation] PRIMARY KEY CLUSTERED 
(
	[AuditOperationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bank]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bank](
	[BankId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[NameInBangla] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Bank] PRIMARY KEY CLUSTERED 
(
	[BankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankBranch]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankBranch](
	[BankBranchId] [int] IDENTITY(1,1) NOT NULL,
	[BankId] [int] NOT NULL,
	[DistrictId] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[NameInBangla] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_BankBranch] PRIMARY KEY CLUSTERED 
(
	[BankBranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillOfMaterial]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillOfMaterial](
	[BillOfMaterialId] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductionReceiveId] [bigint] NOT NULL,
	[RawMaterialId] [int] NOT NULL,
	[UsedQuantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[StockInId] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_BillOfMaterial] PRIMARY KEY CLUSTERED 
(
	[BillOfMaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessCategory]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessCategory](
	[BusinessCategoryId] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[NameInBangla] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_BusinessCategory] PRIMARY KEY CLUSTERED 
(
	[BusinessCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessNature]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessNature](
	[BusinessNatureId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[NameInBangla] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_BusinessNature] PRIMARY KEY CLUSTERED 
(
	[BusinessNatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Content]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Content](
	[ContentId] [bigint] IDENTITY(1,1) NOT NULL,
	[DocumentTypeId] [int] NOT NULL,
	[OrganizationId] [int] NULL,
	[FileUrl] [nvarchar](500) NOT NULL,
	[MimeType] [nvarchar](50) NULL,
	[Node] [nvarchar](500) NULL,
	[ObjectId] [int] NOT NULL,
	[ObjectPrimaryKey] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractType]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractType](
	[ContractTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ContractType] PRIMARY KEY CLUSTERED 
(
	[ContractTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractualProduction]    Script Date: 12/1/2019 2:25:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractualProduction](
	[ContractualProductionId] [int] IDENTITY(1,1) NOT NULL,
	[ContractTypeId] [int] NULL,
	[OrganizationId] [int] NOT NULL,
	[ChallanNo] [nvarchar](50) NULL,
	[ContractNo] [nvarchar](50) NOT NULL,
	[VendorId] [int] NULL,
	[CustomerId] [int] NULL,
	[IssueDate] [datetime2](7) NULL,
	[ContractDate] [datetime] NOT NULL,
	[IsClosed] [bit] NOT NULL,
	[ClosingDate] [datetime] NOT NULL,
	[ClosedBy] [int] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ContractualProduction] PRIMARY KEY CLUSTERED 
(
	[ContractualProductionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractualProductionProductDetails]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractualProductionProductDetails](
	[ContractualProductionProductDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[ContractualProductionId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
 CONSTRAINT [PK_ContractualProductionProductDetails] PRIMARY KEY CLUSTERED 
(
	[ContractualProductionProductDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractualProductionTransferRawMaterial]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractualProductionTransferRawMaterial](
	[ContractualProductionTransferRawMaterialId] [int] IDENTITY(1,1) NOT NULL,
	[ContractualProductionId] [int] NOT NULL,
	[TransfereDate] [datetime] NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[ChallanNo] [nvarchar](50) NOT NULL,
	[ChallanIssueDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_ContractualProductionTransferRawMaterial] PRIMARY KEY CLUSTERED 
(
	[ContractualProductionTransferRawMaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractualProductionTransferRawMaterialDetails]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractualProductionTransferRawMaterialDetails](
	[ContractualProductionTransferRawMaterialDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[ContractualProductionTransferRawMaterialId] [int] NOT NULL,
	[RawMaterialId] [int] NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ContractualProductionTransferRawMaterialDetails] PRIMARY KEY CLUSTERED 
(
	[ContractualProductionTransferRawMaterialDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditNote]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditNote](
	[CreditNoteId] [int] IDENTITY(1,1) NOT NULL,
	[SalesId] [int] NOT NULL,
	[ReasonOfReturn] [nvarchar](500) NULL,
	[ReturnDate] [datetime] NOT NULL,
	[MushakGenerationId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_CreditNote] PRIMARY KEY CLUSTERED 
(
	[CreditNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditNoteDetail]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditNoteDetail](
	[CreditNoteDetailId] [int] IDENTITY(1,1) NOT NULL,
	[CreditNoteId] [int] NOT NULL,
	[SalesDetailId] [int] NOT NULL,
	[ReturnQuantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_CreditNoteDetail] PRIMARY KEY CLUSTERED 
(
	[CreditNoteDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomsAndVATCommissionarate]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomsAndVATCommissionarate](
	[CustomsAndVATCommissionarateId] [int] NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[NameInBangla] [nvarchar](250) NOT NULL,
	[DistrictId] [int] NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[OperationalCode] [nvarchar](10) NOT NULL,
	[OperationalCode1stDigit] [nvarchar](2) NOT NULL,
	[OperationalCode2ndDigit] [nvarchar](2) NOT NULL,
	[OperationalCode3rdDigit] [nvarchar](2) NOT NULL,
	[OperationalCode4thDigit] [nvarchar](2) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_CustomsAndVATCommissionarate] PRIMARY KEY CLUSTERED 
(
	[CustomsAndVATCommissionarateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Damage]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Damage](
	[DamageId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[StockInId] [bigint] NOT NULL,
	[DamageQty] [decimal](18, 2) NOT NULL,
	[DamageTypeId] [int] NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Damage] PRIMARY KEY CLUSTERED 
(
	[DamageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DamageType]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DamageType](
	[DamageTypeId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_DamageType] PRIMARY KEY CLUSTERED 
(
	[DamageTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DebitNote]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DebitNote](
	[DebitNoteId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[ReasonOfReturn] [nvarchar](500) NULL,
	[ReturnDate] [datetime] NOT NULL,
	[MushakGenerationId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_DebitNote] PRIMARY KEY CLUSTERED 
(
	[DebitNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DebitNoteDetail]    Script Date: 12/1/2019 2:25:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DebitNoteDetail](
	[DebitNoteDetailId] [int] IDENTITY(1,1) NOT NULL,
	[DebitNoteId] [int] NOT NULL,
	[PurchaseDetailId] [int] NOT NULL,
	[ReturnQuantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_DebitNoteDetail] PRIMARY KEY CLUSTERED 
(
	[DebitNoteDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryMethod]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryMethod](
	[DeliveryMethodId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DeliveryMethod] PRIMARY KEY CLUSTERED 
(
	[DeliveryMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[DistrictId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[NameInBangla] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_District] PRIMARY KEY CLUSTERED 
(
	[DistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentType]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentType](
	[DocumentTypeId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_DocumentType] PRIMARY KEY CLUSTERED 
(
	[DocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExportType]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExportType](
	[ExportTypeId] [int] NOT NULL,
	[ExportTypeName] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ExportType] PRIMARY KEY CLUSTERED 
(
	[ExportTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialActivityNature]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialActivityNature](
	[FinancialActivityNatureId] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[NameInBangla] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_FinancialActivityNature] PRIMARY KEY CLUSTERED 
(
	[FinancialActivityNatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InputOutputCoEfficient]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InputOutputCoEfficient](
	[InputOutputCoEfficientId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[PriceSetupId] [int] NULL,
	[IsSubmitted] [bit] NOT NULL,
	[SubmissionDate] [datetime] NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_InputOutputCoEfficient] PRIMARY KEY CLUSTERED 
(
	[InputOutputCoEfficientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasurementUnits]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasurementUnits](
	[MeasurementUnitId] [int] IDENTITY(1040,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_MeasurementUnits] PRIMARY KEY CLUSTERED 
(
	[MeasurementUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MushakGeneration]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MushakGeneration](
	[MushakGenerationId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[MushakForYear] [int] NOT NULL,
	[MushakForMonth] [int] NOT NULL,
	[GenerateDate] [datetime] NOT NULL,
	[AmountForVat] [decimal](18, 2) NULL,
	[AmountForSuppDuty] [decimal](18, 2) NULL,
	[InterestForDueVat] [decimal](18, 2) NULL,
	[InterestForDueSuppDuty] [decimal](18, 2) NULL,
	[FinancialPenalty] [decimal](18, 2) NULL,
	[ExciseDuty] [decimal](18, 2) NULL,
	[DevelopmentSurcharge] [decimal](18, 2) NULL,
	[ItDevelopmentSurcharge] [decimal](18, 2) NULL,
	[HealthDevelopmentSurcharge] [decimal](18, 2) NULL,
	[EnvironmentProtectSurcharge] [decimal](18, 2) NULL,
	[LastClosingVatAmount] [decimal](18, 2) NULL,
	[LastClosingSuppDutyAmount] [decimal](18, 2) NULL,
	[PaidVatAmount] [decimal](18, 2) NULL,
	[VatEconomicCodeId] [int] NULL,
	[VatPaymentDate] [datetime] NULL,
	[VatPaymentChallanNo] [nvarchar](20) NULL,
	[VatPaymentBankBranchId] [int] NULL,
	[PaidSuppDutyAmount] [decimal](18, 2) NULL,
	[SuppDutyEconomicCodeId] [int] NULL,
	[SuppDutyPaymentDate] [datetime] NULL,
	[SuppDutyChallanNo] [nvarchar](20) NULL,
	[SuppDutyBankBranchId] [int] NULL,
	[PaidInterestAmountForDueVat] [decimal](18, 2) NULL,
	[InterestForDueVatEconomicCodeId] [int] NULL,
	[InterestForDueVatPaymentDate] [datetime] NULL,
	[InterestForDueVatChallanNo] [nvarchar](20) NULL,
	[InterestForDueVatBankBranchId] [int] NULL,
	[PaidInterestAmountForDueSuppDuty] [decimal](18, 2) NULL,
	[InterestForDueSuppDutyEconomicCodeId] [int] NULL,
	[InterestForDueSuppDutyPaymentDate] [datetime] NULL,
	[InterestForDueSuppDutyChallanNo] [nvarchar](20) NULL,
	[InterestForDueSuppDutyBankBranchId] [int] NULL,
	[PaidFinancialPenalty] [decimal](18, 2) NULL,
	[FinancialPenaltyEconomicCodeId] [int] NULL,
	[FinancialPenaltyPaymentDate] [datetime] NULL,
	[FinancialPenaltyChallanNo] [nvarchar](20) NULL,
	[FinancialPenaltyBankBranchId] [int] NULL,
	[PaidExciseDuty] [decimal](18, 2) NULL,
	[ExciseDutyEconomicCodeId] [int] NULL,
	[ExciseDutyPaymentDate] [datetime] NULL,
	[ExciseDutyChallanNo] [nvarchar](20) NULL,
	[ExciseDutyBankBranchId] [int] NULL,
	[PaidDevelopmentSurcharge] [decimal](18, 2) NULL,
	[DevelopmentSurchargeEconomicCodeId] [int] NULL,
	[DevelopmentSurchargePaymentDate] [datetime] NULL,
	[DevelopmentSurchargeChallanNo] [nvarchar](20) NULL,
	[DevelopmentSurchargeBankBranchId] [int] NULL,
	[PaidItDevelopmentSurcharge] [decimal](18, 2) NULL,
	[ItDevelopmentSurchargeEconomicCodeId] [int] NULL,
	[ItDevelopmentSurchargePaymentDate] [datetime] NULL,
	[ItDevelopmentSurchargeChallanNo] [nvarchar](20) NULL,
	[ItDevelopmentSurchargeBankBranchId] [int] NULL,
	[PaidHealthDevelopmentSurcharge] [decimal](18, 2) NULL,
	[HealthDevelopmentSurchargeEconomicCodeId] [int] NULL,
	[HealthDevelopmentSurchargePaymentDate] [datetime] NULL,
	[HealthDevelopmentSurchargeChallanNo] [nvarchar](20) NULL,
	[HealthDevelopmentSurchargeBankBranchId] [int] NULL,
	[PaidEnvironmentProtectSurcharge] [decimal](18, 2) NULL,
	[EnvironmentProtectSurchargeEconomicCodeId] [int] NULL,
	[EnvironmentProtectSurchargePaymentDate] [datetime] NULL,
	[EnvironmentProtectSurchargeChallanNo] [nvarchar](20) NULL,
	[EnvironmentProtectSurchargeBankBranchId] [int] NULL,
	[MiscIncrementalAdjustmentAmount] [decimal](18, 2) NULL,
	[MiscIncrementalAdjustmentDesc] [nvarchar](500) NULL,
	[MiscDecrementalAdjustmentAmount] [decimal](18, 2) NULL,
	[MiscDecrementalAdjustmentDesc] [nvarchar](500) NULL,
	[IsSubmitted] [bit] NULL,
	[SubmissionDate] [datetime] NULL,
	[IsWantToGetBackClosingAmount] [bit] NOT NULL,
	[ReturnAmountFromClosingVat] [decimal](18, 2) NULL,
	[ReturnFromClosingVatChequeBankId] [int] NULL,
	[ReturnFromClosingVatChequeNo] [nvarchar](50) NULL,
	[ReturnFromClosingVatChequeDate] [datetime] NULL,
	[ReturnAmountFromClosingSd] [decimal](18, 2) NULL,
	[ReturnFromClosingSdChequeBankId] [int] NULL,
	[ReturnFromClosingSdChequeNo] [nvarchar](50) NULL,
	[ReturnFromClosingSdChequeDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[MushakGenerationStageId] [tinyint] NOT NULL,
 CONSTRAINT [PK_MushakGeneration] PRIMARY KEY CLUSTERED 
(
	[MushakGenerationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MushakGenerationStage]    Script Date: 12/1/2019 2:25:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MushakGenerationStage](
	[MushakGenerationStageId] [tinyint] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[NameInBangla] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_MushakGenerationStage] PRIMARY KEY CLUSTERED 
(
	[MushakGenerationStageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbrEconomicCode]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbrEconomicCode](
	[NbrEconomicCodeId] [int] NOT NULL,
	[NbrEconomicCodeTypeId] [int] NOT NULL,
	[EconomicTitle] [nvarchar](200) NOT NULL,
	[EconomicCode] [nvarchar](200) NOT NULL,
	[Code1stDisit] [nvarchar](2) NOT NULL,
	[Code2ndDisit] [nvarchar](2) NOT NULL,
	[Code3rdDisit] [nvarchar](2) NOT NULL,
	[Code4thDisit] [nvarchar](2) NOT NULL,
	[Code5thDisit] [nvarchar](2) NOT NULL,
	[Code6thDisit] [nvarchar](2) NOT NULL,
	[Code7thDisit] [nvarchar](2) NOT NULL,
	[Code8thDisit] [nvarchar](2) NOT NULL,
	[Code9thDisit] [nvarchar](2) NOT NULL,
	[Code10thDisit] [nvarchar](2) NOT NULL,
	[Code11thDisit] [nvarchar](2) NOT NULL,
	[Code12thDisit] [nvarchar](2) NOT NULL,
	[Code13thDisit] [nvarchar](2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
 CONSTRAINT [PK_NbrEconomicCode] PRIMARY KEY CLUSTERED 
(
	[NbrEconomicCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NbrEconomicCodeType]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NbrEconomicCodeType](
	[NbrEconomicCodeTypeId] [int] NOT NULL,
	[CodeTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_NbrEconomicCodeType] PRIMARY KEY CLUSTERED 
(
	[NbrEconomicCodeTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectType]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectType](
	[ObjectTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ObjectType] PRIMARY KEY CLUSTERED 
(
	[ObjectTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OverHeadCost]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OverHeadCost](
	[OverHeadCostId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_OverHeadCost] PRIMARY KEY CLUSTERED 
(
	[OverHeadCostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethod]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethod](
	[PaymentMethodId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED 
(
	[PaymentMethodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceSetup]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceSetup](
	[PriceSetupId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[BaseTP] [decimal](18, 2) NULL,
	[MRP] [decimal](18, 2) NULL,
	[PurchaseUnitPrice] [decimal](18, 2) NOT NULL,
	[SalesUnitPrice] [decimal](18, 2) NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_PriceSetup] PRIMARY KEY CLUSTERED 
(
	[PriceSetupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PriceSetupProductCost]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceSetupProductCost](
	[PriceSetupProductCostId] [int] IDENTITY(1,1) NOT NULL,
	[PriceSetupId] [int] NOT NULL,
	[RawMaterialId] [int] NULL,
	[RequiredQty] [decimal](18, 2) NULL,
	[MeasurementUnitId] [int] NULL,
	[OverHeadCostId] [int] NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[WastagePercentage] [decimal](18, 2) NULL,
 CONSTRAINT [PK_PriceSetupProductCost] PRIMARY KEY CLUSTERED 
(
	[PriceSetupProductCostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[ProductCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductGroups]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductGroups](
	[ProductGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentGroupId] [int] NULL,
	[Node] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ProductGroups] PRIMARY KEY CLUSTERED 
(
	[ProductGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionDetails]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionDetails](
	[ProductionDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[ProductionId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_ProductionDetails] PRIMARY KEY CLUSTERED 
(
	[ProductionDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductionReceive]    Script Date: 12/1/2019 2:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductionReceive](
	[ProductionReceiveId] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchNo] [nvarchar](50) NULL,
	[OrganizationId] [int] NOT NULL,
	[ProductionId] [int] NULL,
	[ProductId] [int] NOT NULL,
	[ReceiveQuantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[ReceiveTime] [datetime] NOT NULL,
	[MaterialCost] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ProductionReceive] PRIMARY KEY CLUSTERED 
(
	[ProductionReceiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productions]    Script Date: 12/1/2019 2:25:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productions](
	[ProductionId] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderId] [int] NULL,
	[OrganizationId] [int] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ExpectedDate] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Productions] PRIMARY KEY CLUSTERED 
(
	[ProductionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductProductTypeMapping]    Script Date: 12/1/2019 2:25:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductProductTypeMapping](
	[ProductProductTypeMappingId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductTypeId] [int] NOT NULL,
 CONSTRAINT [PK_ProductProductTypeMapping] PRIMARY KEY CLUSTERED 
(
	[ProductProductTypeMappingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 12/1/2019 2:25:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ModelNo] [nvarchar](50) NULL,
	[Code] [nvarchar](50) NULL,
	[HSCode] [nvarchar](50) NULL,
	[ProductCategoryId] [int] NULL,
	[ProductGroupId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[TotalQuantity] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[IsSellable] [bit] NOT NULL,
	[IsRawMaterial] [bit] NOT NULL,
	[IsNonRebateable] [bit] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductType]    Script Date: 12/1/2019 2:25:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductType](
	[ProductTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[ProductTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductVATs]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductVATs](
	[ProductVATId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVATTypeId] [int] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[IsActive] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ProductVAT] PRIMARY KEY CLUSTERED 
(
	[ProductVATId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductVATTypes]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductVATTypes](
	[ProductVATTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DefaultVatPercent] [decimal](18, 2) NOT NULL,
	[SupplementaryDutyPercent] [decimal](18, 0) NULL,
	[DisplayName] [nvarchar](100) NOT NULL,
	[EffectiveFrom] [nvarchar](50) NOT NULL,
	[EffectiveTo] [nvarchar](50) NULL,
	[TransactionTypeId] [int] NULL,
	[PurchaseTypeId] [int] NULL,
	[SalesTypeId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_ProductVATType] PRIMARY KEY CLUSTERED 
(
	[ProductVATTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Purchase]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchase](
	[PurchaseId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[VendorId] [int] NOT NULL,
	[VatChallanNo] [nvarchar](50) NULL,
	[VatChallanIssueDate] [datetime] NULL,
	[VendorInvoiceNo] [nvarchar](50) NOT NULL,
	[InvoiceNo] [nvarchar](50) NOT NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[PurchaseTypeId] [int] NOT NULL,
	[PurchaseReasonId] [int] NOT NULL,
	[NoOfIteams] [int] NOT NULL,
	[TotalPriceWithoutVat] [decimal](18, 2) NOT NULL,
	[DiscountOnTotalPrice] [decimal](18, 2) NOT NULL,
	[TotalDiscountOnIndividualProduct] [decimal](18, 2) NOT NULL,
	[TotalImportDuty] [decimal](18, 2) NOT NULL,
	[TotalRegulatoryDuty] [decimal](18, 2) NOT NULL,
	[TotalSupplementaryDuty] [decimal](18, 2) NOT NULL,
	[TotalVAT] [decimal](18, 2) NOT NULL,
	[TotalAdvanceTax] [decimal](18, 2) NOT NULL,
	[TotalAdvanceIncomeTax] [decimal](18, 2) NOT NULL,
	[AdvanceTaxPaidAmount] [decimal](18, 2) NULL,
	[ATPDate] [datetime] NULL,
	[ATPBankBranchId] [int] NULL,
	[ATPNbrEconomicCodeId] [int] NULL,
	[ATPChallanNo] [nvarchar](20) NULL,
	[IsVatDeductedInSource] [bit] NOT NULL,
	[VDSAmount] [decimal](18, 2) NULL,
	[VDSCertificateNo] [nvarchar](50) NULL,
	[VDSCertificateDate] [datetime] NULL,
	[PayableAmount]  AS (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)),
	[PaidAmount] [decimal](18, 2) NULL,
	[DueAmount]  AS (CONVERT([decimal](21,2),case when [IsVatDeductedInSource]=(1) then [TotalPriceWithoutVat]-[DiscountOnTotalPrice] else ([TotalPriceWithoutVat]+[TotalVAT])-[DiscountOnTotalPrice] end)-[PaidAmount]),
	[ExpectedDeliveryDate] [datetime] NULL,
	[DeliveryDate] [datetime] NULL,
	[LcNo] [nvarchar](50) NULL,
	[LcDate] [datetime] NULL,
	[BillOfEntry] [nvarchar](50) NULL,
	[BillOfEntryDate] [datetime] NULL,
	[CustomsAndVATCommissionarateId] [int] NULL,
	[DueDate] [datetime] NULL,
	[TermsOfLc] [nvarchar](500) NULL,
	[PoNumber] [nvarchar](50) NULL,
	[MushakGenerationId] [int] NULL,
	[IsComplete] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Purchase] PRIMARY KEY CLUSTERED 
(
	[PurchaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseDetails]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseDetails](
	[PurchaseDetailId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVATTypeId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[DiscountPerItem] [decimal](18, 2) NOT NULL,
	[ImportDutyPercent] [decimal](18, 2) NOT NULL,
	[RegulatoryDutyPercent] [decimal](18, 2) NOT NULL,
	[SupplementaryDutyPercent] [decimal](18, 2) NOT NULL,
	[VATPercent] [decimal](18, 2) NOT NULL,
	[AdvanceTaxPercent] [decimal](18, 2) NOT NULL,
	[AdvanceIncomeTaxPercent] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_PurchaseDetails] PRIMARY KEY CLUSTERED 
(
	[PurchaseDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchasePayment]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchasePayment](
	[PurchasePaymentId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseId] [int] NOT NULL,
	[PaymentMethodId] [int] NOT NULL,
	[PaidAmount] [decimal](18, 2) NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_PurchasePayment] PRIMARY KEY CLUSTERED 
(
	[PurchasePaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseReason]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseReason](
	[PurchaseReasonId] [int] NOT NULL,
	[Reason] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PurchaseReason] PRIMARY KEY CLUSTERED 
(
	[PurchaseReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseTypes]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseTypes](
	[PurchaseTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_PurchaseType] PRIMARY KEY CLUSTERED 
(
	[PurchaseTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rights]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rights](
	[RightId] [int] IDENTITY(1,1) NOT NULL,
	[RightName] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Rights] PRIMARY KEY CLUSTERED 
(
	[RightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleRights]    Script Date: 12/1/2019 2:26:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleRights](
	[RoleRightId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[RightId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_RoleFeatures] PRIMARY KEY CLUSTERED 
(
	[RoleRightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/1/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](64) NOT NULL,
	[OrganizationId] [int] NULL,
	[RoleDefaultPageId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesDeliveryType]    Script Date: 12/1/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesDeliveryType](
	[SalesDeliveryTypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_SalesDeliveryType] PRIMARY KEY CLUSTERED 
(
	[SalesDeliveryTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesDetails]    Script Date: 12/1/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesDetails](
	[SalesDetailId] [int] IDENTITY(1,1) NOT NULL,
	[SalesId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductVATTypeId] [int] NOT NULL,
	[StockInId] [int] NOT NULL,
	[Quantity] [decimal](18, 2) NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[DiscountPerItem] [decimal](18, 2) NOT NULL,
	[VATPercent] [decimal](18, 2) NOT NULL,
	[SupplementaryDutyPercent] [decimal](18, 2) NOT NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_SalesDetails] PRIMARY KEY CLUSTERED 
(
	[SalesDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesPaymentReceive]    Script Date: 12/1/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesPaymentReceive](
	[SalesPaymentReceiveId] [int] IDENTITY(1,1) NOT NULL,
	[SalesId] [int] NOT NULL,
	[ReceivedPaymentMethodId] [int] NOT NULL,
	[ReceiveAmount] [decimal](18, 2) NOT NULL,
	[ReceiveDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_SalesPaymentReceive] PRIMARY KEY CLUSTERED 
(
	[SalesPaymentReceiveId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesType]    Script Date: 12/1/2019 2:26:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesType](
	[SalesTypeId] [int] NOT NULL,
	[SalesTypeName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_SalesType] PRIMARY KEY CLUSTERED 
(
	[SalesTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockIn]    Script Date: 12/1/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockIn](
	[StockInId] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductionReceiveId] [bigint] NULL,
	[PurchaseDetailId] [int] NULL,
	[InitialQuantity] [decimal](18, 2) NULL,
	[InQuantity] [decimal](18, 2) NOT NULL,
	[InitUnitPriceWithoutVat] [decimal](18, 2) NULL,
	[EndUnitPriceWithoutVat] [decimal](18, 2) NULL,
	[MeasurementUnitId] [int] NOT NULL,
	[SaleQuantity] [decimal](18, 2) NOT NULL,
	[DamageQuantity] [decimal](18, 2) NOT NULL,
	[UsedInProductionQuantity] [decimal](18, 2) NOT NULL,
	[PurchaseReturnQty] [decimal](18, 2) NOT NULL,
	[SalesReturnQty] [decimal](18, 2) NOT NULL,
	[CurrentStock]  AS ((((([InQuantity]-[SaleQuantity])-[DamageQuantity])-[UsedInProductionQuantity])-[PurchaseReturnQty])+[SalesReturnQty]),
	[IsFinishedGoods] [bit] NOT NULL,
	[InputOutputCoEfficientId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_StockIn] PRIMARY KEY CLUSTERED 
(
	[StockInId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplimentaryDuty]    Script Date: 12/1/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplimentaryDuty](
	[SupplimentaryDutyId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[SdPercent] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_SupplimentaryDuty] PRIMARY KEY CLUSTERED 
(
	[SupplimentaryDutyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransectionTypes]    Script Date: 12/1/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransectionTypes](
	[TransectionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_TransectionTypes] PRIMARY KEY CLUSTERED 
(
	[TransectionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/1/2019 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(22,1) NOT NULL,
	[FullName] [nvarchar](200) NULL,
	[UserName] [nvarchar](64) NOT NULL,
	[EmailAddress] [nvarchar](64) NULL,
	[Password] [nvarchar](64) NULL,
	[UserTypeId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[OrganizationId] [int] NULL,
	[Mobile] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[IsDefaultPassword] [bit] NOT NULL,
	[IsCompanyRepresentative] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Users_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTypes]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTypes](
	[UserTypeId] [int] IDENTITY(1,1) NOT NULL,
	[UserTypeName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[EffectiveTo] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
 CONSTRAINT [PK_UserTypes] PRIMARY KEY CLUSTERED 
(
	[UserTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[VendorId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationId] [int] NULL,
	[VendorOrganizationId] [int] NULL,
	[Name] [nvarchar](200) NOT NULL,
	[BinNo] [nvarchar](20) NULL,
	[NationalIdNo] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NOT NULL,
	[ContactNo] [varchar](20) NULL,
	[CreatedBy] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[IsRegisteredAsTurnOverOrg] [bit] NOT NULL,
	[IsRegistered] [bit] NOT NULL,
 CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED 
(
	[VendorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[NbrEconomicCode] ADD  CONSTRAINT [DF_NbrEconomicCode_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NbrEconomicCode] ADD  CONSTRAINT [DF_NbrEconomicCode_EffectiveFrom]  DEFAULT (getdate()) FOR [EffectiveFrom]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_TotalQuantity]  DEFAULT ((0)) FOR [TotalQuantity]
GO
ALTER TABLE [dbo].[ProductVATTypes] ADD  CONSTRAINT [DF__tmp_ms_xx__Defau__72C60C4A]  DEFAULT ((0)) FOR [DefaultVatPercent]
GO
ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF__tmp_ms_xx__Sales__7D439ABD]  DEFAULT ((1)) FOR [SalesDeliveryTypeId]
GO
ALTER TABLE [dbo].[SalesDetails] ADD  CONSTRAINT [DF__tmp_ms_xx__Produ__02084FDA]  DEFAULT ((139)) FOR [ProductVATTypeId]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_AuditOperation] FOREIGN KEY([AuditOperationId])
REFERENCES [dbo].[AuditOperation] ([AuditOperationID])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_AuditOperation]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_ObjectType] FOREIGN KEY([ObjectTypeId])
REFERENCES [dbo].[ObjectType] ([ObjectTypeId])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_ObjectType]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_Organizations]
GO
ALTER TABLE [dbo].[AuditLog]  WITH CHECK ADD  CONSTRAINT [FK_AuditLog_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_Users]
GO
ALTER TABLE [dbo].[BankBranch]  WITH CHECK ADD  CONSTRAINT [FK_BankBranch_Bank] FOREIGN KEY([BankId])
REFERENCES [dbo].[Bank] ([BankId])
GO
ALTER TABLE [dbo].[BankBranch] CHECK CONSTRAINT [FK_BankBranch_Bank]
GO
ALTER TABLE [dbo].[BankBranch]  WITH CHECK ADD  CONSTRAINT [FK_BankBranch_District] FOREIGN KEY([DistrictId])
REFERENCES [dbo].[District] ([DistrictId])
GO
ALTER TABLE [dbo].[BankBranch] CHECK CONSTRAINT [FK_BankBranch_District]
GO
ALTER TABLE [dbo].[BillOfMaterial]  WITH CHECK ADD  CONSTRAINT [FK_BillOfMaterial_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[BillOfMaterial] CHECK CONSTRAINT [FK_BillOfMaterial_MeasurementUnits]
GO
ALTER TABLE [dbo].[BillOfMaterial]  WITH CHECK ADD  CONSTRAINT [FK_BillOfMaterial_ProductionReceive] FOREIGN KEY([ProductionReceiveId])
REFERENCES [dbo].[ProductionReceive] ([ProductionReceiveId])
GO
ALTER TABLE [dbo].[BillOfMaterial] CHECK CONSTRAINT [FK_BillOfMaterial_ProductionReceive]
GO
ALTER TABLE [dbo].[BillOfMaterial]  WITH CHECK ADD  CONSTRAINT [FK_BillOfMaterial_Products] FOREIGN KEY([RawMaterialId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[BillOfMaterial] CHECK CONSTRAINT [FK_BillOfMaterial_Products]
GO
ALTER TABLE [dbo].[BillOfMaterial]  WITH CHECK ADD  CONSTRAINT [FK_BillOfMaterial_StockIn] FOREIGN KEY([StockInId])
REFERENCES [dbo].[StockIn] ([StockInId])
GO
ALTER TABLE [dbo].[BillOfMaterial] CHECK CONSTRAINT [FK_BillOfMaterial_StockIn]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_DocumentType] FOREIGN KEY([DocumentTypeId])
REFERENCES [dbo].[DocumentType] ([DocumentTypeId])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Content_DocumentType]
GO
ALTER TABLE [dbo].[ContractualProduction]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProduction_ContractType] FOREIGN KEY([ContractTypeId])
REFERENCES [dbo].[ContractType] ([ContractTypeId])
GO
ALTER TABLE [dbo].[ContractualProduction] CHECK CONSTRAINT [FK_ContractualProduction_ContractType]
GO
ALTER TABLE [dbo].[ContractualProduction]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProduction_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[ContractualProduction] CHECK CONSTRAINT [FK_ContractualProduction_Customer]
GO
ALTER TABLE [dbo].[ContractualProduction]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProduction_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([VendorId])
GO
ALTER TABLE [dbo].[ContractualProduction] CHECK CONSTRAINT [FK_ContractualProduction_Vendor]
GO
ALTER TABLE [dbo].[ContractualProductionProductDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionProductDetails_ContractualProduction] FOREIGN KEY([ContractualProductionId])
REFERENCES [dbo].[ContractualProduction] ([ContractualProductionId])
GO
ALTER TABLE [dbo].[ContractualProductionProductDetails] CHECK CONSTRAINT [FK_ContractualProductionProductDetails_ContractualProduction]
GO
ALTER TABLE [dbo].[ContractualProductionProductDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionProductDetails_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ContractualProductionProductDetails] CHECK CONSTRAINT [FK_ContractualProductionProductDetails_Products]
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterial]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionTransferRawMaterial_ContractualProduction] FOREIGN KEY([ContractualProductionId])
REFERENCES [dbo].[ContractualProduction] ([ContractualProductionId])
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterial] CHECK CONSTRAINT [FK_ContractualProductionTransferRawMaterial_ContractualProduction]
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_ContractualProductionTransferRawMaterial] FOREIGN KEY([ContractualProductionTransferRawMaterialId])
REFERENCES [dbo].[ContractualProductionTransferRawMaterial] ([ContractualProductionTransferRawMaterialId])
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails] CHECK CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_ContractualProductionTransferRawMaterial]
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails] CHECK CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_MeasurementUnits]
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_Products] FOREIGN KEY([RawMaterialId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ContractualProductionTransferRawMaterialDetails] CHECK CONSTRAINT [FK_ContractualProductionTransferRawMaterialDetails_Products]
GO
ALTER TABLE [dbo].[CreditNote]  WITH CHECK ADD  CONSTRAINT [FK_CreditNote_MushakGeneration] FOREIGN KEY([MushakGenerationId])
REFERENCES [dbo].[MushakGeneration] ([MushakGenerationId])
GO
ALTER TABLE [dbo].[CreditNote] CHECK CONSTRAINT [FK_CreditNote_MushakGeneration]
GO
ALTER TABLE [dbo].[CreditNote]  WITH CHECK ADD  CONSTRAINT [FK_CreditNote_Sales] FOREIGN KEY([SalesId])
REFERENCES [dbo].[Sales] ([SalesId])
GO
ALTER TABLE [dbo].[CreditNote] CHECK CONSTRAINT [FK_CreditNote_Sales]
GO
ALTER TABLE [dbo].[CreditNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_CreditNoteDetail_CreditNote] FOREIGN KEY([CreditNoteId])
REFERENCES [dbo].[CreditNote] ([CreditNoteId])
GO
ALTER TABLE [dbo].[CreditNoteDetail] CHECK CONSTRAINT [FK_CreditNoteDetail_CreditNote]
GO
ALTER TABLE [dbo].[CreditNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_CreditNoteDetail_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[CreditNoteDetail] CHECK CONSTRAINT [FK_CreditNoteDetail_MeasurementUnits]
GO
ALTER TABLE [dbo].[CreditNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_CreditNoteDetail_SalesDetails] FOREIGN KEY([SalesDetailId])
REFERENCES [dbo].[SalesDetails] ([SalesDetailId])
GO
ALTER TABLE [dbo].[CreditNoteDetail] CHECK CONSTRAINT [FK_CreditNoteDetail_SalesDetails]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Organizations]
GO
ALTER TABLE [dbo].[Damage]  WITH CHECK ADD  CONSTRAINT [FK_Damage_DamageType] FOREIGN KEY([DamageTypeId])
REFERENCES [dbo].[DamageType] ([DamageTypeId])
GO
ALTER TABLE [dbo].[Damage] CHECK CONSTRAINT [FK_Damage_DamageType]
GO
ALTER TABLE [dbo].[Damage]  WITH CHECK ADD  CONSTRAINT [FK_Damage_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Damage] CHECK CONSTRAINT [FK_Damage_Organizations]
GO
ALTER TABLE [dbo].[Damage]  WITH CHECK ADD  CONSTRAINT [FK_Damage_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[Damage] CHECK CONSTRAINT [FK_Damage_Products]
GO
ALTER TABLE [dbo].[Damage]  WITH CHECK ADD  CONSTRAINT [FK_Damage_StockIn] FOREIGN KEY([StockInId])
REFERENCES [dbo].[StockIn] ([StockInId])
GO
ALTER TABLE [dbo].[Damage] CHECK CONSTRAINT [FK_Damage_StockIn]
GO
ALTER TABLE [dbo].[DamageType]  WITH CHECK ADD  CONSTRAINT [FK_DamageType_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[DamageType] CHECK CONSTRAINT [FK_DamageType_Organizations]
GO
ALTER TABLE [dbo].[DebitNote]  WITH CHECK ADD  CONSTRAINT [FK_DebitNote_MushakGeneration] FOREIGN KEY([MushakGenerationId])
REFERENCES [dbo].[MushakGeneration] ([MushakGenerationId])
GO
ALTER TABLE [dbo].[DebitNote] CHECK CONSTRAINT [FK_DebitNote_MushakGeneration]
GO
ALTER TABLE [dbo].[DebitNote]  WITH CHECK ADD  CONSTRAINT [FK_DebitNote_Purchase] FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[Purchase] ([PurchaseId])
GO
ALTER TABLE [dbo].[DebitNote] CHECK CONSTRAINT [FK_DebitNote_Purchase]
GO
ALTER TABLE [dbo].[DebitNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_DebitNoteDetail_DebitNote] FOREIGN KEY([DebitNoteId])
REFERENCES [dbo].[DebitNote] ([DebitNoteId])
GO
ALTER TABLE [dbo].[DebitNoteDetail] CHECK CONSTRAINT [FK_DebitNoteDetail_DebitNote]
GO
ALTER TABLE [dbo].[DebitNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_DebitNoteDetail_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[DebitNoteDetail] CHECK CONSTRAINT [FK_DebitNoteDetail_MeasurementUnits]
GO
ALTER TABLE [dbo].[DebitNoteDetail]  WITH CHECK ADD  CONSTRAINT [FK_DebitNoteDetail_PurchaseDetails] FOREIGN KEY([PurchaseDetailId])
REFERENCES [dbo].[PurchaseDetails] ([PurchaseDetailId])
GO
ALTER TABLE [dbo].[DebitNoteDetail] CHECK CONSTRAINT [FK_DebitNoteDetail_PurchaseDetails]
GO
ALTER TABLE [dbo].[DocumentType]  WITH CHECK ADD  CONSTRAINT [FK_DocumentType_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[DocumentType] CHECK CONSTRAINT [FK_DocumentType_Organizations]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingSdChequeBankId] FOREIGN KEY([ReturnFromClosingSdChequeBankId])
REFERENCES [dbo].[Bank] ([BankId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingSdChequeBankId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingVatChequeBankId] FOREIGN KEY([ReturnFromClosingVatChequeBankId])
REFERENCES [dbo].[Bank] ([BankId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_Bank_ReturnFromClosingVatChequeBankId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_DevelopmentSurchargeBankBranchId] FOREIGN KEY([DevelopmentSurchargeBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_DevelopmentSurchargeBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_EnvironmentProtectSurchargeBankBranchId] FOREIGN KEY([EnvironmentProtectSurchargeBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_EnvironmentProtectSurchargeBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_ExciseDutyBankBranchId] FOREIGN KEY([ExciseDutyBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_ExciseDutyBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_FinancialPenaltyBankBranchId] FOREIGN KEY([FinancialPenaltyBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_FinancialPenaltyBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_HealthDevelopmentSurchargeBankBranchId] FOREIGN KEY([HealthDevelopmentSurchargeBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_HealthDevelopmentSurchargeBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueSuppDutyBankBranchId] FOREIGN KEY([InterestForDueSuppDutyBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueSuppDutyBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueVatBankBranchId] FOREIGN KEY([InterestForDueVatBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_InterestForDueVatBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_ItDevelopmentSurchargeBankBranchId] FOREIGN KEY([ItDevelopmentSurchargeBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_ItDevelopmentSurchargeBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_SuppDutyBankBranchId] FOREIGN KEY([SuppDutyBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_SuppDutyBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_BankBranch_VatPaymentBankBranchId] FOREIGN KEY([VatPaymentBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_BankBranch_VatPaymentBankBranchId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_MushakGenerationStage] FOREIGN KEY([MushakGenerationStageId])
REFERENCES [dbo].[MushakGenerationStage] ([MushakGenerationStageId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_MushakGenerationStage]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_DevelopmentSurchargeEconomicCodeId] FOREIGN KEY([DevelopmentSurchargeEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_DevelopmentSurchargeEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_EnvironmentProtectSurchargeEconomicCodeId] FOREIGN KEY([EnvironmentProtectSurchargeEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_EnvironmentProtectSurchargeEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ExciseDutyEconomicCodeId] FOREIGN KEY([ExciseDutyEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ExciseDutyEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_FinancialPenaltyEconomicCodeId] FOREIGN KEY([FinancialPenaltyEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_FinancialPenaltyEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_HealthDevelopmentSurchargeEconomicCodeId] FOREIGN KEY([HealthDevelopmentSurchargeEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_HealthDevelopmentSurchargeEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueSuppDutyEconomicCodeId] FOREIGN KEY([InterestForDueSuppDutyEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueSuppDutyEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueVatEconomicCodeId] FOREIGN KEY([InterestForDueVatEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_InterestForDueVatEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ItDevelopmentSurchargeEconomicCodeId] FOREIGN KEY([ItDevelopmentSurchargeEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_ItDevelopmentSurchargeEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_SuppDutyEconomicCodeId] FOREIGN KEY([SuppDutyEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_SuppDutyEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_VatEconomicCodeId] FOREIGN KEY([VatEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_NbrEconomicCode_VatEconomicCodeId]
GO
ALTER TABLE [dbo].[MushakGeneration]  WITH CHECK ADD  CONSTRAINT [FK_MushakGeneration_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[MushakGeneration] CHECK CONSTRAINT [FK_MushakGeneration_Organizations]
GO
ALTER TABLE [dbo].[NbrEconomicCode]  WITH CHECK ADD  CONSTRAINT [FK_NbrEconomicCode_NbrEconomicCodeType] FOREIGN KEY([NbrEconomicCodeTypeId])
REFERENCES [dbo].[NbrEconomicCodeType] ([NbrEconomicCodeTypeId])
GO
ALTER TABLE [dbo].[NbrEconomicCode] CHECK CONSTRAINT [FK_NbrEconomicCode_NbrEconomicCodeType]
GO
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_Organizations_BusinessCategory] FOREIGN KEY([BusinessCategoryId])
REFERENCES [dbo].[BusinessCategory] ([BusinessCategoryId])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_Organizations_BusinessCategory]
GO
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_Organizations_BusinessNature] FOREIGN KEY([BusinessNatureId])
REFERENCES [dbo].[BusinessNature] ([BusinessNatureId])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_Organizations_BusinessNature]
GO
ALTER TABLE [dbo].[Organizations]  WITH CHECK ADD  CONSTRAINT [FK_Organizations_FinancialActivityNature] FOREIGN KEY([FinancialActivityNatureId])
REFERENCES [dbo].[FinancialActivityNature] ([FinancialActivityNatureId])
GO
ALTER TABLE [dbo].[Organizations] CHECK CONSTRAINT [FK_Organizations_FinancialActivityNature]
GO
ALTER TABLE [dbo].[PriceSetup]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetup_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[PriceSetup] CHECK CONSTRAINT [FK_PriceSetup_MeasurementUnits]
GO
ALTER TABLE [dbo].[PriceSetup]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetup_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[PriceSetup] CHECK CONSTRAINT [FK_PriceSetup_Organizations]
GO
ALTER TABLE [dbo].[PriceSetup]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetup_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[PriceSetup] CHECK CONSTRAINT [FK_PriceSetup_Products]
GO
ALTER TABLE [dbo].[PriceSetupProductCost]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetupProductCost_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[PriceSetupProductCost] CHECK CONSTRAINT [FK_PriceSetupProductCost_MeasurementUnits]
GO
ALTER TABLE [dbo].[PriceSetupProductCost]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetupProductCost_OverHeadCost] FOREIGN KEY([OverHeadCostId])
REFERENCES [dbo].[OverHeadCost] ([OverHeadCostId])
GO
ALTER TABLE [dbo].[PriceSetupProductCost] CHECK CONSTRAINT [FK_PriceSetupProductCost_OverHeadCost]
GO
ALTER TABLE [dbo].[PriceSetupProductCost]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetupProductCost_PriceSetup] FOREIGN KEY([PriceSetupId])
REFERENCES [dbo].[PriceSetup] ([PriceSetupId])
GO
ALTER TABLE [dbo].[PriceSetupProductCost] CHECK CONSTRAINT [FK_PriceSetupProductCost_PriceSetup]
GO
ALTER TABLE [dbo].[PriceSetupProductCost]  WITH CHECK ADD  CONSTRAINT [FK_PriceSetupProductCost_Products] FOREIGN KEY([RawMaterialId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[PriceSetupProductCost] CHECK CONSTRAINT [FK_PriceSetupProductCost_Products]
GO
ALTER TABLE [dbo].[ProductCategory]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategory_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[ProductCategory] CHECK CONSTRAINT [FK_ProductCategory_Organizations]
GO
ALTER TABLE [dbo].[ProductionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ProductionDetails_Productions] FOREIGN KEY([ProductionId])
REFERENCES [dbo].[Productions] ([ProductionId])
GO
ALTER TABLE [dbo].[ProductionDetails] CHECK CONSTRAINT [FK_ProductionDetails_Productions]
GO
ALTER TABLE [dbo].[ProductionDetails]  WITH CHECK ADD  CONSTRAINT [FK_ProductionDetails_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ProductionDetails] CHECK CONSTRAINT [FK_ProductionDetails_Products]
GO
ALTER TABLE [dbo].[ProductionReceive]  WITH CHECK ADD  CONSTRAINT [FK_ProductionReceive_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[ProductionReceive] CHECK CONSTRAINT [FK_ProductionReceive_Organizations]
GO
ALTER TABLE [dbo].[Productions]  WITH CHECK ADD  CONSTRAINT [FK_Productions_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Productions] CHECK CONSTRAINT [FK_Productions_Organizations]
GO
ALTER TABLE [dbo].[ProductProductTypeMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProductProductTypeMapping_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ProductProductTypeMapping] CHECK CONSTRAINT [FK_ProductProductTypeMapping_Products]
GO
ALTER TABLE [dbo].[ProductProductTypeMapping]  WITH CHECK ADD  CONSTRAINT [FK_ProductProductTypeMapping_ProductType] FOREIGN KEY([ProductTypeId])
REFERENCES [dbo].[ProductType] ([ProductTypeId])
GO
ALTER TABLE [dbo].[ProductProductTypeMapping] CHECK CONSTRAINT [FK_ProductProductTypeMapping_ProductType]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_MeasurementUnits]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Organizations]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductCategory] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([ProductCategoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductCategory]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductGroups] FOREIGN KEY([ProductGroupId])
REFERENCES [dbo].[ProductGroups] ([ProductGroupId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductGroups]
GO
ALTER TABLE [dbo].[ProductVATs]  WITH CHECK ADD  CONSTRAINT [FK_ProductVATs_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[ProductVATs] CHECK CONSTRAINT [FK_ProductVATs_Products]
GO
ALTER TABLE [dbo].[ProductVATs]  WITH CHECK ADD  CONSTRAINT [FK_ProductVATs_ProductVATTypes] FOREIGN KEY([ProductVATTypeId])
REFERENCES [dbo].[ProductVATTypes] ([ProductVATTypeId])
GO
ALTER TABLE [dbo].[ProductVATs] CHECK CONSTRAINT [FK_ProductVATs_ProductVATTypes]
GO
ALTER TABLE [dbo].[ProductVATTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductVATTypes_PurchaseTypes] FOREIGN KEY([PurchaseTypeId])
REFERENCES [dbo].[PurchaseTypes] ([PurchaseTypeId])
GO
ALTER TABLE [dbo].[ProductVATTypes] CHECK CONSTRAINT [FK_ProductVATTypes_PurchaseTypes]
GO
ALTER TABLE [dbo].[ProductVATTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductVATTypes_SalesType] FOREIGN KEY([SalesTypeId])
REFERENCES [dbo].[SalesType] ([SalesTypeId])
GO
ALTER TABLE [dbo].[ProductVATTypes] CHECK CONSTRAINT [FK_ProductVATTypes_SalesType]
GO
ALTER TABLE [dbo].[ProductVATTypes]  WITH CHECK ADD  CONSTRAINT [FK_ProductVATTypes_TransectionTypes] FOREIGN KEY([TransactionTypeId])
REFERENCES [dbo].[TransectionTypes] ([TransectionTypeId])
GO
ALTER TABLE [dbo].[ProductVATTypes] CHECK CONSTRAINT [FK_ProductVATTypes_TransectionTypes]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_BankBranch_ATPBankBranchId] FOREIGN KEY([ATPBankBranchId])
REFERENCES [dbo].[BankBranch] ([BankBranchId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_BankBranch_ATPBankBranchId]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_CustomsAndVATCommissionarate] FOREIGN KEY([CustomsAndVATCommissionarateId])
REFERENCES [dbo].[CustomsAndVATCommissionarate] ([CustomsAndVATCommissionarateId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_CustomsAndVATCommissionarate]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_NbrEconomicCodeATPNbrEconomicCodeId] FOREIGN KEY([ATPNbrEconomicCodeId])
REFERENCES [dbo].[NbrEconomicCode] ([NbrEconomicCodeId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_NbrEconomicCodeATPNbrEconomicCodeId]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_Organizations]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_PurchaseReason] FOREIGN KEY([PurchaseReasonId])
REFERENCES [dbo].[PurchaseReason] ([PurchaseReasonId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_PurchaseReason]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_PurchaseTypes] FOREIGN KEY([PurchaseTypeId])
REFERENCES [dbo].[PurchaseTypes] ([PurchaseTypeId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_PurchaseTypes]
GO
ALTER TABLE [dbo].[Purchase]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendor] ([VendorId])
GO
ALTER TABLE [dbo].[Purchase] CHECK CONSTRAINT [FK_Purchase_Vendor]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_MeasurementUnits]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_Products]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_ProductVATTypes] FOREIGN KEY([ProductVATTypeId])
REFERENCES [dbo].[ProductVATTypes] ([ProductVATTypeId])
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_ProductVATTypes]
GO
ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseDetails_Purchase] FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[Purchase] ([PurchaseId])
GO
ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_PurchaseDetails_Purchase]
GO
ALTER TABLE [dbo].[PurchasePayment]  WITH CHECK ADD  CONSTRAINT [FK_PurchasePayment_PaymentMethod] FOREIGN KEY([PaymentMethodId])
REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId])
GO
ALTER TABLE [dbo].[PurchasePayment] CHECK CONSTRAINT [FK_PurchasePayment_PaymentMethod]
GO
ALTER TABLE [dbo].[PurchasePayment]  WITH CHECK ADD  CONSTRAINT [FK_PurchasePayment_Purchase] FOREIGN KEY([PurchaseId])
REFERENCES [dbo].[Purchase] ([PurchaseId])
GO
ALTER TABLE [dbo].[PurchasePayment] CHECK CONSTRAINT [FK_PurchasePayment_Purchase]
GO
ALTER TABLE [dbo].[RoleRights]  WITH CHECK ADD  CONSTRAINT [FK_dbo_RoleFeatures_dbo_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[RoleRights] CHECK CONSTRAINT [FK_dbo_RoleFeatures_dbo_Roles_RoleId]
GO
ALTER TABLE [dbo].[RoleRights]  WITH CHECK ADD  CONSTRAINT [FK_RoleRights_Rights] FOREIGN KEY([RightId])
REFERENCES [dbo].[Rights] ([RightId])
GO
ALTER TABLE [dbo].[RoleRights] CHECK CONSTRAINT [FK_RoleRights_Rights]
GO
ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [FK_Roles_Organizations]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Customer]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_DeliveryMethod] FOREIGN KEY([DeliveryMethodId])
REFERENCES [dbo].[DeliveryMethod] ([DeliveryMethodId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_DeliveryMethod]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_ExportType] FOREIGN KEY([ExportTypeId])
REFERENCES [dbo].[ExportType] ([ExportTypeId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_ExportType]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Organizations]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_OtherBranchOrganization] FOREIGN KEY([OtherBranchOrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_OtherBranchOrganization]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SalesDeliveryType] FOREIGN KEY([SalesDeliveryTypeId])
REFERENCES [dbo].[SalesDeliveryType] ([SalesDeliveryTypeId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_SalesDeliveryType]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SalesType] FOREIGN KEY([SalesTypeId])
REFERENCES [dbo].[SalesType] ([SalesTypeId])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_SalesType]
GO
ALTER TABLE [dbo].[SalesDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalesDetails_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[SalesDetails] CHECK CONSTRAINT [FK_SalesDetails_MeasurementUnits]
GO
ALTER TABLE [dbo].[SalesDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalesDetails_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[SalesDetails] CHECK CONSTRAINT [FK_SalesDetails_Products]
GO
ALTER TABLE [dbo].[SalesDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalesDetails_ProductVATTypes] FOREIGN KEY([ProductVATTypeId])
REFERENCES [dbo].[ProductVATTypes] ([ProductVATTypeId])
GO
ALTER TABLE [dbo].[SalesDetails] CHECK CONSTRAINT [FK_SalesDetails_ProductVATTypes]
GO
ALTER TABLE [dbo].[SalesDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalesDetails_Sales] FOREIGN KEY([SalesId])
REFERENCES [dbo].[Sales] ([SalesId])
GO
ALTER TABLE [dbo].[SalesDetails] CHECK CONSTRAINT [FK_SalesDetails_Sales]
GO
ALTER TABLE [dbo].[SalesPaymentReceive]  WITH CHECK ADD  CONSTRAINT [FK_SalesPaymentReceive_PaymentMethod] FOREIGN KEY([ReceivedPaymentMethodId])
REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId])
GO
ALTER TABLE [dbo].[SalesPaymentReceive] CHECK CONSTRAINT [FK_SalesPaymentReceive_PaymentMethod]
GO
ALTER TABLE [dbo].[SalesPaymentReceive]  WITH CHECK ADD  CONSTRAINT [FK_SalesPaymentReceive_Sales] FOREIGN KEY([SalesId])
REFERENCES [dbo].[Sales] ([SalesId])
GO
ALTER TABLE [dbo].[SalesPaymentReceive] CHECK CONSTRAINT [FK_SalesPaymentReceive_Sales]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_InputOutputCoEfficient] FOREIGN KEY([InputOutputCoEfficientId])
REFERENCES [dbo].[InputOutputCoEfficient] ([InputOutputCoEfficientId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_InputOutputCoEfficient]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_MeasurementUnits] FOREIGN KEY([MeasurementUnitId])
REFERENCES [dbo].[MeasurementUnits] ([MeasurementUnitId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_MeasurementUnits]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_Organizations]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_ProductionReceive] FOREIGN KEY([ProductionReceiveId])
REFERENCES [dbo].[ProductionReceive] ([ProductionReceiveId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_ProductionReceive]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_Products]
GO
ALTER TABLE [dbo].[StockIn]  WITH CHECK ADD  CONSTRAINT [FK_StockIn_PurchaseDetails] FOREIGN KEY([PurchaseDetailId])
REFERENCES [dbo].[PurchaseDetails] ([PurchaseDetailId])
GO
ALTER TABLE [dbo].[StockIn] CHECK CONSTRAINT [FK_StockIn_PurchaseDetails]
GO
ALTER TABLE [dbo].[SupplimentaryDuty]  WITH CHECK ADD  CONSTRAINT [FK_SupplimentaryDuty_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[SupplimentaryDuty] CHECK CONSTRAINT [FK_SupplimentaryDuty_Organizations]
GO
ALTER TABLE [dbo].[SupplimentaryDuty]  WITH CHECK ADD  CONSTRAINT [FK_SupplimentaryDuty_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[SupplimentaryDuty] CHECK CONSTRAINT [FK_SupplimentaryDuty_Products]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo_UserProfiles_dbo_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo_UserProfiles_dbo_Roles_RoleId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Organizations]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserTypes] FOREIGN KEY([UserTypeId])
REFERENCES [dbo].[UserTypes] ([UserTypeId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserTypes]
GO
ALTER TABLE [dbo].[Vendor]  WITH CHECK ADD  CONSTRAINT [FK_Vendor_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
GO
ALTER TABLE [dbo].[Vendor] CHECK CONSTRAINT [FK_Vendor_Organizations]
GO
/****** Object:  StoredProcedure [dbo].[GetMushokNinePointOne]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMushokNinePointOne]
    @OrganizationId INT,
    @Year INT,
    @Month INT
AS
BEGIN
    DECLARE @firstDayOfMonth DATETIME = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01',
            @firstDayOfNextMonth DATETIME = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month + 1 AS VARCHAR(2)) + '-01';

    DECLARE @IsDeductVatInSource BIT;
    SELECT @IsDeductVatInSource = org.IsDeductVatInSource
    FROM Organizations org
    WHERE org.OrganizationId = @OrganizationId;



    DECLARE @TaxYearMonth DATE;

    SET @TaxYearMonth = CAST(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-' + '01' AS DATE);





    CREATE TABLE #TempMushokFor34
    (
        -------------Table 1-------------------------
        Tbl1BIN NVARCHAR(50),
        Tbl1OrganizationName NVARCHAR(200),
        Tbl1Address NVARCHAR(100) NULL,
        Tbl1BusinessNature NVARCHAR(100) NULL,
        Tbl1EconomicNature NVARCHAR(100) NULL,

                                                                        --------------Table 2------------------------

        Tbl2TaxYear INT NULL,
        Tbl2TaxMonth VARCHAR(15) NULL,
        Tbl2CurrentSubmissionCategory VARCHAR(100) NULL,
        Tbl2IsPreviousTaxSubmitted BIT NULL,
        Tbl2PreviousTaxSubmissionDate DATETIME NULL,

                                                                        --------------table3 Sales-------------------
        Tbl3Note1DirectExportAmount DECIMAL(18, 4) NULL,
        Tbl3Note2InDirectExportAmount DECIMAL(18, 4) NULL,
        Tbl3Note3DiscountedProductAmount DECIMAL(18, 4) NULL,
        Tbl3Note4IdealRatedProductAmount DECIMAL(18, 4) NULL,
        Tbl3Note4IdealRatedProductVAT DECIMAL(18, 4) NULL,
        Tbl3Note4IdealRatedProductSD DECIMAL(18, 4) NULL,
        Tbl3Note5MaxRetailPriceProductAmount DECIMAL(18, 4) NULL,
        Tbl3Note5MaxRetailPriceProductVAT DECIMAL(18, 4) NULL,
        Tbl3Note5MaxRetailPriceProductSD DECIMAL(18, 4) NULL,
        Tbl3Note6SpecificTaxBasedProductAmount DECIMAL(18, 4) NULL,
        Tbl3Note6SpecificTaxBasedProductVAT DECIMAL(18, 4) NULL,
        Tbl3Note6SpecificTaxBasedProductSD DECIMAL(18, 4) NULL,
        Tbl3Note7DifferentTaxBasedProductAount DECIMAL(18, 4) NULL,     -- 145
        Tbl3Note7DifferentTaxBasedProductVAT DECIMAL(18, 4) NULL,       -- 145
        Tbl3Note7DifferentTaxBasedProductSD DECIMAL(18, 4) NULL,        -- 145
        Tbl3Note8RetailWholesaleBasedProductAmount DECIMAL(18, 4) NULL, --146
        Tbl3Note8RetailWholesaleBasedProductSD DECIMAL(18, 4) NULL,     --146
        Tbl3Total9Ka DECIMAL(18, 4) NULL,
        Tbl3Total9Kh DECIMAL(18, 4) NULL,
        Tbl3Total9Ga DECIMAL(18, 4) NULL,
                                                                        ------------end table3-----------------------------------
                                                                        ------------start table 4-------------------------------

        BuyZiroTaxableProductAmontLocal DECIMAL(18, 4) NULL,            --148
        BuyZiroTaxableProductAmontImport DECIMAL(18, 4) NULL,
        BuyDiscountedProductAmountLocal DECIMAL(18, 4) NULL,            --150
        BuyDiscountedProductAmountImport DECIMAL(18, 4) NULL,
        BuyIdealRetedProductAmountLocal DECIMAL(18, 4) NULL,            --152
        BuyIdealRetedProductVATLocal DECIMAL(18, 4) NULL,               --152
        BuyIdealRetedProductAmountImport DECIMAL(18, 4) NULL,
        BuyIdealRetedProductVATImport DECIMAL(18, 4) NULL,
        BuyNonStandardRatedProductAmountLocal DECIMAL(18, 4) NULL,      --154
        BuyNonStandardRatedProductVATLocal DECIMAL(18, 4) NULL,         --154
        BuyNonStandardRatedProductAmountImport DECIMAL(18, 4) NULL,     --154
        BuyNonStandardRatedProductVATImport DECIMAL(18, 4) NULL,        --154
        BuySpecifiedRatedProductAmount DECIMAL(18, 4) NULL,             --157
        BuySpecifiedRatedProductVAT DECIMAL(18, 4) NULL,                --157
        BuyNonConcessionProductAmountTurnOver DECIMAL(18, 4) NULL,      --158
        BuyNonConcessionProductVATTurnOver DECIMAL(18, 4) NULL,         --158
        BuyNonConcessionProductAmountUnregistered DECIMAL(18, 4) NULL,  --158
        BuyNonConcessionProductVATUnregistered DECIMAL(18, 4) NULL,     --158
        BuyNonConcessionLocalPurchaseAmount DECIMAL(18, 4) NULL,        --190
        BuyNonConcessionLocalPurchaseVAT DECIMAL(18, 4) NULL,           --190
        BuyNonConcessionImportedPurchaseAmount DECIMAL(18, 4) NULL,     --191
        BuyNonConcessionImportedPurchaseVAT DECIMAL(18, 4) NULL,        --191
        Tbl4Total23Kh DECIMAL(18, 4) NULL,
                                                                        -----------------------------------------------------

                                                                        --------------------Table 5---------------------------
        Tbl5Row1Note24 DECIMAL(18, 4) NULL,
        Tbl5Row2Note25 DECIMAL(18, 4) NULL,
        Tbl5Row3Note26 DECIMAL(18, 4) NULL,
        Tbl5Row4Note27 DECIMAL(18, 4) NULL,
        Tbl5Row5TotalNote28 DECIMAL(18, 4) NULL,

                                                                        ---------------------Table 6---------------------------
        Tbl6Row1Note29 DECIMAL(18, 4) NULL,
        Tbl6Row2Note30 DECIMAL(18, 4) NULL,
        Tbl6Row3Note31 DECIMAL(18, 4) NULL,
        Tbl6Row4Note32 DECIMAL(18, 4) NULL,
        Tbl6Row5Note33 DECIMAL(18, 4) NULL,
        Tbl6Row5Note34Total DECIMAL(18, 4) NULL,
                                                                        -------------------------------------------------------
                                                                        -----------------Table 7-------------------------------
        Tbl7Row1Note35 DECIMAL(18, 4) NULL,
        Tbl7Row2Note36 DECIMAL(18, 4) NULL,
        Tbl7Row3Note37 DECIMAL(18, 4) NULL,
        Tbl7Row4Note38 DECIMAL(18, 4) NULL,
        Tbl7Row5Note39 DECIMAL(18, 4) NULL,
        Tbl7Row6Note40 DECIMAL(18, 4) NULL,
        Tbl7Row7Note41 DECIMAL(18, 4) NULL,
        Tbl7Row8Note42 DECIMAL(18, 4) NULL,
        Tbl7Row9Note43 DECIMAL(18, 4) NULL,
        Tbl7Row10Note44 DECIMAL(18, 4) NULL,
        Tbl7Row11Note45 DECIMAL(18, 4) NULL,
        Tbl7Row12Note46 DECIMAL(18, 4) NULL,
        Tbl7Row13Note47 DECIMAL(18, 4) NULL,
        Tbl7Row14Note48 DECIMAL(18, 4) NULL,
        Tbl7Row15Note49 DECIMAL(18, 4) NULL,
        Tbl7Row16Note50 DECIMAL(18, 4) NULL,
        Tbl7Row17Note51 DECIMAL(18, 4) NULL,
                                                                        ----------------------------------------------
                                                                        ---------------------Table 8--------------------
        Tbl8Row1Note52EcoCode VARCHAR(50) NULL,
        Tbl8Row1Note52Amount DECIMAL(18, 4) NULL,
        Tbl8Row2Note53EcoCode VARCHAR(50) NULL,
        Tbl8Row2Note53Amount DECIMAL(18, 4) NULL,
        Tbl8Row3Note54EcoCode VARCHAR(50) NULL,
        Tbl8Row3Note54Amount DECIMAL(18, 4) NULL,
        Tbl8Row4Note55EcoCode VARCHAR(50) NULL,
        Tbl8Row4Note55Amount DECIMAL(18, 4) NULL,
        Tbl8Row5Note56EcoCode VARCHAR(50) NULL,
        Tbl8Row5Note56Amount DECIMAL(18, 4) NULL,
        Tbl8Row6Note57EcoCode VARCHAR(50) NULL,
        Tbl8Row6Note57Amount DECIMAL(18, 4) NULL,
        Tbl8Row7Note58EcoCode VARCHAR(50) NULL,
        Tbl8Row7Note58Amount DECIMAL(18, 4) NULL,
        Tbl8Row8Note59EcoCode VARCHAR(50) NULL,
        Tbl8Row8Note59Amount DECIMAL(18, 4) NULL,
        Tbl8Row9Note60EcoCode VARCHAR(50) NULL,
        Tbl8Row9Note60Amount DECIMAL(18, 4) NULL,
        Tbl8Row10Note61EcoCode VARCHAR(50) NULL,
        Tbl8Row10Note61Amount DECIMAL(18, 4) NULL,
                                                                        ------------------------------------------------
                                                                        --------------Table 9------------------------
        Tbl9Row1Note62 DECIMAL(18, 4) NULL,
        Tbl9Row2Note63 DECIMAL(18, 4) NULL,
                                                                        ---------------------------------------------
                                                                        --------------Table 10------------------------
        Tbl10Row1 BIT NULL,
                                                                        ----------------------------------------------
                                                                        --------------Table 11------------------------
        Tbl11Name VARCHAR(150) NULL,
        Tbl11Designation VARCHAR(100) NULL,
        Tbl11Date DATETIME NULL,
        Tbl11Mobile VARCHAR(15) NULL,
        Tbl11Email VARCHAR(100) NULL,
    ----------------------------------------------

    );

    ------------------------Table 1----------------------------------

    INSERT INTO #TempMushokFor34
    (
        Tbl1BIN,
        Tbl1OrganizationName,
        Tbl1Address,
        Tbl1BusinessNature,
        Tbl1EconomicNature
    )
    SELECT omg.BIN,
           omg.Name,
           [Address] = omg.Address,
           [BusinessNature] = 'Test',
           [EconomicNature] = 'Test'
    FROM Organizations omg
    WHERE omg.OrganizationId = @OrganizationId;

    ------------------------End Table 1------------------------------

    ------------------Start Table 2-----------------------------------
    UPDATE t1
    SET t1.Tbl2TaxYear = YEAR(@TaxYearMonth),
        t1.Tbl2TaxMonth = FORMAT(@TaxYearMonth, 'MMMM'),
        t1.Tbl2CurrentSubmissionCategory = 'Test',
        t1.Tbl2IsPreviousTaxSubmitted = 1,
        t1.Tbl2PreviousTaxSubmissionDate = FORMAT(GETDATE(), 'dd/MMMM/yyyy')
    FROM #TempMushokFor34 t1;

    ------------------End Table 2-------------------------------------


    --------------------------Table 3--------------------------------

    UPDATE t1
    SET t1.Tbl3Note1DirectExportAmount = ISNULL(
                                         (
                                             SELECT SUM(ISNULL(tbl1.Amount, 20555))
                                             FROM
                                             (
                                                 SELECT DISTINCT
                                                        s.SalesId,
                                                        ISNULL(s.TotalPriceWithoutVat, 30459) Amount
                                                 FROM Sales s
                                                     INNER JOIN SalesDetails sd
                                                         ON s.SalesId = sd.SalesId
                                                     INNER JOIN ProductVATTypes pvt
                                                         ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                            AND pvt.TransactionTypeId = 1
                                                 WHERE s.OrganizationId = @OrganizationId
                                                       AND YEAR(s.SalesDate) = @Year
                                                       AND MONTH(s.SalesDate) = @Month
                                                       AND s.ExportTypeId = 1
                                             ) tbl1
                                         ),
                                         21450
                                               )
    FROM #TempMushokFor34 t1;

    UPDATE t1
    SET t1.Tbl3Note2InDirectExportAmount = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 41330))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          s.SalesId,
                                                          ISNULL(s.TotalPriceWithoutVat, 22540) Amount
                                                   FROM Sales s
                                                       INNER JOIN SalesDetails sd
                                                           ON s.SalesId = sd.SalesId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 1
                                                   WHERE s.OrganizationId = @OrganizationId
                                                         AND YEAR(s.SalesDate) = @Year
                                                         AND MONTH(s.SalesDate) = @Month
                                                         AND s.ExportTypeId = 2
                                               ) tbl1
                                           ),
                                           40333
                                                 )
    FROM #TempMushokFor34 t1;

    /* 
insert into #TempMushokFor34(DirectExportAmount,InDirectExportAmount)
select 
[DirectExportAmount]=sum(case when tbl1.ExportTypeId = 1 then isnull(tbl1.Amount,0) else 0 end)
,[InDirectExportAmount]=sum(case when tbl1.ExportTypeId = 2 then isnull(tbl1.Amount,0) else 0 end)
from
(select distinct  s.SalesId,s.Amount,s.ExportTypeId
from Sales s
inner join SalesDetails sd on s.SalesId = sd.SalesId
inner join ProductVATTypes pvt on sd.ProductVATTypeId = pvt.ProductVATTypeId and pvt.TransactionTypeId = 1
where s.OrganizationId = @OrganizationId and YEAR(s.SalesDate) = @Year and MONTH(s.SalesDate) = @Month) tbl1
*/

    UPDATE #TempMushokFor34
    SET Tbl3Note3DiscountedProductAmount = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 50928))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          s.SalesId,
                                                          ISNULL(s.TotalPriceWithoutVat, 40620) AS Amount,
                                                          pvt.ProductVATTypeId
                                                   FROM Sales s
                                                       INNER JOIN SalesDetails sd
                                                           ON s.SalesId = sd.SalesId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 1
                                                              AND pvt.ProductVATTypeId = 141
                                                   WHERE s.OrganizationId = @OrganizationId
                                                         AND YEAR(s.SalesDate) = @Year
                                                         AND MONTH(s.SalesDate) = @Month
                                               ) tbl1
                                           ),
                                           68430
                                                 );

    UPDATE #TempMushokFor34
    SET Tbl3Note4IdealRatedProductAmount = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 41335))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          s.SalesId,
                                                          ISNULL(s.TotalPriceWithoutVat, 40344) AS Amount,
                                                          ISNULL(s.TotalVAT, 40396) vat
                                                   FROM Sales s
                                                       INNER JOIN SalesDetails sd
                                                           ON s.SalesId = sd.SalesId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 1
                                                              AND pvt.ProductVATTypeId = 142
                                                   WHERE s.OrganizationId = @OrganizationId
                                                         AND YEAR(s.SalesDate) = @Year
                                                         AND MONTH(s.SalesDate) = @Month
                                               ) tbl1
                                           ),
                                           2320
                                                 ),
        Tbl3Note4IdealRatedProductVAT = ISNULL(
                                        (
                                            SELECT SUM(ISNULL(tbl1.vat, 2460))
                                            FROM
                                            (
                                                SELECT DISTINCT
                                                       s.SalesId,
                                                       ISNULL(s.TotalVAT, 2455) vat
                                                FROM Sales s
                                                    INNER JOIN SalesDetails sd
                                                        ON s.SalesId = sd.SalesId
                                                    INNER JOIN ProductVATTypes pvt
                                                        ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                           AND pvt.TransactionTypeId = 1
                                                           AND pvt.ProductVATTypeId = 142
                                                WHERE s.OrganizationId = @OrganizationId
                                                      AND YEAR(s.SalesDate) = @Year
                                                      AND MONTH(s.SalesDate) = @Month
                                            ) tbl1
                                        ),
                                        2466
                                              ),
        Tbl3Note4IdealRatedProductSD = 0.0;

    UPDATE #TempMushokFor34
    SET Tbl3Note5MaxRetailPriceProductAmount = ISNULL(
                                               (
                                                   SELECT SUM(ISNULL(tbl1.Amount, 320457))
                                                   FROM
                                                   (
                                                       SELECT DISTINCT
                                                              s.SalesId,
                                                              ISNULL(s.TotalPriceWithoutVat, 320457) AS Amount
                                                       FROM Sales s
                                                           INNER JOIN SalesDetails sd
                                                               ON s.SalesId = sd.SalesId
                                                           INNER JOIN ProductVATTypes pvt
                                                               ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                  AND pvt.TransactionTypeId = 1
                                                                  AND pvt.ProductVATTypeId = 143
                                                       WHERE s.OrganizationId = @OrganizationId
                                                             AND YEAR(s.SalesDate) = @Year
                                                             AND MONTH(s.SalesDate) = @Month
                                                   ) tbl1
                                               ),
                                               320457
                                                     ),
        Tbl3Note5MaxRetailPriceProductVAT = ISNULL(
                                            (
                                                SELECT SUM(ISNULL(tbl1.vat, 320457))
                                                FROM
                                                (
                                                    SELECT DISTINCT
                                                           s.SalesId,
                                                           ISNULL(s.TotalVAT, 3204) AS vat
                                                    FROM Sales s
                                                        INNER JOIN SalesDetails sd
                                                            ON s.SalesId = sd.SalesId
                                                        INNER JOIN ProductVATTypes pvt
                                                            ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                               AND pvt.TransactionTypeId = 1
                                                               AND pvt.ProductVATTypeId = 143
                                                    WHERE s.OrganizationId = @OrganizationId
                                                          AND YEAR(s.SalesDate) = @Year
                                                          AND MONTH(s.SalesDate) = @Month
                                                ) tbl1
                                            ),
                                            3204
                                                  ),
        Tbl3Note5MaxRetailPriceProductSD = 4630;

    UPDATE #TempMushokFor34
    SET Tbl3Note6SpecificTaxBasedProductAmount = ISNULL(
                                                 (
                                                     SELECT SUM(ISNULL(tbl1.Amount, 60947))
                                                     FROM
                                                     (
                                                         SELECT DISTINCT
                                                                s.SalesId,
                                                                ISNULL(s.TotalPriceWithoutVat, 60947) AS Amount
                                                         FROM Sales s
                                                             INNER JOIN SalesDetails sd
                                                                 ON s.SalesId = sd.SalesId
                                                             INNER JOIN ProductVATTypes pvt
                                                                 ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                    AND pvt.TransactionTypeId = 1
                                                                    AND pvt.ProductVATTypeId = 0
                                                         WHERE s.OrganizationId = @OrganizationId
                                                               AND YEAR(s.SalesDate) = @Year
                                                               AND MONTH(s.SalesDate) = @Month
                                                     ) tbl1
                                                 ),
                                                 60947
                                                       ),
        Tbl3Note6SpecificTaxBasedProductVAT = ISNULL(
                                              (
                                                  SELECT SUM(ISNULL(tbl1.vat, 7430))
                                                  FROM
                                                  (
                                                      SELECT DISTINCT
                                                             s.SalesId,
                                                             ISNULL(s.TotalVAT, 7430) AS vat
                                                      FROM Sales s
                                                          INNER JOIN SalesDetails sd
                                                              ON s.SalesId = sd.SalesId
                                                          INNER JOIN ProductVATTypes pvt
                                                              ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                 AND pvt.TransactionTypeId = 1
                                                                 AND pvt.ProductVATTypeId = 0
                                                      WHERE s.OrganizationId = @OrganizationId
                                                            AND YEAR(s.SalesDate) = @Year
                                                            AND MONTH(s.SalesDate) = @Month
                                                  ) tbl1
                                              ),
                                              2430
                                                    ),
        Tbl3Note6SpecificTaxBasedProductSD = 0.0;

    UPDATE #TempMushokFor34
    SET Tbl3Note7DifferentTaxBasedProductAount = ISNULL(
                                                 (
                                                     SELECT SUM(ISNULL(tbl1.Amount, 60947))
                                                     FROM
                                                     (
                                                         SELECT DISTINCT
                                                                s.SalesId,
                                                                ISNULL(s.TotalPriceWithoutVat, 60947) AS Amount
                                                         FROM Sales s
                                                             INNER JOIN SalesDetails sd
                                                                 ON s.SalesId = sd.SalesId
                                                             INNER JOIN ProductVATTypes pvt
                                                                 ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                    AND pvt.TransactionTypeId = 1
                                                                    AND pvt.ProductVATTypeId = 145
                                                         WHERE s.OrganizationId = @OrganizationId
                                                               AND YEAR(s.SalesDate) = @Year
                                                               AND MONTH(s.SalesDate) = @Month
                                                     ) tbl1
                                                 ),
                                                 4320
                                                       ),
        Tbl3Note7DifferentTaxBasedProductVAT = ISNULL(
                                               (
                                                   SELECT SUM(ISNULL(tbl1.vat, 4304))
                                                   FROM
                                                   (
                                                       SELECT DISTINCT
                                                              s.SalesId,
                                                              ISNULL(s.TotalVAT, 4304) AS vat
                                                       FROM Sales s
                                                           INNER JOIN SalesDetails sd
                                                               ON s.SalesId = sd.SalesId
                                                           INNER JOIN ProductVATTypes pvt
                                                               ON sd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                  AND pvt.TransactionTypeId = 1
                                                                  AND pvt.ProductVATTypeId = 145
                                                       WHERE s.OrganizationId = @OrganizationId
                                                             AND YEAR(s.SalesDate) = @Year
                                                             AND MONTH(s.SalesDate) = @Month
                                                   ) tbl1
                                               ),
                                               4304
                                                     ),
        Tbl3Note7DifferentTaxBasedProductSD = 4304;

    UPDATE #TempMushokFor34
    SET Tbl3Note8RetailWholesaleBasedProductAmount = ISNULL(
                                                     (
                                                         SELECT SUM((sd.UnitPrice - sd.DiscountPerItem) * sd.Quantity) AS SalesAmount
                                                         FROM dbo.SalesDetails sd
                                                             INNER JOIN dbo.Sales sls
                                                                 ON sls.SalesId = sd.SalesId
                                                         WHERE sls.SalesDate >= @firstDayOfMonth
                                                               AND sls.SalesDate < @firstDayOfNextMonth
                                                               AND sls.SalesTypeId = 1
                                                               AND sls.OrganizationId = @OrganizationId
                                                     ),
                                                     0
                                                           ),
        Tbl3Note8RetailWholesaleBasedProductSD = ISNULL(
                                                 (
                                                     SELECT SUM((sd.UnitPrice - sd.DiscountPerItem) * sd.Quantity
                                                                * sd.VATPercent / 100
                                                               ) AS SalesVatAmount
                                                     FROM dbo.SalesDetails sd
                                                         INNER JOIN dbo.Sales sls
                                                             ON sls.SalesId = sd.SalesId
                                                     WHERE sls.SalesDate >= @firstDayOfMonth
                                                           AND sls.SalesDate < @firstDayOfNextMonth
                                                           AND sls.SalesTypeId = 1
                                                           AND sls.OrganizationId = @OrganizationId
                                                 ),
                                                 0
                                                       );
    --isnull((select SUM(isnull(tbl1.Amount,50940))
    --from
    --(select distinct  s.SalesId,isnull(s.TotalPriceWithoutVat,50940) as Amount
    --from Sales s
    --inner join SalesDetails sd on s.SalesId = sd.SalesId
    --inner join ProductVATTypes pvt on sd.ProductVATTypeId = pvt.ProductVATTypeId and pvt.TransactionTypeId = 1 and pvt.ProductVATTypeId = 146
    --where s.OrganizationId = @OrganizationId and YEAR(s.SalesDate) = @Year and MONTH(s.SalesDate) = @Month) tbl1),0)

    UPDATE t
    SET t.Tbl3Total9Ka = ISNULL(t.Tbl3Note1DirectExportAmount, 240630)
                         + ISNULL(t.Tbl3Note2InDirectExportAmount, 240630) + t.Tbl3Note3DiscountedProductAmount
                         + t.Tbl3Note4IdealRatedProductAmount + t.Tbl3Note5MaxRetailPriceProductAmount
                         + t.Tbl3Note6SpecificTaxBasedProductAmount + t.Tbl3Note7DifferentTaxBasedProductAount
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl3Total9Kh = t.Tbl3Note4IdealRatedProductVAT + t.Tbl3Note5MaxRetailPriceProductVAT
                         + t.Tbl3Note6SpecificTaxBasedProductVAT + t.Tbl3Note7DifferentTaxBasedProductVAT + t.Tbl3Note8RetailWholesaleBasedProductSD
    FROM #TempMushokFor34 t;


    UPDATE t
    SET t.Tbl3Total9Ga = t.Tbl3Note4IdealRatedProductSD + t.Tbl3Note5MaxRetailPriceProductSD
                         + t.Tbl3Note6SpecificTaxBasedProductSD + t.Tbl3Note7DifferentTaxBasedProductSD
    FROM #TempMushokFor34 t;

    -----------------------End Table 3---------------------------------

    ---------------------Table 4-----------------------------------


    UPDATE #TempMushokFor34
    SET BuyZiroTaxableProductAmontLocal = ISNULL(
                                          (
                                              SELECT SUM(ISNULL(tbl1.Amount, 50430))
                                              FROM
                                              (
                                                  SELECT DISTINCT
                                                         p.PurchaseId,
                                                         ISNULL(p.TotalPriceWithoutVat, 50430) AS Amount
                                                  FROM Purchase p
                                                      INNER JOIN PurchaseDetails pd
                                                          ON p.PurchaseId = pd.PurchaseId
                                                      INNER JOIN ProductVATTypes pvt
                                                          ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                             AND pvt.TransactionTypeId = 2
                                                             AND pvt.ProductVATTypeId = 148
                                                  WHERE p.PurchaseTypeId = 1
                                                        AND p.OrganizationId = @OrganizationId
                                                        AND YEAR(p.PurchaseDate) = @Year
                                                        AND MONTH(p.PurchaseDate) = @Month
                                              ) tbl1
                                          ),
                                          50430
                                                ),
        BuyZiroTaxableProductAmontImport = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 70470))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          p.PurchaseId,
                                                          ISNULL(p.TotalPriceWithoutVat, 70470) AS Amount
                                                   FROM Purchase p
                                                       INNER JOIN PurchaseDetails pd
                                                           ON p.PurchaseId = pd.PurchaseId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 2
                                                              AND pvt.ProductVATTypeId = 148
                                                   WHERE p.PurchaseTypeId = 2
                                                         AND p.OrganizationId = @OrganizationId
                                                         AND YEAR(p.PurchaseDate) = @Year
                                                         AND MONTH(p.PurchaseDate) = @Month
                                               ) tbl1
                                           ),
                                           70470
                                                 );


    UPDATE #TempMushokFor34
    SET BuyDiscountedProductAmountLocal = ISNULL(
                                          (
                                              SELECT SUM(ISNULL(tbl1.Amount, 35956))
                                              FROM
                                              (
                                                  SELECT DISTINCT
                                                         p.PurchaseId,
                                                         ISNULL(p.TotalPriceWithoutVat, 35956) AS Amount
                                                  FROM Purchase p
                                                      INNER JOIN PurchaseDetails pd
                                                          ON p.PurchaseId = pd.PurchaseId
                                                      INNER JOIN ProductVATTypes pvt
                                                          ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                             AND pvt.TransactionTypeId = 2
                                                             AND pvt.ProductVATTypeId = 150
                                                  WHERE p.PurchaseTypeId = 1
                                                        AND p.OrganizationId = @OrganizationId
                                                        AND YEAR(p.PurchaseDate) = @Year
                                                        AND MONTH(p.PurchaseDate) = @Month
                                              ) tbl1
                                          ),
                                          35956
                                                ),
        BuyDiscountedProductAmountImport = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 65943))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          p.PurchaseId,
                                                          ISNULL(p.TotalPriceWithoutVat, 65943) AS Amount
                                                   FROM Purchase p
                                                       INNER JOIN PurchaseDetails pd
                                                           ON p.PurchaseId = pd.PurchaseId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 2
                                                              AND pvt.ProductVATTypeId = 150
                                                   WHERE p.PurchaseTypeId = 2
                                                         AND p.OrganizationId = @OrganizationId
                                                         AND YEAR(p.PurchaseDate) = @Year
                                                         AND MONTH(p.PurchaseDate) = @Month
                                               ) tbl1
                                           ),
                                           65943
                                                 );


    UPDATE #TempMushokFor34
    SET BuyIdealRetedProductAmountLocal = ISNULL(
                                          (
                                              SELECT SUM(ISNULL(tbl1.Amount, 57840))
                                              FROM
                                              (
                                                  SELECT DISTINCT
                                                         p.PurchaseId,
                                                         ISNULL(p.TotalPriceWithoutVat, 57840) AS Amount
                                                  FROM Purchase p
                                                      INNER JOIN PurchaseDetails pd
                                                          ON p.PurchaseId = pd.PurchaseId
                                                      INNER JOIN ProductVATTypes pvt
                                                          ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                             AND pvt.TransactionTypeId = 2
                                                             AND pvt.ProductVATTypeId = 152
                                                  WHERE p.PurchaseTypeId = 1
                                                        AND p.OrganizationId = @OrganizationId
                                                        AND YEAR(p.PurchaseDate) = @Year
                                                        AND MONTH(p.PurchaseDate) = @Month
                                              ) tbl1
                                          ),
                                          57840
                                                ),
        BuyIdealRetedProductVATLocal = ISNULL(
                                       (
                                           SELECT SUM(ISNULL(tbl1.vat, 3230))
                                           FROM
                                           (
                                               SELECT DISTINCT
                                                      p.PurchaseId,
                                                      ISNULL(p.TotalVAT, 3230) AS vat
                                               FROM Purchase p
                                                   INNER JOIN PurchaseDetails pd
                                                       ON p.PurchaseId = pd.PurchaseId
                                                   INNER JOIN ProductVATTypes pvt
                                                       ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                          AND pvt.TransactionTypeId = 2
                                                          AND pvt.ProductVATTypeId = 152
                                               WHERE p.PurchaseTypeId = 1
                                                     AND p.OrganizationId = @OrganizationId
                                                     AND YEAR(p.PurchaseDate) = @Year
                                                     AND MONTH(p.PurchaseDate) = @Month
                                           ) tbl1
                                       ),
                                       59230
                                             ),
        BuyIdealRetedProductAmountImport = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.Amount, 59230))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          p.PurchaseId,
                                                          ISNULL(p.TotalPriceWithoutVat, 59230) AS Amount
                                                   FROM Purchase p
                                                       INNER JOIN PurchaseDetails pd
                                                           ON p.PurchaseId = pd.PurchaseId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 2
                                                              AND pvt.ProductVATTypeId = 152
                                                   WHERE p.PurchaseTypeId = 2
                                                         AND p.OrganizationId = @OrganizationId
                                                         AND YEAR(p.PurchaseDate) = @Year
                                                         AND MONTH(p.PurchaseDate) = @Month
                                               ) tbl1
                                           ),
                                           59230
                                                 ),
        BuyIdealRetedProductVATImport = ISNULL(
                                        (
                                            SELECT SUM(ISNULL(tbl1.vat, 6430))
                                            FROM
                                            (
                                                SELECT DISTINCT
                                                       p.PurchaseId,
                                                       ISNULL(p.TotalVAT, 6430) AS vat
                                                FROM Purchase p
                                                    INNER JOIN PurchaseDetails pd
                                                        ON p.PurchaseId = pd.PurchaseId
                                                    INNER JOIN ProductVATTypes pvt
                                                        ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                           AND pvt.TransactionTypeId = 2
                                                           AND pvt.ProductVATTypeId = 152
                                                WHERE p.PurchaseTypeId = 2
                                                      AND p.OrganizationId = @OrganizationId
                                                      AND YEAR(p.PurchaseDate) = @Year
                                                      AND MONTH(p.PurchaseDate) = @Month
                                            ) tbl1
                                        ),
                                        78530
                                              );

    UPDATE #TempMushokFor34
    SET BuyNonStandardRatedProductAmountLocal = ISNULL(
                                                (
                                                    SELECT SUM(ISNULL(tbl1.Amount, 78530))
                                                    FROM
                                                    (
                                                        SELECT DISTINCT
                                                               p.PurchaseId,
                                                               ISNULL(p.TotalPriceWithoutVat, 78530) AS Amount
                                                        FROM Purchase p
                                                            INNER JOIN PurchaseDetails pd
                                                                ON p.PurchaseId = pd.PurchaseId
                                                            INNER JOIN ProductVATTypes pvt
                                                                ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                   AND pvt.TransactionTypeId = 2
                                                                   AND pvt.ProductVATTypeId = 154
                                                        WHERE p.PurchaseTypeId = 1
                                                              AND p.OrganizationId = @OrganizationId
                                                              AND YEAR(p.PurchaseDate) = @Year
                                                              AND MONTH(p.PurchaseDate) = @Month
                                                    ) tbl1
                                                ),
                                                78530
                                                      ),
        BuyNonStandardRatedProductVATLocal = ISNULL(
                                             (
                                                 SELECT SUM(ISNULL(tbl1.vat, 5630))
                                                 FROM
                                                 (
                                                     SELECT DISTINCT
                                                            p.PurchaseId,
                                                            ISNULL(p.TotalVAT, 5630) AS vat
                                                     FROM Purchase p
                                                         INNER JOIN PurchaseDetails pd
                                                             ON p.PurchaseId = pd.PurchaseId
                                                         INNER JOIN ProductVATTypes pvt
                                                             ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                AND pvt.TransactionTypeId = 2
                                                                AND pvt.ProductVATTypeId = 154
                                                     WHERE p.PurchaseTypeId = 1
                                                           AND p.OrganizationId = @OrganizationId
                                                           AND YEAR(p.PurchaseDate) = @Year
                                                           AND MONTH(p.PurchaseDate) = @Month
                                                 ) tbl1
                                             ),
                                             90320
                                                   ),
        BuyNonStandardRatedProductAmountImport = ISNULL(
                                                 (
                                                     SELECT SUM(ISNULL(tbl1.Amount, 90320))
                                                     FROM
                                                     (
                                                         SELECT DISTINCT
                                                                p.PurchaseId,
                                                                ISNULL(p.TotalPriceWithoutVat, 90320) AS Amount
                                                         FROM Purchase p
                                                             INNER JOIN PurchaseDetails pd
                                                                 ON p.PurchaseId = pd.PurchaseId
                                                             INNER JOIN ProductVATTypes pvt
                                                                 ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                    AND pvt.TransactionTypeId = 2
                                                                    AND pvt.ProductVATTypeId = 154
                                                         WHERE p.PurchaseTypeId = 2
                                                               AND p.OrganizationId = @OrganizationId
                                                               AND YEAR(p.PurchaseDate) = @Year
                                                               AND MONTH(p.PurchaseDate) = @Month
                                                     ) tbl1
                                                 ),
                                                 90320
                                                       ),
        BuyNonStandardRatedProductVATImport = ISNULL(
                                              (
                                                  SELECT SUM(ISNULL(tbl1.vat, 12350))
                                                  FROM
                                                  (
                                                      SELECT DISTINCT
                                                             p.PurchaseId,
                                                             ISNULL(p.TotalVAT, 12350) AS vat
                                                      FROM Purchase p
                                                          INNER JOIN PurchaseDetails pd
                                                              ON p.PurchaseId = pd.PurchaseId
                                                          INNER JOIN ProductVATTypes pvt
                                                              ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                 AND pvt.TransactionTypeId = 2
                                                                 AND pvt.ProductVATTypeId = 154
                                                      WHERE p.PurchaseTypeId = 2
                                                            AND p.OrganizationId = @OrganizationId
                                                            AND YEAR(p.PurchaseDate) = @Year
                                                            AND MONTH(p.PurchaseDate) = @Month
                                                  ) tbl1
                                              ),
                                              12350
                                                    );

    UPDATE #TempMushokFor34
    SET BuySpecifiedRatedProductAmount = ISNULL(
                                         (
                                             SELECT SUM(ISNULL(tbl1.Amount, 34520))
                                             FROM
                                             (
                                                 SELECT DISTINCT
                                                        p.PurchaseId,
                                                        ISNULL(p.TotalPriceWithoutVat, 34520) AS Amount
                                                 FROM Purchase p
                                                     INNER JOIN PurchaseDetails pd
                                                         ON p.PurchaseId = pd.PurchaseId
                                                     INNER JOIN ProductVATTypes pvt
                                                         ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                            AND pvt.TransactionTypeId = 2
                                                            AND pvt.ProductVATTypeId = 157
                                                 WHERE p.PurchaseTypeId = 1
                                                       AND p.OrganizationId = @OrganizationId
                                                       AND YEAR(p.PurchaseDate) = @Year
                                                       AND MONTH(p.PurchaseDate) = @Month
                                             ) tbl1
                                         ),
                                         34520
                                               ),
        BuySpecifiedRatedProductVAT = ISNULL(
                                      (
                                          SELECT SUM(ISNULL(tbl1.vat, 7230))
                                          FROM
                                          (
                                              SELECT DISTINCT
                                                     p.PurchaseId,
                                                     ISNULL(p.TotalVAT, 5420) AS vat
                                              FROM Purchase p
                                                  INNER JOIN PurchaseDetails pd
                                                      ON p.PurchaseId = pd.PurchaseId
                                                  INNER JOIN ProductVATTypes pvt
                                                      ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                         AND pvt.TransactionTypeId = 2
                                                         AND pvt.ProductVATTypeId = 157
                                              WHERE p.PurchaseTypeId = 1
                                                    AND p.OrganizationId = @OrganizationId
                                                    AND YEAR(p.PurchaseDate) = @Year
                                                    AND MONTH(p.PurchaseDate) = @Month
                                          ) tbl1
                                      ),
                                      5420
                                            );

    UPDATE #TempMushokFor34
    SET BuyNonConcessionProductAmountTurnOver = ISNULL(
                                                (
                                                    SELECT SUM(ISNULL(tbl1.Amount, 92520))
                                                    FROM
                                                    (
                                                        SELECT DISTINCT
                                                               p.PurchaseId,
                                                               ISNULL(p.TotalPriceWithoutVat, 92520) AS Amount
                                                        FROM Purchase p
                                                            INNER JOIN Organizations org
                                                                ON p.OrganizationId = org.OrganizationId
                                                                   AND org.BIN IS NOT NULL
                                                            INNER JOIN PurchaseDetails pd
                                                                ON p.PurchaseId = pd.PurchaseId
                                                            INNER JOIN ProductVATTypes pvt
                                                                ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                   AND pvt.TransactionTypeId = 2
                                                                   AND pvt.ProductVATTypeId = 158
                                                        WHERE p.PurchaseTypeId = 1
                                                              AND p.OrganizationId = @OrganizationId
                                                              AND YEAR(p.PurchaseDate) = @Year
                                                              AND MONTH(p.PurchaseDate) = @Month
                                                    ) tbl1
                                                ),
                                                92520
                                                      ),
        BuyNonConcessionProductVATTurnOver = ISNULL(
                                             (
                                                 SELECT SUM(ISNULL(tbl1.vat, 11420))
                                                 FROM
                                                 (
                                                     SELECT DISTINCT
                                                            p.PurchaseId,
                                                            ISNULL(p.TotalVAT, 11420) AS vat
                                                     FROM Purchase p
                                                         INNER JOIN Organizations org
                                                             ON p.OrganizationId = org.OrganizationId
                                                                AND org.BIN IS NOT NULL
                                                         INNER JOIN PurchaseDetails pd
                                                             ON p.PurchaseId = pd.PurchaseId
                                                         INNER JOIN ProductVATTypes pvt
                                                             ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                AND pvt.TransactionTypeId = 2
                                                                AND pvt.ProductVATTypeId = 158
                                                     WHERE p.PurchaseTypeId = 1
                                                           AND p.OrganizationId = @OrganizationId
                                                           AND YEAR(p.PurchaseDate) = @Year
                                                           AND MONTH(p.PurchaseDate) = @Month
                                                 ) tbl1
                                             ),
                                             92520
                                                   );

    UPDATE #TempMushokFor34
    SET BuyNonConcessionProductAmountUnregistered = ISNULL(
                                                    (
                                                        SELECT SUM(ISNULL(tbl1.Amount, 92520))
                                                        FROM
                                                        (
                                                            SELECT DISTINCT
                                                                   p.PurchaseId,
                                                                   ISNULL(p.TotalPriceWithoutVat, 92520) AS Amount
                                                            FROM Purchase p
                                                                LEFT JOIN Organizations org
                                                                    ON p.OrganizationId = org.OrganizationId
                                                                INNER JOIN PurchaseDetails pd
                                                                    ON p.PurchaseId = pd.PurchaseId
                                                                INNER JOIN ProductVATTypes pvt
                                                                    ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                       AND pvt.TransactionTypeId = 2
                                                                       AND pvt.ProductVATTypeId = 158
                                                            WHERE p.PurchaseTypeId = 1
                                                                  AND org.BIN IS NULL
                                                                  AND p.OrganizationId = @OrganizationId
                                                                  AND YEAR(p.PurchaseDate) = @Year
                                                                  AND MONTH(p.PurchaseDate) = @Month
                                                        ) tbl1
                                                    ),
                                                    92520
                                                          ),
        BuyNonConcessionProductVATUnregistered = ISNULL(
                                                 (
                                                     SELECT SUM(ISNULL(tbl1.Amount, 92520))
                                                     FROM
                                                     (
                                                         SELECT DISTINCT
                                                                p.PurchaseId,
                                                                ISNULL(p.TotalPriceWithoutVat, 92520) AS Amount
                                                         FROM Purchase p
                                                             LEFT JOIN Organizations org
                                                                 ON p.OrganizationId = org.OrganizationId
                                                             INNER JOIN PurchaseDetails pd
                                                                 ON p.PurchaseId = pd.PurchaseId
                                                             INNER JOIN ProductVATTypes pvt
                                                                 ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                    AND pvt.TransactionTypeId = 2
                                                                    AND pvt.ProductVATTypeId = 158
                                                         WHERE p.PurchaseTypeId = 1
                                                               AND org.BIN IS NULL
                                                               AND p.OrganizationId = @OrganizationId
                                                               AND YEAR(p.PurchaseDate) = @Year
                                                               AND MONTH(p.PurchaseDate) = @Month
                                                     ) tbl1
                                                 ),
                                                 83520
                                                       );

    UPDATE #TempMushokFor34
    SET BuyNonConcessionLocalPurchaseAmount = ISNULL(
                                              (
                                                  SELECT SUM(ISNULL(tbl1.Amount, 83520))
                                                  FROM
                                                  (
                                                      SELECT DISTINCT
                                                             p.PurchaseId,
                                                             ISNULL(p.TotalPriceWithoutVat, 83520) AS Amount
                                                      FROM Purchase p
                                                          --left join Organizations org on p.OrganizationId = org.OrganizationId
                                                          INNER JOIN PurchaseDetails pd
                                                              ON p.PurchaseId = pd.PurchaseId
                                                          INNER JOIN ProductVATTypes pvt
                                                              ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                 AND pvt.TransactionTypeId = 2
                                                                 AND pvt.ProductVATTypeId = 190
                                                      WHERE p.PurchaseTypeId = 1
                                                            AND p.OrganizationId = @OrganizationId
                                                            AND YEAR(p.PurchaseDate) = @Year
                                                            AND MONTH(p.PurchaseDate) = @Month
                                                  ) tbl1
                                              ),
                                              12140
                                                    ),
        BuyNonConcessionLocalPurchaseVAT = ISNULL(
                                           (
                                               SELECT SUM(ISNULL(tbl1.vat, 12140))
                                               FROM
                                               (
                                                   SELECT DISTINCT
                                                          p.PurchaseId,
                                                          ISNULL(p.TotalVAT, 12140) AS vat
                                                   FROM Purchase p
                                                       --left join Organizations org on p.OrganizationId = org.OrganizationId
                                                       INNER JOIN PurchaseDetails pd
                                                           ON p.PurchaseId = pd.PurchaseId
                                                       INNER JOIN ProductVATTypes pvt
                                                           ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                              AND pvt.TransactionTypeId = 2
                                                              AND pvt.ProductVATTypeId = 190
                                                   WHERE p.PurchaseTypeId = 1
                                                         AND p.OrganizationId = @OrganizationId
                                                         AND YEAR(p.PurchaseDate) = @Year
                                                         AND MONTH(p.PurchaseDate) = @Month
                                               ) tbl1
                                           ),
                                           12140
                                                 );

    UPDATE #TempMushokFor34
    SET BuyNonConcessionImportedPurchaseAmount = ISNULL(
                                                 (
                                                     SELECT SUM(ISNULL(tbl1.Amount, 12140))
                                                     FROM
                                                     (
                                                         SELECT DISTINCT
                                                                p.PurchaseId,
                                                                ISNULL(p.TotalPriceWithoutVat, 12140) AS Amount
                                                         FROM Purchase p
                                                             --left join Organizations org on p.OrganizationId = org.OrganizationId
                                                             INNER JOIN PurchaseDetails pd
                                                                 ON p.PurchaseId = pd.PurchaseId
                                                             INNER JOIN ProductVATTypes pvt
                                                                 ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                    AND pvt.TransactionTypeId = 2
                                                                    AND pvt.ProductVATTypeId = 191
                                                         WHERE p.PurchaseTypeId = 2
                                                               AND p.OrganizationId = @OrganizationId
                                                               AND YEAR(p.PurchaseDate) = @Year
                                                               AND MONTH(p.PurchaseDate) = @Month
                                                     ) tbl1
                                                 ),
                                                 3230
                                                       ),
        BuyNonConcessionImportedPurchaseVAT = ISNULL(
                                              (
                                                  SELECT SUM(ISNULL(tbl1.vat, 3230))
                                                  FROM
                                                  (
                                                      SELECT DISTINCT
                                                             p.PurchaseId,
                                                             ISNULL(p.TotalVAT, 3230) AS vat
                                                      FROM Purchase p
                                                          --left join Organizations org on p.OrganizationId = org.OrganizationId
                                                          INNER JOIN PurchaseDetails pd
                                                              ON p.PurchaseId = pd.PurchaseId
                                                          INNER JOIN ProductVATTypes pvt
                                                              ON pd.ProductVATTypeId = pvt.ProductVATTypeId
                                                                 AND pvt.TransactionTypeId = 2
                                                                 AND pvt.ProductVATTypeId = 191
                                                      WHERE p.PurchaseTypeId = 2
                                                            AND p.OrganizationId = @OrganizationId
                                                            AND YEAR(p.PurchaseDate) = @Year
                                                            AND MONTH(p.PurchaseDate) = @Month
                                                  ) tbl1
                                              ),
                                              3230
                                                    );

    UPDATE t
    SET t.Tbl4Total23Kh = t.BuyIdealRetedProductVATLocal + t.BuyIdealRetedProductVATImport
                          + t.BuyNonStandardRatedProductVATLocal + t.BuyNonStandardRatedProductVATImport
                          + t.BuySpecifiedRatedProductVAT + t.BuyNonConcessionProductVATTurnOver
                          + t.BuyNonConcessionProductVATUnregistered + t.BuyNonConcessionLocalPurchaseVAT
                          + t.BuyNonConcessionImportedPurchaseVAT
    FROM #TempMushokFor34 t;

    -------------------------END----Table 4--------------------------------

    -----------------------Start---Table 5---------------------------------
    --Tbl5Row1DueToDeductByCustomer

    UPDATE #TempMushokFor34
    SET Tbl5Row1Note24 = ISNULL(
                         (
                             SELECT SUM(ISNULL(s.TotalVAT, 5240))
                             FROM Sales s
                                 INNER JOIN SalesDetails sd
                                     ON s.SalesId = sd.SalesId
                                 INNER JOIN Customer c
                                     ON s.CustomerId = c.CustomerId
                                 INNER JOIN Organizations org
                                     ON c.CustomerOrganizationId = org.OrganizationId
                             --inner join Purchase p on p.InvoiceNo = sd.PurchaseInvoice
                             --inner join PurchaseDetails pd on p.PurchaseId = pd.PurchaseId and pd.ProductId = sd.ProductId
                             WHERE s.OrganizationId = @OrganizationId
                                   AND YEAR(s.SalesDate) = @Year
                                   AND MONTH(s.SalesDate) = @Month
                                   AND org.IsDeductVatInSource = 0
                         ),
                         5240
                               );


    --Tbl5Row2DueToDeductByPur
    UPDATE #TempMushokFor34
    SET Tbl5Row2Note25 = ISNULL(
                         (
                             SELECT SUM(ISNULL(pd.VATPercent, 15))
                             FROM Sales s
                                 INNER JOIN SalesDetails sd
                                     ON s.SalesId = sd.SalesId
                                 --inner join Customer c on s.CustomerId = c.CustomerId
                                 --inner join Organizations org on c.CustomerOrganizationId = org.OrganizationId
                                 INNER JOIN Purchase p
                                     ON p.InvoiceNo = s.InvoiceNo
                                 INNER JOIN PurchaseDetails pd
                                     ON p.PurchaseId = pd.PurchaseId
                                        AND pd.ProductId = sd.ProductId
                             WHERE s.OrganizationId = @OrganizationId
                                   AND YEAR(s.SalesDate) = @Year
                                   AND MONTH(s.SalesDate) = @Month
                         --and org.IsDeductVatInSource = 1
                         --and s.PaymentMethodId = 1
                         ),
                         15
                               );


    UPDATE #TempMushokFor34
    SET Tbl5Row3Note26 = ISNULL(
                         (
                             SELECT SUM(ISNULL(sd.VATPercent, 15))
                             FROM Sales s
                                 INNER JOIN SalesDetails sd
                                     ON s.SalesId = sd.SalesId
                             --inner join Purchase p on p.InvoiceNo = sd.PurchaseInvoice
                             --inner join PurchaseDetails pd on p.PurchaseId = pd.PurchaseId and pd.ProductId = sd.ProductId
                             WHERE s.OrganizationId = @OrganizationId
                                   AND YEAR(s.SalesDate) = @Year
                                   AND MONTH(s.SalesDate) = @Month
                         ),
                         15
                               );

    UPDATE t
    SET t.Tbl5Row5TotalNote28 = t.Tbl5Row1Note24 + t.Tbl5Row2Note25 + t.Tbl5Row3Note26
                                + ISNULL(t.Tbl5Row4Note27, 90420)
    FROM #TempMushokFor34 t;

    -----------------------END---Table 5---------------------------------
    ---------------------Start----Table 6--------------------------------
    UPDATE #TempMushokFor34
    SET Tbl6Row1Note29 = ISNULL(
                         (
                             SELECT SUM(ISNULL(p.TotalVAT, 18540))
                             FROM Sales s
                                 INNER JOIN SalesDetails sd
                                     ON s.SalesId = sd.SalesId
                                 --inner join Customer c on s.CustomerId = c.CustomerId
                                 INNER JOIN Organizations org
                                     ON s.OrganizationId = org.OrganizationId
                                 INNER JOIN Purchase p
                                     ON p.InvoiceNo = s.InvoiceNo
                                 INNER JOIN PurchaseDetails pd
                                     ON p.PurchaseId = pd.PurchaseId
                                        AND pd.ProductId = sd.ProductId
                             WHERE s.OrganizationId = @OrganizationId
                                   AND YEAR(s.SalesDate) = @Year
                                   AND MONTH(s.SalesDate) = @Month
                                   AND org.IsDeductVatInSource = 0
                         ),
                         18540
                               );


    UPDATE #TempMushokFor34
    SET Tbl6Row2Note30 = ISNULL(
                         (
                             SELECT SUM(ISNULL(p.TotalVAT, 18540))
                             FROM Purchase p
                                 INNER JOIN Organizations org
                                     ON p.OrganizationId = org.OrganizationId
                                 INNER JOIN PurchaseTypes pt
                                     ON p.PurchaseTypeId = pt.PurchaseTypeId
                             WHERE org.OrganizationId = @OrganizationId
                                   AND YEAR(p.CreatedTime) = @Year
                                   AND MONTH(p.CreatedTime) = @Month
                                   AND p.PurchaseDate IS NOT NULL
                                   AND p.PurchaseTypeId IN ( 1, 2 )
                         ),
                         18540
                               );

    UPDATE #TempMushokFor34
    SET Tbl6Row3Note31 = ISNULL(
                         (
                             SELECT SUM(ISNULL(p.TotalVAT, 18540))
                             FROM Purchase p
                                 INNER JOIN Organizations org
                                     ON p.OrganizationId = org.OrganizationId
                                 INNER JOIN PurchaseTypes pt
                                     ON p.PurchaseTypeId = pt.PurchaseTypeId
                             WHERE org.OrganizationId = @OrganizationId
                                   AND YEAR(p.CreatedTime) = @Year
                                   AND MONTH(p.CreatedTime) = @Month
                                   AND p.PurchaseReasonId = 3 ---For Export finish goods
                         ),
                         18540
                               );


    UPDATE #TempMushokFor34
    SET Tbl6Row4Note32 = ISNULL(
                         (
                             SELECT SUM(ISNULL(p.TotalVAT, 12230))
                             FROM Purchase p
                                 INNER JOIN Organizations org
                                     ON p.OrganizationId = org.OrganizationId
                                 INNER JOIN PurchaseTypes pt
                                     ON p.PurchaseTypeId = pt.PurchaseTypeId
                             WHERE org.OrganizationId = @OrganizationId
                                   AND YEAR(p.PurchaseDate) = @Year
                                   AND MONTH(p.PurchaseDate) = @Month
                                   AND p.PurchaseReasonId = 3 ---For Export finish goods
                         ),
                         12230
                               );

    UPDATE #TempMushokFor34
    SET Tbl6Row4Note32 = ISNULL(
                         (
                             SELECT SUM(ISNULL(s.TotalVAT, 12230))
                             FROM Sales s
                                 INNER JOIN Organizations org
                                     ON s.OrganizationId = org.OrganizationId
                             WHERE org.OrganizationId = @OrganizationId
                         --and year(s.CancelTime) = @Year
                         --and month(s.CancelTime) = @Month
                         ),
                         12230
                               );

    UPDATE t
    SET t.Tbl6Row5Note34Total = t.Tbl6Row1Note29 + t.Tbl6Row2Note30 + t.Tbl6Row3Note31 + t.Tbl6Row4Note32
                                + ISNULL(t.Tbl6Row5Note33, 12230)
    FROM #TempMushokFor34 t;

    -------------------------------End---Table 6-------------------------------

    ----------------------Table 7----------------------------------------------

    --Note 50 Todo
    UPDATE t
    SET t.Tbl7Row16Note50 = 0.0 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 51 Todo
    UPDATE t
    SET t.Tbl7Row17Note51 = 0.0 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 35
    UPDATE t
    SET t.Tbl7Row1Note35 = t.Tbl3Total9Ga - t.Tbl4Total23Kh + t.Tbl5Row5TotalNote28 - t.Tbl6Row5Note34Total
    FROM #TempMushokFor34 t;

    --Note 36
    UPDATE t
    SET t.Tbl7Row2Note36 = t.Tbl7Row1Note35 - ISNULL(t.Tbl7Row16Note50, 0) --- * first Tbl7Row16Note50 fill up
    FROM #TempMushokFor34 t;

    --Note 39
    UPDATE t
    SET t.Tbl7Row5Note39 = t.Tbl5Row3Note26
    FROM #TempMushokFor34 t;

    --Note 40
    UPDATE t
    SET t.Tbl7Row6Note40 = t.Tbl6Row4Note32
    FROM #TempMushokFor34 t;

    --Note 41
    UPDATE t
    SET t.Tbl7Row7Note41 = t.Tbl6Row3Note31
    FROM #TempMushokFor34 t;

    --Note 37
    UPDATE t
    SET t.Tbl7Row3Note37 = t.Tbl3Total9Ga + Tbl7Row5Note39 - Tbl7Row6Note40 - Tbl7Row7Note41
    FROM #TempMushokFor34 t;

    --Note 38
    UPDATE t
    SET t.Tbl7Row4Note38 = t.Tbl7Row3Note37 - t.Tbl7Row17Note51 --- 51 undefine
    FROM #TempMushokFor34 t;

    --Note 42 Todo
    UPDATE t
    SET t.Tbl7Row8Note42 = 4240 -- come from mushok table
    FROM #TempMushokFor34 t;

    --Note 43 Todo
    UPDATE t
    SET t.Tbl7Row9Note43 = 4240 -- come from mushok
    FROM #TempMushokFor34 t;

    --Note 44 Todo
    UPDATE t
    SET t.Tbl7Row10Note44 = 50100 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 45 Todo
    UPDATE t
    SET t.Tbl7Row11Note45 = 2022 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 46 Todo
    UPDATE t
    SET t.Tbl7Row12Note46 = 3045 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 47 Todo
    UPDATE t
    SET t.Tbl7Row13Note47 = 2022 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 48 Todo
    UPDATE t
    SET t.Tbl7Row14Note48 = 5026 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 49 Todo
    UPDATE t
    SET t.Tbl7Row15Note49 = 4590 -- come from another table
    FROM #TempMushokFor34 t;

    ----------------------End Table 7------------------------------------------

    -------------------Start Table 8------------------------------------------
    --Note 52 Todo
    UPDATE t
    SET t.Tbl8Row1Note52EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row1Note52Amount = 40230 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 53 Todo
    UPDATE t
    SET t.Tbl8Row2Note53EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row2Note53Amount = 30840 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 54 Todo
    UPDATE t
    SET t.Tbl8Row3Note54EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row3Note54Amount = 22100 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 55 Todo
    UPDATE t
    SET t.Tbl8Row4Note55EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row4Note55Amount = 38245 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 56 Todo
    UPDATE t
    SET t.Tbl8Row5Note56EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row5Note56Amount = 27430 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 57 Todo
    UPDATE t
    SET t.Tbl8Row6Note57EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row6Note57Amount = 4240 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 58 Todo
    UPDATE t
    SET t.Tbl8Row7Note58EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row7Note58Amount = 30150 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 59 Todo
    UPDATE t
    SET t.Tbl8Row8Note59EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row8Note59Amount = 25420 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 60 Todo
    UPDATE t
    SET t.Tbl8Row9Note60EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row9Note60Amount = 43230 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 61 Todo
    UPDATE t
    SET t.Tbl8Row10Note61EcoCode = 'COD000' -- come from another table
    FROM #TempMushokFor34 t;

    UPDATE t
    SET t.Tbl8Row10Note61Amount = 45320 -- come from another table
    FROM #TempMushokFor34 t;

    -------------------End Table 8--------------------------------------------

    -------------------Start Table 9------------------------------------------
    --Note 62 Todo
    UPDATE t
    SET t.Tbl9Row1Note62 = 40139 -- come from another table
    FROM #TempMushokFor34 t;

    --Note 62 Todo
    UPDATE t
    SET t.Tbl9Row2Note63 = 46120 -- come from another table
    FROM #TempMushokFor34 t;

    -------------------End Table 9--------------------------------------------

    -----------------Start Table 10-------------------------------------------
    --Todo
    UPDATE t
    SET t.Tbl10Row1 = 1 -- come from another table
    FROM #TempMushokFor34 t;

    -----------------End Table 10---------------------------------------------

    ---------------Start Table 11---------------------------------------------

    ---Todo, data come from a table
    UPDATE t
    SET t.Tbl11Name = 'T',
        t.Tbl11Designation = 'D',
        t.Tbl11Date = GETDATE(),
        t.Tbl11Mobile = '018280000',
        t.Tbl11Email = 'abc@bits.com'
    FROM #TempMushokFor34 t;
    ---------------End Table 11-----------------------------------------------


    SELECT tm34.Tbl1BIN,
           tm34.Tbl1OrganizationName,
           tm34.Tbl1Address,
           tm34.Tbl1BusinessNature,
           tm34.Tbl1EconomicNature,
           tm34.Tbl2TaxYear,
           tm34.Tbl2TaxMonth,
           tm34.Tbl2CurrentSubmissionCategory,
           tm34.Tbl2IsPreviousTaxSubmitted,
           tm34.Tbl2PreviousTaxSubmissionDate,
           tm34.Tbl3Note1DirectExportAmount,
           tm34.Tbl3Note2InDirectExportAmount,
           tm34.Tbl3Note3DiscountedProductAmount,
           tm34.Tbl3Note4IdealRatedProductAmount,
           tm34.Tbl3Note4IdealRatedProductVAT,
           tm34.Tbl3Note4IdealRatedProductSD,
           tm34.Tbl3Note5MaxRetailPriceProductAmount,
           tm34.Tbl3Note5MaxRetailPriceProductVAT,
           tm34.Tbl3Note5MaxRetailPriceProductSD,
           tm34.Tbl3Note6SpecificTaxBasedProductAmount,
           tm34.Tbl3Note6SpecificTaxBasedProductVAT,
           tm34.Tbl3Note6SpecificTaxBasedProductSD,
           tm34.Tbl3Note7DifferentTaxBasedProductAount,
           tm34.Tbl3Note7DifferentTaxBasedProductVAT,
           tm34.Tbl3Note7DifferentTaxBasedProductSD,
           tm34.Tbl3Note8RetailWholesaleBasedProductAmount,
           tm34.Tbl3Note8RetailWholesaleBasedProductSD,
           tm34.Tbl3Total9Ka,
           tm34.Tbl3Total9Kh,
           tm34.Tbl3Total9Ga,
           tm34.BuyZiroTaxableProductAmontLocal,
           tm34.BuyZiroTaxableProductAmontImport,
           tm34.BuyDiscountedProductAmountLocal,
           tm34.BuyDiscountedProductAmountImport,
           tm34.BuyIdealRetedProductAmountLocal,
           tm34.BuyIdealRetedProductVATLocal,
           tm34.BuyIdealRetedProductAmountImport,
           tm34.BuyIdealRetedProductVATImport,
           tm34.BuyNonStandardRatedProductAmountLocal,
           tm34.BuyNonStandardRatedProductVATLocal,
           tm34.BuyNonStandardRatedProductAmountImport,
           tm34.BuyNonStandardRatedProductVATImport,
           tm34.BuySpecifiedRatedProductAmount,
           tm34.BuySpecifiedRatedProductVAT,
           tm34.BuyNonConcessionProductAmountTurnOver,
           tm34.BuyNonConcessionProductVATTurnOver,
           tm34.BuyNonConcessionProductAmountUnregistered,
           tm34.BuyNonConcessionProductVATUnregistered,
           tm34.BuyNonConcessionLocalPurchaseAmount,
           tm34.BuyNonConcessionLocalPurchaseVAT,
           tm34.BuyNonConcessionImportedPurchaseAmount,
           tm34.BuyNonConcessionImportedPurchaseVAT,
           tm34.Tbl4Total23Kh,
           tm34.Tbl5Row1Note24,
           tm34.Tbl5Row2Note25,
           tm34.Tbl5Row3Note26,
           tm34.Tbl5Row4Note27,
           tm34.Tbl5Row5TotalNote28,
           tm34.Tbl6Row1Note29,
           tm34.Tbl6Row2Note30,
           tm34.Tbl6Row3Note31,
           tm34.Tbl6Row4Note32,
           tm34.Tbl6Row5Note33,
           tm34.Tbl6Row5Note34Total,
           tm34.Tbl7Row1Note35,
           tm34.Tbl7Row2Note36,
           tm34.Tbl7Row3Note37,
           tm34.Tbl7Row4Note38,
           tm34.Tbl7Row5Note39,
           tm34.Tbl7Row6Note40,
           tm34.Tbl7Row7Note41,
           tm34.Tbl7Row8Note42,
           tm34.Tbl7Row9Note43,
           tm34.Tbl7Row10Note44,
           tm34.Tbl7Row11Note45,
           tm34.Tbl7Row12Note46,
           tm34.Tbl7Row13Note47,
           tm34.Tbl7Row14Note48,
           tm34.Tbl7Row15Note49,
           tm34.Tbl7Row16Note50,
           tm34.Tbl7Row17Note51,
           tm34.Tbl8Row1Note52EcoCode,
           tm34.Tbl8Row1Note52Amount,
           tm34.Tbl8Row2Note53EcoCode,
           tm34.Tbl8Row2Note53Amount,
           tm34.Tbl8Row3Note54EcoCode,
           tm34.Tbl8Row3Note54Amount,
           tm34.Tbl8Row4Note55EcoCode,
           tm34.Tbl8Row4Note55Amount,
           tm34.Tbl8Row5Note56EcoCode,
           tm34.Tbl8Row5Note56Amount,
           tm34.Tbl8Row6Note57EcoCode,
           tm34.Tbl8Row6Note57Amount,
           tm34.Tbl8Row7Note58EcoCode,
           tm34.Tbl8Row7Note58Amount,
           tm34.Tbl8Row8Note59EcoCode,
           tm34.Tbl8Row8Note59Amount,
           tm34.Tbl8Row9Note60EcoCode,
           tm34.Tbl8Row9Note60Amount,
           tm34.Tbl8Row10Note61EcoCode,
           tm34.Tbl8Row10Note61Amount,
           tm34.Tbl9Row1Note62,
           tm34.Tbl9Row2Note63,
           tm34.Tbl10Row1,
           tm34.Tbl11Name,
           tm34.Tbl11Designation,
           tm34.Tbl11Date,
           tm34.Tbl11Mobile,
           tm34.Tbl11Email
    FROM #TempMushokFor34 tm34;

--select * from SalesType
--select * from SalesDeliveryType
--select * from ExportType
--select * from ProductVATTypes where TransactionTypeId = 1
--select * from ProductVATTypes where TransactionTypeId = 2
--select * from TransectionTypes
--select * from PurchaseTypes
--select * from PurchaseReason


END;

--exec GetMushokNinePointOne 6, 2019,7
GO
/****** Object:  StoredProcedure [dbo].[sp_delete_all_data]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[sp_delete_all_data]
AS
BEGIN
    -- disable referential integrity
    EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

    -- delete data
    EXEC sp_MSforeachtable 'DELETE FROM ?';

    -- enable referential integrity again 
    EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

    -- Reseed
    EXEC sp_MSforeachtable '
 Print ''?''
 IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1
  DBCC CHECKIDENT (''?'', RESEED, 0)';
END;
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnBasicInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnBasicInfo]
    -- Add the parameters for the stored procedure here
    @OrganizationId INT,
    @Year INT,
    @Month INT,
    @GenerateDate DATETIME,
    @InterestForDueVat DECIMAL(18, 2),
    @InterestForDueSd DECIMAL(18, 2),
    @FinancialPenalty DECIMAL(18, 2),
    @ExciseDuty DECIMAL(18, 2),
    @DevelopmentSurcharge DECIMAL(18, 2),
    @ItDevelopmentSurcharge DECIMAL(18, 2),
    @HealthDevelopmentSurcharge DECIMAL(18, 2),
    @EnvironmentProtectSurcharge DECIMAL(18, 2),
    @MiscIncrementalAdjustmentAmount DECIMAL(18, 2),
    @MiscIncrementalAdjustmentDesc NVARCHAR(500),
    @MiscDecrementalAdjustmentAmount DECIMAL(18, 2),
    @MiscDecrementalAdjustmentDesc NVARCHAR(500),
    @IsWantToGetBackClosingAmount BIT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @salesTypeLocalId INT = 1,
            @completedMushakGenerationStageId TINYINT = 6;
    -- @salesTypeExportId INT = 2;
    SET @GenerateDate = ISNULL(@GenerateDate, GETDATE());
    DECLARE @firstDayOfMonth DATETIME = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01',
            @firstDayOfNextMonth DATETIME = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month + 1 AS VARCHAR(2)) + '-01';
    DECLARE @mushakGenerationId INT,
            @inputVatAmmount DECIMAL(18, 2),
            @outputVatAmmount DECIMAL(18, 2),
            @inputSdAmmount DECIMAL(18, 2),
            @outputSdAmmount DECIMAL(18, 2);
    DECLARE @previousMushakMonth INT = MONTH(@firstDayOfMonth),
            @previousMushakYear INT = YEAR(@firstDayOfMonth);
    DECLARE @lastClosingVat DECIMAL(18, 2),
            @lastClosingSd DECIMAL(18, 2);

    SELECT @lastClosingVat
        = ISNULL(mg.AmountForVat, 0) + ISNULL(mg.LastClosingVatAmount, 0) - ISNULL(mg.PaidVatAmount, 0)
          + ISNULL(mg.ReturnAmountFromClosingVat, 0),
           @lastClosingSd
               = ISNULL(mg.AmountForSuppDuty, 0) + ISNULL(mg.LastClosingSuppDutyAmount, 0)
                 - ISNULL(mg.PaidSuppDutyAmount, 0) + ISNULL(mg.ReturnAmountFromClosingSd, 0)
    FROM dbo.MushakGeneration mg
    WHERE mg.MushakForYear = @previousMushakYear
          AND mg.MushakForMonth = @previousMushakMonth
          AND mg.IsActive = 1
          AND mg.MushakGenerationStageId = @completedMushakGenerationStageId;

    SET @lastClosingVat = ISNULL(@lastClosingVat, 0);
    SET @lastClosingSd = ISNULL(@lastClosingSd, 0);

    INSERT INTO dbo.MushakGeneration
    (
        OrganizationId,
        MushakForYear,
        MushakForMonth,
        GenerateDate,
        InterestForDueVat,
        InterestForDueSuppDuty,
        FinancialPenalty,
        ExciseDuty,
        DevelopmentSurcharge,
        ItDevelopmentSurcharge,
        HealthDevelopmentSurcharge,
        EnvironmentProtectSurcharge,
        LastClosingVatAmount,
        LastClosingSuppDutyAmount,
        MiscIncrementalAdjustmentAmount,
        MiscIncrementalAdjustmentDesc,
        MiscDecrementalAdjustmentAmount,
        MiscDecrementalAdjustmentDesc,
        IsSubmitted,
        IsWantToGetBackClosingAmount,
        IsActive,
        MushakGenerationStageId
    )
    VALUES
    (   @OrganizationId,                  -- OrganizationId - int
        @Year,                            -- MushakForYear - int
        @Month,                           -- MushakForMonth - int
        @GenerateDate,                    -- GenerateDate - datetime
        @InterestForDueVat,               -- InterestForDueVat - decimal(18, 2)
        @InterestForDueSd,                -- InterestForDueSuppDuty - decimal(18, 2)
        @FinancialPenalty,                -- FinancialPenalty - decimal(18, 2)
        @ExciseDuty,                      -- ExciseDuty - decimal(18, 2)
        @DevelopmentSurcharge,            -- DevelopmentSurcharge - decimal(18, 2)
        @ItDevelopmentSurcharge,          -- ItDevelopmentSurcharge - decimal(18, 2)
        @HealthDevelopmentSurcharge,      -- HealthDevelopmentSurcharge - decimal(18, 2)
        @EnvironmentProtectSurcharge,     -- EnvironmentProtectSurcharge - decimal(18, 2)
        @lastClosingVat,                  -- LastClosingVatAmount - decimal(18, 2)
        @lastClosingSd,                   -- LastClosingSuppDutyAmount - decimal(18, 2)
        @MiscIncrementalAdjustmentAmount, -- MiscIncrementalAdjustmentAmount - decimal(18, 2)
        @MiscIncrementalAdjustmentDesc,   -- MiscIncrementalAdjustmentDesc - nvarchar(500)
        @MiscDecrementalAdjustmentAmount, -- MiscDecrementalAdjustmentAmount - decimal(18, 2)
        @MiscDecrementalAdjustmentDesc,   -- MiscDecrementalAdjustmentDesc - nvarchar(500)
        0,                                -- IsSubmitted - bit
        @IsWantToGetBackClosingAmount,    -- IsWantToGetBackClosingAmount - bit
        1,                                -- IsActive - bit
        1                                 -- MushakGenerationStage - tinyint
        );

    SET @mushakGenerationId = SCOPE_IDENTITY();

    UPDATE dbo.Sales
    SET MushakGenerationId = @mushakGenerationId
    WHERE SalesDate >= @firstDayOfMonth
          AND SalesDate < @firstDayOfNextMonth
          AND
          (
              SalesTypeId = 1
              OR SalesTypeId = 2
          );

    UPDATE dbo.Purchase
    SET MushakGenerationId = @mushakGenerationId
    WHERE PurchaseDate >= @firstDayOfMonth
          AND PurchaseDate < @firstDayOfNextMonth
          AND PurchaseReasonId = 1;

    SELECT @outputVatAmmount = SUM(slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.VATPercent / 100),
           @outputSdAmmount = SUM(slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100)
    FROM dbo.SalesDetails slsDtl
        INNER JOIN dbo.Sales sls
            ON sls.SalesId = slsDtl.SalesId
    WHERE sls.MushakGenerationId = @mushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocalId;

    SELECT @inputVatAmmount = SUM(purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.VATPercent / 100),
           @inputSdAmmount = SUM(purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.SupplementaryDutyPercent / 100)
    FROM dbo.PurchaseDetails purcDtl
        INNER JOIN dbo.Purchase purc
            ON purc.PurchaseId = purcDtl.PurchaseId
    WHERE purc.MushakGenerationId = @mushakGenerationId;

    UPDATE dbo.MushakGeneration
    SET AmountForVat = @outputVatAmmount - @inputVatAmmount,
        AmountForSuppDuty = @outputSdAmmount - @inputSdAmmount
    WHERE MushakGenerationId = @mushakGenerationId;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnCompleteProcess]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnCompleteProcess]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[MushakGeneration]
    SET MushakGenerationStageId = 6
    WHERE MushakGenerationId = @MushakGenerationId;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnPaymentInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnPaymentInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT,
    @VatPaymentChallanNo NVARCHAR(20),
    @SuppDutyChallanNo NVARCHAR(20),
    @InterestForDueVatChallanNo NVARCHAR(20),
    @InterestForDueSuppDutyChallanNo NVARCHAR(20),
    @FinancialPenaltyChallanNo NVARCHAR(20),
    @ExciseDutyChallanNo NVARCHAR(20),
    @DevelopmentSurchargeChallanNo NVARCHAR(20),
    @ItDevelopmentSurchargeChallanNo NVARCHAR(20),
    @HealthDevelopmentSurchargeChallanNo NVARCHAR(20),
    @EnvironmentProtectSurchargeChallanNo NVARCHAR(20)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[MushakGeneration]
    SET VatPaymentChallanNo = @VatPaymentChallanNo,
        SuppDutyChallanNo = @SuppDutyChallanNo,
        InterestForDueVatChallanNo = @InterestForDueVatChallanNo,
        InterestForDueSuppDutyChallanNo = @InterestForDueSuppDutyChallanNo,
        FinancialPenaltyChallanNo = @FinancialPenaltyChallanNo,
        ExciseDutyChallanNo = @ExciseDutyChallanNo,
        DevelopmentSurchargeChallanNo = @DevelopmentSurchargeChallanNo,
        ItDevelopmentSurchargeChallanNo = @ItDevelopmentSurchargeChallanNo,
        HealthDevelopmentSurchargeChallanNo = @HealthDevelopmentSurchargeChallanNo,
        EnvironmentProtectSurchargeChallanNo = @EnvironmentProtectSurchargeChallanNo,
        MushakGenerationStageId = 3
    WHERE MushakGenerationId = @MushakGenerationId;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnPlanToPaymentInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnReturnedAmountInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnReturnedAmountInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT,
    @ReturnAmountFromClosingVat DECIMAL(18, 2),
    @ReturnFromClosingVatChequeBankId INT,
    @ReturnFromClosingVatChequeNo NVARCHAR(50),
    @ReturnFromClosingVatChequeDate DATETIME,
    @ReturnAmountFromClosingSd DECIMAL(18, 2),
    @ReturnFromClosingSdChequeBankId INT,
    @ReturnFromClosingSdChequeNo NVARCHAR(50),
    @ReturnFromClosingSdChequeDate DATETIME
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[MushakGeneration]
    SET ReturnAmountFromClosingVat = @ReturnAmountFromClosingVat,
        ReturnFromClosingVatChequeBankId = @ReturnFromClosingVatChequeBankId,
        ReturnFromClosingVatChequeNo = @ReturnFromClosingVatChequeNo,
        ReturnFromClosingVatChequeDate = @ReturnFromClosingVatChequeDate,
        ReturnAmountFromClosingSd = @ReturnAmountFromClosingSd,
        ReturnFromClosingSdChequeBankId = @ReturnFromClosingSdChequeBankId,
        ReturnFromClosingSdChequeNo = @ReturnFromClosingSdChequeNo,
        ReturnFromClosingSdChequeDate = @ReturnFromClosingSdChequeDate,
        MushakGenerationStageId = 5
    WHERE MushakGenerationId = @MushakGenerationId;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpAddMushakReturnSubmissionInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpAddMushakReturnSubmissionInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT,
    @SubmissionDate DATETIME
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE [dbo].[MushakGeneration]
    SET IsSubmitted = 1,
        SubmissionDate = @SubmissionDate,
        MushakGenerationStageId = 4
    WHERE MushakGenerationId = @MushakGenerationId;
END;
GO
/****** Object:  StoredProcedure [dbo].[spBranchTransfer]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spBranchTransfer]
(
    
    @SaleId INT
)
AS
BEGIN


    SELECT 

	OrgPar.Name    AS PareName,
	OrgPar.BIN     AS PareBin,
	OrgPar.Address AS PareAddre,
	OrgPar.VatResponsiblePersonName     AS PareVatPName,
	OrgPar.VatResponsiblePersonDesignation  AS PareVatPDes,

	OrgSen.Name    AS SenName,
	OrgSen.Address AS SenAddre,
	
	OrgReci.Name    AS ReciName,
	OrgReci.Address AS ReciAddre,
	Sa.InvoiceNo,
	Sa.SalesDate,
	Pd.Name AS ProduName,
	Sd.Quantity AS Qty,
	(Sd.UnitPrice*Sd.Quantity)-[dbo].[FnGetCalculatedVat] ((Sd.UnitPrice*Sd.Quantity) ,sd.VATPercent,sd.SupplementaryDutyPercent)  AS Price,
     [dbo].[FnGetCalculatedVat] ((Sd.UnitPrice*Sd.Quantity) ,sd.VATPercent,sd.SupplementaryDutyPercent) as VtaAmount

    FROM Sales Sa 
	LEFT JOIN  Organizations OrgSen ON OrgSen.OrganizationId = Sa.OrganizationId
	LEFT JOIN  Organizations OrgReci ON OrgReci.OrganizationId = Sa.OtherBranchOrganizationId
	LEFT JOIN  Organizations OrgPar ON OrgPar.OrganizationId = OrgSen.ParentOrganizationId
	LEFT JOIN  SalesDetails Sd ON Sd.SalesId=Sa.SalesId
	LEFT JOIN  Products Pd ON Pd.ProductId=Sd.ProductId





	where 
	
	
	sa.SalesId = @SaleId
END;
GO
/****** Object:  StoredProcedure [dbo].[SpContractualFinishedGood]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Md Mustafizur Rahman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpContractualFinishedGood]
    -- Add the parameters for the stored procedure here
    @ContractVendorTransferRawMateriallId INT
   
AS
BEGIN

SELECT 

org.Name AS OrgName,
org.BIN  AS OrgBin,
org.Address AS OrgAddress,
vd.name AS VenName,
vd.BIN AS VenBin,
vd.Address AS VenAddress,
CV.ChallanNo AS ChallanNo,
CV.IssueDate AS ChallanIssueDate ,
ms.Name As MesurementName,
pd.Name AS ProductName,
CVD.Quantity AS Quantity,
org.VatResponsiblePersonName ,
org.VatResponsiblePersonDesignation
FROM ContractualProduction CV
LEFT JOIN ContractualProductionProductDetails CVD ON CV.ContractualProductionId = CVD.ContractualProductionId
LEFT JOIN Customer vd ON CV.CustomerId = vd.CustomerId
LEFT JOIN Organizations org ON org.OrganizationId = CV.OrganizationId
LEFT JOIN Products pd ON pd.ProductId = CVD.ProductId
LEFT JOIN MeasurementUnits ms ON ms.MeasurementUnitId = CVD.MeasurementUnitId

WHERE CV.ContractTypeId =2
AND 
 CV.ContractualProductionId = @ContractVendorTransferRawMateriallId
END;
GO
/****** Object:  StoredProcedure [dbo].[SpContractualrawMetaria]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Md Mustafizur Rahman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpContractualrawMetaria]
    -- Add the parameters for the stored procedure here
    @ContractVendorTransferRawMateriallId INT
   
AS
BEGIN

SELECT 

org.Name AS OrgName,
org.BIN  AS OrgBin,
org.Address AS OrgAddress,
vd.name AS VenName,
vd.BinNo AS VenBin,
vd.Address AS VenAddress,
RawMat.ChallanNo,
RawMat.ChallanIssueDate,
ms.Name As MesurementName,
pd.Name AS ProductName,
RawMatD.Quantity ,
org.VatResponsiblePersonName ,
org.VatResponsiblePersonDesignation

 FROM ContractualProductionTransferRawMaterial RawMat 
LEFT JOIN ContractualProductionTransferRawMaterialDetails RawMatD ON RawMatD.ContractualProductionTransferRawMaterialId = RawMat.ContractualProductionTransferRawMaterialId
LEFT JOIN ContractualProduction CV ON CV.ContractualProductionId = RawMat.ContractualProductionId
LEFT JOIN Vendor vd ON CV.VendorId = vd.VendorId
LEFT JOIN Organizations org ON org.OrganizationId = CV.OrganizationId
LEFT JOIN Products pd ON pd.ProductId = RawMatD.RawMaterialId
LEFT JOIN MeasurementUnits ms ON ms.MeasurementUnitId = RawMatD.MeasurementUnitId

WHERE RawMat.ContractualProductionTransferRawMaterialId = @ContractVendorTransferRawMateriallId

END;
GO
/****** Object:  StoredProcedure [dbo].[SpCreditMushak]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpCreditMushak] @CreditNoteID INT
AS
BEGIN

Declare @SuplimenteryDuty Decimal(18,2)
Select
CUS.CustomerId,
CUS.Name        AS CusName,
CUS.BIN         AS CusBin,
SL.InvoiceNo    AS SaleInvoice,
SL.SalesDate    AS SaleDate,
ORG.Name        AS OrgName,
ORG.BIN         AS OrgBin,
ORG.VatResponsiblePersonName AS VatPName,
ORG.VatResponsiblePersonDesignation AS VatPDes,
CR.CreditNoteId AS CreditNo ,
CR.CreatedTime  AS CrTime,
PD.Name         AS ProductName,
MJ.Name    AS Quantity,
CRD.ReturnQuantity AS ReturnQuantity,
SCD.UnitPrice      AS UnitPrice     ,
CR.ReasonOfReturn   AS ReasonOfReturn,
((CRD.ReturnQuantity * SCD.UnitPrice) * SCD.SupplementaryDutyPercent / 100) AS SupplementaryDutyAmount,
((CRD.ReturnQuantity * SCD.UnitPrice) + ((CRD.ReturnQuantity * SCD.UnitPrice) * SCD.SupplementaryDutyPercent / 100)) * SCD.VATPercent / (100 - SCD.VATPercent) AS VATAMOUNT,
0 AS KORTON,
(CRD.ReturnQuantity * SCD.UnitPrice) AS TotalAmount




FROM CreditNote  CR
Inner JOIN  CreditNoteDetail CRD on CR.CreditNoteId = CRD.CreditNoteId
Inner JOIN  Sales Sl ON SL.SalesId=CR.SalesId

Inner JOIN  SalesDetails SCD ON SCD.SalesDetailId=CRD.SalesDetailId
Inner JOIN  Organizations ORG ON ORG.OrganizationId = Sl.OrganizationId
Inner JOIN  Customer CUS ON CUS.CustomerId = Sl.CustomerId
Inner JOIN  Products PD ON PD.ProductId = SCD.ProductId
Inner JOIN  MeasurementUnits MJ ON MJ.MeasurementUnitId=SCD.MeasurementUnitId 

 
where CR.CreditNoteId = @CreditNoteID
Order by PD.Name
END

GO
/****** Object:  StoredProcedure [dbo].[SpCreditNote]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author:		<MD. Sabbir Reza>
CREATE PROCEDURE [dbo].[SpCreditNote]
	-- Add the parameters for the stored procedure here
	@SalesId INT,
	@ReasonOfReturn NVARCHAR(50),
	@ReturnDate DATETIME,
	@CreatedBy INT,
    @CreatedTime DATETIME,
	@CreditNoteDetails NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
	DECLARE @CreditNoteDetail TABLE
    (
        [SalesDetailId] [INT] NOT NULL,
        [ReturnQuantity] [DECIMAL] NOT NULL,
        [MeasurementUnitId] [INT] NOT NULL
    );

	DECLARE @CreditNoteId INT;
	INSERT INTO @CreditNoteDetail(
		SalesDetailId,
		ReturnQuantity,
        MeasurementUnitId
	)
	    SELECT jd.[SalesDetailId],
           jd.[ReturnQuantity],
           jd.[MeasurementUnitId]
    FROM
        OPENJSON(@CreditNoteDetails)
        WITH
        (
            [SalesDetailId] [INT],
            [ReturnQuantity] [DECIMAL],
            [MeasurementUnitId][INT]
          
        ) jd


	DECLARE @StockIn TABLE
    (
        [SalesDetailId] [INT] NOT NULL,
        [ReturnQuantity] [DECIMAL] NOT NULL,
		[StockInId] [INT] NOT NULL
    );
	INSERT INTO @StockIn(
		SalesDetailId,
		ReturnQuantity,
		StockInId
	)
	 SELECT jd.[SalesDetailId],
           jd.[ReturnQuantity],
		   sd.StockInId
    FROM
        OPENJSON(@CreditNoteDetails)
        WITH
        (
            [SalesDetailId] [INT],
            [ReturnQuantity] [DECIMAL]
          
        ) jd INNER JOIN SalesDetails sd on jd.SalesDetailId=sd.SalesDetailId
    -- Update statements for update saleReturnQty in stockin using saleDetailsId
		UPDATE dbo.StockIn
		SET SalesReturnQty=SalesReturnQty+ReturnQuantity
		FROM @StockIn tempsi
		where dbo.StockIn.StockInId=tempsi.StockInId
    -- Insert statements for procedure here
  INSERT INTO dbo.CreditNote
    (
      SalesId,
      ReasonOfReturn,
      ReturnDate,
      CreatedBy,
      CreatedTime
    )
    VALUES
    (@SalesId,@ReasonOfReturn,@ReturnDate, @CreatedBy,@CreatedTime);

	  SET @CreditNoteId = SCOPE_IDENTITY();
    --Insert CreditNote Details
    INSERT INTO dbo.CreditNoteDetail
    (
        CreditNoteId,
		SalesDetailId,
		ReturnQuantity,
        MeasurementUnitId,
        CreatedBy,
        CreatedTime
    )
    SELECT @CreditNoteId,             
           cd.SalesDetailId,          
           cd.ReturnQuantity,         
           cd.MeasurementUnitId,             
           @CreatedBy,                  
           @CreatedTime                 
    FROM @CreditNoteDetail cd;
END
GO
/****** Object:  StoredProcedure [dbo].[SpDamageInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MD.Mustafizur Rahman>
-- Create date: <18/08/2019>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpDamageInfo]
    @OrganizationId INT,
	@ProductId INT
AS
BEGIN

SELECT si.StockInId,
purc.InvoiceNo,
pr.BatchNo,
si.CurrentStock
FROM dbo.StockIn si
LEFT JOIN dbo.ProductionReceive pr
ON pr.ProductionReceiveId = si.ProductionReceiveId
LEFT JOIN dbo.PurchaseDetails purcDtl
ON purcDtl.PurchaseDetailId = si.PurchaseDetailId
LEFT JOIN dbo.Purchase purc
ON purc.PurchaseId = purcDtl.PurchaseId
WHERE si.CurrentStock > 0
AND si.OrganizationId=@OrganizationId
AND si.ProductId=@ProductId;
END

GO
/****** Object:  StoredProcedure [dbo].[SpDamageInsert]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MD.Mustafizur Rahman>
-- Create date: <20/08/2019>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpDamageInsert]
    @OrganizationId INT,
	@ProductId INT,
	@StockInId BIGINT,
	@DamageQty DECIMAL(18,2),
	@DamageTypeId INT,
	@Description NVARCHAR(1000),
	@CreatedBy INT
	
AS
BEGIN
      DECLARE @CreatedTime DATETIME;

      SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());


 
    IF @StockInId > 0
    BEGIN
        INSERT INTO dbo.Damage
              (
      OrganizationId,
      ProductId,
      StockInId,
	  DamageQty,
	  DamageTypeId,
	  Description,
	  IsActive,
	  CreatedBy,
      CreatedTime
    )
     VALUES
     (@OrganizationId,
	 @ProductId,
	 @StockInId,
	 @DamageQty,
 	 @DamageTypeId ,
	 @Description,
	 1 ,
	 @CreatedBy ,
	 @CreatedTime );


	-- Update statements for StockIn table here
     UPDATE dbo.StockIn 
	 SET 
	 DamageQuantity=DamageQuantity+@DamageQty 
	 WHERE StockInId=@StockInId;

   
    END;

	ELSE
	 BEGIN
	  DECLARE @Temp DECIMAL(18,2);
	  DECLARE @TempStocId INT;
	  DECLARE @TempCurrentStock DECIMAL(18,2);
	  SET @Temp = @DamageQty;
	 
	  WHILE @Temp!=0
	  BEGIN

	  SELECT TOP 1 
	  @TempCurrentStock=st.CurrentStock,
	  @TempStocId=st.StockInId
	  
	  FROM dbo.StockIn st
	    
		WHERE 
		st.OrganizationId = @OrganizationId
		AND st.ProductId = @ProductId
		AND st.CurrentStock>0
		
		ORDER BY st.StockInId;


      IF @Temp > @TempCurrentStock
        BEGIN
		
	INSERT INTO dbo.Damage
              (
      OrganizationId,
      ProductId,
      StockInId,
	  DamageQty,
	  DamageTypeId,
	  Description,
	  IsActive,
	  CreatedBy,
      CreatedTime
    )
     VALUES
     (@OrganizationId,
	 @ProductId,
	 @TempStocId,
	 @TempCurrentStock,
 	 @DamageTypeId ,
	 @Description,
	 1 ,
	 @CreatedBy ,
	 @CreatedTime );


	-- Update statements for StockIn table here
     UPDATE dbo.StockIn 
	 SET 
	 DamageQuantity=DamageQuantity+@TempCurrentStock 
	 WHERE 
	 StockInId=@TempStocId;


	 SET @Temp = @Temp - @TempCurrentStock;

	    END;

      ELSE
	  BEGIN
	  INSERT INTO dbo.Damage
              (
      OrganizationId,
      ProductId,
      StockInId,
	  DamageQty,
	  DamageTypeId,
	  Description,
	  IsActive,
	  CreatedBy,
      CreatedTime
    )
     VALUES
     (@OrganizationId,
	 @ProductId,
	 @TempStocId,
	 @TempCurrentStock,
 	 @DamageTypeId ,
	 @Description,
	 1 ,
	 @CreatedBy ,
	 @CreatedTime );

	-- Update statements for StockIn table here
     UPDATE dbo.StockIn 
	 SET 
	 DamageQuantity=DamageQuantity+@Temp 
	 WHERE 
	 StockInId=@TempStocId;

	 SET @Temp = @Temp - @Temp;

	  END;
	  END;


-- Insert statements for Damage table here
      
  END;
  END;
GO
/****** Object:  StoredProcedure [dbo].[SpDamageList]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MD.Mustafizur Rahman>
-- Create date: <20/08/2019>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpDamageList]
@OrganizationId INT
AS
BEGIN

SELECT dm.DamageId,
         og.Name AS Org_Name,
         pd.Name AS Pr_Name,
         dm.DamageQty AS Qty,
         dt.Name AS D_Type,
         dm.Description ,
         dm.IsActive ,
         us.UserName AS U_Name,
         dm.CreatedTime 
   FROM dbo.Damage dm 
       LEFT JOIN dbo.Organizations og
   ON 
       dm.OrganizationId=og.OrganizationId 
       LEFT JOIN dbo.Products pd 
   ON 
       dm.ProductId = pd.ProductId 
       LEFT JOIN dbo.DamageType dt 
   ON 
       dm.DamageTypeId=dt.DamageTypeId
       LEFT JOIN dbo.Users us 
   ON 
       dm.CreatedBy = us.UserId
	   
	   WHERE dm.OrganizationId=@OrganizationId;
END

GO
/****** Object:  StoredProcedure [dbo].[SpDashboard]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MD.Mustafizur Rahman>
-- Create date: <22/08/2019>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpDashboard]
   @OrganizationId INT,
	@FromDate DATETIME,
	@ToDate DATETIME
AS
BEGIN

DECLARE @PurchaseAmount DECIMAL(18,2);
DECLARE @SalesAmount DECIMAL(18,2);
DECLARE @PurchaseDueAmount DECIMAL(18,2);
DECLARE @SalesDueAmount DECIMAL(18,2);
DECLARE @ProductionCost DECIMAL(18,2);
DECLARE @PaidTax DECIMAL(18,2);
DECLARE @TotalRawMat DECIMAL(18,2);
DECLARE @TotalFinishGd DECIMAL(18,2);

SELECT @PurchaseAmount=SUM(ps.PaidAmount), @PurchaseDueAmount=SUM(ps.DueAmount) FROM dbo.Purchase ps WHERE ps.OrganizationId = @OrganizationId AND ps.CreatedTime >= @FromDate AND ps.CreatedTime <= @ToDate ;

SELECT @SalesAmount=SUM(sl.PaymentReceiveAmount),@SalesDueAmount=SUM(sl.PaymentDueAmount) FROM dbo.Sales sl WHERE sl.OrganizationId = @OrganizationId AND sl.CreatedTime >= @FromDate AND sl.CreatedTime <= @ToDate ;

SELECT @ProductionCost = SUM(pr.MaterialCost) FROM dbo.ProductionReceive pr WHERE pr.OrganizationId = @OrganizationId AND pr.CreatedTime >= @FromDate AND pr.CreatedTime <= @ToDate;

SELECT @TotalRawMat = SUM(st.CurrentStock) FROM dbo.StockIn st LEFT JOIN dbo.Products pd ON pd.ProductId = st.ProductId WHERE pd.IsRawMaterial = 1 AND pd.IsActive=1; 
SELECT @TotalFinishGd = SUM(st.CurrentStock) FROM dbo.StockIn st LEFT JOIN dbo.Products pd ON pd.ProductId = st.ProductId WHERE pd.IsSellable = 1 AND pd.IsActive=1; 


SELECT @PurchaseAmount AS PurchaseAmount
       ,@PurchaseDueAmount AS PurchaseDue
       ,@SalesAmount AS SalesAmount
	   ,@SalesDueAmount AS SalesDue
	   ,@ProductionCost AS ProductionCost
	  ,@TotalRawMat AS TotalRawMaterial
	  ,@TotalFinishGd AS TotalFinishedGood
	   -- EXEC SpDashboard @OrganizationId=16 ,@FromDate='2019/01/01' ,@ToDate = '2019/09/01'






END

GO
/****** Object:  StoredProcedure [dbo].[SpDebitMushak]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpDebitMushak] 
@DEBITNOTEID INT
AS
BEGIN

Declare @SuplimenteryDuty Decimal(18,2)
Select
VEN.VendorId,
VEN.Name        AS VENName,
VEN.BinNo       AS VENBin,
PS.InvoiceNo    AS PSInvoice,
PS.PurchaseDate AS PSDate,
ORG.Name        AS OrgName,
ORG.BIN         AS OrgBin,
ORG.VatResponsiblePersonName AS VatPName,
ORG.VatResponsiblePersonDesignation AS VatPDes,
DR.DebitNoteId  AS DebitNo ,
DR.CreatedTime  AS DrTime,
PD.Name         AS ProductName,
MJ.Name         AS Quantity,
DRD.ReturnQuantity AS ReturnQuantity,
PDD.UnitPrice   AS UnitPrice     ,
DR.ReasonOfReturn   AS ReasonOfReturn,
((DRD.ReturnQuantity * PDD.UnitPrice) * PDD.SupplementaryDutyPercent / 100) AS SupplementaryDutyAmount,
((DRD.ReturnQuantity * PDD.UnitPrice) + ((DRD.ReturnQuantity * PDD.UnitPrice) * PDD.SupplementaryDutyPercent / 100)) * PDD.VATPercent / (100 - PDD.VATPercent) AS VATAMOUNT,
0 AS KORTON,
(DRD.ReturnQuantity * PDD.UnitPrice) AS TotalAmount

FROM DebitNote  DR
Inner JOIN  DebitNoteDetail DRD on DR.DebitNoteId = DRD.DebitNoteId
Inner JOIN  Purchase PS ON  PS.PurchaseId=DR.PurchaseId
Inner JOIN  PurchaseDetails PDD ON PDD.PurchaseDetailId=DRD.PurchaseDetailId
Inner JOIN  Organizations ORG ON ORG.OrganizationId = PS.OrganizationId
Inner JOIN  Vendor VEN ON VEN.VendorId = PS.VendorId
Inner JOIN  Products PD ON PD.ProductId = PDD.ProductId
Inner JOIN  MeasurementUnits MJ ON MJ.MeasurementUnitId=PDD.MeasurementUnitId 

 
where DR.DebitNoteId = @DEBITNOTEID

END
GO
/****** Object:  StoredProcedure [dbo].[spGet6P3View]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<MD SABBIR REZA>
-- Create date: <2019-09-22>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGet6P3View]
	-- Add the parameters for the stored procedure here
	-- Add the parameters for the stored procedure here
	@InvoiceNo NVARCHAR(50)= '',
	@CustomerName NVARCHAR(200)= '',
	@FromDate DATETIME= NULL,
	@ToDate DATETIME= NULL

AS
BEGIN
SELECT SalesId,InvoiceNo,Isnull(cus.Name,'Not Found') as CustomerName,sl.SalesDate FROM Sales sl
left join Customer cus on sl.CustomerId=cus.CustomerId
--where 
--(
--@InvoiceNo='' OR
-- sl.InvoiceNo like '%'+@InvoiceNo+'%'
-- )
--AND 
--(
--@CustomerName='' OR cus.Name LIKE '%'+@CustomerName+'%'
--)
--AND 
--( 
--SL.SalesDate >= @FromDate AND SL.SalesDate< DATEADD(DAY, 1, @ToDate)
--)

END
GO
/****** Object:  StoredProcedure [dbo].[SpGetMushakNinePointOne]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  StoredProcedure [dbo].[SpGetMushakReturnChallanInfo]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,> [dbo].[SpGetMushakReturnChallanInfo] 5
-- =============================================
CREATE PROCEDURE [dbo].[SpGetMushakReturnChallanInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ChallanCopy TABLE
    (
        CopyName NVARCHAR(20) NOT NULL
    );

    INSERT INTO @ChallanCopy
    (
        CopyName
    )
    VALUES
    (N'১ম (মূল) কপি' -- CopyName - nvarchar(20)
        ),
    (N'২য় কপি' -- CopyName - nvarchar(20)
    ),
    (N'৩য় কপি' -- CopyName - nvarchar(20)
    );

    SELECT pInfo.MushakGenerationInfoId,
           pInfo.MushakGenerationId,
           pInfo.OrganizationId,
           OrganizationName = org.Name,
           OrganizationAddress = org.Address,
           pInfo.PurposeOfPayment,
           pInfo.PaidAmount,
           PaidAmountTakaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pInfo.PaidAmount, 0), 2)),
           PaidAmountPoishaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pInfo.PaidAmount, 0), 1)),
           PaidAmountInBanglaWords = N'',
           pInfo.EconomicCodeId,
           nec.EconomicCode,
           EconomicCode1stDisit = nec.Code1stDisit,
           EconomicCode2ndDisit = nec.Code2ndDisit,
           EconomicCode3rdDisit = nec.Code3rdDisit,
           EconomicCode4thDisit = nec.Code4thDisit,
           EconomicCode5thDisit = nec.Code5thDisit,
           EconomicCode6thDisit = nec.Code6thDisit,
           EconomicCode7thDisit = nec.Code7thDisit,
           EconomicCode8thDisit = nec.Code8thDisit,
           EconomicCode9thDisit = nec.Code9thDisit,
           EconomicCode10thDisit = nec.Code10thDisit,
           EconomicCode11thDisit = nec.Code11thDisit,
           EconomicCode12thDisit = nec.Code12thDisit,
           EconomicCode13thDisit = nec.Code13thDisit,
           pInfo.PaymentDate,
           pInfo.BankBranchId,
           BankName = bnk.NameInBangla,
           BankBranchName = bnkBrnch.NameInBangla,
           DistrictName = dist.NameInBangla,
           ChallanCopyName = cc.CopyName
    FROM dbo.FnGetMushakGenerationDepositInfo(@MushakGenerationId) pInfo
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = pInfo.OrganizationId
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = pInfo.EconomicCodeId
        INNER JOIN dbo.BankBranch bnkBrnch
            ON bnkBrnch.BankBranchId = pInfo.BankBranchId
        INNER JOIN dbo.Bank bnk
            ON bnk.BankId = bnkBrnch.BankId
        INNER JOIN dbo.District dist
            ON dist.DistrictId = bnkBrnch.DistrictId
        CROSS JOIN @ChallanCopy cc
    ORDER BY pInfo.MushakGenerationInfoId;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpGetProductAutocompleteForBom]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
           ISNULL(SUM(ISNULL(stk.CurrentStock, 0)), 0) AS MaxUseQty,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.StockIn stk
            ON stk.ProductId = prod.ProductId
               AND stk.CurrentStock > 0
			   AND stk.IsFinishedGoods = 0
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.IsRawMaterial = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%'
    GROUP BY prod.ProductId,
             prod.Name,
             prod.ModelNo,
             prod.Code,
             prod.MeasurementUnitId,
             mu.Name;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpGetProductAutocompleteForProductionReceive]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForSale 16, 'o'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForProductionReceive]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.IsSellable = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%'
    GROUP BY prod.ProductId,
             prod.Name,
             prod.ModelNo,
             prod.Code,
             prod.MeasurementUnitId,
             mu.Name;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpGetProductAutocompleteForPurchase]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC [SpGetProductAutocompleteForPurchase] 5, 'co'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForPurchase]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           25.00 AS DefaultImportDutyPercent,
           3.00 AS DefaultRegulatoryDutyPercent,
           ISNULL(sd.SdPercent, 0) AS DefaultSupplimentaryDutyPercent,
           ISNULL(pvt.DefaultVatPercent, 0) AS DefaultVatPercent,
           3.00 AS DefaultAdvanceTaxPercent,
           5.00 AS DefaultAdvanceIncomeTaxPercent,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.ProductVATs pv
            ON pv.ProductId = prod.ProductId
               AND pv.IsActive = 1
        LEFT JOIN dbo.ProductVATTypes pvt
            ON pvt.ProductVATTypeId = pv.ProductVATTypeId
               AND pvt.IsActive = 1
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.SupplimentaryDuty sd
            ON sd.ProductId = prod.ProductId
               AND sd.IsActive = 1
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%';
END;
GO
/****** Object:  StoredProcedure [dbo].[SpGetProductAutocompleteForSale]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForSale 16, 'o'
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
           prodPrice.SalesUnitPrice,
           pvt.DefaultVatPercent,
           ISNULL(sd.SdPercent, 0) AS SupplimentaryDutyPercent,
           ISNULL(SUM(ISNULL(stk.CurrentStock, 0)), 0) AS MaxSaleQty,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        INNER JOIN dbo.PriceSetup prodPrice
            ON prodPrice.ProductId = prod.ProductId
               AND prodPrice.IsActive = 1
        LEFT JOIN dbo.StockIn stk
            ON stk.ProductId = prod.ProductId
               AND stk.CurrentStock > 0
               AND stk.IsFinishedGoods = 1
        INNER JOIN dbo.ProductVATs pv
            ON pv.ProductId = prod.ProductId
               AND pv.IsActive = 1
        INNER JOIN dbo.ProductVATTypes pvt
            ON pvt.ProductVATTypeId = pv.ProductVATTypeId
               AND pvt.IsActive = 1
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.SupplimentaryDuty sd
            ON sd.ProductId = prod.ProductId
               AND sd.IsActive = 1
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.IsSellable = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%'
    GROUP BY prod.ProductId,
             prod.Name,
             prod.ModelNo,
             prod.Code,
             prodPrice.SalesUnitPrice,
             pvt.DefaultVatPercent,
             sd.SdPercent,
             prod.MeasurementUnitId,
             mu.Name;
END;
GO
/****** Object:  StoredProcedure [dbo].[spGetSalePaged]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MD SABBIR REZA>
-- Create date: <2019-09-25>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetSalePaged]
	-- Add the parameters for the stored procedure here
@PageSize INT=10,
@PageNumber INT=1,
@OrganizationId INT =0
AS
BEGIN
DECLARE @OffsetRow INT,
        @TotalRow INT,
		@SerialNo BIGINT
SET @OffsetRow = IIF(@PageNumber > 0, (@PageNumber - 1) * @PageSize, 0);

SELECT TOP(@PageSize) * 
FROM SaleView sv
WHERE sv.serial>@OffsetRow
ORDER BY sv.serial
       
END
GO
/****** Object:  StoredProcedure [dbo].[SpInputOutputCo_ofi]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpInputOutputCo_ofi] @Id INT
AS
BEGIN

select 
[ProductId]=prod.ProductId
,[OrganizationId]=prod.OrganizationId
,[OrganizationName]=org.Name
,[OrgAddress] = org.Address
,[OrgBin]=org.BIN
,[HSCode]=prod.HSCode
,[ProductName]=prod.[Name]
,[UnitName]=mu.[Name]
,[Cost] = pspc.[Cost]
,[WastageAmount]=(pspc.WastagePercentage*pspc.RequiredQty)/100
,[WastagePercentage]=pspc.WastagePercentage
,[TotalAmount]=pspc.RequiredQty+(pspc.WastagePercentage*pspc.RequiredQty)/100
,[OverHeadCostName]=ohc.[Name]
,[Cost1]=pspc.Cost
from Products prod
LEFT JOIN PriceSetup ps on prod.ProductId=ps.ProductId
LEFT JOIN PriceSetupProductCost pspc on ps.PriceSetupId=pspc.PriceSetupId
INNER JOIN MeasurementUnits mu on prod.MeasurementUnitId=mu.MeasurementUnitId
LEFT JOIN OverHeadCost ohc on pspc.OverHeadCostId=ohc.OverHeadCostId
Inner Join Organizations org ON org.OrganizationId=prod.OrganizationId

--LEFT JOIN 
where 1=1
AND ps.IsActive=1
AND prod.ProductId=@Id
ORDER BY prod.ProductId
END

GO
/****** Object:  StoredProcedure [dbo].[spInsertDebitNote]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Md. Sabbir Reza>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spInsertDebitNote]
	-- Add the parameters for the stored procedure here
	@PurchaseId INT,
	@ReasonOfReturn NVARCHAR(50),
	@ReturnDate DATETIME,
	@CreatedBy INT,
    @CreatedTime DATETIME,
	@DebitNoteDetails NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(100) = N'';
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
	DECLARE @DebitNoteDetail TABLE
    (
        [PurchaseDetailId] [INT] NOT NULL,
        [ReturnQuantity] [DECIMAL] NOT NULL,
        [MeasurementUnitId] [INT] NOT NULL
    );

	DECLARE @DebitNoteId INT;
	INSERT INTO @DebitNoteDetail(
		PurchaseDetailId,
		ReturnQuantity,
        MeasurementUnitId
	)
	    SELECT jd.[PurchaseDetailId],
           jd.[ReturnQuantity],
           jd.[MeasurementUnitId]
    FROM
        OPENJSON(@DebitNoteDetails)
        WITH
        (
            [PurchaseDetailId] [INT],
            [ReturnQuantity] [DECIMAL],
            [MeasurementUnitId][INT]
          
        ) jd

	DECLARE @StockIn TABLE
    (
        [PurchaseDetailId] [INT] NOT NULL,
        [ReturnQuantity] [DECIMAL] NOT NULL

    );
	INSERT INTO @StockIn(
		PurchaseDetailId,
		ReturnQuantity
	)
	 SELECT jd.[PurchaseDetailId],
           jd.[ReturnQuantity]

    FROM
        OPENJSON(@DebitNoteDetails)
        WITH
        (
            [PurchaseDetailId] [INT],
            [ReturnQuantity] [DECIMAL]
          
        ) jd 
    -- Update statements for update saleReturnQty in stockin using saleDetailsId
		UPDATE dbo.StockIn
		SET PurchaseReturnQty=PurchaseReturnQty+ReturnQuantity
		FROM @StockIn tempsi
		where dbo.StockIn.PurchaseDetailId=tempsi.PurchaseDetailId
    -- Insert statements for procedure here
  INSERT INTO dbo.DebitNote
    (
      PurchaseId,
      ReasonOfReturn,
      ReturnDate,
      CreatedBy,
      CreatedTime
    )
    VALUES
    (@PurchaseId,@ReasonOfReturn,@ReturnDate, @CreatedBy,@CreatedTime);

	  SET @DebitNoteId = SCOPE_IDENTITY();
    --Insert CreditNote Details
    INSERT INTO dbo.DebitNoteDetail
    (
        DebitNoteId,
		PurchaseDetailId,
		ReturnQuantity,
        MeasurementUnitId,
        CreatedBy,
        CreatedTime
    )
    SELECT @DebitNoteId,             
           dn.PurchaseDetailId,          
           dn.ReturnQuantity,         
           dn.MeasurementUnitId,             
           @CreatedBy,                  
           @CreatedTime                 
    FROM @DebitNoteDetail dn;
	COMMIT TRANSACTION;
	    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;
END
GO
/****** Object:  StoredProcedure [dbo].[SpInsertProductionReceive]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 29, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertProductionReceive]
    @BatchNo NVARCHAR(50),
    @OrganizationId INT,
    @ProductionId INT,
    @ProductId INT,
    @ReceiveQuantity DECIMAL(18, 2),
    @MeasurementUnitId INT,
    @ReceiveTime DATETIME,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @NoOfIteamsUsedInBom INT,
                @ErrorMsg NVARCHAR(100) = N'';

        DECLARE @BomMain TABLE
        (
            [BomMainSl] [INT] NOT NULL,
            [RawMaterialId] [INT] NOT NULL,
            [UsedQuantity] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );


        DECLARE @Bom TABLE
        (
            [BomSl] [INT] NOT NULL,
            [RawMaterialId] [INT] NOT NULL,
            [StockInId] [INT] NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [UsedQuantity] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );


        DECLARE @StockIn TABLE
        (
            [StockInId] [INT] NOT NULL,
            [CurrentStock] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] INT NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL
        );

        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        SET @ReceiveTime = ISNULL(@ReceiveTime, @CreatedTime);

        --Insert Data into Bom Main
        INSERT INTO @BomMain
        (
            BomMainSl,
            RawMaterialId,
            UsedQuantity,
            MeasurementUnitId
        )
        SELECT ROW_NUMBER() OVER (ORDER BY pspc.PriceSetupProductCostId) AS BomMainSl, -- BomMainSl - int
               pspc.RawMaterialId,                                                     -- RawMaterialId - int
               pspc.RequiredQty * @ReceiveQuantity,                                    -- UsedQuantity - decimal(18, 2)
               pspc.MeasurementUnitId                                                  -- MeasurementUnitId - int
        FROM dbo.PriceSetupProductCost pspc
            INNER JOIN dbo.PriceSetup ps
                ON ps.PriceSetupId = pspc.PriceSetupId
        WHERE ps.ProductId = @ProductId
              AND pspc.RawMaterialId IS NOT NULL
              AND ps.IsActive = 1;


        --SELECT ROW_NUMBER() OVER (ORDER BY jd.RawMaterialId) AS BomMainSl, -- BomMainSl - int
        --       jd.RawMaterialId,                                           -- RawMaterialId - int
        --       jd.UsedQuantity,                                            -- UsedQuantity - decimal(18, 2)
        --       jd.MeasurementUnitId                                        -- MeasurementUnitId - int 
        --FROM
        --    OPENJSON(@BomJson)
        --    WITH
        --    (
        --        [RawMaterialId] [INT],
        --        [UsedQuantity] [DECIMAL](18, 2),
        --        [MeasurementUnitId] [INT]
        --    ) jd;

        SELECT ROW_NUMBER() OVER (ORDER BY pspc.PriceSetupProductCostId) AS BomMainSl, -- BomMainSl - int
               pspc.RawMaterialId,                                                     -- RawMaterialId - int
               pspc.RequiredQty,                                                       -- UsedQuantity - decimal(18, 2)
               pspc.MeasurementUnitId                                                  -- MeasurementUnitId - int
        FROM dbo.PriceSetupProductCost pspc
            INNER JOIN dbo.PriceSetup ps
                ON ps.PriceSetupId = pspc.PriceSetupId
        WHERE ps.ProductId = @ProductId
              AND pspc.RawMaterialId IS NOT NULL
              AND ps.IsActive = 1;
        --End Insert Data into Bom Main

        DECLARE @ProcessingBomRow INT = 0,
                @ProdTotalStock DECIMAL(18, 2) = 0;
        SELECT @NoOfIteamsUsedInBom = COUNT(bm.BomMainSl)
        FROM @BomMain bm;

        --Variables for sales Bom Loop
        DECLARE @ProductQtyUsedInBom DECIMAL(18, 2),
                @RawMaterialId INT,
                @RawMaterialMeasurementUnitId INT,
                @StockInId INT,
                @RawMaterialUnitPrice DECIMAL(18, 2),
                @MaxUsageQty DECIMAL(18, 2),
                @ProdUsageQty DECIMAL(18, 2),
                @BomSl INT = 1;
        --End Variables for sales Detail loop

        --Insert sales using while loop
        WHILE @ProcessingBomRow < @NoOfIteamsUsedInBom
        BEGIN
            SET @ProcessingBomRow += 1;

            SELECT @ProductQtyUsedInBom = bm.UsedQuantity,
                   @RawMaterialId = bm.RawMaterialId,
                   @RawMaterialMeasurementUnitId = bm.MeasurementUnitId
            FROM @BomMain bm
            WHERE bm.BomMainSl = @ProcessingBomRow;

            DELETE FROM @StockIn;
            INSERT INTO @StockIn
            (
                StockInId,
                CurrentStock,
                MeasurementUnitId,
                UnitPrice
            )
            SELECT si.StockInId,                                                      -- StockInId - int
                   si.CurrentStock,                                                   -- CurrentStock - decimal(18, 2)
                   si.MeasurementUnitId,                                              -- MeasurementUnitId - int
                   COALESCE(si.EndUnitPriceWithoutVat, si.InitUnitPriceWithoutVat, 0) -- UnitPrice - decimal(18, 2)
            FROM dbo.StockIn si
            WHERE si.CurrentStock > 0
                  AND si.OrganizationId = @OrganizationId
                  AND si.ProductId = @RawMaterialId
                  AND si.IsActive = 1;

            SELECT @ProdTotalStock = SUM(si.CurrentStock)
            FROM @StockIn si;

            IF @ProductQtyUsedInBom > @ProdTotalStock
            BEGIN
                SET @ErrorMsg = N'Used quantity exceeds stock!!';
                RAISERROR(   @ErrorMsg, -- Message text.  
                             20,        -- Severity.  
                             -1         -- State.  
                         );
            END;

            WHILE @ProductQtyUsedInBom > 0
            BEGIN

                SELECT TOP (1)
                       @MaxUsageQty = si.CurrentStock,
                       @RawMaterialMeasurementUnitId = si.MeasurementUnitId,
                       @StockInId = si.StockInId,
                       @RawMaterialUnitPrice = si.UnitPrice
                FROM @StockIn si
                ORDER BY si.StockInId;



                IF @ProductQtyUsedInBom > @MaxUsageQty
                BEGIN
                    SET @ProdUsageQty = @MaxUsageQty;
                END;
                ELSE
                BEGIN
                    SET @ProdUsageQty = @ProductQtyUsedInBom;
                END;
                INSERT INTO @Bom
                (
                    BomSl,
                    RawMaterialId,
                    StockInId,
                    UnitPrice,
                    UsedQuantity,
                    MeasurementUnitId
                )
                VALUES
                (   @BomSl,                       -- BomSl - int
                    @RawMaterialId,               -- RawMaterialId - int
                    @StockInId,                   -- StockInId - int
                    @RawMaterialUnitPrice,        -- UnitPrice - decimal(18, 2)
                    @ProdUsageQty,                -- UsedQuantity - decimal(18, 2)
                    @RawMaterialMeasurementUnitId -- MeasurementUnitId - int
                    );
                SET @BomSl += 1;

                SET @ProductQtyUsedInBom = @ProductQtyUsedInBom - @ProdUsageQty;
                DELETE FROM @StockIn
                WHERE StockInId = @StockInId;

                UPDATE dbo.StockIn
                SET UsedInProductionQuantity = ISNULL(UsedInProductionQuantity, 0) + @ProdUsageQty
                WHERE StockInId = @StockInId;
            END;

        END;



        --End Insert sales using while loop

        DECLARE @MaterialCost DECIMAL(18, 2),
                @ProductionReceiveId INT;

        SELECT @MaterialCost = ISNULL(SUM(bm.UnitPrice * bm.UsedQuantity), 0)
        FROM @Bom bm;


        --Insert Production Receive
        INSERT INTO dbo.ProductionReceive
        (
            BatchNo,
            OrganizationId,
            ProductionId,
            ProductId,
            ReceiveQuantity,
            MeasurementUnitId,
            ReceiveTime,
            MaterialCost,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @BatchNo,           -- BatchNo - nvarchar(50)
            @OrganizationId,    -- OrganizationId - int
            @ProductionId,      -- ProductionId - int
            @ProductId,         -- ProductId - int
            @ReceiveQuantity,   -- ReceiveQuantity - decimal(18, 2)
            @MeasurementUnitId, -- MeasurementUnitId - int
            @ReceiveTime,       -- ReceiveTime - datetime
            @MaterialCost,      -- MaterialCost - decimal(18, 2)
            1,                  -- IsActive - bit
            @CreatedBy,         -- CreatedBy - int
            @CreatedTime        -- CreatedTime - datetime
            );
        --End Insert Production Receive
        -- Set Procuction Receive Id
        SET @ProductionReceiveId = SCOPE_IDENTITY();


        --Insert BOM
        INSERT INTO dbo.BillOfMaterial
        (
            ProductionReceiveId,
            RawMaterialId,
            UsedQuantity,
            MeasurementUnitId,
            StockInId,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        SELECT @ProductionReceiveId, -- ProductionReceiveId - bigint
               bm.RawMaterialId,     -- RawMaterialId - int
               bm.UsedQuantity,      -- UsedQuantity - decimal(18, 2)
               bm.MeasurementUnitId, -- MeasurementUnitId - int
               bm.StockInId,         -- StockInId - bigint
               1,                    -- IsActive - bit
               @CreatedBy,           -- CreatedBy - int
               @CreatedTime          -- CreatedTime - datetime
        FROM @Bom bm;
        --Insert BOM


        DECLARE @InitStock DECIMAL(18, 2),
                @InitUnitPrice DECIMAL(18, 2) = 0;
        SELECT @InitStock = ISNULL(SUM(si.CurrentStock), 0)
        FROM dbo.StockIn si
        WHERE si.CurrentStock > 0
              AND si.ProductId = @ProductId;
        SELECT TOP (1)
               @InitUnitPrice = ISNULL(si.EndUnitPriceWithoutVat, 0)
        FROM dbo.StockIn si
        WHERE si.ProductId = @ProductId
        ORDER BY si.StockInId DESC;



        --Insert StockIn
        INSERT INTO dbo.StockIn
        (
            OrganizationId,
            ProductId,
            ProductionReceiveId,
            PurchaseDetailId,
            InitialQuantity,
            InQuantity,
            InitUnitPriceWithoutVat,
            EndUnitPriceWithoutVat,
            MeasurementUnitId,
            SaleQuantity,
            DamageQuantity,
            UsedInProductionQuantity,
            PurchaseReturnQty,
            SalesReturnQty,
            IsFinishedGoods,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @OrganizationId,                                                                                        -- OrganizationId - int
            @ProductId,                                                                                             -- ProductId - int
            @ProductionReceiveId,                                                                                   -- ProductionReceiveId - bigint
            NULL,                                                                                                   -- PurchaseDetailId - int
            @InitStock,                                                                                             -- InitialQuantity - decimal(18, 2)
            @ReceiveQuantity,                                                                                       -- InQuantity - decimal(18, 2)
            @InitUnitPrice,                                                                                         -- InitUnitPriceWithoutVat - decimal(18, 2)
            ((@InitUnitPrice * @InitStock) + (@ReceiveQuantity * @MaterialCost)) / (@InitStock + @ReceiveQuantity), -- EndUnitPriceWithoutVat - decimal(18, 2)
            @MeasurementUnitId,                                                                                     -- MeasurementUnitId - int
            0,                                                                                                      -- SaleQuantity - decimal(18, 2)
            0,                                                                                                      -- DamageQuantity - decimal(18, 2)
            0,                                                                                                      -- UsedInProductionQuantity - decimal(18, 2)
            0,                                                                                                      -- PurchaseReturnQty - decimal(18, 2)
            1,                                                                                                      -- SalesReturnQty - decimal(18, 2)
            1,                                                                                                      -- IsFinishedGoods - bit
            1,                                                                                                      -- IsActive - bit
            @CreatedBy,                                                                                             -- CreatedBy - int
            @CreatedTime                                                                                            -- CreatedTime - datetime
            );
        --Insert StockIn

        --Insert content
        IF @ContentJson IS NOT NULL
           AND LEN(@ContentJson) > 0
        BEGIN
            INSERT INTO dbo.Content
            (
                DocumentTypeId,
                OrganizationId,
                FileUrl,
                MimeType,
                Node,
                ObjectId,
                ObjectPrimaryKey,
                IsActive,
                CreatedBy,
                CreatedTime
            )
            SELECT jd.DocumentTypeId,
                   @OrganizationId,
                   jd.FileUrl,
                   jd.MimeType,
                   NULL,
                   35,
                   @ProductionReceiveId,
                   1,
                   @CreatedBy,
                   @CreatedTime
            FROM
                OPENJSON(@ContentJson)
                WITH
                (
                    [DocumentTypeId] [INT],
                    [FileUrl] [NVARCHAR](500),
                    [MimeType] [NVARCHAR](50)
                ) jd;
        END;
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;

END;
GO
/****** Object:  StoredProcedure [dbo].[SpInsertPurchase]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 9, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertPurchase]
    @OrganizationId INT,
    @VendorId INT,
    @VatChallanNo NVARCHAR(50),
    @VatChallanIssueDate DATETIME,
    @VendorInvoiceNo NVARCHAR(50),
    @InvoiceNo NVARCHAR(50),
    @PurchaseDate DATETIME,
    @PurchaseTypeId INT,
    @PurchaseReasonId INT,
    @DiscountOnTotalPrice DECIMAL(18, 2),
    @IsVatDeductedInSource BIT,
    @VDSAmount DECIMAL(18, 2),
    @ExpectedDeliveryDate DATETIME,
    @DeliveryDate DATETIME,
    @LcNo NVARCHAR(50),
    @LcDate DATETIME,
    @BillOfEntry NVARCHAR(50),
    @BillOfEntryDate DATETIME,
    @DueDate DATETIME,
    @TermsOfLc NVARCHAR(500),
    @PoNumber NVARCHAR(50),
    @MushakGenerationId INT,
    @IsComplete BIT,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @PurchaseOrderDetailsJson NVARCHAR(MAX),
    @PurchasePaymentJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
    DECLARE @PurchaseDetails TABLE
    (
        [ProductId] [INT] NOT NULL,
        [ProductVATTypeId] [INT] NOT NULL,
        [Quantity] [DECIMAL](18, 2) NOT NULL,
        [UnitPrice] [DECIMAL](18, 2) NOT NULL,
        [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
        [ImportDutyPercent] [DECIMAL](18, 2) NOT NULL,
        [RegulatoryDutyPercent] [DECIMAL](18, 2) NOT NULL,
        [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
        [VATPercent] [DECIMAL](18, 2) NOT NULL,
        [AdvanceTaxPercent] [DECIMAL](18, 2) NOT NULL,
        [AdvanceIncomeTaxPercent] [DECIMAL](18, 2) NOT NULL,
        [MeasurementUnitId] [INT] NOT NULL
    );
    DECLARE @PurchasePayment TABLE
    (
        [PaymentMethodId] [INT] NOT NULL,
        [PaidAmount] [DECIMAL](18, 2) NOT NULL,
        [PaymentDate] [DATETIME] NOT NULL
    );
    DECLARE @NoOfIteams INT,
            @TotalDiscountOnIndividualProduct DECIMAL(18, 2),
            @TotalImportDuty DECIMAL(18, 2),
            @TotalRegulatoryDuty DECIMAL(18, 2),
            @TotalSupplementaryDuty DECIMAL(18, 2),
            @TotalVAT DECIMAL(18, 2),
            @TotalAdvanceTax DECIMAL(18, 2),
            @TotalAdvanceIncomeTax DECIMAL(18, 2),
            @TotalPriceWithoutVat DECIMAL(18, 2),
            @PaidAmount DECIMAL(18, 2),
            @PurchaseId INT;


    INSERT INTO @PurchaseDetails
    (
        ProductId,
        ProductVATTypeId,
        Quantity,
        UnitPrice,
        DiscountPerItem,
        ImportDutyPercent,
        RegulatoryDutyPercent,
        SupplementaryDutyPercent,
        VATPercent,
        AdvanceTaxPercent,
        AdvanceIncomeTaxPercent,
        MeasurementUnitId
    )
    SELECT jd.[ProductId],
           pv.[ProductVATTypeId],
           jd.[Quantity],
           jd.[UnitPrice],
           jd.[DiscountPerItem],
           jd.[ImportDutyPercent],
           jd.[RegulatoryDutyPercent],
           jd.[SupplementaryDutyPercent],
           jd.[Vatpercent],
           jd.[AdvanceTaxPercent],
           jd.[AdvanceIncomeTaxPercent],
           jd.[MeasurementUnitId]
    FROM
        OPENJSON(@PurchaseOrderDetailsJson)
        WITH
        (
            [ProductId] [INT],
            [Quantity] [DECIMAL](18, 2),
            [UnitPrice] [DECIMAL](18, 2),
            [DiscountPerItem] [DECIMAL](18, 2),
            [ImportDutyPercent] [DECIMAL](18, 2),
            [RegulatoryDutyPercent] [DECIMAL](18, 2),
            [SupplementaryDutyPercent] [DECIMAL](18, 2),
            [Vatpercent] [DECIMAL](18, 2),
            [AdvanceTaxPercent] [DECIMAL](18, 2),
            [AdvanceIncomeTaxPercent] [DECIMAL](18, 2),
            [MeasurementUnitId] [INT]
        ) jd
        INNER JOIN dbo.ProductVATs pv
            ON pv.ProductId = jd.ProductId
    WHERE pv.IsActive = 1;

    IF @PurchasePaymentJson IS NOT NULL
       AND LEN(@PurchasePaymentJson) > 0
    BEGIN
        INSERT INTO @PurchasePayment
        (
            PaymentMethodId,
            PaidAmount,
            PaymentDate
        )
        SELECT jd.PaymentMethodId,
               jd.PaidAmount,
               jd.PaymentDate
        FROM
            OPENJSON(@PurchasePaymentJson)
            WITH
            (
                [PaymentMethodId] [INT],
                [PaidAmount] [DECIMAL](18, 2),
                [PaymentDate] [DATETIME]
            ) jd;

    END;


    SELECT @NoOfIteams = COUNT(pod.ProductId),
           @TotalDiscountOnIndividualProduct = SUM(pod.Quantity * pod.DiscountPerItem),
           @TotalImportDuty = SUM((pod.ImportDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalRegulatoryDuty = SUM((pod.RegulatoryDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalSupplementaryDuty = SUM((pod.SupplementaryDutyPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalVAT = SUM((pod.VATPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalAdvanceTax = SUM((pod.VATPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalAdvanceIncomeTax = SUM((pod.AdvanceIncomeTaxPercent * pod.Quantity * pod.UnitPrice) / 100),
           @TotalPriceWithoutVat = SUM(pod.Quantity * pod.UnitPrice)
    FROM @PurchaseDetails pod;

    SELECT @PaidAmount = ISNULL(SUM(pp.PaidAmount), 0)
    FROM @PurchasePayment pp;

    --Insert Purchase Information
    INSERT INTO dbo.Purchase
    (
        OrganizationId,
        VendorId,
        VatChallanNo,
        VatChallanIssueDate,
        VendorInvoiceNo,
        InvoiceNo,
        PurchaseDate,
        PurchaseTypeId,
        PurchaseReasonId,
        NoOfIteams,
        TotalPriceWithoutVat,
        DiscountOnTotalPrice,
        TotalDiscountOnIndividualProduct,
        TotalImportDuty,
        TotalRegulatoryDuty,
        TotalSupplementaryDuty,
        TotalVAT,
        TotalAdvanceTax,
        TotalAdvanceIncomeTax,
        IsVatDeductedInSource,
        VDSAmount,
        PaidAmount,
        ExpectedDeliveryDate,
        DeliveryDate,
        LcNo,
        LcDate,
        BillOfEntry,
        BillOfEntryDate,
        DueDate,
        TermsOfLc,
        PoNumber,
        MushakGenerationId,
        IsComplete,
        CreatedBy,
        CreatedTime
    )
    VALUES
    (@OrganizationId, @VendorId, @VatChallanNo, @VatChallanIssueDate, @VendorInvoiceNo, @InvoiceNo, @PurchaseDate,
     @PurchaseTypeId, @PurchaseReasonId, @NoOfIteams, @TotalPriceWithoutVat, @DiscountOnTotalPrice,
     @TotalDiscountOnIndividualProduct, @TotalImportDuty, @TotalRegulatoryDuty, @TotalSupplementaryDuty, @TotalVAT,
     @TotalAdvanceTax, @TotalAdvanceIncomeTax, @IsVatDeductedInSource, @VDSAmount, @PaidAmount, @ExpectedDeliveryDate,
     @DeliveryDate, @LcNo, @LcDate, @BillOfEntry, @BillOfEntryDate, @DueDate, @TermsOfLc, @PoNumber,
     @MushakGenerationId, @IsComplete, @CreatedBy, @CreatedTime);




    --Get PurchaseId
    SET @PurchaseId = SCOPE_IDENTITY();


    --Insert Purchase Details
    INSERT INTO dbo.PurchaseDetails
    (
        PurchaseId,
        ProductId,
        ProductVATTypeId,
        Quantity,
        UnitPrice,
        DiscountPerItem,
        ImportDutyPercent,
        RegulatoryDutyPercent,
        SupplementaryDutyPercent,
        VATPercent,
        AdvanceTaxPercent,
        AdvanceIncomeTaxPercent,
        MeasurementUnitId,
        CreatedBy,
        CreatedTime
    )
    SELECT @PurchaseId,                 -- PurchaseId - int
           pd.ProductId,                -- ProductId - int
           pd.ProductVATTypeId,         -- ProductVATTypeId - int
           pd.Quantity,                 -- Quantity - decimal(18, 2)
           pd.UnitPrice,                -- UnitPrice - decimal(18, 2)
           pd.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
           pd.ImportDutyPercent,        -- ImportDutyPercent - decimal(18, 2)
           pd.RegulatoryDutyPercent,    -- RegulatoryDutyPercent - decimal(18, 2)
           pd.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
           pd.VATPercent,               -- VATPercent - decimal(18, 2)
           pd.AdvanceTaxPercent,        -- AdvanceTaxPercent - decimal(18, 2)
           pd.AdvanceIncomeTaxPercent,  -- AdvanceIncomeTaxPercent - decimal(18, 2)
           pd.MeasurementUnitId,        -- MeasurementUnitId - int
           @CreatedBy,                  -- CreatedBy - int
           @CreatedTime                 -- CreatedTime - datetime
    FROM @PurchaseDetails pd;


    --Insert into Stockin
    --Variables for Insert into Stockin
    DECLARE @PurchaseDetailId INT,
            @PurchaseProductId INT,
            @PurchaseProductQuantity DECIMAL(18, 2),
            @PurchaseProdMeasurementUnitId INT,
            @PurchaseUnitPrice DECIMAL(18, 2),
            @InitStock DECIMAL(18, 2),
            @InitUnitPrice DECIMAL(18, 2);
    --Variables for Insert into Stockin InitialQuantity
    DECLARE PURCH_DTL_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY FOR
    SELECT purchDtl.PurchaseDetailId,
           purchDtl.ProductId,
           purchDtl.Quantity,
           purchDtl.MeasurementUnitId,
           purchDtl.UnitPrice - purchDtl.DiscountPerItem AS PurchaseUnitPrice
    FROM dbo.PurchaseDetails purchDtl
    WHERE purchDtl.PurchaseId = @PurchaseId;

    OPEN PURCH_DTL_CURSOR;
    FETCH NEXT FROM PURCH_DTL_CURSOR
    INTO @PurchaseDetailId,
         @PurchaseProductId,
         @PurchaseProductQuantity,
         @PurchaseProdMeasurementUnitId,
         @PurchaseUnitPrice;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @InitStock = ISNULL(SUM(si.CurrentStock), 0)
        FROM dbo.StockIn si
        WHERE si.CurrentStock > 0
              AND si.ProductId = @PurchaseProductId;
        SELECT TOP (1)
               @InitUnitPrice = ISNULL(si.EndUnitPriceWithoutVat, 0)
        FROM dbo.StockIn si
        WHERE si.ProductId = @PurchaseProductId
        ORDER BY si.StockInId DESC;
        --PRINT (@PurchaseDetailId);
        INSERT INTO dbo.StockIn
        (
            OrganizationId,
            ProductId,
            ProductionReceiveId,
            PurchaseDetailId,
            InitialQuantity,
            InQuantity,
            InitUnitPriceWithoutVat,
            EndUnitPriceWithoutVat,
            MeasurementUnitId,
            SaleQuantity,
            DamageQuantity,
            UsedInProductionQuantity,
            PurchaseReturnQty,
            SalesReturnQty,
            IsFinishedGoods,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        VALUES
        (   @OrganizationId,                                      -- OrganizationId - int
            @PurchaseProductId,                                   -- ProductId - int
            NULL,                                                 -- ProductionReceiveId - bigint
            @PurchaseDetailId,                                    -- PurchaseDetailId - int
            @InitStock,                                           -- InitialQuantity - decimal(18, 2)
            @PurchaseProductQuantity,                             -- InQuantity - decimal(18, 2)
            @InitUnitPrice,                                       -- InitUnitPriceWithoutVat - decimal(18, 2)
            (ISNULL(@InitStock * @InitUnitPrice, 0) + @PurchaseProductQuantity * @PurchaseUnitPrice)
            / (ISNULL(@InitStock, 0) + @PurchaseProductQuantity), -- EndUnitPriceWithoutVat - decimal(18, 2)
            @PurchaseProdMeasurementUnitId,                       -- MeasurementUnitId - int
            0,                                                    -- SaleQuantity - decimal(18, 2)
            0,                                                    -- DamageQuantity - decimal(18, 2)
            0,                                                    -- UsedInProductionQuantity - decimal(18, 2)
            0,                                                    -- PurchaseReturnQty - decimal(18, 2)
            0,                                                    -- SalesReturnQty - decimal(18, 2)
            0,                                                    -- IsFinishedGoods - bit
            1,                                                    -- IsActive - bit
            @CreatedBy,                                           -- CreatedBy - int
            @CreatedTime                                          -- CreatedTime - datetime
            );
        FETCH NEXT FROM PURCH_DTL_CURSOR
        INTO @PurchaseDetailId,
             @PurchaseProductId,
             @PurchaseProductQuantity,
             @PurchaseProdMeasurementUnitId,
             @PurchaseUnitPrice;
    END;
    CLOSE PURCH_DTL_CURSOR;
    DEALLOCATE PURCH_DTL_CURSOR;

    INSERT INTO dbo.PurchasePayment
    (
        PurchaseId,
        PaymentMethodId,
        PaidAmount,
        PaymentDate,
        CreatedBy,
        CreatedTime
    )
    SELECT @PurchaseId,        -- PurchaseId - int
           pp.PaymentMethodId, -- PaymentMethodId - int
           pp.PaidAmount,      -- PaidAmount - decimal(18, 2)
           pp.PaymentDate,     -- PaymentDate - datetime
           @CreatedBy,         -- CreatedBy - int
           @CreatedTime        -- CreatedTime - datetime
    FROM @PurchasePayment pp;


    --Insert content
    IF @ContentJson IS NOT NULL
       AND LEN(@ContentJson) > 0
    BEGIN
        INSERT INTO dbo.Content
        (
            DocumentTypeId,
            OrganizationId,
            FileUrl,
            MimeType,
            Node,
            ObjectId,
            ObjectPrimaryKey,
            IsActive,
            CreatedBy,
            CreatedTime
        )
        SELECT jd.DocumentTypeId,
               @OrganizationId,
               jd.FileUrl,
               jd.MimeType,
               NULL,
               21,
               @PurchaseId,
               1,
               @CreatedBy,
               @CreatedTime
        FROM
            OPENJSON(@ContentJson)
            WITH
            (
                [DocumentTypeId] [INT],
                [FileUrl] [NVARCHAR](500),
                [MimeType] [NVARCHAR](50)
            ) jd;
    END;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpInsertSale]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 29, 2019
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpInsertSale]
    @InvoiceNo NVARCHAR(50),
    @VatChallanNo NVARCHAR(50),
    @OrganizationId INT,
    @DiscountOnTotalPrice DECIMAL(18, 2),
    @IsVatDeductedInSource BIT,
    @CustomerId INT,
    @ReceiverName NVARCHAR(200),
    @ReceiverContactNo VARCHAR(20),
    @ShippingAddress NVARCHAR(200),
    @ShippingCountryId INT,
    @SalesTypeId INT,
    @SalesDeliveryTypeId INT,
    @WorkOrderNo NVARCHAR(50),
    @SalesDate DATETIME,
    @ExpectedDeliveryDate DATETIME,
    @DeliveryDate DATETIME,
    @DeliveryMethodId INT,
    @ExportTypeId INT,
    @LcNo NVARCHAR(50),
    @LcDate DATETIME,
    @BillOfEntry NVARCHAR(50),
    @BillOfEntryDate DATETIME,
    @DueDate DATETIME,
    @TermsOfLc NVARCHAR(500),
    @CustomerPoNumber NVARCHAR(50),
    @OtherBranchOrganizationId INT,
    @IsComplete BIT,
    @IsTaxInvoicePrined BIT,
    @CreatedBy INT,
    @CreatedTime DATETIME,
    @SalesDetailsJson NVARCHAR(MAX),
    @PaymentReceiveJson NVARCHAR(MAX),
    @ContentJson NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(100) = N'';
        DECLARE @NoOfIteams INT,
                @TotalPriceWithoutVat DECIMAL(18, 2),
                @TotalDiscountOnIndividualProduct DECIMAL(18, 2),
                @TotalVAT DECIMAL(18, 2),
                @TotalSupplimentaryDuty DECIMAL(18, 2),
                @PaymentReceiveAmount DECIMAL(18, 2),
                @SalesId INT,
                @TaxInvoicePrintedTime DATETIME = NULL;

        IF @IsTaxInvoicePrined = 1
        BEGIN
            SET @TaxInvoicePrintedTime = GETDATE();
        END;


        DECLARE @SlsDtlMain TABLE
        (
            [SlsDtlMainSl] [INT] NOT NULL,
            [ProductId] [INT] NOT NULL,
            [ProductVATTypeId] [INT] NOT NULL,
            [Quantity] [DECIMAL](18, 2) NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
            [VATPercent] [DECIMAL](18, 2) NOT NULL,
            [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );


        DECLARE @SlsDtl TABLE
        (
            [ProductId] [INT] NOT NULL,
            [ProductVATTypeId] [INT] NOT NULL,
            [StockInId] [INT] NOT NULL,
            [Quantity] [DECIMAL](18, 2) NOT NULL,
            [UnitPrice] [DECIMAL](18, 2) NOT NULL,
            [DiscountPerItem] [DECIMAL](18, 2) NOT NULL,
            [VATPercent] [DECIMAL](18, 2) NOT NULL,
            [SupplementaryDutyPercent] [DECIMAL](18, 2) NOT NULL,
            [MeasurementUnitId] [INT] NOT NULL
        );

        DECLARE @StockIn TABLE
        (
            [StockInId] [INT] NOT NULL,
            [CurrentStock] [DECIMAL](18, 2) NOT NULL
        );
        DECLARE @PaymentReceive TABLE
        (
            [ReceivedPaymentMethodId] [INT] NOT NULL,
            [ReceiveAmount] [DECIMAL](18, 2) NOT NULL,
            [ReceiveDate] [DATETIME] NOT NULL
        );

        SET @CreatedTime = ISNULL(@CreatedTime, GETDATE());
        SET @SalesDate = ISNULL(@SalesDate, @CreatedTime);

        --Need to add logic 
        --SET @RemainingAmount = ISNULL(@InvoiceNo,COUNT(s.ProductId));--For new invoice	
        DECLARE @MaxSalseId INT;
        SELECT @MaxSalseId = MAX(SalesId)
        FROM dbo.Sales;
        SET @InvoiceNo = ISNULL(@InvoiceNo, 'INVOICE:' + CAST(ISNULL(@MaxSalseId, 0) + 1 AS VARCHAR(8)));


        IF @PaymentReceiveJson IS NOT NULL
           AND LEN(@PaymentReceiveJson) > 0
        BEGIN
            INSERT INTO @PaymentReceive
            (
                ReceivedPaymentMethodId,
                ReceiveAmount,
                ReceiveDate
            )
            SELECT jd.ReceivedPaymentMethodId,
                   jd.ReceiveAmount,
                   jd.ReceiveDate
            FROM
                OPENJSON(@PaymentReceiveJson)
                WITH
                (
                    [ReceivedPaymentMethodId] [INT],
                    [ReceiveAmount] [DECIMAL](18, 2),
                    [ReceiveDate] [DATETIME]
                ) jd;

        END;


        INSERT INTO @SlsDtlMain
        (
            SlsDtlMainSl,
            ProductId,
            ProductVATTypeId,
            Quantity,
            UnitPrice,
            DiscountPerItem,
            VATPercent,
            SupplementaryDutyPercent,
            MeasurementUnitId
        )
        SELECT ROW_NUMBER() OVER (ORDER BY jd.ProductId) AS SlsDtlMainSl, -- SlsDtlMainSl - int
               jd.ProductId,                                              -- ProductId - int
               pv.ProductVATTypeId,                                       -- ProductVATTypeId - int
               jd.Quantity,                                               -- Quantity - decimal(18, 2)
               jd.UnitPrice,                                              -- UnitPrice - decimal(18, 2)
               jd.DiscountPerItem,                                        -- DiscountPerItem - decimal(18, 2)
               jd.Vatpercent,                                             -- VATPercent - decimal(18, 2)
               jd.SupplementaryDutyPercent,                               -- SupplementaryDutyPercent - decimal(18, 2)
               jd.MeasurementUnitId                                       -- MeasurementUnitId - int
        FROM
            OPENJSON(@SalesDetailsJson)
            WITH
            (
                [ProductId] [INT],
                [Quantity] [DECIMAL](18, 2),
                [UnitPrice] [DECIMAL](18, 2),
                [DiscountPerItem] [DECIMAL](18, 2),
                [Vatpercent] [DECIMAL](18, 2),
                [SupplementaryDutyPercent] [DECIMAL](18, 2),
                [MeasurementUnitId] [INT]
            ) jd
            INNER JOIN dbo.ProductVATs pv
                ON pv.ProductId = jd.ProductId
                   AND pv.IsActive = 1;

        SELECT @PaymentReceiveAmount = ISNULL(SUM(pr.ReceiveAmount), 0)
        FROM @PaymentReceive pr;
        SELECT @NoOfIteams = MAX(sdm.SlsDtlMainSl),
               @TotalPriceWithoutVat = SUM(sdm.UnitPrice * sdm.Quantity),
               @TotalDiscountOnIndividualProduct = SUM(sdm.DiscountPerItem * sdm.Quantity),
               @TotalVAT = SUM(sdm.UnitPrice * sdm.Quantity * sdm.VATPercent / 100),
               @TotalSupplimentaryDuty = SUM(sdm.UnitPrice * sdm.Quantity * sdm.SupplementaryDutyPercent / 100)
        FROM @SlsDtlMain sdm;


        DECLARE @ProcessingOrderDetailRow INT = 0,
                @ProdTotalStock DECIMAL(18, 2) = 0;
        IF @NoOfIteams < 1
        BEGIN
            SET @ErrorMsg = N'Sale is not possible without products!!';
            RAISERROR(   'Sale is not possible without products!!', -- Message text.  
                         20,                                        -- Severity.  
                         -1                                         -- State.  
                     );
        END;

        --Insert Sales
        INSERT INTO dbo.Sales
        (
            InvoiceNo,
            VatChallanNo,
            OrganizationId,
            NoOfIteams,
            TotalPriceWithoutVat,
            DiscountOnTotalPrice,
            TotalDiscountOnIndividualProduct,
            TotalVAT,
            TotalSupplimentaryDuty,
            IsVatDeductedInSource,
            PaymentReceiveAmount,
            CustomerId,
            ReceiverName,
            ReceiverContactNo,
            ShippingAddress,
            ShippingCountryId,
            SalesTypeId,
            SalesDeliveryTypeId,
            WorkOrderNo,
            SalesDate,
            ExpectedDeliveryDate,
            DeliveryDate,
            DeliveryMethodId,
            ExportTypeId,
            LcNo,
            LcDate,
            BillOfEntry,
            BillOfEntryDate,
            DueDate,
            TermsOfLc,
            CustomerPoNumber,
            IsComplete,
            IsTaxInvoicePrined,
            TaxInvoicePrintedTime,
            MushakGenerationId,
            CreatedBy,
            CreatedTime,
			OtherBranchOrganizationId
        )
        VALUES
        (   @InvoiceNo,                        -- InvoiceNo - nvarchar(50)
            @VatChallanNo, @OrganizationId,    -- OrganizationId - int
            @NoOfIteams,                       -- NoOfIteams - int
            @TotalPriceWithoutVat,             -- TotalPriceWithoutVat - decimal(18, 2)
            @DiscountOnTotalPrice,             -- DiscountOnTotalPrice - decimal(18, 2)
            @TotalDiscountOnIndividualProduct, -- TotalDiscountOnIndividualProduct - decimal(18, 2)
            @TotalVAT,                         -- TotalVAT - decimal(18, 2)
            @TotalSupplimentaryDuty,           -- TotalSupplimentaryDuty - decimal(18, 2)
            @IsVatDeductedInSource,            -- IsVatDeductedInSource - bit
            @PaymentReceiveAmount,             -- PaymentReceiveAmount - decimal(18, 2)
            @CustomerId,                       -- CustomerId - int
            @ReceiverName,                     -- ReceiverName - nvarchar(200)
            @ReceiverContactNo,                -- ReceiverContactNo - varchar(20)
            @ShippingAddress,                  -- ShippingAddress - nvarchar(200)
            @ShippingCountryId,                -- ShippingCountryId - int
            @SalesTypeId,                      -- SalesTypeId - int
            @SalesDeliveryTypeId,              -- SalesDeliveryTypeId - int
            @WorkOrderNo,                      -- WorkOrderNo - nvarchar(50)
            @SalesDate,                        -- SalesDate - datetime
            @ExpectedDeliveryDate,             -- ExpectedDeleveryDate - datetime
            @DeliveryDate,                     -- DeliveryDate - datetime
            @DeliveryMethodId,                 -- DeliveryMethodId - int
            @ExportTypeId,                     -- ExportTypeId - int
            @LcNo,                             -- LcNo - nvarchar(50)
            @LcDate,                           -- LcDate - datetime
            @BillOfEntry,                      -- BillOfEntry - nvarchar(50)
            @BillOfEntryDate,                  -- BillOfEntryDate - datetime
            @DueDate,                          -- DueDate - datetime
            @TermsOfLc,                        -- TermsOfLc - nvarchar(500)
            @CustomerPoNumber,                 -- CustomerPoNumber - nvarchar(50)
            @IsComplete,                       -- IsComplete - bit
            @IsTaxInvoicePrined,               -- IsTaxInvoicePrined - bit
            @TaxInvoicePrintedTime, NULL,      -- MushakGenerationId - int
            @CreatedBy,                        -- CreatedBy - int
            @CreatedTime,                       -- CreatedTime - datetime
            @OtherBranchOrganizationId                       -- OtherBranchOrganizationId - int
            );


        --Get SalesId
        SET @SalesId = SCOPE_IDENTITY();

        --Variables for sales Detail Loop
        DECLARE @ProductId INT,
                @ProductQty DECIMAL(18, 2),
                @ProductVATTypeId INT,
                @StockInId INT,
                @UnitPrice DECIMAL(18, 2),
                @DiscountPerItem DECIMAL(18, 2),
                @VATPercent DECIMAL(18, 2),
                @SupplementaryDutyPercent DECIMAL(18, 2),
                @MeasurementUnitId INT,
                @MaxSaleQty DECIMAL(18, 2),
                @ProdSaleQty DECIMAL(18, 2);
        --End Variables for sales Detail loop

        --Insert sales using while loop
        WHILE @ProcessingOrderDetailRow < @NoOfIteams
        BEGIN
            SET @ProcessingOrderDetailRow += 1;

            SELECT @ProductId = sdm.ProductId,
                   @ProductVATTypeId = sdm.ProductVATTypeId,
                   @ProductQty = sdm.Quantity,
                   @UnitPrice = sdm.UnitPrice,
                   @DiscountPerItem = sdm.DiscountPerItem,
                   @VATPercent = sdm.VATPercent,
                   @SupplementaryDutyPercent = sdm.SupplementaryDutyPercent,
                   @MeasurementUnitId = sdm.MeasurementUnitId
            FROM @SlsDtlMain sdm
            WHERE sdm.SlsDtlMainSl = @ProcessingOrderDetailRow;

            DELETE FROM @StockIn;
            INSERT INTO @StockIn
            (
                StockInId,
                CurrentStock
            )
            SELECT si.StockInId,   -- StockInId - int
                   si.CurrentStock -- CurrentStock - decimal(18, 2)
            FROM dbo.StockIn si
            WHERE si.CurrentStock > 0
                  AND si.OrganizationId = @OrganizationId
                  AND si.ProductId = @ProductId
                  AND si.IsActive = 1;
            SELECT @ProdTotalStock = SUM(si.CurrentStock)
            FROM @StockIn si;

            IF @ProductQty > @ProdTotalStock
            BEGIN
                SET @ErrorMsg = N'Sales quantity exceeds stock!!';
                RAISERROR(   'Sales quantity exceeds stock!!', -- Message text.  
                             20,                               -- Severity.  
                             -1                                -- State.  
                         );
            END;

            WHILE @ProductQty > 0
            BEGIN

                SELECT TOP (1)
                       @MaxSaleQty = si.CurrentStock,
                       @StockInId = si.StockInId
                FROM @StockIn si
                --WHERE si.StockInId = MIN(si.StockInId);
                ORDER BY si.StockInId;



                IF @ProductQty > @MaxSaleQty
                BEGIN
                    SET @ProdSaleQty = @MaxSaleQty;
                END;
                ELSE
                BEGIN
                    SET @ProdSaleQty = @ProductQty;
                END;

                INSERT INTO @SlsDtl
                (
                    ProductId,
                    ProductVATTypeId,
                    StockInId,
                    Quantity,
                    UnitPrice,
                    DiscountPerItem,
                    VATPercent,
                    SupplementaryDutyPercent,
                    MeasurementUnitId
                )
                VALUES
                (   @ProductId,                -- ProductId - int
                    @ProductVATTypeId,         -- ProductVATTypeId - int
                    @StockInId,                -- StockInId - int
                    @ProdSaleQty,              -- Quantity - decimal(18, 2)
                    @UnitPrice,                -- UnitPrice - decimal(18, 2)
                    @DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
                    @VATPercent,               -- VATPercent - decimal(18, 2)
                    @SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
                    @MeasurementUnitId         -- MeasurementUnitId - int
                    );

                SET @ProductQty = @ProductQty - @ProdSaleQty;
                DELETE FROM @StockIn
                WHERE StockInId = @StockInId;

                UPDATE dbo.StockIn
                SET SaleQuantity = ISNULL(SaleQuantity, 0) + @ProdSaleQty
                WHERE StockInId = @StockInId;
            END;

        END;



        --End Insert sales using while loop

        --Insert Sales Detail
        INSERT INTO dbo.SalesDetails
        (
            SalesId,
            ProductId,
            ProductVATTypeId,
            StockInId,
            Quantity,
            UnitPrice,
            DiscountPerItem,
            VATPercent,
            SupplementaryDutyPercent,
            MeasurementUnitId,
            CreatedBy,
            CreatedTime
        )
        SELECT @SalesId,                        -- SalesId - int
               slsDtl.ProductId,                -- ProductId - int
               slsDtl.ProductVATTypeId,         -- ProductVATTypeId - int
               slsDtl.StockInId,                -- StockInId - int
               slsDtl.Quantity,                 -- Quantity - decimal(18, 2)
               slsDtl.UnitPrice,                -- UnitPrice - decimal(18, 2)
               slsDtl.DiscountPerItem,          -- DiscountPerItem - decimal(18, 2)
               slsDtl.VATPercent,               -- VATPercent - decimal(18, 2)
               slsDtl.SupplementaryDutyPercent, -- SupplementaryDutyPercent - decimal(18, 2)
               slsDtl.MeasurementUnitId,        -- MeasurementUnitId - int
               @CreatedBy,                      -- CreatedBy - int
               @CreatedTime                     -- CreatedTime - datetime
        FROM @SlsDtl slsDtl;
        --End Insert Sales Detail

        --Insert Payment

        INSERT INTO dbo.SalesPaymentReceive
        (
            SalesId,
            ReceivedPaymentMethodId,
            ReceiveAmount,
            ReceiveDate,
            CreatedBy,
            CreatedTime
        )
        SELECT @SalesId,                   -- SalesId - int
               pr.ReceivedPaymentMethodId, -- ReceivedPaymentMethodId - int
               pr.ReceiveAmount,           -- ReceiveAmount - decimal(18, 2)
               pr.ReceiveDate,             -- ReceiveDate - datetime
               @CreatedBy,                 -- CreatedBy - int
               @CreatedTime                -- CreatedTime - datetime
        FROM @PaymentReceive pr;
        --Insert Payment

        --Insert content
        IF @ContentJson IS NOT NULL
           AND LEN(@ContentJson) > 0
        BEGIN
            INSERT INTO dbo.Content
            (
                DocumentTypeId,
                OrganizationId,
                FileUrl,
                MimeType,
                Node,
                ObjectId,
                ObjectPrimaryKey,
                IsActive,
                CreatedBy,
                CreatedTime
            )
            SELECT jd.DocumentTypeId,
                   @OrganizationId,
                   jd.FileUrl,
                   jd.MimeType,
                   NULL,
                   24,
                   @SalesId,
                   1,
                   @CreatedBy,
                   @CreatedTime
            FROM
                OPENJSON(@ContentJson)
                WITH
                (
                    [DocumentTypeId] [INT],
                    [FileUrl] [NVARCHAR](500),
                    [MimeType] [NVARCHAR](50)
                ) jd;
        END;
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH;

END;
GO
/****** Object:  StoredProcedure [dbo].[SPManagePurchaseDue]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE Procedure [dbo].[SPManagePurchaseDue]   
 (  
  @PurchaseId INT,  
  @PaymentMethodId INT,  
  @PaidAmount Decimal,  
  @CreatedBy INT  
 )  
 AS   
 Begin   
    Update Purchase Set PaidAmount= @PaidAmount where PurchaseId= @PurchaseId  
    SET NOCOUNT ON  
   Insert Into PurchasePayment   
   (PurchaseId, PaymentMethodId,PaidAmount,PaymentDate,CreatedBy,CreatedTime)  
   values   
   (@PurchaseId,@PaymentMethodId,@PaidAmount,GETDATE(),@CreatedBy,GETDATE())  
   SET NOCOUNT OFF  
 END  
   
GO
/****** Object:  StoredProcedure [dbo].[SPManageSalesDue]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SPManageSalesDue]   
 ( 
  @SalesId INT,  
  @PaymentMethodId INT,  
  @PaidAmount Decimal,  
  @CreatedBy INT 
 )  
 AS   
 Begin   
    Update Sales Set PaymentReceiveAmount= @PaidAmount where SalesId= @SalesId  
    SET NOCOUNT ON  
   Insert Into SalesPaymentReceive   
   (SalesId, ReceivedPaymentMethodId,ReceiveAmount,ReceiveDate,CreatedBy,CreatedTime)  
   values   
   (@SalesId,@PaymentMethodId,@PaidAmount,GETDATE(),@CreatedBy,GETDATE())  
   SET NOCOUNT OFF  
 END  
   
GO
/****** Object:  StoredProcedure [dbo].[SpPriceDeclarationMushak]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
           DateOfSubmission = prcSt.EffectiveFrom,
           FirstSupplyDate = prcSt.EffectiveFrom,
           prod.FinishedProductId,
           prod.FinishedProductName,
           prod.FinishedProductCode,
           prod.FinishedProductHsCode,
           prod.FinishedProductModelNo,
           prod.FinishedProductMeasurementUnit,
           rm.RmspcPriceSetupId,
           rm.RawMaterialId,
           rm.RawmaterialName,
           rm.RawmaterialRequiredQtyWithWastage,
           rm.RawmaterialPurchasePrice,
           rm.RawmaterialWastageQty,
           rm.RawmaterialWastagePercentage,
           oh.OhcpspcPriceSetupId,
           oh.OverHeadCostId,
           oh.OverHeadCostName,
           oh.OverHeadCostAmount
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
            SELECT CostId = ROW_NUMBER() OVER (ORDER BY rmpspc.PriceSetupProductCostId),
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
            WHERE rmpspc.PriceSetupId = @PricedecID
        ) rm
            ON rm.CostId = prod.CostId
        FULL OUTER JOIN
        (
            SELECT CostId = ROW_NUMBER() OVER (ORDER BY ohcpspc.PriceSetupProductCostId),
                   OhcpspcPriceSetupId = ohcpspc.PriceSetupId,
                   ohcpspc.OverHeadCostId,
                   OverHeadCostName = ohc.Name,
                   OverHeadCostAmount = ohcpspc.Cost
            FROM dbo.PriceSetupProductCost ohcpspc
                INNER JOIN dbo.OverHeadCost ohc
                    ON ohc.OverHeadCostId = ohcpspc.OverHeadCostId
            WHERE ohcpspc.PriceSetupId = @PricedecID
        ) oh
            ON oh.CostId = prod.CostId
        INNER JOIN dbo.PriceSetup prcSt
            ON prcSt.PriceSetupId = COALESCE(rm.RmspcPriceSetupId, oh.OhcpspcPriceSetupId, prod.PriceSetupId)
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = prcSt.OrganizationId;
END;

GO
/****** Object:  StoredProcedure [dbo].[SpPurchaseCalcBook]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 14, 2019
-- Description:	Returns Purchase Calculation Book for Mushak 6.1
-- SpPurchaseCalcBook 5, NULL, NULL, 0, 0
-- =============================================
CREATE PROCEDURE [dbo].[SpPurchaseCalcBook]
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
    @VendorId INT,
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @InitialStock DECIMAL(18, 2) = 0;

    IF @FromDate IS NOT NULL
    BEGIN
        SELECT @InitialStock = ISNULL(SUM(si.CurrentStock), 0)
        FROM dbo.StockIn si
        WHERE si.CreatedTime <= @FromDate
              AND si.ProductId = @ProductId;
    END;
    SELECT pcbb.SlNo,
           pcbb.OperationTime,
           pcbb.OperationType,
           pcbb.StockInId,
           pcbb.OrganizationId,
           org.Name AS OrganizationName,
           org.Address AS OrganizationAddress,
           org.BIN AS OrganizationBin,
           pcbb.PurchaseDetailId,
           pcbb.PurchaseId,
           purch.InvoiceNo,
           purch.VendorInvoiceNo,
           purch.PurchaseDate,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) AS InitialQty,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) * pcbb.EndUnitPriceWithoutVat AS InitPriceWithoutVat,
           ISNULL(purch.VatChallanNo, purch.BillOfEntry) AS VatChallanOrBillOfEntry,
           ISNULL(purch.VatChallanIssueDate, purch.BillOfEntryDate) AS VatChallanOrBillOfEntryDate,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           ISNULL(vndr.BinNo, vndr.NationalIdNo) AS VendorBinOrNid,
           prod.ProductId,
           prod.Name AS ProductName,
           purchDtl.Quantity AS PurchaseQty,
           purchDtl.UnitPrice * purchDtl.Quantity AS PriceWithoutVat,
		   [dbo].[FnGetCalculatedInputSuppDuty](purchDtl.UnitPrice * purchDtl.Quantity, ISNULL(purchDtl.SupplementaryDutyPercent, 0))  AS SupplimentaryDuty,
		   [dbo].[FnGetCalculatedInputVat](purchDtl.UnitPrice * purchDtl.Quantity, ISNULL(purchDtl.VATPercent, 0), ISNULL(purchDtl.SupplementaryDutyPercent, 0)) AS VATAmount,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) + ISNULL(purchDtl.Quantity, 0) AS TotalProdQty,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) * pcbb.EndUnitPriceWithoutVat + ISNULL(purchDtl.UnitPrice * purchDtl.Quantity, 0) AS TotalProdPrice,
           pcbb.UsedInProductionQty,
           pcbb.UsedInProductionQty * pcbb.EndUnitPriceWithoutVat AS PriceWithoutVatForUsedInProduction,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) + ISNULL(purchDtl.Quantity, 0) - ISNULL(pcbb.UsedInProductionQty, 0) AS ClosingProdQty,
           ((@InitialStock
             + ISNULL(
                         SUM(ISNULL(pcbb.PurchaseQty, 0) - ISNULL(pcbb.UsedInProductionQty, 0)) OVER (ORDER BY pcbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                         0
                     )
            ) - ISNULL(pcbb.UsedInProductionQty, 0)
           ) * pcbb.EndUnitPriceWithoutVat
           + ISNULL(purchDtl.Quantity * purchDtl.UnitPrice, 0) AS ClosingTotalPrice,
           pcbb.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.FnGetPurchaseCalcBookBase(@OrganizationId, @FromDate, @ToDate, @VendorId, @ProductId) pcbb
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = pcbb.OrganizationId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = pcbb.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = pcbb.MeasurementUnitId
        LEFT JOIN dbo.Purchase purch
            ON purch.PurchaseId = pcbb.PurchaseId
        LEFT JOIN dbo.PurchaseDetails purchDtl
            ON purchDtl.PurchaseDetailId = pcbb.PurchaseDetailId
        LEFT JOIN dbo.Vendor vndr
            ON vndr.VendorId = purch.VendorId
        LEFT JOIN dbo.Vendor cust
            ON cust.VendorId = purch.VendorId
    ORDER BY pcbb.SlNo;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpPurchaseReport]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SpPurchaseReport]
    @PurchaseReasonId INT,
    @VendorId INT,
    @OrganizationId INT,
    @InvoiceNo NVARCHAR(50),
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SELECT purc.PurchaseId,
           purc.VendorId,
           vndr.Name AS VendorName,
           vndr.BinNo AS VendorBIN,
           vndr.Address AS VendorAddress,
           purc.VendorInvoiceNo,
           purc.InvoiceNo,
           purc.OrganizationId,
           purc.PurchaseDate AS PurchaseDate,
           purc.PurchaseTypeId,
           pt.Name AS PurchaseTypeName,
           purc.PurchaseReasonId,
           pr.Reason AS PurchaseReason,
           prod.Name AS ProductName,
           purcDtl.Quantity AS PurchaseQty,
           mu.Name AS MeasurementUnitName,
           purcDtl.UnitPrice AS UnitPrice,
           purcDtl.Quantity * purcDtl.UnitPrice AS ProductPrice,
           purcDtl.VATPercent AS VatPercent,
           purcDtl.Quantity * purcDtl.UnitPrice * purcDtl.VATPercent / 100 AS ProductVAt,
           (purcDtl.Quantity * purcDtl.UnitPrice) + (purcDtl.Quantity * purcDtl.UnitPrice * purcDtl.VATPercent / 100) AS ProductPriceWithVat
    FROM dbo.Purchase purc
        LEFT JOIN dbo.Vendor vndr
            ON vndr.VendorId = purc.VendorId
        INNER JOIN dbo.PurchaseDetails purcDtl
            ON purcDtl.PurchaseId = purc.PurchaseId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = purcDtl.ProductId
        INNER JOIN dbo.PurchaseTypes pt
            ON pt.PurchaseTypeId = purc.PurchaseTypeId
        INNER JOIN dbo.PurchaseReason pr
            ON pr.PurchaseReasonId = purc.PurchaseReasonId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = purcDtl.MeasurementUnitId
    WHERE (
              @FromDate IS NULL
              OR purc.PurchaseDate >= @FromDate
          )
          AND
          (
              @ToDate IS NULL
              OR purc.PurchaseDate < DATEADD(DAY, 1, @ToDate)
          )
          AND
          (
              @PurchaseReasonId IS NULL
              OR @PurchaseReasonId = 0
              OR pr.PurchaseReasonId = @PurchaseReasonId
          )
          AND
          (
              @VendorId IS NULL
              OR @VendorId = 0
              OR vndr.VendorId = @VendorId
          )
          AND
          (
              @OrganizationId IS NULL
              OR @OrganizationId = 0
              OR purc.OrganizationId = @OrganizationId
          )
          AND
          (
              @InvoiceNo IS NULL
              OR purc.InvoiceNo = @InvoiceNo
          )
          --AND purc.IsCanceled = 0;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpPurchaseSaleCalcBook]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 14, 2019
-- Description:	Returns Purchase Calculation Book for Mushak 6.2.1
-- SpPurchaseSaleCalcBook 16, NULL, NULL, 0, 0
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
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    SELECT ROW_NUMBER() OVER (ORDER BY purchSaleComb.StockInId, purchSaleComb.SlNoByStockIn) AS SlNo,
           purchSaleComb.OrganizationId,
           purchSaleComb.OrganizationName,
           purchSaleComb.OrganizationAddress,
           purchSaleComb.OrganizationBin,
           purchSaleComb.OperationDate,
           purchSaleComb.InitialQty,
           purchSaleComb.InitialPriceWithoutVatInStockIn,
           purchSaleComb.ProductId,
           purchSaleComb.ProductName,
           purchSaleComb.ProductCode,
           purchSaleComb.ProductHsCode,
           purchSaleComb.ProductModel,
           purchSaleComb.PurchaseDetailId,
           purchSaleComb.PurchaseQty,
           purchSaleComb.PurchasePriceWithoutVat,
           purchSaleComb.QtyAfterPurchase,
           purchSaleComb.ProductPriceWithoutVatAfterPurchase,
           purchSaleComb.VendorId,
           purchSaleComb.VendorName,
           purchSaleComb.VendorBinOrNidNo,
           purchSaleComb.VendorAddress,
           purchSaleComb.PurcVatChallanOrBillOfEntryNo,
           purchSaleComb.PurcVatChallanOrBillOfEntryDate,
           purchSaleComb.SalesDetailId,
           purchSaleComb.SoldQty,
           purchSaleComb.SalesPriceWithoutVat,
           purchSaleComb.SalesSupplimentaryDuty,
           purchSaleComb.SalesVat,
           purchSaleComb.CustomerName,
           purchSaleComb.CustomerAddress,
           purchSaleComb.CustomerBinOrNidNo,
           purchSaleComb.SalesVatChallanNo,
           purchSaleComb.SalesVatChallanDate,
           purchSaleComb.EndQty,
           purchSaleComb.EndProductPriceWithoutVat,
           purchSaleComb.TransMeasurementUnitId,
           purchSaleComb.TransMeasurementUnitName
    FROM
    (
        --Purchase Part
        (SELECT ROW_NUMBER() OVER (PARTITION BY prod.ProductId ORDER BY si.StockInId) AS SlNoByStockIn,
                si.StockInId,
                si.OrganizationId,
                org.Name AS OrganizationName,
                org.Address AS OrganizationAddress,
                org.BIN AS OrganizationBin,
                purch.PurchaseDate AS OperationDate,
                si.InitialQuantity AS InitialQty,
                si.InitialQuantity * ISNULL(si.InitUnitPriceWithoutVat, 0) AS InitialPriceWithoutVatInStockIn,
                si.ProductId AS ProductId,
                prod.Name AS ProductName,
                prod.Code AS ProductCode,
                prod.HSCode AS ProductHsCode,
                prod.ModelNo AS ProductModel,
                --Purchase Information
                si.PurchaseDetailId AS PurchaseDetailId,
                si.InQuantity AS PurchaseQty,
                si.InQuantity * ISNULL(si.InitUnitPriceWithoutVat, 0) AS PurchasePriceWithoutVat,
                si.InitialQuantity + si.InQuantity AS QtyAfterPurchase,
                ISNULL((si.InitialQuantity * ISNULL(si.InitUnitPriceWithoutVat, 0)), 0)
                + (si.InQuantity * ISNULL(purchDtl.UnitPrice, 0)) AS ProductPriceWithoutVatAfterPurchase,
                vndr.VendorId AS VendorId,
                vndr.Name AS VendorName,
                CAST(ISNULL(vndr.BinNo, vndr.NationalIdNo) AS NVARCHAR(50)) AS VendorBinOrNidNo,
                vndr.Address AS VendorAddress,
                ISNULL(purch.VatChallanNo, purch.BillOfEntry) AS PurcVatChallanOrBillOfEntryNo,
                ISNULL(purch.VatChallanIssueDate, purch.BillOfEntryDate) AS PurcVatChallanOrBillOfEntryDate,
                --Sales information
                CAST(0 AS INT) AS SalesDetailId,
                CAST(0 AS DECIMAL(18, 2)) SoldQty,
                CAST(0 AS DECIMAL(18, 2)) AS SalesPriceWithoutVat,
                CAST(0 AS DECIMAL(18, 2)) AS SalesSupplimentaryDuty,
                CAST(0 AS DECIMAL(18, 2)) AS SalesVat,
                CAST(N'' AS NVARCHAR(200)) AS CustomerName,
                CAST(N'' AS NVARCHAR(200)) AS CustomerAddress,
                CAST(N'' AS NVARCHAR(50)) AS CustomerBinOrNidNo,
                CAST(N'' AS NVARCHAR(50)) AS SalesVatChallanNo,
                CAST(NULL AS DATETIME) AS SalesVatChallanDate,
                si.InitialQuantity + si.InQuantity AS EndQty,
                ISNULL((si.InitialQuantity * si.InitUnitPriceWithoutVat), 0)
                + (si.InQuantity * purchDtl.UnitPrice) AS EndProductPriceWithoutVat,
                si.MeasurementUnitId AS TransMeasurementUnitId,
                mu.Name AS TransMeasurementUnitName
         FROM dbo.StockIn si
             INNER JOIN dbo.Organizations org
                 ON org.OrganizationId = si.OrganizationId
             INNER JOIN dbo.Products prod
                 ON prod.ProductId = si.ProductId
             INNER JOIN dbo.PurchaseDetails purchDtl
                 ON purchDtl.PurchaseDetailId = si.PurchaseDetailId
             INNER JOIN dbo.Purchase purch
                 ON purch.PurchaseId = purchDtl.PurchaseId
             INNER JOIN dbo.Vendor vndr
                 ON vndr.VendorId = purch.VendorId
             INNER JOIN dbo.MeasurementUnits mu
                 ON mu.MeasurementUnitId = si.MeasurementUnitId
         WHERE si.PurchaseDetailId IS NOT NULL
               AND si.OrganizationId = @OrganizationId
               AND
               (
                   @FromDate IS NULL
                   OR purch.PurchaseDate >= @FromDate
               )
               AND
               (
                   @ToDate IS NULL
                   OR purch.PurchaseDate <= @ToDate
               )
               AND
               (
                   @VendorId = 0
                   OR @VendorId IS NULL
                   OR vndr.VendorId = @VendorId
               )
               AND
               (
                   @ProductId = 0
                   OR @ProductId IS NULL
                   OR prod.ProductId = @ProductId
               ))
        UNION
        --Sales Part
        (SELECT ROW_NUMBER() OVER (PARTITION BY prod.ProductId
                                   ORDER BY si.StockInId,
                                            slsDtl.SalesDetailId
                                  ) + 1 AS SlNoByStockIn,
                si.StockInId,
                si.OrganizationId,
                org.Name AS OrganizationName,
                org.Address AS OrganizationAddress,
                org.BIN AS OrganizationBin,
                sls.SalesDate AS OperationDate,
                ComputedColumn.TotalQtyAfterPurchase AS InitialQty,
                ComputedColumn.ProductPriceWithoutVatAfterPurchase AS InitialPriceWithoutVatInStockIn,
                si.ProductId AS ProductId,
                prod.Name AS ProductName,
                prod.Code AS ProductCode,
                prod.HSCode AS ProductHsCode,
                prod.ModelNo AS ProductModel,
                --Purchase Information
                CAST(0 AS INT) AS PurchaseDetailId,
                CAST(0 AS DECIMAL(18, 2)) AS PurchaseQty,
                CAST(0 AS DECIMAL(18, 2)) AS PurchasePriceWithoutVat,
                ComputedColumn.TotalQtyAfterPurchase AS QtyAfterPurchase,
                ComputedColumn.ProductPriceWithoutVatAfterPurchase AS ProductPriceWithoutVatAfterPurchase,
                CAST(0 AS INT) AS VendorId,
                CAST(N'' AS NVARCHAR(200)) AS VendorName,
                CAST(N'' AS NVARCHAR(50)) AS VendorBinOrNidNo,
                CAST(N'' AS NVARCHAR(500)) AS VendorAddress,
                CAST(N'' AS NVARCHAR(50)) AS PurcVatChallanOrBillOfEntryNo,
                CAST(NULL AS DATETIME) AS PurcVatChallanOrBillOfEntryDate,
                --Sales information
                slsDtl.SalesDetailId AS SalesDetailId,
                slsDtl.Quantity AS SoldQty,
                slsDtl.Quantity * (slsDtl.UnitPrice - slsDtl.DiscountPerItem) AS SalesPriceWithoutVat,
                slsDtl.Quantity * slsDtl.UnitPrice * slsDtl.SupplementaryDutyPercent / 100 AS SalesSupplimentaryDuty,
                ((slsDtl.Quantity * slsDtl.UnitPrice)
                 + (slsDtl.Quantity * slsDtl.UnitPrice * slsDtl.SupplementaryDutyPercent / 100)
                ) * slsDtl.VATPercent
                / 100 AS SalesVat,
                cust.Name AS CustomerName,
                cust.Address AS CustomerAddress,
                CAST(ISNULL(cust.BIN, cust.NIDNo) AS NVARCHAR(50)) AS CustomerBinOrNidNo,
                sls.VatChallanNo AS SalesVatChallanNo,
                sls.TaxInvoicePrintedTime AS SalesVatChallanDate,
                ComputedColumn.TotalQtyAfterPurchase - ComputedColumn.TotalSalesQty AS EndQty,
                ((ComputedColumn.TotalQtyAfterPurchase - ComputedColumn.TotalSalesQty)
                 / (ComputedColumn.TotalQtyAfterPurchase / ComputedColumn.ProductPriceWithoutVatAfterPurchase)
                ) AS EndProductPriceWithoutVat,
                si.MeasurementUnitId AS TransMeasurementUnitId,
                mu.Name AS TransMeasurementUnitName
         FROM dbo.StockIn si
             INNER JOIN dbo.Organizations org
                 ON org.OrganizationId = si.OrganizationId
             INNER JOIN dbo.Products prod
                 ON prod.ProductId = si.ProductId
             INNER JOIN dbo.PurchaseDetails purchDtl
                 ON purchDtl.PurchaseDetailId = si.PurchaseDetailId
             INNER JOIN dbo.Purchase purch
                 ON purch.PurchaseId = purchDtl.PurchaseId
             INNER JOIN dbo.Vendor vndr
                 ON vndr.VendorId = purch.VendorId
             INNER JOIN dbo.SalesDetails slsDtl
                 ON slsDtl.StockInId = si.StockInId
             INNER JOIN dbo.Sales sls
                 ON sls.SalesId = slsDtl.SalesId
             LEFT JOIN dbo.Customer cust
                 ON cust.CustomerId = sls.CustomerId
             INNER JOIN dbo.MeasurementUnits mu
                 ON mu.MeasurementUnitId = si.MeasurementUnitId
             CROSS APPLY
         (
             SELECT (si.InitialQuantity + si.InQuantity) AS TotalQtyAfterPurchase,
                    ISNULL((si.InitialQuantity * si.InitUnitPriceWithoutVat), 0)
                    + (si.InQuantity * purchDtl.UnitPrice) AS ProductPriceWithoutVatAfterPurchase,
                    (
                        SELECT SUM(sd.Quantity)
                        FROM dbo.SalesDetails sd
                        WHERE sd.StockInId = si.StockInId
                              AND sd.SalesDetailId <= slsDtl.SalesDetailId
                    ) AS TotalSalesQty
         ) AS ComputedColumn
         WHERE si.PurchaseDetailId IS NOT NULL
               AND sls.SalesTypeId <> 3
               AND si.OrganizationId = @OrganizationId
               AND
               (
                   @FromDate IS NULL
                   OR sls.SalesDate >= @FromDate
               )
               AND
               (
                   @ToDate IS NULL
                   OR sls.SalesDate <= @ToDate
               )
               AND
               (
                   @VendorId = 0
                   OR @VendorId IS NULL
                   OR vndr.VendorId = @VendorId
               )
               AND
               (
                   @CustomerId = 0
                   OR @CustomerId IS NULL
                   OR cust.CustomerId = @CustomerId
               )
               AND
               (
                   @ProductId = 0
                   OR @ProductId IS NULL
                   OR prod.ProductId = @ProductId
               ))
    ) AS purchSaleComb;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpPurcSalesChallanForHighValSale]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- SpPurcSalesChallanForHighValSale 2011, 50
-- =============================================
CREATE PROCEDURE [dbo].[SpPurcSalesChallanForHighValSale]
    @SalesId INT,
    @LowerLimitOfHighValSale DECIMAL(18, 2) 
AS
BEGIN
    SET @LowerLimitOfHighValSale = ISNULL(@LowerLimitOfHighValSale, 200000);
    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           sls.SalesId,
           sls.InvoiceNo,
           sls.VatChallanNo,
           sls.TaxInvoicePrintedTime,
           sls.SalesDate,
           sls.OrganizationId,
		   org.Name AS TaxRegisteredName,
		   org.BIN AS TaxRegisteredBIN,
		   org.Address AS TaxInvoiceIssueAddress,
		   org.VatResponsiblePersonName,
		   org.VatResponsiblePersonDesignation,
           sls.CustomerId,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           cust.BIN AS CustomerBIN,
           -------------------------------
           (slsDtl.UnitPrice * slsDtl.Quantity)
           + (slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100)
           + (slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.VATPercent / 100) AS ProdPriceInclVATAndDuty
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.SalesId = @SalesId
          AND sls.TotalPriceWithoutVat >= @LowerLimitOfHighValSale;

    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           purc.PurchaseId,
           purc.VendorInvoiceNo,
           purc.PurchaseDate,
           purc.VatChallanNo,
           purc.VatChallanIssueDate,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           ISNULL(vndr.BinNo, vndr.NationalIdNo) AS VendorBinOrNid,
           -------------------------------
           (purcDtl.UnitPrice * purcDtl.Quantity)
           + (purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.SupplementaryDutyPercent / 100)
           + (purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.VATPercent / 100) AS ProdPriceInclVATAndDuty
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.StockIn stkIn
            ON stkIn.StockInId = slsDtl.StockInId
        INNER JOIN dbo.PurchaseDetails purcDtl
            ON purcDtl.PurchaseDetailId = stkIn.PurchaseDetailId
        INNER JOIN dbo.Purchase purc
            ON purc.PurchaseId = purcDtl.PurchaseId
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purc.VendorId
    WHERE sls.SalesId = @SalesId
          AND sls.TotalPriceWithoutVat >= @LowerLimitOfHighValSale;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpPurcSalesChallanForHighValSalePurchase]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- SpPurcSalesChallanForHighValSale 2011, 50
-- =============================================
CREATE PROCEDURE [dbo].[SpPurcSalesChallanForHighValSalePurchase]
    @SalesId INT,
    @LowerLimitOfHighValSale DECIMAL(18, 2)
AS
BEGIN
    SET @LowerLimitOfHighValSale = ISNULL(@LowerLimitOfHighValSale, 200000);

    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           purc.PurchaseId,
           purc.VendorInvoiceNo,
           purc.PurchaseDate,
           purc.VatChallanNo,
           purc.VatChallanIssueDate,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           ISNULL(vndr.BinNo, vndr.NationalIdNo) AS VendorBinOrNid,
           -------------------------------
           (purcDtl.UnitPrice * purcDtl.Quantity)
           + (purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.SupplementaryDutyPercent / 100)
           + (purcDtl.UnitPrice * purcDtl.Quantity * purcDtl.VATPercent / 100) AS ProdPriceInclVATAndDuty
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.StockIn stkIn
            ON stkIn.StockInId = slsDtl.StockInId
        INNER JOIN dbo.PurchaseDetails purcDtl
            ON purcDtl.PurchaseDetailId = stkIn.PurchaseDetailId
        INNER JOIN dbo.Purchase purc
            ON purc.PurchaseId = purcDtl.PurchaseId
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purc.VendorId
    WHERE sls.SalesId = @SalesId
          AND sls.TotalPriceWithoutVat >= @LowerLimitOfHighValSale;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpPurcSalesChallanForHighValSaleSale]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- SpPurcSalesChallanForHighValSale 2011, 50
-- =============================================
CREATE PROCEDURE [dbo].[SpPurcSalesChallanForHighValSaleSale]
    @SalesId INT,
    @LowerLimitOfHighValSale DECIMAL(18, 2) 
AS
BEGIN
    SET @LowerLimitOfHighValSale = ISNULL(@LowerLimitOfHighValSale, 200000);
    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           sls.SalesId,
           sls.InvoiceNo,
           sls.VatChallanNo,
           sls.TaxInvoicePrintedTime,
           sls.SalesDate,
           sls.OrganizationId,
		   org.Name AS TaxRegisteredName,
		   org.BIN AS TaxRegisteredBIN,
		   org.Address AS TaxInvoiceIssueAddress,
		   org.VatResponsiblePersonName,
		   org.VatResponsiblePersonDesignation,
           sls.CustomerId,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           cust.BIN AS CustomerBIN,
           -------------------------------
           (slsDtl.UnitPrice * slsDtl.Quantity)
           + (slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.SupplementaryDutyPercent / 100)
           + (slsDtl.UnitPrice * slsDtl.Quantity * slsDtl.VATPercent / 100) AS ProdPriceInclVATAndDuty
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.SalesId = @SalesId
          AND sls.TotalPriceWithoutVat >= @LowerLimitOfHighValSale;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpSalesCalcBook]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

    DECLARE @InitialStock DECIMAL(18, 2) = 0;

    IF @FromDate IS NOT NULL
    BEGIN
        SELECT @InitialStock = ISNULL(SUM(si.CurrentStock), 0)
        FROM dbo.StockIn si
        WHERE si.CreatedTime <= @FromDate
              AND si.ProductId = @ProductId;
    END;
    SELECT scbb.SlNo,
           scbb.OperationTime,
           scbb.OperationType,
           scbb.StockInId,
           scbb.OrganizationId,
           org.Name AS OrganizationName,
           org.Address AS OrganizationAddress,
           org.BIN AS OrganizationBin,
           scbb.SalesId,
           scbb.SalesDetailId,
           sls.SalesDate,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) AS InitialQty,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) * scbb.UnitPrice AS InitPriceWithoutVat,
           scbb.ProductionQty,
           scbb.ProductionQty * scbb.UnitPrice AS PriceOfProdFromProduction,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) + ISNULL(scbb.ProductionQty, 0) AS TotalProductionQty,
           ((@InitialStock
             + ISNULL(
                         SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                         0
                     )
            ) + ISNULL(scbb.ProductionQty, 0)
           ) * scbb.UnitPrice AS TotalProductionPrice,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           ISNULL(cust.BIN, cust.NIDNo) AS CustomerBinOrNid,
           sls.VatChallanNo,
           sls.TaxInvoicePrintedTime,
           scbb.ProductId,
           prod.Name AS ProductName,
           scbb.SalesQty,
           scbb.UnitPrice,
           slsDtl.UnitPrice AS SalesUnitPrice,
           ComputedColum.TaxablePrice AS TaxablePrice,
           dbo.FnGetCalculatedOutputSuppDuty(ComputedColum.TaxablePrice, ISNULL(slsDtl.SupplementaryDutyPercent, 0)) AS SupplementaryDuty,
           dbo.FnGetCalculatedOutputVat(
                                           ComputedColum.TaxablePrice,
                                           ISNULL(slsDtl.VATPercent, 0),
                                           ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                       ) AS ProdVatAmount,
           (@InitialStock
            + ISNULL(
                        SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                        0
                    )
           ) + ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0) AS ClosingProdQty,
           (((@InitialStock
              + ISNULL(
                          SUM(ISNULL(scbb.ProductionQty, 0) - ISNULL(scbb.SalesQty, 0)) OVER (ORDER BY scbb.SlNo ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING),
                          0
                      )
             ) + ISNULL(scbb.ProductionQty, 0)
            ) * scbb.UnitPrice
           ) - ComputedColum.TaxablePrice AS ClosingProdPrice,
           scbb.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.FnGetSalesCalcBookBase(@OrganizationId, @FromDate, @ToDate, @CustomerId, @ProductId) scbb
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = scbb.OrganizationId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = scbb.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = scbb.MeasurementUnitId
        LEFT JOIN dbo.Sales sls
            ON sls.SalesId = scbb.SalesId
        LEFT JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesDetailId = scbb.SalesDetailId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
        CROSS APPLY
    (
        SELECT ISNULL(scbb.SalesQty * slsDtl.UnitPrice, 0) AS TaxablePrice
    ) AS ComputedColum
    ORDER BY scbb.SlNo;
END;
GO
/****** Object:  StoredProcedure [dbo].[SpSalesCalcBook_new]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sabbir Ahmed Osmani
-- Create date: July 14, 2019
-- Description:	Returns Purchase Calculation Book for Mushak 6.1
-- SpSalesCalcBook 5, NULL, NULL, 0, 0
-- =============================================
CREATE PROCEDURE [dbo].[SpSalesCalcBook_new]
    @OrganizationId INT,
    @FromDate DATETIME,
    @ToDate DATETIME,
	@CustomerId INT,
    @ProductId INT
AS
BEGIN
    SELECT SlNo = ROW_NUMBER() OVER (PARTITION BY si.StockInId ORDER BY slsDtl.SalesDetailId) + 1,
           si.StockInId,
           si.OrganizationId,
           org.Name AS OrganizationName,
           org.Address AS OrganizationAddress,
           org.BIN AS OrganizationBin,
           sls.SalesId,
           slsDtl.SalesDetailId,
           sls.SalesDate,
           ISNULL(si.InitialQuantity, 0) + si.InQuantity AS InitialQty,
           (ISNULL(si.InitialQuantity, 0) + si.InQuantity) * ISNULL(prcstup.SalesUnitPrice, 0) AS InitPriceWithoutVat,
           0 AS ProductionQty,
           0 AS PriceOfProdFromProduction,
           ISNULL(si.InitialQuantity, 0) + si.InQuantity AS TotalProductionQty,
           (ISNULL(si.InitialQuantity, 0) + si.InQuantity) * ISNULL(prcstup.SalesUnitPrice, 0) AS TotalProductionPrice,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           ISNULL(cust.BIN, cust.NIDNo) AS CustomerAddressOrNid,
           sls.VatChallanNo,
           sls.TaxInvoicePrintedTime,
           prod.ProductId,
           prod.Name AS ProductName,
           --slsDtl.UnitPrice,
           slsDtl.Quantity AS SalesQty,
           slsDtl.Quantity * slsDtl.UnitPrice AS TaxablePrice,
           slsDtl.Quantity * slsDtl.UnitPrice * slsDtl.SupplementaryDutyPercent / 100 AS SupplementaryDuty,
           slsDtl.Quantity * slsDtl.UnitPrice * slsDtl.VATPercent / 100 AS ProdVatAmount,
           ISNULL(si.InitialQuantity, 0) + si.InQuantity - slsDtl.Quantity AS ClosingProdQty,
           (ISNULL(si.InitialQuantity, 0) + si.InQuantity - slsDtl.Quantity) * prcstup.SalesUnitPrice AS ClosingProdPrice,
           mu.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.StockIn si
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = si.OrganizationId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = si.ProductId
        INNER JOIN dbo.PriceSetup prcstup
            ON prcstup.ProductId = prod.ProductId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.StockInId = si.StockInId
        INNER JOIN dbo.Sales sls
            ON sls.SalesId = slsDtl.SalesId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = si.MeasurementUnitId
    WHERE si.IsActive = 1
          AND si.IsFinishedGoods = 1
          AND si.OrganizationId = @OrganizationId
          --AND prcstup.IsActive = 1
          AND sls.SalesTypeId <> 3
          AND
          (
              @FromDate IS NULL
              OR sls.SalesDate >= @FromDate
          )
          AND
          (
              @ToDate IS NULL
              OR sls.SalesDate <= @ToDate
          )
          AND
          (
              @CustomerId = 0
              OR @CustomerId IS NULL
              OR cust.CustomerId = @CustomerId
          )
          AND
          (
              @ProductId = 0
              OR @ProductId IS NULL
              OR prod.ProductId = @ProductId
          );
END;


--EXEC SpSalesCalcBook 6, '2019-11-16','2019-11-17',NULL


--SELECT * FROM dbo.Sales s
--INNER JOIN dbo.SalesDetails sd ON sd.SalesId = s.SalesId
--WHERE 1=1
--AND s.OrganizationId = 6
--AND s.SalesTypeId <> 3
GO
/****** Object:  StoredProcedure [dbo].[SpSalesTaxInvoice]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpSalesTaxInvoice] @SalesId INT
AS
BEGIN
    DECLARE @IsTaxInvoicePrined BIT,
            @TaxInvoicePrintedTime DATETIME;
    SELECT @IsTaxInvoicePrined = sls.IsTaxInvoicePrined,
           @TaxInvoicePrintedTime = sls.TaxInvoicePrintedTime
    FROM dbo.Sales sls
    WHERE sls.SalesId = @SalesId;

    IF @IsTaxInvoicePrined = 0
    BEGIN
        SET @TaxInvoicePrintedTime = GETDATE();
        UPDATE dbo.Sales
        SET IsTaxInvoicePrined = 1,
            TaxInvoicePrintedTime = @TaxInvoicePrintedTime
        WHERE SalesId = @SalesId;
    END;
    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           sls.SalesId,
           sls.InvoiceNo,
           sls.VatChallanNo,
           @TaxInvoicePrintedTime AS TaxInvoiceIssueTime,
           sls.SalesDate,
           sls.OrganizationId,
           org.Name AS TaxRegisteredName,
           org.BIN AS TaxRegisteredBIN,
           org.Address AS TaxInvoiceIssueAddress,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonDesignation,
           sls.CustomerId,
           cust.Name AS CustomerName,
           cust.BIN AS CustomerBIN,
           sls.ReceiverName,
           sls.ReceiverContactNo,
           sls.ShippingAddress,
           sls.ShippingCountryId,
           sls.IsTaxInvoicePrined,
           sls.TaxInvoicePrintedTime,
           CASE
               WHEN @IsTaxInvoicePrined = 1 THEN
                   ''
               ELSE
                   '(Copy)'
           END AS InvoiceStatus,
           -------------------------------
           prod.Name AS ProductName,
           prod.ModelNo,
           mu.Name AS MeasurementUnitName,
           slsDtl.Quantity,
           slsDtl.UnitPrice,
           slsDtl.UnitPrice * slsDtl.Quantity AS ProductPrice,
           slsDtl.SupplementaryDutyPercent,
           dbo.FnGetCalculatedOutputSuppDuty(
                                                slsDtl.UnitPrice * slsDtl.Quantity,
                                                ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                            ) AS ProdSupplementaryDutyAmount,
           slsDtl.VATPercent,
           dbo.FnGetCalculatedOutputVat(
                                           slsDtl.UnitPrice * slsDtl.Quantity,
                                           slsDtl.VATPercent,
                                           ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                       ) AS ProdVATAmount,
           (slsDtl.UnitPrice * slsDtl.Quantity)
           + dbo.FnGetCalculatedOutputSuppDuty(
                                                  slsDtl.UnitPrice * slsDtl.Quantity,
                                                  ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                              )
           + dbo.FnGetCalculatedOutputVat(
                                             slsDtl.UnitPrice * slsDtl.Quantity,
                                             slsDtl.VATPercent,
                                             ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                         ) AS ProdPriceInclVATAndDuty
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = slsDtl.MeasurementUnitId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.SalesId = @SalesId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sptransferinvoice]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sptransferinvoice]
(
    @organizationId INT,
    @SaleId INT
)
AS
BEGIN
    DECLARE @parentOrgId INT;
    SELECT @parentOrgId = ISNULL(ParentOrganizationId,@organizationId)
    FROM dbo.Organizations
    WHERE OrganizationId = @organizationId;

    SELECT *
    FROM dbo.Organizations org
        INNER JOIN dbo.Sales sal
            ON sal.OrganizationId = org.OrganizationId
        INNER JOIN dbo.SalesDetails sald
            ON sald.SalesId = sal.SalesId
	WHERE 1=1
	AND sal.SalesId = @SaleId
	AND sal.SalesTypeId = 3
END;
GO
/****** Object:  StoredProcedure [dbo].[spvdspurchasecertificate]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spvdspurchasecertificate]
(
	@PurchaseId INT
)
AS
BEGIN
    SELECT vnd.[Name],
           vnd.BinNo,
           prc.VatChallanNo,
           prc.PurchaseDate,
           prc.TotalPriceWithoutVat,
           prc.TotalVAT,
		   org.Name As ORGNAME,
		   org.Address AS OrgAddress,
		   org.BIN AS OrgBin,
		   
		   org.VatResponsiblePersonName,
		   org.VatResponsiblePersonDesignation
    FROM [dbo].[Purchase] prc
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = prc.OrganizationId
        INNER JOIN dbo.Vendor vnd
            ON vnd.VendorId = prc.VendorId
    WHERE 1 = 1
          AND prc.IsVatDeductedInSource = 1
          --AND org.OrganizationId = 5
          AND prc.PurchaseId = @PurchaseId;
		--AND org.IsDeductVatInSource = 1
END;

GO
/****** Object:  StoredProcedure [dbo].[USP_AuditRestore]    Script Date: 12/1/2019 2:26:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[USP_AuditRestore]  
  (  
  @TableName VARCHAR(100),  
  @PrimaryKeyName VARCHAR(50),  
  @PrimaryKey VARCHAR(50) ,
  @AuditLogId VARCHAR(50) 
  )  
  As   
  BEGIN  
     Declare @value NVARCHAR(500)  
       SET @value = 'Update '+@TableName+'  SET  ISActive =1 where '+@PrimaryKeyName+'='+ @PrimaryKey  
       --Print(@value)   
      Update AuditLog SET IsActive=0 where AuditLogId=@AuditLogId
	   EXec(@value)  
    END  
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Advance Tax Payment Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purchase', @level2type=N'COLUMN',@level2name=N'ATPDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Advance Tax Payment Bank Branch ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purchase', @level2type=N'COLUMN',@level2name=N'ATPBankBranchId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Advance Tax Payment Economic Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purchase', @level2type=N'COLUMN',@level2name=N'ATPNbrEconomicCodeId'
GO
USE [master]
GO
ALTER DATABASE [vms_empty] SET  READ_WRITE 
GO
