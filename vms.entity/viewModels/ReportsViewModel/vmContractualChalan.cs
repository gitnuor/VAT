using System.Collections.Generic;
using vms.entity.StoredProcedureModel.HTMLMushak;

namespace vms.entity.viewModels.ReportsViewModel;

public class vmContractualChalan
{
    public List<Sp6p4> Sp6P4Result { get; set; }
    public int? ContractualProductionTransferRawMaterialId { get; set; }
    public int ContractualProductionId { get; set; }
}