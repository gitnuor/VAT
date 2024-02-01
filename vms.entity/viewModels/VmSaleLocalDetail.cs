using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels;

public class VmSaleLocalDetail
{
    public int SalesDetailId { get; set; }
    public int SalesId { get; set; }
    [Required]
    [DisplayName("Product")]
    public int ProductId { get; set; }

    [DisplayName("Description")]
    [MaxLength(500)]
    public string ProductDescription { get; set; }

    [DisplayName("HS Code")]
    public string Hscode { get; set; }

    [DisplayName("Product Code")]
    [MaxLength(50)]
    public string ProductCode { get; set; }

	[DisplayName("SKU")]
    [MaxLength(50)]
    public string SKUNo { get; set; }

    [DisplayName("SKU ID")]
    [MaxLength(50)]
    public string SKUId { get; set; }

    [DisplayName("Goods ID")]
    [MaxLength(50)]
    public string GoodsId { get; set; }
	[Required(ErrorMessage = "VAT Type is Required")]
    [DisplayName("VAT Type")]
    public int ProductVatTypeId { get; set; }
    public long? ProductTransactionBookId { get; set; }
    [Required(ErrorMessage = "Quantity is Required")]
    [DisplayName("Quantity")]
    public decimal Quantity { get; set; }
    [Required(ErrorMessage ="Current Stock is Required")]
    [DisplayName("Cur. Stock")]
    public decimal CurrentStock { get; set; }

    [DisplayName("Unit Price")]
    public decimal UnitPrice { get; set; }
    public decimal ServiceChargePercent { get; set; }

    [DisplayName("Disc./Item")]
    public decimal DiscountPerItem { get; set; }
    [DisplayName("VAT (%)")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    [Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
    public decimal ProductVatPercent { get; set; }
    [DisplayName("SD (%)")]
    [Required(ErrorMessage = "SD (%) is Required")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
    public decimal SupplementaryDutyPercent { get; set; }
    [Required(ErrorMessage = "M. Unit is Required")]
    [DisplayName("M. Unit")]
    public int MeasurementUnitId { get; set; }
    [DisplayName("M. Unit Name")]
    public string MeasurementUnitName { get; set; }
    [Required(ErrorMessage = "Rate is Required")]
    [DisplayName("Rate")]
    public string Rate { get; set; }
    [DisplayName("Total Price")]
    [Required(ErrorMessage = "Total Price is Required")]
    [RegularExpression(@"^\d+(\.\d{1,4})?$", ErrorMessage = "Maximum 4 digit after decimal is allowed.")]
    public decimal TotalPrice { get; set; }
    [Required(ErrorMessage ="VAT is Required")]
    [DisplayName("VAT")]
    public double Vat { get; set; }
    [Required(ErrorMessage ="Total VAT is Required")]
    [DisplayName("Total VAT")]
    public double TotalVat { get; set; }

}