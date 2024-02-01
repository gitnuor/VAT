using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.models;

[ModelMetadataType(typeof(ProductionMetadata))]
public partial class Production : VmsBaseModel
{
}
public class ProductionMetadata
{
	[Required]
	[Display(Name = "Work Order")]
	public int? WorkOrderId { get; set; }
	[Display(Name = "Created By")]
	public int CreatedBy { get; set; }
	[Display(Name = "Created Time")]
	[Required]
	public DateTime CreatedTime { get; set; }
	[Display(Name = "Expected Date")]
	[Required]
	public DateTime ExpectedDate { get; set; }
	[Display(Name = "Start Date")]
	[Required]
	public DateTime StartDate { get; set; }
	[Display(Name = "End Date")]
	[Required]
	public DateTime EndDate { get; set; }
       
	[Display(Name = "Organization")]
	[Required]
	public int OrganizationId { get; set; }
      
}