
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(PaymentMethodMetadata))]
public partial class PaymentMethod : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}
public class PaymentMethodMetadata
{

	[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
	[Required]
	public string Name { get; set; }
       
}