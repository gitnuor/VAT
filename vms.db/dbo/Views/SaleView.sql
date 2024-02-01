CREATE VIEW [dbo].[SaleView] as
SELECT 
ROW_NUMBER() OVER (ORDER BY SL.SalesDate DESC) AS Serial,
SL.SalesId,
SL.InvoiceNo,
SL.SalesDate,
SL.ExpectedDeliveryDate,
SL.DeliveryDate,
ORG.OrganizationId,
ORG.Name AS OrganizationName

 FROM Sales SL
LEFT JOIN Customer CUS ON SL.CustomerId=CUS.CustomerId
LEFT JOIN Organizations ORG ON SL.OrganizationId=ORG.OrganizationId

