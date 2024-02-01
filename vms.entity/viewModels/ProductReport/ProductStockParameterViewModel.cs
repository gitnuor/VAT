using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using vms.entity.CustomDataAnnotations;

namespace vms.entity.viewModels.ProductReport
{
	public class ProductStockParameterViewModel
	{
		public int OgrId { get; set; }

		[DisplayName("Branch")]
		public int? OgrBranchId { get; set; }
		public IEnumerable<CustomSelectListItem> BranchSelectListItems { get; set; }

		[DisplayName("Product Type")]
		public int? ProductTypeId { get; set; }
		public IEnumerable<CustomSelectListItem> ProductTypeSelectListItems { get; set; }

		[DisplayName("Stock Date")]
		[DateShouldBeUpToToday(ErrorMessage = "To Date should not be greater than today and less than year of 2000!")]
		public DateTime ToDate { get; set; }

		[DisplayName("Report Option")]
		[Range(1, 4, ErrorMessage = "Please select an option")]
		public int ReportProcessOptionId { get; set; }
		public IEnumerable<CustomSelectListItem> ReportOptionSelectListItems { get; set; }
	}
}
