using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.MushakViewModel;

public class VmMushakSixPointSix : VmMushakBase
{
    public VmMushakSixPointSix()
    {
        VdsPurchaseCertificates = new List<SpVdsPurchaseCertificate>();
    }
    public List<SpVdsPurchaseCertificate> VdsPurchaseCertificates { get; set; }
}