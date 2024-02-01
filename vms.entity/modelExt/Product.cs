using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(ProductMetadata))]
public partial class Product : VmsBaseModel
{
	//public int ProductVattypeId { get; set; }
	//public IEnumerable<SelectListItems> ProductVattype;
	public IEnumerable<CustomSelectListItem> MeasurementUnits;
	public IEnumerable<CustomSelectListItem> ProductGroups;
	public IEnumerable<CustomSelectListItem> ProductVattypes { get; set; }
	public IEnumerable<CustomSelectListItem> ProductCategories { get; set; }

        
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;

	[NotMapped] public string CategoryName => ProductCategory== null ? "" : ProductCategory.Name;
	[NotMapped] public string GroupName => ProductGroup== null ? "" : ProductGroup.Name;
	[NotMapped] public string BrandName => Brand== null ? "" : Brand.Name;

	[NotMapped]
	public string ProductCategoryName { get; set; }
	[NotMapped]
	public string ProductGroupName { get; set; }
	[NotMapped]
	public string MeasurementUnitName { get; set; }
	[NotMapped]
	public bool IsSellable { get; set; }
	[NotMapped]
	public bool IsRawMaterial { get; set; }
                
}
public class ProductMetadata
{
	[Display(Name = "Product/Service Name")]
	[StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
	[Required]
	public string Name { get; set; }

	[Display(Name = "Model No")]
	[StringLength(50, ErrorMessage = "ModelNo cannot be longer than 50 characters.")]
	public string ModelNo { get; set; }

	[Display(Name = "Item Code")]
	[StringLength(50, ErrorMessage = "Code cannot be longer than 50 characters.")]
	public string Code { get; set; }

	[Display(Name = "HS/Service code")]
	[StringLength(50, ErrorMessage = "HSCode cannot be longer than 50 characters.")]
	[Required]
	public string Hscode { get; set; }

	[Display(Name = "Product/Service Category")]
       
	public int ProductCategoryId { get; set; }

	[DisplayName("Product/Service Group")]
	[Required(ErrorMessage = "Product Group is Required")]
	public int ProductGroupId { get; set; }

	[DisplayName("Org.")]
	[Required]
	public int OrganizationId { get; set; }

	[DisplayName("Measurement Unit")]
	[Required(ErrorMessage = "Measurement Unit is Required")]
	public int MeasurementUnitId { get; set; }

	[DisplayName("Status")]
	public bool IsActive { get; set; }

	[DisplayName("Saleable")]
	public bool IsSellable { get; set; }

	[DisplayName("Is Raw Material?")]
	public bool IsRawMaterial { get; set; }
	[DisplayName("Product Type")]
	[Required(ErrorMessage = "Product Type is Required")]
	public int ProductTypeId { get; set; }

	[DisplayName("Brand")]
	public int? BrandId { get; set; }

}