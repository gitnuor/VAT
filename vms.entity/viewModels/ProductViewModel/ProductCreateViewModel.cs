using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.ProductViewModel;

public class ProductCreateViewModel
{

	[Display(Name = "Product ID")]
	public int ProductId { get; set; }

	[Display(Name = "Name")]
	public string Name { get; set; }

	[Display(Name = "Brand")]
	public int? BrandId { get; set; }

	[Display(Name = "Product Number")]
	public string ProductNumber { get; set; }

	[Display(Name = "Model Number")]
	public string ModelNo { get; set; }

	[Display(Name = "Device Model")]
	public string DeviceModel { get; set; }

	[Display(Name = "Code")]
	public string Code { get; set; }

	[Display(Name = "Part Number")]
	public string PartNo { get; set; }

	[Display(Name = "Part Code")]
	public string PartCode { get; set; }

	[Display(Name = "Variant")]
	public string Variant { get; set; }

	[Display(Name = "Color")]
	public string Color { get; set; }

	[Display(Name = "Size")]
	public string Size { get; set; }

	[Display(Name = "Weight")]
	public string Weight { get; set; }

	[Display(Name = "Weight in Kg")]
	public decimal? WeightInKg { get; set; }

	[Display(Name = "Specification")]
	public string Specification { get; set; }

	[Display(Name = "HS Code")]
	public string Hscode { get; set; }

	[Display(Name = "Product Category")]
	public int? ProductCategoryId { get; set; }

	[Display(Name = "Product Group")]
	public int ProductGroupId { get; set; }

	[Display(Name = "Product Type")]
	public int ProductTypeId { get; set; }

	[Display(Name = "Organization")]
	public int OrganizationId { get; set; }

	[Display(Name = "Total Quantity")]
	public decimal TotalQuantity { get; set; }

	[Display(Name = "Measurement Unit")]
	public int MeasurementUnitId { get; set; }

	[Display(Name = "Description")]
	public string Description { get; set; }
}