using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vms.entity.models;

namespace vms.entity.viewModels.CustomerViewModel;

public class CustomerSubscriptionCreateViewModel
{
	public int CustomerSubscriptionId { get; set; }

	[Display(Name = "Billing Branch")] public int OrgBranchId { get; set; }

	[Display(Name = "Collection Branch")] public int CollectionOfficeId { get; set; }

	[Display(Name = "Customer")] public int CustomerId { get; set; }

	[StringLength(500)] public string Remarks { get; set; }
	public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; } = new List<CustomSelectListItem>();
	public IEnumerable<Customer> CustomerList { get; set; } = new List<Customer>();

	public IEnumerable<CustomerSubscriptionDetailCreateViewModel> CustomerSubscriptionDetails { get; set; } =
		new List<CustomerSubscriptionDetailCreateViewModel>();
}