using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.CustomDataAnnotations;
using vms.entity.models;
using vms.entity.StoredProcedureModel;
using vms.entity.Utility;

namespace vms.entity.viewModels;

public class VmPurchaseImportSubscription
{
    public VmPurchaseImportSubscription()
    {
        Products = new List<VmPurchaseImportDetail>();
        //Documents = new List<VmPurchaseImportDocument>();
        //Payments = new List<VmPurchaseImportPayment>();

        VendorSelectList = new List<CustomSelectListItem>();
        PurchaseTypeSelectList = new List<SelectListItem>();
        PurchaseReasonSelectList = new List<CustomSelectListItem>();
        ProductList = new List<SpGetProductForPurchase>();
        //VatCommissionarateList = new List<CustomSelectListItem>();
        //AtpBankList = new List<SelectListItem>();
        //EconomicCodeList = new List<SelectListItem>();
        MeasurementUnitSelectList = new List<CustomSelectListItem>();
        ProductVatTypes = new List<ProductVattype>();
        //DocumentTypeSelectList = new List<CustomSelectListItem>();
        //PaymentMethodList = new List<PaymentMethod>();
        //BankSelectList = new List<CustomSelectListItem>();
    }

    public IEnumerable<VmPurchaseImportDetail> Products { get; set; }
    //public IEnumerable<VmPurchaseImportDocument> Documents { get; set; }
    //public IEnumerable<VmPurchaseImportPayment> Payments { get; set; }

    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
    public IEnumerable<CustomSelectListItem> VendorSelectList { get; set; }
    public IEnumerable<SelectListItem> PurchaseTypeSelectList { get; set; }
    public IEnumerable<CustomSelectListItem> PurchaseReasonSelectList { get; set; }
    public IEnumerable<SpGetProductForPurchase> ProductList { get; set; }
    //public IEnumerable<CustomSelectListItem> VatCommissionarateList { get; set; }
    //public IEnumerable<SelectListItem> AtpBankList { get; set; }
    //public IEnumerable<SelectListItem> EconomicCodeList { get; set; }

    public IEnumerable<CustomSelectListItem> MeasurementUnitSelectList { get; set; }
    public IEnumerable<ProductVattype> ProductVatTypes { get; set; }

    #region Purchase Main

    [DisplayName("Branch")]
    [Required]
    public int OrgBranchId { get; set; }
    [DisplayName("Purchase")]
    public int PurchaseId { get; set; }
    [DisplayName("Supplier/Vendor")]
    [Required(ErrorMessage = "Vendor is Required")]
    public int VendorId { get; set; }
    [DisplayName("Reason")]
    [Required(ErrorMessage = "Reason is Required")]
    public int PurchaseReasonId { get; set; }
    //[DisplayName("CI No.")]
    //[Required]
    //[MaxLength(50)]
    //public string VendorInvoiceNo { get; set; }

    //[DisplayName("Invoice No.")]
    //[MaxLength(50)]
    //public string InvoiceNo { get; set; }
    [DisplayName("Voucher No.")]
    [MaxLength(50)]
    public string VoucherNo { get; set; }
    [DisplayName("Purchase Date")]
    public DateTime PurchaseDate { get; set; }

    //[DisplayName("LC No.")]
    //[MaxLength(50)]
    //[Required]
    //public string LcNo { get; set; }
    //[DisplayName("LC Date")]
    //[Required]
    //public DateTime LcDate { get; set; }
    //[DisplayName("Bill Of Entry (BOE)")]
    //[MaxLength(50)]
    //// [Required]
    //public string BillOfEntry { get; set; }
    //[DisplayName("BOE Date")]
    //public DateTime BillOfEntryDate { get; set; }
    //[DisplayName("Due Date")]
    //// [Required]
    //public DateTime? DueDate { get; set; }
    //[DisplayName("Terms Of LC")]
    //// [Required]
    //[MaxLength(250)]
    //public string TermsOfLc { get; set; }
    //[DisplayName("P/O No.")]
    //[MaxLength(50)]
    //public string PoNumber { get; set; }
       

    #endregion


    #region Purchase Import Detail

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
    [DisplayName("M. Unit")]
    public string MeasurementUnitName { get; set; }

    [DisplayName("Current Stock")]
    public decimal CurrentStock { get; set; }

    [DisplayName("Quantity")]
    [Required(ErrorMessage = "Quantity is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal Quantity { get; set; }
    [DisplayName("Unit Price")]
    [Required(ErrorMessage = "Unit Price is Required")]
    [Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal UnitPrice { get; set; }

    [DisplayName("Total Price (AV)")]
    [Required(ErrorMessage = "Total Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalPrice { get; set; }

    [DisplayName("CD (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    public decimal CustomDutyPercent { get; set; } 
    [DisplayName("CD")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal CustomDuty { get; set; }

    [DisplayName("ID (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    public decimal ImportDutyPercent { get; set; }
        
    [DisplayName("ID")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ImportDuty { get; set; }

    [DisplayName("RD (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    public decimal RegulatoryDutyPercent { get; set; }
    [DisplayName("RD")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal RegulatoryDuty { get; set; }

    [DisplayName("SD (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    public decimal SupplementaryDutyPercent { get; set; } 
    [DisplayName("SD")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    public decimal ProductSupplementaryDuty { get; set; }

    [DisplayName("Price for VAT/AT")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal VatAblePrice { get; set; }

    [DisplayName("VAT Type")]
    [Required(ErrorMessage = "{0} is Required")]
    public int ProductVatTypeId { get; set; }

    [DisplayName("VAT (%)")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVatPercent { get; set; }

    [DisplayName("Total VAT")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Required(ErrorMessage = "{0} is Required")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVat { get; set; }

    [DisplayName("Price With VAT")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithVat { get; set; }

    [DisplayName("AT (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal AdvanceTaxPercent { get; set; }

    [DisplayName("AT")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal AdvanceTax { get; set; }

    [DisplayName("AIT (%)")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal AdvanceIncomeTaxPercent { get; set; }
        
    [DisplayName("AIT")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal AdvanceIncomeTax { get; set; }

    [DisplayName("Price Inc. All Tax")]
    [Required(ErrorMessage = "{0} is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal TotalPriceIncludingAllTax { get; set; }

    #endregion


    //#region Purchase Import Document

    //[DisplayName("Document Type")]
    //[Required]
    //public int DocumentTypeId { get; set; }
    //[DisplayName("File")]
    //[Required]
    //[MaxFileSize(1024 * 1024)]
    //[AllowedExtensions(new[] { ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".csv" })]
    //public IFormFile UploadedFile { get; set; }
    //[DisplayName("Remarks")]
    //[MaxLength(50)]
    //public string DocumentRemarks { get; set; }
    //public IEnumerable<CustomSelectListItem> DocumentTypeSelectList { get; set; }

    //#endregion


    //#region Purchase Import Payment

    //[DisplayName("Purchase Payment")]
    //public int PurchasePaymentId { get; set; }

    //[DisplayName("Payment Method")]
    //[Required(ErrorMessage = "Payment Method is Required")]
    //public int PaymentMethodId { get; set; }

    //public bool IsBankingChannel { get; set; }
    //public bool IsMobileTransaction { get; set; }

    //[DisplayName("Bank")]
    //public int? BankId { get; set; }

    //[DisplayName("Wallet No")]
    //[MaxLength(20)]
    //public string MobilePaymentWalletNo { get; set; }

    //[Required(ErrorMessage = "Paid Amount is Required")]
    //[DisplayName("Paid Amount")]
    //[RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    //[Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    //public decimal PaidAmount { get; set; }
    //[Required(ErrorMessage = "Payment Date is Required")]
    //[DisplayName("Payment Date")]
    //public DateTime PaymentDate { get; set; }
    //[DisplayName("Document No./TransactionId")]
    //[MaxLength(50)]
    //public string DocumentNoOrTransactionId { get; set; }
    //[DisplayName("Doc./Trans. Date")]
    //public DateTime PaymentDocumentOrTransDate { get; set; }
    //[DisplayName("Remarks")]
    //[MaxLength(50)]
    //public string PaymentRemarks { get; set; }
    //public IEnumerable<CustomSelectListItem> BankSelectList { get; set; }
    //public IEnumerable<PaymentMethod> PaymentMethodList { get; set; }

    //#endregion


    //#region Purchase Import Tax Payment

    //[DisplayName("Tax Payment")]
    //[Required]
    //public int PurchaseImportTaxPaymentId { get; set; }

    //[Required]
    //[DisplayName("Tax Type")]
    //public int PurchaseImportTaxPaymentTypeId { get; set; }

    //[Required]
    //[DisplayName("Vat Commissionerate")]
    //public int PurchaseImportTaxPaymentVatCommissionarateId { get; set; }

    //[Required]
    //[DisplayName("Bank")]
    //public int PurchaseImportTaxPaymentBankId { get; set; }

    //[Required]
    //[DisplayName("Bank Branch")]
    //public string PurchaseImportTaxPaymentBankBranch { get; set; }

    //[DisplayName("Acc. Code")]
    //[Required]
    //public string PurchaseImportTaxPaymentAccCode { get; set; }

    //[Required]
    //[DisplayName("Bank Branch District")]
    //public int PurchaseImportTaxPaymentBankBranchDistrictId { get; set; }

    //[Required(ErrorMessage = "Amount is Required")]
    //[DisplayName("Amount")]
    //[RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    //[Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    //public decimal PurchaseImportTaxPaymentPaidAmount { get; set; }
    //[Required(ErrorMessage = "Payment Date is Required")]
    //[DisplayName("Payment Date")]
    //public DateTime PurchaseImportTaxPaymentDate { get; set; }
    //[DisplayName("Doc./Challan No.")]
    //[MaxLength(50)]
    //[Required]
    //public string PurchaseImportTaxPaymentDocOrChallanNo { get; set; }
    //[DisplayName("Doc./Challan Date")]
    //[Required]
    //public DateTime PurchaseImportTaxPaymentDocOrChallanDate { get; set; }
    //[DisplayName("Remarks")]
    //[MaxLength(200)]
    //public string PurchaseImportTaxPaymentRemarks { get; set; }
    //public IEnumerable<CustomSelectListItem> PurchaseImportTaxPaymentTypeList { get; set; }
    //public IEnumerable<CustomSelectListItem> DistrictList { get; set; }

    //#endregion

    //#region Misc

    //public bool IsRequireSkuNo { get; set; }
    //public bool IsRequireSkuId { get; set; }
    //public bool IsRequireGoodsId { get; set; }
    //public bool IsRequirePartNo { get; set; }

    //#endregion

}