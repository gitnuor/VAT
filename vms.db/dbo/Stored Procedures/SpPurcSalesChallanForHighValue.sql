-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- SpPurcSalesChallanForHighValue 5, 2019, 12, NULL
-- =============================================
CREATE PROCEDURE [dbo].[SpPurcSalesChallanForHighValue]
    @OrganizationId INT,
    @Year INT,
    @Month INT,
    @LowerLimitOfHighValSale DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SalesTypeLocal INT = 1,
            @SalesTypeExport INT = 2;
    DECLARE @nextMonth INT = @Month + 1,
            @nextMonthYear INT = @Year;

    IF @Month >= 12
    BEGIN
        SET @nextMonth = 1;
        SET @nextMonthYear = @Year + 1;
    END;
    DECLARE @firstDayOfMonth DATETIME
        = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01',
            @firstDayOfNextMonth DATETIME = CAST(@nextMonthYear AS VARCHAR(4)) + '-' + CAST(@nextMonth AS VARCHAR(2))
                                            + '-01';
    SET @LowerLimitOfHighValSale = ISNULL(@LowerLimitOfHighValSale, 200000);
    SELECT ROW_NUMBER() OVER (ORDER BY purc.PurchaseId) AS Sl,
           purc.PurchaseId,
           purc.VendorInvoiceNo,
           purc.PurchaseDate,
           purc.VatChallanNo,
           purc.VatChallanIssueDate,
           vndr.Name AS VendorName,
           vndr.Address AS VendorAddress,
           ISNULL(vndr.BinNo, vndr.NationalIdNo) AS VendorBinOrNid,
           org.Name AS TaxRegisteredName,
           org.BIN AS TaxRegisteredBIN,
           org.Address AS TaxInvoiceIssueAddress,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonDesignation,
                                                                                                              -------------------------------
           purc.TotalPriceWithoutVat,
           purc.TotalPriceWithoutVat + purc.TotalSupplementaryDuty + purc.TotalVAT AS ProdPriceInclVATAndDuty --INTO SpPurcSalesChallanForHighValuePurchase
    -------------------------------
    FROM dbo.Purchase purc
        INNER JOIN dbo.Vendor vndr
            ON vndr.VendorId = purc.VendorId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = purc.OrganizationId
    WHERE purc.OrganizationId = @OrganizationId
          AND purc.CreatedTime >= @firstDayOfMonth
          AND purc.CreatedTime < @firstDayOfNextMonth
          AND purc.TotalPriceWithoutVat + purc.TotalVAT + purc.TotalSupplementaryDuty >= @LowerLimitOfHighValSale;



    SELECT ROW_NUMBER() OVER (ORDER BY sls.SalesId) AS Sl,
           sls.SalesId,
           sls.InvoiceNo,
           sls.VatChallanNo,
           sls.TaxInvoicePrintedTime,
           sls.SalesDate,
           sls.CustomerId,
           cust.Name AS CustomerName,
           cust.Address AS CustomerAddress,
           ISNULL(cust.BIN, cust.NIDNo) AS CustomerBIN,
                                                                                                           -------------------------------
           sls.TotalPriceWithoutVat,
           sls.TotalPriceWithoutVat + sls.TotalSupplimentaryDuty + sls.TotalVAT AS ProdPriceInclVATAndDuty --INTO SpPurcSalesChallanForHighValueSale
    -------------------------------
    FROM dbo.Sales sls
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.OrganizationId = @OrganizationId
          AND
          (
              sls.SalesTypeId = @SalesTypeLocal
              OR sls.SalesTypeId = @SalesTypeExport
          )
          AND sls.CreatedTime >= @firstDayOfMonth
          AND sls.CreatedTime < @firstDayOfNextMonth
          AND sls.TotalPriceWithoutVat + sls.TotalVAT + sls.TotalSupplimentaryDuty >= @LowerLimitOfHighValSale;
END;
