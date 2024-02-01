CREATE FUNCTION [dbo].[FnNumberToWords]
(
    @Number AS BIGINT
)
RETURNS VARCHAR(4000)
AS
BEGIN

    DECLARE @Below20 TABLE
    (
        ID INT IDENTITY(0, 1),
        Word VARCHAR(50)
    );

    DECLARE @Below100 TABLE
    (
        ID INT IDENTITY(2, 1),
        Word VARCHAR(50)
    );

    INSERT @Below20
    (
        Word
    )
    VALUES
    ('Zero'),
    ('One'),
    ('Two'),
    ('Three'),
    ('Four'),
    ('Five'),
    ('Six'),
    ('Seven'),
    ('Eight'),
    ('Nine'),
    ('Ten'),
    ('Eleven'),
    ('Twelve'),
    ('Thirteen'),
    ('Fourteen'),
    ('Fifteen'),
    ('Sixteen'),
    ('Seventeen'),
    ('Eighteen'),
    ('Nineteen');

    INSERT @Below100
    VALUES
    ('Twenty'),
    ('Thirty'),
    ('Forty'),
    ('Fifty'),
    ('Sixty'),
    ('Seventy'),
    ('Eighty'),
    ('Ninety');

    DECLARE @English VARCHAR(4000)
        =
            (
                SELECT CASE
                           WHEN @Number = 0 THEN
                               'Zero'
                           WHEN @Number
                                BETWEEN 1 AND 19 THEN
                           (
                               SELECT Word FROM @Below20 WHERE ID = @Number
                           )
                           WHEN @Number
                                BETWEEN 20 AND 99 THEN
                    (
                        SELECT Word FROM @Below100 WHERE ID = @Number / 10
                    ) + '-' + dbo.FnNumberToWords(@Number % 10)
                           WHEN @Number
                                BETWEEN 100 AND 999 THEN
                    (dbo.FnNumberToWords(@Number / 100)) + ' Hundred ' + dbo.FnNumberToWords(@Number % 100)
                           WHEN @Number
                                BETWEEN 1000 AND 999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000)) + ' Thousand ' + dbo.FnNumberToWords(@Number % 1000)
                           WHEN @Number
                                BETWEEN 1000000 AND 999999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000000)) + ' Million ' + dbo.FnNumberToWords(@Number % 1000000)
                           WHEN @Number
                                BETWEEN 1000000000 AND 999999999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000000000)) + ' Billion '
                    + dbo.FnNumberToWords(@Number % 1000000000)
                           WHEN @Number
                                BETWEEN 1000000000000 AND 999999999999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000000000000)) + ' Trillion '
                    + dbo.FnNumberToWords(@Number % 1000000000000)
                           WHEN @Number
                                BETWEEN 1000000000000000 AND 999999999999999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000000000000000)) + ' Quadrillion '
                    + dbo.FnNumberToWords(@Number % 1000000000000000)
                           WHEN @Number
                                BETWEEN 1000000000000000000 AND 999999999999999999999 THEN
                    (dbo.FnNumberToWords(@Number / 1000000000000000000)) + ' Quintillion '
                    + dbo.FnNumberToWords(@Number % 1000000000000000000)
                           ELSE
                               ' INVALID INPUT'
                       END
            );



    SELECT @English = RTRIM(@English);

    SELECT @English = RTRIM(LEFT(@English, LEN(@English) - 1))
    WHERE RIGHT(@English, 1) = '-';

    RETURN (@English);

END;
