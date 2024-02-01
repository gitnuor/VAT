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
