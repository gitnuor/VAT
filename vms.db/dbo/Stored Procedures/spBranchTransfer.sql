
CREATE PROCEDURE [dbo].[spBranchTransfer]
(
    
    @SaleId INT
)
AS
BEGIN


    SELECT 

	OrgPar.Name    AS PareName,
	OrgPar.BIN     AS PareBin,
	OrgPar.Address AS PareAddre,
	OrgPar.VatResponsiblePersonName     AS PareVatPName,
	OrgPar.VatResponsiblePersonDesignation  AS PareVatPDes,

	OrgSen.Name    AS SenName,
	OrgSen.Address AS SenAddre,
	
	OrgReci.Name    AS ReciName,
	OrgReci.Address AS ReciAddre,
	Sa.InvoiceNo,
	Sa.SalesDate,
	Pd.Name AS ProduName,
	Sd.Quantity AS Qty,
	(Sd.UnitPrice*Sd.Quantity)-[dbo].[FnGetCalculatedVat] ((Sd.UnitPrice*Sd.Quantity) ,sd.VATPercent,sd.SupplementaryDutyPercent)  AS Price,
     [dbo].[FnGetCalculatedVat] ((Sd.UnitPrice*Sd.Quantity) ,sd.VATPercent,sd.SupplementaryDutyPercent) as VtaAmount

    FROM Sales Sa 
	LEFT JOIN  Organizations OrgSen ON OrgSen.OrganizationId = Sa.OrganizationId
	LEFT JOIN  Organizations OrgReci ON OrgReci.OrganizationId = Sa.OtherBranchOrganizationId
	LEFT JOIN  Organizations OrgPar ON OrgPar.OrganizationId = OrgSen.ParentOrganizationId
	LEFT JOIN  SalesDetails Sd ON Sd.SalesId=Sa.SalesId
	LEFT JOIN  Products Pd ON Pd.ProductId=Sd.ProductId





	where 
	
	
	sa.SalesId = @SaleId
END;
