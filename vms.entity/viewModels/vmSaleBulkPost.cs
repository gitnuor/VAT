using System;
using System.Collections.Generic;

namespace vms.entity.viewModels;

public class vmSaleBulkPost
{
    public int OrganizationId { get; set; }
    public string SecurityToken { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedTime { get; set; }
    public IList<vmSalesPost> SaleList { get; set; }
    public IList<vmSalesDetailPost> SaleDetailList { get; set; }
    public IList<vmSalesPaymentPost> SalePaymentList { get; set; }
}