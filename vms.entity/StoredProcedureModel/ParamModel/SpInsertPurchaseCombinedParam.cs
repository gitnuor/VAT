using System;
using System.Collections.Generic;
using vms.entity.viewModels.VmPurchaseCombineParamsModels;

namespace vms.entity.StoredProcedureModel.ParamModel;

public class SpInsertPurchaseCombinedParam
{
    public SpInsertPurchaseCombinedParam()
    {
        PurchaseDetailList = new List<PurchaseDetailCombinedParamModel>();
        ContentInfoList = new List<PurchaseContentInfoCombinedParamModel>();
        PurchasePayments = new List<PurchasePaymentCombinedParamModel>();
        ImportTaxPayments = new List<PurchaseImportTaxPaymentCombinedParamModel>();
    }
    public List<PurchaseDetailCombinedParamModel> PurchaseDetailList { get; set; }
    public List<PurchaseContentInfoCombinedParamModel> ContentInfoList { get; set; }
    public List<PurchasePaymentCombinedParamModel> PurchasePayments { get; set; }
    public List<PurchaseImportTaxPaymentCombinedParamModel> ImportTaxPayments { get; set; }
    public int OrganizationId { get; set; }
    public int OrgBranchId { get; set; }
    public int? VendorId { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public string VoucherNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseTypeId { get; set; }
    public int PurchaseReasonId { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal TotalDiscountOnIndividualProduct { get; set; }
    public decimal TotalImportDuty { get; set; }
    public decimal TotalRegulatoryDuty { get; set; }
    public decimal TotalSupplementaryDuty { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalAdvanceTax { get; set; }
    public decimal TotalAdvanceIncomeTax { get; set; }
    public decimal? AdvanceTaxPaidAmount { get; set; }
    public DateTime? Atpdate { get; set; }
    public int? AtpbankId { get; set; }
    public string AtpbankBranchName { get; set; }
    public int? AtpnbrEconomicCodeId { get; set; }
    public string AtpchallanNo { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? Vdsamount { get; set; }
    public bool? IsVdscertificatePrinted { get; set; }
    public string VdscertificateNo { get; set; }
    public DateTime? VdscertificateDate { get; set; }
    public string VdspaymentBookTransferNo { get; set; }
    public string Vdsnote { get; set; }
    public bool IsTds { get; set; }
    public decimal? TdsAmount { get; set; }
	public decimal? PayableAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public int? CustomsAndVatcommissionarateId { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string PoNumber { get; set; }
    public int? MushakGenerationId { get; set; }
    public string ReferenceKey { get; set; }
    public bool IsComplete { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
}