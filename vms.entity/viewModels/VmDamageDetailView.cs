using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class VmDamageDetailView
{
    public VmDamageDetailView()
    {
        damageDetails = new List<DamageDetail>();
    }
    public Damage damage { get; set; }

    public IEnumerable<DamageDetail> damageDetails { get; set; }
}