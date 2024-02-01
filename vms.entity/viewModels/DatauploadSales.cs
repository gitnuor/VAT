using System;

namespace vms.entity.viewModels;

public class DatauploadSales
{
    public string SalesID { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public string OrganizationId { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal VDSAmount { get; set; }
    public bool IsVDSCertificateReceived { get; set; }
    public string VDSCertificateNo { get; set; }
    public DateTime VDSCertificateIssueTime { get; set; }
    public string VDSPaymentBankBranchId { get; set; }
    public DateTime VDSPaymentDate { get; set; }
    public string VDSPaymentChallanNo { get; set; }
    public string VDSPaymentEconomicCode { get; set; }
    public string VDSPaymentBookTransferNo { get; set; }
    public string VDSNote { get; set; }
    public string CustomerId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverContactNo { get; set; }
    public string ShippingAddress { get; set; }
    public string ShippingCountryId { get; set; }
    public string SalesTypeId { get; set; }
    public string SalesDeliveryTypeId { get; set; }
    public string WorkOrderNo { get; set; }
    public DateTime SalesDate { get; set; }
    public DateTime ExpectedDeliveryDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string DeliveryMethodId { get; set; }
    public string ExportTypeId { get; set; }
    public string LcNo { get; set; }
    public DateTime LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime BillOfEntryDate { get; set; }
    public DateTime DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string CustomerPoNumber { get; set; }
    public bool IsComplete { get; set; }
    public bool IsTaxInvoicePrined { get; set; }
    public DateTime TaxInvoicePrintedTime { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
        

}