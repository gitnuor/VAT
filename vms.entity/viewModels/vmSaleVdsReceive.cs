using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace vms.entity.viewModels;

public class vmSaleVdsReceive
{
    public int SalesId { get; set; }
    public string EncryptedId { get; set; }
    public string InvoiceNo { get; set; }
    public string VatChallanNo { get; set; }
    public int OrganizationId { get; set; }
    public int NoOfIteams { get; set; }
    public decimal TotalPriceWithoutVat { get; set; }
    public decimal DiscountOnTotalPrice { get; set; }
    public decimal TotalDiscountOnIndividualProduct { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalSupplimentaryDuty { get; set; }
    public bool IsVatDeductedInSource { get; set; }
    public decimal? Vdsamount { get; set; }
    public bool? IsVdscertificateReceived { get; set; }
    [Required(ErrorMessage = "Certificate No. is required")]
    [DisplayName("Certificate No.")]
    [StringLength(40, ErrorMessage = "Certificate No. can not be greater than 40 characters")]
    public string VdscertificateNo { get; set; }
    [Required(ErrorMessage = "Issue Time is required")]
    [Display(Name = "Issue Time")]
    public DateTime? VdscertificateIssueTime { get; set; }
    [Required(ErrorMessage = "Bank is required")]
    [Display(Name = "Bank")]
    public int? VdspaymentBankId { get; set; }
    [Required(ErrorMessage = "Bank Branch is required")]
    [Display(Name = "Bank Branch")]
    [StringLength(80, ErrorMessage = "Bank Branch Name can not be greater than 80 characters")]

    public string VdspaymentBankBranchName { get; set; }
    public DateTime? VdspaymentDate { get; set; }
    [Required(ErrorMessage = "Payment Challan No. is required")]
    [StringLength(15, ErrorMessage = "Payment Challan No. can not be greater than 15 characters")]
    public string VdspaymentChallanNo { get; set; }
    [Required(ErrorMessage = "Payment Economic Code is required")]
    [StringLength(15, ErrorMessage = "Payment Economic Code can not be greater than 15 characters")]

    public string VdspaymentEconomicCode { get; set; }
    [Required(ErrorMessage ="Book Transfer No. is required")]
    [StringLength(40, ErrorMessage = "Book Transfer No. can not be greater than 40 characters")]
    public string VdspaymentBookTransferNo { get; set; }
    [Required(ErrorMessage ="Note is required")]
    [StringLength(200,ErrorMessage ="Note can not be greater than 200 characters")]
    public string Vdsnote { get; set; }
    public decimal? ReceivableAmount { get; set; }
    public decimal? PaymentReceiveAmount { get; set; }
    public decimal? PaymentDueAmount { get; set; }
    public int? CustomerId { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverContactNo { get; set; }
    public string ShippingAddress { get; set; }
    public int? ShippingCountryId { get; set; }
    public int SalesTypeId { get; set; }
    public int SalesDeliveryTypeId { get; set; }
    public string WorkOrderNo { get; set; }
    public DateTime SalesDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? DeliveryMethodId { get; set; }
    public int? ExportTypeId { get; set; }
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
    public int? MushakGenerationId { get; set; }
    public int? OtherBranchOrganizationId { get; set; }
    public string TransferChallanNo { get; set; }
    public DateTime? TransferChallanPrintedTime { get; set; }
    public string ReferenceKey { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public long? ApiTransactionId { get; set; }
    [Required(ErrorMessage = "Please select a file")]
    public IFormFile File { get; set; }


    public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }


}