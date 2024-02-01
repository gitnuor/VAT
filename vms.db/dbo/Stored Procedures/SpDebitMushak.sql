-- =============================================
-- Author:		<Author,,MUSTAFIZUR RAHMAN>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- [dbo].[SpSalesTaxInvoice] 8
-- =============================================
CREATE PROCEDURE [dbo].[SpDebitMushak] @DEBITNOTEID INT
AS
BEGIN

    DECLARE @SuplimenteryDuty DECIMAL(18, 2);
    SELECT VEN.VendorId,
           VEN.Name AS VENName,
           VEN.BinNo AS VENBin,
           PS.InvoiceNo AS PSInvoice,
           PS.PurchaseDate AS PSDate,
           ORG.Name AS OrgName,
           ORG.BIN AS OrgBin,
           ORG.VatResponsiblePersonName AS VatPName,
           ORG.VatResponsiblePersonDesignation AS VatPDes,
           DR.DebitNoteId AS DebitNo,
           DR.CreatedTime AS DrTime,
           PD.Name AS ProductName,
           MJ.Name AS Quantity,
           DRD.ReturnQuantity AS ReturnQuantity,
           PDD.UnitPrice AS UnitPrice,
           DR.ReasonOfReturn AS ReasonOfReturn,
           dbo.FnGetCalculatedInputSuppDuty(
                                                ISNULL((DRD.ReturnQuantity * PDD.UnitPrice), 0),
                                                ISNULL(PDD.SupplementaryDutyPercent, 0)
                                            ) AS SupplementaryDutyAmount,
           --((DRD.ReturnQuantity * PDD.UnitPrice) * PDD.SupplementaryDutyPercent / 100) AS SupplementaryDutyAmount,
           --((DRD.ReturnQuantity * PDD.UnitPrice)
           -- + ((DRD.ReturnQuantity * PDD.UnitPrice) * PDD.SupplementaryDutyPercent / 100)
           --) * PDD.VATPercent
           --/ (100 - PDD.VATPercent) AS VATAMOUNT,
           dbo.FnGetCalculatedInputVat(
                                           ISNULL((DRD.ReturnQuantity * PDD.UnitPrice), 0),
                                           ISNULL(PDD.VATPercent, 0),
                                           ISNULL(PDD.SupplementaryDutyPercent, 0)
                                       ) AS VATAMOUNT,
           0 AS KORTON,
           (DRD.ReturnQuantity * PDD.UnitPrice) AS TotalAmount
    FROM DebitNote DR
        INNER JOIN DebitNoteDetail DRD
            ON DR.DebitNoteId = DRD.DebitNoteId
        INNER JOIN Purchase PS
            ON PS.PurchaseId = DR.PurchaseId
        INNER JOIN PurchaseDetails PDD
            ON PDD.PurchaseDetailId = DRD.PurchaseDetailId
        INNER JOIN Organizations ORG
            ON ORG.OrganizationId = PS.OrganizationId
        INNER JOIN Vendor VEN
            ON VEN.VendorId = PS.VendorId
        INNER JOIN Products PD
            ON PD.ProductId = PDD.ProductId
        INNER JOIN MeasurementUnits MJ
            ON MJ.MeasurementUnitId = PDD.MeasurementUnitId
    WHERE DR.DebitNoteId = @DEBITNOTEID;

END;
