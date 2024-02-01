-- =============================================
-- Author:		<Author,,Md Mustafizur Rahman>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SpContractualrawMetaria]
    -- Add the parameters for the stored procedure here
    @ContractVendorTransferRawMateriallId INT
   
AS
BEGIN

SELECT 

org.Name AS OrgName,
org.BIN  AS OrgBin,
org.Address AS OrgAddress,
vd.name AS VenName,
vd.BinNo AS VenBin,
vd.Address AS VenAddress,
RawMat.ChallanNo,
RawMat.ChallanIssueDate,
ms.Name As MesurementName,
pd.Name AS ProductName,
RawMatD.Quantity ,
org.VatResponsiblePersonName ,
org.VatResponsiblePersonDesignation

 FROM ContractualProductionTransferRawMaterial RawMat 
LEFT JOIN ContractualProductionTransferRawMaterialDetails RawMatD ON RawMatD.ContractualProductionTransferRawMaterialId = RawMat.ContractualProductionTransferRawMaterialId
LEFT JOIN ContractualProduction CV ON CV.ContractualProductionId = RawMat.ContractualProductionId
LEFT JOIN Vendor vd ON CV.VendorId = vd.VendorId
LEFT JOIN Organizations org ON org.OrganizationId = CV.OrganizationId
LEFT JOIN Products pd ON pd.ProductId = RawMatD.RawMaterialId
LEFT JOIN MeasurementUnits ms ON ms.MeasurementUnitId = RawMatD.MeasurementUnitId

WHERE RawMat.ContractualProductionTransferRawMaterialId = @ContractVendorTransferRawMateriallId

END;
