-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--EXECUTE SpGetVatType 0, 1, 0, 0, 0
-- =============================================
CREATE PROCEDURE [dbo].[SpGetVatType]
    -- Add the parameters for the stored procedure here
    @IsLocalSale BIT,
    @IsExport BIT,
    @IsLocalPurchase BIT,
    @IsImport BIT,
	@IsVds BIT 

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT pvt.ProductVATTypeId,
           CASE
               WHEN @IsLocalSale = 1 THEN
                   pvt.Name + N' (' + pvt.LocalSaleNote + N')'
               WHEN @IsExport = 1 THEN
                   pvt.Name + N' (' + pvt.ExportNote + N')'
               WHEN @IsLocalPurchase = 1 THEN
                   pvt.Name + N' (' + pvt.LocalPurchaseNote + N')'
               WHEN @IsImport = 1 THEN
                   pvt.Name + N' (' + pvt.ImportNote + N')'
               ELSE
                   pvt.Name
           END AS VatTypeName,
           CASE
               WHEN @IsLocalSale = 1 THEN
                   pvt.NameInBangla + N' (' + pvt.LocalSaleNoteInBn + N')'
               WHEN @IsExport = 1 THEN
                   pvt.NameInBangla + N' (' + pvt.ExportNoteInBn + N')'
               WHEN @IsLocalPurchase = 1 THEN
                   pvt.NameInBangla + N' (' + pvt.LocalPurchaseNoteInBn + N')'
               WHEN @IsImport = 1 THEN
                   pvt.NameInBangla + N' (' + pvt.ImportNoteInBn + N')'
               ELSE
                   pvt.NameInBangla
           END AS VatTypeNameInBn,
           pvt.DefaultVatPercent
    FROM dbo.ProductVATTypes pvt
    WHERE CASE
              WHEN @IsLocalSale = 1 THEN
                  pvt.IsApplicableForLocalSale
              WHEN @IsExport = 1 THEN
                  pvt.IsApplicableForExport
              WHEN @IsLocalPurchase = 1 THEN
                  pvt.IsApplicableForLocalPurchase
              WHEN @IsImport = 1 THEN
                  pvt.IsApplicableForImport
              ELSE
                  0
          END = 1 AND (@IsVds = 1 OR pvt.IsRequireVDS = 0);
END;
