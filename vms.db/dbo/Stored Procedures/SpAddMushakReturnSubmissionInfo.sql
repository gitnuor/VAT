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
