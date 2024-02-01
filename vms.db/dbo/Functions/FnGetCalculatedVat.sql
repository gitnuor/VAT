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
