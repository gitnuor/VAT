using System;

namespace vms.entity.StoredProcedureModel.MushakReturn;

public class SpGetMushakReturnPaymentChallan
{
    public int MushakReturnPaymentId { get; set; }
    public int MushakReturnPaymentTypeId { get; set; }
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string TypeName { get; set; }
    public string TypeNameBn { get; set; }
    public decimal PaidAmount { get; set; }
    public long PaidAmountTaka { get; set; }
    public int PaidAmountPoisha { get; set; }
    public string PaidAmountInWords { get; set; }
    public string PaidAmountTakaInBangla { get; set; }
    public string PaidAmountPoishaInBangla { get; set; }
    public string PaidAmountInBanglaWords { get; set; }
    public int NbrEconomicCodeId { get; set; }
    public string EconomicCode { get; set; }
    public string EconomicCode1stDisit { get; set; }
    public string EconomicCode2ndDisit { get; set; }
    public string EconomicCode3rdDisit { get; set; }
    public string EconomicCode4thDisit { get; set; }
    public string EconomicCode5thDisit { get; set; }
    public string EconomicCode6thDisit { get; set; }
    public string EconomicCode7thDisit { get; set; }
    public string EconomicCode8thDisit { get; set; }
    public string EconomicCode9thDisit { get; set; }
    public string EconomicCode10thDisit { get; set; }
    public string EconomicCode11thDisit { get; set; }
    public string EconomicCode12thDisit { get; set; }
    public string EconomicCode13thDisit { get; set; }
    public string TreasuryChallanNo { get; set; }
    public DateTime PaymentDate { get; set; }
    public int BankBranchId { get; set; }
    public string BankName { get; set; }
    public string BankBranchName { get; set; }
    public string DistrictName { get; set; }
    public byte ChallanCopySl { get; set; }
    public string ChallanCopyName { get; set; }
}