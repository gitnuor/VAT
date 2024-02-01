using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(ProductCategoryMetadata))]
public partial class ProductCategory : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}
public class ProductCategoryMetadata
{

	// [Display(Name = "Org.")]
	public int OrganizationId { get; set; }
	[StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
	[Required]
	public string Name { get; set; }
}