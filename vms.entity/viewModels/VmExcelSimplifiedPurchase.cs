using System;
using System.ComponentModel.DataAnnotations;
/*
namespace vms.entity.viewModels
{
    public class VmExcelSimplifiedPurchase
    {
        public string PurchaseId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string InvoiceNo { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string PurchaseType { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorNid { get; set; }
        public string VendorTin { get; set; }
        public string VendorBin { get; set; }
        public string VendorAddress { get; set; }
        public string PurchaseDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string HsCode { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductType { get; set; }
        public string MeasurementUnitName { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPerItem { get; set; }
        public string VatType { get; set; }
        public decimal VatPercent { get; set; }
        public decimal? SupplementaryDutyPercent { get; set; }
        public bool? IsVds { get; set; }
        public string VdsAmount { get; set; }
        public bool? IsTds { get; set; }
        public decimal? TdsAmount { get; set; }
        public string PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentWalletNo { get; set; }
        public string ChequeBankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
*/
namespace vms.entity.viewModels;

public class VmExcelSimplifiedPurchase
{
    [MaxLength(100, ErrorMessage = "The 'PurchaseId' {1}   characters long.")]
    public string PurchaseId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    [MaxLength(50, ErrorMessage = "The 'InvoiceNo' {1}  characters long.")]
    public string InvoiceNo { get; set; }
    [MaxLength(50, ErrorMessage = "The 'InvoiceNo' {1}  characters long.")]
    public string ChallanNo { get; set; }
    public DateTime? ChallanDate { get; set; }
    [MaxLength(500, ErrorMessage = "The 'BranchId' {1}  characters long.")]
    public string BranchId { get; set; }
    [MaxLength(500, ErrorMessage = "The 'BranchName' {1}  characters long.")]
    public string BranchName { get; set; }
    [MaxLength(500, ErrorMessage = "The 'BranchAddress' {1}  characters long.")]
    public string BranchAddress { get; set; }
    [MaxLength(500, ErrorMessage = "The 'PurchaseType' {1}  characters long.")]
    public string PurchaseType { get; set; }
    [MaxLength(100, ErrorMessage = "The 'VendorId' {1}  characters long.")]
    public string VendorId { get; set; }
    [MaxLength(200, ErrorMessage = "The 'VendorName' {1}  characters long.")]
    public string VendorName { get; set; }
    [MaxLength(50, ErrorMessage = "The ''VendorNid'' {1}  characters long.")]
    public string VendorNid { get; set; }
    [MaxLength(50, ErrorMessage = "The 'VendorTin' {1}  characters long.")]
    public string VendorTin { get; set; }
    [MaxLength(50, ErrorMessage = "The 'VendorBin' {1}  characters long.")]
    public string VendorBin { get; set; }
    [MaxLength(500, ErrorMessage = "The 'VendorAddress' {1}  characters long.")]
    public string VendorAddress { get; set; }
    [MaxLength(100, ErrorMessage = "The 'PurchaseDetailId' {1}  characters long.")]
    public string PurchaseDetailId { get; set; }
    [MaxLength(100, ErrorMessage = "The 'ProductId' {1}  characters long.")]
    public string ProductId { get; set; }
    [MaxLength(200, ErrorMessage = "The 'ProductName' {1}  characters long.")]
    public string ProductName { get; set; }
    [MaxLength(50, ErrorMessage = "The 'HsCode' {1}  characters long.")]
    public string HsCode { get; set; }
    [MaxLength(200, ErrorMessage = "The 'ProductGroupName' {1}  characters long.")]
    public string ProductGroupName { get; set; }
    [MaxLength(200, ErrorMessage = "The 'ProductType' {1}  characters long.")]
    public string ProductType { get; set; }
    [MaxLength(50, ErrorMessage = "The 'MeasurementUnitName' {1}  characters long.")]
    public string MeasurementUnitName { get; set; }
    [Range(0.01, 9999999999999999.99, ErrorMessage = "Quantity must be greater than 0.00")]
    public decimal Quantity { get; set; }
    [Range(0.01, 9999999999999999.99, ErrorMessage = "UnitPrice must be greater than 0.00")]
    public decimal UnitPrice { get; set; }
    public decimal? DiscountPerItem { get; set; }
    [MaxLength(500, ErrorMessage = "The 'VatType' {1}  characters long.")]
    public string VatType { get; set; }
    [Range(0.01, 9999999999999999.99, ErrorMessage = "VATPercent must be greater than 0.00")]
    public decimal VatPercent { get; set; }
    public decimal? SupplementaryDutyPercent { get; set; }
    public bool? IsVds { get; set; }
    [MaxLength(500, ErrorMessage = "The 'VdsAmount' {1}  characters long.")]
    public string VdsAmount { get; set; }
    public bool? IsTds { get; set; }
    [Range(0.00, 9999999999999999.99, ErrorMessage = "TdsAmount must be greater than 0.00")]
    public decimal TdsAmount { get; set; }
    [MaxLength(100, ErrorMessage = "The 'PaymentId' {1}  characters long.")]
    public string PaymentId { get; set; }
    [MaxLength(50, ErrorMessage = "The 'PaymentMethod' {1}  characters long.")]
    public string PaymentMethod { get; set; }
    [MaxLength(50, ErrorMessage = "The 'PaymentTransactionId' {1}  characters long.")]
    public string PaymentTransactionId { get; set; }
    [MaxLength(20, ErrorMessage = "The 'PaymentWalletNo' {1}  characters long.")]
    public string PaymentWalletNo { get; set; }
    [MaxLength(250, ErrorMessage = "The 'ChequeBankName' {1}  characters long.")]
    public string ChequeBankName { get; set; }
    [MaxLength(50, ErrorMessage = "The 'ChequeNo' {1}  characters long.")]
    public string ChequeNo { get; set; }
    public DateTime? ChequeDate { get; set; }
    public DateTime? PaymentDate { get; set; }
    public decimal? PaymentAmount { get; set; }
}