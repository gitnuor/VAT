
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXEC SpGetProductAutocompleteForSale 16, 'o'
-- =============================================
CREATE PROCEDURE [dbo].[SpGetProductAutocompleteForProductionReceive]
    @OrganizationId INT,
    @ContractualProductionId INT,
    @ProductSearchTerm NVARCHAR(100)
AS
BEGIN
    SELECT DISTINCT prod.ProductId,
           prod.Name AS ProductName,
           prod.ModelNo,
           prod.Code,
           prod.MeasurementUnitId,
           mu.Name AS MeasurementUnitName
    FROM dbo.Products prod
        LEFT JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = prod.MeasurementUnitId
        LEFT JOIN dbo.ContractualProductionProductDetails cppd
            ON cppd.ProductId = prod.ProductId
        LEFT JOIN dbo.ContractualProduction conProd
            ON conProd.ContractualProductionId = cppd.ContractualProductionId
    WHERE prod.OrganizationId = @OrganizationId
          AND prod.IsSellable = 1
          AND prod.Name LIKE N'%' + @ProductSearchTerm + '%'
          AND
          (
              @ContractualProductionId IS NULL
              OR @ContractualProductionId = 0
              OR conProd.ContractualProductionId = @ContractualProductionId
          );
END;
