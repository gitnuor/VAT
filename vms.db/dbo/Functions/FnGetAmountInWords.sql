-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetAmountInWords]
(
    -- Add the parameters for the function here
    @Amount DECIMAL(18, 2)
)
RETURNS NVARCHAR(4000)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @AmountInWords NVARCHAR(4000),
            @Taka BIGINT,
            @Poisha INT,
            @TakaInwords NVARCHAR(2000),
            @PoishaInWords NVARCHAR(1000);

    -- Add the T-SQL statements to compute the return value here
    SET @Taka = PARSENAME(ISNULL(@Amount, 0), 2);
    SET @Poisha = PARSENAME(ISNULL(@Amount, 0), 1);

    SELECT @TakaInwords = dbo.FnNumberToWords(@Taka),
           @PoishaInWords = dbo.FnNumberToWords(@Poisha);
    SET @AmountInWords = N'=' + @TakaInwords + N' Taka and ' + @PoishaInWords + N' Poisha Only';
    -- Return the result of the function
    RETURN @AmountInWords;

END;
