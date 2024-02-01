using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace vms.entity.viewModels.ProductReport
{
	public class ProductReportParameterViewModel
	{
		public int OgrId { get; set; }

		[DisplayName("Branch")]
		public int? OgrBranchId { get; set; }
		public IEnumerable<CustomSelectListItem> BranchList { get; set; }

		[DisplayName("Product Type")]
		public int? ProductTypeId { get; set; }
		public IEnumerable<CustomSelectListItem> ProductTypeSelectListItems { get; set; }

		[DisplayName("Report Option")]
		[Range(1, 4, ErrorMessage = "Please select an option")]
		public int ReportProcessOptionId { get; set; }
		public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
	}
}
