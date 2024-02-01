-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpInputOutputCo_ofi] @Id INT
AS
BEGIN

select 
[ProductId]=prod.ProductId
,[OrganizationId]=prod.OrganizationId
,[OrganizationName]=org.Name
,[OrgAddress] = org.Address
,[OrgBin]=org.BIN
,[HSCode]=prod.HSCode
,[ProductName]=prod.[Name]
,[UnitName]=mu.[Name]
,[Cost] = pspc.[Cost]
,[WastageAmount]=(pspc.WastagePercentage*pspc.RequiredQty)/100
,[WastagePercentage]=pspc.WastagePercentage
,[TotalAmount]=pspc.RequiredQty+(pspc.WastagePercentage*pspc.RequiredQty)/100
,[OverHeadCostName]=ohc.[Name]
,[Cost1]=pspc.Cost
from Products prod
LEFT JOIN PriceSetup ps on prod.ProductId=ps.ProductId
LEFT JOIN PriceSetupProductCost pspc on ps.PriceSetupId=pspc.PriceSetupId
INNER JOIN MeasurementUnits mu on prod.MeasurementUnitId=mu.MeasurementUnitId
LEFT JOIN OverHeadCost ohc on pspc.OverHeadCostId=ohc.OverHeadCostId
Inner Join Organizations org ON org.OrganizationId=prod.OrganizationId

--LEFT JOIN 
where 1=1
AND ps.IsActive=1
AND prod.ProductId=@Id
ORDER BY prod.ProductId
END

