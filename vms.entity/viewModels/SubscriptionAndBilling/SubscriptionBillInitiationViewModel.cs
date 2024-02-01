using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace vms.entity.viewModels.SubscriptionAndBilling;

public class SubscriptionBillInitiationViewModel
{
    [Display(Name = "Billing Office")]
    public int BillingOfficeId { get; set; }

    [Display(Name = "Collection Office")]
    public int CollectionOfficeId { get; set; }

    [Display(Name = "Bill Year")]
    public int BillYear { get; set; }

    [Display(Name = "Bill Month")]
    public int BillMonth { get; set; }
    public IEnumerable<CustomSelectListItem> MonthList { get; set; }
    public IEnumerable<CustomSelectListItem> OrgBranchList { get; set; }
}