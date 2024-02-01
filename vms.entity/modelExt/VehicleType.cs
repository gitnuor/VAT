using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

[ModelMetadataType(typeof(VehicleTypeMetadata))]
public partial class VehicleType : VmsBaseModel
{
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
	[NotMapped]
	public string RegistrartionStatus => IsRequireRegistration ? "Yes" : "No";
	[NotMapped]
	public string ShortDescription => Description.Length > 80 ? Description.Substring(0, 79)+" ....." : Description;
}

public class VehicleTypeMetadata
{
	[DisplayName("Name")]
	[StringLength(50, ErrorMessage = "Vehicle Type Name cannot be longer than 50 characters.")]
	[Required(ErrorMessage = "Vehicle Type Name is required")]
        
	public string VehicleTypeName { get; set; }

	[StringLength(300, ErrorMessage = "Description cannot be longer than 300 characters.")]
	[Required(ErrorMessage ="Description is required")]
	public string Description { get; set; }
	[DisplayName("Is Require Registration?")]
	public bool IsRequireRegistration { get; set; }
        

}