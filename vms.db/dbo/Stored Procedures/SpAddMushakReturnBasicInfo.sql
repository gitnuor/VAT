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
            @completedMushakGenerationStageId TINYINT = 6,
            @nextMonth INT,
            @nextMonthYear INT;

    IF @Month >= 12
    BEGIN
        SET @nextMonth = 1;
        SET @nextMonthYear = @Year + 1;
    END;
    ELSE
    BEGIN
        SET @nextMonth = @Month + 1;
        SET @nextMonthYear = @Year;
    END;

    -- @salesTypeExportId INT = 2;
    SET @GenerateDate = ISNULL(@GenerateDate, GETDATE());
    DECLARE @firstDayOfMonth DATETIME
        = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01',
            @firstDayOfNextMonth DATETIME = CAST(@nextMonthYear AS VARCHAR(4)) + '-' + CAST(@nextMonth AS VARCHAR(2))
                                            + '-01';
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

    SELECT @outputVatAmmount
        = SUM(ISNULL(
                        dbo.FnGetCalculatedOutputVat(
                                                        slsDtl.UnitPrice * slsDtl.Quantity,
                                                        slsDtl.VATPercent,
                                                        slsDtl.SupplementaryDutyPercent
                                                    ),
                        0
                    )
             ),
           @outputSdAmmount
               = SUM(ISNULL(
                               dbo.FnGetCalculatedOutputSuppDuty(
                                                                    slsDtl.UnitPrice * slsDtl.Quantity,
                                                                    slsDtl.SupplementaryDutyPercent
                                                                ),
                               0
                           )
                    )
    FROM dbo.SalesDetails slsDtl
        INNER JOIN dbo.Sales sls
            ON sls.SalesId = slsDtl.SalesId
    WHERE sls.MushakGenerationId = @mushakGenerationId
          AND sls.SalesTypeId = @salesTypeLocalId;

    SELECT @inputVatAmmount
        = ISNULL(
                    SUM(ISNULL(
                                  dbo.FnGetCalculatedInputVat(
                                                                 purcDtl.UnitPrice * purcDtl.Quantity,
                                                                 purcDtl.VATPercent,
                                                                 purcDtl.SupplementaryDutyPercent
                                                             ),
                                  0
                              )
                       ),
                    0
                ),
           @inputSdAmmount
               = ISNULL(
                           SUM(ISNULL(
                                         dbo.FnGetCalculatedInputSuppDuty(
                                                                             purcDtl.UnitPrice * purcDtl.Quantity,
                                                                             purcDtl.SupplementaryDutyPercent
                                                                         ),
                                         0
                                     )
                              ),
                           0
                       )
    FROM dbo.PurchaseDetails purcDtl
        INNER JOIN dbo.Purchase purc
            ON purc.PurchaseId = purcDtl.PurchaseId
    WHERE purc.MushakGenerationId = @mushakGenerationId;

    UPDATE dbo.MushakGeneration
    SET AmountForVat = @outputVatAmmount - @inputVatAmmount,
        AmountForSuppDuty = @outputSdAmmount - @inputSdAmmount
    WHERE MushakGenerationId = @mushakGenerationId;
END;
