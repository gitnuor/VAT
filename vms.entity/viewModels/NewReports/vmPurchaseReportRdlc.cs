using System;

namespace vms.entity.viewModels.NewReports;

public class vmPurchaseReportRdlc
{
    public int PurchaseId { get; set; }
    public int OrganizationId { get; set; }
    public int? OrgBranchId { get; set; }
    public int PurchaseSlNo { get; set; }
    public int? TransferSalesId { get; set; }
    public int? TransferBranchId { get; set; }
    public int? VendorId { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public string VoucherNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseTypeId { get; set; }
    public int PurchaseReasonId { get; set; }
    public int NoOfIteams { get; set; }
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
    public bool? IsVdsamountPaid { get; set; }
    public bool? IsVdscertificatePrinted { get; set; }
    public string VdscertificateNo { get; set; }
    public DateTime? VdscertificateDate { get; set; }
    public string VdspaymentBookTransferNo { get; set; }
    public string Vdsnote { get; set; }
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
    public long? ApiTransactionId { get; set; }
    public string OrgName { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public string PurchaseTypeName { get; set; }
    public string VendorName { get; set; }
}