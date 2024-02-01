using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels.NewReports;

public class vmSalesDueReport
{
    public IEnumerable<Sale> salesDueList { get; set; }
    public HeaderModel head { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}