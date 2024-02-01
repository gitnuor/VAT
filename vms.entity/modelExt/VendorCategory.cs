using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

public partial class VendorCategory : VmsBaseModel
{
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}