using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.SalesExport;

public class SalesExportDetailPostWithSalesDto
{
	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Sales Detail Id")]
	public string SalesDetailId { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Product")]
	public string ProductId { get; set; }

	[DisplayName("HS Code")]
	[MaxLength(20)]
	public string Hscode { get; set; }

	[DisplayName("Product Code")]
	[MaxLength(50)]
	public string ProductCode { get; set; }

	[DisplayName("Part No.")]
	[MaxLength(100)]
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

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Quantity")]
	[Range(0.0001, int.MaxValue, ErrorMessage = "{1} to {2} value is allowed.")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Maximum 8 digit after decimal is allowed.")]
	public decimal Quantity { get; set; }

	[DisplayName("Measurement Unit")][Required(ErrorMessage = "{0} is Required")] public string MeasurementUnitName { get; set; }

	[Required(ErrorMessage = "{0} is Required")]
	[DisplayName("Unit Price")]
	[RegularExpression(@"^\d+(\.\d{1,8})?$", ErrorMessage = "Maximum 8 digit after decimal is allowed.")]
	public string UnitPrice { get; set; }
}