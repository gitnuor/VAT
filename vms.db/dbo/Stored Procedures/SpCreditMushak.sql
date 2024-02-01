-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpCreditMushak] @CreditNoteID INT
AS
BEGIN

    DECLARE @SuplimenteryDuty DECIMAL(18, 2);
    SELECT CUS.CustomerId,
           CUS.Name AS CusName,
           CUS.BIN AS CusBin,
           Sl.InvoiceNo AS SaleInvoice,
           Sl.SalesDate AS SaleDate,
           ORG.Name AS OrgName,
           ORG.BIN AS OrgBin,
           ORG.VatResponsiblePersonName AS VatPName,
           ORG.VatResponsiblePersonDesignation AS VatPDes,
           CR.CreditNoteId AS CreditNo,
           CR.CreatedTime AS CrTime,
           PD.Name AS ProductName,
           MJ.Name AS Quantity,
           CRD.ReturnQuantity AS ReturnQuantity,
           SCD.UnitPrice AS UnitPrice,
           CR.ReasonOfReturn AS ReasonOfReturn,
           dbo.FnGetCalculatedOutputSuppDuty(
                                                ISNULL((CRD.ReturnQuantity * SCD.UnitPrice), 0),
                                                ISNULL(SCD.SupplementaryDutyPercent, 0)
                                            ) AS SupplementaryDutyAmount,
           --((CRD.ReturnQuantity * SCD.UnitPrice) * SCD.SupplementaryDutyPercent / 100) AS SupplementaryDutyAmount,
           dbo.FnGetCalculatedOutputVat(
                                           ISNULL((CRD.ReturnQuantity * SCD.UnitPrice), 0),
                                           ISNULL(SCD.VATPercent, 0),
                                           ISNULL(SCD.SupplementaryDutyPercent, 0)
                                       ) AS VATAMOUNT,
           --((CRD.ReturnQuantity * SCD.UnitPrice) + ((CRD.ReturnQuantity * SCD.UnitPrice) * SCD.SupplementaryDutyPercent / 100)) * SCD.VATPercent / (100 - SCD.VATPercent) AS VATAMOUNT,
           0 AS KORTON,
           (CRD.ReturnQuantity * SCD.UnitPrice) AS TotalAmount
    FROM CreditNote CR
        INNER JOIN CreditNoteDetail CRD
            ON CR.CreditNoteId = CRD.CreditNoteId
        INNER JOIN Sales Sl
            ON Sl.SalesId = CR.SalesId
        INNER JOIN SalesDetails SCD
            ON SCD.SalesDetailId = CRD.SalesDetailId
        INNER JOIN Organizations ORG
            ON ORG.OrganizationId = Sl.OrganizationId
        INNER JOIN Customer CUS
            ON CUS.CustomerId = Sl.CustomerId
        INNER JOIN Products PD
            ON PD.ProductId = SCD.ProductId
        INNER JOIN MeasurementUnits MJ
            ON MJ.MeasurementUnitId = SCD.MeasurementUnitId
    WHERE CR.CreditNoteId = @CreditNoteID
    ORDER BY PD.Name;
END;

