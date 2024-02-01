using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.MushakViewModel;

public class TdsCertificateViewModel : VmMushakBase
{
    public TdsCertificateViewModel()
    {
        TdsCertificates = new List<SpGetTdsPurchaseCertificate>();
    }
    public List<SpGetTdsPurchaseCertificate> TdsCertificates { get; set; }
}