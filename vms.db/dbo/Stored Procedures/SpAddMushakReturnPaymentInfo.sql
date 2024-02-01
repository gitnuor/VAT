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
