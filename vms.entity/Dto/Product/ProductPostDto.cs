using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.Dto.Product;

public class ProductPostDto
{
	[Display(Name = "Product ID")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(100)]
	public string Id { get; set; }

	[Display(Name = "Product Name")]
	[Required(ErrorMessage = "{0} is required")]
	[MaxLength(200)]
	public string Name { get; set; }

	[Display(Name = "Brand Name")]
	[MaxLength(100)]
	public string BrandName { get; set; }

	[Display(Name = "Product Number")]
	[MaxLength(50)]
	public string ProductNumber { get; set; }

	[Display(Name = "Model No.")]
	[MaxLength(50)]
	public string ModelNo { get; set; }

	[MaxLength(50)] public string DeviceModel { get; set; }

	[MaxLength(50)] public string Code { get; set; }

	[MaxLength(100)] public string PartNo { get; set; }

	[MaxLength(100)] public string PartCode { get; set; }

	[MaxLength(100)] public string Variant { get; set; }

	[MaxLength(100)] public string Color { get; set; }

	[MaxLength(100)] public string Size { get; set; }

	[MaxLength(100)] public string Weight { get; set; }

	[MaxLength(100)] public string Specification { get; set; }

	[MaxLength(50)] public string Hscode { get; set; }

	[MaxLength(100)] public string ProductCategoryName { get; set; }

	[MaxLength(100)] public string ProductGroupName { get; set; }

	[Required(ErrorMessage = "{0} is required")]
	[Display(Name = "Product Type ID")]
	public int ProductTypeId { get; set; }

	[MaxLength(100)]
	[Required(ErrorMessage = "{0} is required")]
	[Display(Name = "Measurement Unit")]
	public string MeasurementUnitName { get; set; }

	[MaxLength(500)] public string Description { get; set; }
}