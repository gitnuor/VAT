using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace vms.entity.viewModels.ReportsViewModel;

public class ReportOptionViewModel
{
	[DisplayName("Report Option")]
	[Range(1, 4, ErrorMessage = "Please select an option")]
	public int ReportProcessOptionId { get; set; }

	public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
}