using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.Utility;

namespace vms.entity.viewModels;

public class VmPurchaseLocalPost
{
    public VmPurchaseLocalPost()
    {
        Products = new List<VmPurchaseLocalProduct>();
        Documents = new List<VmPurchaseLocalDocument>();
        Payments = new List<VmPurchaseLocalPayment>();
    }

    public int OrganizationId { get; set; }
    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    public int CreatedBy { get; set; }

    [DisplayName("Purchase")]
    public int PurchaseId { get; set; }

    [DisplayName("Purchase Type")]
    public int PurchaseTypeId { get; set; }

    [DisplayName("Purchase Reason")]
    [Required(ErrorMessage = "Purchase Reason")]
    public int PurchaseReasonId { get; set; }

    [DisplayName("Vendor")]
    [Required(ErrorMessage = "Vendor is Required")]
    public int VendorId { get; set; }

    [DisplayName("Vendor Invoice No")]
    public string VendorInvoiceNo { get; set; }

    [DisplayName("Invoice No")]
    public string InvoiceNo { get; set; }

    [DisplayName("Voucher No")]
    public string VoucherNo { get; set; }

    [DisplayName("Purchase Date")]
    public DateTime PurchaseDate { get; set; }

    [DisplayName("Vat Challan No")]
    public string VatChallanNo { get; set; }
    [DisplayName("VAT Challan Time")]
    public DateTime? VatChallanIssueDate { get; set; }
    [DisplayName("Is VDS?")]

    public bool IsVatDeductedInSource { get; set; }
    [DisplayName("VDS Amount")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal? VdsAmount { get; set; }
    [DisplayName("Is TDS?")]
    public bool IsTds { get; set; }
    [DisplayName("TDS Amount")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal? TdsAmount { get; set; }

	public IEnumerable<VmPurchaseLocalProduct> Products { get; set; }
    public IEnumerable<VmPurchaseLocalDocument> Documents { get; set; }
    public IEnumerable<VmPurchaseLocalPayment> Payments { get; set; }
}