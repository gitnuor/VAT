using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmPurchasePost
{
    public string PurchaseId { get; set; }
    public int OrganizationId { get; set; }
    public string TransferSalesId { get; set; }
    public string TransferBranchId { get; set; }
    public string VendorId { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string PurchaseTypeId { get; set; }
    public string PurchaseReasonId { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal? AdvanceTaxPaidAmount { get; set; }
    public DateTime? Atpdate { get; set; }
    public string AtpbankBranchId { get; set; }
    public string AtpnbrEconomicCodeId { get; set; }
    public string AtpchallanNo { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? Vdsamount { get; set; }
    public bool? IsVdscertificatePrinted { get; set; }
    public string VdscertificateNo { get; set; }
    public DateTime? VdscertificateDate { get; set; }
    public string VdspaymentBookTransferNo { get; set; }
    public string Vdsnote { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public string CustomsAndVatcommissionarateId { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string PoNumber { get; set; }
    public string SecurityToken { get; set; }
    public bool IsComplete { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public int? ApiCreatedBy { get; set; }
    public DateTime? ApiCreatedTime { get; set; }
    public IList<vmPurchaseDetailPost> PurchaseDetails { get; set; }
    public IList<vmPurchasePaymentPost> PurchasePayments { get; set; }
}