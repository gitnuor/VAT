using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace vms.entity.models;

[ModelMetadataType(typeof(AdjustmentTypeMetadata))]
public partial class AdjustmentType : VmsBaseModel
{
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}
public class AdjustmentTypeMetadata
{
	[Required]
	public string Name { get; set; }
}