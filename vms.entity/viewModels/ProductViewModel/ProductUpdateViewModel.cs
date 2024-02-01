using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.ProductViewModel;

public class ProductUpdateViewModel
{

	[Display(Name = "Product ID")]
	public int ProductId { get; set; }

	[Display(Name = "Product/Service Name")]
	[StringLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	[Required]
	public string Name { get; set; }

	[Display(Name = "Brand")]
	public int? BrandId { get; set; }

	[Display(Name = "Product Number")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string ProductNumber { get; set; }

	[Display(Name = "Model Number")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string ModelNo { get; set; }

	[Display(Name = "Device Model")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string DeviceModel { get; set; }

	[Display(Name = "Item/Product Code")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Code { get; set; }

	[Display(Name = "Part Number")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string PartNo { get; set; }

	[Display(Name = "Part Code")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string PartCode { get; set; }

	[Display(Name = "Variant")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Variant { get; set; }

	[Display(Name = "Color")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Color { get; set; }

	[Display(Name = "Size (Descriptive)")]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Size { get; set; }

	[Display(Name = "Weight (Descriptive)")]
	[StringLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Weight { get; set; }

	[Display(Name = "Weight in Kg")]
	public decimal? WeightInKg { get; set; }

	[Display(Name = "Specification")]
	[StringLength(100, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Specification { get; set; }

	[Display(Name = "HS/Service Code")]
	[Required]
	[StringLength(50, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Hscode { get; set; }

	[Display(Name = "Product/Service Category")]
	[StringLength(200, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public int? ProductCategoryId { get; set; }

	[Display(Name = "Product/Service Group")]
	public int ProductGroupId { get; set; }

	[Display(Name = "Product/Service Type")]
	public int ProductTypeId { get; set; }

	[Display(Name = "Organization")]
	public int OrganizationId { get; set; }

	[Display(Name = "Total Quantity")]
	public decimal TotalQuantity { get; set; }

	[Display(Name = "Measurement Unit")]
	public int MeasurementUnitId { get; set; }

	[Display(Name = "Description")]
	[StringLength(500, ErrorMessage = "{0} cannot be longer than {1} characters.")]
	public string Description { get; set; }


	public IEnumerable<CustomSelectListItem> ProductVatTypeList { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> ProductTypeSelectListItems { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> BrandsSelectListItems { get; set; } = new List<CustomSelectListItem>();

	public IEnumerable<CustomSelectListItem> MeasurementUnits { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> ProductGroups { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> ProductVatTypes { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<CustomSelectListItem> ProductCategories { get; set; } = new List<CustomSelectListItem>();
}