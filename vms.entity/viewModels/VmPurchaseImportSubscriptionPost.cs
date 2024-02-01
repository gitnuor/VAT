using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmPurchaseImportSubscriptionPost
{
    public VmPurchaseImportSubscriptionPost()
    {
        Products = new List<VmPurchaseImportDetail>();
        //Payments = new List<VmPurchaseImportPayment>();
        //ImportTaxPayments = new List<VmPurchaseImportTaxPayment>();
        //Documents = new List<VmPurchaseImportDocument>();
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

    [DisplayName("Reason")]
    [Required(ErrorMessage = "Reason is Required")]
    public int PurchaseReasonId { get; set; }

    [DisplayName("Vendor")]
    [Required(ErrorMessage = "Vendor is Required")]
    public int VendorId { get; set; }

    [DisplayName("V.Inv. No")]
    [Required(ErrorMessage = "V.Inv. No.")]
    public string VendorInvoiceNo { get; set; }

    [DisplayName("Invoice No")]
    public string InvoiceNo { get; set; }

    [DisplayName("Voucher No")]
    public string VoucherNo { get; set; }

    [DisplayName("Purchase Date")]
    public DateTime PurchaseDate { get; set; }

    //[DisplayName("Lc No.")]
    //public string LcNo { get; set; }
    //[DisplayName("Lc Date")]
    //public DateTime LcDate { get; set; }
    //[DisplayName("Bill Of Entry")]
    //public string BillOfEntry { get; set; }
    //[DisplayName("BOE Date")]
    //public DateTime BillOfEntryDate { get; set; }
    //[DisplayName("Due Date")]
    //public DateTime? DueDate { get; set; }
    //[DisplayName("Terms Of Lc")]
    //public string TermsOfLc { get; set; }
    //[DisplayName("P/O No.")]
    //public string PoNumber { get; set; }

    public IEnumerable<VmPurchaseImportDetail> Products { get; set; }
    //public IEnumerable<VmPurchaseImportDocument> Documents { get; set; }
    //public IEnumerable<VmPurchaseImportTaxPayment> ImportTaxPayments { get; set; }
    //public IEnumerable<VmPurchaseImportPayment> Payments { get; set; }
}