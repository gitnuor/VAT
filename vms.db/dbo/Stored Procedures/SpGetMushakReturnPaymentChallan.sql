
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,> [dbo].[SpGetMushakReturnPaymentChallan] 3
-- =============================================
CREATE PROCEDURE [dbo].[SpGetMushakReturnPaymentChallan]
    -- Add the parameters for the stored procedure here
    @MushakReturnPaymentId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @ChallanCopy TABLE
    (
        CopySl TINYINT NOT NULL,
        CopyName NVARCHAR(20) NOT NULL
    );

    INSERT INTO @ChallanCopy
    (
        CopySl,
        CopyName
    )
    VALUES
    (   1,              -- CopySl - tinyint
        N'১ম (মূল) কপি' -- CopyName - nvarchar(20)
        ),
    (   2,        -- CopySl - tinyint
        N'২য় কপি' -- CopyName - nvarchar(20)
    ),
    (   3,        -- CopySl - tinyint
        N'৩য় কপি' -- CopyName - nvarchar(20)
    );

    SELECT pmntInfo.MushakReturnPaymentId,
           pmntInfo.MushakReturnPaymentTypeId,
           pmntInfo.OrganizationId,
           OrganizationName = org.Name,
           OrganizationAddress = org.Address,
           pmntType.TypeName,
           pmntType.TypeNameBn,
           pmntInfo.PaidAmount,
           PaidAmountTaka = CAST(PARSENAME(ISNULL(pmntInfo.PaidAmount, 0), 2) AS BIGINT),
           PaidAmountPoisha = CAST(PARSENAME(ISNULL(pmntInfo.PaidAmount, 0), 1) AS INT),
           PaidAmountInWords = dbo.FnGetAmountInWords(pmntInfo.PaidAmount),
           PaidAmountTakaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pmntInfo.PaidAmount, 0), 2)),
           PaidAmountPoishaInBangla = dbo.FnConvertIntToBanglaUnicodeNumber(PARSENAME(ISNULL(pmntInfo.PaidAmount, 0), 1)),
           PaidAmountInBanglaWords = N'',
           pmntType.NbrEconomicCodeId,
           nec.EconomicCode,
           EconomicCode1stDisit = nec.Code1stDisit,
           EconomicCode2ndDisit = nec.Code2ndDisit,
           EconomicCode3rdDisit = nec.Code3rdDisit,
           EconomicCode4thDisit = nec.Code4thDisit,
           EconomicCode5thDisit = nec.Code5thDisit,
           EconomicCode6thDisit = cavc.OperationalCode1stDigit,
           EconomicCode7thDisit = cavc.OperationalCode2ndDigit,
           EconomicCode8thDisit = cavc.OperationalCode3rdDigit,
           EconomicCode9thDisit = cavc.OperationalCode4thDigit,
           EconomicCode10thDisit = nec.Code10thDisit,
           EconomicCode11thDisit = nec.Code11thDisit,
           EconomicCode12thDisit = nec.Code12thDisit,
           EconomicCode13thDisit = nec.Code13thDisit,
           pmntInfo.TreasuryChallanNo,
           pmntInfo.PaymentDate,
           pmntInfo.BankBranchId,
           BankName = bnk.NameInBangla,
           BankBranchName = bnkBrnch.NameInBangla,
           DistrictName = dist.NameInBangla,
           ChallanCopySl = cc.CopySl,
           ChallanCopyName = cc.CopyName
    FROM dbo.MushakReturnPayment pmntInfo
        INNER JOIN dbo.MushakReturnPaymentType pmntType
            ON pmntType.MushakReturnPaymentTypeId = pmntInfo.MushakReturnPaymentTypeId
        INNER JOIN dbo.CustomsAndVATCommissionarate cavc
            ON cavc.CustomsAndVATCommissionarateId = pmntInfo.CustomsAndVATCommissionarateId
        INNER JOIN dbo.Organizations org
            ON org.OrganizationId = pmntInfo.OrganizationId
        INNER JOIN dbo.NbrEconomicCode nec
            ON nec.NbrEconomicCodeId = pmntType.NbrEconomicCodeId
        INNER JOIN dbo.BankBranch bnkBrnch
            ON bnkBrnch.BankBranchId = pmntInfo.BankBranchId
        INNER JOIN dbo.Bank bnk
            ON bnk.BankId = bnkBrnch.BankId
        INNER JOIN dbo.District dist
            ON dist.DistrictId = bnkBrnch.DistrictId
        CROSS JOIN @ChallanCopy cc
    WHERE pmntInfo.MushakReturnPaymentId = @MushakReturnPaymentId
    ORDER BY cc.CopySl;
END;
