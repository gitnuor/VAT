-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 5
-- =============================================
CREATE PROCEDURE [dbo].[SpSalesTaxInvoice] @SalesId INT
AS
BEGIN
    DECLARE @IsTaxInvoicePrined BIT,
            @TaxInvoicePrintedTime DATETIME,
            @ChallanNumber INT = 0,
            @OrganizationId INT;
    SELECT @IsTaxInvoicePrined = sls.IsTaxInvoicePrined,
           @TaxInvoicePrintedTime = sls.TaxInvoicePrintedTime,
           @OrganizationId = sls.OrganizationId
    FROM dbo.Sales sls
    WHERE sls.SalesId = @SalesId;

    IF @IsTaxInvoicePrined = 0
    BEGIN
        SELECT @ChallanNumber = COUNT(1)
        FROM dbo.Sales sls
        WHERE sls.IsTaxInvoicePrined = 1
              AND sls.OrganizationId = @OrganizationId;
        SET @ChallanNumber = ISNULL(@ChallanNumber, 0) + 1;
        SET @TaxInvoicePrintedTime = GETDATE();
        UPDATE dbo.Sales
        SET VatChallanNo = N'VCN' + CAST(@ChallanNumber AS NVARCHAR(25)),
            IsTaxInvoicePrined = 1,
            TaxInvoicePrintedTime = @TaxInvoicePrintedTime
        WHERE SalesId = @SalesId;
    END;
    SELECT ROW_NUMBER() OVER (ORDER BY slsDtl.SalesDetailId) AS Sl,
           sls.SalesId,
           sls.InvoiceNo,
           sls.VatChallanNo,
           @TaxInvoicePrintedTime AS TaxInvoiceIssueTime,
           sls.SalesDate,
           sls.OrganizationId,
           org.Name AS TaxRegisteredName,
           org.BIN AS TaxRegisteredBIN,
           org.Address AS TaxInvoiceIssueAddress,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonDesignation,
           sls.CustomerId,
           cust.Name AS CustomerName,
           cust.BIN AS CustomerBIN,
           sls.ReceiverName,
           sls.ReceiverContactNo,
           sls.ShippingAddress,
           sls.ShippingCountryId,
           sls.IsTaxInvoicePrined,
           sls.TaxInvoicePrintedTime,
           CASE
               WHEN @IsTaxInvoicePrined = 1 THEN
                   '(Copy)'
               ELSE
                   ''
           END AS InvoiceStatus,
           -------------------------------
           prod.Name AS ProductName,
		   prod.HSCode,
           prod.ModelNo,
           mu.Name AS MeasurementUnitName,
           slsDtl.Quantity,
           slsDtl.UnitPrice,
           slsDtl.UnitPrice * slsDtl.Quantity AS ProductPrice,
           slsDtl.SupplementaryDutyPercent,
           dbo.FnGetCalculatedOutputSuppDuty(
                                                slsDtl.UnitPrice * slsDtl.Quantity,
                                                ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                            ) AS ProdSupplementaryDutyAmount,
           slsDtl.VATPercent,
           dbo.FnGetCalculatedOutputVat(
                                           slsDtl.UnitPrice * slsDtl.Quantity,
                                           slsDtl.VATPercent,
                                           ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                       ) AS ProdVATAmount,
           (slsDtl.UnitPrice * slsDtl.Quantity)
           + dbo.FnGetCalculatedOutputSuppDuty(
                                                  slsDtl.UnitPrice * slsDtl.Quantity,
                                                  ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                              )
           + dbo.FnGetCalculatedOutputVat(
                                             slsDtl.UnitPrice * slsDtl.Quantity,
                                             slsDtl.VATPercent,
                                             ISNULL(slsDtl.SupplementaryDutyPercent, 0)
                                         ) AS ProdPriceInclVATAndDuty,
           [ExportDetails] = CASE
                                 WHEN sls.SalesTypeId = 2 THEN
                                     'Lc No=' + ISNULL(sls.LcNo, '') + '; Lc Date=' + ISNULL(CAST(sls.LcDate AS VARCHAR(20)), '')
                                     + '; Commercial Invoice=' + ISNULL(sls.BillOfEntry, '') + '; Date'
                                     + ISNULL(CAST(sls.BillOfEntryDate AS VARCHAR(20)), '')
                                 ELSE
                                     ''
                             END
    -------------------------------
    FROM dbo.Sales sls
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = sls.OrganizationId
        INNER JOIN dbo.SalesDetails slsDtl
            ON slsDtl.SalesId = sls.SalesId
        INNER JOIN dbo.Products prod
            ON prod.ProductId = slsDtl.ProductId
        INNER JOIN dbo.MeasurementUnits mu
            ON mu.MeasurementUnitId = slsDtl.MeasurementUnitId
        LEFT JOIN dbo.Customer cust
            ON cust.CustomerId = sls.CustomerId
    WHERE sls.SalesId = @SalesId;
END;


