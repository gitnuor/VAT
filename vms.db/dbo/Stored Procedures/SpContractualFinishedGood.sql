-- =============================================
-- Author:		<Author,,Md Mustafizur Rahman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpContractualFinishedGood]
    -- Add the parameters for the stored procedure here
    @ContractVendorTransferRawMateriallId INT
   
AS
BEGIN

SELECT 

org.Name AS OrgName,
org.BIN  AS OrgBin,
org.Address AS OrgAddress,
vd.name AS VenName,
vd.BIN AS VenBin,
vd.Address AS VenAddress,
CV.ChallanNo AS ChallanNo,
CV.IssueDate AS ChallanIssueDate ,
ms.Name As MesurementName,
pd.Name AS ProductName,
CVD.Quantity AS Quantity,
org.VatResponsiblePersonName ,
org.VatResponsiblePersonDesignation
FROM ContractualProduction CV
LEFT JOIN ContractualProductionProductDetails CVD ON CV.ContractualProductionId = CVD.ContractualProductionId
LEFT JOIN Customer vd ON CV.CustomerId = vd.CustomerId
LEFT JOIN Organizations org ON org.OrganizationId = CV.OrganizationId
LEFT JOIN Products pd ON pd.ProductId = CVD.ProductId
LEFT JOIN MeasurementUnits ms ON ms.MeasurementUnitId = CVD.MeasurementUnitId

WHERE CV.ContractTypeId =2
AND 
 CV.ContractualProductionId = @ContractVendorTransferRawMateriallId
END;
