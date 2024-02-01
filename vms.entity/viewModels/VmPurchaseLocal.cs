using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.Utility;

namespace vms.entity.viewModels;

public class VmPurchaseLocal
{
    public VmPurchaseLocal()
    {
        ProductList = new List<SpGetProductForPurchase>();
        Products = new List<VmPurchaseLocalProduct>();
        Documents = new List<VmPurchaseLocalDocument>();
        Payments = new List<VmPurchaseLocalPayment>();
        ProductVatTypes = new List<ProductVattype>();
    }


    #region Purchase Main

    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    [DisplayName("Purchase")]
    public int PurchaseId { get; set; }
    [DisplayName("Supplier/Vendor")]
    [Required(ErrorMessage = "Supplier/Vendor is Required")]
    public int VendorId { get; set; }
    [DisplayName("Supplier/Vendor Invoice No")]
    [MaxLength(35)]
    public string VendorInvoiceNo { get; set; }
    [DisplayName("Invoice No")]
    [MaxLength(35)]
    public string InvoiceNo { get; set; }
    [DisplayName("Voucher No")]
    [MaxLength(35)]
    public string VoucherNo { get; set; }
    [DisplayName("Purchase Date")]
    public DateTime PurchaseDate { get; set; }
    [DisplayName("Purchase Type")]
    public int PurchaseTypeId { get; set; }
    [DisplayName("Purchase Reason")]
    [Required(ErrorMessage = "Purchase Reason")]
    public int PurchaseReasonId { get; set; }
    [DisplayName("Vat Challan No")]
    [MaxLength(35)]
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

    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> VendorSelectList { get; set; }
    public IEnumerable<SelectListItem> PurchaseTypeSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> PurchaseReasonSelectList { get; set; }
    public IEnumerable<ProductVattype> ProductVatTypes { get; set; }

    #endregion


    #region Purchase Product

    [DisplayName("Purchase Detail")]
    public int PurchaseDetailId { get; set; }

    [DisplayName("Product")]
    [Required(ErrorMessage = "Product is Required")]
    public int ProductId { get; set; }

    [DisplayName("Description")]
    [MaxLength(500)]
    public string ProductDescription { get; set; }

    [DisplayName("HS Code")]
    [MaxLength(50)]
    public string HSCode { get; set; }

    [DisplayName("Part No.")]
    [MaxLength(50)]
    public string PartNo { get; set; }

    [DisplayName("SKU")]
    [MaxLength(50)]
    public string SKUNo { get; set; }

    [DisplayName("SKU ID")]
    [MaxLength(50)]
    public string SKUId { get; set; }

    [DisplayName("Goods ID")]
    [MaxLength(50)]
    public string GoodsId { get; set; }

    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    public string MeasurementUnitName { get; set; }

    [DisplayName("Cur. Stock")]
    public decimal CurrentStock { get; set; }

    [DisplayName("Quantity")]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal Quantity { get; set; }

    [DisplayName("Unit Price")]
    [Required(ErrorMessage = "Unit Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.01, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal UnitPrice { get; set; }

    [DisplayName("Total Price")]
    [Required(ErrorMessage = "Total Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalPrice { get; set; }

    [DisplayName("Disc./Item")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal DiscountPerItem { get; set; }

    [DisplayName("Prod. Disc.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalProductDiscount { get; set; }

    [DisplayName("Price (-Disc.)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.01, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithDiscount { get; set; }

    [DisplayName("SD (%)")]
    [Required(ErrorMessage = "SD(%) is Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal SupplementaryDutyPercent { get; set; }

    [DisplayName("SD")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Required(ErrorMessage = "SD is required")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductSupplementaryDuty { get; set; }

    [DisplayName("Price for VAT")]
    [Required(ErrorMessage = "VAT-able Price is required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal VatAblePrice { get; set; }

    [DisplayName("VAT Type")]
    [Required(ErrorMessage = "Product Vat Type is Required")]
    public int ProductVatTypeId { get; set; }

    [DisplayName("VAT (%)")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVatPercent { get; set; }

    [DisplayName("Total VAT")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVat { get; set; }

    [DisplayName("Price (+SD+VAT)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithVat { get; set; }

    [DisplayName("Price (+SD+VAT-Disc.)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithVatAfterDiscount { get; set; }

    public IEnumerable<SpGetProductForPurchase> ProductList { get; set; }

    #endregion


    #region Purchase Local Document

    [DisplayName("Document Type")]
    [Required]
    public int DocumentTypeId { get; set; }
    [DisplayName("File")]
    [Required]
    [MaxFileSize(1024 * 1024)]
    [AllowedExtensions(new[] { ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv" })]
    public IFormFile UploadedFile { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string DocumentRemarks { get; set; }
    public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }

    #endregion


    #region Purchase Local Payment

    [DisplayName("Purchase Payment")]
    public int PurchasePaymentId { get; set; }

    [DisplayName("Payment Method")]
    [Required(ErrorMessage = "Payment Method is Required")]
    public int PaymentMethodId { get; set; }

    public bool IsBankingChannel { get; set; }
    public bool IsMobileTransaction { get; set; }
        
    [DisplayName("Bank")]
    public int? BankId { get; set; }
        
    [DisplayName("Wallet No")]
    [MaxLength(20)]
    public string MobilePaymentWalletNo { get; set; }

    [Required(ErrorMessage = "Paid Amount is Required")]
    [DisplayName("Paid Amount")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal PaidAmount { get; set; }
    [Required(ErrorMessage = "Payment Date is Required")]
    [DisplayName("Payment Date")]
    public DateTime PaymentDate { get; set; }
    [DisplayName("Document No./TransactionId")]
    [MaxLength(50)]
    public string DocumentNoOrTransactionId { get; set; }
    [DisplayName("Doc./Trans. Date")]
    public DateTime PaymentDocumentOrTransDate { get; set; }
    [DisplayName("Remarks")]
    [MaxLength(50)]
    public string PaymentRemarks { get; set; }
    public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }
    public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }

    #endregion

    #region Misc

    public bool IsRequireSkuNo { get; set; }
    public bool IsRequireSkuId { get; set; }
    public bool IsRequireGoodsId { get; set; }
    public bool IsRequirePartNo { get; set; }

    #endregion
}