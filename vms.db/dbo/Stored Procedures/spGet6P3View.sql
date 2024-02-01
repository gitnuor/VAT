﻿
-- =============================================
-- Author:		<MD SABBIR REZA>
-- Create date: <2019-09-22>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGet6P3View]
	-- Add the parameters for the stored procedure here
	-- Add the parameters for the stored procedure here
	@InvoiceNo NVARCHAR(50)= '',
	@CustomerName NVARCHAR(200)= '',
	@FromDate DATETIME= NULL,
	@ToDate DATETIME= NULL

AS
BEGIN
SELECT SalesId,InvoiceNo,Isnull(cus.Name,'Not Found') as CustomerName,sl.SalesDate FROM Sales sl
left join Customer cus on sl.CustomerId=cus.CustomerId
--where 
--(
--@InvoiceNo='' OR
-- sl.InvoiceNo like '%'+@InvoiceNo+'%'
-- )
--AND 
--(
--@CustomerName='' OR cus.Name LIKE '%'+@CustomerName+'%'
--)
--AND 
--( 
--SL.SalesDate >= @FromDate AND SL.SalesDate< DATEADD(DAY, 1, @ToDate)
--)

END
