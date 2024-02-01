using System;

namespace vms.entity.viewModels;

public class DatauploadPurshase
{
    public string PurchaseID { get; set; }
    public string OrganizationId { get; set; }
    public string VendorId { get; set; }
    public string VatChallanNo { get; set; }
    public DateTime VatChallanIssueDate { get; set; }
    public string VendorInvoiceNo { get; set; }
    public string InvoiceNo { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string PurchaseTypeId { get; set; }
    public string PurchaseReasonId { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal AdvanceTaxPaidAmount { get; set; }
    public DateTime ATPDate { get; set; }
    public string ATPBankBranchId { get; set; }
    public string ATPNbrEconomicCodeId { get; set; }
    public string ATPChallanNo { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal VDSAmount { get; set; }
    public bool IsVDSCertificatePrinted { get; set; }
    public string VDSCertificateNo { get; set; }
    public DateTime VDSCertificateDate { get; set; }
    public string VDSPaymentBookTransferNo { get; set; }
    public string VDSNote { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string LcNo { get; set; }
    public DateTime LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime BillOfEntryDate { get; set; }
    public string CustomsAndVATCommissionarateId { get; set; }
    public DateTime DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string PoNumber { get; set; }
    public bool IsComplete { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }

}