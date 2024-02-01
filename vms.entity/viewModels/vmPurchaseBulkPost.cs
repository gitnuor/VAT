using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmPurchaseBulkPost
{
    public int OrganizationId { get; set; }
    public string SecurityToken { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public IList<vmPurchasePost> PurchaseList { get; set; }
    public IList<vmPurchaseDetailPost> PurchaseDetailList { get; set; }
    public IList<vmPurchasePaymentPost> PurchasePaymentList { get; set; }
}