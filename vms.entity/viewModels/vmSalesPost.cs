using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmSalesPost
{
    public string SalesId { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public int OrganizationId { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? Vdsamount { get; set; }
    public bool? IsVdscertificateReceived { get; set; }
    public string VdscertificateNo { get; set; }
    public DateTime? VdscertificateIssueTime { get; set; }
    public int? VdspaymentBankBranchId { get; set; }
    public DateTime? VdspaymentDate { get; set; }
    public string VdspaymentChallanNo { get; set; }
    public string VdspaymentEconomicCode { get; set; }
    public string VdspaymentBookTransferNo { get; set; }
    public string Vdsnote { get; set; }
    public decimal? ReceivableAmount { get; set; }
    public decimal? PaymentReceiveAmount { get; set; }
    public decimal? PaymentDueAmount { get; set; }
    public string CustomerId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverContactNo { get; set; }
    public string ShippingAddress { get; set; }
    public string ShippingCountryId { get; set; }
    public string SalesTypeId { get; set; }
    public string SalesDeliveryTypeId { get; set; }
    public string WorkOrderNo { get; set; }
    public DateTime SalesDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string DeliveryMethodId { get; set; }
    public string ExportTypeId { get; set; }
    public string LcNo { get; set; }
    public DateTime? LcDate { get; set; }
    public string BillOfEntry { get; set; }
    public DateTime? BillOfEntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string TermsOfLc { get; set; }
    public string CustomerPoNumber { get; set; }
    public bool IsComplete { get; set; }
    public bool IsTaxInvoicePrined { get; set; }
    public DateTime? TaxInvoicePrintedTime { get; set; }
    public string OtherBranchOrganizationId { get; set; }
    public string TransferChallanNo { get; set; }
    public DateTime? TransferChallanPrintedTime { get; set; }
    public string ReferenceKey { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public long? ApiTransactionId { get; set; }
    public int? ApiCreatedBy { get; set; }
    public DateTime? ApiCreatedTime { get; set; }
    public string SecurityToken { get; set; }
    public IList<vmSalesDetailPost> SalesDetails { get; set; }
    public IList<vmSalesPaymentPost> SalesPaymentReceives { get; set; }
}