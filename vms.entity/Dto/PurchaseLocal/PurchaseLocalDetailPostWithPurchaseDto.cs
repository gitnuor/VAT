using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.PurchaseLocal;

public class PurchaseLocalDetailPostWithPurchaseDto
{
	[Required]
	[DisplayName("Purchase Detail")]
	public string PurchaseDetailId { get; set; }

	[DisplayName("Product")]
	[Required(ErrorMessage = "Product is Required")]
	public string ProductId { get; set; }

	[DisplayName("Description")]
	[MaxLength(500)]
	public string ProductDescription { get; set; }

	[DisplayName("HS Code")]
	[MaxLength(50)]
	public string HSCode { get; set; }

	[DisplayName("Product Code")]
	[MaxLength(50)]
	public string ProductCode { get; set; }

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

	[Required]
	[MaxLength(100)]
	[Display(Name = "Measurement Unit")]
	public string MeasurementUnitName { get; set; }

	[DisplayName("Quantity")]
	[Required(ErrorMessage = "Quantity is Required")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	public decimal Quantity { get; set; }

	[DisplayName("Unit Price")]
	[Required(ErrorMessage = "{0} is Required")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	[Range(0.01, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal UnitPrice { get; set; }

	[DisplayName("Disc./Item")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Only positive number and maximum 8 digit after decimal is allowed.")]
	[Range(0, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal? DiscountPerItem { get; set; }

	[DisplayName("SD(%)")]
	[Required(ErrorMessage = "SD(%) is Required")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal? SupplementaryDutyPercent { get; set; }

	[DisplayName("VAT Type")]
	[Required(ErrorMessage = "Product Vat Type is Required")]
	public int ProductVatTypeId { get; set; }

	[DisplayName("VAT(%)")]
	[RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Only positive number and maximum 2 digit after decimal is allowed.")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal ProductVatPercent { get; set; }
}