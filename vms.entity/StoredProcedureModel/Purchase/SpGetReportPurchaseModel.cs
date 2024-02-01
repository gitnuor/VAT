using System;

namespace vms.entity.StoredProcedureModel.Purchase;

public class SpGetReportPurchaseModel
{
    public int SlNo { get; set; }
    public int PurchaseId { get; set; }
    public string PoNumber { get; set; }
    public string InvoiceNo { get; set; }
    public string VoucherNo { get; set; }
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; }
    public string OrganizationAddress { get; set; }
    public string OrganizationBin { get; set; }
    public int? OrgBranchId { get; set; }
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string ReceiveAddress { get; set; }
    public int? VendorId { get; set; }
    public string VendorName { get; set; }
    public string VendorBinOrNid { get; set; }
    public string VendorAddress { get; set; }
    public int PurchaseSlNo { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime? VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int PurchaseTypeId { get; set; }
    public string PurchaseTypeName { get; set; }
    public int PurchaseReasonId { get; set; }
    public string PurchaseReasonName { get; set; }
    public int NoOfIteams { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal TotalDiscountOnIndividualProduct { get; set; }
    public decimal TotalCustomDuty { get; set; }
    public decimal TotalImportDuty { get; set; }
    public decimal TotalRegulatoryDuty { get; set; }
    public decimal TotalSupplementaryDuty { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalAdvanceTax { get; set; }
    public decimal TotalAdvanceIncomeTax { get; set; }
    public bool IsVds { get; set; }
    public DateTime? VdsDate { get; set; }
    public string IsVdsStatus { get; set; }
    public bool? IsTds { get; set; }
    public string IsTdsStatus { get; set; }
    public decimal? TdsAmount { get; set; }
    public decimal? VdsAmount { get; set; }
    public bool? IsVdsAmountPaid { get; set; }
    public bool? IsVdsCertificatePrinted { get; set; }
    public string IsVdsCertificatePrintedStatus { get; set; }
    public string VdsCertificateNo { get; set; }
    public DateTime? VdsCertificateDate { get; set; }
    public string VdsPaymentBookTransferNo { get; set; }
    public string VdsNote { get; set; }
    public decimal? PayableAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string LcaNumber { get; set; }
    public string LcafNumber { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public string PurchaseRemarks { get; set; }
}