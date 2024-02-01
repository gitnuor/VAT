-- =============================================
-- Author:		<MD SABBIR REZA>
-- Create date: <2019-09-25>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetSalePaged]
	-- Add the parameters for the stored procedure here
@PageSize INT=10,
@PageNumber INT=1,
@OrganizationId INT =0
AS
BEGIN
DECLARE @OffsetRow INT,
        @TotalRow INT,
		@SerialNo BIGINT
SET @OffsetRow = IIF(@PageNumber > 0, (@PageNumber - 1) * @PageSize, 0);

SELECT TOP(@PageSize) * 
FROM SaleView sv
WHERE sv.serial>@OffsetRow
ORDER BY sv.serial
       
END
