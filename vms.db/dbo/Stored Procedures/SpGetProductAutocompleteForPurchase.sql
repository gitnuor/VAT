-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC [SpGetProductAutocompleteForPurchase] 5, 'co'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForPurchase]
    @OrganizationId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @StandardRatedProductVatType INT = 3;
    SELECT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           25.00 AS DefaultImportDutyPercent,
           3.00 AS DefaultRegulatoryDutyPercent,
           ISNULL(sd.SdPercent, 0) AS DefaultSupplimentaryDutyPercent,
           ISNULL(pv.ProductVATTypeId, 3) AS ProductVATTypeId,
           ISNULL(pvt.DefaultVatPercent, 15) AS DefaultVatPercent,
           3.00 AS DefaultAdvanceTaxPercent,
           5.00 AS DefaultAdvanceIncomeTaxPercent,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.ProductVATs pv
            ON pv.ProductId = prod.ProductId
               AND pv.IsActive = 1
        INNER JOIN dbo.ProductVATTypes pvt
            ON (
                   pvt.ProductVATTypeId = pv.ProductVATTypeId
                   AND pvt.IsActive = 1
               )
               OR pvt.ProductVATTypeId = @StandardRatedProductVatType
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.SupplimentaryDuty sd
            ON sd.ProductId = prod.ProductId
               AND sd.IsActive = 1
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%';
END;
