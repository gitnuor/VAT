
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.models;

[ModelMetadataType(typeof(DamageMetadata))]
public partial class Damage : VmsBaseModel
{

}
public class DamageMetadata
{
	[Display(Name = "Damage Qty")]
	[Required(ErrorMessage = "Please enter Damage Quantity")]
	public decimal DamageQty { get; set; }

	[Display(Name = "Product")]
	[Required(ErrorMessage = "Please enter Product name")]
	public int ProductId { get; set; }

	[Display(Name = "Description")]
	[StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
	[Required(ErrorMessage = "Please enter Description")]
	public string Description { get; set; }

	[Display(Name = "Damage Type")]
	[Required(ErrorMessage = "Please enter Damage Type")]
	public int DamageTypeId { get; set; }

}