using System;
using System.Collections.Generic;
using vms.entity.models;

namespace vms.entity.viewModels;

public class vmTransferRawMaterial
{
    public int ContractVendorTransferRawMateriallId { get; set; }
    public int ContractualProductionId { get; set; }
    public DateTime TransfereDate { get; set; }
    public string Location { get; set; }
    public string ChallanNo { get; set; }
    public DateTime ChallanIssueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public List<ContractualProductionTransferRawMaterialDetail> Details { get; set; }
}