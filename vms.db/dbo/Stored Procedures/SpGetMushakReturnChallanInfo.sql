-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,> [dbo].[SpGetMushakReturnChallanInfo] 5
-- =============================================
CREATE PROCEDURE [dbo].[SpGetMushakReturnChallanInfo]
    -- Add the parameters for the stored procedure here
    @MushakGenerationId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ChallanCopy TABLE
    (
        CopyName NVARCHAR(20) NOT NULL
    );

    INSERT INTO @ChallanCopy
    (
        CopyName
    )
    VALUES
    (N'১ম (মূল) কপি' -- CopyName - nvarchar(20)
        ),
    (N'২য় কপি' -- CopyName - nvarchar(20)
    ),
    (N'৩য় কপি' -- CopyName - nvarchar(20)
    );

    SELECT pInfo.MushakGenerationInfoId,
           pInfo.MushakGenerationId,
           pInfo.OrganizationId,
           OrganizationName = org.Name,
           OrganizationAddress = org.Address,
           pInfo.PurposeOfPayment,
           pInfo.PaidAmount,
           PaidAmountTakaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pInfo.PaidAmount, 0), 2)),
           PaidAmountPoishaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pInfo.PaidAmount, 0), 1)),
           PaidAmountInBanglaWords = N'',
           pInfo.EconomicCodeId,
           nec.EconomicCode,
           EconomicCode1stDisit = nec.Code1stDisit,
           EconomicCode2ndDisit = nec.Code2ndDisit,
           EconomicCode3rdDisit = nec.Code3rdDisit,
           EconomicCode4thDisit = nec.Code4thDisit,
           EconomicCode5thDisit = nec.Code5thDisit,
           EconomicCode6thDisit = nec.Code6thDisit,
           EconomicCode7thDisit = nec.Code7thDisit,
           EconomicCode8thDisit = nec.Code8thDisit,
           EconomicCode9thDisit = nec.Code9thDisit,
           EconomicCode10thDisit = nec.Code10thDisit,
           EconomicCode11thDisit = nec.Code11thDisit,
           EconomicCode12thDisit = nec.Code12thDisit,
           EconomicCode13thDisit = nec.Code13thDisit,
           pInfo.PaymentDate,
           pInfo.BankBranchId,
           BankName = bnk.NameInBangla,
           BankBranchName = bnkBrnch.NameInBangla,
           DistrictName = dist.NameInBangla,
           ChallanCopyName = cc.CopyName
    FROM dbo.FnGetMushakGenerationDepositInfo(@MushakGenerationId) pInfo
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = pInfo.OrganizationId
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = pInfo.EconomicCodeId
        INNER JOIN dbo.BankBranch bnkBrnch
            ON bnkBrnch.BankBranchId = pInfo.BankBranchId
        INNER JOIN dbo.Bank bnk
            ON bnk.BankId = bnkBrnch.BankId
        INNER JOIN dbo.District dist
            ON dist.DistrictId = bnkBrnch.DistrictId
        CROSS JOIN @ChallanCopy cc
    ORDER BY pInfo.MushakGenerationInfoId;
END;
