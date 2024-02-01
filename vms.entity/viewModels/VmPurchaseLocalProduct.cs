using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vms.entity.Utility;

namespace vms.entity.viewModels;

public class VmPurchaseLocalProduct
{
    [DisplayName("Purchase")]
    public int PurchaseId { get; set; }

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

    [DisplayName("Price(-Disc.)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.01, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithDiscount { get; set; }

    [DisplayName("SD(%)")]
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

    [DisplayName("VAT(%)")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVatPercent { get; set; }

    [DisplayName("Total VAT")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVat { get; set; }

    [DisplayName("Price(+SD+VAT)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithVat { get; set; }

    [DisplayName("Price(+SD+VAT-Disc.)")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Only positive number and maximum 4 digit after decimal is allowed.")]
    [Range(0.0001, ConstantValues.MaxAllowedDoubleValue, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductPriceWithVatAfterDiscount { get; set; }
}