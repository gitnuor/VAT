using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmMushak4P5
{
    public vmMushak4P5()
    {
        Details = new List<vmMushak4P5Details>();
    }
    public Damage Damage { get; set; }
        
    public string OrgName { get; set; }
    public string OrgBin { get; set; }
    public string OrgAddress { get; set; }
    public string VatResponsiblePersonName { get; set; }
    public string VatResponsiblePersonDesignation { get; set; }
    public List<vmMushak4P5Details> Details { get; set; }

}