using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmDamageDetails
{
    public Damage Damage { get; set; }
    public List<DamageDetail> DamageDetails { get; set; }
}