using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.Utility;

namespace vms.entity.Dto.PurchaseLocal;

public class PurchaseLocalPostDto
{
    [DisplayName("Token")] [Required] public string Token { get; set; }

    [DisplayName("Purchase")] public string PurchaseId { get; set; }

    [DisplayName("Branch")] [Required] public string BranchId { get; set; }

    [DisplayName("Purchase Reason")]
    [Required(ErrorMessage = "Purchase Reason")]
    public int PurchaseReasonId { get; set; }

    [DisplayName("Vendor ID")]
    [Required(ErrorMessage = "Vendor is Required")]
    public string VendorId { get; set; }

    [DisplayName("Vendor Name")] public string VendorName { get; set; }
    [DisplayName("Vendor BIN")] public string VendorBin { get; set; }
    [DisplayName("Vendor NID")] public string VendorNid { get; set; }
    [DisplayName("Vendor Address")] public string VendorAddress { get; set; }
    [DisplayName("Vendor Mobile No")] public string VendorContactNo { get; set; }

    [DisplayName("Vendor Invoice No")] public string VendorInvoiceNo { get; set; }

    [DisplayName("Vendor Invoice Date")] public DateTime VendorInvoiceDate { get; set; }

    [DisplayName("Invoice No")]
    [MaxLength(50)]
    public string InvoiceNo { get; set; }

    [Display(Name = "Invoice Date")] public DateTime? InvoiceDate { get; set; }

    [DisplayName("Voucher No")] public string VoucherNo { get; set; }

    [Display(Name = "Voucher Date")] public DateTime? VoucherDate { get; set; }

    [DisplayName("Purchase Date")] public DateTime PurchaseDate { get; set; }

    [DisplayName("Vat Challan No")] public string VatChallanNo { get; set; }
    [DisplayName("VAT Challan Time")] public DateTime? VatChallanIssueDate { get; set; }
    [DisplayName("Is VDS?")] public bool IsVatDeductedInSource { get; set; }

    [DisplayName("VDS Amount")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$",
        ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal? VdsAmount { get; set; }

    public string CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }

    public IEnumerable<PurchaseLocalDetailPostWithPurchaseDto> Details { get; set; } =
        new List<PurchaseLocalDetailPostWithPurchaseDto>();

    public IEnumerable<DocumentPostWithObjectDto> Documents { get; set; } = new List<DocumentPostWithObjectDto>();

    public IEnumerable<PurchaseLocalPaymentPostWithPurchaseDto> Payments { get; set; } =
        new List<PurchaseLocalPaymentPostWithPurchaseDto>();
}