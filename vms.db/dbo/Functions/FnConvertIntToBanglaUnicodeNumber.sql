
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
