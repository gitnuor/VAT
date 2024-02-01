using System.Collections.Generic;
using vms.entity.Enums;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels;

public class VmMushakFourPointFour
{
    public List<SpSalesTaxInvoice> MushakFourPointFours { get; set; }
    public EnumLanguage Language { get; set; }
}