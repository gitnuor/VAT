using System;

namespace vms.entity.viewModels.VmPurchaseCombineParamsModels;

public class PurchaseImportTaxPaymentCombinedParamModel
{
    public int TaxPaymentId { get; set; }
    public int PurchaseId { get; set; }
    public int TaxPaymentTypeId { get; set; }
    public string TaxPaymentTypeName { get; set; }
    public int TaxPaymentVatCommissionerateId { get; set; }
    public string TaxPaymentVatCommissionerateName { get; set; }
    public int TaxPaymentBankId { get; set; }
    public string TaxPaymentBankName { get; set; }
    public string TaxPaymentBankBranchName { get; set; }
    public int TaxPaymentBankBranchDistrictId { get; set; }
    public string TaxPaymentBankBranchDistrictName { get; set; }
    public string TaxPaymentAccCode { get; set; }
    public string TaxPaymentDocOrChallanNo { get; set; }
    public DateTime TaxPaymentDocOrChallanDate { get; set; }
    public DateTime TaxPaymentPaymentDate { get; set; }
    public decimal TaxPaymentPaidAmount { get; set; }
    public string TaxPaymentRemarks { get; set; }
}