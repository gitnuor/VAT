using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesLocal;

public class SalesCombinedDetailPostWithSalesDto
{
	[Required(ErrorMessage = "{0} is Required")]
	public string SalesDetailId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Product")]
	public string ProductId { get; set; }

	[DisplayName("Description")]
	[MaxLength(500)]
	public string ProductDescription { get; set; }

	[DisplayName("HS Code")]
	[MaxLength(20)]
	public string Hscode { get; set; }

	[DisplayName("Product Code")]
	[MaxLength(50)]
	public string ProductCode { get; set; }

	[DisplayName("Part No.")]
	[MaxLength(50)]
	public string PartNo { get; set; }

	[DisplayName("SKU")] [MaxLength(50)] public string SKUNo { get; set; }

	[DisplayName("SKU ID")]
	[MaxLength(50)]
	public string SKUId { get; set; }

	[DisplayName("Goods ID")]
	[MaxLength(50)]
	public string GoodsId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("VAT Type")]
	public string ProductVatTypeId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Quantity")]
	public decimal Quantity { get; set; }

	[Required]
	[MaxLength(100)]
	[Display(Name = "Measurement Unit")]
	public string MeasurementUnitName { get; set; }

	[DisplayName("Unit Price")]
	[Required(ErrorMessage = "{0} is Required")]
	[Range(0.00000001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal UnitPrice { get; set; }

	[DisplayName("Service Charge (%)")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal? ServiceChargePercent { get; set; }

	[DisplayName("Disc./Item")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal? DiscountPerItem { get; set; }

	[DisplayName("VAT (%)")]
	[Range(0, 50000, ErrorMessage = "{1} to {2} value is allowed.")]
	public decimal ProductVatPercent { get; set; }

	[DisplayName("SD (%)")]
	[Required(ErrorMessage = "SD (%) is Required")]
	public decimal? SupplementaryDutyPercent { get; set; }
}