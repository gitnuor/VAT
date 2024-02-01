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
