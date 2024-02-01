using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(OverHeadCostMetadata))]
public partial class OverHeadCost : VmsBaseModel
{
	[NotMapped]
	public string Status => IsActive? "Active" : "Inactive";
}

public class OverHeadCostMetadata
{

	[StringLength(80,ErrorMessage ="Over head cost name can not be greater than 80 characters")]
	[Required(ErrorMessage ="Over Head Cost Name is required")]
	[DisplayName("Over Head cost name")]
	public string Name { get; set; }
}