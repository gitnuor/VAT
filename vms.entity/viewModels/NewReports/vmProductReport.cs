using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels.NewReports;

public class vmProductReport
{
    public IEnumerable<Product> productsList { get; set; }
    public HeaderModel head { get; set; }
}