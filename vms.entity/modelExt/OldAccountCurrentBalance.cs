using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace vms.entity.models;

public partial class OldAccountCurrentBalance : VmsBaseModel
{
	[NotMapped]
	public string MonthName { get; set; }
	[NotMapped]
	public List<SelectListItem> YearList { get; set; }
	[NotMapped]
	public List<SelectListItem> MonthList { get; set; }
}