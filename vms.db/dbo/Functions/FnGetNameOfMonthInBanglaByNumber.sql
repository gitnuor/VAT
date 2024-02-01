
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetNameOfMonthInBanglaByNumber]
(
    -- Add the parameters for the function here
    @numberOfMonth INT
)
RETURNS NVARCHAR(20)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @nameOfMonthInBangla NVARCHAR(20);

    -- Add the T-SQL statements to compute the return value here
    IF @numberOfMonth = 1
    BEGIN
        SET @nameOfMonthInBangla = N'জানুয়ারী';
    END;
    ELSE IF @numberOfMonth = 2
    BEGIN
        SET @nameOfMonthInBangla = N'ফেব্রুয়ারি';
    END;
    ELSE IF @numberOfMonth = 3
    BEGIN
        SET @nameOfMonthInBangla = N'মার্চ';
    END;
    ELSE IF @numberOfMonth = 4
    BEGIN
        SET @nameOfMonthInBangla = N'এপ্রিল';
    END;
    ELSE IF @numberOfMonth = 5
    BEGIN
        SET @nameOfMonthInBangla = N'মে';
    END;
    ELSE IF @numberOfMonth = 6
    BEGIN
        SET @nameOfMonthInBangla = N'জুন';
    END;
    ELSE IF @numberOfMonth = 7
    BEGIN
        SET @nameOfMonthInBangla = N'জুলাই';
    END;
    ELSE IF @numberOfMonth = 8
    BEGIN
        SET @nameOfMonthInBangla = N'অগাস্ট';
    END;
    ELSE IF @numberOfMonth = 9
    BEGIN
        SET @nameOfMonthInBangla = N'সেপ্টেম্বর';
    END;
    ELSE IF @numberOfMonth = 10
    BEGIN
        SET @nameOfMonthInBangla = N'অক্টোবর';
    END;
    ELSE IF @numberOfMonth = 11
    BEGIN
        SET @nameOfMonthInBangla = N'নভেম্বর';
    END;
    ELSE IF @numberOfMonth = 12
    BEGIN
        SET @nameOfMonthInBangla = N'ডিসেম্বর';
    END;
    ELSE
    BEGIN
        SET @nameOfMonthInBangla = N'ভুল ইনপুট';
    END;

    -- Return the result of the function
    RETURN @nameOfMonthInBangla;

END;
