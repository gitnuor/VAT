using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmMushakBranchTransfer
{
    public int SaleId { get; set; }
    public List<SpBranchTransfer> TransferList { get; set; }
}