
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.models;

[ModelMetadataType(typeof(SupplimentaryDutyMetadata))]
public partial class SupplimentaryDuty : VmsBaseModel
{
      
}
public class SupplimentaryDutyMetadata
{
	[Required(ErrorMessage ="Supplimentary Duty is required")]
	[DisplayName("Supplimentary Duty")]
	public decimal SdPercent { get; set; }
}