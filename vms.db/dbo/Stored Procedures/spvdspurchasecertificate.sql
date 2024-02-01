
CREATE PROCEDURE [dbo].[spvdspurchasecertificate]
(@PurchaseId INT)
AS
BEGIN

    DECLARE @organizationId INT,
            @IsVDSCertificatePrinted BIT;
    SELECT @organizationId = purch.OrganizationId,
           @IsVDSCertificatePrinted = purch.IsVDSCertificatePrinted
    FROM dbo.Purchase purch
    WHERE purch.PurchaseId = @PurchaseId;
    IF @IsVDSCertificatePrinted IS NULL
       OR @IsVDSCertificatePrinted = 0
    BEGIN

        UPDATE dbo.Purchase
        SET IsVDSCertificatePrinted = 1,
            VDSCertificateNo = N'VC'
                               + CAST(ISNULL(
                                      (
                                          SELECT COUNT(1)
                                          FROM dbo.Purchase purch
                                          WHERE purch.VDSCertificateNo IS NOT NULL
                                                AND purch.OrganizationId = @organizationId
                                      ),
                                      0
                                            ) + 1 AS NVARCHAR(25)),
            VDSCertificateDate = GETDATE()
        WHERE PurchaseId = @PurchaseId
              AND IsVatDeductedInSource = 1;
    END;
    SELECT vnd.[Name],
           vnd.BinNo,
           prc.VatChallanNo,
           prc.VatChallanIssueDate,
           prc.TotalPriceWithoutVat,
           prc.TotalVAT,
           prc.VDSAmount,
           prc.VDSCertificateNo,
           prc.VDSCertificateDate,
           org.Name AS ORGNAME,
           org.Address AS OrgAddress,
           org.BIN AS OrgBin,
           org.VatResponsiblePersonName,
           org.VatResponsiblePersonDesignation
    FROM [dbo].[Purchase] prc
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = prc.OrganizationId
        INNER JOIN dbo.Vendor vnd
            ON vnd.VendorId = prc.VendorId
    WHERE 1 = 1
          AND prc.IsVatDeductedInSource = 1
          --AND org.OrganizationId = 5
          AND prc.PurchaseId = @PurchaseId;
--AND org.IsDeductVatInSource = 1
END;

