using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.viewModels;

namespace vms.entity.models;

[ModelMetadataType(typeof(PurchaseReasonMetadata))]
public partial class PurchaseReason : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> Roles;
	public IEnumerable<CustomSelectListItem> UserTypes;
}
public class PurchaseReasonMetadata
{

	[StringLength(50, ErrorMessage = "Reason cannot be longer than 50 characters.")]
	[Required]
	public string Reason { get; set; }
}