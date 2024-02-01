using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.SubscriptionAndBilling;

public class SubscriptionBillCreateViewModel
{
	[Display(Name = "Billing Office")]
	public int BillingOfficeId { get; set; }
	[Display(Name = "Billing Office")]
	public string BillingOfficeName { get; set; }

	[Display(Name = "Collection Office")]
	public int CollectionOfficeId { get; set; }

	[Display(Name = "Collection Office")]
	public string CollectionOfficeName { get; set; }

	[Display(Name = "Bill Year")]
	public int BillYear { get; set; }

	[Display(Name = "Bill Month")]
	public int BillMonth { get; set; }

	[Display(Name = "Bill Month")]
	public string BillMonthName { get; set; }

	public IEnumerable<SubscriptionBillDetailCreateViewModel> SubscriptionBillDetails { get; set; }
}