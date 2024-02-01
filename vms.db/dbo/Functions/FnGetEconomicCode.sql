-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetEconomicCode]
(
    -- Add the parameters for the function here
    @Code1stDisit NVARCHAR(2),
    @Code2ndDisit NVARCHAR(2),
    @Code3rdDisit NVARCHAR(2),
    @Code4thDisit NVARCHAR(2),
    @Code5thtDisit NVARCHAR(2),
    @OperationalCode1stDisit NVARCHAR(2),
    @OperationalCode2ndDisit NVARCHAR(2),
    @OperationalCode3rdDisit NVARCHAR(2),
    @OperationalCode4thDisit NVARCHAR(2),
    @Code10thDisit NVARCHAR(2),
    @Code11thDisit NVARCHAR(2),
    @Code12thDisit NVARCHAR(2),
    @Code13thDisit NVARCHAR(2)
)
RETURNS NVARCHAR(50)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @code NVARCHAR(50);

    -- Add the T-SQL statements to compute the return value here
    SET @code
        = @Code1stDisit + N'/' + @Code2ndDisit + @Code3rdDisit + @Code4thDisit + @Code5thtDisit + N'/' + @OperationalCode1stDisit + @OperationalCode2ndDisit + @OperationalCode3rdDisit
          + @OperationalCode4thDisit + N'/'
          + @Code10thDisit + @Code11thDisit + @Code12thDisit + @Code13thDisit;

    -- Return the result of the function
    RETURN @code;

END;
