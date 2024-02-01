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
