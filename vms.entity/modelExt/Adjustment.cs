using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using vms.entity.viewModels;


namespace vms.entity.models;

public partial class Adjustment : VmsBaseModel
{
	public IEnumerable<CustomSelectListItem> AdjustmentTypes;
	[NotMapped]
	public string Status => IsActive ? "Active" : "Inactive";
}