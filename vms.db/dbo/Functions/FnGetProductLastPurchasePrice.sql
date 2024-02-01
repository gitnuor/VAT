-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FnGetProductLastPurchasePrice]
(
    -- Add the parameters for the function here
    @ProductId INT
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @lastPurchaseRate DECIMAL(18, 2);

    -- Add the T-SQL statements to compute the return value here
    SELECT TOP (1)
           @lastPurchaseRate = purchDtl.UnitPrice
    FROM dbo.PurchaseDetails purchDtl
    WHERE purchDtl.ProductId = @ProductId
    ORDER BY purchDtl.PurchaseDetailId DESC;

    -- Return the result of the function
    RETURN @lastPurchaseRate;

END;
