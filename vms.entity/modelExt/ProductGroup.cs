using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(ProductGroupMetadata))]
public partial class ProductGroup : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}
public class ProductGroupMetadata
{

	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Required]
	public string Name { get; set; }
}